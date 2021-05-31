using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace STREAMS
{
    public class Opt_Razm
    {
        public static List<List<L_T_Vetvi>> optim_raschet;
        private int razm;
        string Log;
        private List<int> opt_KA;
        private List<Cp_opt> CP_list = new List<Cp_opt>();
        private List<Prov_zam> Pr_list = new List<Prov_zam>();
        private List<Tr_zam> Tr_list = new List<Tr_zam>();
        private List<List<int>> graf_opt = new List<List<int>>();
        List<L_T_Vetvi> regim = new List<L_T_Vetvi>(); // для промежуточных расчетов режима
        List<List<List<int>>> spisok_KA_value = new List<List<List<int>>>(); //тут будем хранить фактический и оптимальные положения коммутационных аппаратов
        List<float> poteri_value = new List<float>();
        List<int> period;
        List<List<int>> otkl_vetvi = new List<List<int>>();
        List<int> poradok_rascheta;
        private bool napr, z_pr, z_tr, razukrup, ust_ku, prov;
        //int fd = 0;
        public void Setting(int opt_razm, bool napr, bool z_pr, bool z_tr, bool razukr, bool ust_ku)
        {
            this.razm = opt_razm;
            this.napr = napr;
            this.z_pr = z_pr;
            this.z_tr = z_tr;
            this.razukrup = razukr;
            this.ust_ku = ust_ku;
        }
        private void Copy_raschet_info()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, raschet.lRaschet);
                ms.Position = 0;
                optim_raschet = (List<List<L_T_Vetvi>>)bf.Deserialize(ms);
            }
        }
        private void Opredelit_kolichestvo_otkl()
        {
            for (int i = 0; i < optim_raschet.Count; i++)
            {
                otkl_vetvi.Add(new List<int>());
                for (int j = 0; j < optim_raschet[i].Count; j++)
                    if (optim_raschet[i][j].ka == 0)
                        otkl_vetvi[i].Add(j);
            }
        }
        public List<float> Run()
        {
            SprInfo sprInfo = new SprInfo();
            bool pr;
            int col;
            List<Prov_zam> spis_sech_AC = new List<Prov_zam>();
            List<Tr_zam> spis_moshn_TR = new List<Tr_zam>();
            Encoding enc = Encoding.GetEncoding(1251);
            opt_KA = new List<int>();
            optim_raschet = new List<List<L_T_Vetvi>>();
            List<List<float>> vozm_poteri = new List<List<float>>();        // массив возможных потерь от оптимизации
            Copy_raschet_info();
            float nov, tok_zamen, sech, Imax, moshn, tok_zamen_min, tok_zamen_max;
            float[,] effect = new float[4, 2];          // для эффекта от мероприятий
            if (razm != 0)
            {
                Log += "               Определение оптимальных мест размыкания сети:" + Environment.NewLine;
                // определить количество оптимизируемых разрезов
                Opredelit_kolichestvo_otkl();
                int n = (int)Math.Round((float)24 / razm, 0);
                period = new List<int>();
                for (int i = 1; i < razm; i++)
                    period.Add(n);
                period.Add(24 - (razm - 1) * n);
                spisok_KA_value.Clear();
                for (int z = 0; z < period.Count; z++)
                {
                    spisok_KA_value.Add(new List<List<int>>());
                    spisok_KA_value[z].Add(new List<int>());
                    // для каждого часа в периоде  
                    for (int t = z * n; t < (z * n + period[z]); t++)
                    {
                        // обновление текущих положений КА
                        vozm_poteri.Add(new List<float>());
                        vozm_poteri[t].Add(0);
                        spisok_KA_value[z][0].Clear();
                        for (int i = 0; i < raschet.lRaschet[t].Count; i++)
                        {
                            spisok_KA_value[z][0].Add(raschet.lRaschet[t][i].ka);
                            vozm_poteri[t][0] += raschet.lRaschet[t][i].dP + raschet.lRaschet[t][i].dPxx;
                        }
                        //Console.WriteLine("расчетный час = " + t.ToString());
                        eff_Mesta_razr(t);
                        //сравнение с фактическим и другими оптимальными режимами
                        prov = true;
                        for (int j = 0; j < spisok_KA_value[z].Count; j++)
                        {
                            for (int i = 0; i < optim_raschet[t].Count; i++)                    //проверка оптим. с текущими КА
                                if (optim_raschet[t][i].ka != spisok_KA_value[z][j][i])
                                { prov = false; break; }
                            if (!prov)      // если оптимальное положение КА не совпадает с текущим (j)
                                if (j != spisok_KA_value[z].Count - 1)  //если не последнее
                                {
                                    prov = true;
                                    // расчет режима если j!=0, добавление эффекта
                                    if (j != 0)
                                    {
                                        Regim_raschet(t, z, j);
                                        vozm_poteri[t].Add(0);
                                        for (int i = 0; i < regim.Count; i++)
                                            vozm_poteri[t][j] += regim[i].dP + regim[i].dPxx;
                                    }
                                }
                                else
                                {
                                    if (j != 0)
                                    {
                                        Regim_raschet(t, z, j);
                                        vozm_poteri[t].Add(0);
                                        for (int i = 0; i < regim.Count; i++)
                                            vozm_poteri[t][j] += regim[i].dP + regim[i].dPxx;
                                    }
                                    spisok_KA_value[z].Add(new List<int>());
                                    vozm_poteri[t].Add(0);
                                    j++;
                                    for (int i = 0; i < raschet.lRaschet[t].Count; i++)
                                        spisok_KA_value[z][j].Add(optim_raschet[t][i].ka);
                                    Regim_raschet(t, z, j);
                                    for (int i = 0; i < regim.Count; i++)
                                        vozm_poteri[t][j] += regim[i].dP + regim[i].dPxx;
                                    for (int i = t - 1; i >= 0; i--)
                                    {//расчет нового режима и добавление эффекта
                                        Regim_raschet(i, z, j);
                                        vozm_poteri[i].Add(0);
                                        for (int k = 0; k < regim.Count; k++)
                                            vozm_poteri[i][j] += regim[k].dP + regim[k].dPxx;
                                    }
                                }
                            else // если совпали разрезы с уже имеющимися опт. положениями КА
                            {
                                if (j != 0)
                                { //добавление эффекта
                                    vozm_poteri[t].Add(0);
                                    Regim_raschet(t, z, j);
                                    for (int i = 0; i < optim_raschet[t].Count; i++)
                                        vozm_poteri[t][j] += regim[i].dP + regim[i].dPxx;
                                }
                                // определение эффекта от последующих положений КА
                                while (j != spisok_KA_value[z].Count - 1)
                                {
                                    j++;
                                    Regim_raschet(t, z, j);
                                    vozm_poteri[t].Add(0);
                                    for (int k = 0; k < regim.Count; k++)
                                        vozm_poteri[t][j] += regim[k].dP + regim[k].dPxx;
                                }
                            }
                        }
                        //Console.WriteLine("-----> t=" + t.ToString());
                        //for (int i = 0; i < vozm_poteri.Count; i++)
                        //{
                        //    for (int j = 0; j < vozm_poteri[i].Count; j++)
                        //        Console.Write(Math.Round(vozm_poteri[i][j], 3).ToString()+ "  ");
                        //    Console.WriteLine("");
                        //}

                    }
                    opt_KA.Add(0);
                    poteri_value.Clear();
                    for (int i = 0; i < spisok_KA_value[z].Count; i++)
                    {
                        poteri_value.Add(0);
                        for (int t = z * n; t < (z * n + period[z]); t++)
                            poteri_value[i] += vozm_poteri[t][i];
                    }
                    nov = poteri_value[0];
                    effect[0, 0] += poteri_value[0];
                    for (int i = 1; i < poteri_value.Count; i++)
                        if (nov > poteri_value[i])
                        {
                            nov = poteri_value[i];
                            opt_KA[z] = i;
                        }
                    for (int t = z * n; t < (z * n + period[z]); t++)
                    {
                        Regim_raschet(t, z, opt_KA[z]);
                        for (int i = 0; i < optim_raschet[t].Count; i++)
                        {
                            optim_raschet[t][i] = Copy_Vetv(ref regim, i);
                        }
                    }
                    //Console.WriteLine(opt_KA[z]);
                    //float rez = 0;
                    //for (int i = 0; i < vozm_poteri.Count; i++)
                    //    Console.WriteLine("T="+i.ToString()+" "+vozm_poteri[i][5].ToString() + " | ");
                    //Console.WriteLine(rez.ToString());
                    //Regim_raschet(7, 0, 5);
                    //in_testR();
                    effect[0, 1] += nov;
                    for (int i = 0; i < optim_raschet[0].Count; i++)
                        if (raschet.lRaschet[0][i].ka != optim_raschet[0][i].ka)
                            Log += "Ветвь  " + BaseShema.UZ[optim_raschet[0][i].nach_AO].nameCP.ToString() + " - " + BaseShema.UZ[optim_raschet[0][i].kon_AO].nameCP.ToString() +
                                (raschet.lRaschet[0][i].ka == 0 ? "     откл." : "     вкл.") + "  -->  " + (optim_raschet[0][i].ka == 0 ? "откл." : "вкл.") + Environment.NewLine;
                }

            }
            else
            {
                effect[0, 0] = 0;
                for (int i = 0; i < optim_raschet.Count; i++)
                    for (int j = 0; j < optim_raschet[i].Count; j++)
                        effect[0, 0] += optim_raschet[i][j].dP + optim_raschet[i][j].dPxx;
                effect[0, 1] = effect[0, 0];
            }
            effect[1, 0] = effect[0, 1];
            if (napr)
            {
                Log += "         Определение оптимальных значений напряжения в центрах питания:" + Environment.NewLine;
                for (int i = 0; i < BaseShema.UZ.Count; i++)
                    if (BaseShema.UZ[i].tipUz == 0)
                    {
                        CP_list.Add(new Cp_opt(i, new List<float>()));
                        for (int j = 0; j < BaseShema.UZ[i].graficU.Count; j++)
                            CP_list[CP_list.Count - 1].U.Add(BaseShema.UZ[i].graficU[j].Yset);
                    }
                effect[1, 1] = 0;
                for (int i = 0; i < optim_raschet.Count; i++)
                {
                    Napr_opt(i);
                    Opredelit_Poradok_seti_Opt(i, true);
                    Add_regim_info_Opt(i, true, true, new List<int>());
                    for (int q = (poradok_rascheta.Count - 1); q >= 0; q--)
                    {
                        Potok_vetvi_raschet_obr_Opt(i, poradok_rascheta[q], false);
                    }
                    for (int q = 0; q < poradok_rascheta.Count; q++)
                    {
                        Napr_vetvi_raschet_pr_Opt(i, poradok_rascheta[q]);
                    }
                    Add_regim_info_Opt(i, true, false, new List<int>());
                    for (int q = (poradok_rascheta.Count - 1); q >= 0; q--)
                    {
                        Potok_vetvi_raschet_obr_Opt(i, poradok_rascheta[q], true);
                    }
                    for (int q = 0; q < optim_raschet[i].Count; q++)
                    {
                        raschet_Tokov_Opt(i, q);                      
                    }
                    //if (i == 0)
                    //    in_test(i,true);
                    for (int j = 0; j < optim_raschet[i].Count; j++)
                        effect[1, 1] += optim_raschet[i][j].dP + optim_raschet[i][j].dPxx;
                }
            }
            else
            {
                effect[1, 1] = effect[1, 0];
            }
            effect[2, 0] = effect[1, 1];
            if(z_pr||z_tr)
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();                
                    using (Stream input = File.OpenRead(Application.StartupPath + @"\SprInfo.dll"))             //десериализация файла
                    { sprInfo = (SprInfo)formatter.Deserialize(input); }
                }
                catch (Exception err)
                { MessageBox.Show("Ошибка при чтении справочных данных!" + Environment.NewLine + err.ToString()); sprInfo = null; }
            if (z_pr)
            {
                Log += "          Поиск перегруженных проводов для замены:" + Environment.NewLine;              
                for (int i = 0; i < BaseShema.Vet.Count; i++)
                {
                    Imax = 0;
                    sech = 0;
                    tok_zamen = 0;
                    pr = false;
                    if ((BaseShema.Vet[i].tipV == 0) && (BaseShema.Vet[i].k1 != 0) && (BaseShema.Vet[i].k2 != 0))
                    {
                        for (int j = 0; j < sprInfo.ProvInfo.Count; j++)
                        {
                            if (BaseShema.Vet[i].marka == sprInfo.ProvInfo[j].marka)
                            {
                                sech = sprInfo.ProvInfo[j].sechenie;
                                tok_zamen = Return_keff_zamen_provoda(sech);
                                pr = true;
                                break;
                            }
                        }
                        if (pr)
                        {
                            for (int t = 0; t < optim_raschet.Count; t++)
                            {
                                if (optim_raschet[t][i].I > Imax)
                                    Imax = optim_raschet[t][i].I;
                            }
                            if (Imax >= tok_zamen * optim_raschet[0][i].Idop)
                            {
                                Pr_list.Add(new Prov_zam(i, "АС-", 0));
                                spis_sech_AC.Clear();
                                for (int j = 0; j < sprInfo.ProvInfo.Count; j++)
                                {
                                    if ("АС-" == sprInfo.ProvInfo[j].marka.Substring(0, 3))
                                        spis_sech_AC.Add(new Prov_zam(j, sprInfo.ProvInfo[j].marka, sprInfo.ProvInfo[j].sechenie));
                                    else if (("М-" == sprInfo.ProvInfo[j].marka.Substring(0, 2)) && (sech < 25) && (sprInfo.ProvInfo[j].sechenie < 25))
                                        spis_sech_AC.Add(new Prov_zam(j, sprInfo.ProvInfo[j].marka, sprInfo.ProvInfo[j].sechenie));
                                }
                                spis_sech_AC.Sort(new ListComparer_bySech());
                                for (int j = 0; j < spis_sech_AC.Count; j++)
                                    if (spis_sech_AC[j].sech == sech)
                                    {
                                        if (spis_sech_AC.Count - 1 < j + 2)
                                        {
                                            Log += "В справочнике нет проводов с сечением на 2 порядка выше заменяемого - " + sech.ToString() + Environment.NewLine;
                                            Pr_list.RemoveAt(Pr_list.Count - 1);
                                        }
                                        else
                                        {
                                            col = 2;
                                            while (Imax >= Return_keff_zamen_provoda(spis_sech_AC[j + col].sech) * sprInfo.ProvInfo[spis_sech_AC[j + col].Index_vet].idop)
                                            {
                                                if (j+col < spis_sech_AC.Count - 1)
                                                    col++;
                                                else
                                                {
                                                    Log += "Слишком большой ток в ветви " + BaseShema.Vet[i].nach.ToString() + "-" + BaseShema.Vet[i].kon.ToString() + " чтобы подобрать сечение провода (выбрано максимальное - " + spis_sech_AC[j + col].sech + ")" + Environment.NewLine;
                                                    break;
                                                }
                                            }
                                            Log += "Замена на ветви " + BaseShema.Vet[i].nach.ToString() + "-" + BaseShema.Vet[i].kon.ToString() + " марки провода " + BaseShema.Vet[i].marka + " --> " + spis_sech_AC[j + col].mar_new + Environment.NewLine;
                                            Pr_list[Pr_list.Count - 1].sech = spis_sech_AC[j + col].sech;
                                            Pr_list[Pr_list.Count - 1].mar_new = spis_sech_AC[j + col].mar_new;
                                            for (int q = 0; q < optim_raschet.Count; q++)
                                            {
                                                optim_raschet[q][i].R = sprInfo.ProvInfo[spis_sech_AC[j + col].Index_vet].R0 * BaseShema.Vet[i].dlina_moshnost;
                                                optim_raschet[q][i].X = sprInfo.ProvInfo[spis_sech_AC[j + col].Index_vet].X0 * BaseShema.Vet[i].dlina_moshnost;
                                                optim_raschet[q][i].Idop = sprInfo.ProvInfo[spis_sech_AC[j + col].Index_vet].idop;
                                            }
                                        }
                                        break;
                                    }

                            }
                        }
                    }
                }
                // пересчет режима и потерь
                
                effect[2, 1] = 0;
                for (int t = 0; t < optim_raschet.Count; t++)
                {
                    Opredelit_Poradok_seti_Opt(t, true);
                    Add_regim_info_Opt(t, true, true, new List<int>());
                    for (int i = (poradok_rascheta.Count - 1); i >= 0; i--)
                    {
                        Potok_vetvi_raschet_obr_Opt(t, poradok_rascheta[i], false);
                    }
                    for (int i = 0; i < poradok_rascheta.Count; i++)
                    {
                        Napr_vetvi_raschet_pr_Opt(t, poradok_rascheta[i]);
                    }
                    Add_regim_info_Opt(t, true, false,new List<int>());
                    for (int i = (poradok_rascheta.Count - 1); i >= 0; i--)
                    {
                        Potok_vetvi_raschet_obr_Opt(t, poradok_rascheta[i], true);
                    }
                    for (int i = 0; i < optim_raschet[t].Count; i++)
                    {
                        raschet_Tokov_Opt(t, i);
                        effect[2, 1] += optim_raschet[t][i].dPxx + optim_raschet[t][i].dP;
                    }
                    //if (t == 0)
                    //    in_test(t, true);
                }
            }
            else
            {
                effect[2, 1] = effect[2, 0];
            }
            effect[3, 0] = effect[2, 1];
            if (z_tr)
            {
                Log += "          Поиск перегруженных/недогруженных трансформаторов для замены:" + Environment.NewLine;   
                for (int i = 0; i < BaseShema.Vet.Count; i++)
                {
                    Imax = 0;
                    moshn = 0;
                    tok_zamen_min = 0;
                    tok_zamen_max = 0;
                    pr = false;
                    if ((BaseShema.Vet[i].tipV == 1) && (BaseShema.Vet[i].k1 != 0) && (BaseShema.Vet[i].k2 != 0))
                    {
                        for (int j = 0; j < sprInfo.TrInfo.Count; j++)
                        {
                            if (BaseShema.Vet[i].marka == sprInfo.TrInfo[j].marka)
                                if (BaseShema.Vet[i].dlina_moshnost == sprInfo.TrInfo[j].snom)
                                    if ((optim_raschet[0][i].Unom == sprInfo.TrInfo[j].klassNomNapr) && (optim_raschet[0][i].ktr * 1.01f >= sprInfo.TrInfo[j].unn / sprInfo.TrInfo[j].uvn) && (optim_raschet[0][i].ktr * 0.99 <= sprInfo.TrInfo[j].unn / sprInfo.TrInfo[j].uvn))
                                    {
                                        moshn = sprInfo.TrInfo[j].snom;
                                        tok_zamen_min = Return_koef_zagr_min_max(true) * moshn / sprInfo.TrInfo[j].klassNomNapr / (float)Math.Sqrt(3);
                                        tok_zamen_max = Return_koef_zagr_min_max(false) * moshn / sprInfo.TrInfo[j].klassNomNapr / (float)Math.Sqrt(3);
                                        pr = true;
                                        break;
                                    }
                        }
                        if (pr)
                        {
                            for (int t = 0; t < optim_raschet.Count; t++)
                            {
                                if (optim_raschet[t][i].I > Imax)
                                    Imax = optim_raschet[t][i].I;
                            }
                            if ((Imax > tok_zamen_max)||(Imax < tok_zamen_min))     
                            {
                                Tr_list.Add(new Tr_zam(i, "ТМ-", 0));
                                spis_moshn_TR.Clear();
                                for (int j = 0; j < sprInfo.TrInfo.Count; j++)
                                {
                                    if ("ТМ-" == sprInfo.TrInfo[j].marka.Substring(0, 3))
                                        if ((optim_raschet[0][i].Unom == sprInfo.TrInfo[j].klassNomNapr) && (optim_raschet[0][i].ktr * 1.01f >= sprInfo.TrInfo[j].unn / sprInfo.TrInfo[j].uvn) && (optim_raschet[0][i].ktr * 0.99 <= sprInfo.TrInfo[j].unn / sprInfo.TrInfo[j].uvn))
                                            spis_moshn_TR.Add(new Tr_zam(j, sprInfo.TrInfo[j].marka, sprInfo.TrInfo[j].snom));                                  
                                }
                                spis_moshn_TR.Sort(new ListComparer_bySnom());
                                if (Imax > tok_zamen_max)
                                {
                                    for (int j = 0; j < spis_moshn_TR.Count; j++)
                                        if (spis_moshn_TR[j].moshn >= moshn)
                                        {
                                            if (spis_moshn_TR.Count - 1 < j + 1)
                                            {
                                                Log += "В справочнике нет трансформаторов с мощностью, большей заменяемого - " + moshn.ToString() + " кВА" + Environment.NewLine;
                                                Tr_list.RemoveAt(Tr_list.Count - 1);
                                            }
                                            else
                                            {
                                                col = 1;
                                                while (Imax > Return_koef_zagr_min_max(false) * spis_moshn_TR[j + col].moshn / sprInfo.TrInfo[spis_moshn_TR[j + col].Index_vet].klassNomNapr / (float)Math.Sqrt(3))
                                                {
                                                    if (j+col < spis_moshn_TR.Count - 1)
                                                        col++;
                                                    else
                                                    {
                                                        Log += "Слишком большой ток нагрузки трансформатора " + BaseShema.Vet[i].nach.ToString() + "-" + BaseShema.Vet[i].kon.ToString() + " чтобы заменить его (выбран Тр-р наибольшей мощности - " + spis_moshn_TR[j + col].moshn + " кВА)" + Environment.NewLine;
                                                        break;
                                                    }
                                                }
                                                Log += "Замена на ветви " + BaseShema.Vet[i].nach.ToString() + "-" + BaseShema.Vet[i].kon.ToString() + " перегруженного трансформатора " + BaseShema.Vet[i].marka + " --> " + spis_moshn_TR[j + col].mar_new + Environment.NewLine;
                                                Tr_list[Tr_list.Count - 1].moshn = spis_moshn_TR[j + col].moshn;
                                                Tr_list[Tr_list.Count - 1].mar_new = spis_moshn_TR[j + col].mar_new;

                                                optim_raschet[0][i].R = sprInfo.TrInfo[spis_moshn_TR[j + col].Index_vet].dpkz * (float)Math.Pow(sprInfo.TrInfo[spis_moshn_TR[j + col].Index_vet].klassNomNapr, 2) / (float)Math.Pow(sprInfo.TrInfo[spis_moshn_TR[j + col].Index_vet].snom, 2) * 1000;
                                                optim_raschet[0][i].X = (float)Math.Sqrt((float)Math.Pow(sprInfo.TrInfo[spis_moshn_TR[j + col].Index_vet].ukz, 2) - (float)Math.Pow((sprInfo.TrInfo[spis_moshn_TR[j + col].Index_vet].dpkz / sprInfo.TrInfo[spis_moshn_TR[j + col].Index_vet].snom * 100), 2)) * (float)Math.Pow(sprInfo.TrInfo[spis_moshn_TR[j + col].Index_vet].klassNomNapr, 2) / 100 / sprInfo.TrInfo[spis_moshn_TR[j + col].Index_vet].snom * 1000;
                                                optim_raschet[0][i].dPxx = sprInfo.TrInfo[spis_moshn_TR[j + col].Index_vet].dpxx;
                                                optim_raschet[0][i].dQxx = sprInfo.TrInfo[spis_moshn_TR[j + col].Index_vet].ixx * sprInfo.TrInfo[spis_moshn_TR[j + col].Index_vet].snom / 100;
                                                optim_raschet[0][i].ktr = sprInfo.TrInfo[spis_moshn_TR[j + col].Index_vet].unn / sprInfo.TrInfo[spis_moshn_TR[j + col].Index_vet].uvn;
                                                optim_raschet[0][i].Idop = sprInfo.TrInfo[spis_moshn_TR[j + col].Index_vet].snom / sprInfo.TrInfo[spis_moshn_TR[j + col].Index_vet].klassNomNapr / (float)Math.Sqrt(3);
                                                for (int q = 1; q < optim_raschet.Count; q++)
                                                {
                                                    optim_raschet[q][i].R = optim_raschet[0][i].R;
                                                    optim_raschet[q][i].X = optim_raschet[0][i].X;
                                                    optim_raschet[q][i].dPxx = optim_raschet[0][i].dPxx;
                                                    optim_raschet[q][i].dQxx = optim_raschet[0][i].dQxx;
                                                    optim_raschet[q][i].ktr = optim_raschet[0][i].ktr;
                                                    optim_raschet[q][i].Idop = optim_raschet[0][i].Idop;
                                                }
                                            }
                                            break;
                                        }
                                }
                                else
                                {
                                    for (int j = spis_moshn_TR.Count-1; j >=0; j--)
                                        if (spis_moshn_TR[j].moshn <= moshn)
                                        {
                                            if (0 > j - 1)
                                            {
                                                Log += "В справочнике нет трансформаторов с мощностью, меньшей заменяемого - " + moshn.ToString() + " кВА" + Environment.NewLine;
                                                Tr_list.RemoveAt(Tr_list.Count - 1);
                                            }
                                            else
                                            {
                                                col = 1;
                                                while (Imax < Return_koef_zagr_min_max(false) * spis_moshn_TR[j - col].moshn / sprInfo.TrInfo[spis_moshn_TR[j - col].Index_vet].klassNomNapr / (float)Math.Sqrt(3))
                                                {
                                                    if (j - col > 0)
                                                        col++;
                                                    else
                                                    {
                                                        Log += "Слишком малый ток нагрузки трансформатора " + BaseShema.Vet[i].nach.ToString() + "-" + BaseShema.Vet[i].kon.ToString() + " чтобы заменить его на оптимальный (выбран Тр-р наименьшей мощности - " + spis_moshn_TR[j - col].moshn + " кВА)" + Environment.NewLine;
                                                        break;
                                                    }
                                                }
                                                Log += "Замена на ветви " + BaseShema.Vet[i].nach.ToString() + "-" + BaseShema.Vet[i].kon.ToString() + " недогруженного трансформатора " + BaseShema.Vet[i].marka + " --> " + spis_moshn_TR[j - col].mar_new + Environment.NewLine;
                                                Tr_list[Tr_list.Count - 1].moshn = spis_moshn_TR[j - col].moshn;
                                                Tr_list[Tr_list.Count - 1].mar_new = spis_moshn_TR[j - col].mar_new;

                                                optim_raschet[0][i].R = sprInfo.TrInfo[spis_moshn_TR[j - col].Index_vet].dpkz * (float)Math.Pow(sprInfo.TrInfo[spis_moshn_TR[j - col].Index_vet].klassNomNapr, 2) / (float)Math.Pow(sprInfo.TrInfo[spis_moshn_TR[j - col].Index_vet].snom, 2) * 1000;
                                                optim_raschet[0][i].X = (float)Math.Sqrt((float)Math.Pow(sprInfo.TrInfo[spis_moshn_TR[j - col].Index_vet].ukz, 2) - (float)Math.Pow((sprInfo.TrInfo[spis_moshn_TR[j - col].Index_vet].dpkz / sprInfo.TrInfo[spis_moshn_TR[j - col].Index_vet].snom * 100), 2)) * (float)Math.Pow(sprInfo.TrInfo[spis_moshn_TR[j - col].Index_vet].klassNomNapr, 2) / 100 / sprInfo.TrInfo[spis_moshn_TR[j - col].Index_vet].snom * 1000;
                                                optim_raschet[0][i].dPxx = sprInfo.TrInfo[spis_moshn_TR[j - col].Index_vet].dpxx;
                                                optim_raschet[0][i].dQxx = sprInfo.TrInfo[spis_moshn_TR[j - col].Index_vet].ixx * sprInfo.TrInfo[spis_moshn_TR[j - col].Index_vet].snom / 100;
                                                optim_raschet[0][i].ktr = sprInfo.TrInfo[spis_moshn_TR[j - col].Index_vet].unn / sprInfo.TrInfo[spis_moshn_TR[j - col].Index_vet].uvn;
                                                optim_raschet[0][i].Idop = sprInfo.TrInfo[spis_moshn_TR[j - col].Index_vet].snom / sprInfo.TrInfo[spis_moshn_TR[j - col].Index_vet].klassNomNapr / (float)Math.Sqrt(3);
                                                for (int q = 1; q < optim_raschet.Count; q++)
                                                {
                                                    optim_raschet[q][i].R = optim_raschet[0][i].R;
                                                    optim_raschet[q][i].X = optim_raschet[0][i].X;
                                                    optim_raschet[q][i].dPxx = optim_raschet[0][i].dPxx;
                                                    optim_raschet[q][i].dQxx = optim_raschet[0][i].dQxx;
                                                    optim_raschet[q][i].ktr = optim_raschet[0][i].ktr;
                                                    optim_raschet[q][i].Idop = optim_raschet[0][i].Idop;
                                                }
                                            }
                                            break;
                                        }

                                }
                            }
                        }
                    }


                }
                // пересчет режима и потерь
                effect[3, 1] = 0;
                List<int> spisok = new List<int>();
                for (int i = 0; i < Tr_list.Count; i++)
                    spisok.Add(Tr_list[i].Index_vet);
                for (int t = 0; t < optim_raschet.Count; t++)
                {
                    Opredelit_Poradok_seti_Opt(t, true);
                    Add_regim_info_Opt(t, true, true,spisok);
                    for (int i = (poradok_rascheta.Count - 1); i >= 0; i--)
                    {
                        Potok_vetvi_raschet_obr_Opt(t, poradok_rascheta[i], false);
                    }
                    for (int i = 0; i < poradok_rascheta.Count; i++)
                    {
                        Napr_vetvi_raschet_pr_Opt(t, poradok_rascheta[i]);
                    }
                    Add_regim_info_Opt(t, true, false,new List<int>());
                    for (int i = (poradok_rascheta.Count - 1); i >= 0; i--)
                    {
                        Potok_vetvi_raschet_obr_Opt(t, poradok_rascheta[i], true);
                    }
                    for (int i = 0; i < optim_raschet[t].Count; i++)
                    {
                        raschet_Tokov_Opt(t, i);
                        effect[3, 1] += optim_raschet[t][i].dPxx + optim_raschet[t][i].dP;
                    }                  
                }
            }
            else
            {
                effect[3, 1] = effect[3, 0];
            }
            return new List<float> { effect[0, 0] - effect[0, 1], effect[1, 0] - effect[1, 1],
                    effect[2, 0] - effect[2, 1], effect[3, 0] - effect[3, 1] };


        }
        public void Apply_KA()
        {
            if (razm != 0)
            {              
                    for (int j = 0; j < BaseShema.Vet.Count; j++)
                    {
                        if (BaseShema.Vet[j].k1 != -1)
                            BaseShema.Vet[j].k1 = spisok_KA_value[0][opt_KA[0]][j];
                        if (BaseShema.Vet[j].k2 != -1)
                            BaseShema.Vet[j].k2 = spisok_KA_value[0][opt_KA[0]][j];
                    }

                //List<List<int>> ka = new List<List<int>>();
                //for (int z = 0; z < period.Count; z++)
                //{
                //    for (int j = 0; j < period[z]; j++)
                //    {
                //        ka.Add(new List<int>());
                //        for (int i = 0; i < spisok_KA_value[z][0].Count; i++)
                //            ka[ka.Count - 1].Add(spisok_KA_value[z][opt_KA[z]][i]);
                //    }
                //}
            }
        }
        private L_T_Vetvi Copy_Vetv(int T, int j)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, optim_raschet[T][j]);
                ms.Position = 0;
                L_T_Vetvi vet = (L_T_Vetvi)bf.Deserialize(ms);
                return vet;
            }
        }
        private L_T_Vetvi Copy_Vetv(ref List<List<L_T_Vetvi>> lv, int n, int z)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, lv[n][z]);
                ms.Position = 0;
                L_T_Vetvi vet = (L_T_Vetvi)bf.Deserialize(ms);
                return vet;
            }
        }
        private L_T_Vetvi Copy_Vetv(ref List<L_T_Vetvi> lv, int n)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, lv[n]);
                ms.Position = 0;
                L_T_Vetvi vet = (L_T_Vetvi)bf.Deserialize(ms);
                return vet;
            }
        }
        private bool Sravn_vet(int T, int v1, int v2)
        {
            if (optim_raschet[T][v1].nach_AO == optim_raschet[T][v2].nach_AO || optim_raschet[T][v1].nach_AO == optim_raschet[T][v2].kon_AO ||
                optim_raschet[T][v1].kon_AO == optim_raschet[T][v2].nach_AO || optim_raschet[T][v1].kon_AO == optim_raschet[T][v2].kon_AO)
                return true;
            else
                return false;
        }
        private bool Sravn_vet(int T, int v1, int v2, bool nachV1)
        {
            if (nachV1)
            {
                if (optim_raschet[T][v1].nach_AO == optim_raschet[T][v2].nach_AO || optim_raschet[T][v1].nach_AO == optim_raschet[T][v2].kon_AO)
                    return true;
                else
                    return false;
            }
            else
            {
                if (optim_raschet[T][v1].kon_AO == optim_raschet[T][v2].nach_AO || optim_raschet[T][v1].kon_AO == optim_raschet[T][v2].kon_AO)
                    return true;
                else
                    return false;
            }
        }
        private void eff_Mesta_razr(int T)
        {
            List<List<L_T_Vetvi>> kontur = new List<List<L_T_Vetvi>>(), cash_kontur = new List<List<L_T_Vetvi>>();
            List<List<float>> Pnagr = new List<List<float>>();
            List<List<float>> Qnagr = new List<List<float>>();
            List<int> ochered = new List<int>(), provereno = new List<int>(), mesto_r = new List<int>();
            List<List<int>> ssilka = new List<List<int>>();
            List<int> ochered_napr = new List<int>();
            List<float> effect = new List<float>();
            bool opt = false, end = false, prov = false, vverh, vniz, obr, new_otkl = false, prom = false, sovp = false;
            float tek_pot, next_pot, summ_effect = 0, jz;
            int nov = -2, k = 0, colK = 0, tekR, n, l, n_obr, best_kontur = 0, mr_cash;
                while (!opt) // перебор всех разрезов и нахождение их оптимального расположения с пересчетом потерь
                {
                    //if (T == 1)
                    //    in_test(1, true);
                    tekR = otkl_vetvi[T][k];            // индекс рассматриваемого разрыва
                    while (optim_raschet[T][tekR].ka != 0)
                    {
                        otkl_vetvi[T].RemoveAt(k);
                        if (k == otkl_vetvi[T].Count)
                            k = 0;
                        tekR = otkl_vetvi[T][k];
                    }
                    // Добавление контура по одному разрыву
                    kontur.Clear(); ssilka.Clear(); ochered.Clear(); provereno.Clear(); mesto_r.Clear(); effect.Clear();
                    kontur.Add(new List<L_T_Vetvi>()); ssilka.Add(new List<int>());
                    kontur[0].Add(Copy_Vetv(T, tekR)); mesto_r.Add(0); ssilka[0].Add(tekR);
                    //if (T == 8)
                    //    Console.Write("");
                    if (kontur[0][0].posle.Count == 2)
                    {
                        for (int q = 0; q < (kontur[0][q].posle.Count>2?2:kontur[0][q].posle.Count) ; q++)
                            if (optim_raschet[T][kontur[0][q].posle[q]].ka != 0)
                            {
                                if (kontur[0].Count == 1)
                                {
                                    ssilka[0].Insert(0, kontur[0][0].posle[q]);
                                    kontur[0].Insert(0, Copy_Vetv(T, kontur[0][0].posle[q]));
                                    mesto_r[0]++;
                                }
                                else
                                {
                                    ssilka[0].Add(kontur[0][1].posle[q]);
                                    kontur[0].Add(Copy_Vetv(T, kontur[0][1].posle[q]));
                                }
                            }
                    }
                    else if (kontur[0][0].posle.Count == 1) // для цепочек отключенных ветвей
                    {
                        end = false;
                        l = 0;
                        ochered.Add(tekR);
                        provereno.Add(ochered[0]);
                        ssilka[0].Add(kontur[0][0].posle[0]);
                        kontur[0].Add(Copy_Vetv(T, kontur[0][0].posle[0]));

                        while (!end)
                        {
                            n = 0;
                            if ((kontur[n + l][0].posle.Count == 1) && (kontur[n + l][0].posle[0] != kontur[n + l][kontur[n + l].Count - 2].posle[0]))
                            {
                                ssilka[n + l].Insert(0, kontur[n + l][0].posle[0]);
                                kontur[n + l].Insert(0, Copy_Vetv(T, kontur[n + l][0].posle[0]));
                                mesto_r[n + l]++;
                                l++;
                            }
                            else
                            {
                                if (kontur[n + l][0].posle.Count + kontur[n + l][0].posle_razom.Count > 1)
                                {
                                    for (int q = 0; q < kontur[n + l][0].posle_razom.Count; q++)
                                        if (optim_raschet[T][kontur[n + l][0].posle_razom[q]].posle.Count < 2)
                                        {
                                            prov = true;
                                            for (int h = 0; h < provereno.Count; h++)
                                                if (kontur[n + l][0].posle_razom[q] == provereno[h])
                                                { prov = false; break; }
                                            if (prov)
                                            {
                                                provereno.Add(kontur[n + l][0].posle_razom[q]);
                                                ochered.Insert(n + l, kontur[n + l][0].posle_razom[q]);
                                                ssilka.Insert(n + l + 1, new List<int>());
                                                kontur.Insert(n + l + 1, new List<L_T_Vetvi>());
                                                mesto_r.Insert(n + l + 1, mesto_r[n + l]);
                                                for (int z = 0; z < kontur[n + l].Count; z++)
                                                {
                                                    ssilka[n + l + 1].Add(ssilka[n + l][z]);
                                                    kontur[n + l + 1].Add(Copy_Vetv(ref kontur, n + l, z));
                                                }
                                                ssilka[n + l].Insert(0, kontur[n + l][0].posle_razom[q]);
                                                kontur[n + l].Insert(0, Copy_Vetv(T, kontur[n + l][0].posle_razom[q]));
                                                mesto_r[n + l]++;
                                                n++;
                                            }
                                        }
                                    kontur.RemoveAt(n + l);
                                    ssilka.RemoveAt(n + l);
                                    mesto_r.RemoveAt(n + l);
                                }
                                else
                                { kontur.RemoveAt(l); ssilka.RemoveAt(l); mesto_r.RemoveAt(l); }
                            }
                            ochered.RemoveAt(0);
                            if (l == kontur.Count)
                                end = true;
                        }
                    }
                    //if(T==1)
                    //for (int z = 0; z < kontur.Count; z++)
                    //{
                    //    Console.WriteLine("контур:"+z.ToString());
                    //    for (int i = 0; i < kontur[z].Count; i++)
                    //        Console.WriteLine(BaseShema.UZ[kontur[z][i].nach_AO].nameCP + " - " + BaseShema.UZ[kontur[z][i].kon_AO].nameCP);
                    //}
                    //для всех контуров дойти до ЦП
                    //try
                    //{
                        for (int z = 0; z < kontur.Count; z++)
                        {
                            while (kontur[z][0].pred != -1)
                            { ssilka[z].Insert(0, kontur[z][0].pred); kontur[z].Insert(0, Copy_Vetv(T, kontur[z][0].pred)); mesto_r[z]++; }
                            while (kontur[z][kontur[z].Count - 1].pred != -1)
                            { ssilka[z].Add(kontur[z][kontur[z].Count - 1].pred); kontur[z].Add(Copy_Vetv(T, kontur[z][kontur[z].Count - 1].pred)); }
                            while (kontur[z][0].nach_AO == kontur[z][kontur[z].Count - 1].nach_AO && kontur[z][0].kon_AO == kontur[z][kontur[z].Count - 1].kon_AO)
                            { ssilka[z].RemoveAt(kontur[z].Count - 1); kontur[z].RemoveAt(kontur[z].Count - 1); ssilka[z].RemoveAt(0); kontur[z].RemoveAt(0); mesto_r[z]--; }
                        }
                    //}
                    //catch
                    //{
                    //    for (int z = 0; z < kontur.Count; z++)
                    //    {
                    //        Console.WriteLine("контур:");
                    //        for (int i = 0; i < kontur[z].Count; i++)
                    //            Console.WriteLine(BaseShema.UZ[kontur[z][i].nach_AO].nameCP + " - " + BaseShema.UZ[kontur[z][i].kon_AO].nameCP);
                    //    }
                    //}
                    // определение нагрузки в узлах
                    Pnagr.Clear(); Qnagr.Clear();
                    for (int i = 0; i < kontur.Count; i++)
                    {
                        Pnagr.Add(new List<float>()); Qnagr.Add(new List<float>());
                        for (int q = 0; q < mesto_r[i]; q++)
                        {
                            Pnagr[i].Add((kontur[i][q].napravlenie ? kontur[i][q].Pk : kontur[i][q].Pn) - (kontur[i][q + 1].napravlenie ? kontur[i][q + 1].Pn : kontur[i][q + 1].Pk));
                            Qnagr[i].Add((kontur[i][q].napravlenie ? kontur[i][q].Qk : kontur[i][q].Qn) - (kontur[i][q + 1].napravlenie ? kontur[i][q + 1].Qn : kontur[i][q + 1].Qk));
                        }
                        Pnagr[i].Add(0); Qnagr[i].Add(0);
                        for (int q = mesto_r[i] + 1; q < kontur[i].Count; q++)
                        {
                            Pnagr[i].Add((kontur[i][q].napravlenie ? kontur[i][q].Pk : kontur[i][q].Pn) - (kontur[i][q - 1].napravlenie ? kontur[i][q - 1].Pn : kontur[i][q - 1].Pk));
                            Qnagr[i].Add((kontur[i][q].napravlenie ? kontur[i][q].Qk : kontur[i][q].Qn) - (kontur[i][q - 1].napravlenie ? kontur[i][q - 1].Qn : kontur[i][q - 1].Qk));
                        }
                    }

                    using (MemoryStream ms = new MemoryStream())
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        bf.Serialize(ms, kontur);
                        ms.Position = 0;
                        cash_kontur = (List<List<L_T_Vetvi>>)bf.Deserialize(ms);
                    }
                    //Console.WriteLine("-----------------следующий откл. участок------------------- T="+ T.ToString());
                    //for (int q = 0; q < kontur.Count; q++)
                    //{
                    //    Console.WriteLine("контур №" + q.ToString());
                    //    for (int w = 0; w < kontur[q].Count; w++)
                    //        Console.WriteLine(BaseShema.UZ[kontur[q][w].nach_AO].nameCP.ToString() + " - " + BaseShema.UZ[kontur[q][w].kon_AO].nameCP.ToString() + " нагрузка: " + Pnagr[q][w].ToString() + " " + Qnagr[q][w].ToString() + "   строка " + ssilka[q][w].ToString());
                    //}
                    // нахождение оптимального разреза для каждого контура (если их несколько). Выбирается лишь с наименьшими потерями,т.е. наилучшим эффектом
                    //if (napr)
                    //{
                        nov = tekR;
                        for (int z = 0; z < kontur.Count; z++)
                        {

                            n_obr = 0;
                            effect.Add(0);
                            tek_pot = 0;
                            for (int q = 0; q < kontur[z].Count; q++)
                            {
                                tek_pot += kontur[z][q].dP + kontur[z][q].dPxx;
                                if (q != mesto_r[z])                // определяем текущую сумму потерь в контуре
                                    if (kontur[z][q].ka == 0)
                                        kontur[z][q].ka = -1;        // включаем все ветви кроме рассматриваемого разрыва (для соседних отключенных)
                            }
                            prov = false;
                            next_pot = tek_pot;
                            effect[z] = next_pot - tek_pot;
                            vverh = vniz = obr = false;
                            while (!vverh || !vniz)
                            {
                                //for (int q = 0; q < kontur.Count; q++)
                                //{
                                //    Console.WriteLine("контур №" + q.ToString());
                                //    for (int w = 0; w < kontur[q].Count; w++)
                                //        Console.WriteLine(BaseShema.UZ[kontur[q][w].nach_AO].nameCP.ToString() + " - " + BaseShema.UZ[kontur[q][w].kon_AO].nameCP.ToString() + " нагрузка: " + Pnagr[q][w].ToString() + " " + Qnagr[q][w].ToString() + "   строка " + ssilka[q][w].ToString());
                                //}
                                n = mesto_r[z];
                                if (mesto_r[z] == 0)
                                    vverh = true;
                                else if (mesto_r[z] == kontur[z].Count - 1)
                                    vniz = true;
                                mr_cash = mesto_r[z];
                                if (!vverh)                     // сдвиг разрыва вверх по контуру
                                {
                                    for (int q = mesto_r[z] - 1; q >= 0; q--)
                                    {
                                        if (kontur[z][q].ka == 1)
                                        {
                                            kontur[z][mesto_r[z]].ka = 1; kontur[z][q].ka = 0; n = q;
                                            Pnagr[z].RemoveAt(mesto_r[z]); Pnagr[z].Insert(n, 0);
                                            Qnagr[z].RemoveAt(mesto_r[z]); Qnagr[z].Insert(n, 0);
                                            if (q + 1 != mesto_r[z])
                                                for (int i = q + 1; i < mesto_r[z]; i++)
                                                    kontur[z][i].napravlenie = !kontur[z][i].napravlenie;
                                            if ((kontur[z][mesto_r[z]].nach_AO == kontur[z][mesto_r[z] - 1].kon_AO) || (kontur[z][mesto_r[z]].nach_AO == kontur[z][mesto_r[z] - 1].nach_AO))
                                                kontur[z][mesto_r[z]].napravlenie = false;
                                            else if ((kontur[z][mesto_r[z]].kon_AO == kontur[z][mesto_r[z] - 1].kon_AO) || (kontur[z][mesto_r[z]].kon_AO == kontur[z][mesto_r[z] - 1].nach_AO))
                                                kontur[z][mesto_r[z]].napravlenie = true;
                                            else
                                                System.Windows.Forms.MessageBox.Show("ошибка");
                                            mesto_r[z] = n;
                                            break;
                                        }
                                        if (q == 0)
                                        {
                                            n_obr = 1;
                                            vverh = true;
                                        }
                                    }
                                }
                                if (!vniz && vverh)             // сдвиг разрыва вниз по контуру
                                {
                                    n_obr++;
                                    if (n_obr > 1)
                                        obr = false;
                                    else
                                        obr = true;
                                    for (int q = mesto_r[z] + 1; q < kontur[z].Count; q++)
                                    {
                                        if (kontur[z][q].ka == 1)
                                        {
                                            kontur[z][mesto_r[z]].ka = 1; kontur[z][q].ka = 0; n = q;
                                            Pnagr[z].RemoveAt(mesto_r[z]); Pnagr[z].Insert(n, 0);
                                            Qnagr[z].RemoveAt(mesto_r[z]); Qnagr[z].Insert(n, 0);
                                            if (q - 1 != mesto_r[z])
                                                for (int i = q - 1; i > mesto_r[z]; i--)
                                                    kontur[z][i].napravlenie = !kontur[z][i].napravlenie;
                                            if ((kontur[z][mesto_r[z]].nach_AO == kontur[z][mesto_r[z] + 1].kon_AO) || (kontur[z][mesto_r[z]].nach_AO == kontur[z][mesto_r[z] + 1].nach_AO))
                                                kontur[z][mesto_r[z]].napravlenie = false;
                                            else if ((kontur[z][mesto_r[z]].kon_AO == kontur[z][mesto_r[z] + 1].kon_AO) || (kontur[z][mesto_r[z]].kon_AO == kontur[z][mesto_r[z] + 1].nach_AO))
                                                kontur[z][mesto_r[z]].napravlenie = true;
                                            else
                                                System.Windows.Forms.MessageBox.Show("ошибка");
                                            mesto_r[z] = n;
                                            break;
                                        }
                                        if (q == kontur[z].Count-1)
                                            vniz = true;
                                    }
                                }
                                if (ssilka[z][mesto_r[z]] != tekR && !obr)
                                {
                                    for (int q = 0; q < kontur[z].Count; q++)
                                    {
                                        kontur[z][q].dP = 0; kontur[z][q].dQ = 0;
                                        kontur[z][q].Pn = 0; kontur[z][q].Qn = 0;
                                        kontur[z][q].Pk = 0; kontur[z][q].Qk = 0;
                                    }
                                    for (int q = n - 1; q >= 0; q--)          //расчет режима до разреза (обр. ход с ном напряж)
                                    {
                                        if (kontur[z][q].napravlenie)
                                        {
                                            kontur[z][q].Pk = Pnagr[z][q] + (kontur[z][q + 1].napravlenie ? kontur[z][q + 1].Pn : kontur[z][q + 1].Pk);
                                            kontur[z][q].Qk = Qnagr[z][q] + (kontur[z][q + 1].napravlenie ? kontur[z][q + 1].Qn : kontur[z][q + 1].Qk);
                                            kontur[z][q].dP = ((float)Math.Pow(kontur[z][q].Pk, 2) + (float)Math.Pow(kontur[z][q].Qk, 2)) / (float)Math.Pow(kontur[z][q].Unom, 2) * kontur[z][q].R / 1000;
                                            kontur[z][q].dQ = ((float)Math.Pow(kontur[z][q].Pk, 2) + (float)Math.Pow(kontur[z][q].Qk, 2)) / (float)Math.Pow(kontur[z][q].Unom, 2) * kontur[z][q].X / 1000;
                                            if (kontur[z][q].dPxx != 0)
                                                kontur[z][q].dPxx = kontur[z][q].dPxx * (float)Math.Pow((kontur[z][q].Unom / kontur[z][q].Un), 2);
                                            if (kontur[z][q].dQxx != 0)
                                                kontur[z][q].dQxx = kontur[z][q].dQxx * (float)Math.Pow((kontur[z][q].Unom / kontur[z][q].Un), 2);
                                            kontur[z][q].Pn = kontur[z][q].Pk + kontur[z][q].dP + kontur[z][q].dPxx;
                                            kontur[z][q].Qn = kontur[z][q].Qk + kontur[z][q].dQ + kontur[z][q].dQxx;
                                        }
                                        else
                                        {
                                            kontur[z][q].Pn = Pnagr[z][q] + (kontur[z][q + 1].napravlenie ? kontur[z][q + 1].Pn : kontur[z][q + 1].Pk);
                                            kontur[z][q].Qn = Qnagr[z][q] + (kontur[z][q + 1].napravlenie ? kontur[z][q + 1].Qn : kontur[z][q + 1].Qk);
                                            kontur[z][q].dP = ((float)Math.Pow(kontur[z][q].Pn, 2) + (float)Math.Pow(kontur[z][q].Qn, 2)) / (float)Math.Pow(kontur[z][q].Unom, 2) * kontur[z][q].R / 1000;
                                            kontur[z][q].dQ = ((float)Math.Pow(kontur[z][q].Pn, 2) + (float)Math.Pow(kontur[z][q].Qn, 2)) / (float)Math.Pow(kontur[z][q].Unom, 2) * kontur[z][q].X / 1000;
                                            if (kontur[z][q].dPxx != 0)
                                                kontur[z][q].dPxx = kontur[z][q].dPxx * (float)Math.Pow((kontur[z][q].Unom / kontur[z][q].Uk), 2);
                                            if (kontur[z][q].dQxx != 0)
                                                kontur[z][q].dQxx = kontur[z][q].dQxx * (float)Math.Pow((kontur[z][q].Unom / kontur[z][q].Uk), 2);
                                            kontur[z][q].Pk = kontur[z][q].Pn + kontur[z][q].dP + kontur[z][q].dPxx;
                                            kontur[z][q].Qk = kontur[z][q].Qn + kontur[z][q].dQ + kontur[z][q].dQxx;
                                        }
                                    }
                                    for (int q = 0; q < n; q++)          //расчет режима до разреза (прям. ход c действ. напряжением)
                                    {
                                        if (kontur[z][q].napravlenie)
                                        {
                                            if (q != 0)
                                                kontur[z][q].Un = kontur[z][q - 1].napravlenie ? kontur[z][q - 1].Uk : kontur[z][q - 1].Un;
                                            else
                                                if (kontur[z][q].Un == 0)
                                                    kontur[z][q].Un = kontur[z][kontur[z].Count - 1].napravlenie ? kontur[z][kontur[z].Count - 1].Un : kontur[z][kontur[z].Count - 1].Uk;  // только если узел не ЦП
                                            kontur[z][q].dU = ((kontur[z][q].Pn - kontur[z][q].dPxx) * kontur[z][q].R + (kontur[z][q].Qn - kontur[z][q].dQxx) * kontur[z][q].X) / kontur[z][q].Un / 1000;
                                            kontur[z][q].Uk = (kontur[z][q].Un - kontur[z][q].dU) * kontur[z][q].ktr;
                                        }
                                        else
                                        {
                                            if (q != 0)
                                                kontur[z][q].Uk = kontur[z][q - 1].napravlenie ? kontur[z][q - 1].Uk : kontur[z][q - 1].Un;
                                            else
                                                if (kontur[z][q].Uk == 0)
                                                    kontur[z][q].Uk = kontur[z][kontur[z].Count - 1].napravlenie ? kontur[z][kontur[z].Count - 1].Un : kontur[z][kontur[z].Count - 1].Uk;  // только если узел не ЦП
                                            kontur[z][q].dU = ((kontur[z][q].Pk - kontur[z][q].dPxx) * kontur[z][q].R + (kontur[z][q].Qk - kontur[z][q].dQxx) * kontur[z][q].X) / kontur[z][q].Uk / 1000 / kontur[z][q].ktr;
                                            kontur[z][q].Un = kontur[z][q].Uk / kontur[z][q].ktr - kontur[z][q].dU;
                                        }
                                    }
                                    for (int q = n - 1; q >= 0; q--)          //расчет режима до разреза (обр. ход с действ. напряж)
                                    {
                                        if (kontur[z][q].napravlenie)
                                        {
                                            kontur[z][q].Pk = Pnagr[z][q] + (kontur[z][q + 1].napravlenie ? kontur[z][q + 1].Pn : kontur[z][q + 1].Pk);
                                            kontur[z][q].Qk = Qnagr[z][q] + (kontur[z][q + 1].napravlenie ? kontur[z][q + 1].Qn : kontur[z][q + 1].Qk);
                                            kontur[z][q].dP = ((float)Math.Pow(kontur[z][q].Pk, 2) + (float)Math.Pow(kontur[z][q].Qk, 2)) / (float)Math.Pow(kontur[z][q].Uk / kontur[z][q].ktr, 2) * kontur[z][q].R / 1000;
                                            kontur[z][q].dQ = ((float)Math.Pow(kontur[z][q].Pk, 2) + (float)Math.Pow(kontur[z][q].Qk, 2)) / (float)Math.Pow(kontur[z][q].Uk / kontur[z][q].ktr, 2) * kontur[z][q].X / 1000;
                                            if (kontur[z][q].dPxx != 0)
                                                kontur[z][q].dPxx = kontur[z][q].dPxx * (float)Math.Pow((kontur[z][q].Un / kontur[z][q].Unom), 2);
                                            if (kontur[z][q].dQxx != 0)
                                                kontur[z][q].dQxx = kontur[z][q].dQxx * (float)Math.Pow((kontur[z][q].Un / kontur[z][q].Unom), 2);
                                            kontur[z][q].Pn = kontur[z][q].Pk + kontur[z][q].dP + kontur[z][q].dPxx;
                                            kontur[z][q].Qn = kontur[z][q].Qk + kontur[z][q].dQ + kontur[z][q].dQxx;
                                        }
                                        else
                                        {
                                            kontur[z][q].Pn = Pnagr[z][q] + (kontur[z][q + 1].napravlenie ? kontur[z][q + 1].Pn : kontur[z][q + 1].Pk);
                                            kontur[z][q].Qn = Qnagr[z][q] + (kontur[z][q + 1].napravlenie ? kontur[z][q + 1].Qn : kontur[z][q + 1].Qk);
                                            kontur[z][q].dP = ((float)Math.Pow(kontur[z][q].Pn, 2) + (float)Math.Pow(kontur[z][q].Qn, 2)) / (float)Math.Pow(kontur[z][q].Un, 2) * kontur[z][q].R / 1000;
                                            kontur[z][q].dQ = ((float)Math.Pow(kontur[z][q].Pn, 2) + (float)Math.Pow(kontur[z][q].Qn, 2)) / (float)Math.Pow(kontur[z][q].Un, 2) * kontur[z][q].X / 1000;
                                            if (kontur[z][q].dPxx != 0)
                                                kontur[z][q].dPxx = kontur[z][q].dPxx * (float)Math.Pow((kontur[z][q].Uk / kontur[z][q].Unom), 2);
                                            if (kontur[z][q].dQxx != 0)
                                                kontur[z][q].dQxx = kontur[z][q].dQxx * (float)Math.Pow((kontur[z][q].Uk / kontur[z][q].Unom), 2);
                                            kontur[z][q].Pk = kontur[z][q].Pn + kontur[z][q].dP + kontur[z][q].dPxx;
                                            kontur[z][q].Qk = kontur[z][q].Qn + kontur[z][q].dQ + kontur[z][q].dQxx;
                                        }
                                    }
                                    // то же самое для ветвей после разрыва
                                    for (int q = n + 1; q < kontur[z].Count; q++)          //расчет режима после разреза (обр. ход с ном напряж)
                                    {
                                        if (kontur[z][q].napravlenie)
                                        {
                                            kontur[z][q].Pk = Pnagr[z][q] + (kontur[z][q - 1].napravlenie ? kontur[z][q - 1].Pn : kontur[z][q - 1].Pk);
                                            kontur[z][q].Qk = Qnagr[z][q] + (kontur[z][q - 1].napravlenie ? kontur[z][q - 1].Qn : kontur[z][q - 1].Qk);
                                            kontur[z][q].dP = ((float)Math.Pow(kontur[z][q].Pk, 2) + (float)Math.Pow(kontur[z][q].Qk, 2)) / (float)Math.Pow(kontur[z][q].Unom, 2) * kontur[z][q].R / 1000;
                                            kontur[z][q].dQ = ((float)Math.Pow(kontur[z][q].Pk, 2) + (float)Math.Pow(kontur[z][q].Qk, 2)) / (float)Math.Pow(kontur[z][q].Unom, 2) * kontur[z][q].X / 1000;
                                            if (kontur[z][q].dPxx != 0)
                                                kontur[z][q].dPxx = kontur[z][q].dPxx * (float)Math.Pow((kontur[z][q].Unom / kontur[z][q].Un), 2);
                                            if (kontur[z][q].dQxx != 0)
                                                kontur[z][q].dQxx = kontur[z][q].dQxx * (float)Math.Pow((kontur[z][q].Unom / kontur[z][q].Un), 2);
                                            kontur[z][q].Pn = kontur[z][q].Pk + kontur[z][q].dP + kontur[z][q].dPxx;
                                            kontur[z][q].Qn = kontur[z][q].Qk + kontur[z][q].dQ + kontur[z][q].dQxx;
                                        }
                                        else
                                        {
                                            kontur[z][q].Pn = Pnagr[z][q] + (kontur[z][q - 1].napravlenie ? kontur[z][q - 1].Pn : kontur[z][q - 1].Pk);
                                            kontur[z][q].Qn = Qnagr[z][q] + (kontur[z][q - 1].napravlenie ? kontur[z][q - 1].Qn : kontur[z][q - 1].Qk);
                                            kontur[z][q].dP = ((float)Math.Pow(kontur[z][q].Pn, 2) + (float)Math.Pow(kontur[z][q].Qn, 2)) / (float)Math.Pow(kontur[z][q].Unom, 2) * kontur[z][q].R / 1000;
                                            kontur[z][q].dQ = ((float)Math.Pow(kontur[z][q].Pn, 2) + (float)Math.Pow(kontur[z][q].Qn, 2)) / (float)Math.Pow(kontur[z][q].Unom, 2) * kontur[z][q].X / 1000;
                                            if (kontur[z][q].dPxx != 0)
                                                kontur[z][q].dPxx = kontur[z][q].dPxx * (float)Math.Pow((kontur[z][q].Unom / kontur[z][q].Uk), 2);
                                            if (kontur[z][q].dQxx != 0)
                                                kontur[z][q].dQxx = kontur[z][q].dQxx * (float)Math.Pow((kontur[z][q].Unom / kontur[z][q].Uk), 2);
                                            kontur[z][q].Pk = kontur[z][q].Pn + kontur[z][q].dP + kontur[z][q].dPxx;
                                            kontur[z][q].Qk = kontur[z][q].Qn + kontur[z][q].dQ + kontur[z][q].dQxx;
                                        }
                                    }
                                    for (int q = kontur[z].Count - 1; q > n; q--)          //расчет режима после разреза (прям. ход)
                                    {
                                        if (kontur[z][q].napravlenie)
                                        {
                                            if (q != kontur[z].Count - 1)
                                                kontur[z][q].Un = kontur[z][q + 1].napravlenie ? kontur[z][q + 1].Uk : kontur[z][q + 1].Un;
                                            else
                                                if (kontur[z][q].Un == 0)
                                                    kontur[z][q].Un = kontur[z][0].napravlenie ? kontur[z][0].Un : kontur[z][0].Uk;  // только если узел не ЦП
                                            kontur[z][q].dU = ((kontur[z][q].Pn - kontur[z][q].dPxx) * kontur[z][q].R + (kontur[z][q].Qn - kontur[z][q].dQxx) * kontur[z][q].X) / kontur[z][q].Un / 1000;
                                            kontur[z][q].Uk = (kontur[z][q].Un - kontur[z][q].dU) * kontur[z][q].ktr;
                                        }
                                        else
                                        {
                                            if (q != kontur[z].Count - 1)
                                                kontur[z][q].Uk = kontur[z][q + 1].napravlenie ? kontur[z][q + 1].Uk : kontur[z][q + 1].Un;
                                            else
                                                if (kontur[z][q].Uk == 0)
                                                    kontur[z][q].Uk = kontur[z][0].napravlenie ? kontur[z][0].Un : kontur[z][0].Uk;  // только если узел не ЦП
                                            kontur[z][q].dU = ((kontur[z][q].Pk - kontur[z][q].dPxx) * kontur[z][q].R + (kontur[z][q].Qk - kontur[z][q].dQxx) * kontur[z][q].X) / kontur[z][q].Uk / 1000 / kontur[z][q].ktr;
                                            kontur[z][q].Un = kontur[z][q].Uk / kontur[z][q].ktr - kontur[z][q].dU;
                                        }
                                    }
                                    for (int q = n + 1; q < kontur[z].Count; q++)          //расчет режима после разреза (обр. ход с действ. напряж)
                                    {
                                        if (kontur[z][q].napravlenie)
                                        {
                                            kontur[z][q].Pk = Pnagr[z][q] + (kontur[z][q - 1].napravlenie ? kontur[z][q - 1].Pn : kontur[z][q - 1].Pk);
                                            kontur[z][q].Qk = Qnagr[z][q] + (kontur[z][q - 1].napravlenie ? kontur[z][q - 1].Qn : kontur[z][q - 1].Qk);
                                            kontur[z][q].dP = ((float)Math.Pow(kontur[z][q].Pk, 2) + (float)Math.Pow(kontur[z][q].Qk, 2)) / (float)Math.Pow(kontur[z][q].Uk / kontur[z][q].ktr, 2) * kontur[z][q].R / 1000;
                                            kontur[z][q].dQ = ((float)Math.Pow(kontur[z][q].Pk, 2) + (float)Math.Pow(kontur[z][q].Qk, 2)) / (float)Math.Pow(kontur[z][q].Uk / kontur[z][q].ktr, 2) * kontur[z][q].X / 1000;
                                            if (kontur[z][q].dPxx != 0)
                                                kontur[z][q].dPxx = kontur[z][q].dPxx * (float)Math.Pow((kontur[z][q].Un / kontur[z][q].Unom), 2);
                                            if (kontur[z][q].dQxx != 0)
                                                kontur[z][q].dQxx = kontur[z][q].dQxx * (float)Math.Pow((kontur[z][q].Un / kontur[z][q].Unom), 2);
                                            kontur[z][q].Pn = kontur[z][q].Pk + kontur[z][q].dP + kontur[z][q].dPxx;
                                            kontur[z][q].Qn = kontur[z][q].Qk + kontur[z][q].dQ + kontur[z][q].dQxx;
                                        }
                                        else
                                        {
                                            kontur[z][q].Pn = Pnagr[z][q] + (kontur[z][q - 1].napravlenie ? kontur[z][q - 1].Pn : kontur[z][q - 1].Pk);
                                            kontur[z][q].Qn = Qnagr[z][q] + (kontur[z][q - 1].napravlenie ? kontur[z][q - 1].Qn : kontur[z][q - 1].Qk);
                                            kontur[z][q].dP = ((float)Math.Pow(kontur[z][q].Pn, 2) + (float)Math.Pow(kontur[z][q].Qn, 2)) / (float)Math.Pow(kontur[z][q].Un, 2) * kontur[z][q].R / 1000;
                                            kontur[z][q].dQ = ((float)Math.Pow(kontur[z][q].Pn, 2) + (float)Math.Pow(kontur[z][q].Qn, 2)) / (float)Math.Pow(kontur[z][q].Un, 2) * kontur[z][q].X / 1000;
                                            if (kontur[z][q].dPxx != 0)
                                                kontur[z][q].dPxx = kontur[z][q].dPxx * (float)Math.Pow((kontur[z][q].Uk / kontur[z][q].Unom), 2);
                                            if (kontur[z][q].dQxx != 0)
                                                kontur[z][q].dQxx = kontur[z][q].dQxx * (float)Math.Pow((kontur[z][q].Uk / kontur[z][q].Unom), 2);
                                            kontur[z][q].Pk = kontur[z][q].Pn + kontur[z][q].dP + kontur[z][q].dPxx;
                                            kontur[z][q].Qk = kontur[z][q].Qn + kontur[z][q].dQ + kontur[z][q].dQxx;
                                        }
                                    }
                                    // проверка на снижение потерь при перемещении разрыва
                                    next_pot = 0;
                                    for (int q = 0; q < kontur[z].Count; q++)
                                        next_pot += kontur[z][q].dP + kontur[z][q].dPxx;
                                    if (Math.Round(tek_pot - next_pot, 6) > effect[z])
                                    {
                                        effect[z] = (float)Math.Round(tek_pot - next_pot, 6);
                                        if (!vverh)
                                            vniz = true;
                                        using (MemoryStream ms = new MemoryStream())
                                        {
                                            BinaryFormatter bf = new BinaryFormatter();
                                            bf.Serialize(ms, kontur[z]);
                                            ms.Position = 0;
                                            //if (T==8) 
                                            //    Console.WriteLine("");
                                            cash_kontur[z] = (List<L_T_Vetvi>)bf.Deserialize(ms);
                                        }
                                    }
                                    else
                                    {
                                        if (!vverh)
                                        {
                                            vverh = true;
                                            if (vniz)
                                                mesto_r[z] = mr_cash;
                                        }
                                        else
                                        {
                                            vniz = true;
                                            mesto_r[z] = mr_cash;
                                        }
                                    }
                                }
                            }
                        }
                        // выбор лучшего контура
                        best_kontur = 0;
                        jz = 0;
                        for (int z = 0; z < cash_kontur.Count; z++)
                        {
                            if (effect[z] > jz)
                            {
                                jz = effect[z];
                                best_kontur = z;
                            }
                        }
                        if (mesto_r.Count > 0)
                        {
                            nov = ssilka[best_kontur][mesto_r[best_kontur]];
                            summ_effect += effect[best_kontur];
                        }
                    //}
                    //else // если убрать галочку "оптимизация по напряжению"
                    //{
                    //    // учет напряжения центров питания на изменение потерь в отходящих от контура линиях
                    //    // пока не реализовано
                    //}
                    if (nov != tekR)
                    {
                        
                        // если новый - применение к сети с поправкой всех данных
                        //for (int q = 0; q < kontur.Count; q++)
                        //{
                        //    Console.WriteLine("контур №" + q.ToString());
                        //    for (int w = 0; w < kontur[q].Count; w++)
                        //        Console.WriteLine(BaseShema.UZ[kontur[q][w].nach_AO].nameCP.ToString() + " - " + BaseShema.UZ[kontur[q][w].kon_AO].nameCP.ToString() + " нагрузка: " + Pnagr[q][w].ToString() + " " + Qnagr[q][w].ToString());
                        //}
                        //star_otkl = false;
                        new_otkl = false;
                        prom = false;
                        for (int z = 0; z < cash_kontur[best_kontur].Count; z++)
                        {
                            // Режимные параметры
                            optim_raschet[T][ssilka[best_kontur][z]].Pn = cash_kontur[best_kontur][z].Pn;
                            optim_raschet[T][ssilka[best_kontur][z]].Qn = cash_kontur[best_kontur][z].Qn;
                            optim_raschet[T][ssilka[best_kontur][z]].Pk = cash_kontur[best_kontur][z].Pk;
                            optim_raschet[T][ssilka[best_kontur][z]].Qk = cash_kontur[best_kontur][z].Qk;
                            optim_raschet[T][ssilka[best_kontur][z]].dP = cash_kontur[best_kontur][z].dP;
                            optim_raschet[T][ssilka[best_kontur][z]].dQ = cash_kontur[best_kontur][z].dQ;
                            optim_raschet[T][ssilka[best_kontur][z]].dPxx = cash_kontur[best_kontur][z].dPxx;
                            optim_raschet[T][ssilka[best_kontur][z]].dQxx = cash_kontur[best_kontur][z].dQxx;
                            optim_raschet[T][ssilka[best_kontur][z]].Uk = cash_kontur[best_kontur][z].Uk;
                            optim_raschet[T][ssilka[best_kontur][z]].dU = cash_kontur[best_kontur][z].dU;
                            optim_raschet[T][ssilka[best_kontur][z]].Un = cash_kontur[best_kontur][z].Un;
                            // цепочки последовательностей
                            //if ((T==8)&&(tekR==13))
                            //    Console.Write("");
                            if (cash_kontur[best_kontur][z].ka == 0) // для новой отключенной ветви
                            {
                                if (optim_raschet[T][ssilka[best_kontur][z]].pred != -1)                // если новая отключенная ветвь не от центра питания
                                    optim_raschet[T][ssilka[best_kontur][z]].posle.Add(cash_kontur[best_kontur][z].pred);   // добавляем посл. питающую ветвь
                                optim_raschet[T][ssilka[best_kontur][z]].pred = -1;                                            // предыдущая ветвь принимает -1
                                for (int j = 0; j < optim_raschet[T][ssilka[best_kontur][z]].posle.Count; j++)
                                {
                                    prov = false;       // показывает входит ли j-я ветвь в оптимизируемый контур
                                    for (int i = 0; i < ssilka[best_kontur].Count; i++)
                                        if (optim_raschet[T][ssilka[best_kontur][z]].posle[j] == ssilka[best_kontur][i])
                                        {
                                            prov = true;
                                            break;
                                        }
                                    if (!prov)      // если не входит
                                    {
                                        if (optim_raschet[T][optim_raschet[T][ssilka[best_kontur][z]].posle[j]].ka == 0) // если j-я ветвь отключена
                                        {
                                            sovp = false;
                                            for (int r = 0; r < optim_raschet[T][ssilka[best_kontur][z]].posle_razom.Count; r++)
                                                if (optim_raschet[T][ssilka[best_kontur][z]].posle_razom[r] == optim_raschet[T][ssilka[best_kontur][z]].posle[j])
                                                {
                                                    sovp = true;
                                                    break;
                                                }
                                            if(!sovp)
                                                optim_raschet[T][ssilka[best_kontur][z]].posle_razom.Add(optim_raschet[T][ssilka[best_kontur][z]].posle[j]);// перенос в после_разомкн
                                            optim_raschet[T][ssilka[best_kontur][z]].posle.RemoveAt(j);
                                            j--;
                                        }
                                        else   // если j-я ветвь включена
                                        {
                                            if (z == 0)             // если ветвь в начале оптимизируемого контура
                                            {
                                                optim_raschet[T][ssilka[best_kontur][z + 1]].posle.Add(optim_raschet[T][ssilka[best_kontur][z]].posle[j]);// перенос  посл. в следующую ветвь (2-ю)
                                                optim_raschet[T][ssilka[best_kontur][z]].posle.RemoveAt(j);
                                                j--;
                                            }
                                            else if (z == ssilka[best_kontur].Count - 1) // если ветвь в конце оптимизируемого контура
                                            {
                                                optim_raschet[T][ssilka[best_kontur][z - 1]].posle.Add(optim_raschet[T][ssilka[best_kontur][z]].posle[j]);
                                                optim_raschet[T][ssilka[best_kontur][z]].posle.RemoveAt(j);
                                                j--;
                                            }
                                            else
                                            {
                                                if ((optim_raschet[T][optim_raschet[T][ssilka[best_kontur][z]].posle[j]].napravlenie ?
                                                optim_raschet[T][optim_raschet[T][ssilka[best_kontur][z]].posle[j]].nach_AO :
                                                optim_raschet[T][optim_raschet[T][ssilka[best_kontur][z]].posle[j]].kon_AO) == (cash_kontur[best_kontur][z + 1].napravlenie ?
                                                cash_kontur[best_kontur][z + 1].kon_AO : cash_kontur[best_kontur][z + 1].nach_AO))
                                                {
                                                    if (prom)
                                                        optim_raschet[T][ssilka[best_kontur][z + 1]].posle.Add(optim_raschet[T][ssilka[best_kontur][z]].posle[j]);
                                                    optim_raschet[T][ssilka[best_kontur][z]].posle.RemoveAt(j);
                                                    j--;
                                                }
                                                else if ((optim_raschet[T][optim_raschet[T][ssilka[best_kontur][z]].posle[j]].napravlenie ?
                                                optim_raschet[T][optim_raschet[T][ssilka[best_kontur][z]].posle[j]].nach_AO :
                                                optim_raschet[T][optim_raschet[T][ssilka[best_kontur][z]].posle[j]].kon_AO) == (cash_kontur[best_kontur][z - 1].napravlenie ?
                                                cash_kontur[best_kontur][z - 1].kon_AO : cash_kontur[best_kontur][z - 1].nach_AO))
                                                {
                                                    if (prom)
                                                        optim_raschet[T][ssilka[best_kontur][z - 1]].posle.Add(optim_raschet[T][ssilka[best_kontur][z]].posle[j]);
                                                    optim_raschet[T][ssilka[best_kontur][z]].posle.RemoveAt(j);
                                                    j--;
                                                }
                                                else System.Windows.Forms.MessageBox.Show(" error1");
                                            }
                                        }
                                    }

                                }
                                for (int i = 0; i < otkl_vetvi[T].Count; i++)
                                {
                                    if (nov != otkl_vetvi[T][i])
                                    {
                                        for (int iq = 0; iq < optim_raschet[T][otkl_vetvi[T][i]].posle.Count; iq++)
                                            for (int ig = 0; ig < optim_raschet[T][nov].posle.Count; ig++)
                                                if (optim_raschet[T][nov].posle[ig] == optim_raschet[T][otkl_vetvi[T][i]].posle[iq])
                                                {
                                                    sovp = false;
                                                    for (int r = 0; r < optim_raschet[T][otkl_vetvi[T][i]].posle_razom.Count; r++)
                                                        if (optim_raschet[T][otkl_vetvi[T][i]].posle_razom[r] == nov)
                                                        {
                                                            sovp = true;
                                                            break;
                                                        }
                                                    if (!sovp)
                                                        optim_raschet[T][otkl_vetvi[T][i]].posle_razom.Add(nov);
                                                    sovp = false;
                                                    for (int r = 0; r < optim_raschet[T][nov].posle_razom.Count; r++)
                                                        if (optim_raschet[T][nov].posle_razom[r] == otkl_vetvi[T][i])
                                                        {
                                                            sovp = true;
                                                            break;
                                                        }
                                                    if (!sovp)
                                                        optim_raschet[T][nov].posle_razom.Add(otkl_vetvi[T][i]);
                                                    break;
                                                }

                                    }
                                }
                                new_otkl = true;
                            }
                            else if (optim_raschet[T][ssilka[best_kontur][z]].ka == 0)    // для старой отключенной ветви
                            {
                                if (cash_kontur[best_kontur][z].ka != -1)      // если ветвь была старым разрывом а не составляющей цепочки разомкнутых ветвей
                                {
                                    if (BaseShema.UZ[optim_raschet[T][ssilka[best_kontur][z]].nach_AO].tipUz != 0 && BaseShema.UZ[optim_raschet[T][ssilka[best_kontur][z]].kon_AO].tipUz != 0) // ветвь не содержит ЦП
                                        for (int j = 0; j < optim_raschet[T][ssilka[best_kontur][z]].posle.Count; j++)
                                        {
                                            if (z == 0) 
                                            {
                                                if (optim_raschet[T][ssilka[best_kontur][z]].posle[j] != ssilka[best_kontur][z + 1])
                                                {
                                                    optim_raschet[T][ssilka[best_kontur][z]].pred = optim_raschet[T][ssilka[best_kontur][z]].posle[j];
                                                    optim_raschet[T][ssilka[best_kontur][z]].posle.RemoveAt(j);
                                                    break;
                                                }
                                            }
                                            else if (z == ssilka[best_kontur].Count - 1)  
                                            {
                                                if (optim_raschet[T][ssilka[best_kontur][z]].posle[j] != ssilka[best_kontur][z - 1])
                                                {
                                                    optim_raschet[T][ssilka[best_kontur][z]].pred = optim_raschet[T][ssilka[best_kontur][z]].posle[j];
                                                    optim_raschet[T][ssilka[best_kontur][z]].posle.RemoveAt(j);
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                if (new_otkl)
                                                {
                                                    if (ssilka[best_kontur][z + 1] == optim_raschet[T][ssilka[best_kontur][z]].posle[j])
                                                    {
                                                        optim_raschet[T][ssilka[best_kontur][z]].pred = ssilka[best_kontur][z + 1];
                                                        optim_raschet[T][ssilka[best_kontur][z]].posle.RemoveAt(j);
                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    if (ssilka[best_kontur][z - 1] == optim_raschet[T][ssilka[best_kontur][z]].posle[j])
                                                    {
                                                        optim_raschet[T][ssilka[best_kontur][z]].pred = ssilka[best_kontur][z - 1];
                                                        optim_raschet[T][ssilka[best_kontur][z]].posle.RemoveAt(j);
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    for (int j = 0; j < optim_raschet[T][ssilka[best_kontur][z]].posle_razom.Count; j++)
                                    {

                                        if (optim_raschet[T][ssilka[best_kontur][z]].posle_razom[j] != (new_otkl ? ssilka[best_kontur][z - 1] : ssilka[best_kontur][z + 1]))
                                        {
                                            if (cash_kontur[best_kontur][z].napravlenie)
                                            {
                                                if (Sravn_vet(T, ssilka[best_kontur][z], optim_raschet[T][ssilka[best_kontur][z]].posle_razom[j], false))
                                                    optim_raschet[T][ssilka[best_kontur][z]].posle.Add(optim_raschet[T][ssilka[best_kontur][z]].posle_razom[j]);
                                            }
                                            else
                                            {
                                                if (Sravn_vet(T, ssilka[best_kontur][z], optim_raschet[T][ssilka[best_kontur][z]].posle_razom[j], true))
                                                    optim_raschet[T][ssilka[best_kontur][z]].posle.Add(optim_raschet[T][ssilka[best_kontur][z]].posle_razom[j]);
                                            }
                                            for (int x = 0; x < optim_raschet[T][optim_raschet[T][ssilka[best_kontur][z]].posle_razom[j]].posle.Count; x++)
                                                if (optim_raschet[T][optim_raschet[T][ssilka[best_kontur][z]].posle_razom[j]].posle[x] == nov)
                                                {
                                                    optim_raschet[T][optim_raschet[T][ssilka[best_kontur][z]].posle_razom[j]].posle.RemoveAt(x);
                                                    optim_raschet[T][optim_raschet[T][ssilka[best_kontur][z]].posle_razom[j]].posle_razom.Add(nov);
                                                    break;
                                                }
                                            for (int x = 0; x < optim_raschet[T][optim_raschet[T][ssilka[best_kontur][z]].posle_razom[j]].posle_razom.Count; x++)
                                                if (optim_raschet[T][optim_raschet[T][ssilka[best_kontur][z]].posle_razom[j]].posle_razom[x] == ssilka[best_kontur][z])
                                                {
                                                    optim_raschet[T][optim_raschet[T][ssilka[best_kontur][z]].posle_razom[j]].posle_razom.RemoveAt(x);
                                                    if (Sravn_vet(T, ssilka[best_kontur][z], optim_raschet[T][ssilka[best_kontur][z]].posle_razom[j], (cash_kontur[best_kontur][z].napravlenie ? false : true)))
                                                    {
                                                        for(int x1=0;x1<optim_raschet[T][optim_raschet[T][ssilka[best_kontur][z]].posle_razom[j]].posle.Count;x1++)
                                                            try
                                                            {
                                                                if (optim_raschet[T][optim_raschet[T][ssilka[best_kontur][z]].posle_razom[j]].posle[x1] == ssilka[best_kontur][(new_otkl ? (z - 1) : (z + 1))])
                                                                {
                                                                    optim_raschet[T][optim_raschet[T][ssilka[best_kontur][z]].posle_razom[j]].posle.RemoveAt(x1);
                                                                    break;
                                                                }
                                                            }
                                                            finally { }
                                                        optim_raschet[T][optim_raschet[T][ssilka[best_kontur][z]].posle_razom[j]].posle.Add(ssilka[best_kontur][z]);
                                                    }
                                                    break;
                                                }
                                        }
                                        else
                                        {
                                            optim_raschet[T][ssilka[best_kontur][z]].posle.Add(optim_raschet[T][ssilka[best_kontur][z]].posle_razom[j]);
                                        }
                                        optim_raschet[T][ssilka[best_kontur][z]].posle_razom.RemoveAt(j); j--;

                                    }

                                    if (new_otkl)
                                    {
                                        if (!((optim_raschet[T][ssilka[best_kontur][z - 1]].ka == 1) && (cash_kontur[best_kontur][z - 1].ka == -1)))
                                            for (int j = 0; j < cash_kontur[best_kontur][z - 1].posle.Count; j++)
                                                if (cash_kontur[best_kontur][z - 1].posle[j] != ssilka[best_kontur][z])
                                                    if (!poisk_int_v_spiske(cash_kontur[best_kontur][z - 1].posle[j], optim_raschet[T][ssilka[best_kontur][z]].posle))
                                                        optim_raschet[T][ssilka[best_kontur][z]].posle.Add(cash_kontur[best_kontur][z - 1].posle[j]);
                                    }
                                    else
                                    {
                                        if (!((optim_raschet[T][ssilka[best_kontur][z + 1]].ka == 0) && (cash_kontur[best_kontur][z + 1].ka == -1)))
                                            for (int j = 0; j < cash_kontur[best_kontur][z + 1].posle.Count; j++)
                                                if (cash_kontur[best_kontur][z + 1].posle[j] != ssilka[best_kontur][z])
                                                    if (!poisk_int_v_spiske(cash_kontur[best_kontur][z + 1].posle[j], optim_raschet[T][ssilka[best_kontur][z]].posle))
                                                        optim_raschet[T][ssilka[best_kontur][z]].posle.Add(cash_kontur[best_kontur][z + 1].posle[j]);
                                    }

                                    if (new_otkl)
                                    {
                                        if (Sravn_vet(T, ssilka[best_kontur][z], ssilka[best_kontur][z - 1], false))
                                            optim_raschet[T][ssilka[best_kontur][z]].napravlenie = true;
                                        else
                                            optim_raschet[T][ssilka[best_kontur][z]].napravlenie = false;
                                    }
                                    else
                                    {
                                        if (Sravn_vet(T, ssilka[best_kontur][z], ssilka[best_kontur][z + 1], false))
                                            optim_raschet[T][ssilka[best_kontur][z]].napravlenie = true;
                                        else
                                            optim_raschet[T][ssilka[best_kontur][z]].napravlenie = false;
                                    }
                                    //star_otkl = true;
                                }
                                else   // если ветвь была составляющей цепочки разомкнутых ветвей
                                {
                                    if (new_otkl)
                                    {
                                        optim_raschet[T][ssilka[best_kontur][z]].pred = ssilka[best_kontur][z + 1];
                                        optim_raschet[T][ssilka[best_kontur][z]].posle.Clear();
                                        optim_raschet[T][ssilka[best_kontur][z]].posle.Add(ssilka[best_kontur][z - 1]);
                                        for (int i1 = 0; i1 < cash_kontur[best_kontur][z - 1].posle.Count; i1++)
                                            if (cash_kontur[best_kontur][z - 1].posle[i1] != ssilka[best_kontur][z])
                                                optim_raschet[T][ssilka[best_kontur][z]].posle.Add(cash_kontur[best_kontur][z - 1].posle[i1]);
                                        optim_raschet[T][ssilka[best_kontur][z]].posle_razom.Clear();
                                        for (int i1 = 0; i1 < cash_kontur[best_kontur][z + 1].posle_razom.Count; i1++)
                                            for (int i2 = 0; i2 < cash_kontur[best_kontur][z].posle_razom.Count; i2++)
                                                if (cash_kontur[best_kontur][z + 1].posle_razom[i1] == cash_kontur[best_kontur][z].posle_razom[i2])
                                                    optim_raschet[T][ssilka[best_kontur][z]].posle.Add(cash_kontur[best_kontur][z].posle_razom[i2]);
                                    }
                                    else
                                    {
                                        optim_raschet[T][ssilka[best_kontur][z]].pred = ssilka[best_kontur][z - 1];
                                        optim_raschet[T][ssilka[best_kontur][z]].posle.Clear();
                                        optim_raschet[T][ssilka[best_kontur][z]].posle.Add(ssilka[best_kontur][z + 1]);
                                        for (int i1 = 0; i1 < cash_kontur[best_kontur][z + 1].posle.Count; i1++)
                                            if (cash_kontur[best_kontur][z - 1].posle[i1] != ssilka[best_kontur][z])
                                                optim_raschet[T][ssilka[best_kontur][z]].posle.Add(cash_kontur[best_kontur][z + 1].posle[i1]);
                                        optim_raschet[T][ssilka[best_kontur][z]].posle_razom.Clear();
                                        for (int i1 = 0; i1 < cash_kontur[best_kontur][z - 1].posle_razom.Count; i1++)
                                            for (int i2 = 0; i2 < cash_kontur[best_kontur][z].posle_razom.Count; i2++)
                                                if (cash_kontur[best_kontur][z - 1].posle_razom[i1] == cash_kontur[best_kontur][z].posle_razom[i2])
                                                    optim_raschet[T][ssilka[best_kontur][z]].posle.Add(cash_kontur[best_kontur][z].posle_razom[i2]);
                                    }
                                    prom = true;
                                }
                            }
                            // для промежуточной ветви
                            else if (cash_kontur[best_kontur][z].napravlenie != optim_raschet[T][ssilka[best_kontur][z]].napravlenie)
                            {
                                //if (T == 2 && fd == 5)
                                //    Console.Write('2');
                                if (new_otkl) // если новый разрез уже был создан
                                {
                                    optim_raschet[T][ssilka[best_kontur][z]].posle.Clear();
                                    for (int s = 0; s < cash_kontur[best_kontur][z - 1].posle.Count; s++) // для количества посл.ветвей в предыдущей записи
                                    {
                                        if (cash_kontur[best_kontur][z - 1].posle[s] != ssilka[best_kontur][z]) // если посл. ветвь не есть рассматриваемая
                                        {
                                            optim_raschet[T][ssilka[best_kontur][z]].posle.Add(cash_kontur[best_kontur][z - 1].posle[s]); // добавление сторонеей посл.ветви
                                            if (optim_raschet[T][optim_raschet[T][ssilka[best_kontur][z]].posle[s]].ka == 0)
                                            {
                                                for (int s1 = 0; s1 < optim_raschet[T][optim_raschet[T][ssilka[best_kontur][z]].posle[s]].posle.Count; s1++)
                                                {
                                                    if (optim_raschet[T][optim_raschet[T][ssilka[best_kontur][z]].posle[s]].posle[s1] == ssilka[best_kontur][z - 1])
                                                    {
                                                        optim_raschet[T][optim_raschet[T][ssilka[best_kontur][z]].posle[s]].posle.RemoveAt(s1);
                                                        optim_raschet[T][optim_raschet[T][ssilka[best_kontur][z]].posle[s]].posle.Add(ssilka[best_kontur][z]);
                                                        break;
                                                    }
                                                }
                                                if (cash_kontur[best_kontur][z - 1].ka == 0)
                                                {
                                                    prom = true;
                                                    sovp = false;
                                                    for (int r = 0; r < optim_raschet[T][optim_raschet[T][ssilka[best_kontur][z]].posle[s]].posle_razom.Count; r++)
                                                        if (optim_raschet[T][optim_raschet[T][ssilka[best_kontur][z]].posle[s]].posle_razom[r] == ssilka[best_kontur][z - 1])
                                                        {
                                                            sovp = true;
                                                            break;
                                                        }
                                                    if (!sovp)
                                                        optim_raschet[T][optim_raschet[T][ssilka[best_kontur][z]].posle[s]].posle_razom.Add(ssilka[best_kontur][z - 1]);
                                                }
                                            }
                                        }
                                        else
                                            optim_raschet[T][ssilka[best_kontur][z]].posle.Add(ssilka[best_kontur][z - 1]);
                                    }
                                    cash_kontur[best_kontur][z].pred = ssilka[best_kontur][z + 1];
                                }
                                else
                                {
                                    optim_raschet[T][ssilka[best_kontur][z]].posle.Clear();
                                    for (int s = 0; s < cash_kontur[best_kontur][z + 1].posle.Count; s++)
                                    {
                                        if (cash_kontur[best_kontur][z + 1].posle[s] != ssilka[best_kontur][z])
                                        {
                                            optim_raschet[T][ssilka[best_kontur][z]].posle.Add(cash_kontur[best_kontur][z + 1].posle[s]);
                                            if (optim_raschet[T][optim_raschet[T][ssilka[best_kontur][z]].posle[s]].ka == 0)
                                            {
                                                for (int s1 = 0; s < optim_raschet[T][optim_raschet[T][ssilka[best_kontur][z]].posle[s]].posle.Count; s1++)
                                                {
                                                    if (optim_raschet[T][optim_raschet[T][ssilka[best_kontur][z]].posle[s]].posle[s1] == ssilka[best_kontur][z + 1])
                                                    {
                                                        optim_raschet[T][optim_raschet[T][ssilka[best_kontur][z]].posle[s]].posle.RemoveAt(s1);
                                                        optim_raschet[T][optim_raschet[T][ssilka[best_kontur][z]].posle[s]].posle.Add(ssilka[best_kontur][z]);
                                                        break;
                                                    }
                                                }
                                                if (cash_kontur[best_kontur][z + 1].ka == 0)
                                                {
                                                    prom = true;
                                                    sovp = false;
                                                    for (int r = 0; r < optim_raschet[T][optim_raschet[T][ssilka[best_kontur][z]].posle[s]].posle_razom.Count; r++)
                                                        if (optim_raschet[T][optim_raschet[T][ssilka[best_kontur][z]].posle[s]].posle_razom[r] == ssilka[best_kontur][z + 1])
                                                        {
                                                            sovp = true;
                                                            break;
                                                        }
                                                    if (!sovp)
                                                    optim_raschet[T][optim_raschet[T][ssilka[best_kontur][z]].posle[s]].posle_razom.Add(ssilka[best_kontur][z + 1]);
                                                }
                                            }
                                        }
                                        else
                                            optim_raschet[T][ssilka[best_kontur][z]].posle.Add(ssilka[best_kontur][z + 1]);
                                    }
                                    cash_kontur[best_kontur][z].pred = ssilka[best_kontur][z - 1];
                                }
                                optim_raschet[T][ssilka[best_kontur][z]].napravlenie = cash_kontur[best_kontur][z].napravlenie;
                            }
                            if (cash_kontur[best_kontur][z].ka != 0)        // если рассматриваемая ветвь не новая откл.
                                for (int j = 0; j < optim_raschet[T][ssilka[best_kontur][z]].posle.Count; j++)
                                {
                                    if (optim_raschet[T][optim_raschet[T][ssilka[best_kontur][z]].posle[j]].ka != 0)
                                        if (BaseShema.UZ[optim_raschet[T][optim_raschet[T][ssilka[best_kontur][z]].posle[j]].nach_AO].tipUz != 0 &&
                                            BaseShema.UZ[optim_raschet[T][optim_raschet[T][ssilka[best_kontur][z]].posle[j]].kon_AO].tipUz != 0)
                                            optim_raschet[T][optim_raschet[T][ssilka[best_kontur][z]].posle[j]].pred = ssilka[best_kontur][z];
                                        else
                                            optim_raschet[T][optim_raschet[T][ssilka[best_kontur][z]].posle[j]].pred = -1;
                                }
                            // положение КА
                            if (cash_kontur[best_kontur][z].posle_razom.Count > 0 && cash_kontur[best_kontur][z].ka == -1)
                                optim_raschet[T][ssilka[best_kontur][z]].ka = 1;
                            else
                                optim_raschet[T][ssilka[best_kontur][z]].ka = cash_kontur[best_kontur][z].ka;
                            //// пересчет потерь с учетом напряжения в отходящих ветвях
                            //if (cash_kontur[best_kontur][z].ka != 0)        // если рассматриваемая ветвь не новая откл.
                            //{                             
                            //        ochered_napr.Clear();
                            //        for(int i = 0;i<optim_raschet[T][ssilka[best_kontur][z]].posle.Count;i++)
                            //        {
                            //            if (new_otkl)
                            //            {
                            //                if (optim_raschet[T][ssilka[best_kontur][z]].posle[i] != ssilka[best_kontur][z - 1])
                            //                {
                            //                    if (optim_raschet[T][optim_raschet[T][ssilka[best_kontur][z]].posle[i]].ka != 0)
                            //                    {
                            //                        ochered_napr.Add(optim_raschet[T][ssilka[best_kontur][z]].posle[i]);
                            //                        if (optim_raschet[T][ochered_napr[ochered_napr.Count - 1]].napravlenie)
                            //                            optim_raschet[T][ochered_napr[ochered_napr.Count - 1]].Un = optim_raschet[T][ssilka[best_kontur][z]].napravlenie ? optim_raschet[T][ssilka[best_kontur][z]].Uk : optim_raschet[T][ssilka[best_kontur][z]].Un;
                            //                        else
                            //                            optim_raschet[T][ochered_napr[ochered_napr.Count - 1]].Uk = optim_raschet[T][ssilka[best_kontur][z]].napravlenie ? optim_raschet[T][ssilka[best_kontur][z]].Uk : optim_raschet[T][ssilka[best_kontur][z]].Un;
                            //                    }
                            //                }
                            //            }
                            //            else
                            //            {
                            //                if (optim_raschet[T][ssilka[best_kontur][z]].posle[i] != ssilka[best_kontur][z + 1])
                            //                {
                            //                    if (optim_raschet[T][optim_raschet[T][ssilka[best_kontur][z]].posle[i]].ka != 0)
                            //                    {
                            //                        ochered_napr.Add(optim_raschet[T][ssilka[best_kontur][z]].posle[i]);
                            //                        if (optim_raschet[T][ochered_napr[ochered_napr.Count - 1]].napravlenie)
                            //                            optim_raschet[T][ochered_napr[ochered_napr.Count - 1]].Un = optim_raschet[T][ssilka[best_kontur][z]].napravlenie ? optim_raschet[T][ssilka[best_kontur][z]].Uk : optim_raschet[T][ssilka[best_kontur][z]].Un;
                            //                        else
                            //                            optim_raschet[T][ochered_napr[ochered_napr.Count - 1]].Uk = optim_raschet[T][ssilka[best_kontur][z]].napravlenie ? optim_raschet[T][ssilka[best_kontur][z]].Uk : optim_raschet[T][ssilka[best_kontur][z]].Un;
                            //                    }
                            //                }
                            //            }

                            //        }
                            //        while(ochered_napr.Count>0)
                            //        {
                            //            pereschet_poter_ot_napr(T, ochered_napr[0]);
                            //            for(int i = 0;i<optim_raschet[T][ochered_napr[0]].posle.Count;i++)
                            //            {     
                            //                if(optim_raschet[T][optim_raschet[T][ochered_napr[0]].posle[i]].ka!=0)
                            //                {
                            //                    ochered_napr.Add(optim_raschet[T][ochered_napr[0]].posle[i]);
                            //                    if(optim_raschet[T][ochered_napr[ochered_napr.Count-1]].napravlenie)
                            //                        optim_raschet[T][ochered_napr[ochered_napr.Count-1]].Un=optim_raschet[T][ochered_napr[0]].napravlenie? optim_raschet[T][ochered_napr[0]].Uk:optim_raschet[T][ochered_napr[0]].Un;
                            //                    else
                            //                        optim_raschet[T][ochered_napr[ochered_napr.Count-1]].Uk=optim_raschet[T][ochered_napr[0]].napravlenie? optim_raschet[T][ochered_napr[0]].Uk:optim_raschet[T][ochered_napr[0]].Un;
                            //                }                                   
                            //            }                                    
                            //            ochered_napr.RemoveAt(0);
                            //        }                                                         
                            //}
                        }
                        //in_test(T,false);
                        //if (T == 1)
                        //{
                        //    in_test(1, true); //fd++;
                        //}
                        //if (fd == 4)
                        //{ colK = otkl_vetvi[T].Count; opt = end = true; break; }
                        otkl_vetvi[T][k] = nov;
                        colK = 0;
                    }
                    else
                    {
                        colK++;
                        //if (T == 1)
                        //{
                        //in_test(1, true);
                        //fd++;
                        //    //Console.WriteLine(
                        //}
                        //if (fd == 6)
                        //{ colK = otkl_vetvi[T].Count; opt = end = true; break; }
                    }
                    if (colK == otkl_vetvi[T].Count)
                    {
                        opt = true;
                        //System.Windows.Forms.MessageBox.Show("Завершена оптимизация " + T +" часа"); 
                        break;
                    }
                    k++;
                    if (k == otkl_vetvi[T].Count)
                        k = 0;
                }
          
         
        }
        private void Napr_opt(int T)
        {
            int interval = 10;
            Log += "Момент времени T=" + T.ToString() + ":" + Environment.NewLine;
            bool end,end1;
            float kMax, kMin;
            float shag = 0;
            int n = 0;
            Uz_Napr maxU ,minU ;
            List<int> posled = new List<int>();
            List<int> numrl = new List<int>();
            kMax =  1 + 0.05f;
            kMin = 1 - 0.05f;
            for (int q = 0; q < CP_list.Count; q++)
            {
                maxU = new Uz_Napr(-1, 0, true);
                minU = new Uz_Napr(-1, 2, true);
                posled.Clear();
                numrl.Clear();
                // определим последовательность расчета от ЦП до нагрузок, и выберем контрольные узлы с высшим(низшим) напряжением
                for (int i=0;i<optim_raschet[T].Count;i++)
                    if (((optim_raschet[T][i].nach_AO == CP_list[q].Index) || (optim_raschet[T][i].kon_AO == CP_list[q].Index))&&(optim_raschet[T][i].numRL>=0))
                    {
                        posled.Add(i);
                        numrl.Add(optim_raschet[T][i].numRL);
                        if (optim_raschet[T][i].Un / BaseShema.UZ[optim_raschet[T][i].nach_AO].nomN > maxU.koef_k_Unom)
                        {
                            maxU.Index_vet = i;
                            maxU.koef_k_Unom = (float) optim_raschet[T][i].Un / BaseShema.UZ[optim_raschet[T][i].nach_AO].nomN;
                            maxU.nachalo = true;
                        }
                        if (optim_raschet[T][i].Uk / BaseShema.UZ[optim_raschet[T][i].kon_AO].nomN > maxU.koef_k_Unom)
                        {
                            maxU.Index_vet = i;
                            maxU.koef_k_Unom = optim_raschet[T][i].Uk / BaseShema.UZ[optim_raschet[T][i].kon_AO].nomN;
                            maxU.nachalo = false;
                        }
                        if (optim_raschet[T][i].Un / BaseShema.UZ[optim_raschet[T][i].nach_AO].nomN < minU.koef_k_Unom)
                        {
                            minU.Index_vet = i;
                            minU.koef_k_Unom = optim_raschet[T][i].Un / BaseShema.UZ[optim_raschet[T][i].nach_AO].nomN;
                            minU.nachalo = true;
                        }
                        if (optim_raschet[T][i].Uk / BaseShema.UZ[optim_raschet[T][i].kon_AO].nomN < minU.koef_k_Unom)
                        {
                            minU.Index_vet = i;
                            minU.koef_k_Unom = optim_raschet[T][i].Uk / BaseShema.UZ[optim_raschet[T][i].kon_AO].nomN;
                            minU.nachalo = false;
                        }
                    }
                n = 0;
                while (n < posled.Count)
                {
                    for (int i = 0; i < optim_raschet[T][posled[n]].posle.Count; i++)
                        if (optim_raschet[T][optim_raschet[T][posled[n]].posle[i]].ka != 0)
                        {
                            posled.Add(optim_raschet[T][posled[n]].posle[i]);
                            if (optim_raschet[T][optim_raschet[T][posled[n]].posle[i]].Un / BaseShema.UZ[optim_raschet[T][optim_raschet[T][posled[n]].posle[i]].nach_AO].nomN > maxU.koef_k_Unom)
                            {
                                maxU.Index_vet = optim_raschet[T][posled[n]].posle[i];
                                maxU.koef_k_Unom = optim_raschet[T][optim_raschet[T][posled[n]].posle[i]].Un / BaseShema.UZ[optim_raschet[T][optim_raschet[T][posled[n]].posle[i]].nach_AO].nomN;
                                maxU.nachalo = true;
                            }
                            if (optim_raschet[T][optim_raschet[T][posled[n]].posle[i]].Uk / BaseShema.UZ[optim_raschet[T][optim_raschet[T][posled[n]].posle[i]].kon_AO].nomN > maxU.koef_k_Unom)
                            {
                                maxU.Index_vet = optim_raschet[T][posled[n]].posle[i];
                                maxU.koef_k_Unom = optim_raschet[T][optim_raschet[T][posled[n]].posle[i]].Uk / BaseShema.UZ[optim_raschet[T][optim_raschet[T][posled[n]].posle[i]].kon_AO].nomN;
                                maxU.nachalo = false;
                            }
                            if (optim_raschet[T][optim_raschet[T][posled[n]].posle[i]].Un / BaseShema.UZ[optim_raschet[T][optim_raschet[T][posled[n]].posle[i]].nach_AO].nomN < minU.koef_k_Unom)
                            {
                                minU.Index_vet = optim_raschet[T][posled[n]].posle[i];
                                minU.koef_k_Unom = optim_raschet[T][optim_raschet[T][posled[n]].posle[i]].Un / BaseShema.UZ[optim_raschet[T][optim_raschet[T][posled[n]].posle[i]].nach_AO].nomN;
                                minU.nachalo = true;
                            }
                            if (optim_raschet[T][optim_raschet[T][posled[n]].posle[i]].Uk / BaseShema.UZ[optim_raschet[T][optim_raschet[T][posled[n]].posle[i]].kon_AO].nomN < minU.koef_k_Unom)
                            {
                                minU.Index_vet = optim_raschet[T][posled[n]].posle[i];
                                minU.koef_k_Unom = optim_raschet[T][optim_raschet[T][posled[n]].posle[i]].Uk / BaseShema.UZ[optim_raschet[T][optim_raschet[T][posled[n]].posle[i]].kon_AO].nomN;
                                minU.nachalo = false;
                            }
                        }
                    n++;
                }
                end = false;
                shag = 0;
                if ((maxU.koef_k_Unom>kMax)&&(minU.koef_k_Unom<kMin))
                {
                    Log+= "Невозможно оптимизировать напряжение в центре питания '"+BaseShema.UZ[CP_list[q].Index].nameCP.ToString()+
                        "', так как напряжение в некоторых узлах ("+BaseShema.UZ[(maxU.nachalo? optim_raschet[T][maxU.Index_vet].nach_AO:optim_raschet[T][maxU.Index_vet].kon_AO)].nameCP+", "+
                        BaseShema.UZ[(minU.nachalo ? optim_raschet[T][minU.Index_vet].nach_AO : optim_raschet[T][minU.Index_vet].kon_AO)].nameCP + ") вне диапазона допустимых значений" + Environment.NewLine;
                }
                else if (maxU.koef_k_Unom > kMax) //понижать напряжение до допустимого
                {
                    while (!end)
                    {
                        // корректировка напряжения в ЦП
                        for (int i = 0; i < posled.Count; i++)
                            if (optim_raschet[T][posled[i]].napravlenie)
                                if (optim_raschet[T][posled[i]].nach_AO == CP_list[q].Index)
                                {
                                    if (shag == 0)
                                        shag = (optim_raschet[T][posled[i]].Un / maxU.koef_k_Unom) * (float)Math.Round((maxU.koef_k_Unom - kMax),15) / interval;// за интервал примем одну пятую от превышения диапазона
                                    optim_raschet[T][posled[i]].Un = optim_raschet[T][posled[i]].Un - shag;
                                }
                                else
                                    break;
                            else
                          
                                if (optim_raschet[T][posled[i]].kon_AO == CP_list[q].Index)
                                {
                                    if (shag == 0)
                                    {
                                        shag = (optim_raschet[T][posled[i]].Uk / maxU.koef_k_Unom) * (float)Math.Round((maxU.koef_k_Unom - kMax), 15) / (float)interval;
                                    }
                                    optim_raschet[T][posled[i]].Uk = optim_raschet[T][posled[i]].Uk -shag;
                                }
                                else
                                    break;

                         // обратный ход (расчет напряжений)
                        for (int i = 0; i < posled.Count; i++)
                            Napr_vetvi_raschet_pr_Opt(T, posled[i]);
                        Add_regim_info_Opt(T, true,numrl);
                        for (int i = (posled.Count - 1); i >= 0; i--)
                            Potok_vetvi_raschet_obr_Opt(T, posled[i], true);
                        for (int i = 0; i < posled.Count; i++)
                            raschet_Tokov_Opt(T, posled[i]);
                        maxU.koef_k_Unom = (float)Math.Round(maxU.nachalo ? (optim_raschet[T][maxU.Index_vet].Un / BaseShema.UZ[optim_raschet[T][maxU.Index_vet].nach_AO].nomN) :
                            (optim_raschet[T][maxU.Index_vet].Uk / BaseShema.UZ[optim_raschet[T][maxU.Index_vet].kon_AO].nomN),5);
                        minU.koef_k_Unom = (float)Math.Round(minU.nachalo ? (optim_raschet[T][minU.Index_vet].Un / BaseShema.UZ[optim_raschet[T][minU.Index_vet].nach_AO].nomN) :
                            (optim_raschet[T][minU.Index_vet].Uk / BaseShema.UZ[optim_raschet[T][minU.Index_vet].kon_AO].nomN),5);
                        if ((maxU.koef_k_Unom > kMax) && (minU.koef_k_Unom < kMin))
                        {
                            Log += "Оптимизация напряжения в центре питания '" + BaseShema.UZ[CP_list[q].Index].nameCP.ToString() +
                        "' приостановлена на этапе, когда напряжение в узле " + BaseShema.UZ[(minU.nachalo ? optim_raschet[T][minU.Index_vet].nach_AO : optim_raschet[T][minU.Index_vet].kon_AO)].nameCP +
                        " вышло за диапазон допустимых значений" + Environment.NewLine;
                            end = true;
                        }
                        else if (minU.koef_k_Unom < kMin)
                        {
                            end1=false;
                            shag = 0;
                            while (!end1)
                            {
                                for (int i = 0; i < posled.Count; i++)
                                    if (optim_raschet[T][posled[i]].napravlenie)
                                        if (optim_raschet[T][posled[i]].nach_AO == CP_list[q].Index)
                                        {
                                            if (shag == 0)
                                                shag = (optim_raschet[T][posled[i]].Un / minU.koef_k_Unom) * (float)Math.Round((kMin - minU.koef_k_Unom), 15) / interval;// за интервал примем одну пятую от занижения диапазона
                                            optim_raschet[T][posled[i]].Un = optim_raschet[T][posled[i]].Un + shag;
                                        }
                                        else
                                            break;
                                    else

                                        if (optim_raschet[T][posled[i]].kon_AO == CP_list[q].Index)
                                        {
                                            if (shag == 0)
                                                shag = (optim_raschet[T][posled[i]].Uk / minU.koef_k_Unom) * (float)Math.Round((kMin - minU.koef_k_Unom), 15) / interval;
                                            optim_raschet[T][posled[i]].Uk = optim_raschet[T][posled[i]].Uk + shag;
                                        }
                                        else
                                            break;
                                // обратный ход (расчет напряжений)
                                for (int i = 0; i < posled.Count; i++)
                                    Napr_vetvi_raschet_pr_Opt(T, posled[i]);
                                Add_regim_info_Opt(T, true, numrl);
                                for (int i = (posled.Count - 1); i >= 0; i--)
                                    Potok_vetvi_raschet_obr_Opt(T, posled[i], true);
                                for (int i = 0; i < posled.Count; i++)
                                    raschet_Tokov_Opt(T, posled[i]);
                                maxU.koef_k_Unom = (float)Math.Round(maxU.nachalo ? (optim_raschet[T][maxU.Index_vet].Un / BaseShema.UZ[optim_raschet[T][maxU.Index_vet].nach_AO].nomN) :
                                    (optim_raschet[T][maxU.Index_vet].Uk / BaseShema.UZ[optim_raschet[T][maxU.Index_vet].kon_AO].nomN),5);
                                minU.koef_k_Unom = (float)Math.Round(minU.nachalo ? (optim_raschet[T][minU.Index_vet].Un / BaseShema.UZ[optim_raschet[T][minU.Index_vet].nach_AO].nomN) :
                                    (optim_raschet[T][minU.Index_vet].Uk / BaseShema.UZ[optim_raschet[T][minU.Index_vet].kon_AO].nomN),5);
                                if ((maxU.koef_k_Unom > kMax) && (minU.koef_k_Unom < kMin))
                                {
                                    Log += "Оптимизация напряжения в центре питания '" + BaseShema.UZ[CP_list[q].Index].nameCP.ToString() +
                                "' приостановлена на этапе, когда напряжения в узлах " + BaseShema.UZ[(maxU.nachalo ? optim_raschet[T][maxU.Index_vet].nach_AO : optim_raschet[T][maxU.Index_vet].kon_AO)].nameCP + ", " +
                                BaseShema.UZ[(minU.nachalo ? optim_raschet[T][minU.Index_vet].nach_AO : optim_raschet[T][minU.Index_vet].kon_AO)].nameCP +
                                " вышли за диапазон допустимых значений" + Environment.NewLine;
                                    end1 = true;
                                }
                                else if (minU.koef_k_Unom > kMin)
                                    end1 = true;
                            }
                            end = true;
                        }
                        else if (maxU.koef_k_Unom <= kMax)
                            end = true;     
                    }
                }
                else if ((kMax == maxU.koef_k_Unom)||((  maxU.koef_k_Unom > kMax*0.998) && (kMax > maxU.koef_k_Unom))||posled.Count==0)
                {
                    Log += "Изменение напряжения в центре питания '" + BaseShema.UZ[CP_list[q].Index].nameCP.ToString() + "' не требуется" + Environment.NewLine;
                    end = true;
                }
                else           //повышать напряжение до предела
                {
                    while (!end)
                    {
                        // корректировка напряжения в ЦП
                        for (int i = 0; i < posled.Count; i++)
                            if (optim_raschet[T][posled[i]].napravlenie)
                                if (optim_raschet[T][posled[i]].nach_AO == CP_list[q].Index)
                                {
                                    if (shag == 0)
                                        shag = (optim_raschet[T][posled[i]].Un / maxU.koef_k_Unom) * (float)Math.Round((kMax - maxU.koef_k_Unom), 15) / interval;// за интервал примем одну пятую от превышения диапазона
                                    optim_raschet[T][posled[i]].Un = optim_raschet[T][posled[i]].Un + shag;
                                }
                                else
                                    break;
                            else

                                if (optim_raschet[T][posled[i]].kon_AO == CP_list[q].Index)
                                {
                                    if (shag == 0)
                                        shag =(optim_raschet[T][posled[i]].Uk / maxU.koef_k_Unom) * (float)Math.Round((kMax - maxU.koef_k_Unom),15) / interval ;
                                    optim_raschet[T][posled[i]].Uk = optim_raschet[T][posled[i]].Uk + shag;
                                }
                                else
                                    break;
                        // обратный ход (расчет напряжений)
                        for (int i = 0; i < posled.Count; i++)
                            Napr_vetvi_raschet_pr_Opt(T, posled[i]);
                        Add_regim_info_Opt(T, true, numrl);
                        for (int i = (posled.Count - 1); i >= 0; i--)
                            Potok_vetvi_raschet_obr_Opt(T, posled[i], true);
                        for (int i = 0; i < posled.Count; i++)
                            raschet_Tokov_Opt(T, posled[i]);
                        maxU.koef_k_Unom = (float)Math.Round(maxU.nachalo ? (optim_raschet[T][maxU.Index_vet].Un / BaseShema.UZ[optim_raschet[T][maxU.Index_vet].nach_AO].nomN) :
                            (optim_raschet[T][maxU.Index_vet].Uk / BaseShema.UZ[optim_raschet[T][maxU.Index_vet].kon_AO].nomN),5);
                        minU.koef_k_Unom = (float)Math.Round(minU.nachalo ? (optim_raschet[T][minU.Index_vet].Un / BaseShema.UZ[optim_raschet[T][minU.Index_vet].nach_AO].nomN) :
                            (optim_raschet[T][minU.Index_vet].Uk / BaseShema.UZ[optim_raschet[T][minU.Index_vet].kon_AO].nomN),5);
                        if ((maxU.koef_k_Unom > kMax) && (minU.koef_k_Unom < kMin))
                        {
                            Log += "Оптимизация напряжения в центре питания '" + BaseShema.UZ[CP_list[q].Index].nameCP.ToString() +
                        "' приостановлена на этапе, когда напряжение в узле " + BaseShema.UZ[(maxU.nachalo ? optim_raschet[T][maxU.Index_vet].nach_AO : optim_raschet[T][maxU.Index_vet].kon_AO)].nameCP +
                        " вышло за диапазон допустимых значений" + Environment.NewLine;
                            end = true;
                        }
                        else if (maxU.koef_k_Unom > kMax)
                        {
                            end1 = false;
                            shag = 0;
                            while (!end1)
                            {
                                for (int i = 0; i < posled.Count; i++)
                                    if (optim_raschet[T][posled[i]].napravlenie)
                                        if (optim_raschet[T][posled[i]].nach_AO == CP_list[q].Index)
                                        {
                                            if (shag == 0)
                                                shag =(optim_raschet[T][posled[i]].Un / maxU.koef_k_Unom) * (float)Math.Round((maxU.koef_k_Unom - kMax),15) / interval ;// за интервал примем одну пятую от занижения диапазона
                                            optim_raschet[T][posled[i]].Un = optim_raschet[T][posled[i]].Un - shag;
                                        }
                                        else
                                            break;
                                    else

                                        if (optim_raschet[T][posled[i]].kon_AO == CP_list[q].Index)
                                        {
                                            if (shag == 0)
                                                shag =(optim_raschet[T][posled[i]].Uk / maxU.koef_k_Unom) * (float)Math.Round((maxU.koef_k_Unom - kMax),15) / interval ;
                                            optim_raschet[T][posled[i]].Uk = optim_raschet[T][posled[i]].Uk - shag;
                                        }
                                        else
                                            break;
                                // обратный ход (расчет напряжений)
                                for (int i = 0; i < posled.Count; i++)
                                    Napr_vetvi_raschet_pr_Opt(T, posled[i]);
                                Add_regim_info_Opt(T, true, numrl);
                                for (int i = (posled.Count - 1); i >= 0; i--)
                                    Potok_vetvi_raschet_obr_Opt(T, posled[i], true);
                                for (int i = 0; i < posled.Count; i++)
                                    raschet_Tokov_Opt(T, posled[i]);
                                maxU.koef_k_Unom = (float)Math.Round(maxU.nachalo ? (optim_raschet[T][maxU.Index_vet].Un / BaseShema.UZ[optim_raschet[T][maxU.Index_vet].nach_AO].nomN) :
                                    (optim_raschet[T][maxU.Index_vet].Uk / BaseShema.UZ[optim_raschet[T][maxU.Index_vet].kon_AO].nomN),5);
                                minU.koef_k_Unom = (float)Math.Round(minU.nachalo ? (optim_raschet[T][minU.Index_vet].Un / BaseShema.UZ[optim_raschet[T][minU.Index_vet].nach_AO].nomN) :
                                    (optim_raschet[T][minU.Index_vet].Uk / BaseShema.UZ[optim_raschet[T][minU.Index_vet].kon_AO].nomN),5);
                                if ((maxU.koef_k_Unom > kMax) && (minU.koef_k_Unom < kMin))
                                {
                                    Log += "Оптимизация напряжения в центре питания '" + BaseShema.UZ[CP_list[q].Index].nameCP.ToString() +
                                "' приостановлена на этапе, когда напряжения в узлах " + BaseShema.UZ[(maxU.nachalo ? optim_raschet[T][maxU.Index_vet].nach_AO : optim_raschet[T][maxU.Index_vet].kon_AO)].nameCP + ", " +
                                BaseShema.UZ[(minU.nachalo ? optim_raschet[T][minU.Index_vet].nach_AO : optim_raschet[T][minU.Index_vet].kon_AO)].nameCP +
                                " вышли за диапазон допустимых значений" + Environment.NewLine;
                                    end1 = true;
                                }
                                else if (maxU.koef_k_Unom < kMax)
                                    end1 = true;
                            }
                            end = true;
                        }
                        else if(maxU.koef_k_Unom ==kMax)
                            end = true;
                    }
                }
                if(posled.Count!=0)
                    CP_list[q].U[T] =(float)Math.Round(optim_raschet[T][posled[0]].napravlenie ? optim_raschet[T][posled[0]].Un : optim_raschet[T][posled[0]].Uk,3);
                if(CP_list[q].U[T]!=BaseShema.UZ[CP_list[q].Index].graficU[T].Yset)
                    Log += "Напряжение в центре питания '" + BaseShema.UZ[CP_list[q].Index].nameCP.ToString()+"' = " + BaseShema.UZ[CP_list[q].Index].graficU[T].Yset.ToString()+"кВ --> " + Math.Round(CP_list[q].U[T], 3).ToString() + "кВ" + Environment.NewLine;
            }
        }
        private bool poisk_int_v_spiske(int i, List<int> list)
        {
            for (int z = 0; z < list.Count; z++)
                if (list[z] == i)
                {
                    return true;
                }
            return false;
        }        
        private void Regim_raschet(int T, int z, int n_KA)
        {
            if (regim.Count == 0)
                create_change_regim(true, T);
            else
                create_change_regim(false, T);
            for (int i = 0; i < spisok_KA_value[z][n_KA].Count; i++)
                regim[i].ka = spisok_KA_value[z][n_KA][i];
            Opredelit_Poradok_seti();            
            Add_regim_info(T, false);
            for (int i = (poradok_rascheta.Count - 1); i >= 0; i--)
            {
                Potok_vetvi_raschet_obr(poradok_rascheta[i], false);
            }
            for (int i = 0; i < poradok_rascheta.Count; i++)
            {                
                Napr_vetvi_raschet_pr(poradok_rascheta[i]);
            }
            Add_regim_info(T, true);
            for (int i = (poradok_rascheta.Count - 1); i >= 0; i--)
            {               
                Potok_vetvi_raschet_obr(poradok_rascheta[i], true);
            }
            for (int i = 0; i < regim.Count; i++)
            {
                raschet_Tokov(i);
            }
            //in_testR();
            
        }
        private void create_change_regim(bool create, int T)
        {
            if (create)
                for (int i = 0; i < raschet.lRaschet[T].Count; i++)
                {
                    regim.Add(new L_T_Vetvi(-1, raschet.lRaschet[T][i].l_tr, raschet.lRaschet[T][i].nach_AO, raschet.lRaschet[T][i].kon_AO,
                       raschet.lRaschet[T][i].ka, raschet.lRaschet[T][i].ka, raschet.lRaschet[T][i].Unom, raschet.lRaschet[T][i].R,
                       raschet.lRaschet[T][i].X, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                       raschet.lRaschet[T][i].dPxx != 0 ? (raschet.lRaschet[T][i].napravlenie ? (raschet.lRaschet[T][i].dPxx * (float)Math.Pow((raschet.lRaschet[T][i].Unom / raschet.lRaschet[T][i].Un), 2)) : (raschet.lRaschet[T][i].dPxx * (float)Math.Pow((raschet.lRaschet[T][i].Unom / raschet.lRaschet[T][i].Uk), 2))) : 0,
                       raschet.lRaschet[T][i].dQxx != 0 ? (raschet.lRaschet[T][i].napravlenie ? (raschet.lRaschet[T][i].dQxx * (float)Math.Pow((raschet.lRaschet[T][i].Unom / raschet.lRaschet[T][i].Un), 2)) : (raschet.lRaschet[T][i].dQxx * (float)Math.Pow((raschet.lRaschet[T][i].Unom / raschet.lRaschet[T][i].Uk), 2))):0,
                       raschet.lRaschet[T][i].ktr, 0, raschet.lRaschet[T][i].Idop,
                       true, -1, new List<int>(), new List<int>()));
                    //Console.WriteLine(regim[i].dPxx.ToString());
                }
            else
            {
                for (int i = 0; i < regim.Count; i++)
                {
                    regim[i].numRL = -1;
                    if (regim[i].dPxx != 0)
                        if (regim[i].napravlenie)
                            regim[i].dPxx = regim[i].dPxx * (float)Math.Pow((regim[i].Unom / regim[i].Un), 2);
                        else
                            regim[i].dPxx = regim[i].dPxx * (float)Math.Pow((regim[i].Unom / regim[i].Uk), 2);
                    else
                        regim[i].dPxx = 0;
                    if (regim[i].dQxx != 0)
                        if (regim[i].napravlenie)
                            regim[i].dQxx = regim[i].dQxx * (float)Math.Pow((regim[i].Unom / regim[i].Un), 2);
                        else
                            regim[i].dQxx = regim[i].dQxx * (float)Math.Pow((regim[i].Unom / regim[i].Uk), 2);
                    else
                        regim[i].dQxx = 0;
                    regim[i].Pn = 0; regim[i].dP = 0; regim[i].Pk = 0;
                    regim[i].Qn = 0; regim[i].dQ = 0; regim[i].Qk = 0;
                    regim[i].Un = 0; regim[i].dU = 0; regim[i].Uk = 0;
                    regim[i].pred = -1; regim[i].posle = new List<int>(); regim[i].posle_razom = new List<int>();
                    //Console.WriteLine(regim[i].dPxx.ToString());
                }
            }
        }
        private void Opredelit_Poradok_seti()
        {
            poradok_rascheta = new List<int>();
            List<int> ne_provereno = new List<int>();
            int nov;
            int n = -1;
            List<int> pred = new List<int>();
            for (int i = 0; i < regim.Count; i++)
            {
                if (regim[i].ka != 0)
                {
                    if (BaseShema.UZ[regim[i].nach_AO].tipUz == 0)
                    {
                        n++;
                        regim[i].napravlenie = true;
                        ne_provereno.Add(regim[i].kon_AO);
                        pred.Add(i);
                        regim[i].numRL = n;
                        poradok_rascheta.Add(i);
                    }
                    else if (BaseShema.UZ[regim[i].kon_AO].tipUz == 0)
                    {
                        n++;
                        regim[i].napravlenie = false;
                        ne_provereno.Add(regim[i].nach_AO);
                        pred.Add(i);
                        regim[i].numRL = n;
                        poradok_rascheta.Add(i);
                    }
                    while (ne_provereno.Count > 0)
                    {
                        nov = ne_provereno[0];
                        for (int j = 0; j < regim.Count; j++)
                        {
                            if (pred[0] != j)
                                if (regim[j].nach_AO == nov)
                                {
                                    if (regim[j].ka != 0)
                                    {
                                        poradok_rascheta.Add(j);//+
                                        regim[pred[0]].posle.Add(j);//+
                                        ne_provereno.Add(regim[j].kon_AO); //+
                                        pred.Add(j);//+
                                        regim[j].napravlenie = true;//+
                                        regim[j].pred = pred[0];//+
                                        regim[j].numRL = n;//+
                                    }
                                    else
                                    {
                                        regim[pred[0]].posle.Add(j);//+
                                        regim[j].napravlenie = true;//+
                                        regim[j].posle.Add(pred[0]);//+                                        
                                    }
                                }
                                else if (regim[j].kon_AO == nov)
                                {
                                    if (regim[j].ka != 0)
                                    {
                                        poradok_rascheta.Add(j);//+
                                        regim[pred[0]].posle.Add(j);//+
                                        ne_provereno.Add(regim[j].nach_AO);//+
                                        pred.Add(j);//+
                                        regim[j].napravlenie = false;//+
                                        regim[j].pred = pred[0];//+
                                        regim[j].numRL = n;//+
                                    }
                                    else
                                    {
                                        regim[pred[0]].posle.Add(j);//+
                                        regim[j].napravlenie = true;//+
                                        regim[j].posle.Add(pred[0]);//+                                       
                                    }
                                }
                        }
                        ne_provereno.RemoveAt(0); pred.RemoveAt(0);
                    }
                }
                else
                {
                    for (int j = 0; j < regim.Count; j++)
                    {
                        if (i != j && regim[j].ka == 0)
                        {
                            if (regim[i].nach_AO == regim[j].nach_AO || regim[i].kon_AO == regim[j].nach_AO ||
                                regim[i].nach_AO == regim[j].kon_AO || regim[i].kon_AO == regim[j].kon_AO)
                                regim[i].posle_razom.Add(j);
                        }
                    }
                }

            }
        }
        private void Opredelit_Poradok_seti_Opt(int T,bool bez_svazi)
        {
            poradok_rascheta = new List<int>();
              List<int> ne_provereno = new List<int>();
              int nov;
              List<int> pred = new List<int>();
            if (bez_svazi)
            {
                for (int i = 0; i < optim_raschet[T].Count; i++)
                {
                    if (optim_raschet[T][i].ka != 0)
                    {
                        if (BaseShema.UZ[optim_raschet[T][i].nach_AO].tipUz == 0)
                        {
                            poradok_rascheta.Add(i);
                            ne_provereno.Add(optim_raschet[T][i].kon_AO);
                            pred.Add(i);
                        }
                        else if (BaseShema.UZ[optim_raschet[T][i].kon_AO].tipUz == 0)
                        {
                            poradok_rascheta.Add(i);
                            ne_provereno.Add(optim_raschet[T][i].nach_AO);
                            pred.Add(i);
                        }
                          while (ne_provereno.Count > 0)
                        {
                            nov = ne_provereno[0];
                            for (int j = 0; j < optim_raschet[T].Count; j++)
                            {
                                if (pred[0] != j)
                                    if (optim_raschet[T][j].nach_AO == nov)
                                    {
                                        if (optim_raschet[T][j].ka != 0)
                                        {
                                            poradok_rascheta.Add(j);//+
                                            ne_provereno.Add(optim_raschet[T][j].kon_AO); //+    
                                            pred.Add(j);//+
                                        }
                                 
                                    }
                                    else if (optim_raschet[T][j].kon_AO == nov)
                                    {
                                        if (optim_raschet[T][j].ka != 0)
                                        {
                                            poradok_rascheta.Add(j);//+                                           
                                            ne_provereno.Add(optim_raschet[T][j].nach_AO);//+
                                            pred.Add(j);//+
                                        }                                    
                                    }
                            }
                            ne_provereno.RemoveAt(0); pred.RemoveAt(0);
                        }
                    }
                }
            }
            else
            { 
                int n = -1;
                for (int i = 0; i < optim_raschet[T].Count; i++)
                {
                    if (optim_raschet[T][i].ka != 0)
                    {
                        if (BaseShema.UZ[optim_raschet[T][i].nach_AO].tipUz == 0)
                        {
                            n++;
                            optim_raschet[T][i].napravlenie = true;
                            ne_provereno.Add(optim_raschet[T][i].kon_AO);
                            pred.Add(i);
                            optim_raschet[T][i].numRL = n;
                            poradok_rascheta.Add(i);
                        }
                        else if (BaseShema.UZ[optim_raschet[T][i].kon_AO].tipUz == 0)
                        {
                            n++;
                            optim_raschet[T][i].napravlenie = false;
                            ne_provereno.Add(optim_raschet[T][i].nach_AO);
                            pred.Add(i);
                            optim_raschet[T][i].numRL = n;
                            poradok_rascheta.Add(i);
                        }
                        while (ne_provereno.Count > 0)
                        {
                            nov = ne_provereno[0];
                            for (int j = 0; j < optim_raschet[T].Count; j++)
                            {
                                if (pred[0] != j)
                                    if (optim_raschet[T][j].nach_AO == nov)
                                    {
                                        if (optim_raschet[T][j].ka != 0)
                                        {
                                            poradok_rascheta.Add(j);//+
                                            optim_raschet[T][pred[0]].posle.Add(j);//+
                                            ne_provereno.Add(optim_raschet[T][j].kon_AO); //+
                                            pred.Add(j);//+
                                            optim_raschet[T][j].napravlenie = true;//+
                                            optim_raschet[T][j].pred = pred[0];//+
                                            optim_raschet[T][j].numRL = n;//+
                                        }
                                        else
                                        {
                                            optim_raschet[T][pred[0]].posle.Add(j);//+
                                            optim_raschet[T][j].napravlenie = true;//+
                                            optim_raschet[T][j].posle.Add(pred[0]);//+                                        
                                        }
                                    }
                                    else if (optim_raschet[T][j].kon_AO == nov)
                                    {
                                        if (optim_raschet[T][j].ka != 0)
                                        {
                                            poradok_rascheta.Add(j);//+
                                            optim_raschet[T][pred[0]].posle.Add(j);//+
                                            ne_provereno.Add(optim_raschet[T][j].nach_AO);//+
                                            pred.Add(j);//+
                                            optim_raschet[T][j].napravlenie = false;//+
                                            optim_raschet[T][j].pred = pred[0];//+
                                            optim_raschet[T][j].numRL = n;//+
                                        }
                                        else
                                        {
                                            optim_raschet[T][pred[0]].posle.Add(j);//+
                                            optim_raschet[T][j].napravlenie = true;//+
                                            optim_raschet[T][j].posle.Add(pred[0]);//+                                       
                                        }
                                    }
                            }
                            ne_provereno.RemoveAt(0); pred.RemoveAt(0);
                        }
                    }
                    else
                    {
                        for (int j = 0; j < optim_raschet[T].Count; j++)
                        {
                            if (i != j && optim_raschet[T][j].ka == 0)
                            {
                                if (optim_raschet[T][i].nach_AO == optim_raschet[T][j].nach_AO || optim_raschet[T][i].kon_AO == optim_raschet[T][j].nach_AO ||
                                    optim_raschet[T][i].nach_AO == optim_raschet[T][j].kon_AO || optim_raschet[T][i].kon_AO == optim_raschet[T][j].kon_AO)
                                    optim_raschet[T][i].posle_razom.Add(j);
                            }
                        }
                    }

                }
            }
        }
        private void Add_regim_info(int T, bool clear)
        {
            List<int> posled = new List<int>();
            if (clear)
            {
                for (int i = 0; i < regim.Count; i++)
                    posled.Add(i);
            }
            for (int i = 0; i < BaseShema.UZ.Count; i++)                                  // заполнение режимной информации
                switch (BaseShema.UZ[i].tipUz)
                {
                    case 0:
                        if (!clear)
                            for (int j = 0; j < regim.Count; j++)
                                if ((regim[j].napravlenie) && (regim[j].ka != 0))
                                {
                                    if (i == regim[j].nach_AO)
                                    { regim[j].Un = BaseShema.UZ[i].graficU[T].Yset; }
                                }
                                else if (regim[j].ka != 0)
                                {
                                    if (i == regim[j].kon_AO)
                                    { regim[j].Uk = BaseShema.UZ[i].graficU[T].Yset; }
                                }
                        break;
                    case 1:
                        for (int j = 0; j < regim.Count; j++)
                            if ((regim[j].napravlenie) && (regim[j].ka != 0))
                            {
                                if (i == regim[j].kon_AO)
                                {
                                    regim[j].Pk = -BaseShema.UZ[i].graficP[T].Yset;
                                    regim[j].Qk = -BaseShema.UZ[i].graficQ[T].Yset;
                                    if (clear)
                                    { regim[j].dP = regim[j].Pn = regim[j].dQ = regim[j].Qn = 0; posled.Remove(j); }
                                    break;
                                }
                            }
                            else if (regim[j].ka != 0)
                            {
                                if (i == regim[j].nach_AO)
                                {
                                    regim[j].Pn = -BaseShema.UZ[i].graficP[T].Yset;
                                    regim[j].Qn = -BaseShema.UZ[i].graficQ[T].Yset;
                                    if (clear)
                                    { regim[j].dP = regim[j].Pk = regim[j].dQ = regim[j].Qk = 0; posled.Remove(j); }
                                    break;
                                }
                            }
                        break;
                    case 2:
                        for (int j = 0; j < regim.Count; j++)
                            if ((regim[j].napravlenie) && (regim[j].ka != 0))
                            {
                                if (i == regim[j].kon_AO)
                                {
                                    regim[j].Pk = BaseShema.UZ[i].graficP[T].Yset;
                                    regim[j].Qk = BaseShema.UZ[i].graficQ[T].Yset;
                                    if (clear)
                                    { regim[j].dP = regim[j].Pn = regim[j].dQ = regim[j].Qn = 0; posled.Remove(j); }
                                    break;
                                }
                            }
                            else if (regim[j].ka != 0)
                            {
                                if (i == regim[j].nach_AO)
                                {
                                    regim[j].Pn = BaseShema.UZ[i].graficP[T].Yset;
                                    regim[j].Qn = BaseShema.UZ[i].graficQ[T].Yset;
                                    if (clear)
                                    { regim[j].dP = regim[j].Pk = regim[j].dQ = regim[j].Qk = 0; posled.Remove(j); }
                                    break;
                                }
                            }
                        break;
                }
            if (clear)
                for (int i = 0; i < posled.Count; i++)
                    regim[posled[i]].Pn = regim[posled[i]].dP = regim[posled[i]].Pk = regim[posled[i]].Qn = regim[posled[i]].dQ = regim[posled[i]].Qk = 0;
        }
        private void Add_regim_info_Opt(int T, bool clear,List<int> numRL)
        {
            List<int> posled = new List<int>();
            if (clear)
            {
                for (int i = 0; i < optim_raschet[T].Count; i++)
                    for(int j=0;j<numRL.Count;j++)
                        if(optim_raschet[T][i].numRL==numRL[j])
                            posled.Add(i);
            }
            for (int i = 0; i < BaseShema.UZ.Count; i++)                                  // заполнение режимной информации
                switch (BaseShema.UZ[i].tipUz)
                {
                    case 0:
                        if (!clear)
                            for (int j = 0; j < optim_raschet[T].Count; j++)
                                for (int q = 0; q < numRL.Count; q++)
                                    if (optim_raschet[T][j].numRL == numRL[q])
                                    {
                                        if ((optim_raschet[T][j].napravlenie) && (optim_raschet[T][j].ka != 0))
                                        {
                                            if (i == optim_raschet[T][j].nach_AO)
                                            { optim_raschet[T][j].Un = BaseShema.UZ[i].graficU[T].Yset; }
                                        }
                                        else if (optim_raschet[T][j].ka != 0)
                                        {
                                            if (i == optim_raschet[T][j].kon_AO)
                                            { optim_raschet[T][j].Uk = BaseShema.UZ[i].graficU[T].Yset; }
                                        }
                                    }
                        break;
                    case 1:
                        for (int j = 0; j < optim_raschet[T].Count; j++)
                             for (int q = 0; q < numRL.Count; q++)
                                 if (optim_raschet[T][j].numRL == numRL[q])
                                 {
                                     if ((optim_raschet[T][j].napravlenie) && (optim_raschet[T][j].ka != 0))
                                     {
                                         if (i == optim_raschet[T][j].kon_AO)
                                         {
                                             optim_raschet[T][j].Pk = -BaseShema.UZ[i].graficP[T].Yset;
                                             optim_raschet[T][j].Qk = -BaseShema.UZ[i].graficQ[T].Yset;
                                             if (clear)
                                             { optim_raschet[T][j].dP = optim_raschet[T][j].Pn = optim_raschet[T][j].dQ = optim_raschet[T][j].Qn = 0; posled.Remove(j); }
                                             break;
                                         }
                                     }
                                     else if (optim_raschet[T][j].ka != 0)
                                     {
                                         if (i == optim_raschet[T][j].nach_AO)
                                         {
                                             optim_raschet[T][j].Pn = -BaseShema.UZ[i].graficP[T].Yset;
                                             optim_raschet[T][j].Qn = -BaseShema.UZ[i].graficQ[T].Yset;
                                             if (clear)
                                             { optim_raschet[T][j].dP = optim_raschet[T][j].Pk = optim_raschet[T][j].dQ = optim_raschet[T][j].Qk = 0; posled.Remove(j); }
                                             break;
                                         }
                                     }
                                 }
                        break;
                    case 2:
                        for (int j = 0; j < optim_raschet[T].Count; j++)
                             for (int q = 0; q < numRL.Count; q++)
                                 if (optim_raschet[T][j].numRL == numRL[q])
                                 {
                                     if ((optim_raschet[T][j].napravlenie) && (optim_raschet[T][j].ka != 0))
                                     {
                                         if (i == optim_raschet[T][j].kon_AO)
                                         {
                                             optim_raschet[T][j].Pk = BaseShema.UZ[i].graficP[T].Yset;
                                             optim_raschet[T][j].Qk = BaseShema.UZ[i].graficQ[T].Yset;
                                             if (clear)
                                             { optim_raschet[T][j].dP = optim_raschet[T][j].Pn = optim_raschet[T][j].dQ = optim_raschet[T][j].Qn = 0; posled.Remove(j); }
                                             break;
                                         }
                                     }
                                     else if (optim_raschet[T][j].ka != 0)
                                     {
                                         if (i == optim_raschet[T][j].nach_AO)
                                         {
                                             optim_raschet[T][j].Pn = BaseShema.UZ[i].graficP[T].Yset;
                                             optim_raschet[T][j].Qn = BaseShema.UZ[i].graficQ[T].Yset;
                                             if (clear)
                                             { optim_raschet[T][j].dP = optim_raschet[T][j].Pk = optim_raschet[T][j].dQ = optim_raschet[T][j].Qk = 0; posled.Remove(j); }
                                             break;
                                         }
                                     }
                                 }
                        break;
                }
            if (clear)
                for (int i = 0; i < posled.Count; i++)
                    optim_raschet[T][posled[i]].Pn = optim_raschet[T][posled[i]].dP = optim_raschet[T][posled[i]].Pk = optim_raschet[T][posled[i]].Qn = optim_raschet[T][posled[i]].dQ = optim_raschet[T][posled[i]].Qk = 0;
        }
        private void Add_regim_info_Opt(int T, bool clear, bool first, List <int> spis)
        {
            List<int> posled = new List<int>();
            bool prov = false;
            if (clear)
            {
                for (int i = 0; i < optim_raschet[T].Count; i++)
                {
                    prov = false;
                    posled.Add(i);
                    if (first)
                    {
                        for (int j = 0; j < spis.Count; j++)
                            if (i == spis[j])
                            {
                                prov = true;
                                break;
                            }
                        if (!prov)
                        {
                            if ((optim_raschet[T][i].napravlenie) && (optim_raschet[T][i].ka != 0))
                            {
                                optim_raschet[T][i].dPxx = optim_raschet[T][i].dPxx / (float)Math.Pow((optim_raschet[T][i].Un / optim_raschet[T][i].Unom), 2);
                                optim_raschet[T][i].dQxx = optim_raschet[T][i].dQxx / (float)Math.Pow((optim_raschet[T][i].Un / optim_raschet[T][i].Unom), 2);
                            }
                            else if (optim_raschet[T][i].ka != 0)
                            {
                                optim_raschet[T][i].dPxx = optim_raschet[T][i].dPxx / (float)Math.Pow((optim_raschet[T][i].Uk / optim_raschet[T][i].Unom), 2);
                                optim_raschet[T][i].dQxx = optim_raschet[T][i].dQxx / (float)Math.Pow((optim_raschet[T][i].Uk / optim_raschet[T][i].Unom), 2);
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < BaseShema.UZ.Count; i++)                                  // заполнение режимной информации
                switch (BaseShema.UZ[i].tipUz)
                {
                    case 0:
                        if (!clear)
                            for (int j = 0; j < optim_raschet[T].Count; j++)
                                        if ((optim_raschet[T][j].napravlenie) && (optim_raschet[T][j].ka != 0))
                                        {
                                            if (i == optim_raschet[T][j].nach_AO)
                                            { optim_raschet[T][j].Un = BaseShema.UZ[i].graficU[T].Yset; }
                                        }
                                        else if (optim_raschet[T][j].ka != 0)
                                        {
                                            if (i == optim_raschet[T][j].kon_AO)
                                            { optim_raschet[T][j].Uk = BaseShema.UZ[i].graficU[T].Yset; }
                                        }

                        break;
                    case 1:
                        for (int j = 0; j < optim_raschet[T].Count; j++)
                                    if ((optim_raschet[T][j].napravlenie) && (optim_raschet[T][j].ka != 0))
                                    {
                                        if (i == optim_raschet[T][j].kon_AO)
                                        {
                                            optim_raschet[T][j].Pk = -BaseShema.UZ[i].graficP[T].Yset;
                                            optim_raschet[T][j].Qk = -BaseShema.UZ[i].graficQ[T].Yset;
                                            if (clear)
                                            {
                                                optim_raschet[T][j].dP = optim_raschet[T][j].Pn = optim_raschet[T][j].dQ = optim_raschet[T][j].Qn = 0;                                                
                                                posled.Remove(j);
                                            }
                                            break;
                                        }
                                    }
                                    else if (optim_raschet[T][j].ka != 0)
                                    {
                                        if (i == optim_raschet[T][j].nach_AO)
                                        {
                                            optim_raschet[T][j].Pn = -BaseShema.UZ[i].graficP[T].Yset;
                                            optim_raschet[T][j].Qn = -BaseShema.UZ[i].graficQ[T].Yset;
                                            if (clear)
                                            {
                                                optim_raschet[T][j].dP = optim_raschet[T][j].Pk = optim_raschet[T][j].dQ = optim_raschet[T][j].Qk = 0;
                                                posled.Remove(j);
                                            }
                                            break;
                                        }
                                    }
                        break;
                    case 2:
                        for (int j = 0; j < optim_raschet[T].Count; j++)
                           
                                    if ((optim_raschet[T][j].napravlenie) && (optim_raschet[T][j].ka != 0))
                                    {
                                        if (i == optim_raschet[T][j].kon_AO)
                                        {
                                            optim_raschet[T][j].Pk = BaseShema.UZ[i].graficP[T].Yset;
                                            optim_raschet[T][j].Qk = BaseShema.UZ[i].graficQ[T].Yset;
                                            if (clear)
                                            {
                                                optim_raschet[T][j].dP = optim_raschet[T][j].Pn = optim_raschet[T][j].dQ = optim_raschet[T][j].Qn = 0;                                                
                                                posled.Remove(j);
                                            }
                                            break;
                                        }
                                    }
                                    else if (optim_raschet[T][j].ka != 0)
                                    {
                                        if (i == optim_raschet[T][j].nach_AO)
                                        {
                                            optim_raschet[T][j].Pn = BaseShema.UZ[i].graficP[T].Yset;
                                            optim_raschet[T][j].Qn = BaseShema.UZ[i].graficQ[T].Yset;
                                            if (clear)
                                            {
                                                optim_raschet[T][j].dP = optim_raschet[T][j].Pk = optim_raschet[T][j].dQ = optim_raschet[T][j].Qk = 0;                                                
                                                posled.Remove(j);
                                            }
                                            break;
                                        }
                                    }
                        break;
                }
            if (clear)
                for (int i = 0; i < posled.Count; i++)
                {
                    optim_raschet[T][posled[i]].Pn = optim_raschet[T][posled[i]].dP = optim_raschet[T][posled[i]].Pk = optim_raschet[T][posled[i]].Qn = optim_raschet[T][posled[i]].dQ = optim_raschet[T][posled[i]].Qk = 0;
                }
        }
        private void Potok_vetvi_raschet_obr(int n, bool napr_d)
        {
            if (regim[n].napravlenie)
            {
                if (napr_d)
                {
                    regim[n].dP = ((float)Math.Pow(regim[n].Pk, 2) + (float)Math.Pow(regim[n].Qk, 2)) / (float)Math.Pow(regim[n].Uk / regim[n].ktr, 2) * regim[n].R / 1000;
                    regim[n].dQ = ((float)Math.Pow(regim[n].Pk, 2) + (float)Math.Pow(regim[n].Qk, 2)) / (float)Math.Pow(regim[n].Uk / regim[n].ktr, 2) * regim[n].X / 1000;
                    regim[n].dPxx = regim[n].dPxx * (float)Math.Pow((regim[n].Un / regim[n].Unom), 2);
                    regim[n].dQxx = regim[n].dQxx * (float)Math.Pow((regim[n].Un / regim[n].Unom), 2);
                }
                else
                {
                    regim[n].dP = ((float)Math.Pow(regim[n].Pk, 2) + (float)Math.Pow(regim[n].Qk, 2)) / (float)Math.Pow(regim[n].Unom, 2) * regim[n].R / 1000;
                    regim[n].dQ = ((float)Math.Pow(regim[n].Pk, 2) + (float)Math.Pow(regim[n].Qk, 2)) / (float)Math.Pow(regim[n].Unom, 2) * regim[n].X / 1000;
                }
                regim[n].Pn = regim[n].Pk + regim[n].dP + regim[n].dPxx;
                regim[n].Qn = regim[n].Qk + regim[n].dQ + regim[n].dQxx;

                if (regim[n].pred != -1)
                    if (regim[regim[n].pred].napravlenie)
                    {
                        if (regim[n].nach_AO == regim[regim[n].pred].kon_AO)
                        {
                            regim[regim[n].pred].Pk += regim[n].Pn;
                            regim[regim[n].pred].Qk += regim[n].Qn;
                        }
                    }
                    else
                    {
                        if (regim[n].nach_AO == regim[regim[n].pred].nach_AO)
                        {
                            regim[regim[n].pred].Pn += regim[n].Pn;
                            regim[regim[n].pred].Qn += regim[n].Qn;
                        }
                    }
            }
            else
            {
                if (napr_d)
                {
                    regim[n].dP = ((float)Math.Pow(regim[n].Pn, 2) + (float)Math.Pow(regim[n].Qn, 2)) / (float)Math.Pow(regim[n].Un * regim[n].ktr, 2) * regim[n].R / 1000;
                    regim[n].dQ = ((float)Math.Pow(regim[n].Pn, 2) + (float)Math.Pow(regim[n].Qn, 2)) / (float)Math.Pow(regim[n].Un * regim[n].ktr, 2) * regim[n].X / 1000;
                    regim[n].dPxx = regim[n].dPxx * (float)Math.Pow((regim[n].Uk / regim[n].Unom), 2);
                    regim[n].dQxx = regim[n].dQxx * (float)Math.Pow((regim[n].Uk / regim[n].Unom), 2);
                }
                else
                {
                    regim[n].dP = ((float)Math.Pow(regim[n].Pn, 2) + (float)Math.Pow(regim[n].Qn, 2)) / (float)Math.Pow(regim[n].Unom, 2) * regim[n].R / 1000;
                    regim[n].dQ = ((float)Math.Pow(regim[n].Pn, 2) + (float)Math.Pow(regim[n].Qn, 2)) / (float)Math.Pow(regim[n].Unom, 2) * regim[n].X / 1000;
                }
                regim[n].Pk = regim[n].Pn + regim[n].dP + regim[n].dPxx;
                regim[n].Qk = regim[n].Qn + regim[n].dQ + regim[n].dQxx;
                if (regim[n].pred != -1)
                    if (regim[regim[n].pred].napravlenie)
                    {
                        if (regim[n].kon_AO == regim[regim[n].pred].kon_AO)
                        {
                            regim[regim[n].pred].Pk += regim[n].Pk;
                            regim[regim[n].pred].Qk += regim[n].Qk;
                        }
                    }
                    else
                    {
                        if (regim[n].kon_AO == regim[regim[n].pred].nach_AO)
                        {
                            regim[regim[n].pred].Pn += regim[n].Pk;
                            regim[regim[n].pred].Qn += regim[n].Qk;
                        }
                    }

            }
        }
        private void Potok_vetvi_raschet_obr_Opt(int t,int n, bool napr_d)
        {
            if (optim_raschet[t][n].napravlenie)
            {
                if (napr_d)
                {
                    optim_raschet[t][n].dP = ((float)Math.Pow(optim_raschet[t][n].Pk, 2) + (float)Math.Pow(optim_raschet[t][n].Qk, 2)) / (float)Math.Pow(optim_raschet[t][n].Uk / optim_raschet[t][n].ktr, 2) * optim_raschet[t][n].R / 1000;
                    optim_raschet[t][n].dQ = ((float)Math.Pow(optim_raschet[t][n].Pk, 2) + (float)Math.Pow(optim_raschet[t][n].Qk, 2)) / (float)Math.Pow(optim_raschet[t][n].Uk / optim_raschet[t][n].ktr, 2) * optim_raschet[t][n].X / 1000;
                    optim_raschet[t][n].dPxx = optim_raschet[t][n].dPxx * (float)Math.Pow((optim_raschet[t][n].Un / optim_raschet[t][n].Unom), 2);
                    optim_raschet[t][n].dQxx = optim_raschet[t][n].dQxx * (float)Math.Pow((optim_raschet[t][n].Un / optim_raschet[t][n].Unom), 2);
                }
                else
                {
                    optim_raschet[t][n].dP = ((float)Math.Pow(optim_raschet[t][n].Pk, 2) + (float)Math.Pow(optim_raschet[t][n].Qk, 2)) / (float)Math.Pow(optim_raschet[t][n].Unom, 2) * optim_raschet[t][n].R / 1000;
                    optim_raschet[t][n].dQ = ((float)Math.Pow(optim_raschet[t][n].Pk, 2) + (float)Math.Pow(optim_raschet[t][n].Qk, 2)) / (float)Math.Pow(optim_raschet[t][n].Unom, 2) * optim_raschet[t][n].X / 1000;
                }
                optim_raschet[t][n].Pn = optim_raschet[t][n].Pk + optim_raschet[t][n].dP + optim_raschet[t][n].dPxx;
                optim_raschet[t][n].Qn = optim_raschet[t][n].Qk + optim_raschet[t][n].dQ + optim_raschet[t][n].dQxx;

                if (optim_raschet[t][n].pred != -1)
                    if (optim_raschet[t][optim_raschet[t][n].pred].napravlenie)
                    {
                        if (optim_raschet[t][n].nach_AO == optim_raschet[t][optim_raschet[t][n].pred].kon_AO)
                        {
                            optim_raschet[t][optim_raschet[t][n].pred].Pk += optim_raschet[t][n].Pn;
                            optim_raschet[t][optim_raschet[t][n].pred].Qk += optim_raschet[t][n].Qn;
                        }
                    }
                    else
                    {
                        if (optim_raschet[t][n].nach_AO == optim_raschet[t][optim_raschet[t][n].pred].nach_AO)
                        {
                            optim_raschet[t][optim_raschet[t][n].pred].Pn += optim_raschet[t][n].Pn;
                            optim_raschet[t][optim_raschet[t][n].pred].Qn += optim_raschet[t][n].Qn;
                        }
                    }
            }
            else
            {
                if (napr_d)
                {
                    optim_raschet[t][n].dP = ((float)Math.Pow(optim_raschet[t][n].Pn, 2) + (float)Math.Pow(optim_raschet[t][n].Qn, 2)) / (float)Math.Pow(optim_raschet[t][n].Un * optim_raschet[t][n].ktr, 2) * optim_raschet[t][n].R / 1000;
                    optim_raschet[t][n].dQ = ((float)Math.Pow(optim_raschet[t][n].Pn, 2) + (float)Math.Pow(optim_raschet[t][n].Qn, 2)) / (float)Math.Pow(optim_raschet[t][n].Un * optim_raschet[t][n].ktr, 2) * optim_raschet[t][n].X / 1000;
                    optim_raschet[t][n].dPxx = optim_raschet[t][n].dPxx * (float)Math.Pow((optim_raschet[t][n].Uk / optim_raschet[t][n].Unom), 2);
                    optim_raschet[t][n].dQxx = optim_raschet[t][n].dQxx * (float)Math.Pow((optim_raschet[t][n].Uk / optim_raschet[t][n].Unom), 2);
                }
                else
                {
                    optim_raschet[t][n].dP = ((float)Math.Pow(optim_raschet[t][n].Pn, 2) + (float)Math.Pow(optim_raschet[t][n].Qn, 2)) / (float)Math.Pow(optim_raschet[t][n].Unom, 2) * optim_raschet[t][n].R / 1000;
                    optim_raschet[t][n].dQ = ((float)Math.Pow(optim_raschet[t][n].Pn, 2) + (float)Math.Pow(optim_raschet[t][n].Qn, 2)) / (float)Math.Pow(optim_raschet[t][n].Unom, 2) * optim_raschet[t][n].X / 1000;
                }
                optim_raschet[t][n].Pk = optim_raschet[t][n].Pn + optim_raschet[t][n].dP + optim_raschet[t][n].dPxx;
                optim_raschet[t][n].Qk = optim_raschet[t][n].Qn + optim_raschet[t][n].dQ + optim_raschet[t][n].dQxx;
                if (optim_raschet[t][n].pred != -1)
                    if (optim_raschet[t][optim_raschet[t][n].pred].napravlenie)
                    {
                        if (optim_raschet[t][n].kon_AO == optim_raschet[t][optim_raschet[t][n].pred].kon_AO)
                        {
                            optim_raschet[t][optim_raschet[t][n].pred].Pk += optim_raschet[t][n].Pk;
                            optim_raschet[t][optim_raschet[t][n].pred].Qk += optim_raschet[t][n].Qk;
                        }
                    }
                    else
                    {
                        if (optim_raschet[t][n].kon_AO == optim_raschet[t][optim_raschet[t][n].pred].nach_AO)
                        {
                            optim_raschet[t][optim_raschet[t][n].pred].Pn += optim_raschet[t][n].Pk;
                            optim_raschet[t][optim_raschet[t][n].pred].Qn += optim_raschet[t][n].Qk;
                        }
                    }

            }
        }
        private void Napr_vetvi_raschet_pr(int n)
        {
            if (regim[n].napravlenie)
            {
                regim[n].dU = ((regim[n].Pn - regim[n].dPxx) * regim[n].R + (regim[n].Qn - regim[n].dQxx) * regim[n].X) / regim[n].Un / 1000;
                regim[n].Uk = (regim[n].Un - regim[n].dU) * regim[n].ktr;
                for (int i = 0; i < regim[n].posle.Count; i++)
                    if (regim[regim[n].posle[i]].ka != 0)
                    {
                        if (regim[regim[n].posle[i]].napravlenie)
                        {
                            if (regim[regim[n].posle[i]].nach_AO == regim[n].kon_AO)
                            {
                                regim[regim[n].posle[i]].Un = regim[n].Uk; //regim[regim[n].posle[i]].numRL = regim[n].numRL;
                            }
                        }
                        else
                        {
                            if (regim[regim[n].posle[i]].kon_AO == regim[n].kon_AO)
                            {
                                regim[regim[n].posle[i]].Uk = regim[n].Uk; //regim[regim[n].posle[i]].numRL = regim[n].numRL;
                            }
                        }
                    }
            }
            else
            {
                regim[n].dU = ((regim[n].Pk - regim[n].dPxx) * regim[n].R + (regim[n].Qk - regim[n].dQxx) * regim[n].X) / regim[n].Uk / 1000;
                regim[n].Un = (regim[n].Uk - regim[n].dU) / regim[n].ktr;
                for (int i = 0; i < regim[n].posle.Count; i++)
                    if (regim[regim[n].posle[i]].ka != 0)
                    {
                        if (regim[regim[n].posle[i]].napravlenie)
                        {
                            if (regim[regim[n].posle[i]].nach_AO == regim[n].nach_AO)
                            {
                                regim[regim[n].posle[i]].Un = regim[n].Un;// regim[regim[n].posle[i]].numRL = regim[n].numRL;
                            }
                        }
                        else
                        {
                            if (regim[regim[n].posle[i]].kon_AO == regim[n].nach_AO)
                            {
                                regim[regim[n].posle[i]].Uk = regim[n].Un; //regim[regim[n].posle[i]].numRL = regim[n].numRL;
                            }
                        }
                    }
            }
        }
        private void Napr_vetvi_raschet_pr_Opt(int t,int n)
        {
            if (optim_raschet[t][n].napravlenie)
            {
                optim_raschet[t][n].dU = ((optim_raschet[t][n].Pn - optim_raschet[t][n].dPxx) * optim_raschet[t][n].R + (optim_raschet[t][n].Qn - optim_raschet[t][n].dQxx) * optim_raschet[t][n].X) / optim_raschet[t][n].Un / 1000;
                optim_raschet[t][n].Uk = (optim_raschet[t][n].Un - optim_raschet[t][n].dU) * optim_raschet[t][n].ktr;

                for (int i = 0; i < optim_raschet[t][n].posle.Count; i++)
                    if (optim_raschet[t][optim_raschet[t][n].posle[i]].ka != 0)
                    {
                        if (optim_raschet[t][optim_raschet[t][n].posle[i]].napravlenie)
                        {
                            if (optim_raschet[t][optim_raschet[t][n].posle[i]].nach_AO == optim_raschet[t][n].kon_AO)
                            {
                                optim_raschet[t][optim_raschet[t][n].posle[i]].Un = optim_raschet[t][n].Uk; //optim_raschet[t][optim_raschet[t][n].posle[i]].numRL = optim_raschet[t][n].numRL;
                            }
                        }
                        else
                        {
                            if (optim_raschet[t][optim_raschet[t][n].posle[i]].kon_AO == optim_raschet[t][n].kon_AO)
                            {
                                optim_raschet[t][optim_raschet[t][n].posle[i]].Uk = optim_raschet[t][n].Uk; //optim_raschet[t][optim_raschet[t][n].posle[i]].numRL = optim_raschet[t][n].numRL;
                            }
                        }
                    }
            }
            else
            {
                optim_raschet[t][n].dU = ((optim_raschet[t][n].Pk - optim_raschet[t][n].dPxx) * optim_raschet[t][n].R + (optim_raschet[t][n].Qk - optim_raschet[t][n].dQxx) * optim_raschet[t][n].X) / optim_raschet[t][n].Uk / 1000;
                optim_raschet[t][n].Un = (optim_raschet[t][n].Uk - optim_raschet[t][n].dU) / optim_raschet[t][n].ktr;
                for (int i = 0; i < optim_raschet[t][n].posle.Count; i++)
                    if (optim_raschet[t][optim_raschet[t][n].posle[i]].ka != 0)
                    {
                        if (optim_raschet[t][optim_raschet[t][n].posle[i]].napravlenie)
                        {
                            if (optim_raschet[t][optim_raschet[t][n].posle[i]].nach_AO == optim_raschet[t][n].nach_AO)
                            {
                                optim_raschet[t][optim_raschet[t][n].posle[i]].Un = optim_raschet[t][n].Un;// optim_raschet[t][optim_raschet[t][n].posle[i]].numRL = optim_raschet[t][n].numRL;
                            }
                        }
                        else
                        {
                            if (optim_raschet[t][optim_raschet[t][n].posle[i]].kon_AO == optim_raschet[t][n].nach_AO)
                            {
                                optim_raschet[t][optim_raschet[t][n].posle[i]].Uk = optim_raschet[t][n].Un; //optim_raschet[t][optim_raschet[t][n].posle[i]].numRL = optim_raschet[t][n].numRL;
                            }
                        }
                    }
            }
        }
        private void raschet_Tokov(int i)
        {
            if (regim[i].ka != 0)
            {
                if (regim[i].napravlenie)
                    regim[i].I = (float)Math.Sqrt((regim[i].Pn * regim[i].Pn + regim[i].Qn * regim[i].Qn) / 3) / regim[i].Un;
                else
                    regim[i].I = (float)Math.Sqrt((regim[i].Pk * regim[i].Pk + regim[i].Qk * regim[i].Qk) / 3) / regim[i].Uk;
            }
            else
            {
                regim[i].I = 0;
            }
        }
        private void raschet_Tokov_Opt(int T,int i)
        {
            if (optim_raschet[T][i].ka != 0)
            {
                if (optim_raschet[T][i].napravlenie)
                    optim_raschet[T][i].I = (float)Math.Sqrt((optim_raschet[T][i].Pn * optim_raschet[T][i].Pn + optim_raschet[T][i].Qn * optim_raschet[T][i].Qn) / 3) / optim_raschet[T][i].Un;
                else
                    optim_raschet[T][i].I = (float)Math.Sqrt((optim_raschet[T][i].Pk * optim_raschet[T][i].Pk + optim_raschet[T][i].Qk * optim_raschet[T][i].Qk) / 3) / optim_raschet[T][i].Uk;
            }
            else
            {
                optim_raschet[T][i].I = 0;
            }
        }
        private void in_test(int T, bool pokaz)
        {
            TestForm t = new TestForm();
            t.Clear();
            for (int i = 0; i < optim_raschet[T].Count; i++)
                t.addvet(optim_raschet[T][i], BaseShema.UZ[optim_raschet[T][i].nach_AO].nameCP, BaseShema.UZ[optim_raschet[T][i].kon_AO].nameCP);
            //Console.WriteLine(t.Rezultat().ToString());
            if (pokaz)
            {
                t.Rezultat();
                t.Show();
                t.Update();
            }
            else
                t.Dispose();
        }
        private void in_testR()
        {
            TestForm t = new TestForm();
            t.Clear();
            for (int i = 0; i < regim.Count; i++)
                t.addvet(regim[i], BaseShema.UZ[regim[i].nach_AO].nameCP, BaseShema.UZ[regim[i].kon_AO].nameCP);
            t.Rezultat();
            t.Show();
            t.Update();
        }
        public void Apply_napr()
        {
            float[] koef = new float[24];
            float min, max, mashtab, usr;
            int n;
            for (int i = 0; i < CP_list.Count; i++)
            {
                n = 0; usr = 0;
                min = CP_list[i].U[0];
                max = CP_list[i].U[0];
                for (int j = 1; j < 24; j++)
                {
                    if (min > CP_list[i].U[j])
                        min = CP_list[i].U[j];
                    if (max < CP_list[i].U[j])
                        max = CP_list[i].U[j];
                }
                mashtab = (max - min) / 2;
                for (int j = 0; j < 24; j++)
                    koef[j] = (CP_list[i].U[j] - min) / mashtab - 1;
                for (int j = 0; j < CP_list[i].U.Count; j++)
                {
                    BaseShema.UZ[CP_list[i].Index].graficU[j].Yset = CP_list[i].U[j];
                    BaseShema.UZ[CP_list[i].Index].graficU[j].Kset = koef[j];
                    n++;
                    usr += CP_list[i].U[j];
                }
                usr = usr / n;
                BaseShema.UZ[CP_list[i].Index].napr = usr;
            }
        }
        public void Apply_prov()
        {
            if (Pr_list.Count>0)
                for (int i = 0; i < Pr_list.Count; i++)                
                    BaseShema.Vet[Pr_list[i].Index_vet].marka = Pr_list[i].mar_new;
        }
        public void Apply_tr()
        {
            if (Tr_list.Count > 0)
                for (int i = 0; i < Tr_list.Count; i++)
                {
                    BaseShema.Vet[Tr_list[i].Index_vet].marka = Tr_list[i].mar_new;
                    BaseShema.Vet[Tr_list[i].Index_vet].dlina_moshnost = Tr_list[i].moshn;
                }
        }
        private float Return_keff_zamen_provoda(float sech)
        {
            if (sech < 16)
                return 0.5f;
            else if ((sech >= 16) && (sech < 25))
                return 0.5f + (sech - 16) * 0.004274f;
            else if ((sech >= 25) && (sech < 35))
                return 0.538461538f + (sech - 25) * 0.003297f;
            else if ((sech >= 35) && (sech < 50))
                return 0.571428571f + (sech - 35) * 0.004762f;
            else if ((sech >= 50) && (sech < 70))
                return 0.642857143f + (sech - 50) * 0.007480f;
            else if ((sech >= 70) && (sech < 95))
                return 0.79245283f + (sech - 70) * 0.000302f;
            else if (sech >= 95)
                return 0.8f;
            else
            {
                System.Windows.Forms.MessageBox.Show("Сечение не может быть отрицательным");
                return 0;
            }

        }
        private float Return_koef_zagr_min_max(bool min)
        {
            if (min)
                return 0.3f;
            else
                return 1;
        }
        public string Retutn_Log()
        {
            return Log;
        }
    }
    public class Cp_opt
    {
        public int Index;
        public List<float> U;        
        public Cp_opt(int i, List<float> U)
        {
            this.Index = i;            
            this.U = U;
        }
    }
    public class Uz_Napr
    {
        public int Index_vet;
        public float koef_k_Unom;
        public bool nachalo;
        public Uz_Napr(int ind,float k, bool nach)
        {
            Index_vet = ind;
            koef_k_Unom = k;
            nachalo = nach;
        }
    }
    public class Prov_zam
    {
        public int Index_vet;
        public string mar_new;
        public float sech;
        public Prov_zam(int ind, string mar, float sech)
        {
            Index_vet = ind;
            mar_new = mar;
            this.sech = sech;
        }
    }
    public class Tr_zam
    {
        public int Index_vet;
        public string mar_new;
        public float moshn;
        public Tr_zam(int ind, string mar, float m)
        {
            Index_vet = ind;
            mar_new = mar;
            this.moshn = m;
        }
    }
    class ListComparer_bySech : IComparer<Prov_zam>
    {
        public int Compare(Prov_zam x, Prov_zam y)
        {
            if (x.sech > y.sech)
                return 1;
            if (x.sech < y.sech)
                return -1;
            return 0;
        }
    }
    class ListComparer_bySnom : IComparer<Tr_zam>
    {
        public int Compare(Tr_zam x, Tr_zam y)
        {
            if (x.moshn > y.moshn)
                return 1;
            if (x.moshn < y.moshn)
                return -1;
            return 0;
        }
    }
    
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Windows.Forms;

namespace STREAMS
{
    public class raschet
    {
        public static List<List<L_T_Vetvi>> lRaschet;
        List<res_RL> rez_rl;
        List<List<int>> poradok_rascheta;
        public int sum = 0, zamechania = 0;
        int col = 0, errors = 0;
        Proverka prov = new Proverka();
        public bool Show_dialog()
        {
            if (prov.ShowDialog() == DialogResult.Yes)
                return true;
            else
                return false;
        }
        public void change_name_button()
        {
            prov.name_button();
        }
        public void Run()
        {
            lRaschet = new List<List<L_T_Vetvi>>();
            rez_rl = new List<res_RL>();
            poradok_rascheta = new List<List<int>>();
            string ex;
            prov.AddColoredText("                  Запущена проверка исходных данных:" + Environment.NewLine, Color.Black, true);
            prov.AddColoredText("Проверка обязательных для заполнения полей.....................................>>", Color.Black, true);
            ex = proverka_zapolnenia();
            if (ex != "")
            {
                prov.AddColoredText(" ОШИБКИ(" + col.ToString() + ")\n", Color.Red, true);
                prov.AddColoredText(ex, Color.Black, false);
            }
            else
                prov.AddColoredText(" ЗАВЕРШЕНО" + "\n", Color.Green, true);
            sum += col;
            prov.AddColoredText("Проверка соответствия узлов...............................................................>>", Color.Black, true);
            for (int i = 0; i < 24; i++)
            {
                lRaschet.Add(new List<L_T_Vetvi>());
                poradok_rascheta.Add(new List<int>());
            }
            ex = proverka_sootvetstvia_uzlov();
            if (ex != "")
            {
                prov.AddColoredText(" ОШИБКИ(" + col.ToString() + ")\n", Color.Red, true);
                prov.AddColoredText(ex, Color.Black, false);
            }
            else
                prov.AddColoredText(" ЗАВЕРШЕНО" + "\n", Color.Green, true);
            sum += col;
            prov.AddColoredText("Проверка данных по проводам и трансформаторам...............................>>", Color.Black, true);
            ex = proverka_sootvetstvia_prov_transf();
            if (ex != "")
            {
                prov.AddColoredText(" ОШИБКИ(" + col.ToString() + ")\n", Color.Red, true);
                prov.AddColoredText(ex, Color.Black, false);
            }
            else
                prov.AddColoredText(" ЗАВЕРШЕНО" + "\n", Color.Green, true);
            sum += col; 
            if (errors == 0)
            {
            prov.AddColoredText("Формирование схемы сети...................................................................>>", Color.Black, true);           
                ex = formirovanie_shemy();
                if (ex != "")
                {
                    if (zamechania == 0)
                    {
                        prov.AddColoredText(" ОШИБКИ(" + col.ToString() + ")\n", Color.Red, true);
                        prov.AddColoredText(ex, Color.Black, false);
                    }
                    else
                    {
                        prov.AddColoredText(" ЗАМЕЧАНИЯ(" + col.ToString() + ")\n", Color.Orange, true);
                        prov.AddColoredText(ex, Color.Black, false);
                    }
                }

                else
                    prov.AddColoredText(" ЗАВЕРШЕНО" + "\n", Color.Green, true);
                sum += col;
            }
        }
        private string proverka_zapolnenia()
        {
            col = 0;
            string ex = "";
            for (int i = 0; i < BaseShema.UZ.Count; i++)
            {
                if (BaseShema.UZ[i].nameCP == "")
                {
                    ex += "Отсутствует наименование узла №" + (i + 1).ToString() + "\n";
                    col++;
                }
                switch (BaseShema.UZ[i].tipUz)
                {
                    case 0: // узел Центра питания
                        if (BaseShema.UZ[i].napr == 0)
                        { ex += "Не введено ср.экспл. напряжение для ЦП '" + BaseShema.UZ[i].nameCP + "' (строка №" + (i + 1).ToString() + ")\n"; col++; }
                        if (BaseShema.UZ[i].graficU.Count == 0)
                        { ex += "Не сформирован график напряжения в ЦП '" + BaseShema.UZ[i].nameCP + "' (строка №" + (i + 1).ToString() + ")\n"; col++; }
                        break;
                    case 1: // узел Блок-станции
                        if (BaseShema.UZ[i].cosFi == 0)
                        { ex += "Не задан CosFi для блок-станции '" + BaseShema.UZ[i].nameCP + "' (строка №" + (i + 1).ToString() + ")\n"; col++; }
                        if (BaseShema.UZ[i].Pmax == 0)
                        { ex += "Не задано Pmax для блок-станции '" + BaseShema.UZ[i].nameCP + "' (строка №" + (i + 1).ToString() + ")\n"; col++; }
                        if (BaseShema.UZ[i].graficP.Count == 0)
                        { ex += "Не сформирован график активной мощности для блок-станции '" + BaseShema.UZ[i].nameCP + "' (строка №" + (i + 1).ToString() + ")\n"; col++; }
                        if (BaseShema.UZ[i].graficQ.Count == 0)
                        { ex += "Не сформирован график реактивной мощности для блок-станции '" + BaseShema.UZ[i].nameCP + "' (строка №" + (i + 1).ToString() + ")\n"; col++; }
                        break;
                    case 2: // узел Нагрузки
                        if (BaseShema.UZ[i].cosFi == 0)
                        { ex += "Не задан CosFi для узла нагрузки '" + BaseShema.UZ[i].nameCP + "' (строка №" + (i + 1).ToString() + ")\n"; col++; }
                        if (BaseShema.UZ[i].Pmax == 0)
                        { ex += "Не задано Pmax для узла нагрузки '" + BaseShema.UZ[i].nameCP + "' (строка №" + (i + 1).ToString() + ")\n"; col++; }
                        if (BaseShema.UZ[i].graficP.Count == 0)
                        { ex += "Не сформирован график активной мощности для узла нагрузки '" + BaseShema.UZ[i].nameCP + "' (строка №" + (i + 1).ToString() + ")\n"; col++; }
                        if (BaseShema.UZ[i].graficQ.Count == 0)
                        { ex += "Не сформирован график реактивной мощности для узла нагрузки '" + BaseShema.UZ[i].nameCP + "' (строка №" + (i + 1).ToString() + ")\n"; col++; }
                        break;
                }
            }
            return ex;
        }
        private string proverka_sootvetstvia_uzlov()        // проверка узлов по линиям, создание таблицы для расчетов (сразу заполняются нач,кон,ka,Unom, тип ветви)
        {
            bool pr;
            col = 0;
            string ex = "";
            for (int i = 0; i < BaseShema.Vet.Count; i++)
            {
                if (BaseShema.Vet[i].tipV == 0)
                    lRaschet[0].Add(new L_T_Vetvi(-1, true, -1, -1, BaseShema.Vet[i].k1, BaseShema.Vet[i].k2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, true,-1,new List<int>(),new List<int>()));
                else
                    lRaschet[0].Add(new L_T_Vetvi(-1, false, -1, -1, BaseShema.Vet[i].k1, BaseShema.Vet[i].k2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, true, -1, new List<int>(),new List<int>()));
                pr = false;
                for (int j = 0; j < BaseShema.UZ.Count; j++)
                {
                    if (BaseShema.Vet[i].nach == BaseShema.UZ[j].nameCP)
                    {
                        lRaschet[0][i].nach_AO = j;
                        lRaschet[0][i].Unom = BaseShema.UZ[j].nomN;
                        pr = true;
                        break;
                    }
                }
                if (!pr)
                { ex += "Не найден узел НАЧАЛА ветви - " + BaseShema.Vet[i].nach + " (строка №" + (i + 1).ToString() + " в таблице 'ветви')\n"; col++; errors++; }
                pr = false;
                for (int j = 0; j < BaseShema.UZ.Count; j++)
                {
                    if (BaseShema.Vet[i].kon == BaseShema.UZ[j].nameCP)
                    {
                        if (BaseShema.Vet[i].tipV == 0)
                        {
                            lRaschet[0][i].kon_AO = j;
                            if (lRaschet[0][i].Unom != BaseShema.UZ[j].nomN)
                            { ex += "Несоответствие номинальных напряжений узлов в начале и конце ветви (строка №" + (i + 1).ToString() + " в таблице 'ветви')\n"; col++; }
                        }
                        else
                        {
                            lRaschet[0][i].kon_AO = j;
                            lRaschet[0][i].ktr = BaseShema.UZ[j].nomN / lRaschet[0][i].Unom;
                            if (lRaschet[0][i].Unom == BaseShema.UZ[j].nomN)
                            { ex += "Совпадение номинальных напряжений ВН и НН трансформатора (строка №" + (i + 1).ToString() + " в таблице 'ветви')\n"; col++; }
                        }
                        pr = true;
                        break;
                    }
                }
                if (!pr)
                { ex += "Не найден узел КОНЦА ветви - " + BaseShema.Vet[i].kon + " (строка №" + (i + 1).ToString() + " в таблице 'ветви')\n"; col++; errors++; errors++; }
            }
            return ex;
        }
        private string proverka_sootvetstvia_prov_transf()  // проверка наличия марки провода и транформатора в справочнике, заполнение R,X,dPxx,dQxx, idop
        {
            SprInfo sprInfo;
            bool pr, napr, moshn;
            col = 0;
            string ex = "";
            Encoding enc = Encoding.GetEncoding(1251);
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                using (Stream input = File.OpenRead(Application.StartupPath + @"\SprInfo.dll"))             //десериализация файла
                { sprInfo = (SprInfo)formatter.Deserialize(input); }
            }
            catch (Exception err)
            { MessageBox.Show("Ошибка при чтении справочных данных!" + Environment.NewLine + err.ToString()); sprInfo = null; }
            for (int i = 0; i < BaseShema.Vet.Count; i++)
            {
                moshn = false;
                napr = false;
                pr = false;
                if (BaseShema.Vet[i].tipV == 0)
                {
                    for (int j = 0; j < sprInfo.ProvInfo.Count; j++)
                    {
                        if (BaseShema.Vet[i].marka == sprInfo.ProvInfo[j].marka)
                        {
                            lRaschet[0][i].R = sprInfo.ProvInfo[j].R0 * BaseShema.Vet[i].dlina_moshnost;
                            lRaschet[0][i].X = sprInfo.ProvInfo[j].X0 * BaseShema.Vet[i].dlina_moshnost;
                            lRaschet[0][i].Idop = sprInfo.ProvInfo[j].idop;
                            lRaschet[0][i].ktr = 1;
                            pr = true;
                            break;
                        }
                    }
                    if (!pr)
                    {
                        for (int j = 0; j < sprInfo.KabelInfo.Count; j++)
                        {
                            if (BaseShema.Vet[i].marka == sprInfo.KabelInfo[j].marka)
                                if (lRaschet[0][i].Unom <= sprInfo.KabelInfo[j].Uiz)
                                {
                                    lRaschet[0][i].R = sprInfo.KabelInfo[j].fR0 * BaseShema.Vet[i].dlina_moshnost;
                                    lRaschet[0][i].X = sprInfo.KabelInfo[j].fX0 * BaseShema.Vet[i].dlina_moshnost;
                                    lRaschet[0][i].dQxx = sprInfo.KabelInfo[j].B0 * (float)Math.Pow(lRaschet[0][i].Unom, 2) * BaseShema.Vet[i].dlina_moshnost / 1000f;
                                    lRaschet[0][i].dPxx = 0.011f * lRaschet[0][i].dQxx;                     // тангенс угла диэлектрических потерь в КЛ (вынести в настройки как срок службы)    
                                    lRaschet[0][i].Idop = sprInfo.KabelInfo[j].idop;
                                    lRaschet[0][i].ktr = 1;
                                    pr = true;
                                    break;
                                }
                                else
                                    napr = true;
                        }
                        if (!pr)
                        {
                            if (napr)
                                ex += "Unom изоляции кабеля " + BaseShema.Vet[i].marka + " ниже Unom ветви (строка №" + (i + 1).ToString() + " в таблице 'ветви')\n";
                            else
                                ex += "Не найдена марка провода(кабеля) - " + BaseShema.Vet[i].marka + " (строка №" + (i + 1).ToString() + " в таблице 'ветви')\n";
                            col++;
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < sprInfo.TrInfo.Count; j++)
                    {
                        if (BaseShema.Vet[i].marka == sprInfo.TrInfo[j].marka)
                            if (BaseShema.Vet[i].dlina_moshnost == sprInfo.TrInfo[j].snom)
                                if ((lRaschet[0][i].Unom == sprInfo.TrInfo[j].klassNomNapr) && (lRaschet[0][i].ktr * 1.01f >= sprInfo.TrInfo[j].unn / sprInfo.TrInfo[j].uvn) && (lRaschet[0][i].ktr * 0.99 <= sprInfo.TrInfo[j].unn / sprInfo.TrInfo[j].uvn))
                                {
                                    lRaschet[0][i].R = sprInfo.TrInfo[j].dpkz * (float)Math.Pow(sprInfo.TrInfo[j].klassNomNapr, 2) / (float)Math.Pow(sprInfo.TrInfo[j].snom, 2) * 1000;
                                    lRaschet[0][i].X = (float)Math.Sqrt((float)Math.Pow(sprInfo.TrInfo[j].ukz, 2) - (float)Math.Pow((sprInfo.TrInfo[j].dpkz / sprInfo.TrInfo[j].snom * 100), 2)) * (float)Math.Pow(sprInfo.TrInfo[j].klassNomNapr, 2) / 100 / sprInfo.TrInfo[j].snom * 1000;
                                    lRaschet[0][i].dPxx = sprInfo.TrInfo[j].dpxx;
                                    lRaschet[0][i].dQxx = sprInfo.TrInfo[j].ixx * sprInfo.TrInfo[j].snom / 100;
                                    lRaschet[0][i].ktr = sprInfo.TrInfo[j].unn / sprInfo.TrInfo[j].uvn;
                                    lRaschet[0][i].Idop = sprInfo.TrInfo[j].snom / sprInfo.TrInfo[j].klassNomNapr / (float)Math.Sqrt(3);
                                    pr = true;
                                    napr = false;
                                    moshn = false;
                                    break;
                                }
                                else
                                    napr = true;
                            else
                                moshn = true;
                    }
                    if (!pr)
                    {
                        for (int j = 0; j < sprInfo.TrSvInfo.Count; j++)
                        {
                            if (BaseShema.Vet[i].marka == sprInfo.TrSvInfo[j].marka)
                                if (BaseShema.Vet[i].dlina_moshnost == sprInfo.TrSvInfo[j].snom)
                                    if ((lRaschet[0][i].Unom == sprInfo.TrSvInfo[j].klassNomNapr) && (lRaschet[0][i].ktr * 1.01f >= sprInfo.TrSvInfo[j].unn / sprInfo.TrSvInfo[j].uvn) && (lRaschet[0][i].ktr * 0.99 <= sprInfo.TrSvInfo[j].unn / sprInfo.TrSvInfo[j].uvn))
                                    {
                                        lRaschet[0][i].R = sprInfo.TrSvInfo[j].dpkz * (float)Math.Pow(sprInfo.TrSvInfo[j].klassNomNapr, 2) / (float)Math.Pow(sprInfo.TrSvInfo[j].snom, 2) * 1000;
                                        lRaschet[0][i].X = (float)Math.Sqrt((float)Math.Pow(sprInfo.TrSvInfo[j].ukz, 2) - (float)Math.Pow((sprInfo.TrSvInfo[j].dpkz / sprInfo.TrSvInfo[j].snom * 100), 2)) * (float)Math.Pow(sprInfo.TrSvInfo[j].klassNomNapr, 2) / 100 / sprInfo.TrSvInfo[j].snom * 1000;
                                        lRaschet[0][i].dPxx = sprInfo.TrSvInfo[j].dpxx;
                                        lRaschet[0][i].dQxx = sprInfo.TrSvInfo[j].ixx * sprInfo.TrSvInfo[j].snom / 100;
                                        lRaschet[0][i].ktr = sprInfo.TrSvInfo[j].unn / sprInfo.TrSvInfo[j].uvn;
                                        lRaschet[0][i].Idop = sprInfo.TrSvInfo[j].snom / sprInfo.TrSvInfo[j].klassNomNapr / (float)Math.Sqrt(3);
                                        pr = true;
                                        napr = false;
                                        moshn = false;
                                        break;
                                    }
                                    else
                                        napr = true;
                                else
                                    moshn = true;
                        }
                        if (!pr)
                        {
                            if (moshn || napr)
                            {
                                if (moshn)
                                    ex += "Несоответсвует мощность трансформатора - " + BaseShema.Vet[i].marka + " (строка №" + (i + 1).ToString() + " в таблице 'ветви')\n";
                                else if (napr)
                                    ex += "Номинальное напряжение ВН/НН трансформатора - " + BaseShema.Vet[i].marka + " не соответсвует Unom в узлах (строка №" + (i + 1).ToString() + " в таблице 'ветви')\n";
                            }
                            else
                                ex += "Не найден тип трансформатора - " + BaseShema.Vet[i].marka + " (строка №" + (i + 1).ToString() + " в таблице 'ветви')\n";
                            col++;
                        }
                    }
                }
            }
            return ex;
        }
        private string formirovanie_shemy()                 // формирование схемы сети, проверка на замкнутые контуры, создание АО
        {
            col = 0;
            string ex = "";
            bool pr, opt, pereschet;
            float sopr1, sopr2, minDeltaSopr = 0;
            int z, num, colK, cash = -1, colZ = 0, tekR = 0;
            List<int> uz_nagr_bs = new List<int>();
            List<set_AO> vremSet = new List<set_AO>(); // временный список
            List<set_AO> rSet = new List<set_AO>();     // расчетная сеть
            List<set_AO> razomkn = new List<set_AO>();  // список разомкнутых ветвей
            for (int i = 0; i < BaseShema.UZ.Count; i++)
                switch (BaseShema.UZ[i].tipUz)
                {
                    case 0:
                        rSet.Add(new set_AO(0, -1, i, false)); break;
                    case 1:
                        uz_nagr_bs.Add(i); break;
                    case 2:
                        uz_nagr_bs.Add(i);break;
                }
            // проверка на наличие замкнутых контуров:
            for (int i = 0; i < lRaschet[0].Count; i++)
                if (lRaschet[0][i].ka != 0)
                    if (lRaschet[0][i].ka == 1)
                        vremSet.Add(new set_AO(lRaschet[0][i].R, lRaschet[0][i].nach_AO, lRaschet[0][i].kon_AO, true));
                    else
                        vremSet.Add(new set_AO(lRaschet[0][i].R, lRaschet[0][i].nach_AO, lRaschet[0][i].kon_AO, false));
            num = 0;
            while (num != rSet.Count)
            {
                z = rSet[num].K;
                for (int q = 0; q < vremSet.Count; q++)
                {
                    pr = false;
                    if (vremSet[q].N == z)
                    {
                        for (int d = 0; d < rSet.Count; d++)
                            if (vremSet[q].K == rSet[d].K)  // проверка на совпадение конечных АО
                            {
                                colZ++;
                                razomkn.Add(new set_AO(vremSet[q].sopr, vremSet[q].N, vremSet[q].K, vremSet[q].razom));
                                vremSet.RemoveAt(q);
                                q--;
                                pr = true; break;
                            }
                        if (!pr)
                        {
                            rSet.Add(new set_AO(vremSet[q].sopr, vremSet[q].N, vremSet[q].K, vremSet[q].razom));
                            for(int j=0;j<uz_nagr_bs.Count;j++)
                                if (vremSet[q].K == uz_nagr_bs[j])
                                {
                                    uz_nagr_bs.RemoveAt(j);
                                    break;
                                }
                            vremSet.RemoveAt(q);
                            q--;
                        }
                    }
                    else if (vremSet[q].K == z)    // для ветвей "задом наперед"
                    {
                        for (int d = 0; d < rSet.Count; d++)
                            if (vremSet[q].N == rSet[d].K)  // проверка на совпадение конечных АО
                            {
                                colZ++;
                                razomkn.Add(new set_AO(vremSet[q].sopr, vremSet[q].K, vremSet[q].N, vremSet[q].razom));                                
                                vremSet.RemoveAt(q);
                                q--;
                                pr = true; break;
                            }
                        if (!pr)
                        {
                            rSet.Add(new set_AO(vremSet[q].sopr, vremSet[q].K, vremSet[q].N, vremSet[q].razom));
                            for (int j = 0; j < uz_nagr_bs.Count; j++)
                                if (vremSet[q].N == uz_nagr_bs[j])
                                {
                                    uz_nagr_bs.RemoveAt(j);
                                    break;
                                }
                            vremSet.RemoveAt(q);
                            q--;
                        }
                    }
                }
                num++;
            }
            if (vremSet.Count != 0)
            {
                col++;
                ex += "Следующие ветви не были присоединены к сети: \n";
                for (int i = 0; i < vremSet.Count; i++)
                    ex += BaseShema.UZ[vremSet[i].N].nameCP.ToString() + " - " + BaseShema.UZ[vremSet[i].K].nameCP.ToString() + "\n";
            }
            if (uz_nagr_bs.Count > 0)
            {
                col += uz_nagr_bs.Count;
                ex += "Следующие узлы с нагрузкой (генерацией) не подключены к сети: \n";
                for (int i = 0; i < uz_nagr_bs.Count; i++)
                    ex += BaseShema.UZ[uz_nagr_bs[i]].nameCP.ToString() + "\n";
            }

            if (colZ != 0)
            { col += colZ; ex += "В сети есть замкнутые контуры (" + colZ.ToString() + " шт.)\n"; }

            // определение замкнутых контуров от разомкнутых участков
            List<List<set_AO>> kontury = new List<List<set_AO>>();
            for (int i = 0; i < colZ; i++)
            {
                kontury.Add(new List<set_AO>());
                kontury[i].Add(new set_AO(razomkn[i].sopr, razomkn[i].N, razomkn[i].K, razomkn[i].razom));
                z = 0; pr = false;
                for (int j = 0; j < rSet.Count; j++)
                {
                    if (rSet[j].K == kontury[i][z].K)
                    { kontury[i].Add(new set_AO(rSet[j].sopr, rSet[j].N, rSet[j].K, rSet[j].razom)); break; }
                }
                if (kontury[i][0].N != kontury[i][1].N)
                {
                    while (!pr)
                    {
                        for (int j = 0; j < rSet.Count; j++)
                        {
                            if (rSet[j].K == kontury[i][z].N)
                            { cash = rSet[j].N; kontury[i].Add(new set_AO(rSet[j].sopr, rSet[j].N, rSet[j].K, rSet[j].razom)); break; }
                        }
                        for (int j = 0; j < kontury[i].Count - 1; j++)
                            if (kontury[i][j].N == cash)
                            { pr = true; break; }
                        z++;
                    }
                    pr = false;
                    for (int g = kontury[i].Count; g > 0; g--)
                    {
                        for (int j = 0; j < kontury[i].Count; j++)
                            if (cash == kontury[i][j].K)
                            { cash = kontury[i][j].N; kontury[i].RemoveAt(j); j--; break; }
                    }
                }
            }

            // проверка на возможность размыкания в оптимальных точках (по сопротивлению)
            if (kontury.Count != 0)
            {
                int k = 0;
                pr = false;
                colK = 0; opt = false;
                while (!opt)          // будет выполнятся до оптимума
                {
                    num = 0;
                    z = kontury[k][0].K;
                    cash = kontury[k][kontury[k].Count - 1].N;
                    if (kontury[k][0].N != cash)
                    {
                        while (kontury[k][0].N != cash)     // сортировка в последовательную линию
                        {
                            for (int j = 0; j < kontury[k].Count; j++)
                                if (kontury[k][j].K == kontury[k][0].N)
                                {
                                    kontury[k].Insert(0, kontury[k][j]);
                                    kontury[k].RemoveAt(j + 1);
                                    num++;
                                }
                        }
                    }
                    else
                        for (int j = 0; j < kontury[k].Count; j++)
                            if (kontury[k][j].K == razomkn[k].K)
                            { num = j; break; }
                    // определение сопротивлений участков по обе стороны от разрыва
                    sopr1 = sopr2 = 0;
                    tekR = num;
                    List<int> vozm_razr = new List<int>();
                    List<float> vozm_sopr = new List<float>();
                    for (int j = 0; j < kontury[k].Count; j++)
                        sopr2 += kontury[k][j].sopr;
                    for (int j = 0; j < kontury[k].Count; j++)
                    {
                        if (j != 0)
                            sopr1 += kontury[k][j - 1].sopr;
                        sopr2 -= kontury[k][j].sopr;
                        if (kontury[k][j].razom == true)
                        {
                            vozm_razr.Add(j);
                            vozm_sopr.Add(Math.Abs(sopr1 - sopr2));
                        }
                    }
                    if (vozm_razr.Count == 0)
                    {
                        ex += "Невозможно разомкнуть контур :";
                        for (int j = 0; j < kontury[k].Count; j++)
                        {
                            if (kontury[k][j].N != -1)
                                ex += BaseShema.UZ[kontury[k][j].N].nameCP.ToString() + ", ";
                            if (j == num)
                                ex += BaseShema.UZ[kontury[k][j].K].nameCP.ToString() + ", ";
                        }
                        ex = ex.Remove(ex.Length - 2);
                        ex += "\nРазомкните указанный контур, используя коммутационный аппарат.\n";
                        break;
                    }
                    for (int j = 0; j < vozm_sopr.Count; j++)
                        if (j == 0)
                        {
                            minDeltaSopr = vozm_sopr[0];
                            num = vozm_razr[0];
                        }
                        else
                            if (vozm_sopr[j] < minDeltaSopr)
                            {
                                minDeltaSopr = vozm_sopr[j];
                                num = vozm_razr[j];
                            }
                    if (num != tekR)                // если оптимальный разрыв отличается от существующего
                    {
                        // применение к расчетной схеме нового места разрыва
                        if (num < tekR)
                        {
                            for (int v = 0; v < rSet.Count; v++)            // удаление из расчетной сети нового разреза
                                if (kontury[k][num].K == rSet[v].K)
                                {
                                    razomkn.Insert(k, rSet[v]);
                                    rSet.RemoveAt(v); break;
                                }
                            rSet.Add(razomkn[k + 1]);                         // перенос в сеть старого разреза 
                            razomkn.RemoveAt(k + 1);
                            for (int j = num + 1; j <= tekR; j++)           // переворот начала и конца ветви в расчетной сети и текущем контуре
                            {
                                for (int v = 0; v < rSet.Count; v++)
                                {
                                    if (kontury[k][j].K == rSet[v].K)
                                        if (kontury[k][j].N == rSet[v].N)
                                        {
                                            cash = rSet[v].K;
                                            rSet[v].K = rSet[v].N;
                                            rSet[v].N = cash;
                                            break;
                                        }
                                }
                                cash = kontury[k][j].K;
                                kontury[k][j].K = kontury[k][j].N;
                                kontury[k][j].N = cash;
                            }
                        }
                        else
                        {
                            for (int v = 0; v < rSet.Count; v++)            // удаление из расчетной сети нового разреза (с переворотом)
                                if (kontury[k][num].K == rSet[v].K)
                                {
                                    razomkn.Insert(k, new set_AO(rSet[v].sopr, rSet[v].K, rSet[v].N, rSet[v].razom));
                                    rSet.RemoveAt(v); break;
                                }
                            rSet.Add(razomkn[k + 1]);                       // перенос в сеть старого разреза 
                            razomkn.RemoveAt(k + 1);
                            cash = kontury[k][num].K;
                            kontury[k][num].K = kontury[k][num].N;
                            kontury[k][num].N = cash;
                            for (int j = num - 1; j > tekR; j--)           // переворот начала и конца ветви в расчетной сети и текущем контуре
                            {
                                for (int v = 0; v < rSet.Count; v++)
                                {
                                    if (kontury[k][j].K == rSet[v].K)
                                    {
                                        cash = rSet[v].K;
                                        rSet[v].K = rSet[v].N;
                                        rSet[v].N = cash;
                                        break;
                                    }
                                }
                                cash = kontury[k][j].K;
                                kontury[k][j].K = kontury[k][j].N;
                                kontury[k][j].N = cash;
                            }
                        }
                        colK = 0;
                        // пересчет других контуров, если новый разрез попал на какой-либо из них
                        for (int k1 = 0; k1 < kontury.Count; k1++)
                        {
                            pereschet = false;
                            if (k1 != k)
                                for (int k2 = 0; k2 < kontury[k1].Count; k2++)
                                {
                                    if (((kontury[k1][k2].K == kontury[k][num].K)&&(kontury[k1][k2].N == kontury[k][num].N))||
                                        ((kontury[k1][k2].K == kontury[k][num].N)&&(kontury[k1][k2].N == kontury[k][num].K)))
                                    { pereschet = true; break; }
                                }
                            if (pereschet)
                            {
                                kontury[k1].Clear();
                                kontury[k1].Add(new set_AO(razomkn[k1].sopr,razomkn[k1].N,razomkn[k1].K,razomkn[k1].razom));
                                z = 0; pr = false;
                                for (int j = 0; j < rSet.Count; j++)
                                {
                                    if (rSet[j].K == kontury[k1][z].K)
                                    { kontury[k1].Add(new set_AO(rSet[j].sopr,rSet[j].N,rSet[j].K,rSet[j].razom)); break; }
                                }
                                if (kontury[k1][0].N != kontury[k1][1].N)
                                {
                                    while (!pr)
                                    {
                                        for (int j = 0; j < rSet.Count; j++)
                                        {
                                            if (rSet[j].K == kontury[k1][z].N)
                                            { cash = rSet[j].N; kontury[k1].Add(new set_AO(rSet[j].sopr, rSet[j].N, rSet[j].K, rSet[j].razom)); break; }
                                        }
                                        for (int j = 0; j < kontury[k1].Count - 1; j++)
                                            if (kontury[k1][j].N == cash)
                                            { pr = true; break; }
                                        z++;
                                    }
                                    pr = false;
                                    for (int g = kontury[k1].Count; g > 0; g--)             // удаление "хвостов"
                                    {
                                        for (int j = 0; j < kontury[k1].Count; j++)
                                            if (cash == kontury[k1][j].K)
                                            { cash = kontury[k1][j].N; kontury[k1].RemoveAt(j); j--; break; }
                                    }
                                }
                            }
                        }
                    }
                    else
                        colK++;
                    if (colK == kontury.Count)
                    { opt = true; break; }
                    k++;
                    if (k == kontury.Count)
                        k = 0;
                }
                if (colK == kontury.Count)
                {
                    ex += "Для проведения дальнейших расчетов:\n";
                    for (int i = 0; i < kontury.Count; i++)
                    {
                        cash = -2;
                        ex += "в контуре (";
                        for (int j = 0; j < kontury[i].Count; j++)
                        {
                            if (kontury[i][j].K == cash)
                                ex += BaseShema.UZ[kontury[i][j].K].nameCP.ToString() + ", ";
                            cash = kontury[i][j].K;
                            if (kontury[i][j].N != -1)
                                ex += BaseShema.UZ[kontury[i][j].N].nameCP.ToString() + ", ";
                        }
                        ex = ex.Remove(ex.Length - 2);
                        ex += ") будет разомкнута ветвь   " + BaseShema.UZ[razomkn[i].N].nameCP.ToString() + " - " + BaseShema.UZ[razomkn[i].K].nameCP.ToString() + "\n";
                    }
                    zamechania = colK;
                }
                for (int i = 0; i < razomkn.Count; i++)
                    for (int j = 0; j < lRaschet[0].Count; j++)
                    {
                        if (razomkn[i].N == lRaschet[0][j].nach_AO)
                        {
                            if (razomkn[i].K == lRaschet[0][j].kon_AO)
                            { lRaschet[0][j].ka = 0; break; }
                        }
                        else if (razomkn[i].N == lRaschet[0][j].kon_AO)
                            if (razomkn[i].K == lRaschet[0][j].nach_AO)
                            { lRaschet[0][j].ka = 0; break; }
                    }

                if (rSet.Count > 1)
                {
                    z = 1;
                    for (int i = 0; i < rSet.Count; i++)                // пересортировка rset
                    {
                        for (int j = z; j < rSet.Count; j++)
                        {
                            if (rSet[z].N == -1)
                                z++;
                            else if (rSet[j].N == rSet[i].K)
                            {
                                if (j != z)
                                { rSet.Insert(z, rSet[j]); rSet.RemoveAt(j + 1); }
                                z++;
                            }
                        }
                    }
                }
            }
            //for (int i = 0; i < rSet.Count; i++)
            //    for (int j = 0; j < lRaschet[0].Count; j++)
            //    {
            //        if (rSet[i].N == lRaschet[0][j].nach_AO)
            //        {
            //            if (rSet[i].K == lRaschet[0][j].kon_AO)
            //            { poradok_rascheta[0].Add(j); break; }
            //        }
            //        else if (rSet[i].N == lRaschet[0][j].kon_AO)
            //            if (rSet[i].K == lRaschet[0][j].nach_AO)
            //            { poradok_rascheta[0].Add(j); lRaschet[0][j].napravlenie = false; break; }
            //    }
            return ex;
        }
        private void create_new_result()
        {
            int cash;
            int q=0;
            int z ;
            for (int i = 0; i < BaseShema.UZ.Count; i++)                                  // заполнение режимной информации
                switch (BaseShema.UZ[i].tipUz)
                {
                    case 0:
                        cash = -1;
                        z = 0;
                        for (int j = 0; j < lRaschet[0].Count; j++)
                            if (lRaschet[0][j].ka != 0)
                            {
                                if ((i == lRaschet[0][j].nach_AO) || (i == lRaschet[0][j].kon_AO))
                                {
                                    //lRaschet[0][j].numRL = q;
                                    rez_rl.Add(new res_RL());
                                    rez_rl[q].new_result();
                                    if (cash == i)
                                    { z++; rez_rl[q].nameRL = BaseShema.UZ[i].nameCP + "(" + z.ToString() + ")"; }
                                    else
                                        rez_rl[q].nameRL = BaseShema.UZ[i].nameCP;
                                    for (int k = 0; k < 24; k++)
                                        rez_rl[q].add_result();
                                    q++;
                                    cash = i;
                                }
                            }
                        break;
                }
            rez_rl.Add(new res_RL());
            rez_rl[rez_rl.Count - 1].new_result();
            for (int i = 0; i < 24; i++)
                rez_rl[rez_rl.Count - 1].add_result();
            rez_rl[rez_rl.Count - 1].nameRL = "Вся сеть";
        }
        public void Regim()
        {
            create_new_result();
            copy_table_to_all(24);
            for (int i = 0; i < 24; i++)
            { Regim_raschet(i); }
        }
        public void in_test(int T)
        {
            TestForm t = new TestForm();
            t.Clear();
            for (int i = 0; i < lRaschet[T].Count; i++)
                t.addvet(lRaschet[T][i], BaseShema.UZ[lRaschet[T][i].nach_AO].nameCP, BaseShema.UZ[lRaschet[T][i].kon_AO].nameCP);
            t.Rezultat();
            t.Show();
            t.Update();
        }
        private void copy_table_to_all(int T)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, lRaschet[0]);
                for (int i = 1; i < T; i++)
                {ms.Position = 0;
                lRaschet[i] = (List<L_T_Vetvi>)bf.Deserialize(ms);}               
            }
            //for (int i = 1; i < T; i++)
            //{
            //    for(int j =  0;j<poradok_rascheta[0].Count;j++)
            //        poradok_rascheta[i].Add(poradok_rascheta[0][j]);
            //}
        }
        private void Regim_raschet(int T)
        {
            Opredelit_Poradok_seti(T);
            Add_regim_info(T, false);        
            for (int i = (poradok_rascheta[T].Count - 1); i >= 0; i--)
            {               
                Potok_vetvi_raschet_obr(poradok_rascheta[T][i],T,false);
            }            
            for (int i = 0; i<poradok_rascheta[T].Count; i++)
            {
                Napr_vetvi_raschet_pr(poradok_rascheta[T][i], T);
            }           
            Add_regim_info(T, true);
            for (int i = (poradok_rascheta[T].Count - 1); i >= 0; i--)
            {
                Potok_vetvi_raschet_obr(poradok_rascheta[T][i], T,true);
            }
            for (int i = 0; i < lRaschet[T].Count; i++)
            {
                raschet_Tokov(T, i);
            }
            //in_test(T);
            poluchit_result(T);
            
        }
        private void raschet_Tokov(int T,int i)
        {
            if(lRaschet[T][i].ka!=0)
            {
                if (lRaschet[T][i].napravlenie)
                    lRaschet[T][i].I = (float)Math.Sqrt((lRaschet[T][i].Pn * lRaschet[T][i].Pn + lRaschet[T][i].Qn * lRaschet[T][i].Qn) / 3) / lRaschet[T][i].Un;
                else
                    lRaschet[T][i].I = (float)Math.Sqrt((lRaschet[T][i].Pk * lRaschet[T][i].Pk + lRaschet[T][i].Qk * lRaschet[T][i].Qk) / 3) / lRaschet[T][i].Uk;
            }
            else
            {
                lRaschet[T][i].I =0;
            }
        }
        private void Opredelit_Poradok_seti(int t)
        {
            poradok_rascheta[t] = new List<int>();
            List<int> ne_provereno = new List<int>();
            int nov;
            int n = -1;
            List<int> pred = new List<int>();
            for (int i = 0; i < lRaschet[t].Count; i++)
            {
                if (lRaschet[t][i].ka != 0)
                {
                    if (BaseShema.UZ[lRaschet[t][i].nach_AO].tipUz == 0)
                    {
                        n++;
                        lRaschet[t][i].napravlenie = true;
                        ne_provereno.Add(lRaschet[t][i].kon_AO);
                        pred.Add(i);
                        lRaschet[t][i].numRL = n;
                        poradok_rascheta[t].Add(i);
                    }
                    else if (BaseShema.UZ[lRaschet[t][i].kon_AO].tipUz == 0)
                    {
                        n++;
                        lRaschet[t][i].napravlenie = false;
                        ne_provereno.Add(lRaschet[t][i].nach_AO);
                        pred.Add(i);
                        lRaschet[t][i].numRL = n;
                        poradok_rascheta[t].Add(i);
                    }
                    while (ne_provereno.Count > 0)
                    {
                        nov = ne_provereno[0];
                        for (int j = 0; j < lRaschet[t].Count; j++)
                        {
                            if (pred[0] != j)
                                if (lRaschet[t][j].nach_AO == nov)
                                {
                                    if (lRaschet[t][j].ka != 0)
                                    {
                                        poradok_rascheta[t].Add(j);//+
                                        lRaschet[t][pred[0]].posle.Add(j);//+
                                        ne_provereno.Add(lRaschet[t][j].kon_AO); //+
                                        pred.Add(j);//+
                                        lRaschet[t][j].napravlenie = true;//+
                                        lRaschet[t][j].pred = pred[0];//+
                                        lRaschet[t][j].numRL = n;//+
                                    }
                                    else
                                    {
                                        lRaschet[t][pred[0]].posle.Add(j);//+
                                        lRaschet[t][j].napravlenie = true;//+
                                        lRaschet[t][j].posle.Add(pred[0]);//+                                        
                                    }
                                }
                                else if (lRaschet[t][j].kon_AO == nov)
                                {
                                    if (lRaschet[t][j].ka != 0)
                                    {
                                        poradok_rascheta[t].Add(j);//+
                                        lRaschet[t][pred[0]].posle.Add(j);//+
                                        ne_provereno.Add(lRaschet[t][j].nach_AO);//+
                                        pred.Add(j);//+
                                        lRaschet[t][j].napravlenie = false;//+
                                        lRaschet[t][j].pred = pred[0];//+
                                        lRaschet[t][j].numRL = n;//+
                                    }
                                    else
                                    {
                                        lRaschet[t][pred[0]].posle.Add(j);//+
                                        lRaschet[t][j].napravlenie = true;//+
                                        lRaschet[t][j].posle.Add(pred[0]);//+                                       
                                    }
                                }
                        }
                        ne_provereno.RemoveAt(0); pred.RemoveAt(0);
                    }
                }
                else
                {
                    for (int j = 0; j < lRaschet[t].Count; j++)
                    {
                        if (i != j && lRaschet[t][j].ka == 0)
                        {
                            if (lRaschet[t][i].nach_AO == lRaschet[t][j].nach_AO || lRaschet[t][i].kon_AO == lRaschet[t][j].nach_AO ||
                                lRaschet[t][i].nach_AO == lRaschet[t][j].kon_AO || lRaschet[t][i].kon_AO == lRaschet[t][j].kon_AO)
                                lRaschet[t][i].posle_razom.Add(j);
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
                for (int i = 0; i < lRaschet[T].Count; i++)
                    posled.Add(i);
            }
            for (int i = 0; i < BaseShema.UZ.Count; i++)                                  // заполнение режимной информации
                switch (BaseShema.UZ[i].tipUz)
                {
                    case 0:
                        if(!clear)
                        for (int j = 0; j < lRaschet[T].Count; j++)
                            if ((lRaschet[T][j].napravlenie) && (lRaschet[T][j].ka != 0))
                            {
                                if (i == lRaschet[T][j].nach_AO)
                                { lRaschet[T][j].Un = BaseShema.UZ[i].graficU[T].Yset; }
                            }
                            else if (lRaschet[T][j].ka != 0)
                            {
                                if (i == lRaschet[T][j].kon_AO)
                                { lRaschet[T][j].Uk = BaseShema.UZ[i].graficU[T].Yset; }
                            }
                        break;
                    case 1:
                        for (int j = 0; j < lRaschet[T].Count; j++)
                            if ((lRaschet[T][j].napravlenie) && (lRaschet[T][j].ka != 0))
                            {
                                if (i == lRaschet[T][j].kon_AO)
                                {
                                    lRaschet[T][j].Pk = - BaseShema.UZ[i].graficP[T].Yset;
                                    lRaschet[T][j].Qk = - BaseShema.UZ[i].graficQ[T].Yset;
                                    if (clear)
                                        { lRaschet[T][j].dP = lRaschet[T][j].Pn = lRaschet[T][j].dQ = lRaschet[T][j].Qn = 0; posled.Remove(j); }
                                    break;
                                }
                            }
                            else if (lRaschet[T][j].ka != 0)
                            {
                                if (i == lRaschet[T][j].nach_AO)
                                {
                                    lRaschet[T][j].Pn = -BaseShema.UZ[i].graficP[T].Yset;
                                    lRaschet[T][j].Qn = -BaseShema.UZ[i].graficQ[T].Yset;
                                    if (clear)
                                        {lRaschet[T][j].dP = lRaschet[T][j].Pk = lRaschet[T][j].dQ = lRaschet[T][j].Qk = 0;posled.Remove(j); }
                                    break;
                                }
                            }
                        break;
                    case 2:
                        for (int j = 0; j < lRaschet[T].Count; j++)
                            if ((lRaschet[T][j].napravlenie) && (lRaschet[T][j].ka != 0))
                            {
                                if (i == lRaschet[T][j].kon_AO)
                                {
                                    lRaschet[T][j].Pk = BaseShema.UZ[i].graficP[T].Yset;
                                    lRaschet[T][j].Qk = BaseShema.UZ[i].graficQ[T].Yset;
                                    if (clear)
                                    {lRaschet[T][j].dP = lRaschet[T][j].Pn = lRaschet[T][j].dQ = lRaschet[T][j].Qn = 0;posled.Remove(j); }
                                    break;
                                }
                            }
                            else if (lRaschet[T][j].ka != 0)
                            {
                                if (i == lRaschet[T][j].nach_AO)
                                {
                                    lRaschet[T][j].Pn = BaseShema.UZ[i].graficP[T].Yset;
                                    lRaschet[T][j].Qn = BaseShema.UZ[i].graficQ[T].Yset;
                                    if (clear)
                                    { lRaschet[T][j].dP = lRaschet[T][j].Pk = lRaschet[T][j].dQ = lRaschet[T][j].Qk = 0; posled.Remove(j); }
                                    break;
                                }
                            }
                        break;
                }
            if(clear)
            for (int i = 0; i < posled.Count; i++)
                lRaschet[T][posled[i]].Pn = lRaschet[T][posled[i]].dP = lRaschet[T][posled[i]].Pk = lRaschet[T][posled[i]].Qn = lRaschet[T][posled[i]].dQ = lRaschet[T][posled[i]].Qk = 0;
        }
        private void Potok_vetvi_raschet_obr(int n, int T, bool napr_d)
        {
            if (lRaschet[T][n].napravlenie)
            {
                if (napr_d)
                {
                    lRaschet[T][n].dP = ((float)Math.Pow(lRaschet[T][n].Pk, 2) + (float)Math.Pow(lRaschet[T][n].Qk, 2)) / (float)Math.Pow(lRaschet[T][n].Uk / lRaschet[T][n].ktr, 2) * lRaschet[T][n].R / 1000;
                    lRaschet[T][n].dQ = ((float)Math.Pow(lRaschet[T][n].Pk, 2) + (float)Math.Pow(lRaschet[T][n].Qk, 2)) / (float)Math.Pow(lRaschet[T][n].Uk / lRaschet[T][n].ktr, 2) * lRaschet[T][n].X / 1000;
                    lRaschet[T][n].dPxx = lRaschet[T][n].dPxx * (float)Math.Pow((lRaschet[T][n].Un / lRaschet[T][n].Unom), 2);
                    lRaschet[T][n].dQxx = lRaschet[T][n].dQxx * (float)Math.Pow((lRaschet[T][n].Un / lRaschet[T][n].Unom), 2);
                }
                else
                {
                    lRaschet[T][n].dP = ((float)Math.Pow(lRaschet[T][n].Pk, 2) + (float)Math.Pow(lRaschet[T][n].Qk, 2)) / (float)Math.Pow(lRaschet[T][n].Unom, 2) * lRaschet[T][n].R / 1000;
                    lRaschet[T][n].dQ = ((float)Math.Pow(lRaschet[T][n].Pk, 2) + (float)Math.Pow(lRaschet[T][n].Qk, 2)) / (float)Math.Pow(lRaschet[T][n].Unom, 2) * lRaschet[T][n].X / 1000;
                }
                lRaschet[T][n].Pn = lRaschet[T][n].Pk + lRaschet[T][n].dP + lRaschet[T][n].dPxx;
                lRaschet[T][n].Qn = lRaschet[T][n].Qk + lRaschet[T][n].dQ + lRaschet[T][n].dQxx;

                if (lRaschet[T][n].pred != -1)
                    if (lRaschet[T][lRaschet[T][n].pred].napravlenie)
                    {
                        if (lRaschet[T][n].nach_AO == lRaschet[T][lRaschet[T][n].pred].kon_AO)
                        {
                            lRaschet[T][lRaschet[T][n].pred].Pk += lRaschet[T][n].Pn;
                            lRaschet[T][lRaschet[T][n].pred].Qk += lRaschet[T][n].Qn;
                        }
                    }
                    else
                    {
                        if (lRaschet[T][n].nach_AO == lRaschet[T][lRaschet[T][n].pred].nach_AO)
                        {
                            lRaschet[T][lRaschet[T][n].pred].Pn += lRaschet[T][n].Pn;
                            lRaschet[T][lRaschet[T][n].pred].Qn += lRaschet[T][n].Qn;
                        }
                    }
            }
            else
            {
                if (napr_d)
                {
                    lRaschet[T][n].dP = ((float)Math.Pow(lRaschet[T][n].Pn, 2) + (float)Math.Pow(lRaschet[T][n].Qn, 2)) / (float)Math.Pow(lRaschet[T][n].Un * lRaschet[T][n].ktr, 2) * lRaschet[T][n].R / 1000;
                    lRaschet[T][n].dQ = ((float)Math.Pow(lRaschet[T][n].Pn, 2) + (float)Math.Pow(lRaschet[T][n].Qn, 2)) / (float)Math.Pow(lRaschet[T][n].Un * lRaschet[T][n].ktr, 2) * lRaschet[T][n].X / 1000;
                    lRaschet[T][n].dPxx = lRaschet[T][n].dPxx * (float)Math.Pow((lRaschet[T][n].Uk / lRaschet[T][n].Unom), 2);
                    lRaschet[T][n].dQxx = lRaschet[T][n].dQxx * (float)Math.Pow((lRaschet[T][n].Uk / lRaschet[T][n].Unom), 2);
                }
                else
                {
                    lRaschet[T][n].dP = ((float)Math.Pow(lRaschet[T][n].Pn, 2) + (float)Math.Pow(lRaschet[T][n].Qn, 2)) / (float)Math.Pow(lRaschet[T][n].Unom, 2) * lRaschet[T][n].R / 1000;
                    lRaschet[T][n].dQ = ((float)Math.Pow(lRaschet[T][n].Pn, 2) + (float)Math.Pow(lRaschet[T][n].Qn, 2)) / (float)Math.Pow(lRaschet[T][n].Unom, 2) * lRaschet[T][n].X / 1000;
                }
                lRaschet[T][n].Pk = lRaschet[T][n].Pn + lRaschet[T][n].dP + lRaschet[T][n].dPxx;
                lRaschet[T][n].Qk = lRaschet[T][n].Qn + lRaschet[T][n].dQ + lRaschet[T][n].dQxx;
                if (lRaschet[T][n].pred != -1)
                    if (lRaschet[T][lRaschet[T][n].pred].napravlenie)
                    {
                        if (lRaschet[T][n].kon_AO == lRaschet[T][lRaschet[T][n].pred].kon_AO)
                        {
                            lRaschet[T][lRaschet[T][n].pred].Pk += lRaschet[T][n].Pk;
                            lRaschet[T][lRaschet[T][n].pred].Qk += lRaschet[T][n].Qk;
                        }
                    }
                    else
                    {
                        if (lRaschet[T][n].kon_AO == lRaschet[T][lRaschet[T][n].pred].nach_AO)
                        {
                            lRaschet[T][lRaschet[T][n].pred].Pn += lRaschet[T][n].Pk;
                            lRaschet[T][lRaschet[T][n].pred].Qn += lRaschet[T][n].Qk;
                        }
                    }

            }
        }
        private void Napr_vetvi_raschet_pr(int n, int T)
        {
            if (lRaschet[T][n].napravlenie)
            {
                lRaschet[T][n].dU = ((lRaschet[T][n].Pn - lRaschet[T][n].dPxx) * lRaschet[T][n].R + (lRaschet[T][n].Qn - lRaschet[T][n].dQxx) * lRaschet[T][n].X) / lRaschet[T][n].Un / 1000;
                lRaschet[T][n].Uk = (lRaschet[T][n].Un - lRaschet[T][n].dU) * lRaschet[T][n].ktr;
                for (int i = 0; i < lRaschet[T][n].posle.Count; i++)
                    if (lRaschet[T][lRaschet[T][n].posle[i]].ka != 0)
                    {
                        if (lRaschet[T][lRaschet[T][n].posle[i]].napravlenie)
                        {
                            if (lRaschet[T][lRaschet[T][n].posle[i]].nach_AO == lRaschet[T][n].kon_AO)
                            {
                                lRaschet[T][lRaschet[T][n].posle[i]].Un = lRaschet[T][n].Uk; //lRaschet[T][lRaschet[T][n].posle[i]].numRL = lRaschet[T][n].numRL;
                            }
                        }
                        else
                        {
                            if (lRaschet[T][lRaschet[T][n].posle[i]].kon_AO == lRaschet[T][n].kon_AO)
                            {
                                lRaschet[T][lRaschet[T][n].posle[i]].Uk = lRaschet[T][n].Uk; //lRaschet[T][lRaschet[T][n].posle[i]].numRL = lRaschet[T][n].numRL;
                            }
                        }
                    }
            }
            else
            {
                lRaschet[T][n].dU = ((lRaschet[T][n].Pk - lRaschet[T][n].dPxx) * lRaschet[T][n].R + (lRaschet[T][n].Qk - lRaschet[T][n].dQxx) * lRaschet[T][n].X) / lRaschet[T][n].Uk / 1000;
                lRaschet[T][n].Un = (lRaschet[T][n].Uk - lRaschet[T][n].dU) / lRaschet[T][n].ktr;
                for (int i = 0; i < lRaschet[T][n].posle.Count; i++)
                    if (lRaschet[T][lRaschet[T][n].posle[i]].ka != 0)
                    {
                        if (lRaschet[T][lRaschet[T][n].posle[i]].napravlenie)
                        {
                            if (lRaschet[T][lRaschet[T][n].posle[i]].nach_AO == lRaschet[T][n].nach_AO)
                            {
                                lRaschet[T][lRaschet[T][n].posle[i]].Un = lRaschet[T][n].Un;// lRaschet[T][lRaschet[T][n].posle[i]].numRL = lRaschet[T][n].numRL;
                            }
                        }
                        else
                        {
                            if (lRaschet[T][lRaschet[T][n].posle[i]].kon_AO == lRaschet[T][n].nach_AO)
                            {
                                lRaschet[T][lRaschet[T][n].posle[i]].Uk = lRaschet[T][n].Un; //lRaschet[T][lRaschet[T][n].posle[i]].numRL = lRaschet[T][n].numRL;
                            }
                        }
                    }
            }
        }
        private void poluchit_result(int T)
        {
            List<int> posledov = new List<int>();
            for(int i=0;i<rez_rl.Count;i++)
                rez_rl[i].Clear(T);
            for (int i = 0; i < lRaschet[T].Count; i++)
            {
                if (lRaschet[T][i].ka != 0) 
                    if (lRaschet[T][i].l_tr == true)
                    {
                        rez_rl[lRaschet[T][i].numRL].rez[T].dWnagrLin += lRaschet[T][i].dP;
                        rez_rl[lRaschet[T][i].numRL].rez[T].dWxxLin += lRaschet[T][i].dPxx;
                    }
                    else
                    {
                        rez_rl[lRaschet[T][i].numRL].rez[T].dWnagrTR += lRaschet[T][i].dP;
                        rez_rl[lRaschet[T][i].numRL].rez[T].dWxxTR += lRaschet[T][i].dPxx;
                    }                
            }
            for (int i = 0; i < BaseShema.UZ.Count; i++)
            {
                switch (BaseShema.UZ[i].tipUz)
                {
                    case 1:
                        for(int j=0;j<lRaschet[T].Count;j++)
                            if(lRaschet[T][j].ka!=0)
                                if ((lRaschet[T][j].nach_AO == i) || (lRaschet[T][j].kon_AO == i))
                                {
                                    if (BaseShema.UZ[i].graficP[T].Yset > 0)
                                    { rez_rl[lRaschet[T][j].numRL].rez[T].otp_bs += BaseShema.UZ[i].graficP[T].Yset; break; }
                                    else
                                    { rez_rl[lRaschet[T][j].numRL].rez[T].pol_otp += BaseShema.UZ[i].graficP[T].Yset; break; }
                                }
                        break;
                    case 0:
                        for (int j = 0; j < lRaschet[T].Count; j++)
                            if (lRaschet[T][j].ka != 0)
                            {
                                if (lRaschet[T][j].nach_AO == i)
                                {
                                    posledov.Add(j);
                                    if (lRaschet[T][j].Pn > 0)
                                        rez_rl[lRaschet[T][j].numRL].rez[T].otp_CP += lRaschet[T][j].Pn;
                                    else
                                        rez_rl[lRaschet[T][j].numRL].rez[T].otp_iz_seti += -lRaschet[T][j].Pn;
                                }
                                else if (lRaschet[T][j].kon_AO == i)
                                {
                                    posledov.Add(j);
                                    if (lRaschet[T][j].Pk > 0)
                                        rez_rl[lRaschet[T][j].numRL].rez[T].otp_CP += lRaschet[T][j].Pk;
                                    else
                                        rez_rl[lRaschet[T][j].numRL].rez[T].otp_iz_seti += -lRaschet[T][j].Pk;
                                }
                            }
                        break;
                    case 2:
                        for (int j = 0; j < lRaschet[T].Count; j++)
                            if (lRaschet[T][j].ka != 0)
                                if ((lRaschet[T][j].nach_AO == i) || (lRaschet[T][j].kon_AO == i))
                                {
                                    if (BaseShema.UZ[i].graficP[T].Yset < 0)
                                    { rez_rl[lRaschet[T][j].numRL].rez[T].otp_bs += BaseShema.UZ[i].graficP[T].Yset; break; }
                                    else
                                    { rez_rl[lRaschet[T][j].numRL].rez[T].pol_otp += BaseShema.UZ[i].graficP[T].Yset; break; }
                                }                 
                        break;
                }
            }
            for (int i = 0; i < rez_rl.Count-1; i++)
            {
                rez_rl[i].rez[T].dWsum = rez_rl[i].rez[T].dWnagrLin + rez_rl[i].rez[T].dWnagrTR + rez_rl[i].rez[T].dWxxLin + rez_rl[i].rez[T].dWxxTR;
                rez_rl[i].rez[T].otp_vsego = rez_rl[i].rez[T].otp_bs + rez_rl[i].rez[T].otp_CP;
            }
             int cash = -1; float otp=0;

             for (int i = 0; i < poradok_rascheta[T].Count; i++)
                if (lRaschet[T][poradok_rascheta[T][i]].napravlenie)
                {
                    if (BaseShema.UZ[lRaschet[T][poradok_rascheta[T][i]].nach_AO].tipUz == 0)
                    {
                        if (cash != lRaschet[T][poradok_rascheta[T][i]].nach_AO)
                        {
                            if (otp > 0)
                                rez_rl[rez_rl.Count - 1].rez[T].otp_CP += otp;
                            else
                                rez_rl[rez_rl.Count - 1].rez[T].otp_iz_seti -= otp;
                            if (rez_rl[lRaschet[T][poradok_rascheta[T][i]].numRL].rez[T].otp_CP > 0)
                                otp = rez_rl[lRaschet[T][poradok_rascheta[T][i]].numRL].rez[T].otp_CP;
                            else
                                otp = -rez_rl[lRaschet[T][poradok_rascheta[T][i]].numRL].rez[T].otp_iz_seti;
                        }
                        else
                        {
                            if (rez_rl[lRaschet[T][poradok_rascheta[T][i]].numRL].rez[T].otp_CP > 0)
                                otp += rez_rl[lRaschet[T][poradok_rascheta[T][i]].numRL].rez[T].otp_CP;
                            else
                                otp -= rez_rl[lRaschet[T][poradok_rascheta[T][i]].numRL].rez[T].otp_iz_seti;
                        }
                        cash = lRaschet[T][poradok_rascheta[T][i]].nach_AO;
                    }  
                }
                else
                {
                    if (BaseShema.UZ[lRaschet[T][poradok_rascheta[T][i]].kon_AO].tipUz == 0)
                    {
                        if (cash != lRaschet[T][poradok_rascheta[T][i]].kon_AO)
                        {
                            if (otp > 0)
                                rez_rl[rez_rl.Count - 1].rez[T].otp_CP += otp;
                            else
                                rez_rl[rez_rl.Count - 1].rez[T].otp_iz_seti -= otp;
                            otp = 0;
                            if (rez_rl[lRaschet[T][poradok_rascheta[T][i]].numRL].rez[T].otp_CP > 0)
                                otp += rez_rl[lRaschet[T][poradok_rascheta[T][i]].numRL].rez[T].otp_CP;
                            else
                                otp -= rez_rl[lRaschet[T][poradok_rascheta[T][i]].numRL].rez[T].otp_iz_seti;
                        }
                        else
                        {
                            if (rez_rl[i].rez[T].otp_CP > 0)
                                otp += rez_rl[lRaschet[T][poradok_rascheta[T][i]].numRL].rez[T].otp_CP;
                            else
                                otp -= rez_rl[lRaschet[T][poradok_rascheta[T][i]].numRL].rez[T].otp_iz_seti;
                        }
                        cash = lRaschet[T][poradok_rascheta[T][i]].kon_AO;
                    }
                   
                }
                             
                        if (otp > 0)
                            rez_rl[rez_rl.Count - 1].rez[T].otp_CP += otp;
                        else
                            rez_rl[rez_rl.Count - 1].rez[T].otp_iz_seti -= otp;

            for (int i = 0; i < rez_rl.Count - 1; i++)
            {
                rez_rl[rez_rl.Count - 1].rez[T].otp_bs += rez_rl[i].rez[T].otp_bs;
                rez_rl[rez_rl.Count - 1].rez[T].pol_otp += rez_rl[i].rez[T].pol_otp;
                rez_rl[rez_rl.Count - 1].rez[T].dWnagrLin += rez_rl[i].rez[T].dWnagrLin;
                rez_rl[rez_rl.Count - 1].rez[T].dWnagrTR += rez_rl[i].rez[T].dWnagrTR;
                rez_rl[rez_rl.Count - 1].rez[T].dWsum += rez_rl[i].rez[T].dWsum;
                rez_rl[rez_rl.Count - 1].rez[T].dWxxLin += rez_rl[i].rez[T].dWxxLin;
                rez_rl[rez_rl.Count - 1].rez[T].dWxxTR += rez_rl[i].rez[T].dWxxTR;
            }
            rez_rl[rez_rl.Count - 1].rez[T].otp_vsego = rez_rl[rez_rl.Count - 1].rez[T].otp_bs + rez_rl[rez_rl.Count - 1].rez[T].otp_CP;
        }
        public void in_result(float time, Form parrent)
        {
            int col_lin = 0, col_lin_v = 0, col_tr = 0, col_tr_v = 0, col_cp = 0, col_bs = 0;
            float sum_dlin =0, sum_dlin_v = 0, ust_m=0, ust_m_v = 0;

            for (int i = 0;i< BaseShema.Vet.Count; i++)
            {
                if (BaseShema.Vet[i].tipV == 0)
                {
                    col_lin++;
                    sum_dlin += BaseShema.Vet[i].dlina_moshnost;
                    if (lRaschet[0][i].ka != 0)
                    {
                        col_lin_v++;
                        sum_dlin_v += BaseShema.Vet[i].dlina_moshnost;
                    }
                }
                else
                {
                    col_tr++; ust_m += BaseShema.Vet[i].dlina_moshnost;
                    if (lRaschet[0][i].ka != 0)
                    { col_tr_v++; ust_m_v += BaseShema.Vet[i].dlina_moshnost; }
                }
            }
            for (int i=0;i<BaseShema.UZ.Count;i++)
                switch (BaseShema.UZ[i].tipUz)
                {
                    case 0:
                        col_cp++;
                        break;
                    case 1:
                        col_bs++;
                        break;
                }
            Rezultat rezultat = new Rezultat(col_lin,col_lin_v, sum_dlin,sum_dlin_v, col_tr,col_tr_v, ust_m,ust_m_v, col_cp, col_bs, time);
            rezultat.MdiParent = parrent;
            float o1 = 0, o2 = 0, o3 = 0,o4=0, p1 = 0,po=0,  p2 = 0,  p3 = 0,p4 = 0;
            for (int i = 0; i < rez_rl[rez_rl.Count-1].rez.Count; i++)
            {
                o1 += rez_rl[rez_rl.Count - 1].rez[i].otp_vsego;
                o2 += rez_rl[rez_rl.Count - 1].rez[i].otp_CP;
                o3 += rez_rl[rez_rl.Count - 1].rez[i].otp_bs;
                o4 += rez_rl[rez_rl.Count - 1].rez[i].otp_iz_seti;
                p1 += rez_rl[rez_rl.Count - 1].rez[i].dWsum;
                p2 += rez_rl[rez_rl.Count - 1].rez[i].dWxxLin + rez_rl[rez_rl.Count - 1].rez[i].dWnagrLin;
                p3 += rez_rl[rez_rl.Count - 1].rez[i].dWnagrTR;
                p4 += rez_rl[rez_rl.Count - 1].rez[i].dWxxTR;
                po += rez_rl[rez_rl.Count - 1].rez[i].pol_otp;
            }
            rezultat.set_rezult(o1, o2, o3,o4,po, p1, p2, p3, p4);
            for(int i=0;i<24;i++)
            {
                rezultat.add_diagr_2((rez_rl[rez_rl.Count - 1].rez[i].dWnagrLin + rez_rl[rez_rl.Count - 1].rez[i].dWnagrTR) / rez_rl[rez_rl.Count - 1].rez[i].otp_vsego * 100, 1);
                rezultat.add_diagr_2((rez_rl[rez_rl.Count - 1].rez[i].dWxxLin + rez_rl[rez_rl.Count - 1].rez[i].dWxxTR) / rez_rl[rez_rl.Count - 1].rez[i].otp_vsego * 100, 0);
            }
            for (int i = 0; i < rez_rl.Count; i++)
            {
                o1 = 0; p1 = 0; p2 = 0;
                for (int j = 0; j < 24; j++)
                {
                    o1 += rez_rl[i].rez[j].otp_vsego;                    
                    p1 += rez_rl[i].rez[j].dWxxLin +rez_rl[i].rez[j].dWxxTR ;
                    p2 += rez_rl[i].rez[j].dWnagrTR +rez_rl[i].rez[j].dWnagrLin;

                }
                if (o1 != 0)
                {
                    rezultat.add_diagr_1(p1 / o1 * 100, 0, rez_rl[i].nameRL);
                    rezultat.add_diagr_1(p2 / o1 * 100, 1, rez_rl[i].nameRL);
                }
            }
            rezultat.Text = "Результаты - " + BaseShema.path;
            rezultat.times.Text = "Расчет выполнен " + DateTime.Now.ToString();
            rezultat.Show();
            float z;
            for (int i = 0; i < rez_rl.Count - 1; i++)
            {
                z = 0;
                for (int j = 0; j < 24; j++)
                    z += rez_rl[i].rez[j].dWsum;
                //Console.WriteLine(rez_rl[i].nameRL + "  -   " + z.ToString());
            }
            
        }
    }
    public class set_AO
    {
        public int N;
        public int K;
        public bool razom;          // можно ли разомкнуть true - да , false - нет КА
        public float sopr;
        public set_AO(float sopr, int N, int K, bool raz)
        {
            this.N = N;
            this.K = K;
            this.sopr = sopr;
            this.razom = raz;
        }
    }
    public class set_AO_opt : set_AO
    {
        float Irab;
        public set_AO_opt(float sopr, int N, int K, bool raz, float i) : base(sopr,N,K,raz)
        {
            this.N = N;
            this.K = K;
            this.sopr = sopr;
            this.razom = raz;
            this.Irab = i;
        }
    }
[Serializable]
    public class L_T_Vetvi
    {
        public int numRL;
        public bool l_tr;       // true - линия, false - трансф-р
        public int nach_AO;
        public int kon_AO;
        public int ka;
        public float Unom;
        public float R;
        public float X;
        public float Pn;
        public float dP;
        public float Pk;
        public float Qn;
        public float dQ;
        public float Qk;
        public float Un;
        public float dU;
        public float Uk;
        public float dPxx;
        public float dQxx;
        public float ktr;
        public float I;
        public float Idop;
        public bool napravlenie;
        public int pred;
        public List<int> posle;
        public List<int> posle_razom;

        public L_T_Vetvi(int numrl, bool l_tr, int n, int k, int k1, int k2, float Unom, float r, float x, float Pn, float dP, float Pk, float Qn, float dQ, float Qk,
            float Un, float dU, float Uk, float dPxx, float dQxx, float ktr, float i, float idop, bool naprav, int pred,List<int> posl,List<int> posl_raz)
        {
            this.numRL = numrl;
            this.l_tr = l_tr;
            this.nach_AO = n;
            this.kon_AO = k;
            if (k1 * k2 == 0)
                ka = 0;     // есть КА и ветвь выключена
            else if (k1 + k2 >= 0)
                ka = 1;     // есть КА хотябы с одной стороны и ветвь включена
            else
                ka = -1;      // отсутствуют КА и ветвь включена
            this.Unom = Unom;
            this.R = r;
            this.X = x;
            this.Pn = Pn;
            this.dP = dP;
            this.Pk = Pk;
            this.Qn = Qn;
            this.dQ = dQ;
            this.Qk = Qk;
            this.Un = Un;
            this.dU = dU;
            this.Uk = Uk;
            this.dPxx = dPxx;
            this.dQxx = dQxx;
            this.ktr = ktr;
            this.I = i;
            this.Idop = idop;
            this.napravlenie = naprav;
            this.posle = posl;
            this.pred = pred;
            this.posle_razom = posl_raz;
        }
    }
    public class result
    {
        public float dWsum;
        public float dWxxTR;
        public float dWxxLin;
        public float dWnagrTR;
        public float dWnagrLin;
        public float otp_CP;
        public float otp_bs;
        public float otp_vsego;
        public float otp_iz_seti;
        public float pol_otp;
        public void clear()
        {
            dWsum = dWxxTR = dWxxLin = dWnagrTR = dWnagrLin = otp_CP = otp_bs = otp_vsego =pol_otp= otp_iz_seti=0;
        }
    }
    public class res_RL
    {
        public string nameRL;
        public List<result> rez;
        public void new_result()
        {
            this.rez = new List<result>();
        }
        public void add_result()
        {
            this.rez.Add(new result ());
        }
        public void Clear(int T)
        {
            this.rez[T].clear();
        }
    }
}
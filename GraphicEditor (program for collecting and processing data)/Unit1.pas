unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, FIBDatabase, pFIBDatabase, DB, FIBDataSet, pFIBDataSet, Grids,
  DBGridEh, ExtCtrls, StdCtrls, ComCtrls, FIBQuery, pFIBQuery, Spin,Math,
  {DBGridEhGrouping,} ToolCtrlsEh, Buttons{, DBGridEhToolCtrls, DynVarsEh,GridsEh,
  DBAxisGridsEh, System.Rtti, System.Bindings.Outputs, Vcl.Bind.Editors,
  Data.Bind.EngExt, Vcl.Bind.DBEngExt, Data.Bind.Components, Data.Bind.DBScope},
  OleServer, CmAdmCtl, AxCtrls, OleCtrls,ComObj ,ActiveX;

type
  TSezonTypes = (stWinter, stSpring, stSummer, stFall);
  TDaysTypes = (dtBUD,dtVIH);

const
  cSezonTypesDesc: array[TSezonTypes] of string = ('Зима', 'Весна', 'Лето', 'Осень');
  cDaysTypesDesc: array[TDaysTypes] of string = ('Рабочий день','Выходной день');
  ExcelApp = 'Excel.Application';

type
    TFormMain = class(TForm)
    DB: TpFIBDatabase;
    GrData: TpFIBDataSet;
    pFIBTransaction1: TpFIBTransaction;
    GrDataSource: TDataSource;
    GR: TpFIBDataSet;
    OBJECTS: TpFIBDataSet;
    pnConnect: TPanel;
    edDBPath: TEdit;
    Label1: TLabel;
    btOpenDBFile: TButton;
    btConnect: TButton;
    tv: TTreeView;
    insQuery: TpFIBQuery;
    OBJECTSID: TFIBIntegerField;
    OBJECTSNAME: TFIBStringField;
    GRID2: TFIBIntegerField;
    GROBJ_ID: TFIBIntegerField;
    GRSEZON_ID: TFIBIntegerField;
    GRDAY_ID: TFIBIntegerField;
    treeQuery: TpFIBQuery;
    OD: TOpenDialog;
    Panel2: TPanel;
    Grid: TDBGridEh;
    Panel3: TPanel;
    btNewGR: TButton;
    btNewObject: TButton;
    Splitter1: TSplitter;
    GrDataID: TFIBIntegerField;
    GrDataID_GR: TFIBIntegerField;
    GrDataP: TFIBFloatField;
    GrDataQ: TFIBFloatField;
    GrDataGTIME: TFIBTimeField;
    btClear: TButton;
    Button2: TButton;
    OBJECTSDESCR: TFIBStringField;
    OBJECTSOBJ_TYPE: TFIBIntegerField;
    eName: TEdit;
    Label2: TLabel;
    Label3: TLabel;
    eUst: TEdit;
    Label4: TLabel;
    eCos: TEdit;
    Label5: TLabel;
    eDescr: TEdit;
    OBJECTSU_NOM: TFIBIntegerField;
    OBJECTSVID_NAGR: TFIBIntegerField;
    strType: TLabel;
    Label7: TLabel;
    OBJECTSUST_MOSHN: TFIBFloatField;
    OBJECTSCOSFI: TFIBFloatField;
    OBJECTSID_PODTYPE_OBJ: TFIBIntegerField;
    OpenDialog1: TOpenDialog;
    Button1: TButton;
    Button3: TButton;
    Button4: TButton;
    Button5: TButton;
    edExcelPath: TEdit;
    Button6: TButton;
    edDB2Path: TEdit;
    Button7: TButton;
    Label6: TLabel;
    Label8: TLabel;
    procedure btNewObjectClick(Sender: TObject);
    procedure btNewGRClick(Sender: TObject);
    procedure btConnectClick(Sender: TObject);
    procedure FormClose(Sender: TObject; var Action: TCloseAction);
    procedure btOpenDBFileClick(Sender: TObject);
    procedure tvChange(Sender: TObject; Node: TTreeNode);
    procedure btClearClick(Sender: TObject);
    procedure Button2Click(Sender: TObject);
    procedure Button1Click(Sender: TObject);
    procedure Button3Click(Sender: TObject);
    procedure Button4Click(Sender: TObject);
    procedure Button5Click(Sender: TObject);

  private
    { Private declarations }
    fCurObj_ID: Integer;
    fCurGR_ID: Integer;

    procedure RefreshTree;
    procedure SelectNode(aLevel, aID: Integer);
    function AddGR(aObjID, aSezonID, aDayID: Integer): Integer; {new ID}
    procedure OpenGR_DATA(aGR_ID: Integer);
    procedure GenerateGR(aGR_ID: Integer; aTime: Integer; aClearBefore: Boolean = True);
    function GetLastTableID(const aTableName: string): Integer;
   // function Get_Int_bd(sel,table: String): Integer;
    function StrTran(str,  strfrom,  strto: String):String ;
    procedure update_info(aID:Integer);
  public

    { Public declarations }
  end;

var
  FormMain: TFormMain;
  MyExcel: OleVariant;

implementation

{$R *.dfm}

uses
  Unit2, Unit3, Unit4,GraphicList;

procedure TFormMain.btNewObjectClick(Sender: TObject);
var
  aName, aDescr: string;
  aUnom, aVidNagr, aId_ObjType,aId_PodObjType: Integer;
  aUst, aCos: double;
begin
  Form3.cbType.Items.Clear;
  Form3.cbPodType.Items.Clear;
  insQuery.Close;
  insQuery.SQL.Text := 'SELECT NAME FROM TYPE_OBJ ORDER BY ID asc';
  insQuery.ExecQuery;
  while not insQuery.Eof  do
  begin
    Form3.cbType.Items.Add(insQuery.Fields[0].AsString);
    insQuery.Next;
  end;
  Form3.cbType.Items.Add('Создать новую группу...');
  Form3.cbType.ItemIndex:=0;
  insQuery.Close;
  insQuery.SQL.Text := 'SELECT NAME FROM PODTYPE_OBJ WHERE ID_TYPE_OBJ=0 ORDER BY ID asc';
  insQuery.ExecQuery;
  while not insQuery.Eof  do
  begin
    Form3.cbPodType.Items.Add(insQuery.Fields[0].AsString);
    insQuery.Next;
  end;
  Form3.cbPodType.Items.Add('Создать новую подгруппу...');
  Form3.cbPodType.ItemIndex:=0;

  if Form3.ShowModal = mrOK then
  begin
     aName := Form3.eNewName.Text;
     aDescr := form3.eNewDescr.Text;
     if aName='' then
         raise Exception.Create('Имя не указано');
     try
       if DECIMALSEPARATOR='.' then
       begin
       Form3.eNewUst.Text:=StrTran(Form3.eNewUst.Text, ',', '.');
       Form3.eNewCos.Text:=StrTran(Form3.eNewCos.Text, ',', '.');
       end;
       aUst:= StrToFloat(Form3.eNewUst.Text);
       aCos:= StrToFloat(Form3.eNewCos.Text);
     except
        aUst:=0; aCos:=0;
        //('Неверно задана уст. мощность либо коэфф. мощности');
     end;


     aUnom:= Form3.cbUnom.ItemIndex;
     aVidNagr:=Form3.cbVidNagr.ItemIndex;
     aId_ObjType:=Form3.cbType.ItemIndex;
     insQuery.Close;
     insQuery.SQL.Text := 'SELECT ID FROM PODTYPE_OBJ WHERE ID_TYPE_OBJ='+ IntToStr(aId_ObjType) +' AND NAME =''' + Form3.cbPodType.Items[Form3.cbPodType.ItemIndex]+'''';
     insQuery.ExecQuery;
     aId_PodObjType:=insQuery.Fields[0].AsInteger;

    OBJECTS.Open;
    OBJECTS.Append;
    OBJECTSNAME.AsString := aName;
    OBJECTSDESCR.AsString := aDescr;
    OBJECTSU_NOM.AsInteger:= aUnom;
    OBJECTSVID_NAGR.AsInteger:= aVidNagr;
    OBJECTSOBJ_TYPE.AsInteger:= aId_ObjType;
    OBJECTSUST_MOSHN.AsFloat:= aUst;
    OBJECTSCOSFI.AsFloat:= aCos;
    OBJECTSID_PODTYPE_OBJ.AsInteger := aId_PodObjType;
    OBJECTS.Post;
    OBJECTS.Close;
    RefreshTree;
    SelectNode(0, GetLastTableID('OBJECTS'));

  end;
  {//aName := 'Новый объект';
  //if InputQuery('Новый объект', 'Введите имя объекта', aName) then
  begin
    if aName = '' then
      raise Exception.Create('Имя не указано');
    OBJECTS.Open;
    OBJECTS.Append;
    OBJECTSNAME.AsString := aName;
    OBJECTS.Post;
    OBJECTS.Close;
    RefreshTree;
    SelectNode(0, GetLastTableID('OBJECTS'));
  end;   }
end;

procedure TFormMain.RefreshTree;
var
  aPrevOBJ_ID: Integer;
  aPrevSEZON_ID: Integer;
//  aPrevPAR_DAY_ID: Integer;
 // aPrevDAY_ID: Integer;

  aNodeObj, aNodeSezon, aNodeDay: TTreeNode;
begin
  tv.Items.Clear;
  treeQuery.Close;
{
select objects.ID OBJ_ID, objects.name OBJ_NAME, objects.type OBJ_TYPE, objects.descr DESCR,
gr.sezon_id, gr.par_day_id , gr.day_id, gr.ID GR_ID from objects
left join gr on (objects.id = gr.obj_id)
order by objects.id, gr.sezon_id, gr.par_day_id, gr.day_id
}

  aPrevOBJ_ID := -1;
  aPrevSEZON_ID := -1;
//  aPrevDAY_ID := -1;

  if not pFIBTransaction1.Active then
    pFIBTransaction1.StartTransaction;

  treeQuery.ExecQuery;
  while not treeQuery.Eof do
  begin
    if aPrevOBJ_ID <> treeQuery.FieldByName['OBJ_ID'].AsInteger  then
    begin
      aNodeObj := tv.Items.AddChild(nil, treeQuery.FieldByName['OBJ_NAME'].AsString);
      aNodeObj.Data := Pointer(treeQuery.FieldByName['OBJ_ID'].AsInteger);

      aPrevOBJ_ID := treeQuery.FieldByName['OBJ_ID'].AsInteger;
      aPrevSEZON_ID := -1;
//      aPrevDAY_ID := -1;
    end;

    if not treeQuery.FieldByName['GR_ID'].IsNull then
    begin
      if aPrevSEZON_ID <> treeQuery.FieldByName['sezon_id'].AsInteger then
      begin
        aNodeSezon := tv.Items.AddChild(aNodeObj, cSezonTypesDesc[TSezonTypes(treeQuery.FieldByName['sezon_id'].AsInteger)]);
        aNodeSezon.Data := Pointer(treeQuery.FieldByName['sezon_id'].AsInteger);
        aPrevSEZON_ID := treeQuery.FieldByName['sezon_id'].AsInteger;
      //  aPrevDAY_ID := -1;
      end;

  {
      if aPrevDAY_ID <> treeQuery.FieldByName['day_id'].AsInteger then
      begin
        aNodeDay := tv.Items.AddChild(aNodeSezon, treeQuery.FieldByName['day_id'].AsString);
        aNodeDay.Data := Pointer(treeQuery.FieldByName['day_id'].AsInteger);

        aPrevDAY_ID := treeQuery.FieldByName['day_id'].AsInteger;
      end;
  }
      aNodeDay := tv.Items.AddChild(aNodeSezon, cDaysTypesDesc[TDaysTypes(treeQuery.FieldByName['DAY_ID'].AsInteger)]);
      aNodeDay.Data := Pointer(treeQuery.FieldByName['GR_ID'].AsInteger);
    end;

    treeQuery.Next;
  end;
end;

procedure TFormMain.btNewGRClick(Sender: TObject);
var
  aGrID: Integer;
  aSezon, aDay: Integer;

begin
  if GrParamsForm.ShowModal = mrOK then
  begin
    aSezon := GrParamsForm.cbSezon.ItemIndex;
    aDay := GrParamsForm.cbDay.ItemIndex;
    AddGR(fCurObj_ID, aSezon, aDay);

    aGrID := GetLastTableID('GR');

    if GrParamsForm.cbGenerate.Checked then
      GenerateGR(aGrID, GrParamsForm.edTime);

    RefreshTree;
    SelectNode(2, aGrID);
  end;
end;

function TFormMain.AddGR(aObjID, aSezonID, aDayID: Integer): Integer;
begin
  GR.Open;
  GR.Append;
  GROBJ_ID.Value := aObjID;
  GRSEZON_ID.Value := aSezonID;
  GRDAY_ID.Value := aDayID;
  GR.Post;
  GR.Close;
end;

function TFormMain.GetLastTableID(const aTableName: string): Integer;
begin
  Result := 0;
  insQuery.Close;
  insQuery.SQL.Text := 'SELECT MAX(ID) FROM ' + aTableName;
  insQuery.ExecQuery;
  if insQuery.RecordCount >0 then
    Result := insQuery.Fields[0].AsInteger;
  insQuery.Close;
end;

procedure TFormMain.SelectNode(aLevel, aID: Integer);
var
  i: Integer;

begin
  for i := 0 to tv.Items.Count - 1 do
    if tv.Items[i].Level  = aLevel then
      if Integer(tv.Items[i].Data) = aID then
      begin
        tv.Selected := tv.Items[i];
        tv.SetFocus;
        if aLevel=0 then
          update_info (aID);
        Break;
      end;
end;

procedure TFormMain.GenerateGR(aGR_ID, aTime: Integer; aClearBefore: Boolean);
var h: integer;
begin
    if aTime>0 then
    begin
    GrData.Close;
    GrData.Params[0].AsInteger := aGR_ID;
    GrData.Open;
    for  h:= 0 to 23 do
    begin
       GrData.Append;
       GrDataID_GR.Value := aGR_ID;
       GrDataGTIME.Value:=StrToTime(IntToStr(h)+':00');
       GrData.Post;
       if aTime=1 then
       begin
       GrData.Append;
       GrDataID_GR.Value := aGR_ID;
       GrDataGTIME.Value:=StrToTime(IntToStr(h)+':30');
       GrData.Post;
       end;
    end;
    GrData.Close;
    end;
end;

procedure TFormMain.btConnectClick(Sender: TObject);
var
  aPath: string;
begin
  DB.Close;

  aPath := edDBPath.Text;
  if Copy(aPath, 2, 2) <> ':\' then
    aPath := ExtractFilePath(ParamStr(0)) + aPath;

  DB.DBName := aPath;
  DB.Open;

  RefreshTree;
  pnConnect.Visible := False;
  btNewObject.Enabled :=true;
  Button1.Enabled := true;
  Button3.Enabled := true;
  Button4.Enabled := true;
end;

procedure TFormMain.FormClose(Sender: TObject; var Action: TCloseAction);
begin
  DB.Close;
end;

procedure TFormMain.btOpenDBFileClick(Sender: TObject);
begin
  if OD.Execute then
    edDBPath.Text := OD.FileName;
end;

procedure TFormMain.Button2Click(Sender: TObject);
var
   list : array of Integer;
   I: Integer;
begin
  insQuery.Close;
  insQuery.SQL.Text := 'DELETE FROM OBJECTS WHERE ID = :ID_OBJ';
  insQuery.Params[0].Value := fCurObj_ID;
  insQuery.ExecQuery;
  insQuery.Close;
  insQuery.SQL.Text := 'SELECT ID FROM GR WHERE OBJ_ID = :ID_OBJ';
  insQuery.Params[0].Value := fCurObj_ID;
  insQuery.ExecQuery;
  while not insQuery.Eof do
  begin
    SetLength(list,Length(list)+1);
    list[Length(list)-1] := insQuery.Fields[0].AsInteger;
    insQuery.Next;
  end;
  insQuery.Close;
  insQuery.SQL.Text := 'DELETE FROM GR WHERE OBJ_ID = :ID_OBJ';
  insQuery.Params[0].Value := fCurObj_ID;
  insQuery.ExecQuery;
  for I := 0 to Length(list)-1 do
  begin
  insQuery.Close;
  insQuery.SQL.Text := 'DELETE FROM GR_DATA WHERE ID_GR = :ID_GR';
  insQuery.Params[0].Value := list[I];
  insQuery.ExecQuery;
  end;
  SetLength(list,0);
  insQuery.Close;
  RefreshTree;
  fCurObj_ID:= (GetLastTableID('OBJECTS'));
  fCurGR_ID:=-1;
  insQuery.Close;
  insQuery.SQL.Text := 'SELECT ID FROM OBJECTS';
  insQuery.ExecQuery;
  if insQuery.RecordCount>0 then
      SelectNode(0,fCurObj_ID )
  else
    begin
        eName.Text:='';
        eCos.Text:='';
        eUst.Text:='';
        eDescr.Text:='';
        strType.Caption :='';
        btNewGR.Enabled := false;
        Button2.Enabled := false;
    end;
  GrData.Close;
end;

procedure TFormMain.tvChange(Sender: TObject; Node: TTreeNode);
begin
  fCurObj_ID := -1;
  fCurGR_ID := -1;

  if Node.Level = 0 then
    fCurObj_ID := Integer(Node.Data)
  else
    if Node.Level = 2 then
      fCurGR_ID := Integer(Node.Data);

  GR.Close;
  Grid.Visible := False;
  if fCurGR_ID >= 0 then
  begin
    OpenGR_DATA(fCurGR_ID);
    Grid.Visible := True;
  end;
  btNewGR.Enabled := fCurObj_ID >= 0;
  Button2.Enabled := fCurObj_ID >= 0;
  btClear.Enabled := fCurGR_ID >= 0;
  if (fCurObj_ID>=0) then
     update_info(fCurObj_ID);
end;


procedure TFormMain.OpenGR_DATA(aGR_ID: Integer);
begin
  GrData.Close;
  GrData.Params[0].AsInteger := aGR_ID;
  GrData.Open;
end;


procedure TFormMain.btClearClick(Sender: TObject);
begin
  insQuery.Close;
  insQuery.SQL.Text := 'DELETE FROM GR_DATA WHERE ID_GR = :ID_GR';
  insQuery.Params[0].Value := fCurGR_ID;
  insQuery.ExecQuery;
  insQuery.Close;
  OpenGR_DATA(fCurGR_ID);
end;

{function TFormMain.Get_Int_bd(sel,table: String): Integer;
begin
     insQuery.Close;
     insQuery.SQL.Text := 'SELECT '+sel+' FROM '+table;
     insQuery.ExecQuery;
     Result:= insQuery.Fields[0].AsInteger
end;  }

function TFormMain.StrTran(str, strfrom, strto: String): String;
var
  n: integer ;
  outstr, tempstr : string;
begin
  outstr:='' ;
  n:=1;
  While n <> (Length(str)+1) do
  begin
    tempstr:=Copy(str, n, Length(strfrom));
    if tempstr=strfrom then
    begin
      outstr:=outstr+strto;
      n:= n+Length(strfrom)-1;
    end
    else outstr:=outstr+Copy(str, n, 1);
    n:=n+1;
  end;
  Result:=outstr;
end;

procedure TFormMain.update_info(aID:Integer) ;
 var
    str: String;
    id_f,id_p: Integer;
 begin
          insQuery.Close;
          insQuery.SQL.Text := 'SELECT * FROM OBJECTS WHERE ID = '+ IntToStr(aID);
          insQuery.ExecQuery;
          eName.Text:= insQuery.Fields[1].AsString;
          eDescr.Text := insQuery.Fields[2].asString;
          if insQuery.Fields[4].AsInteger=0 then
          str:= ', 10кВ '
          else
          str:= ', 0,38кВ ';
          if insQuery.Fields[5].AsInteger=0 then
          str:=str + ' (трехфазная нагр.) '+#10#13
          else
          str:= str+ ' (однофазная нагр.) '+#10#13;
          if insQuery.Fields[6].AsFloat<>0 then
            eUst.Text := FloatToStr(RoundTo(insQuery.Fields[6].AsFloat, -3))
          else
            eUst.Text :='';
          if insQuery.Fields[7].AsFloat<>0 then
            eCos.Text := FloatToStr(roundto(insQuery.Fields[7].AsFloat, -3))
          else
            eCos.Text :='';
          id_f:=insQuery.Fields[3].AsInteger;
          id_p:=insQuery.Fields[8].AsInteger;
          insQuery.Close;
          insQuery.SQL.Text := 'SELECT NAME FROM TYPE_OBJ WHERE ID = '+ IntToStr(id_f);
          insQuery.ExecQuery;
          str:= insQuery.Fields[0].AsString + str;
          insQuery.Close;
          insQuery.SQL.Text := 'SELECT NAME FROM PODTYPE_OBJ WHERE ID = '+ IntToStr(id_p);
          insQuery.ExecQuery;
          strType.Caption:= str + insQuery.Fields[0].AsString ;
end;

function IsOLEObjectInstalled(Name: String): boolean;
var
  ClassID: TCLSID;
  Rez : HRESULT;
begin
  Rez := CLSIDFromProgID(PWideChar(WideString(Name)), ClassID);
if Rez = S_OK then
    Result := true
  else
    Result := false;
end;

procedure TFormMain.Button1Click(Sender: TObject);
var
 path, name, descr, sezon, den, val: String;
 i,j,gr,podgr,u_nom,vid_nagr, id_gr, col, gr_col: Integer;
 ust,cos:  Double;
// Log: String;
 VExcel, Workbook : Variant;
 StrGroup1,StrGroup2: array of string;
 StrPodGroup1,StrPodGroup2: array of string;

begin
       try
       //проверяем запущен ли Excel
            VExcel := CreateOleObject('Excel.Application');
       except
          on e:Exception do
          ShowMessage('Cannot start MS Excel.');
       end;
       if not IsOLEObjectInstalled('Excel.Application') then
          ShowMessage('Не установлен Excel')
       else
      begin
        gr:=-1;
        OpenDialog1.Filter := 'файл Excel (*.xls)|*.xls|файл Excel (*.xlsx)|*.xlsx';
        if OpenDialog1.Execute then
        begin
         i:=4;
         col:=0;
         gr_col:=0;
           path:=OpenDialog1.FileName;
           VExcel.DisplayAlerts := False;
           VExcel.Application.EnableEvents := false;
           Workbook := VExcel.WorkBooks.Open(path);
           try
           val:=Workbook.WorkSheets[1].Cells[i,10].Value;
           while val<>'' do
           begin
             name:=Workbook.WorkSheets[1].Cells[i,1].Value;
             if name <>'' then
             begin
                 // создание нового объекта
                 insQuery.Close;
                 insQuery.SQL.Text := 'SELECT ID FROM TYPE_OBJ WHERE NAME = '''+ Workbook.WorkSheets[1].Cells[i,2].Value+'''';
                 insQuery.ExecQuery;
                 if insQuery.RecordCount>0 then
                        gr:= insQuery.Fields[0].asInteger
                 else if Length(StrGroup1)>0 then
                 for j := 0 to Length(StrGroup1)-1 do
                 begin
                   if Workbook.WorkSheets[1].Cells[i,2].Value = StrGroup1[j]  then
                      begin
                       Workbook.WorkSheets[1].Cells[i,2].Value := StrGroup2[j];
                       insQuery.Close;
                       insQuery.SQL.Text := 'SELECT ID FROM TYPE_OBJ WHERE NAME = '''+ StrGroup2[j] +'''';
                       insQuery.ExecQuery;
                       gr:= insQuery.Fields[0].asInteger;
                       break;
                      end;
                   if j = (Length(StrGroup1)-1) then
                   begin
                    Form4.Label1.Caption := 'Не найдена группа объекта:'+Workbook.WorkSheets[1].Cells[i,2].Value +#13#10 + ' Выберите из существующей: ';
                    Form4.ComboBox1.Clear;
                    insQuery.Close;
                    insQuery.SQL.Text := 'SELECT NAME FROM TYPE_OBJ';
                    insQuery.ExecQuery;
                    while not insQuery.Eof  do
                        begin
                         Form4.ComboBox1.Items.Add(insQuery.Fields[0].AsString);
                         insQuery.Next;
                        end;
                    if Form4.ShowModal = mrOK then
                        begin
                        SetLength(StrGroup1,Length(StrGroup1)+1);
                        StrGroup1[Length(StrGroup1)-1]:=Workbook.WorkSheets[1].Cells[i,2].Value;
                        SetLength(StrGroup2,Length(StrGroup2)+1);
                        StrGroup2[Length(StrGroup2)-1]:= Form4.ComboBox1.Items[Form4.ComboBox1.ItemIndex];
                        Workbook.WorkSheets[1].Cells[i,2].Value := Form4.ComboBox1.Items[Form4.ComboBox1.ItemIndex];
                        insQuery.Close;
                        insQuery.SQL.Text := 'SELECT ID FROM TYPE_OBJ WHERE NAME = '''+ Form4.ComboBox1.Items[Form4.ComboBox1.ItemIndex]+'''';
                        insQuery.ExecQuery;
                         if insQuery.RecordCount>0 then
                            gr:= insQuery.Fields[0].asInteger
                         else
                            ShowMessage('Ошибка '+Workbook.WorkSheets[1].Cells[i,2].Value);
                        end ;
                   end ;
                  end
                  else
                    begin
                    Form4.Label1.Caption := 'Не найдена группа объекта:'+Workbook.WorkSheets[1].Cells[i,2].Value +#13#10 + ' Выберите из существующей: ';
                    Form4.ComboBox1.Clear;
                    insQuery.Close;
                    insQuery.SQL.Text := 'SELECT NAME FROM TYPE_OBJ';
                    insQuery.ExecQuery;
                    while not insQuery.Eof  do
                        begin
                         Form4.ComboBox1.Items.Add(insQuery.Fields[0].AsString);
                         insQuery.Next;
                        end;
                    if Form4.ShowModal = mrOK then
                        begin
                        SetLength(StrGroup1,Length(StrGroup1)+1);
                        StrGroup1[Length(StrGroup1)-1]:=Workbook.WorkSheets[1].Cells[i,2].Value;
                        SetLength(StrGroup2,Length(StrGroup2)+1);
                        StrGroup2[Length(StrGroup2)-1]:= Form4.ComboBox1.Items[Form4.ComboBox1.ItemIndex];
                        Workbook.WorkSheets[1].Cells[i,2].Value := Form4.ComboBox1.Items[Form4.ComboBox1.ItemIndex];
                        insQuery.Close;
                        insQuery.SQL.Text := 'SELECT ID FROM TYPE_OBJ WHERE NAME = '''+ Form4.ComboBox1.Items[Form4.ComboBox1.ItemIndex]+'''';
                        insQuery.ExecQuery;
                         if insQuery.RecordCount>0 then
                            gr:= insQuery.Fields[0].asInteger
                         else
                            ShowMessage('Ошибка '+Workbook.WorkSheets[1].Cells[i,2].Value);
                        end ;
                   end ;
                 insQuery.Close;
                 insQuery.SQL.Text := 'SELECT ID FROM PODTYPE_OBJ WHERE NAME = '''+
                 Workbook.WorkSheets[1].Cells[i,3].Value+ ''' AND ID_TYPE_OBJ='+IntToStr(gr);
                 insQuery.ExecQuery;
                 if insQuery.RecordCount>0 then
                     podgr:= insQuery.Fields[0].asInteger
                 else if Length(StrPodGroup1)>0  then
                 for j := 0 to Length(StrPodGroup1)-1 do
                 begin
                   if Workbook.WorkSheets[1].Cells[i,3].Value = StrPodGroup1[j]  then
                      begin
                       Workbook.WorkSheets[1].Cells[i,3].Value := StrPodGroup2[j];
                       insQuery.Close;
                       insQuery.SQL.Text := 'SELECT ID FROM TYPE_OBJ WHERE NAME = '''+ StrPodGroup2[j] +'''';
                       insQuery.ExecQuery;
                       podgr:= insQuery.Fields[0].asInteger;
                       break;
                      end;
                   if j = (Length(StrPodGroup1)-1) then
                   begin
                    Form4.Label1.Caption := 'Не найдена подгруппа объекта:'+Workbook.WorkSheets[1].Cells[i,3].Value +#13#10 + ' Выберите из существующей: ';
                    Form4.ComboBox1.Clear;
                    insQuery.Close;
                    insQuery.SQL.Text := 'SELECT NAME FROM PODTYPE_OBJ WHERE ID_TYPE_OBJ='+IntToStr(gr);
                    insQuery.ExecQuery;
                    while not insQuery.Eof  do
                        begin
                         Form4.ComboBox1.Items.Add(insQuery.Fields[0].AsString);
                         insQuery.Next;
                        end;
                    if Form4.ShowModal = mrOK then
                        begin
                        SetLength(StrPodGroup1,Length(StrPodGroup1)+1);
                        StrPodGroup1[Length(StrPodGroup1)-1]:=Workbook.WorkSheets[1].Cells[i,3].Value;
                        SetLength(StrPodGroup2,Length(StrPodGroup2)+1);
                        StrPodGroup2[Length(StrPodGroup2)-1]:= Form4.ComboBox1.Items[Form4.ComboBox1.ItemIndex];
                        Workbook.WorkSheets[1].Cells[i,3].Value := Form4.ComboBox1.Items[Form4.ComboBox1.ItemIndex];
                        insQuery.Close;
                        insQuery.SQL.Text := 'SELECT ID FROM PODTYPE_OBJ WHERE NAME = '''+ Form4.ComboBox1.Items[Form4.ComboBox1.ItemIndex]+
                        ''' AND ID_TYPE_OBJ='+IntToStr(gr);
                        insQuery.ExecQuery;
                         if insQuery.RecordCount>0 then
                            podgr:= insQuery.Fields[0].asInteger
                         else
                            ShowMessage('Ошибка '+Workbook.WorkSheets[1].Cells[i,3].Value);
                        end;
                  end;
                 end
                 else
                   begin
                    Form4.Label1.Caption := 'Не найдена подгруппа объекта:'+Workbook.WorkSheets[1].Cells[i,3].Value +#13#10 + ' Выберите из существующей: ';
                    Form4.ComboBox1.Clear;
                    insQuery.Close;
                    insQuery.SQL.Text := 'SELECT NAME FROM PODTYPE_OBJ WHERE ID_TYPE_OBJ='+IntToStr(gr);
                    insQuery.ExecQuery;
                    while not insQuery.Eof  do
                        begin
                         Form4.ComboBox1.Items.Add(insQuery.Fields[0].AsString);
                         insQuery.Next;
                        end;
                    if Form4.ShowModal = mrOK then
                        begin
                        SetLength(StrPodGroup1,Length(StrPodGroup1)+1);
                        StrPodGroup1[Length(StrPodGroup1)-1]:=Workbook.WorkSheets[1].Cells[i,3].Value;
                        SetLength(StrPodGroup2,Length(StrPodGroup2)+1);
                        StrPodGroup2[Length(StrPodGroup2)-1]:= Form4.ComboBox1.Items[Form4.ComboBox1.ItemIndex];
                        Workbook.WorkSheets[1].Cells[i,3].Value := Form4.ComboBox1.Items[Form4.ComboBox1.ItemIndex];
                        insQuery.Close;
                        insQuery.SQL.Text := 'SELECT ID FROM PODTYPE_OBJ WHERE NAME = '''+ Form4.ComboBox1.Items[Form4.ComboBox1.ItemIndex]+
                        ''' AND ID_TYPE_OBJ='+IntToStr(gr);
                        insQuery.ExecQuery;
                         if insQuery.RecordCount>0 then
                            podgr:= insQuery.Fields[0].asInteger
                         else
                            ShowMessage('Ошибка '+Workbook.WorkSheets[1].Cells[i,3].Value);
                        end;
                  end;
                 ust:= StrToFloat(Workbook.WorkSheets[1].Cells[i,4].Value);
                 If StrToFloat(Workbook.WorkSheets[1].Cells[i,5].Value) = 10 then
                     u_nom:=0
                   else
                     u_nom :=1;
                 If StrToInt(Workbook.WorkSheets[1].Cells[i,6].Value) = 3 then
                   vid_nagr:=0
                 else
                   vid_nagr :=1;
                 den:=Workbook.WorkSheets[1].Cells[i,7].Value;
                 If den <>'' then
                   cos:= StrToFloat(den)
                 else
                   cos :=0;
                 if Workbook.WorkSheets[1].Cells[i,8].Value<>'' then
                  descr:= name + ': '+ Workbook.WorkSheets[1].Cells[i,8].Value
                 else
                  descr:= name;
                 if Length(name)>30 then
                   begin
                   name := Copy(name,0,30);
                   end;
                OBJECTS.Open;
                OBJECTS.Append;
                OBJECTSNAME.AsString := name;
                OBJECTSDESCR.AsString := descr;
                OBJECTSU_NOM.AsInteger:= u_nom;
                OBJECTSVID_NAGR.AsInteger:= vid_nagr;
                OBJECTSOBJ_TYPE.AsInteger:= gr;
                OBJECTSUST_MOSHN.AsFloat:= ust;
                OBJECTSCOSFI.AsFloat:= cos;
                OBJECTSID_PODTYPE_OBJ.AsInteger := podgr;
                OBJECTS.Post;
                OBJECTS.Close;
                col:=col+1;
             end;
             sezon :=  Copy(Workbook.WorkSheets[1].Cells[i,9].Value,0,1);
             den   :=  Copy(Workbook.WorkSheets[1].Cells[i,10].Value ,0,1);
             if sezon = 'л' then                                                           //(Зима, 'Весна', 'Лето', 'Осень')
                  begin
                  if den = 'р' then
                  AddGR(GetLastTableID('OBJECTS'), 2, 0)
                  else if den = 'в' then
                  AddGR(GetLastTableID('OBJECTS'), 2, 1)
                  else
                   ShowMessage('Ошибка: '+sezon+'-'+Workbook.WorkSheets[1].Cells[i,10].Value);
                  end
             else if sezon = 'з' then
                  begin
                  if den = 'р' then
                  AddGR(GetLastTableID('OBJECTS'), 0, 0)
                  else if den = 'в' then
                  AddGR(GetLastTableID('OBJECTS'), 0, 1)
                  else
                   ShowMessage('Ошибка: '+sezon+'-'+Workbook.WorkSheets[1].Cells[i,10].Value);
                  end
             else if sezon = 'о' then
                  begin
                  if den = 'р' then
                  AddGR(GetLastTableID('OBJECTS'), 3, 0)
                  else if den = 'в' then
                  AddGR(GetLastTableID('OBJECTS'), 3, 1)
                  else
                   ShowMessage('Ошибка: '+sezon+'-'+Workbook.WorkSheets[1].Cells[i,10].Value);
                  end
             else if sezon = 'в' then
                  begin
                  if den = 'р' then
                  AddGR(GetLastTableID('OBJECTS'), 1, 0)
                  else if den = 'в' then
                  AddGR(GetLastTableID('OBJECTS'), 1, 1)
                  else
                   ShowMessage('Ошибка: '+sezon+'-'+Workbook.WorkSheets[1].Cells[i,10].Value);
                  end
             else
                 ShowMessage('Ошибка: '+sezon+'-'+Workbook.WorkSheets[1].Cells[i,9].Value);
             id_gr:=GetLastTableID('GR');
             GrData.Open;
             for j:=11 To 58 do
             begin
                GrData.Append;
                GrDataID_GR.Value := id_gr;
                GrDataGTIME.Value:=StrToTime(Workbook.WorkSheets[1].Cells[3,j].Value);
                val:=Workbook.WorkSheets[1].Cells[i,j].Value;
                GrDataP.Value :=StrToFloat(val);
                GrData.Post;
             end;
              GrData.Close;
             i:=i+1;
             val:=Workbook.WorkSheets[1].Cells[i,10].Value;
             gr_col:=gr_col+1;
           end;
          finally
           SetLength(StrPodGroup1,0);
           SetLength(StrPodGroup2,0);
           SetLength(StrGroup1,0);
           SetLength(StrGroup2,0);

           VExcel.DisplayAlerts := true;
           VExcel.Application.EnableEvents := true;
           VExcel.ActiveWorkbook.SaveAs(path);
           VExcel.ActiveWorkbook.Close;
           VExcel.Application.Quit;
           ShowMessage('Загружено: '+#13#10+IntToStr(col)+' объектов' +#13#10+IntToStr(gr_col)+' графиков');

          end;
        end;
     end;
end;

procedure TFormMain.Button3Click(Sender: TObject);
var
        list : Array of Integer;
        i,j, col: Integer;
        n,k: Double;
begin
        col:=0;
        insQuery.Close;
        insQuery.SQL.Text := 'SELECT * FROM GR';
        insQuery.ExecQuery;
        while not insQuery.Eof  do
        begin
                col:=col+1;
                SetLength(list,col);
                list[Length(list)-1]:= insQuery.Fields[0].asInteger;
                insQuery.Next;
        end;
        try
        col:=0;
        for i:=0 to Length(list)-1 do
        begin
            // заполнение получасовых пробелов в данных
            For j:=0 to 23 do
            begin
            insQuery.Close;
            insQuery.SQL.Text := 'SELECT * FROM GR_DATA WHERE ID_GR=' + IntToStr(list[i])+' AND GTIME=''' + IntToStr(j)+':30:00''';
            insQuery.ExecQuery;
            if insQuery.RecordCount=0 then
             begin
             insQuery.Close;
             insQuery.SQL.Text := 'SELECT P FROM GR_DATA WHERE ID_GR=' + IntToStr(list[i])+' AND GTIME=''' + IntToStr(j)+':00:00''';
             insQuery.ExecQuery;
             n:=insQuery.Fields[0].asFloat;
             insQuery.Close;
             if j<23 then
               insQuery.SQL.Text := 'SELECT P FROM GR_DATA WHERE ID_GR=' + IntToStr(list[i])+' AND GTIME=''' + IntToStr(j+1)+':00:00'''
             else
               insQuery.SQL.Text := 'SELECT P FROM GR_DATA WHERE ID_GR=' + IntToStr(list[i])+' AND GTIME=''00:00:00''';
             insQuery.ExecQuery;
             k:=insQuery.Fields[0].asFloat;
             GrData.Open;
             GrData.Append;
             GrDataID_GR.Value := list[i];
             GrDataP.Value:=n+(k-n)/2;
             GrDataGTIME.Value:=StrToTime(IntToStr(j)+':30');
             GrData.Post;
             GrData.Close;
             col:=col+1;
             end;
            end;
            // проверка на непревышение установленной мощности

            
        end;         
        finally
        insQuery.Close;
        SetLength(list,0);
        ShowMessage('Завершено! добавлено: '+#13#10+IntToStr(col)+'  промежуточных значений');
        end;
end;



function CheckExcelInstall: boolean;
var
  ClassID: TCLSID;
  Rez : HRESULT;
begin
// ???? CLSID OLE-???????
  Rez := CLSIDFromProgID(PWideChar(WideString(ExcelApp)), ClassID);
  if Rez = S_OK then  // ?????? ??????
    Result := true
  else
    Result := false;
end;

function RunExcel(DisableAlerts:boolean=true; Visible: boolean=false): boolean;
begin
  try
{????????? ?????????? ?? Excel}
    if CheckExcelInstall then
      begin
        MyExcel:=CreateOleObject(ExcelApp);
//??????????/?? ?????????? ????????? ????????? Excel (????? ?? ??????????)
        MyExcel.Application.EnableEvents:=DisableAlerts;
        MyExcel.Visible:=Visible;
        Result:=true;
      end
    else
      begin
        MessageBox(0,'?????????? MS Excel ?? ??????????? ?? ???? ??????????','??????',MB_OK+MB_ICONERROR);
        Result:=false;
      end;
  except
    Result:=false;
  end;
end;

function CheckExcelRun: boolean;
begin
  try
    MyExcel:=GetActiveOleObject(ExcelApp);
    Result:=True;
  except
    Result:=false;
  end;
end;

function AddWorkBook(AutoRun:boolean=true):boolean;
begin
  if CheckExcelRun then
    begin
      MyExcel.WorkBooks.Add;
      Result:=true;
    end
  else
   if AutoRun then
     begin
       RunExcel;
       MyExcel.WorkBooks.Add;
       Result:=true;
     end
   else
     Result:=false;
end;

function SaveWorkBook(FileName:TFileName; WBIndex:integer):boolean;
begin
  try
    MyExcel.WorkBooks.Item[WBIndex].SaveAs(FileName);
    if MyExcel.WorkBooks.Item[WBIndex].Saved then
      Result:=true
    else
      Result:=false;
  except
    Result:=false;
  end;
end;

function StopExcel:boolean;
begin
  try
    if MyExcel.Visible then MyExcel.Visible:=false;
    MyExcel.Quit;
    MyExcel:=Unassigned;
    Result:=True;
  except
    Result:=false;
  end;
end;

procedure TFormMain.Button4Click(Sender: TObject);
type
DynFloatArray = GraphicList.DynFloatArray;

procedure Read_Gr(id_object: Integer; var sum_rab, sum_vih,zim_rab, zim_vih,
                 ves_rab, ves_vih, os_rab, os_vih: DynFloatArray);
var
 id,q : Integer;
begin
             insQuery.Close;
             insQuery.SQL.Text := 'SELECT * FROM GR WHERE OBJ_ID=' + IntToStr(id_object)+
             ' AND SEZON_ID=2 AND DAY_ID=0';
             insQuery.ExecQuery;
             if insQuery.RecordCount>0 then
             begin
              id:=insQuery.Fields[0].asInteger;
              insQuery.Close;
              insQuery.SQL.Text := 'SELECT * FROM GR_DATA WHERE ID_GR=' + IntToStr(id)+
              ' ORDER BY GTIME';
              insQuery.ExecQuery;
               For q:=0 to 47 do
                begin
                 sum_rab[q]:=insQuery.Fields[2].asDouble;
                 try
                 insQuery.Next;
                 except
                   MessageBox(0,'Неполный график!','Ошибка',MB_OK+MB_ICONERROR);
                 end;
                end;
             end
             else
              sum_rab[0]:=-1;

             insQuery.Close;
             insQuery.SQL.Text := 'SELECT * FROM GR WHERE OBJ_ID=' + IntToStr(id_object)+
             ' AND SEZON_ID=2 AND DAY_ID=1';
             insQuery.ExecQuery;
             if insQuery.RecordCount>0 then
             begin
             id:=insQuery.Fields[0].asInteger;
             insQuery.Close;
             insQuery.SQL.Text := 'SELECT * FROM GR_DATA WHERE ID_GR=' + IntToStr(id)+
             ' ORDER BY GTIME';
             insQuery.ExecQuery;
              For q:=0 to 47 do
              begin
                sum_vih[q]:=insQuery.Fields[2].asDouble;
                try
                insQuery.Next;
                except
                  MessageBox(0,'Неполный график!','Ошибка',MB_OK+MB_ICONERROR);
                end;
              end;
             end
             else
              sum_vih[0]:=-1;

             insQuery.Close;
             insQuery.SQL.Text := 'SELECT * FROM GR WHERE OBJ_ID=' + IntToStr(id_object)+
             ' AND SEZON_ID=0 AND DAY_ID=0';
             insQuery.ExecQuery;
             if insQuery.RecordCount>0 then
             begin
             id:=insQuery.Fields[0].asInteger;
             insQuery.Close;
             insQuery.SQL.Text := 'SELECT * FROM GR_DATA WHERE ID_GR=' + IntToStr(id)+
             ' ORDER BY GTIME';
             insQuery.ExecQuery;
              For q:=0 to 47 do
              begin
                zim_rab[q]:=insQuery.Fields[2].asDouble;
                try
                insQuery.Next;
                except
                  MessageBox(0,'Неполный график!','Ошибка',MB_OK+MB_ICONERROR);
                end;
              end;
             end
             else
              zim_rab[0]:=-1;

             insQuery.Close;
             insQuery.SQL.Text := 'SELECT * FROM GR WHERE OBJ_ID=' + IntToStr(id_object)+
             ' AND SEZON_ID=0 AND DAY_ID=1';
             insQuery.ExecQuery;
             if insQuery.RecordCount>0 then
             begin
             id:=insQuery.Fields[0].asInteger;
             insQuery.Close;
             insQuery.SQL.Text := 'SELECT * FROM GR_DATA WHERE ID_GR=' + IntToStr(id)+
             ' ORDER BY GTIME';
             insQuery.ExecQuery;
              For q:=0 to 47 do
              begin
                zim_vih[q]:=insQuery.Fields[2].asDouble;
                try
                insQuery.Next;
                except
                  MessageBox(0,'Неполный график!','Ошибка',MB_OK+MB_ICONERROR);
                end;
              end;
             end
             else
              zim_vih[0]:=-1;

             insQuery.Close;
             insQuery.SQL.Text := 'SELECT * FROM GR WHERE OBJ_ID=' + IntToStr(id_object)+
             ' AND SEZON_ID=1 AND DAY_ID=0';
             insQuery.ExecQuery;
             if insQuery.RecordCount>0 then
             begin
             id:=insQuery.Fields[0].asInteger;
             insQuery.Close;
             insQuery.SQL.Text := 'SELECT * FROM GR_DATA WHERE ID_GR=' + IntToStr(id)+
             ' ORDER BY GTIME';
             insQuery.ExecQuery;
              For q:=0 to 47 do
              begin
                ves_rab[q]:=insQuery.Fields[2].asDouble;
                try
                insQuery.Next;
                except
                  MessageBox(0,'Неполный график!','Ошибка',MB_OK+MB_ICONERROR);
                end;
              end;
             end
             else
              ves_rab[0]:=-1;

             insQuery.Close;
             insQuery.SQL.Text := 'SELECT * FROM GR WHERE OBJ_ID=' + IntToStr(id_object)+
             ' AND SEZON_ID=1 AND DAY_ID=1';
             insQuery.ExecQuery;
             if insQuery.RecordCount>0 then
             begin
             id:=insQuery.Fields[0].asInteger;
             insQuery.Close;
             insQuery.SQL.Text := 'SELECT * FROM GR_DATA WHERE ID_GR=' + IntToStr(id)+
             ' ORDER BY GTIME';
             insQuery.ExecQuery;
              For q:=0 to 47 do
              begin
                ves_vih[q]:=insQuery.Fields[2].asDouble;
                try
                insQuery.Next;
                except
                  MessageBox(0,'Неполный график!','Ошибка',MB_OK+MB_ICONERROR);
                end;
              end;
             end
             else
              ves_vih[0]:=-1;

             insQuery.Close;
             insQuery.SQL.Text := 'SELECT * FROM GR WHERE OBJ_ID=' + IntToStr(id_object)+
             ' AND SEZON_ID=3 AND DAY_ID=0';
             insQuery.ExecQuery;
             if insQuery.RecordCount>0 then
             begin
             id:=insQuery.Fields[0].asInteger;
             insQuery.Close;
             insQuery.SQL.Text := 'SELECT * FROM GR_DATA WHERE ID_GR=' + IntToStr(id)+
             ' ORDER BY GTIME';
             insQuery.ExecQuery;
              For q:=0 to 47 do
              begin
                os_rab[q]:=insQuery.Fields[2].asDouble;
                try
                insQuery.Next;
                except
                  MessageBox(0,'Неполный график!','Ошибка',MB_OK+MB_ICONERROR);
                end;
              end;
             end
             else
              os_rab[0]:=-1;

             insQuery.Close;
             insQuery.SQL.Text := 'SELECT * FROM GR WHERE OBJ_ID=' + IntToStr(id_object)+
             ' AND SEZON_ID=3 AND DAY_ID=1';
             insQuery.ExecQuery;
             if insQuery.RecordCount>0 then
             begin
             id:=insQuery.Fields[0].asInteger;
             insQuery.Close;
             insQuery.SQL.Text := 'SELECT * FROM GR_DATA WHERE ID_GR=' + IntToStr(id)+
             ' ORDER BY GTIME';
             insQuery.ExecQuery;
              For q:=0 to 47 do
              begin
                os_vih[q]:=insQuery.Fields[2].asDouble;
                try
                insQuery.Next;
                except
                  MessageBox(0,'Неполный график!','Ошибка',MB_OK+MB_ICONERROR);
                end;
              end;
             end
             else
              os_vih[0]:=-1;
end;
type
    Tobj = record
    id_obj: Integer; // индекс объекта
    Del: Boolean; //    установленная мощность
    end;
var
        list_Type,list_PodType : Array of Integer;
        list_obj: Array of Tobj;
        i,j,q,col,col_obj, prov, row_Excel: Integer;
        n,k: Integer;
        DynList: TGraphicList;
        id_obj: Integer; // индекс объекта
        ust_m: Double; //    установленная мощность
        vid_nagr,u_nom: Integer;
        sum_rab: DynFloatArray;
        sum_vih: DynFloatArray;
        zim_rab: DynFloatArray;
        zim_vih: DynFloatArray;
        ves_rab: DynFloatArray;
        ves_vih: DynFloatArray;
        os_rab: DynFloatArray;
        os_vih: DynFloatArray;
        descr: String;
begin
     AddWorkBook;
     MyExcel.Visible:=true;
     row_Excel:=1;
     col_obj:=0;
     prov:=0;
     insQuery.Close;
     insQuery.SQL.Text := 'SELECT * FROM TYPE_OBJ';
     insQuery.ExecQuery;
     col:=0;
     while not insQuery.Eof  do
        begin
                col:=col+1;
                SetLength(list_Type,col);
                list_Type[col-1]:= insQuery.Fields[0].asInteger;
                insQuery.Next;
        end;
     For i:=0 to Length(list_Type)-1 do
     begin
       insQuery.Close;
       insQuery.SQL.Text := 'SELECT * FROM PODTYPE_OBJ WHERE ID_TYPE_OBJ=' + IntToStr(list_Type[i]);
       insQuery.ExecQuery;
       col:=0;
       while not insQuery.Eof  do
        begin
                col:=col+1;
                SetLength(list_PodType,col);
                list_PodType[col-1]:= insQuery.Fields[0].asInteger;
                insQuery.Next;
        end;
       For j:=0 to Length(list_PodType)-1 do
       begin
         insQuery.Close;
         insQuery.SQL.Text := 'SELECT * FROM OBJECTS WHERE OBJ_TYPE=' + IntToStr(list_Type[i]) +
         ' AND ID_PODTYPE_OBJ=' +  IntToStr(list_PodType[j]);
         insQuery.ExecQuery;
         col:=0;
         while not insQuery.Eof  do
           begin
                col:=col+1;
                SetLength(list_obj,col);
                list_obj[col-1].id_obj := insQuery.Fields[0].asInteger;
                list_obj[col-1].Del :=False;
                insQuery.Next;
           end;
         prov:=prov+Length(list_obj);
         while Length(list_obj)<>0 do
         begin
            try
             insQuery.Close;
             insQuery.SQL.Text := 'SELECT * FROM OBJECTS WHERE ID=' + IntToStr(list_obj[0].id_obj);
             insQuery.ExecQuery;
             id_obj:=list_obj[0].id_obj;
             u_nom:=insQuery.Fields[4].asInteger;
             vid_nagr:= insQuery.Fields[5].asInteger;
             ust_m:=insQuery.Fields[6].asDouble;
             descr :=insQuery.Fields[2].asString;
             Read_Gr(id_obj,sum_rab,sum_vih,zim_rab, zim_vih, ves_rab, ves_vih, os_rab, os_vih);
             DynList:=TGraphicList.Create;
             DynList.New_Obj(id_obj,ust_m,sum_rab,sum_vih,zim_rab, zim_vih,
                 ves_rab, ves_vih, os_rab, os_vih, descr);
             col_obj:=col_obj+1;
             list_obj[0].id_obj :=list_obj[High(list_obj)].id_obj ;
             list_obj[0].Del :=list_obj[High(list_obj)].Del ;
             SetLength(list_obj, Length(list_obj)-1);
             For q:=0 to High(list_obj) do
             begin
               insQuery.Close;
               insQuery.SQL.Text := 'SELECT * FROM OBJECTS WHERE ID=' + IntToStr(list_obj[q].id_obj );
               insQuery.ExecQuery;
               if (vid_nagr=insQuery.Fields[5].asInteger) and (u_nom=insQuery.Fields[4].asInteger) then
               begin
                 col_obj:=col_obj+1;
                 ust_m:=insQuery.Fields[6].asDouble;
                 descr :=insQuery.Fields[2].asString;
                 Read_Gr(list_obj[q].id_obj ,sum_rab,sum_vih,zim_rab, zim_vih, ves_rab, ves_vih, os_rab, os_vih);
                 DynList.New_Obj(list_obj[q].id_obj ,ust_m,sum_rab,sum_vih,zim_rab, zim_vih,
                 ves_rab, ves_vih, os_rab, os_vih,descr);
                 list_obj[q].Del := true;
               end;
             end;
             n:=0;
             k:= High(list_obj);
             while k>=n do
             begin
             if list_obj[n].Del then
              begin
              if k<>n then
               begin
               list_obj[n].id_obj := list_obj[High(list_obj)].id_obj;
               list_obj[n].Del  := list_obj[High(list_obj)].Del;
               end;
              SetLength(list_obj, Length(list_obj)-1);
              k:=k-1;
              end
             else
              n:=n+1;
             end;
             // обработка графиков однотипных потребителей в списке и Вывод в эксель графиков
             DynList.Poluchit_Tip_Gr(MyExcel,row_Excel, list_Type[i] ,list_PodType[j]);





             finally
             // очистка памяти
             DynList.Destroy;
          // StopExcel;
             end;


         end;
       end;
     end;
     MyExcel.Visible:=true;
     ShowMessage('Завершено!'+#10#13+IntToStr(col_obj) +'='+IntTostr(prov)+ ' объектов');

end;

procedure TFormMain.Button5Click(Sender: TObject);
type
    gr_potr = record
    id_gr: Integer;
    Name : String;
    end;
    podgr_potr = record
    id_podgr: Integer;
    id_gr: Integer;
    Name : String;
    end;
var
    i,j,col,gr_id,podgr_id,row_Excel,last_id : Integer;
    list_Type: Array of gr_potr;
    list_PodType : array of podgr_potr;
    aPath, str, rab_gr, vih_gr,g_time: String;
    Range, Cell1, Cell2, ArrayData : Variant;
    prov: Boolean;
    gr_values : Array[0..47] of Double;

begin
     col:=0;
     insQuery.Close;
     insQuery.SQL.Text := 'SELECT * FROM TYPE_OBJ';
     insQuery.ExecQuery;
     while not insQuery.Eof  do
        begin
                col:=col+1;
                SetLength(list_Type,col);
                list_Type[col-1].id_gr:= insQuery.Fields[0].asInteger;
                list_Type[col-1].Name := insQuery.Fields[1].asString;
                insQuery.Next;
        end;
     insQuery.Close;
     insQuery.SQL.Text := 'SELECT * FROM PODTYPE_OBJ';
     insQuery.ExecQuery;
     col:=0;
     while not insQuery.Eof  do
        begin
                col:=col+1;
                SetLength(list_PodType,col);
                list_PodType[col-1].id_podgr:= insQuery.Fields[0].asInteger;
                list_PodType[col-1].id_gr:= insQuery.Fields[1].asInteger;
                list_PodType[col-1].Name := insQuery.Fields[2].asString;
                insQuery.Next;
        end;
     try
       DB.Close;
       aPath := edDB2Path.Text;
       if Copy(aPath, 2, 2) <> ':\' then
          aPath := ExtractFilePath(ParamStr(0)) + aPath;
       DB.DBName := aPath;
       DB.Open;
       tv.Items.Clear;
       pnConnect.Visible := True;
       btNewObject.Enabled :=false;
       Button1.Enabled := false;
       Button3.Enabled := false;
       Button4.Enabled := false;
       insQuery.Close;
       insQuery.SQL.Text := 'DELETE FROM CAT_GR_OBJECTS';
       insQuery.ExecQuery;
       insQuery.Close;
       insQuery.SQL.Text := 'DELETE FROM  CAT_GR_POTREB';
       insQuery.ExecQuery;
       insQuery.Close;
       insQuery.SQL.Text := 'DELETE FROM  CAT_GR_VALUES';
       insQuery.ExecQuery;
       insQuery.Close;
       insQuery.SQL.Text := 'DELETE FROM  CAT_PODGR_POTREB';
       insQuery.ExecQuery;
       For i:=0 to High(list_Type) do
       begin
        insQuery.Close;
        insQuery.SQL.Text := 'INSERT INTO CAT_GR_POTREB (ID, NAME) values('+
        IntToStr(list_Type[i].id_gr) + ',''' + list_Type[i].Name + ''')';
        insQuery.ExecQuery;
       end;
       For i:=0 to High(list_PodType) do
       begin
        insQuery.Close;
        insQuery.SQL.Text := 'INSERT INTO CAT_PODGR_POTREB (ID, ID_GR_POTREB, NAME) values('+
        IntToStr(list_PodType[i].id_podgr) +', '+ IntToStr(list_PodType[i].id_gr) + ', ''' +
         list_PodType[i].Name + ''')';
        insQuery.ExecQuery;
       end;

  try
    MyExcel := GetActiveOleObject(ExcelApp);
  except
    on EOLESysError do
     MyExcel:=CreateOleObject(ExcelApp);
  end;
  aPath := edExcelPath.Text;
  if Copy(aPath, 2, 2) <> ':\' then
          aPath := ExtractFilePath(ParamStr(0)) + aPath;
    MyExcel.DisplayAlerts := False;
    MyExcel.WorkBooks.Open(aPath);
    MyExcel.WorkSheets[1].Activate;

  row_Excel:=1;
  str:= MyExcel.WorkSheets[1].Cells[row_Excel, 1].Value;
  While str<>'' do
  begin
    Cell1 := MyExcel.WorkSheets[1].Cells[row_Excel, 1];
    row_Excel:=row_Excel+8;
    // Правая нижняя ячейка области, из которой будем выводить данные
    Cell2 := MyExcel.WorkSheets[1].Cells[row_Excel-1, 51];
    // Область, из которой будем выводить данные
    Range := MyExcel.WorkSheets[1].Range[Cell1, Cell2];
    // Намного быстрее поячеечного присвоения
    ArrayData:=Range.Value;
    str:= ArrayData[1, 1];
    if Length(str)>50 then
         str := Copy(str,0,50);
    gr_id:= ArrayData[1, 2];
    podgr_id:=ArrayData[1, 3];
    rab_gr:='';
    vih_gr:='';
    insQuery.Close;
    insQuery.SQL.Text := 'INSERT INTO CAT_GR_OBJECTS (ID_GR_POTREB,ID_PODGR_POTREB, NAME, RAB_GR,VIH_GR) values('+
    IntToStr(gr_id) +', '+ IntToStr(podgr_id) + ', ''' +  str + ''',0000,0000)';
    insQuery.ExecQuery;
    last_id:=GetLastTableID('CAT_GR_OBJECTS');
    prov:=false;
    For j:=0 to 47 do
    begin
      if ArrayData[1, j+4]>0 then
       begin
         prov:=true;
         Break;
       end;
    end;
    if prov then
    begin
      For j:=0 to 47 do
        begin
        insQuery.Close;
        insQuery.SQL.Text := 'INSERT INTO CAT_GR_VALUES (ID_GR_OBJECTS,GR_TYPE,GR_TIME,GR_VALUE) values('+
        IntToStr(last_id) +',0,'+IntToStr(j)+','+ StringReplace(FloatToStr(Round(ArrayData[1, j+4]*1000)/1000),',','.',[rfReplaceAll])+')';
        insQuery.ExecQuery;
        end;
      rab_gr:=rab_gr+'1';
    end
    else
      rab_gr:=rab_gr+'0';

    prov:=false;
    For j:=0 to 47 do
    begin
      if ArrayData[2, j+4]>0 then
       begin
         prov:=true;
         Break;
       end;
    end;
    if prov then
    begin
      For j:=0 to 47 do
        begin
        insQuery.Close;
        insQuery.SQL.Text := 'INSERT INTO CAT_GR_VALUES (ID_GR_OBJECTS,GR_TYPE,GR_TIME,GR_VALUE) values('+
        IntToStr(last_id) +',1,'+IntToStr(j)+','+StringReplace(FloatToStr(Round(ArrayData[2, j+4]*1000)/1000),',','.',[rfReplaceAll])+')';
        insQuery.ExecQuery;
        end;
      vih_gr:=vih_gr+'1';
    end
    else
      vih_gr:=vih_gr+'0';

    prov:=false;
    For j:=0 to 47 do
    begin
      if ArrayData[3, j+4]>0 then
       begin
         prov:=true;
         Break;
       end;
    end;
    if prov then
    begin
      For j:=0 to 47 do
        begin
        insQuery.Close;
        insQuery.SQL.Text := 'INSERT INTO CAT_GR_VALUES (ID_GR_OBJECTS,GR_TYPE,GR_TIME,GR_VALUE) values('+
        IntToStr(last_id) +',2,'+IntToStr(j)+','+StringReplace(FloatToStr(Round(ArrayData[3, j+4]*1000)/1000),',','.',[rfReplaceAll])+')';
        insQuery.ExecQuery;
        end;
      rab_gr:=rab_gr+'1';
    end
    else
      rab_gr:=rab_gr+'0';

    prov:=false;
    For j:=0 to 47 do
    begin
      if ArrayData[4, j+4]>0 then
       begin
         prov:=true;
         Break;
       end;
    end;
    if prov then
    begin
      For j:=0 to 47 do
        begin
        insQuery.Close;
        insQuery.SQL.Text := 'INSERT INTO CAT_GR_VALUES (ID_GR_OBJECTS,GR_TYPE,GR_TIME,GR_VALUE) values('+
        IntToStr(last_id) +',3,'+IntToStr(j)+','+StringReplace(FloatToStr(Round(ArrayData[4, j+4]*1000)/1000),',','.',[rfReplaceAll])+')';
        insQuery.ExecQuery;
        end;
      vih_gr:=vih_gr+'1';
    end
    else
      vih_gr:=vih_gr+'0';

    prov:=false;
    For j:=0 to 47 do
    begin
      if ArrayData[5, j+4]>0 then
       begin
         prov:=true;
         Break;
       end;
    end;
    if prov then
    begin
      For j:=0 to 47 do
        begin
        insQuery.Close;
        insQuery.SQL.Text := 'INSERT INTO CAT_GR_VALUES (ID_GR_OBJECTS,GR_TYPE,GR_TIME,GR_VALUE) values('+
        IntToStr(last_id) +',4,'+IntToStr(j)+','+StringReplace(FloatToStr(Round(ArrayData[5, j+4]*1000)/1000),',','.',[rfReplaceAll])+')';
        insQuery.ExecQuery;
        end;
      rab_gr:=rab_gr+'1';
    end
    else
      rab_gr:=rab_gr+'0';

     prov:=false;
    For j:=0 to 47 do
    begin
      if ArrayData[6, j+4]>0 then
       begin
         prov:=true;
         Break;
       end;
    end;
    if prov then
    begin
      For j:=0 to 47 do
        begin
        insQuery.Close;
        insQuery.SQL.Text := 'INSERT INTO CAT_GR_VALUES (ID_GR_OBJECTS,GR_TYPE,GR_TIME,GR_VALUE) values('+
        IntToStr(last_id) +',5,'+IntToStr(j)+','+StringReplace(FloatToStr(Round(ArrayData[6, j+4]*1000)/1000),',','.',[rfReplaceAll])+')';
        insQuery.ExecQuery;
        end;
      vih_gr:=vih_gr+'1';
    end
    else
      vih_gr:=vih_gr+'0';

     prov:=false;
    For j:=0 to 47 do
    begin
      if ArrayData[7, j+4]>0 then
       begin
         prov:=true;
         Break;
       end;
    end;
    if prov then
    begin
      For j:=0 to 47 do
        begin
        insQuery.Close;
        insQuery.SQL.Text := 'INSERT INTO CAT_GR_VALUES (ID_GR_OBJECTS,GR_TYPE,GR_TIME,GR_VALUE) values('+
        IntToStr(last_id) +',6,'+IntToStr(j)+','+StringReplace(FloatToStr(Round(ArrayData[7, j+4]*1000)/1000),',','.',[rfReplaceAll])+')';
        insQuery.ExecQuery;
        end;
      rab_gr:=rab_gr+'1';
    end
    else
      rab_gr:=rab_gr+'0';

     prov:=false;
    For j:=0 to 47 do
    begin
      if ArrayData[8, j+4]>0 then
       begin
         prov:=true;
         Break;
       end;
    end;
    if prov then
    begin
      For j:=0 to 47 do
        begin
        insQuery.Close;
        insQuery.SQL.Text := 'INSERT INTO CAT_GR_VALUES (ID_GR_OBJECTS,GR_TYPE,GR_TIME,GR_VALUE) values('+
        IntToStr(last_id) +',7,'+IntToStr(j)+','+StringReplace(FloatToStr(Round(ArrayData[8, j+4]*1000)/1000),',','.',[rfReplaceAll])+')';
        insQuery.ExecQuery;
        end;
      vih_gr:=vih_gr+'1';
    end
    else
      vih_gr:=vih_gr+'0';

    insQuery.Close;
    insQuery.SQL.Text := 'UPDATE CAT_GR_OBJECTS SET RAB_GR='+ rab_gr +', VIH_GR='+ vih_gr +
    'WHERE ID=' + IntToStr(last_id);
    insQuery.ExecQuery;
    str:= MyExcel.WorkSheets[1].Cells[row_Excel, 1].Value;
  end;

     finally
        SetLength(list_PodType,0);
        SetLength(list_Type,0);
        StopExcel;
        ShowMessage('Завершено на строке '+ IntToStr(row_Excel));
     end;


end;

end.

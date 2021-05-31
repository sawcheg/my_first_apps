unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, Word2000, OleServer,
    ExtCtrls,  ComCtrls,Math, Mask, Buttons, OleCtnrs,  ComObj, DBCtrls,ActiveX,DBITypes,
   DBIProcs, DBIErrs,BDE, jpeg, IdBaseComponent, IdComponent,
  IdTCPConnection, IdTCPClient, IdFTP,IniFiles, ShellApi,Tlhelp32, DBGridEh,
  Grids, DB, DBTables;

type

  TForm1 = class(TForm)
    Button1: TButton;
    Label1: TLabel;
    Label2: TLabel;
    Label3: TLabel;
    Label4: TLabel;
    Edit1: TEdit;
    Edit2: TEdit;
    Edit4: TEdit;
    Label5: TLabel;
    pc1: TPageControl;
    ts9: TTabSheet;
    Label6: TLabel;
    Label7: TLabel;
    Img3: TImage;
    Edit3: TEdit;
    Edit5: TEdit;
    Label8: TLabel;
    Label9: TLabel;
    ts1: TTabSheet;
    ts5: TTabSheet;
    TabSheet6: TTabSheet;
    Sg1: TStringGrid;
    Sg2: TStringGrid;
    Edit6: TEdit;
    Button3: TButton;
    Image1: TImage;
    sg3: TStringGrid;
    Button2: TButton;
    ts7: TTabSheet;
    ts8: TTabSheet;
    Image2: TImage;
    Edit7: TEdit;
    Label11: TLabel;
    Label12: TLabel;
    Label13: TLabel;
    Label14: TLabel;
    Label15: TLabel;
    Label16: TLabel;
    Label18: TLabel;
    Edit8: TEdit;
    Image3: TImage;
    Label17: TLabel;
    Img1: TImage;
    Label19: TLabel;
    Edit9: TEdit;
    Label20: TLabel;
    sg4: TStringGrid;
    img2: TImage;
    Image4: TImage;
    Image5: TImage;
    Image6: TImage;
    Image7: TImage;
    Image8: TImage;
    Image9: TImage;
    Label21: TLabel;
    procedure Button1Click(Sender: TObject);
    procedure Button2Click(Sender: TObject);
    procedure Sg1DblClick(Sender: TObject);
    procedure Button3Click(Sender: TObject);
    procedure FormShow(Sender: TObject);
//    procedure Edit9Change(Sender: TObject);
    procedure Sg2Click(Sender: TObject);
  private
    procedure wordspeed(val: boolean);
    procedure ReplaceField(const ADocument: OleVariant);
    procedure InsertRtf(const ARange: OleVariant);
    procedure InsertGraphicFileToDocument(const ACaption: string; const ARange: OleVariant);
    procedure Gettable1;
    procedure Create_Graph(pic: Integer);
    procedure Create_Table(n: Integer);
    function Get_Fi: Double;
    function Get_Oy: Double;
    procedure SM(ini: boolean);
    procedure SaveAsTxt;
    Function  GetINIParam(sectName, paramName: string): string;
    procedure WriteINIParams(sectName: string; const ParamNames: array of string; const ParamValues: array of string); overload;
    procedure WriteINIParams(const SectAndParamNames: array of string; const ParamValues: array of string); overload;


    { Private declarations }
  public
    { Public declarations }
  end;

type TArray = array[1..6] of Real;
procedure SortByB(var A,B: TArray; min, max: Integer);

var
  Form1: TForm1;
  a,bd:Double;
  Fn,Fb,G,Rb,lamn,lamb,sig,Wa,b0,W0,W02,r,R0,Fi0,n1,n2,Oy: Double;
  WordApp, Document: OleVariant;
  tay,b,k1n,Barn,L_n,Ln_lamn,Nn: TArray;
  t3ln, t3dn,Wn: array[0..8] of Double;
  opt: Integer;
  time,start: TdateTime;
  lvl: Integer;

implementation


{$R *.dfm}

//работа с ини файлом
Function TForm1.GetINIParam(sectName, paramName: string): string;
begin
  with TIniFile.Create(ChangeFileExt(Application.ExeName, '.ini')) do
  try
    result:=ReadString(sectName, paramName, '');
  finally
    Free;
  end;
end;

procedure TForm1.WriteINIParams(sectName: string; const ParamNames: array of string; const ParamValues: array of string);
var
  i: integer;
  s: string;
begin
  with TIniFile.Create(ChangeFileExt(Application.ExeName, '.ini')) do
  try
    for i:=0 to Length(ParamNames)-1 do
    begin
      if i>Length(ParamValues)-1 then
        s:=''
      else
        s:=ParamValues[i];
      WriteString(sectName, ParamNames[i], s);
    end;
  finally
    Free;
  end;
end;

procedure TForm1.WriteINIParams(const SectAndParamNames: array of string; const ParamValues: array of string);
var
  i, p: integer;
  sectName, paramName, value, s: string;
begin
  with TIniFile.Create(ChangeFileExt(Application.ExeName, '.ini')) do
  try
    for i:=0 to Length(SectAndParamNames)-1 do
    begin
      if i>Length(ParamValues)-1 then break;
      s:=SectAndParamNames[i];
      p:=Pos('|', s);
      if p=0 then break;
      sectName:=Copy(s, 1, p-1);
      paramName:=Copy(s, p+1, Length(s)-p);
      value:=ParamValues[i];
      WriteString(sectName, paramName, value);
    end;
  finally
    Free;
  end;
end;


procedure TForm1.Button1Click(Sender: TObject);
var
 check: Boolean;
begin
    try
    Fn:= StrToFloat(Edit1.Text);
    Fb:= StrToFloat(Edit2.Text);
    G:= StrToFloat(Edit6.Text);
    Rb:= StrToFloat(Edit4.Text);
    a:=StrToFloat(Edit3.Text);
    bd:=StrToFloat(Edit5.Text);
    check:=true;
    except
       ShowMessage('Проверьте исходные данные');
       check:=false;
    end;
   if check then
   begin
    Button1.Enabled :=false;
    Button3.Enabled :=false;
    Edit1.Enabled :=false;
    Edit2.Enabled :=false;
    Edit3.Enabled :=false;
    Edit4.Enabled :=false;
    Edit5.Enabled :=false;
    Edit6.Enabled :=false;
    sg1.Options :=[goFixedVertLine,goFixedHorzLine,goVertLine,goHorzLine,goRangeSelect,goEditing];
    lamn:= 3*100/Fn;
    lamb:= 3*100/Fb;
    sig:=Fb/Fn;
    lvl:=1;
    pc1.ActivePage := ts1;
    Button2.Visible :=true;
    GetTable1;
   end;
end;

procedure TForm1.Button2Click(Sender: TObject);
var
 i,min: Integer;
 path: String;
 check: Boolean;
begin
  if lvl =1 then
  begin
    try
    for i:=1 to 6 do
    begin
       b[i]:=StrToFloat(sg1.Cells[6-i,0]);
       tay[i]:=StrToFloat(sg1.Cells[6-i,1]);
       if b[i]*tay[i]=0 then Raise Exception.Create('');
    end;
    check:=true;
    except
       ShowMessage('Проверьте значения в таблице');
       check:=false;
    end;
   if check then
   begin
    SaveAsTxt;
    button2.Enabled :=false;
    SortByB(b,tay,1,6);
    for i:=1 to 6 do
    begin
      Barn[i]:=1+4.2*(1-tay[i])*(1+20*(b[i]*b[i]/tay[i]/tay[i])) ;
      k1n[i]:=0.904+3.0693*(0.98-tay[i])-7.23*(0.98-tay[i])*(0.98-tay[i]);
      L_n[i]:=lamn*b[i]/(1-tay[i])*k1n[i]*(1-1/sig/Barn[i]);
      Ln_lamn[i]:=L_n[i]/lamn;
      Nn[i]:=1-Ln(sig*Barn[i])/Ln(tay[i]);
    end;
     {  sg2.Cells[0,0]:='tn';
       sg2.Cells[1,0]:='бn';
       sg2.Cells[2,0]:='Barn';
       sg2.Cells[3,0]:='Ln';
       sg2.Cells[4,0]:='Ln/Лн';
       sg2.Cells[5,0]:='Nn';
       sg2.Cells[6,0]:='K1n';     }
    for i:=1 to 6 do
    begin
       sg2.Cells[0,i-1]:=FloatToStr(Round(tay[i]*1000)/1000);
       sg2.Cells[1,i-1]:=FloatToStr(Round(b[i]*1000)/1000);
       sg2.Cells[2,i-1]:=FloatToStr(Round(Barn[i]*1000)/1000);
       sg2.Cells[3,i-1]:=FloatToStr(Round(L_n[i]*1000)/1000);
       sg2.Cells[4,i-1]:=FloatToStr(Round(Ln_lamn[i]*1000)/1000);
       sg2.Cells[5,i-1]:=FloatToStr(Round(Nn[i]*1000)/1000);
       sg2.Cells[6,i-1]:=FloatToStr(Round(k1n[i]*1000)/1000);
    end;       
    min:=1;
    for i:=2 to 6 do
       if Ln_lamn[i]<Ln_lamn[min] then min:=i;
    Edit9.Text :=IntToStr(min);
    sg2.Row :=min-1;
    Create_Graph(1);
    lvl:=2;
    pc1.ActivePage:=ts5;
    sg1.Options :=[goFixedVertLine,goFixedHorzLine,goVertLine,goHorzLine,goRangeSelect];
    Button2.Enabled :=true;
    sg2.Enabled :=true;
    sg2.SetFocus;
    end;
   end
   else if lvl=2 then
   begin
    try
      opt:=StrToInt(Edit9.Text);
      if (opt<0) or (opt>7) then Raise Exception.Create('');;
      check:=true;
    except
       ShowMessage('Проверьте введенное в поле значение строки');
       check:=false;
    end;
    if check then
    begin
    tay[opt]:=Round(tay[opt]*1000)/1000;
    b[opt]:=Round(b[opt]*100)/100;
    Barn[opt]:=Round(Barn[opt]*1000)/1000;
    L_n[opt]:=Round(L_n[opt]*1000)/1000;
    k1n[opt]:=Round(k1n[opt]*1000)/1000;
    t3ln[0]:=Round(lamn/4*k1n[opt]*1000)/1000;
    for i:=1 to 7 do
      t3ln[i]:= t3ln[0]*Power(tay[opt],i);
    t3ln[8]:=t3ln[0]/sig/Barn[opt];
    t3dn[0]:=4*t3ln[0]*b[opt];

    for i:=1 to 8 do
      t3dn[i]:= t3dn[0]*Power(tay[opt],i);
    sg3.Cells[0,0]:='n';
    sg3.Cells[0,1]:='ln';
    sg3.Cells[0,2]:='dn';
    for i:=0 to 8 do
    begin
       sg3.Cells[i+1,0]:=IntToStr(i);
       sg3.Cells[i+1,1]:=FloatToStr(Round(t3ln[i]*1000)/1000);
       sg3.Cells[i+1,2]:=FloatToStr(Round(t3dn[i]*1000)/1000);
    end;
    Wa:=0;
    for i:=0 to 8 do
      begin
      Wn[i]:=120*(ln(2*t3ln[i]*1000/a)-1);
      Wa:=Wa+Wn[i];
      end;
    sg4.Cells[0,0]:='n';
    sg4.Cells[0,1]:='Wn';
    for i:=0 to 8 do
    begin
       sg4.Cells[i+1,0]:=IntToStr(i);
       sg4.Cells[i+1,1]:=FloatToStr(Round(Wn[i]*1000)/1000);
    end;
    Wa:=Wa/(Trunc(Nn[opt])+1);
    b0:=b[opt]/sqrt(tay[opt]);
    W0:=Rb*((1/8/b0/Wa*Rb)+sqrt((1/8/b0/Wa*Rb)*(1/8/b0/Wa*Rb)+1));
    Create_Graph(2);
    R0:=W02/((1/8/b0/Wa*Rb)+sqrt((1/8/b0/Wa*Rb)*(1/8/b0/Wa*Rb)+1));
    Fi0:=Get_Fi;
    Label12.caption:='='+FloatToStr(Round(tay[opt]*1000)/1000);
    Label13.caption:='='+FloatToStr(Round(b[opt]*1000)/1000);
    Label15.caption:='='+FloatToStr(Round(tay[opt]*1000)/1000);
    Label16.caption:='='+FloatToStr(Round(b[opt]*1000)/1000);
    Edit7.Text :=FloatToStr(Round(Fi0*1000)/1000);
    lvl:=3;
    sg2.Enabled :=false;
    pc1.ActivePage:=ts7;
    Edit7.Enabled :=true;
    Edit7.SetFocus;
    end;
  end
  else if lvl=3 then
  begin
    try
      Fi0:=StrToFloat(Edit7.Text);
      check:=true;
    except
       ShowMessage('Проверьте введенное в поле значение ф');
       check:=false;
    end;
    if check then
    begin
    Oy:=Get_Oy;
    Edit8.Text :=FloatToStr(Round(Oy*1000)/1000);
    lvl:=4;
    pc1.ActivePage:=ts8;
    Button2.Enabled :=true;
    Button2.caption :='Завершить';
    Edit7.Enabled :=false;
    Edit8.Enabled :=true;
    Edit8.SetFocus;
    end;
  end
  else if lvl=4 then
  begin
    try
      Oy:=StrToFloat(Edit8.Text);
      check:=true;
    except
       ShowMessage('Проверьте введенное в поле значение Q');
       check:=false;
    end;
    if check then
    begin
    Edit8.Enabled :=false;
    n1:=-0.15/log10(cos(Fi0*Pi/180/2));
    n2:=-0.15/log10(cos(Oy*Pi/180/2));
    Create_Graph(3);
     pc1.ActivePage:=ts9;
    Button2.caption :='Продолжить';
    Button2.Visible :=false;
    Button1.Enabled :=true;
    Button3.Enabled :=true;
    Edit1.Enabled :=true;
    Edit2.Enabled :=true;
    Edit3.Enabled :=true;
    Edit4.Enabled :=true;
    Edit5.Enabled :=true;
    Edit6.Enabled :=true;
    ShowMessage('Расчет выполнен, графики и диаграммы сохранены в каталог программы.'+ #13#10+
    'Для формирования отчета нажмите кнопку "Сохранить в Word"');
    end;
   end;
end;

procedure TForm1.ReplaceField(const ADocument: OleVariant);
var
  i: Integer;
  BookmarkName: string;
  Range: OleVariant;

  function CompareBm(ABmName: string; const AName: string): Boolean;
  var
    i: Integer;
  begin
    i := Pos('__', ABmName);
    if i > 0 then
      Delete(ABmName, i, Length(ABmName) - i + 1);
    Result := SameText(ABmName, AName);
  end;

begin
  for i := ADocument.Bookmarks.Count downto 1 do
  begin
    BookmarkName := ADocument.Bookmarks.Item(i).Name;
    Range := ADocument.Bookmarks.Item(i).Range;
    if CompareBm(BookmarkName, 'Fmin') then
      Range.Text := FloatToStr(Fn)
    else
    if CompareBm(BookmarkName, 'Fmax') then
      Range.Text := FloatToStr(Fb)
    else
    if CompareBm(BookmarkName, 'G') then
      Range.Text := FloatToStr(G)
    else
    if CompareBm(BookmarkName, 'Rb') then
      Range.Text := FloatToStr(Rb)
    else
    if CompareBm(BookmarkName, 'lamn') then
      Range.Text := FloatToStr(round(lamn*1000)/1000)
    else
    if CompareBm(BookmarkName, 'lamb') then
      Range.Text := FloatToStr(round(lamb*1000)/1000)
    else
    if CompareBm(BookmarkName, 'sig') then
      Range.Text := FloatToStr(round(sig*1000)/1000)
    else
    if CompareBm(BookmarkName, 't111') then
      Range.Text := FloatToStr(round(b[1]*100)/100)
    else
    if CompareBm(BookmarkName, 't112') then
      Range.Text := FloatToStr(round(b[2]*100)/100)
    else
    if CompareBm(BookmarkName, 't113') then
      Range.Text := FloatToStr(round(b[3]*100)/100)
    else
    if CompareBm(BookmarkName, 't114') then
      Range.Text := FloatToStr(round(b[4]*100)/100)
    else
    if CompareBm(BookmarkName, 't115') then
      Range.Text := FloatToStr(round(b[5]*100)/100)
    else
    if CompareBm(BookmarkName, 't116') then
      Range.Text := FloatToStr(round(b[6]*100)/100)
    else
    if CompareBm(BookmarkName, 't121') then
      Range.Text := FloatToStr(round(tay[1]*1000)/1000)
    else
    if CompareBm(BookmarkName, 't122') then
      Range.Text := FloatToStr(round(tay[2]*1000)/1000)
    else
    if CompareBm(BookmarkName, 't123') then
      Range.Text := FloatToStr(round(tay[3]*1000)/1000)
    else
    if CompareBm(BookmarkName, 't124') then
      Range.Text := FloatToStr(round(tay[4]*1000)/1000)
    else
    if CompareBm(BookmarkName, 't125') then
      Range.Text := FloatToStr(round(tay[5]*1000)/1000)
    else
    if CompareBm(BookmarkName, 't126') then
      Range.Text := FloatToStr(round(tay[6]*1000)/1000)
    else
    if CompareBm(BookmarkName, 'tayOpt') then
      Range.Text := FloatToStr(round(tay[opt]*1000)/1000)
    else
    if CompareBm(BookmarkName, 'bOpt') then
      Range.Text := FloatToStr(round(b[opt]*100)/100)
    else
    if CompareBm(BookmarkName, 'BarOpt') then
      Range.Text := FloatToStr(round(Barn[opt]*1000)/1000)
    else
    if CompareBm(BookmarkName, 'LOpt') then
      Range.Text := FloatToStr(round(L_n[opt]*1000)/1000)
    else
    if CompareBm(BookmarkName, 'k1Opt') then
      Range.Text := FloatToStr(round(k1n[opt]*1000)/1000)
    else
    if CompareBm(BookmarkName, 'NOpt') then
      Range.Text := IntToStr(Trunc(Nn[opt])+1)
    else
     if CompareBm(BookmarkName, 'L0') then
      Range.Text := FloatToStr(round(t3ln[0]*1000)/1000)
    else
     if CompareBm(BookmarkName, 'L') then
      Range.Text := FloatToStr(round(t3ln[8]*1000)/1000)
    else
     if CompareBm(BookmarkName, 'D0') then
      Range.Text := FloatToStr(round(t3dn[0]*1000)/1000)
    else
     if CompareBm(BookmarkName, 'Wa') then
      Range.Text := FloatToStr(round(Wa*100)/100)
    else
     if CompareBm(BookmarkName, 'b0') then
      Range.Text := FloatToStr(round(b0*1000)/1000)
    else
      if CompareBm(BookmarkName, 'W0') then
      Range.Text := FloatToStr(round(W0*1000)/1000)
    else
      if CompareBm(BookmarkName, 'W02') then
      Range.Text := FloatToStr(round(W02*1000)/1000)
    else
      if CompareBm(BookmarkName, 'r') then
      Range.Text := FloatToStr(round(r*1000)/1000)
    else
      if CompareBm(BookmarkName, 'R0') then
      Range.Text := FloatToStr(round(R0*1000)/1000)
    else
     if CompareBm(BookmarkName, 'Fi') then
      Range.Text := FloatToStr(round(Fi0*10)/10)
    else
     if CompareBm(BookmarkName, 'Oy') then
      Range.Text := FloatToStr(Oy)
    else
     if CompareBm(BookmarkName, 'a') then
      Range.Text := FloatToStr(a)
    else
     if CompareBm(BookmarkName, 'b') then
      Range.Text := FloatToStr(bd)
    else
    if CompareBm(BookmarkName, 'graf') then
      InsertGraphicFileToDocument('1',Range)
    else
    if CompareBm(BookmarkName, 'graf2') then
      InsertGraphicFileToDocument('2',Range)
    else
    if CompareBm(BookmarkName, 'graf3') then
      InsertGraphicFileToDocument('3',Range);
  end;
end;

procedure TForm1.Create_Table(n: Integer);
var
 i,j : integer;
begin
   if n=2 then
   For i:=1 To 6 Do
     For j:=1 To 7 Do
       begin
          Document.Tables.Item(2).Cell(i+1,j).Range.Text:=FloatToStr(StrToFloat(sg2.Cells[j-1,i-1]));
       end;
   if n=3 then
   For i:=2 To 10 Do
       begin
          Document.Tables.Item(3).Cell(2, i).Range.Text:=FloatToStr(Round(t3ln[i-2]*1000)/1000);
          Document.Tables.Item(3).Cell(3, i).Range.Text:=FloatToStr(Round(t3dn[i-2]*1000)/1000);
       end;
   if n=4 then
   For i:=2 To 10 Do
       begin
          Document.Tables.Item(4).Cell(2, i).Range.Text:=FloatToStr(Round(Wn[i-2]*100)/100);
       end;
end;

procedure TForm1.InsertGraphicFileToDocument(const ACaption: string; const ARange: OleVariant);
var
  FileName: string;
begin
  FileName := ExtractFilePath(Application.ExeName) +'Рисунок '+ ACaption + '.jpg';
  if ACaption='1' then
  img1.Picture.SaveToFile(FileName)
  else if ACaption='2' then
  img2.Picture.SaveToFile(FileName)
  else if ACaption='3' then
  img3.Picture.SaveToFile(FileName);
  ARange.ParagraphFormat.Alignment := wdAlignParagraphLeft;
  ARange.Font.Bold := True;
// ARange.TypeText(ACaption);
// ARange.TypeParagraph;
  ARange.ParagraphFormat.Alignment := wdAlignParagraphCenter;
  ARange.InlineShapes.AddPicture(FileName := FileName, LinkToFile := True, SaveWithDocument := True)
end;

procedure TForm1.Gettable1;
var
i:Integer;
begin
   if GetINIParam(Edit6.Text,'b1')<>'' then
   begin
     sg1.Cells[5,0]:=GetINIParam(Edit6.Text,'b1');
     sg1.Cells[4,0]:=GetINIParam(Edit6.Text,'b2');
     sg1.Cells[3,0]:=GetINIParam(Edit6.Text,'b3');
     sg1.Cells[2,0]:=GetINIParam(Edit6.Text,'b4');
     sg1.Cells[1,0]:=GetINIParam(Edit6.Text,'b5');
     sg1.Cells[0,0]:=GetINIParam(Edit6.Text,'b6');
     sg1.Cells[5,1]:=GetINIParam(Edit6.Text,'t1');
     sg1.Cells[4,1]:=GetINIParam(Edit6.Text,'t2');
     sg1.Cells[3,1]:=GetINIParam(Edit6.Text,'t3');
     sg1.Cells[2,1]:=GetINIParam(Edit6.Text,'t4');
     sg1.Cells[1,1]:=GetINIParam(Edit6.Text,'t5');
     sg1.Cells[0,1]:=GetINIParam(Edit6.Text,'t6');
     SM(true);
   end
  { else if Round((G-7.5)*10)=0 then             //7,5
   begin
     b[6]:=0.21;  tay[6]:=0.86;
     b[5]:=0.19;  tay[5]:=0.826;
     b[4]:=0.16;  tay[4]:=0.780;
     b[2]:=0.10;  tay[2]:=0.780;
     b[1]:=0.06;  tay[1]:=0.852;
     SM(false);
   end
   else if Round((G-8)*10)=0 then      //8
   begin
     b[5]:=0.21;  tay[5]:=0.873;
     b[4]:=0.17;  tay[4]:=0.820;
     b[3]:=0.13;  tay[3]:=0.790;
     b[2]:=0.09;  tay[2]:=0.865;
     b[1]:=0.06;  tay[1]:=0.889;
     SM(false);
   end
   else if Round((G-8.5)*10)=0then      //8,5
   begin
     b[5]:=0.21;  tay[5]:=0.895;
     b[4]:=0.17;  tay[4]:=0.842;
     b[3]:=0.13;  tay[3]:=0.860;
     b[2]:=0.09;  tay[2]:=0.897;
     b[1]:=0.06;  tay[1]:=0.911;
     SM(false);
   end
   else  if Round((G-9)*10)=0 then    //9
   begin
     b[5]:=0.21;  tay[5]:=0.910;
     b[4]:=0.17;  tay[4]:=0.872;
     b[3]:=0.13;  tay[3]:=0.893;
     b[2]:=0.09;  tay[2]:=0.925;
     b[1]:=0.06;  tay[1]:=0.940;
     SM(false);
   end
   else  if Round((G-9.5)*10)=0 then    //9,5
   begin
     b[5]:=0.21;  tay[5]:=0.926;
     b[4]:=0.17;  tay[4]:=0.898;
     b[3]:=0.13;  tay[3]:=0.920;
     b[2]:=0.09;  tay[2]:=0.943;
     b[1]:=0.06;  tay[1]:=0.960;
     SM(false);
   end
   else  if Round((G-10)*10)=0 then    //10
   begin
     b[5]:=0.21;  tay[5]:=0.936;
     b[4]:=0.18;  tay[4]:=0.922;
     b[3]:=0.15;  tay[3]:=0.930;
     b[2]:=0.12;  tay[2]:=0.948;
     b[1]:=0.08;  tay[1]:=0.965;
     SM(false);
   end
   else  if Round((G-10.5)*10)=0 then    //10,5
   begin
     b[5]:=0.21;  tay[5]:=0.948;
     b[4]:=0.19;  tay[4]:=0.937;
     b[3]:=0.17;  tay[3]:=0.931;
     b[2]:=0.14;  tay[2]:=0.950;
     b[1]:=0.11;  tay[1]:=0.968;
     SM(false);
   end
   else  if Round((G-11)*10)=0 then    //11
   begin
     b[5]:=0.21;  tay[5]:=0.956;
     b[4]:=0.19;  tay[4]:=0.949;
     b[3]:=0.17;  tay[3]:=0.945;
     b[2]:=0.15;  tay[2]:=0.954;
     b[1]:=0.12;  tay[1]:=0.972;
     SM(false);
   end
   else  if Round((G-11.5)*10)=0 then    //11,5
   begin
     b[5]:=0.21;  tay[5]:=0.970;
     b[4]:=0.19;  tay[4]:=0.957;
     b[3]:=0.17;  tay[3]:=0.956;
     b[2]:=0.15;  tay[2]:=0.966;
     b[1]:=0.14;  tay[1]:=0.973;
     SM(false);
   end
   else  if Round((G-12)*10)=0 then    //12
   begin
     b[5]:=0.2;   tay[5]:=0.970;
     b[4]:=0.19;  tay[4]:=0.968;
     b[3]:=0.18;  tay[3]:=0.968;
     b[2]:=0.17;  tay[2]:=0.970;
     b[1]:=0.16;  tay[1]:=0.971;
     SM(false);
   end      }
   else
   begin
     ShowMessage('Для G=' +FloatToStr(G)+' требуется внести данные по б и t вручную');
     pc1.Visible:=true;
     for i:=1 to 6 do
     begin
       sg1.Cells[6-i,0]:='';
       sg1.Cells[6-i,1]:='';
     end;
     sg1.SetFocus;
   end;
end;
procedure TForm1.SM(ini: boolean);
Var
 i: Integer;
begin
  pc1.Visible:=true;
  if not ini then
  for i:=1 to 6 do
    begin
       sg1.Cells[6-i,0]:=FloatToStr(Round(b[i]*1000)/1000);
       sg1.Cells[6-i,1]:=FloatToStr(Round(tay[i]*1000)/1000);
    end;
 Button2.SetFocus;
 ShowMessage('Для G=' +FloatToStr(G)+' подобраны значения б и t.'+ #13#10+' В случае необходимости скорректируйте значения и нажмите "Продолжить"');
end;

procedure TForm1.Create_Graph(pic: Integer);
var
  x,y,max,min: Integer;
  s: String;
  maxr: Integer;
  bmp:TBitmap ;
begin
 if pic=1 then
 begin
 bmp:= TBitmap.Create;
 bmp.LoadFromFile( ExtractFilePath(Application.ExeName)+'\1.bmp');
 Img1.Canvas.Draw(0, 0, bmp);
 bmp.Free;
 with img1.Canvas do
 begin
  min:=Trunc(Nn[1]);
  max:=Trunc(Nn[1])+1;
  For x:=2 to 6 do
  begin
    if Nn[x]>max then max:=Trunc(Nn[x])+1;
    if Nn[x]<min then min:=Trunc(Nn[x]);
  end;
  Brush.Color:=clBlack;
  Pen.Color:=clBlack;
  Pen.Width:=1;
  FrameRect(Rect(96,10,336,190));
  FrameRect(Rect(475,10,715,190));
  Pen.Color:=$0040FF00;
  Pen.Width :=1;
  Brush.Color:=clWhite;
  For x:=2 to 5 do
  begin
    moveto(48+48*x,11);
    Lineto(48+48*x,189);
    moveto(427+48*x,11);
    Lineto(427+48*x,189);
  end;
  For y:=0 to (max-min) do
  begin
    if (y<>0) and (y<>(max-min)) then
    begin
    moveto(97,Trunc(10+y*180/(max-min)));
    Lineto(335,Trunc(10+y*180/(max-min)));
    end;
    s:=IntToStr(max-y);
    TextOut(81-TextWidth(s),10+Round(y*180/(max-min)-TextHeight(s)/2),s);
  end;
  Pen.Width :=2;
  Pen.Color:=clRed;
  moveto(96,10+Round((max-Nn[1])*180/(max-min)));
  For x:=2 to 6 do
    Lineto(48+48*x,10+Round((max-Nn[x])*180/(max-min)));
  min:=Trunc(Ln_lamn[1]*2);
  max:=Trunc(Ln_lamn[1]*2)+1;
  For x:=2 to 6 do
  begin
    if Ln_lamn[x]*2>max then max:=Trunc(Ln_lamn[x]*2)+1;
    if Ln_lamn[x]*2<min then min:=Trunc(Ln_lamn[x]*2);
  end;
  moveto(10,115);
  Lineto(60,115);
  moveto(385,135);
  Lineto(435,135);
  Pen.Color:=$0040FF00;
  Pen.Width :=1;
  For y:=0 to (max-min) do
  begin
    if (y<>0) and (y<>(max-min)) then
    begin
    moveto(476,Trunc(10+y*180/(max-min)));
    Lineto(714,Trunc(10+y*180/(max-min)));
    end;
    s:=FloatToStr((max-y)/2);
    TextOut(465-TextWidth(s),10+Round(y*180/(max-min)-TextHeight(s)/2),s);
  end;
  Pen.Width :=2;
  Pen.Color:=clRed;
  moveto(475,10+Round((max-Ln_lamn[1]*2)*180/(max-min)));
  For x:=2 to 6 do
    Lineto(427+48*x,10+Round((max-Ln_lamn[x]*2)*180/(max-min)));
  For x:=1 to 6 do
    begin
    s:=FloatToStr(Round(b[x]*1000)/1000);
    TextOut(48+48*x-Round(TextWidth(s)/2),194,s);
    TextOut(427+48*x-Round(TextWidth(s)/2),194,s);
    end;
 end;
 end
 else if pic=2 then
 begin
 with img2.Canvas do
 begin
  for y := 0 to img2.Height - 1 do
    for x := 0 to img2.Width - 1 do
      Pixels[x, y] := RGB(255, 255, 255);
  min:=0;
  max:=Trunc(W0/60)+1;
  Pen.Color:=clBlack;
  Pen.Width:=1;
  Brush.Color:=clBlack;
  FrameRect(Rect(150,10,620,250));
  Pen.Color:=$0040FF00;
  Pen.Width :=1;
  Brush.Color:=clWhite;
  For x:=1 to 4 do
  begin
    moveto(150+94*x,11);
    Lineto(150+94*x,249);
  end;
  For y:=0 to max do
  begin
    if (y<>0) and (y<>max) then
    begin
    moveto(151,Trunc(10+y*240/max));
    Lineto(619,Trunc(10+y*240/max));
    end;
    s:=IntToStr((max-y)*60);
    TextOut(136-TextWidth(s),10+Round(y*240/max-TextHeight(s)/2),s);
  end;
  TextOut(40,100,'W(r)');
  TextOut(40,140,'W');
  TextOut(40+TextWidth('W'),144,'0');
  Pen.Width :=2;
  Pen.Color:=clRed;
  moveto(40,120);
  Lineto(80,120);
  min:=Round(bd/2);
  W02:=0;
  maxr:=1;
  r:=min;
  while W02<=W0 do
  begin
    r:=r+1;
    W02:=276*log10(2*r/bd);
  end;
  if abs(W02-W0)>abs(276*log10(2*(r-1)/bd)-W0) then
  begin
    r:=r-1;
    W02:=276*log10(2*r/bd);
  end;
  if r>10 then
     maxr:=Trunc(r/10)+1;
  moveto(Round(150+470*min/10/maxr),250);
  For x:=(min+1)*100 to maxr*1000 do
  begin
    if 276*log10(2*x/100/bd)>max*60 then break;
    Lineto(Round(150+470*x/100/maxr/10),Round(250-240/max/60*276*log10(2*x/100/bd)));
  end;
  Pen.Width :=1;
  Pen.Style := psDot;
  Pen.Color:=clBlue;
  moveto(40,160);
  Lineto(80,160);
  moveto(40,161);
  Lineto(80,161);
  moveto(151,250-Round(240/max/60*w0));
  Lineto(619,250-Round(240/max/60*w0));
  moveto(151,251-Round(240/max/60*w0));
  Lineto(619,251-Round(240/max/60*w0));
  For x:=0 to 5 do
    begin
    s:=FloatToStr(Round(maxr*10/5*x)/1000);
    TextOut(150+94*(x)-Round(TextWidth(s)/2),254,s);
    end;
  TextOut(385-Round(TextWidth('r')/2),259,'r');
 end;
 end
 else if pic=3 then
 begin
 bmp:= TBitmap.Create;
 bmp.LoadFromFile(ExtractFilePath(Application.ExeName)+'\2.bmp');
 Img3.Canvas.Draw(0, 0, bmp);
 bmp.Free;
 with img3.Canvas do
 begin
   Brush.Color:=clWhite;
   Pen.Color:=clBlack;
   Pen.Width:=1;
   Ellipse(90,30,300,240);
   Ellipse(470,30,680,240);
   Pen.Color:=$0040FF00;
   Pen.Width :=1;
   For x:=1 To 4 do
   begin
     Ellipse(90+Round(x*105/5),30+Round(x*105/5),300-Round(x*105/5),240-Round(x*105/5));
     Ellipse(470+Round(x*105/5),30+Round(x*105/5),680-Round(x*105/5),240-Round(x*105/5));
   end;
   For x:=0 To 5 do
   begin
     moveto(195-Round(cos(x*30*Pi/180)*104),135-Round(sin(x*30*Pi/180)*104));
     LineTo(195+Round(cos(x*30*Pi/180)*104),135+Round(sin(x*30*Pi/180)*104));
     s:=IntTOStr(0+30*x);
     TextOut(195+Round(cos(x*30*Pi/180)*118-TextWidth(s)/2),135-Round(sin(x*30*Pi/180)*118+TextHeight(s)/2),s);
     s:=IntTOStr(180+30*x);
     TextOut(195-Round(cos(x*30*Pi/180)*118+TextWidth(s)/2),135+Round(sin(x*30*Pi/180)*118-TextHeight(s)/2),s);
   end;
   For x:=0 To 5 do
   begin
     moveto(575-Round(cos(x*30*Pi/180)*104),135-Round(sin(x*30*Pi/180)*104));
     LineTo(575+Round(cos(x*30*Pi/180)*104),135+Round(sin(x*30*Pi/180)*104));
     s:=IntTOStr(0+30*x);
     TextOut(575+Round(cos(x*30*Pi/180)*118-TextWidth(s)/2),135-Round(sin(x*30*Pi/180)*118+TextHeight(s)/2),s);
     s:=IntTOStr(180+30*x);
     TextOut(575-Round(cos(x*30*Pi/180)*118+TextWidth(s)/2),135+Round(sin(x*30*Pi/180)*118-TextHeight(s)/2),s);
   end;
   For x:=1 To 5 do
   begin
     s:=FloatTOStr(1-0.2*x);
     TextOut(195-Round(TextWidth(s)/2),30+Round(x*105/5-TextHeight(s)/2),s);
     TextOut(575-Round(TextWidth(s)/2),30+Round(x*105/5-TextHeight(s)/2),s);
   end;
   Pen.Color:=clRed;
   Pen.Width:=2;
   moveto(195,135);
   For x:=8 DownTo 1 do
   begin
     LineTo(195+Round(exp(n1*ln(cos((360-x*10)*Pi/180)))*cos((360-x*10)*Pi/180)*105),135+Round(exp(n1*ln(cos((360-x*10)*Pi/180)))*sin((360-x*10)*Pi/180)*105));
   end;
   For x:=0 To 8 do
   begin
     LineTo(195+Round(exp(n1*ln(cos(x*10*Pi/180)))*cos(x*10*Pi/180)*105),135+Round(exp(n1*ln(cos(x*10*Pi/180)))*sin(x*10*Pi/180)*105));
   end;
   LineTo(195,135);
   moveto(575,135);
   For x:=8 DownTo 1 do
   begin
     LineTo(575+Round(exp(n2*ln(cos((360-x*10)*Pi/180)))*cos((360-x*10)*Pi/180)*105),135+Round(exp(n2*ln(cos((360-x*10)*Pi/180)))*sin((360-x*10)*Pi/180)*105));
   end;
   For x:=0 To 8 do
   begin
     LineTo(575+Round(exp(n2*ln(cos(x*10*Pi/180)))*cos(x*10*Pi/180)*105),135+Round(exp(n2*ln(cos(x*10*Pi/180)))*sin(x*10*Pi/180)*105));
   end;
   LineTo(575,135);
 end;
 end;
end;

procedure TForm1.wordspeed(val: boolean);
begin
WordApp.screenupdating := val;
WordApp.Options.CheckGrammarAsYouType := val ;
WordApp.Options.CheckGrammarWithSpelling := val   ;
WordApp.Options.ContextualSpeller := val       ;
WordApp.Options.CheckSpellingAsYouType := val    ;
WordApp.Options.ShowReadabilityStatistics := val   ;
end;

function TForm1.Get_Fi: Double;
begin
   if (tay[opt]<=0.835) then
     begin
     if Round(b[opt]-0.06)*100=0 then result:= 53
     else if Round(b[opt]-0.07)*100=0 then result:= 52.2
     else if Round(b[opt]-0.08)*100=0 then result:= 51.6
     else if Round(b[opt]-0.09)*100=0 then result:= 51.3
     else if Round(b[opt]-0.10)*100=0 then result:= 51
     else if Round(b[opt]-0.11)*100=0 then result:= 50.5
     else if Round(b[opt]-0.12)*100=0 then result:= 50
     else if Round(b[opt]-0.13)*100=0 then result:= 49.8
     else if Round(b[opt]-0.14)*100=0 then result:= 50
     else if Round(b[opt]-0.15)*100=0 then result:= 50.2
     else if Round(b[opt]-0.16)*100=0 then result:= 51
     else if Round(b[opt]-0.17)*100=0 then result:= 52
     else if Round(b[opt]-0.18)*100=0 then result:= 53.5
     else if Round(b[opt]-0.19)*100=0 then result:= 55.5
     else if Round(b[opt]-0.20)*100=0 then result:= 58
     else if Round(b[opt]-0.21)*100=0 then result:= 63;
     end
    else if (tay[opt]<=0.895) then
     begin
     if Round(b[opt]-0.06)*100=0 then result:= 51
     else if Round(b[opt]-0.07)*100=0 then result:= 50.5
     else if Round(b[opt]-0.08)*100=0 then result:= 50
     else if Round(b[opt]-0.09)*100=0 then result:= 49.6
     else if Round(b[opt]-0.10)*100=0 then result:= 49.3
     else if Round(b[opt]-0.11)*100=0 then result:= 49
     else if Round(b[opt]-0.12)*100=0 then result:= 48.8
     else if Round(b[opt]-0.13)*100=0 then result:= 48.4
     else if Round(b[opt]-0.14)*100=0 then result:= 48
     else if Round(b[opt]-0.15)*100=0 then result:= 48
     else if Round(b[opt]-0.16)*100=0 then result:= 48.2
     else if Round(b[opt]-0.17)*100=0 then result:= 49
     else if Round(b[opt]-0.18)*100=0 then result:= 50
     else if Round(b[opt]-0.19)*100=0 then result:= 50.5
     else if Round(b[opt]-0.20)*100=0 then result:= 51.6
     else if Round(b[opt]-0.21)*100=0 then result:= 54;
     end
     else if (tay[opt]<=0.935) then
     begin
     if Round(b[opt]-0.06)*100=0 then result:= 50
     else if Round(b[opt]-0.07)*100=0 then result:= 49.5
     else if Round(b[opt]-0.08)*100=0 then result:= 49
     else if Round(b[opt]-0.09)*100=0 then result:= 48.5
     else if Round(b[opt]-0.10)*100=0 then result:= 48
     else if Round(b[opt]-0.11)*100=0 then result:= 47.6
     else if Round(b[opt]-0.12)*100=0 then result:= 47.3
     else if Round(b[opt]-0.13)*100=0 then result:= 47
     else if Round(b[opt]-0.14)*100=0 then result:= 46.6
     else if Round(b[opt]-0.15)*100=0 then result:= 46.2
     else if Round(b[opt]-0.16)*100=0 then result:= 46
     else if Round(b[opt]-0.17)*100=0 then result:= 46.4
     else if Round(b[opt]-0.18)*100=0 then result:= 47
     else if Round(b[opt]-0.19)*100=0 then result:= 47.5
     else if Round(b[opt]-0.20)*100=0 then result:= 48
     else if Round(b[opt]-0.21)*100=0 then result:= 49;
     end
     else if (tay[opt]<=0.960) then
     begin
     if Round(b[opt]-0.06)*100=0 then result:= 49.5
     else if Round(b[opt]-0.07)*100=0 then result:= 48.5
     else if Round(b[opt]-0.08)*100=0 then result:= 48
     else if Round(b[opt]-0.09)*100=0 then result:= 47.5
     else if Round(b[opt]-0.10)*100=0 then result:= 47
     else if Round(b[opt]-0.11)*100=0 then result:= 46.5
     else if Round(b[opt]-0.12)*100=0 then result:= 46
     else if Round(b[opt]-0.13)*100=0 then result:= 45.6
     else if Round(b[opt]-0.14)*100=0 then result:= 45.3
     else if Round(b[opt]-0.15)*100=0 then result:= 44.5
     else if Round(b[opt]-0.16)*100=0 then result:= 44
     else if Round(b[opt]-0.17)*100=0 then result:= 43.8
     else if Round(b[opt]-0.18)*100=0 then result:= 43.5
     else if Round(b[opt]-0.19)*100=0 then result:= 44
     else if Round(b[opt]-0.20)*100=0 then result:= 44.5
     else if Round(b[opt]-0.21)*100=0 then result:= 45;
     end
     else
     begin
     if Round(b[opt]-0.06)*100=0 then result:= 47
     else if Round(b[opt]-0.07)*100=0 then result:= 46.5
     else if Round(b[opt]-0.08)*100=0 then result:= 46
     else if Round(b[opt]-0.09)*100=0 then result:= 45.5
     else if Round(b[opt]-0.10)*100=0 then result:= 45
     else if Round(b[opt]-0.11)*100=0 then result:= 44.5
     else if Round(b[opt]-0.12)*100=0 then result:= 44
     else if Round(b[opt]-0.13)*100=0 then result:= 43.6
     else if Round(b[opt]-0.14)*100=0 then result:= 43
     else if Round(b[opt]-0.15)*100=0 then result:= 42
     else if Round(b[opt]-0.16)*100=0 then result:= 41
     else if Round(b[opt]-0.17)*100=0 then result:= 40
     else if Round(b[opt]-0.18)*100=0 then result:= 39.7
     else if Round(b[opt]-0.19)*100=0 then result:= 40
     else if Round(b[opt]-0.20)*100=0 then result:= 40.5
     else if Round(b[opt]-0.21)*100=0 then result:= 41.5;
     end;
end;

function TForm1.Get_Oy: Double;
begin
   if (tay[opt]<=0.835) then
     begin
     if Round(b[opt]-0.06)*100=0 then result:= 158
     else if Round(b[opt]-0.07)*100=0 then result:= 156
     else if Round(b[opt]-0.08)*100=0 then result:= 154
     else if Round(b[opt]-0.09)*100=0 then result:= 150
     else if Round(b[opt]-0.10)*100=0 then result:= 145
     else if Round(b[opt]-0.11)*100=0 then result:= 138
     else if Round(b[opt]-0.12)*100=0 then result:= 133
     else if Round(b[opt]-0.13)*100=0 then result:= 128
     else if Round(b[opt]-0.14)*100=0 then result:= 125
     else if Round(b[opt]-0.15)*100=0 then result:= 126
     else if Round(b[opt]-0.16)*100=0 then result:= 127
     else if Round(b[opt]-0.17)*100=0 then result:= 133
     else if Round(b[opt]-0.18)*100=0 then result:= 140
     else if Round(b[opt]-0.19)*100=0 then result:= 150
     else if Round(b[opt]-0.20)*100=0 then result:= 160
     else if Round(b[opt]-0.21)*100=0 then result:= 178;
     end
    else if (tay[opt]<=0.895) then
     begin
     if Round(b[opt]-0.06)*100=0 then result:= 135
     else if Round(b[opt]-0.07)*100=0 then result:= 132
     else if Round(b[opt]-0.08)*100=0 then result:= 130
     else if Round(b[opt]-0.09)*100=0 then result:= 129
     else if Round(b[opt]-0.10)*100=0 then result:= 127
     else if Round(b[opt]-0.11)*100=0 then result:= 123
     else if Round(b[opt]-0.12)*100=0 then result:= 119
     else if Round(b[opt]-0.13)*100=0 then result:= 114
     else if Round(b[opt]-0.14)*100=0 then result:= 110
     else if Round(b[opt]-0.15)*100=0 then result:= 107
     else if Round(b[opt]-0.16)*100=0 then result:= 105
     else if Round(b[opt]-0.17)*100=0 then result:= 106
     else if Round(b[opt]-0.18)*100=0 then result:= 107
     else if Round(b[opt]-0.19)*100=0 then result:= 110
     else if Round(b[opt]-0.20)*100=0 then result:= 115
     else if Round(b[opt]-0.21)*100=0 then result:= 123;
     end
     else if (tay[opt]<=0.935) then
     begin
     if Round(b[opt]-0.06)*100=0 then result:= 110
     else if Round(b[opt]-0.07)*100=0 then result:= 108
     else if Round(b[opt]-0.08)*100=0 then result:= 106
     else if Round(b[opt]-0.09)*100=0 then result:= 104
     else if Round(b[opt]-0.10)*100=0 then result:= 102
     else if Round(b[opt]-0.11)*100=0 then result:= 101
     else if Round(b[opt]-0.12)*100=0 then result:= 100
     else if Round(b[opt]-0.13)*100=0 then result:= 99
     else if Round(b[opt]-0.14)*100=0 then result:= 97
     else if Round(b[opt]-0.15)*100=0 then result:= 93
     else if Round(b[opt]-0.16)*100=0 then result:= 90
     else if Round(b[opt]-0.17)*100=0 then result:= 88
     else if Round(b[opt]-0.18)*100=0 then result:= 87
     else if Round(b[opt]-0.19)*100=0 then result:= 90
     else if Round(b[opt]-0.20)*100=0 then result:= 94
     else if Round(b[opt]-0.21)*100=0 then result:= 98;
     end
     else if (tay[opt]<=0.960) then
     begin
     if Round(b[opt]-0.06)*100=0 then result:= 100
     else if Round(b[opt]-0.07)*100=0 then result:= 99
     else if Round(b[opt]-0.08)*100=0 then result:= 98
     else if Round(b[opt]-0.09)*100=0 then result:= 95
     else if Round(b[opt]-0.10)*100=0 then result:= 93
     else if Round(b[opt]-0.11)*100=0 then result:= 90
     else if Round(b[opt]-0.12)*100=0 then result:= 87
     else if Round(b[opt]-0.13)*100=0 then result:= 84
     else if Round(b[opt]-0.14)*100=0 then result:= 80
     else if Round(b[opt]-0.15)*100=0 then result:= 77
     else if Round(b[opt]-0.16)*100=0 then result:= 74
     else if Round(b[opt]-0.17)*100=0 then result:= 72
     else if Round(b[opt]-0.18)*100=0 then result:= 70
     else if Round(b[opt]-0.19)*100=0 then result:= 72
     else if Round(b[opt]-0.20)*100=0 then result:= 74
     else if Round(b[opt]-0.21)*100=0 then result:= 77;
     end
     else
     begin
     if Round(b[opt]-0.06)*100=0 then result:= 90
     else if Round(b[opt]-0.07)*100=0 then result:= 88
     else if Round(b[opt]-0.08)*100=0 then result:= 86
     else if Round(b[opt]-0.09)*100=0 then result:= 84
     else if Round(b[opt]-0.10)*100=0 then result:= 81
     else if Round(b[opt]-0.11)*100=0 then result:= 78
     else if Round(b[opt]-0.12)*100=0 then result:= 75
     else if Round(b[opt]-0.13)*100=0 then result:= 73
     else if Round(b[opt]-0.14)*100=0 then result:= 70
     else if Round(b[opt]-0.15)*100=0 then result:= 68
     else if Round(b[opt]-0.16)*100=0 then result:= 66
     else if Round(b[opt]-0.17)*100=0 then result:= 64
     else if Round(b[opt]-0.18)*100=0 then result:= 62
     else if Round(b[opt]-0.19)*100=0 then result:= 63
     else if Round(b[opt]-0.20)*100=0 then result:= 65
     else if Round(b[opt]-0.21)*100=0 then result:= 68;
     end;
end;

procedure TForm1.InsertRtf(const ARange: OleVariant);
begin

end;

procedure TForm1.Sg1DblClick(Sender: TObject);
begin
   sg1.EditorMode :=true;
end;

procedure TForm1.Button3Click(Sender: TObject);
var
 TempleateFileName: string;
begin
  TempleateFileName := ExtractFilePath(Application.ExeName) + '\Result.dot';
  try
    WordApp := GetActiveOleObject('Word.Application');
  except
    try
      WordApp := CreateOleObject('Word.Application');
    except
      on E: Exception do
      begin
        ShowMessage('Не удалось запустить Word!'#13#10 + E.Message);
        Exit;
      end;
    end;
  end;  
  try
   Document := WordApp.Documents.Add(Template := TempleateFileName, NewTemplate := False);
   wordspeed(false);
   Create_Table(2);
   Create_Table(3);
   Create_Table(4);
   ReplaceField(Document);
   wordspeed(true);
  finally
    WordApp.Visible := True;
    WordApp := Unassigned;
    Button3.Enabled :=false;
  end;
end;

procedure TForm1.SaveAsTxt;
begin
  WriteINIParams(Edit6.Text,['b1','b2','b3','b4','b5','b6','t1','t2','t3','t4','t5','t6'],
  [sg1.Cells[5,0],sg1.Cells[4,0],sg1.Cells[3,0],sg1.Cells[2,0],sg1.Cells[1,0],sg1.Cells[0,0],sg1.Cells[5,1],sg1.Cells[4,1],sg1.Cells[3,1],sg1.Cells[2,1],sg1.Cells[1,1],sg1.Cells[0,1]] );
end;

procedure TForm1.FormShow(Sender: TObject);
begin
 // sg1.Cells[0,0]:='б';
//  sg1.Cells[0,1]:='t';
end;



procedure TForm1.Sg2Click(Sender: TObject);
begin
   Edit9.Text := IntToStr(sg2.Row+1);
end;

procedure SortByB(var A,B: TArray; min, max: Integer);
var i, j : Integer;
tmp,tmp1,supp: real;
begin
supp:=A[max-((max-min) div 2)];
i:=min; j:=max;
while i<j do
  begin
    while A[i]<supp do i:=i+1;
    while A[j]>supp do j:=j-1;
    if i<=j then
      begin
        tmp:=A[i]; A[i]:=A[j]; A[j]:=tmp;
        tmp1:=B[i]; B[i]:=B[j]; B[j]:=tmp1;
        i:=i+1; j:=j-1;
      end;
  end;
if min<j then SortByB(A,B, min, j);
if i<max then SortByB(A,B, i, max);
end;

end.

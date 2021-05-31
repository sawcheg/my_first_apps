unit Unit3;

interface

uses
  {Winapi.Windows, Winapi.Messages, System.SysUtils, System.Variants, System.Classes, Vcl.Graphics,
  Vcl.Controls, Vcl.Forms, Vcl.Dialogs, Vcl.StdCtrls, Vcl.Buttons, Vcl.ExtCtrls,   }
  Graphics,Forms,Controls, StdCtrls, Buttons, ExtCtrls, Classes,SysUtils,Dialogs,Variants,Messages,Windows,
  FIBDatabase, pFIBDatabase, FIBQuery, pFIBQuery;

type
  TForm3 = class(TForm)
    Label1: TLabel;
    Label2: TLabel;
    Label3: TLabel;
    Label4: TLabel;
    eNewName: TEdit;
    eNewUst: TEdit;
    eNewCos: TEdit;
    eNewDescr: TEdit;
    Bevel1: TBevel;
    Label5: TLabel;
    BitBtn1: TBitBtn;
    BitBtn2: TBitBtn;
    Label6: TLabel;
    Label7: TLabel;
    Label8: TLabel;
    cbUnom: TComboBox;
    cbVidNagr: TComboBox;
    cbType: TComboBox;
    Label9: TLabel;
    cbPodType: TComboBox;
    procedure eNewUstKeyPress(Sender: TObject; var Key: Char);
    procedure eNewCosKeyPress(Sender: TObject; var Key: Char);
    procedure FormCreate(Sender: TObject);
    procedure eNewUstChange(Sender: TObject);
    procedure eNewCosChange(Sender: TObject);
    procedure cbTypeSelect(Sender: TObject);
    procedure FormShow(Sender: TObject);
    procedure cbPodTypeSelect(Sender: TObject);
    procedure cbTypeDropDown(Sender: TObject);
    procedure cbPodTypeDropDown(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form3: TForm3;
  flag1:  byte;
  flag2:  byte;
  cash, cash1: Integer;
implementation

uses Unit1;

{$R *.dfm}


procedure TForm3.cbTypeSelect(Sender: TObject);
var
  aName : String;
begin
   if  cbType.ItemIndex=cbType.Items.Count-1 then
   begin
     if InputQuery('Новая группа потребителей', 'Введите наименование', aName) then
     begin
      if aName <> '' then
       begin
       if Length(aName)>30 then
         aName := Copy(aName,0,30);
       cbType.Items.Insert(cbType.ItemIndex, aName);
       cbType.ItemIndex:=cbType.Items.Count-2;
       FormMain.insQuery.close;
       FormMain.insQuery.SQL.Text := 'SELECT COUNT(*) FROM TYPE_OBJ WHERE NAME = ''' + Form3.cbType.Items[cbType.ItemIndex]+ '''';
       FormMain.insQuery.ExecQuery;
       if FormMain.insQuery.Fields[0].AsInteger = 0 then
        begin
         FormMain.insQuery.close;
         FormMain.insQuery.SQL.Text := 'INSERT INTO TYPE_OBJ (NAME) VALUES (''' + Form3.cbType.Items[cbType.ItemIndex] + ''')';
         FormMain.insQuery.ExecQuery;
        end;
       cbPodType.Items.Clear;
       cbPodType.Items.Add('Создать новую подгруппу...');
       cbPodType.ItemIndex:=0;
       end
       else
       cbType.ItemIndex:=cash;
     end
     else
     cbType.ItemIndex:=cash;
   end
   else if cbType.ItemIndex<>cash then
   begin
        cbPodType.Items.Clear;
        FormMain.insQuery.Close;
        FormMain.insQuery.SQL.Text := 'SELECT NAME FROM PODTYPE_OBJ WHERE ID_TYPE_OBJ='+IntToStr(cbType.ItemIndex)+' ORDER BY ID asc';
        FormMain.insQuery.ExecQuery;
        while not FormMain.insQuery.Eof  do
        begin
                cbPodType.Items.Add(FormMain.insQuery.Fields[0].AsString);
                FormMain.insQuery.Next;
        end;
        cbPodType.Items.Add('Создать новую подгруппу...');
        cbPodType.ItemIndex:=0;
   end;
end;

procedure TForm3.eNewCosChange(Sender: TObject);
begin
    If (eNewCos.Text=',') Then eNewCos.Text:='';
end;

procedure TForm3.eNewCosKeyPress(Sender: TObject; var Key: Char);
var
   i,j:Integer;
begin
  if Not (Key in ['0'..'9', ',', #8]) then Key:=#0;
  If flag1<>0 Then
    If Not (Key in ['0'..'9', #8]) then Key:=#0;
  If (Key=',') and (eNewCos.Text<>'') then flag1:=1;
  j:=1;
  If Key=#8 Then
  Begin
    For i:=1 To Length(eNewCos.Text)-1 Do
      If (eNewCos.Text[i]=',') Then
      begin
      j:=0;
      break;
      end;
    If j=0 Then flag1:=1 else flag1:=0;
  End;
end;

procedure TForm3.eNewUstChange(Sender: TObject);
begin
   If (eNewUst.Text=',') Then eNewUst.Text:='';
end;

procedure TForm3.eNewUstKeyPress(Sender: TObject; var Key: Char);
var
   i,j:Integer;
begin
   if Not (Key in ['0'..'9', ',', #8]) then Key:=#0;
  If flag2<>0 Then
    If Not (Key in ['0'..'9', #8]) then Key:=#0;
  If (Key=',') and (eNewUst.Text<>'') then flag2:=1;
  j:=1;
  If Key=#8 Then
  Begin
    For i:=1 To Length(eNewUst.Text)-1 Do
      If (eNewUst.Text[i]=',') Then
      begin
      j:=0;
      break;
      end;
    If j=0 Then flag2:=1 else flag2:=0;
  End;
end;

procedure TForm3.FormCreate(Sender: TObject);
begin
   flag1:=0;
   flag2:=0;
end;

procedure TForm3.FormShow(Sender: TObject);
begin
    eNewUst.Text :='';
    eNewCos.Text :='';
    eNewDescr.Text :='';
    eNewName.Text := 'Новый объект';
    eNewName.SetFocus;
    eNewName.SelectAll;
end;

procedure TForm3.cbPodTypeSelect(Sender: TObject);
var
  aName : String;
begin
   if  cbPodType.ItemIndex=cbPodType.Items.Count-1 then
   begin
     if InputQuery('Новая подгруппа потребителей в группе "'+cbType.Items[cbType.ItemIndex]+'"', 'Введите наименование', aName) then
     begin
      if aName <> '' then
       begin
       if Length(aName)>50 then
         aName := Copy(aName,0,50);
       cbPodType.Items.Insert(cbPodType.ItemIndex, aName);
       cbPodType.ItemIndex:=cbPodType.Items.Count-2;
       FormMain.insQuery.close;
       FormMain.insQuery.SQL.Text := 'SELECT COUNT(*) FROM PODTYPE_OBJ WHERE NAME = ''' + cbPodType.Items[cbPodType.ItemIndex]+ ''' AND ID_TYPE_OBJ=' + IntToStr(cbType.ItemIndex);
       FormMain.insQuery.ExecQuery;
       if FormMain.insQuery.Fields[0].AsInteger = 0 then
        begin
         FormMain.insQuery.close;
         FormMain.insQuery.SQL.Text := 'INSERT INTO PODTYPE_OBJ (ID_TYPE_OBJ, NAME) VALUES ('+ IntToStr(cbType.ItemIndex) +',''' + cbPodType.Items[cbPodType.ItemIndex] + ''')';
         FormMain.insQuery.ExecQuery;
        end;
       end
       else
       cbPodType.ItemIndex:=cash1;
     end
     else
     cbPodType.ItemIndex:=cash1;
   end;
end;

procedure TForm3.cbTypeDropDown(Sender: TObject);
begin
    cash:= cbType.ItemIndex;
end;

procedure TForm3.cbPodTypeDropDown(Sender: TObject);
begin
        cash1:= cbPodType.ItemIndex;
end;

end.

unit Unit2;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, Buttons, Spin, ExtCtrls, Unit1;

type
 // ['{F37D3F4D-778C-46D0-85D2-E271C8BFA769}']
 // ['{25A0CA13-24E9-43DB-B1A5-EF6179E10478}']
  //['{5E7B09C5-2A0D-4ED1-A63E-E7CFFDD01D32}']
 // ['{5CCBADF7-DB12-4C00-97DB-40397423CF64}']
 // ['{F43A7C6D-9027-4A8B-A424-8725948C1F8A}']
  TGrParamsForm = class(TForm)
    Label2: TLabel;
    Label3: TLabel;
    Label4: TLabel;
    cbSezon: TComboBox;
    BitBtn1: TBitBtn;
    BitBtn2: TBitBtn;
    Bevel1: TBevel;
    cbGenerate: TCheckBox;
    tim1: TRadioButton;
    Tim2: TRadioButton;
    cbDay: TComboBox;
    Bevel2: TBevel;
    procedure FormCreate(Sender: TObject);
    procedure cbGenerateClick(Sender: TObject);
    procedure Tim2Click(Sender: TObject);
    procedure tim1Click(Sender: TObject);
    procedure cbSezonSelect(Sender: TObject);

  private
    procedure FillCombos;
  public
  edTime: Integer;  // 0 - не генерировать, 1-по 30 мин , 2- по 60 мин
    { Public declarations }
  end;

var
  GrParamsForm: TGrParamsForm;
  curType: Integer;

implementation

{$R *.dfm}



procedure TGrParamsForm.FillCombos;
var
  iSezon: TSezonTypes;
  iDay: TDaysTypes;
begin
  cbSezon.Clear;
  for iSezon := Low(TSezonTypes) to High(TSezonTypes) do
    cbSezon.Items.Add(cSezonTypesDesc[iSezon]);
  cbDay.Clear;
  for iDay := Low(TDaysTypes) to High(TDaysTypes) do
     cbDay.Items.Add(cDaysTypesDesc[iDay]);
  cbDay.Enabled:=false;
end;

procedure TGrParamsForm.FormCreate(Sender: TObject);
begin
  FillCombos;
  if(not cbGenerate.Checked) then
   edTime:=0
  else if tim1.Checked  then
   edTime:=1
  else
   edTime:=2;
end;

procedure TGrParamsForm.tim1Click(Sender: TObject);
begin
    tim1.Checked := true;
    tim2.Checked := false;
    edTime:=1;
end;

procedure TGrParamsForm.Tim2Click(Sender: TObject);begin
    tim2.Checked := true;
    tim1.Checked := false;
    edTime:=2;
end;

  procedure TGrParamsForm.cbGenerateClick(Sender: TObject);
begin
   if(not cbGenerate.Checked) then
   begin
     tim1.Enabled :=false;
     tim2.Enabled :=false;
     label4.Enabled := false;
     edTime:=0;
   end
   else
   begin
      tim1.Enabled :=true;
      tim2.Enabled :=true;
      label4.Enabled := true;
      if(tim1.Checked) then
       edTime:=1
      else
       edTime:=2;
   end;

end;

procedure TGrParamsForm.cbSezonSelect(Sender: TObject);
begin
    cbDay.Enabled :=true;
end;

end.

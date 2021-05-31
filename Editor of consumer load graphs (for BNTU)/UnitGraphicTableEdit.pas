unit UnitGraphicTableEdit;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, Grids, DBGrids, TitleGrid, ExtCtrls, TeeProcs, TeEngine, Chart,
  StdCtrls, Buttons, DBGridEh, DB, FIBDataSet, pFIBDataSet, FIBQuery,
  pFIBQuery, Series;

type
    TFormGraphicTableEdit = class(TForm)
    Chart1: TChart;
    BitBtn1: TBitBtn;
    BitBtn2: TBitBtn;
    Grid: TDBGridPlus;
    DataSource1: TDataSource;
    GRvalues: TpFIBDataSet;
    GRvaluesGR_VALUE: TFIBFloatField;
    GRvaluesID: TFIBIntegerField;
    GRvaluesID_GR_OBJECTS: TFIBIntegerField;
    GRvaluesGR_TYPE: TFIBIntegerField;
    GRvaluesGR_TIME: TFIBIntegerField;
    GRvaluesTB_TIME: TStringField;
    Series1: TBarSeries;
    procedure FormClose(Sender: TObject; var Action: TCloseAction);
    procedure GRvaluesCalcFields(DataSet: TDataSet);
    procedure GRvaluesNewRecord(DataSet: TDataSet);
    procedure GRvaluesBeforeDelete(DataSet: TDataSet);
    procedure GRvaluesAfterPost(DataSet: TDataSet);
  private
    procedure Add_Point_To_Series;
    { Private declarations }
  public
    procedure Load_Gr(curr_obj, curr_img: Integer);
    { Public declarations }
  end;


var
  FormGraphicTableEdit: TFormGraphicTableEdit;
  change : boolean;

implementation
uses
   UnitGraphicEditor, UnitDM;

{$R *.dfm}
procedure TFormGraphicTableEdit.Load_Gr(curr_obj: Integer; curr_img: Integer);
begin
  GRvalues.SelectSQL.Text := 'SELECT * FROM CAT_GR_VALUES WHERE '+
   'ID_GR_OBJECTS='+IntToStr(curr_obj)+ ' AND GR_TYPE=' + IntToStr(curr_img)+
   ' ORDER BY GR_TIME';
   GRvalues.AutoUpdateOptions.UpdateTableName :='CAT_GR_VALUES';
  try
   GRvalues.Open;
   GRvalues.GenerateSQLs;
   Add_Point_To_Series;
   change:=false;
  except
   GRvalues.Close;
  end;

end;



procedure TFormGraphicTableEdit.FormClose(Sender: TObject;
  var Action: TCloseAction);
begin
    GRvalues.Close;
end;


procedure TFormGraphicTableEdit.GRvaluesCalcFields(DataSet: TDataSet);
Function GetTextTime(index: Integer): string;
  begin
    result:=IntToStr(Trunc(index/2));
    if index mod 2 = 0 then
      result:= result+':00'
    else
      result:= result+':30';
  end;
begin
  inherited;
  GRvaluesTB_TIME.AsString:=GetTextTime(GRvaluesGR_TIME.AsInteger);
end;

procedure TFormGraphicTableEdit.Add_Point_To_Series;
begin
    Series1.Clear;
    GRvalues.First;
    while not GRvalues.Eof do
        begin
        Series1.AddY(GRvalues.fieldbyname('GR_VALUE').AsFloat,GRvalues.fieldbyname('TB_TIME').AsString);
        GRvalues.Next
        end;
    GRvalues.First;
end;

procedure TFormGraphicTableEdit.GRvaluesNewRecord(DataSet: TDataSet);
begin
  inherited;
  if GRvalues.RecordCount>=24 then Abort;
end;


procedure TFormGraphicTableEdit.GRvaluesBeforeDelete(DataSet: TDataSet);
begin
  Abort;
end;

procedure TFormGraphicTableEdit.GRvaluesAfterPost(DataSet: TDataSet);
begin
   if not change then
   begin
   if DataSet.FieldByName('GR_VALUE').AsFloat >1 then
      begin
      DataSet.Edit;
      DataSet.FieldByName('GR_VALUE').AsFloat :=1;
      change:=true;
      DataSet.Post;
      end
   else if DataSet.FieldByName('GR_VALUE').AsFloat<0 then
      begin
      DataSet.Edit;
      DataSet.FieldByName('GR_VALUE').AsFloat :=0;
      change:=true;
      DataSet.Post;
      end
   end
   else
      change:=false;
   Series1.YValue[DataSet.FieldByName('GR_TIME').AsInteger-1]:=DataSet.FieldByName('GR_VALUE').AsFloat;
end;

end.


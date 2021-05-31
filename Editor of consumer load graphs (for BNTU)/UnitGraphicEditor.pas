unit UnitGraphicEditor;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, ExtCtrls, StdCtrls, DB, IBCustomDataSet, IBQuery, ActnList,
  ImgList, ComCtrls, UnitDM, IBDatabase,FibDataSet, Buttons, pFIBDataSet,
  Grids, DBGridEh, DBGrids, StrUtils,Math;

type
  TFormGraphicEditor = class(TForm)
    Panel1: TPanel;
    cbGroup: TComboBox;
    Label1: TLabel;
    Label2: TLabel;
    LvGraf: TListView;
    ImageList1: TImageList;
    cbPodGroup: TComboBox;
    Label3: TLabel;
    btNewItem: TBitBtn;
    btCopyItem: TBitBtn;
    btDeleteItem: TBitBtn;
    ImageList2: TImageList;
    GRDataset: TpFIBDataSet;
    SpeedButton1: TSpeedButton;
    SpeedButton2: TSpeedButton;
    SpeedButton5: TSpeedButton;
    SpeedButton6: TSpeedButton;
    Image16: TImage;
    Image15: TImage;
    Image14: TImage;
    Image13: TImage;
    Image1: TImage;
    Image2: TImage;
    Image3: TImage;
    Image4: TImage;
    Image5: TImage;
    Image6: TImage;
    Image7: TImage;
    Image8: TImage;
    Label4: TLabel;
    Label5: TLabel;
    Image9: TImage;
    Image10: TImage;
    Image11: TImage;
    Image12: TImage;
    btClose: TImage;
    btEdit: TImage;
    btOk: TImage;
    btCancel: TImage;
    Label6: TLabel;
    btTable: TImage;
    procedure FormShow(Sender: TObject);
    procedure cbGroupSelect(Sender: TObject);
    procedure cbGroupDropDown(Sender: TObject);
    procedure cbPodGroupDropDown(Sender: TObject);
    procedure cbPodGroupSelect(Sender: TObject);
    procedure btNewItemClick(Sender: TObject);
    procedure LvGrafSelectItem(Sender: TObject; Item: TListItem;
      Selected: Boolean);
    procedure Image1MouseMove(Sender: TObject; Shift: TShiftState; X,
      Y: Integer);
    procedure FormMouseMove(Sender: TObject; Shift: TShiftState; X,
      Y: Integer);
    procedure Image_Mouse_Move(img: TImage;i:Integer; x: Integer; y:Integer);
    procedure Image5MouseMove(Sender: TObject; Shift: TShiftState; X,
      Y: Integer);
    procedure Image3MouseMove(Sender: TObject; Shift: TShiftState; X,
      Y: Integer);
    procedure Image7MouseMove(Sender: TObject; Shift: TShiftState; X,
      Y: Integer);
    procedure Image2MouseMove(Sender: TObject; Shift: TShiftState; X,
      Y: Integer);
    procedure Image4MouseMove(Sender: TObject; Shift: TShiftState; X,
      Y: Integer);
    procedure Image6MouseMove(Sender: TObject; Shift: TShiftState; X,
      Y: Integer);
    procedure Image8MouseMove(Sender: TObject; Shift: TShiftState; X,
      Y: Integer);
    procedure Image1Click(Sender: TObject);
    procedure FormClose(Sender: TObject; var Action: TCloseAction);
    procedure btCloseClick(Sender: TObject);
    procedure btEditClick(Sender: TObject);
    procedure btCancelClick(Sender: TObject);
    procedure btOkClick(Sender: TObject);
    procedure Image1MouseDown(Sender: TObject; Button: TMouseButton;
      Shift: TShiftState; X, Y: Integer);
    procedure Image1MouseUp(Sender: TObject; Button: TMouseButton;
      Shift: TShiftState; X, Y: Integer);
    procedure FormClick(Sender: TObject);
    procedure Image2Click(Sender: TObject);
    procedure Image3Click(Sender: TObject);
    procedure Image4Click(Sender: TObject);
    procedure Image5Click(Sender: TObject);
    procedure Image6Click(Sender: TObject);
    procedure Image7Click(Sender: TObject);
    procedure Image8Click(Sender: TObject);
    procedure Image8MouseDown(Sender: TObject; Button: TMouseButton;
      Shift: TShiftState; X, Y: Integer);
    procedure Image7MouseDown(Sender: TObject; Button: TMouseButton;
      Shift: TShiftState; X, Y: Integer);
    procedure Image6MouseDown(Sender: TObject; Button: TMouseButton;
      Shift: TShiftState; X, Y: Integer);
    procedure Image5MouseDown(Sender: TObject; Button: TMouseButton;
      Shift: TShiftState; X, Y: Integer);
    procedure Image4MouseDown(Sender: TObject; Button: TMouseButton;
      Shift: TShiftState; X, Y: Integer);
    procedure Image3MouseDown(Sender: TObject; Button: TMouseButton;
      Shift: TShiftState; X, Y: Integer);
    procedure Image2MouseDown(Sender: TObject; Button: TMouseButton;
      Shift: TShiftState; X, Y: Integer);
    procedure Image2MouseUp(Sender: TObject; Button: TMouseButton;
      Shift: TShiftState; X, Y: Integer);
    procedure Image3MouseUp(Sender: TObject; Button: TMouseButton;
      Shift: TShiftState; X, Y: Integer);
    procedure Image4MouseUp(Sender: TObject; Button: TMouseButton;
      Shift: TShiftState; X, Y: Integer);
    procedure Image5MouseUp(Sender: TObject; Button: TMouseButton;
      Shift: TShiftState; X, Y: Integer);
    procedure Image6MouseUp(Sender: TObject; Button: TMouseButton;
      Shift: TShiftState; X, Y: Integer);
    procedure Image7MouseUp(Sender: TObject; Button: TMouseButton;
      Shift: TShiftState; X, Y: Integer);
    procedure Image8MouseUp(Sender: TObject; Button: TMouseButton;
      Shift: TShiftState; X, Y: Integer);
    procedure btDeleteItemClick(Sender: TObject);
    procedure btCopyItemClick(Sender: TObject);
    procedure LvGrafEdited(Sender: TObject; Item: TListItem;
      var S: String);
    procedure Image1DblClick(Sender: TObject);
    procedure Image2DblClick(Sender: TObject);
    procedure Image3DblClick(Sender: TObject);
    procedure Image4DblClick(Sender: TObject);
    procedure Image5DblClick(Sender: TObject);
    procedure Image6DblClick(Sender: TObject);
    procedure Image7DblClick(Sender: TObject);
    procedure Image8DblClick(Sender: TObject);
    procedure Image2DragOver(Sender, Source: TObject; X, Y: Integer;
      State: TDragState; var Accept: Boolean);
    procedure Image1DragOver(Sender, Source: TObject; X, Y: Integer;
      State: TDragState; var Accept: Boolean);
    procedure Image3DragOver(Sender, Source: TObject; X, Y: Integer;
      State: TDragState; var Accept: Boolean);
    procedure Image5DragOver(Sender, Source: TObject; X, Y: Integer;
      State: TDragState; var Accept: Boolean);
    procedure Image6DragOver(Sender, Source: TObject; X, Y: Integer;
      State: TDragState; var Accept: Boolean);
    procedure Image7DragOver(Sender, Source: TObject; X, Y: Integer;
      State: TDragState; var Accept: Boolean);
    procedure Image8DragOver(Sender, Source: TObject; X, Y: Integer;
      State: TDragState; var Accept: Boolean);
    procedure Image4DragOver(Sender, Source: TObject; X, Y: Integer;
      State: TDragState; var Accept: Boolean);
    procedure Image3DragDrop(Sender, Source: TObject; X, Y: Integer);
    procedure Image1DragDrop(Sender, Source: TObject; X, Y: Integer);
    procedure Image2DragDrop(Sender, Source: TObject; X, Y: Integer);
    procedure Image4DragDrop(Sender, Source: TObject; X, Y: Integer);
    procedure Image5DragDrop(Sender, Source: TObject; X, Y: Integer);
    procedure Image6DragDrop(Sender, Source: TObject; X, Y: Integer);
    procedure Image7DragDrop(Sender, Source: TObject; X, Y: Integer);
    procedure Image8DragDrop(Sender, Source: TObject; X, Y: Integer);
    procedure btTableClick(Sender: TObject);
    procedure SpeedButton1Click(Sender: TObject);
    procedure SpeedButton2Click(Sender: TObject);
    procedure SpeedButton5Click(Sender: TObject);
    procedure SpeedButton6Click(Sender: TObject);
    procedure Panel1MouseMove(Sender: TObject; Shift: TShiftState; X,
      Y: Integer);
    procedure LvGrafMouseMove(Sender: TObject; Shift: TShiftState; X,
      Y: Integer);

  private
    procedure Paint_User_Graphic(img: TImage);
    procedure End_Edit();
    procedure Image_Mouse_Click(img: TImage; i: Integer);
    procedure Create_Graphic (img: TImage; i: Integer);
    procedure New_Graphic (img: TImage; i: Integer);
    procedure Load_Objects(id_gr: Integer ; name_podgroup: String);
    procedure Select_Podgroup(name_Group: String);
    procedure Update_Img_Count();
    procedure Load_Graphic(i: Integer);
    procedure Paint_Graphic(img: TImage);
    procedure Img_Mouse_Down(img: TImage; x, y: Integer);
    procedure Img_Enable(val: Boolean);
    procedure Start_Edit;
    procedure End_Drag(img: TImage; i: Integer);
    function Return_Image: TImage;
    function BinToDec(const Value1, Value2: String): Integer;
    procedure Buttons_enabled(val_group, val_podgr: boolean);
    procedure Form_Mouse_Move;
    procedure All_Images_Visible(val: boolean);
    { Private declarations }
  public

    { Public declarations }
  end;

var
  FormGraphicEditor: TFormGraphicEditor;
  cash: Integer;                      // временный индекс для возврата к предыдущему положению при отмене
  editMode : boolean;                 // активен ли режим редактирования
  curr_obj, curr_img: Integer;        // текущий объект, текущее изображение
  emptyImage: array [0..7] of Boolean;// определяет пустые графики
  gr_values: array [0..48] of Real;   // массив значений активного графика
  img_gr_values: array [0..192] of Integer;
  doDraw, Drag_Mode: boolean;          // зажата ли клавиша мыши для черчения графика
 // cashImg: TPicture;                  // память для предыдущего графика

implementation

uses
  UnitFind, {UnitBdeAdmin}UnitFibAdmin, UnitMain, UnitGraphicTableEdit;

{$R *.dfm}

procedure TFormGraphicEditor.FormShow(Sender: TObject);
var
  bmp: TBitMap;
  i,j: Integer;
  mas: Array[0..7] of Boolean;
begin
  cbGroup.Items.Clear;
  GRDataset.SelectSQL.Text :='SELECT * FROM CAT_GR_POTREB ORDER BY ID';
  try
   GRDataset.Open;
   GRDataset.First;
   while not GRDataset.Eof do
   begin
     cbGroup.Items.Add(GRDataset.FieldByName('NAME').AsString);
     GRDataset.Next;
   end;
   cbGroup.Items.Add('Создать новую группу...');
   cbGroup.ItemIndex:=0;
  finally
   GRDataset.Close;
  end;
  Select_Podgroup(cbGroup.Items[cbGroup.ItemIndex]);
  editMode:=false;
  curr_img:=-1;
  doDraw:=false;
  Drag_Mode:=false;
  if ImageList1.Count =1 then
  begin
     bmp:=TBitmap.Create;
     bmp.Width:= 80;
     bmp.Height:=40;
     For i:=0 to 7 do
        mas[i]:=false;
     For i:=1 to 255 do
     begin
      ImageList1.GetBitmap(0, Bmp);
        if not mas[0] then
           mas[0]:=true
        else
        begin
          mas[0]:=false;
          if not mas[1] then
           mas[1]:=true
           else
           begin
             mas[1]:=false;
             if not mas[2] then
              mas[2]:=true
             else
              begin
                mas[2]:=false;
                if not mas[3] then
                mas[3]:=true
                else
                begin
                   mas[3]:=false;
                  if not mas[4] then
                   mas[4]:=true
                  else
                  begin
                    mas[4]:=false;
                    if not mas[5] then
                     mas[5]:=true
                    else
                    begin
                      mas[5]:=false;
                      if not mas[6] then
                      mas[6]:=true
                      else
                        begin
                            mas[6]:=false;
                          if not mas[7] then
                             mas[7]:=true
                          else
                            begin
                              ShowMessage('ошибка при i='+IntToStr(i));
                            end;
                        end;
                    end;
                  end;
                end;
              end;
           end;
        end;
      with bmp.Canvas do
      begin
      brush.Color :=clLime;
        For j:=0 to 3 do
          If mas[j] then
           Rectangle(35+j*10, 11, j*10+46, 24);
        For j:=4 to 7 do
          If mas[j] then
           Rectangle(35+(j-4)*10, 25, (j-4)*10+46, 38);
      end;
      ImageList1.Add(bmp,nil);
     end;
     bmp.Free;
   end;
   if cbGroup.Items.Count>1 then
       begin
       if cbPodGroup.Items.Count>1 then
          Buttons_enabled(true,true)
       else
          Buttons_enabled(true,false);
       end;
end;

procedure TFormGraphicEditor.Select_Podgroup(name_Group: String);
var
  id: Integer;
begin
   cbPodGroup.Items.Clear;
   GRDataset.SelectSQL.Text  := 'SELECT * FROM CAT_GR_POTREB WHERE NAME=''' + name_Group + '''';
   try
    GRDataset.Open;
    id:=GRDataset.fieldByName('ID').AsInteger ;
    GRDataset.Close;
    GRDataset.SelectSQL.Text := 'SELECT * FROM CAT_PODGR_POTREB WHERE ID_GR_POTREB=' + IntToStr(id)+' ORDER BY ID';
    GRDataset.Open;
    GRDataset.First;
    while not GRDataset.Eof do
    begin
     cbPodGroup.Items.Add(GRDataset.FieldByName('NAME').AsString);
     GRDataset.Next;
    end;
    cbPodGroup.Items.Add('Создать новую подгруппу...');
    cbPodGroup.ItemIndex:=0;
   finally
    GRDataset.Close;
   end;

  //подгрузить графики
  if cbPodGroup.Items.Count >1 then
      begin
      Load_Objects(id, cbPodGroup.Items[cbPodGroup.ItemIndex]);
       btNewItem.Enabled :=true;
      end
  else
    begin
       LvGraf.Items.Clear;
       All_Images_Visible(false);
       btNewItem.Enabled :=false;
       btCopyItem.Enabled :=false;
       btDeleteItem.Enabled :=false;
    end;
end;


procedure TFormGraphicEditor.cbGroupSelect(Sender: TObject);
var
  aName : String;
begin
   if  cbGroup.ItemIndex=cbGroup.Items.Count-1 then
   begin
     if InputQuery('Новая группа потребителей', 'Введите наименование', aName) then
     begin
      if aName <> '' then
       begin
       if Length(aName)>30 then
         aName := Copy(aName,0,30);
       cbGroup.Items.Insert(cbGroup.ItemIndex, aName);
       cbGroup.ItemIndex:=cbGroup.Items.Count-2;
       GRDataset.SelectSQL.Text := 'SELECT COUNT(*) FROM CAT_GR_POTREB WHERE NAME = '''
        + cbGroup.Items[cbGroup.ItemIndex]+ '''';
       try
        GRDataset.Open;
        GRDataset.First;
        if GRDataset.Fields[0].AsInteger = 0 then
         begin
          GRDataset.Close;
          GRDataset.SelectSQL.Text := 'SELECT * FROM CAT_GR_POTREB';
          GRDataset.AutoUpdateOptions.UpdateTableName :='CAT_GR_POTREB';
          GRDataset.AutoUpdateOptions.GeneratorName :='GEN_CAT_GR_POTREB_ID';
          GRDataset.Open;
          GRDataset.GenerateSQLs;
          GRDataset.Append;
          GRDataset.FieldByName('NAME').AsString := cbGroup.Items[cbGroup.ItemIndex];
          GRDataset.Post;
         end;
       finally
        GRDataset.Close;
       end;
       cbPodGroup.Items.Clear;
       cbPodGroup.Items.Add('Создать новую подгруппу...');
       cbPodGroup.ItemIndex:=0;
       LvGraf.Items.Clear;
       All_Images_Visible(false);
       btNewItem.Enabled :=false;
       btCopyItem.Enabled :=false;
       btDeleteItem.Enabled :=false;
       Buttons_enabled(true,false);
       end
       else
       cbGroup.ItemIndex:=cash;
     end
     else
     cbGroup.ItemIndex:=cash;
   end
   else if cbGroup.ItemIndex<>cash then
   begin
        cash:=cbGroup.ItemIndex;
        Select_Podgroup(cbGroup.Items[cbGroup.ItemIndex]);
   end;
end;

procedure TFormGraphicEditor.cbGroupDropDown(Sender: TObject);
begin
    cash:= cbGroup.ItemIndex;
end;

procedure TFormGraphicEditor.cbPodGroupDropDown(Sender: TObject);
begin
    cash:= cbPodGroup.ItemIndex;
end;

procedure TFormGraphicEditor.cbPodGroupSelect(Sender: TObject);
var
  aName : String;
  id: Integer;
begin
   if  cbPodGroup.ItemIndex=cbPodGroup.Items.Count-1 then
   begin
     if InputQuery('Новая подгруппа потребителей', 'Введите наименование', aName) then
     begin
       if aName <> '' then
       begin
         if Length(aName)>50 then
            aName := Copy(aName,0,50);
         cbPodGroup.Items.Insert(cbPodGroup.ItemIndex, aName);
         cbPodGroup.ItemIndex:=cbPodGroup.Items.Count-2;
         GRDataset.SelectSQL.Text:='SELECT * FROM CAT_GR_POTREB WHERE NAME = '''
          + cbGroup.Items[cbGroup.ItemIndex]+ '''';
         try
          GRDataset.Open;
          GRDataset.First;
          id:= GRDataset.FieldByName('ID').AsInteger;
         finally
          GRDataset.Close;
         end;
         GRDataset.SelectSQL.Text := 'SELECT COUNT(*) FROM CAT_PODGR_POTREB WHERE NAME = '''
         + cbPodGroup.Items[cbPodGroup.ItemIndex]+ ''' AND ID_GR_POTREB = ' + IntToStr(id);
         try
          GRDataset.Open;
          GRDataset.First;
          if GRDataset.Fields[0].AsInteger = 0 then
           begin
           GRDataset.Close;
           GRDataset.SelectSQL.Text := 'SELECT * FROM CAT_PODGR_POTREB';
           GRDataset.AutoUpdateOptions.UpdateTableName :='CAT_PODGR_POTREB';
           GRDataset.AutoUpdateOptions.GeneratorName :='GEN_CAT_PODGR_POTREB_ID';
           GRDataset.Open;
           GRDataset.GenerateSQLs;
           GRDataset.Append;
           GRDataset.FieldByName('ID_GR_POTREB').AsInteger := id;
           GRDataset.FieldByName('NAME').AsString := cbPodGroup.Items[cbPodGroup.ItemIndex] ;
           GRDataset.Post;
           GRDataset.Close;
           GRDataset.SelectSQL.Text := 'SELECT MAX(ID) FROM CAT_PODGR_POTREB';
           GRDataset.Open;
           id:=GRDataset.Fields[0].AsInteger;
           GRDataset.Close;
           Buttons_enabled(true,true);
           Load_Objects(id, cbPodGroup.Items[cbPodGroup.ItemIndex]);
           end;
         finally
          GRDataset.Close;
         end;
         btNewItem.Enabled :=true;
       end
       else
         cbPodGroup.ItemIndex:=cash;
     end
     else
       cbPodGroup.ItemIndex:=cash;
   end
   else if cbPodGroup.ItemIndex<>cash then
   begin
       GRDataset.SelectSQL.Text :='SELECT * FROM CAT_GR_POTREB WHERE NAME = '''
       + cbGroup.Items[cbGroup.ItemIndex]+ '''';
       try
        GRDataset.Open;
        id:= GRDataset.FieldByName('ID').AsInteger;
       finally
        GRDataset.Close;
       end;
       Load_Objects(id, cbPodGroup.Items[cbPodGroup.ItemIndex]);
   end;
end;

function TFormGraphicEditor.BinToDec(const Value1: String;const Value2: String): Integer;
var i: Integer;
    S,S1,S2: String;
    X: Extended;
begin
  S1:= Value1;
  while Length(S1)<>4 do
     S1:='0'+S1;
  S2 := Value2;
  while Length(S2)<>4 do
     S2:='0'+S2;
  S:=S1+S2;
  X := 0;
  for i := 1 to Length(S) do
    X := X + StrToInt(S[i])*(Power(2, i-1));
  Result := Trunc(X);
end;

procedure TFormGraphicEditor.Load_Objects(id_gr: Integer; name_podgroup: String);
var
 id_podgr: Integer;
 LI: TListItem;
begin
   LvGraf.Items.Clear;
   GRDataset.SelectSQL.Text := 'SELECT * FROM CAT_PODGR_POTREB WHERE NAME='''
    + name_podgroup + ''' AND ID_GR_POTREB='+IntToStr(id_gr);
   try
    GRDataset.Open;
    id_podgr:=GRDataset.fieldByName('ID').AsInteger ;
   finally
    GRDataset.Close;
   end;
  GRDataset.SelectSQL.Text := 'SELECT * FROM CAT_GR_OBJECTS WHERE ID_GR_POTREB = '
  +IntToStr(id_gr)+' AND ID_PODGR_POTREB='+ IntToStr(id_podgr) + 'ORDER BY ID';
  try
  GRDataset.Open;
  GRDataset.First;
  curr_obj:=GRDataset.FieldByName('ID').AsInteger;
  while not GRDataset.Eof do
  begin
     LI:=LvGraf.Items.add;
     LI.Caption:= GRDataset.FieldByName('NAME').AsString;
     LI.SubItems.Add(IntToStr(GRDataset.FieldByName('RAB_GR').AsInteger)+' '+
     IntToStr(GRDataset.FieldByName('VIH_GR').AsInteger));
     LI.SubItems.Add(IntToStr(GRDataset.FieldByName('ID').AsInteger));
     LI.ImageIndex := BinToDec(GRDataset.FieldByName('RAB_GR').AsString,
     GRDataset.FieldByName('VIH_GR').AsString);
     GRDataset.Next;
  end;
  finally
   GRDataset.Close;
  end;
  if LvGraf.Items.Count<>0 then
        begin
        if Image1.Visible=false then
          All_Images_Visible(true);
        LvGraf.SetFocus;
        LvGraf.Selected := LvGraf.Items[0];
        LvGraf.ItemFocused  := LvGraf.Items[0];
        end
  else
     All_Images_Visible(false);
   if LvGraf.Items.Count =0 then
      begin
          btCopyItem.Enabled :=false;
          btDeleteItem.Enabled :=false;
      end
   else
      begin
          btCopyItem.Enabled :=true;
          btDeleteItem.Enabled :=true;
      end;
end;

procedure TFormGraphicEditor.btNewItemClick(Sender: TObject);
var
 id_gr,id_podgr: Integer;
 LI: TListItem;
begin
   LI:=LvGraf.Items.add;
   Li.ImageIndex :=0;
   LI.Caption:= 'Новый объект';
   LI.SubItems.Add('0000 0000');


   GRDataset.SelectSQL.Text := 'SELECT * FROM CAT_GR_POTREB WHERE NAME='''
   + cbGroup.Items[cbGroup.ItemIndex] + '''';
   try
    GRDataset.Open;
    id_gr:=GRDataset.fieldByName('ID').AsInteger ;
   finally
   GRDataset.Close;
   end;
    GRDataset.SelectSQL.Text := 'SELECT * FROM CAT_PODGR_POTREB WHERE NAME='''
   + cbPodGroup.Items[cbPodGroup.ItemIndex] + ''' AND ID_GR_POTREB='+ IntToStr(id_gr);
   try
    GRDataset.Open;
    id_podgr:=GRDataset.fieldByName('ID').AsInteger;
   finally
    GRDataset.Close;
   end;


   GRDataset.SelectSQL.Text :='SELECT * FROM CAT_GR_OBJECTS';
   GRDataset.AutoUpdateOptions.UpdateTableName :='CAT_GR_OBJECTS';
   GRDataset.AutoUpdateOptions.GeneratorName :='GEN_CAT_GR_OBJECTS_ID';
   try
   GRDataset.Open;
   GRDataset.GenerateSQLs;
   GRDataset.Close;
   GRDataset.Open;
   GRDataset.Append;
   GRDataset.FieldByName('ID_GR_POTREB').AsInteger := id_gr;
   GRDataset.FieldByName('ID_PODGR_POTREB').AsInteger:= id_podgr;
   GRDataset.FieldByName('NAME').AsString := 'Новый объект';
   GRDataset.FieldByName('RAB_GR').AsInteger:= 0000;
   GRDataset.FieldByName('VIH_GR').AsInteger:= 0000;
   GRDataset.Post;
   finally
   GRDataset.Close;
   end;
   GRDataset.SelectSQL.Text := 'SELECT MAX(ID) FROM CAT_GR_OBJECTS';
   try
   GRDataset.Open;
   LI.SubItems.Add(GRDataset.Fields[0].AsString);
   finally
   GRDataset.Close;
   end;
   if Image1.Visible=false then
   begin
       All_Images_Visible(true);
   end;
      if LvGraf.Items.Count =0 then
      begin
          btCopyItem.Enabled :=false;
          btDeleteItem.Enabled :=false;
      end
   else
      begin
          btCopyItem.Enabled :=true;
          btDeleteItem.Enabled :=true;
      end;
   LvGraf.Selected := LI;
   LvGraf.ItemFocused := LI;
   LI.EditCaption;
end;

procedure TFormGraphicEditor.LvGrafSelectItem(Sender: TObject;
  Item: TListItem; Selected: Boolean);
var
   str: String;
begin
   GRDataset.SelectSQL.Text :='SELECT * FROM CAT_GR_OBJECTS WHERE ID=' + Item.SubItems[1];
   try
    GRDataset.Open;
    curr_obj:= StrToInt(Item.SubItems[1]);
    str:= GRDataset.FieldByName('RAB_GR').AsString;
   finally
    GRDataset.Close;
   end;
   While Length(str)<>4 do
       str:='0'+str;
   If str[1]='0' then
        begin
        Image1.Picture:=nil;
        ImageList2.GetBitmap(0, Image1.Picture.Bitmap);
        Image1.Repaint;
        emptyImage[0]:=true;
        end
   else
       begin
       Create_Graphic(Image1,0);
       Load_Graphic(0);
       Paint_Graphic(Image1);
       emptyImage[0]:=False;
       end;
   If str[2]='0' then
        begin
        Image3.Picture:=nil;
        ImageList2.GetBitmap(0, Image3.Picture.Bitmap);
        image3.Repaint;
        emptyImage[2]:=true;
        end
   else
       begin
       Create_Graphic(Image3,2);
       Load_Graphic(2);
       Paint_Graphic(Image3);
       emptyImage[2]:=False;
       end;
   If str[3]='0' then
        begin
        Image5.Picture:=nil;
        ImageList2.GetBitmap(0, Image5.Picture.Bitmap);
        Image5.Repaint;
        emptyImage[4]:=true;
        end
   else
       begin
       Create_Graphic(Image5,4);
       Load_Graphic(4);
       Paint_Graphic(Image5);
       emptyImage[4]:=False;
       end;
   If str[4]='0' then
        begin
        Image7.Picture:=nil;
        ImageList2.GetBitmap(0, Image7.Picture.Bitmap);
        Image7.Repaint;
        emptyImage[6]:=true;
        end
   else
       begin
       Create_Graphic(Image7,6);
       Load_Graphic(6);
       Paint_Graphic(Image7);
       emptyImage[6]:=False;
       end;
   GRDataset.SelectSQL.Text :='SELECT * FROM CAT_GR_OBJECTS WHERE ID=' + Item.SubItems[1];
   try
    GRDataset.Open;
    str:= GRDataset.FieldByName('VIH_GR').AsString;
   finally
    GRDataset.Close;
   end;
   While Length(str)<>4 do
       str:='0'+str;
   If str[1]='0' then
        begin
        Image2.Picture:=nil;
        ImageList2.GetBitmap(0, Image2.Picture.Bitmap);
        Image2.Repaint;
        emptyImage[1]:=true;
        end
   else
       begin
       Create_Graphic(Image2,1);
       Load_Graphic(1);
       Paint_Graphic(Image2);
       emptyImage[1]:=False;
       end;
   If str[2]='0' then
        begin
        Image4.Picture:=nil;
       ImageList2.GetBitmap(0, Image4.Picture.Bitmap);
       image4.Repaint;
       emptyImage[3]:=true;
       end
   else
       begin
       Create_Graphic(Image4,3);
       Load_Graphic(3);
       Paint_Graphic(Image4);
       emptyImage[3]:=False;
       end;
   If str[3]='0' then
        begin
        Image6.Picture:=nil;
        ImageList2.GetBitmap(0, Image6.Picture.Bitmap);
        Image6.Repaint;
        emptyImage[5]:=true;
        end
   else
       begin
       Create_Graphic(Image6,5);
       Load_Graphic(5);
       Paint_Graphic(Image6);
       emptyImage[5]:=False;
       end;
   If str[4]='0' then
        begin
        Image8.Picture:=nil;
        ImageList2.GetBitmap(0, Image8.Picture.Bitmap);
        Image8.Repaint;
        emptyImage[7]:=true;
        end
   else
       begin
       Create_Graphic(Image8,7);
       Load_Graphic(7);
       Paint_Graphic(Image8);
       emptyImage[7]:=False;
       end;
end;

procedure TFormGraphicEditor.Image1MouseMove(Sender: TObject;
  Shift: TShiftState; X, Y: Integer);
begin
   Image_Mouse_Move(Image1,0,x,y);
end;

procedure TFormGraphicEditor.FormMouseMove(Sender: TObject;
  Shift: TShiftState; X, Y: Integer);
begin
   Form_Mouse_Move
end;

procedure TFormGraphicEditor.Image_Mouse_Move(img: TImage;i:Integer; x: Integer; y:Integer);
begin

  if emptyImage[i] then
  begin
     if (curr_img<>i) and (not EditMode) then
     begin
     ImageList2.GetBitmap(1, img.Picture.Bitmap);
     img.Repaint;
     curr_img:=i;
     end;
  end
  else
  begin
    if not editMode then
    begin
       btClose.Left :=img.Left + 192;
       btClose.Top := img.Top;
       btClose.Visible :=True;
       btEdit.Left := img.Left + 130;
       btEdit.Top := img.Top;
       btEdit.Visible :=true;
       btTable.Left := img.Left + 161;
       btTable.Top := img.Top;
       btTable.Visible :=true;
       curr_img:=i;
    end
    else
       if doDraw then
       begin
          with img.Canvas do
          begin
            if x>212 then
              x:=212
            else if x<20 then
              x:=20;
            if y<5 then
               y:=5
            else if y>105 then
               y:=105;
            img_gr_values[x-20]:=y;
            LineTo(x,y);
          end;
       end;
    end;
end;

procedure TFormGraphicEditor.Image5MouseMove(Sender: TObject;
  Shift: TShiftState; X, Y: Integer);
begin
   Image_Mouse_Move(Image5,4,x,y);
end;

procedure TFormGraphicEditor.Image3MouseMove(Sender: TObject;
  Shift: TShiftState; X, Y: Integer);
begin
    Image_Mouse_Move(Image3,2,x,y);
end;

procedure TFormGraphicEditor.Image7MouseMove(Sender: TObject;
  Shift: TShiftState; X, Y: Integer);
begin
   Image_Mouse_Move(Image7,6,x,y);
end;

procedure TFormGraphicEditor.Image2MouseMove(Sender: TObject;
  Shift: TShiftState; X, Y: Integer);
begin
   Image_Mouse_Move(Image2,1,x,y);
end;

procedure TFormGraphicEditor.Image4MouseMove(Sender: TObject;
  Shift: TShiftState; X, Y: Integer);
begin
    Image_Mouse_Move(Image4,3,x,y);
end;

procedure TFormGraphicEditor.Image6MouseMove(Sender: TObject;
  Shift: TShiftState; X, Y: Integer);
begin
   Image_Mouse_Move(Image6,5,x,y);
end;

procedure TFormGraphicEditor.Image8MouseMove(Sender: TObject;
  Shift: TShiftState; X, Y: Integer);
begin
   Image_Mouse_Move(Image8,7,x,y);
end;

procedure TFormGraphicEditor.Image1Click(Sender: TObject);
begin
   Image_Mouse_Click(Image1,0);
end;

procedure TFormGraphicEditor.Image_Mouse_Click(img: TImage; i: Integer);
begin
  if (curr_img=i) and (emptyImage[i]) then
  begin
     Create_Graphic(img,i);
     New_Graphic(img,i);
     img.Repaint;
     emptyImage[i]:=false;
     Update_Img_Count;
  end;
end;

procedure TFormGraphicEditor.Create_Graphic (img: TImage; i: Integer);
var
   x: Integer;
begin
   img.Picture:=nil;
   with img.Canvas do
   begin    // рамка
   Brush.Style:=bsClear;
   if editMode then
        pen.Color := clLime
   else
        pen.Color := clBlack;
   Pen.Width:=2;
   Rectangle(1, 1,img.Width-1,img.Height-1);
   // фон
   Brush.Style:=bsSolid;
   Brush.Color:=clInactiveBorder;
   FloodFill(3,3,Pixels[3,3],fsSurface);

   // деления осей и подписи
   pen.color:=clBlue;
   Pen.Width:=1;
   x:=1;
   Font.Size :=6;
   Font.Name := 'MS Serif';
   TextOut(12,107,'0');
   while x<=10 do
   begin
      MoveTo(18,105-x*10);
      if x<>10 then
         LineTo(22,105-x*10);
      TextOut(3,103-x*10,FloatToStr(x*0.1));
      x:=x+1;
   end;
   x:=1;
   while x<=24 do
   begin
      MoveTo(20+x*8,107);
      LineTo(20+x*8,103);
      if X mod 2 = 0 then
         if x<10 then
         TextOut(18+x*8,107,IntToStr(x))
         else
         TextOut(16+x*8,107,IntToStr(x));
      x:=x+1;
   end;
   // оси
   pen.color:=clBlue;
   Pen.Width:=1;
   MoveTo(20,110);
   LineTo(20,5);
   LineTo(23,10);
   MoveTo(20,5);
   LineTo(17,10);
   MoveTo(15,105);
   LineTo(218,105);
   LineTo(213,108);
   MoveTo(218,105);
   LineTo(213,102);
   // штриховка
   Pen.color:=clAqua;
  // Pen.Style := psDot;
   For x:=1 To 24 do
      begin
      MoveTo(20+x*8,103);
      LineTo(20+x*8,5);
      end;
   For x:=1 To 10 do
    begin
      MoveTo(22,105-x*10);
      LineTo(212,105-x*10);
    end;
   end;
end;

procedure TFormGraphicEditor.New_Graphic (img: TImage; i: Integer);
var
  k: Integer;
begin
   with img.Canvas do
   begin
     pen.Width :=3;
     pen.Color :=clGreen;
     moveTo(20,5);
     LineTo(212,5);
   end;
   GRDataset.SelectSQL.Text :='SELECT * FROM CAT_GR_VALUES';
   GRDataset.AutoUpdateOptions.UpdateTableName :='CAT_GR_VALUES';
   GRDataset.AutoUpdateOptions.GeneratorName :='GEN_CAT_GR_VALUES_ID';
   try
   GRDataset.Open;
   GRDataset.GenerateSQLs;
   For k:=0 to 47 do
   begin
   GRDataset.Append;
   GRDataset.FieldByName('ID_GR_OBJECTS').AsInteger := curr_obj;
   GRDataset.FieldByName('GR_TYPE').AsInteger:= i;
   GRDataset.FieldByName('GR_TIME').AsInteger := k;
   GRDataset.FieldByName('GR_VALUE').AsFloat:= 1.0;
   GRDataset.Post;
   end;
   finally
   GRDataset.Close;
   end;
end;

procedure TFormGraphicEditor.Update_Img_Count();
var
   str1,str2: String;
begin
   if emptyImage[0] then
     str1:='0'
   else
     str1:='1';
   if emptyImage[2] then
     str1:=str1+'0'
   else
     str1:=str1+'1';
   if emptyImage[4] then
     str1:=str1+'0'
   else
     str1:=str1+'1';
   if emptyImage[6] then
     str1:=str1+'0'
   else
     str1:=str1+'1';
   if emptyImage[1] then
     str2:='0'
   else
     str2:='1';
   if emptyImage[3] then
     str2:=str2+'0'
   else
     str2:=str2+'1';
   if emptyImage[5] then
     str2:=str2+'0'
   else
     str2:=str2+'1';
   if emptyImage[7] then
     str2:=str2+'0'
   else
     str2:=str2+'1';
   GRDataset.SelectSQL.Text := 'UPDATE CAT_GR_OBJECTS SET RAB_GR='+str1+
   ', VIH_GR='+str2+ ' WHERE ID=' + IntToStr(curr_obj);
   try
   GRDataset.Open;
   finally
   GRDataset.Close;
   end;
   Lvgraf.ItemFocused.SubItems[0]:=str1+' '+str2;
   Lvgraf.ItemFocused.ImageIndex := BinToDec(str1,str2);
end;

procedure TFormGraphicEditor.Load_Graphic(i: Integer);
var
   k: Integer;
begin
   For k:=0 To 48 do
       gr_values[k]:=0;
   GRDataset.SelectSQL.Text :=' SELECT * FROM CAT_GR_VALUES WHERE ID_GR_OBJECTS='
   + IntToStr(curr_obj)+' AND GR_TYPE='+IntToStr(i)+' ORDER BY GR_TIME';
   try
   GRDataset.Open;
   GRDataset.First;
   while not GRDataset.Eof do
   begin
      gr_values[GRDataset.FieldByName('GR_TIME').AsInteger]:=GRDataset.FieldByName('GR_VALUE').AsFloat;
      GRDataset.Next;
   end;
   gr_values[48]:=gr_values[0];
   finally
   GRDataset.Close;
   end;
end;

procedure TFormGraphicEditor.Paint_Graphic(img: TImage);
var
  k: Integer;
  PointArray : Array[0..48] of TPoint;
begin
   For k:=0 To 48 do
   begin
     PointArray[k].X := 20+k*4;
     PointArray[k].Y := 105-trunc(gr_values[k]*100);
   end;
   with img.Canvas do
   begin
     pen.Width :=3;
     pen.Color :=clGreen;
     Polyline(PointArray);
   end;
   img.Repaint;
end;

procedure TFormGraphicEditor.FormClose(Sender: TObject;
  var Action: TCloseAction);
begin
   LvGraf.Clear;
   if EditMode then
     end_edit;
end;

procedure TFormGraphicEditor.btCloseClick(Sender: TObject);
var
  img: TImage;
begin
   img:=Return_Image;
   img.Picture :=nil;
   ImageList2.GetBitmap(1, img.Picture.Bitmap);
   img.Repaint;
   btClose.Visible :=false;
   btEdit.Visible :=false;
   btTable.Visible :=false;
   GRDataset.SelectSQL.Text := ' DELETE FROM CAT_GR_VALUES WHERE ID_GR_OBJECTS='
   +IntToStr(curr_obj)+' AND GR_TYPE=' + IntToStr(curr_img);
   try
    GRDataset.Open;
   finally
    GRDataset.Close;
   end;
   emptyImage[curr_img]:=True;
   Update_Img_Count;
end;

procedure TFormGraphicEditor.btEditClick(Sender: TObject);
var
 img: TImage;
begin
   editMode:= true;
   Panel1.Enabled :=false;
   btClose.Visible :=false;
   btEdit.Visible :=false;
   btTable.Visible :=false;
   img:=Return_Image;
   Img_Enable(false);
   img.Enabled := true;
   btCancel.Left :=img.Left +222;
   btCancel.Top := img.Top;
   btCancel.Visible :=true;
   btOk.Left :=img.Left +222;
   btOk.Top := img.Top+31;
   btOk.Visible :=true;
   with img.Canvas do
   begin    // рамка
   pen.Color := clLime;
   Brush.Style:=bsClear;
   Pen.Width:=2;
   Rectangle(1, 1,img.Width-1,img.Height-1);
   end;
   Load_Graphic(curr_img);
end;

procedure TFormGraphicEditor.btCancelClick(Sender: TObject);
var
  img: TImage;
begin
   img:=Return_Image;
   editMode:=false;
   Img_Enable(true);
   Create_Graphic(img,curr_img);
   Load_Graphic(curr_img);
   Paint_Graphic(img);
   btCancel.Visible :=false;
   btOk.Visible :=false;
end;

procedure TFormGraphicEditor.End_Edit();
var
  img: TImage;
  k: Integer;
begin
   Img_Enable(true);
   Panel1.Enabled :=true;
   img:= Return_Image;
   with img.Canvas do
   begin    // рамка
   pen.Color := clBlack;
   Brush.Style:=bsClear;
   Pen.Width:=2;
   Rectangle(1, 1,img.Width-1,img.Height-1);
   end;
   btCancel.Visible :=false;
   btOk.Visible :=false;
   editMode:=false;
    For k:=0 to 47 do
    begin
    GRDataset.SelectSQL.Text :='UPDATE CAT_GR_VALUES SET GR_VALUE='+
    FLoatToStr(gr_values[k])+' WHERE ID_GR_OBJECTS='+IntToStr(curr_obj)+
    ' AND GR_TYPE=' + IntToStr(curr_img)+' AND GR_TIME=' + IntToStr(k);
     try
     GRDataset.Open;
     finally
     GRDataset.Close;
     end;
    end;

end;

procedure TFormGraphicEditor.btOkClick(Sender: TObject);
begin
   End_edit;
end;

procedure TFormGraphicEditor.Image1MouseDown(Sender: TObject;
  Button: TMouseButton; Shift: TShiftState; X, Y: Integer);
begin
   Img_Mouse_Down(Image1,x,y);
end;

Procedure TFormGraphicEditor.Img_Mouse_Down(img: TImage; x:Integer; y:Integer);
var
  k: Integer;
begin
  if EditMode then
  begin  // редактирование графика
  doDraw:=true;
  with img.Canvas  do
    begin
    Pen.Width :=2;
    pen.Color := clLime;
            if x>212 then
              x:=212
            else if x<20 then
              x:=20;
            if y<5 then
               y:=5
            else if y>105 then
               y:=105;
    moveTo(x,y);
    end;
  For k:=0 to 192 do
     img_gr_values[k]:=0;
  end
  else if not emptyImage[curr_img] then
  begin   //драг энд дроп
    img.BeginDrag(true);
    Drag_Mode := true;
  end;
end;


procedure TFormGraphicEditor.Image1MouseUp(Sender: TObject;
  Button: TMouseButton; Shift: TShiftState; X, Y: Integer);
begin
   Paint_User_Graphic(Image1);
end;

procedure TFormGraphicEditor.Paint_User_Graphic(img: TImage);
var
  i,j,n,k,num,val: Integer;
begin
   if Drag_mode then
     begin
     img.EndDrag(true);
     end;
   if editMode then
   begin
   doDraw:=false;
   //применение
   // 1 - Заполнение пробелов в массиве
   n:=0; val:=0;
   while (img_gr_values[n]=0) and (n<192) do
      n:=n+1;
   k:=192;
   while (img_gr_values[k]=0) and (k>0) do
      k:=k-1;
   if n<>192 then
   begin
    num:=0;
    For i:=n to k do
    begin
      if img_gr_values[i]=0 then
      begin
        num:=num+1;
      end
      else
         if num=0 then
            val:=img_gr_values[i]
         else
            begin
              for j:=1 to num do
                 img_gr_values[i-j]:=img_gr_values[i]-trunc((img_gr_values[i]-val)*j/(num+1));
              num:=0;
              val:=img_gr_values[i];
            end;
    end;
    // 2 - перенос координат img в нужный массив координат
    For k:=0 to 48 do
    begin
     if img_gr_values[k*4]<>0 then
        begin
        gr_values[k]:=(105-img_gr_values[k*4])/100;
        if k=0 then
          gr_values[48]:=gr_values[0]
        else if k=48 then
          gr_values[0]:=gr_values[48];
        end;
    end;
    // 3 - Рисование графика
    Create_Graphic(img,curr_img);
    Paint_Graphic(img);
   end;
   end;
end;

procedure TFormGraphicEditor.FormClick(Sender: TObject);
begin
  if editMode then
     end_edit;
end;


procedure TFormGraphicEditor.Image2Click(Sender: TObject);
begin
  Image_Mouse_Click(Image2,1);
end;

procedure TFormGraphicEditor.Image3Click(Sender: TObject);
begin
    Image_Mouse_Click(Image3,2);
end;

procedure TFormGraphicEditor.Image4Click(Sender: TObject);
begin
    Image_Mouse_Click(Image4,3);
end;

procedure TFormGraphicEditor.Image5Click(Sender: TObject);
begin
    Image_Mouse_Click(Image5,4);
end;

procedure TFormGraphicEditor.Image6Click(Sender: TObject);
begin
    Image_Mouse_Click(Image6,5);
end;

procedure TFormGraphicEditor.Image7Click(Sender: TObject);
begin
    Image_Mouse_Click(Image7,6);
end;

procedure TFormGraphicEditor.Image8Click(Sender: TObject);
begin
     Image_Mouse_Click(Image8,7);
end;

procedure TFormGraphicEditor.Image8MouseDown(Sender: TObject;
  Button: TMouseButton; Shift: TShiftState; X, Y: Integer);
begin
     Img_Mouse_Down(Image8,x,y);
end;

procedure TFormGraphicEditor.Image7MouseDown(Sender: TObject;
  Button: TMouseButton; Shift: TShiftState; X, Y: Integer);
begin
    Img_Mouse_Down(Image7,x,y);
end;

procedure TFormGraphicEditor.Image6MouseDown(Sender: TObject;
  Button: TMouseButton; Shift: TShiftState; X, Y: Integer);
begin
    Img_Mouse_Down(Image6,x,y);
end;

procedure TFormGraphicEditor.Image5MouseDown(Sender: TObject;
  Button: TMouseButton; Shift: TShiftState; X, Y: Integer);
begin
    Img_Mouse_Down(Image5,x,y);
end;

procedure TFormGraphicEditor.Image4MouseDown(Sender: TObject;
  Button: TMouseButton; Shift: TShiftState; X, Y: Integer);
begin
Img_Mouse_Down(Image4,x,y);
end;

procedure TFormGraphicEditor.Image3MouseDown(Sender: TObject;
  Button: TMouseButton; Shift: TShiftState; X, Y: Integer);
begin
Img_Mouse_Down(Image3,x,y);
end;

procedure TFormGraphicEditor.Image2MouseDown(Sender: TObject;
  Button: TMouseButton; Shift: TShiftState; X, Y: Integer);
begin
Img_Mouse_Down(Image2,x,y);
end;

procedure TFormGraphicEditor.Image2MouseUp(Sender: TObject;
  Button: TMouseButton; Shift: TShiftState; X, Y: Integer);
begin
   Paint_User_Graphic(Image2);
end;

procedure TFormGraphicEditor.Image3MouseUp(Sender: TObject;
  Button: TMouseButton; Shift: TShiftState; X, Y: Integer);
begin
   Paint_User_Graphic(Image3);
end;

procedure TFormGraphicEditor.Image4MouseUp(Sender: TObject;
  Button: TMouseButton; Shift: TShiftState; X, Y: Integer);
begin
   Paint_User_Graphic(Image4);
end;

procedure TFormGraphicEditor.Image5MouseUp(Sender: TObject;
  Button: TMouseButton; Shift: TShiftState; X, Y: Integer);
begin
   Paint_User_Graphic(Image5);
end;

procedure TFormGraphicEditor.Image6MouseUp(Sender: TObject;
  Button: TMouseButton; Shift: TShiftState; X, Y: Integer);
begin
   Paint_User_Graphic(Image6);
end;

procedure TFormGraphicEditor.Image7MouseUp(Sender: TObject;
  Button: TMouseButton; Shift: TShiftState; X, Y: Integer);
begin
   Paint_User_Graphic(Image7);
end;

procedure TFormGraphicEditor.Image8MouseUp(Sender: TObject;
  Button: TMouseButton; Shift: TShiftState; X, Y: Integer);
begin
   Paint_User_Graphic(Image8);
end;

procedure TFormGraphicEditor.Img_Enable (val: Boolean);
begin
    Image1.Enabled :=val;
    Image2.Enabled :=val;
    Image3.Enabled :=val;
    Image4.Enabled :=val;
    Image5.Enabled :=val;
    Image6.Enabled :=val;
    Image7.Enabled :=val;
    Image8.Enabled :=val;
end;


procedure TFormGraphicEditor.btDeleteItemClick(Sender: TObject);
var
  {id_gr,}sel_item: Integer;
begin
   GRDataset.SelectSQL.Text :='DELETE FROM CAT_GR_VALUES WHERE ID_GR_OBJECTS='+IntToStr(curr_obj);
   try
    GRDataset.Open;
   finally
    GRDataset.Close;
   end;
   GRDataset.SelectSQL.Text:='DELETE FROM CAT_GR_OBJECTS WHERE ID='+IntToStr(curr_obj);
   try
    GRDataset.Open;
   finally
    GRDataset.Close;
   end;
  { GRDataset.SelectSQL.Text:='SELECT * FROM CAT_GR_POTREB WHERE NAME=''' + cbGroup.Items[cbGroup.ItemIndex] + '''';
   try
    GRDataset.Open;
    id_gr:=GRDataset.fieldByName('ID').AsInteger;
   finally
    GRDataset.Close;
   end;  }
   sel_item:= LvGraf.ItemIndex-1;
   LvGraf.ItemFocused.Delete;
   if sel_item>0 then
   begin
   LvGraf.Selected := LvGraf.Items[sel_item];
   LvGraf.ItemFocused := LvGraf.Items[sel_item];
   end
   else if LvGraf.Items.Count >0 then
   begin
   LvGraf.Selected := LvGraf.Items[0];
   LvGraf.ItemFocused := LvGraf.Items[0];
   end
   else
   begin
     All_Images_Visible(false);
     btCopyItem.Enabled :=false;
     btDeleteItem.Enabled :=false;
   end;
   //Load_Objects(id_gr, cbPodGroup.Items[cbPodGroup.ItemIndex]);
end;

procedure TFormGraphicEditor.btCopyItemClick(Sender: TObject);
var
 id_gr,id_podgr, rab_gr, vih_gr,new_id: Integer;
 LI: TListItem;
 q1: TFibDataSet;
begin
   LvGraf.Focused;
   LI:=LvGraf.Items.add;
   LI.Caption:= LvGraf.ItemFocused.Caption + '(копия)';
   LI.SubItems.Add(LvGraf.ItemFocused.SubItems[0]);
   LI.ImageIndex := LvGraf.ItemFocused.ImageIndex;

   GRDataset.SelectSQL.Text:='SELECT * FROM CAT_GR_POTREB WHERE NAME=''' + cbGroup.Items[cbGroup.ItemIndex] + '''';
    try
    GRDataset.Open;
   id_gr:=GRDataset.fieldByName('ID').AsInteger ;
   finally
    GRDataset.Close;
   end;
      GRDataset.SelectSQL.Text:='SELECT * FROM CAT_PODGR_POTREB WHERE NAME=''' + cbPodGroup.Items[cbPodGroup.ItemIndex] +
      ''' AND ID_GR_POTREB='+IntTOStr(id_gr);
   try
    GRDataset.Open;
    id_podgr:=GRDataset.fieldByName('ID').AsInteger ;
    finally
    GRDataset.Close;
   end;


   GRDataset.SelectSQL.Text:='SELECT * FROM CAT_GR_OBJECTS WHERE ID='+ IntToStr(curr_obj);
   try
    GRDataset.Open;
    rab_gr:=GRDataset.fieldByName('RAB_GR').AsInteger;
    vih_gr:=GRDataset.fieldByName('VIH_GR').AsInteger;
   finally
    GRDataset.Close;
   end;
   GRDataset.SelectSQL.Text:='SELECT * FROM CAT_GR_OBJECTS';
   GRDataset.AutoUpdateOptions.UpdateTableName :='CAT_GR_OBJECTS';
   GRDataset.AutoUpdateOptions.GeneratorName :='GEN_CAT_GR_OBJECTS_ID';
   try
   GRDataset.Open;
   GRDataset.GenerateSQLs;
   GRDataset.Append;
   GRDataset.FieldByName('ID_GR_POTREB').AsInteger := id_gr;
   GRDataset.FieldByName('ID_PODGR_POTREB').AsInteger:= id_podgr;
   GRDataset.FieldByName('NAME').AsString := LvGraf.ItemFocused.Caption + '(копия)';
   GRDataset.FieldByName('RAB_GR').AsInteger:= rab_gr;
   GRDataset.FieldByName('VIH_GR').AsInteger:= vih_gr;
   GRDataset.Post;
   finally
    GRDataset.Close;
   end;
   GRDataset.SelectSQL.Text:='SELECT MAX(ID) FROM CAT_GR_OBJECTS';
   try
    GRDataset.Open;
    new_id:=GRDataset.Fields[0].AsInteger;
    LI.SubItems.Add(GRDataset.Fields[0].AsString);
    finally
    GRDataset.Close;
   end;
   GRDataset.SelectSQL.Text:='SELECT * FROM CAT_GR_VALUES WHERE ID_GR_OBJECTS=' + IntToStr(curr_obj);
   try
   q1:= DM.GenerateDataset('CAT_GR_VALUES');
    try
    q1.Open;
    GRDataset.Open;
    GRDataset.First;
    while not GRDataset.Eof do
    begin
       q1.Append;
       q1.FieldByName('ID_GR_OBJECTS').AsInteger := new_id;
       q1.FieldByName('GR_TYPE').AsInteger:= GRDataset.FieldByName('GR_TYPE').AsInteger;
       q1.FieldByName('GR_TIME').AsInteger := GRDataset.FieldByName('GR_TIME').AsInteger;
       q1.FieldByName('GR_VALUE').AsFloat:= GRDataset.FieldByName('GR_VALUE').AsFloat;
       q1.Post;
      GRDataset.Next;
    end;
    finally
    q1.Free;
    end;
   finally
   GRDataset.Close;
   end;
   LvGraf.Selected := LI;
   LvGraf.ItemFocused := LI;
   LI.EditCaption;

end;

procedure TFormGraphicEditor.LvGrafEdited(Sender: TObject; Item: TListItem;
  var S: String);
begin
    if Length(s)>50 then
         s := Copy(s,0,50);
   GRDataset.SelectSQL.Text:='UPDATE CAT_GR_OBJECTS SET NAME='''+ s
   +''' WHERE ID='+IntToStr(curr_obj);
   try
   GRDataset.Open;
   finally
   GRDataset.Close;
   end;
end;

procedure TFormGraphicEditor.Image1DblClick(Sender: TObject);
begin
 Start_Edit;
end;

procedure TFormGraphicEditor.Start_Edit;
begin
   if not EditMode and not emptyImage[curr_img] then
     btEditClick(btEdit);
end;

procedure TFormGraphicEditor.Image2DblClick(Sender: TObject);
begin
Start_Edit;
end;

procedure TFormGraphicEditor.Image3DblClick(Sender: TObject);
begin
Start_Edit;
end;

procedure TFormGraphicEditor.Image4DblClick(Sender: TObject);
begin
Start_Edit;
end;

procedure TFormGraphicEditor.Image5DblClick(Sender: TObject);
begin
Start_Edit;
end;

procedure TFormGraphicEditor.Image6DblClick(Sender: TObject);
begin
Start_Edit;
end;

procedure TFormGraphicEditor.Image7DblClick(Sender: TObject);
begin
Start_Edit;
end;

procedure TFormGraphicEditor.Image8DblClick(Sender: TObject);
begin
    Start_Edit;
end;

procedure TFormGraphicEditor.Image2DragOver(Sender, Source: TObject; X,
  Y: Integer; State: TDragState; var Accept: Boolean);
begin
   if source is TImage then accept:=true else accept:=false;
end;

procedure TFormGraphicEditor.End_Drag(img: TImage; i: Integer);
var
   q1: TFIBDataSet;
begin
    Drag_mode := false;
    if curr_img<>i then
    begin
        GRDataset.SelectSQL.Text:=' SELECT * FROM CAT_GR_VALUES WHERE ID_GR_OBJECTS='
         + IntToStr(curr_obj)+' AND GR_TYPE=' + IntToStr(curr_img);
        if emptyImage[i] then
         begin
          try
          GRDataset.Open;
          GRDataset.First;
          q1:= DM.GenerateDataset('CAT_GR_VALUES');
           try
            q1.Open;
            while not GRDataset.Eof do
            begin
            q1.Append;
            q1.FieldByName('ID_GR_OBJECTS').AsInteger := curr_obj;
            q1.FieldByName('GR_TYPE').AsInteger:= i;
            q1.FieldByName('GR_TIME').AsInteger := GRDataset.FieldByName('GR_TIME').AsInteger;
            q1.FieldByName('GR_VALUE').AsFloat:= GRDataset.FieldByName('GR_VALUE').AsFloat;
            q1.Post;
            GRDataset.Next;
            end;
            finally
             q1.Free;
            end;
           finally
           GRDataset.Close;
           end;
          curr_img:=i;
          emptyImage[i]:=false;
          Update_Img_Count;
         end
        else
         begin
         try
          GRDataset.Open;
          GRDataset.First;
          while not GRDataset.Eof do
          begin
          q1 := DM.GenerateDataset('', False,
          'UPDATE CAT_GR_VALUES SET GR_VALUE='+ GRDataset.FieldByName('GR_VALUE').AsString  +
          ' WHERE ID_GR_OBJECTS='+IntToStr(curr_obj)+ ' AND GR_TYPE=' + IntToStr(i)+
          ' AND GR_TIME=' +GRDataset.FieldByName('GR_TIME').AsString );
          GRDataset.Next;
          q1.Free;
          end;
           finally
          GRDataset.Close;
         end;
        end;
    curr_img:=i;
    Create_Graphic(img,i);
    Load_Graphic(i);
    Paint_Graphic(img);
    end;
end;

procedure TFormGraphicEditor.Image1DragOver(Sender, Source: TObject; X,
  Y: Integer; State: TDragState; var Accept: Boolean);
begin
if source is TImage then accept:=true else accept:=false;
end;

procedure TFormGraphicEditor.Image3DragOver(Sender, Source: TObject; X,
  Y: Integer; State: TDragState; var Accept: Boolean);
begin
if source is TImage then accept:=true else accept:=false;
end;

procedure TFormGraphicEditor.Image5DragOver(Sender, Source: TObject; X,
  Y: Integer; State: TDragState; var Accept: Boolean);
begin
   if source is TImage then accept:=true else accept:=false;
end;

procedure TFormGraphicEditor.Image6DragOver(Sender, Source: TObject; X,
  Y: Integer; State: TDragState; var Accept: Boolean);
begin
if source is TImage then accept:=true else accept:=false;
end;

procedure TFormGraphicEditor.Image7DragOver(Sender, Source: TObject; X,
  Y: Integer; State: TDragState; var Accept: Boolean);
begin
if source is TImage then accept:=true else accept:=false;
end;

procedure TFormGraphicEditor.Image8DragOver(Sender, Source: TObject; X,
  Y: Integer; State: TDragState; var Accept: Boolean);
begin
   if source is TImage then accept:=true else accept:=false;
end;

procedure TFormGraphicEditor.Image4DragOver(Sender, Source: TObject; X,
  Y: Integer; State: TDragState; var Accept: Boolean);
begin
    if (source is TImage)then accept:=true else accept:=false;
end;

procedure TFormGraphicEditor.Image3DragDrop(Sender, Source: TObject; X,
  Y: Integer);
begin
  End_Drag(Image3,2);
end;

procedure TFormGraphicEditor.Image1DragDrop(Sender, Source: TObject; X,
  Y: Integer);
begin
  End_Drag(Image1,0);
end;

procedure TFormGraphicEditor.Image2DragDrop(Sender, Source: TObject; X,
  Y: Integer);
begin
   End_Drag(Image2,1);
end;

procedure TFormGraphicEditor.Image4DragDrop(Sender, Source: TObject; X,
  Y: Integer);
begin
   End_Drag(Image4,3);
end;

procedure TFormGraphicEditor.Image5DragDrop(Sender, Source: TObject; X,
  Y: Integer);
begin
   End_Drag(Image5,4);
end;

procedure TFormGraphicEditor.Image6DragDrop(Sender, Source: TObject; X,
  Y: Integer);
begin
   End_Drag(Image6,5);
end;

procedure TFormGraphicEditor.Image7DragDrop(Sender, Source: TObject; X,
  Y: Integer);
begin
   End_Drag(Image7,6);
end;

procedure TFormGraphicEditor.Image8DragDrop(Sender, Source: TObject; X,
  Y: Integer);
begin
   End_Drag(Image8,7);
end;

function TFormGraphicEditor.Return_Image : TImage ;
begin
   if curr_img=0 then
       result:=Image1
   else if curr_img=1 then
       result:=Image2
   else if curr_img=2 then
       result:=Image3
   else if curr_img=3 then
       result:=Image4
   else if curr_img=4 then
       result:=Image5
   else if curr_img=5 then
       result:=Image6
   else if curr_img=6 then
       result:=Image7
   else
       result:=Image8;
end;

procedure TFormGraphicEditor.btTableClick(Sender: TObject);
var
   EditForm: TFormGraphicTableEdit;
   k: Integer;
begin
   EditForm:= TFormGraphicTableEdit.Create(FormGraphicEditor);
   EditForm.Load_Gr(curr_obj, curr_img);
   if (EditForm.ShowModal=mrOK) then
   begin
       Create_Graphic(Return_Image,curr_img);
       Load_Graphic(curr_img);
       Paint_Graphic(Return_Image);
   end
   else
    For k:=0 to 47 do
     begin
     GRDataset.SelectSQL.Text :='UPDATE CAT_GR_VALUES SET GR_VALUE='+
     FLoatToStr(gr_values[k])+' WHERE ID_GR_OBJECTS='+IntToStr(curr_obj)+
     ' AND GR_TYPE=' + IntToStr(curr_img)+' AND GR_TIME=' + IntToStr(k);
     try
     GRDataset.Open;
     finally
     GRDataset.Close;
     end;
     end;
end;

procedure TFormGraphicEditor.SpeedButton1Click(Sender: TObject);
var
   id: Integer;
   aName: String;
begin
    if InputQuery('Редактирование группы потребителей', 'Введите новое наименование группы'+#13#10+'(не более 30 символов)', aName) then
     begin
       if aName <> '' then
       begin
        if Length(aName)>30 then
            aName := Copy(aName,0,30);
        GRDataset.SelectSQL.Text :='SELECT * FROM CAT_GR_POTREB WHERE NAME='''+cbGroup.Items[cbGroup.ItemIndex]+'''';
        try
        GRDataset.Open;
        id:=GRDataset.FieldByName('ID').AsInteger;
        finally
        GRDataset.Close;
        end;
        GRDataset.SelectSQL.Text :='UPDATE CAT_GR_POTREB SET NAME=''' +aName+ ''' WHERE ID='+IntToStr(id);
        try
        GRDataset.Open;
        id:=cbGroup.ItemIndex;
        cbGroup.Items[cbGroup.ItemIndex]:=aName;
        cbGroup.ItemIndex:=id;
        finally
        GRDataset.Close;
        end;
       end;

     end;
end;


procedure TFormGraphicEditor.SpeedButton2Click(Sender: TObject);
var
  obj_list: Array of Integer;
  id,i: Integer;
begin
    case MessageBox(Handle,'Удалить группу вместе с принадлежащими ей типовыми графиками нагрузки?','Подтверждение удаления',MB_ICONQUESTION+MB_YESNO) of
     mrYes:
     begin
      GRDataset.SelectSQL.Text :='SELECT * FROM CAT_GR_POTREB WHERE NAME='''+cbGroup.Items[cbGroup.ItemIndex]+'''';
      try
       GRDataset.Open;
       id:=GRDataset.FieldByName('ID').AsInteger;
      finally
       GRDataset.Close;
      end;
      GRDataset.SelectSQL.Text :='SELECT * FROM CAT_GR_OBJECTS WHERE ID_GR_POTREB='+IntToStr(id);
      try
      GRDataset.Open;
       while not GRDataset.Eof do
          begin
          SetLength(obj_list,Length(obj_list)+1);
          obj_list[Length(obj_list)-1]:=GRDataset.FieldByName('ID').AsInteger;
          GRDataset.Next;
          end;
      finally
       GRDataset.Close;
      end;
      For i:=0 to Length(obj_list)-1 do
       begin
        GRDataset.SelectSQL.Text :='DELETE FROM CAT_GR_VALUES WHERE ID_GR_OBJECTS='+IntToStr(obj_list[i]);
        try
         GRDataset.Open;
        finally
         GRDataset.Close;
        end;
       end;
       SetLength(obj_list,0);
       GRDataset.SelectSQL.Text:='DELETE FROM CAT_GR_OBJECTS WHERE ID_GR_POTREB='+IntToStr(id);
       try
         GRDataset.Open;
       finally
         GRDataset.Close;
       end;
       GRDataset.SelectSQL.Text:='DELETE FROM CAT_PODGR_POTREB WHERE ID_GR_POTREB='+IntToStr(id);
       try
         GRDataset.Open;
       finally
         GRDataset.Close;
       end;
       GRDataset.SelectSQL.Text:='DELETE FROM CAT_GR_POTREB WHERE ID='+IntToStr(id);
       try
         GRDataset.Open;
       finally
         GRDataset.Close;
       end;
       id:=cbGroup.ItemIndex;
       cbGroup.Items.Delete(id);

       if id<>0 then
        cbGroup.ItemIndex := id-1
       else
        cbGroup.ItemIndex := id;
       if cbGroup.Items.Count=1 then
       begin
         cbPodGroup.Items.Clear;
         cbPodGroup.Items.Add('Создать новую подгруппу...');
         cbPodGroup.ItemIndex:=0;
         Buttons_enabled(false,false);
       end;
       Select_Podgroup(cbGroup.Items[cbGroup.ItemIndex]);
     end;
    end;

end;

procedure TFormGraphicEditor.SpeedButton5Click(Sender: TObject);
var
   id,id_gr: Integer;
   aName: String;
begin
    if InputQuery('Редактирование подгруппы', 'Введите новое наименование подгруппы'+#13#10+'(не более 50 символов)', aName) then
     begin
       if aName <> '' then
       begin
        if Length(aName)>50 then
            aName := Copy(aName,0,50);

        GRDataset.SelectSQL.Text :='SELECT * FROM CAT_GR_POTREB WHERE NAME='''+cbGroup.Items[cbGroup.ItemIndex]+'''';
        try
          GRDataset.Open;
          id_gr:=GRDataset.FieldByName('ID').AsInteger;
        finally
         GRDataset.Close;
        end;
        GRDataset.SelectSQL.Text :='SELECT * FROM CAT_PODGR_POTREB WHERE NAME='''+cbPodGroup.Items[cbPodGroup.ItemIndex]+
        ''' AND ID_GR_POTREB='+IntToStr(id_gr);
        try
        GRDataset.Open;
        id:=GRDataset.FieldByName('ID').AsInteger;
        finally
        GRDataset.Close;
        end;
        GRDataset.SelectSQL.Text :='UPDATE CAT_PODGR_POTREB SET NAME=''' +aName+ ''' WHERE ID='+IntToStr(id);
        try
        GRDataset.Open;
        id:=cbPodGroup.ItemIndex;
        cbPodGroup.Items[cbPodGroup.ItemIndex]:=aName;
        cbPodGroup.ItemIndex:=id;
        finally
        GRDataset.Close;
        end;
       end;
     end;
end;

procedure TFormGraphicEditor.SpeedButton6Click(Sender: TObject);
var
  obj_list: Array of Integer;
  id,i,id_gr: Integer;
begin
    case MessageBox(Handle,'Удалить подгруппу вместе с принадлежащими ей типовыми графиками нагрузки?','Подтверждение удаления',MB_ICONQUESTION+MB_YESNO) of
     mrYes:
     begin
        GRDataset.SelectSQL.Text :='SELECT * FROM CAT_GR_POTREB WHERE NAME='''+cbGroup.Items[cbGroup.ItemIndex]+'''';
        try
          GRDataset.Open;
          id_gr:=GRDataset.FieldByName('ID').AsInteger;
        finally
         GRDataset.Close;
        end;
        GRDataset.SelectSQL.Text :='SELECT * FROM CAT_PODGR_POTREB WHERE NAME='''+cbPodGroup.Items[cbPodGroup.ItemIndex]+
        ''' AND ID_GR_POTREB='+IntToStr(id_gr);
        try
        GRDataset.Open;
        id:=GRDataset.FieldByName('ID').AsInteger;
        finally
        GRDataset.Close;
        end;
      GRDataset.SelectSQL.Text :='SELECT * FROM CAT_GR_OBJECTS WHERE ID_PODGR_POTREB='+IntToStr(id);
      try
      GRDataset.Open;
       while not GRDataset.Eof do
          begin
          SetLength(obj_list,Length(obj_list)+1);
          obj_list[Length(obj_list)-1]:=GRDataset.FieldByName('ID').AsInteger;
          GRDataset.Next;
          end;
      finally
       GRDataset.Close;
      end;
      For i:=0 to Length(obj_list)-1 do
       begin
        GRDataset.SelectSQL.Text :='DELETE FROM CAT_GR_VALUES WHERE ID_GR_OBJECTS='+IntToStr(obj_list[i]);
        try
         GRDataset.Open;
        finally
         GRDataset.Close;
        end;
       end;
       SetLength(obj_list,0);
       GRDataset.SelectSQL.Text:='DELETE FROM CAT_GR_OBJECTS WHERE ID_PODGR_POTREB='+IntToStr(id);
       try
         GRDataset.Open;
       finally
         GRDataset.Close;
       end;
       GRDataset.SelectSQL.Text:='DELETE FROM CAT_PODGR_POTREB WHERE ID='+IntToStr(id);
       try
         GRDataset.Open;
       finally
         GRDataset.Close;
       end;
       id:=cbPodGroup.ItemIndex;
       cbPodGroup.Items.Delete(id);
       if id<>0 then
        cbPodGroup.ItemIndex := id-1
       else
        cbPodGroup.ItemIndex := id;
       //подгрузить графики
      if cbPodGroup.Items.Count >1 then
       begin
        Load_Objects(id, cbPodGroup.Items[cbPodGroup.ItemIndex]);
        btNewItem.Enabled :=true;
       end
     else
       begin
       LvGraf.Items.Clear;
       All_Images_Visible(false);
       btNewItem.Enabled :=false;
       btCopyItem.Enabled :=false;
       btDeleteItem.Enabled :=false;
       Buttons_enabled(true,false);
       end;
     end;
    end;

end;

Procedure TFormGraphicEditor.Buttons_enabled(val_group: boolean; val_podgr:boolean);
begin
    SpeedButton1.Enabled :=val_group;
    SpeedButton2.Enabled :=val_group;
    cbPodGroup.Enabled :=val_group;
    SpeedButton5.Enabled :=val_podgr;
    SpeedButton6.Enabled :=val_podgr;
end;

procedure TFormGraphicEditor.Panel1MouseMove(Sender: TObject;
  Shift: TShiftState; X, Y: Integer);
begin
   Form_Mouse_Move;
end;

procedure TFormGraphicEditor.Form_Mouse_Move();
begin
    if Drag_mode then
       begin
       Drag_mode := false;
       btClose.Visible := false;
       btEdit.Visible := false;
       btTable.Visible := false;
       end;
    if not EditMode then
    begin
    if (curr_img>=0) then
    begin
     if emptyImage[0] then
      begin
       ImageList2.GetBitmap(0, Image1.Picture.Bitmap);
       Image1.Repaint;
      end;
     if emptyImage[2] then
      begin
       ImageList2.GetBitmap(0, Image3.Picture.Bitmap);
       Image3.Repaint;
      end;
     if emptyImage[4] then
      begin
       ImageList2.GetBitmap(0, Image5.Picture.Bitmap);
       image5.Repaint ;
      end;
     if emptyImage[6] then
      begin
       ImageList2.GetBitmap(0, Image7.Picture.Bitmap);
       Image7.repaint;
      end;
     if emptyImage[1] then
      begin
       ImageList2.GetBitmap(0, Image2.Picture.Bitmap);
       Image2.repaint;
      end;
     if emptyImage[3] then
      begin
       ImageList2.GetBitmap(0, Image4.Picture.Bitmap);
       Image4.repaint;
      end;
     if emptyImage[5] then
      begin
       ImageList2.GetBitmap(0, Image6.Picture.Bitmap);
       Image6.repaint;
      end;
     if emptyImage[7] then
      begin
       ImageList2.GetBitmap(0, Image8.Picture.Bitmap);
       Image8.repaint;
      end;
    If not EditMode then
      curr_img:=-1;
    end;
    btClose.Visible :=false;
    btEdit.Visible :=false;
    btTable.Visible :=false;
    end;
 end;

procedure TFormGraphicEditor.LvGrafMouseMove(Sender: TObject;
  Shift: TShiftState; X, Y: Integer);
begin
  Form_Mouse_Move;
end;

procedure TFormGraphicEditor.All_Images_Visible(val: boolean);
begin
       Image1.Visible :=val;
       Image2.Visible :=val;
       Image3.Visible :=val;
       Image4.Visible :=val;
       Image5.Visible :=val;
       Image6.Visible :=val;
       Image7.Visible :=val;
       Image8.Visible :=val;
end;

end.

unit GraphicList;

interface

uses  classes, Sysutils,Dialogs,Variants;


type
  TError = procedure (Sender:TObject; Error: string) of object;
  DynFloatArray = Array[0..47] of Double;
  TGraphicList = class(TObject)
  private
   FonError:  TError;
   FPrecision : byte;
  public
   constructor Create;
   destructor Destroy; override ;
   procedure New_Obj(id: Integer; ust: Double;
    s_rab: DynFloatArray;
    s_vih: DynFloatArray;
    z_rab: DynFloatArray;
    z_vih: DynFloatArray;
    v_rab: DynFloatArray;
    v_vih: DynFloatArray;
    o_rab: DynFloatArray;
    o_vih: DynFloatArray; descr: string);
   procedure Print;
   procedure Poluchit_Tip_Gr(var Excel: OleVariant; var row_Excel: Integer;
   gr_id, podgr_id: Integer);
   function Return_id_head: integer;
   property Precision: byte read FPrecision write FPrecision;
   property onError: TError read FonError write FonError;
  end;

implementation
{ TGraphicList }

type
 TPObj = ^TObj;
  TObj = record
    id_obj: Integer; // индекс объекта
    ust_m: Double; //    установленная мощность
    sum_rab: DynFloatArray;
    sum_vih: DynFloatArray;
    zim_rab: DynFloatArray;
    zim_vih: DynFloatArray;
    ves_rab: DynFloatArray;
    ves_vih: DynFloatArray;
    os_rab: DynFloatArray;
    os_vih: DynFloatArray;
    Group: Integer;
    Descr: String;
    next: TPObj; // следующий объект
  end;


var
  head: TPObj; // первый (голова) объект

procedure TGraphicList.New_Obj(id: Integer; ust: Double;
    s_rab: DynFloatArray;
    s_vih: DynFloatArray;
    z_rab: DynFloatArray;
    z_vih: DynFloatArray;
    v_rab: DynFloatArray;
    v_vih: DynFloatArray;
    o_rab: DynFloatArray;
    o_vih: DynFloatArray;
    descr: string );
var
  node: TPObj; // новый узел
  curr: TPObj; // текущий
  pre: TPObj; // предыдущий, относительно curr, узел
begin
  try
  new(node); // выделяем память под новый объект
  node^.id_obj := id;
  node^.ust_m := Round(ust);
  node^.sum_rab:= s_rab;
  node^.sum_vih:= s_vih;
  node^.zim_rab:= z_rab;
  node^.zim_vih:= z_vih;
  node^.ves_rab:= v_rab;
  node^.ves_vih:= v_vih;
  node^.os_rab:= o_rab;
  node^.os_vih:= o_vih;
  node^.Group :=0;
  node^.Descr :=descr;

  curr := head;
  pre := nil;
  while (curr <> nil) and (node^.id_obj > curr^.id_obj) do
  begin
    pre := curr;
    curr := curr^.next; //
  end;
  if pre = nil
    then
  begin
    node^.next := head;
    head := node;
  end
  else
  begin
    node^.next := pre^.next;
    pre^.next := node;
  end;
  except
    on e: exception do
      if Assigned(FOnError) then
         FOnError(Self,e.Message );
  end;
end;

procedure TGraphicList.Print;
var
  curr: TPObj; //
  n: integer;
  st: string;
begin
  n := 0;
  st := '';
  curr := head;
  while curr <> nil do
  begin
    n := n + 1;
    st := st + IntToStr(curr^.id_obj) + #13;
    curr := curr^.next;
  end;
  if n <> 0
    then ShowMessage(IntToStr(n)+' объектов в списке:' + #13 + st)
  else ShowMessage('Список пуст');
end;

function TGraphicList.Return_id_head: integer;
begin
  result:= head^.id_obj;
end;

constructor TGraphicList.Create;
var
 str: string;
begin
  inherited;
  head:=nil;

end;

destructor TGraphicList.Destroy;
var
 curr, next: TPObj; //
begin
  curr := head;
  while curr <> nil do
  begin
  next:= curr^.next;
  Dispose(curr);
  curr := next;
  end;
  inherited;
end;

procedure TGraphicList.Poluchit_Tip_Gr(var Excel: OleVariant; var row_Excel: Integer;
gr_id, podgr_id: Integer);
var
  curr, next: TPObj; //
  Range, Cell1, Cell2, ArrayData : Variant;
  i,j,n,gr_count: Integer;
  str: String;
  max,dop_max,dop_sr,summ: Double;
  otkl: DynFloatArray;
  sum_rab, sum_vih, zim_rab, zim_vih, os_rab, os_vih, ves_rab, ves_vih: DynFloatArray;
begin
  dop_max:=0.5;
  dop_sr:=0.3;
  ArrayData := VarArrayCreate([1, 8, 1, 51], varVariant);
  curr := head;
 // SetLength(group,0)
  // подготовка к сравнению
  while curr <> nil do
  begin
  {  if Round(curr^.ust_m) = 0 then
    begin  }
     max:=0;
     if curr^.sum_rab[0]<>-1 then
     For i:=0 to 47 do
       if curr^.sum_rab[i]>max then
          max:= curr^.sum_rab[i];
     if curr^.sum_vih[0]<>-1 then
     For i:=0 to 47 do
        if curr^.sum_vih[i]>max then
          max:=curr^.sum_vih[i];
     if curr^.ves_rab[0]<>-1 then
     For i:=0 to 47 do
         if curr^.ves_rab[i]>max then
          max:= curr^.ves_rab[i];
     if curr^.ves_vih[0]<>-1 then
     For i:=0 to 47 do
        if curr^.ves_vih[i]>max then
          max:= curr^.ves_vih[i];
     if curr^.zim_rab[0]<>-1 then
     For i:=0 to 47 do
         if curr^.zim_rab[i]>max then
          max:= curr^.zim_rab[i];
     if curr^.zim_vih[0]<>-1 then
     For i:=0 to 47 do
        if curr^.zim_vih[i]>max then
          max:= curr^.zim_vih[i];
     if curr^.os_rab[0]<>-1 then
     For i:=0 to 47 do
         if curr^.os_rab[i]>max then
          max:= curr^.os_rab[i];
     if curr^.os_vih[0]<>-1 then
     For i:=0 to 47 do
        if curr^.os_vih[i]>max then
          max:= curr^.os_vih[i];
     if max > 0 then
       curr^.ust_m := max
     else
       ShowMessage('Ошибка при определении установленной мощности ' +curr^.Descr +  'Id=' + IntToStr(curr^.id_obj));
   // end;
   if curr^.sum_rab[0]<>-1 then
     For i:=0 to 47 do
        curr^.sum_rab[i]:=Round(curr^.sum_rab[i]/curr^.ust_m*1000)/1000;
   if curr^.sum_vih[0]<>-1 then
     For i:=0 to 47 do
        curr^.sum_vih[i]:=Round(curr^.sum_vih[i]/curr^.ust_m*1000)/1000;
   if curr^.ves_rab[0]<>-1 then
     For i:=0 to 47 do
        curr^.ves_rab[i]:=Round(curr^.ves_rab[i]/curr^.ust_m*1000)/1000;
   if curr^.ves_vih[0]<>-1 then
     For i:=0 to 47 do
        curr^.ves_vih[i]:=Round(curr^.ves_vih[i]/curr^.ust_m*1000)/1000;
   if curr^.zim_rab[0]<>-1 then
     For i:=0 to 47 do
        curr^.zim_rab[i]:=Round(curr^.zim_rab[i]/curr^.ust_m*1000)/1000;
   if curr^.zim_vih[0]<>-1 then
     For i:=0 to 47 do
        curr^.zim_vih[i]:=Round(curr^.zim_vih[i]/curr^.ust_m*1000)/1000;
   if curr^.os_rab[0]<>-1  then
     For i:=0 to 47 do
        curr^.os_rab[i]:=Round(curr^.os_rab[i]/curr^.ust_m*1000)/1000;
   if curr^.os_vih[0]<>-1  then
     For i:=0 to 47 do
        curr^.os_vih[i]:=Round(curr^.os_vih[i]/curr^.ust_m*1000)/1000;
   curr:= curr^.next;
  end;
  // группировка схожих графиков
  curr := head;
  n:=1;
   while curr <> nil do
   begin
     if curr^.Group = 0 then
     begin
     curr^.Group := n;
     next:=curr;
     curr:= curr^.next;
      while (curr <> nil) and (curr^.Group=0) do
      begin
        max:=0;
        gr_count:=0;
        summ:=0;
        if next^.sum_rab[0]<>-1 then
          begin
          if curr^.sum_rab[0]<>-1 then
           begin
            gr_count:=gr_count+1;
            For i:=0 to 47 do
            begin
             otkl[i]:= Abs(next^.sum_rab[i] - curr^.sum_rab[i]);
             if otkl[i]>max then
               max:=otkl[i];
             summ:=summ+otkl[i];
            end;
           end
          else
            curr^.Group:=-1;
          end
        else if curr^.sum_rab[0]<>-1 then
            curr^.Group:=-1;

         if next^.sum_vih[0]<>-1 then
          begin
          if curr^.sum_vih[0]<>-1 then
           begin
            gr_count:=gr_count+1;
            For i:=0 to 47 do
            begin
             otkl[i]:= Abs(next^.sum_vih[i] - curr^.sum_vih[i]);
             if otkl[i]>max then
               max:=otkl[i];
             summ:=summ+otkl[i];
            end;
           end
          else
            curr^.Group:=-1;
          end
        else if curr^.sum_vih[0]<>-1 then
            curr^.Group:=-1;

       if next^.zim_rab[0]<>-1 then
          begin
          if curr^.zim_rab[0]<>-1 then
           begin
            gr_count:=gr_count+1;
            For i:=0 to 47 do
            begin
             otkl[i]:= Abs(next^.zim_rab[i] - curr^.zim_rab[i]);
             if otkl[i]>max then
               max:=otkl[i];
             summ:=summ+otkl[i];
            end;
           end
          else
            curr^.Group:=-1;
          end
        else if curr^.zim_rab[0]<>-1 then
            curr^.Group:=-1;

        if next^.zim_vih[0]<>-1 then
          begin
          if curr^.zim_vih[0]<>-1 then
           begin
            gr_count:=gr_count+1;
            For i:=0 to 47 do
            begin
             otkl[i]:= Abs(next^.zim_vih[i] - curr^.zim_vih[i]);
             if otkl[i]>max then
               max:=otkl[i];
             summ:=summ+otkl[i];
            end;
           end
          else
            curr^.Group:=-1;
          end
        else if curr^.zim_vih[0]<>-1 then
            curr^.Group:=-1;

       if next^.ves_rab[0]<>-1 then
          begin
          if curr^.ves_rab[0]<>-1 then
           begin
            gr_count:=gr_count+1;
            For i:=0 to 47 do
            begin
             otkl[i]:= Abs(next^.ves_rab[i] - curr^.ves_rab[i]);
             if otkl[i]>max then
               max:=otkl[i];
             summ:=summ+otkl[i];
            end;
           end
          else
            curr^.Group:=-1;
          end
        else if curr^.ves_rab[0]<>-1 then
            curr^.Group:=-1;

        if next^.ves_vih[0]<>-1 then
          begin
          if curr^.ves_vih[0]<>-1 then
           begin
            gr_count:=gr_count+1;
            For i:=0 to 47 do
            begin
             otkl[i]:= Abs(next^.ves_vih[i] - curr^.ves_vih[i]);
             if otkl[i]>max then
               max:=otkl[i];
             summ:=summ+otkl[i];
            end;
           end
          else
            curr^.Group:=-1;
          end
        else if curr^.ves_vih[0]<>-1 then
            curr^.Group:=-1;

       if next^.os_rab[0]<>-1 then
          begin
          if curr^.os_rab[0]<>-1 then
           begin
            gr_count:=gr_count+1;
            For i:=0 to 47 do
            begin
             otkl[i]:= Abs(next^.os_rab[i] - curr^.os_rab[i]);
             if otkl[i]>max then
               max:=otkl[i];
             summ:=summ+otkl[i];
            end;
           end
          else
            curr^.Group:=-1;
          end
        else if curr^.os_rab[0]<>-1 then
            curr^.Group:=-1;

        if next^.os_vih[0]<>-1 then
          begin
          if curr^.os_vih[0]<>-1 then
           begin
            gr_count:=gr_count+1;
            For i:=0 to 47 do
            begin
             otkl[i]:= Abs(next^.os_vih[i] - curr^.os_vih[i]);
             if otkl[i]>max then
               max:=otkl[i];
             summ:=summ+otkl[i];
            end;
           end
          else
            curr^.Group:=-1;
          end
        else if curr^.os_vih[0]<>-1 then
            curr^.Group:=-1;

        if curr^.Group=-1 then
           curr^.Group:=0
        else
        begin
          if (max<=dop_max) and  (summ/(48*gr_count)< dop_sr) then
            curr^.Group:=n
           else
            curr^.Group:=0;
        end;
        curr:= curr^.next;
      end;
     curr:=next;
     n:=n+1;
     end;
     curr:= curr^.next;
   end;
  // Вывод в эксель графиков
  n:=n-1;
  For i:=1 to n do
  begin
   gr_count:=0;
   str:='';
   For j:=0 to 47 do
    begin
     sum_rab[j]:=0;
     sum_vih[j]:=0;
     zim_rab[j]:=0;
     zim_vih[j]:=0;
     os_rab[j]:=0;
     os_vih[j]:=0;
     ves_rab[j]:=0;
     ves_vih[j]:=0;
    end;
   curr := head;
   while curr <> nil do
   begin
     if curr^.Group = i then
       begin
       gr_count:=gr_count+1;
       str:=str + curr^.Descr + ', ';
       For j:=0 to 47 do
        begin
        if curr^.sum_rab[0]<>-1 then
           sum_rab[j]:=sum_rab[j]+curr^.sum_rab[j];
        if curr^.sum_vih[0]<>-1 then
           sum_vih[j]:=sum_vih[j]+curr^.sum_vih[j];
        if curr^.zim_rab[0]<>-1 then
           zim_rab[j]:=zim_rab[j]+curr^.zim_rab[j];
        if curr^.zim_vih[0]<>-1 then
           zim_vih[j]:=zim_vih[j]+curr^.zim_vih[j];
        if curr^.os_rab[0]<>-1 then
           os_rab[j]:=os_rab[j]+curr^.os_rab[j];
        if curr^.os_vih[0]<>-1 then
           os_vih[j]:=os_vih[j]+curr^.os_vih[j];
        if curr^.ves_rab[0]<>-1 then
           ves_rab[j]:=ves_rab[j]+curr^.ves_rab[j];
        if curr^.ves_vih[0]<>-1 then
           ves_vih[j]:=ves_vih[j]+curr^.ves_vih[j];
        end;
       end;
     curr:= curr^.next;
   end;
    ArrayData[1, 1] := str;
    ArrayData[1, 2] := gr_id;
    ArrayData[1, 3] := podgr_id;
    For j:=0 to 47 do
    begin
      ArrayData[1, j+4] :=sum_rab[j]/gr_count;
       ArrayData[2, j+4] :=sum_vih[j]/gr_count;
      ArrayData[3, j+4] :=zim_rab[j]/gr_count;
       ArrayData[4, j+4] :=zim_vih[j]/gr_count;
      ArrayData[5, j+4] :=ves_rab[j]/gr_count;
       ArrayData[6, j+4] :=ves_vih[j]/gr_count;
      ArrayData[7, j+4] :=os_rab[j]/gr_count;
       ArrayData[8, j+4] :=os_vih[j]/gr_count;
    end;
    // Левая верхняя ячейка области, в которую будем выводить данные

    Cell1 := Excel.WorkSheets[1].Cells[row_Excel, 1];
    row_Excel:=row_Excel+8;
    // Правая нижняя ячейка области, в которую будем выводить данные
    Cell2 := Excel.WorkSheets[1].Cells[row_Excel-1, 51];
    // Область, в которую будем выводить данные
    Range := Excel.WorkSheets[1].Range[Cell1, Cell2];
    // А вот и сам вывод данных
    // Намного быстрее поячеечного присвоения
    Range.Value := ArrayData;
  end;
end;


end.

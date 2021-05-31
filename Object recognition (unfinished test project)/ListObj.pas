unit ListObj;

interface
uses
  classes, Sysutils,Dialogs,Variants;

type
  TPObj = ^TObj;
  TObj = record
    val: Integer;  // индекс объекта
    x: Integer;    //
    y: Integer;    //
    next: TPObj;   // следующий объект
  end;

type
  TError = procedure (Sender:TObject; Error: string) of object;
  TListObj = class (TObject)
  private
   FonError:  TError;
   FPrecision: byte;
   Count: Integer;
   Id_Min_val: Integer;
   Min_val: Integer;
   head: TPObj;
         // первый (голова) объект

  public
   constructor Create;
   destructor Destroy; override ;
   procedure New_obj(val: Integer; x: Integer; y: Integer; sort: Boolean);
   function Print: String;
    procedure Insert(val, x, y: Integer);  
   function Get_val(i: integer): Integer;
   function Get_x(i: integer): Integer;
   function Get_y(i: integer): Integer;
   function RazrezPoMin: TPObj;
   procedure SetHead(h: TPObj);
   procedure UpdateProp;
   procedure ClearList;
  // function Return_id_head: integer;
   property Precision: byte read FPrecision write FPrecision;
   property onError: TError read FonError write FonError;
   property Get_Count: Integer read Count;
   property Get_IdMin: Integer read Id_Min_val;
   property Get_Min: Integer read Min_val;
  end;


implementation
{ TListCl }

procedure TListObj.New_obj(val: Integer; x: Integer; y: Integer; sort: Boolean);
var
  node: TPObj; // новый узел
  curr: TPObj; // текущий
  pre: TPObj; // предыдущий, относительно curr, узел
  n:integer;
begin
  try
  new(node); // выделяем память под новый объект
  node^.val := val;
  node^.x := x;
  node^.y := y;
  curr := head;
  n:=0;
  pre := nil;
  if sort then
   while (curr <> nil) and (node^.val > curr^.val) do //сортировка по возрастанию a
    begin
     pre := curr;
     curr:= curr^.next; //
     inc(n);
    end
  else
    while (curr <> nil) do
     begin
     pre := curr;
     curr:= curr^.next; //
     inc(n);
     end;
  if val<Min_val then
  begin
    Min_val:=val;
    id_min_val:=n;
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
   Count:=Count+1;
  except
    on e: exception do
      if Assigned(FOnError) then
         FOnError(Self,e.Message );
  end;
end;
procedure TListObj.Insert(val: Integer; x: Integer; y: Integer);
var
  node: TPObj; // новый узел
begin
  try
  new(node); // выделяем память под новый объект
  node^.val := val;
  node^.x := x;
  node^.y := y;
  node^.next := head;
  head := node;
  if val<Min_val then
  begin
    Min_val:=val;
    id_min_val:=0;
  end;
  Count:=Count+1;
  except
    on e: exception do
      if Assigned(FOnError) then
         FOnError(Self,e.Message );
  end;
end;

function  TListObj.RazrezPoMin: TPObj;
var
 n: Integer;
 curr,ret: TPobj;
begin
  n:=0;
  ret:=nil;
  curr:=head;
  while (n<>id_min_val) and (curr<>nil) do
  begin
    ret:=curr;
    curr:=curr^.next;
    n:=n+1;
  end;
  if count=id_min_val+1 then
  begin
    ret^.next:=nil;
    Dispose(curr);
    UpdateProp;
    result:=nil;
  end
  else
  begin
   Count:=n+1;
   ret:=curr^.next;
   curr^.next := nil;
   result:=ret;
  end;
end;

procedure  TListObj.SetHead(h: TPObj);
begin
    head:=h;   
end;

procedure  TListObj.UpdateProp;
var
 n: Integer;
 curr: TPobj;
begin
  n:=0;
  id_Min_val:=-1;
  Min_val:=256;
  curr:=head;
  while (curr<>nil) do
  begin
    if curr^.val<Min_val then
    begin
      Min_val:=curr^.val;
      id_min_val:=n;
    end;
    curr:=curr^.next;
    n:=n+1;
  end;
  Count:=n;
end;


function TListObj.Get_val(i:integer): Integer;
var
 n: Integer;
 curr: TPobj;
begin
  n:=0;
  curr:=head;
  while n<>i do
  begin
    curr:=curr^.next;
    n:=n+1;
  end;
  result:=curr^.val;
end;

function TListObj.Get_x(i:integer): Integer;
var
 n: Integer;
 curr: TPobj;
begin
  n:=0;
  curr:=head;
  while n<>i do
  begin
    curr:=curr^.next;
    n:=n+1;
  end;
  result:=curr^.x;
end;

function TListObj.Get_y(i:integer): Integer;
var
 n: Integer;
 curr: TPobj;
begin
  n:=0;
  curr:=head;
  while n<>i do
  begin
    curr:=curr^.next;
    n:=n+1;
  end;
  result:=curr^.y;
end;

function TListObj.Print: String;
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
   // st := st + IntToStr(curr^.a) +'-'+floatToStr(Round(curr^.b*1000)/1000) + #13#10;
    curr := curr^.next;
  end;
  if n <> 0
    then  begin
     result:=IntToStr(n)+' объектов в списке:' + #13 + st;
    end
  else result:='Список пуст';
end;
procedure TListObj.ClearList;
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
  head:=nil;
  Count:=0;
  id_Min_val:=-1;
  Min_val:=256;
end;

constructor TListObj.Create;
begin
  inherited Create;
  head:=nil;
  Count:=0;
  id_Min_val:=-1;
  Min_val:=256;
end;

destructor TListObj.Destroy;
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
  head:=nil;
  Count:=0;
  inherited Destroy;
end;




end.
 
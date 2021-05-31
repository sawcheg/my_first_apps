unit ListLine;

interface
uses
  classes, Sysutils,Dialogs,Variants;

type
  TPObj = ^TObj;
  TObj = record
    x1: Integer;    //    начало линии
    y1: Integer;    //    начало линии
    x2: Integer;    //    конец линии
    y2: Integer;    //    конец линии
    a: Double;      //    y:=a+b*x
    b: Double;
    next: TPObj;    //    следующий объект
  end;

type
  TError = procedure (Sender:TObject; Error: string) of object;
  TListLine = class (TObject)
  private
   FonError:  TError;
   FPrecision: byte;
   Count: Integer;
   head: TPObj;
    // первый (голова) объект

  public
   constructor Create;
   destructor Destroy; override ;
   procedure New_obj(x1, x2,y1,y2: Integer; a,b: double);
   function Get_x(i,ind: integer): Integer;    // ind - 1 или 2
   function Get_y(i,ind: integer): Integer;
   function Get_a(i: integer): double;
   function Get_b(i: integer): double;
   procedure ClearList;
   procedure Edit(i, x, y: integer);
  // function Return_id_head: integer;
   property Precision: byte read FPrecision write FPrecision;
   property onError: TError read FonError write FonError;
   property Get_Count: Integer read Count;
  end;


implementation
{ TListCl }

procedure TListLine.New_obj(x1, x2,y1,y2: Integer; a,b: Double);
var
  node: TPObj; // новый узел
begin
  try
  new(node); // выделяем память под новый объект
  node^.x1 := x1;
  node^.x2 := x2;
  node^.a := a;
  node^.b := b;
  node^.y1 :=y1;
  node^.y2 :=y2;
  node^.next := head;
  head := node;
  Count:=Count+1;
  except
    on e: exception do
      if Assigned(FOnError) then
         FOnError(Self,e.Message );
  end;
end;

procedure TListLine.Edit(i,x,y: integer);
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
  curr^.x2:=x;
  curr^.y2:=y;
end;


function TListLine.Get_x(i,ind:integer): Integer;
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
  if ind=1 then
    result:=curr^.x1
  else
    result:=curr^.x2
end;

function TListLine.Get_y(i,ind:integer): Integer;
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
  if ind=1 then
    result:=curr^.y1
  else
    result:=curr^.y2
end;

function TListLine.Get_a(i:integer): Double;
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
  result:=curr^.a;
end;

function TListLine.Get_b(i:integer): Double;
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
  result:=curr^.b;
end;

procedure TListLine.ClearList;
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
end;

constructor TListLine.Create;
begin
  inherited Create;
  head:=nil;
  Count:=0;
end;

destructor TListLine.Destroy;
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
 
unit ListCl;

interface
uses
  classes, Sysutils,Dialogs,Variants;

type
  TPObj = ^TObj;
  TObj = record
    a: Integer;   // индекс объекта
    b: Integer;    // установленная мощность
    next: TPObj;  // следующий объект
  end;   

type
  TError = procedure (Sender:TObject; Error: string) of object;
  TListCl = class (TObject)
  private
   FonError:  TError;
   FPrecision: byte;
   Count: Integer;
   head: TPObj;    // первый (голова) объект

  public
   constructor Create;
   destructor Destroy; override ;
   procedure New_obj(a: Integer; b: Integer);
   function Print: String;
   function Get_a(i: integer): Integer;
   function Get_b(i: integer): Integer;
  // function Return_id_head: integer;
   property Precision: byte read FPrecision write FPrecision;
   property onError: TError read FonError write FonError;
   property Get_Count: Integer read Count;
   procedure Get_Lines;
   procedure Add_Empty_Lines(horiz: boolean);
   procedure Delete_Peres_Line;
  end;


implementation
{ TListCl }




  
procedure TListCl.New_obj(a: Integer; b: Integer);
var
  node: TPObj; // новый узел
  curr: TPObj; // текущий
  pre: TPObj; // предыдущий, относительно curr, узел
begin
  try
  new(node); // выделяем память под новый объект
  node^.a := a;
  node^.b := b;
  curr := head;
  pre := nil;
  while (curr <> nil) and (node^.a > curr^.a) do //сортировка по возрастанию a
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
   Count:=Count+1;
  except
    on e: exception do
      if Assigned(FOnError) then
         FOnError(Self,e.Message );
  end;
end;

procedure TListCl.Get_Lines;
var
  curr,prev,next,start: TPObj; //
  n,sum_a,col: integer;
  sum_b:double;
  sr: Double;
  st: string;
begin
  st := '';
  n:=0;
  sr:=0;
  next:=head;
  start := head;
  prev := head;
  curr := head^.next;
  while curr<>nil do
  begin
   sum_a:=prev^.a;
   sum_b:=prev^.b;
   col:=1;
   while (curr^.a-prev^.a)<=2 do
   begin
     sum_a:=sum_a+curr^.a;
     sum_b:=sum_b+curr^.b;
     col:=col+1;
     curr := curr^.next;
     prev := prev^.next;
     if curr=nil then break;
   end;
   if n<>0 then
   begin
      next^.next:=prev;
      sr:=sr+(Round(sum_a/col)-next^.a);
   end;
   curr := start;
   while curr<>prev do
   begin
    next:= curr^.next;
    if curr<>nil then
      Dispose(curr);
    curr := next;
    Count:=Count-1
   end;
   curr^.a:=Round(sum_a/col);
   curr^.b:=Round(sum_b/col);
   if n=0 then
      head:=curr;
   n:=n+1;
   next := curr;
   start:= curr^.next;
   prev := curr^.next;
   if prev=nil then
     break;
   curr := prev^.next;
  end;
  sr:=sr/n;
  curr := head;
end;

procedure TListCl.Delete_Peres_Line;
var
 n: Integer;
 curr,prev: TPobj;
begin
  n:=0;
  prev:=head;
  if prev^.next<>nil then
  begin
   curr:= prev^.next;
   while curr<>nil do
   begin
       if (curr^.a<=prev^.a) or (curr^.b<=prev^.b) then
       begin
          if (n=0) and (curr^.next<>nil) then
          begin
             if (curr^.next^.a<=curr^.a) or (curr^.next^.b<=curr^.b) then
             begin
                prev^.next := curr^.next;
                Dispose(curr);
                Count:=Count-1
             end
             else
             begin
                head := curr;
                Dispose(prev);
                Count:=Count-1
             end;
             prev:=head;
             curr:=prev^.next;
          end
          else if (n=Count-1) then
          begin
             prev^.next:=nil ;
             Dispose(curr);
             Count:=Count-1;
          end
          else
          begin
             prev^.next := curr^.next;
             Dispose(curr);
             curr:=prev^.next;
             Count:=Count-1;
          end;
       end
       else
       begin
         curr:=curr^.next;
         prev:=prev^.next;
         n:=n+1;
       end;
   end;
  end;
end;

procedure TListCl.Add_Empty_Lines(horiz: boolean);
var
 n,max: Integer;
 curr,prev,node: TPobj;
 sr_a,sr_b,koef:Double;
begin
  n:=0;
  max:=300;
  sr_a:=0;
  sr_b:=0;
  prev:=head;
  curr:=head^.next;
  while curr<>nil do
  begin
    sr_a:=sr_a+(curr^.a-prev^.a);
    sr_b:=sr_b+(curr^.b-prev^.b);
    curr:=curr^.next;
    prev:=prev^.next;
  end;
  sr_a:=sr_a/(count-1);
  sr_b:=sr_b/(count-1);
  prev:=head;
  curr:=head^.next;
  while curr<>nil do
  begin
    if horiz then
      koef:=(2*curr^.a-max)/max
    else
      koef:=0;
    if ((curr^.a-prev^.a)>(1.7+koef)*sr_a) and ((curr^.b-prev^.b)>(1.7+koef)*sr_b) then
    begin
      new(node); // выделяем память под новый объект
      node^.a := prev^.a+round((curr^.a-prev^.a)/(trunc((curr^.a-prev^.a)/(1.7+koef)/sr_a)+1));
      node^.b := prev^.b+round((curr^.b-prev^.b)/(trunc((curr^.b-prev^.b)/(1.7+koef)/sr_b)+1));
      prev^.next :=node;
      sr_a:=sr_a*(count-1)/count;
      sr_a:=sr_a*(count-1)/count;
      count:=count+1;
      node^.next :=curr;
      prev:=node;
    end
    else
    begin
    curr:=curr^.next;
    prev:=prev^.next;
    end;
  end;

end;


function TListCl.Get_a(i:integer): Integer;
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

function TListCl.Get_b(i:integer): Integer;
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

function TListCl.Print: String;
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
    st := st + IntToStr(curr^.a) +'-'+floatToStr(Round(curr^.b*1000)/1000) + #13#10;
    curr := curr^.next;
  end;
  if n <> 0
    then  begin
     result:=IntToStr(n)+' объектов в списке:' + #13 + st;
    end
  else result:='Список пуст';
end;

constructor TListCl.Create;
begin
  inherited Create;
  head:=nil;
  Count:=0;
end;

destructor TListCl.Destroy;
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
 
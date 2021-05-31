unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, jpeg, ExtCtrls, ComCtrls,ListCl,ListObj,ListLine,Math ;

type
  IntArray= array of array of integer;
    LineParams = record
     a: double;
     b: double;
     end;
  TForm1 = class(TForm)
    Button1: TButton;
    Button2: TButton;
    Memo1: TMemo;
    PageControl1: TPageControl;
    TabSheet1: TTabSheet;
    TabSheet2: TTabSheet;
    Image1: TImage;
    Image2: TImage;
    Image3: TImage;
    Edit1: TEdit;
    Edit2: TEdit;
    Label1: TLabel;
    Label2: TLabel;
    Edit3: TEdit;
    Image5: TImage;
    Image4: TImage;
    Image6: TImage;
    procedure Button2Click(Sender: TObject);
  private
    procedure BlackWhite;
    procedure Razmetka;
//    procedure MinPix_1_To_3;
    procedure SetNull;
    procedure Line_search;
 //   function otsuThreshold(var xn,xk,yn,yk: Integer): Integer;
    procedure Gray;
    procedure Desc;
    procedure GetItemColors;
    procedure GetItems;
    procedure ShowColors(count: Integer);
    procedure RGBToHSL(R,G,B:double;var Hue: double; var Saturation: double;var Light: double);
    procedure GetItemColorsHSL;
    procedure ShowObject(index: Integer);
    procedure GoLeft(var x, y, j: Integer);
    procedure DoLine(var x,y,j: Integer; Ins: Boolean);
    procedure GoDown(var x, y, j: Integer);
    procedure GoRight(var x, y, j: Integer);
    procedure GoUp(var x, y, j: Integer);
    function Is_Ellipce(pnt: IntArray): boolean;
    function Is_Line(pnt: IntArray; n:Integer): boolean;
    function Get_LineParams(pnt: IntArray; n: Integer): LineParams;
    procedure ClearImg;
 //   procedure Change_Size(var bmp: TBitmap; Height: Integer);

    { Private declarations }
  public
    { Public declarations }
  end;

type
    TPixelBMP24 = packed record
     R: byte;
     G: byte;
     B: byte;
     end;



     ColorSet = record
     R: byte;
     G: byte;
     B: byte;
     count: Integer;
     end;
     Pnt = record
     x:integer;
     y: integer;
     end;

     Desc_rect = record
     count_obj: Integer;
     Pixels_rect: Integer;
     id_obj: array of Integer;
     Pixels_obj: array of Integer;
     Summ: array of Pnt;
     MinX: array of Pnt;
     MinY: array of Pnt;
     MaxX: array of Pnt;
     MaxY: array of Pnt;
     end;

     ColorHSL = record
     h: double;
     S: double;
     L: double;
     end;

     THSV = record  // hue saturation value (HSV)
     Hue , Sat , Val : Double;
     end;
     matrica=array[1..5,1..5] of double;

var
  Form1: TForm1;
  DynArray: IntArray;     // черно-белый
  DynArray2: IntArray;    // переходы
  DynArray3: IntArray;
  DynArray4: IntArray;    // индексы объектов
  Desc_Item: IntArray;
  Gx, Gy, Gxy: IntArray;  // градиенты (по x,y), магнитуда
  Arr_Colors: array of ColorSet;
  horiz, vert: TListCl;
  magn: TListObj;
  segm: Array of TListObj;
  Lines: Array of TListLine;
  x_n,y_n,rec: integer;      //начальные индексы сетки доски, rec - размерность доски
  Arr_ColorsHSL: array of array of ColorHSL;
  Arr_ColorsHSV: array of array of THSV;
  Arr_ColorsYUV: array of array of TPixelBMP24;
  Arr_ParamObj: array of ColorHSL;
  Desc_Info: array of array of Desc_rect;
implementation

uses GBlur2;
{$R *.dfm}

function BMPRGBtoYUV(rgb: TPixelBMP24): TPixelBMP24;
var y,u,v:longint;
begin
   y := rgb.G*150 + rgb.B*29 + rgb.R*77; // 0.587 x 256, 0.114 x 256, 0.299 x 256
   u := (rgb.B shl 8 - y) * 144;         // 0.564 x 256
   v := (rgb.R shl 8 - y) * 183;         // 0,713 x 256
   Result.G :=y shr 8;
   Result.B :=u shr 16 + $80;
   Result.R :=v shr 16 + $80;
end;

procedure movestrings(k,l:integer; var a: matrica; n:integer); //Процедура перестановки k-ой и l-ой строк в матрице а порядка n
var j : integer;
    r : double;
begin
 if (k<=n) and (l<=n)then
 begin
 for j := 1 to n do
  begin
   r := a[l,j];
   a[l,j] :=a[k,j];
   a[k,j] := r;
  end;
 end
end; //movestrings

procedure prhod(var a:matrica; n : integer; var det: double);
//Преобразование матрицы а, размерности n и вычисление определителя det - соответствует "прямому ходу" метода Гаусса
var
 i,j,k,l : integer;
 b : matrica;
 k1,k2,d:double;
begin
 d:=1;
 for k := 1 to n-1 do
 begin
 //Если ведущий элемент ненулевой
  if a[k,k]<>0 then
  k1:=a[k,k]
  else
 //В противном случае: перестановка строк,...
  begin
    l:=k;
    repeat
    l:=l+1
 //ищем первый ненулевой элемент данного столбца, стоящий ниже диагонального,...
    until (a[l,k]<>0) or (l=n+1);
 //если такой элемент найден,...
   if l<=n then
   begin
//...меняем строки местами,...
   movestrings(k,l,a,n);
//...определитедь умножается на -1,...
   d:=d*(-1);
//...определение значения ведущего элемента - k1,...
   k1:=a[k,k];
   end
   else
   //В противном случае такой элемент отсутствует, что означает, что определитель системы равен нулю
   begin
    det:=0;
    //выход из процедуры
    exit;
   end;
 end;
if d<>0 then
//Вычитание из каждой i-ой строки, лежащей ниже k-ой,...
for i := k+1 to n do
begin
k2:=a[i,k];
//...вычитание k-ой строки, умноженной на коэффициент
for j := k to  n+1   do
a[i,j] := a[i,j]-a[k,j]*k2/k1;
end;//цикл по i
d:=d*a[k,k];// readln;wywod; writeln('d=',d:5:2);
end;//цикл по k
det:=d*a[n,n]; // Определитель системы равен произведению диагональных элементов
end;


function HSVtoRGB(H:Integer; S, V: Byte):TColor;
var
  ht, d, t1, t2, t3:Integer;
  R,G,B:Word;
begin
  if S = 0 then
   begin
    R := V; G := V; B := V;
   end
  else
   begin
    ht := H * 6;
    d := ht mod 360;
    t1 := round(V * (255 - S) / 255);
    t2 := round(V * (255 - S * d / 360) / 255);
    t3 := round(V * (255 - S * (360 - d) / 360) / 255);
    case ht div 360 of
    0:begin
        R := V; G := t3; B := t1;
      end;
    1:begin
        R := t2; G := V; B := t1;
      end;
    2:begin
        R := t1; G := V; B := t3;
      end;
    3:begin
        R := t1; G := t2; B := V;
      end;
    4:begin
        R := t3; G := t1; B := V;
      end;
    else begin
        R := V; G := t1; B := t2;
         end;
    end;
   end;
  Result:=RGB(R,G,B);
end;

function BMPYUVtoRGB(yuv: TPixelBMP24): TPixelBMP24;
var temp: integer;
begin
   temp := yuv.G + (yuv.B - $80) * 256 div 144  ;
   if temp > 0 then Result.B:=temp else Result.B:=0;
   if temp > 255 then Result.B:=255;
   temp := yuv.G + (yuv.R - $80) * 256 div 183  ;
   if temp > 0 then Result.R:=temp else Result.R:=0;
   if temp > 255 then Result.R:=255;
   temp := (yuv.G shl 8 - Result.B*29 - Result.R*77) div 150;
   if temp > 0 then Result.G:=temp else Result.G:=0;
   if temp > 255 then Result.G:=255;
end;

function RGB2HSV (R,G,B : Byte) : THSV;
var
  Min_, Max_, Delta : Double;
  H , S , V : Double ;
begin
  H := 0.0 ;
  Min_ := Min (Min( R,G ), B);
  Max_ := Max (Max( R,G ), B);
  Delta := ( Max_ - Min_ );
  V := Max_ ;
  If ( Max_ <> 0.0 ) then
    S := 255.0 * Delta / Max_
  else
    S := 0.0 ;
  If (S <> 0.0) then
    begin
      If R = Max_ then
        H := (G - B) / Delta
      else
        If G = Max_ then
          H := 2.0 + (B - R) / Delta
        else
          If B = Max_ then
            H := 4.0 + (R - G) / Delta
    End
  else
    H := -1.0 ;
  H := H * 60 ;
  If H < 0.0 then H := H + 360.0;
  with Result Do
    begin
      Hue := H ;             // Hue -> 0..360
      Sat := S * 100 / 255; // Saturation -> 0..100 %
      Val := V * 100 / 255; // Value - > 0..100 %
    end;
end;

function HSL2RGB(H: double; S,L: double):RGBTriple;
var
  i: Integer;
  p,q,Hk: double;
  t: array[1..3] of double;
  c: array[1..3] of double;
begin
  if L<0.5 then q:=L*(1+S)
  else q:=L+S-(L*S);
  p:=2*L-q;
  Hk:=H/360;
  t[1]:=Hk+1/3;
  t[2]:=Hk;
  t[3]:=Hk-1/3;
  for i:=1 to 3 do
  begin
   if t[i]<0 then t[i]:=t[i]+1
   else if t[i]>1 then t[i]:=t[i]-1;
   if t[i]<1/6 then
     c[i]:=P+((q-p)*6*t[i])
   else if t[i]<1/2 then
     c[i]:=q
   else if t[i]<1/2 then
     c[i]:=P+((q-p)*6*(2/3-t[i]))
   else
     c[i]:=p;
   end;
   result.rgbtRed:=Round(c[1]*255);
   result.rgbtGreen:=Round(c[2]*255);
   result.rgbtBlue:=Round(c[3]*255);
end;

procedure TForm1.RGBToHSL(R,G,B:double;var Hue: double;
  var Saturation: double;
  var Light: double);
var
    minRGB, maxRGB, delta ,sum: double;
begin
    R:=R/255;
    G:=G/255;
    B:=B/255;
    minRGB := Min(Min(R, G), B) ;
    maxRGB := Max(Max(R, G), B) ;
    delta := (maxRGB - minRGB) ;
    sum:=(maxRGB + minRGB);
    Light:=sum/2;
    if (Light = 0) or (maxRGB = minRGB)  then Saturation := 0
    else if Light <= 0.5 then Saturation := delta / (2 * Light)
    else Saturation := delta / (2 - 2* Light);
    if (Saturation <> 0) then
    begin
      if R = maxRGB then
        begin
        if G>=B then Hue := 60*(G - B) / Delta + 0
        else Hue := 60* (G - B) / Delta + 360;
        end
      else
        if G = maxRGB then Hue := 60* (B - R) / Delta + 120
        else
          if B = maxRGB then Hue := 60* (R - G) / Delta + 240;
    end
    else Hue := 0;
end;


function Sobel(x: Integer; y: Integer; var bmp:  IntArray): Double;
//var
//i,j :Integer;
//maskH,maskV: array[1..3,1..3] of integer;
begin
 //  maskH[1,1]:=-1; maskH[2,1]:=0; maskH[3,1]:=1;
 //  maskH[1,2]:=-2; maskH[2,2]:=0; maskH[3,2]:=2;
  // maskH[1,3]:=-1; maskH[2,3]:=0; maskH[3,3]:=1;

 //  maskV[1,1]:=-1; maskV[2,1]:=-2; maskV[3,1]:=-1;
 //  maskV[1,2]:=0;  maskV[2,2]:=0;  maskV[3,2]:=0;
 //  maskV[1,3]:=1;  maskV[2,3]:=2;  maskV[3,3]:=1;
   Gy[x,y]:=Round((bmp[x-1,y-1]+2*bmp[x-1,y]+bmp[x-1,y+1]-bmp[x+1,y-1]-2*bmp[x+1,y]-bmp[x-1,y+1])/4);
   Gx[x,y]:=Round((bmp[x-1,y-1]+2*bmp[x,y-1]+bmp[x+1,y-1]-bmp[x-1,y+1]-2*bmp[x,y+1]-bmp[x+1,y+1])/4);
   Gxy[x,y]:=Round(sqrt(Gy[x,y]*Gy[x,y]+Gx[x,y]*Gx[x,y]));
   Result:=Gxy[x,y];
end;

function SobelS(x: Integer; y: Integer): Double;
//var
//i,j :Integer;
//maskH,maskV: array[1..3,1..3] of integer;
begin
 //  maskH[1,1]:=-1; maskH[2,1]:=0; maskH[3,1]:=1;
 //  maskH[1,2]:=-2; maskH[2,2]:=0; maskH[3,2]:=2;
  // maskH[1,3]:=-1; maskH[2,3]:=0; maskH[3,3]:=1;

 //  maskV[1,1]:=-1; maskV[2,1]:=-2; maskV[3,1]:=-1;
 //  maskV[1,2]:=0;  maskV[2,2]:=0;  maskV[3,2]:=0;
 //  maskV[1,3]:=1;  maskV[2,3]:=2;  maskV[3,3]:=1;
   Result:= sqrt(Power(Arr_ColorsHSV[x-1,y-1].Sat+2*Arr_ColorsHSV[x-1,y].Sat+Arr_ColorsHSV[x-1,y+1].Sat-
                  Arr_ColorsHSV[x+1,y-1].Sat-2*Arr_ColorsHSV[x+1,y].Sat-Arr_ColorsHSV[x-1,y+1].Sat,2)+
            Power(Arr_ColorsHSV[x-1,y-1].Sat+2*Arr_ColorsHSV[x,y-1].Sat+Arr_ColorsHSV[x+1,y-1].Sat-
                  Arr_ColorsHSV[x-1,y+1].Sat-2*Arr_ColorsHSV[x,y+1].Sat-Arr_ColorsHSV[x+1,y+1].Sat,2));
end;

procedure TForm1.GoLeft(var x,y,j: Integer);
begin
       j:=1;
       if (Gxy[x-1, y-1] > Gxy[x-1, y]) and (Gxy[x-1, y-1] > Gxy[x-1, y+1]) then
       begin  x := x-1; y := y-1; end    // Up-Left
       else if (Gxy[x-1, y+1] > Gxy[x-1, y]) and (Gxy[x-1, y+1] > Gxy[x-1, y-1]) then
          begin  x := x-1; y := y+1; end // Down-Left
       else x := x-1;                    // Straight-Left
       DoLine(x,y,j,false);
end;
procedure TForm1.GoRight(var x,y,j: Integer);
begin
       j:=2;
             if (Gxy[x+1, y-1] > Gxy[x+1, y]) and (Gxy[x+1, y-1] > Gxy[x+1, y+1]) then
               begin  x := x+1; y := y-1; end    // Up-R
              else if (Gxy[x+1, y+1] > Gxy[x+1, y]) and (Gxy[x+1, y+1] > Gxy[x+1, y-1]) then
               begin  x := x+1; y := y+1; end // Down-R
              else x := x+1;                    // Straight-R
       DoLine(x,y,j,true);

end;
procedure TForm1.GoUp(var x,y,j: Integer);
begin
       j:=3;
        if (Gxy[x-1, y-1] > Gxy[x, y-1]) and (Gxy[x-1, y-1] > Gxy[x+1, y-1]) then
               begin  x := x-1; y := y-1; end    // Up-Left
              else if (Gxy[x+1, y-1] > Gxy[x-1, y-1]) and (Gxy[x+1, y-1] > Gxy[x, y-1]) then
               begin  x := x+1; y := y-1; end   // Up-R
              else y := y-1;                    // Up
       DoLine(x,y,j,false);
end;
procedure TForm1.GoDown(var x,y,j: Integer);
begin
       j:=4;
       if (Gxy[x-1, y+1] > Gxy[x, y+1]) and (Gxy[x-1, y+1] > Gxy[x+1, y+1]) then
               begin  x := x-1; y := y+1; end    // down-Left
              else if (Gxy[x+1, y+1] > Gxy[x-1, y+1]) and (Gxy[x+1, y-1] > Gxy[x, y+1]) then
               begin  x := x+1; y := y+1; end   // down-R
              else y := y+1;                    // down
       DoLine(x,y,j,true);
end;

procedure TForm1.DoLine(var x,y,j: Integer; Ins: Boolean);
var
 x1,x2,y1,y2,i,n,k: INteger;
begin
     while (x*y<>0) and (x<>Image1.Width) and (y<>Image1.Height) and (DynArray4[x,y]=0) and (Gxy[x,y]>0) do
       begin
          DynArray4[x,y]:=Length(segm);
          if ins then
            segm[Length(segm)-1].Insert(DynArray3[x,y],x,y)
          else
            segm[Length(segm)-1].New_obj(DynArray3[x,y],x,y,false);
      //    Application.ProcessMessages;
      //    Sleep(1);
          Image1.Canvas.Pixels[x,y]:=clGreen;
          dynArray4[x,y]:=Length(segm);
          if abs(Gx[x,y])>abs(Gy[x,y]) then
          begin
             if j=1 then
             begin
              if (Gxy[x-1, y-1] > Gxy[x-1, y]) and (Gxy[x-1, y-1] > Gxy[x-1, y+1]) then
               begin  x := x-1; y := y-1; end    // Up-Left
              else if (Gxy[x-1, y+1] > Gxy[x-1, y]) and (Gxy[x-1, y+1] > Gxy[x-1, y-1]) then
               begin  x := x-1; y := y+1; end // Down-Left
              else x := x-1;                    // Straight-Left
             end
             else if j=2 then
             begin
              if (Gxy[x+1, y-1] > Gxy[x+1, y]) and (Gxy[x+1, y-1] > Gxy[x+1, y+1]) then
               begin  x := x+1; y := y-1; end    // Up-R
              else if (Gxy[x+1, y+1] > Gxy[x+1, y]) and (Gxy[x+1, y+1] > Gxy[x+1, y-1]) then
               begin  x := x+1; y := y+1; end // Down-R
              else x := x+1;                    // Straight-R
             end
             else
             begin
              if max(Gxy[x-1, y-1],max(Gxy[x-1, y],Gxy[x-1, y+1]))>max(Gxy[x+1, y-1],max(Gxy[x+1, y],Gxy[x+1, y+1])) then
               begin
              if (Gxy[x-1, y-1] > Gxy[x-1, y]) and (Gxy[x-1, y-1] > Gxy[x-1, y+1]) then
               begin  x := x-1; y := y-1; end    // Up-Left
              else if (Gxy[x-1, y+1] > Gxy[x-1, y]) and (Gxy[x-1, y+1] > Gxy[x-1, y-1]) then
               begin  x := x-1; y := y+1; end // Down-Left
              else x:= x-1;                    // Straight-Left
              j:=1;
              end
              else
              begin
              if (Gxy[x+1, y-1] > Gxy[x+1, y]) and (Gxy[x+1, y-1] > Gxy[x+1, y+1]) then
               begin  x := x+1; y := y-1; end    // Up-R
              else if (Gxy[x+1, y+1] > Gxy[x+1, y]) and (Gxy[x+1, y+1] > Gxy[x+1, y-1]) then
               begin  x := x+1; y := y+1; end // Down-R
              else x := x+1;                    // Straight-R
              j:=2;
              end;
             end;
          end
          else
          begin
             if j=3 then
             begin
              if (Gxy[x-1, y-1] > Gxy[x, y-1]) and (Gxy[x-1, y-1] > Gxy[x+1, y-1]) then
               begin  x := x-1; y := y-1; end    // Up-Left
              else if (Gxy[x+1, y-1] > Gxy[x-1, y-1]) and (Gxy[x+1, y-1] > Gxy[x, y-1]) then
               begin  x := x+1; y := y-1; end   // Up-R
              else y := y-1;                    // Up
             end
             else if j=4 then
             begin
              if (Gxy[x-1, y+1] > Gxy[x, y+1]) and (Gxy[x-1, y+1] > Gxy[x+1, y+1]) then
               begin  x := x-1; y := y+1; end    // down-Left
              else if (Gxy[x+1, y+1] > Gxy[x-1, y+1]) and (Gxy[x+1, y-1] > Gxy[x, y+1]) then
               begin  x := x+1; y := y+1; end   // down-R
              else y := y+1;                    // down
             end
             else
             begin
              if max(Gxy[x-1, y-1],max(Gxy[x, y-1],Gxy[x+1, y-1]))>max(Gxy[x-1, y+1],max(Gxy[x, y+1],Gxy[x+1, y+1])) then
               begin
               if (Gxy[x-1, y-1] > Gxy[x, y-1]) and (Gxy[x-1, y-1] > Gxy[x+1, y-1]) then
               begin  x := x-1; y := y-1; end    // Up-Left
              else if (Gxy[x+1, y-1] > Gxy[x-1, y-1]) and (Gxy[x+1, y-1] > Gxy[x, y-1]) then
               begin  x := x+1; y := y-1; end   // Up-R
              else y := y-1;                    // Up
              j:=3;
              end
              else
              begin
               if (Gxy[x-1, y+1] > Gxy[x, y+1]) and (Gxy[x-1, y+1] > Gxy[x+1, y+1]) then
               begin  x := x-1; y := y+1; end    // down-Left
              else if (Gxy[x+1, y+1] > Gxy[x-1, y+1]) and (Gxy[x+1, y-1] > Gxy[x, y+1]) then
               begin  x := x+1; y := y+1; end   // down-R
              else y := y+1;                    // down
              j:=4;
              end;
             end;
          end;
       end;
       if (DynArray4[x,y]<>0) and (DynArray4[x,y]<>Length(segm)) then
       begin
        if ins then
        begin
          n:=Length(segm)-1;
          x1:=segm[n].Get_x(0);
          y1:=segm[n].Get_y(0);
          k:=DynArray4[x,y]-1;
          if (x1=segm[k].Get_x(0)) and (y1=segm[k].Get_y(0)) then
          begin
             for i:=0 to segm[k].Get_Count-1 do
             begin
                DynArray4[segm[k].Get_x(i),segm[k].Get_y(i)]:=n+1;
                segm[n].Insert(segm[k].Get_val(i),segm[k].Get_x(i),segm[k].Get_y(i));
             end;
             segm[k].ClearList;
          end
          else if (x1=segm[k].Get_x(segm[k].Get_Count-1)) and (y1=segm[k].Get_y(segm[k].Get_Count-1)) then
          begin
             for i:=segm[k].Get_Count-1 downto 0 do
             begin
                DynArray4[segm[k].Get_x(i),segm[k].Get_y(i)]:=n+1;
                segm[n].Insert(segm[k].Get_val(i),segm[k].Get_x(i),segm[k].Get_y(i));
             end;               
             segm[k].ClearList;
          end;
        end
        else
        begin
          n:=Length(segm)-1;
          x1:=segm[n].Get_x(segm[n].Get_Count-1);
          y1:=segm[n].Get_y(segm[n].Get_Count-1);
          k:=DynArray4[x,y]-1;
          if (x1=segm[k].Get_x(0)) and (y1=segm[k].Get_y(0)) then
          begin
             for i:=0 to segm[k].Get_Count-1 do
             begin
                DynArray4[segm[k].Get_x(i),segm[k].Get_y(i)]:=n+1;
                segm[n].New_obj(segm[k].Get_val(i),segm[k].Get_x(i),segm[k].Get_y(i),false);
             end;
             segm[k].ClearList;
          end
          else if (x1=segm[k].Get_x(segm[k].Get_Count-1)) and (y1=segm[k].Get_y(segm[k].Get_Count-1)) then
          begin
             for i:=segm[k].Get_Count-1 downto 0 do
             begin
                DynArray4[segm[k].Get_x(i),segm[k].Get_y(i)]:=n+1;
                segm[n].New_obj(segm[k].Get_val(i),segm[k].Get_x(i),segm[k].Get_y(i),false);
             end;
             segm[k].ClearList;
          end;
        end;
       end;
end;

procedure TForm1.Gray;
var
 bmp,sbmp ,SrcBMP,DstBMP24,ContrBMP: TBitMap;
 x, y ,imgh,imgw,sr,sr1,col,proc,del,i,j,count,n,q:integer;
 r,g,b,minL,maxL: byte;
 d,rowH,rowS, rowL, colH,colS,colL,Np,NPA,xx,yy:double;
 SrcPixel: ^TPixelBMP24;
 DstPixel: ^TPixelBMP24;
 yuv: TPixelBMP24;
 pix: RGBTriple;
 updown: boolean;
 pnts: IntArray;
 gist,gistH: array[0..255] of integer;
 porH,porS,porL : Integer;
 Lpar: LineParams;
begin
  SrcBMP:=TBitmap.Create;  {Src Bitmap erstellen}
  DstBMP24:=TBitmap.Create;  {24-Bit Dst Bitmap erstellen}
  sbmp:=Tbitmap.create;
  bmp:=TBitmap.Create;
  ContrBMP:=TBitmap.Create;
 try
  //загрузка исходного изображения
  SrcBMP.LoadFromFile(ExtractFilePath(Application.ExeName)+Edit1.Text+'.bmp'); //sollte 24Bit sein
  //уменьшение размеров
  imgh:=200;
  imgw:=Round(SrcBMP.Width*(200/SrcBMP.Height))  ;
  sbmp.width:=imgw;
  sbmp.Height:=imgh;
  sbmp.pixelFormat:=pf24bit;
  SetStretchBltMode(sbmp.canvas.handle,4);// ?????? ????????????
  StretchBlt(sbmp.canvas.handle,0,0,imgw,imgh,SrcBMP.canvas.handle,
               0,0,SrcBMP.width,SrcBMP.height,SRCCOPY);
  // создание массивов для обработки пикселей в системах HSL и YUV
  SetLength(Arr_ColorsHSL, sbmp.Width,sbmp.Height);
  SetLength(Arr_ColorsHSV, sbmp.Width,sbmp.Height);
  SetLength(Arr_ColorsYUV, sbmp.Width,sbmp.Height);

  DstBMP24.PixelFormat:=pf24bit;
  DstBMP24.Width:=sbmp.Width;
  DstBMP24.Height:=sbmp.Height;
  {SrcBMP.PixelFormat:=pf24bit;
  SrcBMP.Width:=DstBMP24.Width;
  SrcBMP.Height:=DstBMP24.Height;
 { ContrBMP.PixelFormat:=pf24bit;
  ContrBMP.Width:=DstBMP24.Width;
  ContrBMP.Height:=DstBMP24.Height;     }
  sr:=255; // sr1:=255;
  for y:=0 to sbmp.Height-1 do
     for x:=0 to sbmp.Width-1 do
     begin
       RGBToHSL(GetRValue(sbmp.Canvas.Pixels[x,y]),GetGValue(sbmp.Canvas.Pixels[x,y]),GetBValue(sbmp.Canvas.Pixels[x,y]),
       Arr_ColorsHSL[x,y].h,Arr_ColorsHSL[x,y].s,Arr_ColorsHSL[x,y].l);
       Arr_ColorsHSV[x,y]:=RGB2HSV(GetRValue(sbmp.Canvas.Pixels[x,y]),GetGValue(sbmp.Canvas.Pixels[x,y]),GetBValue(sbmp.Canvas.Pixels[x,y]));
       r:=Round(255-Arr_ColorsHSL[x,y].l*255);
     // pix:=HSL2RGB(Arr_ColorsHSL[x,y].h,Arr_ColorsHSL[x,y].s,Arr_ColorsHSL[x,y].l);
     //  SrcBMP.Canvas.Pixels[x,y]:=RGB(r, r, r);
       sr:=Min(sr,r);
     //  r:=Round(255-Arr_ColorsHSL[x,y].s*255);
     //  ContrBMP.Canvas.Pixels[x,y]:=RGB(r, r, r);
     //  sr1:=Min(sr1,r);
     end;
  //GBlur(SrcBMP, StrToFloat(Edit3.Text));
  // GBlur(ContrBMP, StrToFloat(Edit3.Text));
 {  for y:=0 to DstBMP24.Height-1 do
     for x:=0 to DstBMP24.Width-1 do
      begin
        Arr_ColorsHSL[x,y].l:= ((getRValue(SrcBMP.Canvas.Pixels[x,y])-sr)/255*(1-Arr_ColorsHSL[x,y].s)+Arr_ColorsHSL[x,y].l);
        if Arr_ColorsHSL[x,y].l>1 then Arr_ColorsHSL[x,y].l:=1
        else if Arr_ColorsHSL[x,y].l<0 then Arr_ColorsHSL[x,y].l:=0;
       { Arr_ColorsHSL[x,y].s:= ((getRValue(ContrBMP.Canvas.Pixels[x,y])-sr1)/255*0.8+Arr_ColorsHSL[x,y].s);
        if Arr_ColorsHSL[x,y].s>1 then Arr_ColorsHSL[x,y].s:=1
        else if Arr_ColorsHSL[x,y].s<0 then Arr_ColorsHSL[x,y].s:=0;                   }
   {     pix:=HSV2RGB(Arr_ColorsHSL[x,y].h,Arr_ColorsHSL[x,y].s,Arr_ColorsHSL[x,y].l);
        sbmp.Canvas.Pixels[x,y]:=RGB(pix.rgbtRed,pix.rgbtGreen,pix.rgbtBlue);
      end;
                               }
  //преобразование sbmp в оттенки серого DstBMP24
  minL:=255; maxL:=0;
  For x:=0 to 255 do gist[x]:=0;
  for y:=0 to sbmp.Height-1 do   //  DstBMP24
  begin
     SrcPixel:=sbmp.ScanLine[y];
     DstPixel:=DstBMP24.ScanLine[y];
     for x:=0 to sbmp.Width-1 do
     begin
       yuv:=BMPRGBtoYUV(SrcPixel^);
       Arr_ColorsYUV[x,y].R :=yuv.R;
       Arr_ColorsYUV[x,y].G :=yuv.G;
       Arr_ColorsYUV[x,y].B :=yuv.B;
       yuv.R:=$80;
       yuv.B:=$80;
       DstPixel^ := BMPYUVtoRGB(yuv);
       inc(SrcPixel);
       inc(DstPixel);
       inc(gist[Arr_ColorsYUV[x,y].G]);
     end;
   end;
   For x:=0 to 255 do
     if gist[x]>200 then
     begin
        minL:=x;
        break;
     end;
   For x:=255 downto 0 do
     if gist[x]>200 then
     begin
        maxL:=x;
        break;
     end;
 //  Image1.Canvas.Draw(DstBMP24.Width, 0, DstBMP24);
 //  Image1.Picture.Graphic:=DstBMP24;
 //  Image5.Canvas.Draw(sbmp.Width, 0, sbmp);
  // Image5.Picture.Graphic:=sbmp;
 //  application.ProcessMessages;
  //    ShowMessage(InTToStr(minL)+'-'+InTToStr(MaxL));
{ for y:=0 to DstBMP24.Height-1 do   //  DstBMP24 \
  begin
    DstPixel:=DstBMP24.ScanLine[y];
    SrcPixel:=sbmp.ScanLine[y];
    for x:=0 to DstBMP24.Width-1 do
    begin
        if  (Arr_ColorsYUV[x,y].G>=minL) and(Arr_ColorsYUV[x,y].G<=maxL) then
          Arr_ColorsYUV[x,y].G :=Round((255-30)*(Arr_ColorsYUV[x,y].G-minL)/(maxL-minL)+30)
        else
          Arr_ColorsYUV[x,y].G:=0;
        yuv.G:=Arr_ColorsYUV[x,y].G;
        yuv.R:=Arr_ColorsYUV[x,y].R;
        yuv.B:=Arr_ColorsYUV[x,y].B;
        SrcPixel^ := BMPYUVtoRGB(yuv);
    end;
  end;      }
  SetLength(DynArray2,imgw ,imgh);
  SetLength(DynArray3, imgw,imgh);
   //создание гауссовского размытия (bmp) и нормализация яркости (DstBMP24)
   bmp.PixelFormat:=pf24bit;
   bmp.Width:=DstBMP24.Width;
   bmp.Height:=DstBMP24.Height;
   BitBlt(bmp.canvas.Handle,0, 0, DstBMP24.Width, DstBMP24.Height,DstBMP24.Canvas.Handle, 0, 0, SRCCOPY);
   GBlur(DstBMP24,40);
    for y:=0 to DstBMP24.Height-1 do
    begin
     DstPixel:=sbmp.ScanLine[y];
     for x:=0 to DstBMP24.Width-1 do
     begin
        r:=255-GetRValue(bmp.Canvas.Pixels[x,y])+(getRValue(DstBMP24.Canvas.Pixels[x,y]));
        DstBMP24.Canvas.Pixels[x,y]:=RGB(r, r, r);
        r:=GetRValue(bmp.Canvas.Pixels[x,y])-Round(1/20/(Arr_ColorsHSL[x,y].s+0.1)*(GetRValue(bmp.Canvas.Pixels[x,y])-getRValue(DstBMP24.Canvas.Pixels[x,y])));
        yuv.G:=r;
        yuv.R:=Arr_ColorsYUV[x,y].R;
        yuv.B:=Arr_ColorsYUV[x,y].B;
       // DstPixel^ := BMPYUVtoRGB(yuv);
        inc(DstPixel);
      //  pix:=HSL2RGB(Arr_ColorsHSL[x,y].h,0.5,0.5);
      //  r:=Round(-Arr_ColorsHSL[x,y].s*255+Arr_ColorsHSL[x,y].l*255);
      //  r:=Round(255-RGB2HSV(GetRValue(sbmp.Canvas.Pixels[x,y]),GetGValue(sbmp.Canvas.Pixels[x,y]),GetBValue(sbmp.Canvas.Pixels[x,y])).Sat/100*255);
      //  Image5.Canvas.Pixels[x,y]:=RGB(r,r,r);
     //   Image5.Canvas.Pixels[x,y]:=RGB(pix.rgbtRed,pix.rgbtGreen,pix.rgbtBlue);
     end;
    end;
   Image1.Canvas.Draw(DstBMP24.Width, 0, DstBMP24);
   Image1.Picture.Graphic:=DstBMP24;
   application.ProcessMessages;
   { for y:=0 to DstBMP24.Height-1 do
     for x:=0 to DstBMP24.Width-1 do
      if dynarray3[x,y]=1 then Image5.Canvas.Pixels[x,y]:=clBlack else Image5.Canvas.Pixels[x,y]:=clWhite; }
   For x:=0 to 255 do gist[x]:=0;
    for y:=0 to sbmp.Height-1 do   //  DstBMP24
    begin
     SrcPixel:=sbmp.ScanLine[y];
     DstPixel:=DstBMP24.ScanLine[y];
     for x:=0 to sbmp.Width-1 do
     begin
       yuv:=BMPRGBtoYUV(SrcPixel^);
       Arr_ColorsYUV[x,y].R :=yuv.R;
       Arr_ColorsYUV[x,y].G :=yuv.G;
       Arr_ColorsYUV[x,y].B :=yuv.B;
       yuv.R:=$80;
       yuv.B:=$80;
       DstPixel^ := BMPYUVtoRGB(yuv);
       inc(SrcPixel);
       inc(DstPixel);
 //     inc(gist[Arr_ColorsYUV[x,y].G]);
     end;
   end;
//   Image5.Canvas.Draw(sbmp.Width, 0, DstBMP24);
//   Image5.Picture.Graphic:=DstBMP24;
   for y:=0 to sbmp.Height-1 do
    for x:=0 to sbmp.Width-1 do
    begin
    //  if ((y=0) or (y=sbmp.Height-1) or (x=0) or (x=sbmp.Width-1)) then
         DynArray2[x,y]:=255- GetRValue(DstBMP24.Canvas.Pixels[x,y]);
   //   begin
    //     DynArray2[x,y]:= 5*GetRValue(DstBMP24.Canvas.Pixels[x,y])-GetRValue(DstBMP24.Canvas.Pixels[x-1,y])-GetRValue(DstBMP24.Canvas.Pixels[x+1,y])-
   //      GetRValue(DstBMP24.Canvas.Pixels[x,y+1])-GetRValue(DstBMP24.Canvas.Pixels[x,y-1]);
   //   end;
    end;
   magn:= TListObj.Create;
   SetLength(DynArray4, imgw,imgh);
   SetLength(Gx, imgw,imgh);
   SetLength(Gy, imgw,imgh);
   SetLength(Gxy, imgw,imgh);
   for y:=0 to sbmp.Height-1 do
    for x:=0 to sbmp.Width-1 do
    begin
      if (y=0) or (y=sbmp.Height-1) or (x=0) or (x=sbmp.Width-1)  then
       r:=0
      else
      begin
      // DynArray2[x,y]:=Round(Sobel(x,y,DstBMP24)/255);
      r:=Round(Sobel(x,y,DynArray2));
     // if r=0 then
     //   r:=Round(SobelS(x,y)/100)*255;
      end;
      DynArray3[x,y]:=Round(r);///255);
     // r:=255-r;
      sbmp.Canvas.Pixels[x,y]:=RGB(r,r,r);
      Image1.Canvas.Pixels[x,y]:=clWhite;
   end;
   application.ProcessMessages;
   for y:=1 to sbmp.Height-2 do
    for x:=1 to sbmp.Width-2 do
    begin
      DynArray4[x,y]:=0;
      if (DynArray3[x,y]>15) and (DynArray3[x,y]>DynArray3[x+1,y]) and (DynArray3[x,y]>DynArray3[x-1,y])
      and (DynArray3[x,y]>DynArray3[x-1,y+1]) and (DynArray3[x,y]>DynArray3[x,y+1])
      and (DynArray3[x,y]>DynArray3[x+1,y+1]) and (DynArray3[x,y]>DynArray3[x-1,y-1])
      and (DynArray3[x,y]>DynArray3[x,y-1])   and (DynArray3[x,y]>DynArray3[x+1,y-1]) then
      begin
          magn.New_obj(r,x,y,true);
          Image1.Canvas.Pixels[x,y]:=clGreen;
      end;
    end;
   For i:=0 to magn.Get_Count-1 do
   begin
     x:=magn.Get_x(i);
     y:=magn.Get_y(i);
     j:=0;                   // 1-влево 2-вправо 3-вверх 4-вниз
     if (DynArray4[x,y]=0) and (y*x<>0) and (x<>imgw-1) and (y<>imgh-1) then
     begin
      SetLength(segm,Length(segm)+1);
      segm[Length(segm)-1]:= TListObj.Create;
      segm[Length(segm)-1].New_obj(DynArray3[x,y],x,y,false);
      DynArray4[x,y]:=Length(segm);
      Image1.Canvas.Pixels[x,y]:=clGreen;
      if abs(Gx[x,y])>abs(Gy[x,y]) then
      begin
        GoLeft(x,y,j);
        x:=magn.Get_x(i);
        y:=magn.Get_y(i);
        GoRight(x,y,j);
      end
      else
      begin
        GoUp(x,y,j);
        x:=magn.Get_x(i);
        y:=magn.Get_y(i);
        GoDown(x,y,j);
      end;
     end;
   end;
//   Application.ProcessMessages;
//   ShowMessage(IntTOStr(Length(segm)));
   // отсев незначимых элементов
   count:=0;
  { Np:=0;
   For i:=0 to 255 do
     gistH[i]:=0;
   For i:=0 to Length(segm)-1 do
   begin
     For j:=0 to segm[i].Get_Count-1 do
       For x:= segm[i].Get_val(j) downto 0 do
         inc(gistH[x]);
     Np:=Np+segm[i].Get_Count*(segm[i].Get_Count-1)/2;
   end;
   count:=0;
   i:=0;
   While i< Length(segm) do
   begin
     NPA:= NP*Power(gistH[segm[i].Get_val(segm[i].Get_IdMin)]/gistH[0],segm[i].Get_Count);
     if (segm[i].Get_Count<3) or (NPA<100) then
        inc(i)
     else
     begin
      SetLength(segm,Length(segm)+1);
      segm[Length(segm)-1]:= TListObj.Create;
      segm[Length(segm)-1].SetHead(segm[i].RazrezPoMin);
      segm[Length(segm)-1].UpdateProp;
      if segm[Length(segm)-1].Get_Count=0 then
      begin
       FreeAndNil(segm[Length(segm)-1]);
       SetLength(segm,Length(segm)-1);
      end;
     end;
   end;                  }
   // Неудавшийся метод определения прямых
{   n:=12;
   SetLength(pnts,n,2);
   for i:=0 to Length(segm)-1 do
     if (segm[i].Get_Count>=n) then
       begin
       For j:=0 to segm[i].Get_Count-n do
        begin
        for x:=0 to n-1 do
        begin
         pnts[x,0]:=segm[i].Get_x(j+x);
         pnts[x,1]:=segm[i].Get_y(j+x);
        end;
        if Is_Line(pnts,n) then
        begin
          For x:=j to j+n-1 do
            Image1.Canvas.Pixels[segm[i].Get_x(x),segm[i].Get_y(x)]:=clWhite;
        end
        else
        begin
       //   For x:=j to j+n-1 do
      //      Image1.Canvas.Pixels[segm[i].Get_x(x),segm[i].Get_y(x)]:=clred;
        end;
        end;
        application.ProcessMessages;
       end;    }

 {  for i:=0 to Length(segm)-1 do
     if (segm[i].Get_Count>=9) then
       begin
        inc(count);
        For j:=0 to segm[i].Get_Count-1 do
         begin
            Image5.Canvas.Pixels[segm[i].Get_x(j),segm[i].Get_y(j)]:=clgreen;
         end;

        end;
       end;    }
  //  ShowMessage(IntTOStr(count));
   //линеаризация сегментов
   SetLength(Lines,Length(segm));
   n:=5;
   SetLength(pnts,n+1,2);
   ClearImg;
   for i:=0 to Length(segm)-1 do
   begin
     Lines[i]:= TListLine.Create;
     j:=0;
     Lpar.a :=0; Lpar.b:=0;
     count:=0;
     while (segm[i].Get_Count-j>=n) do
     begin
     if Lines[i].Get_Count=0 then      // если определяется первая линия сегмента
     begin
       for q:=0 to n-1 do
        begin
         pnts[q,0]:=segm[i].Get_x(j+q);
         pnts[q,1]:=segm[i].Get_y(j+q);
        end;
       Lpar:=Get_LineParams(pnts,n);
       if Lpar.a*Lpar.b =0 then
         Lines[i].New_obj(pnts[0,0],pnts[n-1,0],pnts[0,1],pnts[n-1,1],Lpar.a,Lpar.b)
       else
         Lines[i].New_obj(pnts[0,0],pnts[n-1,0],Round(Lpar.a+Lpar.b*pnts[0,0]),Round(Lpar.a+Lpar.b*pnts[n-1,0]),Lpar.a,Lpar.b);
       Image1.Canvas.pen.Color := clRed;
       Image1.Canvas.MoveTo(Lines[i].Get_x(0,1),Lines[i].Get_y(0,1));
       Image1.Canvas.LineTo(Lines[i].Get_x(0,2),Lines[i].Get_y(0,2));
    //   application.ProcessMessages;
       inc(count);
       j:=j+5;
     end
     else
     begin
       x:=segm[i].Get_x(j);
       y:=segm[i].Get_y(j);
       if Lpar.a*Lpar.b=0 then
       begin
        yy:=y;
        xx:=Lines[i].Get_x(count-1,2);
       end
       else
       begin
        yy:=Lines[i].Get_a(count-1)+Lines[i].Get_b(count-1)*x;
        if Lines[i].Get_b(count-1)=0 then xx:=x else
        xx:=(y-Lines[i].Get_a(count-1))/Lines[i].Get_b(count-1);
       end;
       if power(xx-x,2)+power(yy-y,2)>power(2,2) then   //создание новой линии
       begin
        for q:=0 to n do
        begin
         pnts[q,0]:=segm[i].Get_x(j+q-1);
         pnts[q,1]:=segm[i].Get_y(j+q-1);
        end;
        Lpar:=Get_LineParams(pnts,n+1);
        if Lpar.a*Lpar.b =0 then
         Lines[i].New_obj(pnts[0,0],pnts[n,0],pnts[0,1],pnts[n,1],Lpar.a,Lpar.b)
        else
         Lines[i].New_obj(pnts[0,0],pnts[n,0],Round(Lpar.a+Lpar.b*pnts[0,0]),Round(Lpar.a+Lpar.b*pnts[n,0]),Lpar.a,Lpar.b);
        Image1.Canvas.pen.Color := clRed;
        Image1.Canvas.MoveTo(Lines[i].Get_x(0,1),Lines[i].Get_y(0,1));
        Image1.Canvas.LineTo(Lines[i].Get_x(0,2),Lines[i].Get_y(0,2));
   //     application.ProcessMessages;
        inc(count);
        j:=j+5;
       end
       else       //добавление в текущую линию пикселя сегмента с проекцией на прямую
       begin
          Image1.Canvas.pen.Color := clWhite;
          Image1.Canvas.MoveTo(Lines[i].Get_x(0,1),Lines[i].Get_y(0,1));
          Image1.Canvas.LineTo(Lines[i].Get_x(0,2),Lines[i].Get_y(0,2));
       //   application.ProcessMessages;
          Lines[i].Edit(count-1,Round(xx),Round(yy));
          Image1.Canvas.pen.Color := clRed;
          Image1.Canvas.MoveTo(Lines[i].Get_x(0,1),Lines[i].Get_y(0,1));
          Image1.Canvas.LineTo(Lines[i].Get_x(0,2),Lines[i].Get_y(0,2));
          inc(j);
   //        application.ProcessMessages;
       end;
     end;
     end; //end while
     if (j<>segm[i].Get_Count) and (j<>0) then
     begin
    //   if (j<>0) then
   //    begin
       for q:=0 to segm[i].Get_Count-j do
        begin
         pnts[q,0]:=segm[i].Get_x(j+q-1);
         pnts[q,1]:=segm[i].Get_y(j+q-1);
        end;
       Lpar:=Get_LineParams(pnts,segm[i].Get_Count-j+1);
          if Lpar.a*Lpar.b =0 then
         Lines[i].New_obj(pnts[0,0],pnts[segm[i].Get_Count-j,0],pnts[0,1],pnts[segm[i].Get_Count-j,1],Lpar.a,Lpar.b)
        else
         Lines[i].New_obj(pnts[0,0],pnts[segm[i].Get_Count-j,0],Round(Lpar.a+Lpar.b*pnts[0,0]),Round(Lpar.a+Lpar.b*pnts[segm[i].Get_Count-j,0]),Lpar.a,Lpar.b);
    //   end
     {  else
       begin
        for q:=0 to segm[i].Get_Count-1 do
        begin
         pnts[q,0]:=segm[i].Get_x(q);
         pnts[q,1]:=segm[i].Get_y(q);
        end;
       Lpar:=Get_LineParams(pnts,segm[i].Get_Count);
        if Lpar.a*Lpar.b =0 then
         Lines[i].New_obj(pnts[0,0],pnts[segm[i].Get_Count-1,0],pnts[0,1],pnts[segm[i].Get_Count-1,1],Lpar.a,Lpar.b)
        else
         Lines[i].New_obj(pnts[0,0],pnts[segm[i].Get_Count-1,0],Round(Lpar.a+Lpar.b*pnts[0,0]),Round(Lpar.a+Lpar.b*pnts[segm[i].Get_Count-1,0]),Lpar.a,Lpar.b);
       end;  }

        Image1.Canvas.pen.Color := clRed;
        Image1.Canvas.MoveTo(Lines[i].Get_x(0,1),Lines[i].Get_y(0,1));
        Image1.Canvas.LineTo(Lines[i].Get_x(0,2),Lines[i].Get_y(0,2));
     end;

   end;




   //выделение объектов по критериям цвета, яркости, контраста
 {  SetLength(Arr_ParamObj,0);
   col:=0;
   SetLength(DynArray4, imgw,imgh);
   proc:=10;
  // porH:=36;  porS:=10;    porL:=10;
   rowH:=0;   rowS:=0;     rowL:=0;
   colH:=0;   colS:=0;     colL:=0;
   for y:=0 to sbmp.Height-1 do
    for x:=0 to sbmp.Width-1 do
    begin
      if (y=0) then
      begin
        if (x=0) then
        begin
          DynArray4[x,y]:=col;
          SetLength(Arr_ParamObj,1);
          Arr_ParamObj[col].h :=Arr_ColorsHSL[x,y].h;
          Arr_ParamObj[col].s :=Arr_ColorsHSL[x,y].s;
          Arr_ParamObj[col].l :=Arr_ColorsHSL[x,y].l;
          inc(col);
        end
        else
        begin
          rowH:=abs(Arr_ParamObj[DynArray4[x-1,y]].h - Arr_ColorsHSL[x,y].h);
          rowS:=abs(Arr_ParamObj[DynArray4[x-1,y]].s - Arr_ColorsHSL[x,y].s);
          rowL:=abs(Arr_ParamObj[DynArray4[x-1,y]].l - Arr_ColorsHSL[x,y].l);
          if max(rowH/3.6,max(rowS,rowL))>proc then
          begin
          DynArray4[x,y]:=col;
          SetLength(Arr_ParamObj,Length(Arr_ParamObj)+1);
          Arr_ParamObj[col].h :=Arr_ColorsHSL[x,y].h;
          Arr_ParamObj[col].s :=Arr_ColorsHSL[x,y].s;
          Arr_ParamObj[col].l :=Arr_ColorsHSL[x,y].l;
          inc(col);
          end
          else
            DynArray4[x,y]:=DynArray4[x-1,y];
        end;
      end
      else if (x=0) then
      begin
         colH:=abs(Arr_ParamObj[DynArray4[x,y-1]].h - Arr_ColorsHSL[x,y].h);
         colS:=abs(Arr_ParamObj[DynArray4[x,y-1]].s - Arr_ColorsHSL[x,y].s);
         colL:=abs(Arr_ParamObj[DynArray4[x,y-1]].l - Arr_ColorsHSL[x,y].l);
          if max(colH/3.6,max(colS,colL))>proc then
          begin
          DynArray4[x,y]:=col;
          SetLength(Arr_ParamObj,Length(Arr_ParamObj)+1);
          Arr_ParamObj[col].h :=Arr_ColorsHSL[x,y].h;
          Arr_ParamObj[col].s :=Arr_ColorsHSL[x,y].s;
          Arr_ParamObj[col].l :=Arr_ColorsHSL[x,y].l;
          inc(col);
          end
          else
            DynArray4[x,y]:=DynArray4[x,y-1];
      end
      else
      begin
         rowH:=abs(Arr_ParamObj[DynArray4[x-1,y]].h - Arr_ColorsHSL[x,y].h);
         rowS:=abs(Arr_ParamObj[DynArray4[x-1,y]].s - Arr_ColorsHSL[x,y].s);
         rowL:=abs(Arr_ParamObj[DynArray4[x-1,y]].l - Arr_ColorsHSL[x,y].l);
         colH:=abs(Arr_ParamObj[DynArray4[x,y-1]].h - Arr_ColorsHSL[x,y].h);
         colS:=abs(Arr_ParamObj[DynArray4[x,y-1]].s - Arr_ColorsHSL[x,y].s);
         colL:=abs(Arr_ParamObj[DynArray4[x,y-1]].l - Arr_ColorsHSL[x,y].l);
          if max(rowH/3.6,max(rowS,rowL))>proc then
          begin
            if max(colH/3.6,max(colS,colL))>proc then
            begin
            DynArray4[x,y]:=col;
            SetLength(Arr_ParamObj,Length(Arr_ParamObj)+1);
            Arr_ParamObj[col].h :=Arr_ColorsHSL[x,y].h;
            Arr_ParamObj[col].s :=Arr_ColorsHSL[x,y].s;
            Arr_ParamObj[col].l :=Arr_ColorsHSL[x,y].l;
            inc(col);
            end
            else
              DynArray4[x,y]:=DynArray4[x,y-1];
          end
          else if max(colH/3.6,max(colS,colL))>proc then
            DynArray4[x,y]:=DynArray4[x-1,y]
          else
          begin
           if max(abs(Arr_ParamObj[DynArray4[x-1,y]].h - Arr_ParamObj[DynArray4[x,y-1]].h)/3.6,
           max(abs(Arr_ParamObj[DynArray4[x-1,y]].s - Arr_ParamObj[DynArray4[x,y-1]].s),
           abs(Arr_ParamObj[DynArray4[x-1,y]].l - Arr_ParamObj[DynArray4[x,y-1]].l)))>proc then
           begin
             if max(rowH/3.6,max(rowS,rowL))>max(colH/3.6,max(colS,colL)) then
               DynArray4[x,y]:=DynArray4[x,y-1]
             else
               DynArray4[x,y]:=DynArray4[x-1,y] ;
           end
           else
           begin
           del:=DynArray4[x,y-1];
             for j:=0 to y-1 do
              for i:=0 to sbmp.Width-1 do
               if DynArray4[i,j]=del then DynArray4[i,j]:=DynArray4[x-1,y];
           DynArray4[x,y]:=DynArray4[x-1,y];
           end;
          end;
      end;
    end;
    Showmessage( InTToStr(Length(Arr_ParamObj))+' объектов'); }
   //GBlur(sbmp,1);
   Image5.Canvas.Draw(sbmp.Width, 0, sbmp);
   Image5.Picture.Graphic:=sbmp;
   finally
   sbmp.Free;
   bmp.Free;
   SrcBMP.free;
   DstBMP24.Free;
   ContrBMP.Free;
   SetLength(DynArray2,0 ,0);
   SetLength(Gx, 0,0);
   SetLength(Gy, 0,0);
   SetLength(Gxy, 0,0);
   SetLength(pnts,0,0);
   application.ProcessMessages;
   end;
end;



procedure TForm1.Button2Click(Sender: TObject);
var
st: TdateTime;
begin
   st:=now;
   try
   // черно-белый цвет
   Gray;
   application.ProcessMessages ;
 //  showMessage(')');
 {  BlackWhite;
   application.ProcessMessages ;
 //  showMessage(')');
   Razmetka;
  //    showMessage(')');
   Line_search;
   Desc;
   application.ProcessMessages;
   GetItemColors;   }
  // GetItems;
  // GetItemColorsHSL;
   finally
   SetNull;
   Label2.Caption:=TimeToStr(now-st);
   end;
end;

{procedure TForm1.MinPix_1_To_3;
var
 x, y,stepsx,stepsy,ostx,osty, sum,st,zap,proc :Integer;
 sumrow,sumcol: Array of integer;
  bmp,sbmp: TBitmap;
begin
  stepsx:=trunc(Image1.Width/3);
  stepsy:=trunc(Image1.Height/3);
  ostx:=Image1.Width-stepsx*3;
  osty:=Image1.Height-stepsy*3;
  if ostx>0 then
    begin
    if osty>0 then
     SetLength(DynArray3,stepsx+1,stepsy+1)
    else
     SetLength(DynArray3,stepsx+1,stepsy);
    end
  else if osty>0 then
     SetLength(DynArray3,stepsx,stepsy+1)
  else
     SetLength(DynArray3,stepsx,stepsy);
  for y := 0 to stepsy-1 do
  begin
    for x := 0 to stepsx-1 do
    begin
      sum:=0+DynArray[x*3,y*3]+DynArray[x*3+1,y*3]+DynArray[x*3+2,y*3]+DynArray[x*3,y*3+1]+DynArray[x*3+1,y*3+1]+DynArray[x*3+2,y*3+1]+DynArray[x*3,y*3+2]+DynArray[x*3+1,y*3+2]+DynArray[x*3+2,y*3+2];
      if sum>4 then
        DynArray3[x,y]:=1
      else
        DynArray3[x,y]:=0;
    end;
  end;
  if osty>0 then
  for x := 0 to stepsx-1 do
  begin
      sum:=DynArray[x*3,Image1.Height-1]+DynArray[x*3+1,Image1.Height-1]+DynArray[x*3+2,Image1.Height-1];
      if osty=2 then
      begin
        sum:=sum+DynArray[x*3,Image1.Height-2]+DynArray[x*3+1,Height-2]+DynArray[x*3+2,Height-2];
        if sum>3 then DynArray3[x,stepsy]:=1 else  DynArray3[x,stepsy]:=0;
      end
      else if sum>1 then DynArray3[x,stepsy]:=1 else  DynArray3[x,stepsy]:=0;
  end;
  if ostx>0 then
  for y := 0 to stepsy-1 do
  begin
      sum:=DynArray[Image1.Width-1,y*3]+DynArray[Image1.Width-1,y*3+1]+DynArray[Image1.Width-1,y*3+2];
      if ostx=2 then
      begin
        sum:=sum+ DynArray[Image1.Width-2,y*3]+DynArray[Image1.Width-2,y*3+1]+DynArray[Image1.Width-2,y*3+2];
        if sum>3 then DynArray3[stepsx,y]:=1 else DynArray3[stepsx,y]:=0;
      end
      else if sum>1 then DynArray3[stepsx,y]:=1 else DynArray3[stepsx,y]:=0;
  end;
  if osty*ostx>0 then
    if (osty=1) or (ostx=1)  then
      DynArray3[stepsx,stepsy]:=DynArray[Image1.Width-1,Image1.Height-1]
    else
    begin
      if DynArray[Image1.Width-2,Image1.Height-1]+DynArray[Image1.Width-1,Image1.Height-2]+DynArray[Image1.Width-2,Image1.Height-2]>2 then
      DynArray3[stepsx,stepsy]:=1
      else
      DynArray3[stepsx,stepsy]:=0;
    end;
  for y := 0 to stepsy do
    for x := 0 to stepsx do
      if DynArray3[x,y]=1 then
          Image4.Canvas.Pixels[x,y] := clBlack
      else
          Image4.Canvas.Pixels[x,y] := clWhite;
  //суммирование по рядам и столбцам
  SetLength(sumrow,stepsy+1) ;
  for y := 0 to stepsy do
    begin
    sumrow[y]:=0;
    Image2.Canvas.Pixels[stepsx,y] := clBlue;
    st:=0;
    for x := 0 to stepsx do
       if DynArray3[x,y]=1 then
       begin
       sumrow[y]:=sumrow[y]+1;
       Image2.Canvas.Pixels[st,y] := clBlue;
        st:=st+1;
       end;
    end;
  SetLength(sumcol,stepsx+1) ;
  zap:=0;
  for x := 0 to stepsx do
    begin
    sumcol[x]:=0;
    st:=0;
    Image3.Canvas.Pixels[x,stepsy]:= clBlue;
    for y := 0 to stepsy do
       if DynArray3[x,y]=1 then
       begin
       sumcol[x]:=sumcol[x]+1;
       Image3.Canvas.Pixels[x,st]:= clBlue;
       st:=st+1;
       end;
    zap:=zap+sumcol[x];
    end;
    proc:=round(zap/(stepsy+1)/(stepsx+1)*100);
    label1.Caption:=InttoStr(proc);
    if proc>50 then
    begin
      for x := 0 to stepsx do
        for y := 0 to stepsy do
        begin
        if Image4.Canvas.Pixels[x,y]= clBlack then
            Image4.Canvas.Pixels[x,y]:= clWhite
        else   Image4.Canvas.Pixels[x,y]:= clBlack;
        if Image2.Canvas.Pixels[x,y]= clBlue then
            Image2.Canvas.Pixels[x,y]:= clWhite
        else   Image2.Canvas.Pixels[x,y]:= clBlue;
        if Image3.Canvas.Pixels[x,y]= clBlue then
            Image3.Canvas.Pixels[x,y]:= clWhite
        else   Image3.Canvas.Pixels[x,y]:= clBlue;
        end;
    end;

    setlength(DynArray,stepsx+1,stepsy+1);
    for x := 0 to stepsx do
      for y := 0 to stepsy do
      begin
       DynArray[x,y]:=DynArray3[x,y];
       if DynArray[x,y]=1 then  Image1.Canvas.Pixels[x,y]:= clBlack else  Image1.Canvas.Pixels[x,y]:= clWhite;
      end;


end;   }

procedure TForm1.Razmetka;
var
 x, y ,imgh,imgw,col: integer;
begin
  //выделение переходов
   imgh :=Image1.Height;
   imgw :=Image1.Width;
  SetLength(DynArray2,imgw ,imgh) ;
  for y := 1 to Image1.Height-1 do
  begin
    for x := 1 to Image1.Width-1 do
    begin
      if (DynArray[x,y]<>DynArray[x-1,y]) then
      begin
        DynArray2[x,y]:=1;
        DynArray2[x-1,y]:=1;
     //     Image1.Canvas.Pixels[x,y] := clGreen  ;
     //     Image1.Canvas.Pixels[x-1,y] := clGreen  ;
      end
      else if(DynArray[x,y]<>DynArray[x,y-1]) then
      begin
    //  Image1.Canvas.Pixels[x,y] := clGreen;
      DynArray2[x,y]:=1;
    //  Image1.Canvas.Pixels[x,y-1] := clGreen;
      DynArray2[x,y-1]:=1;
      end
      else
      begin
      DynArray2[x,y]:=0;
    //  Image1.Canvas.Pixels[x,y] := clWhite;
      end;
    end;
  end;
{  for y := 1 to Image1.Height-2 do
  begin
    for x := 1 to Image1.Width-2 do
       begin
       col:=DynArray2[x,y]+DynArray2[x-1,y]+DynArray2[x+1,y]+DynArray2[x,y-1]+DynArray2[x-1,y-1]+DynArray2[x+1,y-1]+
       DynArray2[x,y+1]+DynArray2[x-1,y+1]+DynArray2[x+1,y+1];
       if col>5 then DynArray[x,y]:=1 else DynArray[x,y]:=0;
       end;
  end;
 for y := 1 to Image1.Height-2 do
  begin
    for x := 1 to Image1.Width-2 do
       if DynArray[x,y]=1 then
       begin
       DynArray2[x,y]:=1;
       Image5.Canvas.Pixels[x,y]:=clBlack;
       end
       else
       begin DynArray2[x,y]:=0;
       Image5.Canvas.Pixels[x,y]:=clWhite;
       end;
  end;      }
end;

procedure TForm1.Line_search;
var
 x, y ,xx,yy,i,ii,sovp,imgh,imgw,angle,angle_max,angle_min,a,max,ne_naid,min_y,max_y: integer;
 proc,proc2,sr,b_min,b_max: double;
 count_ang,sum_ang,a_min,a_max,y_max,y_min,x_min,x_max: integer;
 left,rigth: boolean;
 b: double;
 myFile : TextFile;
 st: String;
begin
   imgh := Image1.Height;
   imgw := Image1.Width;
   angle_max:=trunc(imgh/2-1);
   max:=0;
   min_y:=-1;
   max_y:=0;
   ne_naid:=0;
   left:=false;
   rigth:=false;
   count_ang:=0;
   sum_ang:=0;
  horiz:= TListCl.Create;
 // Image5.Picture.Graphic:=DstBMP24;
  for angle:=0 to angle_max  do
  begin
    b:=angle/(imgw-1);
    if left or rigth then
       ne_naid:=ne_naid+1;
    for a:=0 to (imgh-angle-2) do
    begin
     proc:=0.4-0.1*(a/(Image1.Height-1));
     if not rigth then
     begin
     sovp:=0;
     for x:=0 to imgw-1 do
      sovp:=sovp+DynArray2[x,Round(a+b*x)]+DynArray2[x,Round(a+b*x)+1];
     if sovp/2/(sqrt(imgw*imgw+angle*angle)) > proc then
     begin
     ne_naid:=0;
   // Showmessage(IntToStr(a)+'+'+FloatToStr(b)+'*x');
     left:=true;
     if (min_y<0) or (min_y>a) then min_y:=a;
     if max_y<(a+angle) then max_y:=(a+angle);
     count_ang:=count_ang+1;
     sum_ang:=sum_ang+angle;
     horiz.New_obj(a,a+angle);
  //  for x:=0 to imgw-1 do
  //     Image1.Canvas.Pixels[x,Round(a+b*x)]:=clgreen;
     end;
     if sovp>max then max:=sovp;
     end;
     if not left then
     begin
     sovp:=0;
     for x:=0 to imgw-1 do
      sovp:=sovp+DynArray2[imgw-1-x,Round(a+b*x)]+DynArray2[imgw-1-x,Round(a+b*x)+1];
     if sovp/2/(sqrt(imgw*imgw+angle*angle)) > proc then
     begin
   //  Showmessage('-'+IntToStr(a)+'+'+FloatToStr(b)+'*x');
     rigth:=true;
     ne_naid:=0;
     count_ang:=count_ang+1;
     sum_ang:=sum_ang+angle;
      horiz.New_obj(a+angle,a);
     if (min_y<0) or (min_y>a) then min_y:=a;
     if max_y<(a+angle) then max_y:=(a+angle);
     for x:=0 to imgw-1 do
       Image1.Canvas.Pixels[imgw-1-x,Round(a+b*x)]:=clgreen;
     end;
     if sovp>max then max:=sovp;
    end;
    end;
    if ne_naid>2 then break;
    if (count_ang<>0) and ((angle-(sum_ang/count_ang))>(angle_max/20)) then break;
  //  application.ProcessMessages;
  end;
  horiz.Get_Lines;
  horiz.Delete_Peres_Line;
  sr:=0;
 { for y:=0 to horiz.Get_Count-1 do
  begin
  Image1.Canvas.Pen.Color := clRed;
  //sr:=sr+horiz.Get_a(y)-horiz.Get_b(y);
  Image1.Canvas.MoveTo(0,horiz.Get_a(y));
  Image1.Canvas.LineTo(imgw-1,horiz.Get_b(y));
  end;      }
 // ShowMessage('');
  application.ProcessMessages;
  sr:=sr/horiz.Get_Count;
  //вертикальные линии
  vert:= TListCl.Create;
  a_min:=horiz.Get_a(0);
  b_min:=(horiz.Get_b(0)-a_min)/(imgw-1);
  a_max:=horiz.Get_a(horiz.Get_Count-1);
  b_max:=(horiz.Get_b(horiz.Get_Count-1)-a_max)/(imgw-1);
  ne_naid:=0;
 //  count_ang:=0;
 //  sum_ang:=0;
  imgh:=max_y-min_y;
  proc2:=0.4*Image1.Height/(max_y-min_y);
  for a:=0 to (imgw-2) do
  begin
    ne_naid:=ne_naid+1;
    if 2*a>(imgw-1) then
    begin
       angle_max:=trunc(imgw-2-a);
       angle_min:=-trunc(imgw/2-1);
    end
    else
    begin
       angle_max:=trunc(imgw/2-2);
       angle_min:=-a;//-trunc(a*(imgh-1-a_max)/a_max);
    end;
  //  Image5.Picture.Graphic:=DstBMP24;
   // proc2:=0.6*(max_y-min_y)/(max_y-min_y-sr)*angle_max/angle;
    for angle:=angle_min to angle_max  do
    begin
     b:=angle/(imgh-1);
     sovp:=0;
     y_min:=Round((a_min+a*b_min)/(1-b_min*b))-min_y;
     y_max:=Round((a_max+a*b_max)/(1-b_max*b))-min_y;
  //   Image1.Canvas.Pixels[Round(a+b*y_min),y_min]:=clBlue;
  //   Image1.Canvas.Pixels[Round(a+b*y_max),y_max]:=clBlue;
     for y:=y_min to y_max-1 do
     begin
      sovp:=sovp+DynArray2[Round(a+b*y),min_y+y]+DynArray2[Round(a+b*y)+1,min_y+y];
    //  Image5.Canvas.Pixels[Round(a+b*y),min_y+y]:=clgreen;
     end;
     if sovp/2/(sqrt((y_max-y_min)*(y_max-y_min)+angle*angle)) > proc2 then
     begin
      ne_naid:=0;
     vert.New_obj(a,a+angle);
   //  for y:=y_min to y_max-1 do
  //    Image1.Canvas.Pixels[Round(a+b*y),min_y+y]:=clgreen;
     end;
    end;
   // if (angle-(sum_ang/count_ang))>(angle_max/100) then break;
   //application.ProcessMessages;
  end;
  vert.Get_Lines;
  vert.Delete_Peres_Line;
  vert.Add_Empty_Lines(false);
 { for y:=0 to vert.Get_Count-1 do
  begin
  //Image1.Canvas.Pen.Color := clRed;
  //Image1.Canvas.MoveTo(vert.Get_a(y),min_y);
 // Image1.Canvas.LineTo(vert.Get_b(y),min_y+imgh-1);
  Image5.Canvas.Pen.Color := clRed;
  Image5.Canvas.MoveTo(vert.Get_a(y),min_y);
  Image5.Canvas.LineTo(vert.Get_b(y),min_y+imgh-1);
  end;    }
  application.ProcessMessages;
  horiz.Destroy;
  horiz:= TListCl.Create;
  ne_naid:=0;
  a_min:=vert.Get_a(0);
  b_min:=(vert.Get_b(0)-a_min)/(imgh-1);
  a_max:=vert.Get_a(vert.Get_Count-1);
  b_max:=(vert.Get_b(vert.Get_Count-1)-a_max)/(imgh-1);
  for a:=min_y to min_y+imgh-2 do
  begin
    proc:=0.6-0.2*(a/(Image1.Height-1));
    ne_naid:=ne_naid+1;
    if 2*(a-min_y)>(imgh-1) then
    begin
       angle_max:=trunc(imgh-2-(a-min_y));
       angle_min:=-trunc((imgh-2)/2);
    end
    else
    begin
       angle_max:=trunc((imgh-2)/2);
       angle_min:=-a+min_y;
    end;
    for angle:=angle_min to angle_max  do
    begin
     b:=angle/(imgw-1);
     sovp:=0;
     x_min:=Round((a_min+(a-min_y)*b_min)/(1-b_min*b));
     x_max:=Round((a_max+(a-min_y)*b_max)/(1-b_max*b));
   //  Image1.Canvas.Pixels[x_min,Round(a+b*x_min)]:=clBlue;
    // Image1.Canvas.Pixels[x_max,Round(a+b*x_max)]:=clBlue;
     for x:=x_min to x_max-1 do
      sovp:=sovp+DynArray2[x,Round(a+b*x)]+DynArray2[x,Round(a+b*x)+1];
     if sovp/2/(sqrt((x_max-x_min)*(x_max-x_min)+angle*angle)) > proc then
     begin
     ne_naid:=0;
     horiz.New_obj(a,a+angle);
 //     for x:=x_min to x_max-1 do
 //     Image1.Canvas.Pixels[x,Round(a+b*x)]:=clgreen;
     end;
    end;
   // application.ProcessMessages;
  end;
  horiz.Get_Lines;
  horiz.Delete_Peres_Line;
  horiz.Add_Empty_Lines(true);
  for y:=0 to horiz.Get_Count-1 do
  begin
  Image1.Canvas.Pen.Color := clRed;
  Image1.Canvas.MoveTo(0,horiz.Get_a(y));
  Image1.Canvas.LineTo(imgw-1,horiz.Get_b(y));
  {Image5.Canvas.Pen.Color := clRed;
  Image5.Canvas.MoveTo(0,horiz.Get_a(y));
  Image5.Canvas.LineTo(imgw-1,horiz.Get_b(y));  }
  end;
  //вертикальные линии 2 волна
  vert.Destroy;
  vert:= TListCl.Create;
  a_min:=horiz.Get_a(0);
  b_min:=(horiz.Get_b(0)-a_min)/(imgw-1);
  a_max:=horiz.Get_a(horiz.Get_Count-1);
  b_max:=(horiz.Get_b(horiz.Get_Count-1)-a_max)/(imgw-1);
  ne_naid:=0;
 //  count_ang:=0;
 //  sum_ang:=0;
  imgh:=max_y-min_y;
  proc2:=0.4*Image1.Height/(max_y-min_y);
  for a:=0 to (imgw-2) do
  begin
    ne_naid:=ne_naid+1;
    if 2*a>(imgw-1) then
    begin
       angle_max:=trunc(imgw-2-a);
       angle_min:=-trunc(imgw/2-1);
    end
    else
    begin
       angle_max:=trunc(imgw/2-2);
       angle_min:=-a-trunc(a*(imgh-1-a_max)/a_max);
    end;
   // proc2:=0.6*(max_y-min_y)/(max_y-min_y-sr)*angle_max/angle;
    for angle:=angle_min to angle_max  do
    begin
     b:=angle/(imgh-1);
     sovp:=0;
     y_min:=Round((a_min+a*b_min)/(1-b_min*b))-min_y;
     y_max:=Round((a_max+a*b_max)/(1-b_max*b))-min_y;
  //   Image1.Canvas.Pixels[Round(a+b*y_min),y_min]:=clBlue;
  //   Image1.Canvas.Pixels[Round(a+b*y_max),y_max]:=clBlue;
     for y:=y_min to y_max-1 do
     begin
      sovp:=sovp+DynArray2[Round(a+b*y),min_y+y]+DynArray2[Round(a+b*y)+1,min_y+y];
    //  Image5.Canvas.Pixels[Round(a+b*y),min_y+y]:=clgreen;
     end;
     if sovp/2/(sqrt((y_max-y_min)*(y_max-y_min)+angle*angle)) > proc2 then
     begin
     ne_naid:=0;
     vert.New_obj(Round(a-min_y*b),Round(a+angle+(Image1.Height-min_y-imgh)*b));
   //  for y:=y_min to y_max-1 do
  //    Image1.Canvas.Pixels[Round(a+b*y),min_y+y]:=clgreen;
     end;
    end;
   // if (angle-(sum_ang/count_ang))>(angle_max/100) then break;
   // application.ProcessMessages;
  end;
  vert.Get_Lines;
  vert.Delete_Peres_Line;
  vert.Add_Empty_Lines(false);
  for y:=0 to vert.Get_Count-1 do
  begin
  Image1.Canvas.Pen.Color := clRed;
  Image1.Canvas.MoveTo(vert.Get_a(y),0);
  Image1.Canvas.LineTo(vert.Get_b(y),Image1.Height-1);
  {Image5.Canvas.Pen.Color := clRed;
  Image5.Canvas.MoveTo(vert.Get_a(y),min_y);
  Image5.Canvas.LineTo(vert.Get_b(y),min_y+imgh-1);    }
  end;
  application.ProcessMessages;
//  st:=horiz.Print;
 // Memo1.Lines.Add(st);
end;

procedure TForm1.ClearImg;
begin
with image1.Canvas do
       begin
       Brush.Color:=clWhite;
       Pen.Color:=clWhite;
       Pen.Width:=1;
       FillRect(Rect(0,0,image1.Width ,image1.Height));
       end;
end;

procedure TForm1.Desc;
var
 x, y,xx,yy,imgh,imgw,minay,minax,maxay,maxax,minby,minbx,maxby,maxbx,count,hc,vc: integer;
 bx,by,sr: Double;
// red:boolean;
begin
    with image4.Canvas do
       begin
       Brush.Color:=clWhite;
       Pen.Color:=clWhite;
       Pen.Width:=1;
       FillRect(Rect(0,0,150,150));
       end;
       with image6.Canvas do
       begin
       Brush.Color:=clWhite;
       Pen.Color:=clWhite;
       Pen.Width:=1;
       FillRect(Rect(0,0,200,200));
       end;
  hc:=horiz.Get_Count;
  vc:=vert.Get_Count;
  SetLength(Desc_Item,vc ,hc );
  imgh := Image1.Height;
  imgw := Image1.Width;
  //red:=false;
  try
  for y:=1 to hc-1 do
  begin
    minay:=horiz.Get_a(y-1);
    maxay:=horiz.Get_a(y);
    minby:=horiz.Get_b(y-1);
    maxby:=horiz.Get_b(y);
    for x:=1 to vc-1 do
      begin
      minax:=vert.Get_a(x-1);
      maxax:=vert.Get_a(x);
      minbx:=vert.Get_b(x-1);
      maxbx:=vert.Get_b(x);
      sr:=0;
      count:=0;
   //   if red then red:=false else red:=true;
      for xx:=minbx to maxbx do
       begin
        bx:=((minax-minbx)+(xx-minbx)/(maxbx-minbx)*(maxax-maxbx-minax+minbx))/(imgh-1);
        for yy:=minay to maxay do
         begin
          by:=((minby-minay)+(yy-minay)/(maxay-minay)*(maxby-maxay-minby+minay))/(imgw-1);
          sr:=sr+DynArray[Round((xx+(imgh-yy)*bx)/(1+bx*by)),Round((yy+by*(xx+bx*imgh))/(1+bx*by))];
     {     if red then
              Image1.Canvas.Pixels[Round((xx+(imgh-yy)*bx)/(1+bx*by)),Round((yy+by*(xx+bx*imgh))/(1+bx*by))] :=clRed
          else
          Image1.Canvas.Pixels[Round((xx+(imgh-yy)*bx)/(1+bx*by)),Round((yy+by*(xx+bx*imgh))/(1+bx*by))] :=clGreen;}
          count:=count+1;
         end;
        end;
       Desc_Item[x-1,y-1]:=round(sr/count*100);
       if Desc_Item[x-1,y-1]>40 then
       begin
         image4.Canvas.Brush.Color:=clBlack ;
         Desc_Item[x-1,y-1]:=1;
       end
       else
       begin
         image4.Canvas.Brush.Color:=clWhite;
         Desc_Item[x-1,y-1]:=0;
       end;
       with image4.Canvas do
       begin
       Pen.Color:=clBlack;
       Pen.Width:=1;
       FillRect(Rect((x-1)*10,(y-1)*10,x*10,y*10));
       end;
     //  image6.Canvas.FillRect(Rect((x-1)*15,(y-1)*15,x*15,y*15));
     //  image6.Canvas.TextOut((x-1)*15,(y-1)*15,IntTOStr(Desc_Item[x-1,y-1]));
       application.ProcessMessages;
      end;
    end;
    except
     //  ShowMessage(IntToStr(xx)+'x'+IntToStr(yy));
    end;
    if (hc>9) and (vc>9) then
    for y:=0 to hc-10 do
      for x:=0 to vc-10 do
       begin
       count:=0;
       for yy:=y to y+9 do
         for xx:=x to x+9 do
            if Desc_Item[xx,yy] = ((yy-y+xx-x+1) mod 2) then
              count:=count+1;
       if count>=95 then
       begin
      //   Showmessage ('10x10: '+IntToStr(x)+'-'+IntToStr(y));
         x_n:=x;
         y_n:=y;
         rec:=10;
         exit;
       end;
       end;
    for y:=0 to hc-8 do
      for x:=0 to vc-8 do
       begin
       count:=0;
       for yy:=y to y+7 do
         for xx:=x to x+7 do
            if Desc_Item[xx,yy] = ((yy-y+xx-x+1) mod 2) then
              count:=count+1;
       if count>=61 then
       begin
       //  Showmessage ('8x8: '+IntToStr(x)+'-'+IntToStr(y));
         x_n:=x;
         y_n:=y;
         rec:=8;
         exit;
       end;
       end;
end;

procedure TForm1.GetItems;
var
 i,j,q,xp,yp, x, y,xx,yy,imgh,imgw,minay,minax,maxay,maxax,minby,minbx,maxby,maxbx,minx,miny,maxx,maxy,count,razn,delta,centrx,centry: integer;
 bx1,bx2,bx3,by1,by2,by3,sr: Double;
 x1,x2,x3,x4,y1,y2,y3,y4,rx,ry,sovp,r: Integer;
 a: double;
 poisk: boolean;
begin
  imgh := Image1.Height;
  imgw := Image1.Width;
  razn:=StrToInt(Edit3.text);
  for y:=y_n+1 to y_n+rec do
  begin
    minay:=horiz.Get_a(y-1);
    maxay:=horiz.Get_a(y);
    minby:=horiz.Get_b(y-1);
    maxby:=horiz.Get_b(y);
    for x:=x_n+1 to x_n+rec do
     if ((y-y_n+x-x_n) mod 2)=0 then
      begin
      setLength(Arr_Colors,0);
      minax:=vert.Get_a(x-1);
      maxax:=vert.Get_a(x);
      minbx:=vert.Get_b(x-1);
      maxbx:=vert.Get_b(x);
      bx1:=(minax-minbx)/(imgh-1);
      by1:=(minby-minay)/(imgw-1);
      bx2:=((minax-minbx)+1/2*(maxax-maxbx-minax+minbx))/(imgh-1);
      by2:=((minby-minay)+1/2*(maxby-maxay-minby+minay))/(imgw-1);
      bx3:=(maxax-maxbx)/(imgh-1);
      by3:=(maxby-maxay)/(imgw-1);
      x1:=Round((minbx+(imgh-minay)*bx1)/(1+bx1*by1));
      y1:=Round((minay+by1*(minbx+bx1*imgh))/(1+bx1*by1));
      x2:=Round((maxbx+(imgh-minay)*bx3)/(1+bx3*by1));
      y2:=Round((minay+by1*(maxbx+bx3*imgh))/(1+bx3*by1));
      x3:=Round((maxbx+(imgh-maxay)*bx3)/(1+bx3*by3));
      y3:=Round((maxay+by3*(maxbx+bx3*imgh))/(1+bx3*by3));
      x4:=Round((minbx+(imgh-maxay)*bx1)/(1+bx1*by3));
      y4:=Round((maxay+by3*(minbx+bx1*imgh))/(1+bx1*by3));
      minx:=min(x1,x4);
      maxx:=max(x2,x3);
      miny:=min(y1,y2);
      maxy:=max(y3,y4);
      xx:=Round(((x2-x1)+(x3-x4))/2/2);
      yy:= Round(((y4-y1)+(y3-y2))/2/2);
      poisk:=true;
      for rx:=xx downto Round(xx/4) do
      begin
       if not poisk then break;
       ry:=Round(rx/xx*yy);
       for i:=minx to maxx do
       begin
        if not poisk then break;
        for j:= miny to maxy do
        begin
         count:=0; sovp:=0;
         For q:= -rx to rx do
         begin
          if DynArray3[i+q,Round(j+sqrt(1-q*q/rx/rx)*ry)]=1 then
            inc(sovp);
          if DynArray3[i+q,Round(j-sqrt(1-q*q/rx/rx)*ry)]=1 then
            inc(sovp);
          inc(count); inc(count);
         end;
         if sovp/count>0.5+0.3*(j/image5.Height) then
         begin
          Image5.Canvas.Pen.Color:=clGreen;
          Image5.Canvas.Pen.Width :=2;
          Image5.Canvas.MoveTo(i-rx,j);
          For q:= -rx to rx do
            Image5.Canvas.LineTo(i+q,Round(j+sqrt(1-q*q/rx/rx)*ry));
          For q:= rx downto -rx do
            Image5.Canvas.LineTo(i+q,Round(j-sqrt(1-q*q/rx/rx)*ry));
          poisk:=false;
          break;
         end;
        end;
       end;
      end;
    // centrx:=Round((minbx+(maxbx-minbx)/2+(imgh-minay-(maxay-minay)/2)*bx2)/(1+bx2*by2));
    //  centry:=Round((minay+(maxay-minay)/2+by2*(minbx+(maxbx-minbx)/2+bx2*imgh))/(1+bx2*by2));

     { Image1.Canvas.MoveTo(centrx-xx,centry);
      Image1.Canvas.Pen.Color:=clGreen;
      Image1.Canvas.Pen.Width :=2; }


     { Image1.Canvas.MoveTo(x1,y1);
      Image1.Canvas.LineTo(x2,y2);
      Image1.Canvas.LineTo(x3,y3);
      Image1.Canvas.LineTo(x4,y4);
      Image1.Canvas.LineTo(x1,y1); }
      application.ProcessMessages;
     { Image1.Canvas.MoveTo(Round(maxbx+(imgh-centry)/(imgh-1)*(maxax-maxbx)),centry);
      Image1.Canvas.LineTo(Round(minbx+(imgh-centry)/(imgh-1)*(minax-minbx)),centry);
      Image1.Canvas.MoveTo(centrx,Round(maxay+centrx/(imgw-1)*(maxby-maxay)));
      Image1.Canvas.LineTo(centrx,Round(minay+centrx/(imgw-1)*(minby-minay)));
      Image1.Canvas.MoveTo(Round(centrx+(maxbx+(imgh-centry)/(imgh-1)*(maxax-maxbx)-centrx)/3),Round(centry+(maxay+centrx/(imgw-1)*(maxby-maxay)-centry)*3/4));
      Image1.Canvas.LineTo(Round(centrx-(maxbx+(imgh-centry)/(imgh-1)*(maxax-maxbx)-centrx)/3),Round(centry-(maxay+centrx/(imgw-1)*(maxby-maxay)-centry)*3/4));
   //   Image1.Canvas.MoveTo(Round(centrx+(maxbx+(imgh-centry)/(imgh-1)*(maxax-maxbx)-centrx)*3/4),Round(centry+(maxay+centrx/(imgw-1)*(maxby-maxay)-centry)/4));
    //  Image1.Canvas.LineTo(Round(centrx-(maxbx+(imgh-centry)/(imgh-1)*(maxax-maxbx)-centrx)*3/4),Round(centry-(maxay+centrx/(imgw-1)*(maxby-maxay)-centry)/4));      }
      end;
    end;
end;

procedure TForm1.ShowColors(count: Integer);
var
i: Integer;
 c1,c2,c3,c4,c5,c6: array[0..1] of Integer;
begin
   with image6.Canvas do
       begin
       Brush.Color:=clWhite;
       Pen.Color:=clWhite;
       Pen.Width:=1;
       FillRect(Rect(0,0,200,200));
       end;
    c1[0]:=0;
    c2[0]:=0;
    c3[0]:=0;
    c4[0]:=0;
    c5[0]:=0;
    c6[0]:=0;
    for i:=0 to Length(Arr_Colors)-1 do
        if Arr_colors[i].count>c1[0] then
        begin
           c6[0]:=c5[0];  c6[1]:=c5[1];
           c5[0]:=c4[0];  c5[1]:=c4[1];
           c4[0]:=c3[0];  c4[1]:=c3[1];
           c3[0]:=c2[0];  c3[1]:=c2[1];
           c2[0]:=c1[0];  c2[1]:=c1[1];
           c1[0]:=Arr_colors[i].count;
           c1[1]:=i;
        end
        else if Arr_colors[i].count>c2[0] then
        begin
           c6[0]:=c5[0];  c6[1]:=c5[1];
           c5[0]:=c4[0];  c5[1]:=c4[1];
           c4[0]:=c3[0];  c4[1]:=c3[1];
           c3[0]:=c2[0];  c3[1]:=c2[1];
           c2[0]:=Arr_colors[i].count;
           c2[1]:=i;
        end
        else if Arr_colors[i].count>c3[0] then
        begin
           c6[0]:=c5[0];  c6[1]:=c5[1];
           c5[0]:=c4[0];  c5[1]:=c4[1];
           c4[0]:=c3[0];  c4[1]:=c3[1];
           c3[0]:=Arr_colors[i].count;
           c3[1]:=i;
        end
        else if Arr_colors[i].count>c4[0] then
        begin
           c6[0]:=c5[0];  c6[1]:=c5[1];
           c5[0]:=c4[0];  c5[1]:=c4[1];
           c4[0]:=Arr_colors[i].count;
           c4[1]:=i;
        end
        else if Arr_colors[i].count>c5[0] then
        begin
           c6[0]:=c5[0];  c6[1]:=c5[1];
           c5[0]:=Arr_colors[i].count;
           c5[1]:=i;
        end
        else if Arr_colors[i].count>c6[0] then
        begin
           c6[0]:=Arr_colors[i].count;
           c6[1]:=i;
        end;
    for i:=0 to Length(Arr_Colors)-1 do
       Arr_Colors[i].count:=Round(Arr_Colors[i].count/count*100);
    with image6.Canvas do
       begin
       Pen.Color:=clBlack;
       Pen.Width:=1;
       Brush.Color:=RGB(Arr_colors[c1[1]].R, Arr_colors[c1[1]].G, Arr_colors[c1[1]].B);
       FillRect(Rect(0,0,20,20));
       Textout(0,0,IntToStr(Arr_colors[c1[1]].count));
       if c2[1]<Length(Arr_colors) then
       begin
       Brush.Color:=RGB(Arr_colors[c2[1]].R, Arr_colors[c2[1]].G, Arr_colors[c2[1]].B);
       FillRect(Rect(0,20,20,40));
       Textout(0,20,IntToStr(Arr_colors[c2[1]].count));
       if c3[1]<Length(Arr_colors) then
       begin
       Brush.Color:=RGB(Arr_colors[c3[1]].R, Arr_colors[c3[1]].G, Arr_colors[c3[1]].B);
       FillRect(Rect(0,40,20,60));
       Textout(0,40,IntToStr(Arr_colors[c3[1]].count));
       if c4[1]<Length(Arr_colors) then
       begin
       Brush.Color:=RGB(Arr_colors[c4[1]].R, Arr_colors[c4[1]].G, Arr_colors[c4[1]].B);
       FillRect(Rect(0,60,20,80));
       Textout(0,60,IntToStr(Arr_colors[c4[1]].count));
       if c5[1]<Length(Arr_colors) then
       begin
       Brush.Color:=RGB(Arr_colors[c5[1]].R, Arr_colors[c5[1]].G, Arr_colors[c5[1]].B);
       FillRect(Rect(0,80,20,100));
       Textout(0,80,IntToStr(Arr_colors[c5[1]].count));
       if c6[1]<Length(Arr_colors) then
       begin
       Brush.Color:=RGB(Arr_colors[c6[1]].R, Arr_colors[c6[1]].G, Arr_colors[c6[1]].B);
       FillRect(Rect(0,100,20,120));
       Textout(0,100,IntToStr(Arr_colors[c6[1]].count));
       end;
       end;
       end;
       end;
       end;
       end;
    ShowMessage(IntToStr(Length(Arr_Colors)));
end;

procedure TForm1.GetItemColorsHSL;
var
 i,j,q,xp,yp, x, y,xx,yy,imgh,imgw,minay,minax,maxay,maxax,minby,minbx,maxby,maxbx,count,razn,min_delta,delta,ind: integer;
 bx,by,sr,H1,S1,L1,H2,S2,L2: Double;
 r,g,b: byte;
 c1,c2,c3,c4,c5,c6: array[0..1] of Integer;
begin
  imgh := Image1.Height;
  imgw := Image1.Width;
  razn:=StrToInt(Edit3.text);
  for y:=y_n+1 to y_n+rec do
  begin
    minay:=horiz.Get_a(y-1);
    maxay:=horiz.Get_a(y);
    minby:=horiz.Get_b(y-1);
    maxby:=horiz.Get_b(y);
    for x:=x_n+1 to x_n+rec do
      if ((y-y_n+x-x_n) mod 2)=0 then
      begin
      count:=0;
      setLength(Arr_Colors,0);
      minax:=vert.Get_a(x-1);
      maxax:=vert.Get_a(x);
      minbx:=vert.Get_b(x-1);
      maxbx:=vert.Get_b(x);
      for xx:=minbx to maxbx do
       begin
        bx:=((minax-minbx)+(xx-minbx)/(maxbx-minbx)*(maxax-maxbx-minax+minbx))/(imgh-1);
        for yy:=minay to maxay do
         begin
          by:=((minby-minay)+(yy-minay)/(maxay-minay)*(maxby-maxay-minby+minay))/(imgw-1);
          xp:=Round((xx+(imgh-yy)*bx)/(1+bx*by));
          yp:=Round((yy+by*(xx+bx*imgh))/(1+bx*by));
          min_delta:=razn;
          ind:=-1;
          if Length(Arr_Colors)=0 then
           begin
             setLength(Arr_Colors,1);
             Arr_Colors[0].R := GetRValue(Image5.Canvas.Pixels[xp,yp]);
             Arr_Colors[0].G := GetGValue(Image5.Canvas.Pixels[xp,yp]);
             Arr_Colors[0].B := GetBValue(Image5.Canvas.Pixels[xp,yp]);
             Arr_Colors[0].count :=1;
             count:=1;
           end
          else
           for i:=0 to Length(Arr_Colors)-1 do
            begin
              RGBToHSL(Arr_Colors[i].R,Arr_Colors[i].G,Arr_Colors[i].B,H1,S1,L1);
              RGBToHSL(GetRValue(Image5.Canvas.Pixels[xp,yp]),GetGValue(Image5.Canvas.Pixels[xp,yp]),GetBValue(Image5.Canvas.Pixels[xp,yp]),H2,S2,L2);
              delta:=Round(Max(Abs(H1-H2)/2,abs(L1-L2)*255));
              if delta <= min_delta then
              begin
                min_delta:=delta;
                ind:=i;
              end;
              if i=Length(Arr_Colors)-1 then
              begin
                if min_delta<razn then
                begin
                Arr_Colors[ind].R := Round((Arr_Colors[ind].R*Arr_Colors[ind].count + GetRValue(Image5.Canvas.Pixels[xp,yp]))/(Arr_Colors[ind].count+1));
                Arr_Colors[ind].G := Round((Arr_Colors[ind].G*Arr_Colors[ind].count + GetGValue(Image5.Canvas.Pixels[xp,yp]))/(Arr_Colors[ind].count+1));
                Arr_Colors[ind].B := Round((Arr_Colors[ind].B*Arr_Colors[ind].count + GetBValue(Image5.Canvas.Pixels[xp,yp]))/(Arr_Colors[ind].count+1));
                Arr_Colors[ind].count :=Arr_Colors[ind].count+1;
                end
                else
                begin
                setLength(Arr_Colors,Length(Arr_Colors)+1);
                Arr_Colors[Length(Arr_Colors)-1].R := GetRValue(Image5.Canvas.Pixels[xp,yp]);
                Arr_Colors[Length(Arr_Colors)-1].G := GetGValue(Image5.Canvas.Pixels[xp,yp]);
                Arr_Colors[Length(Arr_Colors)-1].B := GetBValue(Image5.Canvas.Pixels[xp,yp]);
                Arr_Colors[Length(Arr_Colors)-1].count :=1;
                end;
                count:=count+1;
              end;
            end;
         end;
        end;
        i:=0;
        q:=Length(Arr_Colors)-1;
        while i<q do
        begin
          For j:=i+1 to q do
          begin
              r:=Arr_Colors[i].R-Arr_Colors[j].R;
              g:=Arr_Colors[i].G-Arr_Colors[j].G;
              b:=Arr_Colors[i].B-Arr_Colors[j].B;
              delta:=Round((abs(r-g)+abs(g-b)+abs(b-r))/3);
              if delta<razn then
              begin
                Arr_Colors[i].R := Round((Arr_Colors[i].R*Arr_Colors[i].count + Arr_Colors[j].R*Arr_Colors[j].count)/(Arr_Colors[i].count+Arr_Colors[j].count));
                Arr_Colors[i].G := Round((Arr_Colors[i].G*Arr_Colors[i].count + Arr_Colors[j].G*Arr_Colors[j].count)/(Arr_Colors[i].count+Arr_Colors[j].count));
                Arr_Colors[i].B := Round((Arr_Colors[i].B*Arr_Colors[i].count + Arr_Colors[j].B*Arr_Colors[j].count)/(Arr_Colors[i].count+Arr_Colors[j].count));
                Arr_Colors[i].count :=Arr_Colors[i].count+Arr_Colors[j].count;
                For ind:=j to Length(Arr_Colors)-2 do
                  begin
                     Arr_Colors[ind].R :=Arr_Colors[ind+1].R ;
                     Arr_Colors[ind].G :=Arr_Colors[ind+1].G ;
                     Arr_Colors[ind].B :=Arr_Colors[ind+1].B ;
                     Arr_Colors[ind].count :=Arr_Colors[ind+1].count ;
                  end;
               SetLength(Arr_Colors,Length(Arr_Colors)-1);
               break;
              end
              else if j= Length(Arr_Colors)-1  then
               i:=i+1;
          end;
          q:=Length(Arr_Colors)-1;
        end;
      { with image4.Canvas do
       begin
       Pen.Color:=clBlack;
       Pen.Width:=1;
       FillRect(Rect((x-1)*10,(y-1)*10,x*10,y*10));
       end;
     //  image6.Canvas.FillRect(Rect((x-1)*15,(y-1)*15,x*15,y*15));
     //  image6.Canvas.TextOut((x-1)*15,(y-1)*15,IntTOStr(Desc_Item[x-1,y-1]));   }
       application.ProcessMessages;
       ShowColors(count);
      end;
    end;
end;


procedure TForm1.GetItemColors;
var
 i,j,q,xp,yp,x,y,xx,yy,imgh,imgw,minay,minax,maxay,maxax,minby,minbx,maxby,maxbx,count,razn,del,min_delta,d: integer;
 minY,maxY,minX,maxX,x1,x2,x3,x4,y1,y2,y3,y4,centrX,centrY,rx,ry: Integer;
 bx,by,sr,S: Double;
 r,g,b: byte;
 c1,c2,c3,c4,c5,c6: array[0..1] of Integer;
begin
  imgh := Image1.Height;                                 //1-левый верхний угол
  imgw := Image1.Width;                                  //2-правый верхний угол
  razn:=30;//StrToInt(Edit3.text);                            //3-левый нижний угол
  minay:=horiz.Get_a(y_n);                               //4-правый нижний угол
  maxay:=horiz.Get_a(y_n+rec);
  minby:=horiz.Get_b(y_n);
  maxby:=horiz.Get_b(y_n+rec);
  minax:=vert.Get_a(x_n);
  maxax:=vert.Get_a(x_n+rec);
  minbx:=vert.Get_b(x_n);
  maxbx:=vert.Get_b(x_n+rec);
  by:=(minby-minay)/(imgw-1);
  bx:=(minbx-minax)/(imgh-1);
  x1:=Round((minax+minay*bx)/(1-bx*by));
  y1:=Round((minay+minax*by)/(1-bx*by));
  bx:=(maxbx-maxax)/(imgh-1);
  x2:=Round((maxax+minay*bx)/(1-bx*by));
  y2:=Round((minay+maxax*by)/(1-bx*by));
  minY:=min(y1,y2);
  by:=(maxby-maxay)/(imgw-1);
  bx:=(minbx-minax)/(imgh-1);
  x3:=Round((minax+maxay*bx)/(1-bx*by));
  y3:=Round((maxay+minax*by)/(1-bx*by));
  bx:=(maxbx-maxax)/(imgh-1);
  x4:=Round((maxax+maxay*bx)/(1-bx*by));
  y4:=Round((maxay+maxax*by)/(1-bx*by));
  maxY:=max(y3,y4);
  SetLength(DynArray4,imgw,imgh);
  For x:=0 to imgw-1 do
   for y:=0 to imgh-1 do
     DynArray4[x,y]:=-1;
  count:=0;
  For y:=minY to maxY do
  begin
    if y<y1 then
    begin
      by:=(minby-minay)/(imgw-1);
      bx:=(maxbx-maxax)/(imgh-1);
      minX:=Round((y-minay)/by);
      maxX:=Round(maxax+y*bx);
    end
    else if y<y2 then
    begin
      by:=(minby-minay)/(imgw-1);
      bx:=(minbx-minax)/(imgh-1);
      minX:=Round(minax+y*bx);
      maxX:=Round((y-minay)/by);
    end
    else if y<=y3 then
    begin
      bx:=(minbx-minax)/(imgh-1);
      minX:=Round(minax+y*bx);
      if y<=y4 then
      begin
        bx:=(maxbx-maxax)/(imgh-1);
        maxX:=Round(maxax+y*bx);
      end
      else
      begin
        by:=(maxby-maxay)/(imgw-1);
        maxX:=Round((y-maxay)/by);
      end;
    end
    else
    begin
      by:=(maxby-maxay)/(imgw-1);
      minX:=Round((y-maxay)/by);
      if y<=y4 then
      begin
        bx:=(maxbx-maxax)/(imgh-1);
        maxX:=Round(maxax+y*bx);
      end
      else
      begin
        by:=(maxby-maxay)/(imgw-1);
        maxX:=Round((y-maxay)/by);
      end;
    end;
  For x:=minX to maxX do
  begin
 //   Image5.Canvas.Pixels[x,y]:=clGray;
    if count=0 then
    begin
      setLength(Arr_Colors,1);
      DynArray4[x,y]:=0;
      Arr_Colors[0].R := GetRValue(Image5.Canvas.Pixels[x,y]);
      Arr_Colors[0].G := GetGValue(Image5.Canvas.Pixels[x,y]);
      Arr_Colors[0].B := GetBValue(Image5.Canvas.Pixels[x,y]);
      Arr_Colors[0].count :=1;
      count:=1;
    end
    else
    begin
      if DynArray4[x,y-1]<0 then
      begin
        if x<0 then
         ShowMessage('');
        if DynArray4[x-1,y]<0 then
        begin
          DynArray4[x,y]:=count;
          SetLength(Arr_Colors,Length(Arr_Colors)+1);
          Arr_Colors[count].R := GetRValue(Image5.Canvas.Pixels[x,y]);
          Arr_Colors[count].G := GetGValue(Image5.Canvas.Pixels[x,y]);
          Arr_Colors[count].B := GetBValue(Image5.Canvas.Pixels[x,y]);
          Arr_Colors[count].count:=1;
          inc(count);
        end
        else
        begin
          r:=abs(Arr_Colors[DynArray4[x-1,y]].R - GetRValue(Image5.Canvas.Pixels[x,y]));
          g:=abs(Arr_Colors[DynArray4[x-1,y]].G - GetGValue(Image5.Canvas.Pixels[x,y]));
          b:=abs(Arr_Colors[DynArray4[x-1,y]].B - GetBValue(Image5.Canvas.Pixels[x,y]));
          d:=Round(sqrt(r*r+g*g+b*b));
          if d>razn then
          begin
          DynArray4[x,y]:=count;
          SetLength(Arr_Colors,Length(Arr_Colors)+1);
          Arr_Colors[count].R := GetRValue(Image5.Canvas.Pixels[x,y]);
          Arr_Colors[count].G := GetGValue(Image5.Canvas.Pixels[x,y]);
          Arr_Colors[count].B := GetBValue(Image5.Canvas.Pixels[x,y]);
          Arr_Colors[count].count:=1;
          inc(count);
          end
          else
          begin
            DynArray4[x,y]:=DynArray4[x-1,y];
            Arr_Colors[DynArray4[x-1,y]].R:=Round((Arr_Colors[DynArray4[x-1,y]].R*Arr_Colors[DynArray4[x-1,y]].count+GetRValue(Image5.Canvas.Pixels[x,y]))/(Arr_Colors[DynArray4[x-1,y]].count+1));
            Arr_Colors[DynArray4[x-1,y]].G:=Round((Arr_Colors[DynArray4[x-1,y]].G*Arr_Colors[DynArray4[x-1,y]].count+GetGValue(Image5.Canvas.Pixels[x,y]))/(Arr_Colors[DynArray4[x-1,y]].count+1));
            Arr_Colors[DynArray4[x-1,y]].B:=Round((Arr_Colors[DynArray4[x-1,y]].B*Arr_Colors[DynArray4[x-1,y]].count+GetBValue(Image5.Canvas.Pixels[x,y]))/(Arr_Colors[DynArray4[x-1,y]].count+1));
            inc(Arr_Colors[DynArray4[x-1,y]].count);
          end;
        end;
      end
      else if DynArray4[x-1,y]<0 then
      begin
          r:=abs(Arr_Colors[DynArray4[x,y-1]].R - GetRValue(Image5.Canvas.Pixels[x,y]));
          g:=abs(Arr_Colors[DynArray4[x,y-1]].G - GetGValue(Image5.Canvas.Pixels[x,y]));
          b:=abs(Arr_Colors[DynArray4[x,y-1]].B - GetBValue(Image5.Canvas.Pixels[x,y]));
          d:=Round(sqrt(r*r+g*g+b*b));
          if d>razn then
          begin
          DynArray4[x,y]:=count;
          SetLength(Arr_Colors,Length(Arr_Colors)+1);
          Arr_Colors[count].R := GetRValue(Image5.Canvas.Pixels[x,y]);
          Arr_Colors[count].G := GetGValue(Image5.Canvas.Pixels[x,y]);
          Arr_Colors[count].B := GetBValue(Image5.Canvas.Pixels[x,y]);
          Arr_Colors[count].count:=1;
          inc(count);
          end
          else
          begin
            DynArray4[x,y]:=DynArray4[x,y-1];
            Arr_Colors[DynArray4[x,y]].R:=Round((Arr_Colors[DynArray4[x,y]].R*Arr_Colors[DynArray4[x,y]].count+GetRValue(Image5.Canvas.Pixels[x,y]))/(Arr_Colors[DynArray4[x,y]].count+1));
            Arr_Colors[DynArray4[x,y]].G:=Round((Arr_Colors[DynArray4[x,y]].G*Arr_Colors[DynArray4[x,y]].count+GetGValue(Image5.Canvas.Pixels[x,y]))/(Arr_Colors[DynArray4[x,y]].count+1));
            Arr_Colors[DynArray4[x,y]].B:=Round((Arr_Colors[DynArray4[x,y]].B*Arr_Colors[DynArray4[x,y]].count+GetBValue(Image5.Canvas.Pixels[x,y]))/(Arr_Colors[DynArray4[x,y]].count+1));
            inc(Arr_Colors[DynArray4[x,y]].count);
          end;
      end
      else
      begin
         r:=abs(Arr_Colors[DynArray4[x-1,y]].R - GetRValue(Image5.Canvas.Pixels[x,y]));
         g:=abs(Arr_Colors[DynArray4[x-1,y]].G - GetGValue(Image5.Canvas.Pixels[x,y]));
         b:=abs(Arr_Colors[DynArray4[x-1,y]].B - GetBValue(Image5.Canvas.Pixels[x,y]));
         min_delta:=Round(sqrt(r*r+g*g+b*b));
         r:=abs(Arr_Colors[DynArray4[x,y-1]].R - GetRValue(Image5.Canvas.Pixels[x,y]));
         g:=abs(Arr_Colors[DynArray4[x,y-1]].G - GetGValue(Image5.Canvas.Pixels[x,y]));
         b:=abs(Arr_Colors[DynArray4[x,y-1]].B - GetBValue(Image5.Canvas.Pixels[x,y]));
         d:=Round(sqrt(r*r+g*g+b*b));
          if min_delta>razn then
          begin
            if d>razn then
            begin
              DynArray4[x,y]:=count;
              SetLength(Arr_Colors,Length(Arr_Colors)+1);
              Arr_Colors[count].R := GetRValue(Image5.Canvas.Pixels[x,y]);
              Arr_Colors[count].G := GetGValue(Image5.Canvas.Pixels[x,y]);
              Arr_Colors[count].B := GetBValue(Image5.Canvas.Pixels[x,y]);
              Arr_Colors[count].count:=1;
              inc(count);
            end
            else
            begin
            DynArray4[x,y]:=DynArray4[x,y-1];
            Arr_Colors[DynArray4[x,y]].R:=Round((Arr_Colors[DynArray4[x,y]].R*Arr_Colors[DynArray4[x,y]].count+GetRValue(Image5.Canvas.Pixels[x,y]))/(Arr_Colors[DynArray4[x,y]].count+1));
            Arr_Colors[DynArray4[x,y]].G:=Round((Arr_Colors[DynArray4[x,y]].G*Arr_Colors[DynArray4[x,y]].count+GetGValue(Image5.Canvas.Pixels[x,y]))/(Arr_Colors[DynArray4[x,y]].count+1));
            Arr_Colors[DynArray4[x,y]].B:=Round((Arr_Colors[DynArray4[x,y]].B*Arr_Colors[DynArray4[x,y]].count+GetBValue(Image5.Canvas.Pixels[x,y]))/(Arr_Colors[DynArray4[x,y]].count+1));
            inc(Arr_Colors[DynArray4[x,y]].count);
            end;
          end
          else if d>razn then
          begin
            DynArray4[x,y]:=DynArray4[x-1,y];
            if Length(Arr_Colors)-1<DynArray4[x,y] then
              showmessage ('');

            Arr_Colors[DynArray4[x-1,y]].R:=Round((Arr_Colors[DynArray4[x-1,y]].R*Arr_Colors[DynArray4[x-1,y]].count+GetRValue(Image5.Canvas.Pixels[x,y]))/(Arr_Colors[DynArray4[x-1,y]].count+1));
            Arr_Colors[DynArray4[x-1,y]].G:=Round((Arr_Colors[DynArray4[x-1,y]].G*Arr_Colors[DynArray4[x-1,y]].count+GetGValue(Image5.Canvas.Pixels[x,y]))/(Arr_Colors[DynArray4[x-1,y]].count+1));
            Arr_Colors[DynArray4[x-1,y]].B:=Round((Arr_Colors[DynArray4[x-1,y]].B*Arr_Colors[DynArray4[x-1,y]].count+GetBValue(Image5.Canvas.Pixels[x,y]))/(Arr_Colors[DynArray4[x-1,y]].count+1));
            inc(Arr_Colors[DynArray4[x-1,y]].count);
          end
          else
          begin
           r:=abs(Arr_Colors[DynArray4[x,y-1]].R - Arr_Colors[DynArray4[x-1,y]].R);
           g:=abs(Arr_Colors[DynArray4[x,y-1]].G - Arr_Colors[DynArray4[x-1,y]].G);
           b:=abs(Arr_Colors[DynArray4[x,y-1]].B - Arr_Colors[DynArray4[x-1,y]].B);
           if sqrt(r*r+g*g+b*b)>razn then
           begin
             if d>min_delta then
             begin
               DynArray4[x,y]:=DynArray4[x-1,y];
               Arr_Colors[DynArray4[x-1,y]].R:=Round((Arr_Colors[DynArray4[x-1,y]].R*Arr_Colors[DynArray4[x-1,y]].count+GetRValue(Image5.Canvas.Pixels[x,y]))/(Arr_Colors[DynArray4[x-1,y]].count+1));
               Arr_Colors[DynArray4[x-1,y]].G:=Round((Arr_Colors[DynArray4[x-1,y]].G*Arr_Colors[DynArray4[x-1,y]].count+GetGValue(Image5.Canvas.Pixels[x,y]))/(Arr_Colors[DynArray4[x-1,y]].count+1));
               Arr_Colors[DynArray4[x-1,y]].B:=Round((Arr_Colors[DynArray4[x-1,y]].B*Arr_Colors[DynArray4[x-1,y]].count+GetBValue(Image5.Canvas.Pixels[x,y]))/(Arr_Colors[DynArray4[x-1,y]].count+1));
               inc(Arr_Colors[DynArray4[x-1,y]].count);
             end
             else
             begin
              DynArray4[x,y]:=DynArray4[x,y-1];
              Arr_Colors[DynArray4[x,y]].R:=Round((Arr_Colors[DynArray4[x,y]].R*Arr_Colors[DynArray4[x,y]].count+GetRValue(Image5.Canvas.Pixels[x,y]))/(Arr_Colors[DynArray4[x,y]].count+1));
              Arr_Colors[DynArray4[x,y]].G:=Round((Arr_Colors[DynArray4[x,y]].G*Arr_Colors[DynArray4[x,y]].count+GetGValue(Image5.Canvas.Pixels[x,y]))/(Arr_Colors[DynArray4[x,y]].count+1));
              Arr_Colors[DynArray4[x,y]].B:=Round((Arr_Colors[DynArray4[x,y]].B*Arr_Colors[DynArray4[x,y]].count+GetBValue(Image5.Canvas.Pixels[x,y]))/(Arr_Colors[DynArray4[x,y]].count+1));
              inc(Arr_Colors[DynArray4[x,y]].count);
             end;
           end
           else if DynArray4[x,y-1]=DynArray4[x-1,y] then
           begin
              DynArray4[x,y]:=DynArray4[x,y-1];
              Arr_Colors[DynArray4[x,y]].R:=Round((Arr_Colors[DynArray4[x,y]].R*Arr_Colors[DynArray4[x,y]].count+GetRValue(Image5.Canvas.Pixels[x,y]))/(Arr_Colors[DynArray4[x,y]].count+1));
              Arr_Colors[DynArray4[x,y]].G:=Round((Arr_Colors[DynArray4[x,y]].G*Arr_Colors[DynArray4[x,y]].count+GetGValue(Image5.Canvas.Pixels[x,y]))/(Arr_Colors[DynArray4[x,y]].count+1));
              Arr_Colors[DynArray4[x,y]].B:=Round((Arr_Colors[DynArray4[x,y]].B*Arr_Colors[DynArray4[x,y]].count+GetBValue(Image5.Canvas.Pixels[x,y]))/(Arr_Colors[DynArray4[x,y]].count+1));
              inc(Arr_Colors[DynArray4[x,y]].count);
           end
           else
           begin
           del:=DynArray4[x,y-1];
             for j:=0 to y do
              for i:=0 to imgw-1 do
               if DynArray4[i,j]=del then DynArray4[i,j]:=DynArray4[x-1,y];
           DynArray4[x,y]:=DynArray4[x-1,y];
           Arr_Colors[DynArray4[x,y]].R:=Round((Arr_Colors[DynArray4[x,y]].R*Arr_Colors[DynArray4[x,y]].count+Arr_Colors[del].R*Arr_Colors[del].count+GetRValue(Image5.Canvas.Pixels[x,y]))/(Arr_Colors[DynArray4[x,y]].count+Arr_Colors[del].count+1));
           Arr_Colors[DynArray4[x,y]].G:=Round((Arr_Colors[DynArray4[x,y]].G*Arr_Colors[DynArray4[x,y]].count+Arr_Colors[del].G*Arr_Colors[del].count+GetGValue(Image5.Canvas.Pixels[x,y]))/(Arr_Colors[DynArray4[x,y]].count+Arr_Colors[del].count+1));
           Arr_Colors[DynArray4[x,y]].B:=Round((Arr_Colors[DynArray4[x,y]].B*Arr_Colors[DynArray4[x,y]].count+Arr_Colors[del].B*Arr_Colors[del].count+GetBValue(Image5.Canvas.Pixels[x,y]))/(Arr_Colors[DynArray4[x,y]].count+Arr_Colors[del].count+1));
           Arr_Colors[DynArray4[x,y]].count:=Arr_Colors[DynArray4[x,y]].count+Arr_Colors[del].count+1;
           Arr_Colors[del].count:=0;
           end;
          end;
      end;
    end;
  end; // for x
  end; //for y
  //визуализация результата
  //  count:=0;
  {  For x:=0 to imgw-1 do
      for y:=0 to imgh-1 do
        if Arr_Colors[DynArray4[x,y]].count > 20 then
          Image1.Canvas.Pixels[x,y]:= RGB(Arr_Colors[DynArray4[x,y]].R,Arr_Colors[DynArray4[x,y]].G,Arr_Colors[DynArray4[x,y]].B)
        else
          Image1.Canvas.Pixels[x,y]:= clgreen;
  // for i:=0 to Length(Arr_Colors)-1 do
  //      if Arr_Colors[i].count > 20 then
  //       inc(count);
  //  ShowMessage(IntToStr(count));  }
  SetLength(Desc_Info,rec,rec);
  for yy:=y_n+1 to y_n+rec do
  begin
    minay:=horiz.Get_a(yy-1);
    maxay:=horiz.Get_a(yy);
    minby:=horiz.Get_b(yy-1);
    maxby:=horiz.Get_b(yy);
    for xx:=x_n+1 to x_n+rec do
      begin
      minax:=vert.Get_a(xx-1);
      maxax:=vert.Get_a(xx);
      minbx:=vert.Get_b(xx-1);
      maxbx:=vert.Get_b(xx);
      by:=(minby-minay)/(imgw-1);
      bx:=(minbx-minax)/(imgh-1);
      x1:=Round((minax+minay*bx)/(1-bx*by))+1;
      y1:=Round((minay+minax*by)/(1-bx*by))+1;
      bx:=(maxbx-maxax)/(imgh-1);
      x2:=Round((maxax+minay*bx)/(1-bx*by))-1;
      y2:=Round((minay+maxax*by)/(1-bx*by))+1;
      minY:=min(y1,y2);
      by:=(maxby-maxay)/(imgw-1);
      bx:=(minbx-minax)/(imgh-1);
      x3:=Round((minax+maxay*bx)/(1-bx*by))+1;
      y3:=Round((maxay+minax*by)/(1-bx*by))-1;
      bx:=(maxbx-maxax)/(imgh-1);
      x4:=Round((maxax+maxay*bx)/(1-bx*by))-1;
      y4:=Round((maxay+maxax*by)/(1-bx*by))-1;
      maxY:=max(y3,y4);
      //Обнуление в памяти данных о клетке доски
      Desc_Info[xx-x_n-1,yy-y_n-1].count_obj:=0;
      Desc_Info[xx-x_n-1,yy-y_n-1].Pixels_rect:=0;
      SetLength(Desc_Info[xx-x_n-1,yy-y_n-1].id_obj,0);
      SetLength(Desc_Info[xx-x_n-1,yy-y_n-1].Pixels_obj,0);
      For y:=minY to maxY do
      begin
        if y<y1 then
        begin
         by:=(minby-minay)/(imgw-1);
         bx:=(maxbx-maxax)/(imgh-1);
         minX:=Round((y-minay)/by);
         maxX:=Round(maxax+y*bx);
        end
        else if y<y2 then
        begin
         by:=(minby-minay)/(imgw-1);
         bx:=(minbx-minax)/(imgh-1);
         minX:=Round(minax+y*bx);
         maxX:=Round((y-minay)/by);
        end
        else if y<=y3 then
        begin
         bx:=(minbx-minax)/(imgh-1);
         minX:=Round(minax+y*bx);
         if y<=y4 then
         begin
           bx:=(maxbx-maxax)/(imgh-1);
           maxX:=Round(maxax+y*bx);
         end
         else
         begin
          by:=(maxby-maxay)/(imgw-1);
          maxX:=Round((y-maxay)/by);
         end;
        end
        else
        begin
         by:=(maxby-maxay)/(imgw-1);
         minX:=Round((y-maxay)/by);
         if y<=y4 then
         begin
           bx:=(maxbx-maxax)/(imgh-1);
           maxX:=Round(maxax+y*bx);
         end
         else
         begin
           by:=(maxby-maxay)/(imgw-1);
           maxX:=Round((y-maxay)/by);
         end;
        end;
        For x:=minX to maxX do
        begin
          inc(Desc_Info[xx-x_n-1,yy-y_n-1].Pixels_rect);
          if Desc_Info[xx-x_n-1,yy-y_n-1].count_obj=0 then
          begin
            inc(Desc_Info[xx-x_n-1,yy-y_n-1].count_obj);
            SetLength(Desc_Info[xx-x_n-1,yy-y_n-1].id_obj,Desc_Info[xx-x_n-1,yy-y_n-1].count_obj);
            SetLength(Desc_Info[xx-x_n-1,yy-y_n-1].Pixels_obj,Desc_Info[xx-x_n-1,yy-y_n-1].count_obj);
            SetLength(Desc_Info[xx-x_n-1,yy-y_n-1].Summ,Desc_Info[xx-x_n-1,yy-y_n-1].count_obj);
            SetLength(Desc_Info[xx-x_n-1,yy-y_n-1].MinX,Desc_Info[xx-x_n-1,yy-y_n-1].count_obj);
            SetLength(Desc_Info[xx-x_n-1,yy-y_n-1].MaxX,Desc_Info[xx-x_n-1,yy-y_n-1].count_obj);
            SetLength(Desc_Info[xx-x_n-1,yy-y_n-1].MinY,Desc_Info[xx-x_n-1,yy-y_n-1].count_obj);
            SetLength(Desc_Info[xx-x_n-1,yy-y_n-1].MaxY,Desc_Info[xx-x_n-1,yy-y_n-1].count_obj);
            Desc_Info[xx-x_n-1,yy-y_n-1].id_obj[Desc_Info[xx-x_n-1,yy-y_n-1].count_obj-1]:=DynArray4[x,y];
            Desc_Info[xx-x_n-1,yy-y_n-1].Pixels_obj[Desc_Info[xx-x_n-1,yy-y_n-1].count_obj-1]:=1;
            Desc_Info[xx-x_n-1,yy-y_n-1].Summ[Desc_Info[xx-x_n-1,yy-y_n-1].count_obj-1].x:=x;
            Desc_Info[xx-x_n-1,yy-y_n-1].Summ[Desc_Info[xx-x_n-1,yy-y_n-1].count_obj-1].y:=y;
            Desc_Info[xx-x_n-1,yy-y_n-1].MinX[Desc_Info[xx-x_n-1,yy-y_n-1].count_obj-1].x:=x;
            Desc_Info[xx-x_n-1,yy-y_n-1].MinX[Desc_Info[xx-x_n-1,yy-y_n-1].count_obj-1].y:=y;
            Desc_Info[xx-x_n-1,yy-y_n-1].MaxX[Desc_Info[xx-x_n-1,yy-y_n-1].count_obj-1].x:=x;
            Desc_Info[xx-x_n-1,yy-y_n-1].MaxX[Desc_Info[xx-x_n-1,yy-y_n-1].count_obj-1].y:=y;
            Desc_Info[xx-x_n-1,yy-y_n-1].MinY[Desc_Info[xx-x_n-1,yy-y_n-1].count_obj-1].x:=x;
            Desc_Info[xx-x_n-1,yy-y_n-1].MinY[Desc_Info[xx-x_n-1,yy-y_n-1].count_obj-1].y:=y;
            Desc_Info[xx-x_n-1,yy-y_n-1].MaxY[Desc_Info[xx-x_n-1,yy-y_n-1].count_obj-1].x:=x;
            Desc_Info[xx-x_n-1,yy-y_n-1].MaxY[Desc_Info[xx-x_n-1,yy-y_n-1].count_obj-1].y:=y;
          end
          else
          For i:=0 to Desc_Info[xx-x_n-1,yy-y_n-1].count_obj-1 do
          begin
            if DynArray4[x,y]= Desc_Info[xx-x_n-1,yy-y_n-1].id_obj[i] then
            begin
            inc(Desc_Info[xx-x_n-1,yy-y_n-1].Pixels_obj[i]);
            Desc_Info[xx-x_n-1,yy-y_n-1].Summ[i].x:=Desc_Info[xx-x_n-1,yy-y_n-1].Summ[i].x+x;
            Desc_Info[xx-x_n-1,yy-y_n-1].Summ[i].y:=Desc_Info[xx-x_n-1,yy-y_n-1].Summ[i].y+y;
            if x<Desc_Info[xx-x_n-1,yy-y_n-1].MinX[i].x then
             begin
             Desc_Info[xx-x_n-1,yy-y_n-1].MinX[i].x:=x;
             Desc_Info[xx-x_n-1,yy-y_n-1].MinX[i].y:=y;
             end;
            if x>Desc_Info[xx-x_n-1,yy-y_n-1].MaxX[i].x then
             begin
             Desc_Info[xx-x_n-1,yy-y_n-1].MaxX[i].x:=x;
             Desc_Info[xx-x_n-1,yy-y_n-1].MaxX[i].y:=y;
             end;
            if y<Desc_Info[xx-x_n-1,yy-y_n-1].MinY[i].y then
             begin
             Desc_Info[xx-x_n-1,yy-y_n-1].MinY[i].x:=x;
             Desc_Info[xx-x_n-1,yy-y_n-1].MinY[i].y:=y;
             end;
            if y>Desc_Info[xx-x_n-1,yy-y_n-1].MaxY[i].y then
             begin
             Desc_Info[xx-x_n-1,yy-y_n-1].MaxY[i].x:=x;
             Desc_Info[xx-x_n-1,yy-y_n-1].MaxY[i].y:=y;
             end;
            break;
            end
            else if i=Desc_Info[xx-x_n-1,yy-y_n-1].count_obj-1 then
            begin
            inc(Desc_Info[xx-x_n-1,yy-y_n-1].count_obj);
            SetLength(Desc_Info[xx-x_n-1,yy-y_n-1].id_obj,Desc_Info[xx-x_n-1,yy-y_n-1].count_obj);
            SetLength(Desc_Info[xx-x_n-1,yy-y_n-1].Pixels_obj,Desc_Info[xx-x_n-1,yy-y_n-1].count_obj);
            SetLength(Desc_Info[xx-x_n-1,yy-y_n-1].Summ,Desc_Info[xx-x_n-1,yy-y_n-1].count_obj);
            SetLength(Desc_Info[xx-x_n-1,yy-y_n-1].MinX,Desc_Info[xx-x_n-1,yy-y_n-1].count_obj);
            SetLength(Desc_Info[xx-x_n-1,yy-y_n-1].MaxX,Desc_Info[xx-x_n-1,yy-y_n-1].count_obj);
            SetLength(Desc_Info[xx-x_n-1,yy-y_n-1].MinY,Desc_Info[xx-x_n-1,yy-y_n-1].count_obj);
            SetLength(Desc_Info[xx-x_n-1,yy-y_n-1].MaxY,Desc_Info[xx-x_n-1,yy-y_n-1].count_obj);
            Desc_Info[xx-x_n-1,yy-y_n-1].id_obj[Desc_Info[xx-x_n-1,yy-y_n-1].count_obj-1]:=DynArray4[x,y];
            Desc_Info[xx-x_n-1,yy-y_n-1].Pixels_obj[Desc_Info[xx-x_n-1,yy-y_n-1].count_obj-1]:=1;
            Desc_Info[xx-x_n-1,yy-y_n-1].Summ[Desc_Info[xx-x_n-1,yy-y_n-1].count_obj-1].x:=x;
            Desc_Info[xx-x_n-1,yy-y_n-1].Summ[Desc_Info[xx-x_n-1,yy-y_n-1].count_obj-1].y:=y;
            Desc_Info[xx-x_n-1,yy-y_n-1].MinX[Desc_Info[xx-x_n-1,yy-y_n-1].count_obj-1].x:=x;
            Desc_Info[xx-x_n-1,yy-y_n-1].MinX[Desc_Info[xx-x_n-1,yy-y_n-1].count_obj-1].y:=y;
            Desc_Info[xx-x_n-1,yy-y_n-1].MaxX[Desc_Info[xx-x_n-1,yy-y_n-1].count_obj-1].x:=x;
            Desc_Info[xx-x_n-1,yy-y_n-1].MaxX[Desc_Info[xx-x_n-1,yy-y_n-1].count_obj-1].y:=y;
            Desc_Info[xx-x_n-1,yy-y_n-1].MinY[Desc_Info[xx-x_n-1,yy-y_n-1].count_obj-1].x:=x;
            Desc_Info[xx-x_n-1,yy-y_n-1].MinY[Desc_Info[xx-x_n-1,yy-y_n-1].count_obj-1].y:=y;
            Desc_Info[xx-x_n-1,yy-y_n-1].MaxY[Desc_Info[xx-x_n-1,yy-y_n-1].count_obj-1].x:=x;
            Desc_Info[xx-x_n-1,yy-y_n-1].MaxY[Desc_Info[xx-x_n-1,yy-y_n-1].count_obj-1].y:=y;
            break;
            end;
          end;
        end; //for x
       end; //for y
      // if xx-x_n-1=0 then
     //    begin
         count:=0;
          with image6.Canvas do
          begin
          Brush.Color:=clWhite;
          Pen.Color:=clWhite;
          Pen.Width:=1;
          FillRect(Rect(0,0,200,200));
          end;
          For i:=0 to Desc_Info[xx-x_n-1,yy-y_n-1].count_obj-1 do
           if (Desc_Info[xx-x_n-1,yy-y_n-1].Pixels_obj[i]/Desc_Info[xx-x_n-1,yy-y_n-1].Pixels_rect >0.2)
           and (Desc_Info[xx-x_n-1,yy-y_n-1].Pixels_obj[i]/Desc_Info[xx-x_n-1,yy-y_n-1].Pixels_rect <0.8)then
           begin
            image6.Canvas.Brush.Color:=RGB(Arr_Colors[Desc_Info[xx-x_n-1,yy-y_n-1].id_obj[i]].R,
            Arr_Colors[Desc_Info[xx-x_n-1,yy-y_n-1].id_obj[i]].G,Arr_Colors[Desc_Info[xx-x_n-1,yy-y_n-1].id_obj[i]].B);
            image6.Canvas.FillRect(Rect(0,0+20*count,20,20+20*count));
            image6.Canvas.Textout(0,0+20*count,IntToStr(Round(Desc_Info[xx-x_n-1,yy-y_n-1].Pixels_obj[i]/Desc_Info[xx-x_n-1,yy-y_n-1].Pixels_rect*100)));
            inc(count);
            ShowObject(Desc_Info[xx-x_n-1,yy-y_n-1].id_obj[i]);
            S:= Pi*(Desc_Info[xx-x_n-1,yy-y_n-1].maxx[i].x-Desc_Info[xx-x_n-1,yy-y_n-1].minx[i].x)*(Desc_Info[xx-x_n-1,yy-y_n-1].maxY[i].Y-Desc_Info[xx-x_n-1,yy-y_n-1].minY[i].Y)/4;
            if (S<1.3*Desc_Info[xx-x_n-1,yy-y_n-1].Pixels_obj[i]) and (S>0.7*Desc_Info[xx-x_n-1,yy-y_n-1].Pixels_obj[i])
            and (abs(Desc_Info[xx-x_n-1,yy-y_n-1].maxX[i].Y-Desc_Info[xx-x_n-1,yy-y_n-1].minX[i].Y)<0.3*(Desc_Info[xx-x_n-1,yy-y_n-1].maxY[i].Y-Desc_Info[xx-x_n-1,yy-y_n-1].minY[i].Y))
            and (abs(Desc_Info[xx-x_n-1,yy-y_n-1].maxY[i].X-Desc_Info[xx-x_n-1,yy-y_n-1].minY[i].X)<0.3*(Desc_Info[xx-x_n-1,yy-y_n-1].maxX[i].X-Desc_Info[xx-x_n-1,yy-y_n-1].minX[i].X)) then
              begin
              centrX:=Round(Desc_Info[xx-x_n-1,yy-y_n-1].Summ[i].x/Desc_Info[xx-x_n-1,yy-y_n-1].Pixels_obj[i]);
              centrY:=Round(Desc_Info[xx-x_n-1,yy-y_n-1].Summ[i].Y/Desc_Info[xx-x_n-1,yy-y_n-1].Pixels_obj[i]);
              rx:=Round((Desc_Info[xx-x_n-1,yy-y_n-1].maxx[i].x-Desc_Info[xx-x_n-1,yy-y_n-1].minx[i].x)/2);
              ry:=Round((Desc_Info[xx-x_n-1,yy-y_n-1].maxY[i].Y-Desc_Info[xx-x_n-1,yy-y_n-1].minY[i].Y)/2);
            //  Image1.Canvas.Pen.Color:=clGreen;
           //   Image1.Canvas.Pen.Width :=2;
            //  Image1.Canvas.MoveTo(centrX-rx,centrY);
            //  For q:= -rx to rx do
            //   Image1.Canvas.LineTo(centrX+q,Round(centrY+sqrt(1-q*q/rx/rx)*ry));
            //  For q:= rx downto -rx do
            //   Image1.Canvas.LineTo(centrX+q,Round(centrY-sqrt(1-q*q/rx/rx)*ry));
              end;
           end;
          Application.ProcessMessages;
        //  ShowMessage(IntToStr(count));
      //   end;
     end; //for xx
  end; //for yy
end;

procedure TForm1.ShowObject(index: Integer);
var
 x,y: Integer;
 col: TColor;
begin
  col:=RGB(Random(255), Random(255), Random(255));
  For x:=0 to Image1.Width-1 do
  For y:=0 to Image1.Height-1 do
  if DynArray4[x,y]=index then
    Image1.Canvas.Pixels[x,y]:=col;
end;

 {
procedure TForm1.GetItemColors;
var
 i,j,q,xp,yp, x, y,xx,yy,imgh,imgw,minay,minax,maxay,maxax,minby,minbx,maxby,maxbx,count,razn,min_delta,delta,ind: integer;
 bx,by,sr: Double;
 r,g,b: byte;
 c1,c2,c3,c4,c5,c6: array[0..1] of Integer;
begin
  imgh := Image1.Height;
  imgw := Image1.Width;
  razn:=StrToInt(Edit3.text);
  for y:=y_n+1 to y_n+rec do
  begin
    minay:=horiz.Get_a(y-1);
    maxay:=horiz.Get_a(y);
    minby:=horiz.Get_b(y-1);
    maxby:=horiz.Get_b(y);
    for x:=x_n+1 to x_n+rec do
      if ((y-y_n+x-x_n) mod 2)=0 then
      begin
      count:=0;
      setLength(Arr_Colors,0);
      minax:=vert.Get_a(x-1);
      maxax:=vert.Get_a(x);
      minbx:=vert.Get_b(x-1);
      maxbx:=vert.Get_b(x);
      for xx:=minbx to maxbx do
       begin
        bx:=((minax-minbx)+(xx-minbx)/(maxbx-minbx)*(maxax-maxbx-minax+minbx))/(imgh-1);
        for yy:=minay to maxay do
         begin
          by:=((minby-minay)+(yy-minay)/(maxay-minay)*(maxby-maxay-minby+minay))/(imgw-1);
          xp:=Round((xx+(imgh-yy)*bx)/(1+bx*by));
          yp:=Round((yy+by*(xx+bx*imgh))/(1+bx*by));
          min_delta:=razn;
          ind:=-1;
          if Length(Arr_Colors)=0 then
           begin
             setLength(Arr_Colors,1);
             Arr_Colors[0].R := GetRValue(Image5.Canvas.Pixels[xp,yp]);
             Arr_Colors[0].G := GetGValue(Image5.Canvas.Pixels[xp,yp]);
             Arr_Colors[0].B := GetBValue(Image5.Canvas.Pixels[xp,yp]);
             Arr_Colors[0].count :=1;
             count:=1;
           end
          else
           for i:=0 to Length(Arr_Colors)-1 do
            begin
              r:=Arr_Colors[i].R-GetRValue(Image5.Canvas.Pixels[xp,yp]);
              g:=Arr_Colors[i].G-GetGValue(Image5.Canvas.Pixels[xp,yp]);
              b:=Arr_Colors[i].B-GetBValue(Image5.Canvas.Pixels[xp,yp]);
              delta:=Round(abs(r-g)+abs(g-b)+abs(b-r));
              if delta <= min_delta then
              begin
                min_delta:=delta;
                ind:=i;
              end;
              if i=Length(Arr_Colors)-1 then
              begin
                if min_delta<razn then
                begin
                Arr_Colors[ind].R := Round((Arr_Colors[ind].R*Arr_Colors[ind].count + GetRValue(Image5.Canvas.Pixels[xp,yp]))/(Arr_Colors[ind].count+1));
                Arr_Colors[ind].G := Round((Arr_Colors[ind].G*Arr_Colors[ind].count + GetGValue(Image5.Canvas.Pixels[xp,yp]))/(Arr_Colors[ind].count+1));
                Arr_Colors[ind].B := Round((Arr_Colors[ind].B*Arr_Colors[ind].count + GetBValue(Image5.Canvas.Pixels[xp,yp]))/(Arr_Colors[ind].count+1));
                Arr_Colors[ind].count :=Arr_Colors[ind].count+1;
                end
                else
                begin
                setLength(Arr_Colors,Length(Arr_Colors)+1);
                Arr_Colors[Length(Arr_Colors)-1].R := GetRValue(Image5.Canvas.Pixels[xp,yp]);
                Arr_Colors[Length(Arr_Colors)-1].G := GetGValue(Image5.Canvas.Pixels[xp,yp]);
                Arr_Colors[Length(Arr_Colors)-1].B := GetBValue(Image5.Canvas.Pixels[xp,yp]);
                Arr_Colors[Length(Arr_Colors)-1].count :=1;
                end;
                count:=count+1;
              end;
            end;
         end;
        end;
        i:=0;
        q:=Length(Arr_Colors)-1;
        while i<q do
        begin
          For j:=i+1 to q do
          begin
              r:=Arr_Colors[i].R-Arr_Colors[j].R;
              g:=Arr_Colors[i].G-Arr_Colors[j].G;
              b:=Arr_Colors[i].B-Arr_Colors[j].B;
              delta:=Round((abs(r-g)+abs(g-b)+abs(b-r))/3);
              if delta<razn then
              begin
                Arr_Colors[i].R := Round((Arr_Colors[i].R*Arr_Colors[i].count + Arr_Colors[j].R*Arr_Colors[j].count)/(Arr_Colors[i].count+Arr_Colors[j].count));
                Arr_Colors[i].G := Round((Arr_Colors[i].G*Arr_Colors[i].count + Arr_Colors[j].G*Arr_Colors[j].count)/(Arr_Colors[i].count+Arr_Colors[j].count));
                Arr_Colors[i].B := Round((Arr_Colors[i].B*Arr_Colors[i].count + Arr_Colors[j].B*Arr_Colors[j].count)/(Arr_Colors[i].count+Arr_Colors[j].count));
                Arr_Colors[i].count :=Arr_Colors[i].count+Arr_Colors[j].count;
                For ind:=j to Length(Arr_Colors)-2 do
                  begin
                     Arr_Colors[ind].R :=Arr_Colors[ind+1].R ;
                     Arr_Colors[ind].G :=Arr_Colors[ind+1].G ;
                     Arr_Colors[ind].B :=Arr_Colors[ind+1].B ;
                     Arr_Colors[ind].count :=Arr_Colors[ind+1].count ;
                  end;
               SetLength(Arr_Colors,Length(Arr_Colors)-1);
               break;
              end
              else if j= Length(Arr_Colors)-1  then
               i:=i+1;
          end;
          q:=Length(Arr_Colors)-1;
        end;
      { with image4.Canvas do
       begin
       Pen.Color:=clBlack;
       Pen.Width:=1;
       FillRect(Rect((x-1)*10,(y-1)*10,x*10,y*10));
       end;
     //  image6.Canvas.FillRect(Rect((x-1)*15,(y-1)*15,x*15,y*15));
     //  image6.Canvas.TextOut((x-1)*15,(y-1)*15,IntTOStr(Desc_Item[x-1,y-1]));
       application.ProcessMessages;
       ShowColors(count);
      end;
    end;
end;}



procedure TForm1.BlackWhite;
var
 x, y ,imgh,imgw{,y_n,y_k,x_n,x_k}: integer;
 r, g, b : byte;
 c: dword;
begin
   // Image1.Picture.LoadFromFile('d:\'+Edit1.Text+'.bmp');
  imgh := Image1.Height;
  imgw :=Image1.Width;
  SetLength(DynArray, imgw,imgh) ;
//  sum:=0;
//  y_n:= round((Image1.Height)*0.3);
//  y_k:= round((Image1.Height)*0.7);
//  x_n:=round((Image1.Width)*0.3);
//  x_k:=round((Image1.Height)*0.7);
 { for y :=y_n  to y_k do
    for x := x_n to x_k do
    begin
      r := GetRValue(Image1.Canvas.Pixels[x,y]);
     sum:=sum+(r);
    end;
  Edit2.text:= FloatToStr(Round(sum/(y_k-y_n)/(x_k-x_n))) ;      }
//  Edit2.text:= IntToStr(otsuThreshold(x_n,x_k,y_n,y_k));
  application.ProcessMessages;
  for y := 0 to Image1.Height-1 do
  begin
    for x := 0 to Image1.Width-1 do
    begin
   //   c := Image1.Canvas.Pixels[x,y];
      r := GetRValue(Image1.Canvas.Pixels[x,y]);
     // g := GetGValue(c);
    //  b := GetBValue(c);
      if ({(}r {+ g + b) div 3} > StrToInt(Edit2.Text)) {or ((r + g + b) div 3 < StrToInt(Edit3.Text))} then
      begin
      //Image1.Canvas.Pixels[x,y] := clWhite;
      //  DynArray[x,y]:=false;
         DynArray[x,y]:=0;
      end
      else
      begin
      // Image1.Canvas.Pixels[x,y] := clBlack;
       //  DynArray[x,y]:=true;
       DynArray[x,y]:=1;
      end;
    end;
  end;
 { sum:=0;
  for y :=y_n  to y_k do
    for x := x_n to x_k do
    begin
      c := Image1.Canvas.Pixels[x,y];
      r := GetRValue(c);
      g := GetGValue(c);
      b := GetBValue(c);
     sum:=sum+(r + g + b) div 3 ;
    end;
  application.ProcessMessages;
  ShowMessage(FloatToStr(sum/(y_k-y_n)/(x_k-x_n)));   }
end;

{procedure TForm1.Change_Size(var bmp: TBitmap; Height: Integer);
var
imgh,imgw: integer;
 sbmp: TBitMap;
begin
     // if Image1.Height >300 then
  //  begin
    sbmp:=Tbitmap.create;
    imgh:=Height;
    imgw:=Round(bmp.Width*(Height/bmp.Height))  ;
    sbmp.width:=imgw;
    sbmp.Height:=imgh;
    sbmp.pixelFormat:=pf24bit;
    SetStretchBltMode(sbmp.canvas.handle,4);// ?????? ????????????
    StretchBlt(sbmp.canvas.handle,0,0,imgw,imgh,bmp.canvas.handle,
               0,0,bmp.width,bmp.height,SRCCOPY);
  //  Image1.Picture.Assign(sbmp) ;
    sbmp.Free;
   // end;
end; }

procedure TForm1.SetNull;
var
x,y:integer;
begin
SetLength(DynArray,0,0);
SetLength(DynArray2,0,0);
SetLength(DynArray4,0,0);
SetLength(Desc_Item,0,0);
for x:=0 to rec-1 do
for y:=0 to rec-1 do
begin
  SetLength(Desc_Info[x,y].id_obj,0);
  SetLength(Desc_Info[x,y].Pixels_obj,0);
  SetLength(Desc_Info[x,y].Summ,0);
  SetLength(Desc_Info[x,y].MinX,0);
  SetLength(Desc_Info[x,y].MaxX,0);
  SetLength(Desc_Info[x,y].MinY,0);
  SetLength(Desc_Info[x,y].MaxY,0);
end;
SetLength(Desc_Info, 0,0);
for x:=0 to Length(segm)-1 do
   FreeAndNil(segm[x]);
for x:=0 to Length(Lines)-1 do
   FreeAndNil(Lines[x]);
SetLength(segm, 0);
SetLength(Arr_ColorsHSL, 0,0);
SetLength(Arr_ColorsHSV, 0,0);
SetLength(Arr_ColorsYUV, 0,0);
SetLength(Arr_Colors,0);
DynArray:=nil ;
DynArray2:=nil;
Desc_Item:=nil ;
//SetLength(DynArray3,0);
FreeAndNil(horiz);
FreeAndNil(vert);
FreeAndNil(magn);
end;

{function TForm1.otsuThreshold(var xn,xk,yn,yk: Integer): Integer;
var
   min,max,i,x,y,temp,temp1,histSize,alpha, beta, threshold: Integer;
   sigma, maxSigma,w1,a: double;
   hist: array of Integer;
begin
   min:=GetGValue(image1.Canvas.Pixels[0,0]);
   max:=GetGValue(image1.Canvas.Pixels[0,0]);
 //  alpha:=0; beta:=0;
    threshold:=0;
//   sigma:=-1;
maxSigma:=-1;
   for y := yn to yk do
    for x := xn to xk do
   begin
      temp:=GetGValue(image1.Canvas.Pixels[x,y]);
      if(temp<min) then min := temp;
      if(temp>max) then max := temp;
   end;
   histSize:=max-min+1;
   Setlength(hist,histSize);
   for i:=0 to histSize-1 do
      hist[i]:=0;
   for y := yn to yk do
    for x := xn to xk do
      begin
      i:= GetGValue(image1.Canvas.Pixels[x,y])-min;
      hist[i]:= hist[i]+1;
      end;

   temp:=0; temp1:=0;
   alpha:=0; beta:=0;
   for i:=0 to (max-min) do
   begin
      temp :=temp + i*hist[i];
      temp1 :=temp1 + hist[i];
   end;

   for i:=0 to (max-min-1) do
 //  if (beta + hist[i])<>0 then
   begin
      alpha:=alpha+ i*hist[i];
      beta:=beta+ hist[i];
      w1 := beta / temp1;
      a := alpha / beta - (temp - alpha) / (temp1 - beta);
      sigma:=w1*(1-w1)*a*a;

      if(sigma>maxSigma) then
      begin
         maxSigma:=sigma;
         threshold:=i;
      end;
   end;
   SetLength(hist,0);
   result :=(threshold + min);
end;       }

function TForm1.Is_Line(pnt: IntArray; n:Integer): boolean;
var
 i,j,q,sumx,sumy,sumxy,sum2x,sum2y:integer;
 ax,bx,ay,by,delta: double;
begin
  sumx:=0; sumy:=0;sumxy:=0;sum2x:=0; sum2y:=0;
  for i:=0 to n-1 do
  begin
    sumx:=sumx+pnt[i,0];
    sumy:=sumy+pnt[i,1];
    sumxy:=sumxy+pnt[i,0]*pnt[i,1];
    sum2x:=sum2x+pnt[i,0]*pnt[i,0];
    sum2y:=sum2y+pnt[i,1]*pnt[i,1];
  end;
  if ((n*sum2x-sumx*sumx) = 0) or ((n*sum2y-sumy*sumy)=0) then
  result:=true
  else
  begin
  ax:=(sumxy*n-sumx*sumy)/(sum2x*n-sumx*sumx);
  bx:=(sumy-ax*sumx)/n;
  ay:=(sumxy*n-sumx*sumy)/(sum2y*n-sumy*sumy);
  by:=(sumx-ay*sumy)/n;
  delta:=0;
  if abs(ax)<abs(ay) then
   for i:=0 to n-1 do
     delta:=delta+abs((pnt[i,1]-Round((ax*pnt[i,0]+bx))))
  else
   for i:=0 to n-1 do
     delta:=delta+abs((pnt[i,0]-Round((ay*pnt[i,1]+by))));
  if delta<=2 then
  begin
    result:=true;
    if abs(ax)<abs(ay) then
       for i:=0 to image1.Width-1 do
         Image1.Canvas.Pixels[i,Round(ax*i+bx)]:=clBlue;
    {else
     for i:=0 to image1.Height-1 do
         Image1.Canvas.Pixels[Round(ay*i+by),i]:=clBlue  }
  end
  else
    result:=false;
  end;
end;

function TForm1.Get_LineParams(pnt: IntArray; n:Integer): LineParams;
var
 i,j,q,sumx,sumy,sumxy,sum2x,sum2y:integer;
begin
  sumx:=0; sumy:=0;sumxy:=0;sum2x:=0; sum2y:=0;
  for i:=0 to n-1 do
  begin
    sumx:=sumx+pnt[i,0];
    sumy:=sumy+pnt[i,1];
    sumxy:=sumxy+pnt[i,0]*pnt[i,1];
    sum2x:=sum2x+pnt[i,0]*pnt[i,0];
    sum2y:=sum2y+pnt[i,1]*pnt[i,1];
  end;
  if ((n*sum2x-sumx*sumx) = 0) then    //вертикальная прямая (x=const)
  begin
  result.a:=0;
  result.b:=0;
  end
  else if ((n*sum2y-sumy*sumy)=0) then  //горизонтальная прямая (y=const)
  begin
  result.a:=sumy/n;
  result.b:=0;
  end
  else
  begin
    result.b:=(sumxy*n-sumx*sumy)/(sum2x*n-sumx*sumx);
    result.a:=(sumy-result.b*sumx)/n;
  end;
end;



function TForm1.Is_Ellipce(pnt: IntArray): boolean;
var
 a,b:matrica;
 i,j,q:integer;
 det,opr:double;
 s: String;
 k: array[1..5] of double;
begin
  det:=0;
  for i:=0 to 4 do
  begin
    a[i+1,4]:=pnt[i,0];            //x
    a[i+1,1]:=pnt[i,0]*pnt[i,0];   //x^2
    a[i+1,5]:=pnt[i,1];             //y
    a[i+1,2]:=pnt[i,0]*pnt[i,1];   //xy
    a[i+1,3]:= pnt[i,1]*pnt[i,1];  //y^2
  end;
  //x^2             xy        y^2          x           y
{  a[1,1]:=0;  a[1,2]:=0;  a[1,3]:=1;  a[1,4]:=0;    a[1,5]:=1;
  a[2,1]:=1;  a[2,2]:=2;  a[2,3]:=4;  a[2,4]:=1;    a[2,5]:=2;
  a[3,1]:=4;  a[3,2]:=10; a[3,3]:=25;  a[3,4]:=2;    a[3,5]:=5;
  a[4,1]:=1; a[4,2]:=-2;   a[4,3]:=4;  a[4,4]:=-1;   a[4,5]:=2;
  a[5,1]:=4; a[5,2]:=-10;  a[5,3]:=25; a[5,4]:=-2;   a[5,5]:=5;      }
  s:='';
  for i:=1 to 5 do
    for j:=1 to 5 do
      b[i,j]:=a[i,j];
  prhod(b,5,det);
  opr:=det;
   // ShowMessage(FloatToStr(det));
  if opr=0 then Result:=false
  else
  begin
  for i:=1 to 5 do
  begin
    for j:=1 to 5 do
  {  if j<i then
    begin
      for q:=1 to 5 do
      if q<i then
        b[j,q]:=a[j,q]
      else if q>i then
        b[j,q-1]:=a[j,q];
    end
    else if j>i then     }
    begin
      for q:=1 to 5 do
      if q<>i then
        b[j,q]:=a[j,q]
      else
        b[j,q]:=20;
    //  else if q>i then
    //  b[j,q-1]:=a[j,q];
    end;
    prhod(b,5,det);
    k[i]:=det/opr;
    s:=s+FloatToStr(Round(k[i]*100)/100)+#13#10;
   end;//for i
  //ShowMessage(s);
  if (Round(k[1]*100)=0) and (Round(k[2]*100)=0) and (Round(k[3]*100)=0) then
  result:=false               // прямая
  else //if (k[1]) and (k[4]<0.01) and (k[5]<0.01) then
  result:=true ;
 // else
 // result:=false;
  end;
end;

end.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        
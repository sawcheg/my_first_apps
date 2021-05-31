unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, Word2000, OleServer,
  ExtCtrls,  ComCtrls,Math, Mask, Buttons, OleCtnrs,  ComObj, DBCtrls,ActiveX,DBITypes,
   DBIProcs, DBIErrs,BDE, jpeg, IdBaseComponent, IdComponent,
  IdTCPConnection, IdTCPClient, IdFTP,IniFiles, ShellApi,Tlhelp32, DBGridEh,
   xmldom, XMLIntf, msxmldom, XMLDoc;


type
  TForm1 = class(TForm)
    Edit1: TEdit;
    Button1: TButton;
    Label3: TLabel;
    Button2: TButton;
    Label1: TLabel;
    pb1: TProgressBar;
    SpeedButton1: TSpeedButton;
    OpenDialog1: TOpenDialog;
    Button3: TButton;
    wd: TWordDocument;
    Memo1: TMemo;
    WordApplication1: TWordApplication;
    procedure Button1Click(Sender: TObject);
    procedure Button2Click(Sender: TObject);
    procedure FormShow(Sender: TObject);
    procedure SpeedButton1Click(Sender: TObject);
    procedure Button3Click(Sender: TObject);
  private
    procedure wordspeed(val: boolean);
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;
  wdApp, wdDoc,wdDoc1,paragraph_current: Variant;
  stop_pr:boolean;

implementation

{$R *.dfm}

procedure TForm1.Button1Click(Sender: TObject);
var
 FileName  : OleVariant;
 i,j,q: integer;
 stop_pr:boolean;
 str:TStringList;
 clr,cash,blk: Tcolor;
 time,start: TdateTime;
begin
stop_pr:=false;
Button1.Enabled :=false;
Button2.Enabled :=true;
wdApp := CreateOleObject('Word.Application');
start:=now;
wordspeed(false);
try
FileName :=Edit1.Text;
wdDoc := wdApp.Documents.Add(FileName);
wdDoc1 := wdApp.Documents.Add;
//wdRng := wdDoc.Content;
j:= Length(wdDoc.Range.Text)-1;
//j:= wdDoc.Words.count-1;
label1.caption:='Символов: '+IntToStr(j);
blk:=-16777216;
pb1.Max:=j;
clr:= blk;
q:=0;
wdDoc1.Select;
for i:=0 to j do
begin
cash:=  wdDoc.Range(i,i+1).Font.Color ;
   if (cash<>blk) and (cash=clr) then
   begin
     wdApp.Selection.InsertAfter(wdDoc.Range(i,i+1));
     wdDoc1.Range(q,q+1).Font.Color:=cash;
        q:=q+1;
   end
   else if cash=blk then
   begin
     if clr<>blk then begin
    // wdApp.Selection.InsertAfter(IntToStr(wdDoc.Range(i,i+1).Information[3])) ;
     wdApp.Selection.InsertAfter(#13#10);
     clr:=blk;
     q:=q+1;
     end;
   end
   else
   begin
      wdApp.Selection.InsertAfter(wdDoc.Range(i,i+1));
      wdDoc1.Range(q,q+1).Font.Color:=cash;
      clr:=cash;
      q:=q+1;
      if Button1.Enabled then break;
      pb1.Position :=i;
      application.ProcessMessages;
   end;
end;
finally
wordspeed(true);
wdDoc.close;
time:=now-start;
label3.Caption:='Время:' + TimeToStr(time);
wdApp.Visible := True;
 Button1.Enabled :=true;
 Button2.Enabled :=false;
end;
end;

procedure TForm1.wordspeed(val: boolean);
begin
wdApp.screenupdating := val;
wdApp.Options.CheckGrammarAsYouType := val ;
wdApp.Options.CheckGrammarWithSpelling := val   ;
wdApp.Options.ContextualSpeller := val       ;
wdApp.Options.CheckSpellingAsYouType := val    ;
wdApp.Options.ShowReadabilityStatistics := val   ;
//wdApp.Options.ShowGrammaticalErrors := val      ;
//wdApp.Options.ShowSpellingErrors := val         ;
end;


procedure TForm1.Button2Click(Sender: TObject);
begin
 Button1.Enabled :=true;
 Button2.Enabled :=false;
end;

procedure TForm1.FormShow(Sender: TObject);
begin
//wdApp := CreateOleObject('Word.Application');
end;

procedure TForm1.SpeedButton1Click(Sender: TObject);
begin
  try
        OpenDialog1.Filter := 'файл Word (*.docx)|*.docx|файл Word 2003 (*.doc)|*.doc';
        if OpenDialog1.Execute then
                begin
                Edit1.Text:=OpenDialog1.FileName;
                end;
        except
        on e:Exception do
        end;
end;

procedure TForm1.Button3Click(Sender: TObject);
var
 FileName,fname  : OleVariant;
 i,j,q: integer;
 stop_pr:boolean;
 str:TStringList;
 clr,cash,blk: Tcolor;
 time,start: TdateTime;
 XMLDocument1:TXMLDocument ;

begin
stop_pr:=false;
Button3.Enabled :=false;
Button2.Enabled :=true;
wdApp := CreateOleObject('Word.Application');
start:=now;
wordspeed(false);
try
FileName :=Edit1.Text;
wdDoc := wdApp.Documents.Add(FileName);
wdDoc1 := wdApp.Documents.Add;
fname := ExtractFilePath(FileName) + 'test.xml';
wdDoc.SaveAs(FileName := fname, FileFormat := wdFormatRTF);
XMLDocument1.Active := true;
//XMLDocument1.FileName := fname;
Memo1.Text:=XMLDocument1.xml.Text ;
XMLDocument1.Active := false;

//wdRng := wdDoc.Content
j:= wdDoc.Words.count;
label1.caption:='Слов: '+IntToStr(j);
blk:=-0;
{pb1.Max:=j;
clr:= blk;
q:=1;  
wdDoc1.Select;
for i:=1 to j do
begin

cash:= wdDoc.Words.Item(i).Font.ColorIndex;
   if (cash<>blk) and (cash=clr) then
   begin
     wdApp.Selection.InsertAfter(wdDoc.Words.Item(i));
     wdDoc1.Words.Item(q).Font.ColorIndex:=cash;
        q:=q+1;
   end
   else if cash=blk then
   begin
     if clr<>blk then begin
    // wdApp.Selection.InsertAfter(IntToStr(wdDoc.Range(i,i+1).Information[3])) ;
     wdApp.Selection.InsertAfter(#13#10);
     clr:=blk;
     q:=q+1;
     end;
   end
   else
   begin
      wdApp.Selection.InsertAfter(wdDoc.Words.Item(i));
      wdDoc1.Words.Item(q).Font.ColorIndex:=cash;
      clr:=cash;
      q:=q+1;
      if Button1.Enabled then break;
      pb1.Position :=i;
      application.ProcessMessages;
   end;
end;           }
finally
wordspeed(true);
wdDoc.close;
time:=now-start;
label3.Caption:='Время:' + TimeToStr(time);
wdApp.Visible := True;
 Button1.Enabled :=true;
 Button2.Enabled :=false;
end;

end;

end.

program Project1;

uses
  Forms,
  Unit1 in 'Unit1.pas' {Form1},
  GBlur2 in 'GBlur2.pas',
  ListCl in 'ListCl.pas',
  ListObj in 'ListObj.pas',
  ListLine in 'ListLine.pas';

{$R *.res}

begin
  Application.Initialize;
  Application.CreateForm(TForm1, Form1);
  Application.Run;
end.

program GraphicEditor;

uses
  Forms,
  Unit1 in 'Unit1.pas' {FormMain},
  Unit2 in 'Unit2.pas' {GrParamsForm},
  Unit3 in 'Unit3.pas' {Form3},
  Unit4 in 'Unit4.pas' {Form4},
  GraphicList in 'GraphicList.pas';

{$R *.res}

begin
  Application.Initialize;
  Application.CreateForm(TFormMain, FormMain);
  Application.CreateForm(TGrParamsForm, GrParamsForm);
  Application.CreateForm(TForm3, Form3);
  Application.CreateForm(TForm4, Form4);
  Application.Run;
end.

object Form1: TForm1
  Left = 567
  Top = 127
  Width = 577
  Height = 450
  Caption = 'Form1'
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  OnShow = FormShow
  PixelsPerInch = 96
  TextHeight = 13
  object Label3: TLabel
    Left = 120
    Top = 80
    Width = 32
    Height = 13
    Caption = 'Label3'
  end
  object Label1: TLabel
    Left = 16
    Top = 80
    Width = 32
    Height = 13
    Caption = 'Label1'
  end
  object SpeedButton1: TSpeedButton
    Left = 512
    Top = 16
    Width = 23
    Height = 22
    Caption = '...'
    OnClick = SpeedButton1Click
  end
  object Edit1: TEdit
    Left = 8
    Top = 16
    Width = 505
    Height = 21
    TabOrder = 0
    Text = 'c:\Users\sania\Desktop\1.docx'
  end
  object Button1: TButton
    Left = 8
    Top = 48
    Width = 75
    Height = 25
    Caption = #1057#1090#1072#1088#1090
    TabOrder = 1
    OnClick = Button1Click
  end
  object Button2: TButton
    Left = 88
    Top = 48
    Width = 75
    Height = 25
    Caption = #1057#1090#1086#1087
    TabOrder = 2
    OnClick = Button2Click
  end
  object pb1: TProgressBar
    Left = 176
    Top = 48
    Width = 361
    Height = 17
    Min = 0
    Max = 100
    TabOrder = 3
  end
  object Button3: TButton
    Left = 256
    Top = 72
    Width = 75
    Height = 25
    Caption = 'Button3'
    TabOrder = 4
    OnClick = Button3Click
  end
  object Memo1: TMemo
    Left = 24
    Top = 136
    Width = 505
    Height = 265
    Lines.Strings = (
      'Memo1')
    TabOrder = 5
  end
  object OpenDialog1: TOpenDialog
    Left = 184
    Top = 72
  end
  object wd: TWordDocument
    AutoConnect = False
    ConnectKind = ckRunningOrNew
    Left = 424
    Top = 72
  end
  object WordApplication1: TWordApplication
    AutoConnect = False
    ConnectKind = ckRunningOrNew
    AutoQuit = False
    Left = 80
    Top = 104
  end
end

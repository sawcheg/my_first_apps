object GrParamsForm: TGrParamsForm
  Left = 389
  Top = 158
  BorderStyle = bsDialog
  Caption = #1055#1072#1088#1072#1084#1077#1090#1088#1099' '#1075#1088#1072#1092#1080#1082#1072
  ClientHeight = 170
  ClientWidth = 243
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  Position = poOwnerFormCenter
  OnCreate = FormCreate
  PixelsPerInch = 96
  TextHeight = 13
  object Label2: TLabel
    Left = 8
    Top = 8
    Width = 31
    Height = 13
    Caption = #1057#1077#1079#1086#1085
  end
  object Label3: TLabel
    Left = 8
    Top = 36
    Width = 27
    Height = 13
    Caption = #1044#1077#1085#1100
  end
  object Label4: TLabel
    Left = 4
    Top = 105
    Width = 86
    Height = 13
    Caption = #1044#1080#1072#1087#1072#1079#1086#1085' ('#1084#1080#1085'.):'
  end
  object Bevel1: TBevel
    Left = 6
    Top = 130
    Width = 229
    Height = 11
    Shape = bsTopLine
  end
  object Bevel2: TBevel
    Left = 6
    Top = 65
    Width = 229
    Height = 11
    Shape = bsTopLine
  end
  object cbSezon: TComboBox
    Left = 90
    Top = 5
    Width = 145
    Height = 21
    Style = csDropDownList
    ItemHeight = 13
    TabOrder = 0
    OnSelect = cbSezonSelect
  end
  object BitBtn1: TBitBtn
    Left = 77
    Top = 139
    Width = 75
    Height = 25
    TabOrder = 1
    Kind = bkOK
  end
  object BitBtn2: TBitBtn
    Left = 158
    Top = 139
    Width = 75
    Height = 25
    Caption = #1054#1090#1084#1077#1085#1072
    TabOrder = 2
    Kind = bkCancel
  end
  object cbGenerate: TCheckBox
    Left = 6
    Top = 82
    Width = 170
    Height = 17
    Caption = #1057#1075#1077#1085#1077#1088#1080#1088#1086#1074#1072#1090#1100' '#1089#1077#1090#1082#1091' '#1085#1072' '#1089#1091#1090#1082#1080
    Checked = True
    State = cbChecked
    TabOrder = 3
    OnClick = cbGenerateClick
  end
  object tim1: TRadioButton
    Left = 96
    Top = 104
    Width = 80
    Height = 17
    Caption = '30 ('#1087#1086#1083#1091#1095#1072#1089')'
    Checked = True
    TabOrder = 4
    TabStop = True
    OnClick = tim1Click
  end
  object Tim2: TRadioButton
    Left = 182
    Top = 104
    Width = 56
    Height = 17
    Caption = '60 ('#1095#1072#1089')'
    TabOrder = 5
    OnClick = Tim2Click
  end
  object cbDay: TComboBox
    Left = 90
    Top = 32
    Width = 145
    Height = 21
    Style = csDropDownList
    ItemHeight = 13
    TabOrder = 6
  end
end

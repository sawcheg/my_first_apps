object Form1: TForm1
  Left = 210
  Top = 39
  Width = 1056
  Height = 649
  Caption = #1044#1086#1089#1082#1072
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  DesignSize = (
    1040
    611)
  PixelsPerInch = 96
  TextHeight = 13
  object Label2: TLabel
    Left = 872
    Top = 524
    Width = 32
    Height = 13
    Anchors = [akRight, akBottom]
    Caption = 'Label2'
  end
  object Button1: TButton
    Left = 746
    Top = 562
    Width = 75
    Height = 25
    Anchors = [akRight, akBottom]
    Caption = 'Button1'
    TabOrder = 1
  end
  object Button2: TButton
    Left = 625
    Top = 562
    Width = 116
    Height = 25
    Anchors = [akRight, akBottom]
    Caption = 'Button2'
    TabOrder = 0
    OnClick = Button2Click
  end
  object Memo1: TMemo
    Left = 10
    Top = 466
    Width = 607
    Height = 137
    Anchors = [akRight, akBottom]
    TabOrder = 2
  end
  object PageControl1: TPageControl
    Left = 8
    Top = 0
    Width = 1002
    Height = 443
    ActivePage = TabSheet2
    Anchors = [akLeft, akTop, akRight, akBottom]
    TabIndex = 1
    TabOrder = 3
    object TabSheet1: TTabSheet
      Caption = 'TabSheet1'
      object Image2: TImage
        Left = 544
        Top = 24
        Width = 481
        Height = 257
      end
      object Image3: TImage
        Left = 40
        Top = 280
        Width = 497
        Height = 225
      end
      object Label1: TLabel
        Left = 616
        Top = 320
        Width = 32
        Height = 13
        Caption = 'Label1'
      end
    end
    object TabSheet2: TTabSheet
      Caption = 'TabSheet2'
      ImageIndex = 1
      object Image1: TImage
        Left = 0
        Top = 4
        Width = 409
        Height = 205
        AutoSize = True
      end
      object Image5: TImage
        Left = 520
        Top = 4
        Width = 353
        Height = 181
        AutoSize = True
      end
      object Image4: TImage
        Left = 0
        Top = 216
        Width = 177
        Height = 185
      end
      object Image6: TImage
        Left = 184
        Top = 216
        Width = 233
        Height = 185
      end
    end
  end
  object Edit1: TEdit
    Left = 642
    Top = 522
    Width = 41
    Height = 21
    Anchors = [akRight, akBottom]
    TabOrder = 4
    Text = '6'
  end
  object Edit2: TEdit
    Left = 722
    Top = 522
    Width = 41
    Height = 21
    Anchors = [akRight, akBottom]
    TabOrder = 5
    Text = '130'
  end
  object Edit3: TEdit
    Left = 776
    Top = 524
    Width = 65
    Height = 21
    Anchors = [akRight, akBottom]
    TabOrder = 6
    Text = '40'
  end
end

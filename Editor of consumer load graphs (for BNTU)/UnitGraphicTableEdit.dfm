object FormGraphicTableEdit: TFormGraphicTableEdit
  Left = 676
  Top = 223
  Width = 617
  Height = 380
  BorderStyle = bsSizeToolWin
  Caption = #1056#1077#1076#1072#1082#1090#1080#1088#1086#1074#1072#1085#1080#1077' '#1079#1085#1072#1095#1077#1085#1080#1081' '#1090#1080#1087#1086#1074#1086#1075#1086' '#1075#1088#1072#1092#1080#1082#1072' '#1085#1072#1075#1088#1091#1079#1086#1082
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  OnClose = FormClose
  DesignSize = (
    601
    342)
  PixelsPerInch = 96
  TextHeight = 13
  object Chart1: TChart
    Left = 0
    Top = 0
    Width = 421
    Height = 342
    BackWall.Brush.Color = clWhite
    BackWall.Brush.Style = bsClear
    BackWall.Color = clWhite
    BackWall.Pen.Color = clWhite
    BackWall.Pen.SmallDots = True
    BottomWall.Brush.Color = clWhite
    Gradient.EndColor = clSilver
    Gradient.Visible = True
    LeftWall.Brush.Color = clWhite
    LeftWall.Color = clYellow
    LeftWall.Dark3D = False
    Title.Color = clOlive
    Title.Text.Strings = (
      'TChart')
    Title.Visible = False
    BackColor = clWhite
    BottomAxis.AxisValuesFormat = '##'
    BottomAxis.Grid.Color = clWhite
    BottomAxis.Grid.SmallDots = True
    BottomAxis.LabelsAngle = 90
    BottomAxis.LabelsSeparation = 50
    BottomAxis.LabelStyle = talText
    BottomAxis.MinorTickCount = 0
    BottomAxis.MinorTicks.Color = -1
    BottomAxis.RoundFirstLabel = False
    BottomAxis.TickOnLabelsOnly = False
    BottomAxis.Ticks.Color = -1
    BottomAxis.TicksInner.Color = -1
    Chart3DPercent = 5
    Frame.Color = clWhite
    Frame.SmallDots = True
    LeftAxis.Automatic = False
    LeftAxis.AutomaticMaximum = False
    LeftAxis.AutomaticMinimum = False
    LeftAxis.Axis.Width = 1
    LeftAxis.Grid.Color = clWhite
    LeftAxis.Grid.SmallDots = True
    LeftAxis.Maximum = 1
    LeftAxis.MinorTickLength = 1
    LeftAxis.MinorTicks.Color = -1
    LeftAxis.RoundFirstLabel = False
    LeftAxis.TickOnLabelsOnly = False
    LeftAxis.Ticks.Color = -1
    LeftAxis.TicksInner.Color = -1
    LeftAxis.TicksInner.SmallDots = True
    LeftAxis.Title.Caption = #1047#1085#1072#1095#1077#1085#1080#1103', '#1086'.'#1077'.'
    LeftAxis.TitleSize = 1
    Legend.ColorWidth = 15
    Legend.Visible = False
    View3DOptions.Elevation = 348
    View3DOptions.HorizOffset = 2
    View3DOptions.Perspective = 25
    View3DOptions.Zoom = 101
    Align = alLeft
    TabOrder = 0
    Anchors = [akLeft, akTop, akRight, akBottom]
    object Series1: TBarSeries
      Marks.ArrowLength = 8
      Marks.Clip = True
      Marks.Frame.Style = psDashDotDot
      Marks.Frame.Visible = False
      Marks.Style = smsValue
      Marks.Visible = False
      SeriesColor = 16420148
      BarWidthPercent = 100
      Dark3D = False
      SideMargins = False
      XValues.DateTime = False
      XValues.Name = 'X'
      XValues.Multiplier = 1
      XValues.Order = loAscending
      YValues.DateTime = False
      YValues.Name = 'Bar'
      YValues.Multiplier = 1
      YValues.Order = loNone
    end
  end
  object BitBtn1: TBitBtn
    Left = 427
    Top = 317
    Width = 81
    Height = 25
    Anchors = [akRight, akBottom]
    Caption = 'OK'
    Default = True
    ModalResult = 1
    TabOrder = 1
    Glyph.Data = {
      DE010000424DDE01000000000000760000002800000024000000120000000100
      0400000000006801000000000000000000001000000000000000000000000000
      80000080000000808000800000008000800080800000C0C0C000808080000000
      FF0000FF000000FFFF00FF000000FF00FF00FFFF0000FFFFFF00333333333333
      3333333333333333333333330000333333333333333333333333F33333333333
      00003333344333333333333333388F3333333333000033334224333333333333
      338338F3333333330000333422224333333333333833338F3333333300003342
      222224333333333383333338F3333333000034222A22224333333338F338F333
      8F33333300003222A3A2224333333338F3838F338F33333300003A2A333A2224
      33333338F83338F338F33333000033A33333A222433333338333338F338F3333
      0000333333333A222433333333333338F338F33300003333333333A222433333
      333333338F338F33000033333333333A222433333333333338F338F300003333
      33333333A222433333333333338F338F00003333333333333A22433333333333
      3338F38F000033333333333333A223333333333333338F830000333333333333
      333A333333333333333338330000333333333333333333333333333333333333
      0000}
    NumGlyphs = 2
  end
  object BitBtn2: TBitBtn
    Left = 515
    Top = 317
    Width = 83
    Height = 25
    Anchors = [akRight, akBottom]
    Caption = #1054#1090#1084#1077#1085#1072
    TabOrder = 2
    Kind = bkCancel
  end
  object Grid: TDBGridPlus
    Left = 421
    Top = 0
    Width = 180
    Height = 313
    Align = alCustom
    Anchors = [akTop, akRight, akBottom]
    BiDiMode = bdLeftToRight
    Ctl3D = False
    DataSource = DataSource1
    Options = [dgEditing, dgTitles, dgColumnResize, dgColLines, dgRowLines, dgTabs, dgCancelOnExit]
    ParentBiDiMode = False
    ParentCtl3D = False
    TabOrder = 3
    TitleFont.Charset = DEFAULT_CHARSET
    TitleFont.Color = clWindowText
    TitleFont.Height = -11
    TitleFont.Name = 'MS Sans Serif'
    TitleFont.Style = []
    TitleSeparator = 0
    FrameLine.Strings = (
      '|~~'#1042#1088#1077#1084#1103'~~|'#1047#1085#1072#1095#1077#1085#1080#1077', '#1086'.'#1077'.|')
    Height_Shap = 62
    TitleLineHeight = 20
    AlwaysFocusEditor = False
    ColCount = 2
    FixedParams.FreeColor = clWindow
    FixedParams.FixColor = 15658734
    EditorParams.ButtonsShowing = bsAll
    EditorParams.ButtonsWidth = 16
    EditorParams.ButtonsColor = clBtnFace
    VisibleHScroll = True
    Columns = <
      item
        Color = cl3DLight
        Expanded = False
        FieldName = 'TB_TIME'
        ReadOnly = True
        Title.Caption = #1042#1088#1077#1084#1103
        Width = 73
        Visible = True
      end
      item
        Expanded = False
        FieldName = 'GR_VALUE'
        Title.Caption = #1047#1085#1072#1095#1077#1085#1080#1077', '#1086'.'#1077'.'
        Width = 83
        Visible = True
      end>
  end
  object DataSource1: TDataSource
    DataSet = GRvalues
    Left = 32
  end
  object GRvalues: TpFIBDataSet
    Database = DM.fbDB
    Transaction = DM.fbTrans
    Options = [poTrimCharFields, poRefreshAfterPost, poStartTransaction, poAutoFormatFields, poAllowChangeSqls]
    BufferChunks = 32
    CachedUpdates = False
    UniDirectional = False
    UpdateRecordTypes = [cusUnmodified, cusModified, cusInserted]
    SelectSQL.Strings = (
      'SELECT * FROM GR_VALUES')
    AfterPost = GRvaluesAfterPost
    BeforeDelete = GRvaluesBeforeDelete
    OnCalcFields = GRvaluesCalcFields
    OnNewRecord = GRvaluesNewRecord
    DefaultFormats.DateTimeDisplayFormat = 'd MMMM yyyy '#39#1075'.'#39
    AutoUpdateOptions.UpdateTableName = 'CAT_GR_VALUES'
    AutoUpdateOptions.KeyFields = 'ID'
    AutoUpdateOptions.GeneratorName = 'GEN_CAT_GR_VALUES_ID'
    AutoUpdateOptions.WhenGetGenID = wgOnNewRecord
    poUseBooleanField = True
    poSQLINT64ToBCD = True
    object GRvaluesGR_VALUE: TFIBFloatField
      FieldName = 'GR_VALUE'
    end
    object GRvaluesID: TFIBIntegerField
      FieldName = 'ID'
    end
    object GRvaluesID_GR_OBJECTS: TFIBIntegerField
      FieldName = 'ID_GR_OBJECTS'
    end
    object GRvaluesGR_TYPE: TFIBIntegerField
      FieldName = 'GR_TYPE'
    end
    object GRvaluesGR_TIME: TFIBIntegerField
      FieldName = 'GR_TIME'
    end
    object GRvaluesTB_TIME: TStringField
      FieldKind = fkCalculated
      FieldName = 'TB_TIME'
      Size = 6
      Calculated = True
    end
  end
end

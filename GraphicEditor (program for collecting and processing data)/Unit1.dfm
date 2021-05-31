object FormMain: TFormMain
  Left = 525
  Top = 139
  Width = 632
  Height = 699
  Caption = #1043#1088#1072#1092#1080#1082#1080' '#1085#1072#1075#1088#1091#1079#1086#1082
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  Position = poScreenCenter
  OnClose = FormClose
  PixelsPerInch = 96
  TextHeight = 13
  object Splitter1: TSplitter
    Left = 367
    Top = 46
    Width = 5
    Height = 434
    Cursor = crHSplit
    Align = alRight
    Color = clGray
    ParentColor = False
  end
  object pnConnect: TPanel
    Left = 0
    Top = 0
    Width = 616
    Height = 46
    Align = alTop
    TabOrder = 0
    DesignSize = (
      616
      46)
    object Label1: TLabel
      Left = 5
      Top = 8
      Width = 19
      Height = 13
      Caption = #1041#1044':'
    end
    object edDBPath: TEdit
      Left = 30
      Top = 5
      Width = 498
      Height = 21
      Anchors = [akLeft, akTop, akRight]
      TabOrder = 0
      Text = 'C:\Users\Solovey_AV\Desktop\BrestES.FDB'
    end
    object btOpenDBFile: TButton
      Left = 530
      Top = 5
      Width = 21
      Height = 21
      Anchors = [akTop, akRight]
      Caption = '...'
      TabOrder = 1
      OnClick = btOpenDBFileClick
    end
    object btConnect: TButton
      Left = 556
      Top = 3
      Width = 61
      Height = 23
      Anchors = [akTop, akRight]
      Caption = #1054#1090#1082#1088#1099#1090#1100
      Default = True
      TabOrder = 2
      OnClick = btConnectClick
    end
  end
  object tv: TTreeView
    Left = 0
    Top = 46
    Width = 367
    Height = 434
    Align = alClient
    Indent = 19
    ReadOnly = True
    TabOrder = 1
    OnChange = tvChange
  end
  object Panel2: TPanel
    Left = 372
    Top = 46
    Width = 244
    Height = 434
    Align = alRight
    Caption = #1085#1077#1090' '#1076#1072#1085#1085#1099#1093
    TabOrder = 2
    object Grid: TDBGridEh
      Left = 1
      Top = 1
      Width = 242
      Height = 432
      Align = alClient
      AutoFitColWidths = True
      DataSource = GrDataSource
      Flat = True
      FooterColor = clWindow
      FooterFont.Charset = DEFAULT_CHARSET
      FooterFont.Color = clWindowText
      FooterFont.Height = -11
      FooterFont.Name = 'MS Sans Serif'
      FooterFont.Style = []
      TabOrder = 0
      TitleFont.Charset = DEFAULT_CHARSET
      TitleFont.Color = clWindowText
      TitleFont.Height = -11
      TitleFont.Name = 'MS Sans Serif'
      TitleFont.Style = []
      Visible = False
      Columns = <
        item
          DisplayFormat = 'hh:mm'
          EditButtons = <>
          FieldName = 'GTIME'
          Footers = <>
          Title.Caption = #1042#1088#1077#1084#1103
        end
        item
          EditButtons = <>
          FieldName = 'P'
          Footers = <>
          Title.Caption = 'P, '#1082#1042#1090
        end
        item
          EditButtons = <>
          FieldName = 'Q'
          Footers = <>
          Title.Caption = 'Q, '#1082#1074#1072#1088
        end>
    end
  end
  object Panel3: TPanel
    Left = 0
    Top = 480
    Width = 616
    Height = 181
    Align = alBottom
    BevelOuter = bvNone
    TabOrder = 3
    DesignSize = (
      616
      181)
    object Label2: TLabel
      Left = 117
      Top = 37
      Width = 107
      Height = 13
      Caption = #1055#1072#1088#1072#1084#1077#1090#1088#1099' '#1086#1073#1098#1077#1082#1090#1072':'
    end
    object Label3: TLabel
      Left = 142
      Top = 56
      Width = 82
      Height = 26
      Alignment = taCenter
      Caption = #1059#1089#1090#1072#1085#1086#1074#1083#1077#1085#1085#1072#1103' '#1084#1086#1097#1085#1086#1089#1090#1100', '#1082#1042#1090
      ParentShowHint = False
      ShowHint = False
      Transparent = True
      WordWrap = True
    end
    object Label4: TLabel
      Left = 242
      Top = 56
      Width = 96
      Height = 26
      Alignment = taCenter
      Caption = #1050#1086#1101#1092#1092'. '#1084#1086#1097#1085#1086#1089#1090#1080' (cosFi), o.'#1077'. '
      WordWrap = True
    end
    object Label5: TLabel
      Left = 344
      Top = 29
      Width = 66
      Height = 13
      Caption = #1055#1088#1080#1084#1077#1095#1072#1085#1080#1077':'
    end
    object strType: TLabel
      Left = 0
      Top = 112
      Width = 337
      Height = 34
      Alignment = taCenter
      AutoSize = False
      BiDiMode = bdLeftToRight
      ParentBiDiMode = False
      Layout = tlBottom
      WordWrap = True
    end
    object Label7: TLabel
      Left = 21
      Top = 56
      Width = 76
      Height = 13
      Caption = #1053#1072#1080#1084#1077#1085#1086#1074#1072#1085#1080#1077
    end
    object Label6: TLabel
      Left = 16
      Top = 160
      Width = 29
      Height = 13
      Caption = 'Excel:'
    end
    object Label8: TLabel
      Left = 272
      Top = 160
      Width = 18
      Height = 13
      Caption = 'BD:'
    end
    object btNewGR: TButton
      Left = 220
      Top = 6
      Width = 101
      Height = 25
      Caption = #1053#1086#1074#1099#1081' '#1075#1088#1072#1092#1080#1082
      Enabled = False
      TabOrder = 0
      OnClick = btNewGRClick
    end
    object btNewObject: TButton
      Left = 5
      Top = 6
      Width = 106
      Height = 25
      Caption = #1053#1086#1074#1099#1081' '#1086#1073#1098#1077#1082#1090
      Enabled = False
      TabOrder = 1
      OnClick = btNewObjectClick
    end
    object btClear: TButton
      Left = 453
      Top = 6
      Width = 75
      Height = 25
      Anchors = [akTop, akRight]
      Caption = #1054#1095#1080#1089#1090#1080#1090#1100
      Enabled = False
      TabOrder = 2
      OnClick = btClearClick
    end
    object Button2: TButton
      Left = 113
      Top = 6
      Width = 101
      Height = 25
      Caption = #1059#1076#1072#1083#1080#1090#1100' '#1086#1073#1098#1077#1082#1090
      Enabled = False
      TabOrder = 3
      OnClick = Button2Click
    end
    object eName: TEdit
      Left = 5
      Top = 88
      Width = 116
      Height = 21
      ReadOnly = True
      TabOrder = 4
    end
    object eUst: TEdit
      Left = 142
      Top = 88
      Width = 82
      Height = 21
      ReadOnly = True
      TabOrder = 5
    end
    object eCos: TEdit
      Left = 242
      Top = 88
      Width = 96
      Height = 21
      ReadOnly = True
      TabOrder = 6
    end
    object eDescr: TEdit
      Left = 344
      Top = 48
      Width = 265
      Height = 73
      AutoSize = False
      ReadOnly = True
      TabOrder = 7
    end
    object Button1: TButton
      Left = 348
      Top = 126
      Width = 61
      Height = 25
      Caption = #1047#1072#1075#1088#1091#1079#1080#1090#1100
      Enabled = False
      TabOrder = 8
      OnClick = Button1Click
    end
    object Button3: TButton
      Left = 420
      Top = 126
      Width = 69
      Height = 25
      Caption = #1054#1073#1088#1072#1073#1086#1090#1082#1072
      Enabled = False
      TabOrder = 9
      OnClick = Button3Click
    end
    object Button4: TButton
      Left = 496
      Top = 126
      Width = 113
      Height = 25
      Caption = #1057#1086#1079#1076#1072#1085#1080#1077' '#1090#1080#1087#1086#1074#1099#1093
      Enabled = False
      TabOrder = 10
      OnClick = Button4Click
    end
    object Button5: TButton
      Left = 495
      Top = 153
      Width = 113
      Height = 25
      Caption = #1047#1072#1075#1088#1091#1079#1082#1072' '#1074' '#1073#1072#1079#1091
      TabOrder = 11
      OnClick = Button5Click
    end
    object edExcelPath: TEdit
      Left = 54
      Top = 155
      Width = 179
      Height = 21
      Anchors = [akLeft, akTop, akRight]
      TabOrder = 12
      Text = 'C:\Users\Solovey_AV\Desktop\'#1058#1080#1087#1086#1074#1099#1077'2.xlsx'
    end
    object Button6: TButton
      Left = 234
      Top = 155
      Width = 21
      Height = 21
      Anchors = [akTop, akRight]
      Caption = '...'
      TabOrder = 13
    end
    object edDB2Path: TEdit
      Left = 294
      Top = 155
      Width = 179
      Height = 21
      Anchors = [akLeft, akTop, akRight]
      TabOrder = 14
      Text = 'C:\Users\Solovey_AV\Desktop\DWRES_2.FDB'
    end
    object Button7: TButton
      Left = 474
      Top = 155
      Width = 21
      Height = 21
      Anchors = [akTop, akRight]
      Caption = '...'
      TabOrder = 15
    end
  end
  object DB: TpFIBDatabase
    DBName = 'TEST.FDB '
    DBParams.Strings = (
      'lc_ctype=WIN1251'
      'user_name=SYSDBA'
      'password=masterkey')
    DefaultTransaction = pFIBTransaction1
    SQLDialect = 3
    Timeout = 0
    WaitForRestoreConnect = 0
    Left = 69
    Top = 261
  end
  object GrData: TpFIBDataSet
    Database = DB
    Transaction = pFIBTransaction1
    Options = [poTrimCharFields, poRefreshAfterPost, poStartTransaction, poAutoFormatFields]
    AutoCommit = True
    BufferChunks = 32
    CachedUpdates = False
    UniDirectional = False
    UpdateRecordTypes = [cusUnmodified, cusModified, cusInserted]
    UpdateSQL.Strings = (
      'UPDATE GR_DATA'
      'SET '
      '    ID_GR = :ID_GR,'
      '    GTIME = :GTIME,'
      '    P = :P,'
      '    Q = :Q'
      'WHERE'
      '    ID = :OLD_ID'
      '    ')
    DeleteSQL.Strings = (
      'DELETE FROM'
      '    GR_DATA'
      'WHERE'
      '        ID = :OLD_ID'
      '    ')
    InsertSQL.Strings = (
      'INSERT INTO GR_DATA('
      '    ID,'
      '    ID_GR,'
      '    GTIME,'
      '    P,'
      '    Q'
      ')'
      'VALUES('
      '    :ID,'
      '    :ID_GR,'
      '    :GTIME,'
      '    :P,'
      '    :Q'
      ')')
    RefreshSQL.Strings = (
      'SELECT'
      '    GR_.ID,'
      '    GR_.ID_GR,'
      '    GR_.GTIME,'
      '    GR_.P,'
      '    GR_.Q'
      'FROM'
      '    GR_DATA GR_'
      'where( '
      '    GR_.ID_GR = :ID'
      '     ) and (     GR_.ID = :OLD_ID'
      '     )'
      '    ')
    SelectSQL.Strings = (
      'SELECT'
      '    GR_.ID,'
      '    GR_.ID_GR,'
      '    GR_.GTIME,'
      '    GR_.P,'
      '    GR_.Q'
      'FROM'
      '    GR_DATA GR_'
      'where'
      '    GR_.ID_GR = :ID'
      'ORDER by'
      '    GR_.GTIME')
    DefaultFormats.DateTimeDisplayFormat = 'd MMMM yyyy '#39#1075'.'#39
    Left = 317
    Top = 261
    poUseBooleanField = True
    poSQLINT64ToBCD = True
    object GrDataID: TFIBIntegerField
      FieldName = 'ID'
    end
    object GrDataID_GR: TFIBIntegerField
      FieldName = 'ID_GR'
    end
    object GrDataP: TFIBFloatField
      FieldName = 'P'
    end
    object GrDataQ: TFIBFloatField
      FieldName = 'Q'
    end
    object GrDataGTIME: TFIBTimeField
      FieldName = 'GTIME'
      DisplayFormat = 'hh:mm AMPM'
    end
  end
  object pFIBTransaction1: TpFIBTransaction
    DefaultDatabase = DB
    TimeoutAction = TACommitRetaining
    TRParams.Strings = (
      'write'
      'nowait'
      'rec_version'
      'read_committed')
    TPBMode = tpbDefault
    Left = 148
    Top = 261
  end
  object GrDataSource: TDataSource
    DataSet = GrData
    Left = 110
    Top = 320
  end
  object GR: TpFIBDataSet
    Database = DB
    Transaction = pFIBTransaction1
    Options = [poTrimCharFields, poRefreshAfterPost, poStartTransaction, poAutoFormatFields]
    AutoCommit = True
    BufferChunks = 32
    CachedUpdates = False
    UniDirectional = False
    UpdateRecordTypes = [cusUnmodified, cusModified, cusInserted]
    UpdateSQL.Strings = (
      'UPDATE GR'
      'SET '
      '    OBJ_ID = :OBJ_ID,'
      '    SEZON_ID = :SEZON_ID,'
      '    DAY_ID = :DAY_ID'
      'WHERE'
      '    ID = :OLD_ID'
      '    ')
    DeleteSQL.Strings = (
      'DELETE FROM'
      '    GR'
      'WHERE'
      '        ID = :OLD_ID'
      '    ')
    InsertSQL.Strings = (
      'INSERT INTO GR('
      '    ID,'
      '    OBJ_ID,'
      '    SEZON_ID,'
      '    DAY_ID'
      ')'
      'VALUES('
      '    :ID,'
      '    :OBJ_ID,'
      '    :SEZON_ID,'
      '    :DAY_ID'
      ')')
    RefreshSQL.Strings = (
      'SELECT'
      '    GR.ID,'
      '    GR.OBJ_ID,'
      '    GR.SEZON_ID,'
      '    GR.DAY_ID'
      'FROM'
      '    GR GR'
      ''
      ' WHERE '
      '        GR.ID = :OLD_ID'
      '    ')
    SelectSQL.Strings = (
      'SELECT'
      '    GR.ID,'
      '    GR.OBJ_ID,'
      '    GR.SEZON_ID,'
      '    GR.DAY_ID'
      'FROM'
      '    GR GR')
    DefaultFormats.DateTimeDisplayFormat = 'd MMMM yyyy '#39#1075'.'#39
    Left = 263
    Top = 259
    poUseBooleanField = True
    poSQLINT64ToBCD = True
    object GRID2: TFIBIntegerField
      FieldName = 'ID'
    end
    object GROBJ_ID: TFIBIntegerField
      FieldName = 'OBJ_ID'
    end
    object GRSEZON_ID: TFIBIntegerField
      FieldName = 'SEZON_ID'
    end
    object GRDAY_ID: TFIBIntegerField
      FieldName = 'DAY_ID'
    end
  end
  object OBJECTS: TpFIBDataSet
    Database = DB
    Transaction = pFIBTransaction1
    Options = [poTrimCharFields, poRefreshAfterPost, poStartTransaction, poAutoFormatFields]
    AutoCommit = True
    BufferChunks = 32
    CachedUpdates = False
    UniDirectional = False
    UpdateRecordTypes = [cusUnmodified, cusModified, cusInserted]
    UpdateSQL.Strings = (
      'UPDATE OBJECTS'
      'SET '
      '    NAME = :NAME,'
      '    DESCR = :DESCR,'
      '    OBJ_TYPE = :OBJ_TYPE,'
      '    U_NOM = :U_NOM,'
      '    VID_NAGR = :VID_NAGR,'
      '    UST_MOSHN = :UST_MOSHN,'
      '    COSFI = :COSFI,'
      '    ID_PODTYPE_OBJ=:ID_PODTYPE_OBJ'
      'WHERE'
      '    ID = :OLD_ID'
      '    ')
    DeleteSQL.Strings = (
      'DELETE FROM'
      '    OBJECTS'
      'WHERE'
      '        ID = :OLD_ID'
      '    ')
    InsertSQL.Strings = (
      'INSERT INTO OBJECTS('
      '    ID,'
      '    NAME,'
      '    DESCR,'
      '    OBJ_TYPE,'
      '    U_NOM,'
      '    VID_NAGR,'
      '    UST_MOSHN,'
      '    COSFI,'
      '    ID_PODTYPE_OBJ'
      ')'
      'VALUES('
      '    :ID,'
      '    :NAME,'
      '    :DESCR,'
      '    :OBJ_TYPE,'
      '    :U_NOM,'
      '    :VID_NAGR,'
      '    :UST_MOSHN,'
      '    :COSFI,'
      '    :ID_PODTYPE_OBJ'
      ')')
    RefreshSQL.Strings = (
      'SELECT'
      '    ID,'
      '    NAME,'
      '    DESCR,'
      '    OBJ_TYPE,'
      '    U_NOM,'
      '    VID_NAGR,'
      '    UST_MOSHN,'
      '    COSFI,'
      '    ID_PODTYPE_OBJ'
      'FROM'
      '    OBJECTS '
      ''
      ' WHERE '
      '        OBJECTS.ID = :OLD_ID'
      '    ')
    SelectSQL.Strings = (
      'SELECT'
      '    ID,'
      '    NAME,'
      '    DESCR,'
      '    OBJ_TYPE,'
      '    U_NOM,'
      '    VID_NAGR,'
      '    UST_MOSHN,'
      '    COSFI,'
      '    ID_PODTYPE_OBJ'
      'FROM'
      '    OBJECTS ')
    DefaultFormats.DateTimeDisplayFormat = 'd MMMM yyyy '#39#1075'.'#39
    Left = 208
    Top = 259
    poUseBooleanField = True
    poSQLINT64ToBCD = True
    object OBJECTSID: TFIBIntegerField
      FieldName = 'ID'
    end
    object OBJECTSNAME: TFIBStringField
      FieldName = 'NAME'
      Size = 64
      Transliterate = False
      EmptyStrToNull = True
    end
    object OBJECTSDESCR: TFIBStringField
      FieldName = 'DESCR'
      Size = 100
      Transliterate = False
      EmptyStrToNull = True
    end
    object OBJECTSOBJ_TYPE: TFIBIntegerField
      FieldName = 'OBJ_TYPE'
    end
    object OBJECTSU_NOM: TFIBIntegerField
      FieldName = 'U_NOM'
    end
    object OBJECTSVID_NAGR: TFIBIntegerField
      FieldName = 'VID_NAGR'
    end
    object OBJECTSUST_MOSHN: TFIBFloatField
      FieldName = 'UST_MOSHN'
    end
    object OBJECTSCOSFI: TFIBFloatField
      FieldName = 'COSFI'
    end
    object OBJECTSID_PODTYPE_OBJ: TFIBIntegerField
      FieldName = 'ID_PODTYPE_OBJ'
    end
  end
  object insQuery: TpFIBQuery
    Database = DB
    ParamCheck = True
    Transaction = pFIBTransaction1
    Left = 171
    Top = 324
    qoStartTransaction = True
  end
  object treeQuery: TpFIBQuery
    Database = DB
    ParamCheck = True
    SQL.Strings = (
      
        'select objects.ID OBJ_ID, objects.name OBJ_NAME, objects.obj_typ' +
        'e OBJ_TYPE, objects.descr DESCR,objects.u_nom U_NOM, objects.vid' +
        '_nagr VID_NAGR,'
      'gr.sezon_id , gr.day_id, gr.ID GR_ID from objects'
      'left join gr on (objects.id = gr.obj_id)'
      'order by objects.id, gr.sezon_id, gr.day_id')
    Transaction = pFIBTransaction1
    Left = 119
    Top = 195
    qoAutoCommit = True
    qoStartTransaction = True
  end
  object OD: TOpenDialog
    Filter = 'Firebird Databases|*.FDB'
    Left = 118
    Top = 81
  end
  object OpenDialog1: TOpenDialog
    Left = 32
    Top = 152
  end
end

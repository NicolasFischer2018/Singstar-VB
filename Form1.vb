Public Class Singstar_VB
    Dim PACK_EEToc As PAK_Toc
    Dim PAK_IOP0Toc As PAK_Toc
    Dim PAK_IOP1Toc As PAK_Toc
    Dim PAK_IOPMToc As PAK_Toc
    Dim Song() As SongDetails

    Private Declare Function SceeWhPx_TOC_Read_Dll Lib "Singstar.dll" (ByVal PAK As String, ByVal txtfile As String) As Integer
    Private Declare Function SceeWhPx_Create_Dll Lib "Singstar.dll" (ByVal PAK As String, ByVal source As String, ByVal type As Integer) As Integer
    Private Declare Function SceeWhPx_File_Extract_Filename_Dll Lib "Singstar.dll" (ByVal PAK As String, ByVal filename As String, ByVal destination As String, ByVal relative As Integer) As Integer
    Private Declare Function SceeWhPx_File_Extract_All_Dll Lib "Singstar.dll" (ByVal PAK As String, ByVal destination As String) As Integer

    Private Declare Function TX2RAW_Dll Lib "TX2RAW.dll" (ByVal src As String, ByVal dest As String) As Integer

    Private Declare Function FreeImage_GetFileType Lib "FreeImage.dll" Alias "_FreeImage_GetFileType@8" (ByVal filename As String, ByVal size As Long) As FreeImageAPI.FREE_IMAGE_FORMAT
    Private Declare Function FreeImage_FIFSupportsReadingInt Lib "FreeImage.dll" Alias "_FreeImage_FIFSupportsReading@4" (ByVal Format As FreeImageAPI.FREE_IMAGE_FORMAT) As Long
    Private Declare Function FreeImage_Load Lib "FreeImage.dll" Alias "_FreeImage_Load@12" (ByVal Format As FreeImageAPI.FREE_IMAGE_FORMAT, ByVal Filename As String, ByVal Flags As FreeImageAPI.FREE_IMAGE_LOAD_FLAGS) As FreeImageAPI.FIBITMAP
    Private Declare Function FreeImage_Save Lib "FreeImage.dll" Alias "_FreeImage_Save@16" (ByVal Format As FreeImageAPI.FREE_IMAGE_FORMAT, ByVal dib As FreeImageAPI.FIBITMAP, ByVal filename As String, ByVal Flags As FreeImageAPI.FREE_IMAGE_LOAD_FLAGS) As Boolean
    Private Declare Function FreeImage_Copy Lib "FreeImage.dll" Alias "_FreeImage_Copy@20" (ByVal Bitmap As FreeImageAPI.FIBITMAP, ByVal Left As Integer, ByVal Top As Integer, ByVal Right As Integer, ByVal Bottom As Integer) As FreeImageAPI.FIBITMAP
    Private Declare Function FreeImage_ConvertFromRawBits Lib "FreeImage.dll" Alias "_FreeImage_ConvertFromRawBits@36" (ByVal BitsPtr() As Byte, ByVal Width As Integer, ByVal Height As Integer, ByVal Pitch As Integer, ByVal BitsPerPixel As Integer, ByVal RedMask As UInteger, ByVal GreenMask As UInteger, ByVal BlueMask As UInteger, ByVal TopDown As Boolean) As FreeImageAPI.FIBITMAP
    Private Declare Function FreeImage_Invert Lib "FreeImage.dll" Alias "_FreeImage_Invert@4" (ByVal Bitmap As FreeImageAPI.FIBITMAP) As Boolean
    Private Declare Function FreeImage_Paste Lib "FreeImage.dll" Alias "_FreeImage_Paste@20" (ByVal BitmapDst As FreeImageAPI.FIBITMAP, ByVal BitmapSrc As FreeImageAPI.FIBITMAP, ByVal Left As Integer, ByVal Top As Integer, ByVal Alpha As Integer) As Boolean
    Private Declare Function FreeImage_ConvertTo24Bits Lib "FreeImage.dll" Alias "_FreeImage_ConvertTo24Bits@4" (ByVal Bitmap As FreeImageAPI.FIBITMAP) As FreeImageAPI.FIBITMAP
    Private Declare Function FreeImage_ColorQuantize Lib "FreeImage.dll" Alias "_FreeImage_ColorQuantize@8" (ByVal Bitmap As FreeImageAPI.FIBITMAP, ByVal QuantizeMethod As FreeImageAPI.FREE_IMAGE_QUANTIZE) As FreeImageAPI.FIBITMAP
    
    Private Function FixFilename(ByVal filename As String) As String
        Dim sName As String, sLetter As String
        Dim iAsc As Integer
        Dim i As Integer

        sName = filename

        For i = 1 To Len(sName)
            sLetter = Mid(sName, i, i + 1)
            iAsc = Asc(sLetter)
            If (iAsc = Asc("\") OrElse iAsc = Asc("/") OrElse iAsc = Asc(":") OrElse iAsc = Asc("*") OrElse iAsc = Asc("?") OrElse iAsc = Asc("""") OrElse iAsc = Asc("<") OrElse iAsc = Asc(">") OrElse iAsc = Asc("|")) Then
                sName = SLeft(sName, i - 1) & "?" & SRight(sName, Len(sName) - i)
            End If

        Next

        Return Replace(sName, "?", "")
    End Function

    Private Function Extract_XML_Songs(ByVal ID As Integer, ByVal dest As String) As Integer
        Dim xml_song_file As String
        Dim TextLine As String
        Dim TextBuffer As String
        Dim TempFolder As String

        Dim i, ret, TempID As Integer

        i = 0
        While i <> Song(0).Nbrofsongs
            If Song(i).ID = ID Then
                Exit While
            End If
            i = i + 1
        End While

        TempFolder = System.IO.Path.GetTempPath()

        xml_song_file = Song(i).xml_file

        i = 0
        While (i <> PACK_EEToc.PAKinfos.nbfiles)
            If InStr(PACK_EEToc.PAKFilelist(i).filename, xml_song_file) <> 0 Then
                If SceeWhPx_File_Extract_Filename_Dll(SingstarDVD.Text & "PACK_EE.PAK", PACK_EEToc.PAKFilelist(i).filename, TempFolder, 0) = -1 Then
                    MessageBox.Show("Error")
                    Return -1
                End If
                Exit While
            End If
            i = i + 1
        End While

        Dim objReader As New System.IO.StreamReader(TempFolder & xml_song_file)
        Dim objWriter As New System.IO.StreamWriter(dest)
        While objReader.Peek() <> -1
            TextLine = objReader.ReadLine()

            ret = InStr(TextLine, "SONG ID=")
            If ret <> 0 Then
                TextBuffer = SRight(TextLine, (Len(TextLine) + 1) - (ret + Len("SONG ID=") + 1))
                ret = InStr(TextBuffer, "TITLE=")
                TempID = Val(SLeft(TextBuffer, ret - 3))
                If TempID = ID Then
                    Do While InStr(TextLine, "</SONG>") = 0
                        objWriter.WriteLine(TextLine)
                        TextLine = objReader.ReadLine()
                    Loop
                    objWriter.WriteLine(TextLine)
                    Exit While
                End If
            End If
        End While

        objReader.Close()
        objWriter.Close()

        Return 0
    End Function

    Private Function Generate_Texture_Page(ByVal source As String, ByVal destination As String, ByVal page As Integer, ByVal picpos As Integer) As Integer
        Dim dibsrc As FreeImageAPI.FIBITMAP
        Dim dibdst As FreeImageAPI.FIBITMAP

        Dim fifsrc As FreeImageAPI.FREE_IMAGE_FORMAT
        Dim fifdst As FreeImageAPI.FREE_IMAGE_FORMAT

        Dim result As Boolean

        If picpos = 0 Then
            Dim buffer(0 To 512 * 256 * 4) As Byte
            dibdst = FreeImage_ConvertFromRawBits(buffer, 512, 256, 512 * 4, 32, 0, 0, 0, True)
            'Convert to 24Bits
            dibdst = FreeImage_ConvertTo24Bits(dibdst)
           
            If FreeImage_Save(FreeImageAPI.FREE_IMAGE_FORMAT.FIF_BMP, dibdst, destination, FreeImageAPI.FREE_IMAGE_LOAD_FLAGS.DEFAULT) = False Then
                Return -1
            End If
        End If
       

        fifsrc = FreeImage_GetFileType(source, 0)
        fifdst = FreeImage_GetFileType(destination, 0)
        If fifsrc = FreeImageAPI.FREE_IMAGE_FORMAT.FIF_UNKNOWN OrElse fifdst = FreeImageAPI.FREE_IMAGE_FORMAT.FIF_UNKNOWN Then
            Return -1
        End If
        
        dibsrc = FreeImage_Load(fifsrc, source, 0)
        'Convert to 24Bits
        dibsrc = FreeImage_ConvertTo24Bits(dibsrc)
        
        dibdst = FreeImage_Load(fifdst, destination, 0)

        If picpos = 0 Then
            result = FreeImage_Paste(dibdst, dibsrc, 0, 0, 256)
        End If
        If picpos = 1 Then
            result = FreeImage_Paste(dibdst, dibsrc, 170, 0, 256)
        End If
        If picpos = 2 Then
            result = FreeImage_Paste(dibdst, dibsrc, 340, 0, 256)
        End If

        If FreeImage_Save(FreeImageAPI.FREE_IMAGE_FORMAT.FIF_BMP, dibdst, destination, FreeImageAPI.FREE_IMAGE_LOAD_FLAGS.DEFAULT) = False Then
            Return -1
        End If

        Return 0
    End Function
    Private Function Convert_BMPTOTX2(ByVal src As String, ByVal dest As String) As Integer
        Dim TexturePath As String
        Dim TempFolder As String
        Dim AppID As Integer

        TexturePath = System.IO.Path.GetDirectoryName(src)
        TempFolder = System.IO.Path.GetTempPath()
        Convert_BMP_8BIT(src, src)
        If SceeWhPx_File_Extract_Filename_Dll(SingstarDVD.Text & "PACK_EE.PAK", "export\textures\page_0.tx2", TempFolder, 1) = -1 Then
            Logs.Items.Add("Error extracting template TX2 cover from Singstar DVD")
            Return -1
        End If
        IO.File.Delete(TempFolder & "source.tx2")
        IO.File.Delete(TempFolder & "temp.bmp")
        IO.File.Copy(TempFolder & "page_0.tx2", TempFolder & "source.tx2")
        IO.File.Copy(src, TempFolder & "temp.bmp")
        AppID = Shell(System.IO.Path.GetDirectoryName(Application.ExecutablePath) & "\BMP2TX2.EXE " & TempFolder & "temp.bmp" & " " & TempFolder & "temp.tx2", AppWinStyle.NormalNoFocus, False)
        System.Threading.Thread.Sleep(1000)
        AppActivate(AppID)
        SendKeys.SendWait("<")
        SendKeys.SendWait("<")
        System.Threading.Thread.Sleep(1000)

        IO.File.Move(TempFolder & "temp.tx2.TX2", dest)
        'IO.File.Delete(TexturePath & "\source.tx2")
        Return 0
    End Function
    Private Function Convert_BMP_8BIT(ByVal src As String, ByVal dest As String) As Integer
        Dim dibsrc As FreeImageAPI.FIBITMAP
        Dim fifsrc As FreeImageAPI.FREE_IMAGE_FORMAT

        fifsrc = FreeImage_GetFileType(src, 0)

        If fifsrc = FreeImageAPI.FREE_IMAGE_FORMAT.FIF_UNKNOWN Then
            Return -1
        End If

        dibsrc = FreeImage_Load(fifsrc, src, 0)

        'Convert to 8Bits
        dibsrc = FreeImage_ColorQuantize(dibsrc, FreeImageAPI.FREE_IMAGE_QUANTIZE.FIQ_NNQUANT)

        If FreeImage_Save(FreeImageAPI.FREE_IMAGE_FORMAT.FIF_BMP, dibsrc, dest, FreeImageAPI.FREE_IMAGE_LOAD_FLAGS.DEFAULT) = False Then
            Return -1
        End If

        Return 0
    End Function
    Private Function Convert_TX2TOBMP(ByVal tx2file As String, ByVal bmpfile As String, ByVal xori As Integer, ByVal yori As Integer) As Integer
        Dim dibsrc As FreeImageAPI.FIBITMAP
        Dim dibdst As FreeImageAPI.FIBITMAP
        Dim i As Integer
        Dim rawfile As String
        Dim TempFolder As String
        Dim buffer() As Byte
        Dim hwtemp As String = ""
        Dim width As Long
        Dim height As Long
        Dim pitch As Long


        TempFolder = System.IO.Path.GetTempPath()

        rawfile = TempFolder & "cover.raw"
        If TX2RAW_Dll(tx2file, rawfile) = -1 Then
            Return -1
        End If

        Dim objReader As New System.IO.FileStream(rawfile, IO.FileMode.Open)
        i = 0
        ReDim buffer(0 To objReader.Length)
        buffer(0) = 1
        Do While buffer(0) <> 0
            objReader.Read(buffer, 0, 1)
            If (buffer(0) <> 0) Then
                hwtemp = hwtemp & Chr(buffer(0))
            End If
        Loop
        width = Val(hwtemp)
        hwtemp = ""
        buffer(0) = 1
        Do While buffer(0) <> 0
            objReader.Read(buffer, 0, 1)
            If (buffer(0) <> 0) Then
                hwtemp = hwtemp & Chr(buffer(0))
            End If
        Loop
        height = Val(hwtemp)
        pitch = width * 4

        Do While objReader.Position <> objReader.Length
            objReader.Read(buffer, i, 1)
            i = i + 1
        Loop

        objReader.Close()
        dibsrc = FreeImage_ConvertFromRawBits(buffer, width, height, pitch, 32, 0, 0, 0, True)

        If FreeImage_FIFSupportsReadingInt(FreeImageAPI.FREE_IMAGE_FORMAT.FIF_BMP) = -1 Then
            Return -1
        End If

        dibdst = FreeImage_Copy(dibsrc, xori, yori, xori + 170, yori + 170)
        If FreeImage_Save(FreeImageAPI.FREE_IMAGE_FORMAT.FIF_BMP, dibdst, bmpfile, FreeImageAPI.FREE_IMAGE_LOAD_FLAGS.DEFAULT) = False Then
            Return -1
        End If
        Return 0
    End Function
    Private Function SRight(ByVal str As String, ByVal nbr As Integer) As String
        SRight = Microsoft.VisualBasic.Right(str, nbr)
    End Function
    Private Function SLeft(ByVal str As String, ByVal nbr As Integer) As String
        SLeft = Microsoft.VisualBasic.Left(str, nbr)
    End Function
    Private Sub Read_TOC_Function(ByRef PAKToc As PAK_Toc, ByVal PAK_File As String, ByVal TXTFile As String)
        Dim i As Integer
        Dim TextLine As String

        SceeWhPx_TOC_Read_Dll(PAK_File, TXTFile)

        If System.IO.File.Exists(TXTFile) = True Then
            Dim objReader As New System.IO.StreamReader(TXTFile)
            Do While objReader.Peek() <> -1
                TextLine = objReader.ReadLine()
                PAKToc.PAKinfos.PAK_File = TextLine
                TextLine = objReader.ReadLine()
                PAKToc.PAKinfos.err = CInt(TextLine)
                TextLine = objReader.ReadLine()
                PAKToc.PAKinfos.readed_TOC = CInt(TextLine)
                TextLine = objReader.ReadLine()
                PAKToc.PAKinfos.format = TextLine
                TextLine = objReader.ReadLine()
                PAKToc.PAKinfos.date_str = TextLine
                TextLine = objReader.ReadLine()
                PAKToc.PAKinfos.time = TextLine
                TextLine = objReader.ReadLine()
                PAKToc.PAKinfos.hdrsize = CInt(TextLine)
                TextLine = objReader.ReadLine()
                PAKToc.PAKinfos.nbfiles = CInt(TextLine)
                TextLine = objReader.ReadLine()
                PAKToc.PAKinfos.StrCompilePath = TextLine

                ReDim PAKToc.PAKinfos.extlist(0 To 31)
                Do While (1)
                    TextLine = objReader.ReadLine()
                    If TextLine = "Extension" Then
                        Exit Do
                    End If
                    PAKToc.PAKinfos.extlist(i) = TextLine
                    i = i + 1
                Loop

                i = 0
                ReDim PAKToc.PAKFilelist(0 To PAKToc.PAKinfos.nbfiles)
                Do While (i <> PAKToc.PAKinfos.nbfiles)
                    TextLine = objReader.ReadLine()
                    TextLine = objReader.ReadLine()
                    PAKToc.PAKFilelist(i).filename = TextLine
                    TextLine = objReader.ReadLine()
                    TextLine = objReader.ReadLine()
                    TextLine = objReader.ReadLine()
                    TextLine = objReader.ReadLine()
                    TextLine = objReader.ReadLine()
                    TextLine = objReader.ReadLine()
                    TextLine = objReader.ReadLine()
                    PAKToc.PAKFilelist(i).size = Val(TextLine)
                    TextLine = objReader.ReadLine()
                    TextLine = objReader.ReadLine()
                    PAKToc.PAKFilelist(i).extension = TextLine
                    TextLine = objReader.ReadLine()

                    i = i + 1
                Loop
            Loop
            objReader.Close()
        End If
    End Sub

    Private Sub List_Songs_Button(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim FolderBrowser As New FolderBrowserDialog
        Dim i, j, k, ret, doublons, UTF8 As Integer
        Dim ID As Integer
        Dim TempFolder As String
        Dim Songs_0_Filename As String
        Dim Songs_1_Filename As String
        Dim Songs_0_Filename_Old As String
        Dim Songs_1_Filename_Old As String
        Dim Songs_0_Filename_Old2 As String
        Dim Songs_1_Filename_Old2 As String
        Dim TextLine As String

        'With FolderBrowser
        '.RootFolder = Environment.SpecialFolder.Desktop
        '.Description = "Select Singstar DVD"
        'If .ShowDialog = DialogResult.OK Then
        'SingstarDVD.Text = .SelectedPath
        'Else
        'Exit Sub
        'End If
        'End With

        SingstarDVD.Text = "I:\"

        Songs_ListBox.Items.Clear()

        TempFolder = System.IO.Path.GetTempPath()

        Read_TOC_Function(PACK_EEToc, SingstarDVD.Text & "PACK_EE.PAK", TempFolder & "PACK_EE.txt")
        Read_TOC_Function(PAK_IOP0Toc, SingstarDVD.Text & "PAK_IOP0.PAK", TempFolder & "PAK_IOP0.txt")
        Read_TOC_Function(PAK_IOP1Toc, SingstarDVD.Text & "PAK_IOP1.PAK", TempFolder & "PAK_IOP1.txt")
        Read_TOC_Function(PAK_IOPMToc, SingstarDVD.Text & "PAK_IOPM.PAK", TempFolder & "PAK_IOPM.txt")

        i = 0

        ret = 0
        ID = 0
        ReDim Song(0 To 1)
        Song(0).Nbrofsongs = 0


        Do While (i <> PACK_EEToc.PAKinfos.nbfiles)
            j = 0
            Do While (j <> 11)
                Songs_0_Filename = "export\songs_" & j & "_0.xml"
                Songs_1_Filename = "export\songs_" & j & "_1.xml"
                Songs_0_Filename_Old = "export\songs_" & j & "_0.utf8.xml"
                Songs_1_Filename_Old = "export\songs_" & j & "_1.utf8.xml"
                Songs_0_Filename_Old2 = "export\songs" & j & ".xml"
                Songs_1_Filename_Old2 = "export\songs" & j & ".xml"
                If PACK_EEToc.PAKFilelist(i).filename = Songs_0_Filename OrElse PACK_EEToc.PAKFilelist(i).filename = Songs_1_Filename OrElse PACK_EEToc.PAKFilelist(i).filename = Songs_0_Filename_Old OrElse PACK_EEToc.PAKFilelist(i).filename = Songs_1_Filename_Old OrElse PACK_EEToc.PAKFilelist(i).filename = Songs_0_Filename_Old2 OrElse PACK_EEToc.PAKFilelist(i).filename = Songs_1_Filename_Old2 Then
                    If SceeWhPx_File_Extract_Filename_Dll(SingstarDVD.Text & "PACK_EE.PAK", PACK_EEToc.PAKFilelist(i).filename, TempFolder, 0) = -1 Then
                        MessageBox.Show("Error")
                        Exit Sub
                    End If
                    If System.IO.File.Exists(TempFolder & PACK_EEToc.PAKFilelist(i).filename) = True Then
                        Dim objReader As New System.IO.StreamReader(TempFolder & PACK_EEToc.PAKFilelist(i).filename)
                        UTF8 = 0
                        Do While objReader.Peek() <> -1
                            TextLine = objReader.ReadLine()
                            If InStr(TextLine, "UTF-8") OrElse PACK_EEToc.PAKFilelist(i).filename = Songs_0_Filename_Old2 OrElse PACK_EEToc.PAKFilelist(i).filename = Songs_1_Filename_Old2 Then
                                UTF8 = 1
                            End If

                            If InStr(TextLine, "SONG ID=") Then
                                ret = InStr(TextLine, "SONG ID=")
                                TextLine = SRight(TextLine, (Len(TextLine) + 1) - (ret + Len("SONG ID=") + 1))
                                ret = InStr(TextLine, "TITLE=")
                                ID = SLeft(TextLine, ret - 3)
                                k = 0
                                doublons = 0
                                While (k <> Song(0).Nbrofsongs)
                                    If ID = Song(k).ID Then
                                        doublons = 1
                                        Exit While
                                    End If
                                    k = k + 1
                                End While
                                If doublons = 0 And UTF8 = 1 Then
                                    ReDim Preserve Song(0 To Song(0).Nbrofsongs)
                                    If InStr(PACK_EEToc.PAKFilelist(i).filename, "0") Then
                                        Song(Song(0).Nbrofsongs).IOPX = 0
                                    Else
                                        Song(Song(0).Nbrofsongs).IOPX = 1
                                    End If
                                    If Not System.IO.File.Exists(SingstarDVD.Text & "PAK_IOP1.PAK") Then
                                        Song(Song(0).Nbrofsongs).IOPX = 0
                                    End If
                                    Song(Song(0).Nbrofsongs).File = j
                                    Song(Song(0).Nbrofsongs).ID = ID
                                    TextLine = SRight(TextLine, (Len(TextLine) + 1) - (ret + Len("TITLE=") + 1))
                                    ret = InStr(TextLine, "PERFORMANCE_TYPE=")
                                    Song(Song(0).Nbrofsongs).Title = SLeft(TextLine, ret - 3)
                                    TextLine = SRight(TextLine, (Len(TextLine) + 1) - (ret + Len("PERFORMANCE_TYPE=") + 1))
                                    ret = InStr(TextLine, "PERFORMANCE_NAME=")
                                    TextLine = SRight(TextLine, (Len(TextLine) + 1) - (ret + Len("PERFORMANCE_NAME=") + 1))
                                    Song(Song(0).Nbrofsongs).Artist = SLeft(TextLine, Len(TextLine) - 2)
                                    Song(Song(0).Nbrofsongs).xml_file = PACK_EEToc.PAKFilelist(i).filename

                                    Song(0).Nbrofsongs = Song(0).Nbrofsongs + 1
                                End If
                            End If
                        Loop
                        objReader.Close()
                    End If
                End If
                j = j + 1
            Loop
            i = i + 1
        Loop
        ReDim Preserve Song(0 To Song(0).Nbrofsongs)
        Song(Song(0).Nbrofsongs).ID = 0
        Song(0).Nbrofsongs = 0
        Do While Song(Song(0).Nbrofsongs).ID <> 0
            Songs_ListBox.Items.Add(Song(Song(0).Nbrofsongs).Artist & " - " & Song(Song(0).Nbrofsongs).Title)
            Song(0).Nbrofsongs = Song(0).Nbrofsongs + 1
        Loop
        Songs_ListBox.Sorted = True
        Label2.Text = "Number of Songs: " & Song(0).Nbrofsongs
    End Sub

    Private Sub Singstar_VB_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim path_INI As String
        Dim TextLine As String

        Logs.SelectionMode = SelectionMode.One
        path_INI = SLeft(Application.ExecutablePath, Len(Application.ExecutablePath) - 4) & ".ini"
        If System.IO.File.Exists(path_INI) Then
            Dim objReader As New System.IO.StreamReader(path_INI)
            Do While objReader.Peek() <> -1
                TextLine = objReader.ReadLine()
                If TextLine = "[LIBRARY_PATH]" Then
                    Library_path.Text = objReader.ReadLine()
                    Output_path.Text = Library_path.Text & "\OUTPUT"
                    TextLine = ""
                End If
            Loop
            objReader.Close()
        Else
            Dim writer As New System.IO.StreamWriter(path_INI)
            writer.WriteLine("[LIBRARY_PATH]")

            'Creating default set of folders
            Dim path_Lib As String
            path_Lib = System.IO.Path.GetDirectoryName(path_INI)
            System.IO.Directory.CreateDirectory(path_Lib & "\SONGS")
            System.IO.Directory.CreateDirectory(path_Lib & "\OUTPUT")
            System.IO.Directory.CreateDirectory(path_Lib & "\OUTPUT\PACK_EE")
            System.IO.Directory.CreateDirectory(path_Lib & "\OUTPUT\PAK_IOPM")
            System.IO.Directory.CreateDirectory(path_Lib & "\OUTPUT\PAK_IOP0")
            System.IO.Directory.CreateDirectory(path_Lib & "\OUTPUT\PAK_IOP1")

            writer.WriteLine(System.IO.Path.GetDirectoryName(path_INI))
            Library_path.Text = System.IO.Path.GetDirectoryName(path_INI)
            Output_path.Text = System.IO.Path.GetDirectoryName(path_INI) & "\OUTPUT"
            writer.Close()
        End If

        Dim Root As New IO.DirectoryInfo(Library_path.Text & "\SONGS")
        Dim Dirs As IO.DirectoryInfo() = Root.GetDirectories("*.*")
        Dim DirectoryName As IO.DirectoryInfo
        Library_ListBox.Items.Clear()
        For Each DirectoryName In Dirs
            Library_ListBox.Items.Add(SRight(DirectoryName.FullName, Len(DirectoryName.FullName) - Len(Library_path.Text & "\SONGS") - 1))
        Next
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim FolderBrowser As New FolderBrowserDialog
        Dim path_INI As String

        path_INI = SLeft(Application.ExecutablePath, Len(Application.ExecutablePath) - 4) & ".ini"
        With FolderBrowser
            .RootFolder = Environment.SpecialFolder.Desktop
            .Description = "Select local library folder"
            If .ShowDialog = DialogResult.OK Then
                Library_path.Text = .SelectedPath
            End If
        End With

        'Creating default set of folders
        Dim writer As New System.IO.StreamWriter(path_INI)
        writer.WriteLine("[LIBRARY_PATH]")

        'Creating default set of folders + writing INI
        Dim path_Lib As String
        path_Lib = Library_path.Text

        System.IO.Directory.CreateDirectory(path_Lib & "\SONGS")
        System.IO.Directory.CreateDirectory(path_Lib & "\OUTPUT\")
        System.IO.Directory.CreateDirectory(path_Lib & "\OUTPUT\PACK_EE")
        System.IO.Directory.CreateDirectory(path_Lib & "\OUTPUT\PAK_IOPM")
        System.IO.Directory.CreateDirectory(path_Lib & "\OUTPUT\PAK_IOP0")
        System.IO.Directory.CreateDirectory(path_Lib & "\OUTPUT\PAK_IOP1")
        writer.WriteLine(Library_path.Text)
        writer.Close()

        Dim Root As New IO.DirectoryInfo(Library_path.Text & "\SONGS")
        Dim Dirs As IO.DirectoryInfo() = Root.GetDirectories("*.*")
        Dim DirectoryName As IO.DirectoryInfo
        Library_ListBox.Items.Clear()
        For Each DirectoryName In Dirs
            Library_ListBox.Items.Add(SRight(DirectoryName.FullName, Len(DirectoryName.FullName) - Len(Library_path.Text & "\SONGS") - 1))
        Next
    End Sub

    Private Sub Copy_Songs_to_Library_Button(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim i, j, k, m, p As Integer
        Dim xori, yori As Integer
        Dim str1, str2 As String
        Dim PageFilename As String
        Dim TX2PageFilename As String
        Dim TempFolder As String
        Dim TextLine As String
        Dim array(0 To 10) As String
        Dim temparray(0 To 5) As String


        TempFolder = System.IO.Path.GetTempPath()
        Logs.Items.Add("Copying selected items to local library")
        i = 0
        Do While (i <> Songs_ListBox.SelectedItems.Count)
            str1 = Songs_ListBox.SelectedItems.Item(i).ToString
            j = 0
            Do While (j <> Song(0).Nbrofsongs)
                str2 = Song(j).Artist & " - " & Song(j).Title
                If str1 = str2 Then
                    System.IO.Directory.CreateDirectory(Library_path.Text & "\SONGS\" & FixFilename(Song(j).Artist & " - " & Song(j).Title))
                    Logs.Items.Add("Extracting " & str1)
                    Logs.SelectedIndex = Logs.Items.Count - 1
                    Application.DoEvents()

                    'Extracting XML files
                    Logs.Items.Add("XML files...")
                    Logs.SelectedIndex = Logs.Items.Count - 1
                    Application.DoEvents()
                    Extract_XML_Songs(Song(j).ID, Library_path.Text & "\SONGS\" & FixFilename(Song(j).Artist & " - " & Song(j).Title) & "\songs.xml")

                    'Extracting Covers
                    Logs.Items.Add("Covers...")
                    Logs.SelectedIndex = Logs.Items.Count - 1
                    Application.DoEvents()
                    k = 0
                    Do While (k <> PACK_EEToc.PAKinfos.nbfiles)
                        If InStr(PACK_EEToc.PAKFilelist(k).filename, "export\covers.xml") <> 0 Then
                            If SceeWhPx_File_Extract_Filename_Dll(SingstarDVD.Text & "PACK_EE.PAK", PACK_EEToc.PAKFilelist(k).filename, TempFolder, 0) = -1 Then
                                MessageBox.Show("Error")
                                Exit Sub
                            End If
                            If System.IO.File.Exists(TempFolder & PACK_EEToc.PAKFilelist(k).filename) = True Then
                                Dim objReader As New System.IO.StreamReader(TempFolder & PACK_EEToc.PAKFilelist(k).filename)
                                Do While objReader.Peek() <> -1
                                    TextLine = objReader.ReadLine()
                                    array = Split(TextLine, " ", 10, CompareMethod.Text)
                                    m = 0
                                    While (m <> array.Length)
                                        If InStr(array(m), "cover_" & Song(j).ID) <> 0 Then
                                            p = 0
                                            xori = 0
                                            yori = 0
                                            'Split page filename
                                            temparray = Split(array(m + 1), "=", 5, CompareMethod.Text)
                                            temparray = Split(temparray(1), """", 5, CompareMethod.Text)
                                            PageFilename = "export\textures\" & temparray(1) & ".tga"
                                            TX2PageFilename = "export\textures\" & temparray(1) & ".tx2"
                                            'Split origin X axis
                                            temparray = Split(array(m + 2), "=", 5, CompareMethod.Text)
                                            temparray = Split(temparray(1), """", 5, CompareMethod.Text)
                                            xori = Val(temparray(1))
                                            'Split origin Y axis
                                            temparray = Split(array(m + 3), "=", 5, CompareMethod.Text)
                                            temparray = Split(temparray(1), """", 5, CompareMethod.Text)
                                            yori = Val(temparray(1))
                                            While (p <> PACK_EEToc.PAKinfos.nbfiles)
                                                'Convert TX2 File
                                                If InStr(PACK_EEToc.PAKFilelist(p).filename, TX2PageFilename) <> 0 Then
                                                    If SceeWhPx_File_Extract_Filename_Dll(SingstarDVD.Text & "PACK_EE.PAK", PACK_EEToc.PAKFilelist(p).filename, TempFolder, 0) = -1 Then
                                                        MessageBox.Show("Error")
                                                        Exit Sub
                                                    End If
                                                    If Convert_TX2TOBMP(TempFolder & PACK_EEToc.PAKFilelist(p).filename, Library_path.Text & "\SONGS\" & FixFilename(Song(j).Artist & " - " & Song(j).Title) & "\cover.bmp", xori, yori) = -1 Then
                                                        MessageBox.Show("Error")
                                                        Exit Sub
                                                    End If
                                                    Exit While
                                                End If
                                                p = p + 1
                                            End While
                                            Exit While
                                        End If
                                        m = m + 1
                                    End While
                                Loop
                                objReader.Close()
                            End If
                        End If
                        k = k + 1
                    Loop

                    'Extracting Song files
                    Logs.Items.Add("Music & Video data...")
                    Logs.SelectedIndex = Logs.Items.Count - 1
                    Logs.Items.Add(" ")
                    Logs.SelectedIndex = Logs.Items.Count - 1
                    Application.DoEvents()
                    k = 0
                    Do While (k <> PACK_EEToc.PAKinfos.nbfiles)
                        If InStr(PACK_EEToc.PAKFilelist(k).filename, "\" & Song(j).ID & "\") <> 0 Then
                            If SceeWhPx_File_Extract_Filename_Dll(SingstarDVD.Text & "PACK_EE.PAK", PACK_EEToc.PAKFilelist(k).filename, Library_path.Text & "\SONGS\" & FixFilename(Song(j).Artist & " - " & Song(j).Title) & "\PACK_EE", 1) = -1 Then
                                MessageBox.Show("Error")
                                Exit Sub
                            End If
                        End If
                        k = k + 1
                    Loop
                    k = 0
                    Do While (k <> PAK_IOP0Toc.PAKinfos.nbfiles)
                        If InStr(PAK_IOP0Toc.PAKFilelist(k).filename, Song(j).ID & "\") <> 0 Then
                            If SceeWhPx_File_Extract_Filename_Dll(SingstarDVD.Text & "PAK_IOP0.PAK", PAK_IOP0Toc.PAKFilelist(k).filename, Library_path.Text & "\SONGS\" & FixFilename(Song(j).Artist & " - " & Song(j).Title) & "\PAK_IOPX", 1) = -1 Then
                                MessageBox.Show("Error")
                                Exit Sub
                            End If
                        End If
                        k = k + 1
                    Loop
                    k = 0
                    Do While (k <> PAK_IOP1Toc.PAKinfos.nbfiles)
                        If InStr(PAK_IOP1Toc.PAKFilelist(k).filename, Song(j).ID & "\") <> 0 Then
                            If SceeWhPx_File_Extract_Filename_Dll(SingstarDVD.Text & "PAK_IOP1.PAK", PAK_IOP1Toc.PAKFilelist(k).filename, Library_path.Text & "\SONGS\" & FixFilename(Song(j).Artist & " - " & Song(j).Title) & "\PAK_IOPX", 1) = -1 Then
                                MessageBox.Show("Error")
                                Exit Sub
                            End If
                        End If
                        k = k + 1
                    Loop
                    Extract_XML_Songs(Song(j).ID, Library_path.Text & "\SONGS\" & FixFilename(Song(j).Artist & " - " & Song(j).Title) & "\songs.xml")
                End If
                j = j + 1
            Loop
            i = i + 1
        Loop
        Logs.Items.Add("Done")
        Logs.SelectedIndex = Logs.Items.Count - 1
        Application.DoEvents()

        Dim Root As New IO.DirectoryInfo(Library_path.Text & "\SONGS")
        Dim Dirs As IO.DirectoryInfo() = Root.GetDirectories("*.*")
        Dim DirectoryName As IO.DirectoryInfo
        Library_ListBox.Items.Clear()
        For Each DirectoryName In Dirs
            Library_ListBox.Items.Add(SRight(DirectoryName.FullName, Len(DirectoryName.FullName) - Len(Library_path.Text & "\SONGS") - 1))
        Next
    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim FolderBrowser As New FolderBrowserDialog

        With FolderBrowser
            .RootFolder = Environment.SpecialFolder.Desktop
            .Description = "Select local library folder"
            If .ShowDialog = DialogResult.OK Then
                Output_path.Text = .SelectedPath
            End If
        End With

    End Sub

    Private Sub Create_Singstar_DVD(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        Dim i, j, doublons, IOPX, Duet As Integer
        Dim IDTempIOP0(1) As Integer
        Dim IDTempIOP1(1) As Integer
        Dim ret As Integer
        Dim Textline As String
        Dim Song As SongDetails



        'With FolderBrowser
        '.RootFolder = Environment.SpecialFolder.Desktop
        '.Description = "Select Singstar DVD"
        'If .ShowDialog = DialogResult.OK Then
        'SingstarDVD.Text = .SelectedPath
        'Else
        'Exit Sub
        'End If
        'End With

        SingstarDVD.Text = "I:\"

        'Extracting Core files from inserted DVD
        'Logs.Items.Add(" ")
        'Logs.Items.Add("Extracting Core files from inserted Singstar DVD")
        'Logs.SelectedIndex = Logs.Items.Count - 1
        'Logs.Items.Add("PACK_EE...")
        'Logs.SelectedIndex = Logs.Items.Count - 1
        'Application.DoEvents()
        'If SceeWhPx_File_Extract_All_Dll(SingstarDVD.Text & "PACK_EE.PAK", Output_path.Text & "\PACK_EE") = -1 Then
        '    Logs.Items.Add("Error")
        '    Logs.SelectedIndex = Logs.Items.Count - 1
        '    Exit Sub
        'End If

        'Logs.Items.Add("PAK_IOPM...")
        'Logs.SelectedIndex = Logs.Items.Count - 1
        'Application.DoEvents()
        'If SceeWhPx_File_Extract_All_Dll(SingstarDVD.Text & "PAK_IOPM.PAK", Output_path.Text & "\PAK_IOPM") = -1 Then
        '    Logs.Items.Add("Error")
        '    Logs.SelectedIndex = Logs.Items.Count - 1
        '    Exit Sub
        'End If

        i = 2
        IOPX = 0
        Duet = 0
        Song.IOPX = 0

        'Finding version of inserted DVD and number of IOP files supported
        While (i <> 9)
            If System.IO.File.Exists(Output_path.Text & "\PACK_EE\export\songs_" & i & "_0.xml") = False Then
                Exit While
            End If
            i = i + 1
        End While
        Song.Version = i - 1
        If System.IO.File.Exists(Output_path.Text & "\PACK_EE\export\songs_" & Song.Version & "_1.xml") = True Then
            Song.IOPX = 1
        End If

        If Song.Version < 5 OrElse Song.Version > 9 Then
            Logs.Items.Add("Singstar DVD version not supported. Abort.")
            Logs.SelectedIndex = Logs.Items.Count - 1
            Application.DoEvents()
            Exit Sub
        End If

        'Find Duet Song or Abort
        Song.ID = ""
        i = 0
        While (i <> Library_ListBox.SelectedItems.Count)
            If System.IO.File.Exists(Library_path.Text & "\SONGS\" & Library_ListBox.SelectedItems.Item(i).ToString & "\songs.xml") = True Then
                Dim objReader As New System.IO.StreamReader(Library_path.Text & "\SONGS\" & Library_ListBox.SelectedItems.Item(i).ToString & "\songs.xml")
                Do While objReader.Peek() <> -1
                    Textline = objReader.ReadLine()
                    If InStr(Textline, "SONG ID=") Then
                        ret = InStr(Textline, "SONG ID=")
                        Textline = SRight(Textline, (Len(Textline) + 1) - (ret + Len("SONG ID=") + 1))
                        ret = InStr(Textline, "TITLE=")
                        Song.ID = SLeft(Textline, ret - 3)
                    End If
                    If InStr(Textline, "<DUET") Then
                        Duet = 1
                    End If
                Loop
                objReader.Close()
                If Duet = 1 Then
                    Duet = Song.ID
                    Exit While
                End If
            End If
            i = i + 1
        End While
        If Duet = 0 Then
            Logs.Items.Add("No Duet Song. Abort.")
            Logs.SelectedIndex = Logs.Items.Count - 1
            Application.DoEvents()
            Exit Sub
        End If

        'Deleting useless files
        Singstar_Tools.Delete_Useless_Files(Output_path.Text)
        System.IO.Directory.CreateDirectory(Output_path.Text & "\PACK_EE\export\textures")

        i = 0
        'Examine Song.xml for ID and song infos
        Do While (i <> Library_ListBox.SelectedItems.Count)
            Logs.Items.Add("Injecting " & Library_ListBox.SelectedItems.Item(i).ToString & "...")
            Logs.SelectedIndex = Logs.Items.Count - 1
            Application.DoEvents()
            Song.ID = ""
            Song.ID_Artist = ""
            Song.Artist = ""
            If System.IO.File.Exists(Library_path.Text & "\SONGS\" & Library_ListBox.SelectedItems.Item(i).ToString & "\songs.xml") = True Then
                Dim objReader As New System.IO.StreamReader(Library_path.Text & "\SONGS\" & Library_ListBox.SelectedItems.Item(i).ToString & "\songs.xml")
                Do While objReader.Peek() <> -1
                    Textline = objReader.ReadLine()
                    If InStr(Textline, "SONG ID=") Then
                        ret = InStr(Textline, "SONG ID=")
                        Textline = SRight(Textline, (Len(Textline) + 1) - (ret + Len("SONG ID=") + 1))
                        ret = InStr(Textline, "TITLE=")
                        Song.ID = SLeft(Textline, ret - 3)
                        Textline = SRight(Textline, (Len(Textline) + 1) - (ret + Len("TITLE=") + 1))
                        ret = InStr(Textline, "PERFORMANCE_TYPE=")
                        Song.Title = SLeft(Textline, ret - 3)
                        Textline = SRight(Textline, (Len(Textline) + 1) - (ret + Len("PERFORMANCE_TYPE=") + 1))
                        ret = InStr(Textline, "PERFORMANCE_NAME=")
                        Textline = SRight(Textline, (Len(Textline) + 1) - (ret + Len("PERFORMANCE_NAME=") + 1))
                        Song.Artist = SLeft(Textline, Len(Textline) - 2)
                    End If
                    If InStr(Textline, "PERFORMED_BY ID=") Then
                        ret = InStr(Textline, "PERFORMED_BY ID=")
                        Textline = SRight(Textline, (Len(Textline) + 1) - (ret + Len("PERFORMED_BY ID=") + 1))
                        ret = InStr(Textline, ">")
                        Song.ID_Artist = SLeft(Textline, ret - 2)
                    End If
                Loop
                objReader.Close()

                'Creating Acts_X.xml
                j = 0
                doublons = 0
                If IOPX = 0 Then
                    While j <> IDTempIOP0.Length
                        If Song.ID_Artist = IDTempIOP0(j) Then
                            doublons = 1
                            Exit While
                        End If
                        j = j + 1
                    End While
                Else
                    While j <> IDTempIOP1.Length
                        If Song.ID_Artist = IDTempIOP1(j) Then
                            doublons = 1
                            Exit While
                        End If
                        j = j + 1
                    End While
                End If

                If doublons = 0 Then
                    If IOPX = 0 Then
                        ReDim Preserve IDTempIOP0(j)
                        IDTempIOP0(j) = Song.ID_Artist
                    Else
                        ReDim Preserve IDTempIOP1(j)
                        IDTempIOP1(j) = Song.ID_Artist
                    End If

                    j = 2
                    Do While (j <> Song.Version + 1)
                        'If i = 0 OrElse i = 1 Then
                        If i = 0 Then
                            If (j = 2 OrElse j = 3) Then
                                Singstar_Tools.Create_acts_XML(Output_path.Text & "\PACK_EE\export\acts_" & j & "_" & IOPX & ".xml", Song.ID_Artist, Song.Artist, False, 0)
                            Else
                                Singstar_Tools.Create_acts_XML(Output_path.Text & "\PACK_EE\export\acts_" & j & "_" & IOPX & ".xml", Song.ID_Artist, Song.Artist, True, 0)
                            End If
                        End If
                        If i = 1 And Song.IOPX = 1 Then
                            If (j = 2 OrElse j = 3) Then
                                Singstar_Tools.Create_acts_XML(Output_path.Text & "\PACK_EE\export\acts_" & j & "_" & IOPX & ".xml", Song.ID_Artist, Song.Artist, False, 0)
                            Else
                                Singstar_Tools.Create_acts_XML(Output_path.Text & "\PACK_EE\export\acts_" & j & "_" & IOPX & ".xml", Song.ID_Artist, Song.Artist, True, 0)
                            End If
                        End If
                        Singstar_Tools.Create_acts_XML(Output_path.Text & "\PACK_EE\export\acts_" & j & "_" & IOPX & ".xml", Song.ID_Artist, Song.Artist, True, 1)
                        If i = Library_ListBox.SelectedItems.Count - 1 Then
                            Singstar_Tools.Create_acts_XML(Output_path.Text & "\PACK_EE\export\acts_" & j & "_" & 0 & ".xml", Song.ID_Artist, Song.Artist, True, 2)
                            If i <> 0 And Song.IOPX = 1 Then
                                Singstar_Tools.Create_acts_XML(Output_path.Text & "\PACK_EE\export\acts_" & j & "_" & 1 & ".xml", Song.ID_Artist, Song.Artist, True, 2)
                            End If
                        End If

                        j = j + 1
                    Loop
                End If

                'Create covers.xml
                If i = 0 Then
                    Singstar_Tools.Create_covers_XML(Output_path.Text & "\PACK_EE\export\covers.xml", Song.ID, 0, 0, False, 0)
                End If
                Generate_Texture_Page(Library_path.Text & "\SONGS\" & Library_ListBox.SelectedItems.Item(i).ToString & "\cover.bmp", Output_path.Text & "\PACK_EE\export\textures\page_" & i \ 3 & ".bmp", i \ 3, i Mod 3)
                Singstar_Tools.Create_covers_XML(Output_path.Text & "\PACK_EE\export\covers.xml", Song.ID, i \ 3, i Mod 3, False, 1)
                If i = Library_ListBox.SelectedItems.Count - 1 Then
                    Singstar_Tools.Create_covers_XML(Output_path.Text & "\PACK_EE\export\covers.xml", Song.ID, 0, 0, False, 2)
                End If
                If i Mod 3 = 2 Then
                    Convert_BMPTOTX2(Output_path.Text & "\PACK_EE\export\textures\page_" & i \ 3 & ".bmp", Output_path.Text & "\PACK_EE\export\textures\page_" & i \ 3 & ".tx2")
                    IO.File.Delete(Output_path.Text & "\PACK_EE\export\textures\page_" & (i - 1) \ 3 & ".bmp")
                End If

                'Creating Songs_X.xml
                j = 2
                Do While (j <> Song.Version + 1)
                    'If i = 0 OrElse i = 1 Then
                    If i = 0 Then
                        If (j = 2 OrElse j = 3) Then
                            Singstar_Tools.Create_songs_XML(Output_path.Text & "\PACK_EE\export\songs_" & j & "_" & IOPX & ".xml", Library_path.Text & "\SONGS\" & Library_ListBox.SelectedItems.Item(i).ToString & "\songs.xml", False, 0)
                        Else
                            Singstar_Tools.Create_songs_XML(Output_path.Text & "\PACK_EE\export\songs_" & j & "_" & IOPX & ".xml", Library_path.Text & "\SONGS\" & Library_ListBox.SelectedItems.Item(i).ToString & "\songs.xml", True, 0)
                        End If
                    End If
                    If i = 1 And Song.IOPX = 1 Then
                        If (j = 2 OrElse j = 3) Then
                            Singstar_Tools.Create_songs_XML(Output_path.Text & "\PACK_EE\export\songs_" & j & "_" & IOPX & ".xml", Library_path.Text & "\SONGS\" & Library_ListBox.SelectedItems.Item(i).ToString & "\songs.xml", False, 0)
                        Else
                            Singstar_Tools.Create_songs_XML(Output_path.Text & "\PACK_EE\export\songs_" & j & "_" & IOPX & ".xml", Library_path.Text & "\SONGS\" & Library_ListBox.SelectedItems.Item(i).ToString & "\songs.xml", True, 0)
                        End If
                    End If
                    Singstar_Tools.Create_songs_XML(Output_path.Text & "\PACK_EE\export\songs_" & j & "_" & IOPX & ".xml", Library_path.Text & "\SONGS\" & Library_ListBox.SelectedItems.Item(i).ToString & "\songs.xml", True, 1)
                    If i = Library_ListBox.SelectedItems.Count - 1 Then
                        Singstar_Tools.Create_songs_XML(Output_path.Text & "\PACK_EE\export\songs_" & j & "_" & 0 & ".xml", Library_path.Text & "\SONGS\" & Library_ListBox.SelectedItems.Item(i).ToString & "\songs.xml", True, 2)
                        If i <> 0 And Song.IOPX = 1 Then
                            Singstar_Tools.Create_songs_XML(Output_path.Text & "\PACK_EE\export\songs_" & j & "_" & 1 & ".xml", Library_path.Text & "\SONGS\" & Library_ListBox.SelectedItems.Item(i).ToString & "\songs.xml", True, 2)
                        End If
                    End If

                    j = j + 1
                Loop

                'Creating SonglistX.xml
                j = 3
                Do While (j <> Song.Version + 1)
                    If i = 0 Then
                        If j = 3 Then
                            Singstar_Tools.Create_songlist_XML(Output_path.Text & "\PACK_EE\export\songlists_" & j & ".xml", Song.ID, Duet, False, 0)
                        Else
                            Singstar_Tools.Create_songlist_XML(Output_path.Text & "\PACK_EE\export\songlists_" & j & ".xml", Song.ID, Duet, True, 0)
                        End If
                    End If
                    Singstar_Tools.Create_songlist_XML(Output_path.Text & "\PACK_EE\export\songlists_" & j & ".xml", Song.ID, Duet, True, 1)
                    If i = Library_ListBox.SelectedItems.Count - 1 Then
                        Singstar_Tools.Create_songlist_XML(Output_path.Text & "\PACK_EE\export\songlists_" & j & ".xml", Song.ID, Duet, True, 2)
                    End If

                    j = j + 1
                Loop

                'Creating ID folders for XML per song
                System.IO.Directory.CreateDirectory(Output_path.Text & "\PACK_EE\export\" & Song.ID)
                Dim DirSongsXML As New IO.DirectoryInfo(Library_path.Text & "\SONGS\" & Library_ListBox.SelectedItems.Item(i).ToString & "\PACK_EE")
                Dim AnyDirSongsXMLFiles As IO.FileInfo() = DirSongsXML.GetFiles("*")
                Dim DirFileXML As IO.FileInfo
                For Each DirFileXML In AnyDirSongsXMLFiles
                    IO.File.Copy(Library_path.Text & "\SONGS\" & Library_ListBox.SelectedItems.Item(i).ToString & "\PACK_EE\" & DirFileXML.ToString, Output_path.Text & "\PACK_EE\export\" & Song.ID & "\" & DirFileXML.ToString, True)
                Next

                'Copying Files to IOPX directory
                System.IO.Directory.CreateDirectory(Output_path.Text & "\PAK_IOP" & IOPX & "\" & Song.ID)

                Dim DirSongs As New IO.DirectoryInfo(Library_path.Text & "\SONGS\" & Library_ListBox.SelectedItems.Item(i).ToString & "\PAK_IOPX")
                Dim AnyDirSongsFiles As IO.FileInfo() = DirSongs.GetFiles("*")
                Dim DirFile As IO.FileInfo
                For Each DirFile In AnyDirSongsFiles
                    IO.File.Copy(Library_path.Text & "\SONGS\" & Library_ListBox.SelectedItems.Item(i).ToString & "\PAK_IOPX\" & DirFile.ToString, Output_path.Text & "\PAK_IOP" & IOPX & "\" & Song.ID & "\" & DirFile.ToString, True)
                Next

            End If
            i = i + 1
            If Song.IOPX = 1 Then
                If IOPX = 0 Then
                    IOPX = 1
                Else
                    IOPX = 0
                End If
            End If
        Loop
        If i Mod 3 <> 0 Then
            Convert_BMPTOTX2(Output_path.Text & "\PACK_EE\export\textures\page_" & (i - 1) \ 3 & ".bmp", Output_path.Text & "\PACK_EE\export\textures\page_" & (i - 1) \ 3 & ".tx2")
            IO.File.Delete(Output_path.Text & "\PACK_EE\export\textures\page_" & (i - 1) \ 3 & ".bmp")
        End If

        Logs.SelectedIndex = Logs.Items.Count - 1

        Logs.Items.Add("Packing PACK_EE.PAK...")
        Logs.SelectedIndex = Logs.Items.Count - 1
        Application.DoEvents()
        SceeWhPx_Create_Dll(Output_path.Text & "\PACK_EE.PAK", Output_path.Text & "\PACK_EE\", 1)
        Logs.Items.Add("Packing PAK_IOP0.PAK...")
        Logs.SelectedIndex = Logs.Items.Count - 1
        Application.DoEvents()
        SceeWhPx_Create_Dll(Output_path.Text & "\PAK_IOP0.PAK", Output_path.Text & "\PAK_IOP0", 0)
        If Song.IOPX = 1 Then
            Logs.Items.Add("Packing PAK_IOP1.PAK...")
            Logs.SelectedIndex = Logs.Items.Count - 1
            Application.DoEvents()
            SceeWhPx_Create_Dll(Output_path.Text & "\PAK_IOP1.PAK", Output_path.Text & "\PAK_IOP1", 0)
        End If
        Logs.Items.Add("Packing PAK_IOPM.PAK...")
        Logs.SelectedIndex = Logs.Items.Count - 1
        Application.DoEvents()
        SceeWhPx_Create_Dll(Output_path.Text & "\PAK_IOPM.PAK", Output_path.Text & "\PAK_IOPM", 0)
        Logs.Items.Add("Done...")
        Logs.SelectedIndex = Logs.Items.Count - 1
        Application.DoEvents()
    End Sub

    Private Sub Extract_Core_Files_Button(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        SingstarDVD.Text = "I:\"

        Logs.Items.Add("Extracting Core files from inserted Singstar DVD")
        Logs.SelectedIndex = Logs.Items.Count - 1
        Logs.Items.Add("PACK_EE...")
        Logs.SelectedIndex = Logs.Items.Count - 1
        Application.DoEvents()
        If SceeWhPx_File_Extract_All_Dll(SingstarDVD.Text & "PACK_EE.PAK", Output_path.Text & "\PACK_EE") = -1 Then
            Logs.Items.Add("Error")
            Logs.SelectedIndex = Logs.Items.Count - 1
            Exit Sub
        End If

        Logs.Items.Add("PAK_IOPM...")
        Logs.SelectedIndex = Logs.Items.Count - 1
        Application.DoEvents()
        If SceeWhPx_File_Extract_All_Dll(SingstarDVD.Text & "PAK_IOPM.PAK", Output_path.Text & "\PAK_IOPM") = -1 Then
            Logs.Items.Add("Error")
            Logs.SelectedIndex = Logs.Items.Count - 1
            Exit Sub
        End If
        Logs.Items.Add("Done")
    End Sub

    Private Sub Library_ListBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Library_ListBox.SelectedIndexChanged
        Label5.Text = "Number of Songs: " & Library_ListBox.SelectedItems.Count
    End Sub

    Private Sub Logs_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Logs.SelectedIndexChanged

    End Sub
End Class

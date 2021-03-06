
Structure PAK_Infos
    Public PAK_File As String
    Public err As Integer
    Public readed_TOC As Integer
    Public format As String
    Public date_str As String
    Public time As String
    Public hdrsize As Long
    Public nbfiles As Long
    Public StrCompilePath As String
    Public extlist() As String
End Structure 'MyStruct 

Structure PAK_Filelist
    Public offset As Long
    Public filename As String
    Public filename_len As Integer
    Public reduced_filename As String
    Public reduced_filename_len As Integer
    Public previous_letters As Integer
    Public header_len As Integer
    Public unknown As Integer
    Public size As Long
    Public checksum As Long
    Public extension As Integer
    Public increased_header_len As Long
End Structure 'MyStruct 

Structure PAK_Toc
    Public PAKinfos As PAK_Infos
    Public PAKFilelist() As PAK_Filelist
End Structure

Structure SongDetails
    Public Version As String
    Public xml_file As String
    Public File As Integer
    Public IOPX As Integer
    Public ID As String
    Public ID_Artist As String
    Public Title As String
    Public Artist As String
    Public Nbrofsongs As Integer
End Structure

Public Class Singstar_Tools
    Public Shared Function Delete_Useless_Files(ByVal output_path As String) As Integer
        Dim Dir As New IO.DirectoryInfo(output_path & "\PACK_EE\export\")
        If (Dir.Attributes = -1) Then
            Return -1
        End If

        Dim AnyDir As IO.DirectoryInfo() = Dir.GetDirectories("*")
        Dim AnyDirFiles As IO.FileInfo() = Dir.GetFiles("*")
        Dim DirFile As IO.FileInfo
        Dim DirName As IO.DirectoryInfo

        For Each DirName In AnyDir
            IO.Directory.Delete(output_path & "\PACK_EE\export\" & DirName.ToString, True)
        Next

        For Each DirFile In AnyDirFiles
            If (InStr(DirFile.ToString, "song") <> 0) Then
                IO.File.Delete(output_path & "\PACK_EE\export\" & DirFile.ToString)
            End If
            If (InStr(DirFile.ToString, "acts") <> 0) Then
                IO.File.Delete(output_path & "\PACK_EE\export\" & DirFile.ToString)
            End If
            If (InStr(DirFile.ToString, "covers.xml") <> 0) Then
                IO.File.Delete(output_path & "\PACK_EE\export\" & DirFile.ToString)
            End If
        Next
        Return 0
    End Function

    Public Shared Function Create_acts_XML(ByVal filename As String, ByVal ID As String, ByVal Artist As String, ByVal UTF8 As Boolean, ByVal mode As Integer) As Integer
        If mode = 0 Then
            Dim objReader As New System.IO.StreamWriter(filename, False)
            If UTF8 = False Then
                objReader.WriteLine("<?xml version=""1.0"" encoding=""ISO-8859-1""?>")
                objReader.WriteLine("<ACT_SET xmlns=""http://www.singstargame.com"" xmlns:ss=""http://www.singstargame.com"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:schemaLocation=""http://www.singstargame.com http://bri-sstar/xml_schema/acts.xsd"">")
            Else
                objReader.WriteLine("<?xml version=""1.0"" encoding=""UTF-8""?>")
                objReader.WriteLine("<ACT_SET xmlns=""http://www.singstargame.com"" xmlns:ss=""http://www.singstargame.com"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:schemaLocation=""http://www.singstargame.com http://bri-sstar/xml_schema/acts.xsd"">")
            End If
            objReader.Close()
        End If

        If mode = 1 Then
            Dim objReader As New System.IO.StreamWriter(filename, True)
            objReader.WriteLine("<ACT ID=""" & ID & """ NAME=""" & Artist & """>")
            objReader.WriteLine("<NAME_LEGACY>" & Artist & "</NAME_LEGACY>")
            objReader.WriteLine("</ACT>")
            objReader.Close()
        End If

        If mode = 2 Then
            Dim objReader As New System.IO.StreamWriter(filename, True)
            objReader.WriteLine("</ACT_SET>")
            objReader.Close()
        End If

        Return 0
    End Function

    Public Shared Function Create_covers_XML(ByVal filename As String, ByVal ID As String, ByVal page As Integer, ByVal picpos As Integer, ByVal UTF8 As Boolean, ByVal mode As Integer) As Integer
        If mode = 0 Then
            Dim objReader As New System.IO.StreamWriter(filename, False)
            If UTF8 = False Then
                objReader.WriteLine("<?xml version=""1.0"" encoding=""ISO-8859-1""?>")
                objReader.WriteLine("<TPAGE_BIT_SET xmlns=""http://www.singstargame.com"" xmlns:ss=""http://www.singstargame.com"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:schemaLocation=""http://www.singstargame.com http://bri-sstar/xml_schema/covers.xsd"">")
            Else
                objReader.WriteLine("<?xml version=""1.0"" encoding=""UTF-8""?>")
                objReader.WriteLine("<TPAGE_BIT_SET xmlns=""http://www.singstargame.com"" xmlns:ss=""http://www.singstargame.com"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:schemaLocation=""http://www.singstargame.com http://bri-sstar/xml_schema/covers.xsd"">")
            End If
            objReader.Close()
        End If

        If mode = 1 Then
            Dim objReader As New System.IO.StreamWriter(filename, True)
            If picpos = 0 Then
                objReader.WriteLine("<TPAGE_BIT NAME=""cover_" & ID & """ TEXTURE=""page_" & page & """ U=""0"" V=""0"" WIDTH=""170"" HEIGHT=""170""></TPAGE_BIT>")
            End If
            If picpos = 1 Then
                objReader.WriteLine("<TPAGE_BIT NAME=""cover_" & ID & """ TEXTURE=""page_" & page & """ U=""170"" V=""0"" WIDTH=""170"" HEIGHT=""170""></TPAGE_BIT>")
            End If
            If picpos = 2 Then
                objReader.WriteLine("<TPAGE_BIT NAME=""cover_" & ID & """ TEXTURE=""page_" & page & """ U=""340"" V=""0"" WIDTH=""170"" HEIGHT=""170""></TPAGE_BIT>")
            End If
            objReader.Close()
        End If

        If mode = 2 Then
            Dim objReader As New System.IO.StreamWriter(filename, True)
            objReader.WriteLine("</TPAGE_BIT_SET>")
            objReader.Close()
        End If

        Return 0
    End Function

    Public Shared Function Create_songs_XML(ByVal filename As String, ByVal source As String, ByVal UTF8 As Boolean, ByVal mode As Integer) As Integer
        If mode = 0 Then
            Dim objReader As New System.IO.StreamWriter(filename, False)
            If UTF8 = False Then
                objReader.WriteLine("<?xml version=""1.0"" encoding=""ISO-8859-1""?>")
                objReader.WriteLine("<SONG_SET xmlns=""http://www.singstargame.com"" xmlns:ss=""http://www.singstargame.com"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:schemaLocation=""http://www.singstargame.com http://bri-sstar/xml_schema/songs.xsd"">")
            Else
                objReader.WriteLine("<?xml version=""1.0"" encoding=""UTF-8""?>")
                objReader.WriteLine("<SONG_SET xmlns=""http://www.singstargame.com"" xmlns:ss=""http://www.singstargame.com"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:schemaLocation=""http://www.singstargame.com http://bri-sstar/xml_schema/songs.xsd"">")
            End If
            objReader.Close()
        End If

        If mode = 1 Then
            Dim TempString As String
            Dim objReader As New System.IO.StreamReader(source)
            Dim objWriter As New System.IO.StreamWriter(filename, True)
            While objReader.Peek <> -1
                TempString = objReader.ReadLine
                objWriter.WriteLine(TempString)
            End While
            objReader.Close()
            objWriter.Close()
        End If

        If mode = 2 Then
            Dim objReader As New System.IO.StreamWriter(filename, True)
            objReader.WriteLine("</SONG_SET>")
            objReader.Close()
        End If

        Return 0
    End Function

    Public Shared Function Create_songlist_XML(ByVal filename As String, ByVal ID As String, ByVal Duet As String, ByVal UTF8 As Boolean, ByVal mode As Integer) As Integer
        If mode = 0 Then
            Dim objReader As New System.IO.StreamWriter(filename, False)
            If UTF8 = False Then
                objReader.WriteLine("<?xml version=""1.0"" encoding=""ISO-8859-1""?>")
            Else
                objReader.WriteLine("<?xml version=""1.0"" encoding=""UTF-8""?>")
            End If
            objReader.WriteLine("<SUBSETS xmlns=""http://www.singstargame.com"" xmlns:ss=""http://www.singstargame.com"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:schemaLocation=""http://www.singstargame.com http://bri-sstar/xml_schema/subsets.xsd"">")
            objReader.WriteLine("<GROUP Name=""Root"">")
            objReader.WriteLine("<SUBSET Name=""DefaultSong"">")
            objReader.WriteLine("<Name_Legacy>DefaultSong</Name_Legacy>")
            objReader.WriteLine("<SONG_REF ID=""" & ID & """></SONG_REF>")
            objReader.WriteLine("</SUBSET>")
            objReader.WriteLine("<SUBSET Name=""DefaultDuetSong"">")
            objReader.WriteLine("<Name_Legacy>DefaultDuetSong</Name_Legacy>")
            objReader.WriteLine("<SONG_REF ID=""" & Duet & """></SONG_REF>")
            objReader.WriteLine("</SUBSET>")
            objReader.WriteLine("<SUBSET Name=""Carousel"">")
            objReader.WriteLine("<Name_Legacy>Carousel</Name_Legacy>")
            objReader.Close()
        End If

        If mode = 1 Then
            Dim objReader As New System.IO.StreamWriter(filename, True)
            objReader.WriteLine("<SONG_REF ID=""" & ID & """></SONG_REF>")
            objReader.Close()
        End If

        If mode = 2 Then
            Dim objReader As New System.IO.StreamWriter(filename, True)
            objReader.WriteLine("</SUBSET>")
            objReader.WriteLine("</GROUP>")
            objReader.WriteLine("</SUBSETS>")
            objReader.Close()
        End If

        Return 0
    End Function
End Class
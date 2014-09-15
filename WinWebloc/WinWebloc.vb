Imports System.IO
Imports System.Text.RegularExpressions

Module WinWebloc
    Sub Main()
        If My.Application.CommandLineArgs.Count = 0 Then
            Console.WriteLine("No File dragged into the application's executable!")
        Else
            Dim filename = My.Application.CommandLineArgs.Item(0)
            Dim s = GetFileContents(filename)
            Dim url As String = ""
            Try
                If InStr(s, "<?xml") > 0 Then
                    'new text style
                    Dim xpos = InStr(InStr(s, "<dict>"), s, "http")
                    If xpos > 0 Then
                        s = Right(s, Len(s) - xpos + 1)
                    End If
                Else
                    'old bin style
                    Dim xpos = InStr(s, "http")
                    If xpos > 0 Then
                        s = Right(s, Len(s) - xpos + 1)
                    End If
                    s = Regex.Replace(s, "[\x01-\x1F]", "")
                End If
                For i = 1 To s.Length
                    Dim c As Char = Mid(s, i, 1)
                    If Convert.ToInt32(c) < 32 Or c = "<" Then
                        Exit For
                    End If
                    url &= c
                Next
                If url.Length > 0 Then Process.Start(url)
            Catch ex As Exception
                Console.WriteLine(ex.ToString)
            End Try
        End If
    End Sub


    Private Function GetFileContents(ByVal filename As String) As String
        Dim contents As String
        Dim reader As StreamReader
        Try
            reader = New StreamReader(filename)
            contents = reader.ReadToEnd
            reader.Close()
            Return contents
        Catch ex As Exception
            Return ""
        End Try
    End Function

End Module

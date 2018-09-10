Sub Create()
    Set fsT = CreateObject("ADODB.Stream")
    fsT.Type = 2
    fsT.Charset = "utf-8"
    fsT.Open
    'Open ThisWorkbook.Path & "\deletequery.sql" For Output As #1
    For i = 6 To 118
        If Not Trim(Sheet6.Cells(i, 2).Value) = "" Then
            Dim buffer As String
            buffer = ""
            buffer = buffer + "INSERT INTO CMCODE(group_cd,code,value,priority,p_group_cd,p_code,attribute_int,attribute_str,is_use,description,remark,ix) VALUES("
            
            buffer = buffer + "'" + Trim(Sheet6.Cells(i, 2).Value) + "',"
            
            buffer = buffer + "'" + Trim(Sheet6.Cells(i, 3).Value) + "',"
            
            buffer = buffer + "'" + Trim(Sheet6.Cells(i, 4).Value) + "',"
            
            buffer = buffer + "" + Trim(Sheet6.Cells(i, 5).Value) + ","
            
            If Trim(Sheet6.Cells(i, 6).Value) = "" Then
                buffer = buffer + "null,"
            Else
                buffer = buffer + "'" + Trim(Sheet6.Cells(i, 6).Value) + "',"
            End If
            
            If Trim(Sheet6.Cells(i, 7).Value) = "" Then
                buffer = buffer + "null,"
            Else
                buffer = buffer + "'" + Trim(Sheet6.Cells(i, 7).Value) + "',"
            End If
            
            If Trim(Sheet6.Cells(i, 8).Value) = "" Then
                buffer = buffer + "null,"
            Else
                buffer = buffer + Trim(Sheet6.Cells(i, 8).Value) + ","
            End If
            
            If Trim(Sheet6.Cells(i, 9).Value) = "" Then
                buffer = buffer + "null,"
            Else
                buffer = buffer + "'" + Trim(Sheet6.Cells(i, 9).Value) + "',"
            End If
            
            buffer = buffer + "" + Trim(Sheet6.Cells(i, 10).Value) + ","
            
            If Trim(Sheet6.Cells(i, 11).Value) = "" Then
                buffer = buffer + "null,"
            Else
                buffer = buffer + "'" + Trim(Sheet6.Cells(i, 11).Value) + "',"
            End If
            
            If Trim(Sheet6.Cells(i, 12).Value) = "" Then
                buffer = buffer + "null,null);"
            Else
                buffer = buffer + "'" + Trim(Sheet6.Cells(i, 12).Value) + "',null);"
            End If
            
            fsT.WriteText buffer + Chr(13)
            'Print #1, buffer
        End If
    Next
    'Close #1
    fsT.SaveToFile ThisWorkbook.Path & "\deletequery.sql", 2
End Sub






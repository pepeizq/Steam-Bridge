Module Powershell

    Public Function Contenido(ejecutable As String, argumentos As String)

        Dim lineas As String = Nothing

        lineas = lineas + "@echo off" + Environment.NewLine
        lineas = lineas + "powershell.exe " + ChrW(34) + "Start-Process " + ejecutable + ChrW(34)

        Return lineas
    End Function

End Module

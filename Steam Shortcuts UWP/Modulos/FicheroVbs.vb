Module FicheroVbs

    Public Function Contenido(ejecutable As String, argumentos As String)

        Dim lineas As String = Nothing

        lineas = lineas + "Launcher = " + ChrW(34) + argumentos + ChrW(34) + Environment.NewLine
        lineas = lineas + "Client = " + ChrW(34) + ejecutable + ChrW(34) + Environment.NewLine

        lineas = lineas + Environment.NewLine
        lineas = lineas + "If WScript.Arguments.length = 0 Then" + Environment.NewLine
        lineas = lineas + "Home = WScript.ScriptFullName" + Environment.NewLine
        lineas = lineas + "Home = Left(Home, InStr(Home, WScript.ScriptName)-1)" + Environment.NewLine

        lineas = lineas + Environment.NewLine
        lineas = lineas + "Set objShell = WScript.CreateObject(" + ChrW(34) + "Shell.Application" + ChrW(34) + ")" + Environment.NewLine
        lineas = lineas + "objShell.ShellExecute Launcher, " + ChrW(34) + ChrW(34) + ", Home" + Environment.NewLine

        lineas = lineas + Environment.NewLine
        lineas = lineas + "set fso = CreateObject(" + ChrW(34) + "Scripting.FileSystemObject" + ChrW(34) + ")" + Environment.NewLine
        lineas = lineas + "set tempfolder = fso.GetSpecialFolder(2)" + Environment.NewLine
        lineas = lineas + "tempname = tempfolder & " + ChrW(34) + "\" + ChrW(34) + " & " + ChrW(34) + "steam.tmp" + ChrW(34) + Environment.NewLine
        lineas = lineas + "set tempfile = fso.CreateTextFile(tempname)" + Environment.NewLine
        lineas = lineas + "tempfile.close()" + Environment.NewLine

        lineas = lineas + Environment.NewLine
        lineas = lineas + "Set objShell = CreateObject(" + ChrW(34) + "Shell.Application" + ChrW(34) + ")" + Environment.NewLine
        lineas = lineas + "objShell.ShellExecute " + ChrW(34) + "cscript.exe" + ChrW(34) + ", Chr(34) & WScript.ScriptFullName & Chr(34) & " + ChrW(34) + " " + ChrW(34) + " & tempname, " + ChrW(34) + ChrW(34) + ", " + ChrW(34) + "runas" + ChrW(34) + ", 2" + Environment.NewLine

        lineas = lineas + Environment.NewLine
        lineas = lineas + "Do While True" + Environment.NewLine
        lineas = lineas + "set tempfile = fso.GetFile(tempname)" + Environment.NewLine
        lineas = lineas + "If tempfile.Size > 0 Then Exit Do" + Environment.NewLine
        lineas = lineas + "WScript.Sleep 1000" + Environment.NewLine
        lineas = lineas + "Loop" + Environment.NewLine

        lineas = lineas + Environment.NewLine
        lineas = lineas + "ExecutablePath = Home" + Environment.NewLine
        lineas = lineas + "set tempfile = fso.OpenTextFile(tempname)" + Environment.NewLine
        lineas = lineas + "CommandLine = tempfile.ReadLine" + Environment.NewLine
        lineas = lineas + "tempfile.close()" + Environment.NewLine

        lineas = lineas + Environment.NewLine
        lineas = lineas + "Set objShell = CreateObject(" + ChrW(34) + "Shell.Application" + ChrW(34) + ")" + Environment.NewLine
        lineas = lineas + "objShell.ShellExecute Client, CommandLine, ExecutablePath" + Environment.NewLine

        lineas = lineas + Environment.NewLine
        lineas = lineas + "fso.DeleteFile tempname" + Environment.NewLine
        lineas = lineas + "WScript.Quit" + Environment.NewLine

        lineas = lineas + Environment.NewLine
        lineas = lineas + "else" + Environment.NewLine

        lineas = lineas + Environment.NewLine
        lineas = lineas + "Set objWMIService = GetObject(" + ChrW(34) + "winmgmts:" + ChrW(34) + " & " + ChrW(34) + "{impersonationLevel=impersonate}!\\.\root\cimv2" + ChrW(34) + ")" + Environment.NewLine

        lineas = lineas + Environment.NewLine
        lineas = lineas + "While True" + Environment.NewLine
        lineas = lineas + "Set InstanceList = objWMIService.ExecQuery(" + ChrW(34) + "Select * from Win32_Process Where Name = '" + ChrW(34) + " & Client & " + ChrW(34) + "'" + ChrW(34) + ")" + Environment.NewLine

        lineas = lineas + Environment.NewLine
        lineas = lineas + "for Each Instance in InstanceList" + Environment.NewLine
        lineas = lineas + "cmdline = Instance.CommandLine" + Environment.NewLine
        lineas = lineas + "Instance.Terminate()" + Environment.NewLine

        lineas = lineas + Environment.NewLine
        lineas = lineas + "position = InStr(1, cmdline, " + ChrW(34) + ChrW(34) + ChrW(34) + " " + ChrW(34) + ") + 1" + Environment.NewLine
        lineas = lineas + "cleanCmdLine = Right(cmdline, Len(cmdline) - position)" + Environment.NewLine

        lineas = lineas + Environment.NewLine
        lineas = lineas + "tempname = WScript.Arguments(0)" + Environment.NewLine
        lineas = lineas + "set fso = CreateObject(" + ChrW(34) + "Scripting.FileSystemObject" + ChrW(34) + ")" + Environment.NewLine
        lineas = lineas + "set tempfile = fso.OpenTextFile(tempname, 2)" + Environment.NewLine
        lineas = lineas + "tempfile.WriteLine(cleanCmdLine)" + Environment.NewLine
        lineas = lineas + "tempfile.close()" + Environment.NewLine

        lineas = lineas + Environment.NewLine
        lineas = lineas + "WScript.Quit" + Environment.NewLine
        lineas = lineas + "Next" + Environment.NewLine

        lineas = lineas + Environment.NewLine
        lineas = lineas + "WScript.Sleep 1000" + Environment.NewLine
        lineas = lineas + "Wend" + Environment.NewLine
        lineas = lineas + "End If" + Environment.NewLine

        Return lineas
    End Function

End Module

Module Registro

    Public Sub Mensaje(tb As TextBox, categoria As String, mensaje As String)

        Dim minutos As String = DateTime.Now.Minute.ToString

        If minutos.Length = 1 Then
            minutos = "0" + minutos
        End If

        Dim segundos As String = DateTime.Now.Second.ToString

        If segundos.Length = 1 Then
            segundos = "0" + segundos
        End If

        tb.Text = tb.Text + DateTime.Now.Hour.ToString + ":" + minutos + ":" + segundos + " - " + categoria + " - " + mensaje + Environment.NewLine

    End Sub

End Module

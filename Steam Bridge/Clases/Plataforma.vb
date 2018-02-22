Public Class Plataforma

    Public Property Nombre As String
    Public Property Imagen As String
    Public Property Acceso As Boolean

    Public Sub New(ByVal nombre As String, ByVal imagen As String, ByVal acceso As Boolean)
        Me.Nombre = nombre
        Me.Imagen = imagen
        Me.Acceso = acceso
    End Sub

End Class

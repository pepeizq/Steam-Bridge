Imports Windows.Storage

Public Class Juego

    Public Property Nombre As String
    Public Property Ejecutable As String
    Public Property Argumentos As String
    Public Property Icono As StorageFile
    Public Property ColorFondo As String
    Public Property Añadir As Boolean
    Public Property Categoria As String

    Public Sub New(ByVal nombre As String, ByVal ejecutable As String, ByVal argumentos As String,
                   ByVal icono As StorageFile, ByVal colorfondo As String, ByVal añadir As Boolean,
                   ByVal categoria As String)
        Me.Nombre = nombre
        Me.Ejecutable = ejecutable
        Me.Argumentos = argumentos
        Me.Icono = icono
        Me.ColorFondo = colorfondo
        Me.Añadir = añadir
        Me.Categoria = categoria
    End Sub

End Class

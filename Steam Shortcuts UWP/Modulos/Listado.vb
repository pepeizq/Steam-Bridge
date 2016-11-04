﻿Imports System.Globalization
Imports Windows.UI

Module Listado

    Public Function GenerarGrid(juego As Juego, bitmap As BitmapImage)

        Dim grid As New Grid

        Dim col1 As New ColumnDefinition
        Dim col2 As New ColumnDefinition
        Dim col3 As New ColumnDefinition

        col1.Width = New GridLength(1, GridUnitType.Auto)
        col2.Width = New GridLength(1, GridUnitType.Auto)
        col3.Width = New GridLength(1, GridUnitType.Auto)

        grid.ColumnDefinitions.Add(col1)
        grid.ColumnDefinitions.Add(col2)
        grid.ColumnDefinitions.Add(col3)

        grid.Margin = New Thickness(5, 5, 5, 5)

        '----------------------------------------------------

        Dim checkBox As New CheckBox
        checkBox.VerticalAlignment = VerticalAlignment.Center
        checkBox.Tag = juego
        checkBox.Margin = New Thickness(10, 0, 10, 0)
        checkBox.MinWidth = 20
        checkBox.Name = "cbJuego"

        AddHandler checkBox.Checked, AddressOf cbChecked
        AddHandler checkBox.Unchecked, AddressOf cbUnChecked

        Grid.SetColumn(checkBox, 0)
        grid.Children.Add(checkBox)

        '----------------------------------------------------

        Dim borde As New Border
        Dim imagen As New Image

        If Not bitmap Is Nothing Then
            If Not juego.ColorFondo = Nothing Then
                Dim hex As String = juego.ColorFondo

                If Not hex = Nothing Then
                    If hex = "white" Then
                        hex = "#ffffff"
                    End If

                    hex = hex.Replace("#", Nothing)

                    Try
                        Dim byteA As Byte = Byte.Parse(255)
                        Dim byteR As Byte = Byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber)
                        Dim byteG As Byte = Byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber)
                        Dim byteB As Byte = Byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber)

                        borde.Background = New SolidColorBrush(Color.FromArgb(byteA, byteR, byteG, byteB))
                    Catch ex As Exception
                        borde.Background = New SolidColorBrush(Colors.RoyalBlue)
                    End Try
                Else
                    borde.Background = New SolidColorBrush(Colors.RoyalBlue)
                End If
            Else
                borde.Background = New SolidColorBrush(Colors.RoyalBlue)
            End If

            imagen.Source = bitmap

            imagen.Width = 40
            imagen.Height = 40

            borde.Width = 40
            borde.Height = 40
        Else
            borde.Width = 0
            borde.Height = 40
        End If

        borde.Child = imagen
        borde.Margin = New Thickness(10, 0, 10, 0)
        Grid.SetColumn(borde, 1)
        grid.Children.Add(borde)

        '----------------------------------------------------

        Dim textoBloque As New TextBlock
        textoBloque.Text = juego.Nombre
        textoBloque.VerticalAlignment = VerticalAlignment.Center
        textoBloque.Margin = New Thickness(10, 0, 0, 0)
        textoBloque.FontSize = 15

        Grid.SetColumn(textoBloque, 2)
        grid.Children.Add(textoBloque)

        Return grid
    End Function

    Private Sub cbChecked(ByVal sender As Object, ByVal e As RoutedEventArgs)

        Dim cb As CheckBox = e.OriginalSource
        Dim juegoCb As Juego = TryCast(cb.Tag, Juego)

        juegoCb.Añadir = True
        BotonCrearDisponible()

    End Sub

    Private Sub cbUnChecked(ByVal sender As Object, ByVal e As RoutedEventArgs)

        Dim cb As CheckBox = e.OriginalSource
        Dim juegoCb As Juego = TryCast(cb.Tag, Juego)

        juegoCb.Añadir = False
        BotonCrearDisponible()

    End Sub

    Private Sub BotonCrearDisponible()

        Dim frame As Frame = Window.Current.Content

        If Not frame Is Nothing Then
            Dim pagina As Page = frame.Content

            Dim botonDisponible As Boolean = False
            Dim boton As Button = pagina.FindName("buttonAñadirJuegos")

            Dim pivot As PivotItem = pagina.FindName("pivotItemWindowsStore")
            Dim listView As ListView = pivot.Content

            For Each grid As Grid In listView.Items

                Dim cb As IEnumerable(Of CheckBox) = grid.Children.OfType(Of CheckBox)

                Dim juego As Juego = TryCast(cb(0).Tag, Juego)

                If juego.Añadir = True Then
                    botonDisponible = True
                End If
            Next

            If botonDisponible = True Then
                boton.IsEnabled = True
            Else
                boton.IsEnabled = False
            End If
        End If
    End Sub

End Module
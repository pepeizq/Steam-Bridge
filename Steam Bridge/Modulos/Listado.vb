Imports System.Globalization
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Windows.Storage
Imports Windows.Storage.AccessCache
Imports Windows.UI

Module Listado

    Public Function GenerarGrid(juego As Juego, bitmap As BitmapImage, estado As Boolean)

        Dim grid As New Grid

        Dim col1 As New ColumnDefinition
        Dim col2 As New ColumnDefinition
        Dim col3 As New ColumnDefinition
        Dim col4 As New ColumnDefinition

        col1.Width = New GridLength(1, GridUnitType.Auto)
        col2.Width = New GridLength(1, GridUnitType.Auto)
        col3.Width = New GridLength(1, GridUnitType.Star)
        col4.Width = New GridLength(1, GridUnitType.Auto)

        grid.ColumnDefinitions.Add(col1)
        grid.ColumnDefinitions.Add(col2)
        grid.ColumnDefinitions.Add(col3)
        grid.ColumnDefinitions.Add(col4)

        grid.Margin = New Thickness(5, 5, 5, 5)

        '----------------------------------------------------

        Dim cb As New CheckBox With {
            .VerticalAlignment = VerticalAlignment.Center,
            .Tag = juego,
            .Margin = New Thickness(5, 0, 5, 0),
            .MinWidth = 20,
            .Name = "cbJuego",
            .IsHitTestVisible = False
        }

        Grid.SetColumn(cb, 0)
        grid.Children.Add(cb)

        '----------------------------------------------------

        Dim borde As New Border
        Dim imagen As ImageEx = Nothing

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
                        If juego.Categoria = "Microsoft Store" Then
                            borde.Background = New SolidColorBrush(Colors.RoyalBlue)
                        Else
                            borde.Background = New SolidColorBrush(Colors.Transparent)
                        End If
                    End Try
                Else
                    If juego.Categoria = "Microsoft Store" Then
                        borde.Background = New SolidColorBrush(Colors.RoyalBlue)
                    Else
                        borde.Background = New SolidColorBrush(Colors.Transparent)
                    End If
                End If
            Else
                If juego.Categoria = "Microsoft Store" Then
                    borde.Background = New SolidColorBrush(Colors.RoyalBlue)
                Else
                    borde.Background = New SolidColorBrush(Colors.Transparent)
                End If
            End If

            imagen = New ImageEx With {
                .Source = bitmap,
                .Width = 40,
                .Height = 40,
                .IsCacheEnabled = True
            }

            borde.Width = 40
            borde.Height = 40

            borde.Child = imagen
            borde.Margin = New Thickness(10, 0, 10, 0)
            Grid.SetColumn(borde, 1)

            grid.Children.Add(borde)
        End If

        '----------------------------------------------------

        Dim tituloTexto As New TextBlock With {
            .Text = juego.Nombre,
            .VerticalAlignment = VerticalAlignment.Center,
            .Margin = New Thickness(10, 0, 0, 0),
            .FontSize = 15
        }

        If imagen Is Nothing Then
            Grid.SetColumn(tituloTexto, 1)
        Else
            Grid.SetColumn(tituloTexto, 2)
        End If

        grid.Children.Add(tituloTexto)

        '----------------------------------------------------

        'Dim enlaceTexto As New TextBlock With {
        '    .Text = juego.Ejecutable + juego.Argumentos,
        '    .VerticalAlignment = VerticalAlignment.Center,
        '    .FontStyle = Text.FontStyle.Italic,
        '    .Margin = New Thickness(10, 0, 10, 0),
        '    .FontSize = 15
        '}

        'Grid.SetColumn(enlaceTexto, 3)
        'grid.Children.Add(enlaceTexto)

        Return grid

    End Function

    Public Async Sub Clickeo(ByVal sender As Object, ByVal e As ItemClickEventArgs)

        Dim grid As Grid = e.ClickedItem
        Dim cb As CheckBox = grid.Children(0)
        Dim juegoCb As Juego = TryCast(cb.Tag, Juego)

        If cb.IsChecked = False Then
            cb.IsChecked = True
            juegoCb.Añadir = True
        Else
            cb.IsChecked = False
            juegoCb.Añadir = False
        End If

        Dim frame As Frame = Window.Current.Content

        If Not frame Is Nothing Then
            Dim pagina As Page = frame.Content

            Dim botonDisponible As Boolean = False
            Dim boton As Button = pagina.FindName("botonAñadirJuegos")

            Dim lvJuegos As ListView = pagina.FindName("lvPlataformaJuegos")

            If Not lvJuegos Is Nothing Then
                For Each subgrid As Grid In lvJuegos.Items
                    Dim subcb As IEnumerable(Of CheckBox) = subgrid.Children.OfType(Of CheckBox)
                    Dim juego As Juego = TryCast(subcb(0).Tag, Juego)

                    If juego.Añadir = True Then
                        botonDisponible = True
                    End If
                Next
            End If

            Dim carpetaSteam As StorageFolder = Nothing

            Try
                carpetaSteam = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("SteamPath")
            Catch ex As Exception

            End Try

            If carpetaSteam Is Nothing Then
                botonDisponible = False
            End If

            If botonDisponible = True Then
                boton.IsEnabled = True
            Else
                boton.IsEnabled = False
            End If
        End If

    End Sub

End Module

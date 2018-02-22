Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Windows.Storage
Imports Windows.Storage.AccessCache
Imports Windows.UI
Imports Windows.UI.Core

Module Interfaz

    Public Async Sub GenerarListado(gv As GridView)

        Dim carpetaBlizzard As StorageFolder = Nothing
        Dim activarBlizzard As Boolean = False

        Try
            carpetaBlizzard = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("BattlenetPath")
        Catch ex As Exception

        End Try

        If Not carpetaBlizzard Is Nothing Then
            activarBlizzard = True
        End If

        Dim blizzard As New Plataforma("Blizzard App", "Assets\blizzard_logo.png", activarBlizzard)

        gv.Items.Add(GenerarListadoItem(blizzard))

        '------------------------------------------------

        Dim microsoftStore As New Plataforma("Microsoft Store", "Assets\windowsstore_logo.png", True)

        gv.Items.Add(GenerarListadoItem(microsoftStore))

        '------------------------------------------------

        AddHandler gv.ItemClick, AddressOf GvItemClick

    End Sub

    Private Function GenerarListadoItem(plataforma As Plataforma)

        Dim item As New GridViewItem With {
            .Padding = New Thickness(0, 0, 0, 0),
            .Background = New SolidColorBrush(App.Current.Resources("ColorSecundario")),
            .BorderBrush = New SolidColorBrush(App.Current.Resources("ColorSecundario")),
            .BorderThickness = New Thickness(1, 1, 1, 1),
            .Margin = New Thickness(10, 10, 10, 10),
            .IsEnabled = plataforma.Acceso
        }

        Dim grid As New Grid With {
            .Tag = plataforma
        }

        Dim row1 As New RowDefinition
        Dim row2 As New RowDefinition

        row1.Height = New GridLength(1, GridUnitType.Star)
        row2.Height = New GridLength(1, GridUnitType.Auto)

        grid.RowDefinitions.Add(row1)
        grid.RowDefinitions.Add(row2)

        Dim imagen As New ImageEx With {
            .IsCacheEnabled = True,
            .Stretch = Stretch.UniformToFill,
            .HorizontalAlignment = HorizontalAlignment.Stretch,
            .VerticalAlignment = VerticalAlignment.Stretch,
            .Source = plataforma.Imagen,
            .Width = 200,
            .Height = 200
        }

        imagen.SetValue(Grid.RowProperty, 0)
        grid.Children.Add(imagen)

        Dim tb As New TextBlock With {
            .Foreground = New SolidColorBrush(Colors.White),
            .Margin = New Thickness(0, 10, 0, 10),
            .VerticalAlignment = VerticalAlignment.Center,
            .HorizontalAlignment = HorizontalAlignment.Center,
            .Text = plataforma.Nombre
        }

        tb.SetValue(Grid.RowProperty, 1)
        grid.Children.Add(tb)

        item.Content = grid

        AddHandler item.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler item.PointerExited, AddressOf UsuarioSaleBoton

        Return item

    End Function

    Private Async Sub GvItemClick(sender As Object, e As ItemClickEventArgs)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim item As Grid = e.ClickedItem
        Dim plataforma As Plataforma = item.Tag

        Dim tbTitulo As TextBlock = pagina.FindName("tbTitulo")
        tbTitulo.Text = Package.Current.DisplayName + " (" + Package.Current.Id.Version.Major.ToString + "." + Package.Current.Id.Version.Minor.ToString + "." + Package.Current.Id.Version.Build.ToString + "." + Package.Current.Id.Version.Revision.ToString + ") - " + plataforma.Nombre

        Dim gridPlataformaElegida As Grid = pagina.FindName("gridPlataformaElegida")
        gridPlataformaElegida.Visibility = Visibility.Visible

        Dim tbPlataformaSeleccionada As TextBlock = pagina.FindName("tbPlataformaSeleccionada")
        tbPlataformaSeleccionada.Text = plataforma.Nombre

        Dim carpetaSteam As StorageFolder = Nothing

        Try
            carpetaSteam = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("SteamPath")
        Catch ex As Exception

        End Try

        Dim gridAviso As Grid = pagina.FindName("gridAvisoSteam")

        If Not carpetaSteam Is Nothing Then
            gridAviso.Visibility = Visibility.Collapsed
        Else
            gridAviso.Visibility = Visibility.Visible
        End If


        Dim pbJuegos As ProgressBar = pagina.FindName("pbPlataformaJuegos")
        Dim lvJuegos As ListView = pagina.FindName("lvPlataformaJuegos")
        lvJuegos.Items.Clear()

        If plataforma.Nombre = "Blizzard App" Then
            Blizzard.Generar(pbJuegos, lvJuegos)
        ElseIf plataforma.Nombre = "Microsoft Store" Then
            WindowsStore.Generar2(pbJuegos, lvJuegos)
        End If


    End Sub

    Private Sub UsuarioEntraBoton(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

    End Sub

    Private Sub UsuarioSaleBoton(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

    End Sub

End Module

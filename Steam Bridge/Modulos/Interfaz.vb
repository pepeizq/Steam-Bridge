Imports Microsoft.Toolkit.Uwp.UI.Animations
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

        Dim carpetaGOG As StorageFolder = Nothing
        Dim activarGOG As Boolean = False

        Try
            carpetaGOG = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("GOGGalaxyPath")
        Catch ex As Exception

        End Try

        If Not carpetaGOG Is Nothing Then
            activarGOG = True
        End If

        Dim gogGalaxy As New Plataforma("GOG Galaxy", "Assets\gog_logo.png", activarGOG)

        gv.Items.Add(GenerarListadoItem(gogGalaxy))

        '------------------------------------------------

        Dim microsoftStore As New Plataforma("Microsoft Store", "Assets\windowsstore_logo.png", True)

        gv.Items.Add(GenerarListadoItem(microsoftStore))

        '------------------------------------------------

        Dim carpetaOrigin As StorageFolder = Nothing
        Dim activarOrigin As Boolean = False

        Try
            carpetaOrigin = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("OriginPath")
        Catch ex As Exception

        End Try

        If Not carpetaOrigin Is Nothing Then
            activarOrigin = True
        End If

        Dim origin As New Plataforma("Origin", "Assets\origin_logo.png", activarOrigin)

        gv.Items.Add(GenerarListadoItem(origin))

        '------------------------------------------------

        Dim carpetaTwitch As StorageFolder = Nothing
        Dim activarTwitch As Boolean = False

        Try
            carpetaTwitch = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("TwitchPath")
        Catch ex As Exception

        End Try

        If Not carpetaTwitch Is Nothing Then
            activarTwitch = True
        End If

        Dim twitch As New Plataforma("Twitch Desktop App", "Assets\twitch_logo.png", activarTwitch)

        gv.Items.Add(GenerarListadoItem(twitch))

        '------------------------------------------------

        Dim activarUplay As Boolean = False

        Dim carpetaUplayCliente As StorageFolder = Nothing

        Try
            carpetaUplayCliente = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("UplayPathCliente")
        Catch ex As Exception

        End Try

        If Not carpetaUplayCliente Is Nothing Then
            activarUplay = True
        End If

        Dim carpetaUplayJuegos As StorageFolder = Nothing

        Try
            carpetaUplayJuegos = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("UplayPathJuegos")
        Catch ex As Exception

        End Try

        If carpetaUplayJuegos Is Nothing Then
            activarUplay = False
        End If

        Dim uplay As New Plataforma("Uplay", "Assets\uplay_logo.png", activarUplay)

        gv.Items.Add(GenerarListadoItem(uplay))

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
        ElseIf plataforma.Nombre = "GOG Galaxy" Then
            GOGGalaxy.Generar(pbJuegos, lvJuegos)
        ElseIf plataforma.Nombre = "Microsoft Store" Then
            MicrosoftStore.Generar2(pbJuegos, lvJuegos)
        ElseIf plataforma.Nombre = "Origin" Then
            Origin.Generar(pbJuegos, lvJuegos)
        ElseIf plataforma.Nombre = "Twitch Desktop App" Then
            Twitch.Generar(pbJuegos, lvJuegos)
        ElseIf plataforma.Nombre = "Uplay" Then
            Uplay.Generar(pbJuegos, lvJuegos)
        End If

    End Sub

    Private Sub UsuarioEntraBoton(sender As Object, e As PointerRoutedEventArgs)

        Dim gvItem As GridViewItem = sender
        Dim grid As Grid = gvItem.Content
        Dim imagen As ImageEx = grid.Children(0)

        imagen.Saturation(0).Start()

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

    End Sub

    Private Sub UsuarioSaleBoton(sender As Object, e As PointerRoutedEventArgs)

        Dim gvItem As GridViewItem = sender
        Dim grid As Grid = gvItem.Content
        Dim imagen As ImageEx = grid.Children(0)

        imagen.Saturation(1).Start()

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

    End Sub

End Module

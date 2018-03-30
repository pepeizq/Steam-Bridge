Imports Windows.Storage
Imports Windows.Storage.AccessCache
Imports Windows.UI
Imports Windows.UI.Core

Public NotInheritable Class MainPage
    Inherits Page

    Private Sub Nv_Loaded(sender As Object, e As RoutedEventArgs)

        Dim recursos As New Resources.ResourceLoader()

        nvPrincipal.MenuItems.Add(NavigationViewItems.Generar(recursos.GetString("Bridge"), New SymbolIcon(Symbol.Home), 0))
        nvPrincipal.MenuItems.Add(New NavigationViewItemSeparator)
        nvPrincipal.MenuItems.Add(NavigationViewItems.Generar(recursos.GetString("Config"), New SymbolIcon(Symbol.Setting), 1))

    End Sub

    Private Sub Nv_ItemInvoked(sender As NavigationView, args As NavigationViewItemInvokedEventArgs)

        Dim recursos As New Resources.ResourceLoader()

        Dim item As TextBlock = args.InvokedItem

        If item.Text = recursos.GetString("Bridge") Then
            GridVisibilidad(gridBridge, item.Text)
        ElseIf item.Text = recursos.GetString("Config") Then
            GridVisibilidad(gridConfig, item.Text)
        End If

    End Sub

    Private Sub Page_Loaded(sender As FrameworkElement, args As Object)

        'Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "es-ES"
        'Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "en-US"

        MasCosas.Generar()

        Dim recursos As New Resources.ResourceLoader()

        GridVisibilidad(gridBridge, recursos.GetString("Bridge"))
        nvPrincipal.IsPaneOpen = False

        Interfaz.GenerarListado(gvBridge)
        Steam.Arranque(False)

        Dim opciones As ApplicationDataContainer = ApplicationData.Current.LocalSettings

        If opciones.Values("SteamOverlay") = Nothing Then
            opciones.Values("SteamOverlay") = 0
        End If

        cbSteamOverlay.SelectedIndex = opciones.Values("SteamOverlay")

        '--------------------------------------------------------

        Dim transpariencia As New UISettings
        TransparienciaEfectosFinal(transpariencia.AdvancedEffectsEnabled)
        AddHandler transpariencia.AdvancedEffectsEnabledChanged, AddressOf TransparienciaEfectosCambia

    End Sub

    Private Sub TransparienciaEfectosCambia(sender As UISettings, e As Object)

        TransparienciaEfectosFinal(sender.AdvancedEffectsEnabled)

    End Sub

    Private Async Sub TransparienciaEfectosFinal(estado As Boolean)

        Await Dispatcher.RunAsync(CoreDispatcherPriority.High, Sub()
                                                                   If estado = True Then
                                                                       gridPlataformaElegida.Background = App.Current.Resources("GridAcrilico")
                                                                       gridConfig.Background = App.Current.Resources("GridAcrilico")
                                                                       gridConfigBridge.Background = App.Current.Resources("GridTituloBackground")
                                                                       gridConfigOtherOptions.Background = App.Current.Resources("GridTituloBackground")
                                                                       gridMasCosas.Background = App.Current.Resources("GridAcrilico")
                                                                   Else
                                                                       gridPlataformaElegida.Background = New SolidColorBrush(Colors.LightGray)
                                                                       gridConfig.Background = New SolidColorBrush(Colors.LightGray)
                                                                       gridConfigBridge.Background = New SolidColorBrush(App.Current.Resources("ColorPrimario"))
                                                                       gridConfigOtherOptions.Background = New SolidColorBrush(App.Current.Resources("ColorPrimario"))
                                                                       gridMasCosas.Background = New SolidColorBrush(Colors.LightGray)
                                                                   End If
                                                               End Sub)

    End Sub

    Private Sub GridVisibilidad(grid As Grid, tag As String)

        tbTitulo.Text = Package.Current.DisplayName + " (" + Package.Current.Id.Version.Major.ToString + "." + Package.Current.Id.Version.Minor.ToString + "." + Package.Current.Id.Version.Build.ToString + "." + Package.Current.Id.Version.Revision.ToString + ") - " + tag

        gridPlataformaElegida.Visibility = Visibility.Collapsed
        gridConfig.Visibility = Visibility.Collapsed
        gridMasCosas.Visibility = Visibility.Collapsed

        grid.Visibility = Visibility.Visible

    End Sub

    Private Sub UsuarioEntraBoton(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

    End Sub

    Private Sub UsuarioSaleBoton(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

    End Sub

    'BRIDGE-----------------------------------------------------------------------------

    Private Sub LvPlataformaJuegos_ItemClick(sender As Object, e As ItemClickEventArgs) Handles lvPlataformaJuegos.ItemClick

        InterfazListado.Clickeo(sender, e)

    End Sub

    Private Async Sub BotonAñadirJuegos_Click(sender As Object, e As RoutedEventArgs) Handles botonAñadirJuegos.Click

        Dim recursos As New Resources.ResourceLoader()
        Dim carpetaSteam As StorageFolder = Nothing

        Try
            carpetaSteam = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("SteamPath")
        Catch ex As Exception

        End Try

        If Not carpetaSteam Is Nothing Then
            Dim listaFinal As New List(Of Juego)

            For Each grid As Grid In lvPlataformaJuegos.Items
                Dim cb As IEnumerable(Of CheckBox) = grid.Children.OfType(Of CheckBox)
                Dim juego As Juego = TryCast(cb(0).Tag, Juego)

                If juego.Añadir = True Then
                    listaFinal.Add(juego)
                End If
            Next

            If listaFinal.Count > 0 Then
                Steam.CrearAccesos(listaFinal, carpetaSteam, botonAñadirJuegos)
            End If
        End If

    End Sub

    'CONFIG-----------------------------------------

    Private Sub BotonSteamRuta_Click(sender As Object, e As RoutedEventArgs) Handles botonSteamRuta.Click

        Steam.Arranque(True)

    End Sub

    Private Sub LvConfigItemClick(sender As Object, args As ItemClickEventArgs)

        Dim sp As StackPanel = args.ClickedItem

        If sp.Tag.ToString = 0 Then

            GridConfigVisibilidad(spConfigBlizzard, botonBridgeConfigBlizzard)

        ElseIf sp.Tag.ToString = 1 Then

            GridConfigVisibilidad(spConfigGOGGalaxy, botonBridgeConfigGOGGalaxy)

        ElseIf sp.Tag.ToString = 2 Then

            GridConfigVisibilidad(spConfigOrigin, botonBridgeConfigOrigin)

        ElseIf sp.Tag.ToString = 3 Then

            GridConfigVisibilidad(spConfigTwitch, botonBridgeConfigTwitch)

        ElseIf sp.Tag.ToString = 4 Then

            GridConfigVisibilidad(spConfigUplay, botonBridgeConfigUplay)

        End If

    End Sub

    Private Sub GridConfigVisibilidad(sp As StackPanel, boton As ListViewItem)

        botonBridgeConfigBlizzard.Background = New SolidColorBrush(Colors.DarkOrchid)
        botonBridgeConfigGOGGalaxy.Background = New SolidColorBrush(Colors.DarkOrchid)
        botonBridgeConfigOrigin.Background = New SolidColorBrush(Colors.DarkOrchid)
        botonBridgeConfigTwitch.Background = New SolidColorBrush(Colors.DarkOrchid)
        botonBridgeConfigUplay.Background = New SolidColorBrush(Colors.DarkOrchid)

        boton.Background = New SolidColorBrush(Colors.DarkMagenta)

        spConfigBlizzard.Visibility = Visibility.Collapsed
        spConfigGOGGalaxy.Visibility = Visibility.Collapsed
        spConfigOrigin.Visibility = Visibility.Collapsed
        spConfigTwitch.Visibility = Visibility.Collapsed
        spConfigUplay.Visibility = Visibility.Collapsed

        sp.Visibility = Visibility.Visible

    End Sub

    Private Sub BotonBlizzardRuta_Click(sender As Object, e As RoutedEventArgs) Handles botonBlizzardRuta.Click

        Blizzard.Config(True)

    End Sub

    Private Sub BotonGOGGalaxyRuta_Click(sender As Object, e As RoutedEventArgs) Handles botonGOGGalaxyRuta.Click

        GOGGalaxy.Config(True)

    End Sub

    Private Sub BotonOriginRuta_Click(sender As Object, e As RoutedEventArgs) Handles botonOriginRuta.Click

        Origin.Config(True)

    End Sub

    Private Sub BotonTwitchRuta_Click(sender As Object, e As RoutedEventArgs) Handles botonTwitchRuta.Click

        Twitch.Config(True)

    End Sub

    Private Sub BotonUplayRutaCliente_Click(sender As Object, e As RoutedEventArgs) Handles botonUplayRutaCliente.Click

        Uplay.Config(1, True)

    End Sub

    Private Sub BotonUplayRutaJuegos_Click(sender As Object, e As RoutedEventArgs) Handles botonUplayRutaJuegos.Click

        Uplay.Config(0, True)

    End Sub

    Private Sub CbSteamOverlay_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cbSteamOverlay.SelectionChanged

        Dim opciones As ApplicationDataContainer = ApplicationData.Current.LocalSettings
        opciones.Values("SteamOverlay") = cbSteamOverlay.SelectedIndex

    End Sub

    Private Async Sub BotonBorrarConfig_Click(sender As Object, e As RoutedEventArgs) Handles botonBorrarConfig.Click

        Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()

        Await ApplicationData.Current.ClearAsync()

        Try
            StorageApplicationPermissions.FutureAccessList.Remove("SteamPath")
            botonSteamRutaTexto.Text = recursos.GetString("Add2")
            tbSteamRuta.Text = String.Empty
        Catch ex As Exception

        End Try

        Try
            StorageApplicationPermissions.FutureAccessList.Remove("BattlenetPath")
            botonBlizzardRutaTexto.Text = recursos.GetString("Add2")
            tbBlizzardRuta.Text = String.Empty
        Catch ex As Exception

        End Try

        Try
            StorageApplicationPermissions.FutureAccessList.Remove("GOGGalaxyPath")
            botonGOGGalaxyRutaTexto.Text = recursos.GetString("Add2")
            tbGOGGalaxyRuta.Text = String.Empty
        Catch ex As Exception

        End Try

        Try
            StorageApplicationPermissions.FutureAccessList.Remove("OriginPath")
            botonOriginRutaTexto.Text = recursos.GetString("Add2")
            tbOriginRuta.Text = String.Empty
        Catch ex As Exception

        End Try

        Try
            StorageApplicationPermissions.FutureAccessList.Remove("TwitchPath")
            botonTwitchRutaTexto.Text = recursos.GetString("Add2")
            tbTwitchRuta.Text = String.Empty
        Catch ex As Exception

        End Try

        Try
            StorageApplicationPermissions.FutureAccessList.Remove("UplayPathCliente")
            StorageApplicationPermissions.FutureAccessList.Remove("UplayPathJuegos")
            botonUplayRutaTextoCliente.Text = recursos.GetString("Add2")
            tbUplayRutaCliente.Text = String.Empty
            botonUplayRutaTextoJuegos.Text = recursos.GetString("Add2")
            tbUplayRutaJuegos.Text = String.Empty
        Catch ex As Exception

        End Try

    End Sub

End Class

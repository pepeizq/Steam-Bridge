Imports Microsoft.Services.Store.Engagement
Imports Microsoft.Toolkit.Uwp.Helpers
Imports Windows.ApplicationModel.Core
Imports Windows.Storage
Imports Windows.Storage.AccessCache
Imports Windows.System
Imports Windows.UI

Public NotInheritable Class MainPage
    Inherits Page

    Dim listaBattlenet As List(Of Juego)
    Dim listaGOGGalaxy As List(Of Juego)
    Dim listaOrigin As List(Of Juego)
    Dim listaTwitch As List(Of Juego)
    Dim listaUplay As List(Of Juego)
    Dim listaWindowsStore As List(Of Juego)

    Private Sub Nv_Loaded(sender As Object, e As RoutedEventArgs)

        Dim recursos As New Resources.ResourceLoader()

        nvPrincipal.MenuItems.Add(NavigationViewItems.Generar(recursos.GetString("Bridge"), New SymbolIcon(Symbol.Home), 0))
        nvPrincipal.MenuItems.Add(New NavigationViewItemSeparator)
        nvPrincipal.MenuItems.Add(NavigationViewItems.Generar(recursos.GetString("Config"), New SymbolIcon(Symbol.Setting), 1))
        nvPrincipal.MenuItems.Add(NavigationViewItems.Generar(recursos.GetString("MoreThings"), New SymbolIcon(Symbol.More), 2))

    End Sub

    Private Sub Nv_ItemInvoked(sender As NavigationView, args As NavigationViewItemInvokedEventArgs)

        Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()

        Dim item As TextBlock = args.InvokedItem

        If item.Text = recursos.GetString("Bridge") Then
            GridVisibilidad(gridBridge, item.Text)
        ElseIf item.Text = recursos.GetString("Config") Then
            GridVisibilidad(gridConfig, item.Text)
        ElseIf item.Text = recursos.GetString("MoreThings") Then
            GridVisibilidad(gridMasCosas, item.Text)
        End If

    End Sub

    Private Async Sub Page_Loaded(sender As FrameworkElement, args As Object)

        'Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "es-ES"
        'Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "en-US"

        Dim coreBarra As CoreApplicationViewTitleBar = CoreApplication.GetCurrentView.TitleBar
        coreBarra.ExtendViewIntoTitleBar = True

        Dim barra As ApplicationViewTitleBar = ApplicationView.GetForCurrentView().TitleBar
        barra.ButtonBackgroundColor = Colors.Transparent
        barra.ButtonForegroundColor = Colors.White
        barra.ButtonInactiveBackgroundColor = Colors.Transparent

        '--------------------------------------------------------

        Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()

        GridVisibilidad(gridBridge, recursos.GetString("Bridge"))
        nvPrincipal.IsPaneOpen = False

        Steam.Arranque(tbSteamRuta, botonSteamRutaTexto, False)

        Dim opciones As ApplicationDataContainer = ApplicationData.Current.LocalSettings

        If opciones.Values("SteamOverlay") = Nothing Then
            opciones.Values("SteamOverlay") = 0
        End If

        cbSteamOverlay.SelectedIndex = opciones.Values("SteamOverlay")

        If Await Blizzard.Config(False) = True Then
            botonBridgeBlizzard.IsEnabled = True
        End If

        If Await GOGGalaxy.Config(False) = True Then
            botonBridgeGOGGalaxy.IsEnabled = True
        End If

        If Await Origin.Config(False) = True Then
            botonBridgeOrigin.IsEnabled = True
        End If

        If Await Twitch.Config(False) = True Then
            botonBridgeTwitch.IsEnabled = True
        End If

        If Await Uplay.Config(1, False) = True Then
            botonBridgeUplay.IsEnabled = True
        End If

        If Await WindowsStore.Config(False) = True Then
            botonBridgeWindowsStore.IsEnabled = True
        End If

    End Sub

    Private Sub GridVisibilidad(grid As Grid, tag As String)

        tbTitulo.Text = Package.Current.DisplayName + " (" + Package.Current.Id.Version.Major.ToString + "." + Package.Current.Id.Version.Minor.ToString + "." + Package.Current.Id.Version.Build.ToString + "." + Package.Current.Id.Version.Revision.ToString + ") - " + tag

        gridBridge.Visibility = Visibility.Collapsed
        gridConfig.Visibility = Visibility.Collapsed
        gridMasCosas.Visibility = Visibility.Collapsed

        grid.Visibility = Visibility.Visible

    End Sub

    Private Sub LvBridgeItemClick(sender As Object, args As ItemClickEventArgs)

        If panelMensajeBridge.Visibility = Visibility.Visible Then
            panelMensajeBridge.Visibility = Visibility.Collapsed
        End If

        botonBridgeBlizzard.Background = New SolidColorBrush(App.Current.Resources("ColorSecundario"))
        botonBridgeGOGGalaxy.Background = New SolidColorBrush(App.Current.Resources("ColorSecundario"))
        botonBridgeOrigin.Background = New SolidColorBrush(App.Current.Resources("ColorSecundario"))
        botonBridgeTwitch.Background = New SolidColorBrush(App.Current.Resources("ColorSecundario"))
        botonBridgeUplay.Background = New SolidColorBrush(App.Current.Resources("ColorSecundario"))
        botonBridgeWindowsStore.Background = New SolidColorBrush(App.Current.Resources("ColorSecundario"))

        gridBridgeBlizzard.Visibility = Visibility.Collapsed
        gridBridgeGOGGalaxy.Visibility = Visibility.Collapsed
        gridBridgeOrigin.Visibility = Visibility.Collapsed
        gridBridgeTwitch.Visibility = Visibility.Collapsed
        gridBridgeUplay.Visibility = Visibility.Collapsed
        gridBridgeWindowsStore.Visibility = Visibility.Collapsed

        Dim sp As StackPanel = args.ClickedItem

        If sp.Tag.ToString = 0 Then

            tbTitulo.Text = Package.Current.DisplayName + " (" + Package.Current.Id.Version.Major.ToString + "." + Package.Current.Id.Version.Minor.ToString + "." + Package.Current.Id.Version.Build.ToString + "." + Package.Current.Id.Version.Revision.ToString + ") - Blizzard"
            botonBridgeBlizzard.Background = New SolidColorBrush(App.Current.Resources("ColorPrimario"))
            gridBridgeBlizzard.Visibility = Visibility.Visible

            If lvBlizzard.Items.Count = 0 Then
                EjecutarBlizzard()
            End If

        ElseIf sp.Tag.ToString = 1 Then

            tbTitulo.Text = Package.Current.DisplayName + " (" + Package.Current.Id.Version.Major.ToString + "." + Package.Current.Id.Version.Minor.ToString + "." + Package.Current.Id.Version.Build.ToString + "." + Package.Current.Id.Version.Revision.ToString + ") - GOG Galaxy"
            botonBridgeGOGGalaxy.Background = New SolidColorBrush(App.Current.Resources("ColorPrimario"))
            gridBridgeGOGGalaxy.Visibility = Visibility.Visible

            If lvGOGGalaxy.Items.Count = 0 Then
                EjecutarGOGGalaxy()
            End If

        ElseIf sp.Tag.ToString = 2 Then

            tbTitulo.Text = Package.Current.DisplayName + " (" + Package.Current.Id.Version.Major.ToString + "." + Package.Current.Id.Version.Minor.ToString + "." + Package.Current.Id.Version.Build.ToString + "." + Package.Current.Id.Version.Revision.ToString + ") - Origin"
            botonBridgeOrigin.Background = New SolidColorBrush(App.Current.Resources("ColorPrimario"))
            gridBridgeOrigin.Visibility = Visibility.Visible

            If lvOrigin.Items.Count = 0 Then
                EjecutarOrigin()
            End If

        ElseIf sp.Tag.ToString = 3 Then

            tbTitulo.Text = Package.Current.DisplayName + " (" + Package.Current.Id.Version.Major.ToString + "." + Package.Current.Id.Version.Minor.ToString + "." + Package.Current.Id.Version.Build.ToString + "." + Package.Current.Id.Version.Revision.ToString + ") - Twitch"
            botonBridgeTwitch.Background = New SolidColorBrush(App.Current.Resources("ColorPrimario"))
            gridBridgeTwitch.Visibility = Visibility.Visible

            If lvTwitch.Items.Count = 0 Then
                EjecutarTwitch()
            End If

        ElseIf sp.Tag.ToString = 4 Then

            tbTitulo.Text = Package.Current.DisplayName + " (" + Package.Current.Id.Version.Major.ToString + "." + Package.Current.Id.Version.Minor.ToString + "." + Package.Current.Id.Version.Build.ToString + "." + Package.Current.Id.Version.Revision.ToString + ") - Uplay"
            botonBridgeUplay.Background = New SolidColorBrush(App.Current.Resources("ColorPrimario"))
            gridBridgeUplay.Visibility = Visibility.Visible

            If lvUplay.Items.Count = 0 Then
                EjecutarUplay()
            End If

        ElseIf sp.Tag.ToString = 5 Then

            tbTitulo.Text = Package.Current.DisplayName + " (" + Package.Current.Id.Version.Major.ToString + "." + Package.Current.Id.Version.Minor.ToString + "." + Package.Current.Id.Version.Build.ToString + "." + Package.Current.Id.Version.Revision.ToString + ") - Windows Store"
            botonBridgeWindowsStore.Background = New SolidColorBrush(App.Current.Resources("ColorPrimario"))
            gridBridgeWindowsStore.Visibility = Visibility.Visible

            If lvWindowsStore.Items.Count = 0 Then
                EjecutarWindowsStore()
            End If

        End If

    End Sub

    'BRIDGE-----------------------------------------------------------------------------

    Private Sub LvBlizzard_ItemClick(sender As Object, e As ItemClickEventArgs) Handles lvBlizzard.ItemClick

        Listado.Clickeo(sender, e)

    End Sub

    Private Async Sub EjecutarBlizzard()

        Dim carpetaBlizzard As StorageFolder = Nothing

        Try
            carpetaBlizzard = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("BattlenetPath")
        Catch ex As Exception

        End Try

        Dim battleBool As Boolean = False

        If Not carpetaBlizzard Is Nothing Then
            battleBool = Await Blizzard.Config(False)

            If battleBool = True Then
                listaBattlenet = New List(Of Juego)
                Blizzard.Generar(listaBattlenet, carpetaBlizzard)
            Else
                panelAvisoNoJuegosBlizzard.Visibility = Visibility.Visible
            End If
        Else
            panelAvisoNoJuegosBlizzard.Visibility = Visibility.Visible
        End If

    End Sub

    Private Sub LvGOGGalaxy_ItemClick(sender As Object, e As ItemClickEventArgs) Handles lvGOGGalaxy.ItemClick

        Listado.Clickeo(sender, e)

    End Sub

    Private Async Sub EjecutarGOGGalaxy()

        Dim carpetaGOGGalaxy As StorageFolder = Nothing

        Try
            carpetaGOGGalaxy = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("GOGGalaxyPath")
        Catch ex As Exception

        End Try

        Dim galaxyBool As Boolean = False

        If Not carpetaGOGGalaxy Is Nothing Then
            galaxyBool = Await GOGGalaxy.Config(False)

            If galaxyBool = True Then
                listaGOGGalaxy = New List(Of Juego)
                GOGGalaxy.Generar(listaGOGGalaxy, carpetaGOGGalaxy)
            Else
                panelAvisoNoJuegosGOGGalaxy.Visibility = Visibility.Visible
            End If
        Else
            panelAvisoNoJuegosGOGGalaxy.Visibility = Visibility.Visible
        End If

    End Sub

    Private Sub LvOrigin_ItemClick(sender As Object, e As ItemClickEventArgs) Handles lvOrigin.ItemClick

        Listado.Clickeo(sender, e)

    End Sub

    Private Async Sub EjecutarOrigin()

        Dim carpetaOrigin As StorageFolder = Nothing

        Try
            carpetaOrigin = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("OriginPath")
        Catch ex As Exception

        End Try

        Dim originBool As Boolean = False

        If Not carpetaOrigin Is Nothing Then
            originBool = Await Origin.Config(False)

            If originBool = True Then
                listaOrigin = New List(Of Juego)
                Origin.Generar(listaOrigin, carpetaOrigin)
            Else
                panelAvisoNoJuegosOrigin.Visibility = Visibility.Visible
            End If
        Else
            panelAvisoNoJuegosOrigin.Visibility = Visibility.Visible
        End If

    End Sub

    Private Sub LvTwitch_ItemClick(sender As Object, e As ItemClickEventArgs) Handles lvTwitch.ItemClick

        Listado.Clickeo(sender, e)

    End Sub

    Private Async Sub EjecutarTwitch()

        Dim carpetaTwitch As StorageFolder = Nothing

        Try
            carpetaTwitch = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("TwitchPath")
        Catch ex As Exception

        End Try

        Dim twitchBool As Boolean = False

        If Not carpetaTwitch Is Nothing Then
            twitchBool = Await Twitch.Config(False)

            If twitchBool = True Then
                listaTwitch = New List(Of Juego)
                Twitch.Generar(listaTwitch, carpetaTwitch)
            Else
                panelAvisoNoJuegosTwitch.Visibility = Visibility.Visible
            End If
        Else
            panelAvisoNoJuegosTwitch.Visibility = Visibility.Visible
        End If

    End Sub

    Private Sub LvUplay_ItemClick(sender As Object, e As ItemClickEventArgs) Handles lvUplay.ItemClick

        Listado.Clickeo(sender, e)

    End Sub

    Private Async Sub EjecutarUplay()

        Dim carpetaUplayCliente As StorageFolder = Nothing

        Try
            carpetaUplayCliente = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("UplayPathCliente")
        Catch ex As Exception

        End Try

        Dim carpetaUplayJuegos As StorageFolder = Nothing

        Try
            carpetaUplayJuegos = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("UplayPathJuegos")
        Catch ex As Exception

        End Try

        Dim uplaybool As Boolean = False

        If Not carpetaUplayCliente Is Nothing Then
            If Not carpetaUplayJuegos Is Nothing Then
                uplaybool = Await Uplay.Config(99, False)

                If uplaybool = True Then
                    listaUplay = New List(Of Juego)
                    Uplay.Generar(listaUplay, carpetaUplayCliente, carpetaUplayJuegos)
                Else
                    panelAvisoNoJuegosUplay.Visibility = Visibility.Visible
                End If
            Else
                panelAvisoNoJuegosUplay.Visibility = Visibility.Visible
            End If
        Else
            panelAvisoNoJuegosUplay.Visibility = Visibility.Visible
        End If

    End Sub

    Private Sub LvWindowsStore_ItemClick(sender As Object, e As ItemClickEventArgs) Handles lvWindowsStore.ItemClick

        Listado.Clickeo(sender, e)

    End Sub

    Private Async Sub EjecutarWindowsStore()

        Dim carpetaWindowsStore As StorageFolder = Nothing

        Try
            carpetaWindowsStore = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("WindowsStorePath")
        Catch ex As Exception

        End Try

        Dim windowsbool As Boolean = False

        If Not carpetaWindowsStore Is Nothing Then
            windowsbool = Await WindowsStore.Config(False)

            If windowsbool = True Then
                listaWindowsStore = New List(Of Juego)
                WindowsStore.Generar(listaWindowsStore, carpetaWindowsStore)
            Else
                panelAvisoNoJuegosWindowsStore.Visibility = Visibility.Visible
            End If
        Else
            panelAvisoNoJuegosWindowsStore.Visibility = Visibility.Visible
        End If

    End Sub

    Private Async Sub BotonAñadirJuegos_Click(sender As Object, e As RoutedEventArgs) Handles botonAñadirJuegos.Click

        Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()
        Dim carpetaSteam As StorageFolder = Nothing

        Try
            carpetaSteam = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("SteamPath")
        Catch ex As Exception
            Toast(recursos.GetString("NoSteam"), Nothing)
        End Try

        If Not carpetaSteam Is Nothing Then
            Dim listaFinal As List(Of Juego) = New List(Of Juego)

            If Not listaBattlenet Is Nothing Then
                If listaBattlenet.Count > 0 Then
                    For Each juego As Juego In listaBattlenet
                        If juego.Añadir = True Then
                            listaFinal.Add(juego)
                        End If
                    Next
                End If
            End If

            If Not listaGOGGalaxy Is Nothing Then
                If listaGOGGalaxy.Count > 0 Then
                    For Each juego As Juego In listaGOGGalaxy
                        If juego.Añadir = True Then
                            listaFinal.Add(juego)
                        End If
                    Next
                End If
            End If

            If Not listaOrigin Is Nothing Then
                If listaOrigin.Count > 0 Then
                    For Each juego As Juego In listaOrigin
                        If juego.Añadir = True Then
                            listaFinal.Add(juego)
                        End If
                    Next
                End If
            End If

            If Not listaUplay Is Nothing Then
                If listaUplay.Count > 0 Then
                    For Each juego As Juego In listaUplay
                        If juego.Añadir = True Then
                            listaFinal.Add(juego)
                        End If
                    Next
                End If
            End If

            If Not listaWindowsStore Is Nothing Then
                If listaWindowsStore.Count > 0 Then
                    For Each juego As Juego In listaWindowsStore
                        If juego.Añadir = True Then
                            listaFinal.Add(juego)
                        End If
                    Next
                End If
            End If

            If listaFinal.Count > 0 Then
                Steam.CrearAccesos(listaFinal, carpetaSteam, botonAñadirJuegos)
            End If
        End If

    End Sub

    Private Sub BotonAñadirJuegos_PointerEntered(sender As Object, e As PointerRoutedEventArgs) Handles botonAñadirJuegos.PointerEntered

        panelAvisoAñadirJuegos.Visibility = Visibility.Visible

    End Sub

    Private Sub BotonAñadirJuegos_PointerExited(sender As Object, e As PointerRoutedEventArgs) Handles botonAñadirJuegos.PointerExited

        panelAvisoAñadirJuegos.Visibility = Visibility.Collapsed

    End Sub

    'CONFIG-----------------------------------------

    Private Sub BotonSteamRuta_Click(sender As Object, e As RoutedEventArgs) Handles botonSteamRuta.Click

        Steam.Arranque(tbSteamRuta, botonSteamRutaTexto, True)

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

        ElseIf sp.Tag.ToString = 5 Then

            GridConfigVisibilidad(spConfigWindowsStore, botonBridgeConfigWindowsStore)

        End If

    End Sub

    Private Sub GridConfigVisibilidad(sp As StackPanel, boton As ListViewItem)

        botonBridgeConfigBlizzard.Background = New SolidColorBrush(Colors.DarkOrchid)
        botonBridgeConfigGOGGalaxy.Background = New SolidColorBrush(Colors.DarkOrchid)
        botonBridgeConfigOrigin.Background = New SolidColorBrush(Colors.DarkOrchid)
        botonBridgeConfigTwitch.Background = New SolidColorBrush(Colors.DarkOrchid)
        botonBridgeConfigUplay.Background = New SolidColorBrush(Colors.DarkOrchid)
        botonBridgeConfigWindowsStore.Background = New SolidColorBrush(Colors.DarkOrchid)

        boton.Background = New SolidColorBrush(Colors.DarkMagenta)

        spConfigBlizzard.Visibility = Visibility.Collapsed
        spConfigGOGGalaxy.Visibility = Visibility.Collapsed
        spConfigOrigin.Visibility = Visibility.Collapsed
        spConfigTwitch.Visibility = Visibility.Collapsed
        spConfigUplay.Visibility = Visibility.Collapsed
        spConfigWindowsStore.Visibility = Visibility.Collapsed

        sp.Visibility = Visibility.Visible

    End Sub

    Private Async Sub BotonBlizzardRuta_Click(sender As Object, e As RoutedEventArgs) Handles botonBlizzardRuta.Click

        If Await Blizzard.Config(True) = True Then
            botonBridgeBlizzard.IsEnabled = True
        End If

    End Sub

    Private Async Sub BotonGOGGalaxyRuta_Click(sender As Object, e As RoutedEventArgs) Handles botonGOGGalaxyRuta.Click

        If Await GOGGalaxy.Config(True) = True Then
            botonBridgeGOGGalaxy.IsEnabled = True
        End If

    End Sub

    Private Async Sub BotonOriginRuta_Click(sender As Object, e As RoutedEventArgs) Handles botonOriginRuta.Click

        If Await Origin.Config(True) = True Then
            botonBridgeOrigin.isEnabled = True
        End If

    End Sub

    Private Async Sub BotonTwitchRuta_Click(sender As Object, e As RoutedEventArgs) Handles botonTwitchRuta.Click

        If Await Twitch.Config(True) = True Then
            botonBridgeTwitch.isEnabled = True
        End If

    End Sub

    Private Async Sub BotonUplayRutaCliente_Click(sender As Object, e As RoutedEventArgs) Handles botonUplayRutaCliente.Click

        If Await Uplay.Config(1, True) = True Then
            botonBridgeUplay.IsEnabled = True
        End If

    End Sub

    Private Async Sub BotonUplayRutaJuegos_Click(sender As Object, e As RoutedEventArgs) Handles botonUplayRutaJuegos.Click
        If Await Uplay.Config(0, True) = True Then
            botonBridgeUplay.IsEnabled = True
        End If

    End Sub

    Private Async Sub BotonWindowsStoreRuta_Click(sender As Object, e As RoutedEventArgs) Handles botonWindowsStoreRuta.Click

        If Await WindowsStore.Config(True) = True Then
            botonBridgeWindowsStore.IsEnabled = True
        End If

    End Sub

    Private Async Sub BotonWindowsStoreTutorial_Click(sender As Object, e As RoutedEventArgs) Handles botonWindowsStoreTutorial.Click

        Try
            Await Launcher.LaunchUriAsync(New Uri("https://www.maketecheasier.com/access-windowsapps-folder-windows-10/"))
        Catch ex As Exception

        End Try

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
            lvBlizzard.Items.Clear()
            botonBlizzardRutaTexto.Text = recursos.GetString("Add2")
            tbBlizzardRuta.Text = String.Empty
        Catch ex As Exception

        End Try

        Try
            StorageApplicationPermissions.FutureAccessList.Remove("GOGGalaxyPath")
            lvGOGGalaxy.Items.Clear()
            botonGOGGalaxyRutaTexto.Text = recursos.GetString("Add2")
            tbGOGGalaxyRuta.Text = String.Empty
        Catch ex As Exception

        End Try

        Try
            StorageApplicationPermissions.FutureAccessList.Remove("OriginPath")
            lvOrigin.Items.Clear()
            botonOriginRutaTexto.Text = recursos.GetString("Add2")
            tbOriginRuta.Text = String.Empty
        Catch ex As Exception

        End Try

        Try
            StorageApplicationPermissions.FutureAccessList.Remove("TwitchPath")
            lvTwitch.Items.Clear()
            botonTwitchRutaTexto.Text = recursos.GetString("Add2")
            tbTwitchRuta.Text = String.Empty
        Catch ex As Exception

        End Try

        Try
            StorageApplicationPermissions.FutureAccessList.Remove("UplayPathCliente")
            StorageApplicationPermissions.FutureAccessList.Remove("UplayPathJuegos")
            lvUplay.Items.Clear()
            botonUplayRutaTextoCliente.Text = recursos.GetString("Add2")
            tbUplayRutaCliente.Text = String.Empty
            botonUplayRutaTextoJuegos.Text = recursos.GetString("Add2")
            tbUplayRutaJuegos.Text = String.Empty
        Catch ex As Exception

        End Try

        Try
            StorageApplicationPermissions.FutureAccessList.Remove("WindowsStorePath")
            lvWindowsStore.Items.Clear()
            botonWindowsStoreRutaTexto.Text = recursos.GetString("Add2")
            tbWindowsStoreRuta.Text = String.Empty
        Catch ex As Exception

        End Try

        botonBridgeBlizzard.Background = New SolidColorBrush(Colors.DarkOrchid)
        botonBridgeGOGGalaxy.Background = New SolidColorBrush(Colors.DarkOrchid)
        botonBridgeOrigin.Background = New SolidColorBrush(Colors.DarkOrchid)
        botonBridgeTwitch.Background = New SolidColorBrush(Colors.DarkOrchid)
        botonBridgeUplay.Background = New SolidColorBrush(Colors.DarkOrchid)
        botonBridgeWindowsStore.Background = New SolidColorBrush(Colors.DarkOrchid)

        gridBridgeBlizzard.Visibility = Visibility.Collapsed
        gridBridgeGOGGalaxy.Visibility = Visibility.Collapsed
        gridBridgeOrigin.Visibility = Visibility.Collapsed
        gridBridgeTwitch.Visibility = Visibility.Collapsed
        gridBridgeUplay.Visibility = Visibility.Collapsed
        gridBridgeWindowsStore.Visibility = Visibility.Collapsed

        panelMensajeBridge.Visibility = Visibility.Visible

    End Sub

    'MASCOSAS-----------------------------------------

    Private Async Sub LvMasCosasItemClick(sender As Object, args As ItemClickEventArgs)

        Dim sp As StackPanel = args.ClickedItem

        If sp.Tag.ToString = 0 Then

            Await Launcher.LaunchUriAsync(New Uri("ms-windows-store:REVIEW?PFN=" + Package.Current.Id.FamilyName))

        ElseIf sp.Tag.ToString = 1 Then

            wvMasCosas.Navigate(New Uri("https://pepeizqapps.com/"))

        ElseIf sp.Tag.ToString = 2 Then

            wvMasCosas.Navigate(New Uri("https://pepeizqapps.com/contact/"))

        ElseIf sp.Tag.ToString = 3 Then

            If StoreServicesFeedbackLauncher.IsSupported = True Then
                Dim ejecutador As StoreServicesFeedbackLauncher = StoreServicesFeedbackLauncher.GetDefault()
                Await ejecutador.LaunchAsync()
            Else
                wvMasCosas.Navigate(New Uri("https://pepeizqapps.com/contact/"))
            End If

        ElseIf sp.Tag.ToString = 4 Then

            wvMasCosas.Navigate(New Uri("https://poeditor.com/join/project/UYVDZ4FYAt"))

        ElseIf sp.Tag.ToString = 5 Then

            wvMasCosas.Navigate(New Uri("https://github.com/pepeizq/Steam-Bridge"))

        ElseIf sp.Tag.ToString = 6 Then

            wvMasCosas.Navigate(New Uri("https://pepeizqapps.com/thanks/"))

        End If

    End Sub

End Class

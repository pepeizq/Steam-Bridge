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

    Private Sub Page_Loaded(sender As FrameworkElement, args As Object)

        'Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "es-ES"
        'Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "en-US"

        Acrilico.Generar(gridTopAcrilico)
        Acrilico.Generar(gridMenuAcrilico)

        Dim barra As ApplicationViewTitleBar = ApplicationView.GetForCurrentView().TitleBar
        barra.ButtonBackgroundColor = Colors.Transparent
        barra.ButtonForegroundColor = Colors.White
        barra.ButtonPressedBackgroundColor = Colors.DarkMagenta
        barra.ButtonInactiveBackgroundColor = Colors.Transparent
        Dim coreBarra As CoreApplicationViewTitleBar = CoreApplication.GetCurrentView.TitleBar
        coreBarra.ExtendViewIntoTitleBar = True

        '--------------------------------------------------------

        Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()

        botonConfigTexto.Text = recursos.GetString("Boton Config")
        botonVotarTexto.Text = recursos.GetString("Boton Votar")
        botonMasCosasTexto.Text = recursos.GetString("Boton Cosas")

        botonMasAppsTexto.Text = recursos.GetString("Boton Web")
        botonContactoTexto.Text = recursos.GetString("Boton Contacto")
        botonReportarTexto.Text = recursos.GetString("Boton Reportar")
        botonCodigoFuenteTexto.Text = recursos.GetString("Boton Codigo Fuente")

        tbBlizzardNoJuegos.Text = recursos.GetString("No Juegos")
        tbGOGGalaxyNoJuegos.Text = recursos.GetString("No Juegos")
        tbOriginNoJuegos.Text = recursos.GetString("No Juegos")
        tbTwitchNoJuegos.Text = recursos.GetString("No Juegos")
        tbUplayNoJuegos.Text = recursos.GetString("No Juegos")
        tbWindowsStoreNoJuegos.Text = recursos.GetString("No Juegos")

        buttonAñadirJuegosTexto.Text = recursos.GetString("Boton Añadir Juegos")
        tbAvisoAñadirJuegos.Text = recursos.GetString("Aviso Añadir Juegos")

        tbSteamConfigInstrucciones.Text = recursos.GetString("Texto Steam Config")
        buttonSteamConfigPathTexto.Text = recursos.GetString("Boton Añadir")
        tbSteamConfigPath.Text = recursos.GetString("Texto Carpeta")
        tbSteamOverlayConfigInstrucciones.Text = recursos.GetString("Texto Steam Overlay Config")
        botonBorrarConfigTexto.Text = recursos.GetString("Borrar Config")

        tbBattlenetConfigInstrucciones.Text = recursos.GetString("Texto Battlenet Config")
        buttonBattlenetConfigPathTexto.Text = recursos.GetString("Boton Añadir")
        tbBattlenetConfigPath.Text = recursos.GetString("Texto Carpeta")

        tbGOGGalaxyConfigInstrucciones.Text = recursos.GetString("Texto GOG Galaxy Config")
        buttonGOGGalaxyConfigPathTexto.Text = recursos.GetString("Boton Añadir")
        tbGOGGalaxyConfigPath.Text = recursos.GetString("Texto Carpeta")

        tbOriginConfigInstrucciones.Text = recursos.GetString("Texto Origin Config")
        buttonOriginConfigPathTexto.Text = recursos.GetString("Boton Añadir")
        tbOriginConfigPath.Text = recursos.GetString("Texto Carpeta")

        tbTwitchConfigInstrucciones.Text = recursos.GetString("Texto Twitch Config")
        buttonTwitchConfigPathTexto.Text = recursos.GetString("Boton Añadir")
        tbTwitchConfigPath.Text = recursos.GetString("Texto Carpeta")

        tbUplayConfigInstruccionesCliente.Text = recursos.GetString("Texto Uplay Config Cliente")
        buttonUplayConfigPathTextoCliente.Text = recursos.GetString("Boton Añadir")
        tbUplayConfigPathCliente.Text = recursos.GetString("Texto Carpeta")
        tbUplayConfigInstruccionesJuegos.Text = recursos.GetString("Texto Uplay Config Juegos")
        buttonUplayConfigPathTextoJuegos.Text = recursos.GetString("Boton Añadir")
        tbUplayConfigPathJuegos.Text = recursos.GetString("Texto Carpeta")
        tbUplayConfigInstruccionesAviso.Text = recursos.GetString("Texto Uplay Config Aviso")
        buttonUplayConfigAviso.Content = recursos.GetString("Boton Uplay Config Aviso")

        tbWindowsStoreConfigInstrucciones.Text = recursos.GetString("Texto Windows Store Config")
        buttonWindowsStoreConfigPathTexto.Text = recursos.GetString("Boton Añadir")
        tbWindowsStoreConfigPath.Text = recursos.GetString("Texto Carpeta")
        buttonWindowsStoreConfigTutorial.Content = recursos.GetString("Boton Windows Store Tutorial")

        '--------------------------------------------------------

        GridVisibilidad(gridWindowsStore, botonWindowsStore, "Windows Store")
        EjecutarWindowsStore()

        Dim opciones As ApplicationDataContainer = ApplicationData.Current.LocalSettings

        If opciones.Values("SteamOverlay") = Nothing Then
            opciones.Values("SteamOverlay") = 0
        End If

        cbSteamOverlay.SelectedIndex = opciones.Values("SteamOverlay")

    End Sub

    Private Sub GridVisibilidad(grid As Grid, boton As Button, seccion As String)

        tbTitulo.Text = "Steam Bridge (" + SystemInformation.ApplicationVersion.Major.ToString + "." + SystemInformation.ApplicationVersion.Minor.ToString + "." + SystemInformation.ApplicationVersion.Build.ToString + "." + SystemInformation.ApplicationVersion.Revision.ToString + ") - " + seccion

        gridBridge.Visibility = Visibility.Visible
        gridBlizzard.Visibility = Visibility.Collapsed
        gridGOGGalaxy.Visibility = Visibility.Collapsed
        gridOrigin.Visibility = Visibility.Collapsed
        gridTwitch.Visibility = Visibility.Collapsed
        gridUplay.Visibility = Visibility.Collapsed
        gridWindowsStore.Visibility = Visibility.Collapsed
        gridConfig.Visibility = Visibility.Collapsed

        grid.Visibility = Visibility.Visible

        If grid.Name = "gridConfig" Then
            gridBridge.Visibility = Visibility.Collapsed
        End If

        botonBlizzard.Background = New SolidColorBrush(Colors.Transparent)
        botonGOGGalaxy.Background = New SolidColorBrush(Colors.Transparent)
        botonOrigin.Background = New SolidColorBrush(Colors.Transparent)
        botonTwitch.Background = New SolidColorBrush(Colors.Transparent)
        botonUplay.Background = New SolidColorBrush(Colors.Transparent)
        botonWindowsStore.Background = New SolidColorBrush(Colors.Transparent)
        botonConfig.Background = New SolidColorBrush(Colors.Transparent)

        If Not boton Is Nothing Then
            boton.Background = New SolidColorBrush(Colors.DarkOrchid)
        End If

    End Sub

    Private Sub BotonBlizzard_Click(sender As Object, e As RoutedEventArgs) Handles botonBlizzard.Click

        GridVisibilidad(gridBlizzard, botonBlizzard, "Blizzard App")

        If lvBlizzard.Items.Count = 0 Then
            EjecutarBlizzard()
        End If

    End Sub

    Private Sub BotonGOGGalaxy_Click(sender As Object, e As RoutedEventArgs) Handles botonGOGGalaxy.Click

        GridVisibilidad(gridGOGGalaxy, botonGOGGalaxy, "GOG Galaxy")

        If lvGOGGalaxy.Items.Count = 0 Then
            EjecutarGOGGalaxy()
        End If

    End Sub

    Private Sub BotonOrigin_Click(sender As Object, e As RoutedEventArgs) Handles botonOrigin.Click

        GridVisibilidad(gridOrigin, botonOrigin, "Origin")

        If lvOrigin.Items.Count = 0 Then
            EjecutarOrigin()
        End If

    End Sub

    Private Sub BotonTwitch_Click(sender As Object, e As RoutedEventArgs) Handles botonTwitch.Click

        GridVisibilidad(gridTwitch, botonTwitch, "Twitch")

        If lvTwitch.Items.Count = 0 Then
            EjecutarTwitch()
        End If

    End Sub

    Private Sub BotonUplay_Click(sender As Object, e As RoutedEventArgs) Handles botonUplay.Click

        GridVisibilidad(gridUplay, botonUplay, "Uplay")

        If lvUplay.Items.Count = 0 Then
            EjecutarUplay()
        End If

    End Sub

    Private Sub BotonWindowsStore_Click(sender As Object, e As RoutedEventArgs) Handles botonWindowsStore.Click

        GridVisibilidad(gridWindowsStore, botonWindowsStore, "Windows Store")

        If lvWindowsStore.Items.Count = 0 Then
            EjecutarWindowsStore()
        End If

    End Sub

    Private Sub BotonConfig_Click(sender As Object, e As RoutedEventArgs) Handles botonConfig.Click

        Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()
        GridVisibilidad(gridConfig, botonConfig, recursos.GetString("Boton Config"))
        GridConfigVisibilidad(gridConfigApp, botonConfigApp)

    End Sub

    Private Async Sub BotonVotar_Click(sender As Object, e As RoutedEventArgs) Handles botonVotar.Click

        Await Launcher.LaunchUriAsync(New Uri("ms-windows-store:REVIEW?PFN=" + Package.Current.Id.FamilyName))

    End Sub

    Private Sub BotonMasCosas_Click(sender As Object, e As RoutedEventArgs) Handles botonMasCosas.Click

        If popupMasCosas.IsOpen = True Then
            botonMasCosas.Background = New SolidColorBrush(Colors.Transparent)
            popupMasCosas.IsOpen = False
        Else
            botonMasCosas.Background = New SolidColorBrush(Colors.DarkOrchid)
            popupMasCosas.IsOpen = True
        End If

    End Sub

    Private Sub PopupMasCosas_LayoutUpdated(sender As Object, e As Object) Handles popupMasCosas.LayoutUpdated

        popupMasCosas.Height = spMasCosas.ActualHeight

    End Sub

    Private Async Sub BotonMasApps_Click(sender As Object, e As RoutedEventArgs) Handles botonMasApps.Click

        Await Launcher.LaunchUriAsync(New Uri("https://pepeizqapps.com/"))

    End Sub

    Private Async Sub BotonContacto_Click(sender As Object, e As RoutedEventArgs) Handles botonContacto.Click

        Await Launcher.LaunchUriAsync(New Uri("https://pepeizqapps.com/contact/"))

    End Sub

    Private Async Sub BotonReportar_Click(sender As Object, e As RoutedEventArgs) Handles botonReportar.Click

        If StoreServicesFeedbackLauncher.IsSupported = True Then
            Dim ejecutador As StoreServicesFeedbackLauncher = StoreServicesFeedbackLauncher.GetDefault()
            Await ejecutador.LaunchAsync()
        Else
            Await Launcher.LaunchUriAsync(New Uri("https://pepeizqapps.com/contact/"))
        End If

    End Sub

    Private Async Sub BotonCodigoFuente_Click(sender As Object, e As RoutedEventArgs) Handles botonCodigoFuente.Click

        Await Launcher.LaunchUriAsync(New Uri("https://github.com/pepeizq/Steam-Bridge"))

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

    Private Async Sub ButtonAñadirJuegos_Click(sender As Object, e As RoutedEventArgs) Handles buttonAñadirJuegos.Click

        Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()
        Dim carpetaSteam As StorageFolder = Nothing

        Try
            carpetaSteam = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("SteamPath")
        Catch ex As Exception
            Notificaciones.Toast("Steam Bridge", recursos.GetString("Steam No Config"))
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
                Steam.CrearAccesos(listaFinal, carpetaSteam, buttonAñadirJuegos)
            End If
        End If

    End Sub

    Private Sub ButtonAñadirJuegos_PointerEntered(sender As Object, e As PointerRoutedEventArgs) Handles buttonAñadirJuegos.PointerEntered

        panelAvisoAñadirJuegos.Visibility = Visibility.Visible

    End Sub

    Private Sub ButtonAñadirJuegos_PointerExited(sender As Object, e As PointerRoutedEventArgs) Handles buttonAñadirJuegos.PointerExited

        panelAvisoAñadirJuegos.Visibility = Visibility.Collapsed

    End Sub

    'CONFIG------------------------------------------------

    Private Sub GridConfigVisibilidad(grid As Grid, boton As Button)

        gridConfigApp.Visibility = Visibility.Collapsed
        gridConfigBlizzard.Visibility = Visibility.Collapsed
        gridConfigGOGGalaxy.Visibility = Visibility.Collapsed
        gridConfigOrigin.Visibility = Visibility.Collapsed
        gridConfigTwitch.Visibility = Visibility.Collapsed
        gridConfigUplay.Visibility = Visibility.Collapsed
        gridConfigWindowsStore.Visibility = Visibility.Collapsed

        grid.Visibility = Visibility.Visible

        botonConfigApp.Background = New SolidColorBrush(Colors.Transparent)
        botonConfigBlizzard.Background = New SolidColorBrush(Colors.Transparent)
        botonConfigGOGGalaxy.Background = New SolidColorBrush(Colors.Transparent)
        botonConfigOrigin.Background = New SolidColorBrush(Colors.Transparent)
        botonConfigTwitch.Background = New SolidColorBrush(Colors.Transparent)
        botonConfigUplay.Background = New SolidColorBrush(Colors.Transparent)
        botonConfigWindowsStore.Background = New SolidColorBrush(Colors.Transparent)

        boton.Background = New SolidColorBrush(Colors.DarkMagenta)

    End Sub

    Private Sub BotonConfigApp_Click(sender As Object, e As RoutedEventArgs) Handles botonConfigApp.Click

        GridConfigVisibilidad(gridConfigApp, botonConfigApp)

    End Sub

    Private Sub BotonConfigBlizzard_Click(sender As Object, e As RoutedEventArgs) Handles botonConfigBlizzard.Click

        GridConfigVisibilidad(gridConfigBlizzard, botonConfigBlizzard)

    End Sub

    Private Sub BotonConfigGOGGalaxy_Click(sender As Object, e As RoutedEventArgs) Handles botonConfigGOGGalaxy.Click

        GridConfigVisibilidad(gridConfigGOGGalaxy, botonConfigGOGGalaxy)

    End Sub

    Private Sub BotonConfigOrigin_Click(sender As Object, e As RoutedEventArgs) Handles botonConfigOrigin.Click

        GridConfigVisibilidad(gridConfigOrigin, botonConfigOrigin)

    End Sub

    Private Sub BotonConfigTwitch_Click(sender As Object, e As RoutedEventArgs) Handles botonConfigTwitch.Click

        GridConfigVisibilidad(gridConfigTwitch, botonConfigTwitch)

    End Sub

    Private Sub BotonConfigUplay_Click(sender As Object, e As RoutedEventArgs) Handles botonConfigUplay.Click

        GridConfigVisibilidad(gridConfigUplay, botonConfigUplay)

    End Sub

    Private Sub BotonConfigWindowsStore_Click(sender As Object, e As RoutedEventArgs) Handles botonConfigWindowsStore.Click

        GridConfigVisibilidad(gridConfigWindowsStore, botonConfigWindowsStore)

    End Sub

    Private Sub CbSteamOverlay_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cbSteamOverlay.SelectionChanged

        Dim opciones As ApplicationDataContainer = ApplicationData.Current.LocalSettings
        opciones.Values("SteamOverlay") = cbSteamOverlay.SelectedIndex

    End Sub

    Private Async Sub ButtonBattlenetConfigPath_Click(sender As Object, e As RoutedEventArgs) Handles buttonBattlenetConfigPath.Click

        Dim battleBool As Boolean = Await Blizzard.Config(True)
        Dim carpeta As StorageFolder = Nothing

        Try
            carpeta = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("BattlenetPath")
        Catch ex As Exception

        End Try

        If battleBool = True Then
            listaBattlenet = New List(Of Juego)
            Blizzard.Generar(listaBattlenet, carpeta)
        End If

    End Sub

    Private Async Sub ButtonGOGGalaxyConfigPath_Click(sender As Object, e As RoutedEventArgs) Handles buttonGOGGalaxyConfigPath.Click

        Dim galaxyBool As Boolean = Await GOGGalaxy.Config(True)
        Dim carpeta As StorageFolder = Nothing

        Try
            carpeta = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("GOGGalaxyPath")
        Catch ex As Exception

        End Try

        If galaxyBool = True Then
            listaGOGGalaxy = New List(Of Juego)
            GOGGalaxy.Generar(listaGOGGalaxy, carpeta)
        End If

    End Sub

    Private Async Sub ButtonOriginConfigPath_Click(sender As Object, e As RoutedEventArgs) Handles buttonOriginConfigPath.Click

        Dim originBool As Boolean = Await Origin.Config(True)
        Dim carpeta As StorageFolder = Nothing

        Try
            carpeta = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("OriginPath")
        Catch ex As Exception

        End Try

        If originBool = True Then
            listaOrigin = New List(Of Juego)
            Origin.Generar(listaOrigin, carpeta)
        End If

    End Sub

    Private Async Sub ButtonTwitchConfigPath_Click(sender As Object, e As RoutedEventArgs) Handles buttonTwitchConfigPath.Click

        Dim twitchBool As Boolean = Await Twitch.Config(True)
        Dim carpeta As StorageFolder = Nothing

        Try
            carpeta = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("TwitchPath")
        Catch ex As Exception

        End Try

        If twitchBool = True Then
            listaTwitch = New List(Of Juego)
            Twitch.Generar(listaTwitch, carpeta)
        End If

    End Sub

    Private Async Sub ButtonUplayConfigPathCliente_Click(sender As Object, e As RoutedEventArgs) Handles buttonUplayConfigPathCliente.Click

        Dim uplayBool As Boolean = Await Uplay.Config(0, True)

        Dim carpetaCliente As StorageFolder = Nothing

        Try
            carpetaCliente = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("UplayPathCliente")
        Catch ex As Exception

        End Try

        Dim carpetaJuegos As StorageFolder = Nothing

        Try
            carpetaJuegos = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("UplayPathJuegos")
        Catch ex As Exception

        End Try

        If uplayBool = True Then
            listaUplay = New List(Of Juego)
            Uplay.Generar(listaUplay, carpetaCliente, carpetaJuegos)
        End If

    End Sub

    Private Async Sub ButtonUplayConfigPathJuegos_Click(sender As Object, e As RoutedEventArgs) Handles buttonUplayConfigPathJuegos.Click

        Dim uplayBool As Boolean = Await Uplay.Config(1, True)

        Dim carpetaCliente As StorageFolder = Nothing

        Try
            carpetaCliente = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("UplayPathCliente")
        Catch ex As Exception

        End Try

        Dim carpetaJuegos As StorageFolder = Nothing

        Try
            carpetaJuegos = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("UplayPathJuegos")
        Catch ex As Exception

        End Try

        If uplayBool = True Then
            listaUplay = New List(Of Juego)
            Uplay.Generar(listaUplay, carpetaCliente, carpetaJuegos)
        End If

    End Sub

    Private Async Sub ButtonUplayConfigAviso_Click(sender As Object, e As RoutedEventArgs) Handles buttonUplayConfigAviso.Click

        Try
            Await Launcher.LaunchUriAsync(New Uri("http://i.imgur.com/VFwY7nN.png"))
        Catch ex As Exception

        End Try

    End Sub

    Private Async Sub ButtonWindowsStoreConfigPath_Click(sender As Object, e As RoutedEventArgs) Handles buttonWindowsStoreConfigPath.Click

        Dim windowsBool As Boolean = Await WindowsStore.Config(True)
        Dim carpeta As StorageFolder = Nothing

        Try
            carpeta = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("WindowsStorePath")
        Catch ex As Exception

        End Try

        If windowsBool = True Then
            listaWindowsStore = New List(Of Juego)
            WindowsStore.Generar(listaWindowsStore, carpeta)
        End If

    End Sub

    Private Async Sub ButtonWindowsStoreConfigTutorial_Click(sender As Object, e As RoutedEventArgs) Handles buttonWindowsStoreConfigTutorial.Click

        Try
            Await Launcher.LaunchUriAsync(New Uri("https://www.maketecheasier.com/access-windowsapps-folder-windows-10/"))
        Catch ex As Exception

        End Try

    End Sub

    Private Async Sub BotonBorrarConfig_Click(sender As Object, e As RoutedEventArgs) Handles botonBorrarConfig.Click

        Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()

        Await ApplicationData.Current.ClearAsync()

        Try
            StorageApplicationPermissions.FutureAccessList.Remove("SteamPath")
            buttonSteamConfigPathTexto.Text = recursos.GetString("Boton Añadir")
            tbSteamConfigPath.Text = recursos.GetString("Texto Carpeta")
        Catch ex As Exception

        End Try

        Try
            StorageApplicationPermissions.FutureAccessList.Remove("BattlenetPath")
            lvBlizzard.Items.Clear()
            buttonBattlenetConfigPathTexto.Text = recursos.GetString("Boton Añadir")
            tbBattlenetConfigPath.Text = recursos.GetString("Texto Carpeta")
        Catch ex As Exception

        End Try

        Try
            StorageApplicationPermissions.FutureAccessList.Remove("GOGGalaxyPath")
            lvGOGGalaxy.Items.Clear()
            buttonGOGGalaxyConfigPathTexto.Text = recursos.GetString("Boton Añadir")
            tbGOGGalaxyConfigPath.Text = recursos.GetString("Texto Carpeta")
        Catch ex As Exception

        End Try

        Try
            StorageApplicationPermissions.FutureAccessList.Remove("OriginPath")
            lvOrigin.Items.Clear()
            buttonOriginConfigPathTexto.Text = recursos.GetString("Boton Añadir")
            tbOriginConfigPath.Text = recursos.GetString("Texto Carpeta")
        Catch ex As Exception

        End Try

        Try
            StorageApplicationPermissions.FutureAccessList.Remove("TwitchPath")
            lvTwitch.Items.Clear()
            buttonTwitchConfigPathTexto.Text = recursos.GetString("Boton Añadir")
            tbTwitchConfigPath.Text = recursos.GetString("Texto Carpeta")
        Catch ex As Exception

        End Try

        Try
            StorageApplicationPermissions.FutureAccessList.Remove("UplayPathCliente")
            StorageApplicationPermissions.FutureAccessList.Remove("UplayPathJuegos")
            lvUplay.Items.Clear()
            buttonUplayConfigPathTextoCliente.Text = recursos.GetString("Boton Añadir")
            tbUplayConfigPathCliente.Text = recursos.GetString("Texto Carpeta")
            buttonUplayConfigPathTextoJuegos.Text = recursos.GetString("Boton Añadir")
            tbUplayConfigPathJuegos.Text = recursos.GetString("Texto Carpeta")
        Catch ex As Exception

        End Try

        Try
            StorageApplicationPermissions.FutureAccessList.Remove("WindowsStorePath")
            lvWindowsStore.Items.Clear()
            buttonWindowsStoreConfigPathTexto.Text = recursos.GetString("Boton Añadir")
            tbWindowsStoreConfigPath.Text = recursos.GetString("Texto Carpeta")
        Catch ex As Exception

        End Try

    End Sub

    Private Sub ButtonSteamConfigPath_Click(sender As Object, e As RoutedEventArgs) Handles buttonSteamConfigPath.Click

        Steam.Arranque(tbSteamConfigPath, buttonSteamConfigPathTexto, True)

    End Sub

End Class

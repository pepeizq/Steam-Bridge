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

        'tbBlizzardNoJuegos.Text = recursos.GetString("No Juegos")
        'tbGOGGalaxyNoJuegos.Text = recursos.GetString("No Juegos")
        'tbOriginNoJuegos.Text = recursos.GetString("No Juegos")
        'tbTwitchNoJuegos.Text = recursos.GetString("No Juegos")
        'tbUplayNoJuegos.Text = recursos.GetString("No Juegos")
        'tbWindowsStoreNoJuegos.Text = recursos.GetString("No Juegos")

        'buttonAñadirJuegosTexto.Text = recursos.GetString("Boton Añadir Juegos")
        'tbAvisoAñadirJuegos.Text = recursos.GetString("Aviso Añadir Juegos")

        'tbSteamConfigInstrucciones.Text = recursos.GetString("Texto Steam Config")
        'buttonSteamConfigPathTexto.Text = recursos.GetString("Boton Añadir")
        'tbSteamConfigPath.Text = recursos.GetString("Texto Carpeta")
        'tbSteamOverlayConfigInstrucciones.Text = recursos.GetString("Texto Steam Overlay Config")
        'botonBorrarConfigTexto.Text = recursos.GetString("Borrar Config")


        'tbGOGGalaxyConfigInstrucciones.Text = recursos.GetString("Texto GOG Galaxy Config")
        'buttonGOGGalaxyConfigPathTexto.Text = recursos.GetString("Boton Añadir")
        'tbGOGGalaxyConfigPath.Text = recursos.GetString("Texto Carpeta")

        'tbOriginConfigInstrucciones.Text = recursos.GetString("Texto Origin Config")
        'buttonOriginConfigPathTexto.Text = recursos.GetString("Boton Añadir")
        'tbOriginConfigPath.Text = recursos.GetString("Texto Carpeta")

        'tbTwitchConfigInstrucciones.Text = recursos.GetString("Texto Twitch Config")
        'buttonTwitchConfigPathTexto.Text = recursos.GetString("Boton Añadir")
        'tbTwitchConfigPath.Text = recursos.GetString("Texto Carpeta")

        'tbUplayConfigInstruccionesCliente.Text = recursos.GetString("Texto Uplay Config Cliente")
        'buttonUplayConfigPathTextoCliente.Text = recursos.GetString("Boton Añadir")
        'tbUplayConfigPathCliente.Text = recursos.GetString("Texto Carpeta")
        'tbUplayConfigInstruccionesJuegos.Text = recursos.GetString("Texto Uplay Config Juegos")
        'buttonUplayConfigPathTextoJuegos.Text = recursos.GetString("Boton Añadir")
        'tbUplayConfigPathJuegos.Text = recursos.GetString("Texto Carpeta")
        'tbUplayConfigInstruccionesAviso.Text = recursos.GetString("Texto Uplay Config Aviso")
        'buttonUplayConfigAviso.Content = recursos.GetString("Boton Uplay Config Aviso")

        'tbWindowsStoreConfigInstrucciones.Text = recursos.GetString("Texto Windows Store Config")
        'buttonWindowsStoreConfigPathTexto.Text = recursos.GetString("Boton Añadir")
        'tbWindowsStoreConfigPath.Text = recursos.GetString("Texto Carpeta")
        'buttonWindowsStoreConfigTutorial.Content = recursos.GetString("Boton Windows Store Tutorial")

        '--------------------------------------------------------

        'GridVisibilidad(gridWindowsStore, botonWindowsStore, "Windows Store")
        'EjecutarWindowsStore()

        Dim opciones As ApplicationDataContainer = ApplicationData.Current.LocalSettings

        If opciones.Values("SteamOverlay") = Nothing Then
            opciones.Values("SteamOverlay") = 0
        End If

        cbSteamOverlay.SelectedIndex = opciones.Values("SteamOverlay")

        '------------------------------------------

        Dim battleBool As Boolean = Await Blizzard.Config(False)
        Dim carpeta As StorageFolder = Nothing

        Try
            carpeta = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("BattlenetPath")
        Catch ex As Exception

        End Try

        If battleBool = True Then
            botonBridgeBlizzard.isEnabled = True
        End If

    End Sub

    Private Sub GridVisibilidad(grid As Grid, tag As String)

        tbTitulo.Text = "Steam Bridge (" + SystemInformation.ApplicationVersion.Major.ToString + "." + SystemInformation.ApplicationVersion.Minor.ToString + "." + SystemInformation.ApplicationVersion.Build.ToString + "." + SystemInformation.ApplicationVersion.Revision.ToString + ") - " + tag

        gridBridge.Visibility = Visibility.Collapsed
        gridConfig.Visibility = Visibility.Collapsed
        gridMasCosas.Visibility = Visibility.Collapsed

        grid.Visibility = Visibility.Visible

    End Sub

    Private Sub LvBridgeItemClick(sender As Object, args As ItemClickEventArgs)

        If panelMensajeBridge.Visibility = Visibility.Visible Then
            panelMensajeBridge.Visibility = Visibility.Collapsed
        End If

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

        Dim sp As StackPanel = args.ClickedItem

        If sp.Tag.ToString = 0 Then

            botonBridgeBlizzard.Background = New SolidColorBrush(Colors.DarkMagenta)
            gridBridgeBlizzard.Visibility = Visibility.Visible
            EjecutarBlizzard()

        End If

    End Sub

    'Private Sub BotonGOGGalaxy_Click(sender As Object, e As RoutedEventArgs) Handles botonGOGGalaxy.Click

    '    'GridVisibilidad(gridGOGGalaxy, botonGOGGalaxy, "GOG Galaxy")

    '    If lvGOGGalaxy.Items.Count = 0 Then
    '        EjecutarGOGGalaxy()
    '    End If

    'End Sub

    'Private Sub BotonOrigin_Click(sender As Object, e As RoutedEventArgs) Handles botonOrigin.Click

    '    'GridVisibilidad(gridOrigin, botonOrigin, "Origin")

    '    If lvOrigin.Items.Count = 0 Then
    '        EjecutarOrigin()
    '    End If

    'End Sub

    'Private Sub BotonTwitch_Click(sender As Object, e As RoutedEventArgs) Handles botonTwitch.Click

    '    'GridVisibilidad(gridTwitch, botonTwitch, "Twitch")

    '    If lvTwitch.Items.Count = 0 Then
    '        EjecutarTwitch()
    '    End If

    'End Sub

    'Private Sub BotonUplay_Click(sender As Object, e As RoutedEventArgs) Handles botonUplay.Click

    '    'GridVisibilidad(gridUplay, botonUplay, "Uplay")

    '    If lvUplay.Items.Count = 0 Then
    '        EjecutarUplay()
    '    End If

    'End Sub

    'Private Sub BotonWindowsStore_Click(sender As Object, e As RoutedEventArgs) Handles botonWindowsStore.Click

    '    'GridVisibilidad(gridWindowsStore, botonWindowsStore, "Windows Store")

    '    If lvWindowsStore.Items.Count = 0 Then
    '        EjecutarWindowsStore()
    '    End If

    'End Sub




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

    'CONFIG------------------------------------------------




    Private Sub CbSteamOverlay_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cbSteamOverlay.SelectionChanged

        Dim opciones As ApplicationDataContainer = ApplicationData.Current.LocalSettings
        opciones.Values("SteamOverlay") = cbSteamOverlay.SelectedIndex

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
            'buttonSteamConfigPathTexto.Text = recursos.GetString("Boton Añadir")
            'tbSteamConfigPath.Text = recursos.GetString("Texto Carpeta")
        Catch ex As Exception

        End Try

        Try
            StorageApplicationPermissions.FutureAccessList.Remove("BattlenetPath")
            lvBlizzard.Items.Clear()
            'buttonBattlenetConfigPathTexto.Text = recursos.GetString("Boton Añadir")
            'tbBattlenetConfigPath.Text = recursos.GetString("Texto Carpeta")
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



    'CONFIG-----------------------------------------

    Private Sub BotonSteamRuta_Click(sender As Object, e As RoutedEventArgs) Handles botonSteamRuta.Click

        Steam.Arranque(tbSteamRuta, botonSteamRutaTexto, True)

    End Sub

    Private Async Sub BotonBlizzardRuta_Click(sender As Object, e As RoutedEventArgs) Handles botonBlizzardRuta.Click

        Dim battleBool As Boolean = Await Blizzard.Config(True)
        Dim carpeta As StorageFolder = Nothing

        Try
            carpeta = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("BattlenetPath")
        Catch ex As Exception

        End Try

        If battleBool = True Then
            botonBridgeBlizzard.isEnabled = True
        End If

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

            wvMasCosas.Navigate(New Uri("https://poeditor.com/join/project/YaZAR0uIW4"))

        ElseIf sp.Tag.ToString = 5 Then

            wvMasCosas.Navigate(New Uri("https://github.com/pepeizq/Steam-Bridge"))

        ElseIf sp.Tag.ToString = 6 Then

            wvMasCosas.Navigate(New Uri("https://pepeizqapps.com/thanks/"))

        End If

    End Sub

End Class

Imports Microsoft.Toolkit.Uwp.Helpers
Imports Windows.ApplicationModel.DataTransfer
Imports Windows.Storage
Imports Windows.Storage.AccessCache
Imports Windows.System
Imports Windows.UI

Public NotInheritable Class MainPage
    Inherits Page

    Dim listaBattlenet As List(Of Juego)
    Dim listaGOGGalaxy As List(Of Juego)
    Dim listaOrigin As List(Of Juego)
    Dim listaUplay As List(Of Juego)
    Dim listaWindowsStore As List(Of Juego)

    Private Sub Page_Loaded(sender As FrameworkElement, args As Object)

        Dim barra As ApplicationViewTitleBar = ApplicationView.GetForCurrentView().TitleBar

        barra.BackgroundColor = Colors.DarkMagenta
        barra.ForegroundColor = Colors.White
        barra.InactiveForegroundColor = Colors.White
        barra.ButtonBackgroundColor = Colors.DarkMagenta
        barra.ButtonForegroundColor = Colors.White
        barra.ButtonInactiveForegroundColor = Colors.White

        '--------------------------------------------------------

        Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()

        botonInicioTexto.Text = recursos.GetString("Boton Inicio")
        botonConfigTexto.Text = recursos.GetString("Boton Config")

        commadBarTop.DefaultLabelPosition = CommandBarDefaultLabelPosition.Right

        botonInicioVotarTexto.Text = recursos.GetString("Boton Votar")
        botonInicioCompartirTexto.Text = recursos.GetString("Boton Compartir")
        botonInicioContactoTexto.Text = recursos.GetString("Boton Contactar")
        botonInicioMasAppsTexto.Text = recursos.GetString("Boton Web")

        tbRSS.Text = recursos.GetString("RSS")

        tbBattlenetNoJuegos.Text = recursos.GetString("No Juegos")
        tbGOGGalaxyNoJuegos.Text = recursos.GetString("No Juegos")
        tbOriginNoJuegos.Text = recursos.GetString("No Juegos")
        tbUplayNoJuegos.Text = recursos.GetString("No Juegos")
        tbWindowsStoreNoJuegos.Text = recursos.GetString("No Juegos")

        buttonAñadirJuegosTexto.Text = recursos.GetString("Boton Añadir Juegos")

        botonConfigApp.Content = recursos.GetString("App")
        botonConfigClientes.Content = recursos.GetString("Clientes")
        botonConfigRegistro.Content = recursos.GetString("Registro")

        tbConfigAppTitulo.Text = recursos.GetString("App")
        tbSteamConfigInstrucciones.Text = recursos.GetString("Texto Steam Config")
        buttonSteamConfigPathTexto.Text = recursos.GetString("Boton Añadir")
        tbSteamConfigPath.Text = recursos.GetString("Texto Carpeta")
        tbSteamOverlayConfigInstrucciones.Text = recursos.GetString("Texto Steam Overlay Config")
        botonBorrarConfigTexto.Text = recursos.GetString("Borrar Config")

        tbConfigClientesTitulo.Text = recursos.GetString("Clientes")
        tbBattlenetConfigInstrucciones.Text = recursos.GetString("Texto Battlenet Config")
        buttonBattlenetConfigPathTexto.Text = recursos.GetString("Boton Añadir")
        tbBattlenetConfigPath.Text = recursos.GetString("Texto Carpeta")

        tbGOGGalaxyConfigInstrucciones.Text = recursos.GetString("Texto GOG Galaxy Config")
        buttonGOGGalaxyConfigPathTexto.Text = recursos.GetString("Boton Añadir")
        tbGOGGalaxyConfigPath.Text = recursos.GetString("Texto Carpeta")

        tbOriginConfigInstrucciones.Text = recursos.GetString("Texto Origin Config")
        buttonOriginConfigPathTexto.Text = recursos.GetString("Boton Añadir")
        tbOriginConfigPath.Text = recursos.GetString("Texto Carpeta")

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

        tbConfigRegistroTitulo.Text = recursos.GetString("Registro")
        tbConfigRegistroAviso.Text = recursos.GetString("Registro Aviso")

        '--------------------------------------------------------
        tbConsejoConfig.Text = recursos.GetString("Consejo Config")
        tbInicioGrid.Text = recursos.GetString("Grid Arranque")

        cbItemArranqueInicio.Content = recursos.GetString("Boton Inicio")
        cbItemArranqueConfig.Content = recursos.GetString("Boton Config")

        If ApplicationData.Current.LocalSettings.Values("cbarranque") = Nothing Then
            cbArranque.SelectedIndex = 0
            ApplicationData.Current.LocalSettings.Values("cbarranque") = "0"
        Else
            cbArranque.SelectedIndex = ApplicationData.Current.LocalSettings.Values("cbarranque")

            If cbArranque.SelectedIndex = 0 Then
                GridVisibilidad(gridInicio, botonInicio)
            ElseIf cbArranque.SelectedIndex = 1 Then
                GridVisibilidad(gridBridge, botonBridge)
                EjecutarBattleNet()
            ElseIf cbArranque.SelectedIndex = 2 Then
                GridVisibilidad(gridBridge, botonBridge)
                EjecutarGOGGalaxy()
            ElseIf cbArranque.SelectedIndex = 3 Then
                GridVisibilidad(gridBridge, botonBridge)
                EjecutarOrigin()
            ElseIf cbArranque.SelectedIndex = 4 Then
                GridVisibilidad(gridBridge, botonBridge)
                EjecutarUplay()
            ElseIf cbArranque.SelectedIndex = 5 Then
                GridVisibilidad(gridBridge, botonBridge)
                EjecutarWindowsStore()
            ElseIf cbArranque.SelectedIndex = 6 Then
                GridVisibilidad(gridConfig, botonConfig)
            Else
                GridVisibilidad(gridInicio, botonInicio)
            End If
        End If

        tbVersionApp.Text = "App " + SystemInformation.ApplicationVersion.Major.ToString + "." + SystemInformation.ApplicationVersion.Minor.ToString + "." + SystemInformation.ApplicationVersion.Build.ToString + "." + SystemInformation.ApplicationVersion.Revision.ToString
        tbVersionWindows.Text = "Windows " + SystemInformation.OperatingSystemVersion.Major.ToString + "." + SystemInformation.OperatingSystemVersion.Minor.ToString + "." + SystemInformation.OperatingSystemVersion.Build.ToString + "." + SystemInformation.OperatingSystemVersion.Revision.ToString

        '--------------------------------------------------------

        Try
            RSS.Generar()
        Catch ex As Exception

        End Try

        '--------------------------------------------------------

        Dim opciones As ApplicationDataContainer = ApplicationData.Current.LocalSettings

        If opciones.Values("DOSBoxSteamOverlay") = Nothing Then
            opciones.Values("DOSBoxSteamOverlay") = True
        End If

        If opciones.Values("DOSBoxSteamOverlay") = True Then
            cbDosboxSteamOverlay.IsChecked = True
        ElseIf opciones.Values("DOSBoxSteamOverlay") = False Then
            cbDosboxSteamOverlay.IsChecked = False
        End If

        If opciones.Values("WindowsStoreSteamOverlay") = Nothing Then
            opciones.Values("WindowsStoreSteamOverlay") = True
        End If

        If opciones.Values("WindowsStoreSteamOverlay") = True Then
            cbWindowsStoreSteamOverlay.IsChecked = True
        ElseIf opciones.Values("WindowsStoreSteamOverlay") = False Then
            cbWindowsStoreSteamOverlay.IsChecked = False
        End If

    End Sub

    Private Sub GridVisibilidad(grid As Grid, boton As AppBarButton)

        gridInicio.Visibility = Visibility.Collapsed
        gridBridge.Visibility = Visibility.Collapsed
        gridConfig.Visibility = Visibility.Collapsed
        gridWeb.Visibility = Visibility.Collapsed

        grid.Visibility = Visibility.Visible

        botonInicio.BorderBrush = New SolidColorBrush(Colors.Transparent)
        botonInicio.BorderThickness = New Thickness(0, 0, 0, 0)
        botonBridge.BorderBrush = New SolidColorBrush(Colors.Transparent)
        botonBridge.BorderThickness = New Thickness(0, 0, 0, 0)
        botonConfig.BorderBrush = New SolidColorBrush(Colors.Transparent)
        botonConfig.BorderThickness = New Thickness(0, 0, 0, 0)

        If Not boton Is Nothing Then
            boton.BorderBrush = New SolidColorBrush(Colors.White)
            boton.BorderThickness = New Thickness(0, 2, 0, 0)
        End If

    End Sub

    Private Sub BotonInicio_Click(sender As Object, e As RoutedEventArgs) Handles botonInicio.Click

        GridVisibilidad(gridInicio, botonInicio)

    End Sub

    Private Sub BotonBridge_Click(sender As Object, e As RoutedEventArgs) Handles botonBridge.Click

        GridVisibilidad(gridBridge, botonBridge)
        EjecutarBattleNet()

    End Sub

    Private Sub BotonConfig_Click(sender As Object, e As RoutedEventArgs) Handles botonConfig.Click

        GridVisibilidad(gridConfig, botonConfig)
        GridConfigVisibilidad(gridConfigApp, botonConfigApp)

    End Sub

    Private Async Sub BotonInicioVotar_Click(sender As Object, e As RoutedEventArgs) Handles botonInicioVotar.Click

        Await Launcher.LaunchUriAsync(New Uri("ms-windows-store:REVIEW?PFN=" + Package.Current.Id.FamilyName))

    End Sub

    Private Sub BotonInicioCompartir_Click(sender As Object, e As RoutedEventArgs) Handles botonInicioCompartir.Click

        Dim datos As DataTransferManager = DataTransferManager.GetForCurrentView()
        AddHandler datos.DataRequested, AddressOf MainPage_DataRequested
        DataTransferManager.ShowShareUI()

    End Sub

    Private Sub MainPage_DataRequested(sender As DataTransferManager, e As DataRequestedEventArgs)

        Dim request As DataRequest = e.Request
        request.Data.SetText("Download: https://www.microsoft.com/store/apps/9nblggh441c9")
        request.Data.Properties.Title = "Steam Bridge"
        request.Data.Properties.Description = "Add shortcuts in Steam"

    End Sub

    Private Sub BotonInicioContacto_Click(sender As Object, e As RoutedEventArgs) Handles botonInicioContacto.Click

        GridVisibilidad(gridWeb, Nothing)

    End Sub

    Private Sub BotonInicioMasApps_Click(sender As Object, e As RoutedEventArgs) Handles botonInicioMasApps.Click

        If spMasApps.Visibility = Visibility.Visible Then
            spMasApps.Visibility = Visibility.Collapsed
        Else
            spMasApps.Visibility = Visibility.Visible
        End If

    End Sub

    Private Async Sub BotonAppSteamTiles_Click(sender As Object, e As RoutedEventArgs) Handles botonAppSteamTiles.Click

        Await Launcher.LaunchUriAsync(New Uri("ms-windows-store://pdp/?productid=9nblggh51sb3"))

    End Sub

    Private Async Sub BotonAppSteamDeals_Click(sender As Object, e As RoutedEventArgs) Handles botonAppSteamDeals.Click

        Await Launcher.LaunchUriAsync(New Uri("ms-windows-store://pdp/?productid=9p7836m1tw15"))

    End Sub

    Private Async Sub BotonAppSteamCategories_Click(sender As Object, e As RoutedEventArgs) Handles botonAppSteamCategories.Click

        Await Launcher.LaunchUriAsync(New Uri("ms-windows-store://pdp/?productid=9p54scg1n6bm"))

    End Sub

    Private Async Sub BotonAppSteamSkins_Click(sender As Object, e As RoutedEventArgs) Handles botonAppSteamSkins.Click

        Await Launcher.LaunchUriAsync(New Uri("ms-windows-store://pdp/?productid=9nblggh55b7f"))

    End Sub

    Private Async Sub LvRSSUpdates_ItemClick(sender As Object, e As ItemClickEventArgs) Handles lvRSSUpdates.ItemClick

        Dim feed As FeedRSS = e.ClickedItem
        Await Launcher.LaunchUriAsync(feed.Enlace)

    End Sub

    Private Sub CbArranque_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cbArranque.SelectionChanged

        ApplicationData.Current.LocalSettings.Values("cbarranque") = cbArranque.SelectedIndex

    End Sub

    Private Async Sub BotonSocialTwitter_Click(sender As Object, e As RoutedEventArgs) Handles botonSocialTwitter.Click

        Await Launcher.LaunchUriAsync(New Uri("https://twitter.com/pepeizqapps"))

    End Sub

    Private Async Sub BotonSocialGitHub_Click(sender As Object, e As RoutedEventArgs) Handles botonSocialGitHub.Click

        Await Launcher.LaunchUriAsync(New Uri("https://github.com/pepeizq"))

    End Sub

    Private Async Sub BotonSocialPaypal_Click(sender As Object, e As RoutedEventArgs) Handles botonSocialPaypal.Click

        Await Launcher.LaunchUriAsync(New Uri("https://paypal.me/pepeizq/1"))

    End Sub

    '-----------------------------------------------------------------------------

    Private Sub GridBridgeVisibilidad(grid As Grid, boton As Button)

        gridBattlenet.Visibility = Visibility.Collapsed
        gridGOGGalaxy.Visibility = Visibility.Collapsed
        gridOrigin.Visibility = Visibility.Collapsed
        gridUplay.Visibility = Visibility.Collapsed
        gridWindowsStore.Visibility = Visibility.Collapsed

        grid.Visibility = Visibility.Visible

        botonBridgeBattlenet.BorderBrush = New SolidColorBrush(Colors.Transparent)
        botonBridgeGOG.BorderBrush = New SolidColorBrush(Colors.Transparent)
        botonBridgeOrigin.BorderBrush = New SolidColorBrush(Colors.Transparent)
        botonBridgeUplay.BorderBrush = New SolidColorBrush(Colors.Transparent)
        botonBridgeWindowsStore.BorderBrush = New SolidColorBrush(Colors.Transparent)

        boton.BorderBrush = New SolidColorBrush(Colors.White)

    End Sub

    Private Sub BotonBridgeBattlenet_Click(sender As Object, e As RoutedEventArgs) Handles botonBridgeBattlenet.Click

        EjecutarBattleNet()

    End Sub

    Private Async Sub EjecutarBattleNet()

        GridBridgeVisibilidad(gridBattlenet, botonBridgeBattlenet)

        Dim carpetaBattlenet As StorageFolder = Nothing

        Try
            carpetaBattlenet = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("BattlenetPath")
        Catch ex As Exception

        End Try

        Dim battleBool As Boolean = False

        If Not carpetaBattlenet Is Nothing Then
            battleBool = Await Battlenet.Config(False)

            If battleBool = True Then
                listaBattlenet = New List(Of Juego)
                Battlenet.Generar(listaBattlenet, carpetaBattlenet)
            Else
                tbBattlenetNoJuegos.Visibility = Visibility.Visible
            End If
        Else
            tbBattlenetNoJuegos.Visibility = Visibility.Visible
        End If

    End Sub

    Private Sub BotonBridgeGOG_Click(sender As Object, e As RoutedEventArgs) Handles botonBridgeGOG.Click

        EjecutarGOGGalaxy()

    End Sub

    Private Async Sub EjecutarGOGGalaxy()

        GridBridgeVisibilidad(gridGOGGalaxy, botonBridgeGOG)

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
                tbGOGGalaxyNoJuegos.Visibility = Visibility.Visible
            End If
        Else
            tbGOGGalaxyNoJuegos.Visibility = Visibility.Visible
        End If

    End Sub

    Private Sub BotonBridgeOrigin_Click(sender As Object, e As RoutedEventArgs) Handles botonBridgeOrigin.Click

        EjecutarOrigin()

    End Sub

    Private Async Sub EjecutarOrigin()

        GridBridgeVisibilidad(gridOrigin, botonBridgeOrigin)

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
                tbOriginNoJuegos.Visibility = Visibility.Visible
            End If
        Else
            tbOriginNoJuegos.Visibility = Visibility.Visible
        End If

    End Sub

    Private Sub BotonBridgeUplay_Click(sender As Object, e As RoutedEventArgs) Handles botonBridgeUplay.Click

        EjecutarUplay()

    End Sub

    Private Async Sub EjecutarUplay()

        GridBridgeVisibilidad(gridUplay, botonBridgeUplay)

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
                    tbUplayNoJuegos.Visibility = Visibility.Visible
                End If
            Else
                tbUplayNoJuegos.Visibility = Visibility.Visible
            End If
        Else
            tbUplayNoJuegos.Visibility = Visibility.Visible
        End If

    End Sub

    Private Sub BotonBridgeWindowsStore_Click(sender As Object, e As RoutedEventArgs) Handles botonBridgeWindowsStore.Click

        EjecutarWindowsStore()

    End Sub

    Private Async Sub EjecutarWindowsStore()

        GridBridgeVisibilidad(gridWindowsStore, botonBridgeWindowsStore)

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
                tbWindowsStoreNoJuegos.Visibility = Visibility.Visible
            End If
        Else
            tbWindowsStoreNoJuegos.Visibility = Visibility.Visible
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
                Steam.CrearAccesos(listaFinal, carpetaSteam, buttonAñadirJuegos, tbConfigRegistro)
            End If
        End If

    End Sub

    'CONFIG------------------------------------------------

    Private Sub GridConfigVisibilidad(grid As Grid, boton As Button)

        gridConfigApp.Visibility = Visibility.Collapsed
        gridConfigClientes.Visibility = Visibility.Collapsed
        gridConfigRegistro.Visibility = Visibility.Collapsed

        grid.Visibility = Visibility.Visible

        botonConfigApp.BorderBrush = New SolidColorBrush(Colors.Transparent)
        botonConfigApp.Background = New SolidColorBrush(Colors.Transparent)
        botonConfigClientes.BorderBrush = New SolidColorBrush(Colors.Transparent)
        botonConfigClientes.Background = New SolidColorBrush(Colors.Transparent)
        botonConfigRegistro.BorderBrush = New SolidColorBrush(Colors.Transparent)
        botonConfigRegistro.Background = New SolidColorBrush(Colors.Transparent)

        boton.BorderBrush = New SolidColorBrush(Colors.White)
        boton.Background = New SolidColorBrush(Colors.DarkMagenta)

    End Sub

    Private Sub BotonConfigApp_Click(sender As Object, e As RoutedEventArgs) Handles botonConfigApp.Click

        GridConfigVisibilidad(gridConfigApp, botonConfigApp)

    End Sub

    Private Sub BotonConfigClientes_Click(sender As Object, e As RoutedEventArgs) Handles botonConfigClientes.Click

        GridConfigVisibilidad(gridConfigClientes, botonConfigClientes)

    End Sub

    Private Sub BotonConfigRegistro_Click(sender As Object, e As RoutedEventArgs) Handles botonConfigRegistro.Click

        GridConfigVisibilidad(gridConfigRegistro, botonConfigRegistro)

    End Sub

    Private Sub CbDosboxSteamOverlay_Checked(sender As Object, e As RoutedEventArgs) Handles cbDosboxSteamOverlay.Checked

        Dim opciones As ApplicationDataContainer = ApplicationData.Current.LocalSettings
        opciones.Values("DOSBoxSteamOverlay") = True

    End Sub

    Private Sub CbDosboxSteamOverlay_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbDosboxSteamOverlay.Unchecked

        Dim opciones As ApplicationDataContainer = ApplicationData.Current.LocalSettings
        opciones.Values("DOSBoxSteamOverlay") = False

    End Sub

    Private Sub CbWindowsStoreSteamOverlay_Checked(sender As Object, e As RoutedEventArgs) Handles cbWindowsStoreSteamOverlay.Checked

        Dim opciones As ApplicationDataContainer = ApplicationData.Current.LocalSettings
        opciones.Values("WindowsStoreSteamOverlay") = True

    End Sub

    Private Sub CbWindowsStoreSteamOverlay_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbWindowsStoreSteamOverlay.Unchecked

        Dim opciones As ApplicationDataContainer = ApplicationData.Current.LocalSettings
        opciones.Values("WindowsStoreSteamOverlay") = False

    End Sub

    Private Async Sub ButtonBattlenetConfigPath_Click(sender As Object, e As RoutedEventArgs) Handles buttonBattlenetConfigPath.Click

        Dim battleBool As Boolean = Await Battlenet.Config(True)
        Dim carpeta As StorageFolder = Nothing

        Try
            carpeta = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("BattlenetPath")
        Catch ex As Exception

        End Try

        If battleBool = True Then
            listaBattlenet = New List(Of Juego)
            Battlenet.Generar(listaBattlenet, carpeta)
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
            gridBattlenetContenido.Children.Clear()
            buttonBattlenetConfigPathTexto.Text = recursos.GetString("Boton Añadir")
            tbBattlenetConfigPath.Text = recursos.GetString("Texto Carpeta")
        Catch ex As Exception

        End Try

        Try
            StorageApplicationPermissions.FutureAccessList.Remove("GOGGalaxyPath")
            gridGOGGalaxyContenido.Children.Clear()
            buttonGOGGalaxyConfigPathTexto.Text = recursos.GetString("Boton Añadir")
            tbGOGGalaxyConfigPath.Text = recursos.GetString("Texto Carpeta")
        Catch ex As Exception

        End Try

        Try
            StorageApplicationPermissions.FutureAccessList.Remove("OriginPath")
            gridOriginContenido.Children.Clear()
            buttonOriginConfigPathTexto.Text = recursos.GetString("Boton Añadir")
            tbOriginConfigPath.Text = recursos.GetString("Texto Carpeta")
        Catch ex As Exception

        End Try

        Try
            StorageApplicationPermissions.FutureAccessList.Remove("UplayPathCliente")
            StorageApplicationPermissions.FutureAccessList.Remove("UplayPathJuegos")
            gridUplayContenido.Children.Clear()
            buttonUplayConfigPathTextoCliente.Text = recursos.GetString("Boton Añadir")
            tbUplayConfigPathCliente.Text = recursos.GetString("Texto Carpeta")
            buttonUplayConfigPathTextoJuegos.Text = recursos.GetString("Boton Añadir")
            tbUplayConfigPathJuegos.Text = recursos.GetString("Texto Carpeta")
        Catch ex As Exception

        End Try

        Try
            StorageApplicationPermissions.FutureAccessList.Remove("WindowsStorePath")
            gridWindowsStoreContenido.Children.Clear()
            buttonWindowsStoreConfigPathTexto.Text = recursos.GetString("Boton Añadir")
            tbWindowsStoreConfigPath.Text = recursos.GetString("Texto Carpeta")
        Catch ex As Exception

        End Try

    End Sub

    Private Sub ButtonSteamConfigPath_Click(sender As Object, e As RoutedEventArgs) Handles buttonSteamConfigPath.Click

        Steam.Arranque(tbSteamConfigPath, buttonSteamConfigPathTexto, tbConfigRegistro, True)

    End Sub
End Class

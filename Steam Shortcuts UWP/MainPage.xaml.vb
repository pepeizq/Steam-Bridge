Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Windows.ApplicationModel.DataTransfer
Imports Windows.Storage
Imports Windows.Storage.AccessCache
Imports Windows.System
Imports Windows.UI

Public NotInheritable Class MainPage
    Inherits Page

    Dim coleccion As HamburgerMenuItemCollection = New HamburgerMenuItemCollection

    Dim listaGOGGalaxy As List(Of Juego)
    Dim listaOrigin As List(Of Juego)
    Dim listaUplay As List(Of Juego)
    Dim listaWindowsStore As List(Of Juego)

    Private Async Sub Page_Loaded(sender As FrameworkElement, args As Object)

        Dim barra As ApplicationViewTitleBar = ApplicationView.GetForCurrentView().TitleBar

        barra.BackgroundColor = Colors.DarkMagenta
        barra.ForegroundColor = Colors.White
        barra.InactiveForegroundColor = Colors.White
        barra.ButtonBackgroundColor = Colors.DarkMagenta
        barra.ButtonForegroundColor = Colors.White
        barra.ButtonInactiveForegroundColor = Colors.White

        '--------------------------------------------------------

        Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()

        menuItemConfig.Label = recursos.GetString("Boton Configuracion")
        menuItemVote.Label = recursos.GetString("Boton Votar")
        menuItemShare.Label = recursos.GetString("Boton Compartir")
        menuItemContact.Label = recursos.GetString("Boton Contactar")
        menuItemWeb.Label = recursos.GetString("Boton Web")

        buttonAñadirJuegosTexto.Text = recursos.GetString("Boton Añadir Juegos")
        tbAvisoAñadir.Text = recursos.GetString("Aviso Añadir Juegos")

        tbConfig.Text = recursos.GetString("Boton Configuracion")

        tbSteamConfigInstrucciones.Text = recursos.GetString("Texto Steam Config")
        buttonSteamConfigPathTexto.Text = recursos.GetString("Boton Añadir")
        tbSteamConfigPath.Text = recursos.GetString("Texto Carpeta")

        tbSteamOverlayConfigInstrucciones.Text = recursos.GetString("Texto Steam Overlay Config")

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

        buttonConfigRegistro.Content = recursos.GetString("Registro")
        tbConfigRegistroAviso.Text = recursos.GetString("Registro Aviso")

        '--------------------------------------------------------

        Steam.Arranque(tbSteamConfigPath, buttonSteamConfigPathTexto, tbConfigRegistro, False)

        '--------------------------------------------------------

        Dim carpetaGOGGalaxy As StorageFolder = Nothing

        Try
            carpetaGOGGalaxy = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("GOGGalaxyPath")
        Catch ex As Exception

        End Try

        Dim galaxyBool As Boolean = False

        If Not carpetaGOGGalaxy Is Nothing Then
            galaxyBool = Await GOGGalaxy.Config(tbGOGGalaxyConfigPath, buttonGOGGalaxyConfigPathTexto, tbConfigRegistro, False)

            If galaxyBool = True Then
                listaGOGGalaxy = New List(Of Juego)
                GOGGalaxy.Generar(listaGOGGalaxy, carpetaGOGGalaxy, gridGOGGalaxyContenido, progressBarGOGGalaxy, coleccion, hamburgerMaestro)
            End If
        End If

        '--------------------------------------------------------

        Dim carpetaOrigin As StorageFolder = Nothing

        Try
            carpetaOrigin = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("OriginPath")
        Catch ex As Exception

        End Try

        Dim originBool As Boolean = False

        If Not carpetaOrigin Is Nothing Then
            originBool = Await Origin.Config(tbOriginConfigPath, buttonOriginConfigPathTexto, tbConfigRegistro, False)

            If originBool = True Then
                listaOrigin = New List(Of Juego)
                Origin.Generar(listaOrigin, carpetaOrigin, gridOriginContenido, progressBarOrigin, coleccion, hamburgerMaestro)
            End If
        End If

        '--------------------------------------------------------

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
                uplaybool = Await Uplay.Config(99, tbUplayConfigPathCliente, buttonUplayConfigPathTextoCliente, tbUplayConfigPathJuegos, buttonUplayConfigPathTextoJuegos, tbConfigRegistro, False)

                If uplaybool = True Then
                    listaUplay = New List(Of Juego)
                    Uplay.Generar(listaUplay, carpetaUplayCliente, carpetaUplayJuegos, gridUplayContenido, progressBarUplay, coleccion, hamburgerMaestro)
                End If
            End If
        End If

        '--------------------------------------------------------

        Dim carpetaWindowsStore As StorageFolder = Nothing

        Try
            carpetaWindowsStore = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("WindowsStorePath")
        Catch ex As Exception

        End Try

        Dim windowsbool As Boolean = False

        If Not carpetaWindowsStore Is Nothing Then
            windowsbool = Await WindowsStore.Config(tbWindowsStoreConfigPath, buttonWindowsStoreConfigPathTexto, tbConfigRegistro, False)

            If windowsbool = True Then
                listaWindowsStore = New List(Of Juego)
                WindowsStore.Generar(listaWindowsStore, carpetaWindowsStore, gridWindowsStoreContenido, progressBarWindowsStore, coleccion, hamburgerMaestro)
            End If
        End If

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

        '--------------------------------------------------------

        If galaxyBool = False Then
            If originBool = False Then
                If uplaybool = False Then
                    If windowsbool = False Then
                        GridVisibilidad(gridConfig, False)
                        GridConfigVisibilidad(gridConfigSteam, buttonConfigSteam)
                    Else
                        GridVisibilidad(gridWindowsStore, True)
                    End If
                Else
                    GridVisibilidad(gridUplay, True)
                End If
            Else
                GridVisibilidad(gridOrigin, True)
            End If
        Else
            GridVisibilidad(gridGOGGalaxy, True)
        End If

    End Sub

    Private Sub GridVisibilidad(grid As Grid, barra As Boolean)

        If barra = True Then
            barraInferior.Visibility = Visibility.Visible
        Else
            barraInferior.Visibility = Visibility.Collapsed
        End If

        gridGOGGalaxy.Visibility = Visibility.Collapsed
        gridOrigin.Visibility = Visibility.Collapsed
        gridUplay.Visibility = Visibility.Collapsed
        gridWindowsStore.Visibility = Visibility.Collapsed

        gridConfig.Visibility = Visibility.Collapsed
        gridWebContacto.Visibility = Visibility.Collapsed
        gridWeb.Visibility = Visibility.Collapsed

        grid.Visibility = Visibility.Visible

    End Sub

    Private Async Sub buttonAñadirJuegos_Click(sender As Object, e As RoutedEventArgs) Handles buttonAñadirJuegos.Click

        Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()
        Dim carpetaSteam As StorageFolder = Nothing

        Try
            carpetaSteam = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("SteamPath")
        Catch ex As Exception
            'Notificaciones.Toast("Steam Bridge", recursos.GetString("Texto Steam No Config"))
        End Try

        If Not carpetaSteam Is Nothing Then
            Dim listaFinal As List(Of Juego) = New List(Of Juego)

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

    Private Sub GridConfigVisibilidad(grid As Grid, button As Button)

        buttonConfigSteam.Background = New SolidColorBrush(Microsoft.Toolkit.Uwp.ColorHelper.ToColor("#e3e3e3"))
        buttonConfigSteam.BorderBrush = New SolidColorBrush(Colors.Transparent)
        buttonConfigSteamOverlay.Background = New SolidColorBrush(Microsoft.Toolkit.Uwp.ColorHelper.ToColor("#e3e3e3"))
        buttonConfigSteamOverlay.BorderBrush = New SolidColorBrush(Colors.Transparent)
        buttonConfigGOGGalaxy.Background = New SolidColorBrush(Microsoft.Toolkit.Uwp.ColorHelper.ToColor("#e3e3e3"))
        buttonConfigGOGGalaxy.BorderBrush = New SolidColorBrush(Colors.Transparent)
        buttonConfigOrigin.Background = New SolidColorBrush(Microsoft.Toolkit.Uwp.ColorHelper.ToColor("#e3e3e3"))
        buttonConfigOrigin.BorderBrush = New SolidColorBrush(Colors.Transparent)
        buttonConfigUplay.Background = New SolidColorBrush(Microsoft.Toolkit.Uwp.ColorHelper.ToColor("#e3e3e3"))
        buttonConfigUplay.BorderBrush = New SolidColorBrush(Colors.Transparent)
        buttonConfigWindowsStore.Background = New SolidColorBrush(Microsoft.Toolkit.Uwp.ColorHelper.ToColor("#e3e3e3"))
        buttonConfigWindowsStore.BorderBrush = New SolidColorBrush(Colors.Transparent)
        buttonConfigRegistro.Background = New SolidColorBrush(Microsoft.Toolkit.Uwp.ColorHelper.ToColor("#e3e3e3"))
        buttonConfigRegistro.BorderBrush = New SolidColorBrush(Colors.Transparent)

        button.Background = New SolidColorBrush(Microsoft.Toolkit.Uwp.ColorHelper.ToColor("#bfbfbf"))
        button.BorderBrush = New SolidColorBrush(Colors.Black)

        gridConfigSteam.Visibility = Visibility.Collapsed
        gridConfigSteamOverlay.Visibility = Visibility.Collapsed
        gridConfigGOGGalaxy.Visibility = Visibility.Collapsed
        gridConfigUplay.Visibility = Visibility.Collapsed
        gridConfigOrigin.Visibility = Visibility.Collapsed
        gridConfigWindowsStore.Visibility = Visibility.Collapsed
        gridConfigRegistro.Visibility = Visibility.Collapsed

        grid.Visibility = Visibility.Visible

    End Sub

    Private Sub buttonConfigSteam_Click(sender As Object, e As RoutedEventArgs) Handles buttonConfigSteam.Click

        GridConfigVisibilidad(gridConfigSteam, buttonConfigSteam)

    End Sub

    Private Sub buttonSteamConfigPath_Click(sender As Object, e As RoutedEventArgs) Handles buttonSteamConfigPath.Click

        Steam.Arranque(tbSteamConfigPath, buttonSteamConfigPathTexto, tbConfigRegistro, True)

    End Sub

    Private Sub buttonConfigSteamOverlay_Click(sender As Object, e As RoutedEventArgs) Handles buttonConfigSteamOverlay.Click

        GridConfigVisibilidad(gridConfigSteamOverlay, buttonConfigSteamOverlay)

    End Sub

    Private Sub cbDosboxSteamOverlay_Checked(sender As Object, e As RoutedEventArgs) Handles cbDosboxSteamOverlay.Checked

        Dim opciones As ApplicationDataContainer = ApplicationData.Current.LocalSettings
        opciones.Values("DOSBoxSteamOverlay") = True

    End Sub

    Private Sub cbDosboxSteamOverlay_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbDosboxSteamOverlay.Unchecked

        Dim opciones As ApplicationDataContainer = ApplicationData.Current.LocalSettings
        opciones.Values("DOSBoxSteamOverlay") = False

    End Sub

    Private Sub cbWindowsStoreSteamOverlay_Checked(sender As Object, e As RoutedEventArgs) Handles cbWindowsStoreSteamOverlay.Checked

        Dim opciones As ApplicationDataContainer = ApplicationData.Current.LocalSettings
        opciones.Values("WindowsStoreSteamOverlay") = True

    End Sub

    Private Sub cbWindowsStoreSteamOverlay_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbWindowsStoreSteamOverlay.Unchecked

        Dim opciones As ApplicationDataContainer = ApplicationData.Current.LocalSettings
        opciones.Values("WindowsStoreSteamOverlay") = False

    End Sub

    Private Sub buttonConfigGOGGalaxy_Click(sender As Object, e As RoutedEventArgs) Handles buttonConfigGOGGalaxy.Click

        GridConfigVisibilidad(gridConfigGOGGalaxy, buttonConfigGOGGalaxy)

    End Sub

    Private Async Sub buttonGOGGalaxyConfigPath_Click(sender As Object, e As RoutedEventArgs) Handles buttonGOGGalaxyConfigPath.Click

        Dim galaxyBool As Boolean = Await GOGGalaxy.Config(tbGOGGalaxyConfigPath, buttonGOGGalaxyConfigPathTexto, tbConfigRegistro, True)
        Dim carpeta As StorageFolder = Nothing

        Try
            carpeta = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("GOGGalaxyPath")
        Catch ex As Exception

        End Try

        If galaxyBool = True Then
            listaGOGGalaxy = New List(Of Juego)
            GOGGalaxy.Generar(listaGOGGalaxy, carpeta, gridGOGGalaxyContenido, progressBarGOGGalaxy, coleccion, hamburgerMaestro)
        End If

    End Sub

    Private Sub buttonConfigOrigin_Click(sender As Object, e As RoutedEventArgs) Handles buttonConfigOrigin.Click

        GridConfigVisibilidad(gridConfigOrigin, buttonConfigOrigin)

    End Sub

    Private Async Sub buttonOriginConfigPath_Click(sender As Object, e As RoutedEventArgs) Handles buttonOriginConfigPath.Click

        Dim originBool As Boolean = Await Origin.Config(tbOriginConfigPath, buttonOriginConfigPathTexto, tbConfigRegistro, True)
        Dim carpeta As StorageFolder = Nothing

        Try
            carpeta = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("OriginPath")
        Catch ex As Exception

        End Try

        If originBool = True Then
            listaOrigin = New List(Of Juego)
            Origin.Generar(listaOrigin, carpeta, gridOriginContenido, progressBarOrigin, coleccion, hamburgerMaestro)
        End If

    End Sub

    Private Sub buttonConfigUplay_Click(sender As Object, e As RoutedEventArgs) Handles buttonConfigUplay.Click

        GridConfigVisibilidad(gridConfigUplay, buttonConfigUplay)

    End Sub

    Private Async Sub buttonUplayConfigPathCliente_Click(sender As Object, e As RoutedEventArgs) Handles buttonUplayConfigPathCliente.Click

        Dim uplayBool As Boolean = Await Uplay.Config(0, tbUplayConfigPathCliente, buttonUplayConfigPathTextoCliente, tbUplayConfigPathJuegos, buttonUplayConfigPathTextoJuegos, tbConfigRegistro, True)

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
            Uplay.Generar(listaUplay, carpetaCliente, carpetaJuegos, gridUplayContenido, progressBarUplay, coleccion, hamburgerMaestro)
        End If

    End Sub

    Private Async Sub buttonUplayConfigPathJuegos_Click(sender As Object, e As RoutedEventArgs) Handles buttonUplayConfigPathJuegos.Click

        Dim uplayBool As Boolean = Await Uplay.Config(1, tbUplayConfigPathCliente, buttonUplayConfigPathTextoCliente, tbUplayConfigPathJuegos, buttonUplayConfigPathTextoJuegos, tbConfigRegistro, True)

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
            Uplay.Generar(listaUplay, carpetaCliente, carpetaJuegos, gridUplayContenido, progressBarUplay, coleccion, hamburgerMaestro)
        End If

    End Sub

    Private Async Sub buttonUplayConfigAviso_Click(sender As Object, e As RoutedEventArgs) Handles buttonUplayConfigAviso.Click

        Try
            Await Launcher.LaunchUriAsync(New Uri("http://i.imgur.com/VFwY7nN.png"))
        Catch ex As Exception

        End Try

    End Sub

    Private Sub buttonConfigWindowsStore_Click(sender As Object, e As RoutedEventArgs) Handles buttonConfigWindowsStore.Click

        GridConfigVisibilidad(gridConfigWindowsStore, buttonConfigWindowsStore)

    End Sub

    Private Async Sub buttonWindowsStoreConfigPath_Click(sender As Object, e As RoutedEventArgs) Handles buttonWindowsStoreConfigPath.Click

        Dim windowsBool As Boolean = Await WindowsStore.Config(tbWindowsStoreConfigPath, buttonWindowsStoreConfigPathTexto, tbConfigRegistro, True)
        Dim carpeta As StorageFolder = Nothing

        Try
            carpeta = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("WindowsStorePath")
        Catch ex As Exception

        End Try

        If windowsBool = True Then
            listaWindowsStore = New List(Of Juego)
            WindowsStore.Generar(listaWindowsStore, carpeta, gridWindowsStoreContenido, progressBarWindowsStore, coleccion, hamburgerMaestro)
        End If

    End Sub

    Private Async Sub buttonWindowsStoreConfigTutorial_Click(sender As Object, e As RoutedEventArgs) Handles buttonWindowsStoreConfigTutorial.Click

        Try
            Await Launcher.LaunchUriAsync(New Uri("https://www.maketecheasier.com/access-windowsapps-folder-windows-10/"))
        Catch ex As Exception

        End Try

    End Sub

    Private Sub buttonConfigRegistro_Click(sender As Object, e As RoutedEventArgs) Handles buttonConfigRegistro.Click

        GridConfigVisibilidad(gridConfigRegistro, buttonConfigRegistro)

    End Sub

    'HAMBURGER------------------------------------------------

    Private Sub hamburgerMaestro_ItemClick(sender As Object, e As ItemClickEventArgs) Handles hamburgerMaestro.ItemClick

        Dim menuItem As HamburgerMenuGlyphItem = TryCast(e.ClickedItem, HamburgerMenuGlyphItem)

        If menuItem.Tag = 0 Then
            GridVisibilidad(gridGOGGalaxy, True)
        ElseIf menuItem.Tag = 1 Then
            GridVisibilidad(gridOrigin, True)
        ElseIf menuItem.Tag = 2 Then
            GridVisibilidad(gridUplay, True)
        ElseIf menuItem.Tag = 3 Then
            GridVisibilidad(gridWindowsStore, True)
        End If

    End Sub

    Private Async Sub hamburgerMaestro_OptionsItemClick(sender As Object, e As ItemClickEventArgs) Handles hamburgerMaestro.OptionsItemClick

        Dim menuItem As HamburgerMenuGlyphItem = TryCast(e.ClickedItem, HamburgerMenuGlyphItem)

        If menuItem.Tag = 99 Then
            GridVisibilidad(gridConfig, False)
            GridConfigVisibilidad(gridConfigSteam, buttonConfigSteam)
        ElseIf menuItem.Tag = 100 Then
            Await Launcher.LaunchUriAsync(New Uri("ms-windows-store:REVIEW?PFN=" + Package.Current.Id.FamilyName))
        ElseIf menuItem.Tag = 101 Then
            Dim datos As DataTransferManager = DataTransferManager.GetForCurrentView()
            AddHandler datos.DataRequested, AddressOf MainPage_DataRequested
            DataTransferManager.ShowShareUI()
        ElseIf menuItem.Tag = 102 Then
            GridVisibilidad(gridWebContacto, False)
        ElseIf menuItem.Tag = 103 Then
            GridVisibilidad(gridWeb, False)
        End If

    End Sub

    Private Sub MainPage_DataRequested(sender As DataTransferManager, e As DataRequestedEventArgs)

        Dim request As DataRequest = e.Request
        request.Data.SetText("Steam Bridge")
        request.Data.Properties.Title = "Steam Bridge"
        request.Data.Properties.Description = "Add shortcuts in Steam"

    End Sub


End Class

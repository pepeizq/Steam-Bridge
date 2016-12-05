Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Windows.ApplicationModel.DataTransfer
Imports Windows.Storage
Imports Windows.Storage.AccessCache
Imports Windows.Storage.Pickers
Imports Windows.System
Imports Windows.UI

Public NotInheritable Class MainPage
    Inherits Page

    Dim listaGOGGalaxy As List(Of Juego)
    Dim listaOrigin As List(Of Juego)
    Dim listaWindowsStore As List(Of Juego)

    Private Async Sub Page_Loading(sender As FrameworkElement, args As Object)

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

        tbOriginConfigInstrucciones1.Text = recursos.GetString("Texto Origin Config 1")
        buttonOriginConfigLocalContentPathTexto.Text = recursos.GetString("Boton Añadir")
        tbOriginConfigLocalContentPath.Text = recursos.GetString("Texto Carpeta")
        tbOriginConfigInstrucciones2.Text = recursos.GetString("Texto Origin Config 2")
        buttonOriginConfigGamesPathTexto.Text = recursos.GetString("Boton Añadir")
        tbOriginConfigGamesPath.Text = recursos.GetString("Texto Carpeta")

        tbWindowsStoreConfigInstrucciones.Text = recursos.GetString("Texto Windows Store Config")
        buttonWindowsStoreConfigPathTexto.Text = recursos.GetString("Boton Añadir")
        tbWindowsStoreConfigPath.Text = recursos.GetString("Texto Carpeta")

        '--------------------------------------------------------

        Dim carpetaSteam As StorageFolder = Nothing

        Try
            carpetaSteam = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("SteamPath")

            If Not carpetaSteam Is Nothing Then
                StorageApplicationPermissions.FutureAccessList.AddOrReplace("SteamPath", carpetaSteam)
                tbSteamConfigPath.Text = carpetaSteam.Path

                buttonSteamConfigPathTexto.Text = recursos.GetString("Boton Cambiar")
            End If
        Catch ex As Exception

        End Try

        '--------------------------------------------------------

        Dim carpetaGOGGalaxy As StorageFolder = Nothing

        Try
            carpetaGOGGalaxy = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("GOGGalaxyPath")

            If Not carpetaGOGGalaxy Is Nothing Then
                tbGOGGalaxyConfigPath.Text = carpetaGOGGalaxy.Path
                buttonGOGGalaxyConfigPathTexto.Text = recursos.GetString("Boton Cambiar")

                listaGOGGalaxy = New List(Of Juego)

                GOGGalaxy.Cargar(listaGOGGalaxy, carpetaGOGGalaxy, gridGOGGalaxyContenido, progressBarGOGGalaxy, tbGOGGalaxyMensaje)
            End If
        Catch ex As Exception

        End Try

        '--------------------------------------------------------

        Dim carpetaOriginLocalContent As StorageFolder = Nothing

        Try
            carpetaOriginLocalContent = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("OriginLocalContentPath")

            If Not carpetaOriginLocalContent Is Nothing Then
                tbOriginConfigLocalContentPath.Text = carpetaOriginLocalContent.Path
                buttonOriginConfigLocalContentPathTexto.Text = recursos.GetString("Boton Cambiar")
            End If
        Catch ex As Exception

        End Try

        Dim carpetaOriginGames As StorageFolder = Nothing

        Try
            carpetaOriginGames = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("OriginGamesPath")

            If Not carpetaOriginGames Is Nothing Then
                tbOriginConfigGamesPath.Text = carpetaOriginGames.Path
                buttonOriginConfigGamesPathTexto.Text = recursos.GetString("Boton Cambiar")
            End If
        Catch ex As Exception

        End Try

        If Not carpetaOriginLocalContent Is Nothing Then
            If Not carpetaOriginGames Is Nothing Then
                listaOrigin = New List(Of Juego)

                Origin.Cargar(listaOrigin, carpetaOriginLocalContent, carpetaOriginGames, gridOriginContenido, progressBarOrigin, tbOriginMensaje)
            End If
        End If

        '--------------------------------------------------------

        Dim carpetaWindowsStore As StorageFolder = Nothing

        Try
            carpetaWindowsStore = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("WindowsStorePath")

            If Not carpetaWindowsStore Is Nothing Then
                tbWindowsStoreConfigPath.Text = carpetaWindowsStore.Path
                buttonWindowsStoreConfigPathTexto.Text = recursos.GetString("Boton Cambiar")

                listaWindowsStore = New List(Of Juego)

                WindowsStore.Cargar(listaWindowsStore, carpetaWindowsStore, gridWindowsStoreContenido, progressBarWindowsStore, tbWindowsStoreMensaje)
            End If
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

        '--------------------------------------------------------

        If carpetaGOGGalaxy Is Nothing Then
            tbGOGGalaxyMensaje.Visibility = Visibility.Visible
            tbGOGGalaxyMensaje.Text = recursos.GetString("Texto GOG Galaxy No Config")
        End If

        If carpetaOriginLocalContent Is Nothing Then
            If carpetaOriginGames Is Nothing Then
                tbOriginMensaje.Visibility = Visibility.Visible
                tbOriginMensaje.Text = recursos.GetString("Texto Origin No Config")
            End If
        End If

        If carpetaWindowsStore Is Nothing Then
            tbWindowsStoreMensaje.Visibility = Visibility.Visible
            tbWindowsStoreMensaje.Text = recursos.GetString("Texto Windows Store No Config")
        End If

        '--------------------------------------------------------

        If carpetaGOGGalaxy Is Nothing Then
            If carpetaOriginLocalContent Is Nothing And carpetaOriginGames Is Nothing Then
                If carpetaWindowsStore Is Nothing Then
                    GridVisibilidad(gridConfig, False)
                Else
                    hamburgerMaestro.SelectedIndex = 2
                    GridVisibilidad(gridWindowsStore, True)
                End If
            Else
                hamburgerMaestro.SelectedIndex = 1
                GridVisibilidad(gridOrigin, True)
            End If
        Else
            hamburgerMaestro.SelectedIndex = 0
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
            MessageBox.EnseñarMensaje(recursos.GetString("Texto Steam No Config"))
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
        buttonConfigWindowsStore.Background = New SolidColorBrush(Microsoft.Toolkit.Uwp.ColorHelper.ToColor("#e3e3e3"))
        buttonConfigWindowsStore.BorderBrush = New SolidColorBrush(Colors.Transparent)

        button.Background = New SolidColorBrush(Microsoft.Toolkit.Uwp.ColorHelper.ToColor("#bfbfbf"))
        button.BorderBrush = New SolidColorBrush(Colors.Black)

        gridConfigSteam.Visibility = Visibility.Collapsed
        gridConfigSteamOverlay.Visibility = Visibility.Collapsed
        gridConfigGOGGalaxy.Visibility = Visibility.Collapsed
        gridConfigOrigin.Visibility = Visibility.Collapsed
        gridConfigWindowsStore.Visibility = Visibility.Collapsed

        grid.Visibility = Visibility.Visible

    End Sub

    Private Sub buttonConfigSteam_Click(sender As Object, e As RoutedEventArgs) Handles buttonConfigSteam.Click

        GridConfigVisibilidad(gridConfigSteam, buttonConfigSteam)

    End Sub

    Private Async Sub buttonSteamConfigPath_Click(sender As Object, e As RoutedEventArgs) Handles buttonSteamConfigPath.Click

        Dim carpetaSteam As StorageFolder

        Try
            Dim picker As FolderPicker = New FolderPicker()

            picker.FileTypeFilter.Add("*")
            picker.ViewMode = PickerViewMode.List

            carpetaSteam = Await picker.PickSingleFolderAsync()

            If Not carpetaSteam Is Nothing Then
                StorageApplicationPermissions.FutureAccessList.AddOrReplace("SteamPath", carpetaSteam)
                tbSteamConfigPath.Text = carpetaSteam.Path

                Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()
                buttonSteamConfigPathTexto.Text = recursos.GetString("Boton Cambiar")
            End If
        Catch ex As Exception

        End Try

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

        Dim carpetaGOGGalaxy As StorageFolder

        Try
            Dim picker As FolderPicker = New FolderPicker()

            picker.FileTypeFilter.Add("*")
            picker.ViewMode = PickerViewMode.List

            carpetaGOGGalaxy = Await picker.PickSingleFolderAsync()

            If Not carpetaGOGGalaxy Is Nothing Then
                StorageApplicationPermissions.FutureAccessList.AddOrReplace("GOGGalaxyPath", carpetaGOGGalaxy)
                tbGOGGalaxyConfigPath.Text = carpetaGOGGalaxy.Path

                Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()
                buttonGOGGalaxyConfigPathTexto.Text = recursos.GetString("Boton Cambiar")

                listaGOGGalaxy = New List(Of Juego)

                GOGGalaxy.Cargar(listaGOGGalaxy, carpetaGOGGalaxy, gridGOGGalaxyContenido, progressBarGOGGalaxy, tbGOGGalaxyMensaje)
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub buttonConfigOrigin_Click(sender As Object, e As RoutedEventArgs) Handles buttonConfigOrigin.Click

        GridConfigVisibilidad(gridConfigOrigin, buttonConfigOrigin)

    End Sub

    Private Async Sub buttonOriginConfigLocalContentPath_Click(sender As Object, e As RoutedEventArgs) Handles buttonOriginConfigLocalContentPath.Click

        Dim carpetaOriginLocalContent As StorageFolder

        Try
            Dim picker As FolderPicker = New FolderPicker()

            picker.FileTypeFilter.Add("*")
            picker.ViewMode = PickerViewMode.List

            carpetaOriginLocalContent = Await picker.PickSingleFolderAsync()

            If Not carpetaOriginLocalContent Is Nothing Then
                StorageApplicationPermissions.FutureAccessList.AddOrReplace("OriginLocalContentPath", carpetaOriginLocalContent)
                tbOriginConfigLocalContentPath.Text = carpetaOriginLocalContent.Path

                Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()
                buttonOriginConfigLocalContentPathTexto.Text = recursos.GetString("Boton Cambiar")

                Dim carpetaOriginGames As StorageFolder = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("OriginGamesPath")

                listaOrigin = New List(Of Juego)

                Origin.Cargar(listaOrigin, carpetaOriginLocalContent, carpetaOriginGames, gridOriginContenido, progressBarOrigin, tbOriginMensaje)
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Async Sub buttonOriginConfigGamesPath_Click(sender As Object, e As RoutedEventArgs) Handles buttonOriginConfigGamesPath.Click

        Dim carpetaOriginGames As StorageFolder

        Try
            Dim picker As FolderPicker = New FolderPicker()

            picker.FileTypeFilter.Add("*")
            picker.ViewMode = PickerViewMode.List

            carpetaOriginGames = Await picker.PickSingleFolderAsync()

            If Not carpetaOriginGames Is Nothing Then
                StorageApplicationPermissions.FutureAccessList.AddOrReplace("OriginGamesPath", carpetaOriginGames)
                tbOriginConfigGamesPath.Text = carpetaOriginGames.Path

                Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()
                buttonOriginConfigGamesPathTexto.Text = recursos.GetString("Boton Cambiar")

                Dim carpetaOriginLocalContent As StorageFolder = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("OriginLocalContentPath")

                listaOrigin = New List(Of Juego)

                Origin.Cargar(listaOrigin, carpetaOriginLocalContent, carpetaOriginGames, gridOriginContenido, progressBarOrigin, tbOriginMensaje)
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub buttonConfigWindowsStore_Click(sender As Object, e As RoutedEventArgs) Handles buttonConfigWindowsStore.Click

        GridConfigVisibilidad(gridConfigWindowsStore, buttonConfigWindowsStore)

    End Sub

    Private Async Sub buttonWindowsStoreConfigPath_Click(sender As Object, e As RoutedEventArgs) Handles buttonWindowsStoreConfigPath.Click

        Dim carpetaWindowsStore As StorageFolder

        Try
            Dim picker As FolderPicker = New FolderPicker()

            picker.FileTypeFilter.Add("*")
            picker.ViewMode = PickerViewMode.List

            carpetaWindowsStore = Await picker.PickSingleFolderAsync()

            If Not carpetaWindowsStore Is Nothing Then
                StorageApplicationPermissions.FutureAccessList.AddOrReplace("WindowsStorePath", carpetaWindowsStore)
                tbWindowsStoreConfigPath.Text = carpetaWindowsStore.Path

                Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()
                buttonWindowsStoreConfigPathTexto.Text = recursos.GetString("Boton Cambiar")

                listaWindowsStore = New List(Of Juego)

                WindowsStore.Cargar(listaWindowsStore, carpetaWindowsStore, gridWindowsStoreContenido, progressBarWindowsStore, tbWindowsStoreMensaje)
            End If
        Catch ex As Exception

        End Try

    End Sub

    'HAMBURGER------------------------------------------------

    Private Sub hamburgerMaestro_ItemClick(sender As Object, e As ItemClickEventArgs) Handles hamburgerMaestro.ItemClick

        Dim menuItem As HamburgerMenuGlyphItem = TryCast(e.ClickedItem, HamburgerMenuGlyphItem)

        If menuItem.Tag = 0 Then
            GridVisibilidad(gridGOGGalaxy, True)
        ElseIf menuItem.Tag = 1 Then
            GridVisibilidad(gridOrigin, True)
        ElseIf menuItem.Tag = 2 Then
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

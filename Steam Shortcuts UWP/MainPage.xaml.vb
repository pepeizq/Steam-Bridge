Imports Windows.ApplicationModel.DataTransfer
Imports Windows.Storage
Imports Windows.Storage.AccessCache
Imports Windows.Storage.Pickers
Imports Windows.System
Imports Windows.UI

Public NotInheritable Class MainPage
    Inherits Page

    Dim listaGOGGalaxy As List(Of Juego)
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

        buttonConfigTexto.Text = recursos.GetString("Boton Configuracion")
        buttonVolverTexto.Text = recursos.GetString("Boton Volver")
        buttonVotacionesTexto.Text = recursos.GetString("Boton Votar")
        buttonCompartirTexto.Text = recursos.GetString("Boton Compartir")
        buttonContactarTexto.Text = recursos.GetString("Boton Contactar")
        buttonWebTexto.Text = recursos.GetString("Boton Web")

        buttonAñadirJuegosTexto.Text = recursos.GetString("Boton Añadir Juegos")
        tbAvisoAñadir.Text = recursos.GetString("Aviso Añadir Juegos")

        tbConfig.Text = recursos.GetString("Boton Configuracion")

        tbSteamConfigInstrucciones.Text = recursos.GetString("Texto Steam Config")
        buttonSteamConfigPathTexto.Text = recursos.GetString("Boton Añadir")
        tbSteamConfigPath.Text = recursos.GetString("Texto Carpeta")

        tbGOGGalaxyConfigInstrucciones.Text = recursos.GetString("Texto GOG Galaxy Config")
        buttonGOGGalaxyConfigPathTexto.Text = recursos.GetString("Boton Añadir")
        tbGOGGalaxyConfigPath.Text = recursos.GetString("Texto Carpeta")

        tbWindowsStoreConfigInstrucciones.Text = recursos.GetString("Texto Windows Store Config")
        buttonWindowsStoreConfigPathTexto.Text = recursos.GetString("Boton Añadir")
        tbWindowsStoreConfigPath.Text = recursos.GetString("Texto Carpeta")
        cbWindowsStoreSteamOverlay.Content = recursos.GetString("Checkbox Windows Store Steam Overlay")

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

                GOGGalaxy.Cargar(listaGOGGalaxy, carpetaGOGGalaxy, pivotMaestro, progressBarGeneral)
            End If
        Catch ex As Exception

        End Try

        '--------------------------------------------------------

        Dim carpetaWindowsStore As StorageFolder = Nothing

        Try
            carpetaWindowsStore = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("WindowsStorePath")

            If Not carpetaWindowsStore Is Nothing Then
                tbWindowsStoreConfigPath.Text = carpetaWindowsStore.Path
                buttonWindowsStoreConfigPathTexto.Text = recursos.GetString("Boton Cambiar")

                listaWindowsStore = New List(Of Juego)

                WindowsStore.Cargar(listaWindowsStore, carpetaWindowsStore, pivotMaestro, progressBarGeneral)
            End If
        Catch ex As Exception

        End Try

        Dim opciones As ApplicationDataContainer = ApplicationData.Current.LocalSettings

        If opciones.Values("WindowsStoreSteamOverlay") = Nothing Then
            opciones.Values("WindowsStoreSteamOverlay") = True
        End If

        If opciones.Values("WindowsStoreSteamOverlay") = True Then
            cbWindowsStoreSteamOverlay.IsChecked = True
        ElseIf opciones.Values("WindowsStoreSteamOverlay") = False Then
            cbWindowsStoreSteamOverlay.IsChecked = False
        End If

        '--------------------------------------------------------

        If carpetaSteam Is Nothing Then
            If carpetaGOGGalaxy Is Nothing Then
                If carpetaWindowsStore Is Nothing Then
                    gridConfig.Visibility = Visibility.Visible
                    gridWeb.Visibility = Visibility.Collapsed
                    gridWebContacto.Visibility = Visibility.Collapsed
                    pivotPrincipal.Visibility = Visibility.Collapsed

                    buttonVolver.Visibility = Visibility.Visible
                    buttonConfig.Visibility = Visibility.Collapsed
                End If
            End If
        End If

    End Sub

    Private Async Sub buttonAñadirJuegos_Click(sender As Object, e As RoutedEventArgs) Handles buttonAñadirJuegos.Click

        Dim carpetaSteam As StorageFolder

        Try
            carpetaSteam = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("SteamPath")

            If Not carpetaSteam Is Nothing Then
                Dim listaFinal As List(Of Juego) = New List(Of Juego)

                If listaGOGGalaxy.Count > 0 Then
                    For Each juego As Juego In listaGOGGalaxy
                        If juego.Añadir = True Then
                            listaFinal.Add(juego)
                        End If
                    Next
                End If

                If listaWindowsStore.Count > 0 Then
                    For Each juego As Juego In listaWindowsStore
                        If juego.Añadir = True Then
                            listaFinal.Add(juego)
                        End If
                    Next
                End If

                If listaFinal.Count > 0 Then
                    Steam.CrearAccesos(listaFinal, carpetaSteam, buttonAñadirJuegos)
                End If
            End If
        Catch ex As Exception

        End Try

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

                GOGGalaxy.Cargar(listaGOGGalaxy, carpetaGOGGalaxy, pivotMaestro, progressBarGeneral)
            End If
        Catch ex As Exception

        End Try

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

                WindowsStore.Cargar(listaWindowsStore, carpetaWindowsStore, pivotMaestro, progressBarGeneral)
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub cbWindowsStoreSteamOverlay_Checked(sender As Object, e As RoutedEventArgs) Handles cbWindowsStoreSteamOverlay.Checked

        Dim opciones As ApplicationDataContainer = ApplicationData.Current.LocalSettings
        opciones.Values("WindowsStoreSteamOverlay") = True

    End Sub

    Private Sub cbWindowsStoreSteamOverlay_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbWindowsStoreSteamOverlay.Unchecked

        Dim opciones As ApplicationDataContainer = ApplicationData.Current.LocalSettings
        opciones.Values("WindowsStoreSteamOverlay") = False

    End Sub

    Private Sub buttonConfig_Click(sender As Object, e As RoutedEventArgs) Handles buttonConfig.Click

        gridConfig.Visibility = Visibility.Visible
        gridWeb.Visibility = Visibility.Collapsed
        gridWebContacto.Visibility = Visibility.Collapsed
        pivotPrincipal.Visibility = Visibility.Collapsed

        buttonVolver.Visibility = Visibility.Visible
        buttonConfig.Visibility = Visibility.Collapsed

    End Sub

    'VOTAR-----------------------------------------------------------------------------

    Private Async Sub buttonVotaciones_Click(sender As Object, e As RoutedEventArgs) Handles buttonVotaciones.Click

        Await Launcher.LaunchUriAsync(New Uri("ms-windows-store:REVIEW?PFN=" + Package.Current.Id.FamilyName))

    End Sub

    'COMPARTIR-----------------------------------------------------------------------------

    Private Sub buttonCompartir_Click(sender As Object, e As RoutedEventArgs) Handles buttonCompartir.Click

        Dim datos As DataTransferManager = DataTransferManager.GetForCurrentView()
        AddHandler datos.DataRequested, AddressOf MainPage_DataRequested
        DataTransferManager.ShowShareUI()

    End Sub

    Private Sub MainPage_DataRequested(sender As DataTransferManager, e As DataRequestedEventArgs)

        Dim request As DataRequest = e.Request
        request.Data.SetText("Steam Bridge")
        request.Data.Properties.Title = "Steam Bridge"
        request.Data.Properties.Description = "Add shortcuts in Steam of games from Windows Store"

    End Sub

    'CONTACTAR-----------------------------------------------------------------------------

    Private Sub buttonContactar_Click(sender As Object, e As RoutedEventArgs) Handles buttonContactar.Click

        gridConfig.Visibility = Visibility.Collapsed
        gridWeb.Visibility = Visibility.Collapsed
        gridWebContacto.Visibility = Visibility.Visible
        pivotPrincipal.Visibility = Visibility.Collapsed

        buttonVolver.Visibility = Visibility.Visible
        buttonConfig.Visibility = Visibility.Collapsed

    End Sub

    'VOLVER-----------------------------------------------------------------------------

    Private Sub buttonVolver_Click(sender As Object, e As RoutedEventArgs) Handles buttonVolver.Click

        gridConfig.Visibility = Visibility.Collapsed
        gridWeb.Visibility = Visibility.Collapsed
        gridWebContacto.Visibility = Visibility.Collapsed
        pivotPrincipal.Visibility = Visibility.Visible

        buttonVolver.Visibility = Visibility.Collapsed
        buttonConfig.Visibility = Visibility.Visible

    End Sub

    'WEB-----------------------------------------------------------------------------

    Private Sub buttonWeb_Click(sender As Object, e As RoutedEventArgs) Handles buttonWeb.Click

        gridConfig.Visibility = Visibility.Collapsed
        gridWeb.Visibility = Visibility.Visible
        gridWebContacto.Visibility = Visibility.Collapsed
        pivotPrincipal.Visibility = Visibility.Collapsed

        buttonVolver.Visibility = Visibility.Visible
        buttonConfig.Visibility = Visibility.Collapsed

    End Sub


End Class

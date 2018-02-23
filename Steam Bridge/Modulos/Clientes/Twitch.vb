Imports SQLite.Net
Imports SQLite.Net.Platform.WinRT
Imports Windows.Storage
Imports Windows.Storage.AccessCache
Imports Windows.Storage.Pickers

Module Twitch

    Public Async Sub Config(picker As Boolean)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim recursos As New Resources.ResourceLoader()
        Dim carpeta As StorageFolder = Nothing

        Try
            If picker = True Then
                Dim carpetapicker As FolderPicker = New FolderPicker()

                carpetapicker.FileTypeFilter.Add("*")
                carpetapicker.ViewMode = PickerViewMode.List

                carpeta = Await carpetapicker.PickSingleFolderAsync()
            Else
                carpeta = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("TwitchPath")
            End If

            If Not carpeta Is Nothing Then
                If carpeta.Path.Contains("AppData\Roaming\Twitch") Then
                    StorageApplicationPermissions.FutureAccessList.AddOrReplace("TwitchPath", carpeta)

                    Dim tbTwitchRuta As TextBlock = pagina.FindName("tbTwitchRuta")
                    tbTwitchRuta.Text = carpeta.Path

                    Dim botonTwitchRutaTexto As TextBlock = pagina.FindName("botonTwitchRutaTexto")
                    botonTwitchRutaTexto.Text = recursos.GetString("Change")

                    Dim gv As GridView = pagina.FindName("gvBridge")

                    For Each item As GridViewItem In gv.Items
                        Dim grid As Grid = item.Content
                        Dim plataforma As Plataforma = grid.Tag

                        If plataforma.Nombre = "Twitch Desktop App" Then
                            item.IsEnabled = True
                        End If
                    Next
                End If
            End If
        Catch ex As Exception

        End Try

    End Sub

    Public Async Sub Generar(pb As ProgressBar, lv As ListView)

        lv.IsEnabled = False
        pb.Visibility = Visibility.Visible

        Dim listaJuegos As New List(Of Juego)

        Dim carpeta As StorageFolder = Nothing

        Try
            carpeta = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("TwitchPath")
        Catch ex As Exception

        End Try

        If Not carpeta Is Nothing Then
            Dim carpetaJuegos As StorageFolder = Await carpeta.GetFolderAsync("Games\Sql")

            If Not carpetaJuegos Is Nothing Then
                Dim fichero As String = carpetaJuegos.Path + "\GameProductInfo.sqlite"

                Dim bdOrigen As StorageFile = Await StorageFile.GetFileFromPathAsync(fichero)
                Dim bdFinal As StorageFile = Await ApplicationData.Current.LocalFolder.CreateFileAsync("basedatos.sqlite", CreationCollisionOption.ReplaceExisting)

                Await bdOrigen.CopyAndReplaceAsync(bdFinal)

                Dim conexion As New SQLiteConnection(New SQLitePlatformWinRT, bdFinal.Path, Interop.SQLiteOpenFlags.ReadWrite)

                Dim juegos As TableQuery(Of TwitchDB) = conexion.Table(Of TwitchDB)

                For Each juegoTwitch As TwitchDB In juegos
                    Dim ejecutable As String = "twitch://fuel-launch/" + juegoTwitch.Id

                    Dim juego As New Juego(juegoTwitch.Titulo, ejecutable, Nothing, Nothing, Nothing, False, "Twitch Desktop App")

                    listaJuegos.Add(juego)

                    lv.Items.Add(InterfazListado.GenerarGrid(juego, Nothing, False))
                Next
            End If
        End If

        If listaJuegos.Count > 0 Then
            lv.Items.Clear()
            listaJuegos.Sort(Function(x, y) x.Nombre.CompareTo(y.Nombre))

            For Each juego As Juego In listaJuegos
                'Dim bitmap As BitmapImage = New BitmapImage
                'If Not juego.Icono Is Nothing Then
                '    Using stream As IRandomAccessStream = Await juego.Icono.OpenAsync(FileAccessMode.Read)
                '        bitmap.DecodePixelWidth = 40
                '        bitmap.DecodePixelHeight = 40
                '        Await bitmap.SetSourceAsync(stream)
                '    End Using
                'End If

                lv.Items.Add(InterfazListado.GenerarGrid(juego, Nothing, True))
            Next
        End If

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim avisoNoJuegos As Grid = pagina.FindName("gridAvisoJuegos")

        If listaJuegos.Count = 0 Then
            avisoNoJuegos.Visibility = Visibility.Visible
        Else
            avisoNoJuegos.Visibility = Visibility.Collapsed
        End If

        lv.IsEnabled = True
        pb.Visibility = Visibility.Collapsed

    End Sub

End Module

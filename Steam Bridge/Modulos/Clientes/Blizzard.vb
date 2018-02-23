Imports Windows.Graphics.Imaging
Imports Windows.Storage
Imports Windows.Storage.AccessCache
Imports Windows.Storage.FileProperties
Imports Windows.Storage.Pickers
Imports Windows.Storage.Streams

Module Blizzard

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
                carpeta = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("BattlenetPath")
            End If

            If Not carpeta Is Nothing Then
                Dim carpetasJuegos As IReadOnlyList(Of StorageFolder) = Await carpeta.GetFoldersAsync()
                Dim detectadoBool As Boolean = False

                For Each carpetaJuego As StorageFolder In carpetasJuegos
                    Dim ficheros As IReadOnlyList(Of StorageFile) = Await carpetaJuego.GetFilesAsync()
                    Dim temp As String = Nothing

                    For Each fichero As StorageFile In ficheros
                        Dim nombreFichero As String = fichero.DisplayName.ToLower

                        If nombreFichero = "destiny2" And fichero.FileType = ".exe" Then
                            detectadoBool = True
                            GenerarIcono(fichero, carpetaJuego)
                        End If

                        If nombreFichero = "diablo iii" And fichero.FileType = ".exe" Then
                            detectadoBool = True
                            GenerarIcono(fichero, carpetaJuego)
                        End If

                        If nombreFichero = "hearthstone" And fichero.FileType = ".exe" Then
                            detectadoBool = True
                            GenerarIcono(fichero, carpetaJuego)
                        End If

                        If nombreFichero = "heroes of the storm" And fichero.FileType = ".exe" Then
                            detectadoBool = True
                            GenerarIcono(fichero, carpetaJuego)
                        End If

                        If nombreFichero = "overwatch" And fichero.FileType = ".exe" Then
                            detectadoBool = True
                            GenerarIcono(fichero, carpetaJuego)
                        End If

                        If nombreFichero = "starcraft" And fichero.FileType = ".exe" Then
                            detectadoBool = True
                            GenerarIcono(fichero, carpetaJuego)
                        End If

                        If nombreFichero = "sc2" And fichero.FileType = ".exe" Then
                            detectadoBool = True
                            GenerarIcono(fichero, carpetaJuego)
                        End If

                        If nombreFichero = "starcraft ii" And fichero.FileType = ".exe" Then
                            detectadoBool = True
                            GenerarIcono(fichero, carpetaJuego)
                        End If

                        If nombreFichero = "wow" And fichero.FileType = ".exe" Then
                            detectadoBool = True
                            GenerarIcono(fichero, carpetaJuego)
                        End If
                    Next
                Next

                If detectadoBool = True Then
                    StorageApplicationPermissions.FutureAccessList.AddOrReplace("BattlenetPath", carpeta)

                    Dim tbBlizzardRuta As TextBlock = pagina.FindName("tbBlizzardRuta")
                    tbBlizzardRuta.Text = carpeta.Path

                    Dim botonBlizzardRutaTexto As TextBlock = pagina.FindName("botonBlizzardRutaTexto")
                    botonBlizzardRutaTexto.Text = recursos.GetString("Change")

                    Dim gv As GridView = pagina.FindName("gvBridge")

                    For Each item As GridViewItem In gv.Items
                        Dim grid As Grid = item.Content
                        Dim plataforma As Plataforma = grid.Tag

                        If plataforma.Nombre = "Blizzard App" Then
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
            carpeta = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("BattlenetPath")
        Catch ex As Exception

        End Try

        If Not carpeta Is Nothing Then
            Dim carpetasJuegos As IReadOnlyList(Of StorageFolder) = Await carpeta.GetFoldersAsync()

            For Each carpetaJuego As StorageFolder In carpetasJuegos
                Dim ficheros As IReadOnlyList(Of StorageFile) = Await carpetaJuego.GetFilesAsync()

                For Each fichero As StorageFile In ficheros
                    Dim nombre As String = carpetaJuego.Name

                    Dim ejecutable As String = Nothing
                    Dim argumentos As String = Nothing

                    Dim nombreFichero As String = fichero.DisplayName.ToLower

                    If nombreFichero = "destiny2" And fichero.FileType = ".exe" Then
                        ejecutable = "battlenet://DST2"
                    End If

                    If nombreFichero = "diablo iii" And fichero.FileType = ".exe" Then
                        ejecutable = "battlenet://D3"
                    End If

                    If nombreFichero = "hearthstone" And fichero.FileType = ".exe" Then
                        ejecutable = "battlenet://WTCG"
                    End If

                    If nombreFichero = "heroes of the storm" And fichero.FileType = ".exe" Then
                        ejecutable = "battlenet://Hero"
                    End If

                    If nombreFichero = "overwatch" And fichero.FileType = ".exe" Then
                        ejecutable = "battlenet://Pro"
                    End If

                    If nombreFichero = "starcraft" And fichero.FileType = ".exe" Then
                        ejecutable = "battlenet://S1"
                    End If

                    If nombreFichero = "sc2" And fichero.FileType = ".exe" Then
                        ejecutable = "battlenet://S2"
                    End If

                    If nombreFichero = "starcraft ii" And fichero.FileType = ".exe" Then
                        ejecutable = "battlenet://S2"
                    End If

                    If nombreFichero = "wow" And fichero.FileType = ".exe" Then
                        ejecutable = "battlenet://WoW"
                    End If

                    If Not ejecutable = Nothing Then
                        Dim tempIcono As String = fichero.Path.Replace(".exe", ".png")
                        Dim icono As StorageFile = Nothing

                        Try
                            icono = Await StorageFile.GetFileFromPathAsync(tempIcono)
                        Catch ex As Exception

                        End Try

                        Dim juego As New Juego(nombre, ejecutable, argumentos, icono, Nothing, False, "Blizzard App")

                        listaJuegos.Add(juego)

                        Dim bitmap As BitmapImage = New BitmapImage
                        If Not juego.Icono Is Nothing Then
                            Try
                                Using stream As IRandomAccessStream = Await juego.Icono.OpenAsync(FileAccessMode.Read)
                                    bitmap.DecodePixelWidth = 40
                                    bitmap.DecodePixelHeight = 40
                                    Await bitmap.SetSourceAsync(stream)
                                End Using
                            Catch ex As Exception
                                bitmap = Nothing
                            End Try
                        End If

                        lv.Items.Add(InterfazListado.GenerarGrid(juego, bitmap, False))
                    End If
                Next
            Next
        End If

        If listaJuegos.Count > 0 Then
            lv.Items.Clear()
            listaJuegos.Sort(Function(x, y) x.Nombre.CompareTo(y.Nombre))

            For Each juego As Juego In listaJuegos
                Dim bitmap As BitmapImage = New BitmapImage
                If Not juego.Icono Is Nothing Then
                    Try
                        Using stream As IRandomAccessStream = Await juego.Icono.OpenAsync(FileAccessMode.Read)
                            bitmap.DecodePixelWidth = 40
                            bitmap.DecodePixelHeight = 40
                            Await bitmap.SetSourceAsync(stream)
                        End Using
                    Catch ex As Exception
                        bitmap = Nothing
                    End Try
                End If

                lv.Items.Add(InterfazListado.GenerarGrid(juego, bitmap, True))
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

    Private Async Sub GenerarIcono(juego As StorageFile, carpeta As StorageFolder)

        Dim ficheroIcono As StorageFile = Nothing

        Try
            ficheroIcono = Await carpeta.GetFileAsync(juego.DisplayName + ".png")
        Catch ex As Exception

        End Try

        If ficheroIcono Is Nothing Then
            Dim writeableBitmap As New WriteableBitmap(32, 32)

            Using fileStream As IRandomAccessStream = Await juego.GetThumbnailAsync(ThumbnailMode.PicturesView, 100, ThumbnailOptions.UseCurrentScale)
                Await writeableBitmap.SetSourceAsync(fileStream)
            End Using

            Dim iconoFichero As StorageFile = Nothing

            Try
                Await carpeta.CreateFileAsync(juego.DisplayName + ".png")
                iconoFichero = Await StorageFile.GetFileFromPathAsync(carpeta.Path + "\" + juego.DisplayName + ".png")
            Catch ex As Exception

            End Try

            If Not iconoFichero Is Nothing Then
                Dim stream As IRandomAccessStream = Await iconoFichero.OpenAsync(FileAccessMode.ReadWrite)
                Dim encoder As BitmapEncoder = Await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream)

                Dim pixelStream As Stream = writeableBitmap.PixelBuffer.AsStream()
                Dim pixels As Byte() = New Byte(pixelStream.Length - 1) {}
                Await pixelStream.ReadAsync(pixels, 0, pixels.Length)

                encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Straight, CUInt(writeableBitmap.PixelWidth), CUInt(writeableBitmap.PixelHeight), 96.0, 96.0, pixels)
                Await encoder.FlushAsync()
                stream.Dispose()
            End If
        End If

    End Sub

End Module

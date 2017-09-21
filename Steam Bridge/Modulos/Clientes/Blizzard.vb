Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Windows.Graphics.Imaging
Imports Windows.Storage
Imports Windows.Storage.AccessCache
Imports Windows.Storage.FileProperties
Imports Windows.Storage.Pickers
Imports Windows.Storage.Streams

Module Blizzard

    Public Async Function Config(picker As Boolean) As Task(Of Boolean)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim tbBlizzardRuta As TextBlock = pagina.FindName("tbBlizzardRuta")
        Dim botonBlizzardRutaTexto As TextBlock = pagina.FindName("botonBlizzardRutaTexto")

        Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()
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
                    tbBlizzardRuta.Text = carpeta.Path
                    botonBlizzardRutaTexto.Text = recursos.GetString("Change")
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try

    End Function

    Public Async Sub Generar(listaJuegos As List(Of Juego), carpeta As StorageFolder)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim progreso As ProgressBar = pagina.FindName("progressBarBattlenet")
        progreso.Visibility = Visibility.Visible

        Dim lvGrids As ListView = pagina.FindName("lvBlizzard")
        lvGrids.IsEnabled = False

        If Not carpeta Is Nothing Then
            Dim carpetasJuegos As IReadOnlyList(Of StorageFolder) = Await carpeta.GetFoldersAsync()

            For Each carpetaJuego As StorageFolder In carpetasJuegos
                Dim ficheros As IReadOnlyList(Of StorageFile) = Await carpetaJuego.GetFilesAsync()

                For Each fichero As StorageFile In ficheros
                    Dim nombre As String = carpetaJuego.Name

                    Dim ejecutable As String = Nothing
                    Dim argumentos As String = Nothing

                    Dim nombreFichero As String = fichero.DisplayName.ToLower

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

                        lvGrids.Items.Add(Listado.GenerarGrid(juego, bitmap, False))
                    End If
                Next
            Next
        End If

        If listaJuegos.Count > 0 Then
            lvGrids.Items.Clear()
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

                lvGrids.Items.Add(Listado.GenerarGrid(juego, bitmap, True))
            Next
        End If

        Dim panelNoJuegos As DropShadowPanel = pagina.FindName("panelAvisoNoJuegosBlizzard")

        If listaJuegos.Count = 0 Then
            Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()
            Toast("Steam Bridge - Blizzard App", recursos.GetString("Texto No Juegos"))

            panelNoJuegos.Visibility = Visibility.Visible
        Else
            panelNoJuegos.Visibility = Visibility.Collapsed
        End If

        lvGrids.IsEnabled = True
        progreso.Visibility = Visibility.Collapsed

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

﻿Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Windows.Graphics.Imaging
Imports Windows.Storage
Imports Windows.Storage.AccessCache
Imports Windows.Storage.FileProperties
Imports Windows.Storage.Pickers
Imports Windows.Storage.Streams

Module Battlenet

    Public Async Function Config(tbConfigPath As TextBlock, buttonConfigPath As TextBlock, reg As TextBox, picker As Boolean) As Task(Of Boolean)

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
                        If fichero.DisplayName = "Diablo III" And fichero.FileType = ".exe" Then
                            detectadoBool = True
                            GenerarIcono(fichero, carpetaJuego)
                        End If

                        If fichero.DisplayName = "Hearthstone" And fichero.FileType = ".exe" Then
                            detectadoBool = True
                            GenerarIcono(fichero, carpetaJuego)
                        End If

                        If fichero.DisplayName = "Heroes of the Storm" And fichero.FileType = ".exe" Then
                            detectadoBool = True
                            GenerarIcono(fichero, carpetaJuego)
                        End If

                        If fichero.DisplayName = "Overwatch" And fichero.FileType = ".exe" Then
                            detectadoBool = True
                            GenerarIcono(fichero, carpetaJuego)
                        End If

                        If fichero.DisplayName = "SC2" And fichero.FileType = ".exe" Then
                            detectadoBool = True
                            GenerarIcono(fichero, carpetaJuego)
                        End If

                        If fichero.DisplayName = "WoW" And fichero.FileType = ".exe" Then
                            detectadoBool = True
                            GenerarIcono(fichero, carpetaJuego)
                        End If
                    Next
                Next

                If detectadoBool = True Then
                    StorageApplicationPermissions.FutureAccessList.AddOrReplace("BattlenetPath", carpeta)
                    tbConfigPath.Text = carpeta.Path
                    buttonConfigPath.Text = recursos.GetString("Boton Cambiar")
                    Registro.Mensaje(reg, "Config", "Battle.net detectado")
                    Return True
                Else
                    Registro.Mensaje(reg, "Config", "Battle.net no detectado")
                    Return False
                End If
            Else
                Registro.Mensaje(reg, "Config", "Battle.net no seleccionado")
                Return False
            End If
        Catch ex As Exception
            Registro.Mensaje(reg, "Config", "Battle.net error")
            Return False
        End Try

    End Function

    Public Async Sub Generar(listaJuegos As List(Of Juego), carpeta As StorageFolder, grid As Grid, progreso As ProgressBar, coleccion As HamburgerMenuItemCollection, hamburguesa As HamburgerMenu)

        grid.IsHitTestVisible = False
        progreso.Visibility = Visibility.Visible

        Dim listaGrid As New ListView
        listaGrid.Name = "listaBattlenet"

        If Not carpeta Is Nothing Then
            Dim carpetasJuegos As IReadOnlyList(Of StorageFolder) = Await carpeta.GetFoldersAsync()

            For Each carpetaJuego As StorageFolder In carpetasJuegos
                Dim ficheros As IReadOnlyList(Of StorageFile) = Await carpetaJuego.GetFilesAsync()

                For Each fichero As StorageFile In ficheros
                    Dim nombre As String = carpetaJuego.Name
                    Dim ejecutable As String = Nothing
                    Dim argumentos As String = Nothing
                    Dim icono As StorageFile = Nothing

                    If fichero.DisplayName = "Diablo III" And fichero.FileType = ".exe" Then
                        ejecutable = "battlenet://D3"
                    End If

                    If fichero.DisplayName = "Hearthstone" And fichero.FileType = ".exe" Then
                        ejecutable = "battlenet://WTCG"
                    End If

                    If fichero.DisplayName = "Heroes of the Storm" And fichero.FileType = ".exe" Then
                        ejecutable = "battlenet://Hero"
                    End If

                    If fichero.DisplayName = "Overwatch" And fichero.FileType = ".exe" Then
                        ejecutable = "battlenet://Pro"
                    End If

                    If fichero.DisplayName = "SC2" And fichero.FileType = ".exe" Then
                        ejecutable = "battlenet://S2"
                    End If

                    If fichero.DisplayName = "WoW" And fichero.FileType = ".exe" Then
                        ejecutable = "battlenet://WoW"
                    End If

                    If Not ejecutable = Nothing Then
                        Dim tempIcono As String = fichero.Path.Replace(".exe", ".png")
                        icono = Await StorageFile.GetFileFromPathAsync(tempIcono)

                        Dim juego As New Juego(nombre, ejecutable, argumentos, icono, Nothing, False, "Battle.net")

                        listaJuegos.Add(juego)

                        If listaJuegos.Count = 1 Then
                            Hamburger.Generar("Battle.net", "0", "/Assets/battlenet_logo.png", coleccion, hamburguesa)
                        End If

                        Dim bitmap As BitmapImage = New BitmapImage
                        If Not juego.Icono Is Nothing Then
                            Using stream As IRandomAccessStream = Await juego.Icono.OpenAsync(FileAccessMode.Read)
                                bitmap.DecodePixelWidth = 40
                                bitmap.DecodePixelHeight = 40
                                Await bitmap.SetSourceAsync(stream)
                            End Using
                        End If

                        listaGrid.Items.Add(Listado.GenerarGrid(juego, bitmap, False))
                        grid.Children.Clear()
                        grid.Children.Add(listaGrid)
                    End If
                Next
            Next
        End If

        If listaJuegos.Count > 0 Then
            listaGrid.Items.Clear()
            listaJuegos.Sort(Function(x, y) x.Nombre.CompareTo(y.Nombre))

            For Each juego As Juego In listaJuegos
                Dim bitmap As BitmapImage = New BitmapImage
                If Not juego.Icono Is Nothing Then
                    Using stream As IRandomAccessStream = Await juego.Icono.OpenAsync(FileAccessMode.Read)
                        bitmap.DecodePixelWidth = 40
                        bitmap.DecodePixelHeight = 40
                        Await bitmap.SetSourceAsync(stream)
                    End Using
                End If

                listaGrid.Items.Add(Listado.GenerarGrid(juego, bitmap, True))
            Next
        End If

        If listaJuegos.Count = 0 Then
            Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()
            Toast("Steam Bridge - Battle.net", recursos.GetString("Texto No Juegos"))
        End If

        grid.Children.Clear()
        grid.Children.Add(listaGrid)
        grid.IsHitTestVisible = True
        progreso.Visibility = Visibility.Collapsed

    End Sub

    Private Async Sub GenerarIcono(juego As StorageFile, carpeta As StorageFolder)

        Dim ficheroIcono As StorageFile = Nothing

        Try
            ficheroIcono = carpeta.GetFileAsync(juego.DisplayName + ".png")
        Catch ex As Exception

        End Try

        If ficheroIcono Is Nothing Then
            Dim writeableBitmap As New WriteableBitmap(32, 32)

            Using fileStream As IRandomAccessStream = Await juego.GetThumbnailAsync(ThumbnailMode.PicturesView, 100, ThumbnailOptions.UseCurrentScale)
                Await writeableBitmap.SetSourceAsync(fileStream)
            End Using

            Dim iconoFichero As StorageFile = Await StorageFile.GetFileFromPathAsync(carpeta.Path + "\" + juego.DisplayName + ".png")
            Dim stream As IRandomAccessStream = Await iconoFichero.OpenAsync(FileAccessMode.ReadWrite)
            Dim encoder As BitmapEncoder = Await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream)

            Dim pixelStream As Stream = writeableBitmap.PixelBuffer.AsStream()
            Dim pixels As Byte() = New Byte(pixelStream.Length - 1) {}
            Await pixelStream.ReadAsync(pixels, 0, pixels.Length)

            encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Straight, CUInt(writeableBitmap.PixelWidth), CUInt(writeableBitmap.PixelHeight), 96.0, 96.0, pixels)
            Await encoder.FlushAsync()
            stream.Dispose()
        End If

    End Sub

End Module

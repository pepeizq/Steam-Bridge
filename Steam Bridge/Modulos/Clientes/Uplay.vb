Imports System.Text
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Windows.Storage
Imports Windows.Storage.AccessCache
Imports Windows.Storage.Pickers
Imports Windows.Storage.Streams

Module Uplay

    Public Async Function Config(opcion As Integer, picker As Boolean) As Task(Of Boolean)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim tbUplayRutaCliente As TextBlock = pagina.FindName("tbUplayRutaCliente")
        Dim botonUplayRutaTextoCliente As TextBlock = pagina.FindName("botonUplayRutaTextoCliente")

        Dim tbUplayRutaJuegos As TextBlock = pagina.FindName("tbUplayRutaJuegos")
        Dim botonUplayRutaTextoJuegos As TextBlock = pagina.FindName("botonUplayRutaTextoJuegos")

        Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()
        Dim carpetaCliente As StorageFolder = Nothing
        Dim carpetaJuegos As StorageFolder = Nothing
        Dim boolCliente As Boolean = False
        Dim boolJuegos As Boolean = False

        Try
            If picker = True Then
                Dim carpetapicker As FolderPicker = New FolderPicker()

                carpetapicker.FileTypeFilter.Add("*")
                carpetapicker.ViewMode = PickerViewMode.List

                If opcion = 0 Then
                    carpetaCliente = Await carpetapicker.PickSingleFolderAsync()
                ElseIf opcion = 1 Then
                    carpetaJuegos = Await carpetapicker.PickSingleFolderAsync()
                End If
            Else
                Try
                    carpetaCliente = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("UplayPathCliente")
                    carpetaJuegos = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("UplayPathJuegos")
                Catch ex As Exception
                    carpetaCliente = Nothing
                    carpetaJuegos = Nothing
                End Try
            End If

            If Not carpetaCliente Is Nothing Then
                Dim ejecutable As StorageFile = Nothing

                Try
                    ejecutable = Await carpetaCliente.GetFileAsync("Uplay.exe")
                Catch ex As Exception

                End Try

                If Not ejecutable Is Nothing Then
                    StorageApplicationPermissions.FutureAccessList.AddOrReplace("UplayPathCliente", carpetaCliente)
                    tbUplayRutaCliente.Text = carpetaCliente.Path
                    botonUplayRutaTextoCliente.Text = recursos.GetString("Change")
                    boolCliente = True
                End If
            End If

            If Not carpetaJuegos Is Nothing Then
                Dim carpetasJuegos As IReadOnlyList(Of StorageFolder) = Await carpetaJuegos.GetFoldersAsync()

                Dim boolManifiesto As Boolean = False

                For Each carpetaJuego_ As StorageFolder In carpetasJuegos
                    Dim manifiesto As StorageFile = Nothing

                    Try
                        manifiesto = Await carpetaJuego_.GetFileAsync("uplay_install.manifest")
                    Catch ex As Exception

                    End Try

                    If Not manifiesto Is Nothing Then
                        boolManifiesto = True
                    End If
                Next

                If boolManifiesto = True Then
                    StorageApplicationPermissions.FutureAccessList.AddOrReplace("UplayPathJuegos", carpetaJuegos)
                    tbUplayRutaJuegos.Text = carpetaJuegos.Path
                    botonUplayRutaTextoJuegos.Text = recursos.GetString("Change")
                    boolJuegos = True
                End If
            End If
        Catch ex As Exception

        End Try

        If boolCliente = True And boolJuegos = True Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Async Sub Generar(listaJuegos As List(Of Juego), carpetaCliente As StorageFolder, carpetaJuegos As StorageFolder)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim progreso As ProgressBar = pagina.FindName("progressBarUplay")
        progreso.Visibility = Visibility.Visible

        Dim lvGrids As ListView = pagina.FindName("lvUplay")
        lvGrids.IsEnabled = False

        If Not carpetaJuegos Is Nothing Then
            Dim carpetasJuegos_ As IReadOnlyList(Of StorageFolder) = Await carpetaJuegos.GetFoldersAsync()

            For Each carpetaJuego As StorageFolder In carpetasJuegos_
                Dim configurations As StorageFile = Nothing

                Try
                    configurations = Await carpetaCliente.GetFileAsync("cache\configuration\configurations")
                Catch ex As Exception

                End Try

                If Not configurations Is Nothing Then
                    Dim buffer As IBuffer = Await FileIO.ReadBufferAsync(configurations)
                    Dim lector As DataReader = DataReader.FromBuffer(buffer)
                    Dim contenido(lector.UnconsumedBufferLength - 1) As Byte
                    lector.ReadBytes(contenido)
                    Dim texto As String = Encoding.UTF8.GetString(contenido, 0, contenido.Length)

                    If Not texto = Nothing Then
                        Dim nombre As String = carpetaJuego.Name

                        Dim ficheros As IReadOnlyList(Of StorageFile) = Await carpetaJuego.GetFilesAsync()

                        If ficheros.Count > 0 Then
                            Dim ejecutable As String = BuscarEjecutable(nombre, carpetaCliente.Path + "\Uplay.exe")

                            Dim argumentos As String = Nothing

                            Dim temp, temp2, temp3 As String
                            Dim int, int2, int3 As Integer

                            int = texto.LastIndexOf("game_identifier: " + nombre)
                            temp = texto.Remove(int, texto.Length - int)

                            int2 = temp.LastIndexOf("icon_image:")
                            temp2 = temp.Remove(0, int2 + 11)

                            int3 = temp2.IndexOf(".ico")
                            temp3 = temp2.Remove(int3, temp2.Length - int3)

                            Dim icono As StorageFile = Await StorageFile.GetFileFromPathAsync(carpetaCliente.Path + "\data\games\" + temp3.Trim + ".ico")

                            Dim juego As New Juego(nombre, ejecutable, argumentos, icono, Nothing, False, "Uplay")

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

                                End Try
                            End If

                            lvGrids.Items.Add(Listado.GenerarGrid(juego, bitmap, False))
                        End If
                    End If
                End If
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

                    End Try
                End If

                lvGrids.Items.Add(Listado.GenerarGrid(juego, bitmap, True))
            Next
        End If

        Dim panelNoJuegos As DropShadowPanel = pagina.FindName("panelAvisoNoJuegosUplay")

        If listaJuegos.Count = 0 Then
            panelNoJuegos.Visibility = Visibility.Visible
        Else
            panelNoJuegos.Visibility = Visibility.Collapsed
        End If

        lvGrids.IsEnabled = True
        progreso.Visibility = Visibility.Collapsed

    End Sub

    Private Function BuscarEjecutable(nombre As String, uplay As String)

        Dim ejecutable As String = Nothing

        If nombre = "Far Cry 3 Blood Dragon" Then
            ejecutable = "uplay://launch/205/0"
        ElseIf nombre = "From Dust" Then
            ejecutable = "uplay://launch/30/0"
        ElseIf nombre = "Prince of Persia Sands of Time" Then
            ejecutable = "uplay://launch/111/0"
        ElseIf nombre = "Rayman Origins" Then
            ejecutable = "uplay://launch/80/0"
        ElseIf nombre = "The Crew" Then
            ejecutable = "uplay://launch/413/0"
        ElseIf nombre = "Tom Clancy's Splinter Cell" Then
            ejecutable = "uplay://launch/109/0"
        ElseIf nombre = "Far Cry 4" Then
            ejecutable = "uplay://launch/420/0"
        ElseIf nombre = "Anno 2205" Then
            ejecutable = "uplay://launch/1253/0"
        ElseIf nombre = "Assassin's Creed Unity" Then
            ejecutable = "uplay://launch/720/0"
        ElseIf nombre = "WATCH_DOGS" Then
            ejecutable = "uplay://launch/274/0"
        ElseIf nombre = "WATCH_DOGS2" Then
            ejecutable = "uplay://launch/2688/0"
        End If

        If ejecutable = Nothing Then
            ejecutable = uplay
        End If

        Return ejecutable
    End Function

End Module

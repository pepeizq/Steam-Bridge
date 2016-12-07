Imports System.Text
Imports Windows.Storage
Imports Windows.Storage.Streams

Module Uplay

    Public Async Sub Cargar(listaJuegos As List(Of Juego), carpetaCliente As StorageFolder, carpetaJuegos As StorageFolder, grid As Grid, progreso As ProgressBar, textobloque As TextBlock)

        grid.IsHitTestVisible = False
        progreso.Visibility = Visibility.Visible
        textobloque.Visibility = Visibility.Collapsed

        Dim listaGrid As New ListView
        listaGrid.Name = "listaUplay"

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
                                Using stream As IRandomAccessStream = Await juego.Icono.OpenAsync(FileAccessMode.Read)
                                    bitmap.DecodePixelWidth = 40
                                    bitmap.DecodePixelHeight = 40
                                    Await bitmap.SetSourceAsync(stream)
                                End Using
                            End If

                            listaGrid.Items.Add(Listado.GenerarGrid(juego, bitmap))
                            grid.Children.Clear()
                            grid.Children.Add(listaGrid)
                        End If
                    End If
                End If
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

                listaGrid.Items.Add(Listado.GenerarGrid(juego, bitmap))
            Next
        End If

        If listaJuegos.Count = 0 Then
            textobloque.Visibility = Visibility.Visible

            Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()
            textobloque.Text = recursos.GetString("Texto No Juegos")
        Else
            textobloque.Visibility = Visibility.Collapsed
        End If

        grid.Children.Clear()
        grid.Children.Add(listaGrid)
        grid.IsHitTestVisible = True
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
        End If

        If ejecutable = Nothing Then
            ejecutable = uplay
        End If

        Return ejecutable
    End Function

End Module

Imports System.Text
Imports Windows.Storage
Imports Windows.Storage.Streams

Module GOGGalaxy

    Public Async Sub Cargar(listaJuegos As List(Of Juego), carpeta As StorageFolder, grid As Grid, progreso As ProgressBar, textobloque As TextBlock)

        grid.IsHitTestVisible = False
        progreso.Visibility = Visibility.Visible
        textobloque.Visibility = Visibility.Collapsed

        Dim listaGrid As New ListView
        listaGrid.Name = "listaGOGGalaxy"

        If Not carpeta Is Nothing Then
            Dim carpetasJuegos As IReadOnlyList(Of StorageFolder) = Await carpeta.GetFoldersAsync()

            For Each carpetaJuego As StorageFolder In carpetasJuegos
                Dim ficheros As IReadOnlyList(Of StorageFile) = Await carpetaJuego.GetFilesAsync()

                For Each fichero As StorageFile In ficheros
                    If fichero.DisplayName.Contains("dosbox") And fichero.FileType.Contains(".conf") Then
                        Dim opciones As ApplicationDataContainer = ApplicationData.Current.LocalSettings

                        If opciones.Values("DOSBoxSteamOverlay") = True Then
                            Dim buffer As IBuffer = Await FileIO.ReadBufferAsync(fichero)
                            Dim lector As DataReader = DataReader.FromBuffer(buffer)
                            Dim contenido(lector.UnconsumedBufferLength - 1) As Byte
                            lector.ReadBytes(contenido)
                            Dim texto As String = Encoding.UTF8.GetString(contenido, 0, contenido.Length)

                            If Not texto = Nothing Then
                                texto = texto.Replace("output=surface", "output=opengl")
                                Await FileIO.WriteTextAsync(fichero, texto)
                            End If
                        End If
                    End If

                    If fichero.DisplayName.Contains("goggame-") And fichero.FileType.Contains(".dll") Then
                        Dim buffer As IBuffer = Await FileIO.ReadBufferAsync(fichero)
                        Dim lector As DataReader = DataReader.FromBuffer(buffer)
                        Dim contenido(lector.UnconsumedBufferLength - 1) As Byte
                        lector.ReadBytes(contenido)
                        Dim texto As String = Encoding.UTF8.GetString(contenido, 0, contenido.Length)

                        If Not texto = Nothing Then
                            Dim temp, temp2 As String
                            Dim int, int2 As Integer

                            int = texto.IndexOf("<Name>")
                            temp = texto.Remove(0, int + 6)

                            int2 = temp.IndexOf("</Name>")
                            temp2 = temp.Remove(int2, temp.Length - int2)

                            Dim nombre As String = temp2.Trim

                            Dim temp3, temp4, temp5, temp6 As String
                            Dim int3, int4, int5, int6 As Integer

                            int3 = texto.IndexOf("<Primary>")
                            temp3 = texto.Remove(0, int3 + 2)

                            int4 = temp3.IndexOf("</Primary>")
                            temp4 = temp3.Remove(int4, temp3.Length - int4)

                            int5 = temp4.IndexOf("path=")
                            temp5 = temp4.Remove(0, int5 + 6)

                            int6 = temp5.IndexOf(ChrW(34))
                            temp6 = temp5.Remove(int6, temp5.Length - int6)

                            Dim ejecutable As String = carpetaJuego.Path + "\" + temp6

                            Dim argumentos As String = Nothing

                            If temp4.Contains("arguments=") Then
                                Dim temp7, temp8 As String
                                Dim int7, int8 As Integer

                                int7 = temp4.IndexOf("arguments=")
                                temp7 = temp4.Remove(0, int7 + 11)

                                int8 = temp7.IndexOf(ChrW(34))
                                temp8 = temp7.Remove(int8, temp7.Length - int8)

                                argumentos = temp8.Trim
                            End If

                            Dim iconoString As String = fichero.Path
                            iconoString = iconoString.Replace(".dll", ".ico")

                            Dim icono As StorageFile = Await StorageFile.GetFileFromPathAsync(iconoString)

                            Dim juego As New Juego(nombre, ejecutable, argumentos, icono, Nothing, False, "GOG Galaxy")

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

End Module

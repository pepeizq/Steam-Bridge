Imports Microsoft.Toolkit.Uwp
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Windows.Storage
Imports Windows.Storage.AccessCache
Imports Windows.Storage.Pickers
Imports Windows.Storage.Streams
Imports Windows.UI

Module Twitch

    Public Async Function Config(picker As Boolean) As Task(Of Boolean)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim tbConfigPath As TextBlock = pagina.FindName("tbTwitchConfigPath")
        Dim buttonConfigPath As TextBlock = pagina.FindName("buttonTwitchConfigPathTexto")

        Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()
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
                    tbConfigPath.Text = carpeta.Path
                    buttonConfigPath.Text = recursos.GetString("Boton Cambiar")
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

        Dim progreso As ProgressBar = pagina.FindName("progressBarTwitch")
        progreso.Visibility = Visibility.Visible

        Dim lvGrids As ListView = pagina.FindName("lvTwitch")
        lvGrids.IsEnabled = False

        If Not carpeta Is Nothing Then
            Dim carpetaFuel As StorageFolder = Await carpeta.GetFolderAsync("Fuel\db\GameProductInfo")

            If Not carpetaFuel Is Nothing Then
                Dim ficheros As IReadOnlyList(Of StorageFile) = Await carpetaFuel.GetFilesAsync()

                For Each fichero In ficheros
                    If fichero.FileType.Contains(".cfs") Then
                        Dim texto As String = Nothing

                        Using inputStream As IRandomAccessStreamWithContentType = Await fichero.OpenReadAsync
                            Using clasicoStream As Stream = inputStream.AsStreamForRead
                                Using sr As StreamReader = New StreamReader(clasicoStream)
                                    While sr.Peek <> 0
                                        texto = texto + sr.ReadLine
                                    End While
                                End Using
                            End Using
                        End Using

                        If Not texto = Nothing Then
                            Dim i As Integer = 0

                            While i < 200
                                If texto.Contains(ChrW(34) + "ProductTitle" + ChrW(34)) Then
                                    Dim temp, temp2 As String
                                    Dim int, int2 As Integer

                                    int = texto.IndexOf(ChrW(34) + "ProductTitle" + ChrW(34))
                                    temp = texto.Remove(0, int + 14)

                                    texto = temp

                                    int = temp.IndexOf(ChrW(34))
                                    temp = temp.Remove(0, int + 1)

                                    int2 = temp.IndexOf(ChrW(34))
                                    temp2 = temp.Remove(int2, temp.Length - int2)

                                    Dim titulo As String = temp2.Trim

                                    Dim temp3, temp4 As String
                                    Dim int3, int4 As Integer

                                    int3 = texto.IndexOf(ChrW(34) + "Id" + ChrW(34))
                                    temp3 = texto.Remove(0, int3 + 4)

                                    int3 = temp3.IndexOf(ChrW(34))
                                    temp3 = temp3.Remove(0, int3 + 1)

                                    int4 = temp3.IndexOf(ChrW(34))
                                    temp4 = temp3.Remove(int4, temp3.Length - int4)

                                    Dim id As String = temp4.Trim

                                    Dim tituloBool As Boolean = False
                                    Dim g As Integer = 0
                                    While g < listaJuegos.Count
                                        If listaJuegos(g).Nombre = titulo Then
                                            tituloBool = True
                                        End If
                                        g += 1
                                    End While

                                    If tituloBool = False Then
                                        Dim ejecutable As String = "twitch://fuel-launch/" + id

                                        Dim juego As New Juego(titulo, ejecutable, Nothing, Nothing, Nothing, False, "Twitch")

                                        listaJuegos.Add(juego)

                                        lvGrids.Items.Add(Listado.GenerarGrid(juego, Nothing, False))
                                    End If
                                End If
                                i += 1
                            End While
                        End If
                    End If
                Next
            End If
        End If

        If listaJuegos.Count > 0 Then
            lvGrids.Items.Clear()
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

                lvGrids.Items.Add(Listado.GenerarGrid(juego, Nothing, True))
            Next
        End If

        Dim panelNoJuegos As DropShadowPanel = pagina.FindName("panelAvisoNoJuegosTwitch")

        If listaJuegos.Count = 0 Then
            Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()
            Toast("Steam Bridge - Twitch", recursos.GetString("Texto No Juegos"))

            panelNoJuegos.Visibility = Visibility.Visible
        Else
            panelNoJuegos.Visibility = Visibility.Collapsed
        End If

        lvGrids.IsEnabled = True
        progreso.Visibility = Visibility.Collapsed

    End Sub

End Module

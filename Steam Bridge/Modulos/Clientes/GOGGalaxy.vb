Imports System.Text
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Windows.Storage
Imports Windows.Storage.AccessCache
Imports Windows.Storage.Pickers
Imports Windows.Storage.Streams

Module GOGGalaxy

    Public Async Function Config(picker As Boolean) As Task(Of Boolean)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim tbConfigPath As TextBlock = pagina.FindName("tbGOGGalaxyConfigPath")
        Dim buttonConfigPath As TextBlock = pagina.FindName("buttonGOGGalaxyConfigPathTexto")
        Dim reg As TextBox = pagina.FindName("tbConfigRegistro")

        Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()
        Dim carpeta As StorageFolder = Nothing

        Try
            If picker = True Then
                Dim carpetapicker As FolderPicker = New FolderPicker()

                carpetapicker.FileTypeFilter.Add("*")
                carpetapicker.ViewMode = PickerViewMode.List

                carpeta = Await carpetapicker.PickSingleFolderAsync()
            Else
                carpeta = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("GOGGalaxyPath")
            End If

            If Not carpeta Is Nothing Then
                Dim carpetasJuegos As IReadOnlyList(Of StorageFolder) = Await carpeta.GetFoldersAsync()
                Dim detectadoBool As Boolean = False

                For Each carpetaJuego As StorageFolder In carpetasJuegos
                    Dim ficheros As IReadOnlyList(Of StorageFile) = Await carpetaJuego.GetFilesAsync()

                    For Each fichero As StorageFile In ficheros
                        If fichero.DisplayName.Contains("goggame-") Then
                            detectadoBool = True
                        End If
                    Next
                Next

                If detectadoBool = True Then
                    StorageApplicationPermissions.FutureAccessList.AddOrReplace("GOGGalaxyPath", carpeta)
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

        Dim progreso As ProgressBar = pagina.FindName("progressBarGOGGalaxy")
        progreso.Visibility = Visibility.Visible

        Dim lvGrids As ListView = pagina.FindName("lvGOGGalaxy")
        lvGrids.IsEnabled = False

        If Not carpeta Is Nothing Then
            Dim carpetasJuegos As IReadOnlyList(Of StorageFolder) = Await carpeta.GetFoldersAsync()

            For Each carpetaJuego As StorageFolder In carpetasJuegos
                Dim ficheros As IReadOnlyList(Of StorageFile) = Await carpetaJuego.GetFilesAsync()

                For Each fichero As StorageFile In ficheros
                    If fichero.DisplayName.Contains("goggame-") And fichero.FileType.Contains(".dll") Then
                        Dim id As String = fichero.DisplayName.Replace("goggame-", Nothing)
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

                            Dim ejecutable As String = "goggalaxy://openGameView/" + id

                            Dim iconoString As String = fichero.Path
                            iconoString = iconoString.Replace(".dll", ".ico")

                            Dim icono As StorageFile = Await StorageFile.GetFileFromPathAsync(iconoString)

                            Dim juego As New Juego(nombre, ejecutable, Nothing, icono, Nothing, False, "GOG Galaxy")

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

                    End Try
                End If

                lvGrids.Items.Add(Listado.GenerarGrid(juego, bitmap, True))
            Next
        End If

        Dim panelNoJuegos As DropShadowPanel = pagina.FindName("panelAvisoNoJuegosGOGGalaxy")

        If listaJuegos.Count = 0 Then
            Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()
            Toast("Steam Bridge - GOG Galaxy", recursos.GetString("Texto No Juegos"))

            panelNoJuegos.Visibility = Visibility.Visible
        Else
            panelNoJuegos.Visibility = Visibility.Collapsed
        End If

        lvGrids.IsEnabled = True
        progreso.Visibility = Visibility.Collapsed

    End Sub

End Module

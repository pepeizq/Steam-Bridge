Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Windows.Storage
Imports Windows.Storage.AccessCache
Imports Windows.Storage.Pickers
Imports Windows.Storage.Streams

Module WindowsStore

    Public Async Function Config(picker As Boolean) As Task(Of Boolean)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim tbConfigPath As TextBlock = pagina.FindName("tbWindowsStoreConfigPath")
        Dim buttonConfigPath As TextBlock = pagina.FindName("buttonWindowsStoreConfigPathTexto")
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
                carpeta = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("WindowsStorePath")
            End If

            If Not carpeta Is Nothing Then
                If carpeta.Path.Contains("WindowsApps") Then
                    StorageApplicationPermissions.FutureAccessList.AddOrReplace("WindowsStorePath", carpeta)
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

        Dim progreso As ProgressBar = pagina.FindName("progressBarWindowsStore")
        progreso.Visibility = Visibility.Visible

        Dim lvGrids As ListView = pagina.FindName("lvWindowsStore")
        lvGrids.IsEnabled = False

        Dim listaSubcarpetas As IReadOnlyList(Of StorageFolder) = Await carpeta.GetFoldersAsync()

        For Each subcarpeta As StorageFolder In listaSubcarpetas
            Dim manifiesto As StorageFile = Nothing

            Try
                manifiesto = Await subcarpeta.GetFileAsync("AppxManifest.xml")
            Catch ex As Exception

            End Try

            If Not manifiesto Is Nothing Then
                Dim lineas As String = Nothing

                Try
                    lineas = Await FileIO.ReadTextAsync(manifiesto)
                Catch ex As Exception

                End Try

                If Not lineas = Nothing Then
                    If lineas.Contains("<Application Id=") Then
                        Dim temp, temp2, temp3 As String
                        Dim int, int2, int3 As Integer

                        int = lineas.IndexOf("<Application Id=")
                        temp = lineas.Remove(0, int)

                        int2 = temp.IndexOf(ChrW(34))
                        temp2 = temp.Remove(0, int2 + 1)

                        int3 = temp2.IndexOf(ChrW(34))
                        temp3 = temp2.Remove(int3, temp2.Length - int3)

                        Dim id As String = temp3

                        If lineas.Contains("<Identity Name=") Then
                            Dim temp4, temp5, temp6 As String
                            Dim int4, int5, int6 As Integer

                            int4 = lineas.IndexOf("<Identity Name=")
                            temp4 = lineas.Remove(0, int4)

                            int5 = temp4.IndexOf(ChrW(34))
                            temp5 = temp4.Remove(0, int5 + 1)

                            int6 = temp5.IndexOf(ChrW(34))
                            temp6 = temp5.Remove(int6, temp5.Length - int6)

                            Dim identidad As String = temp6

                            Dim int7 As Integer = subcarpeta.Path.LastIndexOf("\")
                            Dim paquete As String = subcarpeta.Path.Remove(0, int7 + 1)

                            If paquete.Contains("_") Then
                                Dim int11 As Integer = paquete.IndexOf("_")
                                Dim int12 As Integer = paquete.LastIndexOf("_")

                                paquete = paquete.Remove(int11, int12 - int11)
                            End If

                            Dim temp8, temp9, temp10 As String
                            Dim int8, int9, int10 As Integer

                            int8 = lineas.IndexOf("<DisplayName>")
                            temp8 = lineas.Remove(0, int8)

                            int9 = temp8.IndexOf(">")
                            temp9 = temp8.Remove(0, int9 + 1)

                            int10 = temp9.IndexOf("</DisplayName>")
                            temp10 = temp9.Remove(int10, temp9.Length - int10)

                            If temp10.Contains("ms-resource:") Then
                                temp10 = BuscarTitulo(lineas)
                            End If

                            Dim nombre As String = temp10.Trim

                            If Not nombre.Contains("ms-resource:") Then

                                Dim icono As StorageFile = Nothing

                                Try
                                    icono = Await SacarImagen(lineas, subcarpeta.Path)
                                Catch ex As Exception

                                End Try

                                Dim colorFondo As String

                                If lineas.Contains("BackgroundColor=" + ChrW(34)) Then
                                    Dim temp14, temp15, temp16 As String
                                    Dim int14, int15, int16 As Integer

                                    int14 = lineas.IndexOf("BackgroundColor=" + ChrW(34))
                                    temp14 = lineas.Remove(0, int14)

                                    int15 = temp14.IndexOf(ChrW(34))
                                    temp15 = temp14.Remove(0, int15 + 1)

                                    int16 = temp15.IndexOf(ChrW(34))
                                    temp16 = temp15.Remove(int16, temp15.Length - int16)

                                    temp16 = temp16.Trim

                                    If temp16 = "transparent" Then
                                        temp16 = Nothing
                                    End If

                                    colorFondo = temp16
                                Else
                                    colorFondo = Nothing
                                End If

                                Dim tituloBool As Boolean = False
                                Dim i As Integer = 0
                                While i < listaJuegos.Count
                                    If listaJuegos(i).Nombre = nombre Then
                                        tituloBool = True
                                    End If
                                    i += 1
                                End While

                                If tituloBool = False Then
                                    Dim juego As New Juego(nombre, "shell:AppsFolder\" + paquete + "!" + id, Nothing, icono, colorFondo, False, "Windows Store")

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
                    End If
                End If
            End If
        Next

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

        Dim panelNoJuegos As DropShadowPanel = pagina.FindName("panelAvisoNoJuegosWindowsStore")

        If listaJuegos.Count = 0 Then
            Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()
            Toast("Steam Bridge - Windows Store", recursos.GetString("Texto No Juegos"))

            panelNoJuegos.Visibility = Visibility.Visible
        Else
            panelNoJuegos.Visibility = Visibility.Collapsed
        End If

        lvGrids.IsEnabled = True
        progreso.Visibility = Visibility.Collapsed

    End Sub

    Private Function BuscarTitulo(texto As String)

        Dim temp, temp2, temp3 As String
        Dim int, int2, int3 As Integer

        int = texto.IndexOf("<Identity Name=")
        temp = texto.Remove(0, int)

        int2 = temp.IndexOf(ChrW(34))
        temp2 = temp.Remove(0, int2 + 1)

        int3 = temp2.IndexOf(ChrW(34))
        temp3 = temp2.Remove(int3, temp2.Length - int3)

        temp3 = temp3.Replace("Windows", Nothing)
        temp3 = temp3.Replace("Zune", Nothing)
        temp3 = temp3.Replace("Bing", Nothing)

        If temp3.Contains(".") Then
            Dim int4 As Integer = temp3.LastIndexOf(".")
            temp3 = temp3.Remove(0, int4 + 1)
        End If

        Return temp3
    End Function

    Private Async Function SacarImagen(lineas As String, path As String) As Task(Of StorageFile)

        Dim temp, temp2, temp3 As String
        Dim int, int2, int3 As Integer

        int = lineas.IndexOf("<Logo>")
        temp = lineas.Remove(0, int)

        int2 = temp.IndexOf(">")
        temp2 = temp.Remove(0, int2 + 1)

        int3 = temp2.IndexOf("</Logo>")
        temp3 = temp2.Remove(int3, temp2.Length - int3)

        path = path + "\" + temp3

        Dim ficheroImagen As StorageFile = Nothing

        Try
            ficheroImagen = Await StorageFile.GetFileFromPathAsync(path)
        Catch ex As Exception

        End Try

        If ficheroImagen Is Nothing Then
            Dim int4 As Integer = path.LastIndexOf(".")
            path = path.Insert(int4, ".scale-100")

            Try
                ficheroImagen = Await StorageFile.GetFileFromPathAsync(path)
            Catch ex As Exception

            End Try
        End If

        If ficheroImagen Is Nothing Then
            path = path.Replace(".scale-100", Nothing)
            Dim int4 As Integer = path.LastIndexOf(".")
            path = path.Insert(int4, ".scale-125")

            Try
                ficheroImagen = Await StorageFile.GetFileFromPathAsync(path)
            Catch ex As Exception

            End Try
        End If

        If ficheroImagen Is Nothing Then
            path = path.Replace(".scale-125", Nothing)
            Dim int4 As Integer = path.LastIndexOf(".")
            path = path.Insert(int4, ".scale-150")

            Try
                ficheroImagen = Await StorageFile.GetFileFromPathAsync(path)
            Catch ex As Exception

            End Try
        End If

        If ficheroImagen Is Nothing Then
            path = path.Replace(".scale-125", Nothing)
            Dim int4 As Integer = path.LastIndexOf(".")
            path = path.Insert(int4, ".scale-150")

            Try
                ficheroImagen = Await StorageFile.GetFileFromPathAsync(path)
            Catch ex As Exception

            End Try
        End If

        If ficheroImagen Is Nothing Then
            path = path.Replace(".scale-150", Nothing)
            Dim int4 As Integer = path.LastIndexOf(".")
            path = path.Insert(int4, ".scale-200")

            Try
                ficheroImagen = Await StorageFile.GetFileFromPathAsync(path)
            Catch ex As Exception

            End Try
        End If

        If ficheroImagen Is Nothing Then
            path = path.Replace(".scale-200", Nothing)
            Dim int4 As Integer = path.LastIndexOf(".")
            path = path.Insert(int4, ".scale-400")

            Try
                ficheroImagen = Await StorageFile.GetFileFromPathAsync(path)
            Catch ex As Exception

            End Try
        End If

        Return ficheroImagen
    End Function

End Module

Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Windows.Storage
Imports Windows.Storage.AccessCache
Imports Windows.Storage.Pickers

Module Origin

    Public Async Function Config(picker As Boolean) As Task(Of Boolean)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim tbConfigPath As TextBlock = pagina.FindName("tbOriginConfigPath")
        Dim buttonConfigPath As TextBlock = pagina.FindName("buttonOriginConfigPathTexto")

        Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()
        Dim carpeta As StorageFolder = Nothing

        Try
            If picker = True Then
                Dim carpetapicker As FolderPicker = New FolderPicker()

                carpetapicker.FileTypeFilter.Add("*")
                carpetapicker.ViewMode = PickerViewMode.List

                carpeta = Await carpetapicker.PickSingleFolderAsync()
            Else
                carpeta = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("OriginPath")
            End If

            If Not carpeta Is Nothing Then
                If carpeta.Path.Contains("Origin\LocalContent") Then
                    StorageApplicationPermissions.FutureAccessList.AddOrReplace("OriginPath", carpeta)
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

        Dim progreso As ProgressBar = pagina.FindName("progressBarOrigin")
        progreso.Visibility = Visibility.Visible

        Dim lvGrids As ListView = pagina.FindName("lvOrigin")
        lvGrids.IsEnabled = False

        If Not carpeta Is Nothing Then
            Dim carpetasJuegos As IReadOnlyList(Of StorageFolder) = Await carpeta.GetFoldersAsync()

            For Each carpetaJuego As StorageFolder In carpetasJuegos
                Dim ficheros As IReadOnlyList(Of StorageFile) = Await carpetaJuego.GetFilesAsync()

                For Each fichero As StorageFile In ficheros
                    Dim ficheroext As String = fichero.DisplayName + fichero.FileType

                    If Not ficheroext = "map.crc" Then
                        Dim titulo As String = carpetaJuego.Name

                        titulo = titulo.Replace("Mirrors Edge", "Mirror's Edge")

                        Dim ejecutable As String = Nothing

                        If fichero.FileType = ".ddc" Then
                            Dim ficheroDDC As StorageFile = fichero

                            Dim texto As String = Await FileIO.ReadTextAsync(ficheroDDC)

                            If Not texto = Nothing Then
                                If texto.Contains(ChrW(34) + "productId" + ChrW(34)) Then
                                    Dim temp, temp2 As String
                                    Dim int, int2 As Integer

                                    int = texto.IndexOf(ChrW(34) + "productId" + ChrW(34))
                                    temp = texto.Remove(0, int + 11)

                                    int = temp.IndexOf(ChrW(34))
                                    temp = temp.Remove(0, int + 1)

                                    int2 = temp.IndexOf(ChrW(34))
                                    temp2 = temp.Remove(int2, temp.Length - int2)

                                    ejecutable = "origin://launchgame/" + temp2
                                End If
                            End If
                        ElseIf fichero.FileType = ".mfst" Then
                            Dim ficheroMFST As StorageFile = fichero

                            Dim texto As String = Await FileIO.ReadTextAsync(ficheroMFST)

                            If Not texto = Nothing Then
                                If texto.Contains("&id=") Then
                                    Dim temp, temp2 As String
                                    Dim int, int2 As Integer

                                    int = texto.IndexOf("&id=")
                                    temp = texto.Remove(0, int + 4)

                                    int2 = temp.IndexOf("&")
                                    temp2 = temp.Remove(int2, temp.Length - int2)

                                    ejecutable = "origin://launchgame/" + temp2
                                End If
                            End If
                        End If

                        If ejecutable = Nothing Then
                            ejecutable = "origin://launchgame/" + fichero.DisplayName

                            If ejecutable.Contains("OFB-EAST") Then
                                ejecutable = ejecutable.Replace("OFB-EAST", "OFB-EAST:")
                            End If

                            If ejecutable.Contains("DR") Then
                                ejecutable = ejecutable.Replace("DR", "DR:")
                            End If
                        End If

                        Dim argumentos As String = Nothing

                        Dim icono As StorageFile = Nothing

                        Dim tituloBool As Boolean = False
                        Dim i As Integer = 0
                        While i < listaJuegos.Count
                            If listaJuegos(i).Nombre = titulo Then
                                tituloBool = True
                            End If
                            i += 1
                        End While

                        If tituloBool = False Then
                            Dim juego As New Juego(titulo, ejecutable, argumentos, icono, Nothing, False, "Origin")

                            listaJuegos.Add(juego)

                            'Dim bitmap As BitmapImage = New BitmapImage
                            'If Not juego.Icono Is Nothing Then
                            '    Using stream As IRandomAccessStream = Await juego.Icono.OpenAsync(FileAccessMode.Read)
                            '        bitmap.DecodePixelWidth = 40
                            '        bitmap.DecodePixelHeight = 40
                            '        Await bitmap.SetSourceAsync(stream)
                            '    End Using
                            'End If

                            lvGrids.Items.Add(Listado.GenerarGrid(juego, Nothing, False))
                        End If
                    End If
                Next
            Next
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

        Dim panelNoJuegos As DropShadowPanel = pagina.FindName("panelAvisoNoJuegosOrigin")

        If listaJuegos.Count = 0 Then
            Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()
            Toast("Steam Bridge - Origin", recursos.GetString("Texto No Juegos"))

            panelNoJuegos.Visibility = Visibility.Visible
        Else
            panelNoJuegos.Visibility = Visibility.Collapsed
        End If

        lvGrids.IsEnabled = True
        progreso.Visibility = Visibility.Collapsed

    End Sub

End Module

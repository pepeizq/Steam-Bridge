Imports Windows.Storage
Imports Windows.Storage.AccessCache
Imports Windows.Storage.Pickers

Module Origin

    Public Async Function Config(picker As Boolean) As Task(Of Boolean)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim tbConfigPath As TextBlock = pagina.FindName("tbOriginConfigPath")
        Dim buttonConfigPath As TextBlock = pagina.FindName("buttonOriginConfigPathTexto")
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
                carpeta = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("OriginPath")
            End If

            If Not carpeta Is Nothing Then
                If carpeta.Path.Contains("Origin\LocalContent") Then
                    StorageApplicationPermissions.FutureAccessList.AddOrReplace("OriginPath", carpeta)
                    tbConfigPath.Text = carpeta.Path
                    buttonConfigPath.Text = recursos.GetString("Boton Cambiar")
                    Registro.Mensaje(reg, "Config", "Origin detectado")
                    Return True
                Else
                    Registro.Mensaje(reg, "Config", "Origin no detectado")
                    Return False
                End If
            Else
                Registro.Mensaje(reg, "Config", "Origin no seleccionado")
                Return False
            End If
        Catch ex As Exception
            Registro.Mensaje(reg, "Config", "Origin error")
            Return False
        End Try

    End Function

    Public Async Sub Generar(listaJuegos As List(Of Juego), carpeta As StorageFolder)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim grid As Grid = pagina.FindName("gridOriginContenido")
        Dim progreso As ProgressBar = pagina.FindName("progressBarOrigin")

        grid.IsHitTestVisible = False
        progreso.Visibility = Visibility.Visible

        Dim listaGrid As New ListView With {
            .Name = "listaOrigin"
        }

        If Not carpeta Is Nothing Then
            Dim carpetasJuegos As IReadOnlyList(Of StorageFolder) = Await carpeta.GetFoldersAsync()

            For Each carpetaJuego As StorageFolder In carpetasJuegos
                Dim ficheros As IReadOnlyList(Of StorageFile) = Await carpetaJuego.GetFilesAsync()

                For Each fichero As StorageFile In ficheros
                    Dim ficheroext As String = fichero.DisplayName + fichero.FileType

                    If Not ficheroext = "map.crc" Then
                        Dim nombre As String = carpetaJuego.Name

                        Dim ejecutable As String = "origin://launchgame/" + fichero.DisplayName

                        If ejecutable.Contains("OFB-EAST") Then
                            ejecutable = ejecutable.Replace("OFB-EAST", "OFB-EAST:")
                        End If

                        If ejecutable.Contains("DR") Then
                            ejecutable = ejecutable.Replace("DR", "DR:")
                        End If

                        Dim argumentos As String = Nothing

                        Dim icono As StorageFile = Nothing

                        Dim tituloBool As Boolean = False
                        Dim i As Integer = 0
                        While i < listaJuegos.Count
                            If listaJuegos(i).Nombre = nombre Then
                                tituloBool = True
                            End If
                            i += 1
                        End While

                        If tituloBool = False Then
                            Dim juego As New Juego(nombre, ejecutable, argumentos, icono, Nothing, False, "Origin")

                            listaJuegos.Add(juego)

                            'Dim bitmap As BitmapImage = New BitmapImage
                            'If Not juego.Icono Is Nothing Then
                            '    Using stream As IRandomAccessStream = Await juego.Icono.OpenAsync(FileAccessMode.Read)
                            '        bitmap.DecodePixelWidth = 40
                            '        bitmap.DecodePixelHeight = 40
                            '        Await bitmap.SetSourceAsync(stream)
                            '    End Using
                            'End If

                            listaGrid.Items.Add(Listado.GenerarGrid(juego, Nothing, False))
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
                'Dim bitmap As BitmapImage = New BitmapImage
                'If Not juego.Icono Is Nothing Then
                '    Using stream As IRandomAccessStream = Await juego.Icono.OpenAsync(FileAccessMode.Read)
                '        bitmap.DecodePixelWidth = 40
                '        bitmap.DecodePixelHeight = 40
                '        Await bitmap.SetSourceAsync(stream)
                '    End Using
                'End If

                listaGrid.Items.Add(Listado.GenerarGrid(juego, Nothing, True))
            Next
        End If

        If listaJuegos.Count = 0 Then
            Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()
            Toast("Steam Bridge - Origin", recursos.GetString("Texto No Juegos"))

            Dim tbNoJuegos As TextBlock = pagina.FindName("tbOriginNoJuegos")
            tbNoJuegos.Visibility = Visibility.Visible
        Else
            Dim tbNoJuegos As TextBlock = pagina.FindName("tbOriginNoJuegos")
            tbNoJuegos.Visibility = Visibility.Collapsed
        End If

        grid.Children.Clear()
        grid.Children.Add(listaGrid)
        grid.IsHitTestVisible = True
        progreso.Visibility = Visibility.Collapsed

    End Sub

End Module

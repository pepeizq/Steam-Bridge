Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Windows.Storage
Imports Windows.Storage.AccessCache
Imports Windows.Storage.Pickers
Imports Windows.Storage.Streams

Module Origin

    Private Sub Icono(coleccion As HamburgerMenuItemCollection, hamburger As HamburgerMenu)

        hamburger.ItemsSource = Nothing

        Dim item As HamburgerMenuGlyphItem = New HamburgerMenuGlyphItem
        item.Tag = 1
        item.Label = "Origin"
        item.Glyph = "/Assets/origin_logo.png"
        coleccion.Add(item)
        coleccion.Sort(Function(x, y) x.Label.CompareTo(y.Label))

        hamburger.ItemsSource = coleccion

    End Sub

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

    Public Async Sub Generar(listaJuegos As List(Of Juego), carpeta As StorageFolder, grid As Grid, progreso As ProgressBar, coleccion As HamburgerMenuItemCollection, hamburger As HamburgerMenu)

        grid.IsHitTestVisible = False
        progreso.Visibility = Visibility.Visible

        Dim listaGrid As New ListView
        listaGrid.Name = "listaOrigin"

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

                            If listaJuegos.Count = 1 Then
                                Origin.Icono(coleccion, hamburger)
                            End If

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
        End If

        grid.Children.Clear()
        grid.Children.Add(listaGrid)
        grid.IsHitTestVisible = True
        progreso.Visibility = Visibility.Collapsed

    End Sub

End Module

Imports Windows.Storage
Imports Windows.Storage.Streams

Module Origin

    Public Async Sub Cargar(listaJuegos As List(Of Juego), carpetaLocalContent As StorageFolder, carpetaJuegos As StorageFolder, grid As Grid, progreso As ProgressBar, textobloque As TextBlock)

        grid.IsHitTestVisible = False
        progreso.Visibility = Visibility.Visible
        textobloque.Visibility = Visibility.Collapsed

        Dim listaGrid As New ListView
        listaGrid.Name = "listaOrigin"

        If Not carpetaLocalContent Is Nothing Then
            Dim carpetasJuegos As IReadOnlyList(Of StorageFolder) = Await carpetaLocalContent.GetFoldersAsync()

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

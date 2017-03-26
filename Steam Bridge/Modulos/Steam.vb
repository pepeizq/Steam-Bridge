Imports Microsoft.Toolkit.Uwp
Imports Windows.Graphics.Imaging
Imports Windows.Storage
Imports Windows.Storage.AccessCache
Imports Windows.Storage.Pickers
Imports Windows.Storage.Streams

Module Steam

    Public Async Sub Arranque(tbConfigPath As TextBlock, buttonConfigPath As TextBlock, reg As TextBox, picker As Boolean)

        Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()
        Dim carpeta As StorageFolder = Nothing

        Try
            If picker = True Then
                Dim carpetapicker As FolderPicker = New FolderPicker()

                carpetapicker.FileTypeFilter.Add("*")
                carpetapicker.ViewMode = PickerViewMode.List

                carpeta = Await carpetapicker.PickSingleFolderAsync()
            Else
                carpeta = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("SteamPath")
            End If

            If Not carpeta Is Nothing Then
                Dim ejecutable As StorageFile = Nothing

                Try
                    ejecutable = Await carpeta.GetFileAsync("Steam.exe")
                Catch ex As Exception

                End Try

                If Not ejecutable Is Nothing Then
                    StorageApplicationPermissions.FutureAccessList.AddOrReplace("SteamPath", carpeta)
                    tbConfigPath.Text = carpeta.Path
                    buttonConfigPath.Text = recursos.GetString("Boton Cambiar")
                    Registro.Mensaje(reg, "Config", "Steam detectado")
                Else
                    If picker = True Then
                        Registro.Mensaje(reg, "Config", "Steam no seleccionado")
                    Else
                        Registro.Mensaje(reg, "Config", "Steam no detectado")
                    End If
                End If
            Else
                If picker = True Then
                    Registro.Mensaje(reg, "Config", "Steam no seleccionado")
                Else
                    Registro.Mensaje(reg, "Config", "Steam no detectado")
                End If
            End If
        Catch ex As Exception
            Registro.Mensaje(reg, "Config", "Steam error")
        End Try

    End Sub

    Public Async Sub CrearAccesos(lista As List(Of Juego), carpeta As StorageFolder, boton As Button, reg As TextBox)

        boton.IsEnabled = False
        Registro.Mensaje(reg, "Steam", lista.Count.ToString + " juegos pendientes")

        Dim exito As Boolean = False
        Dim usuarioID As String = Nothing
        Dim listaSubcarpetas As IReadOnlyList(Of StorageFolder) = Await carpeta.GetFoldersAsync()

        For Each subcarpeta As StorageFolder In listaSubcarpetas
            If subcarpeta.Name.ToLower = "logs" Then
                Dim conexiones As StorageFile = Nothing

                Try
                    conexiones = Await subcarpeta.GetFileAsync("connection_log.txt")
                    Registro.Mensaje(reg, "Steam", "connection_log.txt detectado")
                Catch ex As Exception
                    Registro.Mensaje(reg, "Steam", "connection_log.txt no detectado")
                End Try

                If Not conexiones Is Nothing Then
                    Dim lineas As String = Nothing

                    Try
                        lineas = Await FileIO.ReadTextAsync(conexiones)
                        Registro.Mensaje(reg, "Steam", "connection_log.txt cargado")
                    Catch ex As Exception
                        Registro.Mensaje(reg, "Steam", "connection_log.txt no cargado")
                    End Try

                    If Not lineas = Nothing Then
                        If lineas.Contains("SetSteamID") Then
                            Dim temp, temp2, temp3 As String
                            Dim int, int2, int3 As Integer

                            int = lineas.LastIndexOf("SetSteamID")
                            temp = lineas.Remove(0, int + 5)

                            Dim i As Integer = 0
                            While i < 100
                                If temp.Contains("[U:1:") Then
                                    int2 = temp.IndexOf("[U:1:")
                                    temp2 = temp.Remove(0, int2 + 5)

                                    temp = temp2

                                    int3 = temp2.IndexOf("]")
                                    temp3 = temp2.Remove(int3, temp2.Length - int3)

                                    usuarioID = temp3.Trim

                                    If usuarioID.Length > 1 Then
                                        Exit While
                                    End If
                                End If
                                i += 1
                            End While
                        End If
                    End If
                End If
            End If
        Next

        '------------------------------------------------

        Dim numero As Integer = 0

        If usuarioID = Nothing Then
            Registro.Mensaje(reg, "Steam", "usuarioID no detectado")
        Else
            Registro.Mensaje(reg, "Steam", "usuarioID detectado")
            Dim shortcuts As StorageFile = Nothing

            Try
                shortcuts = Await carpeta.GetFileAsync("userdata\" + usuarioID + "\config\shortcuts.vdf")
                Registro.Mensaje(reg, "Steam", "shortcuts.vdf detectado")
            Catch ex As Exception
                Registro.Mensaje(reg, "Steam", "shortcuts.vdf no detectado")
            End Try

            Dim lineas As String = Nothing

            If shortcuts Is Nothing Then
                shortcuts = Await carpeta.CreateFileAsync("userdata\" + usuarioID + "\config\shortcuts.vdf", CreationCollisionOption.ReplaceExisting)
                Registro.Mensaje(reg, "Steam", "shortcuts.vdf creado")
            Else
                lineas = Await StorageFileHelper.ReadTextFromFileAsync(carpeta, "userdata\" + usuarioID + "\config\shortcuts.vdf")
                Registro.Mensaje(reg, "Steam", "shortcuts.vdf cargado")
            End If

            If Not lineas = Nothing Then
                If lineas.Contains("appname") Then
                    Dim lineasTemp As String = lineas

                    Dim temp4 As String
                    Dim int4 As Integer

                    Dim i As Integer = 0
                    While i < 1000
                        If lineasTemp.Contains("appname") Then
                            int4 = lineasTemp.IndexOf("appname")
                            temp4 = lineasTemp.Remove(0, int4 + 2)
                            lineasTemp = temp4
                            numero += 1
                        End If
                        i += 1
                    End While
                Else
                    numero = 0
                End If
            Else
                numero = 0
            End If

            If Not lineas = Nothing Then
                lineas = lineas.Remove(lineas.Length - 2, 2)
            Else
                lineas = ChrW(0) + "shortcuts" + ChrW(0)
            End If

            For Each juego As Juego In lista
                Dim nombre As String = juego.Nombre

                If Not nombre = Nothing Then
                    nombre = nombre.Replace("™", Nothing)
                    nombre = nombre.Replace("®", Nothing)

                    nombre = nombre.Replace(":", Nothing)
                    nombre = nombre.Replace("\", Nothing)
                    nombre = nombre.Replace("/", Nothing)
                    nombre = nombre.Replace("*", Nothing)
                    nombre = nombre.Replace("?", Nothing)
                    nombre = nombre.Replace(ChrW(34), Nothing)
                    nombre = nombre.Replace("<", Nothing)
                    nombre = nombre.Replace(">", Nothing)
                    nombre = nombre.Replace("|", Nothing)
                End If

                Dim categoria As String = Nothing

                categoria = ChrW(1) + "0" + ChrW(0) + juego.Categoria + ChrW(0)

                '------------------------------------------------

                Dim imagen As String = Nothing

                If Not juego.Icono Is Nothing Then
                    imagen = juego.Icono.Path
                End If

                If Not imagen = Nothing Then
                    If imagen.Contains(".ico") Then
                        Dim icono As StorageFile = Nothing

                        Try
                            icono = Await StorageFile.GetFileFromPathAsync(imagen)
                        Catch ex As Exception

                        End Try

                        If Not icono Is Nothing Then
                            Dim carpetaIcono As StorageFolder = Nothing

                            Try
                                carpetaIcono = Await carpeta.GetFolderAsync("userdata\" + usuarioID + "\config\icons")
                            Catch ex As Exception

                            End Try

                            If carpetaIcono Is Nothing Then
                                carpetaIcono = Await carpeta.CreateFolderAsync("userdata\" + usuarioID + "\config\icons")
                            End If

                            Try
                                Await carpetaIcono.CreateFileAsync(nombre + ".png")
                            Catch ex As Exception

                            End Try

                            Dim writeableBitmap As New WriteableBitmap(32, 32)

                            Using fileStream As IRandomAccessStream = Await icono.OpenAsync(FileAccessMode.Read)
                                Await writeableBitmap.SetSourceAsync(fileStream)
                            End Using

                            Dim iconoFichero As StorageFile = Await StorageFile.GetFileFromPathAsync(carpetaIcono.Path + "\" + nombre + ".png")
                            Dim stream As IRandomAccessStream = Await iconoFichero.OpenAsync(FileAccessMode.ReadWrite)
                            Dim encoder As BitmapEncoder = Await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream)

                            Dim pixelStream As Stream = writeableBitmap.PixelBuffer.AsStream()
                            Dim pixels As Byte() = New Byte(pixelStream.Length - 1) {}
                            Await pixelStream.ReadAsync(pixels, 0, pixels.Length)

                            encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Straight, CUInt(writeableBitmap.PixelWidth), CUInt(writeableBitmap.PixelHeight), 96.0, 96.0, pixels)
                            Await encoder.FlushAsync()
                            stream.Dispose()

                            imagen = iconoFichero.Path
                        End If
                    End If
                End If

                '------------------------------------------------

                Dim ejecutable As String = juego.Ejecutable
                Dim argumentos As String = " " + juego.Argumentos

                If Not ejecutable = Nothing Then
                    If Not argumentos = Nothing Then
                        argumentos = argumentos.Replace("&quot;", ChrW(34))
                        argumentos = argumentos.Replace(ChrW(34) + "exit" + ChrW(34), "exit")
                    End If

                    Dim boolvbs As Boolean = False
                    Dim opciones As ApplicationDataContainer = ApplicationData.Current.LocalSettings

                    If opciones.Values("WindowsStoreSteamOverlay") = True Then
                        If juego.Categoria = "Windows Store" Then
                            boolvbs = True
                        End If
                    End If

                    If juego.Categoria = "Battle.net" Then
                        boolvbs = True
                    End If

                    If boolvbs = True Then
                        Dim carpetaVbs As StorageFolder = Nothing

                        Try
                            carpetaVbs = Await carpeta.GetFolderAsync("userdata\" + usuarioID + "\config\shortcutsvbs")
                        Catch ex As Exception

                        End Try

                        If carpetaVbs Is Nothing Then
                            carpetaVbs = Await carpeta.CreateFolderAsync("userdata\" + usuarioID + "\config\shortcutsvbs")
                        End If

                        If Not carpetaVbs Is Nothing Then
                            Dim vbsFichero As StorageFile = Nothing

                            Try
                                vbsFichero = Await carpetaVbs.GetFileAsync(nombre + ".vbs")
                                Await vbsFichero.DeleteAsync()
                            Catch ex As Exception

                            End Try

                            vbsFichero = Await carpetaVbs.CreateFileAsync(nombre + ".vbs")
                            Await FileIO.WriteTextAsync(vbsFichero, FicheroVbs.Contenido(juego.Ejecutable, juego.Argumentos))

                            ejecutable = "C:\Windows\System32\cscript.exe" + ChrW(34) + " " + ChrW(34) + vbsFichero.Path
                            argumentos = Nothing
                        End If
                    End If
                End If

                '------------------------------------------------

                Dim inicio As String = Nothing

                If ejecutable.ToLower.Contains("dosbox\dosbox.exe") Then
                    Dim temp As String
                    Dim int As Integer

                    int = juego.Ejecutable.IndexOf("dosbox.exe")
                    temp = juego.Ejecutable.Remove(int, juego.Ejecutable.Length - int)

                    inicio = temp
                ElseIf ejecutable.Contains("C:\Windows\System32\cscript.exe") Then
                    inicio = "C:\Windows\System32\"
                Else
                    inicio = "C:\Windows\"
                End If

                lineas = lineas + ChrW(0) + numero.ToString + ChrW(0) + ChrW(1) + "appname" + ChrW(0) + nombre + ChrW(0) + ChrW(1) + "exe" + ChrW(0) + ChrW(34) + ejecutable + ChrW(34) + argumentos + ChrW(0) + ChrW(1) + "StartDir" + ChrW(0) + ChrW(34) + inicio + ChrW(34) + ChrW(0) + ChrW(1) + "icon" + ChrW(0) + imagen + ChrW(0) + ChrW(1) + "ShortcutPath" + ChrW(0) + ChrW(0) + ChrW(2) + "IsHidden" + ChrW(0) + ChrW(0) + ChrW(0) + ChrW(0) + ChrW(0) + ChrW(2) + "AllowDesktopConfig" + ChrW(0) + ChrW(1) + ChrW(0) + ChrW(0) + ChrW(0) + ChrW(2) + "OpenVR" + ChrW(0) + ChrW(0) + ChrW(0) + ChrW(0) + ChrW(0) + ChrW(0) + "tags" + ChrW(0) + categoria + ChrW(8) + ChrW(8)
                Registro.Mensaje(reg, "Steam", nombre + " precargado")

                numero += 1
            Next

            lineas = lineas + ChrW(8) + ChrW(8)

            Await FileIO.WriteTextAsync(shortcuts, lineas)
            exito = True
        End If

        boton.IsEnabled = True

        Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()

        If exito = True Then
            If lista.Count = 1 Then
                Notificaciones.Toast("Steam Bridge", recursos.GetString("Exito"))
            Else
                Notificaciones.Toast("Steam Bridge", recursos.GetString("Exitos"))
            End If
            Registro.Mensaje(reg, "Steam", "shortcuts.vdf actualizado")
        Else
            Notificaciones.Toast("Steam Bridge", recursos.GetString("Error 1"))
            Registro.Mensaje(reg, "Steam", "shortcuts.vdf fallo en la escritura")
        End If

        Registro.Mensaje(reg, "Steam", lista.Count.ToString + " juegos añadidos")

    End Sub

End Module

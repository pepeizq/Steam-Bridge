Imports Windows.Storage

Module Steam

    Public Async Sub CrearAccesos(lista As List(Of Juego), carpeta As StorageFolder, boton As Button)

        boton.IsEnabled = False

        Dim exito As Boolean = False
        Dim usuarioID As String = Nothing
        Dim listaSubcarpetas As IReadOnlyList(Of StorageFolder) = Await carpeta.GetFoldersAsync()

        For Each subcarpeta As StorageFolder In listaSubcarpetas
            If subcarpeta.Name.ToLower = "logs" Then
                Dim conexiones As StorageFile = Nothing

                Try
                    conexiones = Await subcarpeta.GetFileAsync("connection_log.txt")
                Catch ex As Exception

                End Try

                If Not conexiones Is Nothing Then
                    Dim lineas As String = Nothing

                    Try
                        lineas = Await FileIO.ReadTextAsync(conexiones)
                    Catch ex As Exception

                    End Try

                    If Not lineas = Nothing Then
                        If lineas.Contains("SetSteamID") Then
                            Dim temp, temp2, temp3 As String
                            Dim int, int2, int3 As Integer

                            int = lineas.LastIndexOf("SetSteamID")
                            temp = lineas.Remove(0, int + 5)

                            int2 = temp.IndexOf("[U:1:")
                            temp2 = temp.Remove(0, int2 + 5)

                            int3 = temp2.IndexOf("]")
                            temp3 = temp2.Remove(int3, temp2.Length - int3)

                            usuarioID = temp3.Trim
                        End If
                    End If
                End If
            End If
        Next

        '---------------------------------------------

        If Not usuarioID = Nothing Then
            Dim shortcuts As StorageFile = Nothing

            Try
                shortcuts = Await carpeta.GetFileAsync("userdata\" + usuarioID + "\config\shortcuts.vdf")
                Await shortcuts.DeleteAsync()
            Catch ex As Exception

            End Try

            shortcuts = Await carpeta.CreateFileAsync("userdata\" + usuarioID + "\config\shortcuts.vdf", CreationCollisionOption.ReplaceExisting)

            Dim lineas As String = Nothing
            lineas = ChrW(0) + "shortcuts" + ChrW(0)

            Dim numero As Integer = 0

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

                Dim imagen As String = juego.Icono.Path

                'If Not imagen = Nothing Then
                '    If imagen.Contains(".ico") Then
                '        Dim icono As BitmapIcon = New BitmapIcon()
                '        icono.UriSource = New Uri(imagen)

                '        Dim imagenIcono As Image = icono
                '    End If
                'End If

                Dim ejecutable As String = juego.Ejecutable
                Dim argumentos As String = " " + juego.Argumentos

                If Not ejecutable = Nothing Then
                    If Not argumentos = Nothing Then
                        argumentos = argumentos.Replace("&quot;", ChrW(34))
                        argumentos = argumentos.Replace(ChrW(34) + "exit" + ChrW(34), "exit")
                    End If

                    If juego.Categoria = "Windows Store" Then
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
                            Dim nombreTemp As String = juego.Nombre

                            nombreTemp = nombreTemp.Replace(":", Nothing)
                            nombreTemp = nombreTemp.Replace("\", Nothing)
                            nombreTemp = nombreTemp.Replace("/", Nothing)
                            nombreTemp = nombreTemp.Replace("*", Nothing)
                            nombreTemp = nombreTemp.Replace("?", Nothing)
                            nombreTemp = nombreTemp.Replace(ChrW(34), Nothing)
                            nombreTemp = nombreTemp.Replace("|", Nothing)
                            nombreTemp = nombreTemp.Replace("<", Nothing)
                            nombreTemp = nombreTemp.Replace(">", Nothing)

                            Try
                                vbsFichero = Await carpetaVbs.GetFileAsync(nombreTemp + ".vbs")
                                Await vbsFichero.DeleteAsync()
                            Catch ex As Exception

                            End Try

                            vbsFichero = Await carpetaVbs.CreateFileAsync(nombreTemp + ".vbs")
                            Await FileIO.WriteTextAsync(vbsFichero, FicheroVbs.Contenido(juego.Ejecutable, juego.Argumentos))

                            ejecutable = vbsFichero.Path
                            argumentos = Nothing
                        End If
                    End If
                End If

                Dim inicio As String = Nothing

                If Not juego.Ejecutable.ToLower.Contains("dosbox\dosbox.exe") Then
                    inicio = "C:\Windows\"
                Else
                    Dim temp As String
                    Dim int As Integer

                    int = juego.Ejecutable.IndexOf("\dosbox.exe")
                    temp = juego.Ejecutable.Remove(int, juego.Ejecutable.Length - int)

                    inicio = temp
                End If

                lineas = lineas + ChrW(0) + numero.ToString + ChrW(0) + ChrW(1) + "appname" + ChrW(0) + nombre + ChrW(0) + ChrW(1) + "exe" + ChrW(0) + ChrW(34) + ejecutable + ChrW(34) + argumentos + ChrW(0) + ChrW(1) + "StartDir" + ChrW(0) + ChrW(34) + inicio + ChrW(34) + ChrW(0) + ChrW(1) + "icon" + ChrW(0) + imagen + ChrW(0) + ChrW(1) + "ShortcutPath" + ChrW(0) + ChrW(0) + ChrW(2) + "IsHidden" + ChrW(0) + ChrW(0) + ChrW(0) + ChrW(0) + ChrW(0) + ChrW(2) + "AllowDesktopConfig" + ChrW(0) + ChrW(1) + ChrW(0) + ChrW(0) + ChrW(0) + ChrW(2) + "OpenVR" + ChrW(0) + ChrW(0) + ChrW(0) + ChrW(0) + ChrW(0) + ChrW(0) + "tags" + ChrW(0) + categoria + ChrW(8) + ChrW(8)

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
                MessageBox.EnseñarMensaje(recursos.GetString("Exito"))
            Else
                MessageBox.EnseñarMensaje(recursos.GetString("Exitos"))
            End If
        Else
            MessageBox.EnseñarMensaje(recursos.GetString("Error 1"))
        End If

    End Sub

End Module

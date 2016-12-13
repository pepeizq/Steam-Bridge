Imports Microsoft.Toolkit.Uwp.UI.Controls

Module Hamburger

    Public Sub Generar(label As String, tag As String, glyph As String, coleccion As HamburgerMenuItemCollection, hamburger As HamburgerMenu)

        hamburger.ItemsSource = Nothing

        Dim item As HamburgerMenuGlyphItem = New HamburgerMenuGlyphItem
        item.Label = label
        item.Tag = tag
        item.Glyph = glyph

        coleccion.Add(item)
        coleccion.Sort(Function(x, y) x.Label.CompareTo(y.Label))

        hamburger.ItemsSource = coleccion
        hamburger.SelectedIndex = 0

        Dim coleccionOpciones As HamburgerMenuItemCollection = hamburger.OptionsItemsSource
        hamburger.OptionsItemsSource = Nothing
        hamburger.OptionsItemsSource = coleccionOpciones

    End Sub

End Module

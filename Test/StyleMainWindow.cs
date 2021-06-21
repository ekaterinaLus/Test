using Syncfusion.WinForms.DataGrid.Styles;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Test
{
    public static class StyleMainWindow
    {
        public static void MakeWindowStyle(this DataGrid dataGrid1)
        {
            Style buttonStyle = new Style();
            buttonStyle.Setters.Add(new Setter { Property = Control.FontFamilyProperty, Value = new FontFamily("Verdana") });
            buttonStyle.Setters.Add(new Setter { Property = Control.HorizontalAlignmentProperty, Value = HorizontalAlignment.Center });
            dataGrid1.Style = buttonStyle;
        }
    }
}

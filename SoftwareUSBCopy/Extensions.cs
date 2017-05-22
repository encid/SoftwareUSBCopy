using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Windows.Forms;
using System.Drawing;

namespace SoftwareUSBCopy
{
    public static class Extensions
    {
        public static string Pluralize(this string str, int n)
        {
            if (n != 1)
                return PluralizationService.CreateService(new CultureInfo("en-US"))
                .Pluralize(str);
            return str;
        }

        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;
            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }
    }
}

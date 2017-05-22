using System;
using System.Drawing;
using System.Windows.Forms;

namespace SoftwareUSBCopy
{
    public static class Logger
    {
        /// <summary>
        /// Write a message to the status log textbox, with default black foreground text color.
        /// </summary>
        /// <param name="message">Message to write to the status log.</param>
        /// <param name="box">RichTextBox control to use for writing</param>
        public static void Log(string message, RichTextBox box)
        {
            if (string.IsNullOrEmpty(box.Text))
            {
                box.AppendText(message, Color.Black);
            }
            else
                box.AppendText(Environment.NewLine + message, Color.Black);
        }

        /// <summary>
        /// Write a message to the status log textbox, a specified foreground text color.
        /// </summary>
        /// <param name="message">Message to write to the status log.</param>
        /// <param name="box">RichTextBox control to use for writing.</param>
        /// <param name="color">Foreground color of text.</param>
        public static void Log(string message, RichTextBox box, Color color)
        {
            if (string.IsNullOrEmpty(box.Text))
            {
                box.AppendText(message, color);
            }
            else
                box.AppendText(Environment.NewLine + message, color);
        }
    }
}

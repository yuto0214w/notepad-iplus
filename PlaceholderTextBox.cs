using System.Windows.Forms;
using System.Drawing;

namespace NotepadIPlus
{
    public partial class PlaceholderTextBox : TextBox
    {
        public PlaceholderTextBox()
        {
            _placeholderText = "";
        }

        #region Property

        private string _placeholderText;
        public string PlaceholderText
        {
            get
            {
                return _placeholderText;
            }

            set
            {
                _placeholderText = value;
                Refresh();
            }
        }

        #endregion

        private Color CreateNeutralColor()
        {
            return Color.FromArgb(
                ForeColor.A >> 1 + BackColor.A >> 1,
                ForeColor.R >> 1 + BackColor.R >> 1,
                ForeColor.G >> 1 + BackColor.G >> 1,
                ForeColor.B >> 1 + BackColor.B >> 1
            );
        }

        protected override void WndProc(ref Message message)
        {
            base.WndProc(ref message);

            if (message.Msg == 15 && Enabled && !ReadOnly && !Focused &&
                string.IsNullOrEmpty(Text) &&
                !string.IsNullOrEmpty(_placeholderText))
            {
                using (Graphics graphics = CreateGraphics())
                {
                    Brush brush = new SolidBrush(BackColor);
                    graphics.FillRectangle(brush, ClientRectangle);

                    Color placeholderColor = CreateNeutralColor();

                    graphics.DrawString(_placeholderText, Font,
                        new SolidBrush(placeholderColor), 1, 1);
                }
            }
        }
    }
}

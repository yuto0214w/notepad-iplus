using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NotepadIPlus
{
    public partial class Splash : Form
    {
        public Splash()
        {
            InitializeComponent();
        }

        private async void FormLoad(object sender, EventArgs e)
        {
            await Task.Delay(3000);
            Close();
        }
    }
}

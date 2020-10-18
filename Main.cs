using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using NotepadIPlus.Properties;

namespace NotepadIPlus
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        readonly CheckFileExtension checkFileExtension = new CheckFileExtension();

        private void FormLoad(object sender, EventArgs e)
        {
            enableTopMostToolStripMenuItem.Checked = Settings.Default.TopMostEnabled;
            enableShowSplashToolStripMenuItem.Checked = Settings.Default.ShowSplash;
            TopMost = Settings.Default.TopMostEnabled;
            textBox.Text = Settings.Default.TextBoxText;

            if (Settings.Default.ShowSplash && !Settings.Default.fromNewWindow)
            {
                Splash form = new Splash();
                form.ShowDialog(this);
                form.Dispose();
            }

            if (Settings.Default.fromNewWindow)
            {
                Settings.Default.fromNewWindow = false;
                Settings.Default.Save();
            }

            if (Settings.Default.EditingFile != string.Empty)
            {
                try
                {
                    StreamReader sr;

                    if (checkFileExtension.IsThisFileWindowsCommandFile(Settings.Default.EditingFile))
                        sr = new StreamReader(
                            Settings.Default.EditingFile, Encoding.GetEncoding("Shift-JIS"));

                    else sr = new StreamReader(
                        Settings.Default.EditingFile, Encoding.UTF8);

                    textBox.Text = sr.ReadToEnd();
                    sr.Close();
                    SetTitle(Settings.Default.EditingFile);
                }

                catch (Exception)
                {
                    SetTitle("Untitled");
                    Settings.Default.EditingFile = string.Empty;
                    Settings.Default.Save();
                }
            }
        }

        private void CloseForm(object sender, FormClosingEventArgs e)
        {
            if (Text.EndsWith("(Not Saved)"))
            {
                DialogResult result = MessageBox.Show(
                    /* Message */ $"Do you want to save changes to { Text.Replace(" - Notepad i+ (Not Saved)", "") }?",
                    /* Title   */ "Confirm",
                    /* Button  */ MessageBoxButtons.YesNoCancel,
                    /* Icon    */ MessageBoxIcon.Information
                );

                if (result == DialogResult.Yes)
                {
                    if (Settings.Default.EditingFile != string.Empty)
                    {
                        try
                        {
                            StreamWriter sw;

                            if (checkFileExtension.IsThisFileWindowsCommandFile(Settings.Default.EditingFile))
                                sw = new StreamWriter(Settings.Default.EditingFile,
                                    false, Encoding.GetEncoding("Shift-JIS"));

                            else sw =
                                new StreamWriter(Settings.Default.EditingFile, false, Encoding.UTF8);

                            sw.Write(textBox.Text);
                            sw.Close();
                        }

                        catch (Exception)
                        {
                            Save();
                        }
                    }

                    else Save();
                }

                else if (result == DialogResult.Cancel) e.Cancel = true;
            }
        }

        private void TextBoxTextChanged(object sender, EventArgs e)
        {
            if (!Text.EndsWith("(Not Saved)")) Text += " (Not Saved)";
            Settings.Default.TextBoxText = textBox.Text;
            Settings.Default.Save();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox.Text = string.Empty;
            SetTitle("Untitled");
            Settings.Default.EditingFile = string.Empty;
            Settings.Default.Save();
        }

        private void newWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.Default.fromNewWindow = true;
            Settings.Default.Save();
            string path = Assembly.GetEntryAssembly().Location;
            Process.Start(path);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                Filter = "Text Documents (*.txt)|*.txt|Bat Files (*.bat;*.cmd)|*.bat;*.cmd|All Files (*.*)|*.*",
                Title = "Open",
                RestoreDirectory = true
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Stream stream = ofd.OpenFile();
                if (stream != null)
                {
                    StreamReader sr;

                    if (checkFileExtension.IsThisFileWindowsCommandFile(ofd.FileName))
                        sr = new StreamReader(stream, Encoding.GetEncoding("Shift-JIS"));
                    else sr = new StreamReader(stream, Encoding.UTF8);

                    textBox.Text = sr.ReadToEnd();
                    sr.Close();
                    stream.Close();
                }

                SetTitle(ofd.FileName);
                Settings.Default.EditingFile = ofd.FileName;
                Settings.Default.Save();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Settings.Default.EditingFile != string.Empty)
            {
                try
                {
                    StreamWriter sw;

                    if (checkFileExtension.IsThisFileWindowsCommandFile(Settings.Default.EditingFile))
                        sw = new StreamWriter(Settings.Default.EditingFile,
                            false, Encoding.GetEncoding("Shift-JIS"));

                    else sw =
                        new StreamWriter(Settings.Default.EditingFile, false, Encoding.UTF8);

                    sw.Write(textBox.Text);
                    sw.Close();

                    SetTitle(Settings.Default.EditingFile);
                }

                catch (Exception)
                {
                    Save();
                }
            }

            else Save();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void Save()
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                Filter = "Text Documents (*.txt)|*.txt|Bat Files (*.bat;*.cmd)|*.bat;*.cmd|All Files (*.*)|*.*",
                Title = "Save As",
                RestoreDirectory = true
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Stream stream = sfd.OpenFile();
                if (stream != null)
                {
                    StreamWriter sw = new StreamWriter(stream, Encoding.UTF8);
                    if (sfd.FilterIndex == 2) sw =
                        new StreamWriter(stream, Encoding.GetEncoding("Shift-JIS"));
                    sw.Write(textBox.Text);
                    sw.Close();
                    stream.Close();
                }

                SetTitle(sfd.FileName);
                Settings.Default.EditingFile = sfd.FileName;
                Settings.Default.Save();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (checkFileExtension.IsThisFileWindowsCommandFile(Settings.Default.EditingFile))
                Process.Start(Settings.Default.EditingFile);

            else if (Settings.Default.EditingFile == string.Empty) MessageBox.Show(
                /* Message */ "You didn't select the file.\n(Currently Notepad i+ supporting only bat file)",
                /* Title   */ "Information",
                /* Button  */ MessageBoxButtons.OK,
                /* Icon    */ MessageBoxIcon.Error
            );

            else MessageBox.Show(
                /* Message */ "Sorry, but I can't.\nCurrently Notepad i+ supporting only bat file.",
                /* Title   */ "Information",
                /* Button  */ MessageBoxButtons.OK,
                /* Icon    */ MessageBoxIcon.Error
            );
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (textBox.CanUndo)
            {
                textBox.Undo();
                textBox.ClearUndo();
            }
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (textBox.SelectionLength > 0)
            {
                textBox.Cut();
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (textBox.SelectionLength > 0)
            {
                textBox.Copy();
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IDataObject data = Clipboard.GetDataObject();
            if (data != null && data.GetDataPresent(DataFormats.Text))
            {
                textBox.Paste();
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox.SelectedText = string.Empty;
        }

        private void translateWithDeepLTranslatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (textBox.SelectionLength != 0)
                Process.Start("https://www.deepl.com/translator#en/ja/" + textBox.SelectedText);
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox.SelectAll();
        }

        private void timeDateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox.SelectedText = DateTime.Now.ToString();
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontDialog.Font = textBox.Font;
            if (fontDialog.ShowDialog() != DialogResult.Cancel)
            {
                textBox.Font = fontDialog.Font;
            }
        }

        private void previewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Opacity = 0;
            MessageBox.Show(textBox.Text);
            Opacity = 1;
        }

        private void SetTitle(string filepath)
        {
            Text = $"{filepath} - Notepad i+";
        }

        private void enableTopMostToolStripMenuItem_Click(object sender, EventArgs e)
        {
            enableTopMostToolStripMenuItem.Checked = !enableTopMostToolStripMenuItem.Checked;
            if (enableTopMostToolStripMenuItem.Checked) TopMost = true;
            else TopMost = false;
            Settings.Default.TopMostEnabled = enableTopMostToolStripMenuItem.Checked;
            Settings.Default.Save();
        }

        private void enableShowSplashToolStripMenuItem_Click(object sender, EventArgs e)
        {
            enableShowSplashToolStripMenuItem.Checked = !enableShowSplashToolStripMenuItem.Checked;
            Settings.Default.ShowSplash = enableShowSplashToolStripMenuItem.Checked;
            Settings.Default.Save();
        }
    }
}

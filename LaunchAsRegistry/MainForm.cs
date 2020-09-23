using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LaunchAsRegistry {
    public partial class MainForm : Form {
        private int textBoxClicks;
        private Timer textBoxClicksTimer;
        private Point location;
        private Settings settings;
        private string workingFolderPathTemp, shortcutNameTemp;
        private Form dialog;

        public MainForm(Settings settings) {
            Text = Application.ProductName;
            Icon = Properties.Resources.Icon;
            dialog = null;

            textBoxClicks = 0;
            textBoxClicksTimer = new Timer();

            InitializeComponent();

            this.settings = settings;
            textBox1.Text = settings.ApplicationFilePath;
            textBox2.Text = settings.Arguments;
            textBox3.Text = settings.WorkingFolderPath;
            textBox4.Text = settings.RegFilePath;
            textBox5.Text = settings.ShortcutName;
            checkBox.Checked = settings.OneInstance;
        }

        private void TextBoxMouseDown(object sender, MouseEventArgs e) {
            if (e.Button != MouseButtons.Left) {
                textBoxClicks = 0;
                return;
            }
            TextBox textBox = (TextBox)sender;
            textBoxClicksTimer.Stop();
            if (textBox.SelectionLength > 0) {
                textBoxClicks = 2;
            } else if (textBoxClicks == 0 || Math.Abs(e.X - location.X) < 2 && Math.Abs(e.Y - location.Y) < 2) {
                textBoxClicks++;
            } else {
                textBoxClicks = 0;
            }
            location = e.Location;
            if (textBoxClicks == 3) {
                if (textBox.Multiline) {
                    int selectionEnd = Math.Min(textBox.Text.IndexOf('\r', textBox.SelectionStart), textBox.Text.IndexOf('\n', textBox.SelectionStart));
                    if (selectionEnd < 0) {
                        selectionEnd = textBox.TextLength;
                    }
                    selectionEnd = Math.Max(textBox.SelectionStart + textBox.SelectionLength, selectionEnd);
                    int selectionStart = Math.Min(textBox.SelectionStart, selectionEnd);
                    do {
                        selectionStart--;
                    } while (selectionStart > 0 && textBox.Text[selectionStart] != '\n' && textBox.Text[selectionStart] != '\r');
                    textBox.Select(selectionStart, selectionEnd - selectionStart);
                } else {
                    textBox.SelectAll();
                }
                textBoxClicks = 0;
                MouseEvent(MOUSEEVENTF_LEFTUP, Convert.ToUInt32(Cursor.Position.X), Convert.ToUInt32(Cursor.Position.X), 0, 0);
                textBox.Focus();
            } else {
                textBoxClicksTimer.Interval = SystemInformation.DoubleClickTime;
                textBoxClicksTimer.Start();
                textBoxClicksTimer.Tick += new EventHandler((s, t) => {
                    textBoxClicksTimer.Stop();
                    textBoxClicks = 0;
                });
            }
        }

        private void MainFormDragDrop(object sender, DragEventArgs e) {
            try {
                string filePath = ((string[])e.Data.GetData(DataFormats.FileDrop, false))[0];
                if (filePath.EndsWith(Constants.ExtensionReg, StringComparison.OrdinalIgnoreCase)) {
                    textBox4.Text = filePath;
                    textBox4.Focus();
                    textBox4.SelectAll();
                } else {
                    if (string.IsNullOrWhiteSpace(textBox3.Text) || !Directory.Exists(textBox3.Text) || textBox3.Text == workingFolderPathTemp) {
                        textBox3.Text = Path.GetDirectoryName(filePath);
                        textBox3.SelectAll();
                        workingFolderPathTemp = textBox3.Text;
                    }
                    if (string.IsNullOrWhiteSpace(textBox5.Text) || textBox5.Text == shortcutNameTemp) {
                        textBox5.Text = Application.ProductName + " " + Path.GetFileNameWithoutExtension(filePath);
                        textBox5.SelectAll();
                        shortcutNameTemp = textBox5.Text;
                    }
                    textBox1.Text = filePath;
                    textBox1.Focus();
                    textBox1.SelectAll();
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
                dialog = new MessageForm(this, exception.Message, Program.GetTitle() + Constants.NDashWithSpaces + Properties.Resources.CaptionError, MessageForm.Buttons.OK, MessageForm.BoxIcon.Error);
                dialog.ShowDialog();
            }
        }

        private void MainFormDragEnter(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false)) {
                e.Effect = DragDropEffects.All;
            } else {
                e.Effect = DragDropEffects.None;
            }
        }

        private void Close(object sender, EventArgs e) {
            Close();
        }

        private void ShowAbout(object sender, EventArgs e) {
            dialog = new AboutForm();
            dialog.ShowDialog();
        }

        private void KeyDownHandler(object sender, KeyEventArgs e) {
            if (e.Control && e.KeyCode == Keys.A) {
                e.SuppressKeyPress = true;
                if (sender is TextBox) {
                    ((TextBox)sender).SelectAll();
                }
            }
        }

        private void SelectApplication(object sender, EventArgs e) {
            try {
                if (!string.IsNullOrEmpty(textBox1.Text)) {
                    string directoryPath = Path.GetDirectoryName(textBox1.Text);
                    if (Directory.Exists(directoryPath)) {
                        openFileDialog1.InitialDirectory = directoryPath;
                    }
                    if (File.Exists(textBox1.Text)) {
                        openFileDialog1.FileName = Path.GetFileName(textBox1.Text);
                    }
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
            try {
                if (openFileDialog1.ShowDialog() == DialogResult.OK) {
                    textBox1.Text = openFileDialog1.FileName;
                    if (string.IsNullOrWhiteSpace(textBox3.Text) || !Directory.Exists(textBox3.Text) || textBox3.Text == workingFolderPathTemp) {
                        textBox3.Text = Path.GetDirectoryName(textBox1.Text);
                        textBox3.SelectAll();
                        workingFolderPathTemp = textBox3.Text;
                    }
                    if (string.IsNullOrWhiteSpace(textBox5.Text) || textBox5.Text == shortcutNameTemp) {
                        textBox5.Text = Application.ProductName + " " + Path.GetFileNameWithoutExtension(textBox1.Text);
                        textBox5.SelectAll();
                        shortcutNameTemp = textBox5.Text;
                    }
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
                dialog = new MessageForm(this, exception.Message, Program.GetTitle() + Constants.NDashWithSpaces + Properties.Resources.CaptionError, MessageForm.Buttons.OK, MessageForm.BoxIcon.Error);
                dialog.ShowDialog();
            } finally {
                textBox1.Focus();
                textBox1.SelectAll();
            }
        }

        private void SelectFolder(object sender, EventArgs e) {
            try {
                if (!string.IsNullOrEmpty(textBox3.Text)) {
                    if (Directory.Exists(textBox3.Text)) {
                        folderBrowserDialog.SelectedPath = textBox3.Text;
                    }
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
            try {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK) {
                    if (textBox3.Text != folderBrowserDialog.SelectedPath) {
                        textBox3.Text = folderBrowserDialog.SelectedPath;
                        workingFolderPathTemp = textBox3.Text;
                    }
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
                dialog = new MessageForm(this, exception.Message, Program.GetTitle() + Constants.NDashWithSpaces + Properties.Resources.CaptionError, MessageForm.Buttons.OK, MessageForm.BoxIcon.Error);
                dialog.ShowDialog();
            } finally {
                textBox3.Focus();
                textBox3.SelectAll();
            }
        }

        private void SelectRegFile(object sender, EventArgs e) {
            try {
                if (!string.IsNullOrEmpty(textBox4.Text)) {
                    string directoryPath = Path.GetDirectoryName(textBox4.Text);
                    if (Directory.Exists(directoryPath)) {
                        openFileDialog2.InitialDirectory = directoryPath;
                    }
                    if (File.Exists(textBox4.Text)) {
                        openFileDialog2.FileName = Path.GetFileName(textBox4.Text);
                    }
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
            try {
                if (openFileDialog2.ShowDialog() == DialogResult.OK) {
                    textBox4.Text = openFileDialog2.FileName;
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
                dialog = new MessageForm(this, exception.Message, Program.GetTitle() + Constants.NDashWithSpaces + Properties.Resources.CaptionError, MessageForm.Buttons.OK, MessageForm.BoxIcon.Error);
                dialog.ShowDialog();
            } finally {
                textBox4.Focus();
                textBox4.SelectAll();
            }
        }

        private void EditRegFile(object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(textBox4.Text) || !textBox4.Text.EndsWith(Constants.ExtensionReg, StringComparison.OrdinalIgnoreCase)) {
                dialog = new MessageForm(this, Properties.Resources.MessageRegFileNotSet, null, MessageForm.Buttons.OK, MessageForm.BoxIcon.Exclamation);
                dialog.ShowDialog();
                textBox4.Focus();
                textBox4.SelectAll();
                return;
            }
            if (!File.Exists(textBox4.Text)) {
                dialog = new MessageForm(this, Properties.Resources.MessageRegFileNotFound, null, MessageForm.Buttons.OK, MessageForm.BoxIcon.Exclamation);
                dialog.ShowDialog();
                textBox4.Focus();
                textBox4.SelectAll();
                return;
            }
            try {
                Process process = new Process();
                process.StartInfo.FileName = Constants.NotepadExe;
                process.StartInfo.Arguments = ArgumentParser.EscapeArgument(textBox4.Text);
                process.Start();
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
                dialog = new MessageForm(this, exception.Message, Program.GetTitle() + Constants.NDashWithSpaces + Properties.Resources.CaptionError, MessageForm.Buttons.OK, MessageForm.BoxIcon.Error);
                dialog.ShowDialog();
            }
        }

        private void Launch(object sender, EventArgs e) {
            try {
                List<string> arguments = BuildArguments();
                Process process = new Process();
                process.StartInfo.FileName = Application.ExecutablePath;
                process.StartInfo.Arguments = string.Join(" ", arguments);
                process.StartInfo.WorkingDirectory = Application.StartupPath;
                process.Start();
                SaveSettings();
            } catch (ApplicationException exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
                dialog = new MessageForm(this, exception.Message, null, MessageForm.Buttons.OK, MessageForm.BoxIcon.Exclamation);
                dialog.ShowDialog();
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
                dialog = new MessageForm(this, exception.Message, Program.GetTitle() + Constants.NDashWithSpaces + Properties.Resources.CaptionError, MessageForm.Buttons.OK, MessageForm.BoxIcon.Error);
                dialog.ShowDialog();
            }
        }

        private List<string> BuildArguments() {
            List<string> arguments = new List<string>();
            string applicationFilePath = textBox1.Text;
            if (!string.IsNullOrWhiteSpace(applicationFilePath) && !File.Exists(applicationFilePath)) {
                throw new ApplicationException(Properties.Resources.MessageApplicationNotFound);
            }
            if (!string.IsNullOrWhiteSpace(applicationFilePath)) {
                arguments.Add("/i");
                arguments.Add(ArgumentParser.EscapeArgument(applicationFilePath));
            } else {
                throw new ApplicationException(Properties.Resources.MessageApplicationNotSet);
            }
            if (!string.IsNullOrWhiteSpace(textBox2.Text)) {
                arguments.Add("/a");
                arguments.Add(ArgumentParser.EscapeArgument(textBox2.Text));
            }
            if (Directory.Exists(textBox3.Text)) {
                arguments.Add("/w");
                arguments.Add(ArgumentParser.EscapeArgument(textBox3.Text));
            }
            if (checkBox.Checked) {
                arguments.Add("/o");
            }
            string regFilePath = textBox4.Text;
            if (!string.IsNullOrWhiteSpace(regFilePath) && !File.Exists(regFilePath)) {
                throw new ApplicationException(Properties.Resources.MessageRegFileNotFound);
            }
            if (!string.IsNullOrWhiteSpace(regFilePath)) {
                arguments.Add("/r");
                arguments.Add(ArgumentParser.EscapeArgument(regFilePath));
            } else {
                throw new ApplicationException(Properties.Resources.MessageRegFileNotSet);
            }
            return arguments;
        }

        private void CreateShortcut(object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(textBox5.Text)) {
                dialog = new MessageForm(this, Properties.Resources.MessageShortcutNameNotSet, null, MessageForm.Buttons.OK, MessageForm.BoxIcon.Exclamation);
                dialog.ShowDialog();
                textBox5.Focus();
                textBox5.SelectAll();
                return;
            }
            try {
                string shortcutFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), textBox5.Text);
                if (!shortcutFilePath.EndsWith(Constants.ExtensionLnk, StringComparison.OrdinalIgnoreCase)) {
                    shortcutFilePath += Constants.ExtensionLnk;
                }
                if (File.Exists(shortcutFilePath)) {
                    dialog = new MessageForm(this, Properties.Resources.MessageShortcutAlreadyExists, null, MessageForm.Buttons.YesNo, MessageForm.BoxIcon.Warning, MessageForm.DefaultButton.Button2);
                    if (dialog.ShowDialog() != DialogResult.Yes) {
                        textBox5.Focus();
                        textBox5.SelectAll();
                        return;
                    }
                }
                List<string> arguments = BuildArguments();
                ProgramShortcut programShortcut = new ProgramShortcut() {
                    ShortcutFilePath = shortcutFilePath,
                    TargetPath = Application.ExecutablePath,
                    WorkingFolder = Application.StartupPath,
                    Arguments = string.Join(" ", arguments),
                    IconLocation = textBox1.Text
                };
                programShortcut.Create();
            } catch (ApplicationException exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
                dialog = new MessageForm(this, exception.Message, null, MessageForm.Buttons.OK, MessageForm.BoxIcon.Exclamation);
                dialog.ShowDialog();
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
                dialog = new MessageForm(this, exception.Message, Program.GetTitle() + Constants.NDashWithSpaces + Properties.Resources.CaptionError, MessageForm.Buttons.OK, MessageForm.BoxIcon.Error);
                dialog.ShowDialog();
            }
        }

        private void MainFormClosing(object sender, FormClosingEventArgs e) {
            SaveSettings();
        }

        private void MainFormActivated(object sender, EventArgs e) {
            if (dialog != null) {
                dialog.Activate();
            }
        }

        private void OpenRegedit(object sender, EventArgs e) {
            try {
                Process process = new Process();
                process.StartInfo.FileName = Constants.RegeditExe;
                process.Start();
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
                dialog = new MessageForm(this, exception.Message, Program.GetTitle() + Constants.NDashWithSpaces + Properties.Resources.CaptionError, MessageForm.Buttons.OK, MessageForm.BoxIcon.Error);
                dialog.ShowDialog();
            }
        }

        private void OpenHelp(object sender, HelpEventArgs hlpevent) {
            try {
                Process.Start(Properties.Resources.Website.TrimEnd('/').ToLowerInvariant() + '/' + Application.ProductName.ToLowerInvariant() + '/');
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
                dialog = new MessageForm(this, exception.Message, Program.GetTitle() + Constants.NDashWithSpaces + Properties.Resources.CaptionError, MessageForm.Buttons.OK, MessageForm.BoxIcon.Error);
                dialog.ShowDialog();
            }
        }

        private void SaveSettings() {
            settings.ApplicationFilePath = textBox1.Text;
            settings.Arguments = textBox2.Text;
            settings.WorkingFolderPath = textBox3.Text;
            settings.RegFilePath = textBox4.Text;
            settings.ShortcutName = textBox5.Text;
            settings.OneInstance = checkBox.Checked;
            settings.Save();
        }

        [DllImport("user32.dll", EntryPoint = "mouse_event", SetLastError = true)]
        private static extern void MouseEvent(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Net.Mail;
using System.Net;



namespace PS_ZD_Windows_Form_
{
    public partial class Form1 : Form
    {
        private FileSystemWatcher _watcher;

        public Form1()
        {
            InitializeComponent();
            InitializeWatcher();
            T1.Enabled = true;

        }

        private void InitializeWatcher()
        {
            _watcher = new FileSystemWatcher();
            _watcher.Changed += _watcher_Changed;
            _watcher.Created += _watcher_Changed;
            _watcher.Deleted += _watcher_Changed;
            _watcher.Renamed += _watcher_Changed;
        }

        private void _watcher_Changed(object sender, FileSystemEventArgs e)
        {

            string log = "";

            switch (e.ChangeType)
            {
                case WatcherChangeTypes.Created:
                    //       MessageBox.Show($"Stworzono w folderze: {textBox_Path.Text}");
                    log = $"File Created: {e.Name}";
                    break;
                case WatcherChangeTypes.Deleted:
                    //MessageBox.Show($"Skasowano w folderze: {textBox_Path.Text}");
                    log = $"File Deleted: {e.Name}";
                    break;
                case WatcherChangeTypes.Changed:
                    //   MessageBox.Show($"Zmiana w folderze: {textBox_Path.Text}");
                    log = $"File Changed: {e.Name}";
                    break;
                case WatcherChangeTypes.Renamed:
                    //   MessageBox.Show($"Zmieniono nazwę w folderze: {textBox_Path.Text}");
                    log = $"File Renamed: {e.Name}";
                    break;
                case WatcherChangeTypes.All:
                    break;
                default:
                    break;
            }

            if (InvokeRequired)     // dodanie wyjatku do watku monitorowania folderu
            {
                BeginInvoke((Action)(() =>
                {
                    Listbox.Items.Insert(0, log);

                }));
            }


        }
        private void T1_Tick(object sender, EventArgs e)
        {
            DateTime date = DateTime.Now;

            label1.Text = $"Actual date : {date}";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();

            DialogResult result = folderBrowserDialog1.ShowDialog();

            if (result == DialogResult.OK) 
            {
                text_path.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void Form1_Leave(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();  //zapisanie zmian do zamykaniu okna
        }
    }
}

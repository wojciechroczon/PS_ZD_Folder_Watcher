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
            _watcher.Path = text_path.Text;
            _watcher.EnableRaisingEvents = true;  // event to monitorowania statusu sciezki dostepu
        }

     

        private void _watcher_Changed(object sender, FileSystemEventArgs e)
        {
            
            string log ="";

            switch (e.ChangeType)
            {
                case WatcherChangeTypes.Created:
                    //       MessageBox.Show($"Stworzono w folderze: {textBox_Path.Text}");
                    log = $"File Created: {text_path.Text}{e.Name}";
                    break;
                case WatcherChangeTypes.Deleted:
                    //MessageBox.Show($"Skasowano w folderze: {textBox_Path.Text}");
                    log = $"File Deleted: {text_path.Text}{e.Name}";
                    break;
                case WatcherChangeTypes.Changed:
                    //   MessageBox.Show($"Zmiana w folderze: {textBox_Path.Text}");
                    log = $"File Changed: {text_path.Text}{e.Name}";
                    break;
                case WatcherChangeTypes.Renamed:
                    //   MessageBox.Show($"Zmieniono nazwę w folderze: {textBox_Path.Text}");
                    log = $"File Renamed: {text_path.Text}{e.Name}";
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

            EmailSendAsync();

        }

        private async void EmailSendAsync()
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);

            Credentials name = new Credentials();

            client.Credentials = new NetworkCredential()
            {
                UserName = "balluffkurscsharp@gmail.com",
                Password = "0okmNJI("
            };

            client.EnableSsl = true;

           MailMessage message = new MailMessage();
           message.From = new MailAddress("balluffkurscsharp@gmail.comm", "Balluff Kurs Programowania");
            message.To.Add(new MailAddress("marcin.cukrowski@gmail.com", "Marcin"));
            message.Subject = "Kurs C# - informacja o zmianach w folderze";
            message.Body = $"Hej, nastąpiła zmiana w folderze {text_path.Text} w pliku.";
         
           await client.SendMailAsync(message);  //wyslanie maila asynchronicznie 

        }

        private void progressbar()
        {

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

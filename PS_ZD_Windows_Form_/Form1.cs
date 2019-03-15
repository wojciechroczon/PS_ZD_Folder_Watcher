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
            text_path.Text = Properties.Settings.Default.path; 
            MailStatus.BackColor = Color.Green;

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

     

        public void _watcher_Changed(object sender, FileSystemEventArgs e)
        {
            
            string log ="";

            switch (e.ChangeType)
            {
                case WatcherChangeTypes.Created:
                    //       MessageBox.Show($"Stworzono w folderze: {textBox_Path.Text}");
                    log = $"Utworzono plik: {e.FullPath}";
                    break;
                case WatcherChangeTypes.Deleted:
                    //MessageBox.Show($"Skasowano w folderze: {textBox_Path.Text}");
                    log = $"Skasowano plik: {e.FullPath}";
                    break;
                case WatcherChangeTypes.Changed:
                    //   MessageBox.Show($"Zmiana w folderze: {textBox_Path.Text}");
                    log = $"Zmieniono plik: {e.FullPath}";
                    break;
                case WatcherChangeTypes.Renamed:
                    //   MessageBox.Show($"Zmieniono nazwę w folderze: {textBox_Path.Text}");
                    log = $"Zmieniono nazwe pliku: {e.FullPath}";
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

            EmailSendAsync(e);  // wyslanie maila po evencie w folderze
          
        }

        private async void EmailSendAsync(FileSystemEventArgs e)
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
            message.To.Add(new MailAddress("marcin.cukrowski@gmail.com", "Marcin Cukrowski"));
            message.Subject = "Kurs C# - informacja o zmianach w folderze";
            message.Body = $"Uwaga,\r\n nastąpiła zmiana w folderze {text_path.Text} na pliku {e.Name} .\r\n ";


            MailStatus.BackColor = Color.Red;  // jakas logika. nie wiem na czym flage oprzec w IF i dodatkowej metodzie
        
            await client.SendMailAsync(message);  //wyslanie maila asynchronicznie 

            MailStatus.BackColor = Color.Green; // jakas logika. nie wiem na czym flage oprzec w IF i dodatkowej metodzie
       
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
                Properties.Settings.Default.path = text_path.Text;
            }
        }

        private void Form1_Leave(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();  //zapisanie zmian do zamykaniu okna
        }
    }
}

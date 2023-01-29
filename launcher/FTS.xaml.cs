using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
namespace launcher
{
    /// <summary>
    /// Interaction logic for FTS.xaml
    /// </summary>
    public partial class FTS : Window
    {
        WebClient client;
        public FTS()
        {
            InitializeComponent();
        }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            progress_label.Content = "Creating directories...";
            CreateDirs();
            progress_label.Content = "Creating files...";
            CreateFiles();
            progress_label.Content = "Downloading the modding engine";
            await DownloadEngine();
            CopyEngine();
            progress_label.Content = "Setting up the mod database";
            SerializeDB();
            progress_bar.Value = progress_bar.Value + 28;
            progress_label.Content = "Finishing up...";
            CheckCompletion();

        }
        private void CheckCompletion()
        {
            progress_bar.Value = 100;
            progress_label.Content = "We're done here!";
            MessageBox.Show("Hey there and thanks for downloading the launcher! We've been hard at work to make it work, so if you encounter any bugs feel free to reach out on: \nDiscord: BotchedRPR#1282\nGitHub: BotchedRPR\nWe hope that you enjoy using the launcher. Don't forget to copy your RIFT game files to the now created Launcher/RIFT folder.\nOnce again thanks for downloading, and we hope you'll enjoy using it!\n -BotchedRPR and Nikkuss");
            File.WriteAllText(@"Launcher\cfg\fts.cfg", "FN");
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Hide();
            Thread.Sleep(1000);
            this.Close();
        }
        public void SerializeDB()
        {
            File.AppendAllText(@"Launcher\mods\mod.db", "");
        }
        private async Task DownloadEngine()
        {
            client = new WebClient();
            client.DownloadProgressChanged += client_DownloadProgressChanged;
            await client.DownloadFileTaskAsync(new Uri("https://github.com/Rift-Mods/engine/releases/latest/download/engine.exe"), "engine.tmp");
        }
        private void CopyEngine()
        {
            progress_label.Content = "Copying the modding engine";
            if (File.Exists(@"Launcher\Engine\engine.exe"))
                File.Delete(@"Launcher\Engine\engine.exe");
            File.Copy("engine.tmp", @"Launcher\Engine\engine.exe");
            progress_bar.Value += 10;
        }

        private void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progress_bar.Value = 7 + e.ProgressPercentage / 2;
        }
        private void CreateFiles()
        {
            File.WriteAllText(@"Launcher\cfg\fts.cfg", "NF");
            progress_bar.Value++;
            File.AppendAllText(@"Launcher\Log.log", "FTS begin log" + Environment.NewLine);
            progress_bar.Value++;
        }

        private void CreateDirs()
        {
            Directory.CreateDirectory("Launcher");
            progress_bar.Value++;
            Directory.CreateDirectory(@"Launcher\mods");
            progress_bar.Value++;
            Directory.CreateDirectory(@"Launcher\RIFT");
            progress_bar.Value++;
            Directory.CreateDirectory(@"Launcher\cfg");
            progress_bar.Value++;
            Directory.CreateDirectory(@"Launcher\Engine");
            progress_bar.Value++;
            Directory.CreateDirectory(@"Launcher\mods\diffs");
            progress_bar.Value++;
        }
    }
}

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using static launcher.CommonClass;
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
        bool completed = false;
        bool dnC = false;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            progress_label.Content = "Creating directories...";
            CreateDirs();
            progress_label.Content = "Creating files...";
            CreateFiles();
            progress_label.Content = "Downloading the modding engine";
            File.AppendAllText(@"Launcher\Log.log", "ST1" + Environment.NewLine);
            DownloadEngine();
            progress_label.Content = "Setting up the mod database";
            File.AppendAllText(@"Launcher\Log.log", "ST2" + Environment.NewLine);
            SerializeDB();
            progress_label.Content = "Checking for dotnet";
            File.AppendAllText(@"Launcher\Log.log", "ST3" + Environment.NewLine);
            if (DotnetCheck())
            {
                progress_label.Content = "Finishing up...";
                hasDotNet = true;
            }
            else
            {
                progress_label.Content = "Downloading .NET...";
                process.WaitForExit();
            }
            CheckCompletion();

        }
        int i = 0;
        bool hasDotNet = false;
        private async void CheckCompletion()
        {
            var periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(1));
            while (await periodicTimer.WaitForNextTickAsync())
            {
                    if (hasDotNet)
                    {
                        progress_bar.Value = 85;
                        i++;
                    }
                    else
                    {
                        File.AppendAllText(@"Launcher\Log.log", "Nope, retry." + Environment.NewLine);
                        i++;
                        CheckCompletion();
                    }
                if (hasDotNet && i == 1)
                {
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    startInfo.FileName = "cmd.exe";
                    startInfo.Arguments = "/C dotnet tool install ilspycmd -g";
                    process.StartInfo = startInfo;
                    process.Start();
                    progress_bar.Value = 100;
                    progress_label.Content = "We're done here!";
                    File.AppendAllText(@"Launcher\Log.log", "FTS DONE!!!" + Environment.NewLine);
                    File.Delete("dotnet.exe");
                    MessageBox.Show("Hey there and thanks for downloading the launcher! We've been hard at work to make it work, so if you encounter any bugs feel free to reach out on: \nDiscord: BotchedRPR#1282\nGitHub: BotchedRPR\nWe hope that you enjoy using the launcher. Don't forget to copy your RIFT game files to the now created Launcher/RIFT folder.\nOnce again thanks for downloading, and we hope you'll enjoy using it!\n -BotchedRPR and Nikkuss");
                    MainWindow mw = new MainWindow();
                    mw.Show();
                    File.WriteAllText(@"Launcher\cfg\fts.cfg", "FN");
                    this.Hide();
                    Thread.Sleep(1000);
                    this.Close();
                }
                }          
        }
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        private bool DotnetCheck()
        {
            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.Process.Start("dotnet");
                return true;
            }
            catch { MessageBox.Show("Install .NET 7 SDK! The instructions are on the Discord."); return false; }
        }
        private void SerializeDB()
        {
            File.AppendAllText(@"Launcher\mods\mod.db", "");
            progress_bar.Value = progress_bar.Value + 28;
        }
        private void DownloadEngine()
        {
            client = new WebClient();
            client.DownloadFileCompleted += client_DownloadFileCompleted;
            client.DownloadProgressChanged += client_DownloadProgressChanged;
            client.DownloadFile(new Uri("https://files.nikkuss.com/downloadFile?id=rMl8Wdqe6gO5LqY"), "engine.tmp");
        }

        private void client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            i = 0;
            progress_label.Content = "Copying the modding engine";
            if(File.Exists(@"Launcher\Engine\engine.exe"))
                File.Delete(@"Launcher\Engine\engine.exe");
            File.Copy("engine.tmp", @"Launcher\Engine\engine.exe");
            progress_bar.Value = progress_bar.Value + 10;
            File.AppendAllText(@"Launcher\Log.log", "FTSDL complete" + Environment.NewLine);
            dnC = true;
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

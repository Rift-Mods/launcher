using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using Newtonsoft.Json;
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
            }
            else
            {
                progress_label.Content = "Downloading .NET...";
                DownloadDotnet();
            }
            CheckCompletion();

        }
        int i = 0;
        private async void CheckCompletion()
        {
            var periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(1));
            while (await periodicTimer.WaitForNextTickAsync())
            {
                    if (dnC)
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
                if (dnC && i == 1)
                {
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    startInfo.FileName = "cmd.exe";
                    startInfo.Arguments = "/k dotnet tool install ilspycmd -g";
                    process.StartInfo = startInfo;
                    process.Start();
                    progress_bar.Value = 100;
                    progress_label.Content = "We're done here!";
                    File.WriteAllText(@"Launcher\cfg\fts.cfg", "FN");
                    File.AppendAllText(@"Launcher\Log.log", "FTS DONE!!!" + Environment.NewLine);
                    MainWindow mw = new MainWindow();
                    mw.Show();
                    this.Hide();
                    Thread.Sleep(1000);
                    this.Close();
                }
                }          
        }


        private bool DownloadDotnet()
        {
            client = new WebClient();
            client.DownloadFile("https://download.visualstudio.microsoft.com/download/pr/6ba69569-ee5e-460e-afd8-79ae3cd4617b/16a385a4fab2c5806f50f49f5581b4fd/dotnet-sdk-7.0.102-win-x64.exe", "dotnet.exe");
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "dotnet.exe";
            startInfo.Arguments = "/install /quiet /norestart";
            process.StartInfo = startInfo;
            process.Start();

            progress_label.Content = "DON'T PANIC IF IT LOOKS STUCK. Installing .NET";
            File.AppendAllText(@"Launcher\Log.log", ".NET install going" + Environment.NewLine);
            System.Threading.Thread.Sleep(5000);
            return true;
        }
        private bool DotnetCheck()
        {
            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.Process.Start("dotnet");
                return true;
            }
            catch { File.AppendAllText(@"Launcher\Log.log", "!!! POSSIBLE TROUBLE CAUSE! !!!" + Environment.NewLine + "Starting .NET download." + Environment.NewLine); return false; }
        }
        private void SerializeDB()
        {
            ModDB moddb = new ModDB();
            moddb.InstalledMods = new string[] { "JSON test" };
            string output = JsonConvert.SerializeObject(moddb);
            output = CreateMD5(output) + output;
            File.AppendAllText(@"Launcher\mods\mod.db", output);
            progress_bar.Value = progress_bar.Value + 28;
        }
        private void DownloadEngine()
        {
            client = new WebClient();
            client.DownloadFileCompleted += client_DownloadFileCompleted;
            client.DownloadProgressChanged += client_DownloadProgressChanged;
            client.DownloadFileAsync(new Uri("http://speedtest.ftp.otenet.gr/files/test10Mb.db"), "engine.tmp");
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
        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes); // .NET 5 +
            }
        }
    }

    internal class ModDB
    {
        public ModDB()
        {

        }

        public string[] InstalledMods { get; internal set; }
    }
}

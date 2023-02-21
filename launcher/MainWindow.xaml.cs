using launcher.MVVM.View;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static launcher.Updater;

namespace launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string rift_binarypath = Properties.Settings.Default.RiftPath;
        public MainWindow()
        {
            InitializeComponent();
            StartupLogic();
            this.Visibility = Visibility.Hidden;
            if (File.Exists(@"Launcher\cfg\fts.cfg") && File.ReadAllText(@"Launcher\cfg\fts.cfg") == "FN" && !Directory.Exists("Launcher\\Engine"))
            {
                this.Visibility = Visibility.Visible;
            }
            else
            {
                FTS fts = new FTS();
                fts.Show();
            }
            if (File.Exists("launcher.old"))
                File.Delete("launcher.old");
            if (File.Exists("launcher.new"))
                File.Delete("launcher.new");
            FinishStart();
        }

        private void StartupLogic()
        {
            /* Check for shortcut/other program start command (to skip pressing the button in UI) */
            string[] args = Environment.GetCommandLineArgs(); // Get command line args
            if (args.Length != 0) //Something is passed, check it before showing UI
            {
                if (args.Contains("-play") && !args.Contains("-mods"))
                {
                    StartGame();
                }
                else if (args.Contains("-play") && args.Contains("-mods") && args.Contains("on"))
                {
                    DirectoryInfo d = new DirectoryInfo(Properties.Settings.Default.RiftPath + @"\RiftModding\Mods\");

                    FileInfo[] Files = d.GetFiles("*.temp.disabled");

                    foreach (FileInfo file in Files)
                    {
                        file.MoveTo(file.FullName.Replace(".temp.disabled", ""));
                    }
                    PlayView.ModsDisabled = false;
                    StartGame();
                }
                else if (args.Contains("-play") && args.Contains("-mods") && args.Contains("off"))
                {
                    DirectoryInfo d = new DirectoryInfo(Properties.Settings.Default.RiftPath + @"\RiftModding\Mods\");

                    FileInfo[] Files = d.GetFiles("*.*");

                    foreach (FileInfo file in Files)
                    {
                        file.MoveTo(file.FullName + ".temp.disabled");
                    }
                    PlayView.ModsDisabled = true;
                    StartGame();
                }
                else
                {
                    MessageBox.Show("Invalid usage.\nStart RIFT, using the last mod settings:\nlauncher.exe -play\n\nStart RIFT, with mods:\nlauncher.exe -play -mods on\n\nStart RIFT, without mods\nlauncher.exe -play -mods off\n\nStart the launcher\nlauncher.exe");
                }
            }
        }

        private void StartGame()
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = Properties.Settings.Default.RiftPath + "\\RIFT.exe";
            psi.WorkingDirectory = Properties.Settings.Default.RiftPath;
            Process.Start(psi);
        }
        private async void FinishStart()
        {
            if (UpdaterCheckDone)
            {

            }
            else
            {
                CheckForUpdate();
                while (!UpdaterCheckDone)
                {
                    await Task.Delay(25);
                }
                if (UpdateAvailable == true)
                    this.Visibility = Visibility.Hidden;
            }
            if (Updater.Version != "")
                verLabel.Content += Updater.Version;
            else
                MessageBox.Show("Failed to get current version. Something is really messed up.");
        }


        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }
    }
}

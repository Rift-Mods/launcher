using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using static launcher.EngineHandler;
using static launcher.Updater;

namespace launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool[] buttonsEnabled = new bool[4];
        public static string rift_binarypath = @"Launcher\RIFT\RIFT.exe"; //load this from app.settings later
        public MainWindow()
        {
            this.Visibility = Visibility.Hidden;
            if (File.Exists(@"Launcher\cfg\fts.cfg") && File.ReadAllText(@"Launcher\cfg\fts.cfg") == "FN")
            {
                buttonsEnabled[0] = true; //enable play button as default
                this.Visibility = Visibility.Visible;
            }
            else
            {
                FTS fts = new FTS();
                fts.Show();
            }
            if (File.Exists("launcher.old"))
                File.Delete("launcher.old");
            if(File.Exists("launcher.new"))
                File.Delete("launcher.new");
            FinishStart();
        }
        private async void FinishStart()
        {
            CheckForUpdate();
            while (!UpdaterCheckDone)
            {
                await Task.Delay(25);
            }
            if (UpdateAvailable == true)
                this.Visibility = Visibility.Hidden;
            try
            {
                InitializeComponent();
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = "/c dotnet tool install ilspycmd -g";
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
                var lines = File.ReadLines(@"Launcher\mods\mod.db");
                foreach (String line in lines)
                {
                    //ModBox.Items.Add(line.Split('|')[0]);
                }
                verLabel.Content += Updater.Version;
            }
            catch { }
        }
        #region Buttons
        //private void lbl_play_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    SetActive(0, lbl_play);
        //    if(ModsActive)
        //        FadeOutMods();
        //    PlayActive = true;
        //    Storyboard anim = (Storyboard)RiftArt.FindResource("unblurSB");
        //    anim.Begin();
        //    animDone = false;
        //    if (clickCount == 0)
        //    {
        //        clickCount++;
        //        return;
        //    }
        //    try
        //    {
        //        if (rift_binarypath != string.Empty)
        //        {
        //            System.Diagnostics.Process.Start(rift_binarypath);
        //       }
        //        else
        //        {
        //            MessageBox.Show("Startup FAIL!\nCause: RIFT binary path not found.", "RIFT launcher error", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        MessageBox.Show("Startup FAIL!\nCause: " + ex + ", RIFT binary path not found or startup failed for some other reason.", "RIFT launcher error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //    clickCount = 0;
        //}
        //bool PlayActive = true;
        //bool ModsActive = false;
        //bool BoardActive = false;
        //bool SettingsActive = false;
        //private void lbl_mods_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    SetActive(1, lbl_mods);
        //    PlayActive = false;
        //    ModsActive = true;
        //    BoardActive = false;
        //    SettingsActive = false;
        //    DoubleAnimation da = new DoubleAnimation
        //    {
        //        From = 0,
        //        To = 1,
        //        Duration = new Duration(TimeSpan.FromMilliseconds(300)),
        //        AutoReverse = false
        //    };
        //    AddModBtn.BeginAnimation(OpacityProperty, da);
        //    DelModBtn.BeginAnimation(OpacityProperty, da);
        //    ModTitle.BeginAnimation(OpacityProperty, da);
        //    RestoreGameBtn.BeginAnimation(OpacityProperty, da);
        //    ModBox.BeginAnimation(OpacityProperty, da);
        //    DoAnim();
        //    //mods
        //}


        #endregion

        //private void AddModBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    string ZipDir = "";
        //    OpenFileDialog openFileDialog = new OpenFileDialog();
        //    if (openFileDialog.ShowDialog() == true)
        //        ZipDir = openFileDialog.FileName;
        //    else { return; }
        //    if (addMod(ZipDir, ModProgress) != true)
        //    {
        //        MessageBox.Show("A launcher error has occured.\n\nHave you copied RIFT to the Launcher/RIFT directory?", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //    ModBox.Items.Clear();
        //    var lines = File.ReadLines(@"Launcher\mods\mod.db");
        //    foreach (String line in lines)
        //    {
        //        ModBox.Items.Add(line.Split('|')[0]);
        //    }
        //}

        //private void RestoreGameBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    RestoreOG(ModProgress);
        //    File.Delete(@"Launcher\mods\mod.db");
        //    File.AppendAllText(@"Launcher\mods\mod.db", "");
        //}

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void lbl_settings_Copy_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}

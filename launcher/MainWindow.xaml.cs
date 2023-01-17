using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using static launcher.EngineHandler;


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
            //CheckForUpdate();
            try
            {
                this.Visibility = Visibility.Visible;
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
                    ModBox.Items.Add(line.Split('|')[0]);
                }
            }
            catch { }
        }
        #region Buttons
        private void ClearButtons()
        {
            if (buttonsEnabled[0] != true)
            {
                BoardStart("lbl_play_mouseLeave", lbl_play);
                lbl_play.Background = new SolidColorBrush(Colors.Black);
            }
            if (buttonsEnabled[1] != true)
            {
                BoardStart("lbl_mods_mouseLeave", lbl_mods);
                lbl_mods.Background = new SolidColorBrush(Colors.Black);
            }
            if (buttonsEnabled[2] != true)
            {
                BoardStart("lbl_leaderboard_mouseLeave", lbl_leaderboard);
                lbl_leaderboard.Background = new SolidColorBrush(Colors.Black);
            }
            if (buttonsEnabled[3] != true)
            {
                BoardStart("lbl_settings_mouseLeave", lbl_settings);
                lbl_settings.Background = new SolidColorBrush(Colors.Black);
            }
        }
        bool animDone = false;
        private void DoAnim()
        {
            if (animDone != true)
            {
                Storyboard anim = (Storyboard)RiftArt.FindResource("blurSB");
                anim.Begin();
                animDone = true;
            }
        }
        private void SetActive(int count, Label lbl)
        {
            buttonsEnabled = new bool[4];
            buttonsEnabled[count] = true;
            ClearButtons();
            lbl.Background = new SolidColorBrush(Colors.DarkGray);
        }
        int clickCount = 0;
        private void lbl_play_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SetActive(0, lbl_play);
            if(ModsActive)
                FadeOutMods();
            PlayActive = true;
            Storyboard anim = (Storyboard)RiftArt.FindResource("unblurSB");
            anim.Begin();
            animDone = false;
            if (clickCount == 0)
            {
                clickCount++;
                return;
            }
            try
            {
                if (rift_binarypath != string.Empty)
                {
                    System.Diagnostics.Process.Start(rift_binarypath);
               }
                else
                {
                    MessageBox.Show("Startup FAIL!\nCause: RIFT binary path not found.", "RIFT launcher error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Startup FAIL!\nCause: " + ex + ", RIFT binary path not found or startup failed for some other reason.", "RIFT launcher error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            clickCount = 0;
        }
        bool PlayActive = true;
        bool ModsActive = false;
        bool BoardActive = false;
        bool SettingsActive = false;
        private void lbl_mods_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SetActive(1, lbl_mods);
            PlayActive = false;
            ModsActive = true;
            BoardActive = false;
            SettingsActive = false;
            DoubleAnimation da = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromMilliseconds(300)),
                AutoReverse = false
            };
            AddModBtn.BeginAnimation(OpacityProperty, da);
            DelModBtn.BeginAnimation(OpacityProperty, da);
            ModTitle.BeginAnimation(OpacityProperty, da);
            RestoreGameBtn.BeginAnimation(OpacityProperty, da);
            ModBox.BeginAnimation(OpacityProperty, da);
            DoAnim();
            //mods
        }

        private void lbl_leaderboard_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SetActive(2, lbl_leaderboard);
            DoAnim();
            //leaderboard
        }

        private void lbl_settings_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SetActive(3, lbl_settings);
            DoAnim();
            //settings
        }
        private void BoardStart(string Board, Label lbl)
        {
            Storyboard anim = (Storyboard)lbl.FindResource(Board);
            anim.Begin();
        }
        private void FadeOutMods()
        {
            DoubleAnimation da = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = new Duration(TimeSpan.FromMilliseconds(300)),
                AutoReverse = false
            };
            AddModBtn.BeginAnimation(OpacityProperty, da);
            DelModBtn.BeginAnimation(OpacityProperty, da);
            ModTitle.BeginAnimation(OpacityProperty, da);
            RestoreGameBtn.BeginAnimation(OpacityProperty, da);
            ModBox.BeginAnimation(OpacityProperty, da);
            ModsActive = false;
        }
        // i fucking hate this.
        // this is the only way that you can do this in WPF.
        private void lbl_play_MouseEnter(object sender, MouseEventArgs e)
        {
            if (buttonsEnabled[0] != true)
            {
                BoardStart("lbl_play_mouseEnter", lbl_play);
            }
        }

        private void lbl_play_MouseLeave(object sender, MouseEventArgs e)
        {
            if (buttonsEnabled[0] != true)
            {
                BoardStart("lbl_play_mouseLeave", lbl_play);
            }
        }

        private void lbl_mods_MouseEnter(object sender, MouseEventArgs e)
        {
            if (buttonsEnabled[1] != true)
            {
                BoardStart("lbl_mods_mouseEnter", lbl_mods);
            }
        }

        private void lbl_mods_MouseLeave(object sender, MouseEventArgs e)
        {
            if (buttonsEnabled[1] != true)
            {
                BoardStart("lbl_mods_mouseLeave", lbl_mods);
            }
        }

        private void lbl_leaderboard_MouseEnter(object sender, MouseEventArgs e)
        {
            if (buttonsEnabled[2] != true)
            {
                BoardStart("lbl_leaderboard_mouseEnter", lbl_leaderboard);
            }
        }

        private void lbl_leaderboard_MouseLeave(object sender, MouseEventArgs e)
        {
            if (buttonsEnabled[2] != true)
            {
                BoardStart("lbl_leaderboard_mouseLeave", lbl_leaderboard);
            }
        }
        private void lbl_settings_MouseEnter(object sender, MouseEventArgs e)
        {
            if (buttonsEnabled[3] != true)
            {
                BoardStart("lbl_settings_mouseEnter", lbl_settings);
            }
        }

        private void lbl_settings_MouseLeave(object sender, MouseEventArgs e)
        {
            if (buttonsEnabled[3] != true)
            {
                BoardStart("lbl_settings_mouseLeave", lbl_settings);
            }
        }


        #endregion
        #region Scenes
        private void SetScene(string Scene)
        {
            switch(Scene)
            {
                case "mods":
                    Storyboard anim = (Storyboard)ModBox.FindResource("TransformListBox");
                    anim.Begin();
                    break;
                case "leaderboard":
                    //lb
                    break;
                case "settings":
                //settings
                case "launcher":
                    break;
            }
        }
        #endregion

        private void AddModBtn_Click(object sender, RoutedEventArgs e)
        {
            string ZipDir = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                ZipDir = openFileDialog.FileName;
            else { return; }
            if (addMod(ZipDir, ModProgress) != true)
            {
                MessageBox.Show("A launcher error has occured.\n\nHave you copied RIFT to the Launcher/RIFT directory?", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            var lines = File.ReadLines(@"Launcher\mods\mod.db");
            foreach (String line in lines)
            {
                ModBox.Items.Add(line.Split('|')[0]);
            }
        }

        private void RestoreGameBtn_Click(object sender, RoutedEventArgs e)
        {
            RestoreOG(ModProgress);
            //mod.db changes
        }
    }
}

using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;

namespace launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool[] buttonsEnabled = new bool[4];
        string rift_binarypath = string.Empty; //load this from app.settings later
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            buttonsEnabled[0] = true; //enable play button as default
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
        private void SetActive(int count, Label lbl)
        {
            buttonsEnabled = new bool[4];
            buttonsEnabled[count] = true;
            ClearButtons();
            lbl.Background = new SolidColorBrush(Colors.DarkGray);
        }

        private void lbl_play_MouseDown(object sender, MouseButtonEventArgs e)
        {

            SetActive(0, lbl_play);
            /*if (rift_binarypath != string.Empty)
            {
                Process.Start(rift_binarypath);
            }
            else 
            {
                MessageBox.Show("Startup FAIL!\nCause: RIFT binary path not found.", "RIFT launcher error", MessageBoxButton.OK, MessageBoxImage.Error);
            }*/
        }
        private void lbl_mods_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SetActive(1, lbl_mods);
            //mods
        }

        private void lbl_leaderboard_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SetActive(2, lbl_leaderboard);
            //leaderboard
        }

        private void lbl_settings_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SetActive(3, lbl_settings);
            //settings
        }
        private void BoardStart(string Board, Label lbl)
        {
            Storyboard anim = (Storyboard)lbl.FindResource(Board);
            anim.Begin();
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


    }
}

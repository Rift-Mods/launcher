using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;

namespace launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string rift_binarypath = string.Empty; //load this from app.settings later
        public MainWindow()
        {
            InitializeComponent();
        }

        private void lbl_play_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (rift_binarypath != string.Empty)
            {
                Process.Start(rift_binarypath);
            }
            else 
            {
                MessageBox.Show("Startup FAIL!\nCause: RIFT binary path not found.", "RIFT launcher error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void lbl_play_MouseEnter(object sender, MouseEventArgs e)
        {
            
        }
    }
}

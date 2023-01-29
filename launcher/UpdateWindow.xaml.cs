using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace launcher
{
    /// <summary>
    /// Interaction logic for UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow : Window
    {
        public UpdateWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }
        private void HandleProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            UpdateBar.Value = e.ProgressPercentage;
        }

        private async void Confirm_Button_Click(object sender, RoutedEventArgs e)
        {
            Question0.Visibility = Visibility.Hidden;
            Question1.Visibility = Visibility.Hidden;
            WebClient webClient = new WebClient();
            string sourceFile = @"https://github.com/Rift-Mods/launcher/releases/latest/download/launcher.exe";
            string destFile = @"launcher.new";
            webClient.DownloadProgressChanged += HandleProgress;
            await webClient.DownloadFileTaskAsync(new Uri(sourceFile), destFile);
            System.IO.File.Move("launcher.exe", "launcher.old");
            System.IO.File.Move("launcher.new", "launcher.exe");
            MessageBox.Show("Update completed. Please restart the launcher.");
            Application.Current.Shutdown();

        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

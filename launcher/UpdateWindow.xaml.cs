using System;
using System.Net;
using System.Windows;

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
        private void HandleProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            UpdateBar.Value = e.ProgressPercentage;
        }
    }
}

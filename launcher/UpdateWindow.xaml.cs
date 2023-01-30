using System;
using Octokit;
using System.Net;
using System.Windows;
using System.Collections.Generic;

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
            GitHubClient client = new GitHubClient(new ProductHeaderValue("GHClient-RLauncher"));
            IReadOnlyList<Release> releases = await client.Repository.Release.GetAll("Rift-Mods", "launcher");
            var latest = releases[0];
            changelogBlock.Text = latest.Body;
        }
        private void HandleProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            UpdateBar.Value = e.ProgressPercentage;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            WebClient webClient = new WebClient();
            string sourceFile = @"https://github.com/Rift-Mods/launcher/releases/latest/download/launcher.exe";
            string destFile = @"launcher.new";
            webClient.DownloadProgressChanged += HandleProgress;
            await webClient.DownloadFileTaskAsync(new Uri(sourceFile), destFile);
            System.IO.File.Move("launcher.exe", "launcher.old");
            System.IO.File.Move("launcher.new", "launcher.exe");
            MessageBox.Show("Update completed. Please restart the launcher.");
            System.Windows.Application.Current.Shutdown();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }
    }
}

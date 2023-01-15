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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WebClient webClient = new WebClient();
            string sourceFile = @"https://github.com/Rift-Mods/launcher/releases/latest/download/launcher.exe";
            string destFile = @"launcher.new";
            webClient.DownloadFile(new Uri(sourceFile), destFile);
            System.IO.File.Move("launcher.exe", "launcher.old");
            System.IO.File.Move("launcher.new", "launcher.exe");
            MessageBox.Show("Update completed. Please restart the launcher.");
            Application.Current.Shutdown();
        }
    }
}

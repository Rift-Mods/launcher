using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Windows;
using System.Windows.Controls;

namespace launcher.MVVM.View
{
    /// <summary>
    /// Interaction logic for ModView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        private void RiftPath_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = Properties.Settings.Default.RiftPath;
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                Properties.Settings.Default.RiftPath = dialog.FileName;
            }
            MainWindow.rift_binarypath = Properties.Settings.Default.RiftPath;
            if (System.IO.File.Exists(Properties.Settings.Default.RiftPath + @"RIFT.exe"))
                Properties.Settings.Default.Save();
            else
                MessageBox.Show("That's not the right folder. Try again.");
        }

        private void RiftPath_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            RiftPath.Text = MainWindow.rift_binarypath;
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            FTS fts = new FTS();
            fts.CreateShortcuts();
        }
    }
}

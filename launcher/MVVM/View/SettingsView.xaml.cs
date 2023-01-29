using Microsoft.Win32;
using System;
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
        UInt16 click = 0;
        private void TextBox_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            switch (click)
            {
                case 0:
                    click++;
                    RiftPath.Text = Properties.Settings.Default.RiftPath;
                    break;
                case 1:
                    click = 0;
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    if (openFileDialog.ShowDialog() == true)
                        Properties.Settings.Default.RiftPath = openFileDialog.FileName;
                    Properties.Settings.Default.Save();
                    break;
            }
        }
    }
}

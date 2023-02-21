using System.Diagnostics;
using System.IO;
using System.Windows.Controls;

namespace launcher.MVVM.View
{
    /// <summary>
    /// Interaction logic for PlayView.xaml
    /// </summary>
    public partial class PlayView : UserControl
    {
        public static bool ModsDisabled = false;
        public PlayView()
        {
            InitializeComponent();
            if (ModsDisabled)
            {
                ModCheck.IsChecked = true;
            }
            else
            {
                DirectoryInfo d = new DirectoryInfo(Properties.Settings.Default.RiftPath + @"\RiftModding\Mods\");

                FileInfo[] Files = d.GetFiles("*.temp.disabled");

                foreach (FileInfo file in Files)
                {
                    file.MoveTo(file.FullName.Replace(".temp.disabled", ""));
                }
            }
        }

        private void playBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = Properties.Settings.Default.RiftPath + "\\RIFT.exe";
            psi.WorkingDirectory = Properties.Settings.Default.RiftPath;
            Process.Start(psi);
        }

        private void ModCheck_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            DirectoryInfo d = new DirectoryInfo(Properties.Settings.Default.RiftPath + @"\RiftModding\Mods\");

            FileInfo[] Files = d.GetFiles("*.*");

            foreach (FileInfo file in Files)
            {
                file.MoveTo(file.FullName + ".temp.disabled");
            }
            ModsDisabled= true;

        }

        private void ModCheck_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            DirectoryInfo d = new DirectoryInfo(Properties.Settings.Default.RiftPath + @"\RiftModding\Mods\");

            FileInfo[] Files = d.GetFiles("*.temp.disabled");

            foreach (FileInfo file in Files)
            {
                file.MoveTo(file.FullName.Replace(".temp.disabled", ""));
            }
            ModsDisabled= false;
        }
    }
}

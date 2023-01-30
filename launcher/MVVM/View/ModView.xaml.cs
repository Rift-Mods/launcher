using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace launcher.MVVM.View
{
    /// <summary>
    /// Interaction logic for ModView.xaml
    /// </summary>
    public partial class ModView : UserControl
    {
        public ModView()
        {
            InitializeComponent();
            PopulateLB();
        }

        private void PopulateLB()
        {
            listBox.Items.Clear();
            DirectoryInfo d = new DirectoryInfo(Properties.Settings.Default.RiftPath + @"\RiftModding\Mods\");

            FileInfo[] Files = d.GetFiles("*.dll"); //Getting Text files

            foreach (FileInfo file in Files)
            {
                listBox.Items.Add(file.Name.Split('.').First());
            }

            Files = d.GetFiles("*.disabled"); //Getting Text files

            foreach (FileInfo file in Files)
            {
                listBox.Items.Add("[DISABLED] " + file.Name.Split('.').First());
            }
        }
        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {

            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.DefaultExt = ".dll"; // Default file extension
            dialog.Filter = "RIFT mods for engine2|*.dll"; // Filter files by extension

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                if (Directory.Exists(Properties.Settings.Default.RiftPath + @"\RiftModding\Mods"))
                    File.Copy(dialog.FileName, Properties.Settings.Default.RiftPath + @"\RiftModding\Mods\" + dialog.FileName.Split('\\').Last());
                else
                    MessageBox.Show("Can't do that. The engine is not installed."); 
                
                PopulateLB();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(listBox.SelectedItem != null)
            {
                string ModFile = Properties.Settings.Default.RiftPath + @"\RiftModding\Mods\" + listBox.SelectedItem.ToString();
                if (ModFile.Contains("[DISABLED]"))
                    ModFile = ModFile.Replace("[DISABLED] ", "") + ".dll.disabled";
                else
                    ModFile = ModFile + ".dll";
                if (File.Exists(ModFile) && !ModFile.EndsWith(".disabled"))
                {
                    File.Move(ModFile, ModFile + ".disabled");
                }
                else if (File.Exists(ModFile))
                    File.Move(ModFile, ModFile.Replace(".disabled", ""));
                PopulateLB();
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItem != null)
            {
                string ModFile = Properties.Settings.Default.RiftPath + @"\RiftModding\Mods\" + listBox.SelectedItem.ToString();
                if (ModFile.Contains("[DISABLED]"))
                    ModFile = ModFile.Replace("[DISABLED] ", "") + ".dll.disabled";
                else
                    ModFile = ModFile + ".dll";
                if (File.Exists(ModFile))
            {
                File.Delete(ModFile);
                PopulateLB();
            }
        }
    }
    }
}

using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static launcher.Updater;

namespace launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool[] buttonsEnabled = new bool[4];
        public static string rift_binarypath = @"Launcher\RIFT\RIFT.exe"; //load this from app.settings later
        public MainWindow()
        {
            InitializeComponent();
            this.Visibility = Visibility.Hidden;
            if (File.Exists(@"Launcher\cfg\fts.cfg") && File.ReadAllText(@"Launcher\cfg\fts.cfg") == "FN")
            {
                buttonsEnabled[0] = true; //enable play button as default
                this.Visibility = Visibility.Visible;
            }
            else
            {
                FTS fts = new FTS();
                fts.Show();
            }
            if (File.Exists("launcher.old"))
                File.Delete("launcher.old");
            if (File.Exists("launcher.new"))
                File.Delete("launcher.new");
            FinishStart();
        }
        private async void FinishStart()
        {
            CheckForUpdate();
            while (!UpdaterCheckDone)
            {
                await Task.Delay(25);
            }
            if (UpdateAvailable == true)
                this.Visibility = Visibility.Hidden;
            if (Updater.Version != "")
                verLabel.Content += Updater.Version;
            else
                MessageBox.Show("Failed to get current version. Something is really messed up.");
        }

        private void lbl_settings_Copy_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }
    }
}

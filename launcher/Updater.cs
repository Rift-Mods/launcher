using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace launcher
{
    internal class Updater
    {
        public string NewUpdate = "";
        public string NewUpdateURL = "";
        public string NewUpdateChangelog = "";

        public static async void CheckForUpdate()
        {
            GitHubClient client = new GitHubClient(new ProductHeaderValue("GHClient-RLauncher"));
            IReadOnlyList<Release> releases = await client.Repository.Release.GetAll("Rift-Mods", "launcher");

            //Setup the versions
            Version latestGitHubVersion = new Version(releases[0].TagName);
            Version localVersion = new Version("1.0.0"); //Replace this with your local version. 
                                                         //Only tested with numeric values.

            int versionComparison = localVersion.CompareTo(latestGitHubVersion);
            if (versionComparison < 0)
            {
                UpdateWindow uw = new UpdateWindow();
                uw.Show();
            }
            else if (versionComparison > 0)
            {
                MessageBox.Show("You are running a BETA release.\n Hic sunt dracones.");
            }
        }
    }
}

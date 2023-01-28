using Octokit;
using System;
using System.Collections.Generic;
using System.Windows;

namespace launcher
{
    internal class Updater
    {
        public static bool UpdateAvailable = false;
        public static bool UpdaterCheckDone = false;
        public static string Version = "2.0.0";
        public static async void CheckForUpdate()
        {
            try
            {
                GitHubClient client = new GitHubClient(new ProductHeaderValue("GHClient-RLauncher"));
                IReadOnlyList<Release> releases = await client.Repository.Release.GetAll("Rift-Mods", "launcher");

                //Setup the versions
                Version latestGitHubVersion = new Version(releases[0].TagName);
                Version localVersion = new Version(Version); //Replace this with your local version. 
                                                             //Only tested with numeric values.

                int versionComparison = localVersion.CompareTo(latestGitHubVersion);
                if (versionComparison < 0)
                {
                    UpdateAvailable = true;
                    UpdateWindow uw = new UpdateWindow();
                    uw.Show();
                }
                else if (versionComparison > 0)
                {
                    MessageBox.Show("You are running a BETA release.\n Hic sunt dracones.");
                }
                UpdaterCheckDone = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Update check failed. Exception" + ex);
            }
        }
    }
}

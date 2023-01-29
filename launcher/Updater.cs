﻿using Octokit;
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
        public static bool UpdateAvailable = false;
        public static bool UpdatePermission = false;
        public static bool UpdaterCheckDone = false;
        public static async void CheckForUpdate()
        {
            try
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
                    UpdateAvailable = false;
                    UpdateWindow uw = new UpdateWindow();
                    uw.Show();
                }
                else if (versionComparison > 0)
                {
                    MessageBox.Show("You are running a BETA release.\nHic sunt dracones.");
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

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace launcher
{
    public class EngineHandler
    {
        public static bool addMod(string zip, ProgressBar bar)
        {
            if (System.IO.File.Exists(zip) && System.IO.File.Exists(@"Launcher\RIFT\RIFT.exe"))
            {
                bar.Value = 0;
                ProcessStartInfo si = new ProcessStartInfo();
                si.FileName = @"Launcher\Engine\engine.exe";
                si.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
                si.Arguments = "add \"" + MainWindow.rift_binarypath.Replace("RIFT.exe", "") + "\" \"" + zip + "\"";
                si.Arguments = si.Arguments.Replace(@"\", "/"); //nik come on
                si.RedirectStandardOutput = true;
                si.RedirectStandardError = true;
                si.UseShellExecute = false;
                si.CreateNoWindow = true;
                bar.Value++;
                Process proc = new Process();
                proc.StartInfo = si;
                bar.Value++;
                proc.Start();
                string line = proc.StandardError.ReadToEnd();
                line = line + proc.StandardOutput.ReadToEnd();
                if (line.Contains("CRITICAL") || line.Contains("Traceback"))
                {
                    MessageBox.Show("An engine error has occured. The mod may be corrupted.\nInsert funny error text here\n\nEngineError follows:\n" + line); return false;
                }
                bar.Value = 50;
                var definition = new { name = "" };
                string modName_global = "";
                using (ZipArchive zipa = ZipFile.Open(zip, ZipArchiveMode.Read))
                    foreach (ZipArchiveEntry entry in zipa.Entries)
                        if (entry.Name == "manifest.json")
                        {
                            entry.ExtractToFile("temp.json", true);
                            string json = File.ReadAllText("temp.json");
                            var modName = JsonConvert.DeserializeAnonymousType(json, definition);
                            modName_global = modName.name;
                        }
                Directory.CreateDirectory(@"Launcher\mods\diffs\" + modName_global.Replace(" ", ""));
                using (ZipArchive zipa = ZipFile.Open(zip, ZipArchiveMode.Read))
                    foreach (ZipArchiveEntry entry in zipa.Entries)
                        if (entry.Name != "manifest.json")
                        {
                            entry.ExtractToFile(@"Launcher\mods\diffs\" + modName_global.Replace(" ", "") + @"\" + entry.Name);
                        }
                if (!File.Exists("temp.json"))
                {
                    MessageBox.Show("Mod is corrupted. Manifest.json is missing."); return false;
                }
                File.AppendAllText(@"Launcher\mods\mod.db", modName_global + "|" + @"Launcher\mods\diffs\" + modName_global.Replace(" ", "") + Environment.NewLine);
                MessageBox.Show("Mod added succesfully!\nHave fun!");
                return true;
            }
            return false;
        }
        public static bool RestoreOG(ProgressBar bar)
        {
            if (System.IO.File.Exists(@"Launcher\RIFT\RIFT.exe"))
            {
                bar.Value = 0;
                ProcessStartInfo si = new ProcessStartInfo();
                si.FileName = @"Launcher\Engine\engine.exe";
                si.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
                si.Arguments = "restore " + MainWindow.rift_binarypath.Replace("RIFT.exe", "");
                si.RedirectStandardOutput = true;
                si.RedirectStandardError = true;
                si.UseShellExecute = false;
                si.CreateNoWindow = true;
                bar.Value++;
                Process proc = new Process();
                proc.StartInfo = si;
                bar.Value++;
                proc.Start();
                string line = proc.StandardError.ReadToEnd();
                line = line + proc.StandardOutput.ReadToEnd();
                if (line.Contains("CRITICAL") || line.Contains("Traceback"))
                {
                    MessageBox.Show("An engine error has occured. The mod may be corrupted.\nInsert funny error text here\n\nEngineError follows:\n" + line); return false;
                }
                bar.Value = 100;
                //remove mod.db
                MessageBox.Show("All mods removed.\nHave (no more) fun!");
                return true;
            }
            return false;
        }
    }
}

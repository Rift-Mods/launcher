using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace launcher
{
    internal class EngineHandler
    {
        private ProcessStartInfo si = new ProcessStartInfo();
        private bool decompile()
        {
            si.FileName = @"Launcher\Engine\engine.exe";
            si.Arguments = "get_source";
            si.RedirectStandardOutput = true;
            si.RedirectStandardError = true;
            si.UseShellExecute = false;
            si.CreateNoWindow = true;
            return true;
        }
    }
}

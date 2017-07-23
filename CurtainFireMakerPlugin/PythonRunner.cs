using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using IronPython.Hosting;
using IronPython.Runtime;
using Microsoft.Scripting.Hosting;
using System.Windows.Forms;

namespace CurtainFireMakerPlugin
{
    internal class PythonRunner
    {
        private static ScriptEngine engine = Python.CreateEngine();
        private static ScriptScope rootScope = engine.CreateScope();

        public static void Init(string settingScriptPath, string modullesDirPath)
        {
            ICollection<string> paths = engine.GetSearchPaths();
            paths.Add(modullesDirPath);
            engine.SetSearchPaths(paths);

            engine.Execute(
            "# -*- coding: utf-8 -*-\n" +
            "import sys\n" +
            "sys.path.append(r\"" + Application.StartupPath + "\\Plugins\")\n" +
            "import clr\n" +
            "clr.AddReference(\"CurtainFireMakerPlugin\")\n" +
            "clr.AddReference(\"CsPmx\")\n" +
            "clr.AddReference(\"CsVmd\")\n" +
            "clr.AddReference(\"MikuMikuPlugin\")\n", rootScope);

            ScriptScope scope = engine.CreateScope(rootScope);

            engine.ExecuteFile(settingScriptPath, scope);
        }

        public static void RunSpellScript(string path)
        {
            ScriptScope scope = engine.CreateScope(rootScope);

            engine.ExecuteFile(path, scope);
        }
    }
}

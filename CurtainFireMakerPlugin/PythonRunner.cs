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
        private ScriptEngine Engine { get; }
        private ScriptScope RootScope { get; }

        public PythonRunner(string settingScriptPath, string[] modullesDirPaths)
        {
            Engine = Python.CreateEngine();
            RootScope = Engine.CreateScope();

            ICollection<string> paths = Engine.GetSearchPaths();

            foreach (var path in modullesDirPaths)
            {
                paths.Add(path.Trim(' '));
            }
            Engine.SetSearchPaths(paths);

            Engine.Execute(
            "# -*- coding: utf-8 -*-\n" +
            "import sys\n" +
            "sys.path.append(r\"" + Application.StartupPath + "\\Plugins\")\n" +
            "sys.path.append(r\"" + Application.StartupPath + "\\System\\x64\")\n" +
            "import clr\n" +
            "clr.AddReference(\"CurtainFireMakerPlugin\")\n" +
            "clr.AddReference(\"CsPmx\")\n" +
            "clr.AddReference(\"CsVmd\")\n" +
            "clr.AddReference(\"DxMath\")\n" +
            "clr.AddReference(\"VecMath\")\n" +
            "clr.AddReference(\"SolidMath\")\n" +
            "clr.AddReference(\"MikuMikuPlugin\")\n", RootScope);

            ScriptScope scope = Engine.CreateScope(RootScope);

            Engine.ExecuteFile(settingScriptPath, scope);
        }

        public void RunScript(string path, World world)
        {
            ScriptScope scope = Engine.CreateScope(RootScope);

            scope.SetVariable("world", world);

            Engine.ExecuteFile(path, scope);
        }

        public void SetOut(Stream stream)
        {
            Engine.Runtime.IO.SetOutput(stream, Encoding.ASCII);
        }

        public string FormatException(Exception e) => Engine.GetService<ExceptionOperations>().FormatException(e);
    }
}

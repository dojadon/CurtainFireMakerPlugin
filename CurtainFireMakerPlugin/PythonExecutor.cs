using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using IronPython.Hosting;
using IronPython.Runtime;
using Microsoft.Scripting.Hosting;
using System.Windows.Forms;

namespace CurtainFireMakerPlugin
{
    internal class PythonExecutor
    {
        public ScriptEngine Engine { get; set; }
        public ScriptScope RootScope { get; set; }

        public PythonExecutor()
        {
        }

        public void Init(IEnumerable<string> modullesDirPaths)
        {
            Engine = Python.CreateEngine();
            RootScope = Engine.CreateScope();

            Engine.SetSearchPaths(Engine.GetSearchPaths().Concat(modullesDirPaths).ToList());

            Engine.Execute(
            "# -*- coding: utf-8 -*-\n" +
            "import sys\n" +
            $"sys.path.append(r\"{Application.StartupPath}\\Plugins\")\n" +
            $"sys.path.append(r\"{Application.StartupPath}\\System\\x64\")\n", RootScope);
        }

        public dynamic ExecuteScriptOnRootScope(string path)
        {
            return Engine.ExecuteFile(path, RootScope);
        }

        public dynamic ExecuteScriptOnNewScope(string path)
        {
            var scope = Engine.CreateScope(RootScope);
            return Engine.ExecuteFile(path, scope);
        }

        public void SetGlobalVariable(params (string name, object value)[] variables)
        {
            foreach (var variable in variables)
            {
                RootScope.SetVariable(variable.name, variable.value);
            }
        }

        public void SetOut(Stream stream)
        {
            Engine.Runtime.IO.SetOutput(stream, Encoding.UTF8);
        }

        public string FormatException(Exception e) => Engine.GetService<ExceptionOperations>().FormatException(e);
    }
}

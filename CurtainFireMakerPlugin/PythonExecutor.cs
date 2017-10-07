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
    internal class PythonExecutor
    {
        public ScriptEngine Engine { get; set; }
        public ScriptScope RootScope { get; set; }

        public PythonExecutor()
        {
        }

        public void Init(string[] modullesDirPaths)
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
            $"sys.path.append(r\"{Application.StartupPath}\\Plugins\")\n" +
            $"sys.path.append(r\"{Application.StartupPath}\\System\\x64\")\n" +
            "import clr\n" +
            "clr.AddReference(\"CurtainFireMakerPlugin\")\n" +
            "clr.AddReference(\"VecMath\")\n" +
            "clr.AddReference(\"MikuMikuPlugin\")\n", RootScope);
        }

        public dynamic ExecuteScriptOnNewScope(string path, params Variable[] variables)
        {
            ScriptScope scope = Engine.CreateScope(RootScope);

            foreach (var variable in variables)
            {
                scope.SetVariable(variable.Name, variable.Value);
            }

            return Engine.Execute(path, scope);
        }

        public void SetOut(Stream stream)
        {
            Engine.Runtime.IO.SetOutput(stream, Encoding.ASCII);
        }

        public string FormatException(Exception e) => Engine.GetService<ExceptionOperations>().FormatException(e);
    }

    public class Variable
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public Variable(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}

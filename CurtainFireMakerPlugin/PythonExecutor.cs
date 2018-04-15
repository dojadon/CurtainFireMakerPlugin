using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using IronPython.Hosting;
using IronPython.Compiler;
using Microsoft.Scripting.Hosting;
using System.Windows.Forms;

namespace CurtainFireMakerPlugin
{
    public class PythonExecutor
    {
        public ScriptEngine Engine { get; }
        public ScriptScope RootScope { get; }

        public PythonExecutor(IEnumerable<string> modullesDirPaths)
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

        public dynamic ExecuteOnRootScope(string script)
        {
            return Engine.Execute(script, RootScope);
        }

        public dynamic ExecuteOnNewScope(string script)
        {
            var scope = Engine.CreateScope(RootScope);
            return Engine.Execute(script, scope);
        }

        public dynamic ExecuteFileOnRootScope(string path)
        {
            return Engine.ExecuteFile(path, RootScope);
        }

        public dynamic ExecuteFileOnNewScope(string path)
        {
            var scope = Engine.CreateScope(RootScope);
            return Engine.ExecuteFile(path, scope);
        }

        public void SetGlobalVariable(params (string name, object value)[] variables)
        {
            foreach (var (name, value) in variables)
            {
                RootScope.SetVariable(name, value);
            }
        }

        public void SetOut(Stream stream)
        {
            Engine.Runtime.IO.OutputStream.Dispose();
            Engine.Runtime.IO.SetOutput(stream, Encoding.UTF8);
        }

        public void SetOut(TextWriter writer)
        {
            var ms = new MemoryStream();
            Engine.Runtime.IO.SetOutput(ms, writer);
        }

        public string FormatException(Exception e) => Engine.GetService<ExceptionOperations>().FormatException(e);
    }
}

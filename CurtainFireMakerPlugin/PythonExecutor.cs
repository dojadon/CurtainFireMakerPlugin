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
            foreach (var variable in variables)
            {
                RootScope.SetVariable(variable.name, variable.value);
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

    public class MyEvtArgs<T> : EventArgs
    {
        public T Value { get; private set; }

        public MyEvtArgs(T value)
        {
            Value = value;
        }
    }

    public class EventRaisingStreamWriter : StreamWriter
    {
        public event EventHandler<MyEvtArgs<string>> StringWritten;

        public EventRaisingStreamWriter(Stream s) : base(s)
        { }

        private void LaunchEvent(string txtWritten)
        {
            StringWritten?.Invoke(this, new MyEvtArgs<string>(txtWritten));
        }

        public override void Write(string value)
        {
            base.Write(value);
            LaunchEvent(value);
        }

        public override void Write(bool value)
        {
            base.Write(value);
            LaunchEvent(value.ToString());
        }
    }
}

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

        public PythonExecutor()
        {
            Engine = Python.CreateEngine();
            RootScope = Engine.CreateScope();

            RootScope.SetVariable("STARTUP_PATH", Application.StartupPath);
            RootScope.SetVariable("PYTHON_LIB_DIRECTORY", Path.GetDirectoryName(GetPython2File()) + "\\Lib");
        }

        public ScriptScope CreateScope() => Engine.CreateScope(RootScope);

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

        public static string GetPython2File()
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.FileName = "py";
            process.StartInfo.Arguments = $"-2 {Plugin.PluginRootPath + "\\print_pythonpath.py"}";
            process.Start();

            string result = process.StandardOutput.ReadToEnd();

            return result.Split(';').Where(Directory.Exists).SelectMany(Directory.GetFiles).First(s => s.EndsWith("python.exe"));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronPython.Hosting;
using IronPython.Runtime;
using Microsoft.Scripting.Hosting;
using CurtainFireMakerPlugin.ShotTypes;

namespace CurtainFireMakerPlugin
{
    internal class PythonRunner
    {
        private static ScriptEngine engine;

        public static void Init(string path)
        {
            engine = Python.CreateEngine();
            engine.ExecuteFile(path);
        }

        public static void RunShotTypeScript(string path)
        {
            dynamic scope = engine.ExecuteFile(path);

            ShotTypeList.Init(list => scope.setup(list));
        }

        public static void RunSpellScript(string path, World world)
        {
            dynamic scope = engine.ExecuteFile(path);

            scope.setup(world);
        }
    }
}

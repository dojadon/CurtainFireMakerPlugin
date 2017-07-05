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

            try
            {
                ShotTypeList.Init(list => scope.setup(list));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void RunSpellScript(string path, World world)
        {
            dynamic scope = engine.ExecuteFile(path);

            try
            {
                scope.setup(world);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}

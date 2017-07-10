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

        public static void Init()
        {
            engine = Python.CreateEngine();
            engine.Execute(
            "# -*- coding: utf-8 -*- \n" +
            "import sys" +
            "sys.path.append(r\"C:\\tool\\model\\MikuMikuMoving64_v1272\\Plugins\")" +
            "import clr" +
            "clr.AddReference(\"CurtainFireMakerPlugin\")" +
            "clr.AddReference(\"MikuMikuPlugin\")" +
            "clr.AddReference(\"DxMath\")"
            );
        }

        public static void RunSpellScript(string path, World world)
        {
            dynamic scope = engine.ExecuteFile(path);

            ShotTypeList.Init(list => scope.setup_shottype(list));
            scope.setup_world(world);
        }
    }
}

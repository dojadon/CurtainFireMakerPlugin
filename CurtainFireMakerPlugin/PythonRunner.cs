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

        private static void Init()
        {
            engine = Python.CreateEngine();
            engine.Execute(
            "# -*- coding: utf-8 -*-\n" +
            "import sys\n" +
            "sys.path.append(r\"C:\\tool\\model\\MikuMikuMoving64_v1272\\Plugins\")\n" +
            "import clr\n" +
            "clr.AddReference(\"CurtainFireMakerPlugin\")\n" +
            "clr.AddReference(\"MikuMikuPlugin\")\n" +
            "clr.AddReference(\"DxMath\")\n"
            );
        }

        public static void RunSpellScript(string path, World world)
        {
            if (engine == null)
            {
                Init();
            }

            dynamic scope = engine.ExecuteFile(path);

            ShotTypeList.Init(list => scope.setup_shottype(list));
            scope.setup_world(world);
        }
    }
}

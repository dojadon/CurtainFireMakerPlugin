using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronPython.Hosting;
using IronPython.Runtime;
using Microsoft.Scripting.Hosting;
using CurtainFireMakerPlugin.ShotTypes;
using System.Windows.Forms;

namespace CurtainFireMakerPlugin
{
    internal class PythonRunner
    {
        private static ScriptEngine engine;

        public static void Init(string path)
        {
            engine = Python.CreateEngine();
            engine.Execute(
            "# -*- coding: utf-8 -*-\n" +
            "import sys\n" +
            "sys.path.append(r\"" + Application.StartupPath + "\\Plugins\")\n" +
            "import clr\n" +
            "clr.AddReference(\"CurtainFireMakerPlugin\")\n" +
            "clr.AddReference(\"MikuMikuPlugin\")\n");

            dynamic scope = engine.ExecuteFile(path);

            ShotTypeList.Init(list => scope.setup_shottype(list));
        }

        public static void RunSpellScript(string path, World world)
        {
            dynamic scope = engine.ExecuteFile(path);

            scope.setup_world(world);
        }
    }
}

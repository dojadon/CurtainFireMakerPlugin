using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronPython.Hosting;
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
            "import clr \n" +
            "clr.AddReference(\"CurtainFireMakerPlugin\") \n" +
            "clr.AddReference(\"MikuMikuPlugin\")\n" +
            "clr.AddReference(\"DxMath\")\n");
        }

        public static void RunShotTypeScript(string path)
        {
            ScriptScope scope = engine.CreateScope();

            engine.ExecuteFile(path);
            Action<List< ShotType >> action = engine.Operations.GetMember<Action<List<ShotType>>>(scope, "setup");
        }

        public static void RunSpellScript(string path, World world)
        {
            ScriptScope scope = engine.CreateScope();

            engine.ExecuteFile(path);
            Action<World> action = engine.Operations.GetMember<Action<World>>(scope, "setup");
            action(world);
        }
    }
}

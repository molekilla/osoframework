using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using OsoFramework.Http;

namespace  OsoFramework
{
    public class ScriptingRobot : WebRobotBase, IWebRobot
    {
        HttpSettings MySettings = new HttpSettings();

        string scriptCode = String.Empty;
        public ScriptingRobot(string script)
            : base()
        {
            scriptCode = script;
            DatabaseRepository = new DataRepositoryBase();
        }
        public void Start()
        {
            // run code
            ScriptEngine engine = Python.CreateEngine();
           
            ScriptScope scope = engine.CreateScope();
            scope.SetVariable("settings", MySettings);
            scope.SetVariable("self",this);
            ScriptSource source = engine.CreateScriptSourceFromString(scriptCode, Microsoft.Scripting.SourceCodeKind.Statements);
            source.Execute(scope);
        }

        public static ScriptingRobot RunRobotScript(string name, string script)
        {
            var scriptingBot = new ScriptingRobot(script);
            scriptingBot.Name = name;
            return scriptingBot;
        }
   }
}

using System;
using System.IO;
using LangScriptCompilateur;

namespace ScriptLangConsole
{
    class LangConsole
    {
        static void Main(string[] args)
        {
            //if (args.Length == 0) return;

            //string path = @"C:\Users\Thor\Documents\script.sc";
            string path = @"..\scripts\script.sc";
            //var fs = new FileStream(args[0], FileMode.Open);
            var fs = new FileStream(path, FileMode.Open);

            string script = "";
            using (StreamReader sr = new StreamReader(fs))
                script = sr.ReadToEnd();

            Compilateur c = new Compilateur();
            c.Compile(script);

            Console.ReadKey();
        }
    }
}

using LangScriptCompilateur;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ScriptLangConsole
{
    class LangConsole
    {
        static void Main(string[] args)
        {
#if DEBUG
            args = new[] { "../scripts/script.sc" };
#endif

            string scriptText = "";

            if(args.Length == 0)
            {
                List<string> lines = new List<string>();
                while(true)
                {
                    string line = Console.ReadLine();
                    if(line.Length != 0)
                    {
                        lines.Add(line);
                    }

                    if(line.Length == 0 && lines.Count != 0)
                    {
                        var sb = new StringBuilder();
                        lines.ForEach(a => sb.Append(a));
                        lines.Clear();
                        scriptText = sb.ToString();
                        sb.Clear();
                    }
                }
            }
            else
            {
                string path = args[0];
                DirectoryInfo di = new DirectoryInfo(path);
                Console.WriteLine("Current path:" + di.FullName);
                    var fs = new FileStream(path, FileMode.Open);

                using (StreamReader sr = new StreamReader(fs))
                    scriptText = sr.ReadToEnd();
            }

            try
            {
                new Compilateur().Compile(scriptText);
            }
            catch (Exception e)
            {
                Console.ReadKey();
            }
        }
    }
}

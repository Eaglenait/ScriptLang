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
            Console.WriteLine("1 to enter text mode");
            Console.WriteLine("2 to enter file mode");
            Console.WriteLine("3 to quit");
            var readKey = Console.ReadKey();

            if(readKey.KeyChar == '1')
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
                        new Compilateur().Compile(sb.ToString());
                        sb.Clear();
                    }
                }
            }
            else if(readKey.KeyChar == '2')
            {
                string path = @"..\scripts\script.sc";
                Console.WriteLine("Current path:" + path);
                var fs = new FileStream(path, FileMode.Open);

                string script = "";
                using (StreamReader sr = new StreamReader(fs))
                    script = sr.ReadToEnd();

                new Compilateur().Compile(script);
            }
            else
            {
                return;        
            }
        }
    }
}

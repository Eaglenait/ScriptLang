using System;
using System.Collections.Generic;

namespace LangScriptCompilateur
{
    public sealed class KompilationLogger
    {
        private static readonly Lazy<KompilationLogger> lazy = new Lazy<KompilationLogger>(() => new KompilationLogger());
        public static KompilationLogger Instance { get { return lazy.Value; } }

        public List<(string, Severity)> Log { get; private set; }

        public bool HasFatal()
        {
            foreach (var log in Log)
            {
                if(log.Item2 == Severity.Fatal) return true;
            }

            return false;
        }

        private KompilationLogger()
        {
            Log = new List<(string, Severity)>();
        }

        public void LogMessage(string message)
        {
            AddLog(message, Severity.Message);
        }

        public void LogWarning(string message)
        {
            AddLog(message, Severity.Warning);
        }

        public void LogFatal(string message)
        {
            AddLog(message, Severity.Fatal);
        }

        public void AddLog(string message, Severity severity)
        {
            Log.Add((message, severity));
        }
    }

    public enum Severity
    {
        Message,
        Warning,
        Fatal,
    }
}

using LangScriptCompilateur.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LangScriptCompilateur.Models
{
    public class Token
    {
        public override string ToString()
        {
            return $"{Signature.ToString()}:{Word}";
        }
        
        public string Word { get; set; }
        public Signature Signature { get; set; }
    }
  
}

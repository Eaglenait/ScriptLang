using System;
using System.Collections.Generic;
using System.Text;

namespace LangScriptCompilateur.Models.Nodes
{
    interface ICompareOperation
    {
        bool Compare<T>(T lhs, T rhs);
    }
}

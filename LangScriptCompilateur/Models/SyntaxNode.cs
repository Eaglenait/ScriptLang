﻿using LangScriptCompilateur.Models.Enums;
using System.Collections.Generic;

namespace LangScriptCompilateur.Models
{
    public class SyntaxNode
    {
        public OperationType NodeType { get; set; }
        public SyntaxNode Parent { get; set; }
        public List<SyntaxNode> Childrens { get; set; } = new List<SyntaxNode>();

        public static SyntaxNode GetRoot() => new SyntaxNode()
        { 
            NodeType = OperationType.PARENT,
            Parent = null,
        };

        //Root constructor only
        protected SyntaxNode() { }

        public SyntaxNode(OperationType nodeType)
        {
            NodeType = nodeType;
        }

        public bool HasChildrens
        {
            get
            {
                if (Childrens == null)
                {
                    return false;
                }

                if (Childrens.Count != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void AddChild(SyntaxNode child)
        {
            child.Parent = this;
            Childrens.Add(child);
        }

        public void AddChild(OperationType nodeType)
            => AddChild(new SyntaxNode(nodeType));
    }
}

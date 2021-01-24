﻿using LangScriptCompilateur.Models.Enums;
using LangScriptCompilateur.Models.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LangScriptCompilateur.Models
{
    public class SyntaxTree
    {
        public SyntaxNode TreeRoot { get; private set; }
        public SyntaxNode Current { get; private set; }
        private KompilationLogger _logger;
        public List<DeclarationNode> VariableStack { get; set; }

        public SyntaxTree()
        {
            _logger = KompilationLogger.Instance;
            TreeRoot = SyntaxNode.GetRoot();
            Current = TreeRoot;
            VariableStack = new List<DeclarationNode>();
        }
        int GoRootParentAccess { get; set; }

        /// <summary>
        /// Access the parent node
        /// </summary>
        /// <returns>parent node if it exists root otherwise</returns>
        public SyntaxNode Up()
        {
            if(Current.Parent != null)
            {
                var parent = Current.Parent;
                Current = Current.Parent;
                return parent;
            }
            else
            {
                KompilationLogger.Instance.AddLog("SyntaxTree: attempt to go higher than root node", Severity.Warning);
                Current = TreeRoot;
                return TreeRoot;
            }
        }

        /// <summary>
        /// Access a child node
        /// </summary>
        /// <param name="childIndex">index of the child node</param>
        /// <returns>child node or null if it doesn't exist</returns>
        public SyntaxNode Down(int childIndex)
        {
            if(Current.HasChildrens)
            {
                var child = Current.Childrens.ElementAtOrDefault(childIndex);
                if(child != null)
                {
                    Current = child;
                    return child;
                }
            }

            KompilationLogger.Instance.AddLog($"SyntaxTree: Attempt to access invalid child at index {childIndex}", Severity.Warning);
            return null;
        }

        /// <summary>
        /// Goes through all the nodes in the tree
        /// 
        /// </summary>
        public IEnumerable<SyntaxNode> IterateAllTree()
        {
            throw new NotImplementedException();
        }
    }
}

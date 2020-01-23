using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace LangScriptCompilateur.Models
{
    public class SyntaxTree
    {
        public SyntaxNode TreeRoot { get; private set; }
        List<int> CurrentNode { get; set; }

        public SyntaxTree()
        {
            CurrentNode = new List<int> { 0 };
            TreeRoot = new SyntaxNode() {
                Parent = null,
                Childrens = new List<SyntaxNode>()
            };
        }

        int GoRootParentAccess { get; set; }

        public void AddChild(SyntaxNode child)
        {
            Add(child, CurrentNode.ToArray());
            CurrentNode.Add(0);
        }

        public List<SyntaxNode> GetChildrens()
        {
            SyntaxNode syntaxNode = new SyntaxNode();
            for (int i = 0; i < CurrentNode.Count; i++)
            {

               syntaxNode = TreeRoot.Childrens[CurrentNode[i]]; 
            }

            if (syntaxNode.HasChildrens)
            {
                return syntaxNode.Childrens;
            }
            else
            {
                return null;
            }
        }

        public SyntaxNode GoCurrentParent()
        {
            if (TreeRoot.Parent != null)
            {
                GoRootParentAccess = 0;
                SyntaxNode node = new SyntaxNode();
                CurrentNode.RemoveAt(CurrentNode.Count - 1);

                return Get(CurrentNode.ToArray());
            } 
            else
            {
                GoRootParentAccess++;
                if (GoRootParentAccess > 2) throw new Exception("Multiple root access");
                
                return TreeRoot;
            }
        }

        public SyntaxNode Go(params int[] coords)
        {
            CurrentNode = coords.ToList();
            return Get(coords.ToArray());
        }

        public SyntaxNode Get(params int[] coords)
        {
            SyntaxNode syntaxNode = new SyntaxNode();
            if(coords.Length == 1)
            {
                return TreeRoot.Childrens[CurrentNode[coords[0]]];
            }
            for (int i = 0; i < coords.Length; i++)
            {
                syntaxNode = TreeRoot.Childrens[coords[i]]; 
            }
            return syntaxNode;
        }

        public void Add(SyntaxNode node, params int[] coords)
        {
            SyntaxNode syntaxNode = new SyntaxNode();
            if(coords.Length == 1)
            {
                TreeRoot.Childrens.Add(node);
                return;
            }

            for (int i = 0; i < coords.Length; i++)
            {
                syntaxNode = TreeRoot.Childrens[i];
            }

            syntaxNode.Childrens.Add(node);
        }
    }
}

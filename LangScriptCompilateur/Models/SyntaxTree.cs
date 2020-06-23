﻿using LangScriptCompilateur.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LangScriptCompilateur.Models
{
    public class SyntaxTree
    {
        public SyntaxNode TreeRoot { get; private set; }
        public List<int> CurrentNode { get; private set; }
        private KompilationLogger _logger;

        public SyntaxTree()
        {
            _logger = KompilationLogger.Instance;
            CurrentNode = new List<int> { 0 };
            TreeRoot = new SyntaxNode() {
                Parent = null,
                Childrens = new List<SyntaxNode>(),
                NodeType = OperationType.PARENT
            };
        }

        int GoRootParentAccess { get; set; }

        public void AddChild(SyntaxNode child)
        {
            Add(child, CurrentNode.ToArray());
            CurrentNode[CurrentNode.Count - 1]++;
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

                return Peek(CurrentNode.ToArray());
            }
            else
            {
                //meh
                //GoRootParentAccess++;
                //if (GoRootParentAccess > 2)
                //{
                //    _logger.LogFatal("Multiple root access");
                //    throw new Exception("Multiple root access");
                //}

                return TreeRoot;
            }
        }

        public void Go(params int[] coords)
        {
            CurrentNode = coords.ToList();
        }

        /// <summary>
        /// Goes through all the nodes in the tree
        /// 
        /// </summary>
        public IEnumerable<SyntaxNode> IterateAllTree()
        {
            if (!TreeRoot.HasChildrens)
            {
                yield return TreeRoot;
                yield break;
            }

            var coords = new List<int>(){ 0 };
            SyntaxNode currentBranch = null;
            while(true)
            {
                if(coords.Count == 1)
                {
                    if(coords[0] == -1) { break; }

                    if(TreeRoot.Childrens[coords[0]].HasChildrens)
                    {
                        //go down
                        coords.Add(0);

                        if(coords[0] < TreeRoot.Childrens.Count)
                        {
                            coords[0]++;
                        }
                        else
                        {
                            //stop on next root iteration
                            coords[0] = -1;
                        }
                    }
                    else
                    {
                        currentBranch = TreeRoot.Childrens[coords[0]];
                        yield return TreeRoot.Childrens[coords[0]];
                    }
                } 
                else
                {
                    if (currentBranch.HasChildrens)
                    {
                        //go down
                        coords.Add(0);
                        currentBranch = currentBranch.Childrens[0];
                    }
                    else /*if no child*/
                    {
                        //are there other childs ?
                        if (coords[coords.Count - 1] < currentBranch.Childrens.Count)
                        {
                            coords[coords.Count - 1]++;
                        }
                        else
                        {
                            if(coords.Count > 1)
                            {
                                coords.RemoveAt(coords.Count - 1);
                            }
                            //if there's no other child go up until you find another parent that has childrens
                            while (true)
                            {
                                if(currentBranch.Parent.Childrens.Count < coords[coords.Count - 1])
                                {
                                    coords[coords.Count - 1]++;
                                } 
                            }
                        }
                    }

                    //go up if no child
                }
            }
        }

        public SyntaxNode PeekCurrent()
        {
            return Peek(CurrentNode.ToArray());
        }

        public SyntaxNode Peek(params int[] coords)
        {
            if(coords.Length == 1)
            {
                if (TreeRoot.Childrens.Count >= coords[0])
                {
                    //"- 1" is possible bad fix (what happend if coord is 0)
                    return TreeRoot.Childrens[coords[0] - 1];
                }
                else
                {
                    _logger.LogFatal("invalid syntaxTree retrieval coords: " + CoordsToString(coords));
                    return null;
                }
            }

            SyntaxNode syntaxNode = TreeRoot;
            for (int i = 0; i < coords.Length; i++)
            {
                syntaxNode = syntaxNode.Childrens[coords[i]];
            }
            return syntaxNode;
        }

        public void Add(SyntaxNode node, params int[] coords)
        {
            if(coords.Length == 1)
            {
                if (coords[0] != TreeRoot.Childrens.Count) _logger.LogWarning("New Child coordinates mismatch");
                TreeRoot.Childrens.Add(node);
                return;
            }

            SyntaxNode syntaxNode = TreeRoot;
            for (int i = 0; i < coords.Length; i++)
            {
                if (i != coords.Length - 1)
                {
                    syntaxNode = syntaxNode.Childrens[i];
                }
                else
                {
                    //we can add the last child without correct coords
                    //last coord is the index of the child in the last parent node
                    if (coords[0] != syntaxNode.Childrens.Count)
                        _logger.LogWarning(string.Format("New Child coordinates mismatch lastidx:{0} tried:{1} actual {2}"
                                                        , syntaxNode.Childrens.Count, coords[0], syntaxNode.Childrens.Count));

                    syntaxNode.Childrens.Add(node);
                }
            }
        }

        private string CoordsToString(IEnumerable<int> coords)
        {
            var strCoords = new StringBuilder();
            foreach(int coord in coords)
            {
                strCoords.AppendFormat("{0}, ", coord.ToString());
            }

            //Pop last ','
            strCoords.Length--;
            return strCoords.ToString();
        }
    }
}

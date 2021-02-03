using System;
using LangScriptCompilateur.Models;
using LangScriptCompilateur.Models.Enums;
using LangScriptCompilateur.Models.Nodes;

namespace LangScriptCompilateur
{
    public class ComparaisonNode : SyntaxNode
    {
        public ValueNode l_op { get; set; }
        public ValueNode r_op { get; set; }

        public Signature ComparaisonType { get; set; }

        public ComparaisonNode()
        {
            NodeType = OperationType.OPERATION;
            l_op = new ValueNode();
            r_op = new ValueNode();
        }

        public bool Result()
        {
            if (l_op.ValueType == TypesEnum.VOID
            || r_op.ValueType == TypesEnum.VOID)
            {
                return false;
            }

            if (l_op.ValueType == r_op.ValueType)
            {
                if(ComparaisonType == Signature.OP_EQUALS)
                {
                    return l_op.Value.Equals(r_op.Value);
                }

                //Math compatible types 
                if((l_op.ValueType == TypesEnum.INT || l_op.ValueType == TypesEnum.FLOAT)
                    && (r_op.ValueType == TypesEnum.INT || r_op.ValueType == TypesEnum.FLOAT))
                {
                    switch (ComparaisonType)
                    {
                        case Signature.OP_GREATER_THAN:
                            if(l_op.ValueType == TypesEnum.FLOAT)
                            {
                                return (l_op.Value as float?) > (r_op.Value as float?);
                            }        
                            else if(l_op.ValueType == TypesEnum.INT)
                            {
                                return (l_op.Value as int?) > (r_op.Value as int?);
                            }
                            break;

                        case Signature.OP_GREATER_THAN_OR_EQUALS:
                            if(l_op.ValueType == TypesEnum.FLOAT)
                            {
                                return (l_op.Value as float?) >= (r_op.Value as float?);
                            }        
                            else if(l_op.ValueType == TypesEnum.INT)
                            {
                                return (l_op.Value as int?) >= (r_op.Value as int?);
                            }
                            break;

                        case Signature.OP_LESS_THAN:
                            if(l_op.ValueType == TypesEnum.FLOAT)
                            {
                                return (l_op.Value as float?) < (r_op.Value as float?);
                            }        
                            else if(l_op.ValueType == TypesEnum.INT)
                            {
                                return (l_op.Value as int?) < (r_op.Value as int?);
                            }
                            break;

                        case Signature.OP_LESS_THAN_OR_EQUALS:
                            if(l_op.ValueType == TypesEnum.FLOAT)
                            {
                                return (l_op.Value as float?) <= (r_op.Value as float?);
                            }        
                            else if(l_op.ValueType == TypesEnum.INT)
                            {
                                return (l_op.Value as int?) <= (r_op.Value as int?);
                            }
                            break;
                    }
                }
            }

            throw new Exception("Incompatible types");
        }

        #region override
        public override void AddChild(SyntaxNode child)
            => throw new System.Exception("Can't add child to this");

        public override void AddChild(OperationType nodeType)
            => AddChild(nodeType);
        #endregion
    }
}
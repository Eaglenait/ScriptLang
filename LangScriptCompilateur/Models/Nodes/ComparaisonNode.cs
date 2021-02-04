using System;
using LangScriptCompilateur.Models;
using LangScriptCompilateur.Models.Enums;
using LangScriptCompilateur.Models.Nodes;

namespace LangScriptCompilateur
{
    /*
      l_op toujours ValueNode
      nodes autorisés pour r_op
        OperationNode
        VarNode -> pas super propre si c'est une operation que avec des const literaux mais pas le choix
                   sinon on peut pas avoir d'opérations ou l'on fait référence à d'autres variables

    A partir de r_op on peut créer un arbre d'opérations binaires dont chaque résultat est repris
    par l'opération suivante (parente). Lorsque on arrive au dernier parent de l'opération on a le
    résultat de tout le calcul

    Comment get la valeur finale d'un arbre OperationNode ?
    l_op = current l_op [operation] (child l_op value r_op)
    En pseudo
        aller au plus profond des r_op
        quand au dernier
            l_op = l_op [operation] r_op
        
        while parent is OperationNode
            current = parent
            l_op = l_op [operation] child.l_op

        return l_op;
        
        return l_op [operation] r_op
    
    
    1 + 2 - 3
    
    -l_op:1
     parent = null
     type:+
     r_op:
        -l_op:2
         type:-
         parent = (l_op:1 type:+)
         r_op:
            -l_op:3
             type:none
             parent = (l_op:2 type:-)
             r_op:null

    Etape 1 :
        while(r_op != null)
            current = r_op;
    Etape 2 :
        //vérifier
        do
            current = parent
            current.l_op = current.l_op [operation] child.l_op (r_op)
        while(current.parent != null)

     */
    public class OperationNode : SyntaxNode, VarNode
    {
        public ValueNode l_op { get; set; }
        public OperationNode r_op { get; set; }

        public Signature ComparaisonType { get; set; }

        public OperationNode()
        {
            NodeType = OperationType.OPERATION;
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
        {
        }

        public override void AddChild(OperationType nodeType)
            => AddChild(nodeType);
        #endregion
    }
}
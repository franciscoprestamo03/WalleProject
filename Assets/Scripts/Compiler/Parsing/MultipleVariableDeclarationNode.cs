using System.Collections.Generic;

namespace Compiler
{
    public class MultipleVariableDeclarationNode : Node 
    {
        public List<TokenM> VariableNames { get; }

        public Node SequenceN;

        public MultipleVariableDeclarationNode(List<TokenM> variableNames,Node sequenceN)
        {
            VariableNames = variableNames;
            SequenceN = sequenceN;
        }
    }
}
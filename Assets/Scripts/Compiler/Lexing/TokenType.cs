namespace Compiler
{
    public enum TokenType
    {
        //Especial tokens
        WhiteSpaceToken,
        OpenParenToken,
        CloseParenToken,
        BadToken,
        NumberToken,
        IdentifierToken,
        PointDeclaration,
        CircleDeclaration,
        MeasureDeclaration,
        LineDeclaration,

        //Keyword tokens
        VarToken,
        IfToken,
        ElseToken,
        ForToken,
        WhileToken,
        DoToken,
        SwitchToken,
        CaseToken,
        BreakToken,
        ContinueToken,
        DefaultToken,
        ReturnToken,
        TrueToken,
        FalseToken,
        NullToken,
        LetToken,
        InToken,


        //Operators tokens

        AdditionToken,
        SubtractionToken,
        MultiplicationToken,
        DivisionToken,
        ModulusToken,
        EqualityToken,
        InequalityToken,
        GreaterThanToken,
        GreaterThanOrEqualToken,
        LessThanToken,
        LessThanOrEqualToken,
        LogicalAndToken,
        LogicalOrToken,
        LogicalNotToken,
        SemicolonToken,
        LeftParenthesisToken,
        RightParenthesisToken,
        EOF,
        AssignmentToken,
        FunctionToken,
        LeftBraceToken,
        RightBraceToken,
        PrintToken,
        QuestionMarkToken,
        ColonToken,
        CommaToken,
        StringToken,
        StringTypeToken,
        NumberTypeToken,
        TwoDotsToken,
        AssignmentTwoDotsToken,
        ArrowToken,
        ArrobaToken,
        PowToken,
        DrawToken,
        SequenceToken,
        ThreeDotsToken,
        ImportToken,
        SegmentToken
    }
}
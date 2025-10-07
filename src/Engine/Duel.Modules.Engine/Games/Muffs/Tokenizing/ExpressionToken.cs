namespace Duel.Modules.Engine.Games.Muffs.Tokenizing;

public enum TokenType
{
    Number,
    Plus,
    Minus,
    Multiply,
    Divide,
    Power,
    Modulo,
    LeftParen,
    RightParen,
    UnaryMinus,
    Abs,
    Sqrt,
    Factorial,
    EndOfInput
}

public sealed record ExpressionToken(TokenType Type, string Value)
{
    public static ExpressionToken From(TokenType type, string value)
    {
        return new ExpressionToken(type, value);
    }
}

internal static class ExpressionTokenRegistry
{
    public static readonly ExpressionToken Plus = ExpressionToken.From(TokenType.Plus, "+");
    
    public static readonly ExpressionToken Minus = ExpressionToken.From(TokenType.Minus, "-");
    
    public static readonly ExpressionToken Divide = ExpressionToken.From(TokenType.Divide, "/");
    
    public static readonly ExpressionToken Multiply = ExpressionToken.From(TokenType.Multiply, "*");
    
    public static readonly ExpressionToken Power = ExpressionToken.From(TokenType.Power, "^");
    
    public static readonly ExpressionToken Modulo = ExpressionToken.From(TokenType.Modulo, "%");
    
    public static readonly ExpressionToken LeftParen = ExpressionToken.From(TokenType.LeftParen, "(");
    
    public static readonly ExpressionToken RightParen = ExpressionToken.From(TokenType.RightParen, ")");
    
    public static readonly ExpressionToken EndOfInput = ExpressionToken.From(TokenType.EndOfInput, string.Empty);
}
namespace Duel.Modules.Engine.Games.Muffs.Tokenizing;

public sealed record ExpressionToken(ExpressionTokenType Type, string Value)
{
    public static ExpressionToken From(ExpressionTokenType type, string value)
    {
        return new ExpressionToken(type, value);
    }
}

internal static class ExpressionTokenRegistry
{
    public static readonly ExpressionToken Plus = ExpressionToken.From(ExpressionTokenType.Plus, "+");
    
    public static readonly ExpressionToken Minus = ExpressionToken.From(ExpressionTokenType.Minus, "-");
    
    public static readonly ExpressionToken Divide = ExpressionToken.From(ExpressionTokenType.Divide, "/");
    
    public static readonly ExpressionToken Multiply = ExpressionToken.From(ExpressionTokenType.Multiply, "*");
    
    public static readonly ExpressionToken Power = ExpressionToken.From(ExpressionTokenType.Power, "^");
    
    public static readonly ExpressionToken Modulo = ExpressionToken.From(ExpressionTokenType.Modulo, "%");
    
    public static readonly ExpressionToken LeftParen = ExpressionToken.From(ExpressionTokenType.LeftParen, "(");
    
    public static readonly ExpressionToken RightParen = ExpressionToken.From(ExpressionTokenType.RightParen, ")");
    
    public static readonly ExpressionToken EndOfInput = ExpressionToken.From(ExpressionTokenType.EndOfInput, string.Empty);
}
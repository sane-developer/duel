namespace Duel.Core.Games.Muffs.AST;

internal static class Operator
{
    public enum Code
    {
        Add, Subtract, Multiply, Divide, Power
    }

    public static bool IsRightAssociative(Code code)
    {
        return code is Code.Power;
    }

    public static int Precedence(Code code)
    {
        return code switch
        {
            Code.Add or Code.Subtract => 1,
            Code.Multiply or Code.Divide => 2,
            Code.Power => 3,
            _ => 0
        };
    }
}

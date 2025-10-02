using Duel.Core.Games.Muffs.AST;

namespace Duel.Core.Tests.Muffs.Parsing.Suites;

[TestFixture]
public sealed class WhitespaceTestSuite : ExpressionParserTestSuite
{
    [Test]
    public void Whitespace_IgnoredAroundTokens()
    {
        const string expression = "  7   *   (  8  -  5) ";
        
        var result = GetExpression(expression);

        var mul = IsValidBinary<Multiplication>(result);
        
        IsValidConstant(mul.Left, value: 7);

        var sub = IsValidBinary<Subtraction>(mul.Right);
        
        IsValidConstant(sub.Left, value: 8);
        
        IsValidConstant(sub.Right, value: 5);
    }
}
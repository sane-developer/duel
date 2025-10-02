using Duel.Core.Games.Muffs.AST;

namespace Duel.Core.Tests.Muffs.Parsing.Suites;

public class ParenthesesTestSuite : ExpressionParserTestSuite
{
    [Test]
    public void Parentheses_ForceAdditionBeforeMultiplication()
    {
        const string expression = "(2 + 3) * 4";
        
        var result = GetExpression(expression);

        var mul = IsValidBinary<Multiplication>(result);

        var add = IsValidBinary<Addition>(mul.Left);
        
        IsValidConstant(add.Left, value: 2);
        
        IsValidConstant(add.Right, value: 3);

        IsValidConstant(mul.Right, value: 4);
    }
}
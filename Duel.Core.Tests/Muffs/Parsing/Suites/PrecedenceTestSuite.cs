using Duel.Core.Games.Muffs.AST;

namespace Duel.Core.Tests.Muffs.Parsing.Suites;

[TestFixture]
public sealed class PrecedenceTestSuite : ExpressionParserTestSuite
{
    [Test]
    public void Precedence_MultiplicationOverAddition()
    {
        const string expression = "2 + 3 * 4";
        
        var result = GetExpression(expression);

        var add = IsValidBinary<Addition>(result);
        
        IsValidConstant(add.Left, value: 2);

        var mul = IsValidBinary<Multiplication>(add.Right);
        
        IsValidConstant(mul.Left, value: 3);
        
        IsValidConstant(mul.Right, value: 4);
    }

    [Test]
    public void Precedence_PowerOverMultiplication()
    {
        const string expression = "2 * 3 ^ 4";
        
        var result = GetExpression(expression);

        var mul = IsValidBinary<Multiplication>(result);
        
        IsValidConstant(mul.Left, value: 2);

        var pow = IsValidBinary<Power>(mul.Right);
        
        IsValidConstant(pow.Left, value: 3);
        
        IsValidConstant(pow.Right, value: 4);
    }

    [Test]
    public void Precedence_Mixed_AddSubMulDiv()
    {
        const string expression = "1 + 2 * 3 - 4 / 2";
        
        var result = GetExpression(expression);

        var outerSub = IsValidBinary<Subtraction>(result);

        var add = IsValidBinary<Addition>(outerSub.Left);
        
        IsValidConstant(add.Left, value: 1);

        var mul = IsValidBinary<Multiplication>(add.Right);
        
        IsValidConstant(mul.Left, value: 2);
        
        IsValidConstant(mul.Right, value: 3);

        var div = IsValidBinary<Division>(outerSub.Right);
        
        IsValidConstant(div.Left, value: 4);
        
        IsValidConstant(div.Right, value: 2);
    }
}
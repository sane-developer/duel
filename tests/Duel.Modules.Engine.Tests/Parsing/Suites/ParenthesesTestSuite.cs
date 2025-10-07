using Duel.Core.Games.Muffs.AST;

namespace Duel.Core.Tests.Muffs.Parsing.Suites;

[TestFixture]
public sealed class ParenthesesTestSuite : ExpressionParserTestSuite
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
    
    [Test]
    public void Parentheses_Redundant_DoNotChangeStructure()
    {
        const string expression = "((2)) + (((3)))";
        
        var result = GetExpression(expression);

        var add = IsValidBinary<Addition>(result);
        
        IsValidConstant(add.Left, value: 2);
        
        IsValidConstant(add.Right, value: 3);
    }

    [Test]
    public void Parentheses_TightBindingInsideChain()
    {
        const string expression = "1 + (2 * 3) + 4";
        
        var result = GetExpression(expression);

        var outerAdd = IsValidBinary<Addition>(result);
        
        var leftAdd = IsValidBinary<Addition>(outerAdd.Left);

        IsValidConstant(leftAdd.Left, value: 1);

        var mul = IsValidBinary<Multiplication>(leftAdd.Right);
        
        IsValidConstant(mul.Left, value: 2);
        
        IsValidConstant(mul.Right, value: 3);

        IsValidConstant(outerAdd.Right, value: 4);
    }

    [Test]
    public void Parentheses_AffectPowerAndMultiplication()
    {
        const string expression = "(2 ^ 3) * 4";
        
        var result = GetExpression(expression);

        var mul = IsValidBinary<Multiplication>(result);

        var pow = IsValidBinary<Power>(mul.Left);
        
        IsValidConstant(pow.Left, value: 2);
        
        IsValidConstant(pow.Right, value: 3);

        IsValidConstant(mul.Right, value: 4);
    }

    [Test]
    public void Parentheses_AroundEntireExpression()
    {
        const string expression = "(1 + 2 * 3 - 4 / 2)";
        
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

    [Test]
    public void Parentheses_DeepNesting_ReasonableDepthParses()
    {
        const string expression = "((((((((((((((((((((42))))))))))))))))))))";
        
        var result = GetExpression(expression);
        
        IsValidConstant(result, value: 42);
    }
}
using Duel.Modules.Engine.Games.Muffs.AST;

namespace Duel.Modules.Engine.Tests.Parsing.Suites;

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

    [Test]
    public void Precedence_DivisionOverAddition()
    {
        const string expression = "10 + 8 / 4";
        
        var result = GetExpression(expression);

        var add = IsValidBinary<Addition>(result);
        
        IsValidConstant(add.Left, value: 10);

        var div = IsValidBinary<Division>(add.Right);
        
        IsValidConstant(div.Left, value: 8);
        
        IsValidConstant(div.Right, value: 4);
    }

    [Test]
    public void Precedence_AdditiveChains_LeftAssociative()
    {
        const string expression = "1 + 2 - 3 + 4";
        
        var result = GetExpression(expression);

        var outerAdd = IsValidBinary<Addition>(result);

        var leftSub = IsValidBinary<Subtraction>(outerAdd.Left);
        
        var leftAdd = IsValidBinary<Addition>(leftSub.Left);

        IsValidConstant(leftAdd.Left, value: 1);
        
        IsValidConstant(leftAdd.Right, value: 2);
        
        IsValidConstant(leftSub.Right, value: 3);
        
        IsValidConstant(outerAdd.Right, value: 4);
    }

    [Test]
    public void Precedence_MultiplicativeChains_LeftAssociative()
    {
        const string expression = "24 / 3 * 2";

        var result = GetExpression(expression);

        var mul = IsValidBinary<Multiplication>(result);

        var div = IsValidBinary<Division>(mul.Left);
        
        IsValidConstant(div.Left, value: 24);
        
        IsValidConstant(div.Right, value: 3);

        IsValidConstant(mul.Right, value: 2);
    }

    [Test]
    public void Precedence_PowerOverAddition()
    {
        const string expression = "2 + 3 ^ 2";
        
        var result = GetExpression(expression);

        var add = IsValidBinary<Addition>(result);
        
        IsValidConstant(add.Left, value: 2);

        var pow = IsValidBinary<Power>(add.Right);
       
        IsValidConstant(pow.Left, value: 3);
        
        IsValidConstant(pow.Right, value: 2);
    }

    [Test]
    public void Precedence_PowerChains_RightAssociative_DominatesOthers()
    {
        const string expression = "2 ^ 3 ^ 2 * 5";
        
        var result = GetExpression(expression);

        var mul = IsValidBinary<Multiplication>(result);

        var powOuter = IsValidBinary<Power>(mul.Left);
        
        IsValidConstant(powOuter.Left, value: 2);

        var powInner = IsValidBinary<Power>(powOuter.Right);
        
        IsValidConstant(powInner.Left, value: 3);
        
        IsValidConstant(powInner.Right, value: 2);

        IsValidConstant(mul.Right, value: 5);
    }

    [Test]
    public void Precedence_LongChain_NormalizesByRules()
    {
        const string expression = "1 + 2 * 3 + 4 / 2 - 5 ^ 2 + 6";
        
        var result = GetExpression(expression);

        var add6 = IsValidBinary<Addition>(result);
        
        IsValidConstant(add6.Right, value: 6);

        var subPow = IsValidBinary<Subtraction>(add6.Left);
        
        var pow = IsValidBinary<Power>(subPow.Right);
        
        IsValidConstant(pow.Left, value: 5);
        
        IsValidConstant(pow.Right, value: 2);

        var addDiv = IsValidBinary<Addition>(subPow.Left);
        
        var div = IsValidBinary<Division>(addDiv.Right);
        
        IsValidConstant(div.Left, value: 4);
        
        IsValidConstant(div.Right, value: 2);

        var addMul = IsValidBinary<Addition>(addDiv.Left);
        
        IsValidConstant(addMul.Left, value: 1);

        var mul = IsValidBinary<Multiplication>(addMul.Right);
        
        IsValidConstant(mul.Left, value: 2);
        
        IsValidConstant(mul.Right, value: 3);
    }
}
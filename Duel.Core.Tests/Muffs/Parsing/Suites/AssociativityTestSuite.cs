using Duel.Core.Games.Muffs.AST;

namespace Duel.Core.Tests.Muffs.Parsing.Suites;

[TestFixture]
public sealed class AssociativityTestSuite : ExpressionParserTestSuite
{
    [Test]
    public void Associativity_Left_SubtractionChainsLeft()
    {
        const string expression = "10 - 3 - 2";
        
        var result = GetExpression(expression);

        var outer = IsValidBinary<Subtraction>(result);

        var inner = IsValidBinary<Subtraction>(outer.Left);
        
        IsValidConstant(inner.Left, value: 10);
        
        IsValidConstant(inner.Right, value: 3);

        IsValidConstant(outer.Right, value: 2);
    }

    [Test]
    public void Associativity_Left_DivisionChainsLeft()
    {
        const string expression = "20 / 5 / 2";
        
        var result = GetExpression(expression);

        var outer = IsValidBinary<Division>(result);

        var inner = IsValidBinary<Division>(outer.Left);
        
        IsValidConstant(inner.Left, value: 20);
        
        IsValidConstant(inner.Right, value: 5);

        IsValidConstant(outer.Right, value: 2);
    }

    [Test]
    public void Associativity_Right_PowerChainsRight()
    {
        const string expression = "2 ^ 3 ^ 4";
        
        var result = GetExpression(expression);

        var outer = IsValidBinary<Power>(result);
        
        IsValidConstant(outer.Left, value: 2);

        var inner = IsValidBinary<Power>(outer.Right);
       
        IsValidConstant(inner.Left, value: 3);
        
        IsValidConstant(inner.Right, value: 4);
    }
}
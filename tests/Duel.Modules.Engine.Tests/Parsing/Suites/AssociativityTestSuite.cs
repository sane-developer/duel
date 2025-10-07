using Duel.Core.Games.Muffs.AST;

namespace Duel.Core.Tests.Muffs.Parsing.Suites;

[TestFixture]
public sealed class AssociativityTestSuite : ExpressionParserTestSuite
{
    [Test]
    public void Associativity_Addition_LeftAssociative()
    {
        const string expression = "1 + 2 + 3 + 4";
        
        var result = GetExpression(expression);

        var add4 = IsValidBinary<Addition>(result);
        
        IsValidConstant(add4.Right, value: 4);

        var add3 = IsValidBinary<Addition>(add4.Left);
        
        IsValidConstant(add3.Right, value: 3);

        var add2 = IsValidBinary<Addition>(add3.Left);
        
        IsValidConstant(add2.Left, value: 1);
        
        IsValidConstant(add2.Right, value: 2);
    }

    [Test]
    public void Associativity_Subtraction_LeftAssociative()
    {
        const string expression = "10 - 3 - 2 - 1";
        
        var result = GetExpression(expression);

        var sub1 = IsValidBinary<Subtraction>(result);
        
        IsValidConstant(sub1.Right, value: 1);

        var sub2 = IsValidBinary<Subtraction>(sub1.Left);
        
        IsValidConstant(sub2.Right, value: 2);

        var sub3 = IsValidBinary<Subtraction>(sub2.Left);
        
        IsValidConstant(sub3.Left, value: 10);
        
        IsValidConstant(sub3.Right, value: 3);
    }

    [Test]
    public void Associativity_Multiplication_LeftAssociative()
    {
        const string expression = "2 * 3 * 4 * 5";
        
        var result = GetExpression(expression);

        var mul5 = IsValidBinary<Multiplication>(result);
        
        IsValidConstant(mul5.Right, value: 5);

        var mul4 = IsValidBinary<Multiplication>(mul5.Left);
        
        IsValidConstant(mul4.Right, value: 4);

        var mul3 = IsValidBinary<Multiplication>(mul4.Left);
        
        IsValidConstant(mul3.Left, value: 2);
        
        IsValidConstant(mul3.Right, value: 3);
    }

    [Test]
    public void Associativity_Division_LeftAssociative()
    {
        const string expression = "100 / 5 / 2 / 5";
        
        var result = GetExpression(expression);

        var div5 = IsValidBinary<Division>(result);
        
        IsValidConstant(div5.Right, value: 5);

        var div2 = IsValidBinary<Division>(div5.Left);
        
        IsValidConstant(div2.Right, value: 2);

        var div5A = IsValidBinary<Division>(div2.Left);
        
        IsValidConstant(div5A.Left, value: 100);
        
        IsValidConstant(div5A.Right, value: 5);
    }

    [Test]
    public void Associativity_AddSub_MixedSamePrecedence_LeftAssociative()
    {
        const string expression = "10 - 3 + 2 - 1";
        
        var result = GetExpression(expression);

        var sub1 = IsValidBinary<Subtraction>(result);
        
        IsValidConstant(sub1.Right, value: 1);

        var add2 = IsValidBinary<Addition>(sub1.Left);
        
        IsValidConstant(add2.Right, value: 2);

        var sub3 = IsValidBinary<Subtraction>(add2.Left);
       
        IsValidConstant(sub3.Left, value: 10);
        
        IsValidConstant(sub3.Right, value: 3);
    }

    [Test]
    public void Associativity_MulDiv_MixedSamePrecedence_LeftAssociative()
    {
        const string expression = "40 / 5 * 2 / 4";
        
        var result = GetExpression(expression);

        var div4 = IsValidBinary<Division>(result);
        
        IsValidConstant(div4.Right, value: 4);

        var mul2 = IsValidBinary<Multiplication>(div4.Left);
        
        IsValidConstant(mul2.Right, value: 2);

        var div5 = IsValidBinary<Division>(mul2.Left);
        
        IsValidConstant(div5.Left, value: 40);
        
        IsValidConstant(div5.Right, value: 5);
    }

    [Test]
    public void Associativity_Power_RightAssociative_ThreeChain()
    {
        const string expression = "2 ^ 3 ^ 2";
        
        var result = GetExpression(expression);

        var powOuter = IsValidBinary<Power>(result);
        
        IsValidConstant(powOuter.Left, value: 2);

        var powInner = IsValidBinary<Power>(powOuter.Right);
        
        IsValidConstant(powInner.Left, value: 3);
        
        IsValidConstant(powInner.Right, value: 2);
    }

    [Test]
    public void Associativity_Power_RightAssociative_FourChain()
    {
        const string expression = "2 ^ 3 ^ 2 ^ 1";
        
        var result = GetExpression(expression);

        var powA = IsValidBinary<Power>(result);
        
        IsValidConstant(powA.Left, value: 2);

        var powB = IsValidBinary<Power>(powA.Right);
        
        IsValidConstant(powB.Left, value: 3);

        var powC = IsValidBinary<Power>(powB.Right);
        
        IsValidConstant(powC.Left, value: 2);
        
        IsValidConstant(powC.Right, value: 1);
    }

    [Test]
    public void Associativity_Power_WithParentheses_OverridesRightAssociativity()
    {
        const string expression = "(2 ^ 3) ^ 2";
        
        var result = GetExpression(expression);

        var powOuter = IsValidBinary<Power>(result);
        
        IsValidConstant(powOuter.Right, value: 2);

        var powInner = IsValidBinary<Power>(powOuter.Left);
        
        IsValidConstant(powInner.Left, value: 2);
        
        IsValidConstant(powInner.Right, value: 3);
    }
}
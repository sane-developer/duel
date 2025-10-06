using Duel.Core.Games.Muffs.AST;

namespace Duel.Core.Tests.Muffs.Parsing.Suites;

[TestFixture]
public sealed class WhitespaceTestSuite : ExpressionParserTestSuite
{
    [Test]
    public void Whitespace_AroundOperators_Ignored()
    {
        const string expression = "   2    +     3   ";
        
        var result = GetExpression(expression);

        var add = IsValidBinary<Addition>(result);
        
        IsValidConstant(add.Left, value: 2);
        
        IsValidConstant(add.Right, value: 3);
    }

    [Test]
    public void Whitespace_NoSpaces_StillParses()
    {
        const string expression = "1+2*3-4/2";
        
        var result = GetExpression(expression);

        var sub = IsValidBinary<Subtraction>(result);

        var add = IsValidBinary<Addition>(sub.Left);
        
        IsValidConstant(add.Left, value: 1);

        var mul = IsValidBinary<Multiplication>(add.Right);
        
        IsValidConstant(mul.Left, value: 2);
        
        IsValidConstant(mul.Right, value: 3);

        var div = IsValidBinary<Division>(sub.Right);
        
        IsValidConstant(div.Left, value: 4);
        
        IsValidConstant(div.Right, value: 2);
    }

    [Test]
    public void Whitespace_TabsAndNewlines_AroundTokens_Ignored()
    {
        const string expression = "\t7 \n * \r\n (  8   - \t 5 )  ";
        
        var result = GetExpression(expression);

        var mul = IsValidBinary<Multiplication>(result);
        
        IsValidConstant(mul.Left, value: 7);

        var sub = IsValidBinary<Subtraction>(mul.Right);
        
        IsValidConstant(sub.Left, value: 8);
        
        IsValidConstant(sub.Right, value: 5);
    }

    [Test]
    public void Whitespace_AroundParentheses_Ignored()
    {
        const string expression = " ( ( 2 ) )  +   (  ( 3 )  ) ";
        
        var result = GetExpression(expression);

        var add = IsValidBinary<Addition>(result);
        
        IsValidConstant(add.Left, value: 2);
        
        IsValidConstant(add.Right, value: 3);
    }

    [Test]
    public void Whitespace_MultilineFormatting_DoesNotChangeStructure()
    {
        const string expression = " 1  +\n  2   *\n3   -  \r\n 4   /  \n 2 ";
        
        var result = GetExpression(expression);

        var sub = IsValidBinary<Subtraction>(result);

        var add = IsValidBinary<Addition>(sub.Left);
        
        IsValidConstant(add.Left, value: 1);

        var mul = IsValidBinary<Multiplication>(add.Right);
        
        IsValidConstant(mul.Left, value: 2);
        
        IsValidConstant(mul.Right, value: 3);

        var div = IsValidBinary<Division>(sub.Right);
        
        IsValidConstant(div.Left, value: 4);
        
        IsValidConstant(div.Right, value: 2);
    }

    [Test]
    public void Whitespace_LongRuns_DoNotAffectParse()
    {
        const string expression = "2          ^          3";
        
        var result = GetExpression(expression);

        var pow = IsValidBinary<Power>(result);
        
        IsValidConstant(pow.Left, value: 2);
        
        IsValidConstant(pow.Right, value: 3);
    }
}
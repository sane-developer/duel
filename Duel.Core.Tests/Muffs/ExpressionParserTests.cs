using Duel.Core.Games.Muffs.AST;
using Duel.Core.Games.Muffs.Parsing;
using Duel.Core.Games.Muffs.Tokenizing;

namespace Duel.Core.Tests.Muffs;

file static class Utility
{
    public static void IsValidConstant(Expression expression, int value)
    {
        Assert.That(expression, Is.TypeOf<Constant>(), "Expected constant node.");
       
        var constant = (Constant) expression;
        
        Assert.That(constant.Value, Is.EqualTo(value));
    }

    public static Binary IsValidBinary<T>(Expression expression) where T : Binary
    {
        Assert.That(expression, Is.TypeOf<T>(), $"Expected {typeof(T).Name} node.");
        
        var binary = (Binary) expression;
        
        Assert.Multiple(() =>
        {
            Assert.That(binary.Left, Is.Not.Null, "Left node is null.");

            Assert.That(binary.Right, Is.Not.Null, "Right node is null.");
        });

        return binary;
    }
    
    public static Expression GetExpression(string text)
    {
        var tokenizer = new ExpressionTokenizer(text);

        var tokens = tokenizer.Tokenize();
        
        var parser = new ExpressionParser(tokens);

        return parser.Parse();
    }
}

public class ExpressionParserTests
{
    [Test]
    public void Constant()
    {
        //
        //  Assign
        //
        
        const string expression = "3";

        //
        //  Act
        //
        
        var result = Utility.GetExpression(expression);
        
        //
        //  Assert
        //
        
        Utility.IsValidConstant(result, value: 3);
    }

    [Test]
    public void SingleOperator_TwoConstants()
    {
        //
        //  Assign
        //
        
        const string expression = "3 + 4";

        //
        //  Act
        //
        
        var result = Utility.GetExpression(expression);
        
        //
        //  Assert
        //
        
        var binary = Utility.IsValidBinary<Addition>(result);
        
        Utility.IsValidConstant(binary.Left, value: 3);
        
        Utility.IsValidConstant(binary.Right, value: 4);
    }
}
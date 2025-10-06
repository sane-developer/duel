using Duel.Core.Games.Muffs.AST;
using Duel.Core.Games.Muffs.Parsing;
using Duel.Core.Games.Muffs.Tokenizing;

namespace Duel.Core.Tests.Muffs.Parsing;

public abstract class ExpressionParserTestSuite
{
    protected static void IsValidConstant(Expression expression, int value)
    {
        Assert.That(expression, Is.TypeOf<Constant>(), "Expected constant node.");
       
        var constant = (Constant) expression;
        
        Assert.That(constant.Value, Is.EqualTo(value));
    }

    protected static Binary IsValidBinary<T>(Expression expression) where T : Binary
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
    
    protected static Expression GetExpression(string text)
    {
        var tokenizer = new ExpressionTokenizer(text);

        var tokens = tokenizer.Tokenize();
        
        var parser = new ExpressionParser(tokens);

        return parser.Parse();
    }
}
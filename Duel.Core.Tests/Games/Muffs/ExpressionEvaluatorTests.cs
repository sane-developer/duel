using Duel.Core.Games.Muffs;

namespace Duel.Core.Tests.Games.Muffs;

public class ExpressionEvaluatorTests
{
    [Test]
    public void Leaf()
    {
        var expression = Expression.From(42);

        var result = ExpressionEvaluator.Evaluate(expression);
        
        Assert.That(result, Is.EqualTo(42));
    }

    [Test]
    public void LeavesWithOperator()
    {
        var expression = Expression.From(Operator.Subtract, 1, 2);
        
        var result = ExpressionEvaluator.Evaluate(expression);
        
        Assert.That(result, Is.EqualTo(-1));
    }
    
    [Test]
    public void NestedExpressions()
    {
        var oneSubtractTwo = Expression.From(Operator.Subtract, 1, 2);

        var negativeOneAddThree = Expression.From(Operator.Add, oneSubtractTwo, 3);

        var result = ExpressionEvaluator.Evaluate(negativeOneAddThree);
        
        Assert.That(result, Is.EqualTo(2));
    }
}
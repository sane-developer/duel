using Duel.Modules.Engine.Games.Muffs.Evaluating;
using Duel.Modules.Engine.Tests.Parsing;

namespace Duel.Modules.Engine.Tests.Evaluating;

public abstract class ExpressionEvaluatorTestSuite : ExpressionParserTestSuite
{
    private static int GetEvaluation(string text)
    {
        var expression = GetExpression(text);

        var evaluator = new ExpressionEvaluator(expression);
        
        return evaluator.Evaluate();
    }

    protected static void AssertEvaluation(string text, int expected)
    {
        var result = GetEvaluation(text);
        
        var message = GetAssertionMessage(text, expected, result);

        Assert.That(result, Is.EqualTo(expected), message);
    }

    private static string GetAssertionMessage(string text, int expected, int result)
    {
        return $"Expected evaluation of '{text}' to be {expected}, but got {result}.";
    }
}
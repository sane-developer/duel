namespace Duel.Core.Tests.Muffs.Parsing.Suites;

[TestFixture]
public sealed class LiteralsTestSuite : ExpressionParserTestSuite
{
    [Test]
    public void Constant_MultiDigit()
    {
        const string expression = "12345";
        
        var result = GetExpression(expression);
        
        IsValidConstant(result, value: 12345);
    }

    [Test]
    public void Constant_Zero()
    {
        const string expression = "0";
       
        var result = GetExpression(expression);
        
        IsValidConstant(result, value: 0);
    }

    [Test]
    public void Constant_LeadingAndTrailingWhitespace()
    {
        const string expression = "   42   ";
        
        var result = GetExpression(expression);
        
        IsValidConstant(result, value: 42);
    }

    [Test]
    public void Constant_WithNewlinesAndTabs()
    {
        const string expression = "\n\t 77 \r\n";
        
        var result = GetExpression(expression);
        
        IsValidConstant(result, value: 77);
    }

    [Test]
    public void Constant_LargeValue()
    {
        const string expression = "2147483647";
        
        var result = GetExpression(expression);
        
        IsValidConstant(result, value: 2147483647);
    }
}
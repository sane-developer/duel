using Duel.Modules.Engine.Games.Muffs.Expressions.Symbols;

namespace Duel.Modules.Engine.Games.Muffs.Expressions.Settings;

internal sealed class ExpressionSettingsBuilder(ExpressionSettings settings)
{
    private readonly List<Expression.Operator> _operators = [];
    
    public static ExpressionSettingsBuilder New()
    {
        var settings = new ExpressionSettings();
        
        return new ExpressionSettingsBuilder(settings);
    }

    public static ExpressionSettingsBuilder From(ExpressionSettings settings)
    {
        return new ExpressionSettingsBuilder(settings);
    }

    public ExpressionSettingsBuilder WithOperator(Expression.Operator symbol)
    {
        _operators.Add(symbol);

        return this;
    }

    public ExpressionSettingsBuilder WithDepth(int minimum, int maximum)
    {
        settings.Depth = new Range(minimum, maximum);

        return this;
    }

    public ExpressionSettingsBuilder WithBudget(int minimum, int maximum)
    {
        settings.Budget = new Range(minimum, maximum);

        return this;
    }

    public ExpressionSettingsBuilder WithConstant(int minimum, int maximum)
    {
        settings.Constant = new Range(minimum, maximum);

        return this;
    }

    public ExpressionSettingsBuilder WithExponent(int minimum, int maximum)
    {
        settings.Exponent = new Range(minimum, maximum);

        return this;
    }

    public ExpressionSettings Build()
    {
        settings.Operators = [.. _operators.Distinct()];
        
        return settings;
    }
}
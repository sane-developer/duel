using Duel.Modules.Engine.Games.Muffs.Expressions.AST;

namespace Duel.Modules.Engine.Games.Muffs.Expressions.Settings;

internal sealed class SettingsBuilder(Settings settings)
{
    private readonly List<Expression.Operator> _operators = [];
    
    public static SettingsBuilder New()
    {
        var settings = new Settings();
        
        return new SettingsBuilder(settings);
    }

    public static SettingsBuilder From(Settings settings)
    {
        return new SettingsBuilder(settings);
    }

    public SettingsBuilder WithOperator(Expression.Operator type)
    {
        _operators.Add(type);

        return this;
    }

    public SettingsBuilder WithDepth(int minimum, int maximum)
    {
        settings.Depth = new Range(minimum, maximum);

        return this;
    }

    public SettingsBuilder WithBudget(int minimum, int maximum)
    {
        settings.Budget = new Range(minimum, maximum);

        return this;
    }

    public SettingsBuilder WithConstant(int minimum, int maximum)
    {
        settings.Constant = new Range(minimum, maximum);

        return this;
    }

    public SettingsBuilder WithExponent(int minimum, int maximum)
    {
        settings.Exponent = new Range(minimum, maximum);

        return this;
    }

    public Settings Build()
    {
        settings.Operators = [.. _operators.Distinct()];
        
        return settings;
    }
}
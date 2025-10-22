namespace Duel.Modules.Engine.Games.Muffs.Expressions.Settings;

internal static class ExpressionSettingsContextFactory
{
    public static ExpressionSettingsContext Easy()
    {
        return new ExpressionSettingsContext(ExpressionSettingsRegistry.Easy);
    }

    public static ExpressionSettingsContext Medium()
    {
        return new ExpressionSettingsContext(ExpressionSettingsRegistry.Medium);
    }

    public static ExpressionSettingsContext Hard()
    {
        return new ExpressionSettingsContext(ExpressionSettingsRegistry.Hard);
    }
}
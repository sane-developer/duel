using Duel.Modules.Engine.Muffs.Glyphs;

namespace Duel.Modules.Engine.Muffs.Generators;

public sealed class ExpressionGenerator(IExpressionPolicy policy)
{
    public Glyph Generate(Random rng)
    {
        _ = policy;
        
        return new Number(0);
    }
}
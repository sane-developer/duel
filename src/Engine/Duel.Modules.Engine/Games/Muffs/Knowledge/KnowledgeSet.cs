using Duel.Modules.Engine.Games.Muffs.Generation;

namespace Duel.Modules.Engine.Games.Muffs.Knowledge;

/// <summary>
/// Immutable set of all mathematical knowledge (compositions and numbers) for expression generation.
/// Built once and shared across multiple generators.
/// </summary>
public sealed class KnowledgeSet
{
    public CompositionsRegistry Compositions { get; }
    
    public NumbersRegistry Numbers { get; }

    public KnowledgeSet(GeneratorSettings settings)
    {
        Compositions = new CompositionsRegistry(settings);

        Numbers = new NumbersRegistry(Compositions);
    }
}
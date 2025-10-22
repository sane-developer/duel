namespace Duel.Modules.Engine.Games.Muffs.Expressions.Compositions;

public sealed class CompositionRegistryBuilder
{
    private List<Composition> _compositions = [];

    private CompositionRegistryBuilder(Range constant)
    {
        _compositions.AddRange(CompositionFactory.FromAdditions(constant));
        
        _compositions.AddRange(CompositionFactory.FromSubtractions(constant));
        
        _compositions.AddRange(CompositionFactory.FromMultiplications(constant));
        
        _compositions.AddRange(CompositionFactory.FromDivisions(constant));
        
        _compositions.AddRange(CompositionFactory.FromPowers(constant));
        
        _compositions.AddRange(CompositionFactory.FromModulos(constant));
    }

    public static CompositionRegistryBuilder From(Range constant)
    {
        return new CompositionRegistryBuilder(constant);
    }

    public CompositionRegistryBuilder Apply(ICompositionRule filter)
    {
        _compositions = [.. _compositions.Where(filter.Predicate)];

        return this;
    }

    public List<Composition> Build()
    {
        return _compositions;
    }
}
namespace Duel.Modules.Engine.Games.Muffs.Expressions.Compositions;

public sealed class CompositionSet
{
    private List<Composition> _compositions = [];

    private CompositionSet(Range constant)
    {
        _compositions.AddRange(CompositionFactory.FromAdditions(constant));
        
        _compositions.AddRange(CompositionFactory.FromSubtractions(constant));
        
        _compositions.AddRange(CompositionFactory.FromMultiplications(constant));
        
        _compositions.AddRange(CompositionFactory.FromDivisions(constant));
        
        _compositions.AddRange(CompositionFactory.FromPowers(constant));
        
        _compositions.AddRange(CompositionFactory.FromModulos(constant));
    }

    public static CompositionSet From(Range constant)
    {
        return new CompositionSet(constant);
    }

    public CompositionSet Apply(ICompositionFilter filter)
    {
        _compositions = [.. _compositions.Where(filter.Predicate)];

        return this;
    }

    public List<Composition> Build()
    {
        return _compositions;
    }
}
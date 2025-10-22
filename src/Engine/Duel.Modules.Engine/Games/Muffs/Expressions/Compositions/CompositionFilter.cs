namespace Duel.Modules.Engine.Games.Muffs.Expressions.Compositions;

public interface ICompositionFilter
{
    bool Predicate(Composition composition);
}
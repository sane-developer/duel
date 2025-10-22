namespace Duel.Modules.Engine.Games.Muffs.Expressions.Compositions;

public interface ICompositionRule
{
    bool Predicate(Composition composition);
}
namespace Duel.Modules.Engine.Games.Muffs.Expressions;

public abstract record Expression
{
    public T As<T>() where T : Expression
    {
        return (T) this;
    }
}
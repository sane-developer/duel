namespace Duel.Modules.Engine.Games.Muffs.AST;

public abstract record Expression
{
    public T As<T>() where T : Expression
    {
        return (T) this;
    }
}
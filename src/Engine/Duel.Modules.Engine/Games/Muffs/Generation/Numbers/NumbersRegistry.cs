using Duel.Modules.Engine.Games.Muffs.Representation;
using System.Collections.Frozen;

namespace Duel.Modules.Engine.Games.Muffs.Generation.Numbers;

public sealed class NumbersRegistry
{
    private readonly FrozenDictionary<int, Number> _numbers;

    public NumbersRegistry()
    {
        _numbers = Compile(minimum: -1000, maximum: 1000).ToFrozenDictionary();
    }

    public Number GetNumber(int value)
    {
        return _numbers[value];
    }

    private static Dictionary<int, Number> Compile(int minimum, int maximum)
    {
        return Enumerable.Range(minimum, maximum - minimum + 1).ToDictionary(n => n, n => new Number(n));
    }
}
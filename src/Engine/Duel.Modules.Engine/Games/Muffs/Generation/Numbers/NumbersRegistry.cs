using Duel.Modules.Engine.Games.Muffs.Representation;
using System.Collections.Frozen;

namespace Duel.Modules.Engine.Games.Muffs.Generation.Numbers;

public sealed class NumbersRegistry
{
    private const int Minimum = -1000;

    private const int Maximum = 1000;
    
    private readonly FrozenDictionary<int, Number> _numbers;

    public NumbersRegistry()
    {
        _numbers = Enumerable.Range(Minimum, Maximum - Minimum + 1)
            .ToDictionary(n => n, Number.From)
            .ToFrozenDictionary();
    }

    public Number GetNumber(int value)
    {
        return _numbers[value];
    }
}
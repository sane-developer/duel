using System.Diagnostics;

namespace Duel.Core.Shared;

public static class Situation
{
    public static T Unreachable<T>()
    {
        throw new UnreachableException();
    }
}
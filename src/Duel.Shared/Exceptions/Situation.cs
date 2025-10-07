using System.Diagnostics;

namespace Duel.Shared.Common;

public static class Situation
{
    public static T Unreachable<T>()
    {
        throw new UnreachableException();
    }
}
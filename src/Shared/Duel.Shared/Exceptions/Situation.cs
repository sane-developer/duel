using System.Diagnostics;

namespace Duel.Shared.Exceptions;

public static class Situation
{
    public static T Unreachable<T>()
    {
        throw new UnreachableException();
    }
}
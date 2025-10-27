using Duel.Modules.Engine.Games.Muffs.Representation;

namespace Duel.Modules.Engine.Games.Muffs.Knowledge;

public abstract record Composition(OperatorType Type, int Result);

public sealed record UnaryComposition(OperatorType Type, int Operand, int Result) : Composition(Type, Result);

public sealed record BinaryComposition(OperatorType Type, int Lhs, int Rhs, int Result) : Composition(Type, Result);


using Duel.Modules.Engine.Games.Muffs.Generation.Operators;

namespace Duel.Modules.Engine.Games.Muffs.Generation.Compositions;

public abstract record Composition(OperatorType Type, int Result);

public sealed record UnaryComposition(OperatorType Type, int Operand, int Result) : Composition(Type, Result);

public sealed record BinaryComposition(OperatorType Type, int Lhs, int Rhs, int Result) : Composition(Type, Result);
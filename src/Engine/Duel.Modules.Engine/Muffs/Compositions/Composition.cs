using Duel.Modules.Engine.Muffs.Glyphs;

namespace Duel.Modules.Engine.Muffs.Compositions;

public abstract record Composition(GlyphType OperatorType, int Result);

public sealed record BinaryComposition(GlyphType OperatorType, int Lhs, int Rhs, int Result) : Composition(OperatorType, Result);

public sealed record UnaryComposition(GlyphType OperatorType, int Operand, int Result) : Composition(OperatorType, Result);
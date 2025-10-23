namespace Duel.Modules.Engine.Games.Muffs.AST.Operators;

public abstract record Unary(Symbol Operand) : Operator;

public sealed record Negate(Symbol Operand) : Unary(Operand);

public sealed record Absolute(Symbol Operand) : Unary(Operand);

public sealed record SquareRoot(Symbol Operand) : Unary(Operand);

public sealed record Factorial(Symbol Operand) : Unary(Operand);
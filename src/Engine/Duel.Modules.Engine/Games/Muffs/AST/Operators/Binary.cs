namespace Duel.Modules.Engine.Games.Muffs.AST.Operators;

public abstract record Binary(Symbol Lhs, Symbol Rhs) : Operator;

public sealed record Add(Symbol Lhs, Symbol Rhs) : Binary(Lhs, Rhs);

public sealed record Subtract(Symbol Lhs, Symbol Rhs) : Binary(Lhs, Rhs);

public sealed record Multiply(Symbol Lhs, Symbol Rhs) : Binary(Lhs, Rhs);

public sealed record Divide(Symbol Lhs, Symbol Rhs) : Binary(Lhs, Rhs);

public sealed record Modulo(Symbol Lhs, Symbol Rhs) : Binary(Lhs, Rhs);

public sealed record Power(Symbol Lhs, Symbol Rhs) : Binary(Lhs, Rhs);
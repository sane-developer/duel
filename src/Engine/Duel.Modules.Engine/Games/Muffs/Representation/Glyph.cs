namespace Duel.Modules.Engine.Games.Muffs.Representation;

public abstract record Glyph;

public abstract record Literal : Glyph;

public abstract record Operator : Glyph;
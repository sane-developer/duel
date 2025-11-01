namespace Duel.Modules.Engine.Games.Muffs.Generation.Difficulties;

public static class DifficultyRegistry
{
    public static readonly Difficulty Easy = DifficultyBuilder.New()
        .WithDepth(1..3)
        .WithLength(1..5)
        .WithAdditions(weight: 1.0, operands: 1..10)
        .WithSubtractions(weight: 1.0, operands: 1..10)
        .WithMultiplications(weight: 1.0, operands: 1..10)
        .Build();

    public static readonly Difficulty Medium = DifficultyBuilder.New()
        .WithDepth(2..4)
        .WithLength(5..8)
        .WithAdditions(weight: 1.0, operands: new IntegerRange(-10, 20))
        .WithSubtractions(weight: 1.0, operands: new IntegerRange(-10, 20))
        .WithMultiplications(weight: 1.0, operands: 1..12)
        .WithDivisions(weight: 1.0, operands: 1..50)
        .WithModulos(weight: 0.5, operands: 1..20)
        .WithPowers(weight: 0.5, operands: 1..5)
        .Build();

    public static readonly Difficulty Hard = DifficultyBuilder.New()
        .WithDepth(3..5)
        .WithLength(8..10)
        .WithAdditions(weight: 1.0, operands: new IntegerRange(-20, 30))
        .WithSubtractions(weight: 1.0, operands: new IntegerRange(-20, 30))
        .WithMultiplications(weight: 1.0, operands: 1..15)
        .WithDivisions(weight: 1.0, operands: 1..100)
        .WithModulos(weight: 0.8, operands: 1..30)
        .WithPowers(weight: 0.5, operands: 1..5)
        .WithNegations(weight: 0.8, operands: new IntegerRange(-50, 50))
        .WithAbsolute(weight: 0.8, operands: new IntegerRange(-50, 0))
        .WithFactorials(weight: 0.3, operands: 0..5)
        .WithSquareRoots(weight: 0.3, operands: 0..100)
        .Build();
}
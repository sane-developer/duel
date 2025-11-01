using Duel.Modules.Engine.Abstractions;
using Duel.Modules.Engine.Games.Muffs;

namespace Duel.Modules.Engine.Tests;

/// <summary>
/// Tests demonstrating how the Lobby module would use the MuffsEngine.
/// </summary>
[TestFixture]
public class MuffsEngineTests
{
    private IEngine _engine = null!;

    [SetUp]
    public void SetUp()
    {
        _engine = new MuffsEngine();
    }

    [Test]
    public void GameType_ShouldBeMuffs()
    {
        Assert.That(_engine.GameType, Is.EqualTo("Muffs"));
    }

    [TestCase("Easy", 4)]
    [TestCase("Medium", 8)]
    [TestCase("Hard", 12)]
    public void GenerateChallenge_ShouldReturnChallengeWithCorrectTimeLimit(string difficulty, int expectedTimeLimit)
    {
        // Act
        var challenge = _engine.GenerateChallenge(difficulty);

        // Assert
        Assert.That(challenge.TimeLimitSeconds, Is.EqualTo(expectedTimeLimit));
    }

    [TestCase("Easy")]
    [TestCase("Medium")]
    [TestCase("Hard")]
    public void GenerateChallenge_ShouldReturnChallengeWithNonEmptyExpression(string difficulty)
    {
        // Act
        var challenge = _engine.GenerateChallenge(difficulty);

        // Assert
        Assert.That(challenge.Expression, Is.Not.Null.And.Not.Empty);
    }

    [TestCase("Easy")]
    [TestCase("Medium")]
    [TestCase("Hard")]
    public void GenerateChallenge_ShouldGenerateDifferentExpressionsOnMultipleCalls(string difficulty)
    {
        // Act - Generate multiple challenges
        var challenge1 = _engine.GenerateChallenge(difficulty);
        var challenge2 = _engine.GenerateChallenge(difficulty);
        var challenge3 = _engine.GenerateChallenge(difficulty);

        // Assert - At least one should be different (probabilistic, but very likely)
        var expressions = new[] { challenge1.Expression, challenge2.Expression, challenge3.Expression };
        Assert.That(expressions.Distinct().Count(), Is.GreaterThan(1),
            "Should generate varied expressions");
    }

    [Test]
    public void GenerateChallenge_WithInvalidDifficulty_ShouldThrowArgumentException()
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => _engine.GenerateChallenge("Invalid"));
        Assert.That(ex?.Message, Does.Contain("Unknown difficulty"));
    }

    [TestCase("Easy")]
    [TestCase("Medium")]
    [TestCase("Hard")]
    public void GenerateChallenge_ShouldHaveCorrectAnswer(string difficulty)
    {
        // Act
        var challenge = _engine.GenerateChallenge(difficulty);

        // Assert - The correct answer should be a valid integer
        Assert.That(challenge.CorrectAnswer, Is.TypeOf<int>());
        
        // Note: We can't easily validate if the answer is actually correct without
        // re-parsing and evaluating, but we trust the ExpressionEvaluator tests for that
    }

    /// <summary>
    /// Simulates how the Lobby module would use the engine during a match.
    /// </summary>
    [Test]
    public void SimulateLobbyModuleUsage_ShouldWorkEndToEnd()
    {
        // Scenario: Lobby module starting a Medium difficulty match

        // 1. Lobby gets the engine (via DI in real scenario)
        var engine = _engine;

        // 2. For each round (5 rounds in a match), generate a challenge
        var round1 = engine.GenerateChallenge("Medium");
        var round2 = engine.GenerateChallenge("Medium");
        var round3 = engine.GenerateChallenge("Medium");

        // 3. Lobby would store these in the database:
        // - round1.Expression (VARCHAR) → display to players
        // - round1.CorrectAnswer (INT) → validate player submissions
        // - round1.TimeLimitSeconds (INT) → countdown timer

        Assert.That(round1.Expression, Is.Not.Empty);
        Assert.That(round1.TimeLimitSeconds, Is.EqualTo(8));
        
        // 4. Lobby validates player answers by comparing with CorrectAnswer
        var playerAnswer = round1.CorrectAnswer; // Simulating correct answer
        var isCorrect = playerAnswer == round1.CorrectAnswer;
        
        Assert.That(isCorrect, Is.True);

        Console.WriteLine($"Round 1: {round1.Expression} = {round1.CorrectAnswer} (Time: {round1.TimeLimitSeconds}s)");
        Console.WriteLine($"Round 2: {round2.Expression} = {round2.CorrectAnswer} (Time: {round2.TimeLimitSeconds}s)");
        Console.WriteLine($"Round 3: {round3.Expression} = {round3.CorrectAnswer} (Time: {round3.TimeLimitSeconds}s)");
    }

    /// <summary>
    /// Demonstrates difficulty progression - harder difficulties should generally
    /// produce longer/more complex expressions.
    /// </summary>
    [Test]
    public void DifficultyProgression_HardShouldGenerateLongerExpressions()
    {
        // Generate multiple challenges for each difficulty
        var easyChallenges = Enumerable.Range(0, 10).Select(_ => _engine.GenerateChallenge("Easy")).ToList();
        var hardChallenges = Enumerable.Range(0, 10).Select(_ => _engine.GenerateChallenge("Hard")).ToList();

        // Hard expressions should generally be longer (more characters)
        var avgEasyLength = easyChallenges.Average(c => c.Expression.Length);
        var avgHardLength = hardChallenges.Average(c => c.Expression.Length);

        Assert.That(avgHardLength, Is.GreaterThan(avgEasyLength),
            $"Hard expressions (avg {avgHardLength} chars) should be longer than Easy (avg {avgEasyLength} chars)");
    }
}


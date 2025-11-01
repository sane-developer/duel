namespace Duel.Modules.Engine.Abstractions;

/// <summary>
/// Abstraction for game engines that generate challenges for matches.
/// </summary>
public interface IEngine
{
    /// <summary>
    /// Gets the unique identifier for this game type (e.g., "Muffs", "Blackjack").
    /// </summary>
    string GameType { get; }

    /// <summary>
    /// Generates a new challenge based on the specified difficulty level.
    /// </summary>
    /// <param name="difficulty">The difficulty level (e.g., "Easy", "Medium", "Hard").</param>
    /// <returns>A challenge containing the problem, correct answer, and time limit.</returns>
    Challenge GenerateChallenge(string difficulty);
}

/// <summary>
/// Represents a game challenge/round that players must solve.
/// </summary>
/// <param name="Expression">The problem to display to players (e.g., "(2 + 3) * 4").</param>
/// <param name="CorrectAnswer">The correct answer to the challenge.</param>
/// <param name="TimeLimitSeconds">How many seconds players have to answer.</param>
public sealed record Challenge(
    string Expression,
    int CorrectAnswer,
    int TimeLimitSeconds
);
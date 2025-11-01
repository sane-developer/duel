namespace Duel.Modules.Lobby.Domain.Entities;

/// <summary>
/// Represents a game match between two players.
/// </summary>
public sealed class Match
{
    public Guid Id { get; init; }
    
    public Guid LobbyId { get; init; }
    
    public Guid Player1Id { get; init; }
    
    public Guid Player2Id { get; init; }
    
    public Guid? WinnerId { get; set; }
    
    public MatchStatus Status { get; set; }
    
    public int CurrentRound { get; set; }
    
    public int Player1Score { get; set; }
    
    public int Player2Score { get; set; }
    
    public DateTime CreatedAt { get; init; }
    
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// Maximum number of rounds in a match (Best of 5).
    /// </summary>
    public const int MaxRounds = 5;

    /// <summary>
    /// Rounds needed to win the match.
    /// </summary>
    public const int RoundsToWin = 3;

    /// <summary>
    /// Checks if the match has a winner.
    /// </summary>
    public bool HasWinner => Player1Score >= RoundsToWin || Player2Score >= RoundsToWin;

    /// <summary>
    /// Checks if the match is tied after all rounds.
    /// </summary>
    public bool IsTied => CurrentRound > MaxRounds && Player1Score == Player2Score;

    /// <summary>
    /// Checks if all rounds have been played.
    /// </summary>
    public bool IsComplete => CurrentRound > MaxRounds || HasWinner;

    /// <summary>
    /// Gets the current winner if there is one.
    /// </summary>
    public Guid? GetCurrentWinner()
    {
        if (Player1Score >= RoundsToWin) return Player1Id;
        if (Player2Score >= RoundsToWin) return Player2Id;
        return null;
    }
}


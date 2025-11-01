namespace Duel.Modules.Lobby.Domain.Entities;

/// <summary>
/// Represents a single round within a match.
/// </summary>
public sealed class Round
{
    public Guid Id { get; init; }
    
    public Guid MatchId { get; init; }
    
    public int RoundNumber { get; init; }
    
    public string Expression { get; init; } = null!;
    
    public int CorrectAnswer { get; init; }
    
    public int TimeLimitSeconds { get; init; }
    
    public Guid? WinnerId { get; set; }
    
    public int? Player1Answer { get; set; }
    
    public int? Player2Answer { get; set; }
    
    public int? Player1ResponseTimeMs { get; set; }
    
    public int? Player2ResponseTimeMs { get; set; }
    
    public DateTime CreatedAt { get; init; }
    
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// Checks if the round is complete (has a winner or time expired).
    /// </summary>
    public bool IsComplete => WinnerId.HasValue || CompletedAt.HasValue;

    /// <summary>
    /// Checks if both players have submitted answers.
    /// </summary>
    public bool BothAnswered => Player1Answer.HasValue && Player2Answer.HasValue;
}


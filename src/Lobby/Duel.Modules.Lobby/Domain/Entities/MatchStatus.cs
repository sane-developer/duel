namespace Duel.Modules.Lobby.Domain.Entities;

/// <summary>
/// Represents the current state of a match.
/// </summary>
public enum MatchStatus
{
    /// <summary>
    /// Match is currently in progress.
    /// </summary>
    InProgress,
    
    /// <summary>
    /// Match has been completed.
    /// </summary>
    Completed,
    
    /// <summary>
    /// Match was abandoned (e.g., player disconnected).
    /// </summary>
    Abandoned
}


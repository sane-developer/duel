namespace Duel.Modules.Lobby.Domain.Entities;

/// <summary>
/// Represents the current state of a lobby.
/// </summary>
public enum LobbyStatus
{
    /// <summary>
    /// Lobby is waiting for players to join or ready up.
    /// </summary>
    Waiting,
    
    /// <summary>
    /// Both players are ready, match is about to start.
    /// </summary>
    Ready,
    
    /// <summary>
    /// Match is currently in progress.
    /// </summary>
    InProgress,
    
    /// <summary>
    /// Match has been completed.
    /// </summary>
    Completed,
    
    /// <summary>
    /// Lobby was abandoned (e.g., player disconnected before starting).
    /// </summary>
    Abandoned
}


namespace Duel.Modules.Lobby.Domain.Entities;

/// <summary>
/// Represents a game lobby where players wait before starting a match.
/// </summary>
public sealed class Lobby
{
    public Guid Id { get; init; }
    
    public Guid HostId { get; init; }
    
    public Guid? GuestId { get; set; }
    
    public string Difficulty { get; init; } = null!;
    
    public LobbyStatus Status { get; set; }
    
    public bool HostReady { get; set; }
    
    public bool GuestReady { get; set; }
    
    public DateTime CreatedAt { get; init; }
    
    public DateTime? StartedAt { get; set; }
    
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// Checks if the lobby has both players.
    /// </summary>
    public bool IsFull => GuestId.HasValue;

    /// <summary>
    /// Checks if both players are ready to start.
    /// </summary>
    public bool AreBothReady => HostReady && GuestReady && IsFull;

    /// <summary>
    /// Checks if a user is a participant in this lobby.
    /// </summary>
    public bool IsParticipant(Guid userId) => userId == HostId || userId == GuestId;

    /// <summary>
    /// Checks if a user is the host.
    /// </summary>
    public bool IsHost(Guid userId) => userId == HostId;
}


using Duel.Modules.Lobby.Domain.Entities;

namespace Duel.Modules.Lobby.Application.DTOs;

/// <summary>
/// DTO for lobby information sent to clients.
/// </summary>
public sealed record LobbyDto(
    Guid Id,
    Guid HostId,
    Guid? GuestId,
    string Difficulty,
    LobbyStatus Status,
    bool HostReady,
    bool GuestReady,
    DateTime CreatedAt
);


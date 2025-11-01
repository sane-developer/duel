using Duel.Modules.Lobby.Domain.Entities;

namespace Duel.Modules.Lobby.Application.DTOs;

/// <summary>
/// DTO for match information sent to clients.
/// </summary>
public sealed record MatchDto(
    Guid Id,
    Guid Player1Id,
    Guid Player2Id,
    int CurrentRound,
    int Player1Score,
    int Player2Score,
    MatchStatus Status
);


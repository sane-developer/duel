using Duel.Modules.Lobby.Application.DTOs;

namespace Duel.Modules.Lobby.Application.Services;

/// <summary>
/// Service for match orchestration and round progression.
/// </summary>
public interface IMatchService
{
    Task<MatchDto> StartMatchAsync(Guid lobbyId, CancellationToken cancellationToken = default);
    
    Task<RoundDto> StartRoundAsync(Guid matchId, CancellationToken cancellationToken = default);
    
    Task SubmitAnswerAsync(Guid roundId, Guid playerId, int answer, int responseTimeMs, CancellationToken cancellationToken = default);
    
    Task<MatchDto?> GetMatchAsync(Guid matchId, CancellationToken cancellationToken = default);
}


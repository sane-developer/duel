using Duel.Modules.Lobby.Domain.Entities;

namespace Duel.Modules.Lobby.Application.Repositories;

/// <summary>
/// Repository interface for Round entity operations.
/// </summary>
public interface IRoundRepository
{
    Task<Round?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<Round>> GetByMatchIdAsync(Guid matchId, CancellationToken cancellationToken = default);
    
    Task<Round?> GetCurrentRoundAsync(Guid matchId, CancellationToken cancellationToken = default);
    
    Task AddAsync(Round round, CancellationToken cancellationToken = default);
    
    Task UpdateAsync(Round round, CancellationToken cancellationToken = default);
}


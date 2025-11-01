using Duel.Modules.Lobby.Domain.Entities;

namespace Duel.Modules.Lobby.Application.Repositories;

/// <summary>
/// Repository interface for Match entity operations.
/// </summary>
public interface IMatchRepository
{
    Task<Match?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<Match?> GetByLobbyIdAsync(Guid lobbyId, CancellationToken cancellationToken = default);
    
    Task AddAsync(Match match, CancellationToken cancellationToken = default);
    
    Task UpdateAsync(Match match, CancellationToken cancellationToken = default);
}


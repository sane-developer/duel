using Duel.Modules.Lobby.Domain.Entities;

namespace Duel.Modules.Lobby.Application.Repositories;

/// <summary>
/// Repository interface for Lobby entity operations.
/// </summary>
public interface ILobbyRepository
{
    Task<Domain.Entities.Lobby?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<Domain.Entities.Lobby>> GetAllAsync(CancellationToken cancellationToken = default);
    
    Task<IEnumerable<Domain.Entities.Lobby>> GetAvailableLobbiesAsync(CancellationToken cancellationToken = default);
    
    Task<Domain.Entities.Lobby?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    
    Task AddAsync(Domain.Entities.Lobby lobby, CancellationToken cancellationToken = default);
    
    Task UpdateAsync(Domain.Entities.Lobby lobby, CancellationToken cancellationToken = default);
    
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}


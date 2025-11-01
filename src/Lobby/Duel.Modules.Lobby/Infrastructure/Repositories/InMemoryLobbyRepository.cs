using Duel.Modules.Lobby.Application.Repositories;
using Duel.Modules.Lobby.Domain.Entities;
using System.Collections.Concurrent;

namespace Duel.Modules.Lobby.Infrastructure.Repositories;

/// <summary>
/// In-memory implementation of ILobbyRepository for testing/development.
/// </summary>
public sealed class InMemoryLobbyRepository : ILobbyRepository
{
    private readonly ConcurrentDictionary<Guid, Domain.Entities.Lobby> _lobbies = new();

    public Task<Domain.Entities.Lobby?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _lobbies.TryGetValue(id, out var lobby);
        return Task.FromResult(lobby);
    }

    public Task<IEnumerable<Domain.Entities.Lobby>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IEnumerable<Domain.Entities.Lobby>>(_lobbies.Values);
    }

    public Task<IEnumerable<Domain.Entities.Lobby>> GetAvailableLobbiesAsync(CancellationToken cancellationToken = default)
    {
        var available = _lobbies.Values
            .Where(l => l.Status == LobbyStatus.Waiting && !l.IsFull)
            .OrderByDescending(l => l.CreatedAt);

        return Task.FromResult<IEnumerable<Domain.Entities.Lobby>>(available);
    }

    public Task<Domain.Entities.Lobby?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var lobby = _lobbies.Values
            .FirstOrDefault(l => l.IsParticipant(userId) && l.Status != LobbyStatus.Completed && l.Status != LobbyStatus.Abandoned);

        return Task.FromResult(lobby);
    }

    public Task AddAsync(Domain.Entities.Lobby lobby, CancellationToken cancellationToken = default)
    {
        _lobbies.TryAdd(lobby.Id, lobby);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Domain.Entities.Lobby lobby, CancellationToken cancellationToken = default)
    {
        _lobbies[lobby.Id] = lobby;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _lobbies.TryRemove(id, out _);
        return Task.CompletedTask;
    }
}


using Duel.Modules.Lobby.Application.Repositories;
using Duel.Modules.Lobby.Domain.Entities;
using System.Collections.Concurrent;

namespace Duel.Modules.Lobby.Infrastructure.Repositories;

/// <summary>
/// In-memory implementation of IMatchRepository for testing/development.
/// </summary>
public sealed class InMemoryMatchRepository : IMatchRepository
{
    private readonly ConcurrentDictionary<Guid, Match> _matches = new();

    public Task<Match?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _matches.TryGetValue(id, out var match);
        return Task.FromResult(match);
    }

    public Task<Match?> GetByLobbyIdAsync(Guid lobbyId, CancellationToken cancellationToken = default)
    {
        var match = _matches.Values.FirstOrDefault(m => m.LobbyId == lobbyId);
        return Task.FromResult(match);
    }

    public Task AddAsync(Match match, CancellationToken cancellationToken = default)
    {
        _matches.TryAdd(match.Id, match);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Match match, CancellationToken cancellationToken = default)
    {
        _matches[match.Id] = match;
        return Task.CompletedTask;
    }
}


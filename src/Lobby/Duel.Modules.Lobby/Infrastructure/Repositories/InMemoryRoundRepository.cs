using Duel.Modules.Lobby.Application.Repositories;
using Duel.Modules.Lobby.Domain.Entities;
using System.Collections.Concurrent;

namespace Duel.Modules.Lobby.Infrastructure.Repositories;

/// <summary>
/// In-memory implementation of IRoundRepository for testing/development.
/// </summary>
public sealed class InMemoryRoundRepository : IRoundRepository
{
    private readonly ConcurrentDictionary<Guid, Round> _rounds = new();

    public Task<Round?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _rounds.TryGetValue(id, out var round);
        return Task.FromResult(round);
    }

    public Task<IEnumerable<Round>> GetByMatchIdAsync(Guid matchId, CancellationToken cancellationToken = default)
    {
        var rounds = _rounds.Values
            .Where(r => r.MatchId == matchId)
            .OrderBy(r => r.RoundNumber);

        return Task.FromResult<IEnumerable<Round>>(rounds);
    }

    public Task<Round?> GetCurrentRoundAsync(Guid matchId, CancellationToken cancellationToken = default)
    {
        var round = _rounds.Values
            .Where(r => r.MatchId == matchId && !r.IsComplete)
            .OrderByDescending(r => r.RoundNumber)
            .FirstOrDefault();

        return Task.FromResult(round);
    }

    public Task AddAsync(Round round, CancellationToken cancellationToken = default)
    {
        _rounds.TryAdd(round.Id, round);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Round round, CancellationToken cancellationToken = default)
    {
        _rounds[round.Id] = round;
        return Task.CompletedTask;
    }
}


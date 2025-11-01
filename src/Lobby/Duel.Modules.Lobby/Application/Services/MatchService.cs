using Duel.Modules.Engine.Abstractions;
using Duel.Modules.Lobby.Application.DTOs;
using Duel.Modules.Lobby.Application.Repositories;
using Duel.Modules.Lobby.Domain.Entities;

namespace Duel.Modules.Lobby.Application.Services;

/// <summary>
/// Service implementation for match orchestration.
/// </summary>
public sealed class MatchService(IEngine engine, ILobbyRepository lobbyRepository, IMatchRepository matchRepository, IRoundRepository roundRepository) : IMatchService
{
    public async Task<MatchDto> StartMatchAsync(Guid lobbyId, CancellationToken cancellationToken = default)
    {
        var lobby = await lobbyRepository.GetByIdAsync(lobbyId, cancellationToken);
        if (lobby is null)
        {
            throw new InvalidOperationException("Lobby not found.");
        }

        if (!lobby.AreBothReady)
        {
            throw new InvalidOperationException("Both players must be ready to start match.");
        }

        if (lobby.Status != LobbyStatus.Ready)
        {
            throw new InvalidOperationException("Lobby is not ready to start match.");
        }

        // Create match
        var match = new Match
        {
            Id = Guid.NewGuid(),
            LobbyId = lobbyId,
            Player1Id = lobby.HostId,
            Player2Id = lobby.GuestId!.Value,
            Status = MatchStatus.InProgress,
            CurrentRound = 0,
            CreatedAt = DateTime.UtcNow
        };

        await matchRepository.AddAsync(match, cancellationToken);

        // Update lobby status
        lobby.Status = LobbyStatus.InProgress;
        lobby.StartedAt = DateTime.UtcNow;
        await lobbyRepository.UpdateAsync(lobby, cancellationToken);

        return MapToDto(match);
    }

    public async Task<RoundDto> StartRoundAsync(Guid matchId, CancellationToken cancellationToken = default)
    {
        var match = await matchRepository.GetByIdAsync(matchId, cancellationToken);
        if (match is null)
        {
            throw new InvalidOperationException("Match not found.");
        }

        if (match.Status != MatchStatus.InProgress)
        {
            throw new InvalidOperationException("Match is not in progress.");
        }

        if (match.IsComplete)
        {
            throw new InvalidOperationException("Match is already complete.");
        }

        // Get lobby to retrieve difficulty
        var lobby = await lobbyRepository.GetByIdAsync(match.LobbyId, cancellationToken);
        if (lobby is null)
        {
            throw new InvalidOperationException("Lobby not found.");
        }

        // Generate challenge using the engine
        var challenge = engine.GenerateChallenge(lobby.Difficulty);

        // Increment round number
        match.CurrentRound++;

        // Create round
        var round = new Round
        {
            Id = Guid.NewGuid(),
            MatchId = matchId,
            RoundNumber = match.CurrentRound,
            Expression = challenge.Expression,
            CorrectAnswer = challenge.CorrectAnswer,
            TimeLimitSeconds = challenge.TimeLimitSeconds,
            CreatedAt = DateTime.UtcNow
        };

        await roundRepository.AddAsync(round, cancellationToken);
        await matchRepository.UpdateAsync(match, cancellationToken);

        return new RoundDto(
            round.Id,
            round.RoundNumber,
            round.Expression,
            round.TimeLimitSeconds
        );
    }

    public async Task SubmitAnswerAsync(Guid roundId, Guid playerId, int answer, int responseTimeMs, CancellationToken cancellationToken = default)
    {
        var round = await roundRepository.GetByIdAsync(roundId, cancellationToken);
        if (round is null)
        {
            throw new InvalidOperationException("Round not found.");
        }

        var match = await matchRepository.GetByIdAsync(round.MatchId, cancellationToken);
        if (match is null)
        {
            throw new InvalidOperationException("Match not found.");
        }

        // Determine which player submitted
        var isPlayer1 = playerId == match.Player1Id;
        var isPlayer2 = playerId == match.Player2Id;

        if (!isPlayer1 && !isPlayer2)
        {
            throw new InvalidOperationException("Player is not in this match.");
        }

        // Store answer and response time
        if (isPlayer1)
        {
            if (round.Player1Answer.HasValue)
            {
                throw new InvalidOperationException("Player has already submitted an answer.");
            }
            round.Player1Answer = answer;
            round.Player1ResponseTimeMs = responseTimeMs;
        }
        else
        {
            if (round.Player2Answer.HasValue)
            {
                throw new InvalidOperationException("Player has already submitted an answer.");
            }
            round.Player2Answer = answer;
            round.Player2ResponseTimeMs = responseTimeMs;
        }

        // Check if this is the winning answer
        var isCorrect = answer == round.CorrectAnswer;

        if (isCorrect && !round.WinnerId.HasValue)
        {
            // First correct answer wins
            round.WinnerId = playerId;
            round.CompletedAt = DateTime.UtcNow;

            // Update match score
            if (isPlayer1)
            {
                match.Player1Score++;
            }
            else
            {
                match.Player2Score++;
            }

            // Check if match is complete
            if (match.HasWinner)
            {
                match.WinnerId = match.GetCurrentWinner();
                match.Status = MatchStatus.Completed;
                match.CompletedAt = DateTime.UtcNow;

                // Update lobby
                var lobby = await lobbyRepository.GetByIdAsync(match.LobbyId, cancellationToken);
                if (lobby is not null)
                {
                    lobby.Status = LobbyStatus.Completed;
                    lobby.CompletedAt = DateTime.UtcNow;
                    await lobbyRepository.UpdateAsync(lobby, cancellationToken);
                }
            }

            await matchRepository.UpdateAsync(match, cancellationToken);
        }

        await roundRepository.UpdateAsync(round, cancellationToken);
    }

    public async Task<MatchDto?> GetMatchAsync(Guid matchId, CancellationToken cancellationToken = default)
    {
        var match = await matchRepository.GetByIdAsync(matchId, cancellationToken);
        return match is not null ? MapToDto(match) : null;
    }

    private static MatchDto MapToDto(Match match)
    {
        return new MatchDto(
            match.Id,
            match.Player1Id,
            match.Player2Id,
            match.CurrentRound,
            match.Player1Score,
            match.Player2Score,
            match.Status
        );
    }
}


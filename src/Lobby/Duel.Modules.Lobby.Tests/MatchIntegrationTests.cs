using Duel.Modules.Engine.Abstractions;
using Duel.Modules.Engine.Games.Muffs;
using Duel.Modules.Lobby.Application.DTOs;
using Duel.Modules.Lobby.Application.Services;
using Duel.Modules.Lobby.Domain.Entities;
using Duel.Modules.Lobby.Infrastructure.Repositories;

namespace Duel.Modules.Lobby.Tests;

/// <summary>
/// End-to-end integration tests demonstrating full game flow:
/// Create Lobby → Join → Ready Up → Start Match → Play Rounds → Complete Match
/// </summary>
[TestFixture]
public class MatchIntegrationTests
{
    private IEngine _engine = null!;
    private LobbyService _lobbyService = null!;
    private MatchService _matchService = null!;
    private InMemoryRoundRepository _roundRepository = null!;

    [SetUp]
    public void SetUp()
    {
        // Setup dependencies
        _engine = new MuffsEngine();
        
        var lobbyRepository = new InMemoryLobbyRepository();
        var matchRepository = new InMemoryMatchRepository();
        _roundRepository = new InMemoryRoundRepository();

        _lobbyService = new LobbyService(lobbyRepository);
        _matchService = new MatchService(_engine, lobbyRepository, matchRepository, _roundRepository);
    }

    [Test]
    public async Task FullMatchFlow_ShouldCompleteSuccessfully()
    {
        // ========== Phase 1: Lobby Creation ==========
        var player1Id = Guid.NewGuid();
        var player2Id = Guid.NewGuid();

        var lobby = await _lobbyService.CreateLobbyAsync(player1Id, new CreateLobbyRequest("Medium"));
        
        Assert.That(lobby.HostId, Is.EqualTo(player1Id));
        Assert.That(lobby.Difficulty, Is.EqualTo("Medium"));
        Console.WriteLine($"✓ Lobby created: {lobby.Id}");

        // ========== Phase 2: Player 2 Joins ==========
        await _lobbyService.JoinLobbyAsync(lobby.Id, player2Id);
        
        lobby = (await _lobbyService.GetLobbyAsync(lobby.Id))!;
        Assert.That(lobby.GuestId, Is.EqualTo(player2Id));
        Console.WriteLine($"✓ Player 2 joined: {player2Id}");

        // ========== Phase 3: Both Players Ready Up ==========
        await _lobbyService.SetReadyAsync(lobby.Id, player1Id, true);
        await _lobbyService.SetReadyAsync(lobby.Id, player2Id, true);
        
        lobby = (await _lobbyService.GetLobbyAsync(lobby.Id))!;
        Assert.That(lobby.Status, Is.EqualTo(LobbyStatus.Ready));
        Console.WriteLine("✓ Both players ready");

        // ========== Phase 4: Start Match ==========
        var match = await _matchService.StartMatchAsync(lobby.Id);
        
        Assert.That(match.Player1Id, Is.EqualTo(player1Id));
        Assert.That(match.Player2Id, Is.EqualTo(player2Id));
        Assert.That(match.Status, Is.EqualTo(MatchStatus.InProgress));
        Console.WriteLine($"✓ Match started: {match.Id}");

        // ========== Phase 5: Play Rounds (Best of 5) ==========
        for (int roundNum = 1; roundNum <= Match.MaxRounds && match.Status == MatchStatus.InProgress; roundNum++)
        {
            // Start round (engine generates challenge)
            var round = await _matchService.StartRoundAsync(match.Id);
            
            Assert.That(round.RoundNumber, Is.EqualTo(roundNum));
            Assert.That(round.Expression, Is.Not.Empty);
            Assert.That(round.TimeLimitSeconds, Is.EqualTo(8)); // Medium = 8s
            
            Console.WriteLine($"\n  Round {roundNum}:");
            Console.WriteLine($"    Expression: {round.Expression}");
            Console.WriteLine($"    Time Limit: {round.TimeLimitSeconds}s");

            // Fetch the round to get correct answer
            var storedRound = await _roundRepository.GetByIdAsync(round.Id);
            Assert.That(storedRound, Is.Not.Null);

            // Simulate: Player 1 submits correct answer first
            await _matchService.SubmitAnswerAsync(
                round.Id,
                player1Id,
                storedRound!.CorrectAnswer,
                responseTimeMs: 2500
            );

            Console.WriteLine($"    Player 1 answered correctly! ({storedRound.CorrectAnswer})");

            // Check updated scores
            match = (await _matchService.GetMatchAsync(match.Id))!;
            Console.WriteLine($"    Score: Player 1: {match.Player1Score} | Player 2: {match.Player2Score}");

            // If someone has won 3 rounds, match should be complete
            if (match.Player1Score >= Match.RoundsToWin || match.Player2Score >= Match.RoundsToWin)
            {
                Assert.That(match.Status, Is.EqualTo(MatchStatus.Completed));
                break;
            }
        }

        // ========== Phase 6: Verify Match Completion ==========
        Assert.That(match.Status, Is.EqualTo(MatchStatus.Completed));
        Assert.That(match.Player1Score, Is.GreaterThanOrEqualTo(Match.RoundsToWin));
        
        Console.WriteLine($"\n✓ Match completed!");
        Console.WriteLine($"  Winner: Player 1");
        Console.WriteLine($"  Final Score: {match.Player1Score}-{match.Player2Score}");

        // Verify lobby is also marked complete
        lobby = (await _lobbyService.GetLobbyAsync(lobby.Id))!;
        Assert.That(lobby.Status, Is.EqualTo(LobbyStatus.Completed));
    }

    [Test]
    public async Task Round_BothWrongAnswers_NoWinner()
    {
        // Setup
        var player1Id = Guid.NewGuid();
        var player2Id = Guid.NewGuid();

        var lobby = await _lobbyService.CreateLobbyAsync(player1Id, new CreateLobbyRequest("Easy"));
        await _lobbyService.JoinLobbyAsync(lobby.Id, player2Id);
        await _lobbyService.SetReadyAsync(lobby.Id, player1Id, true);
        await _lobbyService.SetReadyAsync(lobby.Id, player2Id, true);

        var match = await _matchService.StartMatchAsync(lobby.Id);
        var round = await _matchService.StartRoundAsync(match.Id);

        // Act: Both players submit wrong answers
        await _matchService.SubmitAnswerAsync(round.Id, player1Id, -999, 1000);
        await _matchService.SubmitAnswerAsync(round.Id, player2Id, -888, 2000);

        // Assert: No score change
        match = (await _matchService.GetMatchAsync(match.Id))!;
        Assert.That(match.Player1Score, Is.EqualTo(0));
        Assert.That(match.Player2Score, Is.EqualTo(0));
        
        Console.WriteLine("✓ Both wrong answers: no points awarded");
    }

    [Test]
    public async Task Round_Player2WinsFirst_ShouldScorePoint()
    {
        // Setup
        var player1Id = Guid.NewGuid();
        var player2Id = Guid.NewGuid();

        var lobby = await _lobbyService.CreateLobbyAsync(player1Id, new CreateLobbyRequest("Hard"));
        await _lobbyService.JoinLobbyAsync(lobby.Id, player2Id);
        await _lobbyService.SetReadyAsync(lobby.Id, player1Id, true);
        await _lobbyService.SetReadyAsync(lobby.Id, player2Id, true);

        var match = await _matchService.StartMatchAsync(lobby.Id);
        var round = await _matchService.StartRoundAsync(match.Id);

        var storedRound = await _roundRepository.GetByIdAsync(round.Id);

        // Act: Player 2 submits correct answer first
        await _matchService.SubmitAnswerAsync(round.Id, player2Id, storedRound!.CorrectAnswer, 1500);

        // Assert: Player 2 scores
        match = (await _matchService.GetMatchAsync(match.Id))!;
        Assert.That(match.Player2Score, Is.EqualTo(1));
        Assert.That(match.Player1Score, Is.EqualTo(0));
        
        Console.WriteLine($"✓ Player 2 wins round! Expression: {round.Expression} = {storedRound.CorrectAnswer}");
    }

    [Test]
    public async Task Match_DifferentDifficulties_ShouldGenerateAppropriately()
    {
        // Test Easy, Medium, Hard generate with correct time limits
        var difficulties = new[] { "Easy", "Medium", "Hard" };
        var expectedTimeLimits = new[] { 4, 8, 12 };

        for (int i = 0; i < difficulties.Length; i++)
        {
            var difficulty = difficulties[i];
            var expectedTime = expectedTimeLimits[i];

            var player1Id = Guid.NewGuid();
            var player2Id = Guid.NewGuid();

            var lobby = await _lobbyService.CreateLobbyAsync(player1Id, new CreateLobbyRequest(difficulty));
            await _lobbyService.JoinLobbyAsync(lobby.Id, player2Id);
            await _lobbyService.SetReadyAsync(lobby.Id, player1Id, true);
            await _lobbyService.SetReadyAsync(lobby.Id, player2Id, true);

            var match = await _matchService.StartMatchAsync(lobby.Id);
            var round = await _matchService.StartRoundAsync(match.Id);

            Assert.That(round.TimeLimitSeconds, Is.EqualTo(expectedTime));
            Console.WriteLine($"✓ {difficulty}: {round.TimeLimitSeconds}s - {round.Expression}");
        }
    }
}


using Duel.Modules.Lobby.Application.DTOs;
using Duel.Modules.Lobby.Application.Services;
using Duel.Modules.Lobby.Infrastructure.Repositories;
using Duel.Modules.Lobby.Domain.Entities;

namespace Duel.Modules.Lobby.Tests;

[TestFixture]
public class LobbyServiceTests
{
    private LobbyService _lobbyService = null!;

    [SetUp]
    public void SetUp()
    {
        var repository = new InMemoryLobbyRepository();
        _lobbyService = new LobbyService(repository);
    }

    [Test]
    public async Task CreateLobby_ShouldSucceed()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new CreateLobbyRequest("Medium");

        // Act
        var lobby = await _lobbyService.CreateLobbyAsync(userId, request);

        // Assert
        Assert.That(lobby.HostId, Is.EqualTo(userId));
        Assert.That(lobby.Difficulty, Is.EqualTo("Medium"));
        Assert.That(lobby.Status, Is.EqualTo(LobbyStatus.Waiting));
        Assert.That(lobby.GuestId, Is.Null);
    }

    [Test]
    public void CreateLobby_WithInvalidDifficulty_ShouldThrow()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new CreateLobbyRequest("Invalid");

        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() => 
            _lobbyService.CreateLobbyAsync(userId, request));
    }

    [Test]
    public async Task JoinLobby_ShouldSucceed()
    {
        // Arrange
        var hostId = Guid.NewGuid();
        var guestId = Guid.NewGuid();
        
        var lobby = await _lobbyService.CreateLobbyAsync(hostId, new CreateLobbyRequest("Easy"));

        // Act
        await _lobbyService.JoinLobbyAsync(lobby.Id, guestId);

        // Assert
        var updated = await _lobbyService.GetLobbyAsync(lobby.Id);
        Assert.That(updated!.GuestId, Is.EqualTo(guestId));
    }

    [Test]
    public async Task SetReady_BothPlayers_ShouldUpdateStatus()
    {
        // Arrange
        var hostId = Guid.NewGuid();
        var guestId = Guid.NewGuid();
        
        var lobby = await _lobbyService.CreateLobbyAsync(hostId, new CreateLobbyRequest("Easy"));
        await _lobbyService.JoinLobbyAsync(lobby.Id, guestId);

        // Act
        await _lobbyService.SetReadyAsync(lobby.Id, hostId, true);
        await _lobbyService.SetReadyAsync(lobby.Id, guestId, true);

        // Assert
        var updated = await _lobbyService.GetLobbyAsync(lobby.Id);
        Assert.That(updated!.Status, Is.EqualTo(LobbyStatus.Ready));
        Assert.That(updated.HostReady, Is.True);
        Assert.That(updated.GuestReady, Is.True);
    }

    [Test]
    public async Task LeaveLobby_Guest_ShouldRemoveGuest()
    {
        // Arrange
        var hostId = Guid.NewGuid();
        var guestId = Guid.NewGuid();
        
        var lobby = await _lobbyService.CreateLobbyAsync(hostId, new CreateLobbyRequest("Easy"));
        await _lobbyService.JoinLobbyAsync(lobby.Id, guestId);

        // Act
        await _lobbyService.LeaveLobbyAsync(lobby.Id, guestId);

        // Assert
        var updated = await _lobbyService.GetLobbyAsync(lobby.Id);
        Assert.That(updated!.GuestId, Is.Null);
        Assert.That(updated.Status, Is.EqualTo(LobbyStatus.Waiting));
    }

    [Test]
    public async Task LeaveLobby_Host_ShouldAbandonLobby()
    {
        // Arrange
        var hostId = Guid.NewGuid();
        var lobby = await _lobbyService.CreateLobbyAsync(hostId, new CreateLobbyRequest("Easy"));

        // Act
        await _lobbyService.LeaveLobbyAsync(lobby.Id, hostId);

        // Assert
        var updated = await _lobbyService.GetLobbyAsync(lobby.Id);
        Assert.That(updated!.Status, Is.EqualTo(LobbyStatus.Abandoned));
    }
}


using Duel.Modules.Lobby.Application.DTOs;
using Duel.Modules.Lobby.Application.Repositories;
using Duel.Modules.Lobby.Domain.Entities;

namespace Duel.Modules.Lobby.Application.Services;

/// <summary>
/// Service implementation for lobby operations.
/// </summary>
public sealed class LobbyService : ILobbyService
{
    private readonly ILobbyRepository _lobbyRepository;

    public LobbyService(ILobbyRepository lobbyRepository)
    {
        _lobbyRepository = lobbyRepository;
    }

    public async Task<LobbyDto> CreateLobbyAsync(Guid userId, CreateLobbyRequest request, CancellationToken cancellationToken = default)
    {
        // Validate difficulty
        if (!IsValidDifficulty(request.Difficulty))
        {
            throw new ArgumentException($"Invalid difficulty: {request.Difficulty}. Must be Easy, Medium, or Hard.");
        }

        // Check if user is already in a lobby
        var existingLobby = await _lobbyRepository.GetByUserIdAsync(userId, cancellationToken);
        if (existingLobby is not null)
        {
            throw new InvalidOperationException("User is already in a lobby.");
        }

        var lobby = new Domain.Entities.Lobby
        {
            Id = Guid.NewGuid(),
            HostId = userId,
            Difficulty = request.Difficulty,
            Status = LobbyStatus.Waiting,
            CreatedAt = DateTime.UtcNow
        };

        await _lobbyRepository.AddAsync(lobby, cancellationToken);

        return MapToDto(lobby);
    }

    public async Task<IEnumerable<LobbyDto>> GetAvailableLobbiesAsync(CancellationToken cancellationToken = default)
    {
        var lobbies = await _lobbyRepository.GetAvailableLobbiesAsync(cancellationToken);
        return lobbies.Select(MapToDto);
    }

    public async Task<LobbyDto?> GetLobbyAsync(Guid lobbyId, CancellationToken cancellationToken = default)
    {
        var lobby = await _lobbyRepository.GetByIdAsync(lobbyId, cancellationToken);
        return lobby is not null ? MapToDto(lobby) : null;
    }

    public async Task JoinLobbyAsync(Guid lobbyId, Guid userId, CancellationToken cancellationToken = default)
    {
        var lobby = await _lobbyRepository.GetByIdAsync(lobbyId, cancellationToken);
        if (lobby is null)
        {
            throw new InvalidOperationException("Lobby not found.");
        }

        if (lobby.Status != LobbyStatus.Waiting)
        {
            throw new InvalidOperationException("Lobby is not available to join.");
        }

        if (lobby.IsFull)
        {
            throw new InvalidOperationException("Lobby is full.");
        }

        if (lobby.IsParticipant(userId))
        {
            throw new InvalidOperationException("User is already in this lobby.");
        }

        // Check if user is already in another lobby
        var existingLobby = await _lobbyRepository.GetByUserIdAsync(userId, cancellationToken);
        if (existingLobby is not null)
        {
            throw new InvalidOperationException("User is already in another lobby.");
        }

        lobby.GuestId = userId;
        await _lobbyRepository.UpdateAsync(lobby, cancellationToken);
    }

    public async Task LeaveLobbyAsync(Guid lobbyId, Guid userId, CancellationToken cancellationToken = default)
    {
        var lobby = await _lobbyRepository.GetByIdAsync(lobbyId, cancellationToken);
        if (lobby is null)
        {
            throw new InvalidOperationException("Lobby not found.");
        }

        if (!lobby.IsParticipant(userId))
        {
            throw new InvalidOperationException("User is not in this lobby.");
        }

        if (lobby.Status == LobbyStatus.InProgress)
        {
            throw new InvalidOperationException("Cannot leave lobby while match is in progress.");
        }

        // If host leaves, abandon the lobby
        if (lobby.IsHost(userId))
        {
            lobby.Status = LobbyStatus.Abandoned;
            await _lobbyRepository.UpdateAsync(lobby, cancellationToken);
            return;
        }

        // Guest leaves
        lobby.GuestId = null;
        lobby.GuestReady = false;
        await _lobbyRepository.UpdateAsync(lobby, cancellationToken);
    }

    public async Task SetReadyAsync(Guid lobbyId, Guid userId, bool ready, CancellationToken cancellationToken = default)
    {
        var lobby = await _lobbyRepository.GetByIdAsync(lobbyId, cancellationToken);
        if (lobby is null)
        {
            throw new InvalidOperationException("Lobby not found.");
        }

        if (!lobby.IsParticipant(userId))
        {
            throw new InvalidOperationException("User is not in this lobby.");
        }

        if (!lobby.IsFull)
        {
            throw new InvalidOperationException("Cannot ready up until lobby is full.");
        }

        if (lobby.IsHost(userId))
        {
            lobby.HostReady = ready;
        }
        else
        {
            lobby.GuestReady = ready;
        }

        // Update status if both ready
        if (lobby.AreBothReady && ready)
        {
            lobby.Status = LobbyStatus.Ready;
        }
        else if (lobby.Status == LobbyStatus.Ready && !ready)
        {
            lobby.Status = LobbyStatus.Waiting;
        }

        await _lobbyRepository.UpdateAsync(lobby, cancellationToken);
    }

    private static LobbyDto MapToDto(Domain.Entities.Lobby lobby)
    {
        return new LobbyDto(
            lobby.Id,
            lobby.HostId,
            lobby.GuestId,
            lobby.Difficulty,
            lobby.Status,
            lobby.HostReady,
            lobby.GuestReady,
            lobby.CreatedAt
        );
    }

    private static bool IsValidDifficulty(string difficulty)
    {
        return difficulty is "Easy" or "Medium" or "Hard";
    }
}


using Duel.Modules.Lobby.Application.DTOs;

namespace Duel.Modules.Lobby.Application.Services;

/// <summary>
/// Service for lobby operations.
/// </summary>
public interface ILobbyService
{
    Task<LobbyDto> CreateLobbyAsync(Guid userId, CreateLobbyRequest request, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<LobbyDto>> GetAvailableLobbiesAsync(CancellationToken cancellationToken = default);
    
    Task<LobbyDto?> GetLobbyAsync(Guid lobbyId, CancellationToken cancellationToken = default);
    
    Task JoinLobbyAsync(Guid lobbyId, Guid userId, CancellationToken cancellationToken = default);
    
    Task LeaveLobbyAsync(Guid lobbyId, Guid userId, CancellationToken cancellationToken = default);
    
    Task SetReadyAsync(Guid lobbyId, Guid userId, bool ready, CancellationToken cancellationToken = default);
}


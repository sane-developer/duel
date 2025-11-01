namespace Duel.Modules.Lobby.Application.DTOs;

/// <summary>
/// Request to create a new lobby.
/// </summary>
public sealed record CreateLobbyRequest(
    string Difficulty
);


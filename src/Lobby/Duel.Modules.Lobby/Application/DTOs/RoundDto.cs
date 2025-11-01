namespace Duel.Modules.Lobby.Application.DTOs;

/// <summary>
/// DTO for round information sent to clients.
/// </summary>
public sealed record RoundDto(
    Guid Id,
    int RoundNumber,
    string Expression,
    int TimeLimitSeconds
);


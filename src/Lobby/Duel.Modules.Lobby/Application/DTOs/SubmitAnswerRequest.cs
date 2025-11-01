namespace Duel.Modules.Lobby.Application.DTOs;

/// <summary>
/// Request to submit an answer for a round.
/// </summary>
public sealed record SubmitAnswerRequest(
    int Answer
);


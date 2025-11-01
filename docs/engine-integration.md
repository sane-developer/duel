# Engine Module Integration Guide

## Overview

This document explains how the Engine module is designed and how the Lobby module should integrate with it in the Duel modular monolith architecture.

---

## Architecture: Modular Monolith In-Process Communication

### What is In-Process Communication?

In a **modular monolith**, all modules run in the same process and communicate through:
- **Interfaces/Abstractions** - Define contracts between modules
- **DTOs (Data Transfer Objects)** - Simple data structures passed across boundaries
- **Dependency Injection** - Modules register services, consumers inject interfaces

**Benefits:**
- ✅ Simple - no network calls, no serialization overhead
- ✅ Fast - direct method calls within the same process
- ✅ Type-safe - compiler-checked contracts
- ✅ Easier debugging - single process to debug
- ✅ Testable - easy to mock interfaces

**Compared to microservices:**
- ❌ Microservices: HTTP/gRPC, network latency, complex deployment
- ✅ Modular Monolith: Direct calls, single deployment unit

---

## Engine Module Structure

### Files Created/Modified

```
src/Engine/Duel.Modules.Engine/
├── Abstractions/
│   └── IEngine.cs                 # ✅ Interface + DTO
├── Games/
│   └── Muffs/
│       ├── MuffsEngine.cs         # ✅ Implementation
│       ├── Generation/            # Internal - Lobby doesn't see this
│       ├── Evaluation/            # Internal - Lobby doesn't see this
│       └── Serialization/         # Internal - Lobby doesn't see this
└── Tests/
    └── MuffsEngineTests.cs        # ✅ Integration examples
```

---

## The Contract: `IEngine` Interface

### Interface Definition

```csharp
public interface IEngine
{
    string GameType { get; }
    Challenge GenerateChallenge(string difficulty);
}

public sealed record Challenge(
    string Expression,
    int CorrectAnswer,
    int TimeLimitSeconds
);
```

### Design Decisions

| Aspect | Decision | Reasoning |
|--------|----------|-----------|
| **Return Type** | `Challenge` record | Clean DTO with all round data |
| **Difficulty** | `string` ("Easy", "Medium", "Hard") | Matches database schema, simple |
| **Expression** | `string` | Ready to display to users |
| **Answer** | `int` | Matches database column type |
| **Time Limit** | Included in response | Engine knows difficulty settings |

**Why not expose internal types like `Glyph`?**
- Internal types leak implementation details
- Lobby module doesn't care about AST structure
- Clean boundaries = better maintainability

---

## How Lobby Module Uses the Engine

### 1. Dependency Injection Setup

**In Startup/Program.cs:**

```csharp
// Register the engine as a service
services.AddSingleton<IEngine, MuffsEngine>();
```

**In LobbyService or MatchService constructor:**

```csharp
public class MatchService
{
    private readonly IEngine _engine;

    public MatchService(IEngine engine)
    {
        _engine = engine; // Injected by DI container
    }
}
```

### 2. Generate Challenge for a Round

```csharp
public async Task<Round> StartNewRound(Match match)
{
    // Get difficulty from lobby settings (e.g., "Medium")
    var difficulty = match.Difficulty;

    // Ask engine to generate a challenge
    var challenge = _engine.GenerateChallenge(difficulty);

    // Create round entity for database
    var round = new Round
    {
        MatchId = match.Id,
        RoundNumber = match.CurrentRound,
        Expression = challenge.Expression,        // Store for display
        CorrectAnswer = challenge.CorrectAnswer,  // Store for validation
        TimeLimitSeconds = challenge.TimeLimitSeconds
    };

    // Save to database
    await _roundRepository.AddAsync(round);

    // Send to players via SignalR
    await _lobbyHub.Clients.Group(match.Id)
        .SendAsync("RoundStarted", new
        {
            Expression = challenge.Expression,
            TimeLimit = challenge.TimeLimitSeconds
        });

    return round;
}
```

### 3. Validate Player Answer

```csharp
public async Task<bool> ValidateAnswer(Guid roundId, int playerAnswer)
{
    // Fetch round from database
    var round = await _roundRepository.GetByIdAsync(roundId);

    // Simple comparison - engine already computed correct answer
    return playerAnswer == round.CorrectAnswer;
}
```

---

## Full Lobby-Engine Flow Example

### Scenario: Medium difficulty match, round 1

```
┌─────────────┐                    ┌──────────────┐                    ┌───────────────┐
│ Player 1 & 2│                    │ Lobby Module │                    │ Engine Module │
│   (SignalR) │                    │ (MatchService)│                   │ (MuffsEngine) │
└──────┬──────┘                    └──────┬───────┘                    └───────┬───────┘
       │                                   │                                    │
       │ 1. Both ready                    │                                    │
       ├──────────────────────────────────>│                                    │
       │                                   │                                    │
       │                                   │ 2. GenerateChallenge("Medium")     │
       │                                   ├───────────────────────────────────>│
       │                                   │                                    │
       │                                   │                                    │ • Create difficulty settings
       │                                   │                                    │ • Generate expression AST
       │                                   │                                    │ • Serialize to "(5 + 3) * 2"
       │                                   │                                    │ • Evaluate to 16
       │                                   │                                    │
       │                                   │ 3. Challenge(                      │
       │                                   │      Expression: "(5 + 3) * 2",    │
       │                                   │      CorrectAnswer: 16,            │
       │                                   │      TimeLimitSeconds: 8           │
       │                                   │    )                               │
       │                                   │<───────────────────────────────────┤
       │                                   │                                    │
       │                                   │ 4. Save to DB:                     │
       │                                   │    INSERT INTO rounds ...          │
       │                                   │                                    │
       │ 5. RoundStarted:                 │                                    │
       │    "(5 + 3) * 2"                 │                                    │
       │    Timer: 8s                     │                                    │
       │<──────────────────────────────────┤                                    │
       │                                   │                                    │
       │ 6. Player 1 submits: 16          │                                    │
       ├──────────────────────────────────>│                                    │
       │                                   │                                    │
       │                                   │ 7. Compare: 16 == 16 ✓             │
       │                                   │    (No engine call needed)         │
       │                                   │                                    │
       │ 8. RoundCompleted:               │                                    │
       │    Winner: Player 1              │                                    │
       │<──────────────────────────────────┤                                    │
       │                                   │                                    │
```

---

## Key Architectural Principles

### 1. **Encapsulation**
- Engine internals (Glyph, ExpressionGenerator, etc.) are hidden
- Lobby only sees `IEngine` interface
- Change engine implementation without touching Lobby

### 2. **Single Responsibility**
- **Engine:** Generate expressions and compute answers
- **Lobby:** Orchestrate matches, validate timing, manage state
- **Database:** Persist round data for history/stats

### 3. **Dependency Direction**
```
┌────────────┐         ┌──────────────┐
│   Lobby    │─────────>│   IEngine    │ (Abstraction)
│   Module   │  depends │  (interface) │
└────────────┘    on    └──────────────┘
                              ▲
                              │ implements
                              │
                        ┌──────────────┐
                        │ MuffsEngine  │
                        │ (Engine Mod) │
                        └──────────────┘
```

Lobby depends on abstraction, not concrete implementation.

### 4. **Testability**

**Mock the engine in Lobby tests:**

```csharp
[Test]
public async Task StartRound_ShouldSendChallengeToPlayers()
{
    // Arrange
    var mockEngine = new Mock<IEngine>();
    mockEngine.Setup(e => e.GenerateChallenge("Easy"))
              .Returns(new Challenge("2 + 2", 4, 4));

    var matchService = new MatchService(mockEngine.Object);

    // Act
    await matchService.StartNewRound(match);

    // Assert
    mockEngine.Verify(e => e.GenerateChallenge("Easy"), Times.Once);
}
```

---

## Future Extensions

### Adding New Game Types

When adding Blackjack or other games in V2:

```csharp
// Register multiple engines
services.AddSingleton<IEngine, MuffsEngine>();
services.AddSingleton<IEngine, BlackjackEngine>();

// Lobby resolves by GameType
public class MatchService
{
    private readonly IEnumerable<IEngine> _engines;

    public MatchService(IEnumerable<IEngine> engines)
    {
        _engines = engines;
    }

    public IEngine GetEngine(string gameType)
    {
        return _engines.First(e => e.GameType == gameType);
    }
}
```

### Async Generation (if needed)

If generation becomes slow:

```csharp
public interface IEngine
{
    Task<Challenge> GenerateChallengeAsync(string difficulty);
}
```

---

## Common Pitfalls to Avoid

### ❌ Don't: Couple Lobby to Engine Internals

```csharp
// BAD - Lobby imports internal engine types
using Duel.Modules.Engine.Games.Muffs.Generation;

var generator = new ExpressionGenerator(...); // Tight coupling!
```

### ✅ Do: Use the Interface

```csharp
// GOOD - Lobby depends on abstraction
using Duel.Modules.Engine.Abstractions;

var challenge = _engine.GenerateChallenge(difficulty);
```

### ❌ Don't: Store Glyph AST in Database

```csharp
// BAD - Leaking internal types to database
public class Round
{
    public Glyph ExpressionAST { get; set; } // Internal type!
}
```

### ✅ Do: Store Serialized String

```csharp
// GOOD - Clean DTO fields
public class Round
{
    public string Expression { get; set; }      // "(2 + 3) * 4"
    public int CorrectAnswer { get; set; }      // 20
}
```

---

## Summary

**In-process communication in a modular monolith:**
1. Define clean interfaces (`IEngine`)
2. Use simple DTOs (`Challenge`)
3. Inject dependencies via DI
4. Keep module internals private
5. Test with mocks

**Lobby module integration:**
- Inject `IEngine` in constructor
- Call `GenerateChallenge(difficulty)` for each round
- Store `Expression` and `CorrectAnswer` in database
- Validate by comparing player answer with stored answer

**No HTTP, no gRPC, no message queues** - just clean, fast, in-process method calls! 🚀

---

**Last Updated:** October 31, 2025  
**Related Files:**
- `src/Engine/Duel.Modules.Engine/Abstractions/IEngine.cs`
- `src/Engine/Duel.Modules.Engine/Games/Muffs/MuffsEngine.cs`
- `src/Engine/Duel.Modules.Engine.Tests/MuffsEngineTests.cs`


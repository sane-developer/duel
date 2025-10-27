using Duel.Modules.Engine.Games.Muffs.Generation;

namespace Duel.Modules.Engine.Games.Muffs.Knowledge;

/// <summary>
/// Static registry of pre-built mathematical knowledge.
/// Initialized once at application startup for optimal performance.
/// </summary>
public static class KnowledgeRegistry
{
    /// <summary>
    /// Universal knowledge base containing ALL possible compositions.
    /// Built from Hard difficulty settings (superset of Easy and Medium).
    /// Shared across all difficulty levels - GeneratorSettings control what gets selected.
    /// </summary>
    public static readonly KnowledgeSet Universal = new(DifficultyPresets.Hard);
}


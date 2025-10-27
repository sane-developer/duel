using Duel.Modules.Engine.Games.Muffs.Knowledge;
using Duel.Modules.Engine.Games.Muffs.Representation;

namespace Duel.Modules.Engine.Games.Muffs.Generation;

/// <summary>
/// Generates random mathematical expressions based on settings and shared knowledge.
/// </summary>
/// <param name="settings">Generator settings controlling operator weights, ranges, depth, and length</param>
/// <param name="knowledge">Shared knowledge base containing all possible compositions and numbers</param>
public sealed class ExpressionGenerator(GeneratorSettings settings, KnowledgeSet knowledge)
{
    private readonly OperatorSelector _operatorSelector = new(settings);

    public Glyph Generate(Random rng)
    {
        var length = settings.Length.Random(rng);
        
        var type = _operatorSelector.SelectBinary(rng);
        
        var result = settings.GetResult(type, rng);
        
        var depth = settings.Depth.Random(rng);
        
        var current = GenerateForResult(result, depth, rng);
        
        for (var i = 0; i < length; i++)
        {
            type = _operatorSelector.SelectBinary(rng);
        
            result = settings.GetResult(type, rng);
        
            depth = settings.Depth.Random(rng);
            
            var right = GenerateForResult(result, depth, rng);
            
            current = Binary.Create(type, current, right);
        }
        
        return current;
    }

    private Glyph GenerateForResult(int result, int depth, Random rng)
    {
        if (depth == 0)
        {
            return knowledge.Numbers.GetNumber(result);
        }
        
        var compositions = knowledge.Compositions.GetCompositions(result);
        
        if (compositions.Length == 0)
        {
            return knowledge.Numbers.GetNumber(result);
        }
        
        var composition = compositions.Random(rng);

        if (composition is BinaryComposition binary)
        {
            var lhs = GenerateForResult(binary.Lhs, depth - 1, rng);
            
            var rhs = GenerateForResult(binary.Rhs, depth - 1, rng);

            return Binary.Create(binary.Type, lhs, rhs);
        }
        
        if (composition is UnaryComposition unary)
        {
            var operand = GenerateForResult(unary.Operand, depth - 1, rng);

            return Unary.Create(unary.Type, operand);
        }
        
        return knowledge.Numbers.GetNumber(result);
    }
}


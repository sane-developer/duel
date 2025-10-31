using Duel.Modules.Engine.Games.Muffs.Representation;
using Duel.Modules.Engine.Games.Muffs.Generation.Difficulties;
using Duel.Modules.Engine.Games.Muffs.Generation.Operators;
using Duel.Modules.Engine.Games.Muffs.Generation.Numbers;
using Duel.Modules.Engine.Games.Muffs.Generation.Compositions;

namespace Duel.Modules.Engine.Games.Muffs.Generation;

public sealed class ExpressionGenerator(Difficulty settings, NumbersRegistry numbers, CompositionsRegistry compositionsRegistry, OperatorSelector operatorSelector)
{
    public Glyph Generate(Random rng)
    {
        var length = settings.Length.Random(rng);
        
        var type = operatorSelector.SelectBinary(rng);
        
        var result = settings.Operators[type].Result.Random(rng);
        
        var depth = settings.Depth.Random(rng);
        
        var current = GenerateForResult(result, depth, rng);
        
        for (var i = 0; i < length; i++)
        {
            type = operatorSelector.SelectBinary(rng);
        
            result = settings.Operators[type].Result.Random(rng);
        
            depth = settings.Depth.Random(rng);
            
            var right = GenerateForResult(result, depth, rng);
            
            current = OperatorFactory.Binary(type, current, right);
        }
        
        return current;
    }

    private Glyph GenerateForResult(int result, int depth, Random rng)
    {
        if (depth == 0)
        {
            return numbers.GetNumber(result);
        }
        
        var compositions = compositionsRegistry.GetCompositions(result);
        
        if (compositions.Length == 0)
        {
            return numbers.GetNumber(result);
        }
        
        var composition = compositions.Random(rng);

        if (composition is BinaryComposition binary)
        {
            var lhs = GenerateForResult(binary.Lhs, depth - 1, rng);
            
            var rhs = GenerateForResult(binary.Rhs, depth - 1, rng);

            return OperatorFactory.Binary(binary.Type, lhs, rhs);
        }
        
        if (composition is UnaryComposition unary)
        {
            var operand = GenerateForResult(unary.Operand, depth - 1, rng);

            return OperatorFactory.Unary(unary.Type, operand);
        }
        
        return numbers.GetNumber(result);
    }
}


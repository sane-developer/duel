using Duel.Modules.Engine.Muffs.Compositions;
using Duel.Modules.Engine.Muffs.Glyphs;
using Duel.Modules.Engine.Muffs.Presets;

namespace Duel.Modules.Engine.Muffs.Expressions;

public readonly record struct ExpressionGeneratorContext(Random Rng, IExpressionPreset Preset);

public sealed class ExpressionGenerator(ExpressionGeneratorContext context)
{
    public Glyph Generate()
    {
        var length = context.Preset.Length.GetLength(context.Rng);

        var operands = GenerateOperands(length);

        return MergeOperands(operands);
    }

    private Stack<Glyph> GenerateOperands(int length)
    {
        var operands = new Stack<Glyph>(length);

        for (var i = 0; i < length; i++)
        {
            var value = context.Preset.Operand.GetNumber(context.Rng);
            
            var depth = context.Preset.Depth.GetDepth(context.Rng);

            var operand = Compose(value, depth);

            operands.Push(operand);
        }

        return operands;
    }

    private Glyph MergeOperands(Stack<Glyph> operands)
    {
        if (operands.Count == 0)
        {
            return Situation.Unreachable<Glyph>();
        }

        if (operands.Count == 1)
        {
            return operands.Pop();
        }

        if (operands.Count == 2)
        {
            return MergeOperands(operands.Pop(), operands.Pop());
        }

        var tail = MergeOperands(operands.Pop(), operands.Pop());

        while (operands.Count > 1)
        {
            tail = MergeOperands(operands.Pop(), tail);
        }

        return MergeOperands(operands.Pop(), tail);
    }

    private BinaryOperator MergeOperands(Glyph lhs, Glyph rhs)
    {
        var operatorType = context.Preset.Operator.GetBinary(context.Rng);

        return BinaryOperator.From(operatorType, lhs, rhs);
    }

    private Glyph Compose(int result, int depth)
    {
        if (depth == 0)
        {
            return context.Preset.NumberRegistry.GetNumber(result);
        }

        var operatorType = context.Preset.Operator.GetAny(context.Rng);

        var composition = GetComposition(result, operatorType);

        if (composition == Composition.Null)
        {
            return context.Preset.NumberRegistry.GetNumber(result);
        }

        if (composition is BinaryComposition binary)
        {
            return ComposeBinary(binary, depth - 1);
        }

        var unary = (UnaryComposition) composition;

        return ComposeUnary(unary, depth - 1);
    }

    private Composition GetComposition(int result, GlyphType operatorType)
    {
        var compositions = context.Preset.CompositionRegistry.GetCompositions(result, operatorType);

        if (compositions.Count == 0)
        {
            return Composition.Null;
        }

        var index = context.Rng.Next(0, compositions.Count);

        return compositions.Items[index];
    }

    private BinaryOperator ComposeBinary(BinaryComposition composition, int depth)
    {
        var lhs = Compose(composition.Lhs, depth);

        var rhs = Compose(composition.Rhs, depth);

        return BinaryOperator.From(composition.OperatorType, lhs, rhs);
    }

    private UnaryOperator ComposeUnary(UnaryComposition composition, int depth)
    {
        var operand = Compose(composition.Operand, depth);

        return UnaryOperator.From(composition.OperatorType, operand);
    }
}
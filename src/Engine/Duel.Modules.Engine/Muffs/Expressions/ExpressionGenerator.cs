using Duel.Modules.Engine.Muffs.Compositions;
using Duel.Modules.Engine.Muffs.Glyphs;
using Duel.Modules.Engine.Muffs.Policies;

namespace Duel.Modules.Engine.Muffs.Expressions;

public sealed record ExpressionGeneratorContext(
    Random Rng,
    IExpressionPolicy Policy, 
    CompositionRegistry CompositionRegistry
);

public sealed class ExpressionGenerator(ExpressionGeneratorContext context)
{
    private readonly Random _rng = context.Rng;

    private readonly IExpressionPolicy _policy = context.Policy;

    private readonly CompositionRegistry _compositionRegistry = context.CompositionRegistry;

    public Glyph Generate()
    {
        var length = _policy.Length.GetLength(_rng);

        var operands = GenerateOperands(length);

        return MergeOperands(operands);
    }

    private Stack<Glyph> GenerateOperands(int length)
    {
        var operands = new Stack<Glyph>(length);

        for (var i = 0; i < length; i++)
        {
            var value = _policy.Operand.GetNumber(_rng);
            
            var depth = _policy.Depth.GetDepth(_rng);

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
        var operatorType = _policy.Operator.GetBinary(_rng);

        return new BinaryOperator(operatorType)
        {
            Lhs = lhs, Rhs = rhs
        };
    }

    private Glyph Compose(int result, int depth)
    {
        if (depth == 0)
        {
            return new Number(result);
        }

        var operatorType = _policy.Operator.GetAny(_rng);

        var composition = GetComposition(result, operatorType);

        if (composition == Composition.Null)
        {
            return new Number(result);
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
        var compositions = _compositionRegistry.For(result, operatorType);

        if (compositions.Count == 0)
        {
            return Composition.Null;
        }

        var index = _rng.Next(0, compositions.Count);

        return compositions[index];
    }

    private BinaryOperator ComposeBinary(BinaryComposition composition, int depth)
    {
        return new BinaryOperator(composition.OperatorType)
        {
            Lhs = Compose(composition.Lhs, depth - 1), Rhs = Compose(composition.Rhs, depth - 1)
        };
    }

    private UnaryOperator ComposeUnary(UnaryComposition composition, int depth)
    {
        return new UnaryOperator(composition.OperatorType)
        {
            Operand = Compose(composition.Operand, depth - 1)
        };
    }
}
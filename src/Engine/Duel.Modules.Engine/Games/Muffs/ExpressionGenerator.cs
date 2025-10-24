using Duel.Modules.Engine.Games.Muffs.AST;
using Duel.Modules.Engine.Games.Muffs.AST.Literals;
using Duel.Modules.Engine.Games.Muffs.AST.Operators;
using Duel.Shared.Extensions;

namespace Duel.Modules.Engine.Games.Muffs;

public sealed class ExpressionGenerator(ExpressionContext context)
{
    public Glyph Generate(Random rng)
    {
        var length = context.GetLength(rng);
        
        var operatorType = GetBinaryOperatorType(rng);
        
        var operatorSettings = GetOperatorSettings(operatorType);
        
        var leftTarget = rng.Next(operatorSettings.Operand.Start, operatorSettings.Operand.End + 1);
        
        var current = GenerateOperand(leftTarget, context.GetDepth(rng), rng);
        
        for (int i = 0; i < length; i++)
        {
            operatorType = GetBinaryOperatorType(rng);
            
            operatorSettings = GetOperatorSettings(operatorType);
            
            var rightTarget = rng.Next(operatorSettings.Operand.Start, operatorSettings.Operand.End + 1);
            
            var right = GenerateOperand(rightTarget, context.GetDepth(rng), rng);
            
            current = CreateBinaryOperator(operatorType, current, right);
        }
        
        return current;
    }
    
    private OperatorType GetBinaryOperatorType(Random rng)
    {
        OperatorType operatorType;
        do
        {
            operatorType = context.GetOperatorType(rng);
        } while (!IsBinaryOperator(operatorType));
        
        return operatorType;
    }
    
    private static bool IsBinaryOperator(OperatorType type)
    {
        return type is OperatorType.Addition
            or OperatorType.Subtraction
            or OperatorType.Multiplication
            or OperatorType.Division
            or OperatorType.Modulo
            or OperatorType.Power;
    }

    private Glyph GenerateOperand(int target, int maxDepth, Random rng)
    {
        if (maxDepth == 0)
        {
            return context.GetNumber(target);
        }
        
        Composition[] compositions;

        try
        {
            compositions = context.GetCompositions(target);
        }
        catch (KeyNotFoundException)
        {
            return context.GetNumber(target);
        }
        
        if (compositions.Length == 0)
        {
            return context.GetNumber(target);
        }
        
        var composition = compositions.Random(rng);
        
        return composition switch
        {
            BinaryComposition binary => CreateBinaryOperator(
                binary.Type,
                GenerateOperand(binary.Lhs, maxDepth - 1, rng),
                GenerateOperand(binary.Rhs, maxDepth - 1, rng)
            ),
            UnaryComposition unary => CreateUnaryOperator(
                unary.Type,
                GenerateOperand(unary.Operand, maxDepth - 1, rng)
            ),
            _ => context.GetNumber(target)
        };
    }

    private OperatorSettings GetOperatorSettings(OperatorType operatorType)
    {
        return operatorType switch
        {
            OperatorType.Addition => context.Settings.Addition,
            OperatorType.Subtraction => context.Settings.Subtraction,
            OperatorType.Multiplication => context.Settings.Multiplication,
            OperatorType.Division => context.Settings.Division,
            OperatorType.Modulo => context.Settings.Modulo,
            OperatorType.Power => context.Settings.Power,
            OperatorType.Negation => context.Settings.Negation,
            OperatorType.AbsoluteValue => context.Settings.AbsoluteValue,
            OperatorType.Factorial => context.Settings.Factorial,
            OperatorType.SquareRoot => context.Settings.SquareRoot,
            _ => Situation.Unreachable<OperatorSettings>()
        };
    }

    private static Glyph CreateBinaryOperator(OperatorType operatorType, Glyph left, Glyph right)
    {
        return operatorType switch
        {
            OperatorType.Addition => new Add(left, right),
            OperatorType.Subtraction => new Subtract(left, right),
            OperatorType.Multiplication => new Multiply(left, right),
            OperatorType.Division => new Divide(left, right),
            OperatorType.Modulo => new Modulo(left, right),
            OperatorType.Power => new Power(left, right),
            _ => Situation.Unreachable<Glyph>()
        };
    }

    private static Glyph CreateUnaryOperator(OperatorType operatorType, Glyph operand)
    {
        return operatorType switch
        {
            OperatorType.Negation => new Negate(operand),
            OperatorType.AbsoluteValue => new Absolute(operand),
            OperatorType.Factorial => new Factorial(operand),
            OperatorType.SquareRoot => new SquareRoot(operand),
            _ => Situation.Unreachable<Glyph>()
        };
    }
}
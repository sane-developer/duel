using Duel.Modules.Engine.Muffs.Glyphs;

namespace Duel.Modules.Engine.Muffs.Expressions;

public static class ExpressionSerializer
{
    public static string Serialize(Glyph root)
    {
        using var writer = new StringWriter();
        
        SerializeToWriter(root, writer);
        
        return writer.ToString();
    }

    private static void SerializeToWriter(Glyph root, StringWriter writer)
    {
        if (root is Number number)
        {
            writer.Write(number.Value);
            
            return;
        }

        if (root is BinaryOperator binary)
        {
            SerializeBinary(binary, writer);
            
            return;
        }

        if (root is UnaryOperator unary)
        {
            SerializeUnary(unary, writer);
            
            return;
        }

        Situation.Unreachable<GlyphType>();
    }

    private static void SerializeBinary(BinaryOperator binary, StringWriter writer)
    {
        writer.Write('(');
        
        SerializeToWriter(binary.Lhs, writer);
        
        writer.Write(' ');
        
        var symbol = GetBinarySymbol(binary.Type);
        
        writer.Write(symbol);
       
        writer.Write(' ');
        
        SerializeToWriter(binary.Rhs, writer);
        
        writer.Write(')');
    }

    private static void SerializeUnary(UnaryOperator unary, StringWriter writer)
    {
        if (unary.Type is GlyphType.Negate)
        {
            writer.Write('-');

            writer.Write('(');
            
            SerializeToWriter(unary.Operand, writer);
            
            writer.Write(')');

            return;
        }

        if (unary.Type is GlyphType.Absolute)
        {
            writer.Write('|');
            
            SerializeToWriter(unary.Operand, writer);
            
            writer.Write('|');
            
            return;
        }

        if (unary.Type is GlyphType.SquareRoot)
        {
            writer.Write('√');
            
            SerializeToWriter(unary.Operand, writer);
            
            return;
        }

        if (unary.Type is GlyphType.Factorial)
        {
            SerializeToWriter(unary.Operand, writer);
            
            writer.Write('!');
            
            return;
        }
    }

    private static char GetBinarySymbol(GlyphType type)
    {
        return type switch
        {
            GlyphType.Add => '+',
            GlyphType.Subtract => '-',
            GlyphType.Multiply => '*',
            GlyphType.Divide => '/',
            GlyphType.Modulo => '%',
            GlyphType.Power => '^',
            _ => Situation.Unreachable<char>()
        };
    }
}
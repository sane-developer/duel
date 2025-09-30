namespace Duel.Core.Games.Muffs;

public static class ExpressionSerializer
{
    private static readonly Dictionary<OperatorType, char> Identifiers = new()
    {
        [OperatorType.Add] = '+',
        [OperatorType.Subtract] = '-',
        [OperatorType.Multiply] = '*',
        [OperatorType.Divide] = '/',
        [OperatorType.Power] = '^'
    };
        
    private static char MapToIdentifier(OperatorType type)
    {
        return Identifiers[type];
    }
    
    public static string ToInfix(Expression e)
    {
        return Format(e);
    }
    
    public static string ToFullyParenthesized(Expression e)
    {
        if (!e.Root.IsOperator) return e.Root.Value.ToString();
        var left = ToFullyParenthesized(e.Left!);
        var right = ToFullyParenthesized(e.Right!);
        return $"({left} {MapToIdentifier(e.Root.Operator.Type)} {right})";
    }

    private static string Format(Expression node)
    {
        if (!node.Root.IsOperator)
            return node.Root.Value.ToString();

        var op = node.Root.Operator;

        var left = Format(node.Left!);
        var right = Format(node.Right!);

        var leftNeeds = NeedsParens(node.Left!, parentOp: op, isLeftChild: true);
        var rightNeeds = NeedsParens(node.Right!, parentOp: op, isLeftChild: false);

        if (leftNeeds)  left  = "(" + left  + ")";
        if (rightNeeds) right = "(" + right + ")";

        return $"{left} {MapToIdentifier(op.Type)} {right}";
    }

    private static bool NeedsParens(Expression child, Operator parentOp, bool isLeftChild)
    {
        if (!child.Root.IsOperator)
        {
            int v = child.Root.Value;
            if (v < 0)
            {
                return true;
            }
            return false;
        }

        var childOp = child.Root.Operator;
        int pParent = Precedence(parentOp);
        int pChild  = Precedence(childOp);

        if (pChild < pParent)
            return true;

        if (pChild > pParent)
            return false;

        bool parentRightAssoc = IsRightAssociative(parentOp);

        if (!parentRightAssoc && !isLeftChild) return true;
        if ( parentRightAssoc &&  isLeftChild) return true;

        return false;
    }

    private static int Precedence(Operator op)
    {
        return MapToIdentifier(op.Type) switch
        {
            '^' => 4,
            '*' or '/' => 3,
            '+' or '-' => 2,
            _ => 0
        };
    }

    private static bool IsRightAssociative(Operator op) => MapToIdentifier(op.Type) == '^';
}
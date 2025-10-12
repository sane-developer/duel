namespace Duel.Modules.Engine.Games.Muffs.Expressions;

public sealed class ExpressionVault
{
    private readonly Dictionary<ExpressionType, Dictionary<int, (int Left, int Right)[]>> _vault = [];

    public static ExpressionVault Create(int minimum, int maximum)
    {
        var vault = new ExpressionVault();
        
        vault.InitializeAddition(minimum, maximum);
        
        vault.InitializeSubtraction(minimum, maximum);
        
        vault.InitializeMultiplication(minimum, maximum);
        
        vault.InitializeDivision(minimum, maximum);
        
        vault.InitializeModulo(minimum, maximum);
        
        vault.InitializePower(minimum, maximum);
        
        return vault;
    }

    private void InitializeAddition(int minimum, int maximum)
    {
        var range = maximum - minimum + 1;
        
        var totalCombinations = range * range;
        
        var counts = new Dictionary<int, int>(totalCombinations);
        
        for (var left = minimum; left <= maximum; left++)
        {
            for (var right = minimum; right <= maximum; right++)
            {
                var result = left + right;
                
                counts.TryGetValue(result, out var count);
                
                counts[result] = count + 1;
            }
        }

        var vault = new Dictionary<int, (int, int)[]>(counts.Count);

        var cursors = new Dictionary<int, int>(counts.Count);
        
        foreach (var (result, count) in counts)
        {
            vault[result] = new (int, int)[count];
            
            cursors[result] = 0;
        }
        
        for (var left = minimum; left <= maximum; left++)
        {
            for (var right = minimum; right <= maximum; right++)
            {
                var result = left + right;
                
                var index = cursors[result]++;
                
                vault[result][index] = (left, right);
            }
        }

        _vault[ExpressionType.Add] = vault;
    }

    private void InitializeSubtraction(int minimum, int maximum)
    {
        var range = maximum - minimum + 1;

        var totalCombinations = range * range;
        
        var counts = new Dictionary<int, int>(totalCombinations);
        
        for (var left = minimum; left <= maximum; left++)
        {
            for (var right = minimum; right <= maximum; right++)
            {
                var result = left - right;
                
                counts.TryGetValue(result, out var count);
                
                counts[result] = count + 1;
            }
        }

        var vault = new Dictionary<int, (int, int)[]>(counts.Count);

        var cursors = new Dictionary<int, int>(counts.Count);
        
        foreach (var (result, count) in counts)
        {
            vault[result] = new (int, int)[count];

            cursors[result] = 0;
        }
        
        for (var left = minimum; left <= maximum; left++)
        {
            for (var right = minimum; right <= maximum; right++)
            {
                var result = left - right;
                
                var index = cursors[result]++;
                
                vault[result][index] = (left, right);
            }
        }

        _vault[ExpressionType.Subtract] = vault;
    }

    private void InitializeMultiplication(int minimum, int maximum)
    {
        var range = maximum - minimum + 1;
        
        var totalCombinations = range * range;
        
        var counts = new Dictionary<int, int>(totalCombinations);
        
        for (var left = minimum; left <= maximum; left++)
        {
            for (var right = minimum; right <= maximum; right++)
            {
                var result = left * right;
                
                counts.TryGetValue(result, out var count);
                
                counts[result] = count + 1;
            }
        }

        var vault = new Dictionary<int, (int, int)[]>(counts.Count);
        
        var cursors = new Dictionary<int, int>(counts.Count);
        
        foreach (var (result, count) in counts)
        {
            vault[result] = new (int, int)[count];
            
            cursors[result] = 0;
        }
        
        for (var left = minimum; left <= maximum; left++)
        {
            for (var right = minimum; right <= maximum; right++)
            {
                var result = left * right;
                
                var index = cursors[result]++;
                
                vault[result][index] = (left, right);
            }
        }

        _vault[ExpressionType.Multiply] = vault;
    }

    private void InitializeDivision(int minimum, int maximum)
    {
        var range = maximum - minimum + 1;
        
        var estimatedCombinations = range * range;
        
        var counts = new Dictionary<int, int>(estimatedCombinations);
        
        for (var left = minimum; left <= maximum; left++)
        {
            for (var right = minimum; right <= maximum; right++)
            {
                if (right is 0)
                {
                    continue;
                }
                
                var result = left / right;
                
                counts.TryGetValue(result, out var count);
                
                counts[result] = count + 1;
            }
        }

        var vault = new Dictionary<int, (int, int)[]>(counts.Count);
        
        var cursors = new Dictionary<int, int>(counts.Count);
        
        foreach (var (result, count) in counts)
        {
            vault[result] = new (int, int)[count];

            cursors[result] = 0;
        }
        
        for (var left = minimum; left <= maximum; left++)
        {
            for (var right = minimum; right <= maximum; right++)
            {
                if (right is 0)
                {
                    continue;
                }
                
                var result = left / right;
                
                var index = cursors[result]++;
                
                vault[result][index] = (left, right);
            }
        }

        _vault[ExpressionType.Divide] = vault;
    }

    private void InitializeModulo(int minimum, int maximum)
    {
        var range = maximum - minimum + 1;
        
        var estimatedCombinations = range * range;
        
        var counts = new Dictionary<int, int>(estimatedCombinations);
        
        for (var left = minimum; left <= maximum; left++)
        {
            for (var right = minimum; right <= maximum; right++)
            {
                if (right is 0)
                {
                    continue;
                }
                
                var result = left % right;
                
                counts.TryGetValue(result, out var count);
                
                counts[result] = count + 1;
            }
        }

        var vault = new Dictionary<int, (int, int)[]>(counts.Count);
        
        var cursors = new Dictionary<int, int>(counts.Count);
        
        foreach (var (result, count) in counts)
        {
            vault[result] = new (int, int)[count];
            
            cursors[result] = 0;
        }
        
        for (var left = minimum; left <= maximum; left++)
        {
            for (var right = minimum; right <= maximum; right++)
            {
                if (right is 0)
                {
                    continue;
                }
                
                var result = left % right;
                
                var index = cursors[result]++;
                
                vault[result][index] = (left, right);
            }
        }

        _vault[ExpressionType.Modulo] = vault;
    }

    private void InitializePower(int minimum, int maximum)
    {
        var range = maximum - minimum + 1;
        
        var estimatedCombinations = range * range;
        
        var counts = new Dictionary<int, int>(estimatedCombinations);
        
        for (var left = minimum; left <= maximum; left++)
        {
            for (var right = minimum; right <= maximum; right++)
            {
                try
                {
                    var result = (int) Math.Pow(left, right);
                    
                    if (result > int.MaxValue / 2 || result < int.MinValue / 2)
                    {
                        continue;
                    }
                    
                    counts.TryGetValue(result, out var count);
                    
                    counts[result] = count + 1;
                }
                catch (OverflowException)
                {
                    continue;
                }
            }
        }

        var vault = new Dictionary<int, (int, int)[]>(counts.Count);

        var cursors = new Dictionary<int, int>(counts.Count);
        
        foreach (var (result, count) in counts)
        {
            vault[result] = new (int, int)[count];

            cursors[result] = 0;
        }
        
        for (var left = minimum; left <= maximum; left++)
        {
            for (var right = minimum; right <= maximum; right++)
            {
                try
                {
                    var result = (int)Math.Pow(left, right);
                    
                    if (result > int.MaxValue / 2 || result < int.MinValue / 2)
                    {
                        continue;
                    }
                    
                    var index = cursors[result]++;

                    vault[result][index] = (left, right);
                }
                catch (OverflowException)
                {
                    continue;
                }
            }
        }

        _vault[ExpressionType.Power] = vault;
    }

    public (int Left, int Right)? GetRandomOperands(Random rng, ExpressionType type, int result)
    {
        if (!_vault.TryGetValue(type, out var results))
        {
            return null;
        }

        if (!results.TryGetValue(result, out var operands))
        {
            return null;
        }

        if (operands.Length is 0)
        {
            return null;
        }

        var index = rng.Next(operands.Length);
        
        return operands[index];
    }

    public bool HasCombination(ExpressionType type, int result)
    {
        if (!_vault.TryGetValue(type, out var results))
        {
            return false;
        }

        if (!results.TryGetValue(result, out var pairs))
        {
            return false;
        }

        return pairs.Length > 0;
    }

    public List<ExpressionType> GetAvailableOperations(int result)
    {
        var expressions = new List<ExpressionType>(_vault.Count);

        foreach (var (type, results) in _vault)
        {
            if (results.TryGetValue(result, out var pairs) && pairs.Length > 0)
            {
                expressions.Add(type);
            }
        }

        return expressions;
    }
}


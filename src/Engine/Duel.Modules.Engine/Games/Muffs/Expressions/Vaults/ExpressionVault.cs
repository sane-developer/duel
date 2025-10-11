namespace Duel.Modules.Engine.Games.Muffs.Expressions.Vaults;

/// <summary>
///     Pre-computed lookup table of operand pairs that produce specific results for each binary operation.
///     This eliminates the need for runtime math decomposition.
///     Should be registered as a singleton and reused across all generators.
/// </summary>
public sealed class ExpressionVault(Random rng)
{
    private readonly Dictionary<ExpressionType, Dictionary<int, (int Left, int Right)[]>> _vault = [];

    /// <summary>
    ///     Creates and initializes the vault with all possible combinations for the given value range.
    ///     This is an expensive operation - call once and reuse.
    /// </summary>
    public static ExpressionVault Create(Random rng, int minimum, int maximum)
    {
        var vault = new ExpressionVault(rng);
        
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

    /// <summary>
    /// Gets a random operand pair that produces the specified result for the given operation type.
    /// Returns null if no combination exists.
    /// </summary>
    public (int Left, int Right)? GetRandomOperands(ExpressionType type, int result)
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

    /// <summary>
    /// Checks if a combination exists that produces the target result for the given operation.
    /// </summary>
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

    /// <summary>
    /// Gets all operation types that can produce the target result.
    /// </summary>
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


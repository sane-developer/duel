namespace Duel.Modules.Engine.Games.Muffs.Expressions.Vaults;

/// <summary>
/// Pre-computed lookup table of operand pairs that produce specific results for each binary operation.
/// This eliminates the need for runtime math decomposition.
/// </summary>
public sealed class ExpressionVault
{
    private readonly Dictionary<ExpressionType, Dictionary<int, List<(int Left, int Right)>>> _vault = [];

    /// <summary>
    /// Creates and initializes the vault with all possible combinations for the given value range.
    /// </summary>
    public static ExpressionVault Create(int minimum, int maximum)
    {
        var vault = new ExpressionVault();
        
        vault.Initialize(minimum, maximum);
        
        return vault;
    }

    private void Initialize(int minimum, int maximum)
    {
        InitializeAddition(minimum, maximum);
        
        InitializeSubtraction(minimum, maximum);
        
        InitializeMultiplication(minimum, maximum);
        
        InitializeDivision(minimum, maximum);
        
        InitializeModulo(minimum, maximum);
        
        InitializePower(minimum, maximum);
    }

    private void InitializeAddition(int minValue, int maxValue)
    {
        var additions = new Dictionary<int, List<(int, int)>>();

        for (var left = minValue; left <= maxValue; left++)
        {
            for (var right = minValue; right <= maxValue; right++)
            {
                var result = left + right;
                
                if (!additions.ContainsKey(result))
                {
                    additions[result] = [];
                }
                
                additions[result].Add((left, right));
            }
        }

        _vault[ExpressionType.Add] = additions;
    }

    private void InitializeSubtraction(int minValue, int maxValue)
    {
        var subtractions = new Dictionary<int, List<(int, int)>>();

        for (int left = minValue; left <= maxValue; left++)
        {
            for (int right = minValue; right <= maxValue; right++)
            {
                var result = left - right;
                
                if (!subtractions.ContainsKey(result))
                {
                    subtractions[result] = new List<(int, int)>();
                }
                
                subtractions[result].Add((left, right));
            }
        }

        _vault[ExpressionType.Subtract] = subtractions;
    }

    private void InitializeMultiplication(int minValue, int maxValue)
    {
        var multiplications = new Dictionary<int, List<(int, int)>>();

        for (int left = minValue; left <= maxValue; left++)
        {
            for (int right = minValue; right <= maxValue; right++)
            {
                var result = left * right;
                
                if (!multiplications.ContainsKey(result))
                {
                    multiplications[result] = new List<(int, int)>();
                }
                
                multiplications[result].Add((left, right));
            }
        }

        _vault[ExpressionType.Multiply] = multiplications;
    }

    private void InitializeDivision(int minValue, int maxValue)
    {
        var divisions = new Dictionary<int, List<(int, int)>>();

        for (int left = minValue; left <= maxValue; left++)
        {
            for (int right = minValue; right <= maxValue; right++)
            {
                // Skip division by zero
                if (right == 0) continue;
                
                var result = left / right;
                
                if (!divisions.ContainsKey(result))
                {
                    divisions[result] = new List<(int, int)>();
                }
                
                divisions[result].Add((left, right));
            }
        }

        _vault[ExpressionType.Divide] = divisions;
    }

    private void InitializeModulo(int minValue, int maxValue)
    {
        var modulos = new Dictionary<int, List<(int, int)>>();

        for (int left = minValue; left <= maxValue; left++)
        {
            for (int right = minValue; right <= maxValue; right++)
            {
                // Skip modulo by zero
                if (right == 0) continue;
                
                var result = left % right;
                
                if (!modulos.ContainsKey(result))
                {
                    modulos[result] = new List<(int, int)>();
                }
                
                modulos[result].Add((left, right));
            }
        }

        _vault[ExpressionType.Modulo] = modulos;
    }

    private void InitializePower(int minValue, int maxValue)
    {
        var powers = new Dictionary<int, List<(int, int)>>();

        for (int left = minValue; left <= maxValue; left++)
        {
            for (int right = minValue; right <= maxValue; right++)
            {
                try
                {
                    var result = (int)Math.Pow(left, right);
                    
                    // Skip if result is too large (overflow protection)
                    if (result > int.MaxValue / 2 || result < int.MinValue / 2)
                        continue;
                    
                    if (!powers.ContainsKey(result))
                    {
                        powers[result] = new List<(int, int)>();
                    }
                    
                    powers[result].Add((left, right));
                }
                catch (OverflowException)
                {
                    // Skip combinations that overflow
                    continue;
                }
            }
        }

        _vault[ExpressionType.Power] = powers;
    }

    /// <summary>
    /// Gets a random operand pair that produces the specified result for the given operation type.
    /// Returns null if no combination exists.
    /// </summary>
    public (int Left, int Right)? GetRandomOperands(ExpressionType operationType, int targetResult, Random rng)
    {
        if (!_vault.TryGetValue(operationType, out var operationResults))
        {
            return null;
        }

        if (!operationResults.TryGetValue(targetResult, out var operandPairs))
        {
            return null;
        }

        if (operandPairs.Count == 0)
        {
            return null;
        }

        var index = rng.Next(operandPairs.Count);
        return operandPairs[index];
    }

    /// <summary>
    /// Checks if a combination exists that produces the target result for the given operation.
    /// </summary>
    public bool HasCombination(ExpressionType operationType, int targetResult)
    {
        if (!_vault.TryGetValue(operationType, out var operationResults))
        {
            return false;
        }

        return operationResults.ContainsKey(targetResult) && operationResults[targetResult].Count > 0;
    }

    /// <summary>
    /// Gets all operation types that can produce the target result.
    /// </summary>
    public List<ExpressionType> GetAvailableOperations(int targetResult)
    {
        var availableOps = new List<ExpressionType>();

        foreach (var kvp in _vault)
        {
            if (kvp.Value.ContainsKey(targetResult) && kvp.Value[targetResult].Count > 0)
            {
                availableOps.Add(kvp.Key);
            }
        }

        return availableOps;
    }
}


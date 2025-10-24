# Muffs Expression System - Design Document

**Status:** ✅ Finalized  
**Last Updated:** October 24, 2025

---

## Executive Summary

The Muffs expression generator uses a **target-driven, spine-based architecture** that guarantees constraint satisfaction while providing rich, varied expressions suitable for mental math challenges.

### Core Architecture

**Spine + Recursive Subtrees:**
```
Expression: (((base op operand₁) op operand₂) ... op operandₙ)
            └────────── spine of N operators ──────────┘
                        │            │                │
                   depth≤D      depth≤D          depth≤D
```

**Target-Driven Generation:**
1. Operator picks target values from its `OperandRange`
2. Sub-expressions are built to produce exactly those targets
3. Uses pre-computed compositions (e.g., "11 can be made via 4+7, 3+8, 22/2...")
4. At depth 0: returns `Number(target)` directly

**Key Properties:**
- ✅ Exact length control (number of operations)
- ✅ Independent depth control (operand complexity)
- ✅ Zero constraint violations (by design)
- ✅ Per-operator flexibility (different ranges for different operators)
- ✅ Rich variation (same target, many constructions)

### Design Decisions Summary

| # | Decision | Choice | Rationale |
|---|----------|--------|-----------|
| **1** | Operand Ranges | **Per-Operator** | Enables factorial(0..5), negation(-50..50), add(1..10) independently |
| **2** | Leaf Generation | **Target-Driven** | Leaves are target values from parent's operandRange, not random picks |
| **3** | Constraint Enforcement | **Design Prevents** | Parents pick valid targets; children produce them. No violations possible. |
| **4** | Generation Strategy | **Composition-Based** | Use pre-cached valid operations to build target values |
| **5** | Complexity Controls | **Length + Depth + Ranges** | Independent control: spine size, subtree complexity, value bounds |
| **6** | Tree Structure | **Spine + Recursion** | Iterative spine for length, recursive subtrees for depth |

---

## Settings Structure

```csharp
ExpressionSettings {
    Length: 3..7,  // Number of operators in spine
    Depth: 0..2,   // Maximum depth of operand subtrees
    
    Addition: {
        Weight: 1.0,           // Selection probability
        OperandRange: -20..30, // Valid inputs (targets picked from here)
        ResultRange: -50..100  // Valid outputs (compositions filtered by this)
    },
    
    Subtraction: { ... },
    Multiplication: { ... },
    Division: { ... },
    // ... other operators
}
```

**No `LiteralRange` needed** - leaf values come from operator targets.

---

## Generation Algorithm

### Main Generation

```csharp
Generate(Random rng):
    length = Random(settings.Length)
    
    // Start with initial operand
    operator = PickWeightedOperator(rng)
    leftTarget = Random(operator.OperandRange)
    current = GenerateOperand(leftTarget, Random(settings.Depth), rng)
    
    // Build spine
    for i in 0..length:
        operator = PickWeightedOperator(rng)
        rightTarget = Random(operator.OperandRange)
        right = GenerateOperand(rightTarget, Random(settings.Depth), rng)
        current = CreateBinaryOperator(operator, current, right)
    
    return current
```

### Target-Driven Operand Generation

```csharp
GenerateOperand(target, maxDepth, rng):
    if maxDepth == 0:
        return Number(target)
    
    compositions = cache.GetCompositions(target)
    if compositions.isEmpty():
        return Number(target)  // Fallback if unreachable
    
    composition = compositions.Random(rng)
    
    if composition is BinaryComposition(op, lhs, rhs, result):
        left = GenerateOperand(lhs, maxDepth-1, rng)
        right = GenerateOperand(rhs, maxDepth-1, rng)
        return CreateBinaryOperator(op, left, right)
    
    if composition is UnaryComposition(op, operand, result):
        child = GenerateOperand(operand, maxDepth-1, rng)
        return CreateUnaryOperator(op, child)
    
    return Number(target)
```

---

## Worked Example

**Configuration:**
- Length: 2
- Depth: 1
- Subtract: operandRange 1..11, weight 1.0
- Add: operandRange 1..10, weight 1.0
- Divide: operandRange 1..20, weight 1.0

**Generation Trace:**

1. **Initial operand:**
   - Pick Subtract, roll target 11
   - GenerateOperand(11, depth=1)
   - GetCompositions(11) → finds (Add, 4, 7, 11)
   - Builds `(4 + 7)`

2. **Spine iteration 1:**
   - Pick Subtract, roll target 2
   - GenerateOperand(2, depth=1)
   - GetCompositions(2) → finds (Divide, 20, 10, 2)
   - Builds `(20 / 10)`
   - Creates `(4 + 7) - (20 / 10)`

3. **Spine iteration 2:**
   - Pick Add, roll target 5
   - GenerateOperand(5, depth=0)
   - Returns `5`
   - Creates `((4 + 7) - (20 / 10)) + 5`

**Result:** `((4 + 7) - (20 / 10)) + 5` = 11 - 2 + 5 = 14

**Properties:**
- Length: 3 operators (Add, Subtract, Add)
- Depths: Operands are ≤1 level deep
- Constraints: All targets within operator ranges
- Valid: Evaluates correctly

---

## Implementation Components

### MuffsCache

**Responsibilities:**
- Pre-compute all valid compositions for each operator
- Cache `Number` instances for all operand ranges + composition results
- Provide `GetCompositions(value)` lookup

**Construction:**
```csharp
MuffsCache(ExpressionSettings settings):
    For each operator with operatorSettings:
        For each (lhs, rhs) in operandRange × operandRange:
            if operation is valid (no div/0, no overflow, etc.):
                result = operation(lhs, rhs)
                if result in resultRange:
                    Add composition: (operator, lhs, rhs, result)
                    Track numbers: lhs, rhs, result
    
    Build frozen dictionaries:
        _numbers: value → Number instance
        _compositions: value → Composition[]
        _divisors: value → int[] (for division)
```

### ExpressionContext

**Responsibilities:**
- Facade to MuffsCache
- Random selection helpers

**Methods:**
```csharp
GetDepth(Random rng) → int
GetLength(Random rng) → int
GetOperatorType(Random rng) → OperatorType (weighted)
GetNumber(int value) → Number
GetCompositions(int value) → Composition[]
GetDivisors(int value) → int[]
```

### ExpressionGenerator

**Responsibilities:**
- Implement spine + target-driven generation
- Use ExpressionContext for all randomness and cache access

**Structure:**
```csharp
ExpressionGenerator(ExpressionContext context)
    
Glyph Generate(Random rng)
    // Spine generation (see algorithm above)

Glyph GenerateOperand(int target, int maxDepth, Random rng)
    // Target-driven recursive generation (see algorithm above)
```

---

## Open Questions

1. ~~Operand range validation?~~ **RESOLVED** - Target-driven prevents violations
2. **Global result overflow cap?** May need max absolute value check
3. **Empty compositions fallback?** Use `Number(target)` directly

---

## Implementation Checklist

- [ ] Update `ExpressionSettings`: add `Length` range, remove `Budget`
- [ ] Update `ExpressionSettingsRegistry`: Easy/Medium/Hard with Length+Depth
- [ ] Update `MuffsCache`: ensure all operand range numbers cached
- [ ] Rewrite `ExpressionGenerator`: spine + target-driven
- [ ] Update `ExpressionContext`: add `GetLength()` method
- [ ] Write tests: spine length, target accuracy, constraint satisfaction
- [ ] Update existing tests to match new architecture

---

## Terminology

- **Length**: Number of operators in the main expression spine
- **Depth**: Maximum height of subtree operands
- **Operand Range**: Valid values an operator accepts as inputs
- **Result Range**: Valid values an operation produces as outputs
- **Target**: Specific value a sub-expression must produce
- **Composition**: Pre-computed valid operation (e.g., "3×4→12")
- **Spine**: Left-associative chain of operations forming the main expression
- **Subtree**: Recursive sub-expression built to produce a target value

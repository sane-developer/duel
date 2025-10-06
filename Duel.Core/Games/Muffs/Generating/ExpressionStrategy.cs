using Duel.Core.Games.Muffs.AST;

namespace Duel.Core.Games.Muffs.Generating;

public interface IExpressionStrategy
{
    bool IsReady(int depth);

    Operator.Code GetOperatorCode();

    int GetConstant();

    int GetDivisor(int dividend);

    int GetExponent();
}
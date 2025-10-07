using Duel.Modules.Engine.Games.Muffs.AST;

namespace Duel.Modules.Engine.Games.Muffs.Generating;

public interface IExpressionStrategy
{
    bool IsReady(int depth);

    Operator.Code GetOperatorCode();

    int GetConstant();

    int GetDivisor(int dividend);

    int GetExponent();
}
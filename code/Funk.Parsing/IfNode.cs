using System.Collections.Generic;

namespace Funk.Parsing
{
    public record IfNode(IReadOnlyList<IfThenNode> IfThens, FunctionEvaluationStatementNode ElseExpression);
}
using System.Linq.Expressions;

namespace Funk.Parsing
{
    public record ParameterAccessNode(FunctionEvaluationStatementNode Expression, IReadOnlyList<string> Names);
}
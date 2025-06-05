using System.Linq.Expressions;

namespace Funk.Parsing
{
    public record ParameterAccessScript(ExpressionScript Expression, IReadOnlyList<string> Names);
}

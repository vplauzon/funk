using System.Linq.Expressions;

namespace Funk.Parsing
{
    public record ParameterAccessScript(ExpressionScript Expression, string[] Names);
}
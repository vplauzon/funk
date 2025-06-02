using System.Linq.Expressions;

namespace Funk.Parsing
{
    public record ParameterAccessScript(string Name, ExpressionScript Expression);
}
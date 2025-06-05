using System.Collections.Generic;

namespace Funk.Parsing
{
    public record IfScript(IReadOnlyList<IfThenScript> IfThens, ExpressionScript ElseExpression);
}
namespace Funk.Parsing
{
    public record Script(IReadOnlyList<RuleScript> Rules, ExpressionScript? Expression);
}
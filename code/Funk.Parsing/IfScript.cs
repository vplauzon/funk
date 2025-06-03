namespace Funk.Parsing
{
    public record IfScript(
        ChainedIfElseScript? ChainedIfElse = null,
        TernaryIfScript? TernaryIf = null);
}

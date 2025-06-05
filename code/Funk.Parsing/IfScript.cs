namespace Funk.Parsing
{
    public record IfScript(
        TernaryIfScript? TernaryIf = null,
        ChainedIfElseScript? ChainedIfElse = null);
}

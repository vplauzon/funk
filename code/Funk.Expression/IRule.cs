namespace Funk.Expression
{
    internal interface IRule
    {
        string Namespace { get; }

        string Name { get; }

        ExpressionBase? Transform(ExpressionBase expression);
    }
}
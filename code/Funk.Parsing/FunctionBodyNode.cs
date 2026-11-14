namespace Funk.Parsing
{
    public record FunctionBodyNode(
        IReadOnlyList<ImportStatementNode> ImportStatements,
        IReadOnlyList<FunctionDeclarationNode> FunctionDeclarations,
        FunctionEvaluationNode FunctionEvaluation);
}
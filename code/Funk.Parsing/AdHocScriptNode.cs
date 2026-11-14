namespace Funk.Parsing
{
    public record AdHocScriptNode(
        IReadOnlyList<ImportStatementNode> ImportStatements,
        IReadOnlyList<FunctionDeclarationNode> FunctionDeclarations,
        FunctionEvaluationStatementNode? FunctionEvaluation);
}
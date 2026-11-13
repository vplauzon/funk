namespace Funk.Parsing
{
    public record AdHocScriptNode(FunctionEvaluationStatementNode? FunctionEvaluationStatement)
    {
        public FunctionBodyNode ToFunctionBodyNode()
        {
            if (FunctionEvaluationStatement == null)
            {
                throw new NullReferenceException(nameof(FunctionEvaluationStatement));
            }
         
            return new FunctionBodyNode(FunctionEvaluationStatement);
        }
    }
}
namespace Funk.Parsing
{
    public record AdHocScriptNode(FunctionEvaluationStatementNode? FunctionEvaluation)
    {
        public FunctionBodyNode ToFunctionBodyNode()
        {
            if (FunctionEvaluation == null)
            {
                throw new NullReferenceException(nameof(FunctionEvaluation));
            }
         
            return new FunctionBodyNode(FunctionEvaluation);
        }
    }
}
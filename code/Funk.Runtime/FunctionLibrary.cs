using Funk.Parsing;
using Funk.Runtime.Core;

namespace Funk.Runtime
{
    internal class FunctionLibrary
    {
        #region Inner types
        private class FunctionLibraryView : IFunctionLibrary
        {
            private readonly FunctionLibrary _parentLibrary;
            private readonly int _maxIndex;

            public FunctionLibraryView(FunctionLibrary parentLibrary, int maxIndex)
            {
                _parentLibrary = parentLibrary;
                _maxIndex = maxIndex;
            }
        }

        private record FunctionDeclarationItem(
            int Index,
            FunctionDeclaration FunctionDeclaration,
            FunctionLibrary FunctionLibrary,
            FunctionEvaluationResolver FunctionEvaluationResolver);
        #endregion

        private readonly IFunctionLibrary? _parentLibrary;
        private readonly string[] _importModules;
        private readonly IDictionary<string, FunctionDeclarationItem[]> _functionDeclarationIndex;

        public FunctionLibrary(
            IEnumerable<ImportStatementNode> importStatements,
            IEnumerable<FunctionDeclarationNode> functionDeclarations,
            IFunctionLibrary? parentLibrary = null)
        {
            _parentLibrary = parentLibrary;
            _importModules = importStatements
                .Select(i => i.ModuleName)
                .ToArray();
            _functionDeclarationIndex = functionDeclarations
                .Index()
                .Select(p => new FunctionDeclarationItem(
                    p.Index,
                    new FunctionDeclaration(p.Item.Name),
                    new FunctionLibrary(
                        p.Item.Body.ImportStatements,
                        p.Item.Body.FunctionDeclarations),
                    new FunctionEvaluationResolver(
                        p.Item.Body.FunctionEvaluation,
                        GetFunctionLibraryView(p.Index))
                ))
                .GroupBy(i => i.FunctionDeclaration.Name)
                .ToDictionary(g => g.Key, g => g.ToArray());
        }

        public IFunctionLibrary GetFunctionLibraryView()
        {
            if (_functionDeclarationIndex.Count > 0)
            {
                var maxIndex = _functionDeclarationIndex.Values
                    .SelectMany(i => i)
                    .Max(i => i.Index);

                return GetFunctionLibraryView(maxIndex);
            }
            else
            {
                return GetFunctionLibraryView(-1);
            }
        }

        private IFunctionLibrary GetFunctionLibraryView(int maxIndex)
        {
            return new FunctionLibraryView(this, maxIndex);
        }
    }
}
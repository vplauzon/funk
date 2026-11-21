using Funk.Parsing;
using Funk.Runtime.Core;

namespace Funk.Runtime
{
    internal class FunctionLibrary
    {
        #region Inner types
        private class FunctionLibraryView : IFunctionLibrary
        {
            private readonly FunctionLibrary _proxiedLibrary;
            private readonly int _maxIndex;

            public FunctionLibraryView(FunctionLibrary proxiedLibrary, int maxIndex)
            {
                _proxiedLibrary = proxiedLibrary;
                _maxIndex = maxIndex;
            }

            FunctionDefinitionPointer IFunctionLibrary.GetFunctionDefinition(string functionName)
            {
                return _proxiedLibrary.GetFunctionDefinition(functionName, _maxIndex);
            }
        }

        private record FunctionDeclarationItem(
            int Index,
            FunctionDeclaration FunctionDeclaration,
            FunctionLibrary FunctionLibrary,
            FunctionEvaluationResolver FunctionEvaluationResolver);
        #endregion

        private readonly string _moduleName;
        private readonly IFunctionLibrary? _parentLibrary;
        private readonly string[] _importModules;
        private readonly IDictionary<string, FunctionDeclarationItem> _functionDeclarationIndex;

        public FunctionLibrary(
            string moduleName,
            IEnumerable<string> defaultModuleNames,
            IEnumerable<ImportStatementNode> importStatements,
            IEnumerable<FunctionDeclarationNode> functionDeclarations,
            IFunctionLibrary? parentLibrary = null)
        {
            _moduleName = moduleName;
            _parentLibrary = parentLibrary;
            _importModules = importStatements
                .Select(i => i.ModuleName)
                .Concat(defaultModuleNames)
                .Distinct()
                .ToArray();
            _functionDeclarationIndex = functionDeclarations
                .Index()
                .Select(p => new FunctionDeclarationItem(
                    p.Index,
                    new FunctionDeclaration(p.Item.Name),
                    new FunctionLibrary(
                        string.Empty,
                        Array.Empty<string>(),
                        p.Item.Body.ImportStatements,
                        p.Item.Body.FunctionDeclarations),
                    new FunctionEvaluationResolver(
                        p.Item.Body.FunctionEvaluation,
                        GetFunctionLibraryView(p.Index))
                ))
                .ToDictionary(i => i.FunctionDeclaration.Name);
        }

        public IFunctionLibrary GetFunctionLibraryView()
        {
            if (_functionDeclarationIndex.Count > 0)
            {
                var maxIndex = _functionDeclarationIndex.Values
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

        private FunctionDefinitionPointer GetFunctionDefinition(string functionName, int maxIndex)
        {
            if (_functionDeclarationIndex.TryGetValue(functionName, out var functionDeclarationItem)
                && functionDeclarationItem.Index <= maxIndex)
            {
                return new FunctionDefinitionPointer(
                    string.IsNullOrWhiteSpace(_moduleName)
                    ? functionName
                    : $"{_moduleName}.{functionName}",
                    () =>
                    {
                        var resolver = functionDeclarationItem.FunctionEvaluationResolver;
                        var simplifiedResolver = resolver.Simplify();

                        return simplifiedResolver.FunctionEvaluation;
                    });
            }
            if (_parentLibrary != null)
            {
                return _parentLibrary.GetFunctionDefinition(functionName);
            }
            else
            {
                throw new FunkException($"Can't find function definition for '{functionName}'");
            }
        }
    }
}
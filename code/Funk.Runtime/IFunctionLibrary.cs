using Funk.Parsing;
using Funk.Runtime.Core;

namespace Funk.Runtime
{
    public interface IFunctionLibrary
    {
        FunctionDefinitionPointer GetFunctionDefinition(string functionName);
    }
}
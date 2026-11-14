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
         
            public FunctionLibraryView(FunctionLibrary parentLibrary)
            {
                _parentLibrary = parentLibrary;
            }
        }
        #endregion

        public IFunctionLibrary GetFunctionLibraryView()
        {
            return new FunctionLibraryView(this);
        }
    }
}
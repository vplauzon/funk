using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funk.Expression
{
    internal class ExpressionTransformer
    {
        #region Inner Types
        private record RuleKey(string Namespace, string Name);
        #endregion

        private readonly IImmutableDictionary<RuleKey, IRule> _ruleMap;

        #region Constructors
        public ExpressionTransformer()
        {
            _ruleMap = ImmutableDictionary<RuleKey, IRule>.Empty;
        }
        #endregion

        public ExpressionBase Transform(ExpressionBase expression)
        {
            throw new NotImplementedException();
        }
    }
}
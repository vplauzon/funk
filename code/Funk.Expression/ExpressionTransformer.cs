using Funk.Expression.Rules;
using Funk.Parsing;
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

        private readonly IImmutableDictionary<RuleKey, IImmutableList<IRule>> _builtInRuleMap =
            CreateBuiltInRuleMap();

        private readonly IImmutableDictionary<RuleKey, IImmutableList<IRule>> _ruleMap;

        #region Constructors
        public ExpressionTransformer()
        {
            _ruleMap = _builtInRuleMap;
        }
        private static IImmutableDictionary<RuleKey, IImmutableList<IRule>> CreateBuiltInRuleMap()
        {
            var rules = new IRule[]
            {
                new BinaryArithmeticRule(BinaryArithmeticOperand.Add),
                new BinaryArithmeticRule(BinaryArithmeticOperand.Subtract),
                new BinaryArithmeticRule(BinaryArithmeticOperand.Product),
                new BinaryArithmeticRule(BinaryArithmeticOperand.Division),
                new BinaryArithmeticRule(BinaryArithmeticOperand.Power)
            };
            var map = rules
                .GroupBy(r => new RuleKey(r.Namespace, r.Name))
                .ToImmutableDictionary(g => g.Key, g => (IImmutableList<IRule>)g.ToImmutableArray());

            return map;
        }
        #endregion

        public ExpressionBase Transform(ExpressionBase expression)
        {
            var transformed = TransformInternal(expression);

            return transformed ?? expression;
        }

        public ExpressionBase? TransformInternal(ExpressionBase expression)
        {
            throw new NotImplementedException();
        }
    }
}
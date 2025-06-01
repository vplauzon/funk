using Funk.Expression.Expressions;
using Funk.Expression.Rules;
using Funk.Parsing;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
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
                new BinaryArithmeticPerformRule(BinaryArithmeticOperand.Add),
                new BinaryArithmeticPerformRule(BinaryArithmeticOperand.Substract),
                new BinaryArithmeticPerformRule(BinaryArithmeticOperand.Product),
                new BinaryArithmeticPerformRule(BinaryArithmeticOperand.Division),
                new BinaryArithmeticPerformRule(BinaryArithmeticOperand.Power),
                new RationalSimplificationRule(),
                new RationalAddRule(),
                new ToFloatRule()
            };
            var map = rules
                .GroupBy(r => new RuleKey(r.Namespace, r.Name))
                .ToImmutableDictionary(g => g.Key, g => (IImmutableList<IRule>)g.ToImmutableArray());

            return map;
        }
        #endregion

        public ExpressionBase Transform(ExpressionBase expression)
        {
            var transformed = TransformExpression(expression);

            return transformed ?? expression;
        }

        private ExpressionBase? TransformExpression(ExpressionBase expression)
        {
            var functionInvoke = expression as FunctionInvokeExpression;

            if (functionInvoke != null)
            {
                return TransformFunctionInvoke(functionInvoke);
            }
            else
            {
                return null;
            }
        }

        private ExpressionBase? TransformFunctionInvoke(FunctionInvokeExpression expression)
        {
            var transformedParameters = TransformParameters(expression.Parameters);
            var ruleKey = new RuleKey(expression.Namespace, expression.Name);
            var transformedExpression = ApplyRules(
                ruleKey,
                transformedParameters ?? expression.Parameters);

            if (transformedExpression != null)
            {   //  Function invoke was transformed
                return transformedExpression;
            }
            else if (transformedParameters != null)
            {   //  But parameters were
                return new FunctionInvokeExpression(
                    ruleKey.Namespace,
                    ruleKey.Name,
                    transformedParameters);
            }
            else
            {   //  Nothing is transformed
                return null;
            }
        }

        private ExpressionBase? ApplyRules(
            RuleKey ruleKey,
            IImmutableList<FunctionParameter> parameters)
        {
            if (_ruleMap.TryGetValue(ruleKey, out var rules))
            {
                foreach (var rule in rules)
                {
                    var transformedExpression = rule.Transform(parameters);

                    if (transformedExpression != null)
                    {
                        return TransformExpression(transformedExpression)
                            ?? transformedExpression;
                    }
                }
            }
         
            return null;
        }

        private IImmutableList<FunctionParameter>? TransformParameters(
            IImmutableList<FunctionParameter> parameters)
        {
            var transformedParameterExpressions = parameters
               .Select(p => TransformExpression(p.Expression))
               .ToImmutableArray();

            if (transformedParameterExpressions.Any(p => p != null))
            {
                var transformedParameters = parameters
                    .Zip(transformedParameterExpressions, (p, tp) => new
                    {
                        Parameter = p,
                        TransformedParameter = tp
                    })
                    .Select(o => o.TransformedParameter == null
                    ? o.Parameter
                    : new FunctionParameter(o.Parameter.Name, o.TransformedParameter))
                    .ToImmutableArray();

                return transformedParameters;
            }
            else
            {
                return null;
            }
        }
    }
}
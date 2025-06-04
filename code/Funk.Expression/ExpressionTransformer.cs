using Funk.Expression.Expressions;
using Funk.Expression.Rules;
using Funk.Expression.Rules.BinaryOperations.Arithmetic;
using Funk.Expression.Rules.BinaryOperations.Logical;
using Funk.Parsing;
using System.Collections.Immutable;

namespace Funk.Expression
{
    internal class ExpressionTransformer
    {
        #region Inner Types
        private record RuleKey(string Namespace, string Name);

        private record ExpressionVisitor<T>(
                Func<PrimitiveExpression, T> PrimitiveFunc,
                Func<FunctionInvokeExpression, T> FunctionInvokeFunc,
                Func<ParameterAccessExpression, T> ParameterAccessFunc) : IExpressionVisitor<T>
        {
            T IExpressionVisitor<T>.VisitPrimitive(PrimitiveExpression expression)
            {
                return PrimitiveFunc(expression);
            }

            T IExpressionVisitor<T>.VisitFunctionInvoke(FunctionInvokeExpression expression)
            {
                return FunctionInvokeFunc(expression);
            }

            T IExpressionVisitor<T>.VisitParameterAccess(ParameterAccessExpression expression)
            {
                return ParameterAccessFunc(expression);
            }
        }
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
                new AddPerformRule(),
                new SubtractPerformRule(),
                new ProductPerformRule(),
                new DivisionPerformRule(),
                new PowerPerformRule(),
                new RationalSimplificationRule(),
                new RationalAddRule(),
                new GreatestCommonDivisorRule(),
                new ToFloatRule(),
                new GreaterOrEqualPerformRule(),
                new GreaterPerformRule(),
                new LesserOrEqualPerformRule(),
                new LesserPerformRule(),
                new AndPerformRule(),
                new OrPerformRule()
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
            return expression.Visit(new ExpressionVisitor<ExpressionBase?>(
                e => null,
                e => TransformFunctionInvoke(e),
                e => TransformParameterAccess(e)));
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

        private ExpressionBase? TransformParameterAccess(ParameterAccessExpression expression)
        {
            var transformedExpression = TransformExpression(expression.Expression);
            var resultingExpression = transformedExpression ?? expression.Expression;
            var name = expression.Name;

            return resultingExpression.Visit(new ExpressionVisitor<ExpressionBase?>(
                e => throw new FunkRuntimeException(
                    $"Primitive expression doesn't have parameters:  " +
                    $"'{resultingExpression}.{name}'"),
                e =>
                {
                    var rules = _ruleMap[new RuleKey(e.Namespace, e.Name)];
                    var sampleRule = rules.First();
                    var index = sampleRule.ParameterNames.IndexOf(name);

                    if (index == -1)
                    {
                        throw new FunkRuntimeException(
                            $"Parameter '{name}' doesn't exist on " +
                            $"expression '{resultingExpression}'");
                    }
                    else if (index >= e.Parameters.Count)
                    {
                        throw new InvalidDataException(
                            "Parameter index shouldn't be out of bound");
                    }
                    else
                    {
                        return e.Parameters[index];
                    }
                },
                e => throw new InvalidOperationException(
                    $"Shouldn't have a parameter access at this point:  {e}")));
        }

        private ExpressionBase? ApplyRules(
            RuleKey ruleKey,
            IImmutableList<ExpressionBase> parameters)
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

        private IImmutableList<ExpressionBase>? TransformParameters(
            IImmutableList<ExpressionBase> parameters)
        {
            var transformedParameterExpressions = parameters
               .Select(p => TransformExpression(p))
               .ToImmutableArray();

            if (transformedParameterExpressions.Any(p => p != null))
            {
                var transformedParameters = parameters
                    .Zip(transformedParameterExpressions, (p, tp) => new
                    {
                        OriginalParameter = p,
                        TransformedParameter = tp
                    })
                    .Select(o => o.TransformedParameter ?? o.OriginalParameter)
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
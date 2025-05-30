﻿using Funk.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funk.Expression
{
    public class ExpressionBuilder
    {
        private ExpressionTransformer _expressionTransformer = new();

        public ExpressionBase? ProcessScript(Script script)
        {
            if (script.Rules.Any())
            {
                throw new NotImplementedException("Rules aren't implemented");
            }
            if (script.Expression != null)
            {
                return ExpressionFactory.Create(script.Expression);
            }
            else
            {
                return null;
            }
        }

        public ExpressionBase Transform(ExpressionBase expression)
        {
            return _expressionTransformer.Transform(expression);
        }
    }
}
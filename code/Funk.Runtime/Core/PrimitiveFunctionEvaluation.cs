using System;
using System.Collections.Generic;
using System.Text;

namespace Funk.Runtime.Core
{
    public class PrimitiveFunctionEvaluation : FunctionEvaluationBase
    {
        #region Inner Types
        private enum PrimitiveType
        {
            Boolean,
            Integer,
            Double,
            String
        }
        #endregion

        private readonly PrimitiveType _primitiveType;

        public PrimitiveFunctionEvaluation(bool value)
        {
            Value = value;
            _primitiveType = PrimitiveType.Boolean;
        }

        public PrimitiveFunctionEvaluation(long value)
        {
            Value = value;
            _primitiveType = PrimitiveType.Integer;
        }

        public PrimitiveFunctionEvaluation(double value)
        {
            Value = value;
            _primitiveType = PrimitiveType.Double;
        }

        public PrimitiveFunctionEvaluation(string value)
        {
            Value = value;
            _primitiveType = PrimitiveType.String;
        }

        public object Value { get; }

        public override T Visit<T>(
            Func<PrimitiveFunctionEvaluation, T> primitiveVisitor,
            Func<NodeFunctionEvaluation, T> nodeVisitor,
            Func<UnboundFunctionEvaluation, T> unboundVisitor,
            Func<BoundFunctionEvaluation, T> boundVisitor)
        {
            return primitiveVisitor(this);
        }

        public override string ToScriptText()
        {
            switch(_primitiveType)
            {
                case PrimitiveType.Boolean:
                    return Value.ToString()?.ToLowerInvariant() ?? string.Empty;
                case PrimitiveType.Integer:
                    return Value.ToString() ?? string.Empty;
                case PrimitiveType.Double:
                    return Value.ToString() ?? string.Empty;
                case PrimitiveType.String:
                    return $"\"{Value}\"";

                default:
                    throw new NotImplementedException($"Primitive type {_primitiveType} is not implemented.");
            }
        }
    }
}
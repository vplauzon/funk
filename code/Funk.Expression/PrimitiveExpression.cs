using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funk.Expression
{
    public record PrimitiveExpression(
        PrimitiveCategory PrimitiveCategory,
        object Primitive) : ExpressionBase
    {
        public static PrimitiveExpression Create(object primitive)
        {
            var category = primitive switch
            {
                bool => PrimitiveCategory.Boolean,
                int => PrimitiveCategory.Integer,
                double => PrimitiveCategory.Float,
                string => PrimitiveCategory.String,
                _ => throw new ArgumentException("Unsupported primitive type", nameof(primitive))
            };

            return new PrimitiveExpression(category, primitive);
        }

        public bool ToBoolean()
        {
            if (PrimitiveCategory != PrimitiveCategory.Boolean)
            {
                throw new InvalidCastException(
                    $"Primitive is of type '{PrimitiveCategory}' but boolean is requested");
            }

            return (bool)Primitive;
        }

        public int ToInteger()
        {
            if (PrimitiveCategory != PrimitiveCategory.Integer)
            {
                throw new InvalidCastException(
                    $"Primitive is of type '{PrimitiveCategory}' but integer is requested");
            }

            return (int)Primitive;
        }

        public double ToFloat()
        {
            if (PrimitiveCategory != PrimitiveCategory.Float)
            {
                throw new InvalidCastException(
                    $"Primitive is of type '{PrimitiveCategory}' but float is requested");
            }

            return (double)Primitive;
        }

        public string ToStringPrimitive()
        {
            if (PrimitiveCategory != PrimitiveCategory.String)
            {
                throw new InvalidCastException(
                    $"Primitive is of type '{PrimitiveCategory}' but string is requested");
            }

            return (string)Primitive;
        }
    }
}
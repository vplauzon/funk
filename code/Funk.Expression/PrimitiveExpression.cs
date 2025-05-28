using Funk.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funk.Expression
{
    internal record PrimitiveExpression(
        PrimitiveCategory PrimitiveCategory,
        object Primitive) : ExpressionBase
    {
        #region Constructors
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

        public static PrimitiveExpression Create(PrimitiveScript script)
        {
            if(script.Boolean!=null)
            {
                return new PrimitiveExpression(PrimitiveCategory.Boolean, script.Boolean.Value);
            }
            else if (script.Integer != null)
            {
                return new PrimitiveExpression(PrimitiveCategory.Integer, script.Integer.Value);
            }
            else if (script.Float != null)
            {
                return new PrimitiveExpression(PrimitiveCategory.Float, script.Float.Value);
            }
            else if (script.String != null)
            {
                return new PrimitiveExpression(PrimitiveCategory.String, script.String);
            }
            else
            {
                throw new ArgumentException("Unsupported primitive type", nameof(script));
            }
        }
        #endregion

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
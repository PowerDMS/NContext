// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpressionTreeVisitor.cs">
//   This file is part of NContext.
//
//   NContext is free software: you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or any later version.
//
//   NContext is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU General Public License for more details.
//
//   You should have received a copy of the GNU General Public License
//   along with NContext.  If not, see <http://www.gnu.org/licenses/>.
// </copyright>
//
// <summary>
//   Base abstract implementation for classes which require traversing, examining or copying an expression tree.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace NContext.Persistence
{
    /// <summary>
    ///   Base abstract implementation for classes which require traversing, examining or copying an expression tree.
    ///   <see ref="http://msdn.microsoft.com/en-us/library/bb882521%28VS.90%29.aspx" />
    /// </summary>
    public abstract class ExpressionTreeVisitor
    {
        /// <summary>
        /// Visits the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public virtual Expression Visit(Expression expression)
        {
            if (expression == null)
            {
                return null;
            }

            switch (expression.NodeType)
            {
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                case ExpressionType.Not:
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.ArrayLength:
                case ExpressionType.Quote:
                case ExpressionType.TypeAs:
                    return VisitUnary((UnaryExpression) expression);
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Divide:
                case ExpressionType.Modulo:
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.Coalesce:
                case ExpressionType.ArrayIndex:
                case ExpressionType.RightShift:
                case ExpressionType.LeftShift:
                case ExpressionType.ExclusiveOr:
                    return VisitBinary((BinaryExpression) expression);
                case ExpressionType.TypeIs:
                    return VisitTypeIs((TypeBinaryExpression) expression);
                case ExpressionType.Conditional:
                    return VisitConditional((ConditionalExpression) expression);
                case ExpressionType.Constant:
                    return VisitConstant((ConstantExpression) expression);
                case ExpressionType.Parameter:
                    return VisitParameter((ParameterExpression) expression);
                case ExpressionType.MemberAccess:
                    return VisitMemberAccess((MemberExpression) expression);
                case ExpressionType.Call:
                    return VisitMethodCall((MethodCallExpression) expression);
                case ExpressionType.Lambda:
                    return VisitLambda((LambdaExpression) expression);
                case ExpressionType.New:
                    return VisitNew((NewExpression) expression);
                case ExpressionType.NewArrayInit:
                case ExpressionType.NewArrayBounds:
                    return VisitNewArray((NewArrayExpression) expression);
                case ExpressionType.Invoke:
                    return VisitInvocation((InvocationExpression) expression);
                case ExpressionType.MemberInit:
                    return VisitMemberInit((MemberInitExpression) expression);
                case ExpressionType.ListInit:
                    return VisitListInit((ListInitExpression) expression);
                default:
                    throw new Exception(string.Format("Unhandled expression type: '{0}'", expression.NodeType));
            }
        }

        protected virtual MemberBinding VisitBinding(MemberBinding binding)
        {
            switch (binding.BindingType)
            {
                case MemberBindingType.Assignment:
                    return VisitMemberAssignment((MemberAssignment) binding);
                case MemberBindingType.MemberBinding:
                    return VisitMemberMemberBinding((MemberMemberBinding) binding);
                case MemberBindingType.ListBinding:
                    return VisitMemberListBinding((MemberListBinding) binding);
                default:
                    throw new Exception(String.Format("Unhandled binding type '{0}'", binding.BindingType));
            }
        }

        protected virtual ElementInit VisitElementInitializer(ElementInit initializer)
        {
            ReadOnlyCollection<Expression> arguments = VisitExpressionList(initializer.Arguments);
            if (arguments != initializer.Arguments)
            {
                return Expression.ElementInit(initializer.AddMethod, arguments);
            }

            return initializer;
        }

        protected virtual Expression VisitUnary(UnaryExpression unaryExpression)
        {
            Expression operand = Visit(unaryExpression.Operand);
            if (operand != unaryExpression.Operand)
            {
                return Expression.MakeUnary(unaryExpression.NodeType, operand, unaryExpression.Type, unaryExpression.Method);
            }

            return unaryExpression;
        }

        protected virtual Expression VisitBinary(BinaryExpression binaryExpression)
        {
            Expression left = Visit(binaryExpression.Left);
            Expression right = Visit(binaryExpression.Right);
            Expression conversion = Visit(binaryExpression.Conversion);
            if (left != binaryExpression.Left || right != binaryExpression.Right || conversion != binaryExpression.Conversion)
            {
                return binaryExpression.NodeType == ExpressionType.Coalesce
                           ? Expression.Coalesce(left, right, conversion as LambdaExpression)
                           : Expression.MakeBinary(binaryExpression.NodeType, left, right, binaryExpression.IsLiftedToNull, binaryExpression.Method);
            }

            return binaryExpression;
        }

        protected virtual Expression VisitTypeIs(TypeBinaryExpression typeBinaryExpression)
        {
            Expression expr = Visit(typeBinaryExpression.Expression);
            if (expr != typeBinaryExpression.Expression)
            {
                return Expression.TypeIs(expr, typeBinaryExpression.TypeOperand);
            }

            return typeBinaryExpression;
        }

        protected virtual Expression VisitConstant(ConstantExpression constantExpression)
        {
            return constantExpression;
        }

        protected virtual Expression VisitConditional(ConditionalExpression conditionalExpression)
        {
            Expression test = Visit(conditionalExpression.Test);
            Expression ifTrue = Visit(conditionalExpression.IfTrue);
            Expression ifFalse = Visit(conditionalExpression.IfFalse);
            if (test != conditionalExpression.Test || ifTrue != conditionalExpression.IfTrue || ifFalse != conditionalExpression.IfFalse)
            {
                return Expression.Condition(test, ifTrue, ifFalse);
            }

            return conditionalExpression;
        }

        protected virtual Expression VisitParameter(ParameterExpression parameterExpression)
        {
            return parameterExpression;
        }

        protected virtual Expression VisitMemberAccess(MemberExpression memberExpression)
        {
            Expression exp = Visit(memberExpression.Expression);
            if (exp != memberExpression.Expression)
            {
                return Expression.MakeMemberAccess(exp, memberExpression.Member);
            }

            return memberExpression;
        }

        protected virtual Expression VisitMethodCall(MethodCallExpression methodCallExpression)
        {
            Expression obj = Visit(methodCallExpression.Object);
            IEnumerable<Expression> args = VisitExpressionList(methodCallExpression.Arguments);
            if (obj != methodCallExpression.Object || args != methodCallExpression.Arguments)
            {
                return Expression.Call(obj, methodCallExpression.Method, args);
            }

            return methodCallExpression;
        }

        protected virtual ReadOnlyCollection<Expression> VisitExpressionList(ReadOnlyCollection<Expression> original)
        {
            List<Expression> list = null;
            Int32 n = original.Count;
            for (Int32 i = 0; i < n; i++)
            {
                Expression p = Visit(original[i]);
                if (list != null)
                {
                    list.Add(p);
                }
                else if (p != original[i])
                {
                    list = new List<Expression>(n);
                    for (Int32 j = 0; j < i; j++)
                    {
                        list.Add(original[j]);
                    }

                    list.Add(p);
                }
            }

            return list != null 
                ? list.AsReadOnly() 
                : original;
        }

        protected virtual MemberAssignment VisitMemberAssignment(MemberAssignment memberAssignment)
        {
            Expression e = Visit(memberAssignment.Expression);
            if (e != memberAssignment.Expression)
            {
                return Expression.Bind(memberAssignment.Member, e);
            }

            return memberAssignment;
        }

        protected virtual MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding binding)
        {
            IEnumerable<MemberBinding> bindings = VisitBindingList(binding.Bindings);
            if (bindings != binding.Bindings)
            {
                return Expression.MemberBind(binding.Member, bindings);
            }
            return binding;
        }

        protected virtual MemberListBinding VisitMemberListBinding(MemberListBinding binding)
        {
            IEnumerable<ElementInit> initializers = VisitElementInitializerList(binding.Initializers);
            if (initializers != binding.Initializers)
            {
                return Expression.ListBind(binding.Member, initializers);
            }
            return binding;
        }

        protected virtual IEnumerable<MemberBinding> VisitBindingList(ReadOnlyCollection<MemberBinding> original)
        {
            List<MemberBinding> list = null;
            var n = original.Count;
            for (Int32 i = 0; i < n; i++)
            {
                MemberBinding b = VisitBinding(original[i]);
                if (list != null)
                {
                    list.Add(b);
                }
                else if (b != original[i])
                {
                    list = new List<MemberBinding>(n);
                    for (Int32 j = 0; j < i; j++)
                    {
                        list.Add(original[j]);
                    }

                    list.Add(b);
                }
            }

            return list != null 
                ? (IEnumerable<MemberBinding>)list 
                : original;
        }

        protected virtual IEnumerable<ElementInit> VisitElementInitializerList(ReadOnlyCollection<ElementInit> original)
        {
            List<ElementInit> list = null;
            Int32 n = original.Count;
            for (Int32 i = 0; i < n; i++)
            {
                ElementInit init = VisitElementInitializer(original[i]);
                if (list != null)
                {
                    list.Add(init);
                }
                else if (init != original[i])
                {
                    list = new List<ElementInit>(n);
                    for (Int32 j = 0; j < i; j++)
                    {
                        list.Add(original[j]);
                    }

                    list.Add(init);
                }
            }

            return list != null 
                ? (IEnumerable<ElementInit>)list 
                : original;
        }

        protected virtual Expression VisitLambda(LambdaExpression lambdaExpression)
        {
            Expression body = Visit(lambdaExpression.Body);
            if (body != lambdaExpression.Body)
            {
                return Expression.Lambda(lambdaExpression.Type, body, lambdaExpression.Parameters);
            }

            return lambdaExpression;
        }

        protected virtual NewExpression VisitNew(NewExpression newExpression)
        {
            IEnumerable<Expression> args = VisitExpressionList(newExpression.Arguments);
            if (args != newExpression.Arguments)
            {
                return Expression.New(newExpression.Constructor, args, newExpression.Members);
            }

            return newExpression;
        }

        protected virtual Expression VisitMemberInit(MemberInitExpression initExpression)
        {
            NewExpression n = VisitNew(initExpression.NewExpression);
            IEnumerable<MemberBinding> bindings = VisitBindingList(initExpression.Bindings);
            if (n != initExpression.NewExpression || bindings != initExpression.Bindings)
            {
                return Expression.MemberInit(n, bindings);
            }

            return initExpression;
        }

        protected virtual Expression VisitListInit(ListInitExpression initExpression)
        {
            NewExpression n = VisitNew(initExpression.NewExpression);
            IEnumerable<ElementInit> initializers = VisitElementInitializerList(initExpression.Initializers);
            if (n != initExpression.NewExpression || initializers != initExpression.Initializers)
            {
                return Expression.ListInit(n, initializers);
            }

            return initExpression;
        }

        protected virtual Expression VisitNewArray(NewArrayExpression arrayExpression)
        {
            IEnumerable<Expression> exprs = VisitExpressionList(arrayExpression.Expressions);
            if (exprs != arrayExpression.Expressions)
            {
                return arrayExpression.NodeType == ExpressionType.NewArrayInit
                           ? Expression.NewArrayInit(arrayExpression.Type.GetElementType(), exprs)
                           : Expression.NewArrayBounds(arrayExpression.Type.GetElementType(), exprs);
            }

            return arrayExpression;
        }

        protected virtual Expression VisitInvocation(InvocationExpression invocationExpression)
        {
            IEnumerable<Expression> args = VisitExpressionList(invocationExpression.Arguments);
            Expression expr = Visit(invocationExpression.Expression);
            if (args != invocationExpression.Arguments || expr != invocationExpression.Expression)
            {
                return Expression.Invoke(expr, args);
            }

            return invocationExpression;
        }
    }
}
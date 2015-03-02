namespace NContext.Data.Specifications
{
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Defines methods for rebinding for expression parameters without the 
    /// use of Invoke as it is not supported in all LINQ query providers.
    /// </summary>
    internal sealed class ParameterRebinder : ExpressionVisitor
    {
        private readonly IDictionary<ParameterExpression, ParameterExpression> _ParameterMap;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterRebinder"/> class.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <remarks></remarks>
        public ParameterRebinder(IDictionary<ParameterExpression, ParameterExpression> map)
        {
            _ParameterMap = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        /// <summary>
        /// Replate parameters in expression with a Map information
        /// </summary>
        /// <param name="map">Map information</param>
        /// <param name="exp">Expression to replace parameters</param>
        /// <returns>Expression with parameters replaced</returns>
        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }

        /// <summary>
        /// Visit pattern method
        /// </summary>
        /// <param name="p">A Parameter expression</param>
        /// <returns>New visited expression</returns>
        protected override Expression VisitParameter(ParameterExpression p)
        {
            ParameterExpression replacement;
            if (_ParameterMap.TryGetValue(p, out replacement))
            {
                p = replacement;
            }

            return base.VisitParameter(p);
        }
    }
}
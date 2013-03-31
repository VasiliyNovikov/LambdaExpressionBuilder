using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LambdaExpressionBuilder.Visitors
{
  class ReplaceParameterVisitor : ExpressionVisitor
  {
    private readonly Dictionary<ParameterExpression, Expression> _parameterMappings;

    public ReplaceParameterVisitor(Dictionary<ParameterExpression, Expression> parameterMappings)
    {
      _parameterMappings = parameterMappings;
    }

    public ReplaceParameterVisitor(ParameterExpression source, Expression target)
      : this(new Dictionary<ParameterExpression, Expression> { { source, target } })
    {
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
      return _parameterMappings.ContainsKey(node) ? _parameterMappings[node] : base.VisitParameter(node);
    }

    protected override Expression VisitLambda<T>(Expression<T> node)
    {
      if (!node.Parameters.Intersect(_parameterMappings.Keys).Any())
      {
        return base.VisitLambda(node);
      }

      var newParameters = new List<ParameterExpression>();
      foreach (var p in node.Parameters)
      {
        Expression replacement;

        if (_parameterMappings.TryGetValue(p, out replacement))
        {
          var newParameter = replacement as ParameterExpression;
          if (newParameter != null)
          {
            newParameters.Add(newParameter);
          }
        }
        else
        {
          newParameters.Add(p);
        }
      }

      return Expression.Lambda(Visit(node.Body), newParameters);
    }
  }
}

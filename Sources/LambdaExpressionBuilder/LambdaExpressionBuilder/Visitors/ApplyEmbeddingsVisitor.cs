using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace LambdaExpressionBuilder.Visitors
{
  class ApplyEmbeddingsVisitor : ExpressionVisitor
  {
    private static HashSet<MethodInfo> GetEmbedMethods()
    {
      var embeds = new LambdaExpression[]
        {
          (Expression<Func<object>>) (() => ExpressionBuilder.Embed<object>(null)),
          (Expression<Func<object>>) (() => ExpressionBuilder.Embed<object, object>(null, null)),
          (Expression<Func<object>>) (() => ExpressionBuilder.Embed<object, object, object>(null, null, null)),
          (Expression<Func<object>>) (() => ExpressionBuilder.Embed<object, object, object, object>(null, null, null, null)),
          (Expression<Func<object>>) (() => ExpressionBuilder.Embed<object, object, object, object, object>(null, null, null, null, null)),
        };
      return new HashSet<MethodInfo>(embeds.Select(e => ((MethodCallExpression) e.Body).Method.GetGenericMethodDefinition()));
    }

    private static readonly HashSet<MethodInfo> EmbedMethods = GetEmbedMethods();

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
      var method = node.Method;
      if (method.IsGenericMethod && EmbedMethods.Contains(method.GetGenericMethodDefinition()))
      {
        var embedding = (LambdaExpression) node.Arguments[0].Compute();

        var parameterMappings = new Dictionary<ParameterExpression, Expression>();
        for (var i = 0; i < embedding.Parameters.Count; ++i)
        {
          parameterMappings.Add(embedding.Parameters[i], Visit(node.Arguments[i + 1]));
        }

        return new ReplaceParameterVisitor(parameterMappings).Visit(embedding.Body);
      }

      return base.VisitMethodCall(node);
    }

    public static readonly ApplyEmbeddingsVisitor Instance = new ApplyEmbeddingsVisitor();
  }
}

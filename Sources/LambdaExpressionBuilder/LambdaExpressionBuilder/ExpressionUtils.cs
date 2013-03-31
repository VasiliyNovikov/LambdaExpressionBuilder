using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace LambdaExpressionBuilder
{
  public static class ExpressionUtils
  {
    public static object Compute(this Expression expression)
    {
      var constantExpression = expression as ConstantExpression;
      if (constantExpression != null)
      {
        return constantExpression.Value;
      }

      var memberExpression = expression as MemberExpression;
      if (memberExpression != null)
      {
        var @object = memberExpression.Expression == null ? null : memberExpression.Expression.Compute();
        var fieldInfo = memberExpression.Member as FieldInfo;
        if (fieldInfo != null)
        {
          return fieldInfo.GetValue(@object);
        }

        return ((PropertyInfo)memberExpression.Member).GetValue(@object, null);
      }

      var methodCallExpression = expression as MethodCallExpression;
      if (methodCallExpression != null)
      {
        var method = methodCallExpression.Method;
        var @object = methodCallExpression.Object == null ? null : methodCallExpression.Object.Compute();
        var args = methodCallExpression.Arguments.Select(arg => arg.Compute()).ToArray();
        return method.Invoke(@object, args);
      }

      throw new NotSupportedException();
    }
  }
}

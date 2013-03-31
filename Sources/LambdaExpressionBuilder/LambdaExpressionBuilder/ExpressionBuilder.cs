using System;
using System.Linq.Expressions;
using LambdaExpressionBuilder.Visitors;

namespace LambdaExpressionBuilder
{
  public static class ExpressionBuilder
  {
    #region Core Construction Routines

    public static TResult Embed<TResult>(this Expression<Func<TResult>> embedding)
    {
      throw new InvalidOperationException("This method can only be a part of lambda expression!!!");
    }

    public static TResult Embed<T, TResult>(this Expression<Func<T, TResult>> embedding, T arg)
    {
      throw new InvalidOperationException("This method can only be a part of lambda expression!!!");
    }

    public static TResult Embed<T1, T2, TResult>(this Expression<Func<T1, T2, TResult>> embedding, T1 arg1, T2 arg2)
    {
      throw new InvalidOperationException("This method can only be a part of lambda expression!!!");
    }

    public static TResult Embed<T1, T2, T3, TResult>(this Expression<Func<T1, T2, T3, TResult>> embedding, T1 arg1, T2 arg2, T3 arg3)
    {
      throw new InvalidOperationException("This method can only be a part of lambda expression!!!");
    }

    public static TResult Embed<T1, T2, T3, T4, TResult>(this Expression<Func<T1, T2, T3, T4, TResult>> embedding, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
      throw new InvalidOperationException("This method can only be a part of lambda expression!!!");
    }

    private static TExpression ApplyEmbeddingsImpl<TExpression>(this TExpression expression)
      where TExpression : LambdaExpression
    {
      return (TExpression)ApplyEmbeddingsVisitor.Instance.Visit(expression);
    }

    public static Expression<Func<TResult>> ApplyEmbeddings<TResult>(this Expression<Func<TResult>> expression)
    {
      return expression.ApplyEmbeddingsImpl();
    }

    public static Expression<Func<T, TResult>> ApplyEmbeddings<T, TResult>(this Expression<Func<T, TResult>> expression)
    {
      return expression.ApplyEmbeddingsImpl();
    }

    public static Expression<Func<T1, T2, TResult>> ApplyEmbeddings<T1, T2, TResult>(this Expression<Func<T1, T2, TResult>> expression)
    {
      return expression.ApplyEmbeddingsImpl();
    }

    public static Expression<Func<T1, T2, T3, TResult>> ApplyEmbeddings<T1, T2, T3, TResult>(this Expression<Func<T1, T2, T3, TResult>> expression)
    {
      return expression.ApplyEmbeddingsImpl();
    }

    public static Expression<Func<T1, T2, T3, T4, TResult>> ApplyEmbeddings<T1, T2, T3, T4, TResult>(this Expression<Func<T1, T2, T3, T4, TResult>> expression)
    {
      return expression.ApplyEmbeddingsImpl();
    }

    #endregion

  }
}
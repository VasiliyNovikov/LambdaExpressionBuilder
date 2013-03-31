using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LambdaExpressionBuilder.Tests
{
  [TestClass]
  public class ExpressionBuilderTests
  {
    [TestMethod]
    public void ApplyEmbeddingsShouldProduceRightExpressionTreeTest1()
    {
      Expression<Func<int, int>> subExpr = i => (i + 2)/5;
      Expression<Func<int, int>> expr = ExpressionBuilder.ApplyEmbeddings((int i) => (i + subExpr.Embed(i))*10);
      Expression<Func<int, int>> expectedExpr = i => (i + (i + 2)/5)*10;
      Assert.AreEqual(expectedExpr.ToString(), expr.ToString());
    }

    [TestMethod]
    public void ApplyEmbeddingsShouldProduceRightExpressionTreeTest2()
    {
      Expression<Func<int, int, int>> subExpr = (i, j) => (i + j) / 2;
      Expression<Func<int, int>> expr = ExpressionBuilder.ApplyEmbeddings((int i) => (i + subExpr.Embed(i, 20)) * 10);
      Expression<Func<int, int>> expectedExpr = i => (i + (i + 20) / 2) * 10;
      Assert.AreEqual(expectedExpr.ToString(), expr.ToString());
    }

    [TestMethod]
    public void ApplyEmbeddingsShouldProduceRightExpressionTreeTest3()
    {
      Expression<Func<int, int>> subExpr1 = i => (i + 2) / 5;
      Expression<Func<int, int, int>> subExpr2 = (i, j) => (i + j) / 2;
      Expression<Func<int, int>> expr = ExpressionBuilder.ApplyEmbeddings((int i) => (subExpr1.Embed(i) + subExpr2.Embed(i, 15)) / 3);
      Expression<Func<int, int>> expectedExpr = i => ((i + 2) / 5 + (i + 15) / 2) / 3;
      Assert.AreEqual(expectedExpr.ToString(), expr.ToString());
    }

    [TestMethod]
    public void ApplyEmbeddingsShouldProduceRightExpressionTreeTest4()
    {
      Expression<Func<int, int>> subExpr1 = i => (i + 2) / 5;
      Expression<Func<int, int, int>> subExpr2 = (i, j) => (i + j) / 2;
      Expression<Func<int, int>> expr = ExpressionBuilder.ApplyEmbeddings((int i) => subExpr2.Embed(i, 15 + subExpr1.Embed(i)) / 3);
      Expression<Func<int, int>> expectedExpr = i => (i + (15 + (i + 2) / 5)) / 2 / 3;
      Assert.AreEqual(expectedExpr.ToString(), expr.ToString());
    }
  }
}

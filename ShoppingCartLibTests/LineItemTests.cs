using System;
using Xunit;
using ShoppingCartLib;

namespace ShoppingCartLibTests
{
    public class LineItemTests
    {
        [Theory]
        [InlineData(1.01, 2, 2.02)]
        [InlineData(1.015, 1, 1.02)]
        [InlineData(1.0124, 2, 2.02)]
        [InlineData(1.0125, 2, 2.03)]
        public void LineItemLineTotalRoundsUpToTwoDigits(decimal price, short quantity, decimal expectedTotal)
        {
            var product = new Product("product", price);
            var lineItem = new LineItem(product, quantity);

            Assert.Equal(expectedTotal, lineItem.LineTotal);
        }
    }
}

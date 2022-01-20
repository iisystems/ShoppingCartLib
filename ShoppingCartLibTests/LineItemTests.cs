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

        [Theory]
        [InlineData(1.00, 2, 2.00)]
        [InlineData(2.00, 3, 4.00)]
        [InlineData(3.00, 6, 12.00)]
        [InlineData(4.00, 8, 24.00)]
        [InlineData(10.00, 100, 670.00)]
        public void LineItemLineTotalWhenBuy2Get1FreeEveryThirdItemIsFree(decimal price, short quantity, decimal expectedCost)
        {
            var product = new Product("product", price, Product.PricingStrategy_Buy2Get1Free);
            var lineItem = new LineItem(product, quantity);

            Assert.Equal(expectedCost, lineItem.LineTotal);
        }
    }
}

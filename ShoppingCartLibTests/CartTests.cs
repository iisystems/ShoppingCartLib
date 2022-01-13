using System;
using Xunit;
using ShoppingCartLib;


namespace ShoppingCartLibTests
{
    public class CartTests
    {
        private static Product EGGS = new Product("Eggs", 2.99m);
        private static Product EGGS_CAGE_FREE = new Product("Eggs - Cage Free", 3.99m);
        private static Product EGGS_FREE_RANGE = new Product("Eggs - Free Range", 4.99m);
        private static Product EGGS_PASTURE_RAISED = new Product("Eggs - Pasture Raised", 6.99m);
        private static Product EGGS_GIRLFRIEND_APPROVED = new Product("Eggs - Limited Edition", 8.99m);
        private static Product FLOUR_BY_OZ = new Product("1 oz Bulk Flour", 0.297m);
        private static Product FLOUR_BY_OZ_ORGANIC = new Product("1 oz Bulk Organic Flour", 0.395m);

        [Fact]
        public void ShoppingCartWhenNewShouldBeEmpty()
        {
            var taxFreeCart = new ShoppingCart();
            var taxedCart = new ShoppingCart(0.06m);

            Assert.Equal(0, taxFreeCart.LineItemCount);
            Assert.Equal(0, taxedCart.LineItemCount);
            Assert.Equal(0.0m, taxFreeCart.Total);
            Assert.Equal(0.0m, taxedCart.Total);
        }

        [Fact]
        public void ShoppingCartWhenDifferentProductsAddedShouldCreateDiscreteLineItems()
        {
            var cart = new ShoppingCart();

            cart.Add(EGGS);
            cart.Add(EGGS_CAGE_FREE);
            cart.Add(EGGS_FREE_RANGE);
            cart.Add(EGGS_PASTURE_RAISED);
            cart.Add(EGGS_GIRLFRIEND_APPROVED);

            Assert.Equal(5, cart.LineItemCount);
        }

        [Fact]
        public void ShoppingCartWhenSameProductsAddedShouldRollupIntoSingleLineItem()
        {
            var cart = new ShoppingCart();

            cart.Add(EGGS);
            cart.Add(EGGS, 3);
            cart.Add(EGGS, 10);

            Assert.Equal(1, cart.LineItemCount);
            Assert.Equal(14, cart[EGGS].Quantity);
        }

        [Theory]
        [InlineData(1, 0.30)]   // 0.297
        [InlineData(2, 0.59)]   // 0.594
        [InlineData(3, 0.89)]   // 0.891
        [InlineData(4, 1.19)]   // 1.188
        [InlineData(5, 1.49)]   // 1.485
        public void ShoppingCartLineItemShouldRoundAwayFromZero(ushort quantity, decimal expectedTotal)
        {
            var cart = new ShoppingCart();

            cart.Add(FLOUR_BY_OZ, quantity);

            Assert.Equal(expectedTotal, cart.Total);
        }

        [Theory]
        [InlineData(0.06, 1.00, 1.06)]      // 1.06
        [InlineData(0.065, 1.00, 1.07)]     // 1.065
        [InlineData(0.065, 2.00, 2.13)]     // 2.13
        [InlineData(0.065, 1.50, 1.60)]     // 1.5975
        [InlineData(0.065, 2.99, 3.18)]     // 3.18435
        public void ShoppingCartTaxShouldRoundToTwoDigits(decimal taxRate, decimal unitPrice, decimal expectedTotal)
        {
            var cart = new ShoppingCart(taxRate);

            cart.Add(new Product("widget", unitPrice));

            Assert.Equal(expectedTotal, cart.Total);
        }

        [Fact]
        public void ShoppingCartShouldAccrueRoundingErrors()
        {
            // Test each line item is rounded individually and the cart total is the sum of all line items plus tax, even if full precision would yield a different result.
            var cart = new ShoppingCart(0.06m);

            cart.Add(FLOUR_BY_OZ);              // 0.297 --> 0.30
            cart.Add(FLOUR_BY_OZ_ORGANIC);      // 0.395 --> 0.40
                                                // 0.70 * 1.06 = 0.742 --> 0.74 (desired result)
                                                // Full precision: (0.297 + 0.395) * 1.06 = 0.73352

            Assert.Equal(0.30m, cart[FLOUR_BY_OZ].LineTotal);
            Assert.Equal(0.40m, cart[FLOUR_BY_OZ_ORGANIC].LineTotal);
            Assert.Equal(0.04m, cart.Tax);
            Assert.Equal(0.74m, cart.Total);
        }
    }
}

using System;
using Xunit;
using ShoppingCartLib;


namespace ShoppingCartLibTests
{
    public class ProductTests
    {
        [Theory]
        [InlineData ("product", 1.00, "product", 1.00, true)]
        [InlineData("product", 1.00, "product", 2.01, true)]
        [InlineData("product", 1.00, "Product", 1.00, false)]
        [InlineData("product", 1.00, "Product", 2.00, false)]
        [InlineData("product", 1.00, "product2", 1.00, false)]
        [InlineData("product", 1.00, "product2", 2.00, false)]
        public void ProductWhenSameNameIsSameProduct(string product1Name, decimal product1Price, string product2Name, decimal product2Price, bool isSame)
        {
            var product1 = new Product(product1Name, product1Price);
            var product2 = new Product(product2Name, product2Price);

            if (isSame)
            {
                Assert.True(product1.Equals(product2));
                Assert.True(product1.Equals(product2 as object));
                Assert.True(product1 == product2);
                Assert.False(product1 != product2);
            }
            else
            {
                Assert.False(product1.Equals(product2));
                Assert.False(product1.Equals(product2 as object));
                Assert.False(product1 == product2);
                Assert.True(product1 != product2);
            }
        }
    }
}

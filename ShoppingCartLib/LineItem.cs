using System;

namespace ShoppingCartLib
{
    public class LineItem
    {
        public Product Product;
        public string ProductName => Product.Name;
        public decimal UnitPrice => Product.Price;
        public short Quantity;
        public decimal LineTotal => (UnitPrice * Quantity).RoundToTwoDigits();

        public LineItem(Product product, short quantity)
        {
            Product = product;
            Quantity = quantity;
        }
    }
}

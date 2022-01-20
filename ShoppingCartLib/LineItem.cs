using System;

namespace ShoppingCartLib
{
    public class LineItem
    {
        public Product Product { get; set; }
        public string ProductName => Product.Name;
        public decimal UnitPrice => Product.Price;
        public short Quantity { get; set; }
        public decimal LineTotal => Product.CalculateBulkPrice(Quantity).RoundToTwoDigits();
        public decimal Discount => (UnitPrice * Quantity).RoundToTwoDigits() - LineTotal;

        public LineItem(Product product, short quantity)
        {
            Product = product;
            Quantity = quantity;
        }
    }
}

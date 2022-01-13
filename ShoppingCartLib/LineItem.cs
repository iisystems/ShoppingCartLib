using System;

namespace ShoppingCartLib
{
    public class LineItem
    {
        public string ProductName;
        public decimal UnitPrice;
        public ushort Quantity;
        public decimal LineTotal { get => Math.Round(UnitPrice * Quantity, 2, MidpointRounding.AwayFromZero); }

        public LineItem(string productName, decimal unitPrice, ushort quantity)
        {
            ProductName = productName;
            UnitPrice = unitPrice;
            Quantity = quantity;
        }
    }
}

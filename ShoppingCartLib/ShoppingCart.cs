using System;
using System.Collections.Generic;


namespace ShoppingCartLib
{
    public class ShoppingCart
    {
        public decimal SubTotal;
        public decimal Tax { get => Math.Round(SubTotal * _taxRate, 2, MidpointRounding.AwayFromZero); }
        public decimal Total { get => SubTotal + Tax; }
        public int LineItemCount { get => _lineItems.Count; }

        private Dictionary<string, LineItem> _lineItems = new();
        private decimal _taxRate;
        private static readonly object _object = new();

        public ShoppingCart()
        {
        }

        public ShoppingCart(decimal taxRate)
        {
            _taxRate = taxRate;
        }

        public void Add(Product product, ushort quantity)
        {
            lock (_object)
            {
                if (_lineItems.TryGetValue(product.Name, out LineItem lineItem))
                    lineItem.Quantity += quantity;
                else
                    _lineItems.Add(product.Name, new LineItem(product.Name, product.Price, quantity));

                SubTotal += Math.Round(product.Price * quantity, 2, MidpointRounding.AwayFromZero);
            }
        }

        public void Add(Product product)
        {
            Add(product, 1);
        }

        public LineItem this[string itemName]
        {
            get
            {
                if (_lineItems.TryGetValue(itemName, out LineItem lineItem))
                    return lineItem;

                return null;
            }
        }

        public LineItem this[Product item]
        {
            get => this[item.Name];
        }
    }
}

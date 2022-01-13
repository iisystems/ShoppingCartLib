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
                LineItem lineItem;
                if (_lineItems.TryGetValue(product.Name, out lineItem))
                {
                    // If item already in cart, update quantity and reset to current price
                    SubTotal -= lineItem.LineTotal;
                    lineItem.UnitPrice = product.Price;
                    lineItem.Quantity += quantity;
                }
                else
                {
                    lineItem = new LineItem(product.Name, product.Price, quantity);
                    _lineItems.Add(product.Name, lineItem);
                }

                SubTotal += lineItem.LineTotal;
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

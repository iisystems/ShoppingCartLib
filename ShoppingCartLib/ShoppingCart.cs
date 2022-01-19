using System;
using System.Collections.Generic;


namespace ShoppingCartLib
{
    public class ShoppingCart
    {
        public decimal SubTotal;
        public decimal Tax => (SubTotal * _taxRate).RoundToTwoDigits();
        public decimal Total => SubTotal + Tax;
        public int LineItemCount => _lineItems.Count;

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

        public void Add(Product product, short quantity = 1)
        {
            lock (_object)
            {
                LineItem lineItem;

                // If product already in cart, update quantity and set to current price
                if (_lineItems.TryGetValue(product.Name, out lineItem))
                {
                    SubTotal -= lineItem.LineTotal;
                    lineItem.Product = product;
                    lineItem.Quantity += quantity;
                }
                else
                {
                    lineItem = new LineItem(product, quantity);
                    _lineItems.Add(product.Name, lineItem);
                }

                SubTotal += lineItem.LineTotal;
            }
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

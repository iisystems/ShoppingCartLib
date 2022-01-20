using System;
using System.Collections.Generic;


namespace ShoppingCartLib
{
    public class ShoppingCart
    {
        public decimal SubTotal { get; private set; }
        public decimal Tax => (SubTotal * _taxRate).RoundToTwoDigits();
        private decimal _totalBeforeCartDiscount => SubTotal + Tax;
        public int LineItemCount => _lineItems.Count;
        public decimal LineItemDiscount { get; private set; }
        public decimal CartDiscount => (_totalBeforeCartDiscount > 1000m) ? (_totalBeforeCartDiscount * 0.1m).RoundToTwoDigits() : 0;
        public decimal Total => _totalBeforeCartDiscount - CartDiscount;

        public decimal TotalDiscount => CartDiscount + LineItemDiscount;

        private Dictionary<string, LineItem> _lineItems = new();
        private readonly decimal _taxRate;

        private LineItem this[string itemName] => _lineItems.TryGetValue(itemName, out LineItem lineItem) ? lineItem : null;
        public LineItem this[Product item] => this[item.Name];

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
                    LineItemDiscount -= (lineItem.Discount * (1 + _taxRate)).RoundToTwoDigits();

                    if (lineItem.Quantity + quantity <= 0)
                    {
                        _lineItems.Remove(lineItem.ProductName);
                        return;
                    }
                    lineItem.Product = product;
                    lineItem.Quantity += quantity;
                }
                else if (quantity > 0)
                {
                    lineItem = new LineItem(product, quantity);
                    _lineItems.Add(product.Name, lineItem);
                }

                SubTotal += lineItem.LineTotal;
                LineItemDiscount += (lineItem.Discount * (1 + _taxRate)).RoundToTwoDigits();
            }
        }
        public void Remove(Product product, short quantity = 1)
        {
            Add(product, (short)(0 - quantity));
        }
    }
}

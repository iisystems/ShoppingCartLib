using System;


namespace ShoppingCartLib
{
    public class Product
    { 
        public string Name { get; }
        public decimal Price { get; }
        public delegate decimal BulkPricingStrategy (decimal price, short quantity);
        private BulkPricingStrategy _bulkPricingStrategy;

        public Product(string name, decimal price, BulkPricingStrategy pricingStrategy = null)
        {
            Name = name;
            Price = price;
            _bulkPricingStrategy = pricingStrategy ?? PricingStrategy_FullPrice;
        }

        // Assume product name is unique for this exercise. Would normally have ID or UPC as a key.
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Product);
        }

        public static bool operator ==(Product product1, Product product2)
        {
            return product1?.Equals(product2) ?? false;
        }

        public static bool operator !=(Product product1, Product product2)
        {
            return !(product1?.Equals(product2) ?? false);
        }

        public bool Equals(Product product)
        {
            return product?.Name == Name;
        }

        public decimal CalculateBulkPrice(short quantity)
        {
            return _bulkPricingStrategy(Price, quantity);
        }

        #region Bulk Pricing Delegates
        public static decimal PricingStrategy_Buy2Get1Free(decimal price, short quantity)
        {
            return price * (quantity - quantity / (short)3);
        }
        public static decimal PricingStrategy_FullPrice(decimal price, short quantity)
        {
            return price * quantity;
        }
         
        #endregion
    }
}

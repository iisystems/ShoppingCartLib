using System;


namespace ShoppingCartLib
{
    public class Product
    { 
        public string Name;
        public decimal Price;

        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
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
    }
}

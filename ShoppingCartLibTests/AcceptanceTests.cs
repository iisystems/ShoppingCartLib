using System;
using Xunit;
using ShoppingCartLib;


namespace ShoppingCartLibTests
{
    public class AcceptanceTests
    {
        private static Product DOVE_SOAP = new Product("DoveSoap", 39.99m);
        private static Product DOVE_SOAP_SPECIAL = new Product("DoveSoap", 39.99m, Product.PricingStrategy_Buy2Get1Free);
        private static Product AXE_DEO = new Product("AxeDeo", 99.99m);

        [Fact]
        public void AC0()
        {
            //  AC 0 : Add a single product.
            //  Given:
            //            *Anemptyshoppingcart
            //            * Andaproduct,_DoveSoap_withaunitpriceof_39.99_
            //  When:
            //            *Theuseraddsasingle_DoveSoap_totheshoppingcart
            //  Then:
            //            *Theshoppingcartshouldasinglelineitemwith1DoveSoapwithaunitprice of _39.99_
            //            * Andtheshoppingcart'stotalpriceshouldequal_39.99_
            //            * Alltotalsshouldberoundedupto2decimalplaces,i.e.0.565shouldresultin 0.57 but 0.5649 should result in 0.56.
            //            * You can[follow this link](http://www.clivemaxfield.com/diycalculator/sp-round.shtml#A3) if you want more details.

            var cart = new ShoppingCart();

            cart.Add(DOVE_SOAP);

            Assert.Equal(1, cart.LineItemCount);
            Assert.Equal(1, cart[DOVE_SOAP].Quantity);
            Assert.Equal(39.99m, cart[DOVE_SOAP].LineTotal);
            Assert.Equal(39.99m, cart.Total);
        }

        [Fact]
        public void AC1()
        {
            //  AC 1 : Add many products.
            //  Given:
            //            *Anemptyshoppingcart
            //            * Andaproduct,_DoveSoap_withaunitpriceof_39.99_
            //  When:
            //            *Theuseradds5_DoveSoaps_totheshoppingcart
            //            * Andthenaddsanother3_DoveSoaps_totheshoppingcart
            //  Then:
            //            *Theshoppingcartshouldcontainasinglelineitem,becauseproductequality is not instance based
            //           * Theshoppingcartshouldcontain8DoveSoapseachwithaunitpriceof _39.99_
            //           * Andtheshoppingcart'stotalpriceshouldequal_319.92_
            //           * Alltotalsshouldberoundedupto2decimalplacesasdescribedinAC0.

            var cart = new ShoppingCart();

            cart.Add(DOVE_SOAP, 5);
            cart.Add(DOVE_SOAP, 3);

            Assert.Equal(1, cart.LineItemCount);
            Assert.Equal(8, cart[DOVE_SOAP].Quantity);
            Assert.Equal(39.99m, cart[DOVE_SOAP].UnitPrice);
            Assert.Equal(319.92m, cart.Total);
        }

        [Fact]
        public void AC2()
        {
            //  AC 2 : Calculate tax rate with many products
            //  Given:
            //            *Anemptyshoppingcart
            //            * Andaproduct,_DoveSoap_withaunitpriceof_39.99_
            //            * Andanotherproduct,_AxeDeo_withaunitpriceof_99.99_
            //            * Andasalestaxrateof12.5 % applicabletoallproductsequally
            //  When:
            //            *Theuseradds2_DoveSoaps_totheshoppingcart * Andthenadds2_AxeDeos_totheshoppingcart
            //  Then:
            //            *Theshoppingcartshouldcontainalineitemwith2DoveSoapseachwitha unit price of _39.99_
            //           * Andtheshoppingcartshouldcontainalineitemwith2AxeDeoseachwitha unit price of _99.99_
            //           * Andthetotalsalestaxamountfortheshoppingcartshouldequal_35.00_
            //           * Andtheshoppingcart'stotalpriceshouldequal_314.96_
            //           * Alltotalsshouldberoundedupto2decimalplacesasdescribedinAC0.

            // Empty shopping cart with 12.5% tax rate
            var cart = new ShoppingCart(0.125m);

            cart.Add(DOVE_SOAP, 2);
            cart.Add(AXE_DEO, 2);

            Assert.Equal(2, cart.LineItemCount);
            Assert.Equal(2, cart[DOVE_SOAP].Quantity);
            Assert.Equal(39.99m, cart[DOVE_SOAP].UnitPrice);
            Assert.Equal(2, cart[AXE_DEO].Quantity);
            Assert.Equal(99.99m, cart[AXE_DEO].UnitPrice);
            Assert.Equal(35.00m, cart.Tax);
            Assert.Equal(314.96m, cart.Total);
        }

        [Fact]
        public void AC3()
        {
            // Remove an item
            // Given the customer has 4 Dove Soaps and 2 Axe Deos in their cart
            // When the customer removes 1 Dove Soap
            // Then the number of Dove Soaps should be 3
            // And the total & the sales tax should reflect the change

            // Tax = 39.99
            // Price = 359.94

            // Empty shopping cart with 12.5% tax rate
            var cart = new ShoppingCart(0.125m);

            cart.Add(DOVE_SOAP, 4);
            cart.Add(AXE_DEO, 2);
            cart.Remove(DOVE_SOAP);

            Assert.Equal(3, cart[DOVE_SOAP].Quantity);
            Assert.Equal(39.99m, cart.Tax);
            Assert.Equal(359.94m, cart.Total);
        }

        [Fact]
        public void AC4()
        {
            // Multi - buy discount(Line Level Discount)

            // Given there is a Buy 2 Get 3rd Free offer on Dove Soap
            // When I add 2 Dove Soap to the cart
            // Then the discount is not applied to the cart
            // And the total discount shown should be 0

            // Tax = 10
            // Price = 89.98

            // Empty shopping cart with 12.5% tax rate
            var cart = new ShoppingCart(0.125m);

            cart.Add(DOVE_SOAP_SPECIAL, 2);

            Assert.Equal(0, cart.LineItemDiscount);
            Assert.Equal(10.0m, cart.Tax);
            Assert.Equal(89.98m, cart.Total);
        }

        [Fact]
        public void AC5()
        {
            // Multi - buy discount(Line Level Discount)

            // Given there is a Buy 2 Get 3rd Free offer on Dove Soap
            // When I add 3 Dove Soap to the cart
            // And 3 Axe Deos to the cart
            // Then the total discount should be the price of 1 Dove Soap
            // And shown inclusive of sales tax
            // And the total & the sales tax should reflect the discount

            // Discount = 44.99
            // Tax = 47.49
            // Price = 427.44

            // Empty shopping cart with 12.5% tax rate
            var cart = new ShoppingCart(0.125m);

            cart.Add(DOVE_SOAP_SPECIAL, 3);
            cart.Add(AXE_DEO, 3);

            Assert.Equal(44.99m, cart.LineItemDiscount);
            Assert.Equal(47.49m, cart.Tax);
            Assert.Equal(427.44m, cart.Total);
        }

        [Fact]
        public void AC6()
        {
            // Multi - buy discount(Line Level Discount)

            // Given there is a Buy 2 Get 3rd Free offer on Dove Soap
            // When I add 6 Dove Soap to the cart
            // Then the total discount should be the price of 2 Dove Soap
            // And shown inclusive of sales tax
            // And the total & the sales tax should show the price for 4 Dove Soap

            // Discount = 89.98
            // Tax = 20.00
            // Price = 179.95

            // Empty shopping cart with 12.5% tax rate
            var cart = new ShoppingCart(0.125m);

            cart.Add(DOVE_SOAP_SPECIAL, 6);

            Assert.Equal(89.98m, cart.LineItemDiscount);
            Assert.Equal(20.00m, cart.Tax);
            Assert.Equal(179.95m, cart.Total);
        }

        [Fact]
        public void AC7()
        {
            // Total price discount (Cart Level Discount)

            // Given there is a 10 % discount for purchases over 1,000 after tax 
            // When I add more than 1,000 worth of items to my cart
            // Then the total price should be reduced by 10%

            // Empty shopping cart with 12.5% tax rate
            var cart = new ShoppingCart(0.125m);

            cart.Add(AXE_DEO, 9);        // 1012.40

            Assert.Equal(101.24m, cart.TotalDiscount);
            Assert.Equal(911.16m, cart.Total);
        }

        [Fact]
        public void AC8()
        {
            // Total price discount (Cart Level Discount)

            // Given there is a 10% Discount for purchases over 1,000 after tax
            // And there is a Buy 2 Get 3rd free offer on Dove Soap
            // When the total is over 1,000 after tax
            // And I add 3 Dove Soap to the cart
            // Then total price discount should only be calculated after the multi - buy discount

            // Empty shopping cart with 12.5% tax rate
            var cart = new ShoppingCart(0.125m);

            cart.Add(AXE_DEO, 9);               // 1012.40
            cart.Add(DOVE_SOAP_SPECIAL, 3);     // +89.98 = 1102.38

            Assert.Equal(44.99m, cart.LineItemDiscount);
            Assert.Equal(110.24m, cart.CartDiscount);
            Assert.Equal(992.14m, cart.Total);
        }
    }
}

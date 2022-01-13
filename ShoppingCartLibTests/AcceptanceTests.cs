using System;
using Xunit;
using ShoppingCartLib;


namespace ShoppingCartLibTests
{
    public class AcceptanceTests
    {
        private static Product DOVE_SOAP = new Product("DoveSoap", 39.99m);
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
    }
}

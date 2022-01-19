using System;

namespace ShoppingCartLib
{
    public static class ExtensionMethods
    {
        public static decimal RoundToTwoDigits(this decimal raw) => Math.Round(raw, 2, MidpointRounding.AwayFromZero);
    }
}

using System;

namespace Oogstplanner.Utilities.ExtensionMethods
{
    public static class MonthExtension
    {

        public static Month Add(this Month input, int amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException("amount", "Amount can not be negative.");

            /* Number of the month (1 to 12) is the base 2 logarithm of the input 
             * because month is a flags enumeration. */
            var monthNumber = Math.Log((int)input, 2);

            /* Raise 2 to the power of the new month number because of the flags enumeration.
             * mod 12 is taken because it might be possible that we go from December to January. 
             * Finally cast the integer back to the enumeration value. */
            return (Month)(Math.Pow(2, (monthNumber + amount) % 12));
        }

        public static Month Subtract(this Month input, int amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException("amount", "Amount can not be negative.");

            /* When subtracting we do an addition of (12 - amount % 12). First we take the mod 12
             * of the month because it might be possible we go from December to January. Then we 
             * add 12 months minus this amount, which equals subtracting since we ignore the year. */
            return Add(input, 12 - amount % 12);
        }

    }
}
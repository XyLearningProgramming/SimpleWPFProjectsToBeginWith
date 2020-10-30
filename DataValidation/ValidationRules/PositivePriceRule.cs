using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;

namespace DataValidation
{
	internal class PositivePriceRule: ValidationRule
	{
        private decimal min = 0;
        private decimal max = Decimal.MaxValue;

        public decimal Min
        {
            get { return min; }
            set { min = value; }
        }

        public decimal Max
        {
            get { return max; }
            set { max = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">Value needed to be validated</param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            decimal price = 0;
            if(value is String valString && valString.Length > 0)
            {
                if(Decimal.TryParse(valString, out price))
                {
                    if((price < Min) || (price > Max))
                    {
                        return new ValidationResult(false,
                          "Not in the range " + Min + " to " + Max + ".");
                    }
                    else
                    {
                        return new ValidationResult(true, null);
                    }
                }
            }
            return new ValidationResult(false, "Illegal characters.");
        }
    }
}

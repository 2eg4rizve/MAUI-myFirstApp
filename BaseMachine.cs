using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myFirstApp
{
    public abstract class BaseMachine
    {

        //string testParamsMakeToF0 = ConfigurationManager.AppSettings["testParamsMakeToF0"];

        protected string allData = "";
        protected StringBuilder allDataStringBuilder = new StringBuilder();
        protected string sampleWithSerial = "";


        protected static DataTable reportDesigns = new DataTable();
        protected static DataTable reportMachineNames = new DataTable();


        protected string FormatDecimal(double value)
        {
            if (makeDacimalePoint4 == "true")
            {
                Console.WriteLine($"Formatted to 4 decimal places: {value}");
                return value.ToString("F4");
            }
            else if (makeDacimalePoint3 == "true")
            {
                Console.WriteLine($"Formatted to 3 decimal places: {value}");
                return value.ToString("F3");
            }
            else if (makeDacimalePoint1 == "true")
            {
                double numericValue = Convert.ToDouble(value);
                numericValue = Math.Round(numericValue, 1, MidpointRounding.AwayFromZero);

                Console.WriteLine($"Formatted to 1 decimal place: {numericValue:F1}");
                var x = numericValue.ToString("F1");
                return x;
            }
            else if (makeDacimalePoint0 == "true" && value == (int)value)
            {
                return value.ToString();

            }
            else if (makeDacimalePointRound == "true")
            {
                Console.WriteLine($"Rounded value: {value}");
                return Math.Round(value).ToString();
            }
            else if (RoundToSingleDecimal == "true")
            {
                double roundedValue = Math.Round(value, 1, MidpointRounding.AwayFromZero);
                Console.WriteLine($"Rounded to single decimal: {roundedValue}");
                return roundedValue.ToString("0.0", CultureInfo.InvariantCulture);
            }

            Console.WriteLine($"Formatted to default 2 decimal places: {value}");
            return value.ToString("F2");
        }

       
    }
}

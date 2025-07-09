//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Data;
//using System.Globalization;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml.Linq;


//namespace myFirstApp
//{
//    //public abstract class BaseMachine
//    //{
//    //    //protected static string sleepingTimems = ConfigurationManager.AppSettings["threadSleep"];
//    //    //string replaceDefaultValue = ConfigurationManager.AppSettings["replaceDefaultValue"];
//    //    //string makeDacimalePoint4 = ConfigurationManager.AppSettings["makeDacimalePoint4"];
//    //    //string makeDacimalePoint3 = ConfigurationManager.AppSettings["makeDacimalePoint3"];
//    //    //string makeDacimalePoint1 = ConfigurationManager.AppSettings["makeDacimalePoint1"];
//    //    //string makeDacimalePoint0 = ConfigurationManager.AppSettings["makeDacimalePoint0"];
//    //    //string makeDacimalePointRound = ConfigurationManager.AppSettings["makeDacimalePointRound"];

//    //    //string makeDacimalePointAsItIs = ConfigurationManager.AppSettings["makeDacimalePointAsItIs"];
//    //    //string RoundToSingleDecimal = ConfigurationManager.AppSettings["RoundToSingleDecimal"];
//    //    //string makeToNotZeroIfLessThanTen = ConfigurationManager.AppSettings["makeToNotZeroIfLessThanTen"];
//    //    //string testParamsMakeToF0 = ConfigurationManager.AppSettings["testParamsMakeToF0"];

//    //    //protected string allData = "";
//    //    //protected StringBuilder allDataStringBuilder = new StringBuilder();
//    //    //protected string sampleWithSerial = "";

//    //    //protected static Dictionary<string, Report> ReportList = new Dictionary<string, Report>();
//    //    //protected static SampleData sampleData = new SampleData();
//    //    //protected static DataTable reportDesigns = new DataTable();
//    //    //protected static DataTable reportMachineNames = new DataTable();

//    //    //protected Dictionary<string, DataModel> reportDictionary = new Dictionary<string, DataModel>();
//    //    //protected List<DataModel> dmList = new List<DataModel>();

//    //    //protected APIHelperV2 apiHelper = new APIHelperV2();

//    //    //protected void ApplyResultModificationBl(List<DataModel> dmList)
//    //    //{
//    //    //    // Console.log($"Applying result modifications to {dmList} items.");

//    //    //    foreach (var result in dmList)
//    //    //    {



//    //    //        // Ensure that modification is necessary and the value is a valid float
//    //    //        if (makeDacimalePointAsItIs == "true" || !float.TryParse(result.FormattedValue, out _))
//    //    //        {
//    //    //            var value1 = result.IsFormetted ? result.FormattedValue : result.Value.ToString();

//    //    //            CustomHelper.Debug($"Machine Code: {result.MapKey}, Result: {value1}");
//    //    //            continue;
//    //    //        }


//    //    //        double resultFloat = CustomHelper.ConvertToFloat(result.FormattedValue, 4);



//    //    //        result.FormattedValue = FormatDecimal(resultFloat);


//    //    //        // Ensure values less than 10 are handled correctly
//    //    //        if (makeToNotZeroIfLessThanTen == "true" && resultFloat < 10)
//    //    //        {
//    //    //            result.FormattedValue = "0" + result.FormattedValue;
//    //    //        }

//    //    //        var value = result.IsFormetted ? result.FormattedValue : result.Value.ToString();

//    //    //        //MessageHelper.Instance.Text = $"Machine Code: {result.MapKey}, Result: {value}";


//    //    //        if (CustomHelper.CheckTestParamsMakeToF0(result.Label))
//    //    //        {
//    //    //            resultFloat = Math.Round(resultFloat);
//    //    //            result.FormattedValue = resultFloat.ToString(); // Format to round
//    //    //        }
//    //    //        CustomHelper.Debug($"Machine Code: {result.MapKey}, Result: {result.FormattedValue}");

//    //    //    }


//    //    //}

//    //    //protected string FormatDecimal(double value)
//    //    //{
//    //    //    if (makeDacimalePoint4 == "true")
//    //    //    {
//    //    //        Console.WriteLine($"Formatted to 4 decimal places: {value}");
//    //    //        return value.ToString("F4");
//    //    //    }
//    //    //    else if (makeDacimalePoint3 == "true")
//    //    //    {
//    //    //        Console.WriteLine($"Formatted to 3 decimal places: {value}");
//    //    //        return value.ToString("F3");
//    //    //    }
//    //    //    else if (makeDacimalePoint1 == "true")
//    //    //    {
//    //    //        double numericValue = Convert.ToDouble(value);
//    //    //        numericValue = Math.Round(numericValue, 1, MidpointRounding.AwayFromZero);

//    //    //        Console.WriteLine($"Formatted to 1 decimal place: {numericValue:F1}");
//    //    //        var x = numericValue.ToString("F1");
//    //    //        return x;
//    //    //    }
//    //    //    else if (makeDacimalePoint0 == "true" && value == (int)value)
//    //    //    {
//    //    //        return value.ToString();

//    //    //    }
//    //    //    else if (makeDacimalePointRound == "true")
//    //    //    {
//    //    //        Console.WriteLine($"Rounded value: {value}");
//    //    //        return Math.Round(value).ToString();
//    //    //    }
//    //    //    else if (RoundToSingleDecimal == "true")
//    //    //    {
//    //    //        double roundedValue = Math.Round(value, 1, MidpointRounding.AwayFromZero);
//    //    //        Console.WriteLine($"Rounded to single decimal: {roundedValue}");
//    //    //        return roundedValue.ToString("0.0", CultureInfo.InvariantCulture);
//    //    //    }

//    //    //    Console.WriteLine($"Formatted to default 2 decimal places: {value}");
//    //    //    return value.ToString("F2");
//    //    //}

//    //    //protected void ApplyShowDisplayBl(List<DataModel> dmList, SampleList sl)
//    //    //{
//    //    //    // Header
//    //    //    MessageHelper.Instance.Text = "\n📄 Result Summary Report";
//    //    //    MessageHelper.Instance.Text = $"🧾 Sample: {sl.SampleId}  {sl.SerialNo}";
//    //    //    MessageHelper.Instance.Text = $"📊 Total Records: {dmList.Count}";
//    //    //    MessageHelper.Instance.Text = new string('═', 65);

//    //    //    // Column headers
//    //    //    MessageHelper.Instance.Text = $"| {"No",-2} | {"Label",-50} | {"Result",-10} |";
//    //    //    MessageHelper.Instance.Text = new string('─', 65);

//    //    //    // Row data
//    //    //    for (int i = 0; i < dmList.Count; i++)
//    //    //    {
//    //    //        var dm = dmList[i];

//    //    //        string label = dm.Label.Length > 35
//    //    //            ? dm.Label.Substring(0, 32) + "..."  // truncate long labels
//    //    //            : dm.Label.PadRight(35);



//    //    //        string resultCell = $"{dm.FormattedValue} ({dm.MapKey})";
//    //    //        resultCell = resultCell.Length > 10
//    //    //            ? resultCell.Substring(0, 17) + "..."
//    //    //            : resultCell.PadRight(10);

//    //    //        MessageHelper.Instance.Text = $"| {i + 1,-3} | {label} | {resultCell} |";
//    //    //    }

//    //    //    // Footer
//    //    //    MessageHelper.Instance.Text = new string('═', 65);
//    //    //}


//    //}
//}

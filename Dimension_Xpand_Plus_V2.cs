using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace myFirstApp
{
 
    public class Dimension_Xpand_Plus_V2 
    {


        #region Global Properties

        string _machineName = "Dimension";// ConfigurationManager.ConnectionStrings["maNameDim"].ToString();
        string _machineID = "EXL_LM";//  ConfigurationManager.ConnectionStrings["maIDExL200"].ToString();
        //string connStr2 = "Data Source=192.168.15.48\\SQLExpress;Initial Catalog=HospitalManagement;Persist Security Info=True;User Id=sa;Password=biorad";

        private static string sampleType;


        private static string _barcode;

        private static string _OrderNo;

        private static string _testdateTime;

        private static string _testResult;

        private static string _testUnit;

        private static string _testCode;

        private static string _errorCode;

        private static string testSequence;

        private string parametertype = "";

        private static int IsResultProcessing = 0;

        private static int IsOrderProcessing = 0;

        public static int counter = 1;
        public static int seq = 4;
        public static string SendToMachine = "SEND: ";
        public static int ordersendcount = 1;
        public static string allResult = "";


        private static string[] errorCode = new string[] { "1", "2", "3", "4", "5", "6" };

        const int wait = 4000;


        private static string SpecimenType = "1";
        private static string[] urineCodes = new[] { "26", "33", "35", "38", "40", "216", "225", "228" };


        private static string _PMID;


        static string soh = char.ConvertFromUtf32(1);
        static string stx = char.ConvertFromUtf32(2);

        static string etx = char.ConvertFromUtf32(3);

        static string eot = char.ConvertFromUtf32(4);

        static string enq = "05";
        static string ack = char.ConvertFromUtf32(6);
        static string fs = char.ConvertFromUtf32(28);

        static string nack = char.ConvertFromUtf32(21);

        static string etb = char.ConvertFromUtf32(23);

        static string lf = char.ConvertFromUtf32(10);

        static string cr = char.ConvertFromUtf32(13);

        const char pipe = '|';
        const char caret = '^';
        string strDate = DateTime.Now.ToString("yyyyMMddhhmmss");
        private SerialPort sp = new SerialPort();

        // private Color[] LogMsgTypeColor = { Color.Blue, Color.Green, Color.Black, Color.Orange, Color.Red };
        // public enum LogMsgType { Incoming, Outgoing, Normal, Warning, Error };

        public static string vSTX = "<STX>", vETX = "<ETX>", vEOT = "<EOT>", vENQ = "<ENQ>",
        vACK = "<ACK>", vETB = "<ETB>", vNAK = "<NAK>", vCR = "<CR>", vLF = "<LF>";

        #endregion

        bool IsConnected = false;
        // private static DataBaseHelper DBH = new DataBaseHelper();

        public SerialPort serialPort;


        private string rPath = "c:\\temp\\Dimension_Xpand_Plus_V2_R.txt";
        private string qPath = "c:\\temp\\Dimension_Xpand_Plus_V2_Q.txt";
        public void SerialPort()
        {
            
        }

        public void start()
        {
           // CustomHelper.Debug("Start Called");
        }


        public Dimension_Xpand_Plus_V2(string portName)
        {
            Parity parity = Parity.None;
            int dataBits = 8;
            StopBits stopBits = StopBits.One;
            serialPort = new SerialPort();


            serialPort.BaudRate = 9600;
            serialPort.DataBits = dataBits;
            serialPort.StopBits = stopBits;
            serialPort.Parity = parity;
            serialPort.DtrEnable = true;
            serialPort.RtsEnable = true;
            serialPort.PortName = portName;
            serialPort.DataReceived += SerialPort_DataReceived;




            // // serialPort.Open();
            // Thread.Sleep(10000);

        }

        public bool portAction()
        {
            //MessageHelper.Instance.Text = "Connecting.....";
            //apiHelper.GetAccessToken();
            if (serialPort.IsOpen)
            {
                //CheckMachineOrder("10002477", "01");
                serialPort.Close();
               // MessageHelper.Instance.Text = "Server Disconnected.";
                return false;
            }
            else
            {
                //SerialPort();
                //this.SendTestOrder(serialPort, "1001515701");

                //CheckMachineOrder("10002477", "01");
                serialPort.Open();
                IsConnected=true;

                // MessageHelper.Instance.Text = "Connected & Server started. Waiting for response.....";
                return true;
            }

        }

        private void CheckMachineOrder(string sampleId, string serial)
        {
            //SampleList sl = new SampleList();
            //sl.SampleId = Int64.Parse(sampleId);
            //sl.SerialNo = serial;
            //sl.Date = DateTime.Now.Date.ToString();
            //sl.Time = DateTime.Now.TimeOfDay.ToString();
            //sl.TestName = "";
            //sl.Optional = "";

            //reportDictionary = apiHelper.GetMachineCodes(sl);

            //var testCode = GetMachineTestCodeBl(reportDictionary);
        }

        //private (string str2, int num) GetMachineTestCodeBl(Dictionary<string, DataModel> reportDictionary)
        //{
        //    var i = 0;
        //    var testCode = "";
        //    SpecimenType = "1";

        //    foreach (var item in reportDictionary)
        //    {

        //        var machineCode = item.Key.ToString();
        //        if (machineCode == "HB1C")
        //        {
        //            SpecimenType = "w";
        //        }
        //        i++;
        //        testCode = string.Concat(testCode, machineCode, fs);
        //        //localTestCode = "";
        //    }

        //    return (testCode, i);
        //}

        private async void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            SerialPort serialPort = (SerialPort)sender;

            string str;
            string bedNo;
            string[] strArrays;
            string[] strArrays1;
            char[] chrArray;
            Thread.Sleep(1000);
            //Thread.Sleep(100);
            if (serialPort.IsOpen)
            {
                int bytesToRead = serialPort.BytesToRead;
                byte[] numArray = new byte[bytesToRead];
                serialPort.Read(numArray, 0, bytesToRead);
                string str1 = (new UTF8Encoding()).GetString(numArray);

               // CustomHelper.Debug("Data Recieved: " + str1);

                string[] strArrays2 = new string[0];
                try
                {
                    if (str1.Length > 2)
                    {
                        //CustomHelper.Debug(string.Concat("RECEIVE : ", this.StringToHex(str1.Replace("\n", "")), "\n"));

                        str1 = str1.Replace("\n", "");
                        if (str1.Contains(string.Concat(stx, "P")))
                        {
                            this.SendAck(serialPort);
                            this.SendNoRequest(serialPort);
                        }
                        else if (str1.Contains(string.Concat(stx, "M")))
                        {
                            this.SendAck(serialPort);
                        }
                        else if (str1.Contains(string.Concat(stx, "I")))
                        {
                            //dmList = new List<DataModel>();
                            //IsOrderProcessing = 1;
                            //strArrays1 = new string[] { fs };
                            //strArrays2 = str1.Split(strArrays1, StringSplitOptions.None);
                            //str = strArrays2[1];
                            //_barcode = str;
                            //if (!str.Contains("-"))
                            //{
                            //    _OrderNo = _barcode;
                            //    this.parametertype = "";
                            //}
                            //else
                            //{
                            //    chrArray = new char[] { '-' };
                            //    strArrays = str.Split(chrArray);
                            //    _OrderNo = strArrays[0];
                            //    this.parametertype = strArrays[1];
                            //}
                            //CustomHelper.Debug(_OrderNo + " <===> " + stx);
                            //CustomHelper.WriteToFile(str1.ToString(), qPath);
                            //this.SendAck(serialPort);
                            //this.SendTestOrder(serialPort, _OrderNo);
                        }
                        else if (str1.Contains(string.Concat(stx, "R")))
                        {
                            //dmList = new List<DataModel>();
                            //this.SendAck(serialPort);
                            //this.SendResultAcceptance(serialPort);


                            //CustomHelper.Debug("Result: " + str1);
                            //IsResultProcessing = 1;
                            //strArrays1 = new string[] { fs };
                            //strArrays2 = str1.Split(strArrays1, StringSplitOptions.None);
                            //str = strArrays2[3];
                            //bedNo = strArrays2[2];
                            //sampleType = strArrays2[4];

                            //_barcode = str;

                            //CustomHelper.WriteToFile(str1.ToString(), rPath);


                            //MessageHelper.Instance.Text = $"Sample ID ---------------------------------------------------------------------------------------- {_barcode}";

                            //Regex tenDigitRegex = new Regex(@"^\d{10}$");

                            //if (tenDigitRegex.IsMatch(_barcode))
                            //{
                            //    CustomHelper.Debug("Got 10 digit");
                            //    // Remove the last two digits
                            //    sampleData.SampleId = _barcode.Substring(0, _barcode.Length - 2);
                            //    // Extract the last two digits
                            //    sampleData.Sl = _barcode.Substring(_barcode.Length - 2);


                            //    SampleList sl = new SampleList
                            //    {
                            //        SampleId = long.Parse(sampleData.SampleId),
                            //        SerialNo = sampleData.Sl,
                            //        Date = DateTime.Now.Date.ToString(),
                            //        Time = DateTime.Now.TimeOfDay.ToString(),
                            //        TestName = "",
                            //        Optional = ""
                            //    };

                            //    CustomHelper.Debug("SampleId: " + sl.SampleId + "-" + sl.SerialNo);
                            //    CustomHelper.Debug("Calling API");

                            //    reportDictionary = new Dictionary<string, DataModel>();
                            //    reportDictionary = await apiHelper.GetMachineCodesAsync(sl);
                            //    //reportDictionary = apiHelper.GetMachineCodesAsync(sl);
                            //    CustomHelper.Debug("Machine codes: " + reportDictionary.Count);
                            //    CustomHelper.Debug(reportDictionary.ToString());

                            //    testSequence = strArrays2[10];
                            //    int arrayLenth = strArrays2.Count();
                            //    int num1 = 11;
                            //    int testTotal = Convert.ToInt32(testSequence);
                            //    int arrayMultiply = testTotal - 1;
                            //    int exactLenth = num1 + (4 * arrayMultiply);

                            //    if (arrayLenth > exactLenth)
                            //    {

                            //        for (int i = 1; i <= testTotal; i++)
                            //        {
                            //            _testCode = strArrays2[num1];
                            //            _testResult = strArrays2[num1 + 1];
                            //            _testUnit = strArrays2[num1 + 2];
                            //            _errorCode = strArrays2[num1 + 3];

                            //            _testdateTime = DateTime.Now.ToString("dd/MMM/yyyy hh:mmtt");

                            //            if (!string.IsNullOrEmpty(_testCode) && !string.IsNullOrEmpty(_testResult))
                            //            {
                            //                bool IsDigits = _barcode.All(char.IsDigit);
                            //                if (IsDigits == true)
                            //                {
                            //                    //if (errorCode.Contains<string>(_errorCode) ? false : _errorCode.Length == 0)

                            //                    //// save Result
                            //                    //this.SaveBiochemResult(_barcode, _testCode, _testResult, _machineName, sampleType);

                            //                    if (_testCode != "")
                            //                    {
                            //                        // MessageHelper.Instance.Text = $"Result ----------{_testCode}------------------------------------------------------------ {_testResult}";

                            //                        if (!reportDictionary.ContainsKey(_testCode)) continue;

                            //                        DataModel dataModel = new DataModel { MapKey = _testCode };
                            //                        dataModel.IsFormetted = true;
                            //                        dataModel.FormattedValue = _testResult;

                            //                        dataModel.Label = reportDictionary[dataModel.MapKey].Label;

                            //                        if (dmList.All(dm => dm.MapKey != dataModel.MapKey))
                            //                        {
                            //                            dmList.Add(dataModel);
                            //                        }
                            //                    }

                            //                    //if (!errorCode.Contains<string>(_errorCode))
                            //                    //{
                            //                    //    this.SaveBiochemResult(_barcode, _testCode, _testResult, _machineName, sampleType);
                            //                    //}
                            //                }
                            //                else
                            //                {
                            //                    //// save QC Result
                            //                    //this.SaveBiochemQcResult(_barcode, _testCode, _testResult, _testUnit, _machineName);
                            //                }
                            //            }
                            //            num1 = num1 + 4;
                            //        }

                            //        // sampleData.Reports = ReportList;


                            //        //this.SendAck(serialPort);
                            //        //this.SendResultAcceptance(serialPort);
                            //    }
                            //    else
                            //    {
                            //        this.SendAck(serialPort);
                            //        this.SendResultAcceptance(serialPort);
                            //    }

                            //    MessageHelper.Instance.Text = $"Data Processing --------------------------------------------------------------------------------  Completed";

                            //    // Insert Result
                            //    if (dmList.Count > 0)
                            //    {
                            //        ApplyResultModificationBl(dmList);
                            //        ApplyShowDisplayBl(dmList, sl);

                            //        MessageHelper.Instance.Text = $"🔄 Storing data for {sl.SampleId} {sl.SerialNo}...";

                            //        _ = Task.Run(async () =>
                            //        {
                            //            try
                            //            {
                            //                bool isStored = await apiHelper.ConnectAndUpdateDataAsync(dmList, sl);
                            //                string status = isStored
                            //                    ? $"✅ Data Store for {sl.SampleId} {sl.SerialNo} — Completed"
                            //                    : $"❌ Data Store for {sl.SampleId} {sl.SerialNo} — Failed";

                            //                // If MessageHelper is thread-safe, this is fine. Otherwise, marshal to UI thread.
                            //                MessageHelper.Instance.Text = status;
                            //            }
                            //            catch (Exception ex)
                            //            {
                            //                MessageHelper.Instance.Text = $"💥 Error storing data: {ex.Message}";
                            //            }
                            //        });
                            //    }
                            //    else
                            //    {
                            //        CustomHelper.Debug("Map key not valid!");
                            //    }

                            //    ////  clear old sample data
                            //    sampleData = new SampleData();
                            //    ReportList = new Dictionary<string, Report>();
                            //    //dmList = new List<DataModel>();

                            //    MessageHelper.Instance.Text = $"";
                            //}
                            //else
                            //{
                            //    MessageHelper.Instance.Text = $"--------------------------------------------- Invalid Sample ID ---------------------------------------------";
                            //    //this.SendAck(serialPort);
                            //    //this.SendResultAcceptance(serialPort);
                            //}
                        }
                        else if (!str1.Contains(string.Concat(stx, "C")))
                        {
                            this.SendAck(serialPort);
                        }
                        else
                        {
                            this.SendAck(serialPort);
                            this.SendResultAcceptance(serialPort);
                        }
                    }
                    else
                    {
                        if (str1.IndexOf(enq) != -1)
                        {
                            //CustomHelper.Debug("RECEIVE : <ENQ>");
                            this.SendAck(serialPort);
                        }
                        if (str1.IndexOf(nack) != -1)
                        {
                           // CustomHelper.Debug("RECEIVE : <NAK>\n");
                        }
                        if (str1.IndexOf(ack) != -1)
                        {
                           // CustomHelper.Debug("RECEIVE : <ACK>\n");
                        }
                        else if (str1.IndexOf(eot) != -1)
                        {
                            //CustomHelper.Debug("RECEIVE : <EOT>\n");
                            if (IsOrderProcessing == 1)
                            {
                                IsOrderProcessing = 0;
                                this.SendEnq(serialPort);
                            }
                            if (IsResultProcessing == 1)
                            {
                                IsResultProcessing = 0;
                            }
                        }
                    }
                }
                catch (Exception exception1)
                {
                    Exception exception = exception1;
                   // CustomHelper.Debug(string.Concat(exception.Message, "\n"));
                }
            }
        }


        public static string CheckSum(string msg)
        {
            string str = null;
            double num = 0;
            for (int i = 0; i < msg.Length; i++)
            {
                char chr = msg[i];
                num = num + Convert.ToDouble(Convert.ToInt32(chr));
            }
            uint num1 = Convert.ToUInt32(num);
            string str1 = string.Format("{0:X2}", num1);
            if (str1.Length == 0)
            {
                str = "00";
            }
            else if (str1.Length != 1)
            {
                str = str1.Substring(checked(str1.Length - 2), 2);
            }
            else
            {
                str = "0";
                str = string.Concat(str, str1);
            }
            return str;
        }

        public string StringToHex(string TextValue)
        {
            char[] charArray = TextValue.ToCharArray();
            string str = "";
            char[] chrArray = charArray;
            for (int i = 0; i < (int)chrArray.Length; i++)
            {
                int num = Convert.ToInt32(chrArray[i]);
                str = string.Concat(str, string.Format("{0:X}", num));
            }
            return TextValue;
        }

        public static byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            return buffer;
        }

        #region Send For Dimension RxL

        private void SendNoRequest(SerialPort serialPort)
        {
            try
            {
                string str = string.Concat("N", fs);
                str = string.Concat(stx, str, CheckSum(str), etx);
                byte[] bytes = Encoding.UTF8.GetBytes(str);
                serialPort.Write(bytes, 0, (int)bytes.Length);
                //CustomHelper.Debug(string.Concat("SEND No Request:", str, "\n"));
            }
            catch (FormatException)
            {
               // CustomHelper.Debug("Not properly formatted hex string:");
            }
        }

        private void SendResultAcceptance(SerialPort serialPort)
        {
            try
            {
                string[] strArrays = new string[] { "M", fs, "A", fs, fs };
                string str = string.Concat(strArrays);
                str = string.Concat(stx, str, CheckSum(str), etx);
                byte[] bytes = Encoding.UTF8.GetBytes(str);
                serialPort.Write(bytes, 0, (int)bytes.Length);
                string str1 = string.Concat(vSTX, str, vETX);
               // CustomHelper.Debug(string.Concat("SEND Result Acceptance: ", str1, "\n"));
            }
            catch (FormatException)
            {
               // CustomHelper.Debug("Not properly formatted hex string:");
            }
        }

        private async void SendTestOrder(SerialPort serialPort, string pBarcode)
        //public void SendTestOrder(string pBarcode)
        {
            string strNames = "";
            string str = "";
            string str1 = "1"; // Serum
            string str2 = "";
            string str3 = "";
            bool containsHB1C = false;
            int num = 0;
            string patientInfo = "";
            string LocationAsAGE = "";
            try
            {
                patientInfo = pBarcode;
                bool allDigits = pBarcode.Trim().All(char.IsDigit);
                if (allDigits == true)
                {
                    // Remove the last two digits
                    string SampleId = pBarcode.Substring(0, pBarcode.Length - 2);

                    //// Extract the last two digits
                    string Sl = pBarcode.Substring(pBarcode.Length - 2);



                    //reportMachineNames = DBH.getItemsSP(SampleId, Sl);

                    //foreach (DataRow row in reportMachineNames.Rows)
                    //{
                    //    if (!string.IsNullOrEmpty(row["MachineUnitName"].ToString()))
                    //    {

                    //        num++;
                    //        str = row["MachineUnitName"].ToString();
                    //        if (str == "HB1C")
                    //        {
                    //            str1 = "W";
                    //        }
                    //        //str = "GLUC";
                    //        str2 = string.Concat(str2, str.ToUpper(), fs);

                    //        strNames += ((strNames != "") ? "^^^" : "") + str;
                    //        //str2 = string.Concat(str2, str, fs);
                    //        //str1 = obj.SampleType.ToString();

                    //        //str1 = "1"; // Serum
                    //    }

                    //}

                    //SampleList sl = new SampleList();
                    //sl.SampleId = Int64.Parse(SampleId);
                    //sl.SerialNo = Sl;
                    //sl.Date = DateTime.Now.Date.ToString();
                    //sl.Time = DateTime.Now.TimeOfDay.ToString();
                    //sl.TestName = "";
                    //sl.Optional = "";

                    //CustomHelper.Debug("Calling API to get machine codes");
                    //reportDictionary = await apiHelper.GetMachineCodesAsync(sl);

                    //(str2, num) = GetMachineTestCodeBl(reportDictionary);
                    //str1 = SpecimenType;
                    //CustomHelper.Debug("Machine codes Query: " + str2);
                }

                if (str2.Length == 0)
                {
                    str2 = string.Concat("AAA", fs);
                   // CustomHelper.Debug(string.Concat("No Test Order found for ", pBarcode, "\n"));
                }

                //MessageHelper.Instance.Text = $"Query ---------------------------------------------------------------------------------------- {pBarcode}";
                //MessageHelper.Instance.Text = $"Test Sending ---------------------------------------------------------------------------------------- Start";
                //MessageHelper.Instance.Text = $"Test Are -------- {str2}";

                //object[] objArray = new object[] { "D", fs, "0", fs, "0", fs, "A", fs, patientInfo, fs, _barcode, fs, str1, fs, fs, "0", fs, "1", fs, "**", fs, "1", fs, num, fs, str2 };
                object[] objArray = new object[] { "D", fs, "0", fs, "0", fs, "A", fs, patientInfo, fs, pBarcode, fs, str1, fs, LocationAsAGE, fs, "0", fs, "1", fs, "**", fs, "1", fs, num, fs, str2 };
                str3 = string.Concat(objArray);
                str3 = string.Concat(stx, str3, CheckSum(str3), etx);
                //CustomHelper.Debug(str3);
                byte[] bytes = Encoding.UTF8.GetBytes(str3);
                serialPort.Write(bytes, 0, (int)bytes.Length);

                //MessageHelper.Instance.Text = $"Test Sending ---------------------------------------------------------------------------------------- Completed";
               // MessageHelper.Instance.Text = $"-------------------------------- {pBarcode} ------------------------------";
               // MessageHelper.Instance.Text = $"";
            }
            catch (FormatException)
            {
              //  CustomHelper.Debug("Not properly formatted hex string:");
            }
        }

        private void SendNak(SerialPort serialPort)
        {
            try
            {
                byte[] SendNak = HexStringToByteArray(nack);

                serialPort.Write(SendNak, 0, SendNak.Length);

                //Log(LogMsgType.Outgoing, ByteArrayToHexString(SendNak) + "\n");
                //Log(LogMsgType.Outgoing, SendToMachine + MachineDataFormat.vNAK + "\n");

            }
            catch (FormatException)
            {
                //Log(LogMsgType.Error, "Not properly formatted hex string:" + txtResult.Text + "\n");
            }

        }

        private void SendEnq(SerialPort serialPort)
        {
            try
            {
                byte[] SendEnq = HexStringToByteArray(enq);

                serialPort.Write(SendEnq, 0, SendEnq.Length);

                //Log(LogMsgType.Outgoing, ByteArrayToHexString(SendEnq) + "\n");
                //Log(LogMsgType.Outgoing, SendToMachine + MachineDataFormat.vENQ + "\n");

            }
            catch (FormatException)
            {
                //CustomHelper.Debug("Not properly formatted hex string");
            }
        }

        private void SendAck(SerialPort serialPort)
        {
            try
            {

                byte[] ackconvert = HexStringToByteArray("06");
                serialPort.Write(ackconvert, 0, ackconvert.Length);
                //Log(LogMsgType.Outgoing, ByteArrayToHexString(ackconvert) + "\n");
                //CustomHelper.Debug(SendToMachine + vACK + "\n");
            }
            catch (FormatException)
            {
                //CustomHelper.Debug("Not properly formatted hex string:");
            }

        }

        private void SendEOT(SerialPort serialPort)
        {
            try
            {
                byte[] eotconvert = HexStringToByteArray("04");
                serialPort.Write(eotconvert, 0, eotconvert.Length);

                //Log(LogMsgType.Outgoing, ByteArrayToHexString(eotconvert) + "\n");
                //Log(LogMsgType.Outgoing, SendToMachine + "<EOT>\n");

            }
            catch (FormatException)
            {
                //CustomHelper.Debug("Not properly formatted hex string:");
            }

        }

        #endregion


        //R*100028630110491114 70524115NA139mmol/LK2.4mmol/LCL103mmol/LALB1.7g/dLCRE20.66mg/dLEDR*163310491114 70524115NA139mmol/LK2.4mmol/LCL103mmol/LALB1.7g/dLCRE20.66mg/dLEDR*163310491114 70524115NA139mmol/LK2.4mmol/LCL103mmol/LALB1.7g/dLCRE20.66mg/dLEDR*163310491114 70524115NA139mmol/LK2.4mmol/LCL103mmol/LALB1.7g/dLCRE20.66mg/dLEDR*163310491114 70524115NA139mmol/LK2.4mmol/LCL103mmol/LALB1.7g/dLCRE20.66mg/dLEDR*163310491114 70524115NA139mmol/LK2.4mmol/LCL103mmol/LALB1.7g/dLCRE20.66mg/dLEDR*163310491114 70524115NA139mmol/LK2.4mmol/LCL103mmol/LALB1.7g/dLCRE20.66mg/dLEDR*163310491114 70524115NA139mmol/LK2.4mmol/LCL103mmol/LALB1.7g/dLCRE20.66mg/dLEDR*163310491114 70524115NA139mmol/LK2.4mmol/LCL103mmol/LALB1.7g/dLCRE20.66mg/dLEDR*163310491114 70524115NA139mmol/LK2.4mmol/LCL103mmol/LALB1.7g/dLCRE20.66mg/dLEDR*163310491114 70524115NA139mmol/LK2.4mmol/LCL103mmol/LALB1.7g/dLCRE20.66mg/dLEDR*163310491114 70524115NA139mmol/LK2.4mmol/LCL103mmol/LALB1.7g/dLCRE20.66mg/dLEDR*163310491114 70524115NA139mmol/LK2.4mmol/LCL103mmol/LALB1.7g/dLCRE20.66mg/dLEDR*163310491114 70524115NA139mmol/LK2.4mmol/LCL103mmol/LALB1.7g/dLCRE20.66mg/dLEDR*163310491114 70524115NA139mmol/LK2.4mmol/LCL103mmol/LALB1.7g/dLCRE20.66mg/dLEDR*163310491114 70524115NA139mmol/LK2.4mmol/LCL103mmol/LALB1.7g/dLCRE20.66mg/dLEDR*163310491114 70524115NA139mmol/LK2.4mmol/LCL103mmol/LALB1.7g/dLCRE20.66mg/dLEDR*163310491114 70524115NA139mmol/LK2.4mmol/LCL103mmol/LALB1.7g/dLCRE20.66mg/dLEDR*163310491114 70524115NA139mmol/LK2.4mmol/LCL103mmol/LALB1.7g/dLCRE20.66mg/dLEDR*163310491114 70524115NA139mmol/LK2.4mmol/LCL103mmol/LALB1.7g/dLCRE20.66mg/dLEDR*163310491114 70524115NA139mmol/LK2.4mmol/LCL103mmol/LALB1.7g/dLCRE20.66mg/dLEDR*163310491114 70524115NA139mmol/LK2.4mmol/LCL103mmol/LALB1.7g/dLCRE20.66mg/dLEDR*163310491114 70524115NA139mmol/LK2.4mmol/LCL103mmol/LALB1.7g/dLCRE20.66mg/dLEDR*1893-r10212614 70524111GLUC8.52mmol/L02R*1630-r10422614 70524111GLUC6.54mmol/LFAR*1376W1452814 70524111A1C10.3%02R*1379-f10073314 70524111GLUC4.32mmol/LF1R*1881-r10313814 70524111GLUC5.48mmol/L05R*881-a10334114 70524111GLUC6.49mmol/LC1R*1317-r10494114 70524111GLUC4.85mmol/L02R*1016-r10574114 70524111GLUC5.77mmol/LFFR*165110065014 70524117NA144mmol/LK2.5mmol/LCL108mmol/LALTI82U/LALB1.8g/dLBUN171mg/dL3CRE24.50mg/dL93R*170310205014 70524114NA128mmol/LK4.0mmol/LCL90mmol/LCRE20.70mg/dL93R*172110235414 70524112TP9.1g/dLCRE21.16mg/dLACR*1530-r10014114 70524111GLUC5.80mmol/LEFR*1806-a10544014 70524111GLUC6.55mmol/LEE





        //private static Report addToReportRow(string name, string result, string unit)
        //{
        //    Report _report = new Report();

        //    _report.Type = "None";
        //    _report.Sequence = 0;
        //    _report.Name = Convert.ToString(name);
        //    _report.Value = Convert.ToString(result);
        //    _report.Unit = Convert.ToString(unit);
        //    _report.Fags = "None";
        //    _report.Status = "None";
        //    _report.DateTime = DateTime.Now;

        //    return _report;
        //}


        public static string getSampleId(string input = "R*8510-R10494713 40524111FI35.70mmol/LE1")
        {

           // CustomHelper.Debug("Sample Id String: " + input);
            // Define the regular expression pattern
            string pattern = @"R*(\d+)-";

            // Match the pattern against the input string
            Match match = Regex.Match(input, pattern);

            // Check if a match is found
            if (match.Success)
            {
                // Extract the sample ID from the matched group
               // CustomHelper.Debug("Sample Id Found: " + match.Groups[1].Value);
                return match.Groups[1].Value;
            }
            else
            {
              //  CustomHelper.Debug("No Sample Id Not Found");
                return "";
            }
        }

        public static string getTestName(string input = "R*8510-R10494713 40524111FI35.70mmol/LE1")
        {
            string[] testNames = {
            "ADD", "CHOL", "FT4", "LNTP", "RCRP", "VANC", "ACP", "CKI", "FT4L", "LPBN",
            "SAL", "VB12", "ACTM", "COC", "GENT", "LTNI", "SIRO", "VITD", "AHDL", "CL",
            "GGT", "MALB", "T4", "WALT", "AIC", "CRBM", "GLUC", "MBI", "TAC", "WAP",
            "ALB", "CRE2", "HB1C", "METH", "TBI", "WAST", "ALDL", "CRP", "HCG", "MG",
            "TC02", "WBLP", "ALPI", "CSA", "HGLC", "MMB", "TGL", "WCE", "ALTI", "CSAE",
            "HIL", "MPAT", "THC", "WCHO", "AMM", "CTNI", "IBCT", "MYO", "THEO", "WCK",
            "AMPH", "DBI", "IGA", "NA", "TNI", "WCRE", "AMY", "DGNA", "IGG", "NAPA",
            "TNIH", "WDB", "AST", "DGTX", "IGM", "NTP", "TOBR", "WIP", "BARB", "DIBC",
            "IRON", "OPI", "WLAP", "BENZ", "ECO2", "K", "PALB", "TPSA", "WLDH", "BNP",
            "ETOH", "LA", "PBNP", "TRNF", "WPL", "BUN", "EXTC", "LDI", "PCHE", "TSH",
            "WTB", "C3", "EZCR", "LHCG", "PCP", "TSHL", "WTG", "C4", "FERR", "LI",
            "PHNO", "TU", "WZTT", "CA", "FOLA", "LIDO", "PHOS", "UCFP", "CCRP", "FPSA",
            "LIPL", "PROC", "URCA", "CHK", "FI3", "LMMB", "PTN", "VALP"
        };

            // CustomHelper.Debug($"Input: {input}");

            string[] parse = input.Split('');

            if (parse.Length == 3)
            {
                string foundTestName = testNames.FirstOrDefault(t => (parse[0] == t));
                if (foundTestName != "")
                {
                    return foundTestName;
                }

                return "";
            }


            string foundTestName2 = testNames.FirstOrDefault(t => input.Contains(t));

            if (foundTestName2 != null)
            {
              //  CustomHelper.Debug($"Found test name: {foundTestName2}");
                return foundTestName2;
            }
            else
            {
                //CustomHelper.Debug("No test name found in input string.");
                return "";
            }
        }

        public static string getUnitName(string input = "R*8510-R10494713 40524111FI35.70mmol/LE1")
        {
            string[] units = { "mmol/L", "mg/dl", "U/L", "g/dl", "UL", "IU/ml", "mg/L", "%", "μg/dL" };

            string foundUnitName = units.FirstOrDefault(t => input.Contains(t));

            if (foundUnitName != null)
            {
               // CustomHelper.Debug($"Found Unit name: {foundUnitName}");
                return foundUnitName;
            }
            else
            {
                //CustomHelper.Debug("No test name found in input string.");
                return "";
            }
        }

        public static double getResult(string input = "5.70mmol/LE1")
        {
           // CustomHelper.Debug("GetResult: " + input);
            // Define a regular expression pattern to match a decimal number
            string pattern = @"(\d+(\.\d+)?)";

            // Match the pattern in the input string
            Match match = Regex.Match(input, pattern);

            // If a match is found
            if (match.Success)
            {
                // Get the matched value as a string
                string matchedValue = match.Value;

                // Parse the matched value into a double
                if (double.TryParse(matchedValue, out double result))
                {
                    //CustomHelper.Debug("Extracted value: " + result);
                    return result;
                }
                else
                {
                   // CustomHelper.Debug("Failed to parse the number.");
                    return 0.00;
                }
            }
            else
            {
               // CustomHelper.Debug("No number found in the input string.");
                return 0.00;
            }
        }

    }
}

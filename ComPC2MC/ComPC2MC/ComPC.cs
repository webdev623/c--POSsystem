using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Timers;
using System.Threading;

namespace ComPC2MC
{
    public class ComPC
    {
        private System.IO.Ports.SerialPort openPort;
        public string receiveData = "";
        private string strErrorMsg = "";
        private string strNormalMsg = "";
        public int moneyAmount = 0;

        public byte[] ptReceiveData = null;
        public int[] balanceData = new int[8];
        private string strBalanceReturn = "";

        private string strBVError = "";
        private string strBDError = "";
        private string strCMError = "";
        private int nCnt = 0;

        private Dictionary<string, byte> hexindex = new Dictionary<string, byte>();

        public List<String> GetAllPorts()
        {
            List<String> allPorts = new List<string>();
            foreach (String portName in System.IO.Ports.SerialPort.GetPortNames())
            {
                allPorts.Add(portName);
            }
            return allPorts;
        }

        public void GetHexArray()
        {
            for (int i = 0; i <= 255; i++)
                hexindex.Add(i.ToString("X2"), (byte)i);
        }

        public void OpenComport(String strComportName)
        {
            if( openPort != null && openPort.IsOpen == true)
                openPort.Close();

            openPort = new SerialPort(strComportName, 
                                           (int)9600, 
                         System.IO.Ports.Parity.None, 
                                              (int)8,
                        System.IO.Ports.StopBits.One);
            openPort.Handshake = System.IO.Ports.Handshake.RequestToSend;
            openPort.RtsEnable = true;
            openPort.Encoding = System.Text.Encoding.Default;

            if (openPort.IsOpen == false)
                openPort.Open();
        }
        private void MicroComDataReceiveHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            if (ptReceiveData != null) ptReceiveData = null;
            string str = sp.ReadExisting();
            byte[] data = new byte[str.Length];
            data = System.Text.ASCIIEncoding.ASCII.GetBytes(str);

            if (str.Length != 0)
            {
                Console.WriteLine("Data Received:" + data[0]);
                ptReceiveData = data;
                strBalanceReturn = BitConverter.ToString(ptReceiveData);
            }
        }

        public void CloseComport()
        {
            if (openPort.IsOpen == true)
                openPort.Close();
        }

        public bool SendReceiveInitData()
        {
            bool bret = true;
            byte[] cmdByteArray = new byte[5];
            openPort.DiscardInBuffer();
            openPort.DiscardOutBuffer();
            // Send byte data
            cmdByteArray[0] = 0x00;
            cmdByteArray[1] = 0xFF;
            cmdByteArray[2] = 0x01;
            cmdByteArray[3] = 0x01;
            cmdByteArray[4] = 0x00;

            openPort.Write(cmdByteArray, 0, 5);

            strBalanceReturn = "";
            if (ptReceiveData != null) ptReceiveData = null;

            while (openPort.BytesToRead == 0 && nCnt < 10)
            {
                nCnt++;
                Thread.Sleep(1000);
            }

            byte[] inbyte = new byte[5];
            openPort.Read(inbyte, 0, 5);
            ptReceiveData = inbyte;
            strBalanceReturn = BitConverter.ToString(inbyte);
            
            if( nCnt >= 10 )
            {
                bret = false;
                strErrorMsg = "There is impossible to receive any data from microcontroller. Make sure the connection state or Select the correct Comport.";
            }
            nCnt = 0;
            return bret;
        }

        public bool SendResetCommand()
        {
            bool bret = false;
            byte[] cmdByteArray = new byte[5];
            openPort.DiscardInBuffer();
            openPort.DiscardOutBuffer();
            // Send byte data
            cmdByteArray[0] = 0x00;
            cmdByteArray[1] = 0xFF;
            cmdByteArray[2] = 01;
            cmdByteArray[3] = 0xFF;
            cmdByteArray[4] = 0xFE;

            openPort.Write(cmdByteArray, 0, 5);
            while (openPort.BytesToRead == 0 && nCnt < 10)
            {
                nCnt++;
                Thread.Sleep(1000);
            }
            nCnt = 0;
            byte[] inbyte = new byte[5];
            openPort.Read(inbyte, 0, 5);
            ptReceiveData = inbyte;
            bret = true;
            return bret;
        }

        public byte GetDetailStatusData(byte b1)
        {
            moneyAmount = 0;
            byte ret;
            byte[] detailStatusData = new byte[14];
            
            detailStatusData = GetStatusData(b1);

            ret = detailStatusData[3];
            byte ret1 = detailStatusData[4];

            for (int i = 0; i < 8; i++)
                balanceData[i] = 0;

            if(ret1 == 0x11 )
            {
                // Deposit Data
                moneyAmount = 10 * HtoD(detailStatusData[5]) + 50 * HtoD(detailStatusData[6]) + 100 * HtoD(detailStatusData[7]) +
                    500 * HtoD(detailStatusData[8]) + 1000 * HtoD(detailStatusData[9]) + 2000 * HtoD(detailStatusData[10]) +
                    5000 * HtoD(detailStatusData[11]) + 10000 * HtoD(detailStatusData[12]);
                for (int ii = 0; ii < 8; ii++)
                    balanceData[ii] = HtoD(detailStatusData[ii+5]);
            }
            else if (ret1 == 0x22)
            {
                //withdrawal data
                moneyAmount = 10 * HtoD(detailStatusData[5]) + 50 *(HtoD(detailStatusData[6])) + 100 * HtoD(detailStatusData[7]) +
                    500 * HtoD(detailStatusData[8]) + 1000 * HtoD(detailStatusData[9]);
                for (int ii = 0; ii < 5; ii++)
                    balanceData[ii] = ii == 1 ? HtoD(detailStatusData[ii + 5]): HtoD(detailStatusData[ii + 5]);
            }
            return ret;
        }

        public int HtoD(byte b)
        {
            int ret = 0;
            return ret = Convert.ToInt32(b.ToString(), 16);
        }


        public string tempS = "";
        private byte[] GetStatusData(byte b1)
        {
            byte[] retByte = new byte[14];
            byte[] tempbyte = new byte[2];
            tempbyte[0] = 0x01;
            tempbyte[1] = b1;
            byte[] FCC = GetXORData(tempbyte);

            byte[] cmdByteArray = new byte[5];
            // send byte data
            cmdByteArray[0] = 0x03;
            cmdByteArray[1] = 0xFC;
            cmdByteArray[2] = 0x01;
            cmdByteArray[3] = b1;
            cmdByteArray[4] = FCC[0];

            tempS = BitConverter.ToString(cmdByteArray);

            openPort.DiscardInBuffer();
            openPort.DiscardOutBuffer();

            openPort.Write(cmdByteArray, 0, 5);

            strBalanceReturn = "";
            while (openPort.BytesToRead == 0 && nCnt < 10 )
            {
                nCnt++;
                Thread.Sleep(1000);
            }
            nCnt = 0;
            openPort.Read(retByte, 0, 14);
            strBalanceReturn = BitConverter.ToString(retByte);
            return retByte;
        }
        public void GetErrorState(byte b)
        {
            byte[] tempbyte = new byte[2];
            tempbyte[0] = 0x01;
            tempbyte[1] = b;
            byte[] FCC = GetXORData(tempbyte);

            byte[] cmdByteArray = new byte[5];
            openPort.DiscardInBuffer();
            openPort.DiscardOutBuffer();

            // Send byte data
            cmdByteArray[0] = 0x05;
            cmdByteArray[1] = 0xFA;
            cmdByteArray[2] = 0x01;
            cmdByteArray[3] = b;
            cmdByteArray[4] = FCC[0];

            openPort.Write(cmdByteArray, 0, 5);

            while (openPort.BytesToRead == 0 && nCnt < 10)
            {
                nCnt++;
                Thread.Sleep(1000);
            }
            if (openPort.BytesToRead > 0)
            {
                byte[] inbyte = new byte[14];
                openPort.Read(inbyte, 0, 14);
                if (inbyte.Length > 0)
                {
                    strBalanceReturn = "";
                    strBalanceReturn = BitConverter.ToString(inbyte);
                    int[] eState = new int[6];
                    eState = StatusErrorAnal((byte)inbyte.GetValue(4));

                    byte value = (byte)inbyte.GetValue(3);

                    if (value == 0x00)
                    {
                        strBVError = "Normal.\n";
                        return;
                    }
                    else if (value == 0xDD)
                        strBVError = "Transmission data error.\n";
                    else if (value == 0xEE)
                        strBVError = "Device error.\n";

                    if (eState[0] == 1)
                        strBVError += "Banknote dispenser near-end sensor ON.\n";
                    if (eState[1] == 1)
                        strBVError += "Electricity turn off.\n";
                    if (eState[2] == 1)
                        strBVError += "Bill validator1 error occure.\n";
                    if (eState[3] == 1)
                        strBVError += "Bill validator2 error occure.\n";
                    if (eState[4] == 1)
                        strBVError += "Banknote dispenser error occure.\n";
                    if (eState[5] == 1)
                        strBVError += "Coin mesh error occure.\n";

                    strBDError = strBVError;
                    strCMError = strBVError;

                    if( b == 0x02 && eState[4] == 1)
                        BDErrorInfoAnal((byte)inbyte.GetValue(9), (byte)inbyte.GetValue(10));
                    else if( b == 0x03 && (eState[0] == 1 || eState[2] == 1) )
                    {
                        BVErrorInfoAnal((byte)inbyte.GetValue(5), (byte)inbyte.GetValue(6), "1");
                    }
                    else if (b == 0x03 && (eState[0] == 1 || eState[3] == 1))
                    {
                        BVErrorInfoAnal((byte)inbyte.GetValue(7), (byte)inbyte.GetValue(8), "2");
                    }
                    else if (b == 0x03 && (eState[0] == 1 || eState[4] == 1))
                    {
                        BDErrorInfoAnal((byte)inbyte.GetValue(9), (byte)inbyte.GetValue(10));
                    }
                    else if (b == 0x01 && eState[2] == 1)
                        BVErrorInfoAnal((byte)inbyte.GetValue(5), (byte)inbyte.GetValue(6), "1");
                    else if (b == 0x01 && eState[3] == 1)
                        BVErrorInfoAnal((byte)inbyte.GetValue(7), (byte)inbyte.GetValue(8), "2");
                    else if( b == 0x04 && eState[5] == 1)
                        CMErrorInfoAnal((byte)inbyte.GetValue(11), (byte)inbyte.GetValue(12));
                    else if ( b == 0x07 && eState[4] == 1 && eState[5] == 1 && eState[2] == 1 && eState[3] == 1)
                    {
                        BVErrorInfoAnal((byte)inbyte.GetValue(5), (byte)inbyte.GetValue(6), "1");
                        BVErrorInfoAnal((byte)inbyte.GetValue(7), (byte)inbyte.GetValue(8), "2");
                        BDErrorInfoAnal((byte)inbyte.GetValue(9), (byte)inbyte.GetValue(10));
                        CMErrorInfoAnal((byte)inbyte.GetValue(11), (byte)inbyte.GetValue(12));
                    }
                }
            }
            else
                strErrorMsg = "Cannot read any data from Microcontroller.";
            nCnt = 0;
        }

        public string GetBVErrorAnalResult() { return strBVError; }
        public string GetBDErrorAnalResult() { return strBDError; }
        public string GetCMErrorAnalResult() { return strCMError; }

        public string GetErrorMessage() { return strErrorMsg; }
        public string GetNormalMessage() { return strNormalMsg; }
        public int[] GetMoneyAmount() { return balanceData; }
        public string GetbalanceByteData() { return strBalanceReturn; }
        public static string DecimalToHexadecimal(int dec)
        {
            if (dec < 1) return "00";

            int hex = dec;
            string hexStr = string.Empty;

            while (dec > 0)
            {
                hex = dec % 16;

                if (hex < 10)
                    hexStr = hexStr.Insert(0, Convert.ToChar(hex + 48).ToString());
                else
                    hexStr = hexStr.Insert(0, Convert.ToChar(hex + 55).ToString());

                dec /= 16;
            }

            if (dec < 17) hexStr = "0" + hexStr;
            return hexStr;
        }
        public static byte[] ToByteArray(string value)
        {
            char[] charArr = value.ToCharArray();
            byte[] bytes = new byte[charArr.Length];
            for (int i = 0; i < charArr.Length; i++)
            {
                byte current = Convert.ToByte(charArr[i]);
                bytes[i] = current;
            }

            return bytes;
        }
        public byte[] StrToByteArray(string str)
        {
            List<byte> hexres = new List<byte>();
            int len = (str.Length / 2) * 2;
            if(len > 0 )
                for (int i = 0; i < len ; i += 2)
                    hexres.Add(hexindex[str.Substring(i, 2)]);

            return hexres.ToArray();
        }
        private byte[] GetXORData(byte[] tpByte)
        {
            byte ret = tpByte[0];
            int nlen = tpByte.Length;

            for ( int i = 1; i < nlen; i++)
            {
                ret = (byte)(ret ^ tpByte[i]);
            }
            string str = DecimalToHexadecimal(ret);
            return StrToByteArray(str.Substring(str.Length - 2, 2));
        }

        public bool SendReceivePermissionAndProhibition(byte perByte, byte prohByte)
        {
            bool bret = false;
            strErrorMsg = "";
            byte[] tempbyte = new byte[3];
            tempbyte[0] = 0x02;
            tempbyte[1] = perByte;
            tempbyte[2] = prohByte;
            byte[] FCC = GetXORData(tempbyte);

            byte[] cmdByteArray = new byte[6];
            openPort.DiscardInBuffer();
            openPort.DiscardOutBuffer();

            // Send byte data
            cmdByteArray[0] = 0x01;
            cmdByteArray[1] = 0xFE;
            cmdByteArray[2] = 0x02;
            cmdByteArray[3] = perByte;
            cmdByteArray[4] = prohByte;
            cmdByteArray[5] = FCC[0];

            string temp = BitConverter.ToString(cmdByteArray);

            openPort.Write(cmdByteArray, 0, 6);

            while (openPort.BytesToRead == 0 && nCnt < 10)
            {
                nCnt++;
                Thread.Sleep(1000);
            }
            strBalanceReturn = "";
            byte[] inbyte = new byte[6];
            openPort.Read(inbyte, 0, 6);
            ptReceiveData = inbyte;
            strBalanceReturn = BitConverter.ToString(inbyte);
            bret = true;
            nCnt = 0;
            return bret;
        }

        public bool SendReceiveWithdrawal(byte b1, byte b2, byte b3, byte b4)
        {
            bool bret = false;
            strErrorMsg = "";

            byte[] tempbyte = new byte[5];
            tempbyte[0] = 0x04;
            tempbyte[1] = b1;
            tempbyte[2] = b2;
            tempbyte[3] = b3;
            tempbyte[4] = b4;
            byte[] FCC = GetXORData(tempbyte);

            byte[] cmdByteArray = new byte[8];
            openPort.DiscardInBuffer();
            openPort.DiscardOutBuffer();

            // Send byte data
            cmdByteArray[0] = 0x02;
            cmdByteArray[1] = 0xFD;
            cmdByteArray[2] = 0x04;
            cmdByteArray[3] = b1;
            cmdByteArray[4] = b2;
            cmdByteArray[5] = b3;
            cmdByteArray[6] = b4;
            cmdByteArray[7] = FCC[0];

            strBalanceReturn = "";
            tempS = "";
            if( ptReceiveData != null ) ptReceiveData = null;

            tempS = BitConverter.ToString(cmdByteArray);

            openPort.Write(cmdByteArray, 0, 8);
            while (openPort.BytesToRead == 0 && nCnt < 10)
            {
                nCnt++;
                Thread.Sleep(1000);
            }
            if (openPort.BytesToRead > 0)
            {
                byte[] inbyte = new byte[5];
                openPort.Read(inbyte, 0, 5);
                ptReceiveData = inbyte;
                strBalanceReturn = BitConverter.ToString(inbyte);
                bret = true;
            }
            else
                strErrorMsg = "Cannot read any data from Microcontroller.";
            nCnt = 0;
            return bret;
        }

        public bool SendReceiveBalance(byte b)
        {
            strNormalMsg = "";
            strBalanceReturn = "";

            bool bret = false;
            byte[] tempbyte = new byte[2];
            tempbyte[0] = 0x01;
            tempbyte[1] = b;
            byte[] FCC = GetXORData(tempbyte);

            byte[] cmdByteArray = new byte[5];
            openPort.DiscardInBuffer();
            openPort.DiscardOutBuffer();

            // Send byte data
            cmdByteArray[0] = 0x04;
            cmdByteArray[1] = 0xFB;
            cmdByteArray[2] = 0x01;
            cmdByteArray[3] = b;
            cmdByteArray[4] = FCC[0];

            openPort.Write(cmdByteArray, 0, 5);
            while (openPort.BytesToRead == 0 && nCnt < 10)
            {
                nCnt++;
                Thread.Sleep(1000);
            }
            if (ptReceiveData != null) ptReceiveData = null;
            if (openPort.BytesToRead > 0)
            {

                byte[] inbyte = new byte[10];
                openPort.Read(inbyte, 0, 10);
                ptReceiveData = inbyte;
                if (inbyte.Length > 0)
                {
                    byte value = (byte)inbyte.GetValue(3);
                    byte val1 = (byte)inbyte.GetValue(4);
                    byte val2 = (byte)inbyte.GetValue(5);
                    byte val3 = (byte)inbyte.GetValue(6);
                    byte val4 = (byte)inbyte.GetValue(7);
                    byte val5 = (byte)inbyte.GetValue(8);
                    // do other necessary processing you may want.
                    strBalanceReturn = BitConverter.ToString(inbyte);
                    if( value == 0x00 )
                    {
                        if (val1 == 0x01 && val2 == 0x11 && val3 == 0x10 && val4 == 0x11 && val5 == 0x06)
                            strNormalMsg = "Balance state is normal";
                        else
                        {
                            if (val1 == 0x00)
                                strNormalMsg += "Banknote ejector near-end sensor is 0" + "\n";
                            if (val2 == 0x00)
                                strNormalMsg += "Coin mesh 10 yen is less" + "\n";
                            if (val3 == 0x00)
                                strNormalMsg += "Coin mesh 50 yen is less" + "\n";
                            if (val4 == 0x00)
                                strNormalMsg += "Coin mesh 100 yen is less" + "\n";
                            if (val5 == 0x00)
                                strNormalMsg += "Coin mesh 500 yen is less" + "\n";
                        }
                        bret = true;
                    }
                    else if( value == 0xDD )
                    {
                        strErrorMsg = "Transmission data Error on balance.";
                        bret = false;
                    }
                    else if( value == 0xEE)
                    {
                        strErrorMsg = "Device Error on balance.";
                        bret = false;
                    }
                }
            }
            else
                strErrorMsg = "Cannot read any data from Microcontroller.";
            nCnt = 0;
            return bret;
        }

// Error Analysis Module ///////////////////////
        private int[] StatusErrorAnal(byte b)
        {
            int[] ret = new int[6];
            ret[0] = -1;
            ret[1] = -1;
            ret[2] = -1;
            ret[3] = -1;
            ret[4] = -1;
            ret[5] = -1;
            string sb = Convert.ToString(b);
            string result = Convert.ToString(Convert.ToInt32(sb, 10), 2).PadLeft(8, '0');
            if (result.Substring(0, 1) == "1")
                ret[0] = 1;
            if (result.Substring(3, 1) == "1")
                ret[1] = 1;
            if (result.Substring(4, 1) == "1")
                ret[2] = 1;
            if (result.Substring(5, 1) == "1")
                ret[3] = 1;
            if (result.Substring(6, 1) == "1")
                ret[4] = 1;
            if (result.Substring(7, 1) == "1")
                ret[5] = 1;
            return ret;
        }
        private void BVErrorInfoAnal(byte b1, byte b2, string type)
        {
            strBVError = "";
            strBVError = "==== 紙幣識別機エラー " + type + " ====\n";

            string sb = Convert.ToString(b1);
            string result1 = Convert.ToString(Convert.ToInt32(sb, 10), 2).PadLeft(8, '0');
            sb = Convert.ToString(b2);
            string result2 = Convert.ToString(Convert.ToInt32(sb, 10), 2).PadLeft(8, '0');

            if (result1.Substring(0, 1) == "0") { strBVError += "代表異常 : 正常\n"; return; }
            else strBVError += "代表異常 : 異常\n";

            if (result1.Substring(1, 1) == "0") strBVError += "動作可 : 異常動作不可\n";
            else strBVError += "動作可 : 異常動作可\n";

            if (result1.Substring(2, 1) == "0") strBVError += "識別機異常 : 異常無\n";
            else strBVError += "識別機異常 : 異常有\n";

            if (result1.Substring(3, 1) == "0") strBVError += "エスクロ引抜異常 : 異常無\n";
            else strBVError += "エスクロ引抜異常 : 異常有\n";

            if (result2.Substring(0, 1) == "0") strBVError += "挿入部異常 : 異常無\n";
            else strBVError += "挿入部異常 : 異常有\n";

            if (result2.Substring(1, 1) == "0") strBVError += "収納部異常 : 異常無\n";
            else strBVError += "収納部異常 : 異常有\n";

            if (result2.Substring(2, 1) == "0") strBVError += "未収金異常 : 異常無\n";
            else strBVError += "未収金異常 : 異常有\n";

            if (result2.Substring(3, 1) == "0") strBVError += "引抜異常 : 異常無\n";
            else strBVError += "引抜異常 : 異常有\n";

            if (result2.Substring(4, 1) == "0") strBVError += "搬送駆動部異常 : 異常無\n";
            else strBVError += "搬送駆動部異常 : 異常有\n\n";
        }
        private void BDErrorInfoAnal(byte b1, byte b2)
        {
            strBDError = "";
            strBDError = "==== 紙幣排出機エラー情報 ====\n";

            if( b1 == 0xE0 )
            {
                strBDError += " Located In : メイン搬送路/";
                string strCode2 = Convert.ToString(b2);
                switch(b2)
                {
                    case 0x10: strBDError += "S1 \n Description : 異常, イメ検知 Sensor\n"; break;
                    case 0x11: strBDError += "S1 \n Description : Check Light Jam\n"; break;
                    case 0x12: strBDError += "S1 \n Description : Check Dark Jam\n"; break;
                    case 0x13: strBDError += "S1 \n Description : Check Size Error\n"; break;
                    case 0x14: strBDError += "S1 \n Description : 長さ枚数検知Error\n"; break;

                    case 0x20: strBDError += "S2 \n Description : 異常, 出口Sensor (10-N Type)\n"; break;
                    case 0x21: strBDError += "S2 \n Description : Light Jam\n"; break;
                    case 0x22: strBDError += "S2 \n Description : Dark Jam\n"; break;
                    case 0x23: strBDError += "S2 \n Description : 異常紙幣検知\n"; break;
                    case 0x24: strBDError += "S2 \n Description : 検知基準Count未達\n"; break;

                    case 0x30: strBDError += "S3 \n Description : 異常, 出口Sensor\n"; break;
                    case 0x31: strBDError += "S3 \n Description : Light Jam\n"; break;
                    case 0x32: strBDError += "S3 \n Description : Dark Jam\n"; break;
                    case 0x33: strBDError += "S3 \n Description : 異常紙幣検知\n"; break;
                    case 0x34: strBDError += "S3 \n Description : 検知基準Count未達\n"; break;

                    case 0x40: strBDError += "S7 \n Description : 異常, Reject Pass Check\n"; break;
                    case 0x41: strBDError += "S7 \n Description : Light Jam\n"; break;
                    case 0x42: strBDError += "S7 \n Description : Dark Jam\n"; break;
                    case 0x43: strBDError += "S7 \n Description : 異常紙幣検知\n"; break;
                    case 0x44: strBDError += "S7 \n Description : 検知基準Count未達\n"; break;

                    case 0x50: strBDError += " \n Description : Dispense再試行5回以上\n"; break;
                    case 0x51: strBDError += " \n Description : 回収Count6回以上\n"; break;
                    case 0x52: strBDError += " \n Description : 連続回数Count6回以上\n"; break;
                    case 0x53: strBDError += " \n Description : 要求枚数以上の紙幣検知\n"; break;
                    case 0x54: strBDError += " \n Description : Pick-Up Count Over\n"; break;
                    case 0x55: strBDError += " \n Description : Dispense前出口Sensor検知\n"; break;
                    case 0x56: strBDError += " \n Description : 動作前Near End検知\n"; break;
                    case 0x57: strBDError += " \n Description : 電源On時の紙幣検知\n"; break;

                    case 0x60: strBDError += " \n Description : Left　一度に1枚検知失敗\n"; break;
                    case 0x61: strBDError += " \n Description : Right 一度に1枚検知失敗\n"; break;

                    case 0x70: strBDError += " \n Description : 異常紙幣検知, 予備\n"; break;
                    case 0x71: strBDError += " \n Description : 異常紙幣検出, 予備 \n"; break;
                    case 0x72: strBDError += " \n Description : 半折れ紙幣検知, 予備\n"; break;
                    case 0x73: strBDError += " \n Description : 長い紙幣紙幣検知, 予備\n"; break;
                    case 0x74: strBDError += " \n Description : 短い長さの紙幣検知, 予備\n"; break;
                }
            }
            else if( b1 == 0xE1 )
            {
                strBDError += " Located In : Pick-Up 部障害/";
                switch (b2)
                {
                    case 0x10: strBDError += " \n Description : Retry 5回 Pick-Up 失敗\n"; break;
                    case 0x11: strBDError += " \n Description : 紙幣 Empty 検知\n"; break;
                    case 0x12: strBDError += " \n Description : Miss-feed Retry 異常検知\n"; break;
                    case 0x13: strBDError += " \n Description : S1 Dark Jam 検知\n"; break;
                }
            }
            else if (b1 == 0xE2)
            {
                strBDError += " Located In : Communication/";
                switch (b2)
                {
                    case 0x10: strBDError += " \n Description : 受信time out、通信断絶\n"; break;
                    case 0x11: strBDError += " \n Description : 異常 Command 受信\n"; break;
                    case 0x12: strBDError += " \n Description : 受信Parameter 異常\n"; break;
                    case 0x13: strBDError += " \n Description : 要求枚数異常\n"; break;
                    case 0x14: strBDError += " \n Description : Data Size 異常\n"; break;
                    case 0x15: strBDError += " \n Description : 動作 Sequence 異常\n"; break;

                    case 0x20: strBDError += " \n Description : File Head Start Error\n"; break;
                    case 0x21: strBDError += " \n Description : File size Error\n"; break;
                    case 0x22: strBDError += " \n Description : File Size Inform Error\n"; break;
                    case 0x23: strBDError += " \n Description : Load File ID Error\n"; break;
                    case 0x24: strBDError += " \n Description : Load File Version Check Error\n"; break;
                    case 0x25: strBDError += " \n Description : Low Version F/W File Error\n"; break;
                    case 0x26: strBDError += " \n Description : Load File Unit Name Error\n"; break;
                    case 0x27: strBDError += " \n Description : File Send Flow Error\n"; break;
                    case 0x28: strBDError += " \n Description : File Size Check Error\n"; break;
                    case 0x29: strBDError += " \n Description : Load File CRC Error\n"; break;
                    case 0x2A: strBDError += " \n Description : Receive File Time Out Error\n"; break;
                }
            }
            else if (b1 == 0xE3)
            {
                strBDError += " Located In : H/W Etc./";
                switch (b2)
                {
                    case 0x10: strBDError += " \n Description : Flash Erase Error\n"; break;
                    case 0x11: strBDError += " \n Description : Flash Write Error\n"; break;
                    case 0x12: strBDError += " \n Description : Flash Read Error\n"; break;
                    case 0x13: strBDError += " \n Description : S1L Error\n"; break;
                    case 0x14: strBDError += " \n Description : S1R Error\n"; break;
                    case 0x15: strBDError += " \n Description : Encoder 検知失敗\n"; break;
                    case 0x16: strBDError += " \n Description : EEPROM 情報胃異常\n"; break;
                }
            }
        }
        private void CMErrorInfoAnal(byte b1, byte b2)
        {
            strCMError = "";
            strCMError = "==== 紙幣識別機エラー情報 ====\n";

            string sb = Convert.ToString(b1);
            string result1 = Convert.ToString(Convert.ToInt32(sb, 10), 2).PadLeft(8, '0');
            sb = Convert.ToString(b2);
            string result2 = Convert.ToString(Convert.ToInt32(sb, 10), 2).PadLeft(8, '0');

            if (result1.Substring(7, 1) == "0") { strCMError += "代表異常 : 正常\n"; }
            else strCMError += "代表異常 : 異常\n";

            if (result1.Substring(6, 1) == "0") strCMError += "動作可 : 異常動作不可\n";
            else strCMError += "動作可 : CLX-G241では０固定\n";

            if (result1.Substring(4, 1) == "0") strCMError += "アプレクター異常 : 硬貨識別機正常\n";
            else strCMError += "アプレクター異常 : 硬貨識別機異常\n";

            if (result1.Substring(3, 1) == "0") strCMError += "10円エンプティスイッチ異常 : 異常無\n";
            else strCMError += "10円エンプティスイッチ異常 : 異常有\n";

            if (result1.Substring(2, 1) == "0") strCMError += "50円エンプティスイッチ異常 : 異常無\n";
            else strCMError += "50円エンプティスイッチ異常 : 異常有\n";

            if (result1.Substring(1, 1) == "0") strCMError += "100円エンプティスイッチ異常 : 異常無\n";
            else strCMError += "100円エンプティスイッチ異常 : 異常有\n";

            if (result1.Substring(0, 1) == "0") strCMError += "500円エンプティスイッチ異常 : 異常無\n";
            else strCMError += "500円エンプティスイッチ異常 : 異常有\n";


            if (result2.Substring(0, 1) == "0") strCMError += "パルススイッチ異常 : 異常無\n";
            else strCMError += "パルススイッチ異常 : 異常有\n";

            if (result2.Substring(1, 1) == "0") strCMError += "セーフティスイッチ異常 : 正常\n";
            else strCMError += "セーフティスイッチ異常 : セーフティスイッチ異常\n";

            if (result2.Substring(2, 1) == "0") strCMError += "コイン払い出し不良 : 異常無\n";
            else strCMError += "コイン払い出し不良 : モーターのロック等で規定時間内に払い出しが出来ない\n";

            if (result2.Substring(3, 1) == "0") strCMError += "返却スイッチ : 返却スイッチ異常無\n";
            else strCMError += "返却スイッチ : 返却スイッチ異常有\n";

        }
    }
}

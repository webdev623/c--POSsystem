using ComPC2MC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ovan_P1
{

    class ComModule
    {
        ComPC dllCom = new ComPC();

        MessageDialog messageDialog = new MessageDialog();
        Constant constants = new Constant();

        private string comport = "COM6";
        private string strLog = "";
        private string logPath = "";

        private byte[] recvData = null;
        private int nRepeat = 0;
        private int[] paperBLdata = new int[5];

        private bool bActive = false;
        private Thread getDepositTh = null;
        private bool bWithdraw = false;

        private bool bPermit = true;

        public int depositAmount = 0;
        public int[] depositArr = new int[8];

        private int orderTotalPrice = 0;
        SaleScreen pSs = null;

        public void Initialize( SaleScreen ss )
        {

            logPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            logPath += "\\";
            dllCom.GetHexArray();
            StartCommunication();

            pSs = ss;
        }

        
        public void OrderChange(string orderPrice)
        {
            orderTotalPrice = int.Parse(orderPrice);
            if (!bPermit && InitCommand())
                PermitAndProhCommand(0x01, 0x01);

            if (getDepositTh == null)
            {
                getDepositTh = new Thread(GetDepositStateThread);
                getDepositTh.Start();
            }
        }

       
        public void TicketRun(int iAmount)
        {
            if (getDepositTh != null)
            {
                getDepositTh.Abort();
                getDepositTh = null;
            }

            if (iAmount <= 0) return;

            if (!InitCommand()) return;

            LogResult("SEND : ", "01-FE-02-00-00-02");
            bool bstop = dllCom.SendReceivePermissionAndProhibition(0x00, 0x00);

            if (bstop)
            {
                LogResult("RECV : ", dllCom.GetbalanceByteData());
                bstop = false;
                bstop = ReceivedDataAnalProc();
                if (bstop)
                {
                    GetDetailDepositStatus(0x01);
                    if (!bPermit)
                        WithdrawChangeAmount(iAmount);
                }
            }
            else
                MessageBox.Show(dllCom.GetErrorMessage());
        }

        public void OrderCancel(int iAmount = 0)
        {
            if (getDepositTh != null)
            {
                getDepositTh.Abort();
                getDepositTh = null;
            }
            if (!InitCommand()) return;

            bool bstop = dllCom.SendReceivePermissionAndProhibition(0x00, 0x00);
            LogResult("SEND : ", "01-FE-02-00-00-02");

            if (bstop)
            {
                LogResult("RECV : ", dllCom.GetbalanceByteData());
                bstop = false;
                bstop = ReceivedDataAnalProc();
                if (bstop)
                {
                    GetDetailDepositStatus(0x01);
                    if (!bPermit)
                        WithdrawRefund(iAmount);
                }
            }
            else
                MessageBox.Show(dllCom.GetErrorMessage());
        }
        private void GetDepositStateThread()
        {
            while (!bActive)
            {
                GetDetailDepositStatus(0x01);
                Thread.Sleep(1000);
                depositAmount = dllCom.moneyAmount;
                depositArr = dllCom.balanceData;
                Console.WriteLine(dllCom.moneyAmount.ToString());
                pSs.SetDepositAmount(depositAmount);
            }
        }

        private void StartCommunication()
        {
            if (!OpenComport())
                return;
            if (!InitCommand())
                return;
            if (!PermitAndProhCommand(0x01, 0x01))
                return;

            GetDetailDepositStatus(0x01);
        }

        private void SaveLogData()
        {
            string fileName = logPath + "log_Byte_Mode.txt";
            try
            {
                // Check if file already exists. If yes, delete it.     
                if (File.Exists(fileName))
                    File.Delete(fileName);
                // Create a new file     
                using (FileStream fs = File.Create(fileName))
                {
                    Byte[] title = new UTF8Encoding(true).GetBytes(strLog);
                    fs.Write(title, 0, title.Length);
                }
                // Open the stream and read it back.    
                using (StreamReader sr = File.OpenText(fileName))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                        Console.WriteLine(s);
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }

        
        private bool OpenComport()
        {
            bool bexist = false;
            List<string> allPorts = new List<String>();
            allPorts = dllCom.GetAllPorts();

            foreach (String portName in allPorts)
                if (portName == "COM6")
                {
                    bexist = true;
                    break;
                }

            if( !bexist )
            {
                MessageBox.Show("Please check comport and restart");
                return false;
            }
            bool ret = false;
            if (comport != "")
            {
                dllCom.OpenComport(comport);
                ret = true;
            }
            else
                MessageBox.Show("Select Comport!");
            return ret;
        }

        private bool InitCommand()
        {
            LogResult("SEND : ", "00-FF-01-01-00");
            bool bret = dllCom.SendReceiveInitData();
            if (bret)
            {
                LogResult("RECV : ", dllCom.GetbalanceByteData());
                bret = ReceivedDataAnalProc();
            }
            else
                MessageBox.Show(dllCom.GetErrorMessage());
            return bret;
        }

        private bool InitDataAnal(byte b)
        {
            bool ret = false;
            if (b == 0x11)
                ret = true;
            else if (b == 0x22)
            {
                Thread.Sleep(2000);
                ret = InitCommand();
            }
            else if (b == 0xDD)
                ret = InitCommand();
            else if (b == 0xEE)
            {
            REPEAT:
                if (nRepeat < 5)
                {
                    nRepeat++;
                    if (dllCom.SendResetCommand())
                    {
                        byte result = dllCom.ptReceiveData[3];
                        if (result == 0x11)
                        {
                            Thread.Sleep(5000);
                            ret = InitCommand();
                        }
                        else if (result == 0xDD)
                            goto REPEAT;
                        else if (result == 0xEE)
                        {
                            nRepeat = 5;
                            goto REPEAT;
                        }
                    }
                }
                else
                    GetErrorStatus(0x07, 0);
                nRepeat = 0;
            }
            return ret;
        }

        private bool PermitAndProhCommand(byte b1, byte b2)
        {
            LogResult("SEND : ", "01-FE-02-01-01-02");
            bool ret = false;
            ret = dllCom.SendReceivePermissionAndProhibition(b1, b2);
            if (ret)
            {
                LogResult("RECV : ", dllCom.GetbalanceByteData());
                ret = ReceivedDataAnalProc();
            }
            else
                MessageBox.Show(dllCom.GetErrorMessage());
            return ret;
        }
        private bool PermitAndProhDataAnal(byte b1, byte b2)
        {
            bool ret = false;
            if ((b1 == 0x11 && b2 == 0x11) || (b1 == 0x22 && b2 == 0x22))
            {
                if (b1 == 0x11 && b2 == 0x11) bPermit = true;
                if (b1 == 0x22 && b2 == 0x22) bPermit = false;
                ret = true;
            }
            else if (b1 == 0x55 && b2 == 0x55)
            {
                if (bPermit)
                    GetDetailDepositStatus(0x01);
                else
                    GetDetailWithdrawStatus();
                ret = true;
            }
            else if (b1 == 0xDD && b2 == 0xDD)
            {
                if (bPermit)
                    ret = PermitAndProhCommand(0x01, 0x01);
                else
                    ret = PermitAndProhCommand(0x00, 0x00);
            }
            else if (b1 == 0xEE && b2 == 0xEE)
            {
            REPEAT:
                if (nRepeat < 5)
                {
                    nRepeat++;
                    if (dllCom.SendResetCommand())
                    {
                        byte result = dllCom.ptReceiveData[3];
                        if (result == 0x11)
                        {
                            Thread.Sleep(2000);
                            StartCommunication();
                        }
                        else if (result == 0xDD)
                            goto REPEAT;
                        else if (result == 0xEE)
                        {
                            nRepeat = 5;
                            goto REPEAT;
                        }
                    }
                }
                else
                    GetErrorStatus(0x07, 1);
                nRepeat = 0;
            }
            return ret;
        }

        private void GetDetailDepositStatus(byte b)
        {
            byte byteStatus;
            byte[] tt = new byte[5];

            byteStatus = dllCom.GetDetailStatusData(b);
            LogResult("SEND : ", dllCom.tempS);

            LogResult("RECV : ", dllCom.GetbalanceByteData());

            if (byteStatus == 0x22 || byteStatus == 0x11)
            {
                if (byteStatus == 0x22)
                {
                    bPermit = false;
                    LogResult("Deposit Amount : ", dllCom.moneyAmount.ToString());
                }
                if (byteStatus == 0x11) bPermit = true;
            }
            else if (byteStatus == 0x33 || byteStatus == 0x44)
            {
                if (byteStatus == 0x44)
                {
                    LogResult("Deposit Amount : ", dllCom.moneyAmount.ToString());
                    bWithdraw = false;
                }
                else bWithdraw = true;
                bPermit = false;
            }
            else if (byteStatus == 0xDD)
            {
                Thread.Sleep(2000);
                GetDetailDepositStatus(b);
            }
            else if (byteStatus == 0xEE)
                GetErrorStatus(0x07, 2);
        }

        int withdrawAmount = 0;
        int[] withdrawArr = new int[8];
        private bool GetDetailWithdrawStatus()
        {
            bool ret = false;
            byte byteStatus;

            LogResult("SEND : ", "03-FC-01-02-03");
            byteStatus = dllCom.GetDetailStatusData(0x02);


            LogResult("RECV : ", dllCom.GetbalanceByteData());
            LogResult("Withdraw Amount: ", dllCom.moneyAmount.ToString());
            withdrawAmount = dllCom.moneyAmount;
            withdrawArr = dllCom.balanceData;
            if (byteStatus == 0x11 || byteStatus == 0x22)
            {
                ret = true;
                if (byteStatus == 0x11) bPermit = true;
                if (byteStatus == 0x22) bPermit = false;
            }
            else if (byteStatus == 0x33 || byteStatus == 0x44)
            {
                ret = true;
                if (byteStatus == 0x33) bWithdraw = true;
                if (byteStatus == 0x44) bWithdraw = false;
            }
            else if (byteStatus == 0xDD)
            {
                Thread.Sleep(1000);
                ret = GetDetailWithdrawStatus();
            }
            else if (byteStatus == 0xEE)
            {
                GetErrorStatus(0x07, 3);
            }
            return ret;
        }
        private void WithdrawRefund(int amount)
        {
            int nWan = amount / 10000;
            int nChon = (amount - 10000 * nWan) / 1000;
            int nBak = (amount - 10000 * nWan - 1000 * nChon) / 100;
            int nSib = (amount - 10000 * nWan - 1000 * nChon - 100 * nBak) / 10;


            byte b1 = GetByteTwoInteger(dllCom.balanceData[0] + 5 * dllCom.balanceData[1], 1);
            byte b2 = GetByteTwoInteger(2 * dllCom.balanceData[5] + 5 * dllCom.balanceData[6], dllCom.balanceData[2] + 5 * dllCom.balanceData[3]);
            byte b3 = GetByteTwoInteger(0, dllCom.balanceData[7]);
            byte bCnt = Convert.ToByte(dllCom.balanceData[4]);

            GetCoinMeshBalanceData(0x02);
            if (paperBLdata[1] == 0 || paperBLdata[2] == 0 || paperBLdata[3] == 0 || paperBLdata[4] == 0)
            {
                MessageBox.Show("Withdraw possible count is less.");
                return;
            }
            bool bret = dllCom.SendReceiveWithdrawal(b1, b2, b3, bCnt);
            LogResult("SEND: ", dllCom.tempS);
            if (bret)
            {
                LogResult("RECV: ", dllCom.GetbalanceByteData());
                bret = false;
                bret = ReceivedDataAnalProc();
                if (bret)
                {
                    GetDetailWithdrawStatus();
                    if (!bWithdraw)
                    {
                        PermitAndProhCommand(0x01, 0x01);
                        GetDetailDepositStatus(0x01);
                    }
                }
            }
        }
        private void WithdrawChangeAmount(int amount)
        {
            int nWan = amount / 10000;
            int nChon = (amount - 10000 * nWan) / 1000;
            int nBak = (amount - 10000 * nWan - 1000 * nChon) / 100;
            int nSib = (amount - 10000 * nWan - 1000 * nChon - 100 * nBak) / 10;

            byte b1 = GetByteTwoInteger(nSib, 0);
            byte b2 = GetByteTwoInteger(0, nBak);
            byte b3 = GetByteTwoInteger(0, nWan);
            byte bCnt = Convert.ToByte(nChon);

            GetCoinMeshBalanceData(0x02);
            if (paperBLdata[1] == 0 || paperBLdata[2] == 0 || paperBLdata[3] == 0 || paperBLdata[4] == 0)
            {
                MessageBox.Show("Withdraw possible count is less.");
                return;
            }

            bool bret = dllCom.SendReceiveWithdrawal(b1, b2, b3, bCnt);
            LogResult("SEND: ", dllCom.tempS);
            if (bret)
            {
                LogResult("RECV: ", dllCom.GetbalanceByteData());
                bret = false;
                bret = ReceivedDataAnalProc();
                if (bret)
                {
                    GetDetailWithdrawStatus();
                    if (!bWithdraw)
                    {
                        PermitAndProhCommand(0x01, 0x01);
                        GetDetailDepositStatus(0x01);
                    }
                }
            }
        }

        private bool WithdrawalDataAnal(byte b)
        {
            bool ret = false;
            if (b == 0x11) ret = true;
            else if (b == 0x55)
                ret = GetDetailWithdrawStatus();
            else if (b == 0xDD)
                MessageBox.Show("Please tap the button 発券 again.");
            else
                GetErrorStatus(0x07, 3);
            return ret;
        }

        private byte GetByteTwoInteger(int val1, int val2)
        {
            byte ret = 0;
            string s1 = Convert.ToString(val1, 2);
            string s2 = Convert.ToString(val2, 2);
            int[] bits1 = s1.PadLeft(4, '0') // Add 0's from left
                         .Select(c => int.Parse(c.ToString())) // convert each char to int
                         .ToArray(); // Convert IEnumerable from select to Array
            int[] bits2 = s2.PadLeft(4, '0') // Add 0's from left
                         .Select(c => int.Parse(c.ToString())) // convert each char to int
                         .ToArray(); // Convert IEnumerable from select to Array
            int[] newbit = new int[8];
            Array.Copy(bits1, newbit, 4);
            Array.Copy(bits2, 0, newbit, 4, 4);

            string s = "";
            for (int i = 0; i < 8; i++)
                s += newbit[i].ToString();
            byte[] bytes = new byte[1];
            bytes[0] = Convert.ToByte(s, 2);
            ret = bytes[0];
            return ret;
        }

        private void LogResult(string str1, string str2)
        {
            strLog += Environment.NewLine;
            strLog += str1 + str2;
            SaveLogData();
        }

        private void LogErrorData(int nCase)
        {
            string strError = "";
            if (nCase == 0)
            {
                strError = dllCom.GetErrorMessage();
                LogResult("Init Reset Error: ", strError);
            }
            if (nCase == 1)
            {
                strError = dllCom.GetErrorMessage();
                LogResult("Deposit Permission and Prohibition Error: ", strError);
            }
            if (nCase == 2)
            {
                strError = dllCom.GetErrorMessage();
                LogResult("Withdrawal Error: ", strError);
            }
            if (nCase == 3)
            {
                strError = dllCom.GetErrorMessage();
                LogResult("Balance state: ", strError);
            }
        }

        private void GetPaperBalanceData(byte b)
        {
            for (int i = 0; i < 5; i++)
                paperBLdata[i] = 0;
            LogResult("SEND : ", "04-FB-01-01-00");
            bool bret = dllCom.SendReceiveBalance(b);
            if (bret)
            {
                LogResult("RECV : ", dllCom.GetbalanceByteData());
                byte[] recv = dllCom.ptReceiveData;
                if (recv[0] == 0x04)
                {
                    if (recv[3] == 0x00)
                        for (int i = 0; i < 5; i++)
                            paperBLdata[i] = dllCom.HtoD(recv[i + 4]);
                    else if (recv[3] == 0xDD)
                        GetPaperBalanceData(b);
                    else if (recv[3] == 0xEE)
                    {
                        LogErrorData(3);
                        dllCom.GetErrorState(0x02);
                        LogResult(dllCom.GetBDErrorAnalResult(), "");
                    }
                }

            }
            else
                MessageBox.Show(dllCom.GetErrorMessage());
        }

        private void GetCoinMeshBalanceData(byte b)
        {
            for (int i = 0; i < 5; i++)
                paperBLdata[i] = 0;
            LogResult("SEND : ", "04-FB-01-02-03");
            bool bret = dllCom.SendReceiveBalance(b);
            if (bret)
            {
                LogResult("RECV : ", dllCom.GetbalanceByteData());
                byte[] recv = dllCom.ptReceiveData;
                if (recv[0] == 0x04)
                {
                    if (recv[3] == 0x00)
                        for (int i = 0; i < 5; i++)
                            paperBLdata[i] = dllCom.HtoD(recv[i + 4]);
                    else if (recv[3] == 0xDD)
                        GetCoinMeshBalanceData(b);
                    else if (recv[3] == 0xEE)
                    {
                        LogErrorData(3);
                        dllCom.GetErrorState(0x04);
                        LogResult(dllCom.GetBDErrorAnalResult(), "");
                    }
                }

            }
            else
                MessageBox.Show(dllCom.GetErrorMessage());
        }

        private void GetErrorStatus(byte b, int obj)
        {
            LogErrorData(0);
            dllCom.GetErrorState(b);
            LogResult(dllCom.GetBVErrorAnalResult(), "");
            LogResult(dllCom.GetBDErrorAnalResult(), "");
            LogResult(dllCom.GetCMErrorAnalResult(), "");
            if(obj == 0)
            {
                string errorMsg = constants.bankNoteErrorMsg;
            }
            if(obj == 1)
            {
                string errorMsg = constants.bankNoteErrorMsg;
            }
            if (obj == 2)
            {
                string errorMsg = constants.bankNoteDepositeErrorMsg + "\n購入金額 = " + orderTotalPrice.ToString() + "円\n入金金額 = " + depositAmount.ToString() + "円\n釣銭金額 = 0円 \n払出済み = 0円";
            }
            if (obj == 3)
            {
                int restPrice = depositAmount - orderTotalPrice;
                string errorMsg = constants.bankNoteWithdrawErrorMsg + "\n購入金額 = " + orderTotalPrice.ToString() + "円\n入金金額 = " + depositAmount.ToString() + "円\n釣銭金額 = " + restPrice.ToString() + "円 \n払出済み = " + withdrawAmount + "円";
            }
            messageDialog.ShowErrorMessage(constants.systemErrorMsg, constants.systemSubErrorMsg);

            //MessageBox.Show("Device Error. Please check error log data.");
        }

        private bool ReceivedDataAnalProc()
        {
            bool ret = false;
            recvData = dllCom.ptReceiveData;
            byte btManage = recvData[0];
            switch (btManage)
            {
                case 0x00:
                    ret = InitDataAnal(recvData[3]);
                    break;
                case 0x01:
                    ret = PermitAndProhDataAnal(recvData[3], recvData[4]);
                    break;
                case 0x02:
                    ret = WithdrawalDataAnal(recvData[3]);
                    break;
                case 0x03:
                    break;
                case 0x04:
                    break;
                case 0x05:
                    break;
            }
            return ret;
        }

    }
}

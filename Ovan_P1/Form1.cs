using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace Ovan_P1
{
    public partial class Form1 : Form
    {
        MainMenu mainMenu = new MainMenu();
        Constant constants = new Constant();
        CreatePanel createPanel = new CreatePanel();

        public Form mainFormGlobal = null;
        public Panel mainPanelGlobal = null;
        public Panel mainPanelGlobal_2 = null;
        public Panel topPanelGlobal = null;
        public Panel bottomPanelGlobal = null;

        SQLiteConnection sqlite_conn;

        bool processState = false;
        bool processDateState = false;
        public bool processStartState = false;
        string storeEndTime = "00:00";
        string startTime = "00:00";
        bool dbState = false;
        bool flagState = false;
        int messageCounter = 0;
      

        DateTime sumDayTime1 = new DateTime(int.Parse(DateTime.Now.ToString("yyyy")), int.Parse(DateTime.Now.ToString("MM")), int.Parse(DateTime.Now.ToString("dd")), 00, 00, 00);
        DateTime sumDayTime2 = new DateTime(int.Parse(DateTime.Now.ToString("yyyy")), int.Parse(DateTime.Now.ToString("MM")), int.Parse(DateTime.Now.ToString("dd")), 23, 59, 59);
        DateTime openDayTime = new DateTime(int.Parse(DateTime.Now.ToString("yyyy")), int.Parse(DateTime.Now.ToString("MM")), int.Parse(DateTime.Now.ToString("dd")), 00, 00, 00);
        string sumDate = DateTime.Now.ToString("yyyy-MM-dd");

        int height = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height;
        int width = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width;


        public Form1()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {

            Panel topPanel = createPanel.CreateMainPanel(this, 0, 30, width, 30, BorderStyle.None, Color.FromArgb(255, 234, 158, 65));
            Panel bottomPanel = createPanel.CreateMainPanel(this, 0, height - 60, width, 30, BorderStyle.None, Color.FromArgb(255, 234, 158, 65));

            topPanelGlobal = topPanel;
            bottomPanelGlobal = bottomPanel;

            Panel mainPanel = createPanel.CreateMainPanel(this, width / 10, height / 9, width * 8 / 10, height * 7 / 9 - 30, BorderStyle.None, Color.FromArgb(255, 249, 246, 224));
            mainPanelGlobal = mainPanel;
            Panel mainPanel_2 = createPanel.CreateMainPanel(this, 0, 0, width, height, BorderStyle.None, Color.White);
            mainPanelGlobal_2 = mainPanel_2;
            mainPanel_2.Hide();

            sqlite_conn = CreateConnection(constants.dbName);
            this.BackColor = Color.White;
            mainMenu.CreateMainMenuScreen(this, mainPanel);
            ProcessDateState();
            Thread oThread = new Thread(ClosingProcessWork);
            //oThread.SetApartmentState(ApartmentState.STA);
            oThread.Start();
            oThread.IsBackground = true;

        }

        private void DBChecking()
        {
            sqlite_conn = CreateConnection(constants.dbName);
            if (sqlite_conn.State == ConnectionState.Closed)
            {
                sqlite_conn.Open();
            }
            SQLiteCommand sqlite_cmd;
            SQLiteDataReader sqlite_datareader;
            DateTime now = DateTime.Now;

            string week = now.ToString("ddd");
            string currentTime = now.ToString("HH:mm");

            try
            {
                string storeEndqurey = "SELECT * FROM " + constants.tbNames[6];
                sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = storeEndqurey;
                sqlite_datareader = sqlite_cmd.ExecuteReader();
                while (sqlite_datareader.Read())
                {
                    if (!sqlite_datareader.IsDBNull(0))
                    {
                        dbState = true;
                    }
                }

            }
            catch
            {
                if (messageCounter == 0)
                {
                    MessageBox.Show("DB_Error2\nDB Checking Error in Form1");
                    Console.WriteLine("db_Thread2");
                    dbState = false;
                    messageCounter++;
                }
            }

        }

        private void ClosingProcessWork()
        {
            while (!flagState)
            {
                DBChecking();
                if (dbState)
                {
                    if (processStartState)
                    {
                        Thread.Sleep(30000);
                    }
                    else
                    {
                        Thread.Sleep(60000);
                    }
                    Console.WriteLine("Hello Threads!");
                    bool processDateState = ProcessDateState();
                    bool processState = false;
                    if (processDateState)
                    {
                        processState = ProcessState();
                    }
                    if (processState && processDateState)
                    {
                        this.ClosingProcessRun();
                        Console.WriteLine("Hello Thread!");
                    }
                }
                else
                {
                    Thread.Sleep(3000);
                    if(messageCounter == 0)
                    {
                        MessageBox.Show("DB ERROR\nDB Error in Thread proccessing");
                        Console.WriteLine("messageDialog");
                        messageCounter += 1;
                    }
                }


            }

        }

        private bool ProcessDateState()
        {
            DateTime now = DateTime.Now;
            if (sqlite_conn.State == ConnectionState.Closed)
            {
                sqlite_conn.Open();
            }
            SQLiteCommand sqlite_cmd;
            SQLiteDataReader sqlite_datareader;

            string week = now.ToString("ddd");
            string currentTime = now.ToString("HH:mm");
            try
            {
                string storeEndqurey = "SELECT * FROM " + constants.tbNames[6]; //store end time checking
                sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = storeEndqurey;
                sqlite_datareader = sqlite_cmd.ExecuteReader();
                while (sqlite_datareader.Read())
                {
                    if (!sqlite_datareader.IsDBNull(0))
                    {
                        if (week == "Sat")
                        {
                            storeEndTime = (sqlite_datareader.GetString(6)).Split('/')[1];
                            startTime = (sqlite_datareader.GetString(4).Split('/')[0].Split('-')[0]);
                        }
                        else if (week == "Sun")
                        {
                            storeEndTime = (sqlite_datareader.GetString(6)).Split('/')[2];
                            startTime = (sqlite_datareader.GetString(5).Split('/')[0].Split('-')[0]);
                        }
                        else
                        {
                            storeEndTime = (sqlite_datareader.GetString(6)).Split('/')[0];
                            startTime = (sqlite_datareader.GetString(3).Split('/')[0].Split('-')[0]);
                        }

                    }
                }
                sumDayTime1 = constants.sumDayTimeStart(storeEndTime);
                sumDayTime2 = constants.sumDayTimeEnd(storeEndTime);
                openDayTime = constants.openDateTime(startTime, storeEndTime);

                sumDate = constants.sumDate(storeEndTime);
                if (DateTime.Compare(sumDayTime1, now) <= 0 && DateTime.Compare(now, openDayTime) <= 0)
                {
                    sumDate = Convert.ToDateTime(sumDate).AddDays(-1).ToString("yyyy-MM-dd");
                    processDateState = true;
                }
                else
                {
                    processDateState = false;
                }

                if (DateTime.Compare(now, openDayTime) >= 0)
                {
                    processStartState = true;
                }
                else
                {
                    processStartState = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                processDateState = false;
                processStartState = false;
            }


            sqlite_conn.Close();
            return processDateState;

        }
        private bool ProcessState()
        {
            DateTime now = DateTime.Now;
            if (sqlite_conn.State == ConnectionState.Closed)
            {
                sqlite_conn.Open();
            }
            SQLiteCommand sqlite_cmd;
            SQLiteDataReader sqlite_datareader;

            string week = now.ToString("ddd");
            string currentTime = now.ToString("HH:mm");
            try
            {
                string storeEndqurey = "SELECT * FROM " + constants.tbNames[6]; //store end time checking
                sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = storeEndqurey;
                sqlite_datareader = sqlite_cmd.ExecuteReader();
                while (sqlite_datareader.Read())
                {
                    if (!sqlite_datareader.IsDBNull(0))
                    {
                        if (week == "Sat")
                        {
                            storeEndTime = (sqlite_datareader.GetString(6)).Split('/')[1];
                            startTime = (sqlite_datareader.GetString(4).Split('/')[0].Split('-')[0]);
                        }
                        else if (week == "Sun")
                        {
                            storeEndTime = (sqlite_datareader.GetString(6)).Split('/')[2];
                            startTime = (sqlite_datareader.GetString(5).Split('/')[0].Split('-')[0]);
                        }
                        else
                        {
                            storeEndTime = (sqlite_datareader.GetString(6)).Split('/')[0];
                            startTime = (sqlite_datareader.GetString(3).Split('/')[0].Split('-')[0]);
                        }

                    }
                }
                sumDate = constants.sumDate(storeEndTime);

                sumDate = Convert.ToDateTime(sumDate).AddDays(-1).ToString("yyyy-MM-dd");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            int processRowNum = 0;
            try
            {
                string processqurey = "SELECT * FROM " + constants.tbNames[7] + " WHERE sumDate=@sumDate"; //processing result checking
                sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = processqurey;
                sqlite_cmd.Parameters.AddWithValue("@sumDate", sumDate);
                processRowNum = Convert.ToInt32(sqlite_cmd.ExecuteScalar());
            }
            catch(Exception ex)
            {
                MessageBox.Show("DaySaleTb Checking Error\n" + ex);
                Console.WriteLine("DaySaleTb Checking Error" + ex);
            }

            if (processRowNum > 0)
            {
                processState = false;
            }
            else
            {
                processState = true;
            }

            sqlite_conn.Close();
            return processState;

        }

        private void ClosingProcessRun()
        {
            DateTime now = DateTime.Now;
            if (sqlite_conn.State == ConnectionState.Closed)
            {
                sqlite_conn.Open();
            }
            SQLiteCommand sqlite_cmd;
            SQLiteDataReader sqlite_datareader;

            string week = now.ToString("ddd");
            string currentTime = now.ToString("HH:mm");

            string storeEndqurey = "SELECT * FROM " + constants.tbNames[6]; // store end time checking
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = storeEndqurey;
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                if (!sqlite_datareader.IsDBNull(0))
                {
                    if (week == "Sat")
                    {
                        storeEndTime = (sqlite_datareader.GetString(6)).Split('/')[1];
                    }
                    else if (week == "Sun")
                    {
                        storeEndTime = (sqlite_datareader.GetString(6)).Split('/')[2];
                    }
                    else
                    {
                        storeEndTime = (sqlite_datareader.GetString(6)).Split('/')[0];
                    }

                }
            }

            sumDayTime1 = constants.sumDayTimeStart(storeEndTime).AddDays(-1);
            sumDayTime2 = constants.sumDayTimeEnd(storeEndTime).AddDays(-1);

            sumDate = constants.sumDate(storeEndTime);
            sumDate = Convert.ToDateTime(sumDate).AddDays(-1).ToString("yyyy-MM-dd");


            string saleTBquery = "SELECT prdName, prdPrice, sum(prdAmount), prdRealID FROM " + constants.tbNames[3] + " WHERE saleDate>=@sumDayTime1 AND saleDate<=@sumDayTime2 GROUP BY prdRealID";
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = saleTBquery;
            sqlite_cmd.Parameters.AddWithValue("@sumDayTime1", sumDayTime1);
            sqlite_cmd.Parameters.AddWithValue("@sumDayTime2", sumDayTime2);
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            int j = 0;
            while (sqlite_datareader.Read())
            {
                if (!sqlite_datareader.IsDBNull(0))
                {
                    string prdName = sqlite_datareader.GetString(0);
                    int prdPrice = sqlite_datareader.GetInt32(1);
                    int prdAmount = sqlite_datareader.GetInt32(2);
                    int prdTotalPrice = prdPrice * prdAmount;
                    int prdID = sqlite_datareader.GetInt32(3);
                    SQLiteCommand sqlite_cmds;
                    string sumQuery = "INSERT INTO " + constants.tbNames[7] + " (prdID, prdName, prdPrice, prdAmount, prdTotalPrice, sumDate) VALUES (@prdID, @prdName, @prdPrice, @prdAmount, @prdTotalPrice, @sumDate)";
                    sqlite_cmds = sqlite_conn.CreateCommand();
                    sqlite_cmds.CommandText = sumQuery;
                    sqlite_cmds.Parameters.AddWithValue("@prdID", prdID);
                    sqlite_cmds.Parameters.AddWithValue("@prdName", prdName);
                    sqlite_cmds.Parameters.AddWithValue("@prdPrice", prdPrice);
                    sqlite_cmds.Parameters.AddWithValue("@prdAmount", prdAmount);
                    sqlite_cmds.Parameters.AddWithValue("@prdTotalPrice", prdTotalPrice);
                    sqlite_cmds.Parameters.AddWithValue("@sumDate", sumDate);
                    sqlite_cmds.ExecuteNonQuery();
                    SQLiteCommand sqlite_cmdss;
                    string sumQuerys = "UPDATE " + constants.tbNames[3] + " SET sumFlag='true', sumDate=@sumDate WHERE saleDate>=@sumDayTime1 AND saleDate<=@sumDayTime2 and prdRealID=@realPrdID";
                    sqlite_cmdss = sqlite_conn.CreateCommand();
                    sqlite_cmdss.CommandText = sumQuerys;
                    sqlite_cmdss.Parameters.AddWithValue("@sumDate", sumDate);
                    sqlite_cmdss.Parameters.AddWithValue("@sumDayTime1", sumDayTime1);
                    sqlite_cmdss.Parameters.AddWithValue("@sumDayTime2", sumDayTime2);
                    sqlite_cmdss.Parameters.AddWithValue("@realPrdID", prdID);
                    sqlite_cmdss.ExecuteNonQuery();
                    j++;
                }

            }
            sqlite_conn.Close();
            string currentDir = Directory.GetCurrentDirectory();
            DBClass dbClass = new DBClass();
            dbClass.DBCopy(sqlite_conn, Path.Combine(currentDir, "SettingValue.db"), "SettingValue", constants.tbNames);
            processState = false;

        }

        static SQLiteConnection CreateConnection(string dbName)
        {

            SQLiteConnection sqlite_conn;
            // Create a new database connection:
            sqlite_conn = new SQLiteConnection("Data Source=" + dbName + ".db; Version = 3; New = True; Compress = True; ");
            // Open the connection:
            try
            {
                sqlite_conn.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return sqlite_conn;
        }

    }
}

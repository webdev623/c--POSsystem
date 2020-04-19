﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ovan_P1
{
    public partial class ClosingProcess : Form
    {
        int height = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height;
        int width = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width;
        Form1 mainFormGlobal = null;
        Form mainProgressDialog = null;
        Panel mainPanelGlobal = null;
        Panel headerPanelG = null;
        Button mainButton = null;
        //private Button buttonGlobal = null;
        private FlowLayoutPanel[] menuFlowLayoutPanelGlobal = new FlowLayoutPanel[4];
        Constant constants = new Constant();

        CreatePanel createPanel = new CreatePanel();
        CreateLabel createLabel = new CreateLabel();
        CustomButton customButton = new CustomButton();
        Button[] menuButtonGlobal = new Button[7];
        Label progressAlertLabel = null;
        ProgressBar progressBar1 = null;
        BackgroundWorker backgroundWorker = null;
        Panel progressDialogPanel = null;

        DetailView detailView = new DetailView();
        SQLiteConnection sqlite_conn;

        bool manualProcessState = false;

        int countNum = 0;
        int rowNumDaySale = 0;
        string storeEndTime = "00:00";
        DateTime now = DateTime.Now;
        DateTime sumDayTime1 = new DateTime(int.Parse(DateTime.Now.ToString("yyyy")), int.Parse(DateTime.Now.ToString("MM")), int.Parse(DateTime.Now.ToString("dd")), 00, 00, 00);
        DateTime sumDayTime2 = new DateTime(int.Parse(DateTime.Now.ToString("yyyy")), int.Parse(DateTime.Now.ToString("MM")), int.Parse(DateTime.Now.ToString("dd")), 23, 59, 59);
        string sumDate = DateTime.Now.ToString("yyyy-MM-dd");

        public ClosingProcess(Form1 mainForm, Panel mainPanel)
        {
            mainForm.Width = width;
            mainForm.Height = height;
            mainFormGlobal = mainForm;
            mainPanelGlobal = mainPanel;
            sqlite_conn = CreateConnection(constants.dbName);
            if (sqlite_conn.State == ConnectionState.Closed)
            {
                sqlite_conn.Open();
            }
            SQLiteCommand sqlite_cmd;
            SQLiteDataReader sqlite_datareader;

            string week = now.ToString("ddd");
            string currentTime = now.ToString("HH:mm");

            string storeEndqurey = "SELECT * FROM " + constants.tbNames[6];
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

            sumDayTime1 = constants.sumDayTimeStart(storeEndTime);
            sumDayTime2 = constants.sumDayTimeEnd(storeEndTime);

            sumDate = constants.sumDate(storeEndTime);

            try
            {
                SQLiteCommand sqlite_cmds = sqlite_conn.CreateCommand();
                string sumIdentify = "SELECT COUNT(id) FROM " + constants.tbNames[7] + " WHERE sumDate=@sumDate";
                sqlite_cmds.CommandText = sumIdentify;
                sqlite_cmds.Parameters.AddWithValue("@sumDate", sumDate);
                rowNumDaySale = Convert.ToInt32(sqlite_cmds.ExecuteScalar());
                if(rowNumDaySale > 0)
                {
                    manualProcessState = true;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Database Error" + e);
            }

            sqlite_conn.Close();

            Panel headerPanel = createPanel.CreateMainPanel(mainForm, 0, 0, width, height / 5, BorderStyle.None, Color.Transparent);
            headerPanelG = headerPanel;
            Label headerLabel = createLabel.CreateLabelsInPanel(headerPanel, "headerLabel", constants.closingProcessTitle, 0, 0, headerPanel.Width / 3 - 10, headerPanel.Height, Color.Transparent, Color.Red, 28, false, ContentAlignment.MiddleLeft);
            headerLabel.Padding = new Padding(30, 0, 0, 20);


            Panel bodyPanel = createPanel.CreateMainPanel(mainForm, 0, headerPanel.Bottom, width, height * 4 / 5, BorderStyle.None, Color.Transparent);
            for (int i = 0; i < 4; i++)
            {
                FlowLayoutPanel menuFlowLayoutPanel = createPanel.CreateFlowLayoutPanel(bodyPanel, width / 5, bodyPanel.Height / 9 + (bodyPanel.Height / 6) * i, width * 3 / 5, bodyPanel.Height / 8, Color.Transparent, new Padding(0, 0, 0, 0));
                menuFlowLayoutPanel.Margin = new Padding(30);

                menuFlowLayoutPanelGlobal[i] = menuFlowLayoutPanel;

                Label menuLabel = createLabel.CreateLabels(menuFlowLayoutPanel, "processLabel_" + i, constants.closingProcessLabel[i], 100, menuFlowLayoutPanel.Height * i, menuFlowLayoutPanel.Width / 3 - 10, menuFlowLayoutPanel.Height, Color.Transparent, Color.Red, 18, false, ContentAlignment.MiddleLeft);

                Image btnImage = Image.FromFile(constants.closingProcessButtonImage[i]);

                Color btnBackColor1 = Color.FromArgb(255, 229, 229, 229);
                Color btnBackColor2 = Color.FromArgb(255, 229, 229, 229);
                Color btnForeColor = Color.Black;
                bool btnEnable = false;

                if (manualProcessState)
                {
                    btnBackColor1 = Color.FromArgb(255, 0, 176, 80);
                    btnBackColor2 = Color.FromArgb(255, 0, 160, 230);
                    btnForeColor = Color.White;
                    btnEnable = true;
                }
                if (i == 0)
                {
                    if (manualProcessState)
                    {
                        Button menuButton_1 = customButton.CreateButton(constants.closingProcessButton[i][0], "processButton_" + i + "_1", menuLabel.Right, menuFlowLayoutPanel.Height * i, menuFlowLayoutPanel.Width / 3 - 50, menuFlowLayoutPanel.Height, Color.FromArgb(255, 229, 229, 229), Color.Transparent, 0);
                        menuFlowLayoutPanel.Controls.Add(menuButton_1);

                        menuButton_1.Margin = new Padding(0, 0, 30, 0);
                        menuButton_1.Enabled = false;

                        Button menuButton_2 = customButton.CreateButton(constants.closingProcessButton[i][1], "processButton_" + i + "_2", menuButton_1.Right + 30, menuFlowLayoutPanel.Height * i, menuFlowLayoutPanel.Width / 3 - 50, menuFlowLayoutPanel.Height, Color.FromArgb(255, 0, 160, 230), Color.Transparent, 0, 1, 12, FontStyle.Regular, Color.White);
                        menuFlowLayoutPanel.Controls.Add(menuButton_2);

                        menuButton_2.Margin = new Padding(30, 0, 0, 0);
                        menuButton_2.Enabled = true;

                        menuButtonGlobal[i * 2] = menuButton_1;
                        menuButtonGlobal[i * 2 + 1] = menuButton_2;
                        mainButton = menuButton_1;
                        menuButton_1.Click += new EventHandler(this.manualProcess);
                        menuButton_2.Click += new EventHandler(this.refreshPages);

                    }
                    else
                    {
                        Button menuButton_1 = customButton.CreateButton(constants.closingProcessButton[i][0], "processButton_" + i + "_1", menuLabel.Right, menuFlowLayoutPanel.Height * i, menuFlowLayoutPanel.Width / 3 - 50, menuFlowLayoutPanel.Height, Color.FromArgb(255, 0, 160, 230), Color.Transparent, 0, 1, 12, FontStyle.Regular, Color.White);
                        menuFlowLayoutPanel.Controls.Add(menuButton_1);

                        menuButton_1.Margin = new Padding(0, 0, 30, 0);


                        Button menuButton_2 = customButton.CreateButton(constants.closingProcessButton[i][1], "processButton_" + i + "_2", menuButton_1.Right + 30, menuFlowLayoutPanel.Height * i, menuFlowLayoutPanel.Width / 3 - 50, menuFlowLayoutPanel.Height, Color.FromArgb(255, 229, 229, 229), Color.Transparent, 0, 1);
                        menuFlowLayoutPanel.Controls.Add(menuButton_2);

                        menuButton_2.Margin = new Padding(30, 0, 0, 0);
                        menuButton_2.Enabled = false;

                        menuButtonGlobal[i * 2] = menuButton_1;
                        menuButtonGlobal[i * 2 + 1] = menuButton_2;

                        mainButton = menuButton_1;
                        menuButton_1.Click += new EventHandler(this.manualProcess);
                        menuButton_2.Click += new EventHandler(this.refreshPages);

                    }
                }

                else if (i < 3)
                {
                    Button menuButton_1 = customButton.CreateButton(constants.closingProcessButton[i][0], "processButton_" + i + "_1", menuLabel.Right, menuFlowLayoutPanel.Height * i, menuFlowLayoutPanel.Width / 3 - 50, menuFlowLayoutPanel.Height, btnBackColor1, Color.Transparent, 0, 1, 12, FontStyle.Regular, btnForeColor);
                    menuFlowLayoutPanel.Controls.Add(menuButton_1);

                    menuButton_1.Margin = new Padding(0, 0, 30, 0);
                    menuButton_1.Enabled = btnEnable;
                    menuButton_1.Click += new EventHandler(detailView.DetailViewIndicator);

                    Button menuButton_2 = customButton.CreateButton(constants.closingProcessButton[i][1], "processButton_" + i + "_2", menuButton_1.Right + 90, menuFlowLayoutPanel.Height * i, menuFlowLayoutPanel.Width / 3 - 50, menuFlowLayoutPanel.Height, btnBackColor2, Color.Transparent, 0, 1, 12, FontStyle.Regular, btnForeColor);
                    menuFlowLayoutPanel.Controls.Add(menuButton_2);

                    menuButtonGlobal[i * 2] = menuButton_1;
                    menuButtonGlobal[i * 2 + 1] = menuButton_2;

                    menuButton_2.Margin = new Padding(30, 0, 0, 0);
                    menuButton_2.Enabled = btnEnable;

                    menuButton_2.Click += new EventHandler(detailView.DetailViewIndicator);
                }
                else
                {
                    Button menuButton_1 = customButton.CreateButton(constants.closingProcessButton[i][0], "processButton_" + i + "_1", menuLabel.Right, menuFlowLayoutPanel.Height * i, menuFlowLayoutPanel.Width / 3 - 50, menuFlowLayoutPanel.Height, Color.FromArgb(255, 0, 176, 80), Color.Transparent, 0, 1, 12, FontStyle.Regular, Color.White);
                    menuFlowLayoutPanel.Controls.Add(menuButton_1);

                    menuButtonGlobal[i * 2] = menuButton_1;

                    menuButton_1.Margin = new Padding(0, 0, 30, 0);

                    menuButton_1.Click += new EventHandler(detailView.DetailViewIndicator);
                }
            }

            Button backButton = customButton.CreateButton(constants.backText, "backButton", bodyPanel.Width - 150, bodyPanel.Height - 100, 100, 50, Color.FromArgb(255, 0, 112, 192), Color.Transparent, 0, 10, 14, FontStyle.Bold, Color.White);
            bodyPanel.Controls.Add(backButton);
            backButton.Click += new EventHandler(this.BackShow);

            InitializeComponent();
            backgroundWorker = new BackgroundWorker();
            progressAlertLabel = new Label();
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
        }

        private void refreshPages(object sender, EventArgs e)
        {
            if (sqlite_conn.State == ConnectionState.Closed)
            {
                sqlite_conn.Open();
            }
            SQLiteCommand sqlite_cmd;
            SQLiteDataReader sqlite_datareader;
            string sumQuery = "SELECT * FROM " + constants.tbNames[7] + " WHERE sumDate=@sumDate";
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = sumQuery;
            sqlite_cmd.Parameters.AddWithValue("@sumDate", sumDate);
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                if (!sqlite_datareader.IsDBNull(0))
                {
                    int prdID = sqlite_datareader.GetInt32(1);
                    SQLiteCommand sqlite_cmdss;
                    string sumQueryss = "UPDATE " + constants.tbNames[3] + " SET sumFlag='false', sumDate='' WHERE saleDate>=@sumDayTime1 AND saleDate<=@sumDayTime2 and prdRealID=@realPrdID";
                    sqlite_cmdss = sqlite_conn.CreateCommand();
                    sqlite_cmdss.CommandText = sumQueryss;
                    sqlite_cmdss.Parameters.AddWithValue("@sumDayTime1", sumDayTime1);
                    sqlite_cmdss.Parameters.AddWithValue("@sumDayTime2", sumDayTime2);
                    sqlite_cmdss.Parameters.AddWithValue("@realPrdID", prdID);
                    sqlite_cmdss.ExecuteNonQuery();
                }
            }


            SQLiteCommand sqlite_cmds;
            string sumQuerys = "DELETE FROM " + constants.tbNames[7] + " WHERE sumDate=@sumDate";
            sqlite_cmds = sqlite_conn.CreateCommand();
            sqlite_cmds.CommandText = sumQuerys;
            sqlite_cmds.Parameters.AddWithValue("@sumDate", sumDate);
            sqlite_cmds.ExecuteNonQuery();
            sqlite_conn.Close();
            countNum = 0;
            manualProcessState = false;
            ButtonBackChange();
        }
        public void BackShow(object sender, EventArgs e)
        {
            mainFormGlobal.Controls.Clear();
            MaintaneceMenu frm = new MaintaneceMenu(mainFormGlobal, mainPanelGlobal);
            frm.TopLevel = false;
            mainFormGlobal.Controls.Add(frm);
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;
            Thread.Sleep(200);
            frm.Show();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if(countNum == 1)
            {
                var backgroundWorker = sender as BackgroundWorker;
                progressAlertLabel.Text = constants.sumProgressAlert;

                if (sqlite_conn.State == ConnectionState.Closed)
                {
                    sqlite_conn.Open();
                }
                SQLiteCommand sqlite_cmd;
                SQLiteDataReader sqlite_datareader;

                string week = now.ToString("ddd");
                string currentTime = now.ToString("HH:mm");

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
                        Thread.Sleep(50);
                        backgroundWorker.ReportProgress(j);
                        j++;
                    }

                }
                sqlite_conn.Close();
                if(j < 100)
                {
                    while(j <= 100)
                    {
                        Thread.Sleep(50);
                        backgroundWorker.ReportProgress(j * 1);
                        j++;
                    }
                }
                countNum++;
            }
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            mainButton.Enabled = false;
            mainProgressDialog.Close();
            ButtonBackChange();
            backgroundWorker.CancelAsync();
            countNum++;

        }


        private void manualProcess(object sender, EventArgs e)
        {
            bool manualProcessHandler = manualProcessState;
            manualProcessState = !manualProcessHandler;
            ProgressDialog();
        }
        private void ProgressDialog()
        {
            Form progressDialog = new Form();
            mainProgressDialog = progressDialog;
            progressDialog.Size = new Size(width / 3, height / 4);
            progressDialog.BackColor = Color.White;
            progressDialog.StartPosition = FormStartPosition.CenterParent;
            progressDialog.WindowState = FormWindowState.Normal;
            progressDialog.ControlBox = false;
            progressDialog.FormBorderStyle = FormBorderStyle.FixedDialog;

            progressDialogPanel = createPanel.CreateMainPanel(progressDialog, 0, 0, progressDialog.Width, progressDialog.Height, BorderStyle.None, Color.Transparent);

            //progressAlertLabel = progressAlert;

            progressBar1 = new ProgressBar();
            progressBar1.Location = new Point(progressDialogPanel.Width / 5, progressDialogPanel.Height / 2 - 20);
            progressBar1.Size = new Size(progressDialogPanel.Width * 3 / 5, 20);
            progressDialogPanel.Controls.Add(progressBar1);
            progressBar1.Maximum = 100;
            progressBar1.Step = 1;
            progressBar1.Value = 0;
            progressBar1.SendToBack();

            FlowLayoutPanel progressLabelPanel = createPanel.CreateFlowLayoutPanel(progressDialogPanel, 0, progressDialogPanel.Height / 2 + 10, progressDialogPanel.Width, progressDialogPanel.Height / 2 - 20, Color.White, new Padding(0));

            progressAlertLabel = createLabel.CreateLabels(progressLabelPanel, "progressAlert", constants.sumProgressAlert, 0, progressDialogPanel.Height / 2 + 30, progressDialogPanel.Width - 10, 50, Color.White, Color.Black, 18, false, ContentAlignment.MiddleCenter);

            if (backgroundWorker.IsBusy != true)
            {
                countNum++;
                Console.WriteLine(countNum);
                backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
                backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
                backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
                backgroundWorker.RunWorkerAsync();
            }


            progressDialog.ShowDialog();

        }

        private void ButtonBackChange()
        {
            if (manualProcessState)
            {
                menuButtonGlobal[0].BackColor = Color.FromArgb(255, 229, 229, 229);
                menuButtonGlobal[0].ForeColor = Color.Black;
                menuButtonGlobal[0].Enabled = false;
            }
            else
            {
                menuButtonGlobal[0].BackColor = Color.FromArgb(255, 0, 160, 230);
                menuButtonGlobal[0].ForeColor = Color.White;
                menuButtonGlobal[0].Enabled = true;
            }
            for(int i = 1; i < 6; i++)
            {
                if (manualProcessState)
                {
                    if(i % 2 == 0)
                    {
                        menuButtonGlobal[i].BackColor = Color.FromArgb(255, 0, 176, 80);
                    }
                    else
                    {
                        menuButtonGlobal[i].BackColor = Color.FromArgb(255, 0, 160, 230);
                    }
                    menuButtonGlobal[i].Enabled = true;
                    menuButtonGlobal[i].ForeColor = Color.White;
                }
                else
                {
                    menuButtonGlobal[i].Enabled = false;
                    menuButtonGlobal[i].BackColor = Color.FromArgb(255, 229, 229, 229); 
                    menuButtonGlobal[i].ForeColor = Color.Black;
                }
            }
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Ovan_P1
{
    public partial class DetailView : Form
    {
        int height = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height;
        int width = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width;
        Constant constants = new Constant();
        CreatePanel createPanel = new CreatePanel();
        CreateLabel createLabel = new CreateLabel();
        CustomButton createButton = new CustomButton();
        CreateCombobox createCombobox = new CreateCombobox();
        DropDownMenu dropDownMenu = new DropDownMenu();
        Form DialogFormGlobal = null;
        Panel dialogPanelGlobal = null;
        Panel tbodyPanelGlobal = null;
        Form cancelDialogFormGlobal = null;
        FlowLayoutPanel dialogFlowLayout = null;
        Label currentYearGlobal = null;
        Label nextYearGlobal = null;
        Label prevYearGlobal = null;
        Label currentMonthGlobal = null;
        Label nextMonthGlobal = null;
        Label prevMonthGlobal = null;
        Label currentDayGlobal = null;
        Label nextDayGlobal = null;
        Label prevDayGlobal = null;
        Label currentHourGlobal = null;
        Label nextHourGlobal = null;
        Label prevHourGlobal = null;
        Label currentMinuteGlobal = null;
        Label nextMinuteGlobal = null;
        Label prevMinuteGlobal = null;
        Label[] columnGlobal1 = null;
        Label[] columnGlobal2 = null;
        Label[] columnGlobal3 = null;
        Label[] columnGlobal4 = null;
        Label[] columnGlobal5 = null;
        int totalRowNum = 0;
        PaperSize paperSize = new PaperSize("papersize", 500, 800);//set the paper size
        PrintDocument printDocumentGlobal = null;
        PrintPreviewDialog printPreviewDialogGlobal = null;
        PrintDialog printDialogGlobal = null;
        string logTimeGlobal = "23";
        int totalNumber = 0;
        int itemperpage = 0;//this is for no of item per page 
        int groupIDGlobal = 0;
        int lineNum = 0;
        int flagInt = 0;
        int groupNumber = 0;
        int lineNums = 0;
        int groupNumbers = 0;
        int flagInts = 0;
        int itemperpages = 0;
        int soldPriceSum = 0;
        int soldAmountSum = 0;
        SQLiteConnection sqlite_conn;
        string storeEndTime = "00:00";
        DateTime now = DateTime.Now;
        string sumDate = DateTime.Now.ToString("yyyy-MM-dd");
        DateTime sumDayTime1 = new DateTime();
        DateTime sumDayTime2 = new DateTime();

        TimeSetting timeHandlerGlobal = null;

        int[] bottomPosition = null;
        public DetailView()
        {
            InitializeComponent();
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
                        storeEndTime = (sqlite_datareader.GetString(7)).Split('/')[1];
                    }
                    else if (week == "Sun")
                    {
                        storeEndTime = (sqlite_datareader.GetString(7)).Split('/')[2];
                    }
                    else
                    {
                        storeEndTime = (sqlite_datareader.GetString(7)).Split('/')[0];
                    }

                }
            }

            sumDayTime1 = constants.sumDayTimeStart(storeEndTime);
            sumDayTime2 = constants.sumDayTimeEnd(storeEndTime);

            sumDate = constants.sumDate(storeEndTime);

            sqlite_conn.Close();
        }
        public void initTimeSetting(TimeSetting timeHandler)
        {
            timeHandlerGlobal = timeHandler;
        }
        public void DetailViewIndicator(object sender, EventArgs e)
        {
            Button btnTemp = (Button)sender;
            if (btnTemp.Name == "processButton_1_1")
            {
                DailyReport();
            }
            else if (btnTemp.Name == "processButton_1_2")
            {
                totalNumber = constants.productBigName[1].Length;
                PrintDocument printDocument1 = new PrintDocument();
                PrintDialog printDialog1 = new PrintDialog();
                PrintPreviewDialog printPreviewDialog1 = new PrintPreviewDialog();
                printDialogGlobal = printDialog1;
                printDocumentGlobal = printDocument1;
                printPreviewDialogGlobal = printPreviewDialog1;
                paperSize = new PaperSize("papersize", constants.singleticketPrintPaperWidth, constants.singleticketPrintPaperHeight);

                printDocument1.PrintPage += new PrintPageEventHandler(DailyReportPrintPage);
                printDocument1.EndPrint += new PrintEventHandler(PrintEnd);
                printDialog1.Document = printDocument1;
                printDocument1.DefaultPageSettings.PaperSize = paperSize;
               // printDialog1.ShowDialog();
                if (printDialog1.ShowDialog() == DialogResult.OK)
                {
                    printDocument1.Print();
                }

            }
            else if (btnTemp.Name == "processButton_2_1")
            {
                ReceiptIssueReport();
            }
            else if (btnTemp.Name == "processButton_2_2")
            {
                PrintDocument printDocument1 = new PrintDocument();
                PrintDialog printDialog1 = new PrintDialog();
                PrintPreviewDialog printPreviewDialog1 = new PrintPreviewDialog();
                printDialogGlobal = printDialog1;
                printDocumentGlobal = printDocument1;
                printPreviewDialogGlobal = printPreviewDialog1;
                printDocument1.PrintPage += new PrintPageEventHandler(ReceiptIssueReportPrintPage);
                printDocument1.EndPrint += new PrintEventHandler(PrintEnd);
                printDialog1.Document = printDocument1;
                paperSize = new PaperSize("papersize", constants.singleticketPrintPaperWidth, constants.singleticketPrintPaperHeight);
                printDocument1.DefaultPageSettings.PaperSize = paperSize;
                // printDialog1.ShowDialog();
                if (printDialog1.ShowDialog() == DialogResult.OK)
                {
                    printDocument1.Print();
                }
            }
            else if (btnTemp.Name == "processButton_3_1")
            {
                LogReport();
            }
            else if(btnTemp.Name == "cancellationButton")
            {
                FalsePurchaseCancellation();
            }
        }

        public void PrintEnd(object sender, PrintEventArgs e)
        {
            lineNums = 0;
            itemperpages = 0;
            soldPriceSum = 0;
            soldAmountSum = 0;
        }
        public void DailyReport()
        {
            Form dialogForm = new Form();
            dialogForm.Size = new Size(width / 2, height * 8 / 9);
            dialogForm.BackColor = Color.White;
            dialogForm.StartPosition = FormStartPosition.CenterParent;
            dialogForm.WindowState = FormWindowState.Normal;
            //  dialogForm.ControlBox = false;
            dialogForm.MinimizeBox = false;
            dialogForm.MaximizeBox = false;
            dialogForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            dialogForm.Padding = new Padding(0, 0, 0, 20);

            DialogFormGlobal = dialogForm;

            Panel dialogPanel = createPanel.CreateMainPanel(dialogForm, 0, 0, dialogForm.Width, dialogForm.Height - 40, BorderStyle.None, Color.Transparent);
            dialogPanel.Margin = new Padding(0, 0, 0, 20);
            dialogPanel.HorizontalScroll.Maximum = 0;
            dialogPanel.AutoScroll = false;
            dialogPanel.VerticalScroll.Visible = false;
            dialogPanel.AutoScroll = true;

            DateTime now = DateTime.Now;

            FlowLayoutPanel dialogTitlePanel = createPanel.CreateFlowLayoutPanel(dialogPanel, 0, 10, dialogPanel.Width, 50, Color.Transparent, new Padding(0));
            Label dialogTitle1 = createLabel.CreateLabelsInPanel(dialogTitlePanel, "dialogTitle1", constants.dailyReportTitle, 0, 0, dialogTitlePanel.Width / 2 - 30, 60, Color.Transparent, Color.Black, 22);
            Label dialogTitle2 = createLabel.CreateLabelsInPanel(dialogTitlePanel, "dialogTitle2", sumDate, dialogTitlePanel.Width / 2, 0, dialogTitlePanel.Width / 2 - 30, 60, Color.Transparent, Color.Black, 22);

            int soldPriceSum = 0;
            int soldAmountSum = 0;
            int k = 0;

            if (sqlite_conn.State == ConnectionState.Closed)
            {
                sqlite_conn.Open();
            }
            SQLiteCommand sqlite_cmd;
            SQLiteDataReader sqlite_datareader;

            string week = now.ToString("ddd");
            string currentTime = now.ToString("HH:mm");

            sqlite_cmd = sqlite_conn.CreateCommand();
            string daySumqurey = "SELECT * FROM " + constants.tbNames[7] + " WHERE sumDate=@sumDate";
            sqlite_cmd.CommandText = daySumqurey;
            sqlite_cmd.Parameters.AddWithValue("@sumDate", sumDate);
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                if (!sqlite_datareader.IsDBNull(0))
                {
                    string prdName = sqlite_datareader.GetString(2);
                    int prdPrice = sqlite_datareader.GetInt32(3);
                    int prdAmount = sqlite_datareader.GetInt32(4);
                    int prdTotalPrice = sqlite_datareader.GetInt32(5);
                    soldPriceSum += prdTotalPrice;
                    soldAmountSum += prdAmount;
                    FlowLayoutPanel productTableBody = createPanel.CreateFlowLayoutPanel(dialogPanel, 0, 70 + 41 * k, dialogPanel.Width, 40, Color.Transparent, new Padding(0));
                    Label prodNameBody1 = createLabel.CreateLabelsInPanel(productTableBody, "prodBody_" + k + "_1", prdName, 0, 0, productTableBody.Width / 4 - 10, productTableBody.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);
                    Label prodNameBody2 = createLabel.CreateLabelsInPanel(productTableBody, "prodBody_" + k + "_2", prdAmount.ToString() + constants.amountUnit, productTableBody.Width / 4, 0, productTableBody.Width / 4 - 10, productTableBody.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);
                    Label prodNameBody3 = createLabel.CreateLabelsInPanel(productTableBody, "prodBody_" + k + "_3", prdPrice.ToString() + constants.unit, productTableBody.Width / 2, 0, productTableBody.Width / 4 - 10, productTableBody.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);
                    Label prodNameBody4 = createLabel.CreateLabelsInPanel(productTableBody, "prodBody_" + k + "_4", prdTotalPrice.ToString() + constants.unit, productTableBody.Width * 3 / 4, 0, productTableBody.Width / 4 - 10, productTableBody.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);
                    dialogFlowLayout = productTableBody;
                    k++;

                }
            }

            FlowLayoutPanel dialogSumPanel = createPanel.CreateFlowLayoutPanel(dialogPanel, 0, k * 41 + 80, dialogPanel.Width, 50, Color.Transparent, new Padding(0));
            Label dialogSumLabel1 = createLabel.CreateLabelsInPanel(dialogSumPanel, "dialogSumLabel1", "合計", 0, 0, dialogSumPanel.Width / 3 - 20, 50, Color.Transparent, Color.Black, 16);
            Label dialogSumLabel2 = createLabel.CreateLabelsInPanel(dialogSumPanel, "dialogSumLabel2", soldAmountSum + constants.amountUnit, dialogSumPanel.Width / 3, 0, dialogSumPanel.Width / 3 - 20, 50, Color.Transparent, Color.Black, 16);
            Label dialogSumLabel3 = createLabel.CreateLabelsInPanel(dialogSumPanel, "dialogSumLabel2", soldPriceSum + constants.unit, dialogSumPanel.Width * 2 / 3, 0, dialogSumPanel.Width / 3 - 20, 50, Color.Transparent, Color.Black, 16);

            FlowLayoutPanel dialogDatePanel = createPanel.CreateFlowLayoutPanel(dialogPanel, 0, dialogSumPanel.Bottom + 10, dialogPanel.Width, 50, Color.Transparent, new Padding(0));
            Label dialogDateLabel = createLabel.CreateLabelsInPanel(dialogDatePanel, "dialogDateLabel", now.ToLocalTime().ToString(), 0, 0, dialogDatePanel.Width, 50, Color.Transparent, Color.Black, 16, false, ContentAlignment.TopLeft);
            dialogDateLabel.Padding = new Padding(50, 0, 0, 0);
            dialogForm.ShowDialog();

        }

        public void ReceiptIssueReport()
        {
            Form dialogForm = new Form();
            dialogForm.Size = new Size(width / 2, height * 8 / 9);
            dialogForm.BackColor = Color.White;
            dialogForm.StartPosition = FormStartPosition.CenterParent;
            dialogForm.WindowState = FormWindowState.Normal;
            //dialogForm.ControlBox = false;
            dialogForm.MinimizeBox = false;
            dialogForm.MaximizeBox = false;
            dialogForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            DialogFormGlobal = dialogForm;

            Panel dialogPanel = createPanel.CreateMainPanel(dialogForm, 0, 0, dialogForm.Width, dialogForm.Height - 30, BorderStyle.None, Color.Transparent);
            dialogPanel.HorizontalScroll.Maximum = 0;
            dialogPanel.AutoScroll = false;
            dialogPanel.VerticalScroll.Visible = false;
            dialogPanel.AutoScroll = true;


            DateTime now = DateTime.Now;
            FlowLayoutPanel dialogTitlePanel = createPanel.CreateFlowLayoutPanel(dialogPanel, 0, 10, dialogPanel.Width, 50, Color.Transparent, new Padding(0));
            Label dialogTitle1 = createLabel.CreateLabelsInPanel(dialogTitlePanel, "dialogTitle1", constants.receiptionTitle, 0, 0, dialogTitlePanel.Width / 2 - 30, 60, Color.Transparent, Color.Black, 22);
            Label dialogTitle2 = createLabel.CreateLabelsInPanel(dialogTitlePanel, "dialogTitle2", now.ToString("yyyy/MM/dd"), dialogTitlePanel.Width / 2, 0, dialogTitlePanel.Width / 2 - 30, 60, Color.Transparent, Color.Black, 22);
            int k = 0;
            if (sqlite_conn.State == ConnectionState.Closed)
            {
                sqlite_conn.Open();
            }
            SQLiteCommand sqlite_cmd;
            SQLiteDataReader sqlite_datareader;

            string week = now.ToString("ddd");
            string currentTime = now.ToString("HH:mm");

            sqlite_cmd = sqlite_conn.CreateCommand();
            string daySumqurey = "SELECT * FROM " + constants.tbNames[8] + " WHERE ReceiptDate>=@receiptDate1 and ReceiptDate<=@receiptDate2";
            sqlite_cmd.CommandText = daySumqurey;
            sqlite_cmd.Parameters.AddWithValue("@receiptDate1", sumDayTime1);
            sqlite_cmd.Parameters.AddWithValue("@receiptDate2", sumDayTime2);
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                if (!sqlite_datareader.IsDBNull(0))
                {
                    int purchasePoint = sqlite_datareader.GetInt32(1);
                    int totalPrice = sqlite_datareader.GetInt32(2);
                    DateTime receiptDate = sqlite_datareader.GetDateTime(3);

                    FlowLayoutPanel productTableBody = createPanel.CreateFlowLayoutPanel(dialogPanel, 0, 70 + 41 * k, dialogPanel.Width, 40, Color.Transparent, new Padding(0));
                    Label prodNameBody1 = createLabel.CreateLabelsInPanel(productTableBody, "prodBody_" + k + "_1", receiptDate.ToString("yyyy/MM/dd"), 0, 0, productTableBody.Width / 3, productTableBody.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);
                    Label prodNameBody2 = createLabel.CreateLabelsInPanel(productTableBody, "prodBody_" + k + "_2", purchasePoint.ToString() + constants.amountUnit1, productTableBody.Width / 3, 0, productTableBody.Width / 3 - 50, productTableBody.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);
                    Label prodNameBody3 = createLabel.CreateLabelsInPanel(productTableBody, "prodBody_" + k + "_3", totalPrice.ToString() + constants.unit, productTableBody.Width * 2 / 3 - 50, 0, productTableBody.Width / 3 - 50, productTableBody.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);
                    dialogFlowLayout = productTableBody;
                    k++;
                }
            }

            FlowLayoutPanel dialogSumPanel = createPanel.CreateFlowLayoutPanel(dialogPanel, 0, k * 41 + 80, dialogPanel.Width, 50, Color.Transparent, new Padding(10));
            Label dialogSumLabel1 = createLabel.CreateLabelsInPanel(dialogSumPanel, "dialogSumLabel1", "合計", 0, 0, dialogSumPanel.Width / 3 - 20, 50, Color.Transparent, Color.Black, 14);
            Label dialogSumLabel2 = createLabel.CreateLabelsInPanel(dialogSumPanel, "dialogSumLabel2", k + constants.amountUnit, dialogSumPanel.Width / 3, 0, dialogSumPanel.Width / 3 - 20, 50, Color.Transparent, Color.Black, 14);

            FlowLayoutPanel dialogDatePanel = createPanel.CreateFlowLayoutPanel(dialogPanel, 0, dialogSumPanel.Bottom + 10, dialogPanel.Width, 50, Color.Transparent, new Padding(10));
           // dialogDatePanel.Padding = new Padding(dialogDatePanel.Width * 2 / 3, 0, 0, 0);
            Label dialogDateLabel = createLabel.CreateLabelsInPanel(dialogDatePanel, "dialogDateLabel", now.ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss"), 0, 0, dialogDatePanel.Width, 50, Color.Transparent, Color.Black, 14, false, ContentAlignment.TopLeft);
            dialogForm.ShowDialog();
        }

        public void LogReport()
        {
            dropDownMenu.initLogReport(this);
            Form dialogForm = new Form();
            dialogForm.Size = new Size(width / 2, height * 8 / 9);
            dialogForm.BackColor = Color.White;
            dialogForm.StartPosition = FormStartPosition.CenterParent;
            dialogForm.WindowState = FormWindowState.Normal;
            //dialogForm.ControlBox = false;
            dialogForm.MaximizeBox = false;
            dialogForm.MinimizeBox = false;
            dialogForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            DialogFormGlobal = dialogForm;

            Panel dialogPanel = createPanel.CreateMainPanel(dialogForm, 0, 0, dialogForm.Width, dialogForm.Height - 50, BorderStyle.None, Color.Transparent);

            DateTime now = DateTime.Now;
            //Panel dialogSubBottomPanel = createPanel.CreateSubPanel(dialogPanel, 0, 80, dialogPanel.Width, dialogPanel.Height - 110, BorderStyle.FixedSingle, Color.Transparent);
            //dialogSubBottomPanel.SendToBack();
            //FlowLayoutPanel dialogTitlePanel = createPanel.CreateFlowLayoutPanel(dialogPanel, 0, 0, dialogPanel.Width, 80, Color.Transparent, new Padding(0, 20, 0, 10));
            Label dialogTitle1 = createLabel.CreateLabelsInPanel(dialogPanel, "dialogTitle1", constants.logReportLabel + " " + now.ToLocalTime().ToString("yyyy/MM/dd"), 0, 0, dialogPanel.Width * 2 / 3 - 50, 60, Color.Transparent, Color.Black, 20);
            Panel dropdownPanel = dropDownMenu.CreateDropDown("logReport", dialogPanel, constants.times, dialogPanel.Width * 2 / 3 - 30, 10, 100, 40, 100, 40 * (constants.times.Length + 1), 100, 40, Color.Red, Color.Yellow, int.Parse(now.ToString("HH")));

            Label titleTimeLabel = createLabel.CreateLabelsInPanel(dialogPanel, "dialogTitle1", constants.timeRangeLabel, dropdownPanel.Right + 5, 0, 100, 60, Color.Transparent, Color.Black, 20);

            Panel dialogPanelBody = createPanel.CreateSubPanel(dialogPanel, 0, 80, dialogForm.Width, dialogForm.Height - 130, BorderStyle.None, Color.Transparent);
            dialogPanelBody.HorizontalScroll.Maximum = 0;
            dialogPanelBody.AutoScroll = false;
            dialogPanelBody.VerticalScroll.Visible = false;
            dialogPanelBody.AutoScroll = true;

            dialogPanelGlobal = dialogPanelBody;
            //ComboBox timeCombobox = createCombobox.CreateComboboxs(dialogTitlePanel, "timeCombobox", constants.times, dialogTitlePanel.Width * 2 / 3, 0, 100, 40, 25, new Font("Comic Sans", 18));
            logReportBody(now.ToString("HH"), dialogPanelBody);


            dialogForm.ShowDialog();

        }

        private void logReportBody(string logReportTime, Panel dialogPanel)
        {
            //bottomPosition = new int[10];
            int bottomPositions = 0;
            int k = 0;
            int m = 0;

            sumDayTime1 = constants.sumDayTimeStart(storeEndTime);
            sumDayTime2 = constants.sumDayTimeEnd(logReportTime + ":00");

            if (sqlite_conn.State == ConnectionState.Closed)
            {
                sqlite_conn.Open();
            }
            SQLiteCommand sqlite_cmd;
            SQLiteDataReader sqlite_datareader;

            sqlite_cmd = sqlite_conn.CreateCommand();
            string daySumqurey = "SELECT saleDate, sum(prdPrice * prdAmount), ticketNo, count(id) FROM " + constants.tbNames[3] + " WHERE saleDate>=@saleDate1 and saleDate<=@saleDate2 GROUP BY ticketNo ORDER BY saleDate DESC";
            sqlite_cmd.CommandText = daySumqurey;
            sqlite_cmd.Parameters.AddWithValue("@saleDate1", sumDayTime1);
            sqlite_cmd.Parameters.AddWithValue("@saleDate2", sumDayTime2);
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            while (sqlite_datareader.Read())
            {
                if (!sqlite_datareader.IsDBNull(0))
                {
                    DateTime saleDate = sqlite_datareader.GetDateTime(0);
                    int totalPrice = sqlite_datareader.GetInt32(1);
                    int ticketNo = sqlite_datareader.GetInt32(2);

                    //if (k != 9)
                    //{
                    //bottomPosition[k + 1] = bottomPosition[k] + 51 + m * 51;
                    //}

                    int addHeight = 0;
                    if (k != 0)
                    {
                        addHeight = 10;
                        PictureBox pB = new PictureBox();
                        pB.Location = new Point(50, bottomPositions + 10);
                        pB.Size = new Size(dialogPanel.Width - 100, 10);
                        dialogPanel.Controls.Add(pB);
                        Bitmap image = new Bitmap(pB.Size.Width, pB.Size.Height);
                        Graphics g = Graphics.FromImage(image);
                        g.DrawLine(new Pen(Color.FromArgb(255, 142, 133, 118), 3) { DashPattern = new float[] { 5, 1.5F } }, 5, 5, dialogPanel.Width - 160, 5);
                        pB.Image = image;

                    }
                    FlowLayoutPanel productTableBody = createPanel.CreateFlowLayoutPanel(dialogPanel, dialogPanel.Width / 10, bottomPositions + addHeight, dialogPanel.Width * 4 / 5, 50, Color.Transparent, new Padding(0));
                    Label prodNameBody1 = createLabel.CreateLabelsInPanel(productTableBody, "prodBody_" + k + "_1", saleDate.ToString("yyyy/MM/dd HH:mm:ss"), 0, 0, productTableBody.Width / 3 + 30, productTableBody.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12, false, ContentAlignment.MiddleLeft);
                    Label prodNameBody2 = createLabel.CreateLabelsInPanel(productTableBody, "prodBody_" + k + "_2", "", productTableBody.Width / 3 + 40, 0, productTableBody.Width / 3 - 60, productTableBody.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);
                    Label prodNameBody3 = createLabel.CreateLabelsInPanel(productTableBody, "prodBody_" + k + "_3", "計 " + totalPrice + constants.unit, productTableBody.Width * 2 / 3 - 20, 0, productTableBody.Width / 3 - 40, productTableBody.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12, false, ContentAlignment.MiddleRight);
                    dialogFlowLayout = productTableBody;
                    bottomPositions += 51;
                    m = 0;
                    SQLiteCommand sqlite_cmds;
                    SQLiteDataReader sqlite_datareaders;

                    sqlite_cmds = sqlite_conn.CreateCommand();
                    string daySumqureys = "SELECT * FROM " + constants.tbNames[3] + " WHERE saleDate>=@saleDate1 AND saleDate<=@saleDate2 AND ticketNo=@ticketNo ORDER BY saleDate DESC";
                    sqlite_cmds.CommandText = daySumqureys;
                    sqlite_cmds.Parameters.AddWithValue("@saleDate1", sumDayTime1);
                    sqlite_cmds.Parameters.AddWithValue("@saleDate2", sumDayTime2);
                    sqlite_cmds.Parameters.AddWithValue("@ticketNo", ticketNo);
                    sqlite_datareaders = sqlite_cmds.ExecuteReader();
                    while (sqlite_datareaders.Read())
                    {
                        if (!sqlite_datareaders.IsDBNull(0))
                        {
                            string prdName = sqlite_datareaders.GetString(2);
                            int prdAmount = sqlite_datareaders.GetInt32(4);
                            int prdPrice = sqlite_datareaders.GetInt32(3);

                            FlowLayoutPanel productTableBodyContent = createPanel.CreateFlowLayoutPanel(dialogPanel, dialogPanel.Width / 10, productTableBody.Bottom + 41 * m, dialogPanel.Width * 4 / 5, 40, Color.Transparent, new Padding(0));
                            Label prodNameBodyContent1 = createLabel.CreateLabelsInPanel(productTableBodyContent, "prodBody_" + m + "_1", prdName, 0, 0, productTableBodyContent.Width / 3 - 10, productTableBodyContent.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12, false, ContentAlignment.MiddleLeft);
                            Label prodNameBodyContent2 = createLabel.CreateLabelsInPanel(productTableBodyContent, "prodBody_" + m + "_2", prdAmount.ToString() + constants.amountUnit, productTableBodyContent.Width / 3, 0, productTableBodyContent.Width / 3 - 30, productTableBodyContent.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);
                            Label prodNameBodyContent3 = createLabel.CreateLabelsInPanel(productTableBodyContent, "prodBody_" + m + "_3", prdPrice.ToString() + constants.unit, productTableBodyContent.Width * 2 / 3 - 30, 0, productTableBodyContent.Width / 3 - 30, productTableBodyContent.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12, false, ContentAlignment.MiddleRight);
                            dialogFlowLayout = productTableBodyContent;

                            bottomPositions += 41;
                            m++;
                        }
                    }

                    k++;
                }
            }

        }

        public void FalsePurchaseCancellation()
        {
            dropDownMenu.initLogReport(this);
            Form dialogForm = new Form();
            dialogForm.Size = new Size(width / 2, height * 8 / 9);
            dialogForm.BackColor = Color.White;
            dialogForm.StartPosition = FormStartPosition.CenterParent;
            dialogForm.WindowState = FormWindowState.Normal;
            //dialogForm.ControlBox = false;
            dialogForm.MaximizeBox = false;
            dialogForm.MinimizeBox = false;
            dialogForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            DialogFormGlobal = dialogForm;

            //connection = new mysqlconnection("datasource=localhost;port=3306;username=root;password=");
            //connection.open();
            //string connectionstate = "";
            //if (connection.state == connectionstate.open)
            //{
            //    connectionstate = "connected";
            //}
            //else
            //{
            //    connectionstate = "disconnected";
            //}

            //mysqlcommand cmd = new mysqlcommand();
            //cmd.connection = connection;

            //cmd.commandtext = "create database if not exists possystem;";
            //cmd.executenonquery();

            //cmd.commandtext = @"create table if not exists possystem.categories(id integer primary key auto_increment,
            //        name text, price int)";
            //cmd.executenonquery();

            Panel dialogPanel = createPanel.CreateMainPanel(dialogForm, 0, 0, dialogForm.Width, dialogForm.Height - 50, BorderStyle.None, Color.Transparent);
            dialogPanel.HorizontalScroll.Maximum = 0;
            dialogPanel.AutoScroll = false;
            dialogPanel.VerticalScroll.Visible = false;
            dialogPanel.AutoScroll = true;
            dialogPanelGlobal = dialogPanel;

            Label dialogTitle1 = createLabel.CreateLabelsInPanel(dialogPanel, "dialogTitle1", constants.falsePurchasePageTitle, 0, 0, dialogPanel.Width / 2 - 30, 60, Color.Transparent, Color.Black, 22);
            Panel dropdownPanel = dropDownMenu.CreateDropDown("falsePurchase", dialogPanel, constants.times, dialogPanel.Width * 2 / 3 - 30, 10, 100, 40, 100, 40 * (constants.times.Length + 1), 100, 40, Color.Red, Color.Yellow);
            Label titleTimeLabel = createLabel.CreateLabelsInPanel(dialogPanel, "dialogTitle1", constants.timeRangeLabel, dropdownPanel.Right + 5, 0, 100, 60, Color.Transparent, Color.Black, 20);


            DateTime now = DateTime.Now;

            FlowLayoutPanel productTableHeader = createPanel.CreateFlowLayoutPanel(dialogPanel, 10, 60, dialogPanel.Width - 20, 60, Color.Transparent, new Padding(0));
            Label prodNameHeader1 = createLabel.CreateLabelsInPanel(productTableHeader, "prodHeader_1", constants.orderTimeField, 0, 0, productTableHeader.Width / 4 + 20, productTableHeader.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);
            Label prodNameHeader2 = createLabel.CreateLabelsInPanel(productTableHeader, "prodHeader_2", constants.saleNumberField, productTableHeader.Width / 4 + 25, 0, productTableHeader.Width / 4 - 35, productTableHeader.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);
            Label prodNameHeader3 = createLabel.CreateLabelsInPanel(productTableHeader, "prodHeader_3", constants.prodNameField, productTableHeader.Width / 2, 0, productTableHeader.Width / 4 - 10, productTableHeader.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);
            Label prodNameHeader4 = createLabel.CreateLabelsInPanel(productTableHeader, "prodHeader_3", constants.priceField, productTableHeader.Width * 3 / 4, 0, productTableHeader.Width / 4 - 20, productTableHeader.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);

            Panel tbodyPanel = createPanel.CreateSubPanel(dialogPanel, 0, 120, dialogPanel.Width, dialogPanel.Height - 120, BorderStyle.None, Color.Transparent);
            tbodyPanelGlobal = tbodyPanel;
            ShowProdListForFalsePurchaseCancell(tbodyPanel);

            dialogForm.ShowDialog();

        }

        private void ShowProdListForFalsePurchaseCancell(Panel mainPanel)
        {
            DateTime now = DateTime.Now;
            columnGlobal1 = new Label[10];
            columnGlobal2 = new Label[10];
            columnGlobal3 = new Label[10];
            columnGlobal4 = new Label[10];
            totalRowNum = 10;
            for (int k = 0; k < 10; k++)
            {
                FlowLayoutPanel productTableBody = createPanel.CreateFlowLayoutPanel(mainPanel, 10, 61 * k, mainPanel.Width - 20, 60, Color.Transparent, new Padding(0));
                productTableBody.Name = "prdID_" + k;
                Label prodNameBody1 = createLabel.CreateLabelsInPanel(productTableBody, "prodBody_" + k + "_1", now.ToLocalTime().ToString(), 0, 0, productTableBody.Width / 4 + 20, productTableBody.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);
                prodNameBody1.Margin = new Padding(0);

                columnGlobal1[k] = prodNameBody1;
                Label prodNameBody2 = createLabel.CreateLabelsInPanel(productTableBody, "prodBody_" + k + "_2", constants.recieptIssueAmount[k].ToString() + constants.amountUnit, productTableBody.Width / 4 + 20, 0, productTableBody.Width / 4 - 35, productTableBody.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);
                prodNameBody2.Margin = new Padding(0);
                columnGlobal2[k] = prodNameBody2;
                Label prodNameBody3 = createLabel.CreateLabelsInPanel(productTableBody, "prodBody_" + k + "_3", constants.productBigName[1][k].ToString(), productTableBody.Width / 2, 0, productTableBody.Width / 4 - 10, productTableBody.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);
                prodNameBody3.Margin = new Padding(0);
                columnGlobal3[k] = prodNameBody3;
                Label prodNameBody4 = createLabel.CreateLabelsInPanel(productTableBody, "prodBody_" + k + "_4", constants.recieptIssuePrice[k].ToString() + constants.unit, productTableBody.Width * 3 / 4, 0, productTableBody.Width / 4 - 20, productTableBody.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);
                dialogFlowLayout = productTableBody;
                prodNameBody4.Margin = new Padding(0);
                columnGlobal4[k] = prodNameBody4;

                prodNameBody1.Click += new EventHandler(this.CancellationSetting);
                prodNameBody2.Click += new EventHandler(this.CancellationSetting);
                prodNameBody3.Click += new EventHandler(this.CancellationSetting);
                prodNameBody4.Click += new EventHandler(this.CancellationSetting);

            }

        }
        private void CancellationSetting(object sender, EventArgs e)
        {

            Label prdTemp = (Label)sender;
            int prdID = int.Parse(prdTemp.Name.Split('_')[1]);
            columnGlobal1[prdID].BackColor = Color.Red;
            columnGlobal1[prdID].ForeColor = Color.White;
            columnGlobal2[prdID].BackColor = Color.Red;
            columnGlobal2[prdID].ForeColor = Color.White;
            columnGlobal3[prdID].BackColor = Color.Red;
            columnGlobal3[prdID].ForeColor = Color.White;
            columnGlobal4[prdID].BackColor = Color.Red;
            columnGlobal4[prdID].ForeColor = Color.White;
            for (int k = 0; k < totalRowNum; k++)
            {
                if (k != prdID)
                {
                    columnGlobal1[k].BackColor = Color.Transparent;
                    columnGlobal1[k].ForeColor = Color.FromArgb(255, 142, 133, 118);
                    columnGlobal2[k].BackColor = Color.Transparent;
                    columnGlobal2[k].ForeColor = Color.FromArgb(255, 142, 133, 118);
                    columnGlobal3[k].BackColor = Color.Transparent;
                    columnGlobal3[k].ForeColor = Color.FromArgb(255, 142, 133, 118);
                    columnGlobal4[k].BackColor = Color.Transparent;
                    columnGlobal4[k].ForeColor = Color.FromArgb(255, 142, 133, 118);
                }
            }

            int productTypes = 3;
            int frmheight = (7 + productTypes) * 50;
            Form dialogForm = new Form();
            dialogForm.Size = new Size(width / 3, frmheight);
            dialogForm.BackColor = Color.White;
            dialogForm.StartPosition = FormStartPosition.CenterParent;
            dialogForm.WindowState = FormWindowState.Normal;
            dialogForm.ControlBox = false;
            dialogForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            //DialogFormGlobal = dialogForm;

            Panel titlePanel = createPanel.CreateMainPanel(dialogForm, 0, 0, dialogForm.Width, dialogForm.Height / 10, BorderStyle.None, Color.Transparent);
            Label titleLabel = createLabel.CreateLabelsInPanel(titlePanel, "titleLabel", "取消確認", 0, 0, titlePanel.Width, titlePanel.Height, Color.Transparent, Color.Black, 22, false, ContentAlignment.BottomCenter);

            Panel dialogPanel = createPanel.CreateMainPanel(dialogForm, dialogForm.Width / 11, dialogForm.Height / 10, dialogForm.Width * 9 / 11, dialogForm.Height * 7 / 10, BorderStyle.FixedSingle, Color.Transparent);

            FlowLayoutPanel leftColumn = createPanel.CreateFlowLayoutPanel(dialogPanel, 0, 0, dialogPanel.Width * 2/ 5, dialogPanel.Height, Color.Transparent, new Padding(0));
            FlowLayoutPanel rightColumn = createPanel.CreateFlowLayoutPanel(dialogPanel, dialogPanel.Width * 2 / 5, 0, dialogPanel.Width * 3 / 5, dialogPanel.Height, Color.Transparent, new Padding(0));

            int rowCount = 4 + productTypes;

            Label column1 = createLabel.CreateLabels(leftColumn, "orderDateColumn", "注文日", 0, 0, leftColumn.Width, leftColumn.Height / rowCount, Color.White, Color.Black, 12, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
            Label column2 = createLabel.CreateLabels(leftColumn, "orderTimeColumn", "注文時間", 0, column1.Bottom, leftColumn.Width, leftColumn.Height / rowCount, Color.White, Color.Black, 12, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
            Label column3 = createLabel.CreateLabels(leftColumn, "orderNumberColumn", "売上連番", 0, column2.Bottom, leftColumn.Width, leftColumn.Height / rowCount, Color.White, Color.Black, 12, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
            Label column4 = createLabel.CreateLabels(leftColumn, "orderNameColumn", "品目", 0, column3.Bottom, leftColumn.Width, leftColumn.Height * productTypes / rowCount, Color.White, Color.Black, 12, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
            Label column5 = createLabel.CreateLabels(leftColumn, "orderPriceColumn", "合計金額", 0, column4.Bottom, leftColumn.Width, leftColumn.Height / rowCount, Color.White, Color.Black, 12, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);

            Label value1 = createLabel.CreateLabels(rightColumn, "orderDateValue", "2020/04/12", 0, 0, rightColumn.Width, rightColumn.Height / rowCount, Color.White, Color.Black, 12, true, ContentAlignment.MiddleCenter, new Padding(0), 2, Color.Gray);
            Label value2 = createLabel.CreateLabels(rightColumn, "orderTimeValue", "12：18：53", 0, column1.Bottom, rightColumn.Width, rightColumn.Height / rowCount, Color.White, Color.Black, 12, true, ContentAlignment.MiddleCenter, new Padding(0), 2, Color.Gray);
            Label value3 = createLabel.CreateLabels(rightColumn, "orderNumberValue", "0000000039", 0, column2.Bottom, rightColumn.Width, rightColumn.Height / rowCount, Color.White, Color.Black, 12, true, ContentAlignment.MiddleCenter, new Padding(0), 2, Color.Gray);
            for (int i = 0; i < productTypes; i++)
            {
                Label values = createLabel.CreateLabels(rightColumn, "orderNameValue_" + i, "味噌ラーメン×1", 0, column3.Bottom + rightColumn.Height * i / rowCount, rightColumn.Width, rightColumn.Height / rowCount, Color.White, Color.Black, 12, true, ContentAlignment.MiddleCenter, new Padding(0), 2, Color.Gray);
            }
            Label value = createLabel.CreateLabels(rightColumn, "orderPriceValue", "合計金額", 0, value3.Bottom + rightColumn.Height * productTypes / rowCount, rightColumn.Width, rightColumn.Height / rowCount, Color.White, Color.Black, 12, true, ContentAlignment.MiddleCenter, new Padding(0), 2, Color.Gray);

            Button closeButton = createButton.CreateButton("キャンセル", "closeButton", dialogPanel.Left, dialogPanel.Bottom + 20, dialogPanel.Width / 2 - 30, 50, Color.FromArgb(255, 0, 112, 192), Color.Transparent, 0, 1, 12, FontStyle.Regular, Color.White);
            closeButton.Click += new EventHandler(this.CancelCloseDialog);

            Button cancelButton = createButton.CreateButton("取消実行", "cancelButton", dialogPanel.Right - dialogPanel.Width / 2 + 30 , dialogPanel.Bottom + 20, dialogPanel.Width / 2 - 30, 50, Color.Red, Color.Transparent, 0, 1, 12, FontStyle.Regular, Color.White);
            cancelDialogFormGlobal = dialogForm;
            dialogForm.Controls.Add(closeButton);
            dialogForm.Controls.Add(cancelButton);
            dialogForm.ShowDialog();
        }

        public void CategoryPrintView()
        {
            Form dialogForm = new Form();
            dialogForm.Size = new Size(width / 2, height * 8 / 9);
            dialogForm.BackColor = Color.White;
            dialogForm.StartPosition = FormStartPosition.CenterParent;
            dialogForm.WindowState = FormWindowState.Normal;
            dialogForm.ControlBox = false;
            dialogForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            DialogFormGlobal = dialogForm;

            Panel dialogPanel = createPanel.CreateMainPanel(dialogForm, 0, 0, dialogForm.Width, dialogForm.Height, BorderStyle.None, Color.Transparent);
            dialogPanel.AutoScroll = true;

            FlowLayoutPanel dialogTopPanel = createPanel.CreateFlowLayoutPanel(dialogPanel, 0, 0, dialogPanel.Width - 30, 50, Color.Transparent, new Padding(dialogPanel.Width - 65, 5, 0, 0));

            Button closeButton = createButton.CreateButton("x", "closeButton", 0, 0, 30, 30, Color.FromArgb(255, 0, 112, 192), Color.Transparent, 0, 1);
            closeButton.ForeColor = Color.White;
            //closeButton.Dock = DockStyle.Right;
            dialogTopPanel.Controls.Add(closeButton);
            closeButton.Click += new EventHandler(this.CloseDialog);

            FlowLayoutPanel dialogTitlePanel1 = createPanel.CreateFlowLayoutPanel(dialogPanel, 0, 50, dialogPanel.Width, 50, Color.Transparent, new Padding(0));
            Label dialogTitle1 = createLabel.CreateLabelsInPanel(dialogTitlePanel1, "dialogTitle1", constants.categoryListPrintTitle, 0, 0, dialogTitlePanel1.Width, 50, Color.Transparent, Color.Black, 22);
            FlowLayoutPanel dialogTitlePanel2 = createPanel.CreateFlowLayoutPanel(dialogPanel, 0, 100, dialogPanel.Width, 50, Color.Transparent, new Padding(0));
            Label dialogTitle2 = createLabel.CreateLabelsInPanel(dialogTitlePanel2, "dialogTitle2", "2020/04/18　　22:05:59", 0, 0, dialogTitlePanel2.Width, 50, Color.Transparent, Color.Black, 22);
            FlowLayoutPanel dialogTitlePanel3 = createPanel.CreateFlowLayoutPanel(dialogPanel, 0, 150, dialogPanel.Width, 50, Color.Transparent, new Padding(0));
            Label dialogTitle3 = createLabel.CreateLabelsInPanel(dialogTitlePanel3, "dialogTitle3", constants.storeName, 0, 0, dialogTitlePanel3.Width, 50, Color.Transparent, Color.Black, 22);

            int m = 0;
            int topPosition = 210;
            foreach (string categoryItem in constants.saleCategories)
            {
              //  int n = (m == 0) ? m : m - 1;
                FlowLayoutPanel productTableBody1 = createPanel.CreateFlowLayoutPanel(dialogPanel, 0, topPosition, dialogPanel.Width, 50, Color.Transparent, new Padding(30, 0, 0, 0));
                Label categoryLabel1 = createLabel.CreateLabelsInPanel(productTableBody1, "categoryNameLabel_" + m + "_1", "表示位置" + (m + 1) + "／カテゴリー" + (m + 1), 0, 0, productTableBody1.Width / 4 - 10, productTableBody1.Height, Color.Transparent, Color.FromArgb(255, 47, 44, 39), 12, false, ContentAlignment.MiddleLeft);
                Label categoryLabel2 = createLabel.CreateLabelsInPanel(productTableBody1, "categoryName_" + m + "_1", categoryItem, productTableBody1.Width / 3, 0, productTableBody1.Width / 3, productTableBody1.Height, Color.Transparent, Color.FromArgb(255, 47, 44, 39), 12, false, ContentAlignment.MiddleLeft);
                FlowLayoutPanel productTableBody2 = createPanel.CreateFlowLayoutPanel(dialogPanel, 0, topPosition + 50, dialogPanel.Width, 50, Color.Transparent, new Padding(30, 0, 0, 0));
                Label categoryTimeLabel = createLabel.CreateLabelsInPanel(productTableBody2, "categoryTimeLabel_" + m + "_2", "販売時刻: ", 0, 0, productTableBody2.Width / 4 - 10, productTableBody2.Height, Color.Transparent, Color.FromArgb(255, 47, 44, 39), 12, false, ContentAlignment.MiddleLeft);
                Label categoryTimeValue = createLabel.CreateLabelsInPanel(productTableBody2, "categoryTimeValue_" + m + "_2", "10：00～21:59", productTableBody2.Width / 3, 0, productTableBody2.Width / 3 - 10, productTableBody2.Height, Color.Transparent, Color.FromArgb(255, 47, 44, 39), 12, false, ContentAlignment.MiddleLeft);

                int k = 0;
                foreach (string prodItem in constants.productBigName[m])
                {
                    FlowLayoutPanel productTableBody = createPanel.CreateFlowLayoutPanel(dialogPanel, 0, topPosition + 100 + 50 * k, dialogPanel.Width, 50, Color.Transparent, new Padding(30, 0, 0, 0));
                    Label prodNameBody1 = createLabel.CreateLabelsInPanel(productTableBody, "prodBody_" + k + "_1", prodItem, 0, 0, productTableBody.Width / 3 - 50, productTableBody.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12, false, ContentAlignment.MiddleLeft);
                    Label prodNameBody2 = createLabel.CreateLabelsInPanel(productTableBody, "prodBody_" + k + "_2", constants.productBigPrice[m][k].ToString() + constants.unit, productTableBody.Width / 3, 0, productTableBody.Width / 3 - 50, productTableBody.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);
                    Label prodNameBody3 = createLabel.CreateLabelsInPanel(productTableBody, "prodBody_" + k + "_3", constants.productBigSaleAmount[m][k].ToString(), productTableBody.Width * 2 / 3, 0, productTableBody.Width / 3 - 50, productTableBody.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);
                    dialogFlowLayout = productTableBody;
                    k++;
               }
                topPosition += 100 + 50 * constants.productBigName[m].Length;
                m++;
            }
            dialogForm.ShowDialog();
        }
        public void GroupPrintView()
        {
            Form dialogForm = new Form();
            dialogForm.Size = new Size(width / 2, height * 8 / 9);
            dialogForm.BackColor = Color.White;
            dialogForm.StartPosition = FormStartPosition.CenterParent;
            dialogForm.WindowState = FormWindowState.Normal;
            dialogForm.ControlBox = false;
            dialogForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            DialogFormGlobal = dialogForm;

            Panel dialogPanel = createPanel.CreateMainPanel(dialogForm, 0, 0, dialogForm.Width, dialogForm.Height, BorderStyle.None, Color.Transparent);
            dialogPanel.AutoScroll = true;

            FlowLayoutPanel dialogTopPanel = createPanel.CreateFlowLayoutPanel(dialogPanel, 0, 0, dialogPanel.Width - 30, 50, Color.Transparent, new Padding(dialogPanel.Width - 65, 5, 0, 0));

            Button closeButton = createButton.CreateButton("x", "closeButton", 0, 0, 30, 30, Color.FromArgb(255, 0, 112, 192), Color.Transparent, 0, 1);
            closeButton.ForeColor = Color.White;
            //closeButton.Dock = DockStyle.Right;
            dialogTopPanel.Controls.Add(closeButton);
            closeButton.Click += new EventHandler(this.CloseDialog);

            FlowLayoutPanel dialogTitlePanel1 = createPanel.CreateFlowLayoutPanel(dialogPanel, 0, 50, dialogPanel.Width, 50, Color.Transparent, new Padding(0));
            Label dialogTitle1 = createLabel.CreateLabelsInPanel(dialogTitlePanel1, "dialogTitle1", constants.groupListPrintTitle, 0, 0, dialogTitlePanel1.Width, 50, Color.Transparent, Color.Black, 22);
            FlowLayoutPanel dialogTitlePanel2 = createPanel.CreateFlowLayoutPanel(dialogPanel, 0, 100, dialogPanel.Width, 50, Color.Transparent, new Padding(0));
            Label dialogTitle2 = createLabel.CreateLabelsInPanel(dialogTitlePanel2, "dialogTitle2", "2020/04/18　　22:05:59", 0, 0, dialogTitlePanel2.Width, 50, Color.Transparent, Color.Black, 22);
            FlowLayoutPanel dialogTitlePanel3 = createPanel.CreateFlowLayoutPanel(dialogPanel, 0, 150, dialogPanel.Width, 50, Color.Transparent, new Padding(0));
            Label dialogTitle3 = createLabel.CreateLabelsInPanel(dialogTitlePanel3, "dialogTitle3", constants.storeName, 0, 0, dialogTitlePanel3.Width, 50, Color.Transparent, Color.Black, 22);

            int m = 0;
            int topPosition = 210;
            foreach (string categoryItem in constants.saleCategories)
            {
                //  int n = (m == 0) ? m : m - 1;
                FlowLayoutPanel productTableBody1 = createPanel.CreateFlowLayoutPanel(dialogPanel, 0, topPosition, dialogPanel.Width, 50, Color.Transparent, new Padding(30, 0, 0, 0));
                Label categoryLabel1 = createLabel.CreateLabelsInPanel(productTableBody1, "categoryNameLabel_" + m + "_1", constants.groupTitleLabel + (m + 1) + ": " + categoryItem, 0, 0, productTableBody1.Width / 4 - 10, productTableBody1.Height, Color.Transparent, Color.FromArgb(255, 47, 44, 39), 12, false, ContentAlignment.MiddleLeft);

                int k = 0;
                foreach (string prodItem in constants.productBigName[m])
                {
                    FlowLayoutPanel productTableBody = createPanel.CreateFlowLayoutPanel(dialogPanel, 0, topPosition + 50 + 50 * k, dialogPanel.Width, 50, Color.Transparent, new Padding(30, 0, 0, 0));
                    Label prodNameBody1 = createLabel.CreateLabelsInPanel(productTableBody, "prodBody_" + k + "_1", prodItem, 0, 0, productTableBody.Width / 2 - 50, productTableBody.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12, false, ContentAlignment.MiddleLeft);
                    Label prodNameBody2 = createLabel.CreateLabelsInPanel(productTableBody, "prodBody_" + k + "_2", constants.productBigPrice[m][k].ToString() + constants.unit, productTableBody.Width / 2, 0, productTableBody.Width / 2 - 50, productTableBody.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12, false, ContentAlignment.MiddleLeft);
                    dialogFlowLayout = productTableBody;
                    k++;
                }
                topPosition += 60 + 50 * constants.productBigName[m].Length;
                m++;
            }
            dialogForm.ShowDialog();
        }

        public void DateSetting(object sender, EventArgs e)
        {
            Form dialogForm = new Form();
            dialogForm.Size = new Size(width / 3, height / 2);
            dialogForm.BackColor = Color.White;
            dialogForm.StartPosition = FormStartPosition.CenterParent;
            dialogForm.WindowState = FormWindowState.Normal;
            dialogForm.ControlBox = false;
            dialogForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            DialogFormGlobal = dialogForm;

            DateTime now = DateTime.Now;

            Panel dialogPanel = createPanel.CreateMainPanel(dialogForm, 0, 0, dialogForm.Width, dialogForm.Height, BorderStyle.None, Color.Transparent);

            Label titleLabel = createLabel.CreateLabelsInPanel(dialogPanel, "titleLabel", constants.dateSettingTitle, 30, 10, dialogPanel.Width - 30, 35, Color.White, Color.Black, 22, false, ContentAlignment.MiddleLeft);

            FlowLayoutPanel yearPanel = createPanel.CreateFlowLayoutPanel(dialogPanel, dialogPanel.Width / 5, 35, dialogPanel.Width / 5, dialogPanel.Height * 4 / 5 - 20, Color.Transparent, new Padding(0));
            Label nextYear = createLabel.CreateLabels(yearPanel, "nextYear", (now.Year + 1).ToString(), 0, 0, yearPanel.Width, yearPanel.Height / 3, Color.White, Color.FromArgb(255, 191, 191, 191), 18);
            nextYearGlobal = nextYear;

            Label currentYear = createLabel.CreateLabels(yearPanel, "currentYear", (now.Year).ToString(), 0, yearPanel.Height / 3, yearPanel.Width, yearPanel.Height / 3, Color.White, Color.Black, 18, true, ContentAlignment.MiddleCenter, default(Padding), 4, Color.FromArgb(255, 0, 176, 240), "top_bottom_line");
            currentYearGlobal = currentYear;

            Label prevYear = createLabel.CreateLabels(yearPanel, "prevYear", (now.Year - 1).ToString(), 0, yearPanel.Height * 2 / 3, yearPanel.Width, yearPanel.Height / 3, Color.White, Color.FromArgb(255, 191, 191, 191), 18);
            prevYearGlobal = prevYear;

            currentYear.MouseWheel += new MouseEventHandler(this.YearSelect);

            int currentMonthValue = now.Month;
            int nextMonthValue = now.Month + 1;
            int prevMonthValue = now.Month - 1;
            if(currentMonthValue == 1)
            {
                prevMonthValue = 12;
            }
            if(currentMonthValue == 12)
            {
                nextMonthValue = 1;
            }
            FlowLayoutPanel monthPanel = createPanel.CreateFlowLayoutPanel(dialogPanel, dialogPanel.Width * 2 / 5 + 10, 35, dialogPanel.Width / 5 - 10, dialogPanel.Height * 4 / 5 - 20, Color.Transparent, new Padding(0));
            Label nextMonth = createLabel.CreateLabels(monthPanel, "nextMonth", nextMonthValue.ToString(), 0, 0, monthPanel.Width, monthPanel.Height / 3, Color.White, Color.FromArgb(255, 191, 191, 191), 18);
            nextMonthGlobal = nextMonth;

            Label currentMonth = createLabel.CreateLabels(monthPanel, "currentMonth", currentMonthValue.ToString(), 0, monthPanel.Height / 3, monthPanel.Width, monthPanel.Height / 3, Color.White, Color.Black, 18, true, ContentAlignment.MiddleCenter, default(Padding), 4, Color.FromArgb(255, 0, 176, 240), "top_bottom_line");
            currentMonthGlobal = currentMonth;

            Label prevMonth = createLabel.CreateLabels(monthPanel, "prevMonth", prevMonthValue.ToString(), 0, monthPanel.Height * 2 / 3, monthPanel.Width, monthPanel.Height / 3, Color.White, Color.FromArgb(255, 191, 191, 191), 18);
            prevMonthGlobal = prevMonth;

            currentMonth.MouseWheel += new MouseEventHandler(this.MonthSelect);

            int currentDayValue = now.Day;
            int nextDayValue = now.Day + 1;
            int prevDayValue = now.Day - 1;
            int endDay = DateTime.DaysInMonth(now.Year, now.Month);
            if (currentDayValue == 1)
            {
                prevDayValue = endDay;
            }
            if (currentDayValue == endDay)
            {
                nextDayValue = 1;
            }

            FlowLayoutPanel dayPanel = createPanel.CreateFlowLayoutPanel(dialogPanel, dialogPanel.Width * 3 / 5 + 10, 35, dialogPanel.Width / 5 - 10, dialogPanel.Height * 4 / 5 - 20, Color.Transparent, new Padding(0));
            Label nextDay = createLabel.CreateLabels(dayPanel, "nextDay", nextDayValue.ToString(), 0, 0, dayPanel.Width, dayPanel.Height / 3, Color.White, Color.FromArgb(255, 191, 191, 191), 18);
            nextDayGlobal = nextDay;

            Label currentDay = createLabel.CreateLabels(dayPanel, "currentDay", currentDayValue.ToString(), 0, dayPanel.Height / 3, dayPanel.Width, dayPanel.Height / 3, Color.White, Color.Black, 18, true, ContentAlignment.MiddleCenter, default(Padding), 4, Color.FromArgb(255, 0, 176, 240), "top_bottom_line");
            currentDayGlobal = currentDay;

            Label prevDay = createLabel.CreateLabels(dayPanel, "prevDay", prevDayValue.ToString(), 0, dayPanel.Height * 2 / 3, dayPanel.Width, dayPanel.Height / 3, Color.White, Color.FromArgb(255, 191, 191, 191), 18);
            prevDayGlobal = prevDay;

            currentDay.MouseWheel += new MouseEventHandler(this.DaySelect);

            Button setButton = createButton.CreateButton("OK", "setDateButton", dialogPanel.Width - 100, dialogPanel.Height - 50, 80, 30, Color.FromArgb(255, 0, 112, 192), Color.Transparent, 0, 1);
            dialogPanel.Controls.Add(setButton);

            setButton.Click += new EventHandler(this.SetDate);

            dialogForm.ShowDialog();
        }
        public void TimeSetting(object sender, EventArgs e)
        {
            Form dialogForm = new Form();
            dialogForm.Size = new Size(width / 3, height / 2);
            dialogForm.BackColor = Color.White;
            dialogForm.StartPosition = FormStartPosition.CenterParent;
            dialogForm.WindowState = FormWindowState.Normal;
            dialogForm.ControlBox = false;
            dialogForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            DialogFormGlobal = dialogForm;

            DateTime now = DateTime.Now;

            Panel dialogPanel = createPanel.CreateMainPanel(dialogForm, 0, 0, dialogForm.Width, dialogForm.Height, BorderStyle.None, Color.Transparent);

            Label titleLabel = createLabel.CreateLabelsInPanel(dialogPanel, "titleLabel", constants.timeSettingTitle, 30, 10, dialogPanel.Width - 30, 35, Color.White, Color.Black, 22, false, ContentAlignment.MiddleLeft);

            int currentHourValue = now.Hour;
            int nextHourValue = now.Hour + 1;
            int prevHourValue = now.Hour - 1;
            if (currentHourValue == 1)
            {
                prevHourValue = 23;
            }
            if (currentHourValue == 23)
            {
                nextHourValue = 0;
            }

            FlowLayoutPanel houurPanel = createPanel.CreateFlowLayoutPanel(dialogPanel, dialogPanel.Width / 5, 35, dialogPanel.Width / 5, dialogPanel.Height * 4 / 5 - 20, Color.Transparent, new Padding(0));

            Label nextHour = createLabel.CreateLabels(houurPanel, "nextHour", nextHourValue.ToString(), 0, 0, houurPanel.Width, houurPanel.Height / 3, Color.White, Color.FromArgb(255, 191, 191, 191), 18);
            nextHourGlobal = nextHour;

            Label currentHour = createLabel.CreateLabels(houurPanel, "currentHour", currentHourValue.ToString(), 0, houurPanel.Height / 3, houurPanel.Width, houurPanel.Height / 3, Color.White, Color.Black, 18, true, ContentAlignment.MiddleCenter, default(Padding), 4, Color.FromArgb(255, 0, 176, 240), "top_bottom_line");
            currentHourGlobal = currentHour;

            Label prevHour = createLabel.CreateLabels(houurPanel, "prevHour", prevHourValue.ToString(), 0, houurPanel.Height * 2 / 3, houurPanel.Width, houurPanel.Height / 3, Color.White, Color.FromArgb(255, 191, 191, 191), 18);
            prevHourGlobal = prevHour;

            currentHour.MouseWheel += new MouseEventHandler(this.HourMinuteSelect);

            int currentMinuteValue = now.Minute;
            int nextMinuteValue = now.Minute + 1;
            int prevMinuteValue = now.Minute - 1;
            int endMinute = 59;
            if (currentMinuteValue == 0)
            {
                prevMinuteValue = endMinute;
            }
            if (currentMinuteValue == endMinute)
            {
                nextMinuteValue = 0;
            }

            FlowLayoutPanel MinutePanel = createPanel.CreateFlowLayoutPanel(dialogPanel, dialogPanel.Width * 3 / 5 + 10, 35, dialogPanel.Width / 5 - 10, dialogPanel.Height * 4 / 5 - 20, Color.Transparent, new Padding(0));
            Label nextMinute = createLabel.CreateLabels(MinutePanel, "nextMinute", nextMinuteValue.ToString(), 0, 0, MinutePanel.Width, MinutePanel.Height / 3, Color.White, Color.FromArgb(255, 191, 191, 191), 18);
            nextMinuteGlobal = nextMinute;

            Label currentMinute = createLabel.CreateLabels(MinutePanel, "currentMinute", currentMinuteValue.ToString(), 0, MinutePanel.Height / 3, MinutePanel.Width, MinutePanel.Height / 3, Color.White, Color.Black, 18, true, ContentAlignment.MiddleCenter, default(Padding), 4, Color.FromArgb(255, 0, 176, 240), "top_bottom_line");
            currentMinuteGlobal = currentMinute;

            Label prevMinute = createLabel.CreateLabels(MinutePanel, "prevMinute", prevMinuteValue.ToString(), 0, MinutePanel.Height * 2 / 3, MinutePanel.Width, MinutePanel.Height / 3, Color.White, Color.FromArgb(255, 191, 191, 191), 18);
            prevMinuteGlobal = prevMinute;

            currentMinute.MouseWheel += new MouseEventHandler(this.HourMinuteSelect);

            Button setButton = createButton.CreateButton("OK", "setTimeButton", dialogPanel.Width - 100, dialogPanel.Height - 50, 80, 30, Color.FromArgb(255, 0, 112, 192), Color.Transparent, 0, 1);
            dialogPanel.Controls.Add(setButton);

            setButton.Click += new EventHandler(this.SetDate);

            dialogForm.ShowDialog();
        }
        public void CloseDialog(object sender, EventArgs e)
        {
            DialogFormGlobal.Close();
        }
        public void CancelCloseDialog(object sender, EventArgs e)
        {
            cancelDialogFormGlobal.Close();
        }

        public void YearSelect(object sender, MouseEventArgs e)
        {
            int currentYear = int.Parse(currentYearGlobal.Text);
            int nextYear = int.Parse(nextYearGlobal.Text);
            int prevYear = int.Parse(prevYearGlobal.Text);
            if (e.Delta > 0)
            {
                currentYear++;
                nextYear++;
                prevYear++;
            }
            else
            {
                currentYear--;
                nextYear--;
                prevYear--;
            }
            currentYearGlobal.Text = currentYear.ToString();
            nextYearGlobal.Text = nextYear.ToString();
            prevYearGlobal.Text = prevYear.ToString();

        }
        public void MonthSelect(object sender, MouseEventArgs e)
        {
            int currentMonth = int.Parse(currentMonthGlobal.Text);
            int nextMonth = int.Parse(nextMonthGlobal.Text);
            int prevMonth = int.Parse(prevMonthGlobal.Text);
            if (e.Delta > 0)
            {
                if(currentMonth == 12)
                {
                    currentMonth = 1;
                    nextMonth++;
                    prevMonth++;
                }
                else if(currentMonth == 11)
                {
                    currentMonth++;
                    nextMonth = 1;
                    prevMonth++;
                }
                else if(currentMonth == 1)
                {
                    currentMonth++;
                    nextMonth++;
                    prevMonth = 1;
                }
                else
                {
                    currentMonth++;
                    nextMonth++;
                    prevMonth++;
                }

            }
            else
            {
                if (currentMonth == 12)
                {
                    currentMonth--;
                    nextMonth = 12;
                    prevMonth--;
                }
                else if (currentMonth == 1)
                {
                    currentMonth = 12;
                    nextMonth--;
                    prevMonth--;
                }
                else if (currentMonth == 2)
                {
                    nextMonth--;
                    currentMonth--;
                    prevMonth = 12;
                }
                else
                {
                    currentMonth--;
                    nextMonth--;
                    prevMonth--;
                }
            }
            currentMonthGlobal.Text = currentMonth.ToString();
            nextMonthGlobal.Text = nextMonth.ToString();
            prevMonthGlobal.Text = prevMonth.ToString();

        }
        public void DaySelect(object sender, MouseEventArgs e)
        {
            int currentYear = int.Parse(currentYearGlobal.Text);
            int currentMonth = int.Parse(currentMonthGlobal.Text);
            int endDate = DateTime.DaysInMonth(currentYear, currentMonth);

            int currentDay = int.Parse(currentDayGlobal.Text);
            int nextDay = int.Parse(nextDayGlobal.Text);
            int prevDay = int.Parse(prevDayGlobal.Text);
            if (e.Delta > 0)
            {

                if (currentDay == endDate)
                {
                    currentDay = 1;
                    nextDay++;
                    prevDay++;
                }
                else if (currentDay == endDate - 1)
                {
                    currentDay++;
                    nextDay = 1;
                    prevDay++;
                }
                else if (currentDay == 1)
                {
                    currentDay++;
                    nextDay++;
                    prevDay = 1;
                }
                else
                {
                    currentDay++;
                    nextDay++;
                    prevDay++;
                }
            }
            else
            {
                if (currentDay == endDate)
                {
                    currentDay--;
                    nextDay = endDate;
                    prevDay--;
                }
                else if (currentDay == 1)
                {
                    currentDay = endDate;
                    nextDay--;
                    prevDay--;
                }
                else if (currentDay == 2)
                {
                    nextDay--;
                    currentDay--;
                    prevDay = endDate;
                }
                else
                {
                    currentDay--;
                    nextDay--;
                    prevDay--;
                }
            }
            currentDayGlobal.Text = currentDay.ToString();
            nextDayGlobal.Text = nextDay.ToString();
            prevDayGlobal.Text = prevDay.ToString();

        }
        public void HourMinuteSelect(object sender, MouseEventArgs e)
        {
            Label lTemp = (Label)sender;
            int endValue = 59;
            int currentValue = int.Parse(currentMinuteGlobal.Text);
            int nextValue = int.Parse(nextMinuteGlobal.Text);
            int prevValue = int.Parse(prevMinuteGlobal.Text);

            if (lTemp.Name == "currentHour")
            {
                endValue = 23;
                currentValue = int.Parse(currentHourGlobal.Text);
                nextValue = int.Parse(nextHourGlobal.Text);
                prevValue = int.Parse(prevHourGlobal.Text);
            }
            if (e.Delta > 0)
            {
                if (currentValue == endValue)
                {
                    currentValue = 0;
                    nextValue++;
                    prevValue++;
                }
                else if (currentValue == endValue - 1)
                {
                    currentValue++;
                    nextValue = 1;
                    prevValue++;
                }
                else if (currentValue == 0)
                {
                    currentValue++;
                    nextValue++;
                    prevValue = 0;
                }
                else
                {
                    currentValue++;
                    nextValue++;
                    prevValue++;
                }

            }
            else
            {
                if (currentValue == endValue)
                {
                    currentValue--;
                    nextValue = endValue;
                    prevValue--;
                }
                else if (currentValue == 0)
                {
                    currentValue = endValue;
                    nextValue--;
                    prevValue--;
                }
                else if (currentValue == 1)
                {
                    nextValue--;
                    currentValue--;
                    prevValue = endValue;
                }
                else
                {
                    currentValue--;
                    nextValue--;
                    prevValue--;
                }
            }
            if (lTemp.Name == "currentHour")
            {
                currentHourGlobal.Text = this.DateTimeFormat(currentValue);
                nextHourGlobal.Text = this.DateTimeFormat(nextValue);
                prevHourGlobal.Text = this.DateTimeFormat(prevValue);
            }
            else
            {
                currentMinuteGlobal.Text = this.DateTimeFormat(currentValue);
                nextMinuteGlobal.Text = this.DateTimeFormat(nextValue);
                prevMinuteGlobal.Text = this.DateTimeFormat(prevValue);
            }

        }

        private string DateTimeFormat(int val)
        {
            if(val < 10)
            {
                return "0" + val.ToString();
            }
            return val.ToString();
        }

        public void SetDate(object sender, EventArgs e)
        {
            DialogFormGlobal.Close();
            Button btnTemp = (Button)sender;
            if(btnTemp.Name == "setDateButton")
            {
                string currentYear = currentYearGlobal.Text;
                string currentMonth = currentMonthGlobal.Text;
                string currentDay = currentDayGlobal.Text;
                timeHandlerGlobal.setVal("setDate", currentYear + "_" + currentMonth + "_" + currentDay);
            }
            else if (btnTemp.Name == "setTimeButton")
            {
                string currentHour = currentHourGlobal.Text;
                string currentMinute = currentMinuteGlobal.Text;
                timeHandlerGlobal.setVal("setTime", currentHour + "_" + currentMinute);
            }
        }
        
        private void DailyReportPrintPage(object sender, PrintPageEventArgs e)
        {
            DateTime now = DateTime.Now;
            float currentY = 40;// declare  one variable for height measurement
            RectangleF rect1 = new RectangleF(30, currentY, constants.dailyReportPrintPaperWidth, 30);
            StringFormat format1 = new StringFormat();
            format1.Alignment = StringAlignment.Center;
            e.Graphics.DrawString(constants.dailyReportTitle + " " + sumDate, new Font("Seri", constants.fontSizeBig, FontStyle.Bold), Brushes.Black, rect1, format1);//this will print one heading/title in every page of the document
            currentY += 40;
            int soldPriceSum = 0;
            int soldAmountSum = 0;
            int k = 0;

            if (sqlite_conn.State == ConnectionState.Closed)
            {
                sqlite_conn.Open();
            }
            SQLiteCommand sqlite_cmd;
            SQLiteDataReader sqlite_datareader;

            string week = now.ToString("ddd");
            string currentTime = now.ToString("HH:mm");

            sqlite_cmd = sqlite_conn.CreateCommand();
            string daySumqurey = "SELECT * FROM " + constants.tbNames[7] + " WHERE sumDate=@sumDate";
            sqlite_cmd.CommandText = daySumqurey;
            sqlite_cmd.Parameters.AddWithValue("@sumDate", sumDate);
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                if (!sqlite_datareader.IsDBNull(0))
                {
                    if(lineNums < k)
                    {
                        string prdName = sqlite_datareader.GetString(2);
                        int prdPrice = sqlite_datareader.GetInt32(3);
                        int prdAmount = sqlite_datareader.GetInt32(4);
                        int prdTotalPrice = sqlite_datareader.GetInt32(5);
                        soldPriceSum += prdTotalPrice;
                        soldAmountSum += prdAmount;

                        RectangleF rect2 = new RectangleF(constants.dailyReportPrintPaperWidth / 10, currentY, constants.dailyReportPrintPaperWidth / 5, 30);
                        StringFormat format2 = new StringFormat();
                        format2.Alignment = StringAlignment.Near;
                        e.Graphics.DrawString(prdName, DefaultFont, Brushes.Black, rect2, format2);//print each item

                        RectangleF rect3 = new RectangleF(constants.dailyReportPrintPaperWidth * 3 /10, currentY, constants.dailyReportPrintPaperWidth / 5, 30);
                        StringFormat format3 = new StringFormat();
                        format3.Alignment = StringAlignment.Center;
                        e.Graphics.DrawString(prdAmount.ToString() + constants.amountUnit, DefaultFont, Brushes.Black, rect3, format3);//print each item

                        RectangleF rect4 = new RectangleF(constants.dailyReportPrintPaperWidth / 2, currentY, constants.singleticketPrintPaperWidth / 5, 30);
                        StringFormat format4 = new StringFormat();
                        format4.Alignment = StringAlignment.Center;
                        e.Graphics.DrawString(prdPrice.ToString() + constants.unit, DefaultFont, Brushes.Black, rect4, format4);//print each item

                        RectangleF rect5 = new RectangleF(constants.dailyReportPrintPaperWidth * 7 / 10, currentY, constants.dailyReportPrintPaperWidth / 5, 30);
                        StringFormat format5 = new StringFormat();
                        format5.Alignment = StringAlignment.Far;
                        e.Graphics.DrawString(prdTotalPrice.ToString() + constants.unit, DefaultFont, Brushes.Black, rect5, format5);//print each item
                        currentY += 30;

                        if (itemperpages < 26) // check whether  the number of item(per page) is more than 20 or not
                        {
                            itemperpages += 1; // increment itemperpage by 1
                            e.HasMorePages = false; // set the HasMorePages property to false , so that no other page will not be added
                        }

                        else // if the number of item(per page) is more than 20 then add one page
                        {
                            itemperpages = 0; //initiate itemperpage to 0 .
                            e.HasMorePages = true; //e.HasMorePages raised the PrintPage event once per page .
                            lineNums = k;
                            return;//It will call PrintPage event again

                        }
                    }
                    k++;

                }
            }
            RectangleF rect6 = new RectangleF(0, currentY, constants.dailyReportPrintPaperWidth * 3 / 7, 30);
            StringFormat format6 = new StringFormat();
            format6.Alignment = StringAlignment.Far;
            e.Graphics.DrawString("合計: " + soldAmountSum + constants.amountUnit, new Font("Seri", constants.fontSizeMedium, FontStyle.Bold), Brushes.Black, rect6, format6);//print each item

            RectangleF rect7 = new RectangleF(constants.dailyReportPrintPaperWidth * 4 / 7, currentY, constants.dailyReportPrintPaperWidth * 3 / 7, 30);
            StringFormat format7 = new StringFormat();
            format7.Alignment = StringAlignment.Near;
            e.Graphics.DrawString(soldPriceSum + " " + constants.unit, new Font("Seri", constants.fontSizeMedium, FontStyle.Bold), Brushes.Black, rect7, format7);//print each item
            currentY += 30;

            RectangleF rect8 = new RectangleF(constants.dailyReportPrintPaperWidth * 1 / 10, currentY, constants.dailyReportPrintPaperWidth * 9 / 10, 30);
            StringFormat format8 = new StringFormat();
            format8.Alignment = StringAlignment.Near;
            e.Graphics.DrawString(now.ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss"), new Font("Seri", constants.fontSizeMedium, FontStyle.Bold), Brushes.Black, rect8, format8);//print each item
            currentY += 30;
            return;

        }
        private void ReceiptIssueReportPrintPage(object sender, PrintPageEventArgs e)
        {
            DateTime now = DateTime.Now;
            float currentY = 40;// declare  one variable for height measurement
            RectangleF rect1 = new RectangleF(0, currentY, constants.receiptReportPrintPaperWidth, 30);
            StringFormat format1 = new StringFormat();
            format1.Alignment = StringAlignment.Center;
            e.Graphics.DrawString(constants.receiptionTitle + "  " + sumDate, new Font("Seri", constants.fontSizeBig, FontStyle.Bold), Brushes.Black, rect1, format1);//this will print one heading/title in every page of the document
            currentY += 40;

            RectangleF rect2 = new RectangleF(30, currentY, (constants.receiptReportPrintPaperWidth - 60) * 3 / 5, 30);
            StringFormat format2 = new StringFormat();
            format2.Alignment = StringAlignment.Near;
            e.Graphics.DrawString(constants.receiptionField, new Font("Seri", 12, FontStyle.Bold), Brushes.Black, rect2, format2);//this will print one heading/title in every page of the document
            RectangleF rect3 = new RectangleF(30 + (constants.receiptReportPrintPaperWidth - 60) * 4 / 5, currentY, (constants.receiptReportPrintPaperWidth - 60) * 1 / 5, 30);
            StringFormat format3 = new StringFormat();
            format3.Alignment = StringAlignment.Far;
            e.Graphics.DrawString(constants.priceField, new Font("Seri", 12, FontStyle.Bold), Brushes.Black, rect3, format3);//this will print one heading/title in
            currentY += 35;
            int k = 0;
            if (sqlite_conn.State == ConnectionState.Closed)
            {
                sqlite_conn.Open();
            }
            SQLiteCommand sqlite_cmd;
            SQLiteDataReader sqlite_datareader;

            string week = now.ToString("ddd");
            string currentTime = now.ToString("HH:mm");

            sqlite_cmd = sqlite_conn.CreateCommand();
            string daySumqurey = "SELECT * FROM " + constants.tbNames[8] + " WHERE ReceiptDate>=@receiptDate1 and ReceiptDate<=@receiptDate2";
            sqlite_cmd.CommandText = daySumqurey;
            sqlite_cmd.Parameters.AddWithValue("@receiptDate1", sumDayTime1);
            sqlite_cmd.Parameters.AddWithValue("@receiptDate2", sumDayTime2);
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                if (!sqlite_datareader.IsDBNull(0))
                {
                    if(lineNums <= k)
                    {
                        int purchasePoint = sqlite_datareader.GetInt32(1);
                        int totalPrice = sqlite_datareader.GetInt32(2);
                        DateTime receiptDate = sqlite_datareader.GetDateTime(3);

                        RectangleF rect4 = new RectangleF(30, currentY, (constants.receiptReportPrintPaperWidth - 60) * 3 / 5, 30);
                        StringFormat format4 = new StringFormat();
                        format4.Alignment = StringAlignment.Near;
                        e.Graphics.DrawString(receiptDate.ToString("yyyy/MM/dd HH:mm:ss"), DefaultFont, Brushes.Black, rect4, format4);//print each item

                        RectangleF rect5 = new RectangleF(30 + (constants.receiptReportPrintPaperWidth - 60) * 3 / 5, currentY, (constants.receiptReportPrintPaperWidth - 60) / 5, 30);
                        StringFormat format5 = new StringFormat();
                        format5.Alignment = StringAlignment.Center;
                        e.Graphics.DrawString(purchasePoint.ToString() + constants.amountUnit, DefaultFont, Brushes.Black, rect5, format5);//print each item

                        RectangleF rect6 = new RectangleF(30 + (constants.receiptReportPrintPaperWidth - 60) * 4 / 5, currentY, (constants.receiptReportPrintPaperWidth - 60) / 5, 30);
                        StringFormat format6 = new StringFormat();
                        format6.Alignment = StringAlignment.Far;
                        e.Graphics.DrawString(totalPrice.ToString() + constants.unit, DefaultFont, Brushes.Black, rect6, format6);//print each item
                        currentY += 30;
                        if (itemperpages < 26) // check whether  the number of item(per page) is more than 20 or not
                        {
                            itemperpages += 1; // increment itemperpage by 1
                            e.HasMorePages = false; // set the HasMorePages property to false , so that no other page will not be added
                        }

                        else // if the number of item(per page) is more than 20 then add one page
                        {
                            itemperpages = 0; //initiate itemperpage to 0 .
                            e.HasMorePages = true; //e.HasMorePages raised the PrintPage event once per page .
                            lineNums = k + 1;
                            return;//It will call PrintPage event again

                        }

                    }
                    k++;
                }
            }

            RectangleF rect7 = new RectangleF(0, currentY, constants.receiptReportPrintPaperWidth, 30);
            StringFormat format7 = new StringFormat();
            format7.Alignment = StringAlignment.Center;
            e.Graphics.DrawString("合計: " + k + constants.amountUnit, new Font("Seri", constants.fontSizeMedium, FontStyle.Bold), Brushes.Black, rect7, format7);//print each item
            currentY += 30;

            RectangleF rect8 = new RectangleF(30 , currentY, constants.receiptReportPrintPaperWidth - 30, 30);
            StringFormat format8 = new StringFormat();
            format8.Alignment = StringAlignment.Near;
            e.Graphics.DrawString(now.ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss"), new Font("Seri", constants.fontSizeMedium, FontStyle.Bold), Brushes.Black, rect8, format8);//print each item
            currentY += 30;
        }

        public void setVal(string dropdownHandler, string sendVal)
        {
            logTimeGlobal = sendVal;
            if(dropdownHandler == "falsePurchase")
            {
                tbodyPanelGlobal.Controls.Clear();
                ShowProdListForFalsePurchaseCancell(tbodyPanelGlobal);
            }
            else if(dropdownHandler == "logReport")
            {
                dialogPanelGlobal.Controls.Clear();
                this.logReportBody(logTimeGlobal, dialogPanelGlobal);
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

            }
            return sqlite_conn;
        }

    }

}

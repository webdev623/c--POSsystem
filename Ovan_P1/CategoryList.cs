using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ovan_P1
{
    public partial class CategoryList : Form
    {
        int height = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height;
        int width = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width;
        Form1 mainFormGlobal = null;
        Panel mainPanelGlobal = null;

        //private Button buttonGlobal = null;
        private FlowLayoutPanel[] menuFlowLayoutPanelGlobal = new FlowLayoutPanel[4];
        Constant constants = new Constant();

        CreatePanel createPanel = new CreatePanel();
        CreateLabel createLabel = new CreateLabel();
        CreateCombobox createCombobox = new CreateCombobox();
        CustomButton customButton = new CustomButton();
        DropDownMenu dropDownMenu = new DropDownMenu();
        MessageDialog messageDialog = new MessageDialog();
        Panel detailPanelGlobal = null;
        Panel tBodyPanelGlobal = null;
        DetailView detailView = new DetailView();

        PaperSize paperSize = new PaperSize("papersize", 500, 800);//set the paper size
        PrintDocument printDocument1 = new PrintDocument();
        PrintPreviewDialog printPreviewDialog1 = new PrintPreviewDialog();
        PrintDialog printDialog1 = new PrintDialog();
        int totalNumber = 0;
        int lineNum = 0;
        int lineNums = 0;
        int itemperpage = 0;
        int itemperpages = 0;

        string storeName = "";

        List<int> soldoutCategoryIndex = new List<int>();
        string[] categoryNameList = null;
        int[] categoryIDList = null;
        int[] categoryDisplayPositionList = null;
        int[] categorySoldStateList = null;
        int[] categoryLayoutList = null;
        string[] categoryOpenTimeList = null;
        List<int> stopedPrdIDArray = null;
        int categoryRowCount = 0;
        int selectedCategoryIndex = 0;

        SQLiteConnection sqlite_conn;
        DateTime now = DateTime.Now;

        public CategoryList(Form1 mainForm, Panel mainPanel)
        {
            InitializeComponent();
            dropDownMenu.initCategoryList(this);
            messageDialog.initCategoryList(this);
            mainFormGlobal = mainForm;
            sqlite_conn = CreateConnection(constants.dbName);
            this.GetCategoryList();

            if (sqlite_conn.State == ConnectionState.Closed)
            {
                sqlite_conn.Open();
            }

            SQLiteCommand sqlite_cmd;
            string week = now.ToString("ddd");
            string currentTime = now.ToString("HH:mm");

            try
            {
                string productQuery = "SELECT count(*) FROM " + constants.tbNames[0] + "";
                sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = productQuery;
                totalNumber = Convert.ToInt32(sqlite_cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            try
            {
                string productQuery1 = "SELECT count(*) FROM " + constants.tbNames[2] + "";
                sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = productQuery1;
                totalNumber += Convert.ToInt32(sqlite_cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            SQLiteDataReader sqlite_datareader;


            string storeEndqurey = "SELECT * FROM " + constants.tbNames[6];
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = storeEndqurey;
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                if (!sqlite_datareader.IsDBNull(0))
                {
                    storeName = sqlite_datareader.GetString(1);

                }
            }


            Panel mainPanels = createPanel.CreateMainPanel(mainForm, 0, 0, mainForm.Width, mainForm.Height, BorderStyle.None, Color.Transparent);
            mainPanelGlobal = mainPanels;
            Label categoryLabel = createLabel.CreateLabelsInPanel(mainPanels, "categoryLabel", constants.categoryListTitleLabel, 0, 50, mainPanels.Width / 2, 50, Color.Transparent, Color.Black, 22, false, ContentAlignment.MiddleRight);

            dropDownMenu.CreateCategoryDropDown1("categoryList", mainPanels, categoryNameList, categoryIDList, categoryDisplayPositionList, categorySoldStateList, mainPanels.Width / 2, 50, 200, 50, 200, 50 * (categoryRowCount + 1), 200, 50, Color.Red, Color.Yellow);

            Label categoryTimeLabel = createLabel.CreateLabelsInPanel(mainPanels, "categoryTimeLabel", constants.TimeLabel + " : ", 0, 130, mainPanels.Width / 2, 50, Color.Transparent, Color.Black, 22, false, ContentAlignment.BottomRight);

            Button previewButton = customButton.CreateButton(constants.prevButtonLabel, "previewButton", mainPanelGlobal.Width - 250, mainPanels.Height * 3 / 5 + 30, 200, 50, Color.FromArgb(255, 0, 176, 80), Color.Transparent, 0, 1, 18);
            mainPanels.Controls.Add(previewButton);
            previewButton.Click += new EventHandler(this.PreviewSalePage);

            Button printButton = customButton.CreateButton(constants.printButtonLabel, "categoryPrintButton", mainPanelGlobal.Width - 250, mainPanels.Height * 3 / 5 + 110, 200, 50, Color.FromArgb(255, 0, 176, 240), Color.Transparent, 0, 1, 18);
            mainPanels.Controls.Add(printButton);
            printButton.Click += new EventHandler(messageDialog.MessageDialogInit);

            Button closeButton = customButton.CreateButton(constants.backText, "closeButton", mainPanelGlobal.Width - 250, mainPanels.Height * 3 / 5 + 190, 200, 50, Color.FromArgb(255, 0, 112, 192), Color.Transparent, 0, 1, 18);
            mainPanels.Controls.Add(closeButton);
            closeButton.Click += new EventHandler(this.BackShow);

            Panel detailPanel = createPanel.CreateSubPanel(mainPanels, 50, 200, mainPanelGlobal.Width * 5 / 7, mainPanelGlobal.Height - 250, BorderStyle.None, Color.Transparent);
            detailPanelGlobal = detailPanel;

            FlowLayoutPanel tableHeaderInUpPanel = createPanel.CreateFlowLayoutPanel(detailPanel, 0, 0, detailPanel.Width, 50, Color.Transparent, new Padding(0));
            Label tableHeaderLabel1 = createLabel.CreateLabels(tableHeaderInUpPanel, "tableHeaderLabel1", constants.printProductNameField, 0, 0, tableHeaderInUpPanel.Width / 2, 50, Color.FromArgb(255, 147, 184, 219), Color.Black, 16, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
            Label tableHeaderLabel2 = createLabel.CreateLabels(tableHeaderInUpPanel, "tableHeaderLabel2", constants.salePriceField, tableHeaderLabel1.Right, 0, tableHeaderInUpPanel.Width / 4, 50, Color.FromArgb(255, 147, 184, 219), Color.Black, 16, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
            Label tableHeaderLabel3 = createLabel.CreateLabels(tableHeaderInUpPanel, "tableHeaderLabel3", constants.saleLimitField, tableHeaderLabel2.Right, 0, tableHeaderInUpPanel.Width / 4, 50, Color.FromArgb(255, 147, 184, 219), Color.Black, 16, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);

            Panel tBodyPanel = createPanel.CreateSubPanel(detailPanel, 0, 50, detailPanel.Width, detailPanel.Height - 50, BorderStyle.FixedSingle, Color.White);
            tBodyPanelGlobal = tBodyPanel;

            printDocument1.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
            printDocument1.EndPrint += new PrintEventHandler(PrintEnd);
            ShowCategoryDetail();
        }

        private void PrintEnd(object sender, PrintEventArgs e)
        {
            //lineNum = -1;
            //groupNumber = 0;
            //itemperpage = 0;
            //lineNums = -1;
            //groupNumbers = 0;
            //itemperpages = 0;

        }
        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            PrintDocument printDocument = (PrintDocument)sender;
            if (printDocument.PrintController.IsPreview == false)
            {
                float currentY = 30;
                RectangleF rect1 = new RectangleF(30, currentY, constants.categorylistPrintPaperWidth, 30);
                StringFormat format1 = new StringFormat();
                format1.Alignment = StringAlignment.Center;
                e.Graphics.DrawString(constants.categoryListPrintTitle, new Font("Seri", constants.fontSizeBig, FontStyle.Bold), Brushes.Black, rect1, format1);
                currentY += 25;

                RectangleF rect2 = new RectangleF(30, currentY, constants.categorylistPrintPaperWidth, 30);
                StringFormat format2 = new StringFormat();
                format2.Alignment = StringAlignment.Center;
                e.Graphics.DrawString(now.ToString("yyyy/MM/dd HH:mm:ss"), new Font("Seri", constants.fontSizeMedium, FontStyle.Bold), Brushes.Black, rect2, format2);
                currentY += 25;

                RectangleF rect3 = new RectangleF(30, currentY, constants.categorylistPrintPaperWidth, 30);
                StringFormat format3 = new StringFormat();
                format3.Alignment = StringAlignment.Center;
                e.Graphics.DrawString(storeName, new Font("Seri", constants.fontSizeMedium, FontStyle.Bold), Brushes.Black, rect3, format3);
                currentY += 35;
                int j = 0;
                int k = 0;
                foreach (string categoryName in categoryNameList)
                {
                    if (lineNums <= j)
                    {
                        RectangleF rect4 = new RectangleF(30, currentY + 5, constants.categorylistPrintPaperWidth, 30);
                        StringFormat format4 = new StringFormat();
                        format4.Alignment = StringAlignment.Near;
                        e.Graphics.DrawString(constants.categoryDiplayLabel + categoryDisplayPositionList[k] + "/" + constants.categoryLabel + categoryIDList[k] + "   " + categoryName, new Font("Seri", constants.fontSizeMedium, FontStyle.Bold), Brushes.Black, rect4, format4);

                        currentY += 30;

                        RectangleF rect5 = new RectangleF(30, currentY, constants.categorylistPrintPaperWidth, 30);
                        StringFormat format5 = new StringFormat();
                        format5.Alignment = StringAlignment.Near;
                        e.Graphics.DrawString(constants.SaleTimeLabel + ": " + categoryOpenTimeList[k], new Font("Seri", constants.fontSizeMedium, FontStyle.Bold), Brushes.Black, rect5, format5);

                        currentY += 35;
                    }

                    SQLiteDataReader sqlite_datareader;
                    SQLiteCommand sqlite_cmd;
                    sqlite_cmd = sqlite_conn.CreateCommand();
                    string queryCmd0 = "SELECT * FROM " + constants.tbNames[2] + " WHERE CategoryID=@categoryID ORDER BY CardNumber";
                    sqlite_cmd.CommandText = queryCmd0;
                    sqlite_cmd.Parameters.AddWithValue("@categoryID", categoryIDList[k]);
                    sqlite_datareader = sqlite_cmd.ExecuteReader();
                    int m = 0;
                    while (sqlite_datareader.Read())
                    {
                        if (!sqlite_datareader.IsDBNull(0))
                        {
                            string prdName = sqlite_datareader.GetString(3);
                            int prdPrice = sqlite_datareader.GetInt32(8);
                            int prdLimitedCnt = sqlite_datareader.GetInt32(9);

                            if(lineNums <= j)
                            {
                                RectangleF rect6 = new RectangleF(30, currentY, (constants.categorylistPrintPaperWidth - 60) * 3 / 5, 20);
                                StringFormat format6 = new StringFormat();
                                format6.Alignment = StringAlignment.Near;
                                e.Graphics.DrawString(prdName, new Font("Seri", constants.fontSizeMedium, FontStyle.Bold), Brushes.Black, rect6, format6);

                                RectangleF rect7 = new RectangleF(30 + (constants.categorylistPrintPaperWidth - 60) * 3 / 5, currentY, (constants.categorylistPrintPaperWidth - 60) / 5, 20);
                                StringFormat format7 = new StringFormat();
                                format7.Alignment = StringAlignment.Center;
                                e.Graphics.DrawString(prdPrice.ToString(), new Font("Seri", constants.fontSizeMedium, FontStyle.Bold), Brushes.Black, rect7, format7);

                                RectangleF rect8 = new RectangleF(30 + (constants.categorylistPrintPaperWidth - 60) * 4 / 5, currentY, (constants.categorylistPrintPaperWidth - 60) / 5, 20);
                                StringFormat format8 = new StringFormat();
                                format8.Alignment = StringAlignment.Far;
                                e.Graphics.DrawString(prdLimitedCnt.ToString(), new Font("Seri", constants.fontSizeMedium, FontStyle.Bold), Brushes.Black, rect8, format8);
                                currentY += 25;
                                if (itemperpages < 21) // check whether  the number of item(per page) is more than 20 or not
                                {
                                    itemperpages += 1; // increment itemperpage by 1
                                    e.HasMorePages = false; // set the HasMorePages property to false , so that no other page will not be added
                                }

                                else // if the number of item(per page) is more than 20 then add one page
                                {
                                    itemperpages = 0; //initiate itemperpage to 0 .
                                    e.HasMorePages = true; //e.HasMorePages raised the PrintPage event once per page .
                                    lineNums = j + 1;
                                    return;//It will call PrintPage event again

                                }
                            }
                            j++;
                            m++;
                        }
                    }
                    k++;
                }
            }
            else
            {
                float currentY = 30;
                RectangleF rect1 = new RectangleF(30, currentY, constants.categorylistPrintPaperWidth, 30);
                StringFormat format1 = new StringFormat();
                format1.Alignment = StringAlignment.Center;                
                e.Graphics.DrawString(constants.categoryListPrintTitle, new Font("Seri", constants.fontSizeBig, FontStyle.Bold), Brushes.Black, rect1, format1);
                currentY += 25;

                RectangleF rect2 = new RectangleF(30, currentY, constants.categorylistPrintPaperWidth, 30);
                StringFormat format2 = new StringFormat();
                format2.Alignment = StringAlignment.Center;
                e.Graphics.DrawString(now.ToString("yyyy/MM/dd HH:mm:ss"), new Font("Seri", constants.fontSizeMedium, FontStyle.Bold), Brushes.Black, rect2, format2);
                currentY += 25;

                RectangleF rect3 = new RectangleF(30, currentY, constants.categorylistPrintPaperWidth, 30);
                StringFormat format3 = new StringFormat();
                format3.Alignment = StringAlignment.Center;
                e.Graphics.DrawString(storeName, new Font("Seri", constants.fontSizeMedium, FontStyle.Bold), Brushes.Black, rect3, format3);
                currentY += 35;

                int j = 0;
                int k = 0;
                foreach (string categoryName in categoryNameList)
                {
                    if (lineNum <= j)
                    {
                        RectangleF rect4 = new RectangleF(30, currentY + 5, constants.categorylistPrintPaperWidth, 30);
                        StringFormat format4 = new StringFormat();
                        format4.Alignment = StringAlignment.Near;
                        e.Graphics.DrawString(constants.categoryDiplayLabel + categoryDisplayPositionList[k] + "/" + constants.categoryLabel + categoryIDList[k] + "   " + categoryName, new Font("Seri", constants.fontSizeMedium, FontStyle.Bold), Brushes.Black, rect4, format4);

                        currentY += 30;

                        RectangleF rect5 = new RectangleF(30, currentY, constants.categorylistPrintPaperWidth, 30);
                        StringFormat format5 = new StringFormat();
                        format5.Alignment = StringAlignment.Near;
                        e.Graphics.DrawString(constants.SaleTimeLabel + ": " + categoryOpenTimeList[k], new Font("Seri", constants.fontSizeMedium, FontStyle.Bold), Brushes.Black, rect5, format5);

                        currentY += 35;
                    }
                    j += 2;

                    SQLiteDataReader sqlite_datareader;
                    SQLiteCommand sqlite_cmd;
                    sqlite_cmd = sqlite_conn.CreateCommand();
                    string queryCmd0 = "SELECT * FROM " + constants.tbNames[2] + " WHERE CategoryID=@categoryID ORDER BY CardNumber";
                    sqlite_cmd.CommandText = queryCmd0;
                    sqlite_cmd.Parameters.AddWithValue("@categoryID", categoryIDList[k]);
                    sqlite_datareader = sqlite_cmd.ExecuteReader();
                    int m = 0;
                    while (sqlite_datareader.Read())
                    {
                        if (!sqlite_datareader.IsDBNull(0))
                        {
                            string prdName = sqlite_datareader.GetString(3);
                            int prdPrice = sqlite_datareader.GetInt32(8);
                            int prdLimitedCnt = sqlite_datareader.GetInt32(9);

                            if (lineNum <= j)
                            {
                                RectangleF rect6 = new RectangleF(30, currentY, (constants.categorylistPrintPaperWidth - 60) * 3 / 5, 20);
                                StringFormat format6 = new StringFormat();
                                format6.Alignment = StringAlignment.Near;
                                e.Graphics.DrawString(prdName, new Font("Seri", constants.fontSizeMedium, FontStyle.Bold), Brushes.Black, rect6, format6);

                                RectangleF rect7 = new RectangleF(30 + (constants.categorylistPrintPaperWidth - 60) * 3 / 5, currentY, (constants.categorylistPrintPaperWidth - 60) / 5, 20);
                                StringFormat format7 = new StringFormat();
                                format7.Alignment = StringAlignment.Center;
                                e.Graphics.DrawString(prdPrice.ToString(), new Font("Seri", constants.fontSizeMedium, FontStyle.Bold), Brushes.Black, rect7, format7);

                                RectangleF rect8 = new RectangleF(30 + (constants.categorylistPrintPaperWidth - 60) * 4 / 5, currentY, (constants.categorylistPrintPaperWidth - 60) / 5, 20);
                                StringFormat format8 = new StringFormat();
                                format8.Alignment = StringAlignment.Far;
                                e.Graphics.DrawString(prdLimitedCnt.ToString(), new Font("Seri", constants.fontSizeMedium, FontStyle.Bold), Brushes.Black, rect8, format8);
                                currentY += 25;
                                if (itemperpage < 21) // check whether  the number of item(per page) is more than 20 or not
                                {
                                    itemperpage += 1; // increment itemperpage by 1
                                    e.HasMorePages = false; // set the HasMorePages property to false , so that no other page will not be added
                                }

                                else // if the number of item(per page) is more than 20 then add one page
                                {
                                    itemperpage = 0; //initiate itemperpage to 0 .
                                    e.HasMorePages = true; //e.HasMorePages raised the PrintPage event once per page .
                                    lineNums = j + 1;
                                    return;//It will call PrintPage event again

                                }
                            }
                            j++;
                            m++;
                        }
                    }

                    k++;

                }
            }
        }

        private void ShowCategoryDetail()
        {
            if (sqlite_conn.State == ConnectionState.Closed)
            {
                sqlite_conn.Open();
            }


            Label categoryTimeValue = createLabel.CreateLabelsInPanel(mainPanelGlobal, "categoryTimeValue", categoryOpenTimeList[selectedCategoryIndex], mainPanelGlobal.Width / 2, 130, mainPanelGlobal.Width / 2, 50, Color.Transparent, Color.Black, 22, false, ContentAlignment.BottomLeft);
            categoryTimeValue.Padding = new Padding(10, 0, 0, 0);


            tBodyPanelGlobal.HorizontalScroll.Maximum = 0;
            tBodyPanelGlobal.AutoScroll = false;
            tBodyPanelGlobal.VerticalScroll.Visible = false;
            tBodyPanelGlobal.AutoScroll = true;

            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();
            string queryCmd0 = "SELECT * FROM " + constants.tbNames[2] + " WHERE CategoryID=@categoryID ORDER BY CardNumber";
            sqlite_cmd.CommandText = queryCmd0;
            sqlite_cmd.Parameters.AddWithValue("@categoryID", categoryIDList[selectedCategoryIndex]);
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            stopedPrdIDArray = new List<int>();
            int k = 0;
            while (sqlite_datareader.Read())
            {
                if (!sqlite_datareader.IsDBNull(0))
                {
                    string prdName = sqlite_datareader.GetString(3);
                    int prdPrice = sqlite_datareader.GetInt32(8);
                    int prdLimitedCnt = sqlite_datareader.GetInt32(9);
                    int prdID = sqlite_datareader.GetInt32(2);
                    int rowID = sqlite_datareader.GetInt32(0);
                    int soldFlag = sqlite_datareader.GetInt32(31);
                    string limitedAmount = prdLimitedCnt.ToString();
                    Color fontColor = Color.Black;
                    if(soldFlag == 1)
                    {
                        limitedAmount = constants.saleStopStatusText;
                        fontColor = Color.Red;
                    }

                    FlowLayoutPanel tableRowPanel = createPanel.CreateFlowLayoutPanel(tBodyPanelGlobal, 0, 50 * k, tBodyPanelGlobal.Width, 50, Color.Transparent, new Padding(0));
                    Label tdLabel1 = createLabel.CreateLabels(tableRowPanel, "tdLabel1_" + k, prdName, 0, 0, tableRowPanel.Width / 2, 50, Color.White, fontColor, 16, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
                    Label tdLabel2 = createLabel.CreateLabels(tableRowPanel, "tdLabel2_" + k, prdPrice.ToString(), tdLabel1.Right, 0, tableRowPanel.Width / 4, 50, Color.White, fontColor, 16, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
                    Label tdLabel3 = createLabel.CreateLabels(tableRowPanel, "tdLabel3_" + k, limitedAmount, tdLabel2.Right, 0, tableRowPanel.Width / 4, 50, Color.White, fontColor, 16, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
                    k++;
                }
            }

        }

        public void setVal(string categoryID)
        {
            tBodyPanelGlobal.Controls.Clear();
            selectedCategoryIndex = int.Parse(categoryID);
            ShowCategoryDetail();
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

        public void PreviewSalePage(object sender, EventArgs e)
        {
            mainFormGlobal.Controls.Clear();
            PreviewSalePage frm = new PreviewSalePage(mainFormGlobal, mainPanelGlobal, selectedCategoryIndex, categoryIDList, categoryNameList, categoryDisplayPositionList, categoryLayoutList);
            frm.TopLevel = false;
            mainFormGlobal.Controls.Add(frm);
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;
            Thread.Sleep(200);
            frm.Show();
        }

        public void PrintPreview_click()
        {
            printPreviewDialog1.Document = printDocument1;
            //printDialog1.Document = printDocument1;

            ((ToolStripButton)((ToolStrip)printPreviewDialog1.Controls[1]).Items[0]).Enabled = true;


            printDocument1.DefaultPageSettings.PaperSize = paperSize;
            printPreviewDialog1.ShowDialog();

        }
        private void GetCategoryList()
        {
            DateTime now = DateTime.Now;
            string week = now.ToString("ddd");
            string currentTime = now.ToString("HH:mm");
            if (sqlite_conn.State == ConnectionState.Closed)
            {
                sqlite_conn.Open();
            }
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();
            string queryCmd0 = "SELECT COUNT(id) FROM " + constants.tbNames[0];
            sqlite_cmd.CommandText = queryCmd0;
            categoryRowCount = Convert.ToInt32(sqlite_cmd.ExecuteScalar());
            categoryNameList = new string[categoryRowCount];
            categoryIDList = new int[categoryRowCount];
            categoryDisplayPositionList = new int[categoryRowCount];
            categoryLayoutList = new int[categoryRowCount];
            categorySoldStateList = new int[categoryRowCount];
            categoryOpenTimeList = new string[categoryRowCount];

            string queryCmd = "SELECT * FROM " + constants.tbNames[0] + " ORDER BY id";
            sqlite_cmd.CommandText = queryCmd;

            sqlite_datareader = sqlite_cmd.ExecuteReader();
            int k = 0;
            while (sqlite_datareader.Read())
            {
                if (!sqlite_datareader.IsDBNull(0))
                {
                    categoryIDList[k] = sqlite_datareader.GetInt32(1);
                    categoryNameList[k] = sqlite_datareader.GetString(2);
                    categoryDisplayPositionList[k] = sqlite_datareader.GetInt32(6);
                    categoryLayoutList[k] = sqlite_datareader.GetInt32(7);
                    bool saleFlag = false;
                    string openTime = "";
                    if (week == "Sat")
                    {
                        openTime = sqlite_datareader.GetString(4);
                    }
                    else if (week == "Sun")
                    {
                        openTime = sqlite_datareader.GetString(5);
                    }
                    else
                    {
                        openTime = sqlite_datareader.GetString(3);
                    }
                    categoryOpenTimeList[k] = openTime;
                    string[] openTimeArr = openTime.Split('/');
                    foreach (string openTimeArrItem in openTimeArr)
                    {
                        string[] openTimeSubArr = openTimeArrItem.Split('-');
                        if (String.Compare(openTimeSubArr[0], currentTime) <= 0 && String.Compare(openTimeSubArr[1], currentTime) >= 0)
                        {
                            saleFlag = true;
                            break;
                        }
                    }
                    categorySoldStateList[k] = 1;
                    if (saleFlag == true)
                    {
                        categorySoldStateList[k] = 0;
                    }
                }
                k++;
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

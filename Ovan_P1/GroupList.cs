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
    public partial class GroupList : Form
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
        int groupRowCount = 0;
        int[] groupIDList = null;
        string[] groupNameList = null;
        int itemperpage = 0;//this is for no of item per page 
        int groupIDGlobal = 0;
        int lineNum = 0;
        int lineNums = 0;
        int itemperpages = 0;
        string storeName = "";
        SQLiteConnection sqlite_conn;
        DateTime now = DateTime.Now;

        public GroupList(Form1 mainForm, Panel mainPanel)
        {
            InitializeComponent();
            dropDownMenu.initGroupList(this);
            messageDialog.initGroupList(this);

            mainFormGlobal = mainForm;
            sqlite_conn = CreateConnection(constants.dbName);

            if (sqlite_conn.State == ConnectionState.Closed)
            {
                sqlite_conn.Open();
            }

            SQLiteCommand sqlite_cmd;
            string week = now.ToString("ddd");
            string currentTime = now.ToString("HH:mm");

            try
            {
                string productQuery = "SELECT count(*) FROM " + constants.tbNames[10] + "";
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
                string productQuery1 = "SELECT count(*) FROM " + constants.tbNames[11] + "";
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

            this.GetGroupList();

            Panel mainPanels = createPanel.CreateMainPanel(mainForm, 0, 0, mainForm.Width, mainForm.Height, BorderStyle.None, Color.Transparent);
            mainPanelGlobal = mainPanels;
            Label categoryLabel = createLabel.CreateLabelsInPanel(mainPanels, "groupLabel", constants.groupListTitleLabel, 0, 50, mainPanels.Width / 2, 50, Color.Transparent, Color.Black, 22, false, ContentAlignment.MiddleRight);

            dropDownMenu.CreateDropDown("groupList", mainPanels, groupNameList, mainPanels.Width / 2, 50, 200, 50, 200, 50 * (groupRowCount + 1), 200, 50, Color.Red, Color.Yellow);


            Button printButton = customButton.CreateButton(constants.printButtonLabel, "groupPrintButton", mainPanelGlobal.Width - 250, mainPanels.Height * 3 / 5 + 110, 200, 50, Color.FromArgb(255, 0, 176, 240), Color.Transparent, 0, 1, 18);
            mainPanels.Controls.Add(printButton);
            printButton.Click += new EventHandler(messageDialog.MessageDialogInit);
            //printButton.Click += new EventHandler(this.btnprintpreview_Click);

            Button closeButton = customButton.CreateButton(constants.backText, "closeButton", mainPanelGlobal.Width - 250, mainPanels.Height * 3 / 5 + 190, 200, 50, Color.FromArgb(255, 0, 112, 192), Color.Transparent, 0, 1, 18);
            mainPanels.Controls.Add(closeButton);
            closeButton.Click += new EventHandler(this.BackShow);

            Panel detailPanel = createPanel.CreateSubPanel(mainPanels, 50, 150, mainPanelGlobal.Width * 5 / 7, mainPanelGlobal.Height - 200, BorderStyle.None, Color.Transparent);
            detailPanelGlobal = detailPanel;

            FlowLayoutPanel tableHeaderInUpPanel = createPanel.CreateFlowLayoutPanel(detailPanel, 0, 0, detailPanel.Width, 50, Color.Transparent, new Padding(0));
            Label tableHeaderLabel1 = createLabel.CreateLabels(tableHeaderInUpPanel, "tableHeaderLabel1", constants.printProductNameField, 0, 0, tableHeaderInUpPanel.Width * 2 / 3, 50, Color.FromArgb(255, 147, 184, 219), Color.Black, 16, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
            Label tableHeaderLabel2 = createLabel.CreateLabels(tableHeaderInUpPanel, "tableHeaderLabel2", constants.salePriceField, tableHeaderLabel1.Right, 0, tableHeaderInUpPanel.Width / 3, 50, Color.FromArgb(255, 147, 184, 219), Color.Black, 16, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);

            Panel tBodyPanel = createPanel.CreateSubPanel(detailPanel, 0, 50, detailPanel.Width, detailPanel.Height - 50, BorderStyle.FixedSingle, Color.White);
            tBodyPanelGlobal = tBodyPanel;

            printDocument1.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
            printDocument1.EndPrint += new PrintEventHandler(PrintEnd);

            ShowGroupDetail(0);

        }

        private void GetGroupList()
        {
            if (sqlite_conn.State == ConnectionState.Closed)
            {
                sqlite_conn.Open();
            }
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();
            string queryCmd0 = "SELECT COUNT(GroupID) FROM " + constants.tbNames[10];
            sqlite_cmd.CommandText = queryCmd0;
            groupRowCount = Convert.ToInt32(sqlite_cmd.ExecuteScalar());
            groupNameList = new string[groupRowCount];
            groupIDList = new int[groupRowCount];

            string queryCmd = "SELECT * FROM " + constants.tbNames[10] + " ORDER BY GroupID";
            sqlite_cmd.CommandText = queryCmd;

            sqlite_datareader = sqlite_cmd.ExecuteReader();
            int k = 0;
            while (sqlite_datareader.Read())
            {
                if (!sqlite_datareader.IsDBNull(0))
                {
                    groupIDList[k] = sqlite_datareader.GetInt32(0);
                    groupNameList[k] = sqlite_datareader.GetString(1);
                    k++;
                }
            }

        }

        private void PrintEnd(object sender, PrintEventArgs e)
        {
            lineNum = 0;
            itemperpage = 0;
            lineNums = 0;
            itemperpages = 0;
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            PrintDocument printDocument = (PrintDocument)sender;
            if(printDocument.PrintController.IsPreview == false)
            {
                float currentY = 30;// declare  one variable for height measurement
                RectangleF rect1 = new RectangleF(30, currentY, constants.grouplistPrintPaperWidth, 30);
                StringFormat format1 = new StringFormat();
                format1.Alignment = StringAlignment.Center;
                e.Graphics.DrawString(constants.groupListPrintTitle, new Font("Seri", constants.fontSizeBig, FontStyle.Bold), Brushes.Black, rect1, format1);//this will print one heading/title in every page of the document
                currentY += 30;

                RectangleF rect2 = new RectangleF(30, currentY, constants.grouplistPrintPaperWidth, 30);
                StringFormat format2 = new StringFormat();
                format2.Alignment = StringAlignment.Center;
                e.Graphics.DrawString(now.ToString("yyyy/MM/dd HH:mm:ss"), new Font("Seri", constants.fontSizeMedium, FontStyle.Bold), Brushes.Black, rect2, format2);//this will print one heading/title in every page of the document
                currentY += 30;

                RectangleF rect3 = new RectangleF(30, currentY, constants.grouplistPrintPaperWidth, 30);
                StringFormat format3 = new StringFormat();
                format3.Alignment = StringAlignment.Center;
                e.Graphics.DrawString(storeName, new Font("Seri", constants.fontSizeMedium, FontStyle.Bold), Brushes.Black, rect3, format3);//this will print one heading/title in every page of the document
                currentY += 35;
                if (sqlite_conn.State == ConnectionState.Closed)
                {
                    sqlite_conn.Open();
                }
                SQLiteDataReader sqlite_datareader;
                SQLiteCommand sqlite_cmd;
                int j = 0;
                int k = 0;
                foreach (string groupName in groupNameList)
                {
                    if (lineNums <= j)
                    {
                        RectangleF rect4 = new RectangleF(30, currentY, (constants.grouplistPrintPaperWidth - 60) / 2, 25);
                        StringFormat format4 = new StringFormat();
                        format4.Alignment = StringAlignment.Near;
                        e.Graphics.DrawString(constants.groupTitleLabel + groupIDList[k].ToString("00") + ": " + groupName, DefaultFont, Brushes.Black, 50, currentY + 10);
                        currentY += 30;
                    }
                    j++;

                    sqlite_cmd = sqlite_conn.CreateCommand();
                    string queryCmd = "SELECT * FROM " + constants.tbNames[11] + " WHERE GroupID=@groupID";
                    sqlite_cmd.CommandText = queryCmd;
                    sqlite_cmd.Parameters.AddWithValue("@groupID", groupIDList[k]);
                    sqlite_datareader = sqlite_cmd.ExecuteReader();
                    int m = 0;
                    while (sqlite_datareader.Read())
                    {
                        if (!sqlite_datareader.IsDBNull(0))
                        {
                            string prdName = sqlite_datareader.GetString(1);
                            int prdPrice = sqlite_datareader.GetInt32(2);
                            if (lineNums <= j)
                            {
                                RectangleF rect5 = new RectangleF(30, currentY, (constants.grouplistPrintPaperWidth - 60) / 2, 25);
                                StringFormat format5 = new StringFormat();
                                format5.Alignment = StringAlignment.Near;
                                e.Graphics.DrawString(prdName, DefaultFont, Brushes.Black, rect5, format5);//print each item

                                RectangleF rect6 = new RectangleF(30 + (constants.grouplistPrintPaperWidth - 60) / 2, currentY, (constants.grouplistPrintPaperWidth - 60) / 2, 25);
                                StringFormat format6 = new StringFormat();
                                format6.Alignment = StringAlignment.Near;
                                e.Graphics.DrawString(prdPrice.ToString() + constants.unit, DefaultFont, Brushes.Black, rect6, format6);

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

                float currentY = 30;// declare  one variable for height measurement
                RectangleF rect1 = new RectangleF(30, currentY, constants.grouplistPrintPaperWidth, 30);
                StringFormat format1 = new StringFormat();
                format1.Alignment = StringAlignment.Center;
                e.Graphics.DrawString(constants.groupListPrintTitle, new Font("Seri", constants.fontSizeBig, FontStyle.Bold), Brushes.Black, rect1, format1);//this will print one heading/title in every page of the document
                currentY += 30;

                RectangleF rect2 = new RectangleF(30, currentY, constants.grouplistPrintPaperWidth, 30);
                StringFormat format2 = new StringFormat();
                format2.Alignment = StringAlignment.Center;
                e.Graphics.DrawString(now.ToString("yyyy/MM/dd HH:mm:ss"), new Font("Seri", constants.fontSizeMedium, FontStyle.Bold), Brushes.Black, rect2, format2);//this will print one heading/title in every page of the document
                currentY += 30;

                RectangleF rect3 = new RectangleF(30, currentY, constants.grouplistPrintPaperWidth, 30);
                StringFormat format3 = new StringFormat();
                format3.Alignment = StringAlignment.Center;
                e.Graphics.DrawString(storeName, new Font("Seri", constants.fontSizeMedium, FontStyle.Bold), Brushes.Black, rect3, format3);//this will print one heading/title in every page of the document
                currentY += 35;
                if (sqlite_conn.State == ConnectionState.Closed)
                {
                    sqlite_conn.Open();
                }
                SQLiteDataReader sqlite_datareader;
                SQLiteCommand sqlite_cmd;
                int j = 0;
                int k = 0;
                foreach(string groupName in groupNameList)
                {
                    if (lineNum <= j)
                    {
                        RectangleF rect4 = new RectangleF(30, currentY, (constants.grouplistPrintPaperWidth - 60) / 2, 25);
                        StringFormat format4 = new StringFormat();
                        format4.Alignment = StringAlignment.Near;
                        e.Graphics.DrawString(constants.groupTitleLabel + groupIDList[k].ToString("00") + ": " + groupName, DefaultFont, Brushes.Black, 50, currentY + 10);
                        currentY += 30;
                    }
                    j++;

                    sqlite_cmd = sqlite_conn.CreateCommand();
                    string queryCmd = "SELECT * FROM " + constants.tbNames[11] + " WHERE GroupID=@groupID";
                    sqlite_cmd.CommandText = queryCmd;
                    sqlite_cmd.Parameters.AddWithValue("@groupID", groupIDList[k]);
                    sqlite_datareader = sqlite_cmd.ExecuteReader();
                    int m = 0;
                    while (sqlite_datareader.Read())
                    {
                        if (!sqlite_datareader.IsDBNull(0))
                        {
                            string prdName = sqlite_datareader.GetString(1);
                            int prdPrice = sqlite_datareader.GetInt32(2);
                            if(lineNum <= j)
                            {
                                RectangleF rect5 = new RectangleF(30, currentY, (constants.grouplistPrintPaperWidth - 60) / 2, 25);
                                StringFormat format5 = new StringFormat();
                                format5.Alignment = StringAlignment.Near;
                                e.Graphics.DrawString(prdName, DefaultFont, Brushes.Black, rect5, format5);//print each item

                                RectangleF rect6 = new RectangleF(30 + (constants.grouplistPrintPaperWidth - 60) / 2, currentY, (constants.grouplistPrintPaperWidth - 60) / 2, 25);
                                StringFormat format6 = new StringFormat();
                                format6.Alignment = StringAlignment.Near;
                                e.Graphics.DrawString(prdPrice.ToString() + constants.unit, DefaultFont, Brushes.Black, rect6, format6);

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
                                    lineNum = j + 1;
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


        private void ShowGroupDetail(int groupID)
        {

            tBodyPanelGlobal.HorizontalScroll.Maximum = 0;
            tBodyPanelGlobal.AutoScroll = false;
            tBodyPanelGlobal.VerticalScroll.Visible = false;
            tBodyPanelGlobal.AutoScroll = true;

            if (sqlite_conn.State == ConnectionState.Closed)
            {
                sqlite_conn.Open();
            }
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();

            string queryCmd = "SELECT * FROM " + constants.tbNames[11] + " WHERE GroupID=@groupID";
            sqlite_cmd.CommandText = queryCmd;
            sqlite_cmd.Parameters.AddWithValue("@groupID", groupIDList[groupID]);
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            int k = 0;
            while (sqlite_datareader.Read())
            {
                if (!sqlite_datareader.IsDBNull(0))
                {
                    string prdName = sqlite_datareader.GetString(1);
                    int prdPrice = sqlite_datareader.GetInt32(2);
                    FlowLayoutPanel tableRowPanel = createPanel.CreateFlowLayoutPanel(tBodyPanelGlobal, 0, 50 * k, tBodyPanelGlobal.Width, 50, Color.Transparent, new Padding(0));
                    Label tdLabel1 = createLabel.CreateLabels(tableRowPanel, "tdLabel1_" + k, prdName, 0, 0, tableRowPanel.Width * 2 / 3, 50, Color.White, Color.Black, 16, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
                    Label tdLabel2 = createLabel.CreateLabels(tableRowPanel, "tdLabel2_" + k, prdPrice.ToString(), tdLabel1.Right, 0, tableRowPanel.Width / 3, 50, Color.White, Color.Black, 16, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
                    k++;
                }
            }
        }

        public void setVal(string groupID)
        {
            tBodyPanelGlobal.Controls.Clear();
            groupIDGlobal = int.Parse(groupID);
            ShowGroupDetail(int.Parse(groupID));
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
        public void btnprintpreview_Click()

        {
          //  printDocument1.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
            printPreviewDialog1.Document = printDocument1;
            //printDialog1.Document = printDocument1;

            ((ToolStripButton)((ToolStrip)printPreviewDialog1.Controls[1]).Items[0]).Enabled = true;


            printDocument1.DefaultPageSettings.PaperSize = paperSize;
            printPreviewDialog1.ShowDialog();
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

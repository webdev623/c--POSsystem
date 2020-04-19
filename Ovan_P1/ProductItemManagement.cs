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
    public partial class ProductItemManagement : Form
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
        Panel downPanelGlobal = null;
        DetailView detailView = new DetailView();
        Label[] columnGlobal1 = null;
        Label[] columnGlobal2 = null;
        Label[] columnGlobal3 = null;
        Label[] columnGlobal4 = null;
        int totalRowNum = 0;

        SQLiteConnection sqlite_conn;
        DateTime now = DateTime.Now;

        public ProductItemManagement(Form1 mainForm, Panel mainPanel)
        {
            InitializeComponent();
            mainFormGlobal = mainForm;
            mainPanelGlobal = mainPanel;
            mainForm.BackColor = Color.White;
            mainPanel.BackColor = Color.White;
            Panel upPanel = createPanel.CreateMainPanel(mainForm, 20, 20, mainForm.Width - 40, mainForm.Height * 5 / 11 - 20, BorderStyle.FixedSingle, Color.White);
            Panel downPanel = createPanel.CreateMainPanel(mainForm, 20, upPanel.Bottom + 20, mainForm.Width - 40, mainForm.Height * 6 / 11 - 20, BorderStyle.None, Color.White);
            downPanelGlobal = downPanel;
            FlowLayoutPanel tableHeaderInUpPanel = createPanel.CreateFlowLayoutPanel(upPanel, 0, 0, upPanel.Width, 50, Color.Transparent, new Padding(0));
            Label tableHeaderLabel1 = createLabel.CreateLabels(tableHeaderInUpPanel, "tableHeaderLabel1", constants.printProductNameField, 0, 0, tableHeaderInUpPanel.Width * 2 / 5, 50, Color.FromArgb(255, 147, 184, 219), Color.Black, 16, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
            Label tableHeaderLabel2 = createLabel.CreateLabels(tableHeaderInUpPanel, "tableHeaderLabel2", constants.salePriceField, tableHeaderLabel1.Right, 0, tableHeaderInUpPanel.Width * 3 / 20, 50, Color.FromArgb(255, 147, 184, 219), Color.Black, 16, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
            Label tableHeaderLabel3 = createLabel.CreateLabels(tableHeaderInUpPanel, "tableHeaderLabel3", constants.TimeLabel, tableHeaderLabel2.Right, 0, tableHeaderInUpPanel.Width * 3 / 10, 50, Color.FromArgb(255, 147, 184, 219), Color.Black, 16, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
            Label tableHeaderLabel4 = createLabel.CreateLabels(tableHeaderInUpPanel, "tableHeaderLabel4", constants.saleLimitField, tableHeaderLabel3.Right, 0, tableHeaderInUpPanel.Width * 3 / 20, 50, Color.FromArgb(255, 147, 184, 219), Color.Black, 16, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);

            Panel tBodyPanel = createPanel.CreateSubPanel(upPanel, 0, 50, upPanel.Width, upPanel.Height - 50, BorderStyle.FixedSingle, Color.White);
            tBodyPanel.HorizontalScroll.Maximum = 0;
            tBodyPanel.AutoScroll = false;
            tBodyPanel.VerticalScroll.Visible = false;
            tBodyPanel.AutoScroll = true;

            sqlite_conn = CreateConnection(constants.dbName);
            if (sqlite_conn.State == ConnectionState.Closed)
            {
                sqlite_conn.Open();
            }
            SQLiteCommand sqlite_cmds;
            string week = now.ToString("ddd");
            string currentTime = now.ToString("HH:mm");

            try
            {
                string productQuerys = "SELECT count(*) FROM (SELECT count(*) FROM " + constants.tbNames[2] + " GROUP BY ProductID ORDER BY CategoryID)";
                sqlite_cmds = sqlite_conn.CreateCommand();
                sqlite_cmds.CommandText = productQuerys;
                totalRowNum = Convert.ToInt32(sqlite_cmds.ExecuteScalar());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            columnGlobal1 = new Label[totalRowNum];
            columnGlobal2 = new Label[totalRowNum];
            columnGlobal3 = new Label[totalRowNum];
            columnGlobal4 = new Label[totalRowNum];
            SQLiteCommand sqlite_cmd;
            SQLiteDataReader sqlite_datareader;

            string productQuery = "SELECT * FROM " + constants.tbNames[2] + " GROUP BY ProductID ORDER BY ProductID";
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = productQuery;

            sqlite_datareader = sqlite_cmd.ExecuteReader();
            int k = 0;
            while (sqlite_datareader.Read())
            {
                if (!sqlite_datareader.IsDBNull(0))
                {
                    int productID = sqlite_datareader.GetInt32(2);
                    SQLiteCommand sqlite_cmd0;
                    SQLiteDataReader sqlite_datareader0;

                    string productQuery0 = "SELECT * FROM " + constants.tbNames[2] + " WHERE ProductID=@productID ORDER BY CategoryID LIMIT 1";
                    sqlite_cmd0 = sqlite_conn.CreateCommand();
                    sqlite_cmd0.CommandText = productQuery0;
                    sqlite_cmd0.Parameters.AddWithValue("@productID", productID);
                    sqlite_datareader0 = sqlite_cmd0.ExecuteReader();
                    while (sqlite_datareader0.Read())
                    {
                        if (!sqlite_datareader0.IsDBNull(0))
                        {
                            int prdID = sqlite_datareader0.GetInt32(2);
                            string saleTime = "10:00-21-59";
                            if (week == "Sat")
                            {
                                saleTime = (sqlite_datareader0.GetString(6)).Split('/')[0];
                            }
                            else if (week == "Sun")
                            {
                                saleTime = (sqlite_datareader0.GetString(7)).Split('/')[0];
                            }
                            else
                            {
                                saleTime = (sqlite_datareader0.GetString(5)).Split('/')[0];
                            }
                            string printName = sqlite_datareader0.GetString(4);
                            int prdPrice = sqlite_datareader0.GetInt32(8);
                            int limitedCnt = sqlite_datareader0.GetInt32(9);
                            int soldFlag = sqlite_datareader0.GetInt32(31);
                            int saleAmount = 0;
                            int restAmount = 0;
                            SQLiteCommand sqlite_cmd1;
                            SQLiteDataReader sqlite_datareader1;
                            string productQuery1 = "SELECT sum(prdAmount) FROM " + constants.tbNames[3] + " WHERE prdID=@prdID and sumFlag='false'";
                            sqlite_cmd1 = sqlite_conn.CreateCommand();
                            sqlite_cmd1.CommandText = productQuery1;
                            sqlite_cmd1.Parameters.AddWithValue("@prdID", prdID);
                            sqlite_datareader1 = sqlite_cmd1.ExecuteReader();
                            while (sqlite_datareader1.Read())
                            {
                                if (!sqlite_datareader1.IsDBNull(0))
                                {
                                    saleAmount = sqlite_datareader1.GetInt32(0);
                                }
                            }
                            if(limitedCnt != 0 && limitedCnt >= saleAmount)
                            {
                                restAmount = limitedCnt - saleAmount;
                            }

                            string saleStatus = "0";
                            Color fontColor = Color.Black;
                            if(soldFlag == 0)
                            {
                                if(limitedCnt != 0)
                                {
                                    saleStatus = restAmount.ToString() + "/" + limitedCnt.ToString();
                                }
                            }
                            else
                            {
                                saleStatus = constants.saleStopStatusText;
                                fontColor = Color.Red;
                            }

                            FlowLayoutPanel tableRowPanel = createPanel.CreateFlowLayoutPanel(tBodyPanel, 0, 50 * k, tBodyPanel.Width, 50, Color.Transparent, new Padding(0));
                            Label tdLabel1 = createLabel.CreateLabels(tableRowPanel, "tdLabel1_" + k + "_" + prdID, printName, 0, 0, tableRowPanel.Width * 2 / 5, 50, Color.White, fontColor, 16, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
                            columnGlobal1[k] = tdLabel1;
                            Label tdLabel2 = createLabel.CreateLabels(tableRowPanel, "tdLabel2_" + k + "_" + prdID, prdPrice.ToString(), tdLabel1.Right, 0, tableRowPanel.Width * 3 / 20, 50, Color.White, fontColor, 16, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
                            columnGlobal2[k] = tdLabel2;
                            Label tdLabel3 = createLabel.CreateLabels(tableRowPanel, "tdLabel3_" + k + "_" + prdID, saleTime, tdLabel2.Right, 0, tableRowPanel.Width * 3 / 10, 50, Color.White, fontColor, 16, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
                            columnGlobal3[k] = tdLabel3;
                            Label tdLabel4 = createLabel.CreateLabels(tableRowPanel, "tdLabel4_" + k + "_" + prdID, saleStatus, tdLabel3.Right, 0, tableRowPanel.Width * 3 / 20, 50, Color.White, fontColor, 16, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
                            columnGlobal4[k] = tdLabel4;

                            tdLabel1.Click += new EventHandler(this.ShowProductDetail);
                            tdLabel2.Click += new EventHandler(this.ShowProductDetail);
                            tdLabel3.Click += new EventHandler(this.ShowProductDetail);
                            tdLabel4.Click += new EventHandler(this.ShowProductDetail);


                            k++;

                        }
                    }
                }
            }

            Button closeButton = customButton.CreateButton(constants.backText, "closeButton", downPanel.Width - 150, downPanel.Height - 100, 100, 50, Color.FromArgb(255, 0, 112, 192), Color.Transparent, 0, 1, 12, FontStyle.Regular, Color.White);
            downPanel.Controls.Add(closeButton);
            closeButton.Click += new EventHandler(this.BackShow);



        }

        private void ShowProductDetail(object sender, EventArgs e)
        {
            Label prdTemp = (Label)sender;
            int prdIndex = int.Parse(prdTemp.Name.Split('_')[1]);
            int prdID = int.Parse(prdTemp.Name.Split('_')[2]);
            columnGlobal1[prdIndex].BackColor = Color.FromArgb(255, 198, 224, 180);
            columnGlobal2[prdIndex].BackColor = Color.FromArgb(255, 198, 224, 180);
            columnGlobal3[prdIndex].BackColor = Color.FromArgb(255, 198, 224, 180);
            columnGlobal4[prdIndex].BackColor = Color.FromArgb(255, 198, 224, 180);
            for (int k = 0; k < totalRowNum; k++)
            {
                if (k != prdIndex)
                {
                    columnGlobal1[k].BackColor = Color.White;
                    columnGlobal2[k].BackColor = Color.White;
                    columnGlobal3[k].BackColor = Color.White;
                    columnGlobal4[k].BackColor = Color.White;
                }
            }

            downPanelGlobal.Controls.Clear();
            Panel detailPanel = createPanel.CreateSubPanel(downPanelGlobal, 50, 10, downPanelGlobal.Width / 2 - 100, downPanelGlobal.Height - 100, BorderStyle.FixedSingle, Color.Transparent);

            int rowCount = 9;

            FlowLayoutPanel leftColumn = createPanel.CreateFlowLayoutPanel(detailPanel, 0, 0, detailPanel.Width * 2 / 5, detailPanel.Height, Color.Transparent, new Padding(0));
            FlowLayoutPanel rightColumn = createPanel.CreateFlowLayoutPanel(detailPanel, detailPanel.Width * 2 / 5, 0, detailPanel.Width * 3 / 5, detailPanel.Height, Color.Transparent, new Padding(0));

            Label column1 = createLabel.CreateLabels(leftColumn, "prdNameColumn", constants.prdNameField, 0, 0, leftColumn.Width, leftColumn.Height / rowCount, Color.FromArgb(255, 198, 224, 180), Color.Black, 12, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
            Label column2 = createLabel.CreateLabels(leftColumn, "prdCategoryColumn", constants.prdCategoryField, 0, column1.Bottom, leftColumn.Width, leftColumn.Height / rowCount, Color.FromArgb(255, 198, 224, 180), Color.Black, 12, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
            Label column3 = createLabel.CreateLabels(leftColumn, "prdPriceColumn", constants.prdPriceFieldIncludTax, 0, column2.Bottom, leftColumn.Width, leftColumn.Height / rowCount, Color.FromArgb(255, 198, 224, 180), Color.Black, 12, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
            Label column4 = createLabel.CreateLabels(leftColumn, "prdTimeColumn", constants.prdSaleTimeField, 0, column3.Bottom, leftColumn.Width, leftColumn.Height * 3 / rowCount, Color.FromArgb(255, 198, 224, 180), Color.Black, 12, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
            Label column5 = createLabel.CreateLabels(leftColumn, "prdLimitColumn", constants.saleLimitField, 0, column4.Bottom, leftColumn.Width, leftColumn.Height / rowCount, Color.FromArgb(255, 198, 224, 180), Color.Black, 12, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
            Label column6 = createLabel.CreateLabels(leftColumn, "prdLayoutColumn", constants.prdScreenText, 0, column5.Bottom, leftColumn.Width, leftColumn.Height / rowCount, Color.FromArgb(255, 198, 224, 180), Color.Black, 12, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
            Label column7 = createLabel.CreateLabels(leftColumn, "prdPrintColumn", constants.prdPrintText, 0, column6.Bottom, leftColumn.Width, leftColumn.Height / rowCount, Color.FromArgb(255, 198, 224, 180), Color.Black, 12, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);

            if (sqlite_conn.State == ConnectionState.Closed)
            {
                sqlite_conn.Open();
            }
            SQLiteCommand sqlite_cmd;
            SQLiteDataReader sqlite_datareader;

            string productQuery = "SELECT * FROM " + constants.tbNames[2] + " WHERE ProductID=@productID ORDER BY CategoryID LIMIT 1";
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = productQuery;
            sqlite_cmd.Parameters.AddWithValue("@productID", prdID);
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                if (!sqlite_datareader.IsDBNull(0))
                {
                    string saleSatTime = (sqlite_datareader.GetString(6));
                    string saleSunTime = (sqlite_datareader.GetString(7));
                    string saleDayTime = (sqlite_datareader.GetString(5));
                    string prdName = sqlite_datareader.GetString(3);
                    int prdPrice = sqlite_datareader.GetInt32(8);
                    int limitedCnt = sqlite_datareader.GetInt32(9);
                    string screenMsg = sqlite_datareader.GetString(12);
                    string printMsg = sqlite_datareader.GetString(13);
                    string prdImgUrl = sqlite_datareader.GetString(11);
                    string prdBadgeUrl = sqlite_datareader.GetString(23);

                    SQLiteCommand sqlite_cmd1;
                    SQLiteDataReader sqlite_datareader1;
                    int m = 0;
                    string prdCategory = "";
                    string productQuery1 = "SELECT " + constants.tbNames[0] + ".CategoryName FROM " + constants.tbNames[2] + " LEFT JOIN " + constants.tbNames[0] + " ON " + constants.tbNames[2] + ".CategoryID=" + constants.tbNames[0] + ".CategoryID WHERE " + constants.tbNames[2] + ".ProductID=@productID";
                    sqlite_cmd1 = sqlite_conn.CreateCommand();
                    sqlite_cmd1.CommandText = productQuery1;
                    sqlite_cmd1.Parameters.AddWithValue("@productID", prdID);
                    sqlite_datareader1 = sqlite_cmd1.ExecuteReader();
                    while (sqlite_datareader1.Read())
                    {
                        if (!sqlite_datareader1.IsDBNull(0))
                        {
                            if (m == 0)
                            {
                                prdCategory = sqlite_datareader1.GetString(0);
                            }
                            else if (m > 0)
                            {
                                prdCategory += ", " + sqlite_datareader1.GetString(0);

                            }
                            m++;
                        }
                    }

                    Label value1 = createLabel.CreateLabels(rightColumn, "prdNameValue", prdName, 0, 0, rightColumn.Width, rightColumn.Height / rowCount, Color.White, Color.Black, 12, true, ContentAlignment.MiddleLeft, new Padding(0), 2, Color.Gray);
                    value1.Padding = new Padding(10, 0, 0, 0);
                    Label value2 = createLabel.CreateLabels(rightColumn, "prdCategoryValue", prdCategory, 0, column1.Bottom, rightColumn.Width, rightColumn.Height / rowCount, Color.White, Color.Black, 12, true, ContentAlignment.MiddleLeft, new Padding(0), 2, Color.Gray);
                    value2.Padding = new Padding(10, 0, 0, 0);
                    Label value3 = createLabel.CreateLabels(rightColumn, "prdPriceValue", prdPrice.ToString() + constants.unit, 0, column2.Bottom, rightColumn.Width, rightColumn.Height / rowCount, Color.White, Color.Black, 12, true, ContentAlignment.MiddleLeft, new Padding(0), 2, Color.Gray);
                    value3.Padding = new Padding(10, 0, 0, 0);

                    Label timeLabel1 = createLabel.CreateLabels(rightColumn, "prdTimeLabel1", "平日", 0, column3.Bottom, rightColumn.Width / 3, rightColumn.Height / rowCount, Color.White, Color.Black, 12, true, ContentAlignment.MiddleCenter, new Padding(0), 2, Color.Gray);
                    Label value4 = createLabel.CreateLabels(rightColumn, "prdTimeValue1", saleDayTime, 0, column3.Bottom, rightColumn.Width * 2 / 3, rightColumn.Height / rowCount, Color.White, Color.Black, 12, true, ContentAlignment.MiddleLeft, new Padding(0), 2, Color.Gray);
                    value4.Padding = new Padding(10, 0, 0, 0);

                    Label timeLabel2 = createLabel.CreateLabels(rightColumn, "prdTimeLabel2", "土曜", 0, column3.Bottom, rightColumn.Width / 3, rightColumn.Height / rowCount, Color.White, Color.FromArgb(255, 0, 112, 192), 12, true, ContentAlignment.MiddleCenter, new Padding(0), 2, Color.Gray);
                    Label value5 = createLabel.CreateLabels(rightColumn, "prdTimeValue2", saleSatTime, 0, column4.Bottom, rightColumn.Width * 2 / 3, rightColumn.Height / rowCount, Color.White, Color.Black, 12, true, ContentAlignment.MiddleLeft, new Padding(0), 2, Color.Gray);
                    value5.Padding = new Padding(10, 0, 0, 0);

                    Label timeLabel3 = createLabel.CreateLabels(rightColumn, "prdTimeLabel3", "日曜", 0, column3.Bottom, rightColumn.Width / 3, rightColumn.Height / rowCount, Color.White, Color.Red, 12, true, ContentAlignment.MiddleCenter, new Padding(0), 2, Color.Gray);
                    Label value6 = createLabel.CreateLabels(rightColumn, "prdTimeValue3", saleSunTime, 0, column5.Bottom, rightColumn.Width * 2 / 3, rightColumn.Height / rowCount, Color.White, Color.Black, 12, true, ContentAlignment.MiddleLeft, new Padding(0), 2, Color.Gray);
                    value6.Padding = new Padding(10, 0, 0, 0);
                    Label value7 = createLabel.CreateLabels(rightColumn, "prdLimitValue", limitedCnt.ToString(), 0, value6.Bottom, rightColumn.Width, rightColumn.Height / rowCount, Color.White, Color.Black, 12, true, ContentAlignment.MiddleLeft, new Padding(0), 2, Color.Gray);
                    value7.Padding = new Padding(10, 0, 0, 0);
                    Label value8 = createLabel.CreateLabels(rightColumn, "prdLayoutValue", screenMsg, 0, value7.Bottom, rightColumn.Width, rightColumn.Height / rowCount, Color.White, Color.Black, 12, true, ContentAlignment.MiddleLeft, new Padding(0), 2, Color.Gray);
                    value8.Padding = new Padding(10, 0, 0, 0);
                    Label value9 = createLabel.CreateLabels(rightColumn, "prdPrintValue", printMsg, 0, value8.Bottom, rightColumn.Width, rightColumn.Height / rowCount, Color.White, Color.Black, 12, true, ContentAlignment.MiddleLeft, new Padding(0), 2, Color.Gray);
                    value9.Padding = new Padding(10, 0, 0, 0);

                    Panel productImagePanel = createPanel.CreateSubPanel(downPanelGlobal, downPanelGlobal.Width / 2 + 100, 10, downPanelGlobal.Width / 4, downPanelGlobal.Height * 3 / 5, BorderStyle.FixedSingle, Color.White);
                    PictureBox productImage = new PictureBox();
                    productImage.Location = new Point(0, 0);
                    productImage.Size = new Size(productImagePanel.Width, productImagePanel.Height * 2 / 3);
                    productImage.Name = "productImage";
                    productImage.SizeMode = PictureBoxSizeMode.StretchImage;
                    productImage.BorderStyle = BorderStyle.FixedSingle;
                    productImagePanel.Controls.Add(productImage);

                    Bitmap backBitmap = null;
                    if(prdImgUrl != "")
                    {
                        Rectangle rc = new Rectangle(productImagePanel.Width / 6, 0, productImagePanel.Width * 2 / 3, productImagePanel.Height);
                        backBitmap = new Bitmap(productImagePanel.Width, productImagePanel.Height);
                        Bitmap bm = new Bitmap(prdImgUrl);
                        using (Graphics gr = Graphics.FromImage(backBitmap))
                            gr.DrawImage(bm, rc);
                        productImage.Image = backBitmap;
                    }

                    if (prdBadgeUrl != "")
                    {
                        Rectangle rc = new Rectangle(productImagePanel.Width * 2 / 3 - 30, productImagePanel.Height / 3, productImagePanel.Width / 3, productImagePanel.Height / 3);
                        Bitmap bm = new Bitmap(prdBadgeUrl);
                        using (Graphics gr = Graphics.FromImage(backBitmap))
                            gr.DrawImage(bm, rc);
                        productImage.Image = backBitmap;
                    }

                    FlowLayoutPanel productLabelPanel = createPanel.CreateFlowLayoutPanel(productImagePanel, 0, productImage.Bottom, productImagePanel.Width, productImagePanel.Height / 3, Color.Transparent, new Padding(0));
                    productLabelPanel.BorderStyle = BorderStyle.FixedSingle;
                    Label prodcutLabel1 = createLabel.CreateLabelsInPanel(productLabelPanel, "productLabel1", prdName + "   " + prdPrice.ToString(), 0, 0, productLabelPanel.Width, productLabelPanel.Height, Color.White, Color.Black, 22, false, ContentAlignment.MiddleCenter);

                }
            }
            
            Button closeButton = customButton.CreateButton(constants.backText, "closeButton", downPanelGlobal.Width - 200, downPanelGlobal.Height - 100, 100, 50, Color.FromArgb(255, 0, 112, 192), Color.Transparent, 0, 1, 12, FontStyle.Regular, Color.White);
            downPanelGlobal.Controls.Add(closeButton);
            closeButton.Click += new EventHandler(this.BackShow);

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

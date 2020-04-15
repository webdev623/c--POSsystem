using System;
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
    public partial class SoldoutSetting1 : Form
    {
        int height = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height;
        int width = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width;
        Form1 mainFormGlobal = null;
        Panel mainPanelGlobal = null;
        int selectedCategoryIndex = 0;
        List<int> soldoutCategoryIndex = new List<int>();
        Constant constants = new Constant();
        CreatePanel createPanel = new CreatePanel();
        CustomButton customButton = new CustomButton();
        CreateLabel createLabel = new CreateLabel();
        DropDownMenu dropDownMenu = new DropDownMenu();
        NumberInput numberInput = new NumberInput();
        Panel productListPanelGlobal = null;
        Label labelForLimitAmount = null;
        Button saleStateButtonGlobal = null;
        private Panel bodyPanelGlobal = null;
        private bool saleStateGlobal = true;

        string[] categoryNameList = null;
        int[] categoryIDList = null;
        int[] categoryDisplayPositionList = null;
        int[] categorySoldStateList = null;
        List<int> stopedPrdIDArray = null;
        int categoryRowCount = 0;

        SQLiteConnection sqlite_conn;

        public SoldoutSetting1(Form1 mainForm, Panel mainPanel)
        {
            sqlite_conn = CreateConnection(constants.dbName);
            SQLiteCommand sqlite_cmd;
            SQLiteDataReader sqlite_datareader;
            this.GetCategoryList();

            mainForm.Width = width;
            mainForm.Height = height;
            mainFormGlobal = mainForm;
            mainPanelGlobal = mainPanel;
            Panel mainPanels = createPanel.CreateMainPanel(mainForm, 0, 0, width, height, BorderStyle.None, Color.Transparent);
            dropDownMenu.initSoldoutSetting(this);
            numberInput.initSoldoutSetting(this);
            Panel headerPanel = createPanel.CreateSubPanel(mainPanels, 0, 0, width, height / 6, BorderStyle.None, Color.Transparent);
            Panel bodyPanel = createPanel.CreateSubPanel(mainPanels, 0, height / 6, width, height * 5 / 6, BorderStyle.None, Color.Transparent);

            Label titleLabel = createLabel.CreateLabelsInPanel(headerPanel, "SoldoutSetting_Title", constants.soldoutSettingTitle, 10, 10, headerPanel.Width / 2, headerPanel.Height, Color.White, Color.Red, 28, false, ContentAlignment.MiddleLeft);

            Label subTitleLabel = createLabel.CreateLabelsInPanel(bodyPanel, "SoldoutSetting_subTitle", constants.categorylistLabel, 80, 0, width / 2 - 300, 50, Color.White, Color.Red, 24, false, ContentAlignment.MiddleRight);


            dropDownMenu.CreateCategoryDropDown("soldoutSetting1", bodyPanel, categoryNameList, categoryIDList, categoryDisplayPositionList, categorySoldStateList, width / 2 - 150, 0, 200, 50, 200, 50 * (constants.saleCategories.Length + 1), 200, 50, Color.Red, Color.Yellow);

            Button saleStateButton = customButton.CreateButton(constants.saleStatusLabel, "saleSateButton", width / 2 + 200, 0, 200, 50, Color.FromArgb(255, 0, 112, 192), Color.Transparent, 0, 10, 14, FontStyle.Bold, Color.White);
            saleStateButton.Click += new EventHandler(this.SaleSateSwitching);
            saleStateButtonGlobal = saleStateButton;

            bodyPanel.Controls.Add(saleStateButton);

            Button backButton = customButton.CreateButton(constants.backText, "backButton", bodyPanel.Width - 150, bodyPanel.Height - 100, 100, 50, Color.FromArgb(255, 0, 112, 192), Color.Transparent, 0, 10, 14, FontStyle.Bold, Color.White);
            bodyPanel.Controls.Add(backButton);
            backButton.Click += new EventHandler(this.BackShow);

            bodyPanelGlobal = bodyPanel;


            FlowLayoutPanel productTableHeader = createPanel.CreateFlowLayoutPanel(bodyPanelGlobal, 150, 60, width - 350, 60, Color.Beige, new Padding(0));
            Label prodNameHeader1 = createLabel.CreateLabelsInPanel(productTableHeader, "prodNameHeader1", constants.prdNameField, 0, 0, productTableHeader.Width / 4 - 10, productTableHeader.Height, Color.Transparent, Color.Silver, 12);
            Label prodNameHeader2 = createLabel.CreateLabelsInPanel(productTableHeader, "prodNameHeader2", constants.salePriceField, productTableHeader.Width / 4, 0, productTableHeader.Width / 4 - 10, productTableHeader.Height, Color.Transparent, Color.Silver, 12);
            Label prodNameHeader3 = createLabel.CreateLabelsInPanel(productTableHeader, "prodNameHeader3", constants.saleLimitField, productTableHeader.Width / 2, 0, productTableHeader.Width / 4 - 10, productTableHeader.Height, Color.Transparent, Color.Silver, 12);
            Label prodNameHeader4 = createLabel.CreateLabelsInPanel(productTableHeader, "prodNameHeader4", constants.saleStateSettingField, productTableHeader.Width * 3 / 4, 0, productTableHeader.Width / 4 - 10, productTableHeader.Height, Color.Transparent, Color.Silver, 12);

            Panel productListPanel = createPanel.CreateSubPanel(bodyPanelGlobal, 150, 120, width - 350, height * 5 / 6 - 150, BorderStyle.None, Color.Transparent);
            productListPanel.AutoScroll = true;
            productListPanelGlobal = productListPanel;

            CreateProductTable();

            InitializeComponent();
        }

        private void CreateProductTable()
        {
            if (sqlite_conn.State == ConnectionState.Closed)
            {
                sqlite_conn.Open();
            }
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
                    string prdName = sqlite_datareader.GetString(4);
                    int prdPrice = sqlite_datareader.GetInt32(9);
                    int prdLimitedCnt = sqlite_datareader.GetInt32(10);
                    int prdID = sqlite_datareader.GetInt32(2);
                    int rowID = sqlite_datareader.GetInt32(0);
                    int soldFlag = sqlite_datareader.GetInt32(31);
                    int restAmount = prdLimitedCnt;
                    SQLiteDataReader sqlite_datareader1;
                    SQLiteCommand sqlite_cmd1;
                    sqlite_cmd1 = sqlite_conn.CreateCommand();
                    string queryCmd1 = "SELECT SUM(prdAmount) as prdRestAmount FROM " + constants.tbNames[3] + " WHERE categoryID=@CategoryID and prdID=@prdID";
                    sqlite_cmd1.CommandText = queryCmd1;
                    sqlite_cmd1.Parameters.AddWithValue("@CategoryID", categoryIDList[selectedCategoryIndex]);
                    sqlite_cmd1.Parameters.AddWithValue("@prdID", rowID);
                    sqlite_datareader1 = sqlite_cmd1.ExecuteReader();
                    while (sqlite_datareader1.Read())
                    {
                        if (!sqlite_datareader1.IsDBNull(0) && prdLimitedCnt != 0)
                        {
                            restAmount = prdLimitedCnt - sqlite_datareader1.GetInt32(0);
                        }
                    }
                    FlowLayoutPanel productTableBody = createPanel.CreateFlowLayoutPanel(productListPanelGlobal, 0, 61 * k, productListPanelGlobal.Width - 18, 60, Color.FromArgb(255, 233, 211, 177), new Padding(0));
                    Label prodNameBody1 = createLabel.CreateLabelsInPanel(productTableBody, "prodBody_" + k + "_1", prdName, 0, 0, productTableBody.Width / 4 - 10, productTableBody.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);
                    Label prodNameBody2 = createLabel.CreateLabelsInPanel(productTableBody, "prodBody_" + k + "_2", prdPrice.ToString(), productTableBody.Width / 4, 0, productTableBody.Width / 4 - 10, productTableBody.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);
                    Label prodNameBody3 = createLabel.CreateLabelsInPanel(productTableBody, "prodBody_" + k + "_3_" + rowID + "_" + prdLimitedCnt, restAmount.ToString() + "(" + prdLimitedCnt.ToString() + ")", productTableBody.Width / 2, 0, productTableBody.Width / 4 - 10, productTableBody.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);
                    prodNameBody3.Click += new EventHandler(this.showNumberInputDialog);

                    string stateText = constants.saleStatusLabel;
                    Color bgColor = Color.FromArgb(255, 0, 112, 192);
                    if (soldFlag == 1)
                    {
                        bgColor = Color.Red;
                        stateText = constants.saleStopLabel;
                        stopedPrdIDArray.Add(rowID);
                    }
                    Label prodNameBody4 = createLabel.CreateLabelsInPanel(productTableBody, "prodBody_" + k + "_4_" + prdID + "_" + rowID, stateText, productTableBody.Width * 3 / 4, 0, productTableBody.Width / 4 - 10, productTableBody.Height, bgColor, Color.White, 12);
                    prodNameBody4.Click += new EventHandler(this.ProductSaleStateSwitching);
                    k++;
                }
            }
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

        public void setVal(string selectedIndex)
        {

            productListPanelGlobal.Controls.Clear();
            selectedCategoryIndex = int.Parse(selectedIndex);
            int soldoutState = soldoutCategoryIndex.Find(elem => elem == selectedCategoryIndex + 1);
            if(soldoutState != 0)
            {
                saleStateButtonGlobal.BackColor = Color.Red;
                saleStateButtonGlobal.Text = constants.saleStopLabel;
                saleStateGlobal = false;
            }
            else
            {
                saleStateButtonGlobal.BackColor = Color.FromArgb(255, 0, 112, 192);
                saleStateButtonGlobal.Text = constants.saleStatusLabel;
                saleStateGlobal = true;
                CreateProductTable();
            }

        }
        private void SaleSateSwitching(object sender, EventArgs e)
        {
            Button btnTemp = (Button)sender;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();

            if (saleStateGlobal)
            {
                string queryCmd0 = "UPDATE " + constants.tbNames[0] + " SET SoldFlag=1 WHERE id=@categoryID";
                sqlite_cmd.CommandText = queryCmd0;
                sqlite_cmd.Parameters.AddWithValue("@categoryID", categoryIDList[selectedCategoryIndex]);
                sqlite_cmd.ExecuteNonQuery();

                string queryCmd1 = "UPDATE " + constants.tbNames[2] + " SET SoldFlag=1 WHERE CategoryID=@categoryID";
                sqlite_cmd.CommandText = queryCmd1;
                sqlite_cmd.Parameters.AddWithValue("@categoryID", categoryIDList[selectedCategoryIndex]);
                sqlite_cmd.ExecuteNonQuery();

                soldoutCategoryIndex.Add(selectedCategoryIndex + 1);
                btnTemp.Text = constants.saleStopLabel;
                btnTemp.BackColor = Color.Red;
                saleStateGlobal = false;
                productListPanelGlobal.Controls.Clear();

            }
            else
            {
                string queryCmd0 = "UPDATE " + constants.tbNames[0] + " SET SoldFlag=0 WHERE id=@categoryID";
                sqlite_cmd.CommandText = queryCmd0;
                sqlite_cmd.Parameters.AddWithValue("@categoryID", categoryIDList[selectedCategoryIndex]);
                sqlite_cmd.ExecuteNonQuery();

                string queryCmd1 = "UPDATE " + constants.tbNames[2] + " SET SoldFlag=0 WHERE CategoryID=@categoryID";
                sqlite_cmd.CommandText = queryCmd1;
                sqlite_cmd.Parameters.AddWithValue("@categoryID", categoryIDList[selectedCategoryIndex]);
                sqlite_cmd.ExecuteNonQuery();

                foreach(int stopedID in stopedPrdIDArray)
                {
                    string queryCmd2 = "UPDATE " + constants.tbNames[2] + " SET SoldFlag=1 WHERE id=@prdID";
                    sqlite_cmd.CommandText = queryCmd2;
                    sqlite_cmd.Parameters.AddWithValue("@prdID", stopedID);
                    sqlite_cmd.ExecuteNonQuery();
                }

                soldoutCategoryIndex.Remove(selectedCategoryIndex + 1);
                btnTemp.Text = constants.saleStatusLabel;
                btnTemp.BackColor = Color.FromArgb(255, 0, 112, 192);
                saleStateGlobal = true;
                CreateProductTable();
            }
          //  MessageBox.Show(soldoutCategoryIndex.ToArray().Length.ToString());
        }

        private void ProductSaleStateSwitching(object sender, EventArgs e)
        {
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();

            Label btnTemp = (Label)sender;
            int prdID = int.Parse(btnTemp.Name.Split('_')[3]);
            int rowID = int.Parse(btnTemp.Name.Split('_')[4]);
            if (btnTemp.Text == constants.saleStatusLabel)
            {
                string queryCmd1 = "UPDATE " + constants.tbNames[2] + " SET SoldFlag=1 WHERE ProductID=@productID";
                sqlite_cmd.CommandText = queryCmd1;
                sqlite_cmd.Parameters.AddWithValue("@productID", prdID);
                sqlite_cmd.ExecuteNonQuery();
                stopedPrdIDArray.Add(rowID);
                btnTemp.Text = constants.saleStopLabel;
                btnTemp.BackColor = Color.Red;
            }
            else
            {
                string queryCmd1 = "UPDATE " + constants.tbNames[2] + " SET SoldFlag=0 WHERE ProductID=@productID";
                sqlite_cmd.CommandText = queryCmd1;
                sqlite_cmd.Parameters.AddWithValue("@productID", prdID);
                sqlite_cmd.ExecuteNonQuery();
                stopedPrdIDArray.Remove(rowID);
                btnTemp.Text = constants.saleStatusLabel;
                btnTemp.BackColor = Color.FromArgb(255, 0, 112, 192);
            }
        }

        private void showNumberInputDialog(object sender, EventArgs e)
        {
            Label objectHandler = (Label)sender;
            labelForLimitAmount = objectHandler;
            string objectHandlerText = objectHandler.Text;
            //string limitAmounts = objectHandlerText.Split('(')[1];
            string limitAmounts = objectHandlerText.Substring(objectHandlerText.IndexOf('(') + 1, objectHandlerText.Length - objectHandlerText.IndexOf('(') - 2);
            int limitAmount = int.Parse(limitAmounts);
            //CreatePanel numberInputPanel = createPanel.
            numberInput.CreateNumberInputDialog("soldoutSetting1", limitAmount, objectHandler.Name);
           // MessageBox.Show(limitAmounts);
        }

        public void SetLimitationValue(string limitAmount)
        {
            string objectHandlerText = labelForLimitAmount.Text;
            string prefixName = labelForLimitAmount.Name.Split('_')[0] + "_" + labelForLimitAmount.Name.Split('_')[1] + "_" + labelForLimitAmount.Name.Split('_')[2];
            int rowID = int.Parse(labelForLimitAmount.Name.Split('_')[3]);
            int oldLimitedAmount = int.Parse(labelForLimitAmount.Name.Split('_')[4]);
            string realAmounts = objectHandlerText.Split('(')[0];

            if (sqlite_conn.State == ConnectionState.Closed)
            {
                sqlite_conn.Open();
            }


            if (limitAmount != "" && int.Parse(limitAmount) >= int.Parse(realAmounts))
            {
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = sqlite_conn.CreateCommand();
                string queryCmd1 = "UPDATE " + constants.tbNames[2] + " SET LimitedCnt=@limitedAmount WHERE id=@rowID";
                sqlite_cmd.CommandText = queryCmd1;
                sqlite_cmd.Parameters.AddWithValue("@limitedAmount", int.Parse(limitAmount));
                sqlite_cmd.Parameters.AddWithValue("@rowID", rowID);
                sqlite_cmd.ExecuteNonQuery();

                realAmounts = (int.Parse(realAmounts) + int.Parse(limitAmount) - oldLimitedAmount).ToString();
                labelForLimitAmount.Name = prefixName + "_" + rowID.ToString() + "_" + limitAmount;
                labelForLimitAmount.Text = realAmounts + "(" + limitAmount + ")";
            }
            else if(int.Parse(limitAmount) == 0)
            {
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = sqlite_conn.CreateCommand();
                string queryCmd1 = "UPDATE " + constants.tbNames[2] + " SET LimitedCnt=@limitedAmount WHERE id=@rowID";
                sqlite_cmd.CommandText = queryCmd1;
                sqlite_cmd.Parameters.AddWithValue("@limitedAmount", int.Parse(limitAmount));
                sqlite_cmd.Parameters.AddWithValue("@rowID", rowID);
                sqlite_cmd.ExecuteNonQuery();

                labelForLimitAmount.Name = prefixName + "_" + rowID.ToString() + "_" + limitAmount;
                labelForLimitAmount.Text = "0(0)";
            }
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
            categorySoldStateList = new int[categoryRowCount];

            string queryCmd = "SELECT * FROM " + constants.tbNames[0] + " ORDER BY id";
            sqlite_cmd.CommandText = queryCmd;

            sqlite_datareader = sqlite_cmd.ExecuteReader();
            int k = 0;
            while (sqlite_datareader.Read())
            {
                if (!sqlite_datareader.IsDBNull(0))
                {
                    categoryIDList[k] = sqlite_datareader.GetInt32(0);
                    categoryNameList[k] = sqlite_datareader.GetString(1);
                    categoryDisplayPositionList[k] = sqlite_datareader.GetInt32(5);
                    bool saleFlag = false;
                    string openTime = "";
                    if (week == "Sat")
                    {
                        openTime = sqlite_datareader.GetString(3);
                    }
                    else if (week == "Sun")
                    {
                        openTime = sqlite_datareader.GetString(4);
                    }
                    else
                    {
                        openTime = sqlite_datareader.GetString(2);
                    }
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
                    if(saleFlag == true)
                    {
                        categorySoldStateList[k] = 0;
                    }
                    if(sqlite_datareader.GetInt32(8) == 1)
                    {
                        soldoutCategoryIndex.Add(k + 1);
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

            }
            return sqlite_conn;
        }

    }
}

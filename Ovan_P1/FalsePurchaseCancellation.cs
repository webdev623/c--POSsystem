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
    public partial class FalsePurchaseCancellation : Form
    {
        int height = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height;
        int width = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width;
        Form1 mainFormGlobal = null;
        Panel mainPanelGlobal = null;
        Form DialogFormGlobal = null;
        //private Button buttonGlobal = null;
        private FlowLayoutPanel[] menuFlowLayoutPanelGlobal = new FlowLayoutPanel[4];
        Constant constants = new Constant();

        CreatePanel createPanel = new CreatePanel();
        CreateLabel createLabel = new CreateLabel();
        CreateCombobox createCombobox = new CreateCombobox();
        CustomButton customButton = new CustomButton();
        DropDownMenu dropDownMenu = new DropDownMenu();
        MonthDropDown dropDownMonth = new MonthDropDown();
        DateDropDown dropDownDate = new DateDropDown();

        DetailView detailView = new DetailView();
        MessageDialog messageDialog = new MessageDialog();
        ComboBox yearComboboxStartGlobal = null;
        ComboBox monthComboboxStartGlobal = null;
        ComboBox dateComboboxStartGlobal = null;

        ComboBox yearComboboxEndGlobal = null;
        ComboBox monthComboboxEndGlobal = null;
        ComboBox dateComboboxEndGlobal = null;

        SQLiteConnection sqlite_conn;
        DateTime now = DateTime.Now;

        string storeEndTime = "00:00";
        string sumDate = DateTime.Now.ToString("yyyy-MM-dd");

        bool manualProcessState = false;
        public FalsePurchaseCancellation(Form1 mainForm, Panel mainPanel)
        {
            InitializeComponent();
            mainFormGlobal = mainForm;
            mainPanelGlobal = mainPanel;
            dropDownMenu.initFalsePurchaseCancellation(this);
            mainForm.AutoScroll = true;

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


            sumDate = constants.sumDate(storeEndTime);

            try
            {
                SQLiteCommand sqlite_cmds = sqlite_conn.CreateCommand();
                string sumIdentify = "SELECT COUNT(id) FROM " + constants.tbNames[7] + " WHERE sumDate=@sumDate";
                sqlite_cmds.CommandText = sumIdentify;
                sqlite_cmds.Parameters.AddWithValue("@sumDate", sumDate);
                int rowNumDaySale = Convert.ToInt32(sqlite_cmds.ExecuteScalar());
                if (rowNumDaySale > 0)
                {
                    manualProcessState = true;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Database Error:" + e);
            }

            sqlite_conn.Close();



            Panel headerPanel = createPanel.CreateMainPanel(mainForm, 0, 0, width, height / 5, BorderStyle.None, Color.Transparent);
            Label headerTitle = createLabel.CreateLabelsInPanel(headerPanel, "headerTitle", constants.falsePurchaseTitle, 50, 60, 200, 50, Color.Transparent, Color.Red, 22);
            Button closeButton = customButton.CreateButton(constants.backText, "closeButton", headerPanel.Width - 200, 60, 100, 50, Color.FromArgb(255, 0, 112, 192), Color.Transparent, 0, 1, 12, FontStyle.Regular, Color.White);
            headerPanel.Controls.Add(closeButton);
            closeButton.Click += new EventHandler(this.BackShow);

            Panel bodyUpPanel = createPanel.CreateMainPanel(mainForm, 0, headerPanel.Bottom, width, height * 1 / 4 + 20, BorderStyle.None, Color.Transparent);
            Label subTitle1 = createLabel.CreateLabelsInPanel(bodyUpPanel, "subTitle1", constants.falsePurchaseSubTitle1, bodyUpPanel.Width / 2 - 100, 10, 200, 50, Color.Transparent, Color.Black, 18);
            Label subContent1 = createLabel.CreateLabelsInPanel(bodyUpPanel, "subContent1", constants.falsePurchaseSubContent1, 100, subTitle1.Bottom + 15, bodyUpPanel.Width - 200, 50, Color.Transparent, Color.Black, 14);
            Button cancellationButton = customButton.CreateButton(constants.falsePurchaseButton, "cancellationButton", bodyUpPanel.Width / 2 - 100, subContent1.Bottom + 20, 200, 50, Color.FromArgb(255, 0, 112, 192), Color.Transparent, 0, 1, 12, FontStyle.Regular, Color.White);
            bodyUpPanel.Controls.Add(cancellationButton);

            if (manualProcessState)
            {
                cancellationButton.Click += new EventHandler(messageDialog.CancelOrderMessage);
            }
            else
            {
                cancellationButton.Click += new EventHandler(detailView.DetailViewIndicator);
            }

            Panel bodyDownPanel = createPanel.CreateMainPanel(mainForm, 0, bodyUpPanel.Bottom, width, height / 2, BorderStyle.None, Color.Transparent);
            Label subTitle2 = createLabel.CreateLabelsInPanel(bodyDownPanel, "subTitle1", constants.falsePurchaseSubTitle2, bodyDownPanel.Width / 2 - 200, 50, 400, 50, Color.Transparent, Color.Black, 18);

            Label startLabel = createLabel.CreateLabelsInPanel(bodyDownPanel, "startLabel", constants.falsePurchaseStartLabel, bodyDownPanel.Width / 5, subTitle2.Bottom + 30, 100, 50, Color.Transparent, Color.Black, 22);

            // dropDownYear.CreateDropDown("falsePurchaseCancellation", bodyDownPanel, new string[] { "2020", "2019", "2018" }, startLabel.Right + 10, subTitle2.Bottom + 30, 150, 50, 150, 200, 150, 50, Color.Transparent, Color.Transparent);
            ComboBox yearCombobox1 = createCombobox.CreateComboboxs(bodyDownPanel, "yearCombobox1", new string[] { "2020", "2019", "2018" }, startLabel.Right + 40, subTitle2.Bottom + 40, 150, 40, 25, new Font("Comic Sans", 18), now.ToString("yyyy"));
            Label yearLabel1 = createLabel.CreateLabelsInPanel(bodyDownPanel, "startLabelYear", constants.yearLabel, startLabel.Right + 200, subTitle2.Bottom + 30, 50, 50, Color.Transparent, Color.Black, 22, false, ContentAlignment.MiddleLeft);
           // yearLabel1.Margin = new Padding(5, 0, 0, 0);
            yearComboboxStartGlobal = yearCombobox1;
            yearCombobox1.DrawItem += new DrawItemEventHandler(createCombobox.dateCombobox_DrawItem);
            yearCombobox1.MeasureItem += new MeasureItemEventHandler(createCombobox.dateCombobox_MeasureItem);

            // dropDownMonth.CreateDropDown("falsePurchaseCancellation", bodyDownPanel, constants.months, startLabel.Right + 230, subTitle2.Bottom + 30, 150, 50, 150, 50 * 13, 150, 50, Color.Transparent, Color.Transparent);
            ComboBox monthCombobox1 = createCombobox.CreateComboboxs(bodyDownPanel, "monthCombobox1", constants.months1, startLabel.Right + 270, subTitle2.Bottom + 40, 150, 40, 25, new Font("Comic Sans", 18), now.ToString("MM"));
            Label monthLabel1 = createLabel.CreateLabelsInPanel(bodyDownPanel, "startLabelMonth", constants.monthLabel, startLabel.Right + 440, subTitle2.Bottom + 30, 50, 50, Color.Transparent, Color.Black, 22, false, ContentAlignment.MiddleLeft);
            monthComboboxStartGlobal = monthCombobox1;
            monthCombobox1.DrawItem += new DrawItemEventHandler(createCombobox.dateCombobox_DrawItem);
            monthCombobox1.MeasureItem += new MeasureItemEventHandler(createCombobox.dateCombobox_MeasureItem);

            ComboBox dateCombobox1 = createCombobox.CreateComboboxs(bodyDownPanel, "dateCombobox1", constants.dates1, startLabel.Right + 500, subTitle2.Bottom + 40, 150, 40, 25, new Font("Comic Sans", 18), now.ToString("dd"));

            //  dropDownDate.CreateDropDown("falsePurchaseCancellation", bodyDownPanel, constants.dates, startLabel.Right + 440, subTitle2.Bottom + 30, 150, 50, 150, 50 * 32, 150, 50, Color.Transparent, Color.Transparent);
            Label dateLabel1 = createLabel.CreateLabelsInPanel(bodyDownPanel, "startLabelDate", constants.dayLabel, startLabel.Right + 660, subTitle2.Bottom + 30, 50, 50, Color.Transparent, Color.Black, 22, false, ContentAlignment.MiddleLeft);
            dateComboboxStartGlobal = dateCombobox1;
            dateCombobox1.DrawItem += new DrawItemEventHandler(createCombobox.dateCombobox_DrawItem);
            dateCombobox1.MeasureItem += new MeasureItemEventHandler(createCombobox.dateCombobox_MeasureItem);

            Label endLabel = createLabel.CreateLabelsInPanel(bodyDownPanel, "endLabel", constants.falsePurchaseEndLabel, bodyDownPanel.Width / 5, startLabel.Bottom + 30, 100, 50, Color.Transparent, Color.Black, 22);

            // dropDownYear.CreateDropDown("falsePurchaseCancellation", bodyDownPanel, new string[] { "2020", "2019", "2018" }, startLabel.Right + 10, subTitle2.Bottom + 30, 150, 50, 150, 200, 150, 50, Color.Transparent, Color.Transparent);
            ComboBox yearCombobox2 = createCombobox.CreateComboboxs(bodyDownPanel, "yearCombobox2", new string[] { "2020", "2019", "2018" }, startLabel.Right + 40, startLabel.Bottom + 40, 150, 40, 25, new Font("Comic Sans", 18), now.ToString("yyyy"));
            Label yearLabel2 = createLabel.CreateLabelsInPanel(bodyDownPanel, "endLabelYear", constants.yearLabel, startLabel.Right + 200, startLabel.Bottom + 30, 50, 50, Color.Transparent, Color.Black, 22, false, ContentAlignment.MiddleLeft);
            yearComboboxEndGlobal = yearCombobox2;
            yearCombobox2.DrawItem += new DrawItemEventHandler(createCombobox.dateCombobox_DrawItem);
            yearCombobox2.MeasureItem += new MeasureItemEventHandler(createCombobox.dateCombobox_MeasureItem);

            // dropDownMonth.CreateDropDown("falsePurchaseCancellation", bodyDownPanel, constants.months, startLabel.Right + 230, subTitle2.Bottom + 30, 150, 50, 150, 50 * 13, 150, 50, Color.Transparent, Color.Transparent);
            ComboBox monthCombobox2 = createCombobox.CreateComboboxs(bodyDownPanel, "monthCombobox2", constants.months2, startLabel.Right + 270, startLabel.Bottom + 40, 150, 40, 25, new Font("Comic Sans", 18), now.ToString("MM"));
            Label monthLabel2 = createLabel.CreateLabelsInPanel(bodyDownPanel, "endLabelMonth", constants.monthLabel, startLabel.Right + 440, startLabel.Bottom + 30, 50, 50, Color.Transparent, Color.Black, 22);
            monthComboboxEndGlobal = monthCombobox2;
            monthCombobox2.DrawItem += new DrawItemEventHandler(createCombobox.dateCombobox_DrawItem);
            monthCombobox2.MeasureItem += new MeasureItemEventHandler(createCombobox.dateCombobox_MeasureItem);

            //  dropDownDate.CreateDropDown("falsePurchaseCancellation", bodyDownPanel, constants.dates, startLabel.Right + 440, subTitle2.Bottom + 30, 150, 50, 150, 50 * 32, 150, 50, Color.Transparent, Color.Transparent);
            ComboBox dateCombobox2 = createCombobox.CreateComboboxs(bodyDownPanel, "dateCombobox2", constants.dates2, startLabel.Right + 500, startLabel.Bottom + 40, 150, 40, 25, new Font("Comic Sans", 18), now.ToString("dd"));
            Label dateLabel2 = createLabel.CreateLabelsInPanel(bodyDownPanel, "endLabelDate", constants.dayLabel, startLabel.Right + 660, startLabel.Bottom + 30, 50, 50, Color.Transparent, Color.Black, 22);
            dateComboboxEndGlobal = dateCombobox2;
            dateCombobox2.DrawItem += new DrawItemEventHandler(createCombobox.dateCombobox_DrawItem);
            dateCombobox2.MeasureItem += new MeasureItemEventHandler(createCombobox.dateCombobox_MeasureItem);

            Button cancellationShowButton = customButton.CreateButton(constants.falsePurchaseListLabel, "cancellationShowButton", bodyDownPanel.Width * 4 / 5, endLabel.Bottom + 30, 200, 50, Color.FromArgb(255, 0, 112, 192), Color.Transparent, 0, 1, 12, FontStyle.Regular, Color.White);
            bodyDownPanel.Controls.Add(cancellationShowButton);
            cancellationShowButton.Click += new EventHandler(this.ShowCanceledResult);
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

        public void BackShowParent(object sender, EventArgs e)
        {
            DialogFormGlobal.Close();
        }

        private void ShowCanceledResult(object sender, EventArgs e)
        {
            string startYear = yearComboboxStartGlobal.GetItemText(yearComboboxStartGlobal.SelectedItem);
            string startMonth = monthComboboxStartGlobal.GetItemText(monthComboboxStartGlobal.SelectedItem);
            string startDate = dateComboboxStartGlobal.GetItemText(dateComboboxStartGlobal.SelectedItem);
            string endYear = yearComboboxEndGlobal.GetItemText(yearComboboxEndGlobal.SelectedItem);
            string endMonth = monthComboboxEndGlobal.GetItemText(monthComboboxEndGlobal.SelectedItem);
            string endDate = dateComboboxEndGlobal.GetItemText(dateComboboxEndGlobal.SelectedItem);
            DateTime startDay = new DateTime(int.Parse(startYear), int.Parse(startMonth), int.Parse(startDate), 00, 00, 00);
            DateTime endDay = new DateTime(int.Parse(endYear), int.Parse(endMonth), int.Parse(endDate), 23, 59, 59);
            DateTime now = DateTime.Now;
            if (sqlite_conn.State == ConnectionState.Closed)
            {
                sqlite_conn.Open();
            }
            SQLiteCommand sqlite_cmd;

            string week = now.ToString("ddd");
            string currentTime = now.ToString("HH:mm");
            try
            {
                string showResult = "SELECT count(*) FROM " + constants.tbNames[9] + " WHERE cancelDate>=@startDay AND cancelDate<=@endDay";
                sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = showResult;
                sqlite_cmd.Parameters.AddWithValue("@startDay", startDay.ToString("yyyy-MM-dd"));
                sqlite_cmd.Parameters.AddWithValue("@endDay", endDay.ToString("yyyy-MM-dd"));
                int countRow = Convert.ToInt32(sqlite_cmd.ExecuteScalar());
                if(countRow == 0)
                {
                    ErrorAlert();
                }
                else
                {
                    CancelResultDetail();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }


        }

        private void CancelResultDetail()
        {
            string startYear = yearComboboxStartGlobal.GetItemText(yearComboboxStartGlobal.SelectedItem);
            string startMonth = monthComboboxStartGlobal.GetItemText(monthComboboxStartGlobal.SelectedItem);
            string startDate = dateComboboxStartGlobal.GetItemText(dateComboboxStartGlobal.SelectedItem);
            string endYear = yearComboboxEndGlobal.GetItemText(yearComboboxEndGlobal.SelectedItem);
            string endMonth = monthComboboxEndGlobal.GetItemText(monthComboboxEndGlobal.SelectedItem);
            string endDate = dateComboboxEndGlobal.GetItemText(dateComboboxEndGlobal.SelectedItem);
            DateTime startDay = new DateTime(int.Parse(startYear), int.Parse(startMonth), int.Parse(startDate), 00, 00, 00);
            DateTime endDay = new DateTime(int.Parse(endYear), int.Parse(endMonth), int.Parse(endDate), 23, 59, 59);
            DateTime now = DateTime.Now;

            Form dialogForm = new Form();
            dialogForm.Size = new Size(mainFormGlobal.Width, mainFormGlobal.Height);
            dialogForm.BackColor = Color.White;
            dialogForm.Location = new Point(0, 0);
            dialogForm.StartPosition = FormStartPosition.WindowsDefaultLocation;
            dialogForm.WindowState = FormWindowState.Maximized;
            dialogForm.ControlBox = true;
            dialogForm.MaximizeBox = false;
            dialogForm.MinimizeBox = false;
            dialogForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            DialogFormGlobal = dialogForm;

            Panel dialogPanel = createPanel.CreateMainPanel(dialogForm, 0, 0, dialogForm.Width, dialogForm.Height, BorderStyle.None, Color.Transparent);

            Label dialogTitle1 = createLabel.CreateLabelsInPanel(dialogPanel, "dialogTitle1", constants.falsePurchasePageTitle, 0, 0, dialogPanel.Width / 2 - 30, 60, Color.Transparent, Color.Black, 22);



            FlowLayoutPanel productTableHeader = createPanel.CreateFlowLayoutPanel(dialogPanel, 10, 60, dialogPanel.Width - 20, 60, Color.Gray, new Padding(1));
            Label prodNameHeader1 = createLabel.CreateLabelsInPanel(productTableHeader, "prodHeader_1", constants.orderTimeField, 0, 0, productTableHeader.Width / 4 + 20, productTableHeader.Height - 5, Color.FromArgb(255, 236, 253, 245), Color.FromArgb(255, 142, 133, 118), 12);
            prodNameHeader1.Margin = new Padding(1, 0, 1, 1);
            Label prodNameHeader2 = createLabel.CreateLabelsInPanel(productTableHeader, "prodHeader_2", constants.saleNumberField, productTableHeader.Width / 4 + 20, 0, productTableHeader.Width / 4 - 35, productTableHeader.Height - 5, Color.FromArgb(255, 236, 253, 245), Color.FromArgb(255, 142, 133, 118), 12);
            prodNameHeader2.Margin = new Padding(1, 0, 1, 1);
            Label prodNameHeader3 = createLabel.CreateLabelsInPanel(productTableHeader, "prodHeader_3", constants.prodNameField, productTableHeader.Width / 2 - 15, 0, productTableHeader.Width / 4 + 30, productTableHeader.Height - 5, Color.FromArgb(255, 236, 253, 245), Color.FromArgb(255, 142, 133, 118), 12);
            prodNameHeader3.Margin = new Padding(1, 0, 1, 1);
            Label prodNameHeader4 = createLabel.CreateLabelsInPanel(productTableHeader, "prodHeader_4", constants.priceField, productTableHeader.Width * 3 / 4 - 15, 0, productTableHeader.Width / 4 - 25, productTableHeader.Height - 5, Color.FromArgb(255, 236, 253, 245), Color.FromArgb(255, 142, 133, 118), 12);
            prodNameHeader4.Margin = new Padding(1, 0, 1, 1);

            Panel tbodyPanel = createPanel.CreateSubPanel(dialogPanel, 0, 120, dialogPanel.Width, dialogPanel.Height - 220, BorderStyle.None, Color.Transparent);
            tbodyPanel.HorizontalScroll.Maximum = 0;
            tbodyPanel.AutoScroll = false;
            tbodyPanel.VerticalScroll.Visible = false;
            tbodyPanel.AutoScroll = true;


            SQLiteCommand sqlite_cmd0;
            SQLiteDataReader sqlite_datareader0;

            int k = 0;
            string showResult0 = "SELECT * FROM " + constants.tbNames[9] + " WHERE cancelDate>=@startDay AND cancelDate<=@endDay ORDER BY id, cancelDate";
            sqlite_cmd0 = sqlite_conn.CreateCommand();
            sqlite_cmd0.CommandText = showResult0;
            sqlite_cmd0.Parameters.AddWithValue("@startDay", startDay.ToString("yyyy-MM-dd"));
            sqlite_cmd0.Parameters.AddWithValue("@endDay", endDay.ToString("yyyy-MM-dd"));
            sqlite_datareader0 = sqlite_cmd0.ExecuteReader();
            while (sqlite_datareader0.Read())
            {
                if (!sqlite_datareader0.IsDBNull(0))
                {

                    string prdDate = sqlite_datareader0.GetDateTime(7).ToString("yyyy/MM/dd HH:mm:ss");
                    string prdNumberRow = sqlite_datareader0.GetInt32(6).ToString("0000000000");
                    string prdName = sqlite_datareader0.GetString(3);
                    string prdPrice = (sqlite_datareader0.GetInt32(4) * sqlite_datareader0.GetInt32(5)).ToString();
                    FlowLayoutPanel productTableRow = createPanel.CreateFlowLayoutPanel(tbodyPanel, 10, 60 * k, dialogPanel.Width - 20, 60, Color.Gray, new Padding(1));
                    Label prodDateRow = createLabel.CreateLabelsInPanel(productTableRow, "prodDate", prdDate, 0, 0, productTableRow.Width / 4 + 20, productTableRow.Height - 2, Color.White, Color.FromArgb(255, 142, 133, 118), 12);
                    prodDateRow.Margin = new Padding(1, 0, 1, 2);
                    Label prodNumberRow = createLabel.CreateLabelsInPanel(productTableRow, "prodNumber", prdNumberRow, productTableRow.Width / 4 + 20, 0, productTableRow.Width / 4 - 35, productTableRow.Height - 2, Color.White, Color.FromArgb(255, 142, 133, 118), 12);
                    prodNumberRow.Margin = new Padding(1, 0, 1, 2);
                    Label prodNameRow = createLabel.CreateLabelsInPanel(productTableRow, "prodName", prdName, productTableRow.Width / 2 - 15, 0, productTableRow.Width / 4 + 30, productTableRow.Height - 2, Color.White, Color.FromArgb(255, 142, 133, 118), 12);
                    prodNameRow.Margin = new Padding(1, 0, 1, 2);
                    Label prodPriceRow = createLabel.CreateLabelsInPanel(productTableRow, "prodPrice", prdPrice, productTableRow.Width * 3 / 4 - 15, 0, productTableRow.Width / 4 - 25, productTableRow.Height - 2, Color.White, Color.FromArgb(255, 142, 133, 118), 12);
                    prodPriceRow.Margin = new Padding(1, 0, 1, 2);
                    k++;
                }
            }

            Button backButton = customButton.CreateButton(constants.backText, "backButton", dialogPanel.Width - 150, dialogPanel.Height - 100, 100, 50, Color.FromArgb(255, 0, 112, 192), Color.Transparent, 0, 10, 14, FontStyle.Bold, Color.White);
            dialogPanel.Controls.Add(backButton);
            backButton.Click += new EventHandler(this.BackShowParent);

            dialogForm.ShowDialog();

        }

        private void ErrorAlert()
        {
            Form dialogForm = new Form();
            dialogForm.Size = new Size(mainFormGlobal.Width / 3, mainFormGlobal.Height / 4);
            dialogForm.BackColor = Color.White;
            dialogForm.StartPosition = FormStartPosition.WindowsDefaultLocation;
            dialogForm.WindowState = FormWindowState.Normal;
            dialogForm.ControlBox = false;
            dialogForm.MaximizeBox = false;
            dialogForm.MinimizeBox = false;
            dialogForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            DialogFormGlobal = dialogForm;

            Panel dialogPanel = createPanel.CreateMainPanel(dialogForm, 0, 0, dialogForm.Width, dialogForm.Height, BorderStyle.None, Color.Transparent);

            Label dialogTitle1 = createLabel.CreateLabelsInPanel(dialogPanel, "dialogTitle1", constants.cancelResultErrorMessage, dialogPanel.Width / 10, 0, dialogPanel.Width * 4 / 5, dialogPanel.Height * 2 / 3, Color.Transparent, Color.Black, 22);


            Button backButton = customButton.CreateButton(constants.backText, "backButton", dialogPanel.Width / 2 - 75, dialogPanel.Height * 2 / 3, 150, 40, Color.FromArgb(255, 0, 112, 192), Color.Transparent, 0, 10, 14, FontStyle.Bold, Color.White);
            dialogPanel.Controls.Add(backButton);
            backButton.Click += new EventHandler(this.BackShowParent);

            dialogForm.ShowDialog();
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

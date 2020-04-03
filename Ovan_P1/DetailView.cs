using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
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
        Form DialogFormGlobal = null;
        Form cancelDialogFormGlobal = null;
        FlowLayoutPanel dialogFlowLayout = null;
        int[] bottomPosition = null;
        public DetailView()
        {
            InitializeComponent();
        }

        public void DetailViewIndicator(object sender, EventArgs e)
        {
            Button btnTemp = (Button)sender;
            if (btnTemp.Name == "processButton_1_1")
            {
                DailyReport();
            }
            else if (btnTemp.Name == "processButton_2_1")
            {
                ReceiptIssueReport();
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

        public void DailyReport()
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

            FlowLayoutPanel dialogTitlePanel = createPanel.CreateFlowLayoutPanel(dialogPanel, 0, 50, dialogPanel.Width, 50, Color.Transparent, new Padding(0));
            Label dialogTitle1 = createLabel.CreateLabelsInPanel(dialogTitlePanel, "dialogTitle1", "売上日報", 0, 0, dialogTitlePanel.Width / 2 - 30, 60, Color.Transparent, Color.Black, 22);
            Label dialogTitle2 = createLabel.CreateLabelsInPanel(dialogTitlePanel, "dialogTitle2", "2020/04/01", dialogTitlePanel.Width / 2, 0, dialogTitlePanel.Width / 2 - 30, 60, Color.Transparent, Color.Black, 22);

            int soldPriceSum = 0;
            int soldAmountSum = 0;
            int k = 0;
            foreach (string prodItem in constants.productBigName[1])
            {
                int productSoldPrice = constants.productSoldAmount[k] * constants.productBigPrice[1][k];
                soldPriceSum += productSoldPrice;
                soldAmountSum += constants.productSoldAmount[k];
                FlowLayoutPanel productTableBody = createPanel.CreateFlowLayoutPanel(dialogPanel, 0, 110 + 61 * k, dialogPanel.Width, 60, Color.Transparent, new Padding(0));
                Label prodNameBody1 = createLabel.CreateLabelsInPanel(productTableBody, "prodBody_" + k + "_1", prodItem, 0, 0, productTableBody.Width / 4 - 10, productTableBody.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);
                Label prodNameBody2 = createLabel.CreateLabelsInPanel(productTableBody, "prodBody_" + k + "_2", constants.productSoldAmount[k].ToString() + "枚", productTableBody.Width / 4, 0, productTableBody.Width / 4 - 10, productTableBody.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);
                Label prodNameBody3 = createLabel.CreateLabelsInPanel(productTableBody, "prodBody_" + k + "_3", constants.productBigPrice[1][k].ToString() + "円", productTableBody.Width / 2, 0, productTableBody.Width / 4 - 10, productTableBody.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);
                Label prodNameBody4 = createLabel.CreateLabelsInPanel(productTableBody, "prodBody_" + k + "_4", productSoldPrice.ToString() + "円", productTableBody.Width * 3 / 4, 0, productTableBody.Width / 4 - 10, productTableBody.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);
                dialogFlowLayout = productTableBody;
                k++;
            }
            FlowLayoutPanel dialogSumPanel = createPanel.CreateFlowLayoutPanel(dialogPanel, 0, constants.productBigName[1].Length * 61 + 120, dialogPanel.Width, 60, Color.Transparent, new Padding(10));
            Label dialogSumLabel1 = createLabel.CreateLabelsInPanel(dialogSumPanel, "dialogSumLabel1", "合計", 0, 0, dialogSumPanel.Width / 3 - 20, 60, Color.Transparent, Color.Black, 22);
            Label dialogSumLabel2 = createLabel.CreateLabelsInPanel(dialogSumPanel, "dialogSumLabel2", soldAmountSum + "枚", dialogSumPanel.Width / 3, 0, dialogSumPanel.Width / 3 - 20, 60, Color.Transparent, Color.Black, 22);
            Label dialogSumLabel3 = createLabel.CreateLabelsInPanel(dialogSumPanel, "dialogSumLabel2", soldPriceSum + "円", dialogSumPanel.Width * 2 / 3, 0, dialogSumPanel.Width / 3 - 20, 60, Color.Transparent, Color.Black, 22);

            FlowLayoutPanel dialogDatePanel = createPanel.CreateFlowLayoutPanel(dialogPanel, 0, dialogSumPanel.Bottom + 10, dialogPanel.Width, 60, Color.Transparent, new Padding(10));
            dialogDatePanel.Padding = new Padding(dialogDatePanel.Width * 2 / 3, 0, 0, 0);
            Label dialogDateLabel = createLabel.CreateLabelsInPanel(dialogDatePanel, "dialogDateLabel", "2020/04/02 02:15:45", 0, 0, dialogDatePanel.Width / 3 - 20, 60, Color.Transparent, Color.Black, 22);
            dialogForm.ShowDialog();
        }

        public void ReceiptIssueReport()
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

            FlowLayoutPanel dialogTitlePanel = createPanel.CreateFlowLayoutPanel(dialogPanel, 0, 50, dialogPanel.Width, 50, Color.Transparent, new Padding(0));
            Label dialogTitle1 = createLabel.CreateLabelsInPanel(dialogTitlePanel, "dialogTitle1", "領収書発行一覧", 0, 0, dialogTitlePanel.Width / 2 - 30, 60, Color.Transparent, Color.Black, 22);
            Label dialogTitle2 = createLabel.CreateLabelsInPanel(dialogTitlePanel, "dialogTitle2", "2020/04/01", dialogTitlePanel.Width / 2, 0, dialogTitlePanel.Width / 2 - 30, 60, Color.Transparent, Color.Black, 22);

            DateTime now = DateTime.Now;

            for (int k = 0; k < 10; k++)
            {
                FlowLayoutPanel productTableBody = createPanel.CreateFlowLayoutPanel(dialogPanel, 100, 110 + 61 * k, dialogPanel.Width - 200, 60, Color.Transparent, new Padding(0));
                Label prodNameBody1 = createLabel.CreateLabelsInPanel(productTableBody, "prodBody_" + k + "_1", now.ToLocalTime().ToString(), 0, 0, productTableBody.Width / 3 - 50, productTableBody.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);
                Label prodNameBody2 = createLabel.CreateLabelsInPanel(productTableBody, "prodBody_" + k + "_2", constants.recieptIssueAmount[k].ToString() + "枚", productTableBody.Width / 4, 0, productTableBody.Width / 3 - 50, productTableBody.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);
                Label prodNameBody3 = createLabel.CreateLabelsInPanel(productTableBody, "prodBody_" + k + "_3", constants.recieptIssuePrice[k].ToString() + "円", productTableBody.Width / 2, 0, productTableBody.Width / 3 - 50, productTableBody.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);
                dialogFlowLayout = productTableBody;
            }
            FlowLayoutPanel dialogSumPanel = createPanel.CreateFlowLayoutPanel(dialogPanel, 0, constants.productBigName[1].Length * 61 + 120, dialogPanel.Width, 60, Color.Transparent, new Padding(10));
            Label dialogSumLabel1 = createLabel.CreateLabelsInPanel(dialogSumPanel, "dialogSumLabel1", "合計", 0, 0, dialogSumPanel.Width / 3 - 20, 60, Color.Transparent, Color.Black, 22);
            Label dialogSumLabel2 = createLabel.CreateLabelsInPanel(dialogSumPanel, "dialogSumLabel2", "10 枚", dialogSumPanel.Width / 3, 0, dialogSumPanel.Width / 3 - 20, 60, Color.Transparent, Color.Black, 22);

            FlowLayoutPanel dialogDatePanel = createPanel.CreateFlowLayoutPanel(dialogPanel, 0, dialogSumPanel.Bottom + 10, dialogPanel.Width, 60, Color.Transparent, new Padding(10));
            dialogDatePanel.Padding = new Padding(dialogDatePanel.Width * 2 / 3, 0, 0, 0);
            Label dialogDateLabel = createLabel.CreateLabelsInPanel(dialogDatePanel, "dialogDateLabel", now.ToLocalTime().ToString(), 0, 0, dialogDatePanel.Width / 3 - 20, 60, Color.Transparent, Color.Black, 22);
            dialogForm.ShowDialog();
        }

        public void LogReport()
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


            FlowLayoutPanel dialogTitlePanel = createPanel.CreateFlowLayoutPanel(dialogPanel, 0, 0, dialogPanel.Width, 80, Color.Transparent, new Padding(0, 20, 0, 10), true);
            Label dialogTitle1 = createLabel.CreateLabelsInPanel(dialogTitlePanel, "dialogTitle1", "ログ表示", 0, 0, dialogTitlePanel.Width / 2 - 30, 60, Color.Transparent, Color.Black, 22);
            Label dialogTitle2 = createLabel.CreateLabelsInPanel(dialogTitlePanel, "dialogTitle2", "2020/04/01", dialogTitlePanel.Width / 2, 0, dialogTitlePanel.Width / 2 - 30, 60, Color.Transparent, Color.Black, 22);

            DateTime now = DateTime.Now;

            bottomPosition = new int[10];
            bottomPosition[0] = 110;

            for (int k = 0; k < 10; k++)
            {
                if(k != 9)
                {
                    bottomPosition[k + 1] = bottomPosition[k] + 51 + 2 * 51;
                }
                int addHeight = 0;
                if(k != 0)
                {
                    addHeight = 10;
                    PictureBox pB = new PictureBox();
                    pB.Location = new Point(100, bottomPosition[k] + 10);
                    pB.Size = new Size(dialogPanel.Width - 200, 10);
                    dialogPanel.Controls.Add(pB);
                    Bitmap image = new Bitmap(pB.Size.Width, pB.Size.Height);
                    Graphics g = Graphics.FromImage(image);
                    g.DrawLine(new Pen(Color.FromArgb(255, 142, 133, 118), 3) { DashPattern = new float[] { 5, 1.5F } }, 5, 5, dialogPanel.Width - 260, 5);
                    pB.Image = image;
                }
                FlowLayoutPanel productTableBody = createPanel.CreateFlowLayoutPanel(dialogPanel, 100, bottomPosition[k] + addHeight, dialogPanel.Width - 200, 50, Color.Transparent, new Padding(0));
                Label prodNameBody1 = createLabel.CreateLabelsInPanel(productTableBody, "prodBody_" + k + "_1", now.ToLocalTime().ToString(), 0, 0, productTableBody.Width / 3 - 10, productTableBody.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);
                Label prodNameBody2 = createLabel.CreateLabelsInPanel(productTableBody, "prodBody_" + k + "_2", "", productTableBody.Width / 3, 0, productTableBody.Width / 3 - 10, productTableBody.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);
                Label prodNameBody3 = createLabel.CreateLabelsInPanel(productTableBody, "prodBody_" + k + "_3", "計 1,400円", productTableBody.Width * 2  / 3, 0, productTableBody.Width / 3 - 10, productTableBody.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);
                dialogFlowLayout = productTableBody;
                for (int m = 0; m < 2; m++)
                {
                    FlowLayoutPanel productTableBodyContent = createPanel.CreateFlowLayoutPanel(dialogPanel, 100, productTableBody.Bottom + 51 * m, dialogPanel.Width - 200, 50, Color.Transparent, new Padding(0));
                    Label prodNameBodyContent1 = createLabel.CreateLabelsInPanel(productTableBodyContent, "prodBody_" + k + "_1", constants.productBigName[1][k], 0, 0, productTableBodyContent.Width / 3 - 10, productTableBodyContent.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);
                    Label prodNameBodyContent2 = createLabel.CreateLabelsInPanel(productTableBodyContent, "prodBody_" + k + "_2", constants.recieptIssueAmount[k].ToString() + "枚", productTableBodyContent.Width / 3, 0, productTableBodyContent.Width / 3 - 10, productTableBodyContent.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);
                    Label prodNameBodyContent3 = createLabel.CreateLabelsInPanel(productTableBodyContent, "prodBody_" + k + "_3", constants.recieptIssuePrice[k].ToString(), productTableBodyContent.Width * 2 / 3, 0, productTableBodyContent.Width / 3 - 10, productTableBodyContent.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);
                    dialogFlowLayout = productTableBodyContent;
                }

            }

            FlowLayoutPanel dialogTopPanel = createPanel.CreateFlowLayoutPanel(dialogPanel, 0, dialogFlowLayout.Bottom - 10, dialogPanel.Width - 30, 80, Color.Transparent, new Padding(dialogPanel.Width - 150, 15, 0, 0));

            Button closeButton = createButton.CreateButton(constants.backText, "closeButton", 0, 0, 80, 50, Color.FromArgb(255, 0, 112, 192), Color.Transparent, 0, 1);
            closeButton.ForeColor = Color.White;
            //closeButton.Dock = DockStyle.Right;
            dialogTopPanel.Controls.Add(closeButton);
            closeButton.Click += new EventHandler(this.CloseDialog);
            dialogForm.ShowDialog();

        }

        MySqlConnection connection;
        public void FalsePurchaseCancellation()
        {
            Form dialogForm = new Form();
            dialogForm.Size = new Size(width / 2, height * 8 / 9);
            dialogForm.BackColor = Color.White;
            dialogForm.StartPosition = FormStartPosition.CenterParent;
            dialogForm.WindowState = FormWindowState.Normal;
            dialogForm.ControlBox = false;
            dialogForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            DialogFormGlobal = dialogForm;

            connection = new MySqlConnection("datasource=localhost;port=3306;username=root;password=");
            connection.Open();
            string connectionState = "";
            if(connection.State == ConnectionState.Open)
            {
                connectionState = "connected";
            }
            else
            {
                connectionState = "disconnected";
            }

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;

            cmd.CommandText = "CREATE DATABASE IF NOT EXISTS POSsystem;";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS POSsystem.categories(id INTEGER PRIMARY KEY AUTO_INCREMENT,
                    name TEXT, price INT)";
            cmd.ExecuteNonQuery();

            Panel dialogPanel = createPanel.CreateMainPanel(dialogForm, 0, 0, dialogForm.Width, dialogForm.Height, BorderStyle.None, Color.Transparent);
            dialogPanel.AutoScroll = true;


            FlowLayoutPanel dialogTitlePanel = createPanel.CreateFlowLayoutPanel(dialogPanel, 0, 0, dialogPanel.Width, 50, Color.Transparent, new Padding(0), true);
            Label dialogTitle1 = createLabel.CreateLabelsInPanel(dialogTitlePanel, "dialogTitle1", "取り消す注文を選択", 0, 0, dialogTitlePanel.Width / 2 - 30, 60, Color.Transparent, Color.Black, 22);
            Label dialogTitle2 = createLabel.CreateLabelsInPanel(dialogTitlePanel, "dialogTitle2", "2020/04/01" + connectionState, dialogTitlePanel.Width / 2, 0, dialogTitlePanel.Width / 2 - 30, 60, Color.Transparent, Color.Black, 22);

            DateTime now = DateTime.Now;

            FlowLayoutPanel productTableHeader = createPanel.CreateFlowLayoutPanel(dialogPanel, 100, 110, dialogPanel.Width - 200, 60, Color.Transparent, new Padding(0));
            Label prodNameHeader1 = createLabel.CreateLabelsInPanel(productTableHeader, "prodHeader_1", "注文日付", 0, 0, productTableHeader.Width / 4 - 10, productTableHeader.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);
            Label prodNameHeader2 = createLabel.CreateLabelsInPanel(productTableHeader, "prodHeader_2", "売上連番", productTableHeader.Width / 4, 0, productTableHeader.Width / 4 - 10, productTableHeader.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);
            Label prodNameHeader3 = createLabel.CreateLabelsInPanel(productTableHeader, "prodHeader_3", "品名", productTableHeader.Width / 2, 0, productTableHeader.Width / 4 - 10, productTableHeader.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);
            Label prodNameHeader4 = createLabel.CreateLabelsInPanel(productTableHeader, "prodHeader_3", "金額", productTableHeader.Width * 3 / 4, 0, productTableHeader.Width / 4 - 10, productTableHeader.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);

            for (int k = 0; k < 10; k++)
            {
                FlowLayoutPanel productTableBody = createPanel.CreateFlowLayoutPanel(dialogPanel, 100, 170 + 61 * k, dialogPanel.Width - 200, 60, Color.Transparent, new Padding(0));
                productTableBody.Name = "prdID_" + k;
                Label prodNameBody1 = createLabel.CreateLabelsInPanel(productTableBody, "prodBody_" + k + "_1", now.ToLocalTime().ToString(), 0, 0, productTableBody.Width / 4 - 10, productTableBody.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);
                Label prodNameBody2 = createLabel.CreateLabelsInPanel(productTableBody, "prodBody_" + k + "_2", constants.recieptIssueAmount[k].ToString() + "枚", productTableBody.Width / 4, 0, productTableBody.Width / 4 - 10, productTableBody.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);
                Label prodNameBody3 = createLabel.CreateLabelsInPanel(productTableBody, "prodBody_" + k + "_3", constants.productBigName[1][k].ToString(), productTableBody.Width / 2, 0, productTableBody.Width / 4 - 10, productTableBody.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);
                Label prodNameBody4 = createLabel.CreateLabelsInPanel(productTableBody, "prodBody_" + k + "_4", constants.recieptIssuePrice[k].ToString() + "円", productTableBody.Width * 3 / 4, 0, productTableBody.Width / 4 - 10, productTableBody.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);
                dialogFlowLayout = productTableBody;

                prodNameBody1.Click += new EventHandler(this.CancellationSetting);
                prodNameBody2.Click += new EventHandler(this.CancellationSetting);
                prodNameBody3.Click += new EventHandler(this.CancellationSetting);
                prodNameBody4.Click += new EventHandler(this.CancellationSetting);

            }

            FlowLayoutPanel dialogTopPanel = createPanel.CreateFlowLayoutPanel(dialogPanel, 0, dialogPanel.Height - 60, dialogPanel.Width - 30, 50, Color.Transparent, new Padding(dialogPanel.Width - 65, 5, 0, 0));

            Button closeButton = createButton.CreateButton("x", "closeButton", 0, 0, 30, 30, Color.FromArgb(255, 0, 112, 192), Color.Transparent, 0, 1);
            closeButton.ForeColor = Color.White;
            //closeButton.Dock = DockStyle.Right;
            dialogTopPanel.Controls.Add(closeButton);
            closeButton.Click += new EventHandler(this.CloseDialog);
            dialogForm.ShowDialog();

        }
        private void CancellationSetting(object sender, EventArgs e)
        {

            Label prdTemp = (Label)sender;
            string prdID = prdTemp.Name.Split('_')[1];
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

        public void CloseDialog(object sender, EventArgs e)
        {
            DialogFormGlobal.Close();
        }
        public void CancelCloseDialog(object sender, EventArgs e)
        {
            cancelDialogFormGlobal.Close();
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
            Label tableHeaderLabel1 = createLabel.CreateLabels(tableHeaderInUpPanel, "tableHeaderLabel1", "印刷品目名", 0, 0, tableHeaderInUpPanel.Width * 2 / 5, 50, Color.FromArgb(255, 147, 184, 219), Color.Black, 16, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
            Label tableHeaderLabel2 = createLabel.CreateLabels(tableHeaderInUpPanel, "tableHeaderLabel2", "販売価格", tableHeaderLabel1.Right, 0, tableHeaderInUpPanel.Width * 3 / 20, 50, Color.FromArgb(255, 147, 184, 219), Color.Black, 16, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
            Label tableHeaderLabel3 = createLabel.CreateLabels(tableHeaderInUpPanel, "tableHeaderLabel3", "販売時間", tableHeaderLabel2.Right, 0, tableHeaderInUpPanel.Width * 3 / 10, 50, Color.FromArgb(255, 147, 184, 219), Color.Black, 16, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
            Label tableHeaderLabel4 = createLabel.CreateLabels(tableHeaderInUpPanel, "tableHeaderLabel4", "限定数", tableHeaderLabel3.Right, 0, tableHeaderInUpPanel.Width * 3 / 20, 50, Color.FromArgb(255, 147, 184, 219), Color.Black, 16, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);

            Panel tBodyPanel = createPanel.CreateSubPanel(upPanel, 0, 50, upPanel.Width, upPanel.Height - 50, BorderStyle.FixedSingle, Color.White);
            tBodyPanel.HorizontalScroll.Maximum = 0;
            tBodyPanel.AutoScroll = false;
            tBodyPanel.VerticalScroll.Visible = false;
            tBodyPanel.AutoScroll = true;
            totalRowNum = constants.productBigName[1].Length;
            columnGlobal1 = new Label[totalRowNum];
            columnGlobal2 = new Label[totalRowNum];
            columnGlobal3 = new Label[totalRowNum];
            columnGlobal4 = new Label[totalRowNum];
            int k = 0;
            foreach(string prodItem in constants.productBigName[1])
            {
                FlowLayoutPanel tableRowPanel = createPanel.CreateFlowLayoutPanel(tBodyPanel, 0, 50 * k, tBodyPanel.Width, 50, Color.Transparent, new Padding(0));
                Label tdLabel1 = createLabel.CreateLabels(tableRowPanel, "tdLabel1_" + k, prodItem, 0, 0, tableRowPanel.Width * 2 / 5, 50, Color.White, Color.Black, 16, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
                columnGlobal1[k] = tdLabel1;
                Label tdLabel2 = createLabel.CreateLabels(tableRowPanel, "tdLabel2_" + k, constants.productBigPrice[1][k].ToString(), tdLabel1.Right, 0, tableRowPanel.Width * 3 / 20, 50, Color.White, Color.Black, 16, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
                columnGlobal2[k] = tdLabel2;
                Label tdLabel3 = createLabel.CreateLabels(tableRowPanel, "tdLabel3_" + k, "10:00~21:59", tdLabel2.Right, 0, tableRowPanel.Width * 3 / 10, 50, Color.White, Color.Black, 16, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
                columnGlobal3[k] = tdLabel3;
                Label tdLabel4 = createLabel.CreateLabels(tableRowPanel, "tdLabel4_" + k, constants.productBigSaleAmount[1][k], tdLabel3.Right, 0, tableRowPanel.Width * 3 / 20, 50, Color.White, Color.Black, 16, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
                columnGlobal4[k] = tdLabel4;

                tdLabel1.Click += new EventHandler(this.ShowProductDetail);
                tdLabel2.Click += new EventHandler(this.ShowProductDetail);
                tdLabel3.Click += new EventHandler(this.ShowProductDetail);
                tdLabel4.Click += new EventHandler(this.ShowProductDetail);


                k++;
            }
            Button closeButton = customButton.CreateButton(constants.backText, "closeButton", downPanel.Width - 150, downPanel.Height - 100, 100, 50, Color.FromArgb(255, 0, 112, 192), Color.Transparent, 0, 1, 12, FontStyle.Regular, Color.White);
            downPanel.Controls.Add(closeButton);
            closeButton.Click += new EventHandler(this.BackShow);



        }

        private void ShowProductDetail(object sender, EventArgs e)
        {
            Label prdTemp = (Label)sender;
            int prdID = int.Parse(prdTemp.Name.Split('_')[1]);
            columnGlobal1[prdID].ForeColor = Color.Red;
            columnGlobal2[prdID].ForeColor = Color.Red;
            columnGlobal3[prdID].ForeColor = Color.Red;
            columnGlobal4[prdID].ForeColor = Color.Red;
            for (int k = 0; k < totalRowNum; k++)
            {
                if (k != prdID)
                {
                    columnGlobal1[k].ForeColor = Color.FromArgb(255, 142, 133, 118);
                    columnGlobal2[k].ForeColor = Color.FromArgb(255, 142, 133, 118);
                    columnGlobal3[k].ForeColor = Color.FromArgb(255, 142, 133, 118);
                    columnGlobal4[k].ForeColor = Color.FromArgb(255, 142, 133, 118);
                }
            }

            downPanelGlobal.Controls.Clear();
            Panel detailPanel = createPanel.CreateSubPanel(downPanelGlobal, 50, 10, downPanelGlobal.Width / 2 - 100, downPanelGlobal.Height - 100, BorderStyle.FixedSingle, Color.Transparent);

            int rowCount = 9;

            FlowLayoutPanel leftColumn = createPanel.CreateFlowLayoutPanel(detailPanel, 0, 0, detailPanel.Width * 2 / 5, detailPanel.Height, Color.Transparent, new Padding(0));
            FlowLayoutPanel rightColumn = createPanel.CreateFlowLayoutPanel(detailPanel, detailPanel.Width * 2 / 5, 0, detailPanel.Width * 3 / 5, detailPanel.Height, Color.Transparent, new Padding(0));

            Label column1 = createLabel.CreateLabels(leftColumn, "prdNameColumn", "品目名", 0, 0, leftColumn.Width, leftColumn.Height / rowCount, Color.FromArgb(255, 198, 224, 180), Color.Black, 12, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
            Label column2 = createLabel.CreateLabels(leftColumn, "prdCategoryColumn", "所属カテゴリー", 0, column1.Bottom, leftColumn.Width, leftColumn.Height / rowCount, Color.FromArgb(255, 198, 224, 180), Color.Black, 12, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
            Label column3 = createLabel.CreateLabels(leftColumn, "prdPriceColumn", "販売価格（税込）", 0, column2.Bottom, leftColumn.Width, leftColumn.Height / rowCount, Color.FromArgb(255, 198, 224, 180), Color.Black, 12, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
            Label column4 = createLabel.CreateLabels(leftColumn, "prdTimeColumn", "販売時刻設定", 0, column3.Bottom, leftColumn.Width, leftColumn.Height * 3 / rowCount, Color.FromArgb(255, 198, 224, 180), Color.Black, 12, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
            Label column5 = createLabel.CreateLabels(leftColumn, "prdLimitColumn", "限定数", 0, column4.Bottom, leftColumn.Width, leftColumn.Height / rowCount, Color.FromArgb(255, 198, 224, 180), Color.Black, 12, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
            Label column6 = createLabel.CreateLabels(leftColumn, "prdLayoutColumn", "画面メッセージ", 0, column5.Bottom, leftColumn.Width, leftColumn.Height / rowCount, Color.FromArgb(255, 198, 224, 180), Color.Black, 12, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
            Label column7 = createLabel.CreateLabels(leftColumn, "prdPrintColumn", "印刷メッセージ", 0, column6.Bottom, leftColumn.Width, leftColumn.Height / rowCount, Color.FromArgb(255, 198, 224, 180), Color.Black, 12, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);

            Label value1 = createLabel.CreateLabels(rightColumn, "prdNameValue", constants.productBigName[1][prdID], 0, 0, rightColumn.Width, rightColumn.Height / rowCount, Color.White, Color.Black, 12, true, ContentAlignment.MiddleLeft, new Padding(0), 2, Color.Gray);
            value1.Padding = new Padding(10, 0, 0, 0);
            Label value2 = createLabel.CreateLabels(rightColumn, "prdCategoryValue", "1, 2", 0, column1.Bottom, rightColumn.Width, rightColumn.Height / rowCount, Color.White, Color.Black, 12, true, ContentAlignment.MiddleLeft, new Padding(0), 2, Color.Gray);
            value2.Padding = new Padding(10, 0, 0, 0);
            Label value3 = createLabel.CreateLabels(rightColumn, "prdPriceValue", constants.productBigPrice[1][prdID].ToString() + "円", 0, column2.Bottom, rightColumn.Width, rightColumn.Height / rowCount, Color.White, Color.Black, 12, true, ContentAlignment.MiddleLeft, new Padding(0), 2, Color.Gray);
            value3.Padding = new Padding(10, 0, 0, 0);

            Label timeLabel1 = createLabel.CreateLabels(rightColumn, "prdTimeLabel1", "平日", 0, column3.Bottom, rightColumn.Width / 3, rightColumn.Height / rowCount, Color.White, Color.Black, 12, true, ContentAlignment.MiddleCenter, new Padding(0), 2, Color.Gray);
            Label value4 = createLabel.CreateLabels(rightColumn, "prdTimeValue1", "10:00 ~ 21:59", 0, column3.Bottom, rightColumn.Width * 2 / 3, rightColumn.Height / rowCount, Color.White, Color.Black, 12, true, ContentAlignment.MiddleLeft, new Padding(0), 2, Color.Gray);
            value4.Padding = new Padding(10, 0, 0, 0);

            Label timeLabel2 = createLabel.CreateLabels(rightColumn, "prdTimeLabel2", "土曜", 0, column3.Bottom, rightColumn.Width / 3, rightColumn.Height / rowCount, Color.White, Color.FromArgb(255, 0, 112, 192), 12, true, ContentAlignment.MiddleCenter, new Padding(0), 2, Color.Gray);
            Label value5 = createLabel.CreateLabels(rightColumn, "prdTimeValue2", "10:00 ~ 21:59", 0, column4.Bottom, rightColumn.Width * 2 / 3, rightColumn.Height / rowCount, Color.White, Color.Black, 12, true, ContentAlignment.MiddleLeft, new Padding(0), 2, Color.Gray);
            value5.Padding = new Padding(10, 0, 0, 0);

            Label timeLabel3 = createLabel.CreateLabels(rightColumn, "prdTimeLabel3", "日曜", 0, column3.Bottom, rightColumn.Width / 3, rightColumn.Height / rowCount, Color.White, Color.Red, 12, true, ContentAlignment.MiddleCenter, new Padding(0), 2, Color.Gray);
            Label value6 = createLabel.CreateLabels(rightColumn, "prdTimeValue3", "10:00 ~ 21:59", 0, column5.Bottom, rightColumn.Width * 2 / 3, rightColumn.Height / rowCount, Color.White, Color.Black, 12, true, ContentAlignment.MiddleLeft, new Padding(0), 2, Color.Gray);
            value6.Padding = new Padding(10, 0, 0, 0);
            Label value7 = createLabel.CreateLabels(rightColumn, "prdLimitValue", constants.productBigSaleAmount[1][prdID], 0, value6.Bottom, rightColumn.Width, rightColumn.Height / rowCount, Color.White, Color.Black, 12, true, ContentAlignment.MiddleLeft, new Padding(0), 2, Color.Gray);
            value7.Padding = new Padding(10, 0, 0, 0);
            Label value8 = createLabel.CreateLabels(rightColumn, "prdLayoutValue", "飽きの来ない伝統の味", 0, value7.Bottom, rightColumn.Width, rightColumn.Height / rowCount, Color.White, Color.Black, 12, true, ContentAlignment.MiddleLeft, new Padding(0), 2, Color.Gray);
            value8.Padding = new Padding(10, 0, 0, 0);
            Label value9 = createLabel.CreateLabels(rightColumn, "prdPrintValue", "麺の堅さは変更出来ます。", 0, value8.Bottom, rightColumn.Width, rightColumn.Height / rowCount, Color.White, Color.Black, 12, true, ContentAlignment.MiddleLeft, new Padding(0), 2, Color.Gray);
            value9.Padding = new Padding(10, 0, 0, 0);

            Panel productImagePanel = createPanel.CreateSubPanel(downPanelGlobal, downPanelGlobal.Width / 2 + 100, 10, downPanelGlobal.Width / 4, downPanelGlobal.Height * 3 / 5, BorderStyle.FixedSingle, Color.White);
            PictureBox productImage = new PictureBox();
            productImage.Location = new Point(0, 0);
            productImage.Size = new Size(productImagePanel.Width, productImagePanel.Height * 2 / 3);
            productImage.Name = "productImage";
            productImage.SizeMode = PictureBoxSizeMode.StretchImage;
            productImage.ImageLocation = @"D:\\ovan\\Ovan_P1\\images\\category1.png";
            productImage.BorderStyle = BorderStyle.FixedSingle;
            productImagePanel.Controls.Add(productImage);

            FlowLayoutPanel productLabelPanel = createPanel.CreateFlowLayoutPanel(productImagePanel, 0, productImage.Bottom, productImagePanel.Width, productImagePanel.Height / 3, Color.Transparent, new Padding(0));
            productLabelPanel.BorderStyle = BorderStyle.FixedSingle;
            Label prodcutLabel1 = createLabel.CreateLabelsInPanel(productLabelPanel, "productLabel1", constants.productBigName[1][prdID] + "   " + constants.productBigPrice[1][prdID].ToString(), 0, 0, productLabelPanel.Width, productLabelPanel.Height, Color.White, Color.Black, 22, false, ContentAlignment.MiddleCenter);

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


    }
}

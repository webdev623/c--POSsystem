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
        public SoldoutSetting1(Form1 mainForm, Panel mainPanel)
        {
            mainFormGlobal = mainForm;
            mainPanelGlobal = mainPanel;
            Panel mainPanels = createPanel.CreateMainPanel(mainForm, 0, 0, width, height, BorderStyle.None, Color.Transparent);
            dropDownMenu.initSoldoutSetting(this);
            numberInput.initSoldoutSetting(this);
            Panel headerPanel = createPanel.CreateSubPanel(mainPanels, 0, 0, width, height / 6, BorderStyle.None, Color.Transparent);
            Panel bodyPanel = createPanel.CreateSubPanel(mainPanels, 0, height / 6, width, height * 5 / 6, BorderStyle.None, Color.Transparent);

            Label titleLabel = createLabel.CreateLabelsInPanel(headerPanel, "SoldoutSetting_Title", "売り切れ設定", 10, 10, headerPanel.Width / 2, headerPanel.Height, Color.White, Color.Red, 40, false, ContentAlignment.MiddleLeft);

            Label subTitleLabel = createLabel.CreateLabelsInPanel(bodyPanel, "SoldoutSetting_subTitle", "カテゴリー選択", 80, 0, width / 2 - 300, 50, Color.White, Color.Red, 30, false, ContentAlignment.MiddleRight);

            dropDownMenu.CreateDropDown("soldoutSetting1", bodyPanel, constants.saleCategories, width / 2 - 150, 0, 300, 50, 300, 50 * (constants.saleCategories.Length + 1), 300, 50, Color.Red, Color.Yellow);

            Button saleStateButton = customButton.CreateButton("販売中", "saleSateButton", width / 2 + 200, 0, 200, 50, Color.FromArgb(255, 0, 112, 192), Color.Transparent, 0, 10, 14, FontStyle.Bold, Color.White);
            saleStateButton.Click += new EventHandler(this.SaleSateSwitching);
            saleStateButtonGlobal = saleStateButton;

            bodyPanel.Controls.Add(saleStateButton);

            Button backButton = customButton.CreateButton(constants.backText, "backButton", width - 150, height - 300, 100, 50, Color.FromArgb(255, 0, 112, 192), Color.Transparent, 0, 10, 14, FontStyle.Bold, Color.White);
            bodyPanel.Controls.Add(backButton);
            backButton.Click += new EventHandler(this.BackShow);

            bodyPanelGlobal = bodyPanel;


            FlowLayoutPanel productTableHeader = createPanel.CreateFlowLayoutPanel(bodyPanelGlobal, 200, 60, width - 400, 60, Color.Beige, new Padding(0));
            Label prodNameHeader1 = createLabel.CreateLabelsInPanel(productTableHeader, "prodNameHeader1", "品目名", 0, 0, productTableHeader.Width / 4 - 10, productTableHeader.Height, Color.Transparent, Color.Silver, 12);
            Label prodNameHeader2 = createLabel.CreateLabelsInPanel(productTableHeader, "prodNameHeader2", "販売価格", productTableHeader.Width / 4, 0, productTableHeader.Width / 4 - 10, productTableHeader.Height, Color.Transparent, Color.Silver, 12);
            Label prodNameHeader3 = createLabel.CreateLabelsInPanel(productTableHeader, "prodNameHeader3", "限定数", productTableHeader.Width / 2, 0, productTableHeader.Width / 4 - 10, productTableHeader.Height, Color.Transparent, Color.Silver, 12);
            Label prodNameHeader4 = createLabel.CreateLabelsInPanel(productTableHeader, "prodNameHeader4", "状態", productTableHeader.Width * 3 / 4, 0, productTableHeader.Width / 4 - 10, productTableHeader.Height, Color.Transparent, Color.Silver, 12);

            Panel productListPanel = createPanel.CreateSubPanel(bodyPanelGlobal, 200, 120, width - 400, height * 5 / 6 - 200, BorderStyle.None, Color.Transparent);
            productListPanel.AutoScroll = true;
            productListPanelGlobal = productListPanel;

            CreateProductTable(constants.productBigName[selectedCategoryIndex]);

            InitializeComponent();
        }

        private void CreateProductTable(string[] productList)
        {
            
            int k = 0;
            foreach(string prodItem in productList)
            {
                FlowLayoutPanel productTableBody = createPanel.CreateFlowLayoutPanel(productListPanelGlobal, 0, 61 * k, productListPanelGlobal.Width, 60, Color.FromArgb(255, 233, 211, 177), new Padding(0));
                Label prodNameBody1 = createLabel.CreateLabelsInPanel(productTableBody, "prodBody_" + k + "_1", prodItem, 0, 0, productTableBody.Width / 4 - 10, productTableBody.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);
                Label prodNameBody2 = createLabel.CreateLabelsInPanel(productTableBody, "prodBody_" + k + "_2", constants.productBigPrice[selectedCategoryIndex][k].ToString(), productTableBody.Width / 4, 0, productTableBody.Width / 4 - 10, productTableBody.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);
                Label prodNameBody3 = createLabel.CreateLabelsInPanel(productTableBody, "prodBody_" + k + "_3", constants.productBigSaleAmount[selectedCategoryIndex][k], productTableBody.Width / 2, 0, productTableBody.Width / 4 - 10, productTableBody.Height, Color.Transparent, Color.FromArgb(255, 142, 133, 118), 12);
                prodNameBody3.Click += new EventHandler(this.showNumberInputDialog);
                Color bgColor = Color.FromArgb(255, 0, 112, 192);
                if (constants.productBigSaleStatus[selectedCategoryIndex][k] == "中止")
                {
                    bgColor = Color.Red;
                }
                Label prodNameBody4 = createLabel.CreateLabelsInPanel(productTableBody, "prodBody_" + k + "_4", constants.productBigSaleStatus[selectedCategoryIndex][k], productTableBody.Width * 3 / 4, 0, productTableBody.Width / 4 - 10, productTableBody.Height, bgColor, Color.White, 12);
                prodNameBody4.Click += new EventHandler(this.ProductSaleStateSwitching);
                k++;
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
                saleStateButtonGlobal.Text = "中止";
                saleStateGlobal = false;
            }
            else
            {
                saleStateButtonGlobal.BackColor = Color.FromArgb(255, 0, 112, 192);
                saleStateButtonGlobal.Text = "販売中";
                saleStateGlobal = true;
                CreateProductTable(constants.productBigName[int.Parse(selectedIndex)]);
            }

        }
        private void SaleSateSwitching(object sender, EventArgs e)
        {
            Button btnTemp = (Button)sender;
            if (saleStateGlobal)
            {
                soldoutCategoryIndex.Add(selectedCategoryIndex + 1);
                btnTemp.Text = "中止";
                btnTemp.BackColor = Color.Red;
                saleStateGlobal = false;
                productListPanelGlobal.Controls.Clear();
            }
            else
            {
                soldoutCategoryIndex.Remove(selectedCategoryIndex + 1);
                btnTemp.Text = "販売中";
                btnTemp.BackColor = Color.FromArgb(255, 0, 112, 192);
                saleStateGlobal = true;
                CreateProductTable(constants.productBigName[selectedCategoryIndex]);
            }
          //  MessageBox.Show(soldoutCategoryIndex.ToArray().Length.ToString());
        }

        private void ProductSaleStateSwitching(object sender, EventArgs e)
        {
            Label btnTemp = (Label)sender;
            if (btnTemp.Text == "販売中")
            {
                btnTemp.Text = "中止";
                btnTemp.BackColor = Color.Red;
            }
            else
            {
                btnTemp.Text = "販売中";
                btnTemp.BackColor = Color.FromArgb(255, 0, 112, 192);
            }
        }

        private void showNumberInputDialog(object sender, EventArgs e)
        {
            Label objectHandler = (Label)sender;
            labelForLimitAmount = objectHandler;
            string objectHandlerText = objectHandler.Text;
            string limitAmounts = objectHandlerText.Split('(')[0];
          //  string limitAmounts = objectHandlerText.Substring(objectHandlerText.IndexOf('(') + 1, objectHandlerText.Length - objectHandlerText.IndexOf('(') - 2);
            int limitAmount = int.Parse(limitAmounts);
            //CreatePanel numberInputPanel = createPanel.
            numberInput.CreateNumberInputDialog("soldoutSetting1", limitAmount, objectHandler.Name);
           // MessageBox.Show(limitAmounts);
        }

        public void SetLimitationValue(string limitAmount)
        {
            string objectHandlerText = labelForLimitAmount.Text;
            string realAmounts = objectHandlerText.Split('(')[1];
            if(limitAmount != "")
            {
                labelForLimitAmount.Text = limitAmount + "(" + realAmounts;
            }
        }
    }
}

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
        Panel detailPanelGlobal = null;
        Panel tBodyPanelGlobal = null;
        DetailView detailView = new DetailView();
        public CategoryList(Form1 mainForm, Panel mainPanel)
        {
            InitializeComponent();
            dropDownMenu.initCategoryList(this);

            mainFormGlobal = mainForm;
            
            Panel mainPanels = createPanel.CreateMainPanel(mainForm, 0, 0, mainForm.Width, mainForm.Height, BorderStyle.None, Color.Transparent);
            mainPanelGlobal = mainPanels;
            Label categoryLabel = createLabel.CreateLabelsInPanel(mainPanels, "categoryLabel", "表示位置/カテゴリーNo", 0, 50, mainPanels.Width / 2, 50, Color.Transparent, Color.Black, 22, false, ContentAlignment.BottomRight);

            dropDownMenu.CreateDropDown("categoryList", mainPanels, constants.saleCategories, mainPanels.Width / 2, 50, 300, 50, 300, 50 * (constants.saleCategories.Length + 1), 300, 50, Color.Red, Color.Yellow);

            Label categoryTimeLabel = createLabel.CreateLabelsInPanel(mainPanels, "categoryTimeLabel", "販売時間 : ", 0, 130, mainPanels.Width / 2, 50, Color.Transparent, Color.Black, 22, false, ContentAlignment.BottomRight);

            Button previewButton = customButton.CreateButton("プレビュー", "previewButton", mainPanelGlobal.Width - 250, mainPanels.Height * 3 / 5 + 30, 200, 50, Color.FromArgb(255, 0, 176, 80), Color.Transparent, 0, 1, 18);
            mainPanels.Controls.Add(previewButton);

            Button printButton = customButton.CreateButton("一覧印刷", "printButton", mainPanelGlobal.Width - 250, mainPanels.Height * 3 / 5 + 110, 200, 50, Color.FromArgb(255, 0, 176, 240), Color.Transparent, 0, 1, 18);
            mainPanels.Controls.Add(printButton);

            Button closeButton = customButton.CreateButton("戻る", "closeButton", mainPanelGlobal.Width - 250, mainPanels.Height * 3 / 5 + 190, 200, 50, Color.FromArgb(255, 0, 112, 192), Color.Transparent, 0, 1, 18);
            mainPanels.Controls.Add(closeButton);
            closeButton.Click += new EventHandler(this.BackShow);

            Panel detailPanel = createPanel.CreateSubPanel(mainPanels, 50, 200, mainPanelGlobal.Width * 5 / 7, mainPanelGlobal.Height - 250, BorderStyle.None, Color.Transparent);
            detailPanelGlobal = detailPanel;

            FlowLayoutPanel tableHeaderInUpPanel = createPanel.CreateFlowLayoutPanel(detailPanel, 0, 0, detailPanel.Width, 50, Color.Transparent, new Padding(0));
            Label tableHeaderLabel1 = createLabel.CreateLabels(tableHeaderInUpPanel, "tableHeaderLabel1", "印刷品目名", 0, 0, tableHeaderInUpPanel.Width / 2, 50, Color.FromArgb(255, 147, 184, 219), Color.Black, 16, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
            Label tableHeaderLabel2 = createLabel.CreateLabels(tableHeaderInUpPanel, "tableHeaderLabel2", "販売価格", tableHeaderLabel1.Right, 0, tableHeaderInUpPanel.Width / 4, 50, Color.FromArgb(255, 147, 184, 219), Color.Black, 16, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
            Label tableHeaderLabel3 = createLabel.CreateLabels(tableHeaderInUpPanel, "tableHeaderLabel3", "限定数", tableHeaderLabel2.Right, 0, tableHeaderInUpPanel.Width / 4, 50, Color.FromArgb(255, 147, 184, 219), Color.Black, 16, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);

            Panel tBodyPanel = createPanel.CreateSubPanel(detailPanel, 0, 50, detailPanel.Width, detailPanel.Height - 50, BorderStyle.FixedSingle, Color.White);
            tBodyPanelGlobal = tBodyPanel;

            ShowCategoryDetail(0);
        }

        private void ShowCategoryDetail(int categoryID)
        {
            Label categoryTimeValue = createLabel.CreateLabelsInPanel(mainPanelGlobal, "categoryTimeValue", "10：00～21:59", mainPanelGlobal.Width / 2, 130, mainPanelGlobal.Width / 2, 50, Color.Transparent, Color.Black, 22, false, ContentAlignment.BottomLeft);
            categoryTimeValue.Padding = new Padding(10, 0, 0, 0);


            tBodyPanelGlobal.HorizontalScroll.Maximum = 0;
            tBodyPanelGlobal.AutoScroll = false;
            tBodyPanelGlobal.VerticalScroll.Visible = false;
            tBodyPanelGlobal.AutoScroll = true;

            int k = 0;
            foreach (string prodItem in constants.productBigName[categoryID])
            {
                FlowLayoutPanel tableRowPanel = createPanel.CreateFlowLayoutPanel(tBodyPanelGlobal, 0, 50 * k, tBodyPanelGlobal.Width, 50, Color.Transparent, new Padding(0));
                Label tdLabel1 = createLabel.CreateLabels(tableRowPanel, "tdLabel1_" + k, prodItem, 0, 0, tableRowPanel.Width / 2, 50, Color.White, Color.Black, 16, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
                Label tdLabel2 = createLabel.CreateLabels(tableRowPanel, "tdLabel2_" + k, constants.productBigPrice[categoryID][k].ToString(), tdLabel1.Right, 0, tableRowPanel.Width / 4, 50, Color.White, Color.Black, 16, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
                Label tdLabel3 = createLabel.CreateLabels(tableRowPanel, "tdLabel3_" + k, constants.productBigSaleAmount[categoryID][k], tdLabel2.Right, 0, tableRowPanel.Width / 4, 50, Color.White, Color.Black, 16, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
                k++;
            }

        }

        public void setVal(string categoryID)
        {
            tBodyPanelGlobal.Controls.Clear();
            ShowCategoryDetail(int.Parse(categoryID));
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

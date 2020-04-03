using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ovan_P1
{
    public partial class  MaintaneceMenu: Form
    {
        int height = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height;
        int width = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width;
        Form1 mainFormGlobal = null;
        Panel mainPanelGlobal = null;
        //private Button buttonGlobal = null;
        private FlowLayoutPanel[] menuFlowLayoutPanelGlobal = new FlowLayoutPanel[3];
        Constant constants = new Constant();

        CreatePanel createPanel = new CreatePanel();
        CreateLabel createLabel = new CreateLabel();
        CustomButton customButton = new CustomButton();

        public MaintaneceMenu(Form1 mainForm, Panel mainPanel)
        {
            mainFormGlobal = mainForm;
            mainPanelGlobal = mainPanel;
            Panel headerPanel = createPanel.CreateMainPanel(mainForm, 0, 0, width, height / 4, BorderStyle.None, Color.Transparent);
            Panel bodyPanel = createPanel.CreateMainPanel(mainForm, 0, headerPanel.Bottom, width, height * 3 / 4, BorderStyle.None, Color.Transparent);
            for(int i = 0; i < 3; i++)
            {
                FlowLayoutPanel menuFlowLayoutPanel = createPanel.CreateFlowLayoutPanel(bodyPanel, width / 7, bodyPanel.Height / 9 + ((bodyPanel.Height * 2) / 9) * i, width * 5 / 7, bodyPanel.Height * 2 / 9, Color.Transparent, new Padding(0, 0, 0, 0));

                menuFlowLayoutPanelGlobal[i] = menuFlowLayoutPanel;

                Label menuLabel = createLabel.CreateLabels(menuFlowLayoutPanel, "menuLabel_" + i, constants.maintanenceLabel[i], 100, menuFlowLayoutPanel.Height * i, menuFlowLayoutPanel.Width / 6 - 10, menuFlowLayoutPanel.Height * 2 / 3, Color.Transparent, Color.Red, 18, false, ContentAlignment.MiddleLeft);
                Image btnImage = Image.FromFile(constants.maitanenceButtonImage[i]);
                Button menuButton_1 = customButton.CreateButtonWithImage(btnImage, "menuButton_" + i + "_1", constants.maintanenceButton[i][0], menuLabel.Right, menuFlowLayoutPanel.Height * i, menuFlowLayoutPanel.Width / 4 - 10, menuFlowLayoutPanel.Height * 2 / 3, 0, 100);
                menuFlowLayoutPanel.Controls.Add(menuButton_1);
                menuButton_1.MouseHover += new EventHandler(this.buttonHover);
                menuButton_1.Click += new EventHandler(this.showSetting);
                Button menuButton_2 = customButton.CreateButtonWithImage(btnImage, "menuButton_" + i + "_2", constants.maintanenceButton[i][1], menuButton_1.Right, menuFlowLayoutPanel.Height * i, menuFlowLayoutPanel.Width / 4 - 10, menuFlowLayoutPanel.Height * 2 / 3, 0, 100);
                menuFlowLayoutPanel.Controls.Add(menuButton_2);
                menuButton_2.MouseHover += new EventHandler(this.buttonHover);
                menuButton_2.Click += new EventHandler(this.showSetting);

                Button menuButton_3 = customButton.CreateButtonWithImage(btnImage, "menuButton_" + i + "_3", constants.maintanenceButton[i][2], menuButton_2.Right, menuFlowLayoutPanel.Height * i, menuFlowLayoutPanel.Width / 4 - 10, menuFlowLayoutPanel.Height * 2 / 3, 0, 100);
                menuFlowLayoutPanel.Controls.Add(menuButton_3);
                menuButton_3.MouseHover += new EventHandler(this.buttonHover);
                menuButton_3.Click += new EventHandler(this.showSetting);

            }
        }

        private void buttonHover(object sender, EventArgs e)
        {
            Button buttonObj = (Button)sender;
            string[] buttonObjNameArray = buttonObj.Name.Split('_');
            //  buttonGlobal.Width = 100;
            buttonObj.BackColor = Color.White;
            buttonObj.FlatStyle = FlatStyle.Flat;
            buttonObj.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
            buttonObj.FlatAppearance.BorderSize = 0;
            //menuFlowLayoutPanelGlobal[int.Parse(buttonObjNameArray[1])].Controls.Add(buttonObj);

        }

        private void showSetting(object sender, EventArgs e)
        {
            mainFormGlobal.Controls.Clear();
         //   Panel mainPanel = createPanel.CreateMainPanel(mainFormGlobal, 0, 0, width, height, BorderStyle.None, Color.Transparent);

            Button buttonObj = (Button)sender;
            string buttonName = buttonObj.Name;
            switch (buttonName)
            {
                case "menuButton_0_1":
                    SoldoutSetting1 soldSetting = new SoldoutSetting1(mainFormGlobal, mainPanelGlobal);
                    soldSetting.TopLevel = false;
                    mainFormGlobal.Controls.Add(soldSetting);
                    soldSetting.FormBorderStyle = FormBorderStyle.None;
                    soldSetting.Dock = DockStyle.Fill;
                    Thread.Sleep(200);
                    soldSetting.Show();
                    break;
                case "menuButton_0_2":
                    ClosingProcess closingProcess = new ClosingProcess(mainFormGlobal, mainPanelGlobal);
                    closingProcess.TopLevel = false;
                    mainFormGlobal.Controls.Add(closingProcess);
                    closingProcess.FormBorderStyle = FormBorderStyle.None;
                    closingProcess.Dock = DockStyle.Fill;
                    Thread.Sleep(200);
                    closingProcess.Show();
                    break;
                case "menuButton_0_3":
                    FalsePurchaseCancellation falsePurchaseCancellation = new FalsePurchaseCancellation(mainFormGlobal, mainPanelGlobal);
                    falsePurchaseCancellation.TopLevel = false;
                    mainFormGlobal.Controls.Add(falsePurchaseCancellation);
                    falsePurchaseCancellation.FormBorderStyle = FormBorderStyle.None;
                    falsePurchaseCancellation.Dock = DockStyle.Fill;
                    Thread.Sleep(200);
                    falsePurchaseCancellation.Show();
                    break;
                case "menuButton_1_1":
                    ProductItemManagement productItemManagement = new ProductItemManagement(mainFormGlobal, mainPanelGlobal);
                    productItemManagement.TopLevel = false;
                    mainFormGlobal.Controls.Add(productItemManagement);
                    productItemManagement.FormBorderStyle = FormBorderStyle.None;
                    productItemManagement.Dock = DockStyle.Fill;
                    Thread.Sleep(200);
                    productItemManagement.Show();
                    break;
                case "menuButton_1_2":
                    CategoryList categoryList = new CategoryList(mainFormGlobal, mainPanelGlobal);
                    categoryList.TopLevel = false;
                    mainFormGlobal.Controls.Add(categoryList);
                    categoryList.FormBorderStyle = FormBorderStyle.None;
                    categoryList.Dock = DockStyle.Fill;
                    Thread.Sleep(200);
                    categoryList.Show();
                    break;
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // MaintaneceMenu
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "MaintaneceMenu";
            this.ResumeLayout(false);

        }
    }
}

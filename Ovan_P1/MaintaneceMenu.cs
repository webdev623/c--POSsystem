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
        Panel mainPanelGlobal_2 = null;
        //private Button buttonGlobal = null;
        private FlowLayoutPanel[] menuFlowLayoutPanelGlobal = new FlowLayoutPanel[3];
        Constant constants = new Constant();

        CreatePanel createPanel = new CreatePanel();
        CreateLabel createLabel = new CreateLabel();
        CustomButton customButton = new CustomButton();

        public MaintaneceMenu(Form1 mainForm, Panel mainPanel)
        {
            mainForm.Width = width;
            mainForm.Height = height;
            mainFormGlobal = mainForm;
            mainPanelGlobal = mainPanel;
            mainPanelGlobal_2 = mainForm.mainPanelGlobal_2;
            Panel headerPanel = createPanel.CreateSubPanel(mainPanel, 0, 0, mainPanel.Width, mainPanel.Height / 10, BorderStyle.None, Color.FromArgb(255, 234, 225, 151));

            Label MainTitle = new Label();
            MainTitle.Location = new Point(0, 0);
            MainTitle.Width = headerPanel.Width;
            MainTitle.Height = headerPanel.Height;
            MainTitle.TextAlign = ContentAlignment.MiddleCenter;
            MainTitle.Font = new Font("Seri", 36, FontStyle.Bold);
            MainTitle.ForeColor = Color.FromArgb(255, 0, 0, 0);
            MainTitle.Text = constants.main_Menu_Title;
            headerPanel.Controls.Add(MainTitle);

            Panel bodyPanel = createPanel.CreateSubPanel(mainPanel, 0, headerPanel.Bottom, mainPanel.Width, mainPanel.Height * 9 / 10, BorderStyle.None, Color.Transparent);
            for(int i = 0; i < 3; i++)
            {
                FlowLayoutPanel menuFlowLayoutPanel = createPanel.CreateFlowLayoutPanel(bodyPanel, bodyPanel.Width / 10, bodyPanel.Height / 9 + ((bodyPanel.Height * 3) / 10) * i, bodyPanel.Width * 4 / 5, bodyPanel.Height * 1 / 5 - 10, Color.White, new Padding(0));

                menuFlowLayoutPanelGlobal[i] = menuFlowLayoutPanel;

                Label menuLabel = createLabel.CreateLabels(menuFlowLayoutPanel, "menuLabel_" + i, constants.maintanenceLabel[i], 0, menuFlowLayoutPanel.Height * i, menuFlowLayoutPanel.Width / 5, menuFlowLayoutPanel.Height, Color.Transparent, Color.Red, 22, false, ContentAlignment.MiddleCenter);
                Image btnImage = Image.FromFile(constants.maitanenceButtonImage[i]);
                Button menuButton_1 = customButton.CreateButtonWithImage(btnImage, "menuButton_" + i + "_1", constants.maintanenceButton[i][0], menuLabel.Right, menuFlowLayoutPanel.Height * i, menuFlowLayoutPanel.Width / 5, menuFlowLayoutPanel.Height * 2 / 3, 0, 50, 18, FontStyle.Bold, Color.White, ContentAlignment.MiddleCenter, 2);
                menuButton_1.Margin = new Padding(menuFlowLayoutPanel.Width / 20, menuFlowLayoutPanel.Height * 1 / 6, 0, menuFlowLayoutPanel.Height * 1 / 6);
                
                menuFlowLayoutPanel.Controls.Add(menuButton_1);
                menuButton_1.Click += new EventHandler(this.showSetting);
                Button menuButton_2 = customButton.CreateButtonWithImage(btnImage, "menuButton_" + i + "_2", constants.maintanenceButton[i][1], menuButton_1.Right, menuFlowLayoutPanel.Height * i, menuFlowLayoutPanel.Width / 5, menuFlowLayoutPanel.Height * 2 / 3, 0, 50, 18, FontStyle.Bold, Color.White, ContentAlignment.MiddleCenter, 2);
                menuButton_2.Margin = new Padding(menuFlowLayoutPanel.Width / 20, menuFlowLayoutPanel.Height * 1 / 6, 0, menuFlowLayoutPanel.Height * 1 / 6);
                menuFlowLayoutPanel.Controls.Add(menuButton_2);
                menuButton_2.Click += new EventHandler(this.showSetting);

                Button menuButton_3 = customButton.CreateButtonWithImage(btnImage, "menuButton_" + i + "_3", constants.maintanenceButton[i][2], menuButton_2.Right, menuFlowLayoutPanel.Height * i, menuFlowLayoutPanel.Width / 5, menuFlowLayoutPanel.Height * 2 / 3, 0, 50, 18, FontStyle.Bold, Color.White, ContentAlignment.MiddleCenter, 2);
                menuButton_3.Margin = new Padding(menuFlowLayoutPanel.Width / 20, menuFlowLayoutPanel.Height * 1 / 6, 0, menuFlowLayoutPanel.Height * 1 / 6);
                menuFlowLayoutPanel.Controls.Add(menuButton_3);
                menuButton_3.Click += new EventHandler(this.showSetting);

            }

            Image backImage = Image.FromFile(constants.backButton);

            Button backButton = customButton.CreateButtonWithImage(backImage, "backButton", "", bodyPanel.Width - 150, bodyPanel.Height - 150, 100, 100, 3, 100);
            backButton.BackgroundImageLayout = ImageLayout.Stretch;
            backButton.Padding = new Padding(0);
            bodyPanel.Controls.Add(backButton);
            backButton.Click += new EventHandler(BackShow);
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
        public void BackShow(object sender, EventArgs e)
        {
            mainPanelGlobal.Controls.Clear();
            MainMenu mainMenu = new MainMenu();
            mainMenu.CreateMainMenuScreen(mainFormGlobal, mainPanelGlobal);
            //frm.TopLevel = false;
            //mainFormGlobal.Controls.Add(frm);
            //frm.FormBorderStyle = FormBorderStyle.None;
            //frm.Dock = DockStyle.Fill;
            //Thread.Sleep(200);
            //frm.Show();
        }

        private void showSetting(object sender, EventArgs e)
        {
            mainPanelGlobal.Controls.Clear();
         //   Panel mainPanel = createPanel.CreateMainPanel(mainFormGlobal, 0, 0, width, height, BorderStyle.None, Color.Transparent);

            Button buttonObj = (Button)sender;
            string buttonName = buttonObj.Name;
            switch (buttonName)
            {
                case "menuButton_0_1":
                    SoldoutSetting1 soldSetting = new SoldoutSetting1(mainFormGlobal, mainPanelGlobal);
                    soldSetting.TopLevel = false;
                    mainPanelGlobal.Controls.Add(soldSetting);
                    soldSetting.FormBorderStyle = FormBorderStyle.None;
                    soldSetting.Dock = DockStyle.Fill;
                    Thread.Sleep(200);
                    soldSetting.Show();
                    break;
                case "menuButton_0_2":
                    ClosingProcess closingProcess = new ClosingProcess(mainFormGlobal, mainPanelGlobal);
                    closingProcess.TopLevel = false;
                    mainPanelGlobal.Controls.Add(closingProcess);
                    closingProcess.FormBorderStyle = FormBorderStyle.None;
                    closingProcess.Dock = DockStyle.Fill;
                    Thread.Sleep(200);
                    closingProcess.Show();
                    break;
                case "menuButton_0_3":
                    mainFormGlobal.topPanelGlobal.Hide();
                    mainFormGlobal.bottomPanelGlobal.Hide();
                    mainFormGlobal.mainPanelGlobal.Hide();
                    mainFormGlobal.mainPanelGlobal_2.Show();
                    FalsePurchaseCancellation falsePurchaseCancellation = new FalsePurchaseCancellation(mainFormGlobal, mainPanelGlobal);
                    falsePurchaseCancellation.TopLevel = false;
                    mainPanelGlobal_2.Controls.Add(falsePurchaseCancellation);
                    falsePurchaseCancellation.FormBorderStyle = FormBorderStyle.None;
                    falsePurchaseCancellation.Dock = DockStyle.Fill;
                    Thread.Sleep(200);
                    falsePurchaseCancellation.Show();
                    break;
                case "menuButton_1_1":
                    ProductItemManagement productItemManagement = new ProductItemManagement(mainFormGlobal, mainPanelGlobal);
                    productItemManagement.TopLevel = false;
                    mainPanelGlobal.Controls.Add(productItemManagement);
                    productItemManagement.FormBorderStyle = FormBorderStyle.None;
                    productItemManagement.Dock = DockStyle.Fill;
                    Thread.Sleep(200);
                    productItemManagement.Show();
                    break;
                case "menuButton_1_2":
                    CategoryList categoryList = new CategoryList(mainFormGlobal, mainPanelGlobal);
                    categoryList.TopLevel = false;
                    mainPanelGlobal.Controls.Add(categoryList);
                    categoryList.FormBorderStyle = FormBorderStyle.None;
                    categoryList.Dock = DockStyle.Fill;
                    Thread.Sleep(200);
                    categoryList.Show();
                    break;
                case "menuButton_1_3":
                    GroupList groupList = new GroupList(mainFormGlobal, mainPanelGlobal);
                    groupList.TopLevel = false;
                    mainPanelGlobal.Controls.Add(groupList);
                    groupList.FormBorderStyle = FormBorderStyle.None;
                    groupList.Dock = DockStyle.Fill;
                    Thread.Sleep(200);
                    groupList.Show();
                    break;
                case "menuButton_2_1":
                    TimeSetting timeSetting = new TimeSetting(mainFormGlobal, mainPanelGlobal);
                    timeSetting.TopLevel = false;
                    mainPanelGlobal.Controls.Add(timeSetting);
                    //timeSetting.Owner = 
                    timeSetting.FormBorderStyle = FormBorderStyle.None;
                    timeSetting.Dock = DockStyle.Fill;
                    Thread.Sleep(200);
                    timeSetting.Show();
                    break;
                case "menuButton_2_2":
                    PasswordSetting passwordSetting = new PasswordSetting(mainFormGlobal, mainPanelGlobal);
                    passwordSetting.TopLevel = false;
                    mainPanelGlobal.Controls.Add(passwordSetting);
                    //timeSetting.Owner = 
                    passwordSetting.FormBorderStyle = FormBorderStyle.None;
                    passwordSetting.Dock = DockStyle.Fill;
                    Thread.Sleep(200);
                    passwordSetting.Show();
                    break;
                case "menuButton_2_3":
                    OpenTimeChange openTimeChange = new OpenTimeChange(mainFormGlobal, mainPanelGlobal);
                    openTimeChange.TopLevel = false;
                    mainPanelGlobal.Controls.Add(openTimeChange);
                    //timeSetting.Owner = 
                    openTimeChange.FormBorderStyle = FormBorderStyle.None;
                    openTimeChange.Dock = DockStyle.Fill;
                    Thread.Sleep(200);
                    openTimeChange.Show();
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

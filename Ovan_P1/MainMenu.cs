using Microsoft.Win32;
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
    class MainMenu : Form
    {
        Form1 FormPanel = null;
        Panel Panels = null;
        Constant constants = new Constant();
        int height = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height;
        int width = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width;
        PasswordInput passwordInput = new PasswordInput();
        CustomButton customButton = new CustomButton();



        public void CreateMainMenuScreen(Form1 forms, Panel panels)
        {
            FormPanel = forms;
            Panels = panels;

            FormPanel.Width = width;
            FormPanel.Height = height;
            passwordInput.initMainMenu(this);


            /**  Main Page Screen Title */

            Label MainTitle = new Label();
            MainTitle.Location = new Point(50, 50);
            MainTitle.Width = width;
            MainTitle.Height = 50;
            MainTitle.Font = new Font("Seri", 24, FontStyle.Bold);
            MainTitle.ForeColor = Color.FromArgb(255, 0, 0, 0);
            MainTitle.Text = constants.main_Menu_Title;
            FormPanel.Controls.Add(MainTitle);

            /** Menu Button Create  */
            int k = 0;
            foreach(string x in constants.main_Menu)
            {
                RoundedButton btn = new RoundedButton();
                btn.Name = constants.main_Menu_Name[k];
                btn.Text = x;
                btn.ForeColor = Color.White;
                int xCordinator = (width / 42) + k * (width * 2 / 7) + k * (width / 21);

                btn.Location = new Point(xCordinator, height / 3);
                btn.Width = width * 2 / 7;
                btn.Height = height * 2 / 5;
                switch (k)
                {
                    case 1:
                        btn.BackColor = Color.FromArgb(255, 255, 192, 0);
                        break;
                    case 2:
                        btn.BackColor = Color.FromArgb(255, 0, 176, 240);
                        break;
                    default:
                        btn.BackColor = Color.FromArgb(255, 0, 176, 80);
                        break;
                }
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderColor = Color.FromArgb(255, 185, 205, 229);
                btn.FlatAppearance.BorderSize = 1;
                
                btn.Font = new Font("Seri", 24, FontStyle.Bold);

                btn.Click += new EventHandler(this.MainMenuBtn);

                FormPanel.Controls.Add(btn);
                k++;
            }

            Image powerImage = Image.FromFile(constants.powerButton);

            Button backButton = customButton.CreateButtonWithImage(powerImage, "powerButton", "", width - 150, height - 150, 100, 100, 3, 100);
            backButton.BackgroundImageLayout = ImageLayout.Stretch;
            backButton.Padding = new Padding(0);
            FormPanel.Controls.Add(backButton);
            backButton.Click += new EventHandler(PowerApplication);

        }

        private void MainMenuBtn(object sender, EventArgs e)
        {

            Button temp = (Button)sender;
            if (temp.Name == "maintenance")
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\WinRegistry");
                if(key != null && key.GetValue("POSPassword") != null)
                {
                    passwordInput.CreateNumberInputDialog("maintenance", temp.Name);
                }

            }
            else if (temp.Name == "salescreen")
            {
                try
                {
                    FormPanel.Controls.Clear();
                    SaleScreen saleScreen = new SaleScreen(FormPanel);
                    saleScreen.TopLevel = false;
                    saleScreen.FormBorderStyle = FormBorderStyle.None;
                    saleScreen.Dock = DockStyle.Fill;
                    Panels.Controls.Add(saleScreen);
                    Thread.Sleep(200);

                    saleScreen.Show();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\WinRegistry");
                if (key != null && key.GetValue("POSPassword") != null)
                {
                    passwordInput.CreateNumberInputDialog("readingmenu", temp.Name);
                }
            }
        }

        private void PowerApplication(object sender, EventArgs e)
        {
            Application.Exit();
        }
        public void getPassword(string objectName, string passwords)
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\WinRegistry");
            string pwd = key.GetValue("POSPassword").ToString();
            if(pwd == passwords)
            {
                switch (objectName)
                {
                    case "maintenance":
                        FormPanel.Controls.Clear();
                        MaintaneceMenu maintaneceMenu = new MaintaneceMenu(FormPanel, Panels);
                        maintaneceMenu.TopLevel = false;
                        Panels.Controls.Add(maintaneceMenu);
                        maintaneceMenu.FormBorderStyle = FormBorderStyle.None;
                        maintaneceMenu.Dock = DockStyle.Fill;
                        Thread.Sleep(200);
                        maintaneceMenu.Show();
                        break;
                    case "readingmenu":
                        FormPanel.Controls.Clear();
                        MenuReading menuReading = new MenuReading(FormPanel, Panels);
                        menuReading.TopLevel = false;
                        Panels.Controls.Add(menuReading);
                        menuReading.FormBorderStyle = FormBorderStyle.None;
                        menuReading.Dock = DockStyle.Fill;
                        Thread.Sleep(200);
                        menuReading.Show();
                        break;
                }
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // MainMenu
            // 
            this.ClientSize = new System.Drawing.Size(862, 261);
            this.Name = "MainMenu";
            this.ResumeLayout(false);

        }
    }
}

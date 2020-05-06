using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
    class MainMenu : Form
    {
        Form1 mainFomeGlobal = null;
        public Panel FormPanel = null;
        public Panel FormPanel_2 = null;
        Constant constants = new Constant();
        int height = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height;
        int width = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width;
        PasswordInput passwordInput = new PasswordInput();
        CustomButton customButton = new CustomButton();
        CreatePanel createPanel = new CreatePanel();
        MessageDialog messageDialog = new MessageDialog();
        SQLiteConnection sqlite_conn;

        bool tb_check = true;

        public void CreateMainMenuScreen(Form1 forms, Panel panels)
        {
            FormPanel = panels;
            mainFomeGlobal = forms;
            FormPanel_2 = forms.mainPanelGlobal_2;
            passwordInput.initMainMenu(this);
            sqlite_conn = CreateConnection(constants.dbName);
            if (sqlite_conn.State == ConnectionState.Closed)
            {
                sqlite_conn.Open();
            }
            SQLiteCommand sqlite_cmd;

            foreach (string tbName in constants.tbNames)
            {
                try
                {
                    string query = "SELECT count(*) FROM " + tbName;
                    sqlite_cmd = sqlite_conn.CreateCommand();
                    sqlite_cmd.CommandText = query;
                    int rowCount = Convert.ToInt32(sqlite_cmd.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    tb_check = false;
                    break;
                }

            }

            Panel titlePanel = createPanel.CreateSubPanel(FormPanel, 0, 0, FormPanel.Width, FormPanel.Height / 10, BorderStyle.None, Color.FromArgb(255, 234, 225, 151));


            /**  Main Page Screen Title */

            Label MainTitle = new Label();
            MainTitle.Location = new Point(0, 0);
            MainTitle.Width = titlePanel.Width;
            MainTitle.Height = titlePanel.Height;
            MainTitle.TextAlign = ContentAlignment.MiddleCenter;
            MainTitle.Font = new Font("Seri", 36, FontStyle.Bold);
            MainTitle.ForeColor = Color.FromArgb(255, 0, 0, 0);
            MainTitle.Text = constants.main_Menu_Title;
            titlePanel.Controls.Add(MainTitle);
            

            /** Menu Button Create  */
            int k = 0;
            foreach(string x in constants.main_Menu)
            {
                RoundedButton btn = new RoundedButton();
                btn.Name = constants.main_Menu_Name[k];
                int xCordinator = (FormPanel.Width / 16) + k * (FormPanel.Width * 2 / 8) + k * (FormPanel.Width / 24);

                btn.Location = new Point(xCordinator, FormPanel.Height / 3);
                btn.Width = FormPanel.Width * 2 / 8;
                btn.Height = FormPanel.Height * 1 / 3;
                switch (k)
                {
                    case 1:
                        btn.BackColor = Color.FromArgb(255, 225, 100, 74);
                        btn.ColorTop = Color.FromArgb(255, 227, 111, 87);
                        btn.ColorBottom = Color.FromArgb(255, 225, 100, 74);

                        break;
                    case 2:
                        btn.BackColor = Color.FromArgb(255, 0, 123, 191);
                        btn.ColorTop = Color.FromArgb(255, 7, 131, 200);
                        btn.ColorBottom = Color.FromArgb(255, 0, 123, 191);
                        break;
                    default:
                        btn.BackColor = Color.FromArgb(255, 94, 162, 83);
                        btn.ColorTop = Color.FromArgb(255, 112, 169, 103);
                        btn.ColorBottom = Color.FromArgb(255, 94, 162, 83);
                        break;
                }
                btn.FlatStyle = FlatStyle.Flat;
                btn.radiusValue = 50;
                btn.ForeColors = Color.White;
                btn.text = x;
                btn.diffValue = 3;
                btn.FlatAppearance.BorderColor = Color.White;
                btn.FlatAppearance.BorderSize = 1;
                
                btn.Font = new Font("Seri", 28, FontStyle.Bold);

                btn.Click += new EventHandler(this.MainMenuBtn);
                FormPanel.Controls.Add(btn);

                k++;
            }

            Image powerImage = Image.FromFile(constants.powerButton);

            Button backButton = customButton.CreateButtonWithImage(powerImage, "powerButton", "", FormPanel.Width - 150, FormPanel.Height - 150, 100, 100, 3, 100);
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
                if (tb_check)
                {
                    //RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\WinRegistry");
                    //if (key != null && key.GetValue("POSPassword") != null)
                    //{
                        passwordInput.CreateNumberInputDialog("maintenance", temp.Name);
                    //}
                }
                else
                {
                    string errorMsg1 = "Database checking is failed.";
                    string errorMsg2 = "Please go to Menu Reading section and get loading data.";
                    messageDialog.ShowErrorMessage(errorMsg1, errorMsg2 + "\n ErrorNo: 001");
                }

            }
            else if (temp.Name == "salescreen")
            {
                if (tb_check)
                {
                    Form1 mainFormCTL = new Form1();
                    Console.WriteLine(mainFormCTL.processStartState);
                    bool processStartState = mainFormCTL.processStartState;
                    if (processStartState)
                    {
                        try
                        {
                            FormPanel.Controls.Clear();
                            FormPanel.Hide();
                            mainFomeGlobal.topPanelGlobal.Hide();
                            mainFomeGlobal.bottomPanelGlobal.Hide();
                            SaleScreen saleScreen = new SaleScreen(mainFomeGlobal, FormPanel);
                            saleScreen.TopLevel = false;
                            saleScreen.FormBorderStyle = FormBorderStyle.None;
                            saleScreen.Dock = DockStyle.Fill;
                            FormPanel.Controls.Add(saleScreen);
                            Thread.Sleep(200);

                            saleScreen.Show();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                            //messageDialog.ShowErrorMessage(constants.systemErrorMsg, constants.systemSubErrorMsg);
                        }
                    }
                    else
                    {
                        string errorMsg1 = "Sale time is over.";
                        string errorMsg2 = "Please wait for the next sale time.";
                        messageDialog.ShowErrorMessage(errorMsg1, errorMsg2 + "\n ErrorNo: 002");
                    }
                }
                else
                {
                    string errorMsg1 = "Database checking is failed.";
                    string errorMsg2 = "Please go to Menu Reading section and get loading data.";
                    messageDialog.ShowErrorMessage(errorMsg1, errorMsg2 + "\n ErrorNo: 003");
                }
            }
            else
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\WinRegistry");
                //if (key != null && key.GetValue("POSPassword") != null)
                //{
                    passwordInput.CreateNumberInputDialog("readingmenu", temp.Name);
                //}
            }
        }

        private void PowerApplication(object sender, EventArgs e)
        {
            Application.Exit();
        }
        public void getPassword(string objectName, string passwords)
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\WinRegistry");
            string pwd = "";
            if (key != null && key.GetValue("POSPassword") != null)
            {
                pwd = key.GetValue("POSPassword").ToString();
            }
            else if (key == null)
            {
                pwd = "";
            }
            if (pwd == passwords)
            {
                switch (objectName)
                {
                    case "maintenance":
                        FormPanel.Controls.Clear();
                        MaintaneceMenu maintaneceMenu = new MaintaneceMenu(mainFomeGlobal, FormPanel);
                        maintaneceMenu.TopLevel = false;
                        FormPanel.Controls.Add(maintaneceMenu);
                        maintaneceMenu.FormBorderStyle = FormBorderStyle.None;
                        maintaneceMenu.Dock = DockStyle.Fill;
                        Thread.Sleep(200);
                        maintaneceMenu.Show();
                        break;
                    case "readingmenu":
                        FormPanel.Controls.Clear();
                        MenuReading menuReading = new MenuReading(mainFomeGlobal, FormPanel);
                        menuReading.TopLevel = false;
                        FormPanel.Controls.Add(menuReading);
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
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(937, 624);
            this.Name = "MainMenu";
            this.ResumeLayout(false);

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

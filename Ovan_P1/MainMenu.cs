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

        

        public void CreateMainMenuScreen(Form1 forms, Panel panels)
        {
            FormPanel = forms;
            Panels = panels;

            FormPanel.Width = width;
            FormPanel.Height = height;

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
        }

        private void MainMenuBtn(object sender, EventArgs e)
        {
            FormPanel.Controls.Clear();

            Button temp = (Button)sender;
            if (temp.Name == "maintenance")
            {
                MaintaneceMenu maintaneceMenu = new MaintaneceMenu(FormPanel, Panels);
                maintaneceMenu.TopLevel = false;
                Panels.Controls.Add(maintaneceMenu);
                maintaneceMenu.FormBorderStyle = FormBorderStyle.None;
                maintaneceMenu.Dock = DockStyle.Fill;
                Thread.Sleep(200);
                maintaneceMenu.Show();
            }
            else if (temp.Name == "salescreen")
            {
                SaleScreen saleScreen = new SaleScreen(FormPanel);
                saleScreen.TopLevel = false;
                Panels.Controls.Add(saleScreen);
                saleScreen.FormBorderStyle = FormBorderStyle.None;
                saleScreen.Dock = DockStyle.Fill;
                Thread.Sleep(200);
                saleScreen.Show();
            }
            else
                MessageBox.Show("this is reading menu");
        }
    }
}

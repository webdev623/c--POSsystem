using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ovan_P1
{
    class DropDownMenu
    {
        Constant constants = new Constant();
        private bool isCollapsed = true;
        Timer timer = null;
        CreatePanel createPanel = new CreatePanel();
        Panel mainPanelGlobal = null;
        Panel subPanelGlobal = null;
        Button mainButtonGlobal = null;
        SoldoutSetting1 soldoutSetting1Global = null;
        CategoryList categoryListGlobal = null;
        GroupList groupListGlobal = null;
        FalsePurchaseCancellation falsePurchaseCancellation = null;
        Button[] submenuButton = null;
        DetailView detailViewGlobal = null;
        Color mainButtonColor = default(Color);
        Color subButtonColor = default(Color);
        string objecNameGlobal = "";


        public void initSoldoutSetting(SoldoutSetting1 sendHandler)
        {
            soldoutSetting1Global = sendHandler;
        }
        public void initCategoryList(CategoryList sendHandler)
        {
            categoryListGlobal = sendHandler;
        }
        public void initGroupList(GroupList sendHandler)
        {
            groupListGlobal = sendHandler;
        }
        public void initFalsePurchaseCancellation(FalsePurchaseCancellation sendHandler)
        {
            falsePurchaseCancellation = sendHandler;
        }
        public void initLogReport(DetailView sendHandler)
        {
            detailViewGlobal = sendHandler;
        }
        public void initValue()
        {
            timer = new Timer();
            timer.Enabled = false;
            timer.Interval = 15;
            timer.Tick += new System.EventHandler(this.timer_Tick);

        }

        public Panel CreateDropDown(string objectName, Panel mainPanel, string[] menuItemArray, int left, int top, int width, int height, int maxWidth, int maxHeight, int minWidth, int minHeight, Color mainItemBackColor, Color subItemBackColor, int defaultItem = 0)
        {
            initValue();
            objecNameGlobal = objectName;
            Panel mainPanels = createPanel.CreateSubPanel(mainPanel, left, top, width, height, BorderStyle.None, Color.Transparent);
            mainPanels.MaximumSize = new Size(maxWidth, maxHeight);
            mainPanels.MinimumSize = new Size(minWidth, minHeight);
            Panel subPanels = createPanel.CreateSubPanel(mainPanels, 0, height, width, height, BorderStyle.None, Color.Transparent);
            subPanels.MaximumSize = new Size(maxWidth, maxHeight);
            subPanels.MinimumSize = new Size(minWidth, 0);
            mainPanelGlobal = mainPanels;
            subPanelGlobal = subPanels;

            int buttonWidth = maxWidth;
            int buttonHeight = maxHeight / (menuItemArray.Length + 1);

            Button mainButton = new Button();
            mainButton.BackColor = mainItemBackColor;
            mainButton.Dock = DockStyle.Top;
            mainButton.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
         //   mainButton.Image = Image.FromFile(constants.dropdownArrowDownIcon);
         //   mainButton.Image = new Bitmap(Image.FromFile(constants.dropdownArrowDownIcon));
            mainButton.BackgroundImage = Image.FromFile(constants.dropdownArrowDownIcon);
            mainButton.BackgroundImageLayout = ImageLayout.Stretch;
            mainButton.ImageAlign = ContentAlignment.MiddleRight;
            mainButton.Padding = new Padding(0, 0, 10, 0);
            mainButton.Location = new Point(0, 0);
            mainButton.Name = "mainButton";
            mainButton.Size = new Size(buttonWidth, buttonHeight);
            mainButton.TabIndex = 0;
            mainButton.Text = menuItemArray[0];
            if(defaultItem != 0)
            {
                mainButton.Text = menuItemArray[defaultItem];
            }
            mainPanels.Controls.Add(mainButton);
            mainButtonGlobal = mainButton;
            mainButton.Click += new EventHandler(this.showDropDown);
            // mainButton.UseVisualStyleBackColor = false;
            mainButtonColor = mainItemBackColor;
            subButtonColor = subItemBackColor;
            //  mainButton.Click += new EventHandler(this.button1_Click);
            int k = 0;
            int len = menuItemArray.Length;
            submenuButton = new Button[len];

            foreach(string menuItem in menuItemArray)
            {
                Button submenuButtons = new Button();
                //submenuButtons.Dock = DockStyle.Top;
                if(k == 0)
                {
                    submenuButtons.BackColor = mainItemBackColor;
                    submenuButtons.ForeColor = Color.White;
                }
                else
                {
                    submenuButtons.BackColor = subItemBackColor;
                    submenuButtons.ForeColor = Color.Black;
                }
                submenuButtons.Location = new Point(0, buttonHeight * k);
                submenuButtons.Name = "submenuButton_" + k;
                submenuButtons.Size = new Size(buttonWidth, buttonHeight);
                submenuButtons.TabIndex = k + 1;
                submenuButtons.Text = menuItemArray[k];
                //submenuButtons.UseVisualStyleBackColor = true;
                subPanels.Controls.Add(submenuButtons);
                submenuButtons.Click += new EventHandler(this.showTable);
                submenuButton[k] = submenuButtons;

                k++;
            }

            return mainPanels;
        }
        public Panel CreateCategoryDropDown(string objectName, Panel mainPanel, string[] menuItemArray, int[] menuIDArray, int[] menuDisplayPositionArray, int[] menuStateArray, int left, int top, int width, int height, int maxWidth, int maxHeight, int minWidth, int minHeight, Color mainItemBackColor, Color subItemBackColor)
        {
            initValue();
            objecNameGlobal = objectName;
            Panel mainPanels = createPanel.CreateSubPanel(mainPanel, left, top, width, height, BorderStyle.None, Color.Transparent);
            mainPanels.MaximumSize = new Size(maxWidth, maxHeight);
            mainPanels.MinimumSize = new Size(minWidth, minHeight);
            Panel subPanels = createPanel.CreateSubPanel(mainPanels, 0, height, width, height, BorderStyle.None, Color.Transparent);
            subPanels.MaximumSize = new Size(maxWidth, maxHeight);
            subPanels.MinimumSize = new Size(minWidth, 0);
            mainPanelGlobal = mainPanels;
            subPanelGlobal = subPanels;

            int buttonWidth = maxWidth;
            int buttonHeight = maxHeight / (menuItemArray.Length + 1);

            Button mainButton = new Button();
            mainButton.BackColor = mainItemBackColor;
            mainButton.Dock = DockStyle.Top;
            mainButton.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            //   mainButton.Image = Image.FromFile(constants.dropdownArrowDownIcon);
            //   mainButton.Image = new Bitmap(Image.FromFile(constants.dropdownArrowDownIcon));
            mainButton.BackgroundImage = Image.FromFile(constants.dropdownArrowDownIcon);
            mainButton.BackgroundImageLayout = ImageLayout.Stretch;
            mainButton.ImageAlign = ContentAlignment.MiddleRight;
            mainButton.Padding = new Padding(0, 0, 10, 0);
            mainButton.Location = new Point(0, 0);
            mainButton.Name = "mainButton";
            mainButton.Size = new Size(buttonWidth, buttonHeight);
            mainButton.TabIndex = 0;
            mainButton.Text = menuDisplayPositionArray[0].ToString() + "-" + menuIDArray[0] + "  " +  menuItemArray[0];
            mainPanels.Controls.Add(mainButton);
            mainButtonGlobal = mainButton;
            mainButton.Click += new EventHandler(this.showDropDown);
            // mainButton.UseVisualStyleBackColor = false;
            mainButtonColor = mainItemBackColor;
            subButtonColor = subItemBackColor;
            //  mainButton.Click += new EventHandler(this.button1_Click);
            int k = 0;
            int len = menuItemArray.Length;
            submenuButton = new Button[len];

            foreach (string menuItem in menuItemArray)
            {
                Button submenuButtons = new Button();
                //submenuButtons.Dock = DockStyle.Top;
                if (k == 0)
                {
                    submenuButtons.BackColor = mainItemBackColor;
                    submenuButtons.ForeColor = Color.White;
                }
                else
                {
                    if(menuStateArray[k] == 0)
                    {
                        submenuButtons.BackColor = subItemBackColor;
                        submenuButtons.ForeColor = Color.Black;
                        submenuButtons.Enabled = true;
                    }
                    else
                    {
                        submenuButtons.BackColor = Color.FromArgb(255, 217, 217, 217);
                        submenuButtons.ForeColor = Color.Black;
                        submenuButtons.Enabled = false;
                    }
                }
                submenuButtons.Location = new Point(0, buttonHeight * k);
                submenuButtons.Name = "submenuButton_" + k;
                submenuButtons.Size = new Size(buttonWidth, buttonHeight);
                submenuButtons.TabIndex = k + 1;
                submenuButtons.Text = menuDisplayPositionArray[k].ToString() + "-" + menuIDArray[k] + "  " + menuItemArray[k];
                //submenuButtons.UseVisualStyleBackColor = true;
                subPanels.Controls.Add(submenuButtons);
                submenuButtons.Click += new EventHandler(this.showTable);
                submenuButton[k] = submenuButtons;

                k++;
            }

            return mainPanels;
        }
        public Panel CreateCategoryDropDown1(string objectName, Panel mainPanel, string[] menuItemArray, int[] menuIDArray, int[] menuDisplayPositionArray, int[] menuStateArray, int left, int top, int width, int height, int maxWidth, int maxHeight, int minWidth, int minHeight, Color mainItemBackColor, Color subItemBackColor)
        {
            initValue();
            objecNameGlobal = objectName;
            Panel mainPanels = createPanel.CreateSubPanel(mainPanel, left, top, width, height, BorderStyle.None, Color.Transparent);
            mainPanels.MaximumSize = new Size(maxWidth, maxHeight);
            mainPanels.MinimumSize = new Size(minWidth, minHeight);
            Panel subPanels = createPanel.CreateSubPanel(mainPanels, 0, height, width, height, BorderStyle.None, Color.Transparent);
            subPanels.MaximumSize = new Size(maxWidth, maxHeight);
            subPanels.MinimumSize = new Size(minWidth, 0);
            mainPanelGlobal = mainPanels;
            subPanelGlobal = subPanels;

            int buttonWidth = maxWidth;
            int buttonHeight = maxHeight / (menuItemArray.Length + 1);

            Button mainButton = new Button();
            mainButton.BackColor = mainItemBackColor;
            mainButton.Dock = DockStyle.Top;
            mainButton.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            //   mainButton.Image = Image.FromFile(constants.dropdownArrowDownIcon);
            //   mainButton.Image = new Bitmap(Image.FromFile(constants.dropdownArrowDownIcon));
            mainButton.BackgroundImage = Image.FromFile(constants.dropdownArrowDownIcon);
            mainButton.BackgroundImageLayout = ImageLayout.Stretch;
            mainButton.ImageAlign = ContentAlignment.MiddleRight;
            mainButton.Padding = new Padding(0, 0, 10, 0);
            mainButton.Location = new Point(0, 0);
            mainButton.Name = "mainButton";
            mainButton.Size = new Size(buttonWidth, buttonHeight);
            mainButton.TabIndex = 0;
            mainButton.Text = menuDisplayPositionArray[0].ToString() + "-" + menuIDArray[0] + "  " + menuItemArray[0];
            mainPanels.Controls.Add(mainButton);
            mainButtonGlobal = mainButton;
            mainButton.Click += new EventHandler(this.showDropDown);
            // mainButton.UseVisualStyleBackColor = false;
            mainButtonColor = mainItemBackColor;
            subButtonColor = subItemBackColor;
            //  mainButton.Click += new EventHandler(this.button1_Click);
            int k = 0;
            int len = menuItemArray.Length;
            submenuButton = new Button[len];

            foreach (string menuItem in menuItemArray)
            {
                Button submenuButtons = new Button();
                //submenuButtons.Dock = DockStyle.Top;
                if (k == 0)
                {
                    submenuButtons.BackColor = mainItemBackColor;
                    submenuButtons.ForeColor = Color.White;
                }
                else
                {
                    submenuButtons.BackColor = subItemBackColor;
                    submenuButtons.ForeColor = Color.Black;
                    submenuButtons.Enabled = true;
                }
                submenuButtons.Location = new Point(0, buttonHeight * k);
                submenuButtons.Name = "submenuButton_" + k;
                submenuButtons.Size = new Size(buttonWidth, buttonHeight);
                submenuButtons.TabIndex = k + 1;
                submenuButtons.Text = menuDisplayPositionArray[k].ToString() + "-" + menuIDArray[k] + "  " + menuItemArray[k];
                //submenuButtons.UseVisualStyleBackColor = true;
                subPanels.Controls.Add(submenuButtons);
                submenuButtons.Click += new EventHandler(this.showTable);
                submenuButton[k] = submenuButtons;

                k++;
            }

            return mainPanels;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (isCollapsed)
            {
                mainButtonGlobal.BackgroundImage = Image.FromFile(constants.dropdownArrowUpIcon);
                mainButtonGlobal.BackgroundImageLayout = ImageLayout.Stretch;

                mainPanelGlobal.Height += 10;
                subPanelGlobal.Height += 10;
                if(subPanelGlobal.Height == 200)
                {
                    mainPanelGlobal.MaximumSize = new Size(mainPanelGlobal.Width, 200 + mainButtonGlobal.Height);
                    subPanelGlobal.MaximumSize = new Size(mainPanelGlobal.Width, 200);

                    subPanelGlobal.HorizontalScroll.Maximum = 0;
                    subPanelGlobal.AutoScroll = false;
                    subPanelGlobal.VerticalScroll.Visible = false;

                     //mainPanelGlobal.AutoScrollMargin = new Size(0, 40);
                   subPanelGlobal.AutoScroll = true;
                }
                if (mainPanelGlobal.Size == mainPanelGlobal.MaximumSize)
                {
                    timer.Stop();
                    isCollapsed = false;
                }
            }
            else
            {
                mainButtonGlobal.BackgroundImage = Image.FromFile(constants.dropdownArrowDownIcon);
                mainButtonGlobal.BackgroundImageLayout = ImageLayout.Stretch;

                mainPanelGlobal.Height -= 10;
                subPanelGlobal.Height -= 10;
                if (subPanelGlobal.Height < 200)
                {
                    subPanelGlobal.AutoScroll = false;
                }

                if (mainPanelGlobal.Size == mainPanelGlobal.MinimumSize)
                {
                    timer.Stop();
                    isCollapsed = true;
                }
            }
        }

        private void showDropDown(object sender, EventArgs e)
        {
            timer.Start();
        }

        private void showTable(object sender, EventArgs e)
        {
            Button btnTemp = (Button)sender;
            mainButtonGlobal.Text = btnTemp.Text;
            int k = 0;
            foreach(Button btnItem in submenuButton)
            {
                if(btnItem == btnTemp)
                {
                    btnItem.BackColor = mainButtonColor;
                    btnItem.ForeColor = Color.White;
                }
                else
                {
                    btnItem.BackColor = subButtonColor;
                    btnItem.ForeColor = Color.Black;
                    if (btnItem.Enabled == false)
                    {
                        btnItem.BackColor = Color.FromArgb(255, 217, 217, 217);
                        btnItem.ForeColor = Color.Black;
                    }
                }
                k++;
            }
            string sendIndex = btnTemp.Name.Split('_')[1].ToString();
            switch (objecNameGlobal)
            {
                case "soldoutSetting1":
                    soldoutSetting1Global.setVal(sendIndex);
                    timer.Start();
                    break;
                case "categoryList":
                    categoryListGlobal.setVal(sendIndex);
                    timer.Start();
                    break;
                case "groupList":
                    groupListGlobal.setVal(sendIndex);
                    timer.Start();
                    break;
                case "logReport":
                    detailViewGlobal.setVal("logReport", int.Parse(sendIndex).ToString("00"));
                    timer.Start();
                    break;
                case "falsePurchase":
                    detailViewGlobal.setVal("falsePurchase", sendIndex);
                    timer.Start();
                    break;
            }
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

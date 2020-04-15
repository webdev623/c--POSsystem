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
    public partial class PreviewSalePage : Form
    {
        Form1 mainFormGlobal = null;
        Panel mainPanelGlobal = null;
        Panel PanelGlobal = null;
        Panel PanelGlobal1 = null;
        PictureBox pBoxGlobal = null;
        Panel MainPanel = null;
        PictureBox MainPicture = null;
        Label MainMenuLabel = null;
        Constant constants = new Constant();
        CustomButton customButton = new CustomButton();

        CreatePanel createPanel = new CreatePanel();
        CreateLabel createLabel = new CreateLabel();
        OrderDialog orderDialog = new OrderDialog();

        int categoryIDGlobal = 0;

 
        int height = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height;
        int width = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width;

        public PreviewSalePage(Form1 mainForm, Panel mainPanel, int categoryID)
        {
            InitializeComponent();

            categoryIDGlobal = categoryID;

            mainFormGlobal = mainForm;
            mainPanelGlobal = mainPanel;

            Panel LeftPanel = createPanel.CreateMainPanel(mainForm, 0, 0, 3 * width / 4, height, BorderStyle.FixedSingle, Color.FromArgb(255, 255, 255, 204));
            Panel RightPanel = createPanel.CreateMainPanel(mainForm, width * 3 / 4, 0, width / 4, height, BorderStyle.FixedSingle, Color.White);
            FlowLayoutPanel FlowButtonLayout = createPanel.CreateFlowLayoutPanel(LeftPanel, 0, height / 7, LeftPanel.Width / 6, height * 4 / 7, Color.FromArgb(255, 255, 204), new Padding(20, 10, 0, 0));
            FlowLayoutPanel FlowTitleLayout = createPanel.CreateFlowLayoutPanel(LeftPanel, LeftPanel.Width / 6, 0, (LeftPanel.Width * 5) / 6, height / 7, Color.FromArgb(255, 255, 204), new Padding(10, 70, 0, 0));
            Panel MenuBodyLayout = createPanel.CreateSubPanel(LeftPanel, LeftPanel.Width / 6, height / 7, (LeftPanel.Width * 5) / 6, height * 6 / 7, BorderStyle.FixedSingle, Color.FromArgb(255, 255, 204));


            /**  Top Screen Title */


            Label MainTitle = new Label();
            //  MainTitle.Location = new Point(FlowTitleLayout.Left+200, FlowTitleLayout.Height/2-24);
            MainTitle.Width = FlowTitleLayout.Width;
            MainTitle.Height = FlowTitleLayout.Height - 70;
            MainTitle.Font = new Font("Seri", 24, FontStyle.Bold);
            MainTitle.ForeColor = Color.FromArgb(255, 255, 0, 0);
            MainTitle.Text = constants.saleScreenTopTitle;
            MainTitle.Text = constants.saleScreenTopTitle;
            //LeftPanel.Controls.Remove(FlowTitleLayout);
            FlowTitleLayout.Controls.Add(MainTitle);

            /** Left category menu button create */

            Color[] saleCategoryButtonColor = constants.getSaleCategoryButtonColor();
            Color[] saleCategoryButtonBorderColor = constants.getSaleCategoryButtonBorderColor();

            int k = 0;
            foreach (string category in constants.saleCategories)
            {
                string categoryButtonText = category;
                string categoryButtonName = constants.saleCategories_btnName[k];
                Color backColor = saleCategoryButtonColor[k];
                Color borderColor = saleCategoryButtonBorderColor[k];
                if (k == categoryID)
                {
                    borderColor = Color.Red;
                }
                int btnLeft = FlowButtonLayout.Left + 10;
                int btnTop = (FlowButtonLayout.Top + 10) + (FlowButtonLayout.Height / 5) * k;
                int btnWidth = FlowButtonLayout.Width - 25;
                int btnHeight = FlowButtonLayout.Height / 5 - 10;
                int borderSize = 5;
                Button btn = customButton.CreateButton(categoryButtonText, categoryButtonName, btnLeft, btnTop, btnWidth, btnHeight, backColor, borderColor, borderSize);
                FlowButtonLayout.Controls.Add(btn);
                /* btn.Click += new System.EventHandler(this.btnSettingSubClick); */
                k++;
            }

            /** Main Product Panel layout */

            CreateMainProductPanel(MenuBodyLayout);
            CreateSubProductPanel(MenuBodyLayout);

            /** right panel  */
            RightPanel.Padding = new Padding(10, 0, 10, 0);
         
            Button closeButton = customButton.CreateButton(constants.backText, "closeButton", RightPanel.Width / 2 - 100, RightPanel.Height * 6 / 7, 200, 50, Color.FromArgb(255, 0, 112, 192), Color.Transparent, 0, 1, 18);
            RightPanel.Controls.Add(closeButton);
            closeButton.Click += new EventHandler(this.BackShow);

        }

        private void CreateMainProductPanel(Panel parentPanel)
        {
            int i = 0;
            int m = 0;
            for (int k = 0; k < 4; k++)
            {
                Panel Panels = new Panel();
                Panels.Name = "panel_big_" + k.ToString();
                Panels.Size = new Size((parentPanel.Width * 2 / 5) - 30, (parentPanel.Height / 3) - 30);
                if (k % 2 == 0)
                {
                    Panels.Location = new Point(20, 20 + ((parentPanel.Height / 3) - 10) * i);
                    i++;
                }
                else
                {
                    Panels.Location = new Point((parentPanel.Width * 2 / 5) + 10, 20 + ((parentPanel.Height / 3) - 10) * m);
                    m++;
                }
                Panels.BorderStyle = BorderStyle.FixedSingle;
                Panels.BackColor = Color.White;

                PanelGlobal1 = Panels;
                Panels.Paint += new PaintEventHandler(this.panel_Paint1);

                parentPanel.Controls.Add(Panels);

                string productName = constants.productBigName[categoryIDGlobal][k];
                int productPrice = constants.productBigPrice[categoryIDGlobal][k];

                // add Images into menu panel
                if (constants.productBigImageUrl[k] != "")
                {
                    string bigImageUrl = constants.productBigImageUrl[k];
                    AddImageOnPanel(Panels, bigImageUrl, productName, productPrice, k, "big");
                }
                if (constants.productBigBadgeImageUrl[k] != "")
                {
                    string badgeImageUrl = constants.productBigBadgeImageUrl[k];
                    AddImageOnPanelBadge(Panels, badgeImageUrl, k, "big");
                }

                // modal dialog show
                MainPanel = Panels;
            }
            // return Panel;
        }


        private void CreateSubProductPanel(Panel parentPanel)
        {
            for (int k = 0; k < 4; k++)
            {
                Panel Panel0 = new Panel();
                Panel0.Name = "panel_small_" + k.ToString();

                Panel0.Size = new Size((parentPanel.Width / 5) - 25, (parentPanel.Height / 6) - 25);
                Panel0.Location = new Point((parentPanel.Width * 4 / 5), 20 + (parentPanel.Height / 6 - 5) * k);
                Panel0.BorderStyle = BorderStyle.FixedSingle;
                Panel0.BackColor = Color.White;

                Panel0.Paint += new PaintEventHandler(this.panel_Paint);
                PanelGlobal = Panel0;
                //Color.FromArgb(255, 79, 129, 189);
                parentPanel.Controls.Add(Panel0);

                string productName = constants.productSmallName[k];
                int productPrice = constants.productSmallPrice[k];

                if (constants.productSmallImageUrl[k] != "")
                {
                    string smallImageUrl = constants.productSmallImageUrl[k];
                    AddImageOnPanel(Panel0, smallImageUrl, productName, productPrice, k, "small");
                }
                if (constants.productSmallBadgeImageUrl[k] != "")
                {
                    string smallImageUrl = constants.productSmallBadgeImageUrl[k];
                    AddImageOnPanelBadge(Panel0, smallImageUrl, k, "small");
                }


            }
            for (int k = 0; k < 5; k++)
            {
                Panel Panel1 = new Panel();
                Panel1.Name = "panel_small_" + (k + 4).ToString();
                Panel1.Size = new Size((parentPanel.Width / 5) - 25, (parentPanel.Height / 6) - 25);
                Panel1.Location = new Point(20 + ((parentPanel.Width / 5) - 5) * k, (parentPanel.Height * 2 / 3));
                Panel1.BorderStyle = BorderStyle.FixedSingle;
                Panel1.BackColor = Color.White;
                PanelGlobal = Panel1;
                Panel1.Paint += new PaintEventHandler(this.panel_Paint);
                //Color.FromArgb(255, 79, 129, 189);
                parentPanel.Controls.Add(Panel1);

                string productName = constants.productSmallName[k + 4];
                int productPrice = constants.productSmallPrice[k + 4];

                if (constants.productSmallImageUrl[k + 4] != "")
                {
                    string smallImageUrl = constants.productSmallImageUrl[k + 4];
                    AddImageOnPanel(Panel1, smallImageUrl, productName, productPrice, k + 4, "small");
                }
                if (constants.productSmallBadgeImageUrl[k + 4] != "")
                {
                    string smallImageUrl = constants.productSmallBadgeImageUrl[k + 4];
                    AddImageOnPanelBadge(Panel1, smallImageUrl, k + 4, "small");
                }

            }
        }

        private void panel_Paint(object sender, PaintEventArgs e)
        {
            if (PanelGlobal.BorderStyle == BorderStyle.FixedSingle)
            {
                int thickness = 5;//it's up to you
                int halfThickness = thickness / 2;
                using (Pen p = new Pen(Color.FromArgb(255, 79, 129, 189), thickness))
                {
                    e.Graphics.DrawRectangle(p, new Rectangle(halfThickness,
                                                              halfThickness,
                                                              PanelGlobal.ClientSize.Width - thickness,
                                                              PanelGlobal.ClientSize.Height - thickness));
                }
            }
        }

        private void panel_Paint1(object sender, PaintEventArgs e)
        {
            if (PanelGlobal1.BorderStyle == BorderStyle.FixedSingle)
            {
                int thickness = 5;//it's up to you
                int halfThickness = thickness / 2;
                using (Pen p = new Pen(Color.FromArgb(255, 79, 129, 189), thickness))
                {
                    e.Graphics.DrawRectangle(p, new Rectangle(halfThickness,
                                                              halfThickness,
                                                              PanelGlobal1.ClientSize.Width - thickness,
                                                              PanelGlobal1.ClientSize.Height - thickness));
                }
            }
        }

        private void AddImageOnPanel(Panel pnl, string imageUrl, string productName, int productPrice, int index, string panelSize)
        {
            PictureBox pBox = new PictureBox();
            pBox.Name = "pBox_" + panelSize + "_" + index.ToString();
            pBox.Width = pnl.Width * 6 / 7 - 10;
            pBox.Height = pnl.Height * 4 / 5 - 10;
            pBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pBox.BackColor = Color.White;

            pBox.Location = new Point((pnl.Width - pBox.Width) / 2, 5);
            pBox.ImageLocation = imageUrl; // @"D:\\ovan\\Ovan_P1\\images\\category1.png";
            pBoxGlobal = pBox;

            pnl.Controls.Add(pBox);

            Label categoryLabel = new Label();

            categoryLabel.Name = "label_" + panelSize + "_" + index.ToString();
            if (panelSize == "big")
            {
                categoryLabel.Text = productName + "\n" + productPrice + constants.unit;
            }
            else if (panelSize == "small")
            {
                categoryLabel.Text = productName + productPrice + constants.unit;
            }
            categoryLabel.Location = new Point(15, pnl.Height * 4 / 5);
            categoryLabel.Size = new Size(pnl.Width - 30, pnl.Height / 5 - 10);
            pnl.Controls.Add(categoryLabel);
            // modal dialog show
            MainMenuLabel = categoryLabel;

            MainPicture = pBox;
        }

        private void AddImageOnPanelBadge(Panel pnl, string imageUrl, int index, string panelSize)
        {
            PictureBox pBox = new PictureBox();
            pBox.Name = "pBox_" + panelSize + "_" + index.ToString();
            pBox.Width = pnl.Width * 2 / 7;
            pBox.Height = pnl.Height * 1 / 3;
            pBox.BackColor = Color.Transparent;
            pBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pBox.Location = new Point(pnl.Width * 4 / 7, 10);
            pBox.ImageLocation = imageUrl; // @"D:\\ovan\\Ovan_P1\\images\\badge1.png";
            pBoxGlobal.Controls.Add(pBox);

            // modal dialog show
            MainPicture = pBox;

        }

        public void BackShow(object sender, EventArgs e)
        {
            mainFormGlobal.Controls.Clear();
            CategoryList frm = new CategoryList(mainFormGlobal, mainPanelGlobal);
            frm.TopLevel = false;
            mainFormGlobal.Controls.Add(frm);
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;
            Thread.Sleep(200);
            frm.Show();
        }


    }
}

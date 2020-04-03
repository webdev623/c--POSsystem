using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.HtmlControls;
using System.Windows.Forms;

namespace Ovan_P1
{
    public partial class SaleScreen : Form
    {

        //  Form FormPanel = null;
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
        Panel TableBackPanelGlobal = null;

        public int orderTotalPrice = 0;
        public int currentSelectedId = 0;

        private int selectedCategoryIndex = 0;
        private Label[] orderProductNameLabel = new Label[100];
        private Label[] orderAmountLabel = new Label[100];
        private Label[] orderDeleteLabel = new Label[100];
        private Button[] orderIncreaseButton = new Button[100];
        private Button[] orderDecreaseButton = new Button[100];
        
        private Label orderPriceTotalLabel = null;

        private string[] orderProductNameArray = new string[100];
        private string[] orderAmountArray = new string[100];
        private string[] orderProductPriceArray = new string[100];
        private int startIndex = 0;
        private int lastIndex = 4;
        int height = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height;
        int width = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width;

        public SaleScreen(Form1 panel)
        {
            orderDialog.initValue(this);

            Panel LeftPanel = createPanel.CreateMainPanel(panel, 0, 0, 3 * width / 4, height, BorderStyle.FixedSingle, Color.FromArgb(255, 255, 255, 204));
            Panel RightPanel = createPanel.CreateMainPanel(panel, width * 3 / 4, 0, width / 4, height, BorderStyle.FixedSingle, Color.White);
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
            foreach(string category in constants.saleCategories)
            {
                string categoryButtonText = category;
                string categoryButtonName = constants.saleCategories_btnName[k];
                Color backColor = saleCategoryButtonColor[k];
                Color borderColor = saleCategoryButtonBorderColor[k];
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

            // Top Button Create
            Image upButtonImage = Image.FromFile(@"D:\\ovan\\Ovan_P1\\images\\upButton.png");
            string upButtonName = constants.upButtonName;
            int upButtonLeft = 10;
            int upButtonTop = 100;
            int upButtonWidth = RightPanel.Width - 30;
            int upButtonHeight = 50;
            int upButtonBorderSize = 5;
            int upButtonRadiusValue = 20;

            Button topButton = customButton.CreateButtonWithImage(upButtonImage, upButtonName, "", upButtonLeft, upButtonTop, upButtonWidth, upButtonHeight, upButtonBorderSize, upButtonRadiusValue);
            RightPanel.Controls.Add(topButton);

            // Create Tabel 
            Panel TableBackPanel = createPanel.CreateSubPanel(RightPanel, 10, topButton.Bottom + 10, RightPanel.Width - 30, RightPanel.Height / 5, BorderStyle.None, Color.FromArgb(255, 191, 191, 191));
            TableBackPanelGlobal = TableBackPanel;
            FlowLayoutPanel deleteColumnPanel = createPanel.CreateFlowLayoutPanel(TableBackPanel, 5, 5, TableBackPanel.Width / 5, TableBackPanel.Height - 10, Color.FromArgb(255, 191, 191, 191), new Padding(0, 0, 0, 0));
            FlowLayoutPanel bookProductColumnPanel = createPanel.CreateFlowLayoutPanel(TableBackPanel, deleteColumnPanel.Width + 5, 5, TableBackPanel.Width * 2 / 5 - 10, TableBackPanel.Height - 10, Color.FromArgb(255, 191, 191, 191), new Padding(0, 0, 0, 0));
            FlowLayoutPanel productNumberColumnPanel = createPanel.CreateFlowLayoutPanel(TableBackPanel, bookProductColumnPanel.Width + deleteColumnPanel.Width + 5, 5, TableBackPanel.Width * 2 / 15, TableBackPanel.Height - 10, Color.FromArgb(255, 191, 191, 191), new Padding(0, 0, 0, 0));
            FlowLayoutPanel productIncreaseColumnPanel = createPanel.CreateFlowLayoutPanel(TableBackPanel, productNumberColumnPanel.Width + bookProductColumnPanel.Width + deleteColumnPanel.Width + 5, 5, TableBackPanel.Width * 2 / 15, TableBackPanel.Height - 10, Color.FromArgb(255, 191, 191, 191), new Padding(0, 0, 0, 0));
            FlowLayoutPanel productDecreaseColumnPanel = createPanel.CreateFlowLayoutPanel(TableBackPanel, productNumberColumnPanel.Width * 2 + bookProductColumnPanel.Width + deleteColumnPanel.Width + 5, 5, TableBackPanel.Width * 2 / 15, TableBackPanel.Height - 10, Color.FromArgb(255, 191, 191, 191), new Padding(0, 0, 0, 0));
            for (int i = 0; i < 5; i++)
            {
                Label deleteLabel = createLabel.CreateLabels(deleteColumnPanel, "del_" + i, constants.deleteText, 0, (deleteColumnPanel.Height / 5 + 10) * i, deleteColumnPanel.Width - 10, deleteColumnPanel.Height / 5 - 10, Color.FromArgb(255, 255, 0, 0), Color.White, 12);
                Label bookProductLabel = createLabel.CreateLabels(bookProductColumnPanel, "prd_" + i, "", 0, (bookProductColumnPanel.Height / 5 + 10) * i, bookProductColumnPanel.Width - 10, bookProductColumnPanel.Height / 5 - 10, Color.White, Color.Black, 12);
                Label productNumberLabel = createLabel.CreateLabels(productNumberColumnPanel, "prdNum_" + i, "", 0, (productNumberColumnPanel.Height / 5 + 10) * i, productNumberColumnPanel.Width - 10, productNumberColumnPanel.Height / 5 - 10, Color.White, Color.Black, 12);

                orderDeleteLabel[i] = deleteLabel;
                orderAmountLabel[i] = productNumberLabel;
                orderProductNameLabel[i] = bookProductLabel;

                Image increaseButtonImage = Image.FromFile(@"D:\\ovan\\Ovan_P1\\images\\increaseButton.png");
                Button productIncreaseButton = customButton.CreateButtonWithImage(increaseButtonImage, "prdIncrease_" + i, "", 0, (productIncreaseColumnPanel.Height / 5 + 10) * i, productIncreaseColumnPanel.Width - 6, productIncreaseColumnPanel.Height / 5 - 6, 0, 1);
                productIncreaseColumnPanel.Controls.Add(productIncreaseButton);
                orderIncreaseButton[i] = productIncreaseButton;
                productIncreaseButton.Click += new EventHandler(this.orderAmountChange);

                Image decreaseButtonImage = Image.FromFile(@"D:\\ovan\\Ovan_P1\\images\\decreaseButton.png");
                Button productDecreaseButton = customButton.CreateButtonWithImage(decreaseButtonImage, "prdDecrease_" + i, "", 0, (productDecreaseColumnPanel.Height / 5 + 10) * i, productDecreaseColumnPanel.Width - 6, productDecreaseColumnPanel.Height / 5 - 6, 0, 1);
                productDecreaseColumnPanel.Controls.Add(productDecreaseButton);
                orderDecreaseButton[i] = productDecreaseButton;
                productDecreaseButton.Click += new EventHandler(this.orderAmountChange);

            }


            // Bottom Button Create
            Image downButtonImage = Image.FromFile(@"D:\\ovan\\Ovan_P1\\images\\downButton.png");
            string downButtonName = constants.downButtonName;
            int downButtonLeft = 10;
            int downButtonTop = TableBackPanel.Bottom + 10;
            int downButtonWidth = RightPanel.Width - 30;
            int downButtonHeight = 50;
            int downButtonBorderSize = 5;
            int downButtonRadiusValue = 20;

            Button downButton = customButton.CreateButtonWithImage(downButtonImage, downButtonName, "", downButtonLeft, downButtonTop, downButtonWidth, downButtonHeight, downButtonBorderSize, downButtonRadiusValue);
            RightPanel.Controls.Add(downButton);

            // top and bottom button action
            topButton.Click += new EventHandler(this.OrderTableView);
            downButton.Click += new EventHandler(this.OrderTableView);

            // transaction result show
            Panel TransactionPanel = createPanel.CreateSubPanel(RightPanel, 10, downButton.Bottom + 30, RightPanel.Width - 30, RightPanel.Height * 7 / 30, BorderStyle.None, Color.White);
            FlowLayoutPanel transactionLabelPanel = createPanel.CreateFlowLayoutPanel(TransactionPanel, 5, 5, TransactionPanel.Width * 3 / 7 - 20, TransactionPanel.Height - 10, Color.White, new Padding(0, 0, 0, 0));
            FlowLayoutPanel transactionResultPanel = createPanel.CreateFlowLayoutPanel(TransactionPanel, transactionLabelPanel.Right + 5, 5, TransactionPanel.Width * 3 / 7, TransactionPanel.Height - 10, Color.White, new Padding(0, 0, 0, 0));
            FlowLayoutPanel transactionUnitPanel = createPanel.CreateFlowLayoutPanel(TransactionPanel, transactionResultPanel.Right + 5, 5, TransactionPanel.Width * 1 / 7, TransactionPanel.Height - 10, Color.White, new Padding(0, 0, 0, 0));
 
            for(int m = 0; m < 3; m++)
            {
                Label transactionLabel = createLabel.CreateLabels(transactionLabelPanel, "transLabel_" + m, constants.transactionLabelName[m], 0, ((transactionLabelPanel.Height / 3) + 10) * m, transactionLabelPanel.Width - 10, transactionLabelPanel.Height / 3 - 10, Color.White, Color.FromArgb(255,81, 163, 211), 14);
                Label transactionResult = createLabel.CreateLabels(transactionResultPanel, "transResult_" + m, "", 0, ((transactionResultPanel.Height / 3) + 10) * m, transactionResultPanel.Width - 10, transactionResultPanel.Height / 3 - 10, Color.White, Color.FromArgb(255, 81, 163, 211), 14, true);
                Label transactionUnit = createLabel.CreateLabels(transactionUnitPanel, "transUnit_" + m, "円", 0, ((transactionUnitPanel.Height / 3) + 10) * m, transactionUnitPanel.Width - 10, transactionUnitPanel.Height / 3 - 10, Color.White, Color.FromArgb(255, 81, 163, 211), 14);
                if(m == 1)
                {
                    orderPriceTotalLabel = transactionResult;
                }
            }

            // payment button show
            Panel PaymentButtonPanel = createPanel.CreateSubPanel(RightPanel, 10, TransactionPanel.Bottom + 30, RightPanel.Width - TransactionPanel.Width / 6, RightPanel.Height - TransactionPanel.Bottom -30, BorderStyle.None, Color.White);
            FlowLayoutPanel paymentButtonTopPanel = createPanel.CreateFlowLayoutPanel(PaymentButtonPanel, 5, 5, PaymentButtonPanel.Width - 10, 70, Color.White, new Padding(0, 0, 0, 0));
            FlowLayoutPanel paymentButtonBottomPanel = createPanel.CreateFlowLayoutPanel(PaymentButtonPanel, 5, paymentButtonTopPanel.Bottom + 5, PaymentButtonPanel.Width - 10, 70, Color.White, new Padding(0, 0, 0, 0));
            Button ticketingButton = customButton.CreateButton(constants.ticketingButtonText, "ticketingButton", 0, 5, paymentButtonTopPanel.Width / 2 - 10, paymentButtonTopPanel.Height - 10, Color.FromArgb(255, 0, 176, 80), Color.FromArgb(255, 85, 142, 213), 5, 14, 20, FontStyle.Bold, Color.White);
            paymentButtonTopPanel.Controls.Add(ticketingButton);
            Button cancelButton = customButton.CreateButton(constants.cancelButtonText, "cancelButton", paymentButtonTopPanel.Width / 2 + 10, 5, paymentButtonTopPanel.Width / 2 - 10, paymentButtonTopPanel.Height - 10, Color.FromArgb(255, 255, 0, 0), Color.FromArgb(255, 185, 205, 229), 5, 14, 20, FontStyle.Bold, Color.White);
            paymentButtonTopPanel.Controls.Add(cancelButton);
            Button receiptButton = customButton.CreateButton(constants.receiptButtonText, "receiptButton", 0, 5, paymentButtonBottomPanel.Width - 10, paymentButtonBottomPanel.Height - 10, Color.FromArgb(255, 217, 217, 217), Color.Transparent, 1);
            paymentButtonBottomPanel.Controls.Add(receiptButton);
        }

        private void CreateMainProductPanel(Panel parentPanel)
        {
            int i = 0;
            int m = 0;
            for(int k = 0; k < 4; k++)
            {
                Panel Panels = new Panel();
                Panels.Name = "panel_big_"+ k.ToString();
                Panels.Size = new Size((parentPanel.Width * 2 / 5 ) - 30, (parentPanel.Height / 3) - 30);
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

                string productName = constants.productBigName[selectedCategoryIndex][k];
                int productPrice = constants.productBigPrice[selectedCategoryIndex][k];

                // add Images into menu panel
                if(constants.productBigImageUrl[k] != "")
                {
                    string bigImageUrl = constants.productBigImageUrl[k];
                    AddImageOnPanel(Panels, bigImageUrl, productName, productPrice, k, "big");
                }
                if(constants.productBigBadgeImageUrl[k] != "")
                {
                    string badgeImageUrl = constants.productBigBadgeImageUrl[k];
                    AddImageOnPanelBadge(Panels, badgeImageUrl, k, "big");
                }

                // modal dialog show
                MainPanel = Panels;
                Panels.Click += new EventHandler(orderDialog.ShowMainMenuDetailFromPanel);
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

                if(constants.productSmallImageUrl[k] != "")
                {
                    string smallImageUrl = constants.productSmallImageUrl[k];
                    AddImageOnPanel(Panel0, smallImageUrl, productName, productPrice, k, "small");
                }
                if (constants.productSmallBadgeImageUrl[k] != "")
                {
                    string smallImageUrl = constants.productSmallBadgeImageUrl[k];
                    AddImageOnPanelBadge(Panel0, smallImageUrl, k, "small");
                }

                Panel0.Click += new EventHandler(orderDialog.ShowMainMenuDetailFromPanel);

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

                Panel1.Click += new EventHandler(orderDialog.ShowMainMenuDetailFromPanel);
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
            if(panelSize == "big")
            {
                categoryLabel.Text = productName + "\n" + productPrice + "円";
            }
            else if(panelSize == "small")
            {
                categoryLabel.Text = productName + productPrice + "円";
            }
            categoryLabel.Location = new Point(15, pnl.Height * 4 / 5);
            categoryLabel.Size = new Size(pnl.Width - 30, pnl.Height / 5 - 10);
            pnl.Controls.Add(categoryLabel);
            // modal dialog show
            MainMenuLabel = categoryLabel;
            categoryLabel.Click += new EventHandler(orderDialog.ShowMainMenuDetailFromLabel);

            MainPicture = pBox;
            pBox.Click += new EventHandler(orderDialog.ShowMainMenuDetailFromPictureBox);
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
            pBox.Click += new EventHandler(orderDialog.ShowMainMenuDetailFromPictureBox);

        }

        private void CreateOrderTable(int startIndex)
        {

            for (int i = 0; i < 5; i++)
            {

                orderProductNameLabel[i].Name = "prd_" + (i + startIndex);
                orderProductNameLabel[i].Text = orderProductNameArray[i + startIndex];
                orderAmountLabel[i].Name = "prdNum_" + (i + startIndex);
                orderAmountLabel[i].Text = orderAmountArray[i + startIndex];
                orderDeleteLabel[i].Name = "del_" + (i + startIndex);
                orderIncreaseButton[i].Name = "prdIncrease_" + (i + startIndex);
                orderDecreaseButton[i].Name = "prdDecrease_" + (i + startIndex);

            }

        }

        private void OrderTableView(object sender, EventArgs e)
        {
            Button tBtn = (Button)sender;
            switch(tBtn.Name)
            {
                case "upButton":
                    if(startIndex > 0)
                    {
                        startIndex--;
                        CreateOrderTable(startIndex);
                    }
                    break;
                case "downButton":
                    if(startIndex + 4 < lastIndex)
                    {
                        startIndex++;
                        CreateOrderTable(startIndex);
                    }
                    break;
            }
        }

        private void orderAmountChange(object sender, EventArgs e)
        {
            Button tempBtn = (Button)sender;
            string[] btnName = tempBtn.Name.Split('_');
            int selectedIndex = int.Parse(btnName[1]);
            int orderAmount = int.Parse(orderAmountLabel[selectedIndex - startIndex].Text);
            if (btnName[0] == "prdIncrease" && orderAmount < 9)
            {
                orderAmount++;
                orderTotalPrice += int.Parse(orderProductPriceArray[selectedIndex]);
            }
            else if(btnName[0] == "prdDecrease" && orderAmount > 0)
            {
                orderAmount--;
                orderTotalPrice -= int.Parse(orderProductPriceArray[selectedIndex]);
            }
            orderAmountArray[selectedIndex] = orderAmount.ToString();
            orderAmountLabel[selectedIndex - startIndex].Text = orderAmount.ToString();
            orderPriceTotalLabel.Text = orderTotalPrice.ToString();
        }
        public void setVal(string[] s)
        {
            string[] msgValue = s;
            orderProductNameArray[currentSelectedId] = msgValue[0];
            orderAmountArray[currentSelectedId] = msgValue[1];
            orderProductPriceArray[currentSelectedId] = msgValue[3];

            if (currentSelectedId < 5)
            {
                orderProductNameLabel[currentSelectedId].Text = msgValue[0];
                orderAmountLabel[currentSelectedId].Text = msgValue[1];
            }
            else
            {
                startIndex = currentSelectedId - 4;
                lastIndex = startIndex + 4;
                CreateOrderTable(startIndex);
              //  orderProductNameLabel[currentSelectedId].Text = msgValue[0];
              //  orderAmountLabel[currentSelectedId].Text = msgValue[1];
            }
            orderTotalPrice += int.Parse(msgValue[2]);
            orderPriceTotalLabel.Text = orderTotalPrice.ToString();
            currentSelectedId++;
        }
    }
}

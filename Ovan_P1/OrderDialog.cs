using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Ovan_P1
{
    class OrderDialog: Form
    {
        private string productName = "";
        private int productPriceValue = 0;

        int selectedCategoryIndex = 0;
        int height = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height;
        int width = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width;
        Constant constants = new Constant();
        CreatePanel createPanel = new CreatePanel();
        CreateLabel createLabel = new CreateLabel();
        CustomButton createButton = new CustomButton();
        RoundedFormDialog DialogFormGlobal = new RoundedFormDialog();
        int productAmounts = 1;
        Label productAmountValue = null;
        SaleScreen pSaleScreen = null;
        int restAmountGlobal = 9;
        public void initValue(SaleScreen saleHandler)
        {
            pSaleScreen = saleHandler;

        }
        public void ShowMainMenuDetail(string prdName, string prdPrice, Image prdImage, int restAmount)
        {
            restAmountGlobal = (restAmount <= 9) ? restAmount : 9;
            //Panel panelTemp = (Panel)sender;
            RoundedFormDialog dialogForm = new RoundedFormDialog();
            dialogForm.Size = new Size(width / 3, height / 3);
            dialogForm.BackColor = Color.White;
            dialogForm.radiusValue = 50;
            dialogForm.borderColor = Color.Black;
            dialogForm.borderSize = 2;
            dialogForm.StartPosition = FormStartPosition.CenterParent;
            dialogForm.WindowState = FormWindowState.Normal;
            dialogForm.ControlBox = false;
            dialogForm.FormBorderStyle = FormBorderStyle.None;
            DialogFormGlobal = dialogForm;
            string dialogTitleText = constants.dialogTitle;
            string dialogInstructionText = constants.dialogInstruction;
            Image productImage = null;

                productName = prdName;
                productPriceValue = int.Parse(prdPrice);
                productImage = prdImage;

            Panel dialogPanel = createPanel.CreateMainPanel(dialogForm, 10, 10, dialogForm.Width - 20, dialogForm.Height - 20, BorderStyle.None, Color.White);

            Label dialogTitle = createLabel.CreateLabelsInPanel(dialogPanel, "dialogTitle", dialogTitleText, 0, 10, dialogPanel.Width - 10, dialogPanel.Height / 6, Color.White, Color.FromArgb(255, 39, 127, 196), 18);

            PictureBox pBox = new PictureBox();
            pBox.Width = dialogPanel.Width / 2 - 60;
            pBox.Height = dialogPanel.Height * 2 / 5 - 10;
            pBox.BackColor = Color.Transparent;
            pBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pBox.Location = new Point(50, dialogTitle.Bottom);
            pBox.Image = productImage; // @"D:\\ovan\\Ovan_P1\\images\\badge1.png";
            dialogPanel.Controls.Add(pBox);


            Label productTitle = createLabel.CreateLabelsInPanel(dialogPanel, "productTitle", productName, dialogPanel.Width / 2, dialogTitle.Bottom + 10, dialogPanel.Width / 2 - 60, dialogPanel.Height / 6, Color.White, Color.Black, dialogPanel.Height / 16, false, ContentAlignment.MiddleRight);
            Label productPrice = createLabel.CreateLabelsInPanel(dialogPanel, "productPrice", productPriceValue + constants.unit, dialogPanel.Width / 2, productTitle.Bottom, dialogPanel.Width / 2 - 60, dialogPanel.Height / 6, Color.White, Color.Black, dialogPanel.Height / 16, false, ContentAlignment.MiddleRight);

            Label dialogInstruction = createLabel.CreateLabelsInPanel(dialogPanel, "dialogInstruction", dialogInstructionText, dialogPanel.Left + 20, productPrice.Bottom + 5, dialogPanel.Width - 50, dialogPanel.Height / 6, Color.White, Color.Black, dialogPanel.Height / 20);

          //  Panel productOrderPanel = createPanel.CreateSubPanel(dialogPanel, dialogPanel.Left + dialogPanel.Width / 3, dialogInstruction.Bottom + 10, dialogPanel.Width / 3, 50, BorderStyle.FixedSingle, Color.Red);
            Panel productOrderLeftPanel = createPanel.CreateSubPanel(dialogPanel, dialogPanel.Left + dialogPanel.Width / 3, dialogInstruction.Bottom + 10, dialogPanel.Width / 3 * 3 / 4, dialogPanel.Height / 6, BorderStyle.FixedSingle, Color.Yellow);
            Panel productOrderRightPanel = createPanel.CreateSubPanel(dialogPanel, productOrderLeftPanel.Right, productOrderLeftPanel.Top, dialogPanel.Width / 3 / 4, dialogPanel.Height / 6, BorderStyle.FixedSingle, Color.Yellow);

            Label productAmount = createLabel.CreateLabelsInPanel(productOrderLeftPanel, "productAmount", constants.productAmount[0], 0, 0, productOrderLeftPanel.Width * 2 / 3, productOrderLeftPanel.Height, Color.FromArgb(255, 191, 191, 191), Color.Black, dialogPanel.Height / 20);
            Label productUp = createLabel.CreateLabelsInPanel(productOrderLeftPanel, "productAmountUp", "▲", productAmount.Right, 0, productOrderLeftPanel.Width / 3, productOrderLeftPanel.Height / 2, Color.FromArgb(255, 191, 191, 191), Color.Black, dialogPanel.Height / 24, false, ContentAlignment.BottomCenter);
            Label productDown = createLabel.CreateLabelsInPanel(productOrderLeftPanel, "productAmountDown", "▼", productAmount.Right, productUp.Bottom, productOrderLeftPanel.Width / 3, productOrderLeftPanel.Height / 2, Color.FromArgb(255, 191, 191, 191), Color.Black, dialogPanel.Height / 24, false, ContentAlignment.TopCenter);
            Label productAmountSelect = createLabel.CreateLabelsInPanel(productOrderRightPanel, "productAmountSelect", "決\n定", 0, 0, productOrderRightPanel.Width, productOrderLeftPanel.Height, Color.FromArgb(255, 191, 191, 191), Color.Black, dialogPanel.Height / 24);
            // CreateDropDown(dialogPanel, dialogPanel.Left + dialogPanel.Width / 3, dialogInstruction.Bottom + 10, dialogPanel.Width / 6, 50, "productAmount", Color.FromArgb(255, 191, 191, 191), Color.Black, Color.FromArgb(255, 191, 191, 191), constants.productAmount, 0);
            productAmountValue = productAmount;
            productUp.Click += new EventHandler(this.ChangeProductAmount);
            productDown.Click += new EventHandler(this.ChangeProductAmount);

            productAmountSelect.Click += new EventHandler(this.CloseDialog);

            dialogForm.ShowDialog();

        }

        public void ShowTicketingDetail(string[] orderProductNameArray, string[] orderProductPriceArray, string[] orderAmountArray, int orderRowNum)
        {
            //Panel panelTemp = (Panel)sender;
            RoundedFormDialog dialogForm = new RoundedFormDialog();
            dialogForm.Size = new Size(width / 3, orderRowNum * 50 + 250);
            dialogForm.BackColor = Color.White;
            dialogForm.radiusValue = 50;
            dialogForm.borderColor = Color.Black;
            dialogForm.borderSize = 2;
            dialogForm.StartPosition = FormStartPosition.CenterParent;
            dialogForm.WindowState = FormWindowState.Normal;
            dialogForm.ControlBox = false;
            dialogForm.FormBorderStyle = FormBorderStyle.None;
            DialogFormGlobal = dialogForm;
            string dialogTitleText = constants.orderDialogRunText;
            string dialogInstructionText = constants.dialogInstruction;
            Image productImage = null;


            Panel dialogPanel = createPanel.CreateMainPanel(dialogForm, dialogForm.Width / 15, 10, dialogForm.Width * 13 / 15, dialogForm.Height - 20, BorderStyle.None, Color.White);

            Label dialogTitle = createLabel.CreateLabelsInPanel(dialogPanel, "dialogTitle", dialogTitleText, 0, 10, dialogPanel.Width - 10, 50, Color.White, Color.FromArgb(255, 39, 127, 196), 18);

            int totalAmount = 0;
            int totalPrice = 0;
            for(int k = 0; k < orderRowNum; k++)
            {
                int orderPrice = int.Parse(orderProductPriceArray[k]) * int.Parse(orderAmountArray[k]);
                totalPrice += orderPrice;
                totalAmount += int.Parse(orderAmountArray[k]);
                FlowLayoutPanel orderRowPanel = createPanel.CreateFlowLayoutPanel(dialogPanel, 0, (k + 1) * 50, dialogPanel.Width, 50, Color.Transparent, new Padding(dialogPanel.Width / 15, 0, 0, 0));
                Label prdNameLabel = createLabel.CreateLabels(orderRowPanel, "productName_" + k, orderProductNameArray[k], 0, 0, orderRowPanel.Width / 3, orderRowPanel.Height, Color.White, Color.Black, orderRowPanel.Height / 3, false, ContentAlignment.MiddleLeft);
                Label prdAmountLabel = createLabel.CreateLabels(orderRowPanel, "productAmount_" + k, "x" + orderAmountArray[k], orderRowPanel.Width / 3 + 10, 0, orderRowPanel.Width / 5 - 5, orderRowPanel.Height, Color.White, Color.Black, orderRowPanel.Height / 3, false, ContentAlignment.MiddleRight);
                Label prdPriceLabel = createLabel.CreateLabels(orderRowPanel, "productPrice_" + k, orderPrice.ToString() + constants.unit, 0, prdAmountLabel.Right + 5, orderRowPanel.Width / 3 - 10, orderRowPanel.Height, Color.White, Color.Black, orderRowPanel.Height / 3, false, ContentAlignment.MiddleRight);
            }

            PictureBox pB = new PictureBox();
            pB.Location = new Point(0, (orderRowNum + 1) * 50 + 10);
            pB.Size = new Size(dialogPanel.Width, 10);
            dialogPanel.Controls.Add(pB);
            Bitmap image = new Bitmap(pB.Size.Width, pB.Size.Height);
            Graphics g = Graphics.FromImage(image);
            g.DrawLine(new Pen(Color.FromArgb(255, 142, 133, 118), 3), 0, 5, dialogPanel.Width, 5);
            pB.Image = image;

            FlowLayoutPanel totalRowPanel = createPanel.CreateFlowLayoutPanel(dialogPanel, 0, pB.Bottom + 10, dialogPanel.Width, 50, Color.Transparent, new Padding(dialogPanel.Width / 15, 0, 0, 0));

            Label totalNameLabel = createLabel.CreateLabels(totalRowPanel, "totalName", constants.sumLabel, 0, 0, totalRowPanel.Width / 3, totalRowPanel.Height, Color.White, Color.Black, totalRowPanel.Height / 3, false, ContentAlignment.MiddleLeft);
            Label totalAmountLabel = createLabel.CreateLabels(totalRowPanel, "totalAmount", totalAmount + constants.amountUnit1, totalRowPanel.Width / 3 + 10, 0, totalRowPanel.Width / 5 - 5, totalRowPanel.Height, Color.White, Color.Black, totalRowPanel.Height / 3, false, ContentAlignment.MiddleRight);
            Label totalPriceLabel = createLabel.CreateLabels(totalRowPanel, "totalPrice", totalPrice.ToString() + constants.unit, 0, totalAmountLabel.Right + 5, totalRowPanel.Width / 3 - 10, totalRowPanel.Height, Color.White, Color.Black, totalRowPanel.Height / 3, false, ContentAlignment.MiddleRight);

            Button ticketingButton = createButton.CreateButton(constants.ticketingButtonText, "ticketingButton", dialogPanel.Width / 2 - 75, totalRowPanel.Bottom + 10, 150, 50, Color.FromArgb(255, 0, 176, 80), Color.FromArgb(255, 0, 176, 80), 0, 1, totalRowPanel.Height / 3, FontStyle.Bold, Color.White);
            dialogPanel.Controls.Add(ticketingButton);
            ticketingButton.Click += new EventHandler(this.TicketingRun);
            dialogForm.ShowDialog();

        }

        private void TicketingRun(object sender, EventArgs e)
        {
            pSaleScreen.Ticketing();
            DialogFormGlobal.Close();
        }

        private void CloseDialog(object sender, EventArgs e)
        {
            productAmounts = 1;
            string productAmount = productAmountValue.Text.Substring(0, 1);
            int productTotalPrice = int.Parse(productAmount) * productPriceValue;
            string[] sendStr = { productName, productAmount, productTotalPrice.ToString(), productPriceValue.ToString()  };
            pSaleScreen.setVal(sendStr);
            DialogFormGlobal.Close();
        }

        private void ChangeProductAmount(object sender, EventArgs e)
        {
            Label temp = (Label)sender;
            if(temp.Name == "productAmountUp" && productAmounts < restAmountGlobal)
            {
                productAmounts++;
            }
            else if(temp.Name == "productAmountDown" && productAmounts > 1)
            {
                productAmounts--;
            }
            productAmountValue.Text = productAmounts.ToString() + " 枚";

        }
        
    }
}

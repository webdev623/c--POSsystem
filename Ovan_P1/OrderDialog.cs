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
        RoundedFormDialog DialogFormGlobal = new RoundedFormDialog();
        int productAmounts = 1;
        Label productAmountValue = null;
        SaleScreen pSaleScreen = null;
        public void initValue(SaleScreen saleHandler)
        {
            pSaleScreen = saleHandler;

        }
        public void ShowMainMenuDetailFromPanel(object sender, EventArgs e)
        {
            Panel tpPanel = (Panel)sender;
            string[] strPnl = tpPanel.Name.Split('_');
            string productType = strPnl[1];
            int index = int.Parse(strPnl[2]);
            ShowMainMenuDetail(index, productType);
        }
        public void ShowMainMenuDetailFromPictureBox(object sender, EventArgs e)
        {
            PictureBox tpPictureBox = (PictureBox)sender;
            string[] strPBox = tpPictureBox.Name.Split('_');
            string productType = strPBox[1];
            int index = int.Parse(strPBox[2]);
            ShowMainMenuDetail(index, productType);
        }
        public void ShowMainMenuDetailFromLabel(object sender, EventArgs e)
        {
            Label tpPanel = (Label)sender;
            string[] strLabel = tpPanel.Name.Split('_');
            string productType = strLabel[1];
            int index = int.Parse(strLabel[2]);
            ShowMainMenuDetail(index, productType);
        }
        public void ShowMainMenuDetail(int index, string productType)
        {

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
            string productImageUrl = "";

            if(productType == "big")
            {
                productName = constants.productBigName[selectedCategoryIndex][index];
                productPriceValue = constants.productBigPrice[selectedCategoryIndex][index];
                productImageUrl = constants.productBigImageUrl[index];
            }
            else
            {
                productName = constants.productSmallName[index];
                productPriceValue = constants.productSmallPrice[index];
                productImageUrl = constants.productSmallImageUrl[index];
            }

            Panel dialogPanel = createPanel.CreateMainPanel(dialogForm, 10, 10, dialogForm.Width - 20, dialogForm.Height - 20, BorderStyle.None, Color.White);

            Label dialogTitle = createLabel.CreateLabelsInPanel(dialogPanel, "dialogTitle", dialogTitleText, 0, 10, dialogPanel.Width - 10, 50, Color.White, Color.FromArgb(255, 39, 127, 196), 18);

            PictureBox pBox = new PictureBox();
            pBox.Width = 200;
            pBox.Height = 140;
            pBox.BackColor = Color.Transparent;
            pBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pBox.Location = new Point(dialogPanel.Width / 2 - 200, dialogTitle.Bottom);
            pBox.ImageLocation = productImageUrl; // @"D:\\ovan\\Ovan_P1\\images\\badge1.png";
            dialogPanel.Controls.Add(pBox);


            Label productTitle = createLabel.CreateLabelsInPanel(dialogPanel, "productTitle", productName, dialogPanel.Width / 2, dialogTitle.Bottom + 10, 150, 60, Color.White, Color.Black, 14, false, ContentAlignment.MiddleRight);
            Label productPrice = createLabel.CreateLabelsInPanel(dialogPanel, "productPrice", productPriceValue + "円", dialogPanel.Width / 2, productTitle.Bottom, 150, 60, Color.White, Color.Black, 14, false, ContentAlignment.MiddleRight);

            Label dialogInstruction = createLabel.CreateLabelsInPanel(dialogPanel, "dialogInstruction", dialogInstructionText, dialogPanel.Left + 20, productPrice.Bottom + 5, dialogPanel.Width - 50, 80, Color.White, Color.Black, 14);

          //  Panel productOrderPanel = createPanel.CreateSubPanel(dialogPanel, dialogPanel.Left + dialogPanel.Width / 3, dialogInstruction.Bottom + 10, dialogPanel.Width / 3, 50, BorderStyle.FixedSingle, Color.Red);
            Panel productOrderLeftPanel = createPanel.CreateSubPanel(dialogPanel, dialogPanel.Left + dialogPanel.Width / 3, dialogInstruction.Bottom + 10, dialogPanel.Width / 3 * 3 / 4, 50, BorderStyle.FixedSingle, Color.Yellow);
            Panel productOrderRightPanel = createPanel.CreateSubPanel(dialogPanel, productOrderLeftPanel.Right, productOrderLeftPanel.Top, dialogPanel.Width / 3 / 4, 50, BorderStyle.FixedSingle, Color.Yellow);

            Label productAmount = createLabel.CreateLabelsInPanel(productOrderLeftPanel, "productAmount", constants.productAmount[0], 0, 0, productOrderLeftPanel.Width * 2 / 3, 50, Color.FromArgb(255, 191, 191, 191), Color.Black, 14);
            Label productUp = createLabel.CreateLabelsInPanel(productOrderLeftPanel, "productAmountUp", "▲", productAmount.Right, 0, productOrderLeftPanel.Width / 3, 25, Color.FromArgb(255, 191, 191, 191), Color.Black, 14, false, ContentAlignment.BottomCenter);
            Label productDown = createLabel.CreateLabelsInPanel(productOrderLeftPanel, "productAmountDown", "▼", productAmount.Right, productUp.Bottom, productOrderLeftPanel.Width / 3, 25, Color.FromArgb(255, 191, 191, 191), Color.Black, 14, false, ContentAlignment.TopCenter);
            Label productAmountSelect = createLabel.CreateLabelsInPanel(productOrderRightPanel, "productAmountSelect", "決\n定", 0, 0, productOrderRightPanel.Width, 50, Color.FromArgb(255, 191, 191, 191), Color.Black, 14);
            // CreateDropDown(dialogPanel, dialogPanel.Left + dialogPanel.Width / 3, dialogInstruction.Bottom + 10, dialogPanel.Width / 6, 50, "productAmount", Color.FromArgb(255, 191, 191, 191), Color.Black, Color.FromArgb(255, 191, 191, 191), constants.productAmount, 0);
            productAmountValue = productAmount;
            productUp.Click += new EventHandler(this.ChangeProductAmount);
            productDown.Click += new EventHandler(this.ChangeProductAmount);

            productAmountSelect.Click += new EventHandler(this.CloseDialog);

            dialogForm.ShowDialog();

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
            if(temp.Name == "productAmountUp" && productAmounts < 9)
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

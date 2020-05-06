
using System;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Printing;
using System.Printing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;
using System.Management;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.IO.Ports;
using Microsoft.Win32;

namespace Ovan_P1
{
    public partial class SaleScreen : Form
    {

        //  Form FormPanel = null;
        ComModule comModule = new ComModule();

        Form1 mainFormGlobal = null;
        Panel LeftPanelGlobal = null;
        Panel MainBodyPanelGlobal = null;
        Panel mainPanelGlobal = null;
        Constant constants = new Constant();
        CustomButton customButton = new CustomButton();
        MessageDialog messageDialog = new MessageDialog();
        DBClass dbClass = new DBClass();
        PasswordInput passwordInput = new PasswordInput();

        CreatePanel createPanel = new CreatePanel();
        CreateLabel createLabel = new CreateLabel();
        OrderDialog orderDialog = new OrderDialog();
        Panel TableBackPanelGlobal = null;
        Label TempLabel = null;

        public int orderTotalPrice = 0;
        public int orderEnPrice = 0;
        public int currentSelectedId = 0;

        private int selectedCategoryIndex = 0;
        private int selectedCategoryID = 0;
        private int selectedCategoryLayout = 0;

        private Label[] orderProductNameLabel = new Label[100];
        private Label[] orderAmountLabel = new Label[100];
        private Label[] orderDeleteLabel = new Label[100];
        private Button[] orderIncreaseButton = new Button[100];
        private Button[] orderDecreaseButton = new Button[100];
        private Button_WOC[] categoryButton = new Button_WOC[100];
        private Button ticketingButtonGlobal = null;
        private Button receiptButtonGlobal = null;


        private Label orderPriceTotalLabel = null;
        private Label orderPriceEnterLabel = null;
        private Label orderRestPriceLabel = null;

        private string[] orderProductNameArray = new string[100];
        private string[] orderAmountArray = new string[100];
        private string[] orderProductPriceArray = new string[100];
        private int[] orderProductIDArray = new int[100];
        private int[] orderRealProductIDArray = new int[100];
        private int[] orderProuctIndexArray = new int[100];
        private int[] orderCategoryIDArray = new int[100];
        private int[] orderCategoryIndexArray = new int[100];
        private int[,] productRestAmountArray = new int[30, 100];
        private int selectedPrdIndex = 0;
        private int[,] productLimitedCntArray = new int[30, 100];
        private int[] productIDArray = new int[100];
        private int[] realProductIDArray = new int[100];
        private int[] categoryIDArray = new int[50];
        private string[] categoryBackImageArray = new string[50];
        private int startIndex = 0;
        private int lastIndex = 4;
        private int categoryDisAmount = 0;

        private int colorPatternValue = 0;
        private string menuTitle1 = "";
        private string menuTitle2 = "";

        private Bitmap BackgroundBitmap = null;
        Color borderClr = Color.FromArgb(255, 23, 55, 94);
        Pen borderPen = null;
        Color penClr = Color.FromArgb(255, 23, 55, 94);
        int nWidth = 0, nHeight = 0;
        int nWidth1 = 0, nHeight1 = 0;
        int nWidth2 = 0, nHeight2 = 0;
        int nWidth3 = 0, nHeight3 = 0;
        Rectangle rc = new Rectangle(0, 0, 0, 0);
        Panel[] p_ProductCon = null;
        PictureBox[] pb_Image = null;
        BorderLabel[] bl_Name = null;
        BorderLabel[] bl_Price = null;
        bool bLoad = false;
        int curProduct = 0;
        int currentSerialNo = 1;
        int currentTicketNo = 1;
        int orderTotalTicketForReceipt = 0;
        int orderTotalPriceForReceipt = 0;

        int PurchaseType = 0;   //if it set 1, then multiple tickets, and if it set 0, then single ticket
        int ReturnTime = 30;    //return time after payment. 0=>auto refund, if nothing is done after payment, time until automatic return 0-255 seconds.
        int MultiPurchase = 1;  //1=> the multiple purchase button is valid, 0=> invalid
        int PurchaseAmount = 10; //the maximum number of pieces that can be purchased (2 to 10 Initial value is 10)
        int SerialNo = 1;   //1=>print the serial number, 0=>don't print the serial number
        int StartSerialNo = 1;  //specity the start serial number
        int NoAfterTight = 1;   //1=>Initial value, 0=>continue number
        int FontSize = 1;   //1=>small, 0=>big
        int ValidDate = 0;//specify the valid date range of the product
        string TicketMsg1 = "";
        string TicketMsg2 = "";

        string ReceiptValid = "true";   //true=> valid, false => invalid
        string TicketTime = "10";       //ticketing time
        string StoreName = "";
        string Address = "";
        string PhoneNumber = "";
        string Other1 = "";
    	string Other2 = "";

        int lineNumber = 0;
        int lineNumbers = 0;

        string currentDir = "";

        string storeEndTime = "00:00";
        string openTime = "00:00";

        DateTime sumDayTime1 = new DateTime(int.Parse(DateTime.Now.ToString("yyyy")), int.Parse(DateTime.Now.ToString("MM")), int.Parse(DateTime.Now.ToString("dd")), 00, 00, 00);
        DateTime sumDayTime2 = new DateTime(int.Parse(DateTime.Now.ToString("yyyy")), int.Parse(DateTime.Now.ToString("MM")), int.Parse(DateTime.Now.ToString("dd")), 23, 59, 59);
        DateTime openDayTime = new DateTime(int.Parse(DateTime.Now.ToString("yyyy")), int.Parse(DateTime.Now.ToString("MM")), int.Parse(DateTime.Now.ToString("dd")), 00, 00, 00);

        string sumDate = DateTime.Now.ToString("yyyy-MM-dd");


        int height = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height;
        int width = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width;
        SQLiteConnection sqlite_conn;

        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetDefaultPrinter(string Name);
        public SaleScreen(Form1 mainFrom, Panel panel)
        {
            mainFormGlobal = mainFrom;
            orderDialog.initValue(this);
            InitIntArray(productRestAmountArray);
            passwordInput.initSaleScreen(this);

            currentDir = Directory.GetCurrentDirectory();

            sqlite_conn = CreateConnection(constants.dbName);
            dbClass.CreateSaleTB(sqlite_conn);
            if (sqlite_conn.State == ConnectionState.Closed)
            {
                sqlite_conn.Open();
            }
            SQLiteCommand sqlite_cmd;
            SQLiteDataReader sqlite_datareader;
            DateTime now = DateTime.Now;
            string week = now.ToString("ddd");
            string currentTime = now.ToString("HH:mm");

            try
            {
                string selectTicketSql = "SELECT * FROM " + constants.tbNames[4];
                sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = selectTicketSql;
                sqlite_datareader = sqlite_cmd.ExecuteReader();
                while (sqlite_datareader.Read())
                {
                    if (!sqlite_datareader.IsDBNull(0))
                    {
                        PurchaseType = sqlite_datareader.GetInt32(0);
                        ReturnTime = sqlite_datareader.GetInt32(1);
                        MultiPurchase = sqlite_datareader.GetInt32(2);
                        PurchaseAmount = sqlite_datareader.GetInt32(3);
                        SerialNo = sqlite_datareader.GetInt32(4);
                        StartSerialNo = sqlite_datareader.GetInt32(5);
                        NoAfterTight = sqlite_datareader.GetInt32(6);
                        FontSize = sqlite_datareader.GetInt32(7);
                        ValidDate = sqlite_datareader.GetInt32(8);
                        TicketMsg1 = sqlite_datareader.GetString(9);
                        TicketMsg2 = sqlite_datareader.GetString(10);
                    }
                }

                string selectReceiptSql = "SELECT * FROM " + constants.tbNames[5];
                sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = selectReceiptSql;
                sqlite_datareader = sqlite_cmd.ExecuteReader();
                while (sqlite_datareader.Read())
                {
                    if (!sqlite_datareader.IsDBNull(0))
                    {
                        ReceiptValid = sqlite_datareader.GetString(0);
                        TicketTime = sqlite_datareader.GetString(1);
                        StoreName = sqlite_datareader.GetString(2);
                        Address = sqlite_datareader.GetString(3);
                        PhoneNumber = sqlite_datareader.GetString(4);
                        Other1 = sqlite_datareader.GetString(5);
                        Other2 = sqlite_datareader.GetString(6);
                    }
                }

                string queryCmd = "SELECT * FROM " + constants.tbNames[0] + " ORDER BY id";
                sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = queryCmd;
                sqlite_datareader = sqlite_cmd.ExecuteReader();
                while (sqlite_datareader.Read())
                {
                    string openTime = "";
                    if (week == "Sat")
                    {
                        openTime = sqlite_datareader.GetString(4);
                    }
                    else if (week == "Sun")
                    {
                        openTime = sqlite_datareader.GetString(5);
                    }
                    else
                    {
                        openTime = sqlite_datareader.GetString(3);
                    }
                    string[] openTimeArr = openTime.Split('/');
                    foreach (string openTimeArrItem in openTimeArr)
                    {
                        string[] openTimeSubArr = openTimeArrItem.Split('-');
                        if (String.Compare(openTimeSubArr[0], currentTime) <= 0 && String.Compare(openTimeSubArr[1], currentTime) >= 0)
                        {
                            categoryDisAmount++;
                            break;
                        }
                    }
                }

                string selectGeneralSql = "SELECT * FROM " + constants.tbNames[12];
                sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = selectGeneralSql;
                sqlite_datareader = sqlite_cmd.ExecuteReader();
                while (sqlite_datareader.Read())
                {
                    if (!sqlite_datareader.IsDBNull(0))
                    {
                        colorPatternValue = sqlite_datareader.GetInt32(0);
                        menuTitle1 = sqlite_datareader.GetString(1);
                        menuTitle2 = sqlite_datareader.GetString(2);
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg1 = "Data loading is failed.";
                string errorMsg2 = "Please go to Menu Reading section and get loading data again.";
                messageDialog.ShowErrorMessage(errorMsg1, errorMsg2 + "\n ErrorNo: 004");
                this.BackShow();
                return;
            }


            Panel LeftPanel = createPanel.CreateMainPanel(mainFrom, 0, 0, 3 * width / 4, height, BorderStyle.FixedSingle, Color.FromArgb(255, 255, 255, 204));
            LeftPanelGlobal = LeftPanel;

            /**  Top Screen Title */
            FlowLayoutPanel FlowTitleLayout = createPanel.CreateFlowLayoutPanel(LeftPanel, LeftPanel.Width / 6, 0, (LeftPanel.Width * 5) / 6, height / 7, Color.Transparent, new Padding(10, 70, 0, 0));

            Label MainTitle = new Label();
            //  MainTitle.Location = new Point(FlowTitleLayout.Left+200, FlowTitleLayout.Height/2-24);
            MainTitle.Width = FlowTitleLayout.Width;
            MainTitle.Height = FlowTitleLayout.Height - 70;
            MainTitle.Font = new Font("Series", 24, FontStyle.Bold);
            MainTitle.ForeColor = Color.FromArgb(255, 255, 0, 0);
            MainTitle.Text = menuTitle1 + "\n" + menuTitle2;
            //LeftPanel.Controls.Remove(FlowTitleLayout);
            FlowTitleLayout.Controls.Add(MainTitle);

            /** Main Product Panel layout */
            Panel MenuBodyLayout = createPanel.CreateSubPanel(LeftPanel, LeftPanel.Width / 6, height / 7, (LeftPanel.Width * 5) / 6, height * 6 / 7, BorderStyle.None, Color.Transparent);
            MainBodyPanelGlobal = MenuBodyLayout;


            FlowLayoutPanel FlowButtonLayout = createPanel.CreateFlowLayoutPanel(LeftPanel, 0, height / 7, LeftPanel.Width / 6, height * 4 / 7, Color.Transparent, new Padding(20, 10, 0, 0));
            try
            {
                CreateCategoryList(FlowButtonLayout);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                MessageBox.Show(ex.ToString());
                messageDialog.ShowErrorMessage(constants.systemErrorMsg, constants.systemSubErrorMsg + "error2");
                this.BackShow();
                return;

            }


            Image backImage = Image.FromFile(constants.backButton);

            Button backButton = customButton.CreateButtonWithImage(backImage, "backButton", "", LeftPanel.Width / 12 - 50, LeftPanel.Height - 150, 100, 100, 3, 100);
            backButton.BackgroundImageLayout = ImageLayout.Stretch;
            backButton.Padding = new Padding(0);
            LeftPanel.Controls.Add(backButton);
            backButton.Click += new EventHandler(BackShowMainMenu);

            Panel RightPanel = createPanel.CreateMainPanel(mainFrom, width * 3 / 4, 0, width / 4, height, BorderStyle.FixedSingle, Color.White);

            /** right panel  */
            RightPanel.Padding = new Padding(10, 0, 10, 0);

            // Top Button Create
            Image upButtonImage = constants.upButtonImage;
            string upButtonName = constants.upButtonName;
            int upButtonLeft = 10;
            int upButtonTop = 50;
            int upButtonWidth = RightPanel.Width - 30;
            int upButtonHeight = (RightPanel.Height * 2 / 5 - 10) / 7;

            Button topButton = customButton.CreateButton("▲   上  へ", upButtonName, upButtonLeft, upButtonTop, upButtonWidth, upButtonHeight, Color.Gray, Color.White, 0, 1, 22, FontStyle.Bold, Color.White, ContentAlignment.MiddleCenter);

            RightPanel.Controls.Add(topButton);

            // Create Tabel 
            Panel TableBackPanel = createPanel.CreateSubPanel(RightPanel, 10, topButton.Bottom + 10, RightPanel.Width - 30, (RightPanel.Height * 2 / 5 - 10) * 5 / 7, BorderStyle.None, Color.FromArgb(255, 191, 191, 191));
            TableBackPanelGlobal = TableBackPanel;
            FlowLayoutPanel deleteColumnPanel = createPanel.CreateFlowLayoutPanel(TableBackPanel, 5, 5, TableBackPanel.Width / 5, TableBackPanel.Height - 10, Color.FromArgb(255, 191, 191, 191), new Padding(0, 0, 0, 0));
            FlowLayoutPanel bookProductColumnPanel = createPanel.CreateFlowLayoutPanel(TableBackPanel, deleteColumnPanel.Width + 5, 5, TableBackPanel.Width * 2 / 5 - 10, TableBackPanel.Height - 10, Color.FromArgb(255, 191, 191, 191), new Padding(0, 0, 0, 0));
            FlowLayoutPanel productNumberColumnPanel = createPanel.CreateFlowLayoutPanel(TableBackPanel, bookProductColumnPanel.Width + deleteColumnPanel.Width + 5, 5, TableBackPanel.Width * 2 / 15, TableBackPanel.Height - 10, Color.FromArgb(255, 191, 191, 191), new Padding(0, 0, 0, 0));
            FlowLayoutPanel productIncreaseColumnPanel = createPanel.CreateFlowLayoutPanel(TableBackPanel, productNumberColumnPanel.Width + bookProductColumnPanel.Width + deleteColumnPanel.Width + 5, 5, TableBackPanel.Width * 2 / 15, TableBackPanel.Height - 10, Color.FromArgb(255, 191, 191, 191), new Padding(0, 0, 0, 0));
            FlowLayoutPanel productDecreaseColumnPanel = createPanel.CreateFlowLayoutPanel(TableBackPanel, productNumberColumnPanel.Width * 2 + bookProductColumnPanel.Width + deleteColumnPanel.Width + 5, 5, TableBackPanel.Width * 2 / 15, TableBackPanel.Height - 10, Color.FromArgb(255, 191, 191, 191), new Padding(0, 0, 0, 0));
            for (int i = 0; i < 5; i++)
            {
                Label deleteLabel = createLabel.CreateLabels(deleteColumnPanel, "del_" + i, constants.deleteText, 0, (deleteColumnPanel.Height / 5 + 10) * i, deleteColumnPanel.Width - 10, deleteColumnPanel.Height / 5 - 10, Color.FromArgb(255, 255, 0, 0), Color.White, 12, false, ContentAlignment.MiddleCenter, new Padding(0, 5, 0, 5));
                deleteLabel.Click += new EventHandler(this.orderDelete);

                Label bookProductLabel = createLabel.CreateLabels(bookProductColumnPanel, "prd_" + i, "", 0, (bookProductColumnPanel.Height / 5 + 10) * i, bookProductColumnPanel.Width - 10, bookProductColumnPanel.Height / 5 - 10, Color.White, Color.Black, 12, false, ContentAlignment.MiddleCenter, new Padding(0, 5, 0, 5));
                Label productNumberLabel = createLabel.CreateLabels(productNumberColumnPanel, "prdNum_" + i, "", 0, (productNumberColumnPanel.Height / 5 + 10) * i, productNumberColumnPanel.Width - 10, productNumberColumnPanel.Height / 5 - 10, Color.White, Color.Black, 12, false, ContentAlignment.MiddleCenter, new Padding(0, 5, 0, 5));

                orderDeleteLabel[i] = deleteLabel;
                orderAmountLabel[i] = productNumberLabel;
                orderProductNameLabel[i] = bookProductLabel;

                Image increaseButtonImage = constants.increaseButtonImage;
                Button productIncreaseButton = customButton.CreateButtonWithImage(increaseButtonImage, "prdIncrease_" + i, "", 0, (productIncreaseColumnPanel.Height / 5 + 10) * i, productIncreaseColumnPanel.Width - 6, productIncreaseColumnPanel.Height / 5 - 6, 0, 1);
                productIncreaseColumnPanel.Controls.Add(productIncreaseButton);
                orderIncreaseButton[i] = productIncreaseButton;
                productIncreaseButton.Click += new EventHandler(this.orderAmountChange);

                Image decreaseButtonImage = constants.decreaseButtonImage;
                Button productDecreaseButton = customButton.CreateButtonWithImage(decreaseButtonImage, "prdDecrease_" + i, "", 0, (productDecreaseColumnPanel.Height / 5 + 10) * i, productDecreaseColumnPanel.Width - 6, productDecreaseColumnPanel.Height / 5 - 6, 0, 1);
                productDecreaseColumnPanel.Controls.Add(productDecreaseButton);
                orderDecreaseButton[i] = productDecreaseButton;
                productDecreaseButton.Click += new EventHandler(this.orderAmountChange);

            }


            // Bottom Button Create
            Image downButtonImage = constants.downButtonImage;
            string downButtonName = constants.downButtonName;
            int downButtonLeft = 10;
            int downButtonTop = TableBackPanel.Bottom + 10;
            int downButtonWidth = RightPanel.Width - 30;
            int downButtonHeight = (RightPanel.Height * 2 / 5 - 10) / 7;

            Button downButton = customButton.CreateButton("▼   下  へ", downButtonName, downButtonLeft, downButtonTop, downButtonWidth, downButtonHeight, Color.Gray, Color.White, 0, 1, 22, FontStyle.Bold, Color.White, ContentAlignment.MiddleCenter);

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
                Label transactionUnit = createLabel.CreateLabels(transactionUnitPanel, "transUnit_" + m, constants.unit, 0, ((transactionUnitPanel.Height / 3) + 10) * m, transactionUnitPanel.Width - 10, transactionUnitPanel.Height / 3 - 10, Color.White, Color.FromArgb(255, 81, 163, 211), 14);
                if(m == 1)
                {
                    orderPriceTotalLabel = transactionResult;
                    orderPriceTotalLabel.TextChanged += new System.EventHandler(DepositAmountChange);

                }
                else if(m == 0)
                {
                    orderPriceEnterLabel = transactionResult;
                }
                else
                {
                    orderRestPriceLabel = transactionResult;
                }
            }

            // payment button show
            Panel PaymentButtonPanel = createPanel.CreateSubPanel(RightPanel, 10, TransactionPanel.Bottom + 30, RightPanel.Width - TransactionPanel.Width / 6, RightPanel.Height - TransactionPanel.Bottom -30, BorderStyle.None, Color.White);
            FlowLayoutPanel paymentButtonTopPanel = createPanel.CreateFlowLayoutPanel(PaymentButtonPanel, 5, 5, PaymentButtonPanel.Width - 10, 70, Color.White, new Padding(0, 0, 0, 0));
            FlowLayoutPanel paymentButtonBottomPanel = createPanel.CreateFlowLayoutPanel(PaymentButtonPanel, 5, paymentButtonTopPanel.Bottom + 5, PaymentButtonPanel.Width - 10, 70, Color.White, new Padding(0, 0, 0, 0));
            Button ticketingButton = customButton.CreateButton(constants.ticketingButtonText, "ticketingButton", 0, 5, paymentButtonTopPanel.Width / 2 - 10, paymentButtonTopPanel.Height - 10, Color.FromArgb(255, 217, 217, 217), Color.FromArgb(255, 85, 142, 213), 5, 14, 20, FontStyle.Bold, Color.Black);
            ticketingButton.Enabled = true;
            paymentButtonTopPanel.Controls.Add(ticketingButton);
            ticketingButtonGlobal = ticketingButton;
            ticketingButton.Click += new EventHandler(this.ShowTicketing);

            Button cancelButton = customButton.CreateButton(constants.cancelButtonText, "cancelButton", paymentButtonTopPanel.Width / 2 + 10, 5, paymentButtonTopPanel.Width / 2 - 10, paymentButtonTopPanel.Height - 10, Color.FromArgb(255, 255, 0, 0), Color.FromArgb(255, 185, 205, 229), 5, 14, 20, FontStyle.Bold, Color.White);
            paymentButtonTopPanel.Controls.Add(cancelButton);
            cancelButton.Click += new EventHandler(this.CancelOrder);

            Button receiptButton = customButton.CreateButton(constants.receiptButtonText, "receiptButton", 0, 5, paymentButtonBottomPanel.Width - 10, paymentButtonBottomPanel.Height - 10, Color.FromArgb(255, 217, 217, 217), Color.Transparent, 1);
            paymentButtonBottomPanel.Controls.Add(receiptButton);
            receiptButton.Enabled = false;
            receiptButtonGlobal = receiptButton;
            receiptButton.Click += new EventHandler(this.ReceiptRun);

            if (categoryDisAmount == 0)
            {
                string errorMsg1 = "Category Data loading is failed. There is no salable category in this time";
                string errorMsg2 = "Please go to Menu Reading section and get loading data again.";
                messageDialog.ShowErrorMessage(errorMsg1, errorMsg2 + "\n ErrorNo: 005");
                this.BackShow();
            }

            comModule.Initialize(this);
        }

        private void DepositAmountChange(object sender, EventArgs e)
        {
            if (orderPriceTotalLabel.Text != "")
                comModule.OrderChange(orderPriceTotalLabel.Text);
        }

        public void SetDepositAmount(int amount)
        {
            if (orderPriceEnterLabel.InvokeRequired)
            {
                orderPriceEnterLabel.Invoke(new MethodInvoker(delegate
                {
                    orderPriceEnterLabel.Text = amount.ToString();
                }));
            }
            if(amount >= orderTotalPrice)
            {
                if (ticketingButtonGlobal.InvokeRequired)
                {
                    ticketingButtonGlobal.Invoke(new MethodInvoker(delegate
                    {
                        ticketingButtonGlobal.BackColor = Color.FromArgb(255, 0, 176, 81);
                        ticketingButtonGlobal.ForeColor = Color.White;
                        ticketingButtonGlobal.Enabled = true;
                    }));
                }
            }
        }
        private void CreateCategoryList(FlowLayoutPanel listPanel)
        {
            DateTime now = DateTime.Now;
            string week = now.ToString("ddd");
            string currentTime = now.ToString("HH:mm");
            Color[][] colorPattern = constants.pattern_Clr;
            colorPattn = colorPattern[colorPatternValue];
            if (sqlite_conn.State == ConnectionState.Closed)
            {
                sqlite_conn.Open();
            }
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();
            string queryCmd = "SELECT * FROM " + constants.tbNames[0] + " ORDER BY id";
            sqlite_cmd.CommandText = queryCmd;

            sqlite_datareader = sqlite_cmd.ExecuteReader();
            int k = 0;
            while (sqlite_datareader.Read())
            {
                bool saleFlag = false;
                string openTime = "";
                if (week == "Sat")
                {
                    openTime = sqlite_datareader.GetString(4);
                }
                else if (week == "Sun")
                {
                    openTime = sqlite_datareader.GetString(5);
                }
                else
                {
                    openTime = sqlite_datareader.GetString(3);
                }
                string[] openTimeArr = openTime.Split('/');
                foreach (string openTimeArrItem in openTimeArr)
                {
                    string[] openTimeSubArr = openTimeArrItem.Split('-');
                    if (String.Compare(openTimeSubArr[0], currentTime) <= 0 && String.Compare(openTimeSubArr[1], currentTime) >= 0)
                    {
                        saleFlag = true;
                        break;
                    }
                }
                if (saleFlag)
                {
                    if (k == 0)
                    {
                        selectedCategoryID = sqlite_datareader.GetInt32(1);
                        selectedCategoryLayout = sqlite_datareader.GetInt32(7);

                        if (selectedCategoryLayout == 13)
                        {
                            CreateProductsList13();
                        }
                        else if (selectedCategoryLayout == 11)
                        {
                            CreateProductsList11();
                        }
                        else
                        {
                            CreateProductsList();
                        }
                    }
                    categoryIDArray[k] = sqlite_datareader.GetInt32(1);
                    categoryBackImageArray[k] = sqlite_datareader.GetString(9);

                    string categoryButtonText = sqlite_datareader.GetString(2);
                    string categoryButtonName = "saleCategoryBtn_" + k + "_" + sqlite_datareader.GetInt32(1).ToString() + "_" + sqlite_datareader.GetInt32(7).ToString();
                    Color backColor = colorPattn[k % 4 + 4];
                    Color borderColor = colorPattn[k % 4];

                    if (selectedCategoryIndex == k)
                    {
                        backColor = colorPattn[k % 4];
                        borderColor = Color.Red;
                    }
                    int btnLeft = listPanel.Left + 10;
                    int btnTop = (listPanel.Top + 10) + (listPanel.Height / 5) * k;
                    int btnWidth = listPanel.Width - 25;
                    int btnHeight = listPanel.Height / categoryDisAmount - 10;
                    if(categoryDisAmount < 6)
                    {
                        btnHeight = listPanel.Height / 6 - 10;
                    }

                    Button_WOC btn = new Button_WOC();
                    btn.Location = new Point(btnLeft, btnTop);
                    btn.Size = new Size(btnWidth, btnHeight);
                    btn.Text = categoryButtonText;
                    if (sqlite_datareader.GetInt32(10) == 1)
                    {
                        btn.Text = constants.saleStopText;
                        btn.ForeColor = Color.Red;
                        btn.TextColor = Color.Red;
                        btn.Enabled = false;
                    }

                    btn.Name = categoryButtonName;
                    btn.BackColor = Color.Transparent;
                    btn.ButtonColor = backColor;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.OnHoverButtonColor = colorPattn[k % 4 + 4];
                    btn.OnHoverBorderColor = borderColor;
                    btn.BorderColor = borderColor;
                    btn.Font = new Font("Seri", 18F, FontStyle.Bold);
                    categoryButton[k] = btn;
                    listPanel.Controls.Add(btn);
                    btn.Invalidate();

                    btn.Click += new EventHandler(this.SelectCategory);

                    //btn.MouseEnter += new EventHandler(this.HoverChange);
                    k++;
                    //categoryDisAmount = k;
                }
            }
            if (categoryBackImageArray[0] != "" && categoryBackImageArray[0] != null)
            {
                LeftPanelGlobal.BackgroundImage = Image.FromFile(categoryBackImageArray[0]);
                LeftPanelGlobal.BackgroundImageLayout = ImageLayout.Stretch;
            }
            else
            {
                LeftPanelGlobal.BackColor = colorPattn[4];
            }


        }

        Color[] colorPattn = new Color[8];
        private void SelectCategory(object sender, EventArgs e)
        {
            Button_WOC btnTemp = (Button_WOC)sender;
            int selectedID = int.Parse(btnTemp.Name.Split('_')[1]);
            selectedCategoryID = int.Parse(btnTemp.Name.Split('_')[2]);
            selectedCategoryLayout = int.Parse(btnTemp.Name.Split('_')[3]);
            Color[][] colorPattern = constants.pattern_Clr;
            colorPattn = colorPattern[colorPatternValue];

            curProduct = 0;


            selectedCategoryIndex = selectedID;
            for(int i = 0; i < categoryDisAmount; i++)
            {
                if(i != selectedID)
                {
                    categoryButton[i].ButtonColor = colorPattn[i % 4 + 4];
                    categoryButton[i].BorderColor = colorPattn[i % 4];
                    categoryButton[i].BackColor = Color.Transparent;
                }
            }
            btnTemp.ButtonColor = colorPattn[selectedID % 4];
            btnTemp.BorderColor = Color.Red;
            btnTemp.BackColor = Color.Transparent;
            if(categoryBackImageArray[selectedID] != "")
            {
                LeftPanelGlobal.BackgroundImage = Image.FromFile(categoryBackImageArray[selectedID]);
                LeftPanelGlobal.BackgroundImageLayout = ImageLayout.Stretch;
            }
            else
            {
                LeftPanelGlobal.BackgroundImage = null;
                LeftPanelGlobal.BackColor = colorPattn[selectedID % 4 + 4];
            }

            btnTemp.Invalidate();

            if(selectedCategoryLayout == 13)
            {
                CreateProductsList13();
            }
            else if(selectedCategoryLayout == 11)
            {
                CreateProductsList11();
            }
            else
            {
                CreateProductsList();
            }
        }

        private void HoverChange(object sender, MouseEventArgs e)
        {
            Button_WOC btnTemp = (Button_WOC)sender;
            Color[][] colorPattern = constants.pattern_Clr;
            btnTemp.BackColor = colorPattern[0][selectedCategoryIndex % 4 + 4];
            btnTemp.BorderColor = Color.Red;
            btnTemp.Invalidate();
        }
        private void CreateProductsList11()
        {
            MainBodyPanelGlobal.Controls.Clear();
            int w1 = (MainBodyPanelGlobal.Width - 100) / 5;
            int h1 = (MainBodyPanelGlobal.Height - 80) / 5;
            nWidth = 3 * w1 + 40;
            nHeight = 3 * h1 + 40;
            nHeight1 = h1;
            nWidth1 = w1;
            nHeight2 = 2 * h1 + 20;
            nWidth2 = 2 * w1 + 20;
            nHeight3 = h1;
            nWidth3 = 2 * w1 + 20;
            DateTime now = DateTime.Now;
            string week = now.ToString("ddd");
            string currentTime = now.ToString("HH:mm");

            p_ProductCon = new Panel[selectedCategoryLayout];
            pb_Image = new PictureBox[selectedCategoryLayout];
            bl_Name = new BorderLabel[selectedCategoryLayout];
            bl_Price = new BorderLabel[selectedCategoryLayout];


            for (int i = 0; i < selectedCategoryLayout; i++)
            {
                string bgColor = "ffffff00";
                string foreColor = "ff000000";
                string borderColor = "ff223300";
                string prdName = "";
                int prdLimitedCnt = 0;
                int prdPrice = 0;
                string prdImageUrl = "";
                int prdImageX = 0;
                int prdImageY = 0;
                int prdImageWidth = 0;
                int prdImageHeight = 0;
                int prdBadgeX = 0;
                int prdBadgeY = 0;
                int prdBadgeWidth = 0;
                int prdBadgeHeight = 0;
                string prdBadgeUrl = "";
                int prdNameX = 0;
                int prdNameY = 0;
                int prdPriceX = 0;
                int prdPriceY = 0;
                int soldFlag = 0;

                if (sqlite_conn.State == ConnectionState.Closed)
                {
                    sqlite_conn.Open();
                }
                SQLiteDataReader sqlite_datareader;
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = sqlite_conn.CreateCommand();
                string queryCmd = "SELECT * FROM " + constants.tbNames[2] + " WHERE CategoryID=@CategoryID and CardNumber=@CardNumber ORDER BY CardNumber";
                sqlite_cmd.CommandText = queryCmd;
                sqlite_cmd.Parameters.AddWithValue("@CategoryID", selectedCategoryID);
                sqlite_cmd.Parameters.AddWithValue("@CardNumber", i + 1);

                sqlite_datareader = sqlite_cmd.ExecuteReader();

                while (sqlite_datareader.Read())
                {
                    string openTime = "";
                    if (week == "Sat")
                    {
                        openTime = sqlite_datareader.GetString(6);
                    }
                    else if (week == "Sun")
                    {
                        openTime = sqlite_datareader.GetString(7);
                    }
                    else
                    {
                        openTime = sqlite_datareader.GetString(5);
                    }
                    string[] openTimeArr = openTime.Split('/');
                    foreach (string openTimeArrItem in openTimeArr)
                    {
                        string[] openTimeSubArr = openTimeArrItem.Split('-');
                        if (String.Compare(openTimeSubArr[0], currentTime) <= 0 && String.Compare(openTimeSubArr[1], currentTime) >= 0)
                        {
                            productIDArray[i] = sqlite_datareader.GetInt32(0);
                            realProductIDArray[i] = sqlite_datareader.GetInt32(2);
                            prdName = sqlite_datareader.GetString(3);
                            prdPrice = sqlite_datareader.GetInt32(8);
                            prdLimitedCnt = sqlite_datareader.GetInt32(9);
                            productLimitedCntArray[selectedCategoryIndex, i] = prdLimitedCnt;
                            prdImageUrl = Path.Combine(currentDir, sqlite_datareader.GetString(11));
                            prdImageX = sqlite_datareader.GetInt32(15);
                            prdImageY = sqlite_datareader.GetInt32(16);
                            prdImageWidth = sqlite_datareader.GetInt32(17);
                            prdImageHeight = sqlite_datareader.GetInt32(18);
                            prdBadgeX = sqlite_datareader.GetInt32(19);
                            prdBadgeY = sqlite_datareader.GetInt32(20);
                            prdBadgeWidth = sqlite_datareader.GetInt32(21);
                            prdBadgeHeight = sqlite_datareader.GetInt32(22);
                            prdBadgeUrl = Path.Combine(currentDir, sqlite_datareader.GetString(23));
                            prdNameX = sqlite_datareader.GetInt32(24);
                            prdNameY = sqlite_datareader.GetInt32(25);
                            prdPriceX = sqlite_datareader.GetInt32(26);
                            prdPriceY = sqlite_datareader.GetInt32(27);
                            bgColor = sqlite_datareader.GetString(28);
                            foreColor = sqlite_datareader.GetString(29);
                            borderColor = sqlite_datareader.GetString(30);
                            soldFlag = sqlite_datareader.GetInt32(31);
                            if (productRestAmountArray[selectedCategoryIndex, i] == -1)
                            {
                                SQLiteDataReader sqlite_datareader1;
                                SQLiteCommand sqlite_cmd1;
                                sqlite_cmd1 = sqlite_conn.CreateCommand();
                                string queryCmd1 = "SELECT SUM(prdAmount) as prdRestAmount FROM " + constants.tbNames[3] + " WHERE categoryID=@CategoryID and prdID=@prdID and sumFlag='false'";
                                sqlite_cmd1.CommandText = queryCmd1;
                                sqlite_cmd1.Parameters.AddWithValue("@CategoryID", selectedCategoryID);
                                sqlite_cmd1.Parameters.AddWithValue("@prdID", productIDArray[i]);

                                sqlite_datareader1 = sqlite_cmd1.ExecuteReader();
                                while (sqlite_datareader1.Read())
                                {
                                    if(productLimitedCntArray[selectedCategoryIndex, i] != 0)
                                    {
                                        if (!sqlite_datareader1.IsDBNull(0))
                                        {
                                            productRestAmountArray[selectedCategoryIndex, i] = productLimitedCntArray[selectedCategoryIndex, i] - sqlite_datareader1.GetInt32(0);
                                        }
                                        else
                                        {
                                            productRestAmountArray[selectedCategoryIndex, i] = productLimitedCntArray[selectedCategoryIndex, i];
                                        }
                                    }
                                    else
                                    {
                                        productRestAmountArray[selectedCategoryIndex, i] = 10000;
                                    }
                                }

                            }
                            break;
                        }
                    }

                }

                Panel p = new Panel();
                borderPen = new Pen(borderClr, 3);
                BackgroundBitmap = null;
                if (i == 0)
                {
                    p = createPanel.CreatePanelForProducts(0, 0, nWidth, nHeight, i.ToString(), true, borderClr, Color.White);
                    BackgroundBitmap = new Bitmap(nWidth, nHeight);
                    p.Paint += new PaintEventHandler(panelbordercolor2_Paint);
                }
                else if (i == 1)
                {
                    p = createPanel.CreatePanelForProducts(nWidth + 20, 0, nWidth2, nHeight2, i.ToString(), true, borderClr, Color.White);
                    BackgroundBitmap = new Bitmap(nWidth2, nHeight2);
                    p.Paint += new PaintEventHandler(panelbordercolor3_Paint);
                }
                else if (i == 2 || i == 6 || i == 10)
                {
                    int hh = 2 * h1 + 40;
                    if (i == 6) hh = 3 * h1 + 60;
                    if (i == 10) hh = 4 * h1 + 80;
                    p = createPanel.CreatePanelForProducts(nWidth + 20, hh, nWidth3, nHeight3, i.ToString(), true, borderClr, Color.White);
                    BackgroundBitmap = new Bitmap(nWidth3, nHeight3);
                    p.Paint += new PaintEventHandler(panelbordercolor4_Paint);
                }
                else if (i < 6)
                {
                    p = createPanel.CreatePanelForProducts((i - 3) * w1 + (i - 3) * 20, nHeight + 20, w1, h1, i.ToString(), true, borderClr, Color.White);
                    BackgroundBitmap = new Bitmap(w1, h1);
                    p.Paint += new PaintEventHandler(panelbordercolor_Paint);
                }
                else
                {
                    p = createPanel.CreatePanelForProducts((i - 7) * w1 + (i - 7) * 20, nHeight + h1 + 40, w1, h1, i.ToString(), true, borderClr, Color.White);
                    BackgroundBitmap = new Bitmap(w1, h1);
                    p.Paint += new PaintEventHandler(panelbordercolor_Paint);
                }
                int h = p.Width - 100 > p.Height - 60 ? (p.Height - 60) / 5 : (p.Width - 100) / 5 - 10;
                h = (h <= 0) ? 11 : h;
                int y = (p.Height - 60) / 5;
                if (p.Width - 100 < p.Height - 60) y = (p.Height - 60 - h - 3) / 4;
                int ftSize = 2 * h / 3;

                Color clr = bgColor != null ? HexToColor(bgColor) : Color.White;
                SetBackgroundColor(clr);

                pb_Image[i] = createPanel.CreatePictureBox(3, 3, p.Width - 6, p.Height - 6, i.ToString(), null);

                rc.X = p.Width / 6;
                rc.Y = p.Height / 6;
                rc.Width = 2 * p.Width / 3;
                rc.Height = 2 * p.Height / 3;

                if (prdImageWidth != 0 && prdImageHeight != 0)
                    rc = new Rectangle(prdImageX, prdImageY, prdImageWidth, prdImageHeight);

                Bitmap bm = null;
                if(prdImageUrl != "" && prdImageUrl != null)
                    bm = ShowCombinedImage(BackgroundBitmap, new Bitmap(prdImageUrl) /*imgUrl[i]*/, rc);

                rc.X = p.Width / 2;
                rc.Y = p.Height / 6;
                rc.Width = p.Width / 3;
                rc.Height = p.Height / 3;

                if (prdBadgeWidth != 0 && prdBadgeHeight != 0)
                   rc = new Rectangle(prdBadgeX, prdBadgeY, prdBadgeWidth, prdBadgeHeight);

                if (prdBadgeUrl != "" && prdBadgeUrl != null)
                {
                    string markPath = prdBadgeUrl;
                    bm = ShowCombinedImage(bm, (Bitmap)Image.FromFile(@markPath), rc);
                }

                if (soldFlag == 1 || (prdLimitedCnt != 0 && productRestAmountArray[selectedCategoryIndex, i] == 0))
                {
                    rc.X = p.Width / 6;
                    rc.Y = p.Height / 6;
                    rc.Width = 2 * p.Width / 3;
                    rc.Height = 2 * p.Height / 3;

                    if (prdImageWidth != 0 && prdImageHeight != 0)
                        rc = new Rectangle(prdImageX, prdImageY, prdImageWidth, prdImageHeight);
                    bm = ShowCombinedImage(bm, (Bitmap)Image.FromFile(constants.soldoutBadge), rc);
                    pb_Image[i].Enabled = false;

                }

                pb_Image[i].Image = bm;

                Font font = new Font("Series", ftSize, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                Point pt = new Point(p.Width / 2 - /*category[0, i].Length/2*/4 * ftSize - 7, 3 * (pb_Image[i].Height) / 5);
                if (prdNameX != 0 && prdNameY != 0)
                    pt = new Point(prdNameX, prdNameY);

                bl_Name[i] = createLabel.CreateBorderLabel(pt.X, pt.Y, ftSize * 8/*category[0,i].Length*/ , pb_Image[i].Height / 5, i.ToString(), /*category[0, i], */ prdName, HexToColor(borderColor), 1, font, HexToColor(foreColor));

                pt = new Point(p.Width / 2 - /*category[1, i].Length/2*/1 * ftSize - 7, 4 * (pb_Image[i].Height) / 5);
                if (prdPriceX != 0 && prdPriceY != 0)
                    pt = new Point(prdPriceX, prdPriceY);

                string prdPriceStr = (prdPrice == 0) ? "" : prdPrice.ToString();

                bl_Price[i] = createLabel.CreateBorderLabel(pt.X, pt.Y, ftSize * 3/*category[1,i].Length*/ + 20, pb_Image[i].Height / 5, i.ToString(), /*category[1, i], */ prdPriceStr, HexToColor(borderColor), 1, font, HexToColor(foreColor));

                if (prdName == "" || prdPriceStr == "")
                {
                    pb_Image[i].Enabled = false;
                    bl_Name[i].Enabled = false;
                    bl_Price[i].Enabled = false;
                }

                pb_Image[i].Click += new EventHandler(onItemPicClk);
                bl_Name[i].Click += new EventHandler(onItemLabelClk);
                bl_Price[i].Click += new EventHandler(onItemLabelClk);

                pb_Image[i].Controls.Add(bl_Name[i]);
                pb_Image[i].Controls.Add(bl_Price[i]);

                p.Controls.Add(pb_Image[i]);

                MainBodyPanelGlobal.Controls.Add(p);
                p_ProductCon[i] = p;
            }
        }

        private void CreateProductsList13()
        {
            MainBodyPanelGlobal.Controls.Clear();
            int w1 = (MainBodyPanelGlobal.Width - 100) / 5;
            int h1 = (MainBodyPanelGlobal.Height - 80) / 5;
            nWidth = 2 * w1 + 20;
            nHeight = 2 * h1 + 20;
            nHeight1 = h1;
            nWidth1 = w1;
            DateTime now = DateTime.Now;
            string week = now.ToString("ddd");
            string currentTime = now.ToString("HH:mm");

            p_ProductCon = new Panel[selectedCategoryLayout];
            pb_Image = new PictureBox[selectedCategoryLayout];
            bl_Name = new BorderLabel[selectedCategoryLayout];
            bl_Price = new BorderLabel[selectedCategoryLayout];


            for (int i = 0; i < selectedCategoryLayout; i++)
            {
                string bgColor = "ffffff00";
                string foreColor = "ff000000";
                string borderColor = "ff223300";
                string prdName = "";
                int prdLimitedCnt = 0;
                int prdPrice = 0;
                string prdImageUrl = "";
                int prdImageX = 0;
                int prdImageY = 0;
                int prdImageWidth = 0;
                int prdImageHeight = 0;
                int prdBadgeX = 0;
                int prdBadgeY = 0;
                int prdBadgeWidth = 0;
                int prdBadgeHeight = 0;
                string prdBadgeUrl = "";
                int prdNameX = 0;
                int prdNameY = 0;
                int prdPriceX = 0;
                int prdPriceY = 0;
                int soldFlag = 0;

                if (sqlite_conn.State == ConnectionState.Closed)
                {
                    sqlite_conn.Open();
                }
                SQLiteDataReader sqlite_datareader;
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = sqlite_conn.CreateCommand();
                string queryCmd = "SELECT * FROM " + constants.tbNames[2] + " WHERE CategoryID=@CategoryID and CardNumber=@CardNumber ORDER BY CardNumber";
                sqlite_cmd.CommandText = queryCmd;
                sqlite_cmd.Parameters.AddWithValue("@CategoryID", selectedCategoryID);
                sqlite_cmd.Parameters.AddWithValue("@CardNumber", i + 1);

                sqlite_datareader = sqlite_cmd.ExecuteReader();

                while (sqlite_datareader.Read())
                {
                    string openTime = "";
                    if (week == "Sat")
                    {
                        openTime = sqlite_datareader.GetString(6);
                    }
                    else if (week == "Sun")
                    {
                        openTime = sqlite_datareader.GetString(7);
                    }
                    else
                    {
                        openTime = sqlite_datareader.GetString(5);
                    }
                    string[] openTimeArr = openTime.Split('/');
                    foreach (string openTimeArrItem in openTimeArr)
                    {
                        string[] openTimeSubArr = openTimeArrItem.Split('-');
                        if (String.Compare(openTimeSubArr[0], currentTime) <= 0 && String.Compare(openTimeSubArr[1], currentTime) >= 0)
                        {
                            productIDArray[i] = sqlite_datareader.GetInt32(0);
                            realProductIDArray[i] = sqlite_datareader.GetInt32(2);
                            prdName = sqlite_datareader.GetString(3);
                            prdPrice = sqlite_datareader.GetInt32(8);
                            prdLimitedCnt = sqlite_datareader.GetInt32(9);
                            productLimitedCntArray[selectedCategoryIndex, i] = prdLimitedCnt;
                            prdImageUrl = Path.Combine(currentDir, sqlite_datareader.GetString(11));
                            prdImageX = sqlite_datareader.GetInt32(15);
                            prdImageY = sqlite_datareader.GetInt32(16);
                            prdImageWidth = sqlite_datareader.GetInt32(17);
                            prdImageHeight = sqlite_datareader.GetInt32(18);
                            prdBadgeX = sqlite_datareader.GetInt32(19);
                            prdBadgeY = sqlite_datareader.GetInt32(20);
                            prdBadgeWidth = sqlite_datareader.GetInt32(21);
                            prdBadgeHeight = sqlite_datareader.GetInt32(22);
                            prdBadgeUrl = Path.Combine(currentDir, sqlite_datareader.GetString(23));
                            prdNameX = sqlite_datareader.GetInt32(24);
                            prdNameY = sqlite_datareader.GetInt32(25);
                            prdPriceX = sqlite_datareader.GetInt32(26);
                            prdPriceY = sqlite_datareader.GetInt32(27);
                            bgColor = sqlite_datareader.GetString(28);
                            foreColor = sqlite_datareader.GetString(29);
                            borderColor = sqlite_datareader.GetString(30);
                            soldFlag = sqlite_datareader.GetInt32(31);
                            if (productRestAmountArray[selectedCategoryIndex, i] == -1)
                            {
                                SQLiteDataReader sqlite_datareader1;
                                SQLiteCommand sqlite_cmd1;
                                sqlite_cmd1 = sqlite_conn.CreateCommand();
                                string queryCmd1 = "SELECT SUM(prdAmount) as prdRestAmount FROM " + constants.tbNames[3] + " WHERE categoryID=@CategoryID and prdID=@prdID and sumFlag='false'";
                                sqlite_cmd1.CommandText = queryCmd1;
                                sqlite_cmd1.Parameters.AddWithValue("@CategoryID", selectedCategoryID);
                                sqlite_cmd1.Parameters.AddWithValue("@prdID", productIDArray[i]);

                                sqlite_datareader1 = sqlite_cmd1.ExecuteReader();
                                while (sqlite_datareader1.Read())
                                {
                                    if(productLimitedCntArray[selectedCategoryIndex, i] != 0)
                                    {
                                        if (!sqlite_datareader1.IsDBNull(0))
                                        {
                                            productRestAmountArray[selectedCategoryIndex, i] = productLimitedCntArray[selectedCategoryIndex, i] - sqlite_datareader1.GetInt32(0);
                                        }
                                        else
                                        {
                                            productRestAmountArray[selectedCategoryIndex, i] = productLimitedCntArray[selectedCategoryIndex, i];
                                        }
                                    }
                                    else
                                    {
                                        productRestAmountArray[selectedCategoryIndex, i] = 10000;
                                    }
                                }

                            }
                            break;
                        }
                    }

                }

                Panel p = new Panel();
                borderPen = new Pen(borderClr, 3);
                BackgroundBitmap = null;
                if (i == 0 || i == 1)
                {
                    p = createPanel.CreatePanelForProducts(i * (2 * w1 + 20) + i * 20, 0, 2 * w1 + 20, 2 * h1 + 20, i.ToString(), true, borderClr, Color.White);
                    BackgroundBitmap = new Bitmap(2 * w1 + 20, 2 * h1 + 20);
                    p.Paint += new PaintEventHandler(panelbordercolor2_Paint);
                }
                else if (i == 2 || i == 3)
                {
                    p = createPanel.CreatePanelForProducts(2 * (2 * w1 + 20) + 2 * 20, (i - 2) * (h1 + 20), w1, h1, i.ToString(), true, borderClr, Color.White);
                    BackgroundBitmap = new Bitmap(w1, h1);
                    p.Paint += new PaintEventHandler(panelbordercolor_Paint);
                }
                else if (i == 4 || i == 5)
                {
                    p = createPanel.CreatePanelForProducts((i - 4) * (2 * w1 + 20) + (i - 4) * 20, 2 * h1 + 40, 2 * w1 + 20, 2 * h1 + 20, i.ToString(), true, borderClr, Color.White);
                    BackgroundBitmap = new Bitmap(2 * w1 + 20, 2 * h1 + 20);
                    p.Paint += new PaintEventHandler(panelbordercolor2_Paint);
                }
                else if (i == 6 || i == 7)
                {
                    p = createPanel.CreatePanelForProducts(2 * (2 * w1 + 20) + 2 * 20, (i - 4) * (h1 + 20), w1, h1, i.ToString(), true, borderClr, Color.White);
                    BackgroundBitmap = new Bitmap(w1, h1);
                    p.Paint += new PaintEventHandler(panelbordercolor_Paint);
                }
                else
                {
                    p = createPanel.CreatePanelForProducts((i - 8) * w1 + (i - 8) * 20, 2 * (2 * h1 + 40), w1, h1, i.ToString(), true, borderClr, Color.White);
                    BackgroundBitmap = new Bitmap(w1, h1);
                    p.Paint += new PaintEventHandler(panelbordercolor_Paint);
                }

                // Get font size to fit the specified card...
                int h = p.Width - 100 > p.Height - 60 ? (p.Height - 60) / 5 : (p.Width - 100) / 5 - 10;
                h = (h <= 0) ? 11 : h;
                int y = (p.Height - 60) / 5;
                if (p.Width - 100 < p.Height - 60) y = (p.Height - 60 - h - 3) / 4;

                int ftSize = 2 * h / 3;
                ///////////////////////////////////////////////////////////
                ///
                Color clr = bgColor != null ? HexToColor(bgColor) : Color.White;
                SetBackgroundColor(clr);

                pb_Image[i] = createPanel.CreatePictureBox(3, 3, p.Width - 6, p.Height - 6, i.ToString(), null);

                rc.X = p.Width / 6;
                rc.Y = p.Height / 6;
                rc.Width = 2 * p.Width / 3;
                rc.Height = 2 * p.Height / 3;

                if (prdImageWidth != 0 && prdImageHeight != 0)
                    rc = new Rectangle(prdImageX, prdImageY, prdImageWidth, prdImageHeight);

                Bitmap bm = null;
                if(prdImageUrl != "" && prdImageUrl != null)
                    bm = ShowCombinedImage(BackgroundBitmap, new Bitmap(prdImageUrl) /*imgUrl[i]*/, rc);

                rc.X = p.Width / 2;
                rc.Y = p.Height / 6;
                rc.Width = p.Width / 3;
                rc.Height = p.Height / 3;

                if (prdBadgeWidth != 0 && prdBadgeHeight != 0)
                    rc = new Rectangle(prdBadgeX, prdBadgeY, prdBadgeWidth, prdBadgeHeight);

                if (prdBadgeUrl != "" && prdBadgeUrl != null)
                {
                    string markPath = prdBadgeUrl;
                    bm = ShowCombinedImage(bm, (Bitmap)Image.FromFile(@markPath), rc);
                }

                if (soldFlag == 1 || (prdLimitedCnt != 0 && productRestAmountArray[selectedCategoryIndex, i] == 0))
                {
                    rc.X = p.Width / 6;
                    rc.Y = p.Height / 6;
                    rc.Width = 2 * p.Width / 3;
                    rc.Height = 2 * p.Height / 3;

                    if (prdImageWidth != 0 && prdImageHeight != 0)
                        rc = new Rectangle(prdImageX, prdImageY, prdImageWidth, prdImageHeight);
                    bm = ShowCombinedImage(bm, (Bitmap)Image.FromFile(constants.soldoutBadge), rc);
                    pb_Image[i].Enabled = false;

                }


                pb_Image[i].Image = bm;

                Font font = new Font("Series", ftSize, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                Point pt = new Point(p.Width / 2 - /*category[0, i].Length/2*/4 * ftSize - 7, 3 * (pb_Image[i].Height) / 5);
                if (prdNameX != 0 && prdNameY != 0)
                    pt = new Point(prdNameX, prdNameY);

                bl_Name[i] = createLabel.CreateBorderLabel(pt.X, pt.Y, ftSize * 8/*category[0,i].Length*/ , pb_Image[i].Height / 5, i.ToString(), /*category[0, i], */ prdName, HexToColor(borderColor), 1, font, HexToColor(foreColor));

                pt = new Point(p.Width / 2 - /*category[1, i].Length/2*/1 * ftSize - 7, 4 * (pb_Image[i].Height) / 5);
                if (prdPriceX != 0 && prdPriceY != 0)
                    pt = new Point(prdPriceX, prdPriceY);

                string prdPriceStr = (prdPrice == 0) ? "" : prdPrice.ToString();
                bl_Price[i] = createLabel.CreateBorderLabel(pt.X, pt.Y, ftSize * 3/*category[1,i].Length*/ + 20, pb_Image[i].Height / 5, i.ToString(), /*category[1, i], */ prdPriceStr, HexToColor(borderColor), 1, font, HexToColor(foreColor));

                if (prdName == "" || prdPriceStr == "")
                {
                    pb_Image[i].Enabled = false;
                    bl_Name[i].Enabled = false;
                    bl_Price[i].Enabled = false;
                }

                pb_Image[i].Click += new EventHandler(onItemPicClk);
                bl_Name[i].Click += new EventHandler(onItemLabelClk);
                bl_Price[i].Click += new EventHandler(onItemLabelClk);

                pb_Image[i].Controls.Add(bl_Name[i]);
                pb_Image[i].Controls.Add(bl_Price[i]);

                p.Controls.Add(pb_Image[i]);
                MainBodyPanelGlobal.Controls.Add(p);

                p_ProductCon[i] = p;
            }
        }

        private void CreateProductsList()
        {
            MainBodyPanelGlobal.Controls.Clear();
            int nWD = 0, nHD = 0;
            if (selectedCategoryLayout == 25 || selectedCategoryLayout == 16 || selectedCategoryLayout == 9 || selectedCategoryLayout == 4)
                nWD = nHD = (int)Math.Sqrt((double)selectedCategoryLayout);
            if (selectedCategoryLayout == 10) { nWD = 2; nHD = 5; }
            if (selectedCategoryLayout == 6) { nWD = 3; nHD = 2; }
            if (selectedCategoryLayout == 8) { nWD = 4; nHD = 2; }
            if (selectedCategoryLayout == 20) { nWD = 5; nHD = 4; }

            int w1 = (MainBodyPanelGlobal.Width - 20 * nWD) / nWD;
            int h1 = (MainBodyPanelGlobal.Height - 20 * (nHD - 1)) / nHD;
            if (selectedCategoryLayout == 10)
                w1 = (MainBodyPanelGlobal.Width - 50 * nWD) / nWD;

            nHeight1 = h1;
            nWidth1 = w1;
            DateTime now = DateTime.Now;
            string week = now.ToString("ddd");
            string currentTime = now.ToString("HH:mm");

            p_ProductCon = new Panel[selectedCategoryLayout];
            pb_Image = new PictureBox[selectedCategoryLayout];
            bl_Name = new BorderLabel[selectedCategoryLayout];
            bl_Price = new BorderLabel[selectedCategoryLayout];

            for (int i = 0; i < selectedCategoryLayout; i++)
            {
                string bgColor = "ffffff00";
                string foreColor = "ff000000";
                string borderColor = "ff223300";
                string prdName = "";
                int prdPrice = 0;
                string prdImageUrl = "";
                int prdImageX = 0;
                int prdImageY = 0;
                int prdImageWidth = 0;
                int prdImageHeight = 0;
                int prdBadgeX = 0;
                int prdBadgeY = 0;
                int prdBadgeWidth = 0;
                int prdBadgeHeight = 0;
                string prdBadgeUrl = "";
                int prdNameX = 0;
                int prdNameY = 0;
                int prdPriceX = 0;
                int prdPriceY = 0;
                int soldFlag = 0;
                int prdLimitedCnt = 0;

                if (sqlite_conn.State == ConnectionState.Closed)
                {
                    sqlite_conn.Open();
                }
                SQLiteDataReader sqlite_datareader;
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = sqlite_conn.CreateCommand();
                string queryCmd = "SELECT * FROM " + constants.tbNames[2] + " WHERE CategoryID=@CategoryID and CardNumber=@CardNumber ORDER BY CardNumber";
                sqlite_cmd.CommandText = queryCmd;
                sqlite_cmd.Parameters.AddWithValue("@CategoryID", selectedCategoryID);
                sqlite_cmd.Parameters.AddWithValue("@CardNumber", i + 1);

                sqlite_datareader = sqlite_cmd.ExecuteReader();

                while (sqlite_datareader.Read())
                {
                    string openTime = "";
                    if (week == "Sat")
                    {
                        openTime = sqlite_datareader.GetString(6);
                    }
                    else if (week == "Sun")
                    {
                        openTime = sqlite_datareader.GetString(7);
                    }
                    else
                    {
                        openTime = sqlite_datareader.GetString(5);
                    }
                    string[] openTimeArr = openTime.Split('/');
                    foreach (string openTimeArrItem in openTimeArr)
                    {
                        string[] openTimeSubArr = openTimeArrItem.Split('-');
                        if (String.Compare(openTimeSubArr[0], currentTime) <= 0 && String.Compare(openTimeSubArr[1], currentTime) >= 0)
                        {
                            productIDArray[i] = sqlite_datareader.GetInt32(0);
                            realProductIDArray[i] = sqlite_datareader.GetInt32(2);
                            prdName = sqlite_datareader.GetString(3);
                            prdPrice = sqlite_datareader.GetInt32(8);
                            prdLimitedCnt = sqlite_datareader.GetInt32(9);
                            productLimitedCntArray[selectedCategoryIndex, i] = prdLimitedCnt;
                            prdImageUrl = Path.Combine(currentDir, sqlite_datareader.GetString(11));
                            prdImageX = sqlite_datareader.GetInt32(15);
                            prdImageY = sqlite_datareader.GetInt32(16);
                            prdImageWidth = sqlite_datareader.GetInt32(17);
                            prdImageHeight = sqlite_datareader.GetInt32(18);
                            prdBadgeX = sqlite_datareader.GetInt32(19);
                            prdBadgeY = sqlite_datareader.GetInt32(20);
                            prdBadgeWidth = sqlite_datareader.GetInt32(21);
                            prdBadgeHeight = sqlite_datareader.GetInt32(22);
                            prdBadgeUrl = Path.Combine(currentDir, sqlite_datareader.GetString(23));
                            prdNameX = sqlite_datareader.GetInt32(24);
                            prdNameY = sqlite_datareader.GetInt32(25);
                            prdPriceX = sqlite_datareader.GetInt32(26);
                            prdPriceY = sqlite_datareader.GetInt32(27);
                            bgColor = sqlite_datareader.GetString(28);
                            foreColor = sqlite_datareader.GetString(29);
                            borderColor = sqlite_datareader.GetString(30);
                            soldFlag = sqlite_datareader.GetInt32(31);
                            if(productRestAmountArray[selectedCategoryIndex, i] == -1)
                            {
                                SQLiteDataReader sqlite_datareader1;
                                SQLiteCommand sqlite_cmd1;
                                sqlite_cmd1 = sqlite_conn.CreateCommand();
                                string queryCmd1 = "SELECT SUM(prdAmount) as prdRestAmount FROM " + constants.tbNames[3] + " WHERE categoryID=@CategoryID and prdID=@prdID and sumFlag='false'";
                                sqlite_cmd1.CommandText = queryCmd1;
                                sqlite_cmd1.Parameters.AddWithValue("@CategoryID", selectedCategoryID);
                                sqlite_cmd1.Parameters.AddWithValue("@prdID", productIDArray[i]);

                                sqlite_datareader1 = sqlite_cmd1.ExecuteReader();
                                while (sqlite_datareader1.Read())
                                {
                                    if(productLimitedCntArray[selectedCategoryIndex, i] != 0)
                                    {
                                        if (!sqlite_datareader1.IsDBNull(0))
                                        {
                                            productRestAmountArray[selectedCategoryIndex, i] = productLimitedCntArray[selectedCategoryIndex, i] - sqlite_datareader1.GetInt32(0);
                                        }
                                        else
                                        {
                                            productRestAmountArray[selectedCategoryIndex, i] = productLimitedCntArray[selectedCategoryIndex, i];
                                        }
                                    }
                                    else
                                    {
                                        productRestAmountArray[selectedCategoryIndex, i] = 10000;
                                    }
                                }

                            }
                            break;
                        }
                    }

                }


                int x = i % nWD;
                int yy = i / nWD;
                int d = 20;
                if (selectedCategoryLayout == 10) d = 50;
                BackgroundBitmap = null;
                Panel p = createPanel.CreatePanelForProducts(x * (w1 + d), yy * (h1 + 20), w1, h1, i.ToString(), true, borderClr, Color.White);
                BackgroundBitmap = new Bitmap(w1, h1);
                borderPen = new Pen(borderClr, 3);
                p.Paint += new PaintEventHandler(panelbordercolor_Paint);

                int h = p.Width - 100 > p.Height - 60 ? (p.Height - 60) / 5 : (p.Width - 100) / 5 - 10;
                h = (h <= 0) ? 11 : h;
                int y = (p.Height - 60) / 5;
                if (p.Width - 100 < p.Height - 60) y = (p.Height - 60 - h - 3) / 4;
                int ftSize = 2 * h / 3;

                Color clr = bgColor != null ? HexToColor(bgColor) : Color.White;
                SetBackgroundColor(clr);

                int temp = p.Width;

                pb_Image[i] = createPanel.CreatePictureBox(3, 3, p.Width - 6, p.Height - 6, i.ToString(), null);

                //MessageBox.Show(p.Height.ToString());

                rc.X = p.Width / 6;
                rc.Y = p.Height / 6;
                rc.Width = 2 * p.Width / 3;
                rc.Height = 2 * p.Height / 3;
                //rc = new Rectangle(p.Width / 6, p.Height / 6, 2 * p.Width / 3, 2 * p.Height / 3);

                if (prdImageWidth != 0 && prdImageHeight != 0)
                    rc = new Rectangle(prdImageX, prdImageY, prdImageWidth, prdImageHeight);
                Bitmap bm = null;
                if(prdImageUrl != "" && prdImageUrl != null)
                {
                    bm = ShowCombinedImage(BackgroundBitmap, new Bitmap(prdImageUrl) /*imgUrl[i]*/, rc);
                }

                rc.X = p.Width / 2;
                rc.Y = p.Height / 6;
                rc.Width = p.Width / 3;
                rc.Height = p.Height / 3;

                if (prdBadgeWidth != 0 && prdBadgeHeight != 0)
                    rc = new Rectangle(prdBadgeX, prdBadgeY, prdBadgeWidth, prdBadgeHeight);

                if (prdBadgeUrl != "" && prdBadgeUrl != null)
                {
                    string markPath = prdBadgeUrl;
                    bm = ShowCombinedImage(bm, (Bitmap)Image.FromFile(@markPath), rc);
                }

                if(soldFlag == 1 || (prdLimitedCnt != 0 && productRestAmountArray[selectedCategoryIndex, i] == 0))
                {
                    rc.X = p.Width / 6;
                    rc.Y = p.Height / 6;
                    rc.Width = 2 * p.Width / 3;
                    rc.Height = 2 * p.Height / 3;

                    if (prdImageWidth != 0 && prdImageHeight != 0)
                        rc = new Rectangle(prdImageX, prdImageY, prdImageWidth, prdImageHeight);
                    bm = ShowCombinedImage(bm, (Bitmap)Image.FromFile(constants.soldoutBadge), rc);
                    pb_Image[i].Enabled = false;

                }

                pb_Image[i].Image = bm;



                Font font = new Font("Series", ftSize, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                Point pt = new Point(p.Width / 2 - /*category[0, i].Length/2*/4 * ftSize - 7, 3 * (pb_Image[i].Height) / 5);
                if (prdNameX != 0 && prdNameY != 0)
                    pt = new Point(prdNameX, prdNameY);

                bl_Name[i] = createLabel.CreateBorderLabel(pt.X, pt.Y, ftSize * 8/*category[0,i].Length*/ , pb_Image[i].Height / 5, i.ToString(), /*category[0, i], */ prdName, HexToColor(borderColor), 1, font, HexToColor(foreColor));


                pt = new Point(p.Width / 2 - /*category[1, i].Length/2*/1 * ftSize - 7, 4 * (pb_Image[i].Height) / 5);
                if (prdPriceX != 0 && prdPriceY != 0)
                    pt = new Point(prdPriceX, prdPriceY);
                string prdPriceStr = (prdPrice == 0) ? "" : prdPrice.ToString();
                bl_Price[i] = createLabel.CreateBorderLabel(pt.X, pt.Y, ftSize * 3/*category[1,i].Length*/ + 20, pb_Image[i].Height / 5, i.ToString(), /*category[1, i], */ prdPriceStr, HexToColor(borderColor), 1, font, HexToColor(foreColor));

                if(prdName == "" || prdPriceStr == "")
                {
                    pb_Image[i].Enabled = false;
                    bl_Name[i].Enabled = false;
                    bl_Price[i].Enabled = false;
                }
                pb_Image[i].Click += new EventHandler(onItemPicClk);
                bl_Name[i].Click += new EventHandler(onItemLabelClk);
                bl_Price[i].Click += new EventHandler(onItemLabelClk);

                pb_Image[i].Controls.Add(bl_Name[i]);
                pb_Image[i].Controls.Add(bl_Price[i]);

                p.Controls.Add(pb_Image[i]);
                MainBodyPanelGlobal.Controls.Add(p);
                p_ProductCon[i] = p;
            }
        }
        public static Color HexToColor(string hexString)
        {
            //replace # occurences
            if (hexString.IndexOf('#') != -1)
                hexString = hexString.Replace("#", "");

            int a, r, g, b = 0;

            a = int.Parse(hexString.Substring(0, 2), NumberStyles.AllowHexSpecifier);
            r = int.Parse(hexString.Substring(2, 2), NumberStyles.AllowHexSpecifier);
            g = int.Parse(hexString.Substring(4, 2), NumberStyles.AllowHexSpecifier);
            b = int.Parse(hexString.Substring(6, 2), NumberStyles.AllowHexSpecifier);

            return Color.FromArgb(a, r, g, b);
        }
        private Bitmap ShowCombinedImage(Bitmap back, Bitmap over, Rectangle rc)
        {
            if (back == null) return null;
            Bitmap CombinedBitmap = new Bitmap(back);
            if (over != null)
                using (Graphics gr = Graphics.FromImage(CombinedBitmap))
                    gr.DrawImage(over, rc);
            return CombinedBitmap;
        }

        private void SetBackgroundColor(Color clr)
        {
            for (int i = 0; i < BackgroundBitmap.Width; i++)
                for (int j = 0; j < BackgroundBitmap.Height; j++)
                    BackgroundBitmap.SetPixel(i, j, clr);
        }
        public void onItemPicClk(object sender, EventArgs e)
        {
            bLoad = true;
            PictureBox p = (PictureBox)sender;
            int index = int.Parse(p.Name);
            ClickEvent(index);
            selectedPrdIndex = index;
            if(productRestAmountArray[selectedCategoryIndex, index] <= 0)
            {
                MessageBox.Show(constants.restEmptyMessage);
            }
            else
            {
                orderDialog.ShowMainMenuDetail(bl_Name[index].Text, bl_Price[index].Text, pb_Image[index].Image, productRestAmountArray[selectedCategoryIndex, index]);
            }
        }
        public void onItemLabelClk(object sender, EventArgs e)
        {
            bLoad = true;
            BorderLabel p = (BorderLabel)sender;
            int index = int.Parse(p.Name);
            ClickEvent(index);
            selectedPrdIndex = index;
            if (productRestAmountArray[selectedCategoryIndex, index] <= 0)
            {
                MessageBox.Show(constants.restEmptyMessage);
            }
            else
            {
                orderDialog.ShowMainMenuDetail(bl_Name[index].Text, bl_Price[index].Text, pb_Image[index].Image, productRestAmountArray[selectedCategoryIndex, index]);
            }
        }
        private void ClickEvent(int index)
        {
            if (index == curProduct)
            {
                penClr = penClr == borderClr ? Color.FromArgb(255, 255, 0, 0) : borderClr;
                borderPen = new Pen(penClr, 3);
                p_ProductCon[curProduct].Invalidate(true);
            }
            else
            {
                int cur = curProduct;
                borderPen = new Pen(borderClr, 3);
                p_ProductCon[cur].Name = index.ToString();
                p_ProductCon[curProduct].Invalidate(true);

                borderPen = new Pen(Color.FromArgb(255, 255, 0, 0), 3);
                curProduct = index;
                p_ProductCon[curProduct].Invalidate(true);
                p_ProductCon[cur].Name = cur.ToString();
            }
        }

        private void panelbordercolor_Paint(object sender, PaintEventArgs e)
        {

            Panel p = (Panel)sender;
            int index = int.Parse(p.Name);
            if (!bLoad)
            {
                Graphics g = e.Graphics;
                g.DrawLine(borderPen, 0, 1, nWidth1, 1);
                g.DrawLine(borderPen, 1, 0, 1, nHeight1);
                g.DrawLine(borderPen, nWidth1 - 2, 0, nWidth1 - 2, nHeight1);
                g.DrawLine(borderPen, 0, nHeight1 - 2, nWidth1, nHeight1 - 2);
                g.Dispose();
            }
            else
            {
                if (curProduct == index)
                {
                    borderPen = new Pen(Color.Red, 3);
                    Graphics g = e.Graphics;
                    g.DrawLine(borderPen, 0, 1, nWidth1, 1);
                    g.DrawLine(borderPen, 1, 0, 1, nHeight1);
                    g.DrawLine(borderPen, nWidth1 - 2, 0, nWidth1 - 2, nHeight1);
                    g.DrawLine(borderPen, 0, nHeight1 - 2, nWidth1, nHeight1 - 2);
                    g.Dispose();
                }
                else
                {
                    Pen pp = new Pen(borderClr, 3);
                    Graphics g = e.Graphics;
                    g.DrawLine(pp, 0, 1, nWidth1, 1);
                    g.DrawLine(pp, 1, 0, 1, nHeight1);
                    g.DrawLine(pp, nWidth1 - 2, 0, nWidth1 - 2, nHeight1);
                    g.DrawLine(pp, 0, nHeight1 - 2, nWidth1, nHeight1 - 2);
                    g.Dispose();
                }
            }

        }

        private void panelbordercolor2_Paint(object sender, PaintEventArgs e)
        {

            Panel p = (Panel)sender;
            int index = int.Parse(p.Name);

            if (!bLoad)
            {
                Graphics g = e.Graphics;
                g.DrawLine(borderPen, 0, 1, nWidth, 1);
                g.DrawLine(borderPen, 1, 0, 1, nHeight);
                g.DrawLine(borderPen, nWidth - 2, 0, nWidth - 2, nHeight);
                g.DrawLine(borderPen, 0, nHeight - 2, nWidth, nHeight - 2);
                g.Dispose();
            }
            else
            {
                if (curProduct == index)
                {
                    borderPen = new Pen(Color.Red, 3);
                    Graphics g = e.Graphics;
                    g.DrawLine(borderPen, 0, 1, nWidth, 1);
                    g.DrawLine(borderPen, 1, 0, 1, nHeight);
                    g.DrawLine(borderPen, nWidth - 2, 0, nWidth - 2, nHeight);
                    g.DrawLine(borderPen, 0, nHeight - 2, nWidth, nHeight - 2);
                    g.Dispose();
                }
                else
                {
                    Pen pp = new Pen(borderClr, 3);
                    Graphics g = e.Graphics;
                    g.DrawLine(pp, 0, 1, nWidth, 1);
                    g.DrawLine(pp, 1, 0, 1, nHeight);
                    g.DrawLine(pp, nWidth - 2, 0, nWidth - 2, nHeight);
                    g.DrawLine(pp, 0, nHeight - 2, nWidth, nHeight - 2);
                    g.Dispose();
                }

            }
        }
        private void panelbordercolor3_Paint(object sender, PaintEventArgs e)
        {
            Panel p = (Panel)sender;
            int index = int.Parse(p.Name);
            if (!bLoad)
            {
                Graphics g = e.Graphics;
                g.DrawLine(borderPen, 0, 1, nWidth2, 1);
                g.DrawLine(borderPen, 1, 0, 1, nHeight2);
                g.DrawLine(borderPen, nWidth2 - 2, 0, nWidth2 - 2, nHeight2);
                g.DrawLine(borderPen, 0, nHeight2 - 2, nWidth2, nHeight2 - 2);
                g.Dispose();
            }
            else
            {
                if (curProduct == index)
                {
                    borderPen = new Pen(Color.Red, 3);
                    Graphics g = e.Graphics;
                    g.DrawLine(borderPen, 0, 1, nWidth2, 1);
                    g.DrawLine(borderPen, 1, 0, 1, nHeight2);
                    g.DrawLine(borderPen, nWidth2 - 2, 0, nWidth2 - 2, nHeight2);
                    g.DrawLine(borderPen, 0, nHeight2 - 2, nWidth2, nHeight2 - 2);
                    g.Dispose();
                }
                else
                {
                    Pen pp = new Pen(borderClr, 3);
                    Graphics g = e.Graphics;
                    g.DrawLine(pp, 0, 1, nWidth2, 1);
                    g.DrawLine(pp, 1, 0, 1, nHeight2);
                    g.DrawLine(pp, nWidth2 - 2, 0, nWidth2 - 2, nHeight2);
                    g.DrawLine(pp, 0, nHeight2 - 2, nWidth2, nHeight2 - 2);
                    g.Dispose();
                }
            }
        }
        private void panelbordercolor4_Paint(object sender, PaintEventArgs e)
        {
            Panel p = (Panel)sender;
            int index = int.Parse(p.Name);
            if (!bLoad)
            {
                Graphics g = e.Graphics;
                g.DrawLine(borderPen, 0, 1, nWidth3, 1);
                g.DrawLine(borderPen, 1, 0, 1, nHeight3);
                g.DrawLine(borderPen, nWidth3 - 2, 0, nWidth3 - 2, nHeight3);
                g.DrawLine(borderPen, 0, nHeight3 - 2, nWidth3, nHeight3 - 2);
                g.Dispose();
            }
            else
            {
                if (curProduct == index)
                {
                    borderPen = new Pen(Color.Red, 3);
                    Graphics g = e.Graphics;
                    g.DrawLine(borderPen, 0, 1, nWidth3, 1);
                    g.DrawLine(borderPen, 1, 0, 1, nHeight3);
                    g.DrawLine(borderPen, nWidth3 - 2, 0, nWidth3 - 2, nHeight3);
                    g.DrawLine(borderPen, 0, nHeight3 - 2, nWidth3, nHeight3 - 2);
                    g.Dispose();
                }
                else
                {
                    Pen pp = new Pen(borderClr, 3);
                    Graphics g = e.Graphics;
                    g.DrawLine(pp, 0, 1, nWidth3, 1);
                    g.DrawLine(pp, 1, 0, 1, nHeight3);
                    g.DrawLine(pp, nWidth3 - 2, 0, nWidth3 - 2, nHeight3);
                    g.DrawLine(pp, 0, nHeight3 - 2, nWidth3, nHeight3 - 2);
                    g.Dispose();
                }
            }
        }

        private void CreateOrderTable(int startIndex)
        {

            for (int i = 0; i < 5; i++)
            {

                orderProductNameLabel[i].Name = "prd_" + (i + startIndex);
                TempLabel = orderProductNameLabel[i];
                SetText(orderProductNameArray[i + startIndex]);
                //orderProductNameLabel[i].Text = orderProductNameArray[i + startIndex];
                orderAmountLabel[i].Name = "prdNum_" + (i + startIndex);
                TempLabel = orderAmountLabel[i];
                SetText(orderAmountArray[i + startIndex]);
                //orderAmountLabel[i].Text = orderAmountArray[i + startIndex];
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
            int selectedPrdIndex = orderProuctIndexArray[selectedIndex];
            int selectedCategoryIndexs = orderCategoryIndexArray[selectedIndex];
            if (btnName[0] == "prdIncrease" && orderAmount < 9 && productRestAmountArray[selectedCategoryIndexs, selectedPrdIndex] > 0)
            {
                orderAmount++;
                orderTotalPrice += int.Parse(orderProductPriceArray[selectedIndex]);
                productRestAmountArray[selectedCategoryIndexs, selectedPrdIndex]--;
            }
            else if(btnName[0] == "prdDecrease" && orderAmount > 1)
            {
                orderAmount--;
                productRestAmountArray[selectedCategoryIndexs, selectedPrdIndex]++;
                orderTotalPrice -= int.Parse(orderProductPriceArray[selectedIndex]);
            }
            orderAmountArray[selectedIndex] = orderAmount.ToString();
            orderAmountLabel[selectedIndex - startIndex].Text = orderAmount.ToString();
            orderPriceTotalLabel.Text = orderTotalPrice.ToString();
            //orderPriceEnterLabel.Text = "150000";
        }

        private void orderDelete(object sender, EventArgs e)
        {
            Label tempBtn = (Label)sender;
            string[] btnName = tempBtn.Name.Split('_');
            int selectedIndex = int.Parse(btnName[1]);
            currentSelectedId--;
            int selectedPrdIndex = orderProuctIndexArray[selectedIndex];
            productRestAmountArray[selectedCategoryIndex, selectedPrdIndex] += int.Parse(orderAmountArray[selectedIndex]);
            orderTotalPrice -= int.Parse(orderProductPriceArray[selectedIndex]) * int.Parse(orderAmountArray[selectedIndex]);
            orderProductIDArray = orderProductIDArray.Where((val, idx) => idx != selectedIndex).ToArray();
            orderProductNameArray = orderProductNameArray.Where((val, idx) => idx != selectedIndex).ToArray();
            orderProductPriceArray = orderProductPriceArray.Where((val, idx) => idx != selectedIndex).ToArray();
            orderAmountArray = orderAmountArray.Where((val, idx) => idx != selectedIndex).ToArray();
            orderProuctIndexArray = orderProuctIndexArray.Where((val, idx) => idx != selectedIndex).ToArray();
            orderCategoryIDArray = orderCategoryIDArray.Where((val, idx) => idx != selectedIndex).ToArray();
            orderCategoryIndexArray = orderCategoryIndexArray.Where((val, idx) => idx != selectedIndex).ToArray();
            orderRealProductIDArray = orderRealProductIDArray.Where((val, idx) => idx != selectedIndex).ToArray();
            if (orderTotalPrice == 0)
            {
                orderPriceTotalLabel.Text = "";
            }
            else
            {
                orderPriceTotalLabel.Text = orderTotalPrice.ToString();
            }
            CreateOrderTable(startIndex);
        }

        public void setVal(string[] s)
        {
            string[] msgValue = s;
            int index = Array.IndexOf(orderProductIDArray, productIDArray[selectedPrdIndex]);
            if(index != -1)
            {
                orderAmountArray[index] = (int.Parse(orderAmountArray[index]) + int.Parse(msgValue[1])).ToString();
                CreateOrderTable(startIndex);
                productRestAmountArray[selectedCategoryIndex, selectedPrdIndex] -= int.Parse(msgValue[1]);
                orderTotalPrice += int.Parse(msgValue[2]);
                orderPriceTotalLabel.Text = orderTotalPrice.ToString();
                orderPriceEnterLabel.Text = "15000";
            }
            else
            {
                orderProductNameArray[currentSelectedId] = msgValue[0];
                orderAmountArray[currentSelectedId] = msgValue[1];
                orderProuctIndexArray[currentSelectedId] = selectedPrdIndex;
                productRestAmountArray[selectedCategoryIndex, selectedPrdIndex] -= int.Parse(orderAmountArray[currentSelectedId]);
                orderProductPriceArray[currentSelectedId] = msgValue[3];
                orderProductIDArray[currentSelectedId] = productIDArray[selectedPrdIndex];
                orderCategoryIDArray[currentSelectedId] = selectedCategoryID;
                orderCategoryIndexArray[currentSelectedId] = selectedCategoryIndex;
                orderRealProductIDArray[currentSelectedId] = realProductIDArray[selectedPrdIndex];
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
                //orderPriceEnterLabel.Text = "15000";
                currentSelectedId++;
            }
        }

        private void ShowTicketing(object sender, EventArgs e)
        {
            int orderRestPrice = int.Parse(orderPriceEnterLabel.Text) - int.Parse(orderPriceTotalLabel.Text);
            if(orderRestPrice >= 0)
            {
                orderDialog.ShowTicketingDetail(orderProductNameArray, orderProductPriceArray, orderAmountArray, currentSelectedId);
            }

        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // SaleScreen
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "SaleScreen";
            this.ResumeLayout(false);

        }
        PaperSize paperSize = new PaperSize("papersize", 203, 800);//set the paper size

        public void Ticketing()
        {
            orderRestPriceLabel.Text = (int.Parse(orderPriceEnterLabel.Text) - int.Parse(orderPriceTotalLabel.Text)).ToString();

            ticketingButtonGlobal.BackColor = Color.FromArgb(255, 217, 217, 217);
            ticketingButtonGlobal.ForeColor = Color.Black;
            ticketingButtonGlobal.Enabled = false;

            int iChange = int.Parse(orderPriceEnterLabel.Text) - int.Parse(orderPriceTotalLabel.Text);
            if (iChange > 0)
                comModule.TicketRun(iChange);


            orderTotalTicketForReceipt = currentSelectedId;
            orderTotalPriceForReceipt = orderTotalPrice;

            PrintRun();


        }

        private void OrderDataSaving()
        {
            DateTime now = DateTime.Now;

            if (sqlite_conn.State == ConnectionState.Closed)
            {
                sqlite_conn.Open();
            }
            SQLiteCommand sqlite_cmd;
            SQLiteDataReader sqlite_datareader;
            SQLiteDataReader sqlite_datareader1;

            string week = now.ToString("ddd");
            string currentTime = now.ToString("HH:mm");

            string storeEndqurey = "SELECT * FROM " + constants.tbNames[6];
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = storeEndqurey;
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                if (!sqlite_datareader.IsDBNull(0))
                {
                    if (week == "Sat")
                    {
                        storeEndTime = (sqlite_datareader.GetString(6)).Split('/')[1];
                        openTime = (sqlite_datareader.GetString(4)).Split('/')[0].Split('-')[0];
                    }
                    else if (week == "Sun")
                    {
                        storeEndTime = (sqlite_datareader.GetString(6)).Split('/')[2];
                        openTime = (sqlite_datareader.GetString(5)).Split('/')[0].Split('-')[0];
                    }
                    else
                    {
                        storeEndTime = (sqlite_datareader.GetString(6)).Split('/')[0];
                        openTime = (sqlite_datareader.GetString(3)).Split('/')[0].Split('-')[0];
                    }

                }
            }

            sumDayTime1 = constants.sumDayTimeStart(storeEndTime);
            sumDayTime2 = constants.sumDayTimeEnd(storeEndTime);
            openDayTime = constants.openDateTime(openTime, storeEndTime);
            sumDate = constants.sumDate(storeEndTime);


            currentTicketNo = 1;
            string ticketsql = "SELECT MAX(ticketNo) FROM " + constants.tbNames[3] + " WHERE saleDate>=@sumDate1 and saleDate<=@sumDate2";
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = ticketsql;
            sqlite_cmd.Parameters.AddWithValue("@sumDate1", sumDayTime1);
            sqlite_cmd.Parameters.AddWithValue("@sumDate2", sumDayTime2);
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                if (sqlite_datareader.IsDBNull(0))
                {
                    currentTicketNo = 1;
                }
                else
                {
                    currentTicketNo = sqlite_datareader.GetInt32(0) + 1;
                }
            }
            string serialsql = "SELECT MAX(serialNo) FROM " + constants.tbNames[3] + "";
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = serialsql;
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            currentSerialNo = 1;
            while (sqlite_datareader.Read())
            {
                if (sqlite_datareader.IsDBNull(0))
                {
                    currentSerialNo = StartSerialNo;
                }
                else
                {
                    currentSerialNo = sqlite_datareader.GetInt32(0) + 1;
                    if (currentSerialNo == 10000)
                    {
                        currentSerialNo = 1;
                    }
                }
            }
            for (int k = 0; k < currentSelectedId; k++)
            {
                string orderRun = "INSERT INTO " + constants.tbNames[3] + " (PrdID, PrdName, PrdPrice, PrdAmount, ticketNo, saleDate, categoryID, serialNo, prdRealID) VALUES (@prdID, @prdName, @prdPrice, @prdAmount, @ticketNo, @saleDate, @categoryID, @serialNo, @realPrdID)";
                sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = orderRun;
                sqlite_cmd.Parameters.AddWithValue("@prdID", orderProductIDArray[k]);
                sqlite_cmd.Parameters.AddWithValue("@prdName", orderProductNameArray[k]);
                sqlite_cmd.Parameters.AddWithValue("@prdPrice", orderProductPriceArray[k]);
                sqlite_cmd.Parameters.AddWithValue("@prdAmount", orderAmountArray[k]);
                sqlite_cmd.Parameters.AddWithValue("@ticketNo", currentTicketNo);
                sqlite_cmd.Parameters.AddWithValue("@saleDate", now);
                sqlite_cmd.Parameters.AddWithValue("@categoryID", orderCategoryIDArray[k]);
                sqlite_cmd.Parameters.AddWithValue("@serialNo", currentSerialNo);
                sqlite_cmd.Parameters.AddWithValue("@realPrdID", orderRealProductIDArray[k]);
                sqlite_cmd.ExecuteNonQuery();

                string queryCmd1 = "SELECT sum(" + constants.tbNames[3] + ".prdAmount) as prdRestAmount, " + constants.tbNames[2] + ".LimitedCnt FROM " + constants.tbNames[2] + " LEFT JOIN " + constants.tbNames[3] + " on " + constants.tbNames[3] + ".prdID = " + constants.tbNames[2] + ".id WHERE " + constants.tbNames[2] + ".id=@prdID";
                sqlite_cmd.CommandText = queryCmd1;
                sqlite_cmd.Parameters.AddWithValue("@prdID", orderProductIDArray[k]);

                sqlite_datareader1 = sqlite_cmd.ExecuteReader();
                int restAmount = 0;
                while (sqlite_datareader1.Read())
                {
                    if (!sqlite_datareader1.IsDBNull(0))
                    {
                        restAmount = sqlite_datareader1.GetInt32(1) - sqlite_datareader1.GetInt32(0);
                    }
                    else
                    {
                        restAmount = sqlite_datareader1.GetInt32(1);
                    }
                }
                if (restAmount == 0)
                {
                    sqlite_cmd = sqlite_conn.CreateCommand();
                    string queryCmd2 = "UPDATE " + constants.tbNames[2] + " SET SoldFlag=1 WHERE ProductID=@productID";
                    sqlite_cmd.CommandText = queryCmd2;
                    sqlite_cmd.Parameters.AddWithValue("@productID", orderProductIDArray[k]);
                    sqlite_cmd.ExecuteNonQuery();
                }
            }
            sqlite_conn.Close();
        }

        delegate void SetTextCallback(string text);

        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (TempLabel.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                TempLabel.Text = text;
            }
        }
        public static PageSettings GetPrinterPageInfo()
        {
            return GetPrinterPageInfo(null);
        }
        public static PageSettings GetPrinterPageInfo(String printerName)
        {
            PrinterSettings settings;

            // If printer name is not set, look for default printer
            if (String.IsNullOrEmpty(printerName))
            {
                foreach (var printer in PrinterSettings.InstalledPrinters)
                {
                    settings = new PrinterSettings();

                    settings.PrinterName = printer.ToString();

                    if (settings.IsDefaultPrinter)
                        return settings.DefaultPageSettings;
                }

                return null; // <- No default printer  
            }

            // printer by its name 
            settings = new PrinterSettings();

            settings.PrinterName = printerName;

            return settings.DefaultPageSettings;
        }

        internal static void SpotTroubleUsingProperties(ref String statusReport, PrintQueue pq)
        {
            
            if (pq.HasPaperProblem)
            {
                statusReport = statusReport + "Has a paper problem. ";
            }
            if (!(pq.HasToner))
            {
                statusReport = statusReport + "Is out of toner. ";
            }
            if (pq.IsDoorOpened)
            {
                statusReport = statusReport + "Has an open door. ";
            }
            if (pq.IsInError)
            {
                statusReport = statusReport + "Is in an error state. ";
            }
            if (pq.IsNotAvailable)
            {
                statusReport = statusReport + "Is not available. ";
            }
            if (pq.IsOffline)
            {
                statusReport = statusReport + "Is off line. ";
            }
            if (pq.IsOutOfMemory)
            {
                statusReport = statusReport + "Is out of memory. ";
            }
            if (pq.IsOutOfPaper)
            {
                statusReport = statusReport + "Is out of paper.";
            }
            if (pq.IsOutputBinFull)
            {
                statusReport = statusReport + "Has a full output bin. ";
            }
            if (pq.IsPaperJammed)
            {
                statusReport = statusReport + "Has a paper jam. ";
            }
            if (pq.IsPaused)
            {
                statusReport = statusReport + "Is paused. ";
            }
            if (pq.IsTonerLow)
            {
                statusReport = statusReport + "Is low on toner. ";
            }
            if (pq.NeedUserIntervention)
            {
                statusReport = statusReport + "Needs user intervention. ";
            }
            
            // Check if queue is even available at this time of day
            // The following method is defined in the complete example.
            //ReportAvailabilityAtThisTime(ref statusReport, pq);
        }//end SpotTroubleUsingProperties
        private void PrintRun()
        {
            PrintDocument ticketPrintDocument = new PrintDocument();
            PrintPreviewDialog ticketPrintPreviewDialog = new PrintPreviewDialog();
            PrintDialog ticketPrintDialog = new PrintDialog();
            if (PurchaseType == 0)
            {
                lineNumbers = 0;
                paperSize = new PaperSize("papersize", constants.singleticketPrintPaperWidth, constants.singleticketPrintPaperHeight);
                ticketPrintDocument.PrintPage += new PrintPageEventHandler(TicketSingle_PrintPage);
            }
            else
            {
                paperSize = new PaperSize("papersize", constants.multiticketPrintPaperWidth, constants.multiticketPrintPaperHeight + 60 * currentSelectedId);
                ticketPrintDocument.PrintPage += new PrintPageEventHandler(TicketMultiple_PrintPage);
            }

            ticketPrintDocument.BeginPrint += new PrintEventHandler(BeginPrintPage);

            ticketPrintPreviewDialog.Document = ticketPrintDocument;
            ticketPrintDialog.Document = ticketPrintDocument;

            PrinterSettings settings = new PrinterSettings();

            ticketPrintDocument.DefaultPageSettings.PaperSize = paperSize;

            string defaultPrinterName = settings.PrinterName;
            //MessageBox.Show(defaultPrinterName);

            PageSettings page = GetPrinterPageInfo();
            page.PaperSize = paperSize;
            page.Margins = new Margins(0, 0, 0, 50);
            ticketPrintDocument.EndPrint += new PrintEventHandler(EndPrintPage);


            try
            {
                ticketPrintDocument.Print();
            }
            catch (InvalidPrinterException ex)
            {
                Console.WriteLine("printer Exception : " + ex);
            }

            if (connectStatus == "disconnect")
            {
                messageDialog.ShowPrintErrorMessage(constants.printOfflineErrorMsg, constants.printSubErrorMsg);
            }
            if (statusVal == "Is out of paper.")
            {
                messageDialog.ShowPrintErrorMessage(constants.printErrorMsg, constants.printSubErrorMsg);
            }

        }

        private void BeginPrintPage(object sender, EventArgs e)
        {
            PrintQueue print_queue = LocalPrintServer.GetDefaultPrintQueue();
            Console.WriteLine("begin++++" + print_queue.IsOutOfPaper);

        }
        private void CancelOrder(object sender, EventArgs e)
        {
            if (sqlite_conn.State == ConnectionState.Closed)
            {
                sqlite_conn.Open();
            }
            SQLiteCommand sqlite_cmd;
            SQLiteDataReader sqlite_datareader;
            DateTime now = DateTime.Now;
            string week = now.ToString("ddd");
            string currentTime = now.ToString("HH:mm");
            for (int i = 0; i < categoryDisAmount; i++)
            {
                sqlite_cmd = sqlite_conn.CreateCommand();
                string queryCmd = "SELECT * FROM " + constants.tbNames[2] + " WHERE CategoryID=@CategoryID ORDER BY CardNumber";
                sqlite_cmd.CommandText = queryCmd;
                sqlite_cmd.Parameters.AddWithValue("@CategoryID", categoryIDArray[i]);

                sqlite_datareader = sqlite_cmd.ExecuteReader();
                int k = 0;
                while (sqlite_datareader.Read())
                {
                    string openTime = "";
                    if (week == "Sat")
                    {
                        openTime = sqlite_datareader.GetString(6);
                    }
                    else if (week == "Sun")
                    {
                        openTime = sqlite_datareader.GetString(7);
                    }
                    else
                    {
                        openTime = sqlite_datareader.GetString(5);
                    }
                    string[] openTimeArr = openTime.Split('/');
                    foreach (string openTimeArrItem in openTimeArr)
                    {
                        string[] openTimeSubArr = openTimeArrItem.Split('-');
                        if (String.Compare(openTimeSubArr[0], currentTime) <= 0 && String.Compare(openTimeSubArr[1], currentTime) >= 0)
                        {
                            SQLiteDataReader sqlite_datareader1;
                            SQLiteCommand sqlite_cmd1;
                            sqlite_cmd1 = sqlite_conn.CreateCommand();
                            string queryCmd1 = "SELECT SUM(prdAmount) as prdRestAmount FROM " + constants.tbNames[3] + " WHERE categoryID=@CategoryID and prdID=@prdID";
                            sqlite_cmd1.CommandText = queryCmd1;
                            sqlite_cmd1.Parameters.AddWithValue("@CategoryID", categoryIDArray[i]);
                            sqlite_cmd1.Parameters.AddWithValue("@prdID", sqlite_datareader.GetInt32(0));

                            sqlite_datareader1 = sqlite_cmd1.ExecuteReader();
                            while (sqlite_datareader1.Read())
                            {
                                if (!sqlite_datareader1.IsDBNull(0))
                                {
                                    productRestAmountArray[i, k] = productLimitedCntArray[i, k] - sqlite_datareader1.GetInt32(0);
                                }
                                else
                                {
                                    productRestAmountArray[i, k] = productLimitedCntArray[i, k];
                                }
                            }

                            break;
                        }
                    }
                    k++;
                }

            }
            orderCategoryIDArray = new int[100];
            orderProductIDArray = new int[100];
            orderProductNameArray = new string[100];
            orderProductPriceArray = new string[100];
            orderAmountArray = new string[100];
            orderProuctIndexArray = new int[100];
            orderPriceTotalLabel.Text = "";
            orderTotalPrice = 0;
            currentSelectedId = 0;
            CreateOrderTable(0);

            sqlite_conn.Close();

            comModule.OrderCancel();
        }

        private void TicketSingle_PrintPage(object sender, PrintPageEventArgs e)
        {
            DateTime now = DateTime.Now;
            PrintDocument ticketPrintDocument = (PrintDocument)sender;
            while (lineNumbers < currentSelectedId)
            {
                float currentY = 0;// declare  one variable for height measurement
                RectangleF rect1 = new RectangleF(5, currentY, constants.singleticketPrintPaperWidth - 10, 30);
                StringFormat format1 = new StringFormat();
                format1.Alignment = StringAlignment.Near;
                e.Graphics.DrawString(now.ToString("yyyy/MM/dd  HH:mm:ss"), new Font("Seri", constants.fontSizeMedium, FontStyle.Regular), Brushes.Black, rect1, format1);
                currentY += 30;
                if (SerialNo == 1)
                {
                    RectangleF rect2 = new RectangleF(5, currentY, constants.singleticketPrintPaperWidth - 10, 20);
                    StringFormat format2 = new StringFormat();
                    format2.Alignment = StringAlignment.Near;
                    e.Graphics.DrawString(currentSerialNo.ToString("0000"), new Font("Seri", constants.fontSizeMedium, FontStyle.Bold), Brushes.Black, rect2, format2);
                    currentY += 20;
                }
                int totalPrice = int.Parse(orderProductPriceArray[lineNumbers]) * int.Parse(orderAmountArray[lineNumber]);
                RectangleF rect3 = new RectangleF(0, currentY, constants.singleticketPrintPaperWidth, 25);
                StringFormat format3 = new StringFormat();
                format3.Alignment = StringAlignment.Center;
                e.Graphics.DrawString(orderProductNameArray[lineNumbers], new Font("Seri", constants.fontSizeMedium, FontStyle.Bold), Brushes.Black, rect3, format3);
                currentY += 25;

                RectangleF rect4 = new RectangleF(0, currentY, constants.singleticketPrintPaperWidth, 30);
                StringFormat format4 = new StringFormat();
                format4.Alignment = StringAlignment.Center;
                e.Graphics.DrawString(totalPrice.ToString() + constants.unit, new Font("Seri", constants.fontSizeMedium, FontStyle.Bold), Brushes.Black, rect4, format4);
                currentY += 30;

                string validText = "当日限り有効";
                if (ValidDate != 0)
                {
                    DateTime validDates = now.AddDays(2);
                    validText = validDates.ToString("yyyy/MM/dd") + "まで有効";
                }
                RectangleF rect5 = new RectangleF(0, currentY, constants.singleticketPrintPaperWidth, 30);
                StringFormat format5 = new StringFormat();
                format5.Alignment = StringAlignment.Center;
                e.Graphics.DrawString(validText, new Font("Seri", constants.fontSizeMedium, FontStyle.Bold), Brushes.Black, rect5, format5);
                currentY += 30;

                RectangleF rect6 = new RectangleF(0, currentY, constants.singleticketPrintPaperWidth, 30);
                StringFormat format6 = new StringFormat();
                format6.Alignment = StringAlignment.Center;
                e.Graphics.DrawString(TicketMsg1, new Font("Seri", constants.fontSizeMedium, FontStyle.Bold), Brushes.Black, rect6, format6);
                currentY += 30;

                RectangleF rect7 = new RectangleF(0, currentY, constants.singleticketPrintPaperWidth, 30);
                StringFormat format7 = new StringFormat();
                format7.Alignment = StringAlignment.Center;
                e.Graphics.DrawString(TicketMsg2, new Font("Seri", constants.fontSizeMedium, FontStyle.Bold), Brushes.Black, rect7, format7);
                currentY += 30;

                RectangleF rect8 = new RectangleF(0, currentY, constants.singleticketPrintPaperWidth - 10, 30);
                StringFormat format8 = new StringFormat();
                format8.Alignment = StringAlignment.Far;
                e.Graphics.DrawString(currentTicketNo.ToString("0000000000"), new Font("Seri", constants.fontSizeMedium, FontStyle.Bold), Brushes.Black, rect8, format8);
                currentY += 30;

                lineNumbers++;
                if (lineNumbers == currentSelectedId)
                {
                    e.HasMorePages = false;
                }
                else
                {
                    e.HasMorePages = true;
                    return;
                }

            }
        }
        private void TicketMultiple_PrintPage(object sender, PrintPageEventArgs e)
        {
            DateTime now = DateTime.Now;
            PrintDocument ticketPrintDocument = (PrintDocument)sender;
            float currentY = 0;// declare  one variable for height measurement
            RectangleF rect1 = new RectangleF(5, currentY, constants.singleticketPrintPaperWidth - 5, 30);
            StringFormat format1 = new StringFormat();
            format1.Alignment = StringAlignment.Near;
            e.Graphics.DrawString(now.ToString("yyyy/MM/dd  HH:mm:ss"), new Font("Seri", constants.fontSizeMedium, FontStyle.Regular), Brushes.Red, rect1, format1);
            currentY += 20;
            if (SerialNo == 1)
            {
                RectangleF rect2 = new RectangleF(5, currentY, constants.singleticketPrintPaperWidth - 5, 30);
                StringFormat format2 = new StringFormat();
                format2.Alignment = StringAlignment.Near;
                e.Graphics.DrawString(currentSerialNo.ToString("0000"), new Font("Seri", constants.fontSizeMedium, FontStyle.Bold), Brushes.Red, rect2, format2);
                currentY += 20;
            }
            currentY += 10;
            int totalSum = 0;
            while (lineNumber < currentSelectedId)
            {
                int totalPrice = int.Parse(orderProductPriceArray[lineNumber]) * int.Parse(orderAmountArray[lineNumber]);
                totalSum += totalPrice;
                RectangleF rect3_1 = new RectangleF(10, currentY, constants.singleticketPrintPaperWidth * 3 / 5 - 20, 30);
                StringFormat format3_1 = new StringFormat();
                format3_1.Alignment = StringAlignment.Near;
                e.Graphics.DrawString(orderProductNameArray[lineNumber], new Font("Seri", constants.fontSizeMedium, FontStyle.Bold), Brushes.Blue, rect3_1, format3_1);

                RectangleF rect3_2 = new RectangleF(constants.singleticketPrintPaperWidth * 3 / 5 - 10, currentY, (constants.singleticketPrintPaperWidth - 20) / 5, 30);
                StringFormat format3_2 = new StringFormat();
                format3_2.Alignment = StringAlignment.Far;
                e.Graphics.DrawString("x" + orderAmountArray[lineNumber], new Font("Seri", constants.fontSizeMedium, FontStyle.Bold), Brushes.Blue, rect3_2, format3_2);

                RectangleF rect3_3 = new RectangleF(constants.singleticketPrintPaperWidth * 4 / 5 - 15, currentY, constants.singleticketPrintPaperWidth / 5 + 15, 30);
                StringFormat format3_3 = new StringFormat();
                format3_3.Alignment = StringAlignment.Far;
                e.Graphics.DrawString(totalPrice.ToString() + constants.unit, new Font("Seri", constants.fontSizeMedium, FontStyle.Bold), Brushes.Blue, rect3_3, format3_3);
                currentY += 20;

                RectangleF rect4 = new RectangleF(10, currentY, (constants.singleticketPrintPaperWidth - 10) * 3 / 5, 30);
                StringFormat format4 = new StringFormat();
                format4.Alignment = StringAlignment.Center;
                e.Graphics.DrawString(orderProductPriceArray[lineNumber] + constants.unit, new Font("Seri", constants.fontSizeMedium, FontStyle.Bold), Brushes.Blue, rect4, format4);
                currentY += 25;


                lineNumber++;
                e.HasMorePages = false;

            }
            currentY += 10;

            RectangleF rect4_1 = new RectangleF(5 + (constants.singleticketPrintPaperWidth - 10) * 2 / 5, currentY, (constants.singleticketPrintPaperWidth - 10) * 1 / 5, 30);
            StringFormat format4_1 = new StringFormat();
            format4_1.Alignment = StringAlignment.Center;
            e.Graphics.DrawString(constants.sumLabel, new Font("Seri", constants.fontSizeMedium, FontStyle.Bold), Brushes.Blue, rect4_1, format4_1);

            RectangleF rect4_2 = new RectangleF(5 + (constants.singleticketPrintPaperWidth - 10) * 3 / 5, currentY, (constants.singleticketPrintPaperWidth - 10) * 2 / 5, 30);
            StringFormat format4_2 = new StringFormat();
            format4_2.Alignment = StringAlignment.Far;
            e.Graphics.DrawString(totalSum.ToString() + constants.unit, new Font("Seri", constants.fontSizeMedium, FontStyle.Bold), Brushes.Blue, rect4_2, format4_2);
            currentY += 35;


            string validText = "当日限り有効";
            if (ValidDate != 0)
            {
                DateTime validDates = now.AddDays(2);
                validText = validDates.ToString("yyyy/MM/dd") + " まで有効";
            }
            RectangleF rect5 = new RectangleF(0, currentY, constants.singleticketPrintPaperWidth, 30);
            StringFormat format5 = new StringFormat();
            format5.Alignment = StringAlignment.Center;
            e.Graphics.DrawString(validText, new Font("Seri", constants.fontSizeMedium, FontStyle.Bold), Brushes.Blue, rect5, format5);
            currentY += 30;

            RectangleF rect6 = new RectangleF(0, currentY, constants.singleticketPrintPaperWidth, 30);
            StringFormat format6 = new StringFormat();
            format6.Alignment = StringAlignment.Center;
            e.Graphics.DrawString(TicketMsg1, new Font("Seri", constants.fontSizeMedium, FontStyle.Bold), Brushes.Blue, rect6, format6);
            currentY += 25;

            RectangleF rect7 = new RectangleF(0, currentY, constants.singleticketPrintPaperWidth, 30);
            StringFormat format7 = new StringFormat();
            format7.Alignment = StringAlignment.Center;
            e.Graphics.DrawString(TicketMsg2, new Font("Seri", constants.fontSizeMedium, FontStyle.Bold), Brushes.Blue, rect7, format7);
            currentY += 35;

            RectangleF rect8 = new RectangleF(0, currentY, constants.singleticketPrintPaperWidth - 5, 30);
            StringFormat format8 = new StringFormat();
            format8.Alignment = StringAlignment.Far;
            e.Graphics.DrawString(currentTicketNo.ToString("0000000000"), new Font("Seri", constants.fontSizeMedium, FontStyle.Regular), Brushes.Blue, rect8, format8);
            currentY += 35;
        }


        PrintQueue pq = LocalPrintServer.GetDefaultPrintQueue();

        string statusVal = null;
        string connectStatus = "";
        //int countNum = 0;
        private void EndPrintPage(object sender, PrintEventArgs e)
        {
            SpotTroubleUsingProperties(ref statusVal, pq);
            Console.WriteLine("status" + statusVal);

            //countNum++;
            //Console.WriteLine()
            ManagementScope scope = new ManagementScope(@"\root\cimv2");
            scope.Connect();

            // Select Printers from WMI Object Collections
            ManagementObjectSearcher searcher = new
             ManagementObjectSearcher("SELECT * FROM Win32_Printer");

            string printerName = "";
            foreach (ManagementObject printer in searcher.Get())
            {


                printerName = printer["Name"].ToString().ToLower();
                if (printerName.Equals(@"hwasung hmk-060"))
                {
                    /*foreach (PropertyData prop in printer.Properties)
                    {
                        Console.WriteLine("sss {0}: {1}", prop.Name, prop.Value);
                    }*/

                    Console.WriteLine("printer status--->" + printer["PrinterStatus"]);


                    Console.WriteLine("Printer = " + printer["Name"]);
                    if (printer["WorkOffline"].ToString().ToLower().Equals("true"))
                    {
                        // printer is offline by user
                        connectStatus = "disconnect";
                        Console.WriteLine("Your Plug-N-Play printer is not connected.");
                    }
                    else
                    {
                        // printer is not offline
                        connectStatus = "connect";
                        Console.WriteLine("Your Plug-N-Play printer is connected.");
                    }
                }
            }


            lineNumber = 0;


            if (ReceiptValid == "true")
            {
                receiptButtonGlobal.BackColor = Color.FromArgb(255, 255, 192, 0);
                receiptButtonGlobal.ForeColor = Color.White;
                receiptButtonGlobal.Enabled = true;
                Task.Delay(int.Parse(TicketTime) * 1000).ContinueWith(t => ReceiptButtonChange());
            }

            OrderDataSaving();

            if (sqlite_conn.State == ConnectionState.Closed)
            {
                sqlite_conn.Open();
            }
            SQLiteCommand sqlite_cmd;
            SQLiteDataReader sqlite_datareader;

            DateTime now = DateTime.Now;
            string week = now.ToString("ddd");
            string currentTime = now.ToString("HH:mm");
            for (int i = 0; i < categoryDisAmount; i++)
            {
                sqlite_cmd = sqlite_conn.CreateCommand();
                string queryCmd = "SELECT * FROM " + constants.tbNames[2] + " WHERE CategoryID=@CategoryID ORDER BY CardNumber";
                sqlite_cmd.CommandText = queryCmd;
                sqlite_cmd.Parameters.AddWithValue("@CategoryID", categoryIDArray[i]);

                sqlite_datareader = sqlite_cmd.ExecuteReader();
                int k = 0;
                while (sqlite_datareader.Read())
                {
                    string openTime = "";
                    if (week == "Sat")
                    {
                        openTime = sqlite_datareader.GetString(5);
                    }
                    else if (week == "Sun")
                    {
                        openTime = sqlite_datareader.GetString(6);
                    }
                    else
                    {
                        openTime = sqlite_datareader.GetString(4);
                    }
                    string[] openTimeArr = openTime.Split('/');
                    foreach (string openTimeArrItem in openTimeArr)
                    {
                        string[] openTimeSubArr = openTimeArrItem.Split('-');
                        if (String.Compare(openTimeSubArr[0], currentTime) <= 0 && String.Compare(openTimeSubArr[1], currentTime) >= 0)
                        {
                            SQLiteDataReader sqlite_datareader1;
                            SQLiteCommand sqlite_cmd1;
                            sqlite_cmd1 = sqlite_conn.CreateCommand();
                            string queryCmd1 = "SELECT SUM(prdAmount) as prdRestAmount FROM " + constants.tbNames[3] + " WHERE categoryID=@CategoryID and prdID=@prdID";
                            sqlite_cmd1.CommandText = queryCmd1;
                            sqlite_cmd1.Parameters.AddWithValue("@CategoryID", categoryIDArray[i]);
                            sqlite_cmd1.Parameters.AddWithValue("@prdID", sqlite_datareader.GetInt32(0));

                            sqlite_datareader1 = sqlite_cmd1.ExecuteReader();
                            while (sqlite_datareader1.Read())
                            {
                                if (!sqlite_datareader1.IsDBNull(0))
                                {
                                    productRestAmountArray[i, k] = productLimitedCntArray[i, k] - sqlite_datareader1.GetInt32(0);
                                }
                                else
                                {
                                    productRestAmountArray[i, k] = productLimitedCntArray[i, k];
                                }
                            }

                            break;
                        }
                    }
                    k++;
                }
            }
            orderCategoryIDArray = new int[100];
            orderProductIDArray = new int[100];
            orderProductNameArray = new string[100];
            orderProductPriceArray = new string[100];
            orderAmountArray = new string[100];
            orderProuctIndexArray = new int[100];
            TempLabel = orderPriceEnterLabel;
            SetText("");
            TempLabel = orderPriceTotalLabel;
            SetText("");
            TempLabel = orderRestPriceLabel;
            SetText("");
            //orderPriceTotalLabel.Text = "";
            orderTotalPrice = 0;
            currentSelectedId = 0;
            CreateOrderTable(0);

            sqlite_conn.Close();


        }
        private void ReceiptRun(object sender, EventArgs e)
        {
            if (sqlite_conn.State == ConnectionState.Closed)
            {
                sqlite_conn.Open();
            }
            SQLiteCommand sqlite_cmd;
            string Createsql = "CREATE TABLE IF NOT EXISTS ReceiptTB (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, PurchasePoint INTEGER NOT NULL, TotalPrice INTEGER NOT NULL DEFAULT 0, ReceiptDate DATETIME)";
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = Createsql;
            sqlite_cmd.ExecuteNonQuery();
            DateTime now = DateTime.Now;
            string orderRun = "INSERT INTO ReceiptTB (PurchasePoint, TotalPrice, ReceiptDate) VALUES (@purchasePoint, @totalPrice, @receiptDate)";
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = orderRun;
            sqlite_cmd.Parameters.AddWithValue("@purchasePoint", orderTotalTicketForReceipt);
            sqlite_cmd.Parameters.AddWithValue("@totalPrice", orderTotalPriceForReceipt);
            sqlite_cmd.Parameters.AddWithValue("@receiptDate", now);
            sqlite_cmd.ExecuteNonQuery();
            sqlite_conn.Close();

            PrintDocument receiptPrintDocument = new PrintDocument();
            PrintPreviewDialog receiptPrintPreviewDialog = new PrintPreviewDialog();
            PrintDialog receiptPrintDialog = new PrintDialog();
            paperSize = new PaperSize("papersize", constants.receiptPrintPaperWidth, constants.receiptPrintPaperHeight);
            receiptPrintDocument.PrintPage += new PrintPageEventHandler(Receipt_PrintPage);

            receiptPrintPreviewDialog.Document = receiptPrintDocument;
            receiptPrintDialog.Document = receiptPrintDocument;

            PrinterSettings settings = new PrinterSettings();
            string defaultPrinterName = settings.PrinterName;
            //MessageBox.Show(defaultPrinterName);

            //((ToolStripButton)((ToolStrip)receiptPrintPreviewDialog.Controls[1]).Items[0]).Enabled = true;

            receiptPrintDocument.DefaultPageSettings.PaperSize = paperSize;
            receiptPrintDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 50);
            //receiptPrintPreviewDialog.ShowDialog();
            //if(receiptPrintDialog.ShowDialog() == DialogResult.OK)
            //{
            receiptPrintDocument.Print();
            //}

            receiptButtonGlobal.BackColor = Color.FromArgb(255, 217, 217, 217);
            receiptButtonGlobal.ForeColor = Color.Black;
            receiptButtonGlobal.Enabled = false;
        }


        private void ReceiptButtonChange()
        {
            if (ReceiptValid == "true")
            {

                Thread.Sleep(int.Parse(TicketTime) * 1000);
                if (receiptButtonGlobal.InvokeRequired)
                {
                    receiptButtonGlobal.Invoke(new MethodInvoker(delegate
                    {
                        receiptButtonGlobal.BackColor = Color.FromArgb(255, 217, 217, 217);
                        receiptButtonGlobal.ForeColor = Color.Black;
                        receiptButtonGlobal.Enabled = false;
                    }));
                }
            }
        }

        private void Receipt_PrintPage(object sender, PrintPageEventArgs e)
        {
            DateTime now = DateTime.Now;
            float currentY = 0;// declare  one variable for height measurement
            RectangleF rect1 = new RectangleF(0, currentY, constants.receiptPrintPaperWidth, 30);
            StringFormat format1 = new StringFormat();
            format1.Alignment = StringAlignment.Center;
            e.Graphics.DrawString(constants.receiptButtonText, new Font("Seri", constants.fontSizeBig, FontStyle.Regular), Brushes.Black, rect1, format1);
            currentY += 30;

            RectangleF rect2 = new RectangleF(0, currentY, constants.receiptPrintPaperWidth, 30);
            StringFormat format2 = new StringFormat();
            format2.Alignment = StringAlignment.Center;
            e.Graphics.DrawString(now.ToString("yyyy/MM/dd  HH:mm:ss"), new Font("Seri", constants.fontSizeMedium, FontStyle.Regular), Brushes.Black, rect2, format2);
            currentY += 30;

            RectangleF rect3 = new RectangleF(0, currentY, constants.receiptPrintPaperWidth, 30);
            StringFormat format3 = new StringFormat();
            format3.Alignment = StringAlignment.Center;
            e.Graphics.DrawString(orderTotalTicketForReceipt.ToString() + constants.amountUnit1 + "  " + constants.sumLabel + "  ¥" + orderTotalPriceForReceipt.ToString(), new Font("Seri", constants.fontSizeMedium, FontStyle.Regular), Brushes.Black, rect3, format3);
            currentY += 30;

            RectangleF rect4 = new RectangleF(0, currentY, constants.receiptPrintPaperWidth, 30);
            StringFormat format4 = new StringFormat();
            format4.Alignment = StringAlignment.Center;
            e.Graphics.DrawString(constants.receiptInstruction, new Font("Seri", constants.fontSizeMedium, FontStyle.Regular), Brushes.Black, rect4, format4);
            currentY += 35;

            RectangleF rect5 = new RectangleF(0, currentY, constants.receiptPrintPaperWidth, 30);
            StringFormat format5 = new StringFormat();
            format5.Alignment = StringAlignment.Center;
            e.Graphics.DrawString(StoreName, new Font("Seri", constants.fontSizeMedium, FontStyle.Regular), Brushes.Black, rect5, format5);
            currentY += 25;

            RectangleF rect6 = new RectangleF(0, currentY, constants.receiptPrintPaperWidth, 30);
            StringFormat format6 = new StringFormat();
            format6.Alignment = StringAlignment.Center;
            e.Graphics.DrawString(Address, new Font("Seri", constants.fontSizeMedium, FontStyle.Regular), Brushes.Black, rect6, format6);
            currentY += 25;

            RectangleF rect7 = new RectangleF(0, currentY, constants.receiptPrintPaperWidth, 30);
            StringFormat format7 = new StringFormat();
            format7.Alignment = StringAlignment.Center;
            e.Graphics.DrawString(PhoneNumber, new Font("Seri", constants.fontSizeMedium, FontStyle.Regular), Brushes.Black, rect7, format7);
            currentY += 35;

            RectangleF rect8 = new RectangleF(0, currentY, constants.receiptPrintPaperWidth, 30);
            StringFormat format8 = new StringFormat();
            format8.Alignment = StringAlignment.Center;
            e.Graphics.DrawString(Other1, new Font("Seri", constants.fontSizeSmall, FontStyle.Regular), Brushes.Black, rect8, format8);
            currentY += 20;

            RectangleF rect9 = new RectangleF(0, currentY, constants.receiptPrintPaperWidth, 30);
            StringFormat format9 = new StringFormat();
            format9.Alignment = StringAlignment.Center;
            e.Graphics.DrawString(Other2, new Font("Seri", constants.fontSizeSmall, FontStyle.Regular), Brushes.Black, rect9, format9);
            currentY += 35;

            e.HasMorePages = false;

        }
        public void BackShow()
        {
            mainPanelGlobal = createPanel.CreateMainPanel(mainFormGlobal, 0, 0, mainFormGlobal.Width, mainFormGlobal.Height, BorderStyle.None, Color.Transparent);
            mainFormGlobal.Controls.Clear();
            MainMenu mainMenu = new MainMenu();
            mainMenu.CreateMainMenuScreen(mainFormGlobal, mainPanelGlobal);
        }


        

        private void BackShowMainMenu(object sender, EventArgs e)
        {
            Button temp = (Button)sender;
            passwordInput.CreateNumberInputDialog("salescreen", temp.Name);
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
                this.BackShow();
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
        static void InitIntArray(int[,] arr)
        {
            for(int k = 0; k < arr.GetLength(0); k++)
            {
                for (int i = 0; i < arr.GetLength(1); i++)
                {
                    arr[k, i] = -1;
                }
            }
        }
    }
}

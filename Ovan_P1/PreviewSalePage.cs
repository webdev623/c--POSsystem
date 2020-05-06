using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
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
        Panel LeftPanelGlobal = null;
        Panel MainBodyPanelGlobal = null;
        Constant constants = new Constant();
        CustomButton customButton = new CustomButton();

        CreatePanel createPanel = new CreatePanel();
        CreateLabel createLabel = new CreateLabel();
        OrderDialog orderDialog = new OrderDialog();

        int categoryIDGlobal = 0;
        string[] categoryNameList = null;
        int[] categoryIDList = null;
        int[] categoryDisplayPositionList = null;
        int[] categoryLayoutList = null;
        string[] categoryBackImageList = null;

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
        int colorPatternValue = 0;
        string menuTitle1 = "";
        string menuTitle2 = "";


        int height = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height;
        int width = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width;

        SQLiteConnection sqlite_conn;

        public PreviewSalePage(Form1 mainForm, Panel mainPanel, int categoryIndex, int[] categoryIDArray, string[] categoryNameArray, int[] categoryDisplayPositionArray, int[] categoryLayoutArray, string[] categoryBackImageArray)
        {
            InitializeComponent();

            categoryIDGlobal = categoryIndex;
            categoryIDList = categoryIDArray;
            categoryNameList = categoryNameArray;
            categoryDisplayPositionList = categoryDisplayPositionArray;
            categoryLayoutList = categoryLayoutArray;
            categoryBackImageList = categoryBackImageArray;

            sqlite_conn = CreateConnection(constants.dbName);

            mainFormGlobal = mainForm;
            mainPanelGlobal = mainPanel;

            Panel LeftPanel = createPanel.CreateMainPanel(mainForm, 0, 0, 3 * width / 4, height, BorderStyle.FixedSingle, Color.FromArgb(255, 255, 255, 204));
            LeftPanelGlobal = LeftPanel;


            sqlite_conn = CreateConnection(constants.dbName);
            if (sqlite_conn.State == ConnectionState.Closed)
            {
                sqlite_conn.Open();
            }
            SQLiteCommand sqlite_cmd;
            SQLiteDataReader sqlite_datareader;
            try
            {
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
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Color[][] colorPattern = constants.pattern_Clr;
            colorPattn = colorPattern[colorPatternValue];
            if(categoryBackImageList[categoryIDGlobal] != "" && categoryBackImageList[categoryIDGlobal] != null)
            {
                LeftPanelGlobal.BackgroundImage = Image.FromFile(categoryBackImageList[categoryIDGlobal]);
                LeftPanelGlobal.BackgroundImageLayout = ImageLayout.Stretch;
            }
            else
            {
                LeftPanelGlobal.BackColor = colorPattn[categoryIDGlobal % 4 + 4];
            }


            Panel RightPanel = createPanel.CreateMainPanel(mainForm, width * 3 / 4, 0, width / 4, height, BorderStyle.FixedSingle, Color.White);
            FlowLayoutPanel FlowButtonLayout = createPanel.CreateFlowLayoutPanel(LeftPanel, 0, height / 7, LeftPanel.Width / 6, height * 4 / 7, Color.Transparent, new Padding(20, 10, 0, 0));
            FlowLayoutPanel FlowTitleLayout = createPanel.CreateFlowLayoutPanel(LeftPanel, LeftPanel.Width / 6, 0, (LeftPanel.Width * 5) / 6, height / 7, Color.Transparent, new Padding(10, 70, 0, 0));
            Panel MenuBodyLayout = createPanel.CreateSubPanel(LeftPanel, LeftPanel.Width / 6, height / 7, (LeftPanel.Width * 5) / 6, height * 6 / 7, BorderStyle.FixedSingle, Color.Transparent);

            MainBodyPanelGlobal = MenuBodyLayout;

            /**  Top Screen Title */


            Label MainTitle = new Label();
            //  MainTitle.Location = new Point(FlowTitleLayout.Left+200, FlowTitleLayout.Height/2-24);
            MainTitle.Width = FlowTitleLayout.Width;
            MainTitle.Height = FlowTitleLayout.Height - 70;
            MainTitle.Font = new Font("Seri", 24, FontStyle.Bold);
            MainTitle.ForeColor = Color.FromArgb(255, 255, 0, 0);
            MainTitle.Text = menuTitle1 + "\n" + menuTitle2;
            //LeftPanel.Controls.Remove(FlowTitleLayout);
            FlowTitleLayout.Controls.Add(MainTitle);

            /** Left category menu button create */

            Color[] saleCategoryButtonColor = constants.getSaleCategoryButtonColor();
            Color[] saleCategoryButtonBorderColor = constants.getSaleCategoryButtonBorderColor();

            CreateCategoryList(FlowButtonLayout);
            if (categoryLayoutList[categoryIDGlobal] == 13)
            {
                CreateProductsList13();
            }
            else if (categoryLayoutList[categoryIDGlobal] == 11)
            {
                CreateProductsList11();
            }
            else
            {
                CreateProductsList();
            }

            /** Main Product Panel layout */

            /** right panel  */
            RightPanel.Padding = new Padding(10, 0, 10, 0);
         
            Button closeButton = customButton.CreateButton(constants.backText, "closeButton", RightPanel.Width / 2 - 100, RightPanel.Height * 6 / 7, 200, 50, Color.FromArgb(255, 0, 112, 192), Color.Transparent, 0, 1, 18);
            RightPanel.Controls.Add(closeButton);
            closeButton.Click += new EventHandler(this.BackShow);

        }

        Color[] colorPattn = new Color[8];

        private void CreateCategoryList(FlowLayoutPanel listPanel)
        {
            DateTime now = DateTime.Now;
            string week = now.ToString("ddd");
            string currentTime = now.ToString("HH:mm");
            Color[][] colorPattern = constants.pattern_Clr;
            colorPattn = colorPattern[colorPatternValue];
            int categoryAmount = categoryNameList.Length;
            int k = 0;
            foreach(string categoryName in categoryNameList)
            {
                Color backColor = colorPattn[k % 4 + 4];
                Color borderColor = colorPattn[k % 4];

                if (categoryIDGlobal == k)
                {
                    backColor = colorPattn[k % 4];
                    borderColor = Color.Red;
                    int btnLeft = listPanel.Left + 10;
                    int btnTop = (listPanel.Top + 10) + (listPanel.Height / 5) * k;
                    int btnWidth = listPanel.Width - 25;
                    int btnHeight = listPanel.Height / categoryAmount - 10;
                    if (categoryAmount < 6)
                    {
                        btnHeight = listPanel.Height / 6 - 10;
                    }

                    Button_WOC btn = new Button_WOC();
                    btn.Location = new Point(btnLeft, btnTop);
                    btn.Size = new Size(btnWidth, btnHeight);
                    btn.Text = categoryName;

                    btn.Name = "category_" + k;
                    btn.BackColor = Color.Transparent;  // colorPattn[categoryIDGlobal % 4 + 4];
                    btn.ButtonColor = backColor;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.OnHoverButtonColor = colorPattn[k % 4 + 4];
                    btn.OnHoverBorderColor = borderColor;
                    btn.BorderColor = borderColor;
                    btn.Font = new Font("Seri", 18F, FontStyle.Bold);
                    listPanel.Controls.Add(btn);
                    btn.Invalidate();
                }
                else if(categoryDisplayPositionList[k] != categoryDisplayPositionList[categoryIDGlobal])
                {
                    if(k == 0)
                    {
                        int btnLeft = listPanel.Left + 10;
                        int btnTop = (listPanel.Top + 10) + (listPanel.Height / 5) * k;
                        int btnWidth = listPanel.Width - 25;
                        int btnHeight = listPanel.Height / categoryAmount - 10;
                        if (categoryAmount < 6)
                        {
                            btnHeight = listPanel.Height / 6 - 10;
                        }

                        Button_WOC btn = new Button_WOC();
                        btn.Location = new Point(btnLeft, btnTop);
                        btn.Size = new Size(btnWidth, btnHeight);
                        btn.Text = categoryName;

                        btn.Name = "category_" + k;
                        btn.BackColor = Color.Transparent; // colorPattn[categoryIDGlobal % 4 + 4];
                        btn.ButtonColor = backColor;
                        btn.FlatAppearance.BorderSize = 0;
                        btn.FlatStyle = FlatStyle.Flat;
                        btn.OnHoverButtonColor = colorPattn[k % 4 + 4];
                        btn.OnHoverBorderColor = borderColor;
                        btn.BorderColor = borderColor;
                        btn.Font = new Font("Seri", 18F, FontStyle.Bold);
                        listPanel.Controls.Add(btn);
                        btn.Invalidate();
                    }
                    else if(categoryDisplayPositionList[k-1] != categoryDisplayPositionList[k])
                    {
                        int btnLeft = listPanel.Left + 10;
                        int btnTop = (listPanel.Top + 10) + (listPanel.Height / 5) * k;
                        int btnWidth = listPanel.Width - 25;
                        int btnHeight = listPanel.Height / categoryAmount - 10;
                        if (categoryAmount < 6)
                        {
                            btnHeight = listPanel.Height / 6 - 10;
                        }

                        Button_WOC btn = new Button_WOC();
                        btn.Location = new Point(btnLeft, btnTop);
                        btn.Size = new Size(btnWidth, btnHeight);
                        btn.Text = categoryName;

                        btn.Name = "category_" + k;
                        btn.BackColor = Color.Transparent; // colorPattn[categoryIDGlobal % 4 + 4];
                        btn.ButtonColor = backColor;
                        btn.FlatAppearance.BorderSize = 0;
                        btn.FlatStyle = FlatStyle.Flat;
                        btn.OnHoverButtonColor = colorPattn[k % 4 + 4];
                        btn.OnHoverBorderColor = borderColor;
                        btn.BorderColor = borderColor;
                        btn.Font = new Font("Seri", 18F, FontStyle.Bold);
                        listPanel.Controls.Add(btn);
                        btn.Invalidate();
                    }
                }

                k++;


            }

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

            p_ProductCon = new Panel[categoryLayoutList[categoryIDGlobal]];
            pb_Image = new PictureBox[categoryLayoutList[categoryIDGlobal]];
            bl_Name = new BorderLabel[categoryLayoutList[categoryIDGlobal]];
            bl_Price = new BorderLabel[categoryLayoutList[categoryIDGlobal]];


            for (int i = 0; i < categoryLayoutList[categoryIDGlobal]; i++)
            {
                string bgColor = "ffffff00";
                string foreColor = "ff000000";
                string borderColor = "ff223300";
                int prdID = 0;
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
                int prdRestAmount = 0;

                if (sqlite_conn.State == ConnectionState.Closed)
                {
                    sqlite_conn.Open();
                }
                SQLiteDataReader sqlite_datareader;
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = sqlite_conn.CreateCommand();
                string queryCmd = "SELECT * FROM " + constants.tbNames[2] + " WHERE CategoryID=@CategoryID and CardNumber=@CardNumber ORDER BY CardNumber";
                sqlite_cmd.CommandText = queryCmd;
                sqlite_cmd.Parameters.AddWithValue("@CategoryID", categoryIDList[categoryIDGlobal]);
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
                            prdID = sqlite_datareader.GetInt32(0);
                            prdName = sqlite_datareader.GetString(3);
                            prdPrice = sqlite_datareader.GetInt32(8);
                            prdLimitedCnt = sqlite_datareader.GetInt32(9);
                            prdImageUrl = sqlite_datareader.GetString(11);
                            prdImageX = sqlite_datareader.GetInt32(15);
                            prdImageY = sqlite_datareader.GetInt32(16);
                            prdImageWidth = sqlite_datareader.GetInt32(17);
                            prdImageHeight = sqlite_datareader.GetInt32(18);
                            prdBadgeX = sqlite_datareader.GetInt32(19);
                            prdBadgeY = sqlite_datareader.GetInt32(20);
                            prdBadgeWidth = sqlite_datareader.GetInt32(21);
                            prdBadgeHeight = sqlite_datareader.GetInt32(22);
                            prdBadgeUrl = sqlite_datareader.GetString(23);
                            prdNameX = sqlite_datareader.GetInt32(24);
                            prdNameY = sqlite_datareader.GetInt32(25);
                            prdPriceX = sqlite_datareader.GetInt32(26);
                            prdPriceY = sqlite_datareader.GetInt32(27);
                            bgColor = sqlite_datareader.GetString(28);
                            foreColor = sqlite_datareader.GetString(29);
                            borderColor = sqlite_datareader.GetString(30);
                            soldFlag = sqlite_datareader.GetInt32(31);
                            SQLiteDataReader sqlite_datareader1;
                            SQLiteCommand sqlite_cmd1;
                            sqlite_cmd1 = sqlite_conn.CreateCommand();
                            string queryCmd1 = "SELECT SUM(prdAmount) as prdRestAmount FROM " + constants.tbNames[3] + " WHERE categoryID=@CategoryID and prdID=@prdID";
                            sqlite_cmd1.CommandText = queryCmd1;
                            sqlite_cmd1.Parameters.AddWithValue("@CategoryID", categoryIDList[categoryIDGlobal]);
                            sqlite_cmd1.Parameters.AddWithValue("@prdID", prdID);

                            sqlite_datareader1 = sqlite_cmd1.ExecuteReader();
                            while (sqlite_datareader1.Read())
                            {
                                if (prdLimitedCnt != 0)
                                {
                                    if (!sqlite_datareader1.IsDBNull(0))
                                    {
                                        prdRestAmount = prdLimitedCnt - sqlite_datareader1.GetInt32(0);
                                    }
                                    else
                                    {
                                        prdRestAmount = prdLimitedCnt;
                                    }
                                }
                                else
                                {
                                    prdRestAmount = 10000;
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
                if (prdImageUrl != "" && prdImageUrl != null)
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

                if (soldFlag == 1 || (prdLimitedCnt != 0 && prdRestAmount == 0))
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

            p_ProductCon = new Panel[categoryLayoutList[categoryIDGlobal]];
            pb_Image = new PictureBox[categoryLayoutList[categoryIDGlobal]];
            bl_Name = new BorderLabel[categoryLayoutList[categoryIDGlobal]];
            bl_Price = new BorderLabel[categoryLayoutList[categoryIDGlobal]];


            for (int i = 0; i < categoryLayoutList[categoryIDGlobal]; i++)
            {
                string bgColor = "ffffff00";
                string foreColor = "ff000000";
                string borderColor = "ff223300";
                int prdID = 0;
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
                int prdRestAmount = 0;

                if (sqlite_conn.State == ConnectionState.Closed)
                {
                    sqlite_conn.Open();
                }
                SQLiteDataReader sqlite_datareader;
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = sqlite_conn.CreateCommand();
                string queryCmd = "SELECT * FROM " + constants.tbNames[2] + " WHERE CategoryID=@CategoryID and CardNumber=@CardNumber ORDER BY CardNumber";
                sqlite_cmd.CommandText = queryCmd;
                sqlite_cmd.Parameters.AddWithValue("@CategoryID", categoryIDList[categoryIDGlobal]);
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
                            prdID = sqlite_datareader.GetInt32(0);
                            prdName = sqlite_datareader.GetString(3);
                            prdPrice = sqlite_datareader.GetInt32(8);
                            prdLimitedCnt = sqlite_datareader.GetInt32(9);
                            prdImageUrl = sqlite_datareader.GetString(11);
                            prdImageX = sqlite_datareader.GetInt32(15);
                            prdImageY = sqlite_datareader.GetInt32(16);
                            prdImageWidth = sqlite_datareader.GetInt32(17);
                            prdImageHeight = sqlite_datareader.GetInt32(18);
                            prdBadgeX = sqlite_datareader.GetInt32(19);
                            prdBadgeY = sqlite_datareader.GetInt32(20);
                            prdBadgeWidth = sqlite_datareader.GetInt32(21);
                            prdBadgeHeight = sqlite_datareader.GetInt32(22);
                            prdBadgeUrl = sqlite_datareader.GetString(23);
                            prdNameX = sqlite_datareader.GetInt32(24);
                            prdNameY = sqlite_datareader.GetInt32(25);
                            prdPriceX = sqlite_datareader.GetInt32(26);
                            prdPriceY = sqlite_datareader.GetInt32(27);
                            bgColor = sqlite_datareader.GetString(28);
                            foreColor = sqlite_datareader.GetString(29);
                            borderColor = sqlite_datareader.GetString(30);
                            soldFlag = sqlite_datareader.GetInt32(31);

                            SQLiteDataReader sqlite_datareader1;
                            SQLiteCommand sqlite_cmd1;
                            sqlite_cmd1 = sqlite_conn.CreateCommand();
                            string queryCmd1 = "SELECT SUM(prdAmount) as prdRestAmount FROM " + constants.tbNames[3] + " WHERE categoryID=@CategoryID and prdID=@prdID";
                            sqlite_cmd1.CommandText = queryCmd1;
                            sqlite_cmd1.Parameters.AddWithValue("@CategoryID", categoryIDList[categoryIDGlobal]);
                            sqlite_cmd1.Parameters.AddWithValue("@prdID", prdID);

                            sqlite_datareader1 = sqlite_cmd1.ExecuteReader();
                            while (sqlite_datareader1.Read())
                            {
                                if (prdLimitedCnt != 0)
                                {
                                    if (!sqlite_datareader1.IsDBNull(0))
                                    {
                                        prdRestAmount = prdLimitedCnt - sqlite_datareader1.GetInt32(0);
                                    }
                                    else
                                    {
                                        prdRestAmount = prdLimitedCnt;
                                    }
                                }
                                else
                                {
                                    prdRestAmount = 1000;
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
                if (prdImageUrl != "" && prdImageUrl != null)
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

                if (soldFlag == 1 || (prdLimitedCnt != 0 && prdRestAmount == 0))
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
            if (categoryLayoutList[categoryIDGlobal] == 25 || categoryLayoutList[categoryIDGlobal] == 16 || categoryLayoutList[categoryIDGlobal] == 9 || categoryLayoutList[categoryIDGlobal] == 4)
                nWD = nHD = (int)Math.Sqrt((double)categoryLayoutList[categoryIDGlobal]);
            if (categoryLayoutList[categoryIDGlobal] == 10) { nWD = 2; nHD = 5; }
            if (categoryLayoutList[categoryIDGlobal] == 6) { nWD = 3; nHD = 2; }
            if (categoryLayoutList[categoryIDGlobal] == 8) { nWD = 4; nHD = 2; }
            if (categoryLayoutList[categoryIDGlobal] == 20) { nWD = 5; nHD = 4; }

            int w1 = (MainBodyPanelGlobal.Width - 20 * nWD) / nWD;
            int h1 = (MainBodyPanelGlobal.Height - 20 * (nHD - 1)) / nHD;
            if (categoryLayoutList[categoryIDGlobal] == 10)
                w1 = (MainBodyPanelGlobal.Width - 50 * nWD) / nWD;

            nHeight1 = h1;
            nWidth1 = w1;
            DateTime now = DateTime.Now;
            string week = now.ToString("ddd");
            string currentTime = now.ToString("HH:mm");

            p_ProductCon = new Panel[categoryLayoutList[categoryIDGlobal]];
            pb_Image = new PictureBox[categoryLayoutList[categoryIDGlobal]];
            bl_Name = new BorderLabel[categoryLayoutList[categoryIDGlobal]];
            bl_Price = new BorderLabel[categoryLayoutList[categoryIDGlobal]];

            for (int i = 0; i < categoryLayoutList[categoryIDGlobal]; i++)
            {
                string bgColor = "ffffff00";
                string foreColor = "ff000000";
                string borderColor = "ff223300";
                int prdID = 0;
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
                int prdRestAmount = 0;

                if (sqlite_conn.State == ConnectionState.Closed)
                {
                    sqlite_conn.Open();
                }
                SQLiteDataReader sqlite_datareader;
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = sqlite_conn.CreateCommand();
                string queryCmd = "SELECT * FROM " + constants.tbNames[2] + " WHERE CategoryID=@CategoryID and CardNumber=@CardNumber ORDER BY CardNumber";
                sqlite_cmd.CommandText = queryCmd;
                sqlite_cmd.Parameters.AddWithValue("@CategoryID", categoryIDList[categoryIDGlobal]);
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
                            prdID = sqlite_datareader.GetInt32(0);
                            prdName = sqlite_datareader.GetString(3);
                            prdPrice = sqlite_datareader.GetInt32(8);
                            prdLimitedCnt = sqlite_datareader.GetInt32(9);
                            prdImageUrl = sqlite_datareader.GetString(11);
                            prdImageX = sqlite_datareader.GetInt32(15);
                            prdImageY = sqlite_datareader.GetInt32(16);
                            prdImageWidth = sqlite_datareader.GetInt32(17);
                            prdImageHeight = sqlite_datareader.GetInt32(18);
                            prdBadgeX = sqlite_datareader.GetInt32(19);
                            prdBadgeY = sqlite_datareader.GetInt32(20);
                            prdBadgeWidth = sqlite_datareader.GetInt32(21);
                            prdBadgeHeight = sqlite_datareader.GetInt32(22);
                            prdBadgeUrl = sqlite_datareader.GetString(23);
                            prdNameX = sqlite_datareader.GetInt32(24);
                            prdNameY = sqlite_datareader.GetInt32(25);
                            prdPriceX = sqlite_datareader.GetInt32(26);
                            prdPriceY = sqlite_datareader.GetInt32(27);
                            bgColor = sqlite_datareader.GetString(28);
                            foreColor = sqlite_datareader.GetString(29);
                            borderColor = sqlite_datareader.GetString(30);
                            soldFlag = sqlite_datareader.GetInt32(31);
                            SQLiteDataReader sqlite_datareader1;
                            SQLiteCommand sqlite_cmd1;
                            sqlite_cmd1 = sqlite_conn.CreateCommand();
                            string queryCmd1 = "SELECT SUM(prdAmount) as prdRestAmount FROM " + constants.tbNames[3] + " WHERE categoryID=@CategoryID and prdID=@prdID";
                            sqlite_cmd1.CommandText = queryCmd1;
                            sqlite_cmd1.Parameters.AddWithValue("@CategoryID", categoryIDList[categoryIDGlobal]);
                            sqlite_cmd1.Parameters.AddWithValue("@prdID", prdID);

                            sqlite_datareader1 = sqlite_cmd1.ExecuteReader();
                            while (sqlite_datareader1.Read())
                            {
                                if (prdLimitedCnt != 0)
                                {
                                    if (!sqlite_datareader1.IsDBNull(0))
                                    {
                                        prdRestAmount = prdLimitedCnt - sqlite_datareader1.GetInt32(0);
                                    }
                                    else
                                    {
                                        prdRestAmount = prdLimitedCnt;
                                    }
                                }
                                else
                                {
                                    prdRestAmount = 1000;
                                }
                            }

                            break;
                        }
                    }

                }


                int x = i % nWD;
                int yy = i / nWD;
                int d = 20;
                if (categoryDisplayPositionList[categoryIDGlobal] == 10) d = 50;
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
                if (prdImageUrl != "" && prdImageUrl != null)
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

                if (soldFlag == 1 || (prdLimitedCnt != 0 && prdRestAmount == 0))
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

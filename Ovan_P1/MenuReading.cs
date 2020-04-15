using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace Ovan_P1
{
    public partial class MenuReading : Form
    {
        int height = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height;
        int width = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width;
        Form1 mainFormGlobal = null;
        Panel mainPanelGlobal = null;
        //private Button buttonGlobal = null;
        private FlowLayoutPanel[] menuFlowLayoutPanelGlobal = new FlowLayoutPanel[4];
        Constant constants = new Constant();
        MessageDialog messageDialog = new MessageDialog();
        SQLiteConnection sqlite_conn;
        public MenuReading(Form1 mainForm, Panel mainPanel)
        {
            InitializeComponent();
            messageDialog.initMenuReading(this);
            sqlite_conn = CreateConnection(constants.dbName);
            //CreateProduct(sqlite_conn);
            //int m = 0;
            //foreach (string categories in constants.saleCategories)
            //{
            //    int k = 1;
            //    foreach (string prods in constants.productBigName[m])
            //    {
            //        InsertProductData(sqlite_conn, m + 1, m + 1, prods, prods, constants.productBigPrice[m][k - 1], int.Parse(constants.productBigSaleAmount[m][k - 1]), constants.productBigImageUrl[k - 1], "", "", k, 20 * k, 20 * m, 120, 140, 20 * k, 20 * m, 120, 140, constants.productBigBadgeImageUrl[k - 1], new Point(2 * k, 3 * m), new Point(3 * k, 2 * m), "09:00-11:59/18:00-23:59", "09:00-20:59", "09:00-20:59", Color.Yellow, Color.FromArgb(255, 255, 0, 0), Color.FromArgb(255, 192, 81, 0));
            //        k++;
            //    }
            //    m++;
            //}

            mainForm.Width = width;
            mainForm.Height = height;
            mainFormGlobal = mainForm;
            mainPanelGlobal = mainPanel;
            headerPanel.Size = new Size(mainForm.Width, mainForm.Height / 5);
            bodyPanel.Size = new Size(mainForm.Width, mainForm.Height * 4 / 5);
            mainForm.Controls.Add(headerPanel);
            mainForm.Controls.Add(bodyPanel);
            menuReadingTitle.Text = constants.menuReadingTitle;

            subContent1.Location = new Point(bodyPanel.Width / 5, 50);
            subContent1.Size = new Size(bodyPanel.Width * 3 / 5, bodyPanel.Height / 8);
            subContent1.Text = constants.menuReadingSubContent1;

            subContent2.Location = new Point(bodyPanel.Width / 5, subContent1.Bottom);
            subContent2.Size = new Size(bodyPanel.Width * 3 / 5, bodyPanel.Height / 8);
            subContent2.Text = constants.menuReadingSubContent2;

            readButton.Location = new Point(bodyPanel.Width / 2 - 100, subContent2.Bottom + 50);
            readButton.Size = new Size(200, 50);
            readButton.Text = constants.menuReadingButton;
            readButton.radiusValue = 20;
            readButton.Click += new EventHandler(this.OpenFileDialog);

            cancelButton.radiusValue = 20;
            cancelButton.Location = new Point(bodyPanel.Width - 150, bodyPanel.Height - 100);
            cancelButton.Size = new Size(100, 50);
            cancelButton.Text = constants.cancelButtonText;
            cancelButton.Click += new EventHandler(this.BackShow);

        }


        private void MenuReading_Load(object sender, EventArgs e)
        {
            InitializeComponent();
        }

        private void OpenFileDialog(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if(sqlite_conn.State == ConnectionState.Open)
                {
                    sqlite_conn.Close();
                }
                try
                {
                    var filePath = openFileDialog1.FileName;
                    var fileName = openFileDialog1.SafeFileName;
                    var curDir = Directory.GetCurrentDirectory();
                    var dbName = fileName.Split('.')[0];
                    Thread.Sleep(100);
                    RestoreDB(curDir, filePath, fileName, true);
                    DBCopy(sqlite_conn, Path.Combine(curDir, fileName), dbName, constants.tbNames);
                    mainFormGlobal.Controls.Clear();
                    MainMenu mainMenu = new MainMenu();
                    mainMenu.CreateMainMenuScreen(mainFormGlobal, mainPanelGlobal);
                }
                catch (Exception ex)
                {
                    messageDialog.ShowMenuReadingMessage();
                }
            }
        }

        public void BackShow(object sender, EventArgs e)
        {
            mainFormGlobal.Controls.Clear();
            MainMenu mainMenu = new MainMenu();
            mainMenu.CreateMainMenuScreen(mainFormGlobal, mainPanelGlobal);
        }

        public void BackShowStart()
        {
            mainFormGlobal.Controls.Clear();
            MainMenu mainMenu = new MainMenu();
            mainMenu.CreateMainMenuScreen(mainFormGlobal, mainPanelGlobal);
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

            }
            return sqlite_conn;
        }
        private static void RestoreDB(string filePath, string srcFilename, string destFileName, bool IsCopy = false)
        {
            var srcfile = srcFilename;
            var destfile = Path.Combine(filePath, destFileName);

            if (File.Exists(destfile)) File.Delete(destfile);

            if (IsCopy)
                BackupDB(filePath, srcFilename, destFileName);
            else
                File.Move(srcfile, destfile);
        }

        private static void BackupDB(string filePath, string srcFilename, string destFileName)
        {
            var srcfile = Path.Combine(filePath, srcFilename);
            var destfile = Path.Combine(filePath, destFileName);

            if (File.Exists(destfile)) File.Delete(destfile);

            File.Copy(srcfile, destfile);
        }

        private static void DBCopy(SQLiteConnection conn, string new_db_path, string new_db, string[] new_tbs)
        {
            if(conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            string Attachedsql = "ATTACH DATABASE '" + new_db_path + "' AS " + new_db;
            sqlite_cmd.CommandText = Attachedsql;
            sqlite_cmd.ExecuteNonQuery();
            foreach (string new_tb in new_tbs)
            {
                switch (new_tb)
                {
                    case "CategoryTB":
                        string Createsql = "CREATE TABLE IF NOT EXISTS " + new_tb + " (id INTEGER PRIMARY KEY AUTOINCREMENT, CategoryName VARCHAR(200) NOT NULL, DayTime VARCHAR(200) NOT NULL DEFAULT '09:00-20-59', SatTime VARCHAR(200) NOT NULL DEFAULT '09:00-20-59', SunTime VARCHAR(200) NOT NULL DEFAULT '09:00-20-59', DisplayPosition INT, LayoutType INTEGER, BackgroundImg TEXT DEFAULT null, SoldFlag INT(2) NOT NULL DEFAULT 0)";
                        sqlite_cmd.CommandText = Createsql;
                        sqlite_cmd.ExecuteNonQuery();
                        DeleteData(conn, new_tb);
                        string Createsql1 = "INSERT INTO " + new_tb + " SELECT * FROM " + new_db + "." + new_tb + "";
                        sqlite_cmd.CommandText = Createsql1;
                        sqlite_cmd.ExecuteNonQuery();
                        break;
                    case "ProductTB":
                        string Createsql2 = "CREATE TABLE IF NOT EXISTS " + new_tb + " (id INTEGER PRIMARY KEY AUTOINCREMENT, CategoryID INT(10) NOT NULL, ProductID INT(10) NOT NULL DEFAULT 0, GroupID INT(10) NOT NULL DEFAULT 0, ProductName VARCHAR(200) NOT NULL, DayTime VARCHAR(200) NOT NULL DEFAULT '09:00-20-59', SatTime VARCHAR(200) NOT NULL DEFAULT '09:00-20-59', SunTime VARCHAR(200) NOT NULL DEFAULT '09:00-20-59', PrintName INT, ProductPrice INTEGER, LimitedCnt INT DEFAULT 0, ImgUrl VARCHAR(200), ScreenMsg VARCHAR(200), PrintMsg VARCHAR(200), CardNumber INT,  RCPhotoX INT, RCPhotoY INT, RCPhotoWidth INT, RCPhotoHeight INT, RCBadgeX INT, RCBadgeY INT, RCBadgeWidth INT, RCBadgeHeight INT, BadgePath VARCHAR(200), PtNameX INT, PtNameY INT, PtPriceX INT, PtPriceY INT, BackColor VARCHAR(200), ForeColor VARCHAR(200), BorderColor VARCHAR(200), SoldFlag INTEGER NOT NULL DEFAULT 0)";
                        sqlite_cmd.CommandText = Createsql2;
                        sqlite_cmd.ExecuteNonQuery();
                        DeleteData(conn, new_tb);
                        string Createsql3 = "INSERT INTO " + new_tb + " SELECT * FROM " + new_db + "." + new_tb + "";
                        sqlite_cmd.CommandText = Createsql3;
                        sqlite_cmd.ExecuteNonQuery();
                        string Createsql4 = "CREATE TABLE IF NOT EXISTS productTempTB (id INTEGER PRIMARY KEY AUTOINCREMENT, CategoryID INT(10) NOT NULL, ProductID INT(10) NOT NULL DEFAULT 0, GroupID INT(10) NOT NULL DEFAULT 0, ProductName VARCHAR(200) NOT NULL, DayTime VARCHAR(200) NOT NULL DEFAULT '09:00-20-59', SatTime VARCHAR(200) NOT NULL DEFAULT '09:00-20-59', SunTime VARCHAR(200) NOT NULL DEFAULT '09:00-20-59', PrintName INT, ProductPrice INTEGER, LimitedCnt INT DEFAULT 0, ImgUrl VARCHAR(200), ScreenMsg VARCHAR(200), PrintMsg VARCHAR(200), CardNumber INT,  RCPhotoX INT, RCPhotoY INT, RCPhotoWidth INT, RCPhotoHeight INT, RCBadgeX INT, RCBadgeY INT, RCBadgeWidth INT, RCBadgeHeight INT, BadgePath VARCHAR(200), PtNameX INT, PtNameY INT, PtPriceX INT, PtPriceY INT, BackColor VARCHAR(200), ForeColor VARCHAR(200), BorderColor VARCHAR(200), SoldFlag INTEGER NOT NULL DEFAULT 0)";
                        sqlite_cmd.CommandText = Createsql4;
                        sqlite_cmd.ExecuteNonQuery();
                        DeleteData(conn, "productTempTB");
                        string Createsql5 = "INSERT INTO productTempTB SELECT * FROM " + new_db + "." + new_tb + "";
                        sqlite_cmd.CommandText = Createsql5;
                        sqlite_cmd.ExecuteNonQuery();
                        break;

                }
            }
            sqlite_cmd.CommandText = "DETACH DATABASE '" + new_db + "'";
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.Dispose();
            conn.Close();
        }


        static void CreateCategory(SQLiteConnection conn)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            SQLiteCommand sqlite_cmd;
            string Createsql = "CREATE TABLE IF NOT EXISTS CategoryTB (id INTEGER PRIMARY KEY AUTOINCREMENT, CategoryName VARCHAR(200) NOT NULL, DayTime VARCHAR(200) NOT NULL DEFAULT '09:00-20-59', SatTime VARCHAR(200) NOT NULL DEFAULT '09:00-20-59', SunTime VARCHAR(200) NOT NULL DEFAULT '09:00-20-59', DisplayPosition INT, LayoutType INTEGER, BackgroundImg TEXT DEFAULT null, SoldFlag INT(2) NOT NULL DEFAULT 0)";
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = Createsql;
            sqlite_cmd.ExecuteNonQuery();
            conn.Close();
        }
        static void CreateProduct(SQLiteConnection conn)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            SQLiteCommand sqlite_cmd;
            string Createsql = "CREATE TABLE IF NOT EXISTS ProductTB (id INTEGER PRIMARY KEY AUTOINCREMENT, CategoryID INT(10) NOT NULL, ProductID INT(10) NOT NULL DEFAULT 0, GroupID INT(10) NOT NULL DEFAULT 0, ProductName VARCHAR(200) NOT NULL, DayTime VARCHAR(200) NOT NULL DEFAULT '09:00-20-59', SatTime VARCHAR(200) NOT NULL DEFAULT '09:00-20-59', SunTime VARCHAR(200) NOT NULL DEFAULT '09:00-20-59', PrintName INT, ProductPrice INTEGER, LimitedCnt INT DEFAULT 0, ImgUrl VARCHAR(200), ScreenMsg VARCHAR(200), PrintMsg VARCHAR(200), CardNumber INT,  RCPhotoX INT, RCPhotoY INT, RCPhotoWidth INT, RCPhotoHeight INT, RCBadgeX INT, RCBadgeY INT, RCBadgeWidth INT, RCBadgeHeight INT, BadgePath VARCHAR(200), PtNameX INT, PtNameY INT, PtPriceX INT, PtPriceY INT, BackColor VARCHAR(200), ForeColor VARCHAR(200), BorderColor VARCHAR(200), SoldFlag INTEGER NOT NULL DEFAULT 0)";
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = Createsql;
            sqlite_cmd.ExecuteNonQuery();
            conn.Close();
        }

        static void InsertCategoryData(SQLiteConnection conn, string categoryName, string DayTime, string SatTime, string SunTime, int DisplayPosition, int LayoutType, string BackgroundImg = null)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "INSERT INTO CategoryTB (CategoryName, DayTime, SatTime, SunTime, DisplayPosition, LayoutType) VALUES(@CategoryName, @DayTime, @SatTime, @SunTime, @DisplayPosition, @LayoutType)";
            sqlite_cmd.Parameters.AddWithValue("@CategoryName", categoryName);
            sqlite_cmd.Parameters.AddWithValue("@DayTime", DayTime);
            sqlite_cmd.Parameters.AddWithValue("@SatTime", SatTime);
            sqlite_cmd.Parameters.AddWithValue("@SunTime", SunTime);
            sqlite_cmd.Parameters.AddWithValue("@DisplayPosition", DisplayPosition);
            sqlite_cmd.Parameters.AddWithValue("@LayoutType", LayoutType);
            sqlite_cmd.ExecuteNonQuery();
            conn.Close();
        }
        static void InsertProductData(SQLiteConnection conn, int categoryID, int groupID, string productName, string printName, int productPrice, int LimitedCnt, string ImgUrl, string ScreenMsg, string PrintMsg, int CardNumber, int RCPhotoX, int RCPhotoY, int RCPhotoWidth, int RCPhotoHeight, int RCBadgeX, int RCBadgeY, int RCBadgeWidth, int RCBadgeHeight, string BadgePath, Point PtName, Point PtPrice, string DayTime, string SatTime, string SunTime, Color BackColor, Color ForeColor, Color BorderColor)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "INSERT INTO ProductTB (CategoryID, GroupID, ProductName, PrintName, ProductPrice, LimitedCnt, ImgUrl, ScreenMsg, PrintMsg, CardNumber, RCPhotoX, RCPhotoY, RCPhotoWidth, RCPhotoHeight, RCBadgeX, RCBadgeY, RCBadgeWidth, RCBadgeHeight, BadgePath, PtNameX, PtNameY, PtPriceX, PtPriceY, DayTime, SatTime, SunTime, BackColor, ForeColor, BorderColor) VALUES(@CategoryID, @GroupID, @ProductName, @PrintName, @ProductPrice, @LimitedCnt, @ImgUrl, @ScreenMsg, @PrintMsg, @CardNumber, @RCPhotoX, @RCPhotoY, @RCPhotoWidth, @RCPhotoHeight, @RCBadgeX, @RCBadgeY, @RCBadgeWidth, @RCBadgeHeight, @BadgePath, @PtNameX, @PtNameY, @PtPriceX, @PtPriceY, @DayTime, @SatTime, @SunTime, @BackColor, @ForeColor, @BorderColor)";
            sqlite_cmd.Parameters.AddWithValue("@CategoryID", categoryID);
            sqlite_cmd.Parameters.AddWithValue("@GroupID", groupID);
            sqlite_cmd.Parameters.AddWithValue("@ProductName", productName);
            sqlite_cmd.Parameters.AddWithValue("@PrintName", printName);
            sqlite_cmd.Parameters.AddWithValue("@ProductPrice", productPrice);
            sqlite_cmd.Parameters.AddWithValue("@LimitedCnt", LimitedCnt);
            sqlite_cmd.Parameters.AddWithValue("@ImgUrl", ImgUrl);
            sqlite_cmd.Parameters.AddWithValue("@ScreenMsg", ScreenMsg);
            sqlite_cmd.Parameters.AddWithValue("@PrintMsg", PrintMsg);
            sqlite_cmd.Parameters.AddWithValue("@CardNumber", CardNumber);
            sqlite_cmd.Parameters.AddWithValue("@RCPhotoX", RCPhotoX);
            sqlite_cmd.Parameters.AddWithValue("@RCPhotoY", RCPhotoY);
            sqlite_cmd.Parameters.AddWithValue("@RCPhotoWidth", RCPhotoWidth);
            sqlite_cmd.Parameters.AddWithValue("@RCPhotoHeight", RCPhotoHeight);
            sqlite_cmd.Parameters.AddWithValue("@RCBadgeX", RCBadgeX);
            sqlite_cmd.Parameters.AddWithValue("@RCBadgeY", RCBadgeY);
            sqlite_cmd.Parameters.AddWithValue("@RCBadgeWidth", RCBadgeWidth);
            sqlite_cmd.Parameters.AddWithValue("@RCBadgeHeight", RCBadgeHeight);
            sqlite_cmd.Parameters.AddWithValue("@RCBadgeHeight", RCBadgeHeight);
            sqlite_cmd.Parameters.AddWithValue("@BadgePath", BadgePath);
            sqlite_cmd.Parameters.AddWithValue("@PtNameX", PtName.X);
            sqlite_cmd.Parameters.AddWithValue("@PtNameY", PtName.Y);
            sqlite_cmd.Parameters.AddWithValue("@PtPriceX", PtPrice.X);
            sqlite_cmd.Parameters.AddWithValue("@PtPriceY", PtPrice.Y);
            sqlite_cmd.Parameters.AddWithValue("@DayTime", DayTime);
            sqlite_cmd.Parameters.AddWithValue("@SatTime", SatTime);
            sqlite_cmd.Parameters.AddWithValue("@SunTime", SunTime);
            sqlite_cmd.Parameters.AddWithValue("@BackColor", BackColor.Name);
            sqlite_cmd.Parameters.AddWithValue("@ForeColor", ForeColor.Name);
            sqlite_cmd.Parameters.AddWithValue("@BorderColor", BorderColor.Name);
            sqlite_cmd.ExecuteNonQuery();
            conn.Close();
        }
        static void DeleteData(SQLiteConnection conn, string tb_name)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "DELETE FROM " + tb_name;
            sqlite_cmd.ExecuteNonQuery();
        }

        static void DeleteTable(SQLiteConnection conn)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "DROP database.SampleTable";
            sqlite_cmd.ExecuteNonQuery();


            sqlite_cmd.CommandText = "DROP database.SampleTable1";
            sqlite_cmd.ExecuteNonQuery();
        }


        static void ReadData(SQLiteConnection conn)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM SampleTable";

            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                string myreader = sqlite_datareader.GetString(0);
                Console.WriteLine(myreader);
            }
            conn.Close();
        }

    }
}

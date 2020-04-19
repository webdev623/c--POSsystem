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
            //if(openFileDialog1.ShowDialog() == DialogResult.OK)
            //{
            //    if(sqlite_conn.State == ConnectionState.Open)
            //    {
            //        sqlite_conn.Close();
            //    }
            //    try
            //    {
            //        var filePath = openFileDialog1.FileName;
            //        var fileName = openFileDialog1.SafeFileName;
            //        var curDir = Directory.GetCurrentDirectory();
            //        var dbName = fileName.Split('.')[0];
            //        Thread.Sleep(100);
            //        RestoreDB(curDir, filePath, fileName, true);
            //        DBCopy(sqlite_conn, Path.Combine(curDir, fileName), dbName, constants.tbNames);

            //        this.CreateSaleTB(sqlite_conn);
            //        this.CreateDaySaleTB(sqlite_conn);
            //        this.CreateReceiptTB(sqlite_conn);
            //        this.CreateCancelOrderTB(sqlite_conn);

            //        mainFormGlobal.Controls.Clear();
            //        MainMenu mainMenu = new MainMenu();
            //        mainMenu.CreateMainMenuScreen(mainFormGlobal, mainPanelGlobal);
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex);
            //        messageDialog.ShowMenuReadingMessage();
            //    }
            //}
            if(folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                if (sqlite_conn.State == ConnectionState.Open)
                {
                    sqlite_conn.Close();
                }
                try
                {
                    string SourcePath = folderBrowserDialog1.SelectedPath;
                    string DestinationPath = Directory.GetCurrentDirectory();
                    DirectoryInfo dir = new DirectoryInfo(SourcePath);
                    FileInfo[] targetFile = dir.GetFiles();
                    string sourceFileName = "";
                    var dbName = "";

                    foreach (FileInfo files in targetFile)
                    {
                        if (files.Extension == ".db")
                        {
                            sourceFileName = files.Name;
                            dbName = sourceFileName.Split('.')[0];
                            break;
                        }
                    }

                    DirectoryCopy(SourcePath, DestinationPath, true);
                    DBCopy(sqlite_conn, Path.Combine(DestinationPath, sourceFileName), dbName, constants.tbNames);

                    this.CreateSaleTB(sqlite_conn);
                    this.CreateDaySaleTB(sqlite_conn);
                    this.CreateReceiptTB(sqlite_conn);
                    this.CreateCancelOrderTB(sqlite_conn);

                    mainFormGlobal.Controls.Clear();
                    MainMenu mainMenu = new MainMenu();
                    mainMenu.CreateMainMenuScreen(mainFormGlobal, mainPanelGlobal);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    messageDialog.ShowMenuReadingMessage();
                }

            }
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                if (File.Exists(temppath)) File.Delete(temppath);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
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
                Console.WriteLine(ex);
            }
            return sqlite_conn;
        }
        private void RestoreDB(string filePath, string srcFilename, string destFileName, bool IsCopy = false)
        {
            var srcfile = srcFilename;
            var destfile = Path.Combine(filePath, destFileName);

            if (File.Exists(destfile)) File.Delete(destfile);

            if (IsCopy)
                BackupDB(filePath, srcFilename, destFileName);
            else
                File.Move(srcfile, destfile);
        }

        private void BackupDB(string filePath, string srcFilename, string destFileName)
        {
            var srcfile = Path.Combine(filePath, srcFilename);
            var destfile = Path.Combine(filePath, destFileName);

            if (File.Exists(destfile)) File.Delete(destfile);

            File.Copy(srcfile, destfile);
        }

        private void DBCopy(SQLiteConnection conn, string new_db_path, string new_db, string[] new_tbs)
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
                string createSql = "";
                switch (new_tb)
                {
                    case "CategoryTB":
                        createSql = "CREATE TABLE IF NOT EXISTS " + new_tb + " (id INTEGER PRIMARY KEY AUTOINCREMENT, CategoryID INT(2) NOT NULL DEFAULT 0, CategoryName VARCHAR(64) NOT NULL, DayTime VARCHAR(256) NOT NULL DEFAULT '09:00-20-59', SatTime VARCHAR(256) NOT NULL DEFAULT '09:00-20-59', SunTime VARCHAR(256) NOT NULL DEFAULT '09:00-20-59', DisplayPosition INT, LayoutType INTEGER, BackgroundImg TEXT DEFAULT NULL, BackImgUrl TEXT DEFAULT NULL, SoldFlag INT(2) NOT NULL DEFAULT 0)";
                        break;
                    case "ProductTB":
                        createSql = "CREATE TABLE IF NOT EXISTS " + new_tb + " (ProductID INT(2) NOT NULL DEFAULT 0, ProductName VARCHAR(32) NOT NULL, PrintName VARCHAR(32) NOT NULL, DayTime VARCHAR(128) NOT NULL DEFAULT '09:00-20-59', SatTime VARCHAR(128) NOT NULL DEFAULT '09:00-20-59', SunTime VARCHAR(128) NOT NULL DEFAULT '09:00-20-59', ProductPrice INT(8), LimitedCnt INT(2) DEFAULT 0, ImgUrl VARCHAR(256), ValidImgUrl VARCHAR(128), ScreenMsg VARCHAR(64), PrintMsg VARCHAR(64))";
                        break;
                    case "CategoryDetailTB":
                        createSql = "CREATE TABLE IF NOT EXISTS " + new_tb + " (id INTEGER PRIMARY KEY AUTOINCREMENT, CategoryID INT(10) NOT NULL DEFAULT 0, ProductID INT(10) NOT NULL DEFAULT 0, ProductName VARCHAR(32) NOT NULL, PrintName VARCHAR(32) NOT NULL, DayTime VARCHAR(128) NOT NULL DEFAULT '09:00-20-59', SatTime VARCHAR(128) NOT NULL DEFAULT '09:00-20-59', SunTime VARCHAR(128) NOT NULL DEFAULT '09:00-20-59', ProductPrice INT(8), LimitedCnt INT(2) DEFAULT 0, ImgUrl VARCHAR(256), ValidImgUrl VARCHAR(128), ScreenMsg VARCHAR(256), PrintMsg VARCHAR(256), CardNumber INT(2),  RCPhotoX INT(4), RCPhotoY INT(4), RCPhotoWidth INT(4), RCPhotoHeight INT(4), RCBadgeX INT(4), RCBadgeY INT(4), RCBadgeWidth INT(4), RCBadgeHeight INT(4), BadgePath VARCHAR(256), PtNameX INT(4), PtNameY INT(4), PtPriceX INT(4), PtPriceY INT(4), BackColor VARCHAR(16), ForeColor VARCHAR(16), BorderColor VARCHAR(16), SoldFlag INT(2) NOT NULL DEFAULT 0)";
                        break;
                    case "TableSetTicket":
                        createSql = "CREATE TABLE IF NOT EXISTS " + new_tb + " (PurchaseType INT(2) NOT NULL DEFAULT 1, ReturnTime INT(4) NOT NULL DEFAULT 30, MultiPurchase INT(2) NOT NULL DEFAULT 1, PurchaseAmount INT(4) NOT NULL DEFAULT 10, SerialNo INT(2) NOT NULL DEFAULT 1, StartSerialNo INT(4) NOT NULL DEFAULT 0, NoAfterTight INT(4) NOT NULL DEFAULT 1, FontSize INT(2) NOT NULL DEFAULT 1, ValidDate INT(2) NOT NULL DEFAULT 1, TicketMsg1 VARCHAR(16) NOT NULL, TicketMsg2 VARCHAR(16) NOT NULL)";
                        break;
                    case "TableSetReceipt":
                        createSql = "CREATE TABLE IF NOT EXISTS " + new_tb + " (ReceiptValid VARCHAR(8) NOT NULL, TicketTime VARCHAR(4) NOT NULL, StoreName VARCHAR(64) NOT NULL, Address VARCHAR(64) NOT NULL, PhoneNumber VARCHAR(16) NOT NULL, Other1 VARCHAR(64) NOT NULL, Other2 VARCHAR(64) NOT NULL)";
                        break;
                    case "TableSetStore":
                        createSql = "CREATE TABLE IF NOT EXISTS " + new_tb + " (StoreName VARCHAR(32) NOT NULL, Address VARCHAR(32) NOT NULL, PhoneNumber VARCHAR(16) NOT NULL, WeekTime VARCHAR(128) NOT NULL, SaturdayTime VARCHAR(128) NOT NULL, SundayTime VARCHAR(128) NOT NULL, EndTime VARCHAR(32) NOT NULL)";
                        break;
                    case "TableGroupName":
                        createSql = "CREATE TABLE IF NOT EXISTS " + new_tb + " (GroupID INT(2) NOT NULL DEFAULT 0, GroupName VARCHAR(32) NOT NULL)";
                        break;
                    case "TableGroupDetail":
                        createSql = "CREATE TABLE IF NOT EXISTS " + new_tb + " (GroupID INT(2) NOT NULL DEFAULT 0, ProductName VARCHAR(32) NOT NULL, ProductPrice INT(8) NOT NULL DEFAULT 0)";
                        break;
                    case "GeneralTB":
                        createSql = "CREATE TABLE IF NOT EXISTS " + new_tb + " (PatternColor INT(2) NOT NULL DEFAULT 0, MenuMsg1 VARCHAR(64) NOT NULL, MenuMsg2 VARCHAR(64) NOT NULL)";
                        break;
                    case "TableSetAudio":
                        createSql = "CREATE TABLE IF NOT EXISTS " + new_tb + " (WaitingAudio VARCHAR(32) NOT NULL, ButtonTouch VARCHAR(32) NOT NULL, CashInsert VARCHAR(16), ValidItemTouch VARCHAR(16), ReturnTouch VARCHAR(16), RefundCompleted VARCHAR(16), DeleteTouch VARCHAR(16), IncreaseTouch VARCHAR(16), DecreaseTouch VARCHAR(16), TicketDisable VARCHAR(16), TicketValid VARCHAR(16), TicketIssue VARCHAR(16), ErrorOccur VARCHAR(16))";
                        break;
                    case "SaleTB":
                        break;
                    case "DaySaleTB":
                        break;
                    case "ReceiptTB":
                        break;
                    case "CancelOrderTB":
                        break;
                    
                }
                if(createSql != "")
                {
                    sqlite_cmd.CommandText = createSql;
                    sqlite_cmd.ExecuteNonQuery();
                    this.DeleteData(conn, new_tb);
                    string Createsql1 = "INSERT INTO " + new_tb + " SELECT * FROM " + new_db + "." + new_tb + "";
                    sqlite_cmd.CommandText = Createsql1;
                    sqlite_cmd.ExecuteNonQuery();
                }

            }
            sqlite_cmd.CommandText = "DETACH DATABASE '" + new_db + "'";
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.Dispose();
            conn.Close();
        }


        private void CreateSaleTB(SQLiteConnection conn)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            SQLiteCommand sqlite_cmd;
            string Createsql = "CREATE TABLE IF NOT EXISTS " + constants.tbNames[3] + " (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, prdID INTEGER NOT NULL, prdRealID INT(10) NOT NULL DEFAULT 0, prdName VARCHAR(200) NOT NULL, prdPrice INTEGER NOT NULL DEFAULT 0, prdAmount INTEGER NOT NULL DEFAULT 0, ticketNo INTEGER NOT NULL DEFAULT 0, saleDate DATETIME, sumFlag BOOLEAN NOT NULL DEFAULT 'false', sumDate DATETIME, categoryID  INTEGER NOT NULL DEFAULT 0, serialNo INTEGER NOT NULL DEFAULT 1)";
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = Createsql;
            sqlite_cmd.ExecuteNonQuery();
            conn.Close();
        }
        private void CreateDaySaleTB(SQLiteConnection conn)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            SQLiteCommand sqlite_cmd;
            string Createsql = "CREATE TABLE IF NOT EXISTS " + constants.tbNames[7] + " (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, prdID INTEGER NOT NULL DEFAULT 0, prdName VARCHAR(128) NOT NULL DEFAULT '', prdPrice INTEGER NOT NULL DEFAULT 0, prdAmount INTEGER NOT NULL DEFAULT 0, prdTotalPrice INTEGER NOT NULL DEFAULT 0, sumDate VARCHAR(10) NOT NULL DEFAULT '')";
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = Createsql;
            sqlite_cmd.ExecuteNonQuery();
            conn.Close();
        }
        private void CreateReceiptTB(SQLiteConnection conn)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            SQLiteCommand sqlite_cmd;
            string Createsql = "CREATE TABLE IF NOT EXISTS " + constants.tbNames[8] + " (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, PurchasePoint INTEGER NOT NULL, TotalPrice INTEGER NOT NULL DEFAULT 0, ReceiptDate DATETIME)";
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = Createsql;
            sqlite_cmd.ExecuteNonQuery();
            conn.Close();
        }

        private void CreateCancelOrderTB(SQLiteConnection conn)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            SQLiteCommand sqlite_cmd;
            string Createsql = "CREATE TABLE IF NOT EXISTS " + constants.tbNames[9] + " (id INTEGER PRIMARY KEY AUTOINCREMENT, saleID INTEGER NOT NULL, prdID INTEGER NOT NULL, prdName VARCHAR(200) NOT NULL, prdPrice INTEGER NOT NULL DEFAULT 0, prdAmount INTEGER NOT NULL DEFAULT 0, ticketNo INTEGER NOT NULL DEFAULT 0, saleDate DATETIME, sumFlag BOOLEAN NOT NULL DEFAULT 'false', sumDate DATETIME, categoryID INTEGER NOT NULL DEFAULT 0, serialNo INTEGER NOT NULL DEFAULT 1, realPrdID INTEGER DEFAULT 0, cancelDate DATETIME)";
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = Createsql;
            sqlite_cmd.ExecuteNonQuery();
            conn.Close();
        }

        private void DeleteData(SQLiteConnection conn, string tb_name)
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

    }
}

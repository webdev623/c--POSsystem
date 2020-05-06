using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ovan_P1
{
    class DBClass
    {
        Constant constants = new Constant();
        SQLiteConnection sqlite_conn;
        public DBClass()
        {
            sqlite_conn = CreateConnection(constants.dbName);
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
        public void DBCopy(SQLiteConnection conn, string new_db_path, string new_db, string[] new_tbs)
        {
            if (conn.State == ConnectionState.Closed)
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
                if (createSql != "")
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

        public void CreateSaleTB(SQLiteConnection conn)
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
        public void CreateDaySaleTB(SQLiteConnection conn)
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
        public void CreateReceiptTB(SQLiteConnection conn)
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

        public void CreateCancelOrderTB(SQLiteConnection conn)
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

        public void DeleteData(SQLiteConnection conn, string tb_name)
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

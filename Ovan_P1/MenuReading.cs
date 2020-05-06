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
        CreatePanel createPanel = new CreatePanel();
        CreateLabel createLabel = new CreateLabel();
        CustomButton customButton = new CustomButton();
        DBClass dbClass = new DBClass();
        MessageDialog messageDialog = new MessageDialog();
        SQLiteConnection sqlite_conn;
        public MenuReading(Form1 mainForm, Panel mainPanel)
        {
            InitializeComponent();
            messageDialog.initMenuReading(this);
            sqlite_conn = CreateConnection(constants.dbName);

            mainFormGlobal = mainForm;
            mainPanelGlobal = mainPanel;

            Panel headerPanel = createPanel.CreateSubPanel(mainPanel, 0, 0, mainPanel.Width, mainPanel.Height / 10, BorderStyle.None, Color.FromArgb(255, 234, 225, 151));
            Label headerLabel = createLabel.CreateLabelsInPanel(headerPanel, "headerTitle", constants.menuReadingTitle, 0, 0, headerPanel.Width, headerPanel.Height, Color.Transparent, Color.Black, 28, false, ContentAlignment.MiddleCenter);
            Panel bodyPanel = createPanel.CreateSubPanel(mainPanel, 0, headerPanel.Bottom, mainPanel.Width, mainPanel.Height * 9 / 10, BorderStyle.None, Color.Transparent);

            Label subContent1 = createLabel.CreateLabelsInPanel(bodyPanel, "subContent1", constants.menuReadingSubContent1, bodyPanel.Width / 5, 50, bodyPanel.Width * 3 / 5, bodyPanel.Height / 8, Color.Transparent, Color.Black, 24);

            Label subContent2 = createLabel.CreateLabelsInPanel(bodyPanel, "subContent1", constants.menuReadingSubContent2, bodyPanel.Width / 5, subContent1.Bottom, bodyPanel.Width * 3 / 5, bodyPanel.Height / 8, Color.Transparent, Color.Black, 24);

            Button readButton = customButton.CreateButtonWithImage(Image.FromFile(constants.menureadingButtonImage), "readButton", constants.menuReadingButton, bodyPanel.Width / 2 - 100, subContent2.Bottom + 50, 200, 50, 0, 20, 18, FontStyle.Bold, Color.White, ContentAlignment.MiddleCenter, 2);
            bodyPanel.Controls.Add(readButton);
            readButton.Click += new EventHandler(this.OpenFileDialog);

            Button cancelButton = customButton.CreateButtonWithImage(Image.FromFile(constants.cancelButton), "readButton", constants.cancelButtonText, bodyPanel.Width - 150, bodyPanel.Height - 100, 100, 50, 0, 20, 18, FontStyle.Bold, Color.White, ContentAlignment.MiddleCenter, 2);
            bodyPanel.Controls.Add(cancelButton);
            cancelButton.Click += new EventHandler(this.BackShow);

        }


        private void MenuReading_Load(object sender, EventArgs e)
        {
            InitializeComponent();
        }

        private void OpenFileDialog(object sender, EventArgs e)
        {
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
                    if(dbName != "")
                    {
                        DirectoryCopy(SourcePath, DestinationPath, true);
                        dbClass.DBCopy(sqlite_conn, Path.Combine(DestinationPath, sourceFileName), dbName, constants.tbNames);

                        dbClass.CreateSaleTB(sqlite_conn);
                        dbClass.CreateDaySaleTB(sqlite_conn);
                        dbClass.CreateReceiptTB(sqlite_conn);
                        dbClass.CreateCancelOrderTB(sqlite_conn);

                        mainPanelGlobal.Controls.Clear();
                        MainMenu mainMenu = new MainMenu();
                        mainMenu.CreateMainMenuScreen(mainFormGlobal, mainPanelGlobal);
                    }
                    else
                    {
                        messageDialog.ShowMenuReadingMessage();
                    }
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
            mainPanelGlobal.Controls.Clear();
            MainMenu mainMenu = new MainMenu();
            mainMenu.CreateMainMenuScreen(mainFormGlobal, mainPanelGlobal);
        }

        public void BackShowStart()
        {
            mainPanelGlobal.Controls.Clear();
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

    }
}

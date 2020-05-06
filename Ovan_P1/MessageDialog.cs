using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ovan_P1
{
    public partial class MessageDialog : Form
    {
        int height = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height;
        int width = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width;
        Constant constants = new Constant();
        CreatePanel createPanel = new CreatePanel();
        CreateLabel createLabel = new CreateLabel();
        CustomButton createButton = new CustomButton();
        CategoryList categoryList = null;
        GroupList groupList = null;
        MenuReading menuReading = null;
        OpenTimeChange openTimeChange = null;

        public Form DialogFormGlobal = null;
        string indicator = "";
        public MessageDialog()
        {
            InitializeComponent();
        }
        public void initCategoryList(CategoryList sendHandler)
        {
            categoryList = sendHandler;
        }
        public void initGroupList(GroupList sendHandler)
        {
            groupList = sendHandler;
        }
        public void initMenuReading(MenuReading sendHandler)
        {
            menuReading = sendHandler;
        }

        public void initOpenTimeChange(OpenTimeChange sendHandler)
        {
            openTimeChange = sendHandler;
        }
        public void MessageDialogInit(object sender, EventArgs e)
        {
            Button btnTemp = (Button)sender;
            indicator = btnTemp.Name;
            if(indicator == "categoryPrintButton")
            {
                ShowCategoryPrintMessage();
            }
            else if (indicator == "groupPrintButton")
            {

                ShowGroupPrintMessage();
            }
        }

        private void ShowCategoryPrintMessage()
        {
            Form dialogForm = new Form();
            dialogForm.Size = new Size(width / 3, height / 3);
            dialogForm.StartPosition = FormStartPosition.CenterParent;
            dialogForm.WindowState = FormWindowState.Normal;
            dialogForm.ControlBox = false;
            dialogForm.FormBorderStyle = FormBorderStyle.None;
            dialogForm.BackgroundImage = Image.FromFile(constants.dialogFormImage);
            dialogForm.BackgroundImageLayout = ImageLayout.Stretch;
            DialogFormGlobal = dialogForm;

            Panel mainPanel = createPanel.CreateMainPanel(dialogForm, 0, 0, dialogForm.Width, dialogForm.Height, BorderStyle.None, Color.Transparent);

            Label messageLabel = createLabel.CreateLabelsInPanel(mainPanel, "messageLabel", constants.categoryListPrintMessage, 50, 0, mainPanel.Width - 100, mainPanel.Height - 100, Color.Transparent, Color.Black, 18, false, ContentAlignment.MiddleCenter);
            messageLabel.Padding = new Padding(30, 0, 30, 0);

            Button okButton = createButton.CreateButtonWithImage(Image.FromFile(constants.rectRedButton), "categoryPrintOkButton", constants.yesStr, mainPanel.Width / 8, messageLabel.Bottom, mainPanel.Width / 4, 50, 0, 1, 14, FontStyle.Bold, Color.White, ContentAlignment.MiddleCenter, 1);
            mainPanel.Controls.Add(okButton);
            okButton.Click += new EventHandler(this.CategoryPrintView);

            Button closeButton = createButton.CreateButtonWithImage(Image.FromFile(constants.rectBlueButton), "closeButton", constants.noStr, mainPanel.Width * 5 / 8, messageLabel.Bottom, mainPanel.Width / 4, 50, 0, 1, 14, FontStyle.Bold, Color.White, ContentAlignment.MiddleCenter, 1);
            closeButton.ForeColor = Color.White;
            mainPanel.Controls.Add(closeButton);
            closeButton.Click += new EventHandler(this.BackShow);

            dialogForm.ShowDialog();
        }

        private void ShowGroupPrintMessage()
        {
            Form dialogForm = new Form();
            dialogForm.Size = new Size(width * 2 / 5, height / 3);
            dialogForm.StartPosition = FormStartPosition.CenterParent;
            dialogForm.WindowState = FormWindowState.Normal;
            dialogForm.ControlBox = false;
            dialogForm.FormBorderStyle = FormBorderStyle.None;
            dialogForm.BackgroundImage = Image.FromFile(constants.dialogFormImage);
            dialogForm.BackgroundImageLayout = ImageLayout.Stretch;
            DialogFormGlobal = dialogForm;

            Panel mainPanel = createPanel.CreateMainPanel(dialogForm, 0, 0, dialogForm.Width, dialogForm.Height, BorderStyle.None, Color.Transparent);

            Label messageLabel = createLabel.CreateLabelsInPanel(mainPanel, "messageLabel", constants.groupListPrintMessage, 50, 0, mainPanel.Width - 100, mainPanel.Height - 100, Color.Transparent, Color.Black, 18, false, ContentAlignment.MiddleCenter);
            messageLabel.Padding = new Padding(30, 0, 30, 0);

            Button okButton = createButton.CreateButtonWithImage(Image.FromFile(constants.rectRedButton), "categoryPrintOkButton", constants.yesStr, mainPanel.Width / 8, mainPanel.Height - 100, mainPanel.Width / 4, 50, 0, 1, 14, FontStyle.Bold, Color.White, ContentAlignment.MiddleCenter, 1);
            mainPanel.Controls.Add(okButton);
            okButton.Click += new EventHandler(this.GroupPrintView);

            Button closeButton = createButton.CreateButtonWithImage(Image.FromFile(constants.rectBlueButton), "closeButton", constants.noStr, mainPanel.Width * 5 / 8, mainPanel.Height - 100, mainPanel.Width / 4, 50, 0, 1, 14, FontStyle.Bold, Color.White, ContentAlignment.MiddleCenter, 1);
            mainPanel.Controls.Add(closeButton);
            closeButton.Click += new EventHandler(this.BackShow);

            dialogForm.ShowDialog();
        }
        public void ShowMenuReadingMessage()
        {
            Form dialogForm = new Form();
            dialogForm.Size = new Size(width * 2 / 5, height / 3);
            dialogForm.StartPosition = FormStartPosition.CenterParent;
            dialogForm.WindowState = FormWindowState.Normal;
            dialogForm.ControlBox = false;
            dialogForm.FormBorderStyle = FormBorderStyle.None;
            dialogForm.BackgroundImage = Image.FromFile(constants.dialogFormImage);
            dialogForm.BackgroundImageLayout = ImageLayout.Stretch;
            DialogFormGlobal = dialogForm;

            Panel mainPanel = createPanel.CreateMainPanel(dialogForm, 0, 0, dialogForm.Width, dialogForm.Height, BorderStyle.None, Color.Transparent);

            Label messageLabel1 = createLabel.CreateLabelsInPanel(mainPanel, "messageLabel1", constants.menuReadingErrorTitle, 50, 0, mainPanel.Width - 100, (mainPanel.Height - 100) / 3, Color.Transparent, Color.Red, 22, false, ContentAlignment.BottomCenter);
            messageLabel1.Padding = new Padding(30, 0, 30, 0);
            Label messageLabel2 = createLabel.CreateLabelsInPanel(mainPanel, "messageLabel2", constants.menuReadingErrorContent, 50, (mainPanel.Height - 100) / 3, mainPanel.Width - 100, (mainPanel.Height - 100) * 2 /3, Color.Transparent, Color.Black, 18, false, ContentAlignment.MiddleCenter);
            messageLabel2.Padding = new Padding(30, 0, 30, 0);


            Button closeButton = createButton.CreateButtonWithImage(Image.FromFile(constants.cancelButton), "closeButton", constants.noStr, mainPanel.Width / 2 - 75, mainPanel.Height - 100, 150, 50, 0, 20, 14, FontStyle.Bold, Color.White, ContentAlignment.MiddleCenter, 2);
            mainPanel.Controls.Add(closeButton);
            closeButton.Click += new EventHandler(this.BackShowMenuReading);

            dialogForm.ShowDialog();
        }
        public void ShowErrorMessage(string errorMsg1, string errorMsg2)
        {
            Form dialogForm = new Form();
            dialogForm.Size = new Size(width * 2 / 5, height / 3);
            dialogForm.StartPosition = FormStartPosition.CenterParent;
            dialogForm.WindowState = FormWindowState.Normal;
            dialogForm.ControlBox = false;
            dialogForm.FormBorderStyle = FormBorderStyle.None;
            dialogForm.BackgroundImage = Image.FromFile(constants.errordialogImage);
            dialogForm.BackgroundImageLayout = ImageLayout.Stretch;
            DialogFormGlobal = dialogForm;

            Panel mainPanel = createPanel.CreateMainPanel(dialogForm, 0, 0, dialogForm.Width, dialogForm.Height, BorderStyle.None, Color.Transparent);

            Label messageLabel1 = createLabel.CreateLabelsInPanel(mainPanel, "messageLabel2", errorMsg1, 50, (mainPanel.Height) / 3, mainPanel.Width - 100, (mainPanel.Height - 160) * 1 / 3, Color.Transparent, Color.Black, 24, false, ContentAlignment.MiddleCenter);
            messageLabel1.Padding = new Padding(30, 0, 30, 0);
            Label messageLabel2 = createLabel.CreateLabelsInPanel(mainPanel, "messageLabel2", errorMsg2, mainPanel.Width / 7, messageLabel1.Bottom + 10, mainPanel.Width * 5 / 7, (mainPanel.Height - 180) * 1 / 3, Color.Transparent, Color.Black, 18, false, ContentAlignment.MiddleCenter);
            messageLabel2.Padding = new Padding(30, 0, 30, 0);


            Button closeButton = createButton.CreateButtonWithImage(Image.FromFile(constants.cancelButton), "closeButton", "クローズ", mainPanel.Width / 2 - 75, mainPanel.Height - 80, 150, 50, 0, 20, 14, FontStyle.Bold, Color.White, ContentAlignment.MiddleCenter, 2);
            closeButton.FlatAppearance.BorderColor = Color.FromArgb(255, 236, 179, 150);
            mainPanel.Controls.Add(closeButton);
            closeButton.Click += new EventHandler(this.BackShow);

            dialogForm.ShowDialog();
        }
        public void ShowPrintErrorMessage(string errorMsg1, string errorMsg2)
        {
            Form dialogForm = new Form();
            dialogForm.Size = new Size(width * 2 / 5, height / 3);
            dialogForm.StartPosition = FormStartPosition.CenterParent;
            dialogForm.WindowState = FormWindowState.Normal;
            dialogForm.ControlBox = false;
            dialogForm.FormBorderStyle = FormBorderStyle.None;
            dialogForm.BackgroundImage = Image.FromFile(constants.errordialogImage);
            dialogForm.BackgroundImageLayout = ImageLayout.Stretch;
            DialogFormGlobal = dialogForm;

            Panel mainPanel = createPanel.CreateMainPanel(dialogForm, 0, 0, dialogForm.Width, dialogForm.Height, BorderStyle.None, Color.Transparent);

            Label messageLabel1 = createLabel.CreateLabelsInPanel(mainPanel, "messageLabel2", errorMsg1, 50, (mainPanel.Height) / 3, mainPanel.Width - 100, (mainPanel.Height - 160) * 1 / 3, Color.Transparent, Color.Black, constants.fontSizeMedium, false, ContentAlignment.MiddleCenter);
            messageLabel1.Padding = new Padding(30, 0, 30, 0);

            Label messageLabel2 = createLabel.CreateLabelsInPanel(mainPanel, "messageLabel2", errorMsg2, mainPanel.Width / 4, (mainPanel.Height - 100) * 2 / 3, mainPanel.Width / 2, (mainPanel.Height - 180) * 1 / 3, Color.Transparent, Color.Black, constants.fontSizeSmall, false, ContentAlignment.MiddleCenter);
            messageLabel2.Padding = new Padding(30, 0, 30, 0);


            Button closeButton = createButton.CreateButtonWithImage(Image.FromFile(constants.cancelButton), "closeButton", constants.noStr, mainPanel.Width / 2 - 75, mainPanel.Height - 100, 150, 50, 0, 20, 14, FontStyle.Bold, Color.White, ContentAlignment.MiddleCenter, 2);
            mainPanel.Controls.Add(closeButton);
            closeButton.Click += new EventHandler(this.BackShow);

            dialogForm.ShowDialog();
        }

        public void ShowOpenTimeCancelMessage()
        {
            Form dialogForm = new Form();
            dialogForm.Size = new Size(width * 2 / 5, height / 3);
            dialogForm.StartPosition = FormStartPosition.CenterParent;
            dialogForm.WindowState = FormWindowState.Normal;
            dialogForm.ControlBox = false;
            dialogForm.FormBorderStyle = FormBorderStyle.None;
            dialogForm.BackgroundImage = Image.FromFile(constants.dialogFormImage);
            dialogForm.BackgroundImageLayout = ImageLayout.Stretch;
            DialogFormGlobal = dialogForm;

            Panel mainPanel = createPanel.CreateMainPanel(dialogForm, 0, 0, dialogForm.Width, dialogForm.Height, BorderStyle.None, Color.Transparent);

            Label messageLabel1 = createLabel.CreateLabelsInPanel(mainPanel, "messageLabel1", constants.openTimeCancelMessageTitle, 50, 0, mainPanel.Width - 100, (mainPanel.Height - 100) / 3, Color.Transparent, Color.Red, 22, false, ContentAlignment.BottomCenter);
            messageLabel1.Padding = new Padding(30, 0, 30, 0);
            Label messageLabel2 = createLabel.CreateLabelsInPanel(mainPanel, "messageLabel2", constants.openTimeCancelMessageContent, 50, (mainPanel.Height - 100) / 3, mainPanel.Width - 100, (mainPanel.Height - 100) * 2 / 3, Color.Transparent, Color.Black, 18, false, ContentAlignment.MiddleCenter);
            messageLabel2.Padding = new Padding(30, 0, 30, 0);

            Button settingButton = createButton.CreateButtonWithImage(Image.FromFile(constants.rectRedButton), "settingButton", constants.yesStr, mainPanel.Width / 8, mainPanel.Height - 100, mainPanel.Width / 4, 50, 0, 1, 14, FontStyle.Bold, Color.White, ContentAlignment.MiddleCenter, 1);
            mainPanel.Controls.Add(settingButton);
            settingButton.Click += new EventHandler(this.OpenTimeCancel);

            Button closeButton = createButton.CreateButtonWithImage(Image.FromFile(constants.rectBlueButton), "closeButton", constants.noStr, mainPanel.Width * 5 / 8, mainPanel.Height - 100, mainPanel.Width / 4, 50, 0, 1, 14, FontStyle.Bold, Color.White, ContentAlignment.MiddleCenter, 1);
            mainPanel.Controls.Add(closeButton);
            closeButton.Click += new EventHandler(this.BackShow);

            dialogForm.ShowDialog();

        }
        public void ShowOpenTimeSettingMessage()
        {
            Form dialogForm = new Form();
            dialogForm.Size = new Size(width * 2 / 5, height / 3);
            dialogForm.StartPosition = FormStartPosition.CenterParent;
            dialogForm.WindowState = FormWindowState.Normal;
            dialogForm.ControlBox = false;
            dialogForm.FormBorderStyle = FormBorderStyle.None;
            dialogForm.BackgroundImage = Image.FromFile(constants.dialogFormImage);
            dialogForm.BackgroundImageLayout = ImageLayout.Stretch;
            DialogFormGlobal = dialogForm;

            Panel mainPanel = createPanel.CreateMainPanel(dialogForm, 0, 0, dialogForm.Width, dialogForm.Height, BorderStyle.None, Color.Transparent);

            Label messageLabel1 = createLabel.CreateLabelsInPanel(mainPanel, "messageLabel1", constants.openTimeSettingMessageTitle, 50, 0, mainPanel.Width - 100, (mainPanel.Height - 100) / 3, Color.Transparent, Color.Red, 22, false, ContentAlignment.BottomCenter);
            messageLabel1.Padding = new Padding(30, 0, 30, 0);
            Label messageLabel2 = createLabel.CreateLabelsInPanel(mainPanel, "messageLabel2", constants.openTimeSettingMessageContent, 50, (mainPanel.Height - 100) / 3, mainPanel.Width - 100, (mainPanel.Height - 100) * 2 / 3, Color.Transparent, Color.Black, 18, false, ContentAlignment.MiddleCenter);
            messageLabel2.Padding = new Padding(30, 0, 30, 0);

            Button settingButton = createButton.CreateButtonWithImage(Image.FromFile(constants.rectRedButton), "settingButton", constants.yesStr, mainPanel.Width / 8, mainPanel.Height - 100, mainPanel.Width
                 / 4, 50, 0, 1, 14, FontStyle.Bold, Color.White, ContentAlignment.MiddleCenter, 1);
            mainPanel.Controls.Add(settingButton);
            settingButton.Click += new EventHandler(this.OpenTimeSetting);

            Button closeButton = createButton.CreateButtonWithImage(Image.FromFile(constants.rectBlueButton), "closeButton", constants.noStr, mainPanel.Width * 5 / 8, mainPanel.Height - 100, mainPanel.Width / 4, 50, 0, 1, 14, FontStyle.Bold, Color.White, ContentAlignment.MiddleCenter, 1);
            mainPanel.Controls.Add(closeButton);
            closeButton.Click += new EventHandler(this.BackShow);

            dialogForm.ShowDialog();

        }

        private void OpenTimeCancel(object sender, EventArgs e)
        {
            DialogFormGlobal.Close();
            openTimeChange.BackShowPage();
        }

        private void OpenTimeSetting(object sender, EventArgs e)
        {
            DialogFormGlobal.Close();
            openTimeChange.DateTimeSetting();
        }

        private void BackShow(object sender, EventArgs e)
        {
            DialogFormGlobal.Close();
        }
        private void BackShowMenuReading(object sender, EventArgs e)
        {
            DialogFormGlobal.Close();
            menuReading.BackShowStart();
        }

        private void CategoryPrintView(object sender, EventArgs e)
        {
            DialogFormGlobal.Close();
            categoryList.PrintPreview_click();
            //detailView.CategoryPrintView();
        }
        private void GroupPrintView(object sender, EventArgs e)
        {
            DialogFormGlobal.Close();
            groupList.btnprintpreview_Click();
            //detailView.GroupPrintView();
        }

        public void ShowDBErrorMessage()
        {
            Form dialogForm = new Form();
            dialogForm.TopLevel = true;
            dialogForm.TopMost = true;

            dialogForm.Size = new Size(width * 2 / 5, height / 3);
            dialogForm.BackColor = Color.White;
            dialogForm.StartPosition = FormStartPosition.CenterParent;
            dialogForm.WindowState = FormWindowState.Normal;
            dialogForm.ControlBox = false;
            dialogForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            DialogFormGlobal = dialogForm;

            Panel mainPanel = createPanel.CreateMainPanel(dialogForm, 0, 0, dialogForm.Width, dialogForm.Height, BorderStyle.None, Color.FromArgb(255, 255, 153, 204));

            Label messageLabel1 = createLabel.CreateLabelsInPanel(mainPanel, "messageLabel1", constants.dbErrorTitle, 50, 30, mainPanel.Width - 100, (mainPanel.Height - 150) / 2, Color.Red, Color.White, constants.fontSizeBig, false, ContentAlignment.MiddleCenter);
            messageLabel1.Padding = new Padding(30, 0, 30, 0);
            Label messageLabel2 = createLabel.CreateLabelsInPanel(mainPanel, "messageLabel2", constants.dbErrorContent, 50, messageLabel1.Bottom, mainPanel.Width - 100, (mainPanel.Height - 190) * 1 / 2, Color.Transparent, Color.Black, constants.fontSizeMedium, false, ContentAlignment.MiddleCenter);
            messageLabel2.Padding = new Padding(30, 0, 30, 0);


            Button closeButton = createButton.CreateButton(constants.noStr, "closeButton", mainPanel.Width / 2 - 75, mainPanel.Height - 100, 150, 50, Color.Red, Color.Transparent, 0, 1);
            closeButton.ForeColor = Color.White;
            mainPanel.Controls.Add(closeButton);
            closeButton.Click += new EventHandler(this.BackShow);

            dialogForm.ShowDialog();
            //dialogForm.Show();
        }


        public void CancelOrderMessage(object sender, EventArgs e)
        {
            Form dialogForm = new Form();
            dialogForm.Size = new Size(width * 2 / 5, height / 3);
            dialogForm.BackColor = Color.White;
            dialogForm.StartPosition = FormStartPosition.CenterParent;
            dialogForm.WindowState = FormWindowState.Normal;
            dialogForm.ControlBox = false;
            dialogForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            DialogFormGlobal = dialogForm;

            Panel mainPanel = createPanel.CreateMainPanel(dialogForm, 0, 0, dialogForm.Width, dialogForm.Height, BorderStyle.None, Color.White);

            Label messageLabel = createLabel.CreateLabelsInPanel(mainPanel, "messageLabel", constants.cancelErrorMessage, 50, 0, mainPanel.Width - 100, mainPanel.Height - 100, Color.White, Color.Black, 18, false, ContentAlignment.MiddleCenter);
            messageLabel.Padding = new Padding(30, 0, 30, 0);

            Button closeButton = createButton.CreateButton(constants.yesStr, "closeButton", mainPanel.Width / 2 - 75, mainPanel.Height - 100, 150, 50, Color.FromArgb(255, 0, 112, 192), Color.Transparent, 0, 1);
            closeButton.ForeColor = Color.White;
            mainPanel.Controls.Add(closeButton);
            closeButton.Click += new EventHandler(this.BackShow);

            dialogForm.ShowDialog();
        }
    }
}

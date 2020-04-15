﻿using System;
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
        DetailView detailView = new DetailView();
        CategoryList categoryList = null;
        GroupList groupList = null;
        MenuReading menuReading = null;

        Form DialogFormGlobal = null;
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
            dialogForm.BackColor = Color.White;
            dialogForm.StartPosition = FormStartPosition.CenterParent;
            dialogForm.WindowState = FormWindowState.Normal;
            dialogForm.ControlBox = false;
            dialogForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            DialogFormGlobal = dialogForm;

            Panel mainPanel = createPanel.CreateMainPanel(dialogForm, 0, 0, dialogForm.Width, dialogForm.Height, BorderStyle.None, Color.White);

            Label messageLabel = createLabel.CreateLabelsInPanel(mainPanel, "messageLabel", constants.categoryListPrintMessage, 50, 0, mainPanel.Width - 100, mainPanel.Height - 100, Color.White, Color.Black, 18, false, ContentAlignment.MiddleCenter);
            messageLabel.Padding = new Padding(30, 0, 30, 0);

            Button okButton = createButton.CreateButton(constants.yesStr, "categoryPrintOkButton", 50, messageLabel.Bottom, 150, 50, Color.Red, Color.Transparent, 0, 1);
            mainPanel.Controls.Add(okButton);
            okButton.ForeColor = Color.White;
            okButton.Click += new EventHandler(this.CategoryPrintView);

            Button closeButton = createButton.CreateButton(constants.noStr, "closeButton", mainPanel.Width - 200, messageLabel.Bottom, 150, 50, Color.FromArgb(255, 0, 112, 192), Color.Transparent, 0, 1);
            closeButton.ForeColor = Color.White;
            mainPanel.Controls.Add(closeButton);
            closeButton.Click += new EventHandler(this.BackShow);

            dialogForm.ShowDialog();
        }

        private void ShowGroupPrintMessage()
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

            Label messageLabel = createLabel.CreateLabelsInPanel(mainPanel, "messageLabel", constants.groupListPrintMessage, 50, 0, mainPanel.Width - 100, mainPanel.Height - 100, Color.White, Color.Black, 18, false, ContentAlignment.MiddleCenter);
            messageLabel.Padding = new Padding(30, 0, 30, 0);

            Button okButton = createButton.CreateButton(constants.yesStr, "categoryPrintOkButton", 50, mainPanel.Height - 100, 150, 50, Color.Red, Color.Transparent, 0, 1);
            okButton.ForeColor = Color.White;
            mainPanel.Controls.Add(okButton);
            okButton.Click += new EventHandler(this.GroupPrintView);

            Button closeButton = createButton.CreateButton(constants.noStr, "closeButton", mainPanel.Width - 200, mainPanel.Height - 100, 150, 50, Color.FromArgb(255, 0, 112, 192), Color.Transparent, 0, 1);
            closeButton.ForeColor = Color.White;
            mainPanel.Controls.Add(closeButton);
            closeButton.Click += new EventHandler(this.BackShow);

            dialogForm.ShowDialog();
        }
        public void ShowMenuReadingMessage()
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

            Label messageLabel1 = createLabel.CreateLabelsInPanel(mainPanel, "messageLabel1", constants.menuReadingErrorTitle, 50, 0, mainPanel.Width - 100, (mainPanel.Height - 100) / 3, Color.White, Color.Red, 22, false, ContentAlignment.BottomCenter);
            messageLabel1.Padding = new Padding(30, 0, 30, 0);
            Label messageLabel2 = createLabel.CreateLabelsInPanel(mainPanel, "messageLabel2", constants.menuReadingErrorContent, 50, (mainPanel.Height - 100) / 3, mainPanel.Width - 100, (mainPanel.Height - 100) * 2 /3, Color.White, Color.Black, 18, false, ContentAlignment.MiddleCenter);
            messageLabel2.Padding = new Padding(30, 0, 30, 0);


            Button closeButton = createButton.CreateButton(constants.noStr, "closeButton", mainPanel.Width / 2 - 75, mainPanel.Height - 100, 150, 50, Color.Red, Color.Transparent, 0, 1);
            closeButton.ForeColor = Color.White;
            mainPanel.Controls.Add(closeButton);
            closeButton.Click += new EventHandler(this.BackShowMenuReading);

            dialogForm.ShowDialog();
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
    }
}
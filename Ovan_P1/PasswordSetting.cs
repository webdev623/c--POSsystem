using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ovan_P1
{
    public partial class PasswordSetting : Form
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
        CreateTextBox createTextBox = new CreateTextBox();
        DetailView detailView = new DetailView();

        private TextBox[] tbBoxGlobal = new TextBox[3];
        private TextBox focusedTbBox = null;
        private int cursorPositionGlobal = 0;
        public PasswordSetting(Form1 mainForm, Panel mainPanel)
        {
            mainFormGlobal = mainForm;
            mainPanelGlobal = mainPanel;


            Panel headerPanel = createPanel.CreateSubPanel(mainPanel, 0, 0, mainPanel.Width, mainPanel.Height / 10, BorderStyle.None, Color.FromArgb(255, 234, 225, 151));
            Label headerLabel = createLabel.CreateLabelsInPanel(headerPanel, "headerLabel", constants.passwordSettingLabel, 0, 0, headerPanel.Width, headerPanel.Height, Color.Transparent, Color.Black, 28, false, ContentAlignment.MiddleCenter);

            DateTime now = DateTime.Now;

            Panel bodyPanel = createPanel.CreateSubPanel(mainPanel, 0, headerPanel.Bottom, mainPanel.Width, mainPanel.Height * 9 / 10, BorderStyle.None, Color.Transparent);

            FlowLayoutPanel tableHeaderInUpPanel = createPanel.CreateFlowLayoutPanel(bodyPanel, bodyPanel.Width / 7, bodyPanel.Height / 12, bodyPanel.Width * 5 / 7, bodyPanel.Height / 6, Color.White, new Padding(30, bodyPanel.Height / 30, 30, bodyPanel.Height / 30));
            Label currentPasswordLabel = createLabel.CreateLabels(tableHeaderInUpPanel, "currentPasswordLabel", constants.oldPasswordLabel, 0, 0, tableHeaderInUpPanel.Width * 2 / 5 - 40, tableHeaderInUpPanel.Height * 3 / 5, Color.Transparent, Color.Black, 18, false, ContentAlignment.MiddleLeft, new Padding(0), 1, Color.Gray);
            TextBox currentPasswordTBox = createTextBox.CreateTextBoxs(tableHeaderInUpPanel, "currentPasswordTBox", currentPasswordLabel.Right, 0, tableHeaderInUpPanel.Width * 3 / 5 - 40, tableHeaderInUpPanel.Height * 3 / 5, 24, BorderStyle.FixedSingle);
            currentPasswordTBox.Margin = new Padding(0, 15, 0, 0);
            tbBoxGlobal[0] = currentPasswordTBox;
            currentPasswordTBox.GotFocus += new EventHandler(this.GetFocus);
            currentPasswordTBox.Focus();

            FlowLayoutPanel tableHeaderInUpPanel2 = createPanel.CreateFlowLayoutPanel(bodyPanel, bodyPanel.Width / 7, bodyPanel.Height * 19 / 60, bodyPanel.Width * 5 / 7, bodyPanel.Height / 6, Color.White, new Padding(30, bodyPanel.Height / 30, 30, bodyPanel.Height / 30));
            Label newPasswordLabel = createLabel.CreateLabels(tableHeaderInUpPanel2, "newPasswordLabel", constants.newPasswordLabel, 0, 0, tableHeaderInUpPanel2.Width * 2 / 5 - 40, tableHeaderInUpPanel2.Height * 3 / 5, Color.White, Color.Black, 18, false, ContentAlignment.MiddleLeft, new Padding(0), 1, Color.Gray);
            TextBox newPasswordTBox = createTextBox.CreateTextBoxs(tableHeaderInUpPanel2, "newPasswordTBox", newPasswordLabel.Right, 0, tableHeaderInUpPanel2.Width * 3 / 5 - 40, tableHeaderInUpPanel2.Height * 3 / 5, 24, BorderStyle.FixedSingle);
            newPasswordTBox.Margin = new Padding(0, 15, 0, 0);
            tbBoxGlobal[1] = newPasswordTBox;
            newPasswordTBox.GotFocus += new EventHandler(this.GetFocus);

            FlowLayoutPanel tableHeaderInUpPanel3 = createPanel.CreateFlowLayoutPanel(bodyPanel, bodyPanel.Width / 7, bodyPanel.Height * 11 / 20, bodyPanel.Width * 5 / 7, bodyPanel.Height / 6, Color.White, new Padding(30, bodyPanel.Height / 30, 30, bodyPanel.Height / 30));
            Label newPasswordConfirmLabel = createLabel.CreateLabels(tableHeaderInUpPanel3, "newPasswordConfirmLabel", constants.confirmPasswordLabel, 0, 0, tableHeaderInUpPanel3.Width * 2 / 5 - 40, tableHeaderInUpPanel3.Height * 3 / 5, Color.White, Color.Black, 18, false, ContentAlignment.MiddleLeft, new Padding(0), 1, Color.Gray);
            TextBox newPasswordConfirmTBox = createTextBox.CreateTextBoxs(tableHeaderInUpPanel3, "confirmPasswordTBox", newPasswordConfirmLabel.Right, 0, tableHeaderInUpPanel3.Width * 3 / 5 - 40, tableHeaderInUpPanel3.Height * 3 / 5, 24, BorderStyle.FixedSingle);
            newPasswordConfirmTBox.Margin = new Padding(0, 15, 0, 0);
            tbBoxGlobal[2] = newPasswordConfirmTBox;
            newPasswordConfirmTBox.GotFocus += new EventHandler(this.GetFocus);

            FlowLayoutPanel numberPanel = createPanel.CreateFlowLayoutPanel(bodyPanel, bodyPanel.Width / 7, bodyPanel.Height * 3 / 4, bodyPanel.Width * 30 / 49, 50, Color.Transparent, new Padding(0));

            for(int k = 0; k < 10; k++)
            {

                Button numberButton = customButton.CreateButtonWithImage(Image.FromFile(constants.numberButtonImage), "numberButton_" + k, k.ToString(), (numberPanel.Width / 10) * k + 10, 0, numberPanel.Width / 10 - 10, 50, 0, 1, 22, FontStyle.Bold, Color.Black, ContentAlignment.MiddleCenter, 0);

                numberButton.Margin = new Padding(0, 0, 10, 0);
                numberButton.FlatStyle = FlatStyle.Flat;
                numberButton.FlatAppearance.BorderSize = 0;
                numberButton.Click += new EventHandler(this.InputNumber);

                numberPanel.Controls.Add(numberButton);
           //     Label numberLabel = createLabel.CreateLabels(numberPanel, "numberLabel_" + k, k.ToString(), (numberPanel.Width / 10) * k, 0, numberPanel.Width / 10 - 10, 50, Color.FromArgb(255, 255, 183, 67), Color.Black, 16, false, ContentAlignment.MiddleCenter, new Padding(0, 0, 10, 0), 1, Color.Gray);
            }

            FlowLayoutPanel settingPanel = createPanel.CreateFlowLayoutPanel(bodyPanel, bodyPanel.Width / 7, numberPanel.Bottom + bodyPanel.Height / 30, bodyPanel.Width * 30 / 49, 50, Color.Transparent, new Padding(0));

            Button prevButton = customButton.CreateButtonWithImage(Image.FromFile(constants.prevkeyButtonImage), "prevButton", "", 0, 0, numberPanel.Width / 5 - 10, 50, 0, 1, 22, FontStyle.Bold, Color.Black, ContentAlignment.MiddleCenter, 0);

            prevButton.Margin = new Padding(0, 0, 10, 0);
            prevButton.FlatStyle = FlatStyle.Flat;
            prevButton.FlatAppearance.BorderSize = 0;

            prevButton.Click += new EventHandler(this.CursorMove);

            settingPanel.Controls.Add(prevButton);

            Button charClearButton = customButton.CreateButtonWithImage(Image.FromFile(constants.clearkeyButtonImage), "charClearButton", constants.charClearLabel, settingPanel.Width / 5, 0, settingPanel.Width * 3 / 10 - 10, 50, 0, 1, 18, FontStyle.Bold, Color.Black, ContentAlignment.MiddleCenter, 0);

            charClearButton.Margin = new Padding(0, 0, 10, 0);
            charClearButton.FlatStyle = FlatStyle.Flat;
            charClearButton.FlatAppearance.BorderSize = 0;

            charClearButton.Click += new EventHandler(this.CharClear);

            settingPanel.Controls.Add(charClearButton);

            Button allClearButton = customButton.CreateButtonWithImage(Image.FromFile(constants.clearkeyButtonImage), "allClearButton", constants.allClearLabel, settingPanel.Width / 2, 0, settingPanel.Width * 3 / 10 - 10, 50, 0, 1, 18, FontStyle.Bold, Color.Black, ContentAlignment.MiddleCenter, 0);

            allClearButton.Margin = new Padding(0, 0, 10, 0);
            allClearButton.FlatStyle = FlatStyle.Flat;
            allClearButton.FlatAppearance.BorderSize = 0;

            allClearButton.Click += new EventHandler(this.AllClear);

            settingPanel.Controls.Add(allClearButton);


            Button nextButton = customButton.CreateButtonWithImage(Image.FromFile(constants.nextkeyButtonImage), "nextButton", "", settingPanel.Width * 4 / 5, 0, settingPanel.Width / 5 - 10, 50, 0, 1, 22, FontStyle.Bold, Color.Black, ContentAlignment.MiddleCenter, 0);
            nextButton.Margin = new Padding(0, 0, 10, 0);
            nextButton.FlatStyle = FlatStyle.Flat;
            nextButton.FlatAppearance.BorderSize = 0;

            nextButton.Click += new EventHandler(this.CursorMove);

            settingPanel.Controls.Add(nextButton);

            Image backImage = Image.FromFile(constants.soldoutButtonImage1);

            Button backButton = customButton.CreateButtonWithImage(backImage, "settingButton", constants.settingLabel, settingPanel.Right + bodyPanel.Width * 5 / 49 - 100, numberPanel.Bottom + bodyPanel.Height / 30, 100, 50, 1, 10, 14, FontStyle.Bold, Color.White, ContentAlignment.MiddleCenter, 2);

            bodyPanel.Controls.Add(backButton);
            backButton.Click += new EventHandler(this.SetBackShow);
        }

        private void GetFocus(object sender, EventArgs e)
        {
            TextBox tbBox = (TextBox)sender;
            focusedTbBox = tbBox;
            focusedTbBox.SelectionLength = 0;
            cursorPositionGlobal = focusedTbBox.SelectionStart;

        }
        public void SetBackShow(object sender, EventArgs e)
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\WinRegistry");
            if(key != null)
            {
                if (key.GetValue("POSPassword") == null)
                {
                    key.SetValue("POSPassword", "");
                }
                string pwd = key.GetValue("POSPassword").ToString();

                if (pwd != null && pwd != "")
                {
                    if(tbBoxGlobal[0].Text == "")
                    {
                        MessageBox.Show("Please enter current password.");
                    }
                    else if(tbBoxGlobal[0].Text != pwd)
                    {
                        MessageBox.Show("Please enter valid current password.");
                    }
                    else if(tbBoxGlobal[1].Text != tbBoxGlobal[2].Text)
                    {
                        MessageBox.Show("Please check your new password");
                    }
                    else
                    {
                        key.SetValue("POSPassword", tbBoxGlobal[1].Text);

                        MessageBox.Show("Password setting successfully.");

                        mainPanelGlobal.Controls.Clear();
                        MaintaneceMenu frm = new MaintaneceMenu(mainFormGlobal, mainPanelGlobal);
                        frm.TopLevel = false;
                        mainPanelGlobal.Controls.Add(frm);
                        frm.FormBorderStyle = FormBorderStyle.None;
                        frm.Dock = DockStyle.Fill;
                        Thread.Sleep(200);
                        frm.Show();

                    }
                }
                else
                {
                    if (tbBoxGlobal[1].Text != tbBoxGlobal[2].Text)
                    {
                        MessageBox.Show("Please check your new password");
                    }
                    else
                    {
                        key.SetValue("POSPassword", tbBoxGlobal[1].Text);

                        MessageBox.Show("Password setting successfully.");

                        mainPanelGlobal.Controls.Clear();
                        MaintaneceMenu frm = new MaintaneceMenu(mainFormGlobal, mainPanelGlobal);
                        frm.TopLevel = false;
                        mainPanelGlobal.Controls.Add(frm);
                        frm.FormBorderStyle = FormBorderStyle.None;
                        frm.Dock = DockStyle.Fill;
                        Thread.Sleep(200);
                    }
                }
            }
            else
            {
                MessageBox.Show("Registry Error.");
            }

        }

        private void AllClear(object sender, EventArgs e)
        {
            
            focusedTbBox.Text = "";
            focusedTbBox.Focus();
            focusedTbBox.SelectionStart = 0;
            focusedTbBox.SelectionLength = 0;
        }

        private void CharClear(object sender, EventArgs e)
        {
            Button btnTemp = (Button)sender;
            int selectionIndex = focusedTbBox.SelectionStart;
            focusedTbBox.Text = focusedTbBox.Text.Remove(selectionIndex - 1, 1);
            focusedTbBox.Focus();
            focusedTbBox.SelectionStart = selectionIndex - 1;
            focusedTbBox.SelectionLength = 0;
        }
        private void CursorMove(object sender, EventArgs e)
        {
            Button btnTemp = (Button)sender;
            focusedTbBox.Focus();
            int cursorPosition = cursorPositionGlobal;
            if (btnTemp.Name == "prevButton")
            {
                if (cursorPosition > 0)
                {
                    cursorPosition--;
                    focusedTbBox.SelectionStart = cursorPosition;
                    focusedTbBox.SelectionLength = 0;
                    cursorPositionGlobal = cursorPosition;
                }
            }
            else if (btnTemp.Name == "nextButton")
            {
                if(cursorPositionGlobal < focusedTbBox.Text.Length)
                {
                    cursorPositionGlobal++;
                    focusedTbBox.SelectionStart = cursorPositionGlobal;
                    focusedTbBox.SelectionLength = 0;
                }
            }

        }

        private void InputNumber(object sender, EventArgs e)
        {
            Button btnTemp = (Button)sender;
            string inputValue = btnTemp.Name.Split('_')[1];
            int selectionIndex = focusedTbBox.SelectionStart;
            focusedTbBox.Text = focusedTbBox.Text.Insert(selectionIndex, inputValue);
            focusedTbBox.Focus();
            focusedTbBox.SelectionStart = selectionIndex + 1;
            focusedTbBox.SelectionLength = 0;

        }

    }
}

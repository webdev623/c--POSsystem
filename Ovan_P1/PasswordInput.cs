using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ovan_P1
{
    class PasswordInput
    {
        int height = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height;
        int width = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width;
        Constant constants = new Constant();
        CreatePanel createPanel = new CreatePanel();
        CreateLabel createLabel = new CreateLabel();
        Form dialogFormGlobal = null;
        TextBox inputValueGlobal = null;
        MainMenu mainMenuGlobal = null;
        string objectNameGlobal = "";
        string objectHandlerNameGlobal = "";

        public void initMainMenu(MainMenu sendHandler)
        {
            mainMenuGlobal = sendHandler;
        }
        public void CreateNumberInputDialog(string objectName, string objectHandlerName)
        {
            objectHandlerNameGlobal = objectHandlerName;
            objectNameGlobal = objectName;
            Form dialogForm = new Form();
            dialogForm.Size = new Size(width / 5, height * 2 / 5 + 30);
            dialogForm.BackColor = Color.White;
            dialogForm.StartPosition = FormStartPosition.CenterParent;
            dialogForm.WindowState = FormWindowState.Normal;
            dialogForm.ControlBox = false;
            dialogForm.FormBorderStyle = FormBorderStyle.None;
            dialogFormGlobal = dialogForm;

            Panel mainPanel = createPanel.CreateMainPanel(dialogForm, 0, 0, dialogForm.Width, dialogForm.Height, BorderStyle.FixedSingle, Color.FromArgb(255, 147, 205, 221));

            Label inputTitle = createLabel.CreateLabelsInPanel(mainPanel, "inputTitle", constants.passwordInputTitle, 0, 0, mainPanel.Width, 60, Color.Transparent, Color.Black, 22, false, ContentAlignment.BottomCenter);

            TextBox inputValueShow = new TextBox();
            inputValueShow.Location = new Point(mainPanel.Width / 6, 70);
            inputValueShow.Size = new Size(mainPanel.Width * 2 / 3, 50);
            inputValueShow.Font = new Font("Microsoft Sans Serif", 24F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            inputValueShow.BorderStyle = BorderStyle.None;
            inputValueShow.TextAlign = HorizontalAlignment.Right;
            mainPanel.Controls.Add(inputValueShow);
            inputValueGlobal = inputValueShow;

            Panel keyboardPanel = createPanel.CreateSubPanel(mainPanel, mainPanel.Width / 9, inputValueShow.Bottom + 10, mainPanel.Width * 7 / 9, mainPanel.Height - inputValueShow.Bottom - 40, BorderStyle.FixedSingle, Color.FromArgb(255, 0, 176, 80));

            for (int k = 3; k >= 0; k--)
            {
                if (k != 0)
                {
                    for (int m = 0; m < 3; m++)
                    {
                        int keyValue = 3 * k - (2 - m);
                        Label numberKeyLabel = createLabel.CreateLabelsInPanel(keyboardPanel, keyValue.ToString(), keyValue.ToString(), keyboardPanel.Width * m / 3, keyboardPanel.Height * (3 - k) / 4, keyboardPanel.Width / 3, keyboardPanel.Height / 4, Color.Transparent, Color.Black, 12, true);
                        numberKeyLabel.BorderStyle = BorderStyle.Fixed3D;
                        numberKeyLabel.Click += new EventHandler(this.InputValueAdd);
                        numberKeyLabel.MouseHover += new EventHandler(this.KeyHover);
                        numberKeyLabel.MouseLeave += new EventHandler(this.KeyLeave);
                    }
                }
                else
                {
                    Label numberKeyLabel = createLabel.CreateLabelsInPanel(keyboardPanel, "0", "0", 0, keyboardPanel.Height * (3 - k) / 4, keyboardPanel.Width / 3, keyboardPanel.Height / 4, Color.Transparent, Color.Black, 12, true);
                    numberKeyLabel.BorderStyle = BorderStyle.Fixed3D;
                    numberKeyLabel.Click += new EventHandler(this.InputValueAdd);
                    numberKeyLabel.MouseHover += new EventHandler(this.KeyHover);
                    numberKeyLabel.MouseLeave += new EventHandler(this.KeyLeave);

                    Label numberKeyLabel_del = createLabel.CreateLabelsInPanel(keyboardPanel, "Del", "Del", keyboardPanel.Width / 3, keyboardPanel.Height * (3 - k) / 4, keyboardPanel.Width / 3, keyboardPanel.Height / 4, Color.Transparent, Color.Black, 12, true);
                    numberKeyLabel_del.BorderStyle = BorderStyle.Fixed3D;
                    numberKeyLabel_del.Click += new EventHandler(this.InputValueAdd);
                    numberKeyLabel_del.MouseHover += new EventHandler(this.KeyHover);
                    numberKeyLabel_del.MouseLeave += new EventHandler(this.KeyLeave);

                    Label numberKeyLabel_ok = createLabel.CreateLabelsInPanel(keyboardPanel, "Ok", "Ok", keyboardPanel.Width * 2 / 3, keyboardPanel.Height * (3 - k) / 4, keyboardPanel.Width / 3, keyboardPanel.Height / 4, Color.Transparent, Color.Black, 12, true);
                    numberKeyLabel_ok.BorderStyle = BorderStyle.Fixed3D;
                    numberKeyLabel_ok.Click += new EventHandler(this.InputValueAdd);
                    numberKeyLabel_ok.MouseHover += new EventHandler(this.KeyHover);
                    numberKeyLabel_ok.MouseLeave += new EventHandler(this.KeyLeave);
                }
            }

            dialogForm.ShowDialog();
        }

        private void GetFocus(object sender, EventArgs e)
        {
            TextBox tbBox = (TextBox)sender;
            inputValueGlobal = tbBox;
            inputValueGlobal.SelectionLength = 0;

        }

        private void InputValueAdd(object sender, EventArgs e)
        {
            Label keyLabel = (Label)sender;
            string keyText = keyLabel.Text;
            if (keyText != "Del" && keyText != "Ok")
            {
                int selectionIndex = inputValueGlobal.SelectionStart;
                inputValueGlobal.Text = inputValueGlobal.Text.Insert(selectionIndex, keyText);
                inputValueGlobal.Focus();
                inputValueGlobal.SelectionStart = selectionIndex + 1;
                inputValueGlobal.SelectionLength = 0;

            }
            else
            {
                if (keyText == "Del")
                {
                    inputValueGlobal.Text = "";
                }
                else
                {
                    string sendText = inputValueGlobal.Text;

                    switch (objectNameGlobal)
                    {
                        case "maintenance":
                            mainMenuGlobal.getPassword(objectNameGlobal, sendText);
                            dialogFormGlobal.Close();
                            break;
                        case "readingmenu":
                            mainMenuGlobal.getPassword(objectNameGlobal, sendText);
                            dialogFormGlobal.Close();
                            break;
                    }
                }
            }
        }

        private void KeyHover(object sender, EventArgs e)
        {
            Label keyLabel = (Label)sender;
            keyLabel.BackColor = Color.FromArgb(10, 10, 0, 0);
        }
        private void KeyLeave(object sender, EventArgs e)
        {
            Label keyLabel = (Label)sender;
            keyLabel.BackColor = Color.Transparent;
        }

    }
}

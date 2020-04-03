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
    public partial class ClosingProcess : Form
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
        Button[] menuButtonGlobal = new Button[7];

        DetailView detailView = new DetailView();

        bool manualProcessState = false;
        public ClosingProcess(Form1 mainForm, Panel mainPanel)
        {
            mainFormGlobal = mainForm;
            mainPanelGlobal = mainPanel;
            Panel headerPanel = createPanel.CreateMainPanel(mainForm, 0, 0, width, height / 5, BorderStyle.None, Color.Transparent);
            Panel bodyPanel = createPanel.CreateMainPanel(mainForm, 0, headerPanel.Bottom, width, height * 4 / 5, BorderStyle.None, Color.Transparent);
            for (int i = 0; i < 4; i++)
            {
                FlowLayoutPanel menuFlowLayoutPanel = createPanel.CreateFlowLayoutPanel(bodyPanel, width / 4, bodyPanel.Height / 9 + (bodyPanel.Height / 6) * i, width * 3 / 5, bodyPanel.Height / 8, Color.Transparent, new Padding(0, 0, 0, 0));
                menuFlowLayoutPanel.Margin = new Padding(30);

                menuFlowLayoutPanelGlobal[i] = menuFlowLayoutPanel;

                Label menuLabel = createLabel.CreateLabels(menuFlowLayoutPanel, "processLabel_" + i, constants.closingProcessLabel[i], 100, menuFlowLayoutPanel.Height * i, menuFlowLayoutPanel.Width / 5 - 10, menuFlowLayoutPanel.Height, Color.Transparent, Color.Red, 18, false, ContentAlignment.MiddleLeft);

                Image btnImage = Image.FromFile(constants.closingProcessButtonImage[i]);

                Color btnBackColor1 = Color.FromArgb(255, 229, 229, 229);
                Color btnBackColor2 = Color.FromArgb(255, 229, 229, 229);
                Color btnForeColor = Color.Black;
                bool btnEnable = false;

                if (manualProcessState)
                {
                    btnBackColor1 = Color.FromArgb(255, 0, 176, 80);
                    btnBackColor2 = Color.FromArgb(255, 0, 160, 230);
                    btnForeColor = Color.White;
                    btnEnable = true;
                }
                if (i == 0)
                {
                    if (manualProcessState)
                    {
                        Button menuButton_1 = customButton.CreateButton(constants.closingProcessButton[i][0], "processButton_" + i + "_1", menuLabel.Right, menuFlowLayoutPanel.Height * i, menuFlowLayoutPanel.Width / 3 - 50, menuFlowLayoutPanel.Height, Color.FromArgb(255, 229, 229, 229), Color.Transparent, 0);
                        menuFlowLayoutPanel.Controls.Add(menuButton_1);

                        menuButton_1.Margin = new Padding(0, 0, 30, 0);

                        Button menuButton_2 = customButton.CreateButton(constants.closingProcessButton[i][1], "processButton_" + i + "_2", menuButton_1.Right + 30, menuFlowLayoutPanel.Height * i, menuFlowLayoutPanel.Width / 3 - 50, menuFlowLayoutPanel.Height, Color.FromArgb(255, 0, 160, 230), Color.Transparent, 0, 1, 12, FontStyle.Regular, Color.White);
                        menuFlowLayoutPanel.Controls.Add(menuButton_2);

                        menuButton_2.Margin = new Padding(30, 0, 0, 0);
                        menuButton_2.Enabled = true;

                        menuButtonGlobal[i * 2] = menuButton_1;
                        menuButtonGlobal[i * 2 + 1] = menuButton_2;

                        menuButton_1.Click += new EventHandler(this.manualProcess);
                        menuButton_2.Click += new EventHandler(detailView.DetailViewIndicator);

                    }
                    else
                    {
                        Button menuButton_1 = customButton.CreateButton(constants.closingProcessButton[i][0], "processButton_" + i + "_1", menuLabel.Right, menuFlowLayoutPanel.Height * i, menuFlowLayoutPanel.Width / 3 - 50, menuFlowLayoutPanel.Height, Color.FromArgb(255, 0, 160, 230), Color.Transparent, 0, 1, 12, FontStyle.Regular, Color.White);
                        menuFlowLayoutPanel.Controls.Add(menuButton_1);

                        menuButton_1.Margin = new Padding(0, 0, 30, 0);


                        Button menuButton_2 = customButton.CreateButton(constants.closingProcessButton[i][1], "processButton_" + i + "_2", menuButton_1.Right + 30, menuFlowLayoutPanel.Height * i, menuFlowLayoutPanel.Width / 3 - 50, menuFlowLayoutPanel.Height, Color.FromArgb(255, 229, 229, 229), Color.Transparent, 0, 1);
                        menuFlowLayoutPanel.Controls.Add(menuButton_2);

                        menuButton_2.Margin = new Padding(30, 0, 0, 0);
                        menuButton_2.Enabled = false;

                        menuButtonGlobal[i * 2] = menuButton_1;
                        menuButtonGlobal[i * 2 + 1] = menuButton_2;

                        menuButton_1.Click += new EventHandler(this.manualProcess);
                        menuButton_2.Click += new EventHandler(detailView.DetailViewIndicator);

                    }
                }

                else if (i < 3)
                {
                    Button menuButton_1 = customButton.CreateButton(constants.closingProcessButton[i][0], "processButton_" + i + "_1", menuLabel.Right, menuFlowLayoutPanel.Height * i, menuFlowLayoutPanel.Width / 3 - 50, menuFlowLayoutPanel.Height, btnBackColor1, Color.Transparent, 0, 1, 12, FontStyle.Regular, btnForeColor);
                    menuFlowLayoutPanel.Controls.Add(menuButton_1);

                    menuButton_1.Margin = new Padding(0, 0, 30, 0);
                    menuButton_1.Enabled = btnEnable;
                    menuButton_1.Click += new EventHandler(detailView.DetailViewIndicator);

                    Button menuButton_2 = customButton.CreateButton(constants.closingProcessButton[i][1], "processButton_" + i + "_2", menuButton_1.Right + 90, menuFlowLayoutPanel.Height * i, menuFlowLayoutPanel.Width / 3 - 50, menuFlowLayoutPanel.Height, btnBackColor2, Color.Transparent, 0, 1, 12, FontStyle.Regular, btnForeColor);
                    menuFlowLayoutPanel.Controls.Add(menuButton_2);

                    menuButtonGlobal[i * 2] = menuButton_1;
                    menuButtonGlobal[i * 2 + 1] = menuButton_2;

                    menuButton_2.Margin = new Padding(30, 0, 0, 0);
                    menuButton_2.Enabled = btnEnable;

                    menuButton_2.Click += new EventHandler(detailView.DetailViewIndicator);
                }
                else
                {
                    Button menuButton_1 = customButton.CreateButton(constants.closingProcessButton[i][0], "processButton_" + i + "_1", menuLabel.Right, menuFlowLayoutPanel.Height * i, menuFlowLayoutPanel.Width / 3 - 50, menuFlowLayoutPanel.Height, Color.FromArgb(255, 0, 176, 80), Color.Transparent, 0, 1, 12, FontStyle.Regular, Color.White);
                    menuFlowLayoutPanel.Controls.Add(menuButton_1);

                    menuButtonGlobal[i * 2] = menuButton_1;

                    menuButton_1.Margin = new Padding(0, 0, 30, 0);

                    menuButton_1.Click += new EventHandler(detailView.DetailViewIndicator);
                }
            }

            Button backButton = customButton.CreateButton(constants.backText, "backButton", width - 250, height - 400, 100, 50, Color.FromArgb(255, 0, 112, 192), Color.Transparent, 0, 10, 14, FontStyle.Bold, Color.White);
            bodyPanel.Controls.Add(backButton);
            backButton.Click += new EventHandler(this.BackShow);

            InitializeComponent();
        }

        public void BackShow(object sender, EventArgs e)
        {
            mainFormGlobal.Controls.Clear();
            MaintaneceMenu frm = new MaintaneceMenu(mainFormGlobal, mainPanelGlobal);
            frm.TopLevel = false;
            mainFormGlobal.Controls.Add(frm);
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;
            Thread.Sleep(200);
            frm.Show();
        }

        private void manualProcess(object sender, EventArgs e)
        {
            bool manualProcessHandler = manualProcessState;
            manualProcessState = !manualProcessHandler;
            ButtonBackChange();
        }

        private void ButtonBackChange()
        {
            if (manualProcessState)
            {
                menuButtonGlobal[0].BackColor = Color.FromArgb(255, 229, 229, 229);
                menuButtonGlobal[0].ForeColor = Color.Black;
            }
            else
            {
                menuButtonGlobal[0].BackColor = Color.FromArgb(255, 0, 160, 230);
                menuButtonGlobal[0].ForeColor = Color.White;
            }
            for(int i = 1; i < 6; i++)
            {
                if (manualProcessState)
                {
                    if(i % 2 == 0)
                    {
                        menuButtonGlobal[i].BackColor = Color.FromArgb(255, 0, 176, 80);
                    }
                    else
                    {
                        menuButtonGlobal[i].BackColor = Color.FromArgb(255, 0, 160, 230);
                    }
                    menuButtonGlobal[i].Enabled = true;
                    menuButtonGlobal[i].ForeColor = Color.White;
                }
                else
                {
                    menuButtonGlobal[i].Enabled = false;
                    menuButtonGlobal[i].BackColor = Color.FromArgb(255, 229, 229, 229); 
                    menuButtonGlobal[i].ForeColor = Color.Black;
                }
            }
        }



    }
}

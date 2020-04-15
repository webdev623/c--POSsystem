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
    public partial class OpenTimeChange : Form
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
        CreateCombobox createCombobox = new CreateCombobox();
        CustomButton customButton = new CustomButton();
        DropDownMenu dropDownMenu = new DropDownMenu();
        MonthDropDown dropDownMonth = new MonthDropDown();
        DateDropDown dropDownDate = new DateDropDown();

        DetailView detailView = new DetailView();
        ComboBox yearComboboxGlobal = null;
        ComboBox monthComboboxGlobal = null;
        ComboBox dateComboboxGlobal = null;
        public OpenTimeChange(Form1 mainForm, Panel mainPanel)
        {
            InitializeComponent();
            mainForm.Width = width;
            mainForm.Height = height;
            mainFormGlobal = mainForm;
            mainPanelGlobal = mainPanel;
            mainForm.AutoScroll = true;

            DateTime now = DateTime.Now;

            Panel headerPanel = createPanel.CreateMainPanel(mainForm, 0, 0, width, height / 5, BorderStyle.None, Color.Transparent);
            Label headerTitle = createLabel.CreateLabelsInPanel(headerPanel, "headerTitle", constants.openTimeChangeTitle, 50, 60, 200, 50, Color.Transparent, Color.Red, 32);

            Panel bodyPanel = createPanel.CreateMainPanel(mainForm, 0, headerPanel.Bottom, width, height * 4 / 5, BorderStyle.None, Color.Transparent);

            Label subTitle = createLabel.CreateLabelsInPanel(bodyPanel, "subTitle", constants.currentDateLabel + " : " + now.ToLocalTime().ToString("yyyy/MM/dd HH:mm"), 0, 10, bodyPanel.Width, 50, Color.Transparent, Color.Black, 22);
            subTitle.Padding = new Padding(100, 0, 0, 0);
            subTitle.TextAlign = ContentAlignment.MiddleLeft;

            Label dayTypeLabel = createLabel.CreateLabelsInPanel(bodyPanel, "dayTypeLabel", constants.dayType, bodyPanel.Width / 5, subTitle.Bottom + 30, bodyPanel.Width / 6 - 30, 40, Color.Transparent, Color.Black, 18);
            dayTypeLabel.TextAlign = ContentAlignment.MiddleLeft;
            ComboBox dayTypeComboBox = createCombobox.CreateComboboxs(bodyPanel, "dayTypeComboBox", constants.dayTypeValue, dayTypeLabel.Right, subTitle.Bottom + 30, 250, 40, 25, new Font("Comic Sans", 18));
            dayTypeComboBox.DrawItem += new DrawItemEventHandler(createCombobox.dateCombobox_DrawItem);
            dayTypeComboBox.MeasureItem += new MeasureItemEventHandler(createCombobox.dateCombobox_MeasureItem);


            Label startLabel = createLabel.CreateLabelsInPanel(bodyPanel, "startLabel", constants.startTimeLabel, bodyPanel.Width / 5, dayTypeLabel.Bottom + 30, bodyPanel.Width / 6 - 30, 40, Color.Transparent, Color.Black, 18);
            startLabel.TextAlign = ContentAlignment.MiddleLeft;
            ComboBox startHour = createCombobox.CreateComboboxs(bodyPanel, "startHour", constants.times, startLabel.Right, dayTypeLabel.Bottom + 30, 150, 40, 25, new Font("Comic Sans", 18));
            startHour.DrawItem += new DrawItemEventHandler(createCombobox.dateCombobox_DrawItem);
            startHour.MeasureItem += new MeasureItemEventHandler(createCombobox.dateCombobox_MeasureItem);
            Label startHourUnit = createLabel.CreateLabelsInPanel(bodyPanel, "startHourUnit", constants.hourLabel, startHour.Right + 10, dayTypeLabel.Bottom + 30, 80, 40, Color.Transparent, Color.Black, 18);

            ComboBox startMinute = createCombobox.CreateComboboxs(bodyPanel, "startMinute", constants.minutes, startHourUnit.Right + 10, dayTypeLabel.Bottom + 30, 150, 40, 25, new Font("Comic Sans", 18));
            Label startMinuteUnit = createLabel.CreateLabelsInPanel(bodyPanel, "startMinuteUnit", constants.minuteLabel, startMinute.Right + 10, dayTypeLabel.Bottom + 30, 80, 40, Color.Transparent, Color.Black, 18, false, ContentAlignment.MiddleLeft);
            startMinute.DrawItem += new DrawItemEventHandler(createCombobox.dateCombobox_DrawItem);
            startMinute.MeasureItem += new MeasureItemEventHandler(createCombobox.dateCombobox_MeasureItem);

            Label endLabel = createLabel.CreateLabelsInPanel(bodyPanel, "endLabel", constants.endTimeLabel, bodyPanel.Width / 5, startLabel.Bottom + 30, bodyPanel.Width / 6 - 30, 40, Color.Transparent, Color.Black, 18);
            endLabel.TextAlign = ContentAlignment.MiddleLeft;
            ComboBox endHour = createCombobox.CreateComboboxs(bodyPanel, "endHour", constants.end_times, endLabel.Right, startLabel.Bottom + 30, 150, 40, 25, new Font("Comic Sans", 18));
            endHour.DrawItem += new DrawItemEventHandler(createCombobox.dateCombobox_DrawItem);
            endHour.MeasureItem += new MeasureItemEventHandler(createCombobox.dateCombobox_MeasureItem);
            Label endHourUnit = createLabel.CreateLabelsInPanel(bodyPanel, "endHourUnit", constants.hourLabel, endHour.Right + 10, startLabel.Bottom + 30, 80, 40, Color.Transparent, Color.Black, 18);

            ComboBox endMinute = createCombobox.CreateComboboxs(bodyPanel, "endMinute", constants.minutes, endHourUnit.Right + 10, startLabel.Bottom + 30, 150, 40, 25, new Font("Comic Sans", 18));
            Label endMinuteUnit = createLabel.CreateLabelsInPanel(bodyPanel, "endMinuteUnit", constants.minuteLabel, endMinute.Right + 10, startLabel.Bottom + 30, 80, 40, Color.Transparent, Color.Black, 18, false, ContentAlignment.MiddleLeft);
            endMinute.DrawItem += new DrawItemEventHandler(createCombobox.dateCombobox_DrawItem);
            endMinute.MeasureItem += new MeasureItemEventHandler(createCombobox.dateCombobox_MeasureItem);

            Button settingButton = customButton.CreateButton(constants.settingLabel, "closeButton", bodyPanel.Width - 150, bodyPanel.Height - 100, 100, 50, Color.FromArgb(255, 0, 112, 192), Color.Transparent, 0, 1, 12, FontStyle.Regular, Color.White);
            bodyPanel.Controls.Add(settingButton);
            settingButton.Click += new EventHandler(this.BackShow);
            Button closeButton = customButton.CreateButton(constants.cancelButtonText, "closeButton", bodyPanel.Width - 300, bodyPanel.Height - 100, 100, 50, Color.Red, Color.Transparent, 0, 1, 12, FontStyle.Regular, Color.White);
            bodyPanel.Controls.Add(closeButton);
            closeButton.Click += new EventHandler(this.BackShow);


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

    }
}

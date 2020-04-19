using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ovan_P1
{
    public partial class TimeSetting : Form
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
        Label tableDateValueGlobal = null;
        Label tableTimeValueGlobal = null;
        DetailView detailView = new DetailView();
        int yearGlobal = DateTime.Now.Year;
        int monthGlobal = DateTime.Now.Month;
        int dayGlobal = DateTime.Now.Day;
        int hourGlobal = DateTime.Now.Hour;
        int minuteGlobal = DateTime.Now.Minute;


        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEMTIME
        {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDay;
            public ushort wDayOfWeek;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;
            public ushort wMilliseconds;
        }
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetLocalTime(out SYSTEMTIME st);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint GetLastError();

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetLocalTime(ref SYSTEMTIME st);
        public TimeSetting(Form1 mainForm, Panel mainPanel)
        {

            InitializeComponent();
            mainForm.Width = width;
            mainForm.Height = height;
            mainFormGlobal = mainForm;
            mainPanelGlobal = mainPanel;
            detailView.initTimeSetting(this);


            Panel headerPanel = createPanel.CreateMainPanel(mainForm, 0, 0, width, height / 5, BorderStyle.None, Color.Transparent);
            FlowLayoutPanel headerFlowPanel = createPanel.CreateFlowLayoutPanel(headerPanel, headerPanel.Width / 15, 80, headerPanel.Width * 5 / 7, 80, Color.Transparent, new Padding(0));
            Label headerLabel = createLabel.CreateLabels(headerFlowPanel, "headerLabel", constants.timeSettingLabel, 0, 0, headerFlowPanel.Width, headerFlowPanel.Height, Color.White, Color.Black, 44, false, ContentAlignment.MiddleLeft, new Padding(80, 0, 30, 0), 1, Color.Gray);

            DateTime now = DateTime.Now;

            Panel bodyPanel = createPanel.CreateMainPanel(mainForm, 0, headerPanel.Bottom, width, height * 4 / 5, BorderStyle.None, Color.Transparent);

            FlowLayoutPanel tableHeaderInUpPanel = createPanel.CreateFlowLayoutPanel(bodyPanel, bodyPanel.Width / 7, 80, bodyPanel.Width * 5 / 7, 50, Color.Transparent, new Padding(0));
            Label tableDateLabel = createLabel.CreateLabels(tableHeaderInUpPanel, "tableDateLabel", constants.currentDateLabel, 0, 0, tableHeaderInUpPanel.Width / 3, 50, Color.White, Color.Black, 16, false, ContentAlignment.MiddleRight, new Padding(0, 0, 30, 0), 1, Color.Gray);
            Label tableDateValue = createLabel.CreateLabels(tableHeaderInUpPanel, "tableDateValue", now.Year + "年 " + now.Month + "月 " + now.Day + "日 ", tableDateLabel.Right, 0, tableHeaderInUpPanel.Width / 3 - 30, 50, Color.FromArgb(255, 147, 184, 219), Color.Black, 16, true, ContentAlignment.MiddleLeft, new Padding(10, 0, 0, 0), 1, Color.Gray);
            tableDateValueGlobal = tableDateValue;

            tableDateValue.Click += new EventHandler(detailView.DateSetting);

            FlowLayoutPanel tableHeaderInUpPanel2 = createPanel.CreateFlowLayoutPanel(bodyPanel, bodyPanel.Width / 7, 150, bodyPanel.Width * 5 / 7, 50, Color.Transparent, new Padding(0));
            Label tableTimeLabel = createLabel.CreateLabels(tableHeaderInUpPanel2, "tableTimeLabel", constants.currentTimeLabel, 0, 0, tableHeaderInUpPanel2.Width / 3, 50, Color.White, Color.Black, 16, false, ContentAlignment.MiddleRight, new Padding(0, 0, 30, 0), 1, Color.Gray);
            Label tableTimeValue = createLabel.CreateLabels(tableHeaderInUpPanel2, "tableTimeValue", now.Hour + "時 " + now.Minute + "分 ", tableTimeLabel.Right, 0, tableHeaderInUpPanel2.Width / 3 - 30, 50, Color.FromArgb(255, 147, 184, 219), Color.Black, 16, true, ContentAlignment.MiddleLeft, new Padding(10, 0, 0, 0), 1, Color.Gray);
            tableTimeValueGlobal = tableTimeValue;

            tableTimeValue.Click += new EventHandler(detailView.TimeSetting);

            Button backButton = customButton.CreateButton(constants.backText, "backButton", bodyPanel.Width - 150, bodyPanel.Height - 100, 100, 50, Color.FromArgb(255, 0, 112, 192), Color.Transparent, 0, 10, 14, FontStyle.Bold, Color.White);
            bodyPanel.Controls.Add(backButton);
            backButton.Click += new EventHandler(this.BackShow);

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

        public void setVal(string setItem, string setValue)
        {
            DateTime now = DateTime.Now;
            if(setItem == "setDate")
            {
                string[] getData = setValue.Split('_');
                tableDateValueGlobal.Text = getData[0] + "年 " + getData[1] + "月 " + getData[2] + "日";
                tableTimeValueGlobal.Text = hourGlobal.ToString("00") + "時 " + minuteGlobal.ToString("00") + "分";
                yearGlobal = int.Parse(getData[0]);
                monthGlobal = int.Parse(getData[1]);
                dayGlobal = int.Parse(getData[2]);
                try
                {
                    SYSTEMTIME st = new SYSTEMTIME();

                    st.wYear = Convert.ToUInt16(yearGlobal); // must be ushort
                    st.wMonth = Convert.ToUInt16(monthGlobal);
                    st.wDayOfWeek = Convert.ToUInt16(dayGlobal);
                    st.wHour = Convert.ToUInt16(hourGlobal); // must be ushort
                    st.wMinute = Convert.ToUInt16(minuteGlobal);
                    st.wSecond = 0;
                    st.wMilliseconds = 0;

                    var ret = SetLocalTime(ref st);
                    Console.WriteLine("SetSystemTime return : " + ret);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                
                
            }
            else if(setItem == "setTime")
            {
                string[] getData = setValue.Split('_');
                tableDateValueGlobal.Text = yearGlobal.ToString("00") + "年 " + monthGlobal.ToString("00") + "月 " + dayGlobal.ToString("00") + "日";
                tableTimeValueGlobal.Text = getData[0] + "時 " + getData[1] + "分";
                hourGlobal = int.Parse(getData[0]);
                minuteGlobal = int.Parse(getData[1]);
                SYSTEMTIME st = new SYSTEMTIME();

                st.wYear = Convert.ToUInt16(yearGlobal); // must be ushort
                st.wMonth = Convert.ToUInt16(monthGlobal);
                st.wDayOfWeek = Convert.ToUInt16(dayGlobal);
                st.wHour = Convert.ToUInt16(hourGlobal); // must be ushort
                st.wMinute = Convert.ToUInt16(minuteGlobal);
                st.wSecond = 0;
                st.wMilliseconds = 0;

                var ret = SetLocalTime(ref st);
                Console.WriteLine("SetSystemTime return : " + ret);

            }
        }

    }
}

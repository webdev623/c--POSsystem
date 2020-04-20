using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
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
        MessageDialog messageDialog = new MessageDialog();

        ComboBox dayTypeGlobal = null;
        ComboBox startHourGlobal = null;
        ComboBox startMinuteGlobal = null;
        ComboBox endHourGlobal = null;
        ComboBox endMinuteGlobal = null;

        int dayTypeValue = 0;
        string startHourValue = "00";
        string startMinuteValue = "00";
        string endHourValue = "00";
        string endMinuteValue = "00";

        bool dayTypeChange = false;
        bool startHourChange = false;
        bool startMinuteChange = false;
        bool endHourChange = false;
        bool endMinuteChange = false;

        int rowNumDaySale = 0;
        bool manualProcessState = false;

        string standardStartTime = "10:00";
        string standardEndTime = "22:59";
        string standardWeek = "平日";

        SQLiteConnection sqlite_conn;
        DateTime now = DateTime.Now;

        string storeEndTime = "00:00";
        DateTime sumDayTime1 = new DateTime(int.Parse(DateTime.Now.ToString("yyyy")), int.Parse(DateTime.Now.ToString("MM")), int.Parse(DateTime.Now.ToString("dd")), 00, 00, 00);
        DateTime sumDayTime2 = new DateTime(int.Parse(DateTime.Now.ToString("yyyy")), int.Parse(DateTime.Now.ToString("MM")), int.Parse(DateTime.Now.ToString("dd")), 23, 59, 59);
        string sumDate = DateTime.Now.ToString("yyyy-MM-dd");

        DetailView detailView = new DetailView();
        public OpenTimeChange(Form1 mainForm, Panel mainPanel)
        {
            InitializeComponent();
            mainForm.Width = width;
            mainForm.Height = height;
            mainFormGlobal = mainForm;
            mainPanelGlobal = mainPanel;
            mainForm.AutoScroll = true;
            messageDialog.initOpenTimeChange(this);

            sqlite_conn = CreateConnection(constants.dbName);
            if (sqlite_conn.State == ConnectionState.Closed)
            {
                sqlite_conn.Open();
            }

            SQLiteCommand sqlite_cmd;
            SQLiteDataReader sqlite_datareader;

            string week = now.ToString("ddd");
            string currentTime = now.ToString("HH:mm");
            string openTimeTemp = "";

            string storeEndqurey = "SELECT * FROM " + constants.tbNames[6];
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = storeEndqurey;
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                if (!sqlite_datareader.IsDBNull(0))
                {
                    if (week == "Sat")
                    {
                        storeEndTime = (sqlite_datareader.GetString(6)).Split('/')[1];
                        openTimeTemp = sqlite_datareader.GetString(4);
                        standardWeek = constants.dayTypeValue[1];
                    }
                    else if (week == "Sun")
                    {
                        storeEndTime = (sqlite_datareader.GetString(6)).Split('/')[2];
                        openTimeTemp = sqlite_datareader.GetString(5);
                        standardWeek = constants.dayTypeValue[2];
                    }
                    else
                    {
                        storeEndTime = (sqlite_datareader.GetString(6)).Split('/')[0];
                        openTimeTemp = sqlite_datareader.GetString(3);
                        standardWeek = constants.dayTypeValue[0];
                    }

                }
            }

            standardStartTime = openTimeTemp.Split('/')[0].Split('-')[0];
            standardEndTime = openTimeTemp.Split('/')[openTimeTemp.Split('/').Length - 1].Split('-')[1];

            sumDayTime1 = constants.sumDayTimeStart(storeEndTime);
            sumDayTime2 = constants.sumDayTimeEnd(storeEndTime);

            sumDate = constants.sumDate(storeEndTime);

            try
            {
                SQLiteCommand sqlite_cmds = sqlite_conn.CreateCommand();
                string sumIdentify = "SELECT COUNT(id) FROM " + constants.tbNames[7] + " WHERE sumDate=@sumDate";
                sqlite_cmds.CommandText = sumIdentify;
                sqlite_cmds.Parameters.AddWithValue("@sumDate", sumDate);
                rowNumDaySale = Convert.ToInt32(sqlite_cmds.ExecuteScalar());
                if (rowNumDaySale > 0)
                {
                    manualProcessState = true;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Database Error" + e);
            }

            sqlite_conn.Close();

            Panel headerPanel = createPanel.CreateMainPanel(mainForm, 0, 0, width, height / 5, BorderStyle.None, Color.Transparent);
            Label headerTitle = createLabel.CreateLabelsInPanel(headerPanel, "headerTitle", constants.openTimeChangeTitle, 50, 60, 200, 50, Color.Transparent, Color.Red, 32);

            Panel bodyPanel = createPanel.CreateMainPanel(mainForm, 0, headerPanel.Bottom, width, height * 4 / 5, BorderStyle.None, Color.Transparent);

            Label subTitle = createLabel.CreateLabelsInPanel(bodyPanel, "subTitle", constants.currentDateLabel + " : " + now.ToLocalTime().ToString("yyyy/MM/dd HH:mm"), 0, 10, bodyPanel.Width, 50, Color.Transparent, Color.Black, 22);
            subTitle.Padding = new Padding(100, 0, 0, 0);
            subTitle.TextAlign = ContentAlignment.MiddleLeft;

            Label dayTypeLabel = createLabel.CreateLabelsInPanel(bodyPanel, "dayTypeLabel", constants.dayType, bodyPanel.Width / 5, subTitle.Bottom + 30, bodyPanel.Width / 6 - 30, 40, Color.Transparent, Color.Black, 18);
            dayTypeLabel.TextAlign = ContentAlignment.MiddleLeft;

            ComboBox dayTypeComboBox = createCombobox.CreateComboboxs(bodyPanel, "dayTypeComboBox", constants.dayTypeValue, dayTypeLabel.Right, subTitle.Bottom + 30, 250, 40, 25, new Font("Comic Sans", 18), standardWeek);
            dayTypeGlobal = dayTypeComboBox;
            dayTypeComboBox.DrawItem += new DrawItemEventHandler(createCombobox.dateCombobox_DrawItem);
            dayTypeComboBox.MeasureItem += new MeasureItemEventHandler(createCombobox.dateCombobox_MeasureItem);
            dayTypeComboBox.SelectedIndexChanged += new EventHandler(ComboboxChanged);

            Label startLabel = createLabel.CreateLabelsInPanel(bodyPanel, "startLabel", constants.startTimeLabel, bodyPanel.Width / 5, dayTypeLabel.Bottom + 30, bodyPanel.Width / 6 - 30, 40, Color.Transparent, Color.Black, 18);
            startLabel.TextAlign = ContentAlignment.MiddleLeft;
            ComboBox startHour = createCombobox.CreateComboboxs(bodyPanel, "startHour", constants.times, startLabel.Right, dayTypeLabel.Bottom + 30, 150, 40, 25, new Font("Comic Sans", 18), standardStartTime.Split(':')[0]);
            startHourGlobal = startHour;
            startHour.DrawItem += new DrawItemEventHandler(createCombobox.dateCombobox_DrawItem);
            startHour.MeasureItem += new MeasureItemEventHandler(createCombobox.dateCombobox_MeasureItem);
            startHour.SelectedIndexChanged += new EventHandler(ComboboxChanged);


            Label startHourUnit = createLabel.CreateLabelsInPanel(bodyPanel, "startHourUnit", constants.hourLabel, startHour.Right + 10, dayTypeLabel.Bottom + 30, 80, 40, Color.Transparent, Color.Black, 18);

            ComboBox startMinute = createCombobox.CreateComboboxs(bodyPanel, "startMinute", constants.minutes, startHourUnit.Right + 10, dayTypeLabel.Bottom + 30, 150, 40, 25, new Font("Comic Sans", 18), standardStartTime.Split(':')[1]);
            startMinuteGlobal = startMinute;
            startMinute.DrawItem += new DrawItemEventHandler(createCombobox.dateCombobox_DrawItem);
            startMinute.MeasureItem += new MeasureItemEventHandler(createCombobox.dateCombobox_MeasureItem);
            startMinute.SelectedIndexChanged += new EventHandler(ComboboxChanged);

            Label startMinuteUnit = createLabel.CreateLabelsInPanel(bodyPanel, "startMinuteUnit", constants.minuteLabel, startMinute.Right + 10, dayTypeLabel.Bottom + 30, 80, 40, Color.Transparent, Color.Black, 18, false, ContentAlignment.MiddleLeft);

            Label endLabel = createLabel.CreateLabelsInPanel(bodyPanel, "endLabel", constants.endTimeLabel, bodyPanel.Width / 5, startLabel.Bottom + 30, bodyPanel.Width / 6 - 30, 40, Color.Transparent, Color.Black, 18);
            endLabel.TextAlign = ContentAlignment.MiddleLeft;
            ComboBox endHour = createCombobox.CreateComboboxs(bodyPanel, "endHour", constants.end_times, endLabel.Right, startLabel.Bottom + 30, 150, 40, 25, new Font("Comic Sans", 18), standardEndTime.Split(':')[0]);
            endHourGlobal = endHour;
            endHour.DrawItem += new DrawItemEventHandler(createCombobox.dateCombobox_DrawItem);
            endHour.MeasureItem += new MeasureItemEventHandler(createCombobox.dateCombobox_MeasureItem);
            endHour.SelectedIndexChanged += new EventHandler(ComboboxChanged);

            Label endHourUnit = createLabel.CreateLabelsInPanel(bodyPanel, "endHourUnit", constants.hourLabel, endHour.Right + 10, startLabel.Bottom + 30, 80, 40, Color.Transparent, Color.Black, 18);

            ComboBox endMinute = createCombobox.CreateComboboxs(bodyPanel, "endMinute", constants.end_minutes, endHourUnit.Right + 10, startLabel.Bottom + 30, 150, 40, 25, new Font("Comic Sans", 18), standardEndTime.Split(':')[1]);
            endMinuteGlobal = endMinute;
            endMinute.DrawItem += new DrawItemEventHandler(createCombobox.dateCombobox_DrawItem);
            endMinute.MeasureItem += new MeasureItemEventHandler(createCombobox.dateCombobox_MeasureItem);
            endMinute.SelectedIndexChanged += new EventHandler(ComboboxChanged);

            Label endMinuteUnit = createLabel.CreateLabelsInPanel(bodyPanel, "endMinuteUnit", constants.minuteLabel, endMinute.Right + 10, startLabel.Bottom + 30, 80, 40, Color.Transparent, Color.Black, 18, false, ContentAlignment.MiddleLeft);

            Button settingButton = customButton.CreateButton(constants.settingLabel, "closeButton", bodyPanel.Width - 150, bodyPanel.Height - 100, 100, 50, Color.FromArgb(255, 0, 112, 192), Color.Transparent, 0, 1, 12, FontStyle.Regular, Color.White);
            bodyPanel.Controls.Add(settingButton);
            settingButton.Click += new EventHandler(this.DateTimeSettingPreview);
            Button closeButton = customButton.CreateButton(constants.cancelButtonText, "closeButton", bodyPanel.Width - 300, bodyPanel.Height - 100, 100, 50, Color.Red, Color.Transparent, 0, 1, 12, FontStyle.Regular, Color.White);
            bodyPanel.Controls.Add(closeButton);
            closeButton.Click += new EventHandler(this.DateTimeCancelMessage);
        }

        private void ComboboxChanged(object sender, EventArgs e)
        {
            ComboBox comboTemp = (ComboBox)sender;
            switch (comboTemp.Name)
            {
                case "dayTypeComboBox":
                    dayTypeChange = true;
                    break;
                case "startHour":
                    startHourChange = true;
                    break;
                case "startMinute":
                    startMinuteChange = true;
                    break;
                case "endHour":
                    endHourChange = true;
                    break;
                case "endMinute":
                    endMinuteChange = true;
                    break;
            }
        }

        private void DateTimeSettingPreview(object sender, EventArgs e)
        {
            dayTypeValue = dayTypeGlobal.SelectedIndex;
            string dayTypeString = dayTypeGlobal.GetItemText(dayTypeGlobal.SelectedItem);
            startHourValue = startHourGlobal.GetItemText(startHourGlobal.SelectedItem);
            startMinuteValue = startMinuteGlobal.GetItemText(startMinuteGlobal.SelectedItem);
            endHourValue = endHourGlobal.GetItemText(endHourGlobal.SelectedItem);
            endMinuteValue = endMinuteGlobal.GetItemText(endMinuteGlobal.SelectedItem);

            string startTime = constants.unChanged;
            if(startHourChange || startMinuteChange)
            {
                startTime = startHourValue + ":" + startMinuteValue;
            }
            string endTime = constants.unChanged;
            if(endHourChange || endMinuteChange)
            {
                endTime = endHourValue + ":" + endMinuteValue;
            }

            mainFormGlobal.Controls.Clear();
            Panel headerPanel = createPanel.CreateMainPanel(mainFormGlobal, 0, 0, width, height / 5, BorderStyle.None, Color.Transparent);
            Label headerTitle = createLabel.CreateLabelsInPanel(headerPanel, "headerTitle", constants.openTimeChangeTitle, 50, 60, 200, 50, Color.Transparent, Color.Red, 32);

            Panel bodyPanel = createPanel.CreateMainPanel(mainFormGlobal, 0, headerPanel.Bottom, width, height * 4 / 5, BorderStyle.None, Color.Transparent);

            Label subTitle = createLabel.CreateLabelsInPanel(bodyPanel, "subTitle", constants.currentDateLabel + " : " + now.ToLocalTime().ToString("yyyy/MM/dd HH:mm"), 0, 10, bodyPanel.Width, 50, Color.Transparent, Color.Black, 22);
            subTitle.Padding = new Padding(100, 0, 0, 0);
            subTitle.TextAlign = ContentAlignment.MiddleLeft;

            Label dayTypeLabel1 = createLabel.CreateLabelsInPanel(bodyPanel, "dayTypeLabel1", constants.dayTypeBefore, bodyPanel.Width / 5, subTitle.Bottom + 30, bodyPanel.Width / 5, 40, Color.Transparent, Color.Black, 18);
            dayTypeLabel1.TextAlign = ContentAlignment.MiddleLeft;

            Label dayTypeBefore = createLabel.CreateLabelsInPanel(bodyPanel, "dayTypeBefore", "平日", dayTypeLabel1.Right, subTitle.Bottom + 30, bodyPanel.Width / 10, 40, Color.Transparent, Color.Black, 18);
            dayTypeBefore.TextAlign = ContentAlignment.MiddleLeft;

            Label dayTypeLabel2 = createLabel.CreateLabelsInPanel(bodyPanel, "dayTypeLabel2", constants.dayTypeAfter, dayTypeBefore.Right, subTitle.Bottom + 30, bodyPanel.Width / 5, 40, Color.Transparent, Color.Black, 18);
            dayTypeLabel2.TextAlign = ContentAlignment.MiddleLeft;

            Label dayTypeAfter = createLabel.CreateLabelsInPanel(bodyPanel, "dayTypeBefore", dayTypeString, dayTypeLabel2.Right, subTitle.Bottom + 30, bodyPanel.Width / 10, 40, Color.Transparent, Color.Black, 18);
            dayTypeAfter.TextAlign = ContentAlignment.MiddleLeft;

            Label startLabel1 = createLabel.CreateLabelsInPanel(bodyPanel, "startLabel1", constants.startTimeBefore, bodyPanel.Width / 5, dayTypeLabel1.Bottom + 30, bodyPanel.Width / 5, 40, Color.Transparent, Color.Black, 18);
            startLabel1.TextAlign = ContentAlignment.MiddleLeft;

            Label startHourBefore = createLabel.CreateLabelsInPanel(bodyPanel, "startHourBefore", "10:00", startLabel1.Right, dayTypeLabel1.Bottom + 30, bodyPanel.Width / 10, 40, Color.Transparent, Color.Black, 18);
            startHourBefore.TextAlign = ContentAlignment.MiddleLeft;

            Label startLabel2 = createLabel.CreateLabelsInPanel(bodyPanel, "startLabel2", constants.startTimeAfter, startHourBefore.Right, dayTypeLabel1.Bottom + 30, bodyPanel.Width / 5, 40, Color.Transparent, Color.Black, 18);
            startLabel2.TextAlign = ContentAlignment.MiddleLeft;

            Label startHourAfter = createLabel.CreateLabelsInPanel(bodyPanel, "startHourAfter", startTime, startLabel2.Right, dayTypeLabel1.Bottom + 30, bodyPanel.Width / 10, 40, Color.Transparent, Color.Black, 18);
            startHourAfter.TextAlign = ContentAlignment.MiddleLeft;

            Label endLabel1 = createLabel.CreateLabelsInPanel(bodyPanel, "endLabel1", constants.endTimeBefore, bodyPanel.Width / 5, startLabel1.Bottom + 30, bodyPanel.Width / 5, 40, Color.Transparent, Color.Black, 18);
            endLabel1.TextAlign = ContentAlignment.MiddleLeft;

            Label endHourBefore = createLabel.CreateLabelsInPanel(bodyPanel, "endHourBefore", "22:59", endLabel1.Right, startLabel1.Bottom + 30, bodyPanel.Width / 10, 40, Color.Transparent, Color.Black, 18);
            endHourBefore.TextAlign = ContentAlignment.MiddleLeft;

            Label endLabel2 = createLabel.CreateLabelsInPanel(bodyPanel, "endLabel2", constants.endTimeAfter, endHourBefore.Right, startLabel1.Bottom + 30, bodyPanel.Width / 5, 40, Color.Transparent, Color.Black, 18);
            endLabel2.TextAlign = ContentAlignment.MiddleLeft;

            Label endHourAfter = createLabel.CreateLabelsInPanel(bodyPanel, "endHourAfter", endTime, endLabel2.Right, startLabel1.Bottom + 30, bodyPanel.Width / 10, 40, Color.Transparent, Color.Black, 18);
            endHourAfter.TextAlign = ContentAlignment.MiddleLeft;

            Label instruction1 = createLabel.CreateLabelsInPanel(bodyPanel, "instruction1", constants.openTimeInstruction1, bodyPanel.Width / 5, endLabel1.Bottom + 50, bodyPanel.Width * 3 / 5, 60, Color.Transparent, Color.Black, 18);
            instruction1.TextAlign = ContentAlignment.MiddleLeft;
            

            Label instruction2 = createLabel.CreateLabelsInPanel(bodyPanel, "instruction2", constants.openTimeInstruction2, bodyPanel.Width / 5, instruction1.Bottom + 20, bodyPanel.Width * 3 / 5, 60, Color.Transparent, Color.Black, 18);
            instruction2.TextAlign = ContentAlignment.MiddleLeft;

            Label instruction3 = createLabel.CreateLabelsInPanel(bodyPanel, "instruction3", constants.openTimeInstruction3, bodyPanel.Width / 5, instruction2.Bottom + 20, bodyPanel.Width * 3 / 5, 90, Color.Transparent, Color.Black, 18);
            instruction3.TextAlign = ContentAlignment.MiddleLeft;


            Button settingButton = customButton.CreateButton(constants.gettingLabel, "settingButton", bodyPanel.Width - 150, bodyPanel.Height - 100, 100, 50, Color.FromArgb(255, 0, 112, 192), Color.Transparent, 0, 1, 12, FontStyle.Regular, Color.White);
            bodyPanel.Controls.Add(settingButton);
            settingButton.Click += new EventHandler(this.DateTimeSettingMessage);
            Button closeButton = customButton.CreateButton(constants.cancelButtonText, "closeButton", bodyPanel.Width - 300, bodyPanel.Height - 100, 100, 50, Color.Red, Color.Transparent, 0, 1, 12, FontStyle.Regular, Color.White);
            bodyPanel.Controls.Add(closeButton);
            closeButton.Click += new EventHandler(this.BackShow);
        }

        private void DateTimeSettingMessage(object sender, EventArgs e)
        {
            messageDialog.ShowOpenTimeSettingMessage();
        }
        private void DateTimeCancelMessage(object sender, EventArgs e)
        {
            messageDialog.ShowOpenTimeCancelMessage();
        }

        public void DateTimeSetting()
        {
            if (!manualProcessState && (startHourChange || startMinuteChange || endHourChange || endMinuteChange))
            {

                if (sqlite_conn.State == ConnectionState.Closed)
                {
                    sqlite_conn.Open();
                }
                SQLiteCommand sqlite_cmd;
                SQLiteDataReader sqlite_datareader;

                string week = now.ToString("ddd");
                string currentTime = now.ToString("HH:mm");
                string openTime = "";
                string storeEndqurey = "SELECT * FROM " + constants.tbNames[6];
                sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = storeEndqurey;
                sqlite_datareader = sqlite_cmd.ExecuteReader();
                while (sqlite_datareader.Read())
                {
                    if (!sqlite_datareader.IsDBNull(0))
                    {
                        if (dayTypeValue == 1)
                        {
                            openTime = sqlite_datareader.GetString(4);
                        }
                        else if (dayTypeValue == 2)
                        {
                            openTime = sqlite_datareader.GetString(5);
                        }
                        else
                        {
                            openTime = sqlite_datareader.GetString(3);
                        }

                    }
                }

                string[] openTimeTemp = openTime.Split('/');
                string startTimeTemp = openTimeTemp[0].Split('-')[0];
                string endTimeTemp = openTimeTemp[openTimeTemp.Length - 1].Split('-')[1];

                string newStartTime = startHourValue + ":" + startMinuteValue;
                string newEndTime = endHourValue + ":" + endMinuteValue;
                if(startHourChange || startMinuteChange)
                {
                    openTime = openTime.Replace(startTimeTemp, newStartTime);
                }
                if(endHourChange || endMinuteChange)
                {
                    openTime = openTime.Replace(endTimeTemp, newEndTime);
                }

                string query = "UPDATE " + constants.tbNames[6] + " SET WeekTime=@openTime";
                if(dayTypeValue == 1)
                {
                    query = "UPDATE " + constants.tbNames[6] + " SET SaturdayTime=@openTime";
                }
                else if(dayTypeValue == 2)
                {
                    query = "UPDATE " + constants.tbNames[6] + " SET SundayTime=@openTime";
                }
                sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = query;
                sqlite_cmd.Parameters.AddWithValue("@openTime", openTime);
                sqlite_cmd.ExecuteNonQuery();

                string categoryQuery = "SELECT * FROM " + constants.tbNames[0] + " WHERE SoldFlag='0' ORDER BY id";
                sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = categoryQuery;
                sqlite_datareader = sqlite_cmd.ExecuteReader();
                while (sqlite_datareader.Read())
                {
                    if (!sqlite_datareader.IsDBNull(0))
                    {
                        string openTimeCat = "";
                        string field = "DayTime";
                        if (dayTypeValue == 0)
                        {
                            openTimeCat = sqlite_datareader.GetString(3);
                            field = "DayTime";
                        }
                        else if (dayTypeValue == 1)
                        {
                            openTimeCat = sqlite_datareader.GetString(4);
                            field = "SatTime";
                        }
                        else
                        {
                            openTimeCat = sqlite_datareader.GetString(5);
                            field = "SunTime";
                        }

                        string startTimeCat = (openTimeCat.Split('/')[0]).Split('-')[0];
                        string endTimeCat = (openTimeCat.Split('/')[openTimeCat.Split('/').Length - 1]).Split('-')[1];
                        if(startHourChange || startMinuteChange)
                        {
                            if (String.Compare(newStartTime, startTimeCat) <= 0)
                            {
                                if (openTimeCat.IndexOf(startTimeTemp) >= 0)
                                {
                                    openTimeCat = openTimeCat.Replace(startTimeTemp, newStartTime);
                                }
                            }
                            else if (String.Compare(newStartTime, startTimeCat) > 0)
                            {
                                openTimeCat = openTimeCat.Replace(startTimeCat, newStartTime);
                            }
                        }
                        if (endHourChange || endMinuteChange)
                        {
                            if (String.Compare(newEndTime, endTimeCat) >= 0)
                            {
                                if (openTimeCat.IndexOf(endTimeTemp) >= 0)
                                {
                                    openTimeCat = openTimeCat.Replace(endTimeTemp, newEndTime);
                                }
                            }
                            else if (String.Compare(newEndTime, endTimeCat) < 0)
                            {
                                openTimeCat = openTimeCat.Replace(endTimeCat, newEndTime);
                            }
                        }


                        string querys = "UPDATE " + constants.tbNames[0] + " SET " + field + "=@openTime WHERE CategoryID=@CateogryID AND SoldFlag='0'";
                        sqlite_cmd = sqlite_conn.CreateCommand();
                        sqlite_cmd.CommandText = querys;
                        sqlite_cmd.Parameters.AddWithValue("@openTime", openTimeCat);
                        sqlite_cmd.Parameters.AddWithValue("@CateogryID", sqlite_datareader.GetInt32(1));
                        sqlite_cmd.ExecuteNonQuery();
                    }
                }


                string productQuery = "SELECT * FROM " + constants.tbNames[2] + " WHERE SoldFlag='0' ORDER BY id";
                sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = productQuery;
                sqlite_datareader = sqlite_cmd.ExecuteReader();
                while (sqlite_datareader.Read())
                {
                    if (!sqlite_datareader.IsDBNull(0))
                    {
                        string openTimePrd = "";
                        string field = "DayTime";
                        if(dayTypeValue == 0)
                        {
                            openTimePrd = sqlite_datareader.GetString(5);
                            field = "DayTime";
                        }
                        else if(dayTypeValue == 1)
                        {
                            openTimePrd = sqlite_datareader.GetString(6);
                            field = "SatTime";
                        }
                        else
                        {
                            openTimePrd = sqlite_datareader.GetString(7);
                            field = "SunTime";
                        }

                        string startTimePrd = (openTimePrd.Split('/')[0]).Split('-')[0];
                        string endTimePrd = (openTimePrd.Split('/')[openTimePrd.Split('/').Length - 1]).Split('-')[1];
                        if(startHourChange || startMinuteChange)
                        {
                            if (String.Compare(newStartTime, startTimePrd) <= 0)
                            {
                                if (openTimePrd.IndexOf(startTimeTemp) >= 0)
                                {
                                    openTimePrd = openTimePrd.Replace(startTimeTemp, newStartTime);
                                }
                            }
                            else if (String.Compare(newStartTime, startTimePrd) > 0)
                            {
                                openTimePrd = openTimePrd.Replace(startTimePrd, newStartTime);
                            }
                        }
                        if (endHourChange || endMinuteChange)
                        {
                            if (String.Compare(newEndTime, endTimePrd) >= 0)
                            {
                                if (openTimePrd.IndexOf(endTimeTemp) >= 0)
                                {
                                    openTimePrd = openTimePrd.Replace(endTimeTemp, newEndTime);
                                }
                            }
                            else if (String.Compare(newEndTime, endTimePrd) < 0)
                            {
                                openTimePrd = openTimePrd.Replace(endTimePrd, newEndTime);
                            }
                        }


                        string querys = "UPDATE " + constants.tbNames[2] + " SET " + field + "=@openTime WHERE id=@prdID AND SoldFlag='0'";
                        sqlite_cmd = sqlite_conn.CreateCommand();
                        sqlite_cmd.CommandText = querys;
                        sqlite_cmd.Parameters.AddWithValue("@openTime", openTimePrd);
                        sqlite_cmd.Parameters.AddWithValue("@prdID", sqlite_datareader.GetInt32(0));
                        sqlite_cmd.ExecuteNonQuery();
                    }
                }
                this.BackShowPage();
            }
            else
            {
                this.BackShowPage();
            }
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

        public void BackShowPage()
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

    }
}

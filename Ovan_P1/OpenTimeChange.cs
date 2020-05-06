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
        Panel bodyPanelGlobal = null;
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
            mainFormGlobal = mainForm;
            mainPanelGlobal = mainPanel;
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

            Panel headerPanel = createPanel.CreateSubPanel(mainPanel, 0, 0, mainPanel.Width, mainPanel.Height / 10, BorderStyle.None, Color.FromArgb(255, 234, 225, 151));
            Label headerLabel = createLabel.CreateLabelsInPanel(headerPanel, "headerTitle", constants.openTimeChangeTitle, 0, 0, headerPanel.Width, headerPanel.Height, Color.Transparent, Color.Black, 28, false, ContentAlignment.MiddleCenter);

            Panel bodyPanel = createPanel.CreateSubPanel(mainPanel, 0, headerPanel.Bottom, mainPanel.Width, mainPanel.Height * 9 / 10, BorderStyle.None, Color.Transparent);
            bodyPanelGlobal = bodyPanel;

            FlowLayoutPanel tableHeaderInUpPanel = createPanel.CreateFlowLayoutPanel(bodyPanel, bodyPanel.Width / 7, bodyPanel.Height / 7, bodyPanel.Width * 5 / 7, bodyPanel.Height / 7, Color.White, new Padding(30, bodyPanel.Height / 28, 30, bodyPanel.Height / 28));


            Label dayTypeLabel = createLabel.CreateLabelsInPanel(tableHeaderInUpPanel, "dayTypeLabel", constants.dayType, 0, 0, tableHeaderInUpPanel.Width / 3 - 30, tableHeaderInUpPanel.Height / 2, Color.Transparent, Color.Black, 24);
            dayTypeLabel.Margin = new Padding(0);
            dayTypeLabel.TextAlign = ContentAlignment.MiddleLeft;

            ComboBox dayTypeComboBox = createCombobox.CreateComboboxs(tableHeaderInUpPanel, "dayTypeComboBox", constants.dayTypeValue, dayTypeLabel.Right, 0, tableHeaderInUpPanel.Width / 5 - 30, tableHeaderInUpPanel.Height / 3, tableHeaderInUpPanel.Height / 3, new Font("Comic Sans", 24), standardWeek);
            dayTypeGlobal = dayTypeComboBox;
            dayTypeComboBox.DrawItem += new DrawItemEventHandler(createCombobox.dateCombobox_DrawItem);
            dayTypeComboBox.MeasureItem += new MeasureItemEventHandler(createCombobox.dateCombobox_MeasureItem);
            dayTypeComboBox.SelectedIndexChanged += new EventHandler(ComboboxChanged);

            FlowLayoutPanel tableHeaderInUpPanel2 = createPanel.CreateFlowLayoutPanel(bodyPanel, bodyPanel.Width / 7, tableHeaderInUpPanel.Bottom + bodyPanel.Height / 20, bodyPanel.Width * 5 / 7, bodyPanel.Height / 7, Color.White, new Padding(30, bodyPanel.Height / 28, 30, bodyPanel.Height / 28));


            Label startLabel = createLabel.CreateLabelsInPanel(tableHeaderInUpPanel2, "startLabel", constants.startTimeLabel, 0, 0, tableHeaderInUpPanel2.Width / 3 - 30, tableHeaderInUpPanel2.Height / 2, Color.Transparent, Color.Black, 24);
            startLabel.Margin = new Padding(0);
            startLabel.TextAlign = ContentAlignment.MiddleLeft;
            ComboBox startHour = createCombobox.CreateComboboxs(tableHeaderInUpPanel2, "startHour", constants.times, startLabel.Right, 0, tableHeaderInUpPanel2.Width / 5 - 30, tableHeaderInUpPanel2.Height / 3, tableHeaderInUpPanel2.Height / 3, new Font("Comic Sans", 24), standardStartTime.Split(':')[0]);
            startHourGlobal = startHour;
            startHour.DrawItem += new DrawItemEventHandler(createCombobox.dateCombobox_DrawItem);
            startHour.MeasureItem += new MeasureItemEventHandler(createCombobox.dateCombobox_MeasureItem);
            startHour.SelectedIndexChanged += new EventHandler(ComboboxChanged);

            Label startHourUnit = createLabel.CreateLabelsInPanel(tableHeaderInUpPanel2, "startHourUnit", constants.hourLabel, startHour.Right, 0, tableHeaderInUpPanel2.Width / 12, tableHeaderInUpPanel2.Height / 2, Color.Transparent, Color.Black, 24);
            startHourUnit.Margin = new Padding(0);
            startHourUnit.TextAlign = ContentAlignment.MiddleLeft;
            ComboBox startMinute = createCombobox.CreateComboboxs(tableHeaderInUpPanel2, "startMinute", constants.minutes, startHourUnit.Right, 0, tableHeaderInUpPanel2.Width / 5 - 30, tableHeaderInUpPanel2.Height / 3, tableHeaderInUpPanel2.Height / 3, new Font("Comic Sans", 24), standardStartTime.Split(':')[1]);
            startMinuteGlobal = startMinute;
            startMinute.DrawItem += new DrawItemEventHandler(createCombobox.dateCombobox_DrawItem);
            startMinute.MeasureItem += new MeasureItemEventHandler(createCombobox.dateCombobox_MeasureItem);
            startMinute.SelectedIndexChanged += new EventHandler(ComboboxChanged);

            Label startMinuteUnit = createLabel.CreateLabelsInPanel(tableHeaderInUpPanel2, "startMinuteUnit", constants.minuteLabel, startMinute.Right, 0, tableHeaderInUpPanel2.Width / 12, tableHeaderInUpPanel2.Height / 2, Color.Transparent, Color.Black, 24, false, ContentAlignment.MiddleLeft);
            startMinuteUnit.Margin = new Padding(0);

            FlowLayoutPanel tableHeaderInUpPanel3 = createPanel.CreateFlowLayoutPanel(bodyPanel, bodyPanel.Width / 7, tableHeaderInUpPanel2.Bottom + bodyPanel.Height / 20, bodyPanel.Width * 5 / 7, bodyPanel.Height / 7, Color.White, new Padding(30, bodyPanel.Height / 28, 30, bodyPanel.Height / 28));

            Label endLabel = createLabel.CreateLabelsInPanel(tableHeaderInUpPanel3, "endLabel", constants.endTimeLabel, 0, 0, tableHeaderInUpPanel3.Width / 3 - 30, tableHeaderInUpPanel3.Height / 2, Color.Transparent, Color.Black, 24);
            endLabel.Margin = new Padding(0);
            endLabel.TextAlign = ContentAlignment.MiddleLeft;
            ComboBox endHour = createCombobox.CreateComboboxs(tableHeaderInUpPanel3, "endHour", constants.end_times, endLabel.Right, 0, tableHeaderInUpPanel3.Width / 5 - 30, tableHeaderInUpPanel3.Height / 3, tableHeaderInUpPanel3.Height / 3, new Font("Comic Sans", 24), standardEndTime.Split(':')[0]);
            endHourGlobal = endHour;
            endHour.DrawItem += new DrawItemEventHandler(createCombobox.dateCombobox_DrawItem);
            endHour.MeasureItem += new MeasureItemEventHandler(createCombobox.dateCombobox_MeasureItem);
            endHour.SelectedIndexChanged += new EventHandler(ComboboxChanged);

            Label endHourUnit = createLabel.CreateLabelsInPanel(tableHeaderInUpPanel3, "endHourUnit", constants.hourLabel, endHour.Right, 0, tableHeaderInUpPanel3.Width / 12, tableHeaderInUpPanel3.Height / 2, Color.Transparent, Color.Black, 24);
            endHourUnit.Margin = new Padding(0);
            endHourUnit.TextAlign = ContentAlignment.MiddleLeft;

            ComboBox endMinute = createCombobox.CreateComboboxs(tableHeaderInUpPanel3, "endMinute", constants.end_minutes, endHourUnit.Right, 0, tableHeaderInUpPanel3.Width / 5 - 30, tableHeaderInUpPanel3.Height / 3, tableHeaderInUpPanel3.Height / 3, new Font("Comic Sans", 24), standardEndTime.Split(':')[1]);
            endMinuteGlobal = endMinute;
            endMinute.DrawItem += new DrawItemEventHandler(createCombobox.dateCombobox_DrawItem);
            endMinute.MeasureItem += new MeasureItemEventHandler(createCombobox.dateCombobox_MeasureItem);
            endMinute.SelectedIndexChanged += new EventHandler(ComboboxChanged);

            Label endMinuteUnit = createLabel.CreateLabelsInPanel(tableHeaderInUpPanel3, "endMinuteUnit", constants.minuteLabel, endMinute.Right, 0, tableHeaderInUpPanel3.Width / 12, tableHeaderInUpPanel3.Height / 2, Color.Transparent, Color.Black, 24, false, ContentAlignment.MiddleLeft);
            endMinuteUnit.Margin = new Padding(0);

            Button settingButton = customButton.CreateButtonWithImage(Image.FromFile(constants.soldoutButtonImage1), "closeButton", constants.settingLabel, bodyPanel.Width * 6 / 7 - 100, tableHeaderInUpPanel3.Bottom + bodyPanel.Height / 20, 100, 50, 0, 30, 18, FontStyle.Bold, Color.White, ContentAlignment.MiddleCenter, 2);
            bodyPanel.Controls.Add(settingButton);
            settingButton.Click += new EventHandler(this.DateTimeSettingPreview);

            Button closeButton = customButton.CreateButtonWithImage(Image.FromFile(constants.soldoutButtonImage2), "closeButton", constants.cancelButtonText, bodyPanel.Width * 6 / 7 - 250, tableHeaderInUpPanel3.Bottom + bodyPanel.Height / 20, 100, 50, 0, 30, 18, FontStyle.Bold, Color.White, ContentAlignment.MiddleCenter, 2);
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

            bodyPanelGlobal.Controls.Clear();

            Label subTitle = createLabel.CreateLabelsInPanel(bodyPanelGlobal, "subTitle", constants.currentDateLabel + " : " + now.ToLocalTime().ToString("yyyy/MM/dd HH:mm"), 0, 10, bodyPanelGlobal.Width, 50, Color.Transparent, Color.Black, 22);
            subTitle.Padding = new Padding(100, 0, 0, 0);
            subTitle.TextAlign = ContentAlignment.MiddleLeft;

            Label dayTypeLabel1 = createLabel.CreateLabelsInPanel(bodyPanelGlobal, "dayTypeLabel1", constants.dayTypeBefore, bodyPanelGlobal.Width / 5, subTitle.Bottom + 30, bodyPanelGlobal.Width / 5, 40, Color.Transparent, Color.Black, 18);
            dayTypeLabel1.TextAlign = ContentAlignment.MiddleLeft;

            Label dayTypeBefore = createLabel.CreateLabelsInPanel(bodyPanelGlobal, "dayTypeBefore", standardWeek, dayTypeLabel1.Right, subTitle.Bottom + 30, bodyPanelGlobal.Width / 10, 40, Color.Transparent, Color.Black, 18);
            dayTypeBefore.TextAlign = ContentAlignment.MiddleLeft;

            Label dayTypeLabel2 = createLabel.CreateLabelsInPanel(bodyPanelGlobal, "dayTypeLabel2", constants.dayTypeAfter, dayTypeBefore.Right, subTitle.Bottom + 30, bodyPanelGlobal.Width / 5, 40, Color.Transparent, Color.Black, 18);
            dayTypeLabel2.TextAlign = ContentAlignment.MiddleLeft;

            Label dayTypeAfter = createLabel.CreateLabelsInPanel(bodyPanelGlobal, "dayTypeBefore", dayTypeString, dayTypeLabel2.Right, subTitle.Bottom + 30, bodyPanelGlobal.Width / 10, 40, Color.Transparent, Color.Black, 18);
            dayTypeAfter.TextAlign = ContentAlignment.MiddleLeft;

            Label startLabel1 = createLabel.CreateLabelsInPanel(bodyPanelGlobal, "startLabel1", constants.startTimeBefore, bodyPanelGlobal.Width / 5, dayTypeLabel1.Bottom + 30, bodyPanelGlobal.Width / 5, 40, Color.Transparent, Color.Black, 18);
            startLabel1.TextAlign = ContentAlignment.MiddleLeft;

            Label startHourBefore = createLabel.CreateLabelsInPanel(bodyPanelGlobal, "startHourBefore", standardStartTime, startLabel1.Right, dayTypeLabel1.Bottom + 30, bodyPanelGlobal.Width / 10, 40, Color.Transparent, Color.Black, 18);
            startHourBefore.TextAlign = ContentAlignment.MiddleLeft;

            Label startLabel2 = createLabel.CreateLabelsInPanel(bodyPanelGlobal, "startLabel2", constants.startTimeAfter, startHourBefore.Right, dayTypeLabel1.Bottom + 30, bodyPanelGlobal.Width / 5, 40, Color.Transparent, Color.Black, 18);
            startLabel2.TextAlign = ContentAlignment.MiddleLeft;

            Label startHourAfter = createLabel.CreateLabelsInPanel(bodyPanelGlobal, "startHourAfter", startTime, startLabel2.Right, dayTypeLabel1.Bottom + 30, bodyPanelGlobal.Width / 10, 40, Color.Transparent, Color.Black, 18);
            startHourAfter.TextAlign = ContentAlignment.MiddleLeft;

            Label endLabel1 = createLabel.CreateLabelsInPanel(bodyPanelGlobal, "endLabel1", constants.endTimeBefore, bodyPanelGlobal.Width / 5, startLabel1.Bottom + 30, bodyPanelGlobal.Width / 5, 40, Color.Transparent, Color.Black, 18);
            endLabel1.TextAlign = ContentAlignment.MiddleLeft;

            Label endHourBefore = createLabel.CreateLabelsInPanel(bodyPanelGlobal, "endHourBefore", standardEndTime, endLabel1.Right, startLabel1.Bottom + 30, bodyPanelGlobal.Width / 10, 40, Color.Transparent, Color.Black, 18);
            endHourBefore.TextAlign = ContentAlignment.MiddleLeft;

            Label endLabel2 = createLabel.CreateLabelsInPanel(bodyPanelGlobal, "endLabel2", constants.endTimeAfter, endHourBefore.Right, startLabel1.Bottom + 30, bodyPanelGlobal.Width / 5, 40, Color.Transparent, Color.Black, 18);
            endLabel2.TextAlign = ContentAlignment.MiddleLeft;

            Label endHourAfter = createLabel.CreateLabelsInPanel(bodyPanelGlobal, "endHourAfter", endTime, endLabel2.Right, startLabel1.Bottom + 30, bodyPanelGlobal.Width / 10, 40, Color.Transparent, Color.Black, 18);
            endHourAfter.TextAlign = ContentAlignment.MiddleLeft;

            Label instruction1 = createLabel.CreateLabelsInPanel(bodyPanelGlobal, "instruction1", constants.openTimeInstruction1, bodyPanelGlobal.Width / 5, endLabel1.Bottom + 50, bodyPanelGlobal.Width * 3 / 5, 60, Color.Transparent, Color.Black, 18);
            instruction1.TextAlign = ContentAlignment.MiddleLeft;
            

            Label instruction2 = createLabel.CreateLabelsInPanel(bodyPanelGlobal, "instruction2", constants.openTimeInstruction2, bodyPanelGlobal.Width / 5, instruction1.Bottom + 20, bodyPanelGlobal.Width * 3 / 5, 60, Color.Transparent, Color.Black, 18);
            instruction2.TextAlign = ContentAlignment.MiddleLeft;

            Label instruction3 = createLabel.CreateLabelsInPanel(bodyPanelGlobal, "instruction3", constants.openTimeInstruction3, bodyPanelGlobal.Width / 5, instruction2.Bottom + 20, bodyPanelGlobal.Width * 3 / 5, 90, Color.Transparent, Color.Black, 18);
            instruction3.TextAlign = ContentAlignment.MiddleLeft;


            Button settingButton = customButton.CreateButtonWithImage(Image.FromFile(constants.soldoutButtonImage1), "closeButton", constants.settingLabel, bodyPanelGlobal.Width - 150, bodyPanelGlobal.Height - 100, 100, 50, 0, 30, 18, FontStyle.Bold, Color.White, ContentAlignment.MiddleCenter, 2);
            bodyPanelGlobal.Controls.Add(settingButton);
            settingButton.Click += new EventHandler(this.DateTimeSettingMessage);

            Button closeButton = customButton.CreateButtonWithImage(Image.FromFile(constants.soldoutButtonImage2), "closeButton", constants.cancelButtonText, bodyPanelGlobal.Width - 300, bodyPanelGlobal.Height - 100, 100, 50, 0, 30, 18, FontStyle.Bold, Color.White, ContentAlignment.MiddleCenter, 2);
            bodyPanelGlobal.Controls.Add(closeButton);
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
            mainPanelGlobal.Controls.Clear();
            MaintaneceMenu frm = new MaintaneceMenu(mainFormGlobal, mainPanelGlobal);
            frm.TopLevel = false;
            mainPanelGlobal.Controls.Add(frm);
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;
            Thread.Sleep(200);
            frm.Show();
        }

        public void BackShowPage()
        {
            mainPanelGlobal.Controls.Clear();
            MaintaneceMenu frm = new MaintaneceMenu(mainFormGlobal, mainPanelGlobal);
            frm.TopLevel = false;
            mainPanelGlobal.Controls.Add(frm);
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

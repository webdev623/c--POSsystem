﻿using System;
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
    public partial class FalsePurchaseCancellation : Form
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
        YearDropDown dropDownYear = new YearDropDown();
        MonthDropDown dropDownMonth = new MonthDropDown();
        DateDropDown dropDownDate = new DateDropDown();

        DetailView detailView = new DetailView();
        ComboBox yearComboboxGlobal = null;
        ComboBox monthComboboxGlobal = null;
        ComboBox dateComboboxGlobal = null;

        public FalsePurchaseCancellation(Form1 mainForm, Panel mainPanel)
        {
            InitializeComponent();
            mainFormGlobal = mainForm;
            mainPanelGlobal = mainPanel;
            dropDownMenu.initFalsePurchaseCancellation(this);
            mainForm.AutoScroll = true;

            Panel headerPanel = createPanel.CreateMainPanel(mainForm, 0, 0, width, height / 4, BorderStyle.None, Color.Transparent);
            Label headerTitle = createLabel.CreateLabelsInPanel(headerPanel, "headerTitle", "誤購入取消", 50, 60, 200, 50, Color.Transparent, Color.Red, 28);
            Button closeButton = customButton.CreateButton(constants.backText, "closeButton", headerPanel.Width - 200, 60, 100, 50, Color.FromArgb(255, 0, 112, 192), Color.Transparent, 0, 1, 12, FontStyle.Regular, Color.White);
            headerPanel.Controls.Add(closeButton);
            closeButton.Click += new EventHandler(this.BackShow);

            Panel bodyUpPanel = createPanel.CreateMainPanel(mainForm, 0, headerPanel.Bottom, width, height * 1 / 4, BorderStyle.None, Color.Transparent);
            Label subTitle1 = createLabel.CreateLabelsInPanel(bodyUpPanel, "subTitle1", "誤購入取消", bodyUpPanel.Width / 2 - 100, 10, 200, 50, Color.Transparent, Color.Black, 22);
            Label subContent1 = createLabel.CreateLabelsInPanel(bodyUpPanel, "subContent1", "誤購入の取消を行う場合は下記のボタンを\nタッチしてください。", 100, subTitle1.Bottom + 15, bodyUpPanel.Width - 200, 50, Color.Transparent, Color.Black, 16);
            Button cancellationButton = customButton.CreateButton("誤購入取消", "cancellationButton", bodyUpPanel.Width / 2 - 100, subContent1.Bottom + 30, 200, 50, Color.FromArgb(255, 0, 112, 192), Color.Transparent, 0, 1, 12, FontStyle.Regular, Color.White);
            bodyUpPanel.Controls.Add(cancellationButton);

            cancellationButton.Click += new EventHandler(detailView.DetailViewIndicator);

            Panel bodyDownPanel = createPanel.CreateMainPanel(mainForm, 0, bodyUpPanel.Bottom, width, height / 2, BorderStyle.None, Color.Transparent);
            Label subTitle2 = createLabel.CreateLabelsInPanel(bodyDownPanel, "subTitle1", "誤購入一覧表示", bodyDownPanel.Width / 2 - 100, 50, 200, 50, Color.Transparent, Color.Black, 22);

            Label startLabel = createLabel.CreateLabelsInPanel(bodyDownPanel, "startLabel", "開始", bodyDownPanel.Width / 4 + 100, subTitle2.Bottom + 30, 100, 50, Color.Transparent, Color.Black, 22);

            // dropDownYear.CreateDropDown("falsePurchaseCancellation", bodyDownPanel, new string[] { "2020", "2019", "2018" }, startLabel.Right + 10, subTitle2.Bottom + 30, 150, 50, 150, 200, 150, 50, Color.Transparent, Color.Transparent);
            ComboBox yearCombobox1 = createCombobox.CreateComboboxs(bodyDownPanel, "yearCombobox1", new string[] { "2020", "2019", "2018" }, startLabel.Right + 40, subTitle2.Bottom + 40, 150, 40, 25, new Font("Comic Sans", 18));
            Label yearLabel1 = createLabel.CreateLabelsInPanel(bodyDownPanel, "startLabelYear", "年", startLabel.Right + 210, subTitle2.Bottom + 30, 30, 50, Color.Transparent, Color.Black, 22);
            yearComboboxGlobal = yearCombobox1;
            yearCombobox1.DrawItem += new DrawItemEventHandler(createCombobox.dateCombobox_DrawItem);
            yearCombobox1.MeasureItem += new MeasureItemEventHandler(createCombobox.dateCombobox_MeasureItem);

            // dropDownMonth.CreateDropDown("falsePurchaseCancellation", bodyDownPanel, constants.months, startLabel.Right + 230, subTitle2.Bottom + 30, 150, 50, 150, 50 * 13, 150, 50, Color.Transparent, Color.Transparent);
            ComboBox monthCombobox1 = createCombobox.CreateComboboxs(bodyDownPanel, "monthCombobox1", constants.months1, startLabel.Right + 280, subTitle2.Bottom + 40, 150, 40, 25, new Font("Comic Sans", 18));
            Label monthLabel1 = createLabel.CreateLabelsInPanel(bodyDownPanel, "startLabelMonth", "月", startLabel.Right + 440, subTitle2.Bottom + 30, 30, 50, Color.Transparent, Color.Black, 22);
            monthComboboxGlobal = monthCombobox1;
            monthCombobox1.DrawItem += new DrawItemEventHandler(createCombobox.dateCombobox_DrawItem);
            monthCombobox1.MeasureItem += new MeasureItemEventHandler(createCombobox.dateCombobox_MeasureItem);

            ComboBox dateCombobox1 = createCombobox.CreateComboboxs(bodyDownPanel, "dateCombobox1", constants.dates1, startLabel.Right + 480, subTitle2.Bottom + 40, 150, 40, 25, new Font("Comic Sans", 18));

            //  dropDownDate.CreateDropDown("falsePurchaseCancellation", bodyDownPanel, constants.dates, startLabel.Right + 440, subTitle2.Bottom + 30, 150, 50, 150, 50 * 32, 150, 50, Color.Transparent, Color.Transparent);
            Label dateLabel1 = createLabel.CreateLabelsInPanel(bodyDownPanel, "startLabelDate", "日", startLabel.Right + 640, subTitle2.Bottom + 30, 30, 50, Color.Transparent, Color.Black, 22);
            dateComboboxGlobal = dateCombobox1;
            dateCombobox1.DrawItem += new DrawItemEventHandler(createCombobox.dateCombobox_DrawItem);
            dateCombobox1.MeasureItem += new MeasureItemEventHandler(createCombobox.dateCombobox_MeasureItem);

            Label endLabel = createLabel.CreateLabelsInPanel(bodyDownPanel, "endLabel", "終了", bodyDownPanel.Width / 4 + 100, startLabel.Bottom + 30, 100, 50, Color.Transparent, Color.Black, 22);

            // dropDownYear.CreateDropDown("falsePurchaseCancellation", bodyDownPanel, new string[] { "2020", "2019", "2018" }, startLabel.Right + 10, subTitle2.Bottom + 30, 150, 50, 150, 200, 150, 50, Color.Transparent, Color.Transparent);
            ComboBox yearCombobox2 = createCombobox.CreateComboboxs(bodyDownPanel, "yearCombobox2", new string[] { "2020", "2019", "2018" }, startLabel.Right + 40, startLabel.Bottom + 40, 150, 40, 25, new Font("Comic Sans", 18));
            Label yearLabel2 = createLabel.CreateLabelsInPanel(bodyDownPanel, "endLabelYear", "年", startLabel.Right + 210, startLabel.Bottom + 30, 30, 50, Color.Transparent, Color.Black, 22);
            yearComboboxGlobal = yearCombobox2;
            yearCombobox2.DrawItem += new DrawItemEventHandler(createCombobox.dateCombobox_DrawItem);
            yearCombobox2.MeasureItem += new MeasureItemEventHandler(createCombobox.dateCombobox_MeasureItem);

            // dropDownMonth.CreateDropDown("falsePurchaseCancellation", bodyDownPanel, constants.months, startLabel.Right + 230, subTitle2.Bottom + 30, 150, 50, 150, 50 * 13, 150, 50, Color.Transparent, Color.Transparent);
            ComboBox monthCombobox2 = createCombobox.CreateComboboxs(bodyDownPanel, "monthCombobox2", constants.months2, startLabel.Right + 280, startLabel.Bottom + 40, 150, 40, 25, new Font("Comic Sans", 18));
            Label monthLabel2 = createLabel.CreateLabelsInPanel(bodyDownPanel, "endLabelMonth", "月", startLabel.Right + 440, startLabel.Bottom + 30, 30, 50, Color.Transparent, Color.Black, 22);
            monthComboboxGlobal = monthCombobox2;
            monthCombobox2.DrawItem += new DrawItemEventHandler(createCombobox.dateCombobox_DrawItem);
            monthCombobox2.MeasureItem += new MeasureItemEventHandler(createCombobox.dateCombobox_MeasureItem);

            //  dropDownDate.CreateDropDown("falsePurchaseCancellation", bodyDownPanel, constants.dates, startLabel.Right + 440, subTitle2.Bottom + 30, 150, 50, 150, 50 * 32, 150, 50, Color.Transparent, Color.Transparent);
            ComboBox dateCombobox2 = createCombobox.CreateComboboxs(bodyDownPanel, "dateCombobox2", constants.dates2, startLabel.Right + 480, startLabel.Bottom + 40, 150, 40, 25, new Font("Comic Sans", 18));
            Label dateLabel2 = createLabel.CreateLabelsInPanel(bodyDownPanel, "endLabelDate", "日", startLabel.Right + 640, startLabel.Bottom + 30, 30, 50, Color.Transparent, Color.Black, 22);
            dateComboboxGlobal = dateCombobox2;
            dateCombobox2.DrawItem += new DrawItemEventHandler(createCombobox.dateCombobox_DrawItem);
            dateCombobox2.MeasureItem += new MeasureItemEventHandler(createCombobox.dateCombobox_MeasureItem);

            Button cancellationShowButton = customButton.CreateButton("一覧表示", "cancellationShowButton", bodyDownPanel.Width * 4 / 5, endLabel.Bottom + 30, 200, 50, Color.FromArgb(255, 0, 112, 192), Color.Transparent, 0, 1, 12, FontStyle.Regular, Color.White);
            bodyDownPanel.Controls.Add(cancellationShowButton);

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

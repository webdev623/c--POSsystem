using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ovan_P1
{
    public partial class CategoryList : Form
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
        MessageDialog messageDialog = new MessageDialog();
        Panel detailPanelGlobal = null;
        Panel tBodyPanelGlobal = null;
        DetailView detailView = new DetailView();

        PaperSize paperSize = new PaperSize("papersize", 500, 800);//set the paper size
        PrintDocument printDocument1 = new PrintDocument();
        PrintPreviewDialog printPreviewDialog1 = new PrintPreviewDialog();
        PrintDialog printDialog1 = new PrintDialog();
        int totalNumber = 0;
        int itemperpage = 0;//this is for no of item per page 
        int groupIDGlobal = 0;
        int lineNum = 0;
        int flagInt = 0;
        int groupNumber = 0;
        int lineNums = 0;
        int groupNumbers = 0;
        int flagInts = 0;
        int itemperpages = 0;

        int categoryIDGlobal = 0;
        public CategoryList(Form1 mainForm, Panel mainPanel)
        {
            InitializeComponent();
            dropDownMenu.initCategoryList(this);
            messageDialog.initCategoryList(this);
            mainFormGlobal = mainForm;

            int i = 0;
            while (i < constants.saleCategories.Length)
            {
                totalNumber++;
                foreach (string prdItem in constants.productBigName[i])
                {
                    totalNumber++;
                }
                i++;
            }

            Panel mainPanels = createPanel.CreateMainPanel(mainForm, 0, 0, mainForm.Width, mainForm.Height, BorderStyle.None, Color.Transparent);
            mainPanelGlobal = mainPanels;
            Label categoryLabel = createLabel.CreateLabelsInPanel(mainPanels, "categoryLabel", constants.categoryListTitleLabel, 0, 50, mainPanels.Width / 2, 50, Color.Transparent, Color.Black, 22, false, ContentAlignment.MiddleRight);

            dropDownMenu.CreateDropDown("categoryList", mainPanels, constants.saleCategories, mainPanels.Width / 2, 50, 200, 50, 200, 50 * (constants.saleCategories.Length + 1), 200, 50, Color.Red, Color.Yellow);

            Label categoryTimeLabel = createLabel.CreateLabelsInPanel(mainPanels, "categoryTimeLabel", constants.TimeLabel + " : ", 0, 130, mainPanels.Width / 2, 50, Color.Transparent, Color.Black, 22, false, ContentAlignment.BottomRight);

            Button previewButton = customButton.CreateButton(constants.prevButtonLabel, "previewButton", mainPanelGlobal.Width - 250, mainPanels.Height * 3 / 5 + 30, 200, 50, Color.FromArgb(255, 0, 176, 80), Color.Transparent, 0, 1, 18);
            mainPanels.Controls.Add(previewButton);
            previewButton.Click += new EventHandler(this.PreviewSalePage);

            Button printButton = customButton.CreateButton(constants.printButtonLabel, "categoryPrintButton", mainPanelGlobal.Width - 250, mainPanels.Height * 3 / 5 + 110, 200, 50, Color.FromArgb(255, 0, 176, 240), Color.Transparent, 0, 1, 18);
            mainPanels.Controls.Add(printButton);
            printButton.Click += new EventHandler(messageDialog.MessageDialogInit);

            Button closeButton = customButton.CreateButton(constants.backText, "closeButton", mainPanelGlobal.Width - 250, mainPanels.Height * 3 / 5 + 190, 200, 50, Color.FromArgb(255, 0, 112, 192), Color.Transparent, 0, 1, 18);
            mainPanels.Controls.Add(closeButton);
            closeButton.Click += new EventHandler(this.BackShow);

            Panel detailPanel = createPanel.CreateSubPanel(mainPanels, 50, 200, mainPanelGlobal.Width * 5 / 7, mainPanelGlobal.Height - 250, BorderStyle.None, Color.Transparent);
            detailPanelGlobal = detailPanel;

            FlowLayoutPanel tableHeaderInUpPanel = createPanel.CreateFlowLayoutPanel(detailPanel, 0, 0, detailPanel.Width, 50, Color.Transparent, new Padding(0));
            Label tableHeaderLabel1 = createLabel.CreateLabels(tableHeaderInUpPanel, "tableHeaderLabel1", constants.printProductNameField, 0, 0, tableHeaderInUpPanel.Width / 2, 50, Color.FromArgb(255, 147, 184, 219), Color.Black, 16, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
            Label tableHeaderLabel2 = createLabel.CreateLabels(tableHeaderInUpPanel, "tableHeaderLabel2", constants.salePriceField, tableHeaderLabel1.Right, 0, tableHeaderInUpPanel.Width / 4, 50, Color.FromArgb(255, 147, 184, 219), Color.Black, 16, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
            Label tableHeaderLabel3 = createLabel.CreateLabels(tableHeaderInUpPanel, "tableHeaderLabel3", constants.saleLimitField, tableHeaderLabel2.Right, 0, tableHeaderInUpPanel.Width / 4, 50, Color.FromArgb(255, 147, 184, 219), Color.Black, 16, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);

            Panel tBodyPanel = createPanel.CreateSubPanel(detailPanel, 0, 50, detailPanel.Width, detailPanel.Height - 50, BorderStyle.FixedSingle, Color.White);
            tBodyPanelGlobal = tBodyPanel;

            printDocument1.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
            printDocument1.EndPrint += new PrintEventHandler(PrintEnd);
            ShowCategoryDetail(0);
        }

        private void PrintEnd(object sender, PrintEventArgs e)
        {
            lineNum = 0;
            groupNumber = 0;
            flagInt = 0;
            itemperpage = 0;
            lineNums = 0;
            groupNumbers = 0;
            flagInts = 0;
            itemperpages = 0;

        }
        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            PrintDocument printDocument = (PrintDocument)sender;
            if (printDocument.PrintController.IsPreview == false)
            {
                float currentY = 30;// declare  one variable for height measurement
                e.Graphics.DrawString(constants.groupListPrintTitle, new Font("Seri", 18, FontStyle.Bold), Brushes.Black, 100, currentY);//this will print one heading/title in every page of the document
                currentY += 30;
                e.Graphics.DrawString("2020/04/18　　22:05:59", new Font("Seri", 14, FontStyle.Bold), Brushes.Black, 100, currentY);//this will print one heading/title in every page of the document
                currentY += 30;
                e.Graphics.DrawString(constants.storeName, new Font("Seri", 18, FontStyle.Bold), Brushes.Black, 100, currentY);//this will print one heading/title in every page of the document
                currentY += 35;
                while (lineNums < totalNumber)
                {
                    //ReadGroupData(e, flagInt, groupNumber, itemperpage, currentY);

                    if (flagInts == 0)
                    {
                        e.Graphics.DrawString(constants.groupTitleLabel + (groupNumbers + 1) + ": " + constants.saleCategories[groupNumbers], DefaultFont, Brushes.Black, 50, currentY + 10);//print each item
                        currentY += 30;
                    }
                    else if (flagInts <= constants.productBigName[groupNumbers].Length)
                    {
                        e.Graphics.DrawString(constants.productBigName[groupNumbers][flagInts - 1], DefaultFont, Brushes.Black, 50, currentY);//print each item
                        e.Graphics.DrawString(constants.productBigPrice[groupNumbers][flagInts - 1].ToString() + constants.unit, DefaultFont, Brushes.Black, 200, currentY);//print each item
                        currentY += 25;
                    }
                    else
                    {
                        flagInts = -1;
                        groupNumbers++;
                    }
                    flagInts++;
                    if (itemperpages < 26) // check whether  the number of item(per page) is more than 20 or not
                    {
                        itemperpages += 1; // increment itemperpage by 1
                        e.HasMorePages = false; // set the HasMorePages property to false , so that no other page will not be added
                    }

                    else // if the number of item(per page) is more than 20 then add one page
                    {
                        itemperpages = 0; //initiate itemperpage to 0 .
                        e.HasMorePages = true; //e.HasMorePages raised the PrintPage event once per page .
                        return;//It will call PrintPage event again

                    }
                    lineNums++;
                }
                //ReadGroupData(e, lineNums, totalNumber, flagInts, groupNumbers, itemperpages);
            }
            else
            {

                float currentY = 10;// declare  one variable for height measurement
                e.Graphics.DrawString(constants.groupListPrintTitle, new Font("Seri", 18, FontStyle.Bold), Brushes.Black, 100, currentY);//this will print one heading/title in every page of the document
                currentY += 30;
                e.Graphics.DrawString("2020/04/18　　22:05:59", new Font("Seri", 14, FontStyle.Bold), Brushes.Black, 100, currentY);//this will print one heading/title in every page of the document
                currentY += 30;
                e.Graphics.DrawString(constants.storeName, new Font("Seri", 18, FontStyle.Bold), Brushes.Black, 100, currentY);//this will print one heading/title in every page of the document
                currentY += 35;
                while (lineNum < totalNumber)
                //foreach (string categoryItem in constants.saleCategories)
                {
                    //ReadGroupData(e, flagInt, groupNumber, itemperpage, currentY);

                    if (flagInt == 0)
                    {
                        e.Graphics.DrawString(constants.groupTitleLabel + (groupNumber + 1) + ": " + constants.saleCategories[groupNumber], DefaultFont, Brushes.Black, 50, currentY + 10);//print each item
                        currentY += 30;
                    }
                    else if (flagInt <= constants.productBigName[groupNumber].Length)
                    {
                        e.Graphics.DrawString(constants.productBigName[groupNumber][flagInt - 1], DefaultFont, Brushes.Black, 50, currentY);//print each item
                        e.Graphics.DrawString(constants.productBigPrice[groupNumber][flagInt - 1].ToString() + constants.unit, DefaultFont, Brushes.Black, 200, currentY);//print each item
                        currentY += 25;
                    }
                    else
                    {
                        flagInt = -1;
                        groupNumber++;
                    }
                    flagInt++;
                    if (itemperpage < 26) // check whether  the number of item(per page) is more than 20 or not
                    {
                        itemperpage += 1; // increment itemperpage by 1
                        e.HasMorePages = false; // set the HasMorePages property to false , so that no other page will not be added
                    }

                    else // if the number of item(per page) is more than 20 then add one page
                    {
                        itemperpage = 0; //initiate itemperpage to 0 .
                        e.HasMorePages = true; //e.HasMorePages raised the PrintPage event once per page .
                        return;//It will call PrintPage event again

                    }
                    lineNum++;
                }

                //ReadGroupData(e, lineNum, totalNumber, flagInt, groupNumber, itemperpage);
            }
        }

        private void ShowCategoryDetail(int categoryID)
        {
            Label categoryTimeValue = createLabel.CreateLabelsInPanel(mainPanelGlobal, "categoryTimeValue", "10：00～21:59", mainPanelGlobal.Width / 2, 130, mainPanelGlobal.Width / 2, 50, Color.Transparent, Color.Black, 22, false, ContentAlignment.BottomLeft);
            categoryTimeValue.Padding = new Padding(10, 0, 0, 0);


            tBodyPanelGlobal.HorizontalScroll.Maximum = 0;
            tBodyPanelGlobal.AutoScroll = false;
            tBodyPanelGlobal.VerticalScroll.Visible = false;
            tBodyPanelGlobal.AutoScroll = true;

            int k = 0;
            foreach (string prodItem in constants.productBigName[categoryID])
            {
                FlowLayoutPanel tableRowPanel = createPanel.CreateFlowLayoutPanel(tBodyPanelGlobal, 0, 50 * k, tBodyPanelGlobal.Width, 50, Color.Transparent, new Padding(0));
                Label tdLabel1 = createLabel.CreateLabels(tableRowPanel, "tdLabel1_" + k, prodItem, 0, 0, tableRowPanel.Width / 2, 50, Color.White, Color.Black, 16, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
                Label tdLabel2 = createLabel.CreateLabels(tableRowPanel, "tdLabel2_" + k, constants.productBigPrice[categoryID][k].ToString(), tdLabel1.Right, 0, tableRowPanel.Width / 4, 50, Color.White, Color.Black, 16, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
                Label tdLabel3 = createLabel.CreateLabels(tableRowPanel, "tdLabel3_" + k, constants.productBigSaleAmount[categoryID][k], tdLabel2.Right, 0, tableRowPanel.Width / 4, 50, Color.White, Color.Black, 16, true, ContentAlignment.MiddleCenter, new Padding(0), 1, Color.Gray);
                k++;
            }

        }

        public void setVal(string categoryID)
        {
            tBodyPanelGlobal.Controls.Clear();
            categoryIDGlobal = int.Parse(categoryID);
            ShowCategoryDetail(int.Parse(categoryID));
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

        public void PreviewSalePage(object sender, EventArgs e)
        {
            mainFormGlobal.Controls.Clear();
            PreviewSalePage frm = new PreviewSalePage(mainFormGlobal, mainPanelGlobal, categoryIDGlobal);
            frm.TopLevel = false;
            mainFormGlobal.Controls.Add(frm);
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;
            Thread.Sleep(200);
            frm.Show();
        }

        public void PrintPreview_click()
        {
            printPreviewDialog1.Document = printDocument1;
            //printDialog1.Document = printDocument1;

            ((ToolStripButton)((ToolStrip)printPreviewDialog1.Controls[1]).Items[0]).Enabled = true;


            printDocument1.DefaultPageSettings.PaperSize = paperSize;
            printPreviewDialog1.ShowDialog();

        }
    }
}

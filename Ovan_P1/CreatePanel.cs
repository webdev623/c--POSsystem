using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ovan_P1
{
    class CreatePanel
    {
        
        public Panel CreateMainPanel(Form formpanel, int PanelLeft, int PanelTop, int PanelWidth, int PanelHeight, BorderStyle borderstyle, Color colors)
        {
            GradientPanel Panel = new GradientPanel();
            Panel.Size = new Size(PanelWidth, PanelHeight);
            Panel.Location = new Point(PanelLeft, PanelTop);
            Panel.BorderStyle = borderstyle;
            Panel.BackColor = colors;
            formpanel.Controls.Add(Panel);
            return Panel;
        }
        public Panel CreateSubPanel(Panel parentPanel, int PanelLeft, int PanelTop, int PanelWidth, int PanelHeight, BorderStyle borderstyle, Color color, Color colorTop = default(Color), Color colorBottom = default(Color))
        {
            GradientPanel Panel = new GradientPanel();
            Panel.Size = new Size(PanelWidth, PanelHeight);
            Panel.Location = new Point(PanelLeft, PanelTop);
            Panel.BorderStyle = borderstyle;
            Panel.BackColor = color;

            Panel.ColorTop = colorTop;
            Panel.ColorBottom = colorBottom;
            parentPanel.Controls.Add(Panel);
            return Panel;
        }
        public FlowLayoutPanel CreateFlowLayoutPanel(Panel panel, int PanelLeft, int PanelTop, int PanelWidth, int PanelHeight, Color color, Padding paddings, bool borderEnable = false)
        {
            FlowLayoutPanel FlowPanel = new FlowLayoutPanel();
            FlowPanel.Size = new Size(PanelWidth, PanelHeight);
            FlowPanel.Location = new Point(PanelLeft, PanelTop);
            FlowPanel.BackColor = color;
            if (borderEnable)
            {
               FlowPanel.BorderStyle = BorderStyle.FixedSingle;
            }
            FlowPanel.Padding = paddings;
            panel.Controls.Add(FlowPanel);
            return FlowPanel;
        }

        public Panel CreatePanelForProducts(int nX, int nY, int W, int H, string name, bool hasBD, Color bdClr, Color backClr)
        {

            Panel p = new Panel();
            p.Location = new Point(nX, nY);
            p.Size = new Size(W, H);
            p.Name = name;
            p.BackColor = backClr;
            //p.Click += new EventHandler(IAFS.onItemPanelClk);

            return p;
        }


        public PictureBox CreatePictureBox(int nX, int nY, int W, int H, string name, string imgUrl)
        {
            PictureBox pb = new PictureBox();
            pb.Location = new Point(nX, nY);
            pb.Name = name;
            pb.Size = new Size(W, H);
            pb.BackColor = Color.Transparent;
            if (imgUrl != null && imgUrl != "")
                pb.Image = Image.FromFile(@imgUrl);

            pb.SizeMode = PictureBoxSizeMode.StretchImage;
            return pb;
        }


    }
}

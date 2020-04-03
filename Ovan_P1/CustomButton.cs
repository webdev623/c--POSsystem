﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ovan_P1
{
    class CustomButton
    {
        public Button CreateButton(string btnText, string btnName, int btnLeft, int btnTop, int btnWidth, int btnHeight, Color backColor, Color borderColor, int borderSize, int radiusValue = 20, int fontSize = 12, FontStyle fontStyle = FontStyle.Regular, Color foreColor = default(Color), ContentAlignment textAlign = ContentAlignment.MiddleCenter)
        {
            RoundedButton btn = new RoundedButton();
            btn.Name = btnName;
            btn.Text = btnText;
            btn.ForeColor = foreColor;
            int xCordinator = btnLeft;

            btn.Location = new Point(btnLeft, btnTop);
            btn.Width = btnWidth;
            btn.Height = btnHeight;
            btn.BackColor = backColor;
            // btn.FlatStyle = FlatStyle.Flat;
            // btn.FlatAppearance.BorderColor = borderColor;
            // btn.FlatAppearance.BorderSize = borderSize;

            btn.Font = new Font("Seri", fontSize, fontStyle);
            btn.TextAlign = ContentAlignment.MiddleCenter;
            btn.radiusValue = radiusValue;
            btn.borderColor = borderColor;
            btn.borderSize = borderSize;

            return btn;

        }
        public Button CreateButtonWithImage(Image btnImage, string btnName, string btnText, int btnLeft, int btnTop, int btnWidth, int btnHeight, int borderSize, int radiusValue, int fontSize = 12, FontStyle fontStyle = FontStyle.Regular, Color foreColor = default(Color), ContentAlignment textAlign = ContentAlignment.MiddleCenter)
        {
            RoundedButton btn = new RoundedButton();
            btn.Name = btnName;
            btn.BackgroundImage = btnImage;
            btn.BackgroundImageLayout = ImageLayout.Stretch;
            btn.Location = new Point(btnLeft, btnTop);
            btn.Size = new Size(btnWidth, btnHeight);
            btn.Font = new Font("Seri", 12, FontStyle.Regular);
            btn.TextAlign = ContentAlignment.MiddleCenter;
            btn.radiusValue = radiusValue;
            btn.borderColor = Color.Transparent;
            btn.borderSize = borderSize;
            btn.TabStop = false;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderColor = Color.FromArgb(255, 255, 255, 255);
            return btn;

        }
    }
}

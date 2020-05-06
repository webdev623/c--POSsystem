using MetroFramework.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ovan_P1
{
    class CreateCombobox
    {
        Constant constants = new Constant();
        public ComboBox CreateComboboxs(Panel combPanel, string combName, string[] combItems, int combLeft, int combTop, int combWidth, int combHeight, int combItemHeight, Font combFont, string selectText)
        {
            ComboBox dateCombobox = new ComboBox();
            dateCombobox.Location = new Point(combLeft, combTop);
            //dateCombobox.DataSource = combItems;
            dateCombobox.DrawMode = DrawMode.OwnerDrawVariable;
            dateCombobox.DropDownWidth = combWidth;
            //dateCombobox.FormattingEnabled = true;
            dateCombobox.Items.AddRange(combItems);
            dateCombobox.ItemHeight = combItemHeight;
            dateCombobox.SelectedIndex = dateCombobox.FindStringExact(selectText);
            dateCombobox.Name = combName;
            dateCombobox.Size = new Size(combWidth, combHeight);
            dateCombobox.Font = combFont;
            combPanel.Controls.Add(dateCombobox);
            dateCombobox.SelectionLength = 0;
            dateCombobox.SelectionStart = 0;

            return dateCombobox;
        }
        public class SeparatorItem
        {
            private object data;
            public SeparatorItem(object data)
            {
                this.data = data;
            }
            public override string ToString()
            {
                if (data != null)
                {
                    return data.ToString();
                }
                return base.ToString();
            }
        }
        public void dateCombobox_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox ComboboxHadler = (ComboBox)sender;
            object comboBoxItem = ComboboxHadler.Items[e.Index];

            bool isSeparatorItem = (comboBoxItem is SeparatorItem);
            e.DrawBackground();
            e.DrawFocusRectangle();
            using (Brush textBrush = new SolidBrush(e.ForeColor))
            {
                Rectangle bounds = e.Bounds;


                using (StringFormat format = new StringFormat())
                {
                    format.LineAlignment = StringAlignment.Center;
                    format.Alignment = StringAlignment.Near;
                    // e.Graphics.DrawString(comboBoxItem.ToString(), comboBox3.Font, textBrush, bounds, format);
                    e.Graphics.DrawString(comboBoxItem.ToString(), ComboboxHadler.Font, textBrush, bounds);
                }

            }
        }

        public void dateCombobox_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            ComboBox ComboboxHadler = (ComboBox)sender;
            object comboBoxItem = ComboboxHadler.Items[e.Index];
            e.ItemWidth = 50;
            e.ItemHeight = ComboboxHadler.Height;

            Size textSize = e.Graphics.MeasureString(comboBoxItem.ToString(), ComboboxHadler.Font).ToSize();

            e.ItemWidth = textSize.Width;

        }
    }
}

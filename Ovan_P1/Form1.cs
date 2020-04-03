using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ovan_P1
{
    public partial class Form1 : Form
    {
        MainMenu mainMenu = new MainMenu();
        public Form1()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            this.BackColor = Color.White;
            mainMenu.CreateMainMenuScreen(this, this.panel1);
        }

    }
}

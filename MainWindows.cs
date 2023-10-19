using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TRBDApp
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Clientbutton_Click(object sender, EventArgs e)
        {
            this.Hide();
            ClientEntry NewEntry = new ClientEntry();
            NewEntry.Show();
        }

        private void Managerbutton_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManagerEntry newManagerEntry = new ManagerEntry();
            newManagerEntry.Show();
        }
    }
}


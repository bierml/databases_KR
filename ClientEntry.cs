using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TRBDApp
{
    public partial class ClientEntry : Form
    {
        public ClientEntry()
        {
            InitializeComponent();
        }

        private void CreateAccount_Click(object sender, EventArgs e)
        {
            this.Close();
            ClientReg NewClientReg = new ClientReg();
            NewClientReg.Show();
        }

        private void ClientEnterButton_Click(object sender, EventArgs e)
        {
            string sql = "SELECT login,password FROM Clients";
            SqlConnection conn = DBUtils.GetDBConnection();
            SqlCommand cmd = new SqlCommand();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = sql;
            using (DbDataReader reader = cmd.ExecuteReader())
            {
                if (UserLogin.Text == "" || UserPassword.Text == "")
                    MessageBox.Show("Заполните все поля!");
                else
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int loginind = reader.GetOrdinal("login");
                            string loginstr = reader.GetString(loginind);
                            int passwordind = reader.GetOrdinal("password");
                            string passwordstr = reader.GetString(passwordind);
                            if (loginstr == UserLogin.Text && passwordstr == UserPassword.Text)
                            {
                                //---смена окна+выход из функции
                                this.Hide();
                                ClientWindow newClientWindow = new ClientWindow();
                                newClientWindow.Show();
                                conn.Close();
                                conn.Dispose();
                                conn = null;
                                return;
                            }
                        }
                    }
                    MessageBox.Show("Аккаунт не найден, проверьте правильность введенных данных!");
                }
            }
        }
        Point lastPoint;
        private void ClientEntryClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ClientEntry_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void ClientEntry_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }
    }
}

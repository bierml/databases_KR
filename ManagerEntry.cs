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
    public partial class ManagerEntry : Form
    {
        public ManagerEntry()
        {
            InitializeComponent();
        }

        private void ManagerEnterButton_Click(object sender, EventArgs e)
        {
            string sql = "SELECT status,login,password FROM ManagerTable";
            SqlConnection conn = DBUtils.GetDBConnection();
            SqlCommand cmd = new SqlCommand();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = sql;
            using (DbDataReader reader = cmd.ExecuteReader())
            {
                if (ManagerLogin.Text == "" || ManagerPassword.Text == "")
                    MessageBox.Show("Заполните все поля!");
                else
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int statusind = reader.GetOrdinal("status");
                            int loginind = reader.GetOrdinal("login");
                            int passwordind = reader.GetOrdinal("password");
                            CurrentUser.CurrentUserStatus = reader.GetInt32(statusind);
                            string loginstr = reader.GetString(loginind);
                            string passwordstr = reader.GetString(passwordind);
                            if (loginstr == ManagerLogin.Text && passwordstr == ManagerPassword.Text)
                            {
                                this.Hide();
                                ManagerWindow newManagerWindow = new ManagerWindow();
                                newManagerWindow.Show();
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

        private void ManagerEntryClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        Point lastPoint;
        private void ManagerEntry_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void ManagerEntry_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }
    }
    static class CurrentUser
    {
        public static int CurrentUserStatus = 0;
    }
}

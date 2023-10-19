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
    public partial class ClientReg : Form
    {
        public ClientReg()
        {
            InitializeComponent();
        }

        private void ClientRegister_Click(object sender, EventArgs e)
        {
            bool a = ((ClientName.Text == "") || (ClientSurname.Text == "") || (ClientPatronymic.Text == "") || (ClientBirthdate.Text == "") || (ClientPhonenum.Text == "") || (ClientEmail.Text == "") || (ClientLogin.Text == "") || (ClientPassword.Text == ""));
            string login = ClientLogin.Text;
            string password = ClientPassword.Text;
            SqlConnection conn = DBUtils.GetDBConnection();
            if (a)
                MessageBox.Show("Заполните все поля для ввода данных!");
            else
            {
                try
                {
                    int Clientid = 0;
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    string sql = "INSERT INTO Clients (FIO,Birthdate,Phonenum,Email,login,password) " + " VALUES (@FIO,@Birthdate,@Phonenum,@Email,@login,@password)";
                    cmd.CommandText = sql;
                    SqlParameter FIOParam = new SqlParameter("@FIO", SqlDbType.VarChar);
                    FIOParam.Value = ClientName.Text + " " + ClientSurname.Text + " " + ClientPatronymic.Text;
                    cmd.Parameters.Add(FIOParam);
                    SqlParameter BirthdateParam = new SqlParameter("@Birthdate", SqlDbType.VarChar);
                    BirthdateParam.Value = ClientBirthdate.Text;
                    cmd.Parameters.Add(BirthdateParam);
                    SqlParameter PhonenumParam = new SqlParameter("@Phonenum", SqlDbType.VarChar);
                    PhonenumParam.Value = ClientPhonenum.Text;
                    cmd.Parameters.Add(PhonenumParam);
                    SqlParameter EmailParam = new SqlParameter("@Email", SqlDbType.VarChar);
                    EmailParam.Value = ClientEmail.Text;
                    cmd.Parameters.Add(EmailParam);
                    SqlParameter loginParam = new SqlParameter("@login", SqlDbType.VarChar);
                    loginParam.Value = ClientLogin.Text;
                    cmd.Parameters.Add(loginParam);
                    SqlParameter passwordParam = new SqlParameter("@password", SqlDbType.VarChar);
                    passwordParam.Value = ClientPassword.Text;
                    cmd.Parameters.Add(passwordParam);
                    cmd.ExecuteNonQuery();
                    string sql1 = "SELECT Clientid FROM Clients WHERE FIO=@FIO AND Birthdate=@Birthdate AND Phonenum=@Phonenum AND Email=@Email AND login=@login AND password=@password";
                    cmd.CommandText = sql1;
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        if(reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int Clientidind = reader.GetOrdinal("Clientid");
                                Clientid = (int)reader.GetInt64(Clientidind);
                            }
                        }
                    }
                    conn.Close();
                    conn.Dispose();
                    conn = null;
                    MessageBox.Show("Вы успешно зарегистрированы!\n" + "Ваш ID: " + Convert.ToString(Clientid));
                }
                catch
                {
                    MessageBox.Show("При выполнении команды произошла ошибка!");
                }
            }
        }
        Point lastPoint;
        private void ClientRegClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ClientReg_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void ClientReg_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }
    }
}

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
    public partial class ClientWindow : Form
    {
        public ClientWindow()
        {
            InitializeComponent();
        }

        private void ClientWindow_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "lastVersDataSet.ServtypeList". При необходимости она может быть перемещена или удалена.
            this.servtypeListTableAdapter1.Fill(this.lastVersDataSet.ServtypeList);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "tR_A_12_19_06DataSet1.ServtypeList". При необходимости она может быть перемещена или удалена.
            this.servtypeListTableAdapter.Fill(this.tR_A_12_19_06DataSet1.ServtypeList);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "tR_A_12_19_06DataSet1.Servtype". При необходимости она может быть перемещена или удалена.
            this.servtypeTableAdapter.Fill(this.tR_A_12_19_06DataSet1.Servtype);
        }

        private void servtypeBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.servtypeBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.tR_A_12_19_06DataSet1);

        }

        private void NewServ_Click(object sender, EventArgs e)
        {
            SqlConnection conn = DBUtils.GetDBConnection();
            bool b = ((Servnum.Text == "") || (VisitTime.Text == "") || (ClientID.Text == ""));
            if (b)
                MessageBox.Show("Заполните все поля для ввода данных!");
            else
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    string sql = "INSERT INTO Servlist (Status,Servtime,Disc,Servnum,Client) "+"VALUES (@Status,@Servtime,@Disc,@Servnum,@Client)";
                    cmd.CommandText = sql;
                    SqlParameter StatusParam = new SqlParameter("@Status", SqlDbType.VarChar);
                    StatusParam.Value = "Запись";
                    cmd.Parameters.Add(StatusParam);
                    SqlParameter ServtimeParam = new SqlParameter("@Servtime", SqlDbType.DateTime);
                    ServtimeParam.Value = Convert.ToDateTime(VisitTime.Text);
                    cmd.Parameters.Add(ServtimeParam);
                    SqlParameter DiscParam = new SqlParameter("@Disc", SqlDbType.Int);
                    DiscParam.Value = 0;
                    cmd.Parameters.Add(DiscParam);
                    SqlParameter ServnumParam = new SqlParameter("@Servnum", SqlDbType.Int);
                    ServnumParam.Value = Convert.ToInt32(Servnum.Text);
                    cmd.Parameters.Add(ServnumParam);
                    SqlParameter ClientParam = new SqlParameter("@Client", SqlDbType.Int);
                    ClientParam.Value = Convert.ToInt32(ClientID.Text);
                    cmd.Parameters.Add(ClientParam);
                    cmd.ExecuteNonQuery();
                    string sql1 = "SELECT Name,Address FROM Filials LEFT JOIN Masters ON Filials.Filid = Masters.Fil LEFT JOIN Servtype ON Servtype.Masterid = Masters.Masterid WHERE Servnum=@Servn";
                    SqlParameter ServnParam = new SqlParameter("@Servn", SqlDbType.BigInt);
                    ServnParam.Value = Convert.ToInt32(Servnum.Text);
                    cmd.Parameters.Add(ServnParam);
                    cmd.CommandText = sql1;
                    string outputstr = "";
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int Nameind = reader.GetOrdinal("Name");
                                string Name = reader.GetString(Nameind);
                                int Addressind = reader.GetOrdinal("Address");
                                string Address = reader.GetString(Addressind);
                                outputstr += Name + " " + Address;
                            }
                        }
                    }
                    MessageBox.Show("Регистрация прошла успешно!\nИнформация о филиале: "+outputstr);
                }
                catch
                {
                    MessageBox.Show("При выполнении команды произошла ошибка!");
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                    conn = null;
                }
            }
        }

        private void ClientWindowClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        Point lastPoint;
        private void ClientWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void ClientWindow_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }
    }
}

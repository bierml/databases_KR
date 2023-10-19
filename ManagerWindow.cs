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
    public partial class ManagerWindow : Form
    {
        public ManagerWindow()
        {
            InitializeComponent();
        }

        private void ManagerWindow_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "lastVersDataSet.ManagerTableList". При необходимости она может быть перемещена или удалена.
            this.managerTableListTableAdapter2.Fill(this.lastVersDataSet.ManagerTableList);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "lastVersDataSet.Fullservlist". При необходимости она может быть перемещена или удалена.
            this.fullservlistTableAdapter.Fill(this.lastVersDataSet.Fullservlist);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "lastVersDataSet.ServtypeList". При необходимости она может быть перемещена или удалена.
            this.servtypeListTableAdapter1.Fill(this.lastVersDataSet.ServtypeList);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "lastVersDataSet.FilList". При необходимости она может быть перемещена или удалена.
            this.filListTableAdapter1.Fill(this.lastVersDataSet.FilList);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "lastVersDataSet.MastersList". При необходимости она может быть перемещена или удалена.
            this.mastersListTableAdapter1.Fill(this.lastVersDataSet.MastersList);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "lastVersDataSet.ClientsList". При необходимости она может быть перемещена или удалена.
            this.clientsListTableAdapter2.Fill(this.lastVersDataSet.ClientsList);
        }

        private void AddClient_Click(object sender, EventArgs e)
        {
            bool a = ClientName.Text == "" || ClientSurname.Text == "" || ClientPatronymic.Text == "" || ClientBirthdate.Text == "" || ClientPhonenum.Text == "" || ClientEmail.Text == "" || ClientLogin.Text == "" || ClientPassword.Text == "";
            if (a)
                MessageBox.Show("Заполните все поля для ввода данных!");
            else
            {
                SqlConnection conn = DBUtils.GetDBConnection();
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    string sql = "EXECUTE AddClient @FIO,@Birthdate,@Phonenum,@Email,@login,@password";
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
                    conn.Close();
                    conn.Dispose();
                    conn = null;
                    MessageBox.Show("Клиент успешно добавлен!");
                }
                catch
                {
                    conn.Close();
                    conn.Dispose();
                    conn = null;
                    MessageBox.Show("При выполнении команды произошла ошибка, проверьте правильность введенных данных!");
                }
            }
        }

        private void UpdateClient_Click(object sender, EventArgs e)
        {
            int Clientid;
            int flag = 0;
            bool a = ClientID.Text=="" || ClientName.Text == "" || ClientSurname.Text == "" || ClientPatronymic.Text == "" || ClientBirthdate.Text == "" || ClientPhonenum.Text == "" || ClientEmail.Text == "" || ClientLogin.Text == "" || ClientPassword.Text == "";
            if (a)
                MessageBox.Show("Заполните все поля для ввода данных!");
            else
            {
                SqlConnection conn = DBUtils.GetDBConnection();
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    string sql1 = "SELECT Clientid FROM Clients";
                    cmd.CommandText = sql1;
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int Clientidind = reader.GetOrdinal("Clientid");
                                Clientid = (int)reader.GetInt64(Clientidind);
                                if (Clientid == Convert.ToInt64(ClientID.Text))
                                {
                                    flag = 1;
                                    break;
                                }
                            }
                        }
                    }
                    if (flag == 0)
                        throw new OverflowException();
                    string sql = "EXECUTE UpdClient @ID,@FIO,@Phonenum,@Email,@login,@password";
                    cmd.CommandText = sql;
                    SqlParameter IDParam = new SqlParameter("@ID", SqlDbType.BigInt);
                    IDParam.Value = Convert.ToInt64(ClientID.Text);
                    cmd.Parameters.Add(IDParam);
                    SqlParameter FIOParam = new SqlParameter("@FIO", SqlDbType.VarChar);
                    FIOParam.Value = ClientName.Text + " " + ClientSurname.Text + " " + ClientPatronymic.Text;
                    cmd.Parameters.Add(FIOParam);
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
                    conn.Close();
                    conn.Dispose();
                    conn = null;
                    MessageBox.Show("Данные клиента успешно обновлены!");
                }
                catch
                {
                    conn.Close();
                    conn.Dispose();
                    conn = null;
                    MessageBox.Show("При выполнении команды произошла ошибка, проверьте правильность введенных данных!");
                }
            }
        }

        private void ClientDelete_Click(object sender, EventArgs e)
        {
            int Clientid;
            int flag = 0;
            SqlConnection conn = DBUtils.GetDBConnection();
            bool a = ClientID.Text == "";
            if (a)
                MessageBox.Show("Заполните все поля для ввода данных!");
            else
            { 
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    string sql1 = "SELECT Clientid FROM Clients";
                    cmd.CommandText = sql1;
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int Clientidind = reader.GetOrdinal("Clientid");
                                Clientid = (int)reader.GetInt64(Clientidind);
                                if (Clientid == Convert.ToInt64(ClientID.Text))
                                {
                                    flag = 1;
                                    break;
                                }
                            }
                        }
                    }
                    if (flag == 0)
                        throw new OverflowException();
                    string sql = "EXECUTE DelClient @ID";
                    cmd.CommandText = sql;
                    SqlParameter IDParam = new SqlParameter("@ID", SqlDbType.BigInt);
                    IDParam.Value = Convert.ToInt64(ClientID.Text);
                    cmd.Parameters.Add(IDParam);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    conn.Dispose();
                    conn = null;
                    MessageBox.Show("Клиент удален!");
                }
                catch
                {
                    conn.Close();
                    conn.Dispose();
                    conn = null;
                    MessageBox.Show("При выполнении команды произошла ошибка, проверьте правильность введенных данных!");
                }
            }
        }

        private void InsertMaster_Click(object sender, EventArgs e)
        {
            bool a = MasterName.Text == "" || MasterSurname.Text == "" || MasterPatronymic.Text == "" || MasterExp.Text == "" || MasterPhonenum.Text == "" || MasterLabcdate.Text == "" || MasterWage.Text == "" || MasterPrepdate.Text == "" || MasterWagedate.Text == "" || MasterFil.Text == "";
            if (a)
                MessageBox.Show("Заполните все поля для ввода данных!");
            else
            {
                SqlConnection conn = DBUtils.GetDBConnection();
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    string sql = "EXECUTE AddMaster @FIO,@Exper,@Phonenum,@Labcdate,@Wage,@Prepdate,@Wagedate,@Fil";
                    cmd.CommandText = sql;
                    SqlParameter FIOParam = new SqlParameter("@FIO", SqlDbType.VarChar);
                    FIOParam.Value = MasterName.Text + " " + MasterSurname.Text + " " + MasterPatronymic.Text;
                    cmd.Parameters.Add(FIOParam);
                    SqlParameter ExperParam = new SqlParameter("@Exper", SqlDbType.Int);
                    ExperParam.Value = Convert.ToInt32(MasterExp.Text);
                    cmd.Parameters.Add(ExperParam);
                    SqlParameter PhonenumParam = new SqlParameter("@Phonenum", SqlDbType.VarChar);
                    PhonenumParam.Value = MasterPhonenum.Text;
                    cmd.Parameters.Add(PhonenumParam);
                    SqlParameter LabcdateParam = new SqlParameter("@Labcdate", SqlDbType.VarChar);
                    LabcdateParam.Value = MasterLabcdate.Text;
                    cmd.Parameters.Add(LabcdateParam);
                    SqlParameter WageParam = new SqlParameter("@Wage", SqlDbType.Money);
                    WageParam.Value = Convert.ToInt32(MasterWage.Text);
                    cmd.Parameters.Add(WageParam);
                    SqlParameter PrepdateParam = new SqlParameter("@Prepdate", SqlDbType.VarChar);
                    PrepdateParam.Value = MasterPrepdate.Text;
                    cmd.Parameters.Add(PrepdateParam);
                    SqlParameter WagedateParam = new SqlParameter("@Wagedate", SqlDbType.VarChar);
                    WagedateParam.Value = MasterWagedate.Text;
                    cmd.Parameters.Add(WagedateParam);
                    SqlParameter FilParam = new SqlParameter("@Fil", SqlDbType.BigInt);
                    FilParam.Value = Convert.ToInt64(MasterFil.Text);
                    cmd.Parameters.Add(FilParam);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    conn.Dispose();
                    conn = null;
                    MessageBox.Show("Мастер успешно добавлен!");
                }
                catch
                {
                    conn.Close();
                    conn.Dispose();
                    conn = null;
                    MessageBox.Show("При выполнении команды произошла ошибка, проверьте правильность введенных данных!");
                }
            }
        }

        private void UpdateMaster_Click(object sender, EventArgs e)
        {
            int Masterid;
            int flag = 0;
            bool a = MasterId.Text=="" || MasterName.Text == "" || MasterSurname.Text == "" || MasterPatronymic.Text == "" || MasterExp.Text == "" || MasterPhonenum.Text == "" || MasterLabcdate.Text == "" || MasterWage.Text == "" || MasterPrepdate.Text == "" || MasterWagedate.Text == "" || MasterFil.Text == "";
            if (a)
                MessageBox.Show("Заполните все поля для ввода данных!");
            else
            {
                SqlConnection conn = DBUtils.GetDBConnection();
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    string sql1 = "SELECT Masterid FROM Masters";
                    cmd.CommandText = sql1;
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int Masterind = reader.GetOrdinal("Masterid");
                                Masterid = (int)reader.GetInt64(Masterind);
                                if (Masterid == Convert.ToInt64(MasterId.Text))
                                {
                                    flag = 1;
                                    break;
                                }
                            }
                        }
                    }
                    if (flag == 0)
                        throw new OverflowException();
                    string sql = "EXECUTE UpdMaster @Masterid,@FIO,@Exper,@Phonenum,@Wage,@Prepdate,@Wagedate,@Fil";
                    cmd.CommandText = sql;
                    SqlParameter MasteridParam = new SqlParameter("@Masterid", SqlDbType.BigInt);
                    MasteridParam.Value = Convert.ToInt64(MasterId.Text);
                    cmd.Parameters.Add(MasteridParam);
                    SqlParameter FIOParam = new SqlParameter("@FIO", SqlDbType.VarChar);
                    FIOParam.Value = MasterName.Text + " " + MasterSurname.Text + " " + MasterPatronymic.Text;
                    cmd.Parameters.Add(FIOParam);
                    SqlParameter ExperParam = new SqlParameter("@Exper", SqlDbType.Int);
                    ExperParam.Value = Convert.ToInt32(MasterExp.Text);
                    cmd.Parameters.Add(ExperParam);
                    SqlParameter PhonenumParam = new SqlParameter("@Phonenum", SqlDbType.VarChar);
                    PhonenumParam.Value = MasterPhonenum.Text;
                    cmd.Parameters.Add(PhonenumParam);
                    SqlParameter WageParam = new SqlParameter("@Wage", SqlDbType.Money);
                    WageParam.Value = Convert.ToInt32(MasterWage.Text);
                    cmd.Parameters.Add(WageParam);
                    SqlParameter PrepdateParam = new SqlParameter("@Prepdate", SqlDbType.VarChar);
                    PrepdateParam.Value = MasterPrepdate.Text;
                    cmd.Parameters.Add(PrepdateParam);
                    SqlParameter WagedateParam = new SqlParameter("@Wagedate", SqlDbType.VarChar);
                    WagedateParam.Value = MasterWagedate.Text;
                    cmd.Parameters.Add(WagedateParam);
                    SqlParameter FilParam = new SqlParameter("@Fil", SqlDbType.BigInt);
                    FilParam.Value = Convert.ToInt64(MasterFil.Text);
                    cmd.Parameters.Add(FilParam);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    conn.Dispose();
                    conn = null;
                    MessageBox.Show("Данные успешно обновлены!");
                }
                catch
                {
                    conn.Close();
                    conn.Dispose();
                    conn = null;
                    MessageBox.Show("При выполнении команды произошла ошибка, проверьте правильность введенных данных!");
                }
            }
        }

        private void DeleteMaster_Click(object sender, EventArgs e)
        {
            int Masterid;
            int flag = 0;
            SqlConnection conn = DBUtils.GetDBConnection();
            bool a = MasterId.Text == "";
            if (a)
                MessageBox.Show("Заполните все поля для ввода данных!");
            else
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    string sql1 = "SELECT Masterid FROM Masters";
                    cmd.CommandText = sql1;
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int Masterind = reader.GetOrdinal("Masterid");
                                Masterid= (int)reader.GetInt64(Masterind);
                                if (Masterid == Convert.ToInt64(MasterId.Text))
                                {
                                    flag = 1;
                                    break;
                                }
                            }
                        }
                    }
                    if (flag == 0)
                        throw new OverflowException();
                    string sql = "EXECUTE DelMaster @ID";
                    cmd.CommandText = sql;
                    SqlParameter IDParam = new SqlParameter("@ID", SqlDbType.BigInt);
                    IDParam.Value = Convert.ToInt64(MasterId.Text);
                    cmd.Parameters.Add(IDParam);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    conn.Dispose();
                    conn = null;
                    MessageBox.Show("Мастер удален!");
                }
                catch
                {
                    conn.Close();
                    conn.Dispose();
                    conn = null;
                    MessageBox.Show("При выполнении команды произошла ошибка, проверьте правильность введенных данных!");
                }
            }
        }

        private void AddFil_Click(object sender, EventArgs e)
        {
            bool a = FilName.Text == "" || FilAddress.Text == "";
            if (a)
                MessageBox.Show("Заполните все поля для ввода данных!");
            else
            {
                SqlConnection conn = DBUtils.GetDBConnection();
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    string sql = "EXECUTE AddFil @Name,@Address";
                    cmd.CommandText = sql;
                    SqlParameter NameParam = new SqlParameter("@Name", SqlDbType.VarChar);
                    NameParam.Value = FilName.Text;
                    cmd.Parameters.Add(NameParam);
                    SqlParameter AddressParam = new SqlParameter("@Address", SqlDbType.VarChar);
                    AddressParam.Value = FilAddress.Text;
                    cmd.Parameters.Add(AddressParam);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    conn.Dispose();
                    conn = null;
                    MessageBox.Show("Филиал успешно добавлен!");
                }
                catch
                {
                    conn.Close();
                    conn.Dispose();
                    conn = null;
                    MessageBox.Show("При выполнении команды произошла ошибка, проверьте правильность введенных данных!");
                }
            }
        }

        private void UpdFil_Click(object sender, EventArgs e)
        {
            int Filialid;
            int flag = 0;
            bool a = Filid.Text=="" || FilName.Text == "" || FilAddress.Text == "";
            if (a)
                MessageBox.Show("Заполните все поля для ввода данных!");
            else
            {
                SqlConnection conn = DBUtils.GetDBConnection();
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    string sql1 = "SELECT Filid FROM Filials";
                    cmd.CommandText = sql1;
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int Filialind = reader.GetOrdinal("Filid");
                                Filialid = (int)reader.GetInt64(Filialind);
                                if (Filialid == Convert.ToInt64(Filid.Text))
                                {
                                    flag = 1;
                                    break;
                                }
                            }
                        }
                    }
                    if (flag == 0)
                        throw new OverflowException();
                    string sql = "EXECUTE UpdFil @Filid,@Name,@Address";
                    cmd.CommandText = sql;
                    SqlParameter FilidParam = new SqlParameter("@Filid", SqlDbType.BigInt);
                    FilidParam.Value = Convert.ToInt64(Filid.Text);
                    cmd.Parameters.Add(FilidParam);
                    SqlParameter NameParam = new SqlParameter("@Name", SqlDbType.VarChar);
                    NameParam.Value = FilName.Text;
                    cmd.Parameters.Add(NameParam);
                    SqlParameter AddressParam = new SqlParameter("@Address", SqlDbType.VarChar);
                    AddressParam.Value = FilAddress.Text;
                    cmd.Parameters.Add(AddressParam);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    conn.Dispose();
                    conn = null;
                    MessageBox.Show("Данные успешно обновлены!");
                }
                catch
                {
                    conn.Close();
                    conn.Dispose();
                    conn = null;
                    MessageBox.Show("При выполнении команды произошла ошибка, проверьте правильность введенных данных!");
                }
            }
        }
        private void DelFil_Click(object sender, EventArgs e)
        {
            int Filialid;
            int flag = 0;
            SqlConnection conn = DBUtils.GetDBConnection();
            bool a = Filid.Text == "";
            if (a)
                MessageBox.Show("Заполните все поля для ввода данных!");
            else
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    string sql1 = "SELECT Filid FROM Filials";
                    cmd.CommandText = sql1;
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int Filialind = reader.GetOrdinal("Filid");
                                Filialid = (int)reader.GetInt64(Filialind);
                                if (Filialid == Convert.ToInt64(Filid.Text))
                                {
                                    flag = 1;
                                    break;
                                }
                            }
                        }
                    }
                    if (flag == 0)
                        throw new OverflowException();
                    string sql = "EXECUTE DelFil @ID";
                    cmd.CommandText = sql;
                    SqlParameter IDParam = new SqlParameter("@ID", SqlDbType.BigInt);
                    IDParam.Value = Convert.ToInt64(Filid.Text);
                    cmd.Parameters.Add(IDParam);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    conn.Dispose();
                    conn = null;
                    MessageBox.Show("Филиал удален!");
                }
                catch
                {
                    conn.Close();
                    conn.Dispose();
                    conn = null;
                    MessageBox.Show("При выполнении команды произошла ошибка, проверьте правильность введенных данных!");
                }
            }
        }

        private void InsServtype_Click(object sender, EventArgs e)
        {
            bool a = Servtype.Text == "" || Cost.Text == "" || Mastid.Text == "";
            if (a)
                MessageBox.Show("Заполните все поля для ввода данных!");
            else
            {
                SqlConnection conn = DBUtils.GetDBConnection();
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    string sql = "EXECUTE AddServtype @Servtype,@Cost,@Masterid";
                    cmd.CommandText = sql;
                    SqlParameter ServtypeParam = new SqlParameter("@Servtype", SqlDbType.VarChar);
                    ServtypeParam.Value = Servtype.Text;
                    cmd.Parameters.Add(ServtypeParam);
                    SqlParameter CostParam = new SqlParameter("@Cost", SqlDbType.Money);
                    CostParam.Value = Convert.ToInt32(Cost.Text);
                    cmd.Parameters.Add(CostParam);
                    SqlParameter MasteridParam = new SqlParameter("@Masterid", SqlDbType.BigInt);
                    MasteridParam.Value = Convert.ToInt64(Mastid.Text);
                    cmd.Parameters.Add(MasteridParam);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    conn.Dispose();
                    conn = null;
                    MessageBox.Show("Вид услуг успешно добавлен!");
                }
                catch
                {
                    conn.Close();
                    conn.Dispose();
                    conn = null;
                    MessageBox.Show("При выполнении команды произошла ошибка, проверьте правильность введенных данных!");
                }
            }
        }

        private void UpdServtype_Click(object sender, EventArgs e)
        {
            int Servnum;
            int flag = 0;
            bool a = ServtypeNum.Text == "" || Servtype.Text == "" || Cost.Text == "" || Mastid.Text == "";
            if (a)
                MessageBox.Show("Заполните все поля для ввода данных!");
            else
            {
                SqlConnection conn = DBUtils.GetDBConnection();
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    string sql1 = "SELECT Servnum FROM Servtype";
                    cmd.CommandText = sql1;
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int Servnumind = reader.GetOrdinal("Servnum");
                                Servnum = (int)reader.GetInt64(Servnumind);
                                if (Servnum == Convert.ToInt64(ServtypeNum.Text))
                                {
                                    flag = 1;
                                    break;
                                }
                            }
                        }
                    }
                    if (flag == 0)
                        throw new OverflowException();
                    string sql = "EXECUTE UpdServtype @Servnum,@Servtype,@Cost,@Masterid";
                    cmd.CommandText = sql;
                    SqlParameter ServnumParam = new SqlParameter("@Servnum", SqlDbType.BigInt);
                    ServnumParam.Value = Convert.ToInt64(ServtypeNum.Text);
                    cmd.Parameters.Add(ServnumParam);
                    SqlParameter ServtypeParam = new SqlParameter("@Servtype", SqlDbType.VarChar);
                    ServtypeParam.Value = Servtype.Text;
                    cmd.Parameters.Add(ServtypeParam);
                    SqlParameter CostParam = new SqlParameter("@Cost", SqlDbType.Money);
                    CostParam.Value = Convert.ToInt32(Cost.Text);
                    cmd.Parameters.Add(CostParam);
                    SqlParameter MasteridParam = new SqlParameter("@Masterid", SqlDbType.BigInt);
                    MasteridParam.Value = Convert.ToInt64(Mastid.Text);
                    cmd.Parameters.Add(MasteridParam);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    conn.Dispose();
                    conn = null;
                    MessageBox.Show("Данные успешно обновлены!");
                }
                catch
                {
                    conn.Close();
                    conn.Dispose();
                    conn = null;
                    MessageBox.Show("При выполнении команды произошла ошибка, проверьте правильность введенных данных!");
                }
            }
        }

        private void DelServtype_Click(object sender, EventArgs e)
        {
            int Servnum;
            int flag = 0;
            SqlConnection conn = DBUtils.GetDBConnection();
            bool a = ServtypeNum.Text == "";
            if (a)
                MessageBox.Show("Заполните все поля для ввода данных!");
            else
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    string sql1 = "SELECT Servnum FROM Servtype";
                    cmd.CommandText = sql1;
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int Servnumind = reader.GetOrdinal("Servnum");
                                Servnum = (int)reader.GetInt64(Servnumind);
                                if (Servnum == Convert.ToInt64(ServtypeNum.Text))
                                {
                                    flag = 1;
                                    break;
                                }
                            }
                        }
                    }
                    if (flag == 0)
                        throw new OverflowException();
                    string sql = "EXECUTE DelServtype @Servnum";
                    cmd.CommandText = sql;
                    SqlParameter ServnumParam = new SqlParameter("@Servnum", SqlDbType.BigInt);
                    ServnumParam.Value = Convert.ToInt64(ServtypeNum.Text);
                    cmd.Parameters.Add(ServnumParam);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    conn.Dispose();
                    conn = null;
                    MessageBox.Show("Вид услуг удален!");
                }
                catch
                {
                    conn.Close();
                    conn.Dispose();
                    conn = null;
                    MessageBox.Show("При выполнении команды произошла ошибка, проверьте правильность введенных данных!");
                }
            }
        }

        private void AddServ_Click(object sender, EventArgs e)
        {
            bool a = ServStatus.Text == "" || ServTime.Text == "" || ServDisc.Text == "" || ServNum.Text == "" || ServClientId.Text == "";
            if (a)
                MessageBox.Show("Заполните все поля для ввода данных!");
            else
            {
                SqlConnection conn = DBUtils.GetDBConnection();
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    string sql = "EXECUTE AddServ @Status,@Servtime,@Disc,@Servnum,@Client";
                    cmd.CommandText = sql;
                    SqlParameter StatusParam = new SqlParameter("@Status", SqlDbType.VarChar);
                    StatusParam.Value = ServStatus.Text;
                    cmd.Parameters.Add(StatusParam);
                    SqlParameter ServtimeParam = new SqlParameter("@Servtime", SqlDbType.DateTime);
                    ServtimeParam.Value = ServTime.Text;
                    cmd.Parameters.Add(ServtimeParam);
                    SqlParameter DiscParam = new SqlParameter("@Disc", SqlDbType.Money);
                    DiscParam.Value = Convert.ToInt32(ServDisc.Text);
                    cmd.Parameters.Add(DiscParam);
                    SqlParameter ServnumParam = new SqlParameter("@Servnum", SqlDbType.BigInt);
                    ServnumParam.Value = Convert.ToInt64(ServNum.Text);
                    cmd.Parameters.Add(ServnumParam);
                    SqlParameter ClientParam = new SqlParameter("@Client", SqlDbType.BigInt);
                    ClientParam.Value = Convert.ToInt64(ServClientId.Text);
                    cmd.Parameters.Add(ClientParam);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    conn.Dispose();
                    conn = null;
                    MessageBox.Show("Вид услуг успешно добавлен!");
                }
                catch
                {
                    conn.Close();
                    conn.Dispose();
                    conn = null;
                    MessageBox.Show("При выполнении команды произошла ошибка, проверьте правильность введенных данных!");
                }
            }
        }

        private void UpdServ_Click(object sender, EventArgs e)
        {
            int Servnum1;
            int flag = 0;
            bool a = ServId.Text == "" || ServStatus.Text == "" || ServTime.Text == "" || ServDisc.Text == "" || ServNum.Text == "" || ServClientId.Text == "";
            if (a)
                MessageBox.Show("Заполните все поля для ввода данных!");
            else
            {
                SqlConnection conn = DBUtils.GetDBConnection();
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    string sql1 = "SELECT Servid FROM Servlist";
                    cmd.CommandText = sql1;
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int Servidind = reader.GetOrdinal("Servid");
                                Servnum1 = (int)reader.GetInt64(Servidind);
                                if (Servnum1 == Convert.ToInt64(ServId.Text))
                                {
                                    flag = 1;
                                    break;
                                }
                            }
                        }
                    }
                    if (flag == 0)
                        throw new OverflowException();
                    string sql = "EXECUTE UpdServ @Servid,@Status,@Servtime,@Disc,@Servnum,@Client";
                    cmd.CommandText = sql;
                    SqlParameter ServidParam = new SqlParameter("@Servid", SqlDbType.BigInt);
                    ServidParam.Value = Convert.ToInt64(ServId.Text);
                    cmd.Parameters.Add(ServidParam);
                    SqlParameter StatusParam = new SqlParameter("@Status", SqlDbType.VarChar);
                    StatusParam.Value = ServStatus.Text;
                    cmd.Parameters.Add(StatusParam);
                    SqlParameter ServtimeParam = new SqlParameter("@Servtime", SqlDbType.DateTime);
                    ServtimeParam.Value = ServTime.Text;
                    cmd.Parameters.Add(ServtimeParam);
                    SqlParameter DiscParam = new SqlParameter("@Disc", SqlDbType.Money);
                    DiscParam.Value = Convert.ToInt32(ServDisc.Text);
                    cmd.Parameters.Add(DiscParam);
                    SqlParameter ServnumParam = new SqlParameter("@Servnum", SqlDbType.BigInt);
                    ServnumParam.Value = Convert.ToInt64(ServNum.Text);
                    cmd.Parameters.Add(ServnumParam);
                    SqlParameter ClientParam = new SqlParameter("@Client", SqlDbType.BigInt);
                    ClientParam.Value = Convert.ToInt64(ServClientId.Text);
                    cmd.Parameters.Add(ClientParam);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    conn.Dispose();
                    conn = null;
                    MessageBox.Show("Данные успешно обновлены!");
                }
                catch
                {
                    conn.Close();
                    conn.Dispose();
                    conn = null;
                    MessageBox.Show("При выполнении команды произошла ошибка, проверьте правильность введенных данных!");
                }
            }
        }

        private void DelServ_Click(object sender, EventArgs e)
        {
            int Servnum1;
            int flag = 0;
            SqlConnection conn = DBUtils.GetDBConnection();
            bool a = ServId.Text == "";
            if (a)
                MessageBox.Show("Заполните все поля для ввода данных!");
            else
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    string sql1 = "SELECT Servid FROM Servlist";
                    cmd.CommandText = sql1;
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int Servidind = reader.GetOrdinal("Servid");
                                Servnum1 = (int)reader.GetInt64(Servidind);
                                if (Servnum1 == Convert.ToInt64(ServId.Text))
                                {
                                    flag = 1;
                                    break;
                                }
                            }
                        }
                    }
                    if (flag == 0)
                        throw new OverflowException();
                    string sql = "EXECUTE DelServ @Servid";
                    cmd.CommandText = sql;
                    SqlParameter ServidParam = new SqlParameter("@Servid", SqlDbType.BigInt);
                    ServidParam.Value = Convert.ToInt64(ServId.Text);
                    cmd.Parameters.Add(ServidParam);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    conn.Dispose();
                    conn = null;
                    MessageBox.Show("Услуга удалена!");
                }
                catch
                {
                    conn.Close();
                    conn.Dispose();
                    conn = null;
                    MessageBox.Show("При выполнении команды произошла ошибка, проверьте правильность введенных данных!");
                }
            }
        }

        private void InsManager_Click(object sender, EventArgs e)
        {
            if (CurrentUser.CurrentUserStatus > 1)
            {
                bool a = ManagerStatus.Text == "" || ManagerLogin.Text == "" || ManagerPassword.Text == "";
                if (a)
                    MessageBox.Show("Заполните все поля для ввода данных!");
                else
                {
                    SqlConnection conn = DBUtils.GetDBConnection();
                    try
                    {
                        conn.Open();
                        SqlCommand cmd = conn.CreateCommand();
                        string sql = "EXECUTE AddMan @status,@login,@password";
                        cmd.CommandText = sql;
                        SqlParameter statusParam = new SqlParameter("@status", SqlDbType.Int);
                        statusParam.Value = Convert.ToInt32(ManagerStatus.Text);
                        cmd.Parameters.Add(statusParam);
                        SqlParameter loginParam = new SqlParameter("@login", SqlDbType.VarChar);
                        loginParam.Value = ManagerLogin.Text;
                        cmd.Parameters.Add(loginParam);
                        SqlParameter passwordParam = new SqlParameter("@password", SqlDbType.VarChar);
                        passwordParam.Value = ManagerPassword.Text;
                        cmd.Parameters.Add(passwordParam);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        conn.Dispose();
                        conn = null;
                        MessageBox.Show("Менеджер успешно добавлен!");
                    }
                    catch
                    {
                        conn.Close();
                        conn.Dispose();
                        conn = null;
                        MessageBox.Show("При выполнении команды произошла ошибка, проверьте правильность введенных данных!");
                    }
                }
            }
            else
                MessageBox.Show("Недостаточно прав для вызова данной функции! Обратитесь к администраторам с status>=2");
        }

        private void UpdManager_Click(object sender, EventArgs e)
        {
            if (CurrentUser.CurrentUserStatus > 1)
            {
                int Managerid;
                int flag = 0;
                bool a = ManagerId.Text == "" || ManagerStatus.Text == "" || ManagerLogin.Text == "" || ManagerPassword.Text == "";
                if (a)
                    MessageBox.Show("Заполните все поля для ввода данных!");
                else
                {
                    SqlConnection conn = DBUtils.GetDBConnection();
                    try
                    {
                        conn.Open();
                        SqlCommand cmd = conn.CreateCommand();
                        string sql1 = "SELECT id FROM ManagerTable";
                        cmd.CommandText = sql1;
                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    int idind = reader.GetOrdinal("id");
                                    Managerid = (int)reader.GetInt64(idind);
                                    if (Managerid == Convert.ToInt64(ManagerId.Text))
                                    {
                                        flag = 1;
                                        break;
                                    }
                                }
                            }
                        }
                        if (flag == 0)
                            throw new OverflowException();
                        string sql = "EXECUTE UpdMan @id,@status,@login,@password";
                        cmd.CommandText = sql;
                        SqlParameter idParam = new SqlParameter("@id", SqlDbType.BigInt);
                        idParam.Value = Convert.ToInt64(ManagerId.Text);
                        cmd.Parameters.Add(idParam);
                        SqlParameter statusParam = new SqlParameter("@status", SqlDbType.Int);
                        statusParam.Value = Convert.ToInt32(ManagerStatus.Text);
                        cmd.Parameters.Add(statusParam);
                        SqlParameter loginParam = new SqlParameter("@login", SqlDbType.VarChar);
                        loginParam.Value = ManagerLogin.Text;
                        cmd.Parameters.Add(loginParam);
                        SqlParameter passwordParam = new SqlParameter("@password", SqlDbType.VarChar);
                        passwordParam.Value = ManagerPassword.Text;
                        cmd.Parameters.Add(passwordParam);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        conn.Dispose();
                        conn = null;
                        MessageBox.Show("Данные успешно обновлены!");
                    }
                    catch
                    {
                        conn.Close();
                        conn.Dispose();
                        conn = null;
                        MessageBox.Show("При выполнении команды произошла ошибка, проверьте правильность введенных данных!");
                    }
                }
            }
            else
                MessageBox.Show("Недостаточно прав для вызова данной функции! Обратитесь к администраторам с status>=2");

        }

        private void DelManager_Click(object sender, EventArgs e)
        {
            if (CurrentUser.CurrentUserStatus > 1)
            {
                int Managerid;
                int flag = 0;
                SqlConnection conn = DBUtils.GetDBConnection();
                bool a = ManagerId.Text == "";
                if (a)
                    MessageBox.Show("Заполните все поля для ввода данных!");
                else
                {
                    try
                    {
                        conn.Open();
                        SqlCommand cmd = conn.CreateCommand();
                        string sql1 = "SELECT id FROM ManagerTable";
                        cmd.CommandText = sql1;
                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    int idind = reader.GetOrdinal("id");
                                    Managerid = (int)reader.GetInt64(idind);
                                    if (Managerid == Convert.ToInt64(ManagerId.Text))
                                    {
                                        flag = 1;
                                        break;
                                    }
                                }
                            }
                        }
                        if (flag == 0)
                            throw new OverflowException();
                        string sql = "EXECUTE DelMan @id";
                        cmd.CommandText = sql;
                        SqlParameter idParam = new SqlParameter("@id", SqlDbType.BigInt);
                        idParam.Value = Convert.ToInt64(ManagerId.Text);
                        cmd.Parameters.Add(idParam);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        conn.Dispose();
                        conn = null;
                        MessageBox.Show("Менеджер удален!");
                    }
                    catch
                    {
                        conn.Close();
                        conn.Dispose();
                        conn = null;
                        MessageBox.Show("При выполнении команды произошла ошибка, проверьте правильность введенных данных!");
                    }
                }
            }
            else
                MessageBox.Show("Недостаточно прав для вызова данной функции! Обратитесь к администраторам с status>=2");
        }

        private void ClientsListUpdate_Click(object sender, EventArgs e)
        {
            this.clientsListTableAdapter2.Fill(this.lastVersDataSet.ClientsList);
        }

        private void MastersListUpdate_Click(object sender, EventArgs e)
        {
            this.mastersListTableAdapter1.Fill(this.lastVersDataSet.MastersList);
        }

        private void FilListUpdate_Click(object sender, EventArgs e)
        {
            this.filListTableAdapter1.Fill(this.lastVersDataSet.FilList);
        }

        private void ServtypeListUpdate_Click(object sender, EventArgs e)
        {
            this.servtypeListTableAdapter1.Fill(this.lastVersDataSet.ServtypeList);
        }

        private void ServListUpdate_Click(object sender, EventArgs e)
        {
            this.fullservlistTableAdapter.Fill(this.lastVersDataSet.Fullservlist);
        }

        private void ManListUpdate_Click(object sender, EventArgs e)
        {
            this.managerTableListTableAdapter2.Fill(this.lastVersDataSet.ManagerTableList);
        }

        private void ExecFindMasFil_Click(object sender, EventArgs e)
        {
            SqlConnection conn = DBUtils.GetDBConnection();
            bool a = FuncFilid.Text == "";
            if (a)
                MessageBox.Show("Заполните поле для ввода данных!");
            else
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    string outputstr = "";
                    string sql1 = "SELECT * FROM FindFilMas(@ID)";
                    SqlParameter IDParam = new SqlParameter("@id", SqlDbType.BigInt);
                    IDParam.Value = Convert.ToInt64(FuncFilid.Text);
                    cmd.Parameters.Add(IDParam);
                    cmd.CommandText = sql1;
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int MasIdind = reader.GetOrdinal("Id мастера");
                                int MasId = (int)reader.GetInt64(MasIdind);
                                int FIOind = reader.GetOrdinal("ФИО мастера");
                                string FIO = reader.GetString(FIOind);
                                int ExpInd = reader.GetOrdinal("Стаж работы");
                                int Exp = reader.GetInt32(ExpInd);
                                int PhonenumInd = reader.GetOrdinal("Номер телефона");
                                string Phonenum = reader.GetString(PhonenumInd);
                                int LabcdateInd = reader.GetOrdinal("Дата закл. труд. договора");
                                string Labcdate = Convert.ToString(reader.GetDateTime(LabcdateInd));
                                int WageInd = reader.GetOrdinal("Размер зп");
                                int Wage = Convert.ToInt32(reader.GetValue(WageInd));
                                int PrepDateInd = reader.GetOrdinal("Дата предопл.");
                                string PrepDate = Convert.ToString(reader.GetDateTime(PrepDateInd));
                                int WageDateInd = reader.GetOrdinal("Дата зп");
                                string WageDate = Convert.ToString(reader.GetDateTime(WageDateInd));
                                outputstr += Convert.ToString(MasId) + " " + FIO + " " + Convert.ToString(Exp) + " " + Phonenum + " " + Labcdate + " " + Convert.ToString(Wage) + " " + PrepDate + " " + WageDate + "\n";
                            }
                        }
                    }
                    conn.Close();
                    conn.Dispose();
                    conn = null;
                    if (outputstr == "")
                        outputstr = "нет";
                    MessageBox.Show("Найденные записи:\n" + outputstr);
                }
                catch
                {
                    conn.Close();
                    conn.Dispose();
                    conn = null;
                    MessageBox.Show("При выполнении команды произошла ошибка, проверьте правильность введенных данных!");
                }
               }
            }

        private void ExecSumServPrice_Click(object sender, EventArgs e)
        {
            SqlConnection conn = DBUtils.GetDBConnection();
            bool a = FuncClientId.Text == "";
            if (a)
                MessageBox.Show("Заполните поле для ввода данных!");
            else
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    string outputstr = "";
                    string sql1 = "SELECT dbo.SumServPrice(@id) AS 'SUMPR'";
                    SqlParameter IDParam = new SqlParameter("@id", SqlDbType.BigInt);
                    IDParam.Value = Convert.ToInt64(FuncClientId.Text);
                    cmd.Parameters.Add(IDParam);
                    cmd.CommandText = sql1;
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int SumprInd = reader.GetOrdinal("SUMPR");
                                int Sumpr = Convert.ToInt32(reader.GetValue(SumprInd));
                                outputstr += Convert.ToString(Sumpr);
                            }
                        }
                    }
                    conn.Close();
                    conn.Dispose();
                    conn = null;
                    MessageBox.Show("Суммарная цена:\n" + outputstr);
                }
                catch
                {
                    conn.Close();
                    conn.Dispose();
                    conn = null;
                    MessageBox.Show("При выполнении команды произошла ошибка, проверьте правильность введенных данных!");
                }
            }
        }

        private void ExecCheckN_Click(object sender, EventArgs e)
        {
            SqlConnection conn = DBUtils.GetDBConnection();
            bool a = Phonenum.Text == "";
            if (a)
                MessageBox.Show("Заполните поле для ввода данных!");
            else
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    string outputstr = "";
                    string sql1 = "SELECT dbo.CheckN(@PhN) AS 'CheckN'";
                    SqlParameter PhNParam = new SqlParameter("@PhN", SqlDbType.NVarChar);
                    PhNParam.Value = Phonenum.Text;
                    cmd.Parameters.Add(PhNParam);
                    cmd.CommandText = sql1;
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int CheckNind = reader.GetOrdinal("CheckN");
                                bool CheckN = reader.GetBoolean(CheckNind);
                                if (!CheckN)
                                  outputstr += "False";
                                else
                                  outputstr += "True";
                        }
                        }
                    }
                    conn.Close();
                    conn.Dispose();
                    conn = null;
                    MessageBox.Show("Полученный результат:\n" + outputstr);
                }
                catch
                {
                    conn.Close();
                    conn.Dispose();
                    conn = null;
                    MessageBox.Show("При выполнении команды произошла ошибка, проверьте правильность введенных данных!");
                }
            }
        }

        private void ExecCheckE_Click(object sender, EventArgs e)
        {
            SqlConnection conn = DBUtils.GetDBConnection();
            bool a = Email.Text == "";
            if (a)
                MessageBox.Show("Заполните поле для ввода данных!");
            else
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    string outputstr = "";
                    string sql1 = "SELECT dbo.CheckE(@email) AS 'CheckE'";
                    SqlParameter emailParam = new SqlParameter("@email", SqlDbType.NVarChar);
                    emailParam.Value = Email.Text;
                    cmd.Parameters.Add(emailParam);
                    cmd.CommandText = sql1;
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int CheckEind = reader.GetOrdinal("CheckE");
                                bool CheckE = reader.GetBoolean(CheckEind);
                                if (!CheckE)
                                    outputstr += "False";
                                else
                                    outputstr += "True";
                            }
                        }
                    }
                    conn.Close();
                    conn.Dispose();
                    conn = null;
                    MessageBox.Show("Полученный результат:\n" + outputstr);
                }
                catch
                {
                    conn.Close();
                    conn.Dispose();
                    conn = null;
                    MessageBox.Show("При выполнении команды произошла ошибка, проверьте правильность введенных данных!");
                }
            }
        }

        private void ManagerWindowClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        Point lastPoint;
        private void ManagerWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void ManagerWindow_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void tabPage6_Enter(object sender, EventArgs e)
        {
            if (CurrentUser.CurrentUserStatus < 2)
                managerTableListDataGridView.Visible = false;
            else
                StatusOneUserWarning.Visible = false;
        }
    }
    }


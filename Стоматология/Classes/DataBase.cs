using Guna.UI2.WinForms;
using Org.BouncyCastle.Asn1.Cms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Forms;
using System.Xml.Linq;
using static Guna.UI2.WinForms.Helpers.GraphicsHelper;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace Стоматология
{
    internal class DataBase
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=DESKTOP-LCGSVS9; Initial Catalog = СП4; Integrated Security = True");

        public void openConnection()
        {if (sqlConnection.State == System.Data.ConnectionState.Closed){  sqlConnection.Open();}}
        public void closeConnection()
        { if (sqlConnection.State == System.Data.ConnectionState.Open){ sqlConnection.Close();  } }
        public SqlConnection GetConnection()
        { return sqlConnection; }
        internal static SqlConnection getConnection()
        {throw new NotImplementedException(); }
//---------------------------------------- Работа с данными
        public string getConnectionString()
        {
            return @"Data Source=DESKTOP-LCGSVS9; Initial Catalog = СП4; Integrated Security = True";
        }
        public DataTable getData(string query)
        {
            openConnection();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(query, sqlConnection);
            DataTable dataTable = new DataTable();
            dataAdapter.Fill(dataTable);
            closeConnection();
            return dataTable;
        }
        public void queryExecute(string query)
        {
            try
            {
                openConnection();
                SqlCommand command = new SqlCommand(query, sqlConnection);
                command.ExecuteNonQuery();
                closeConnection();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Произошла ошибка: " + ex.Message);
            }
        }
        public int queryExecuteScalar(string query) // Считает все строки в таблице
        {
            string connectionString = getConnectionString();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                object result = cmd.ExecuteScalar();
                if (result != null)
                { return Convert.ToInt32(result); }
                else { return 0; }
            }
        }
        public bool IsFileLocked(string filePath) // Считает на открытин файла
        {
            try
            {
                using (FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                { fileStream.Close(); }
            }
            catch (IOException ex)
            {var errorCode = Marshal.GetHRForException(ex) & ((1 << 16) - 1); return errorCode == 32 || errorCode == 33; // 32: файл заблокирован другим процессом, 33: файл заблокирован другим процессом (для чтения)
            }
            return false;
        }
 //----------------------------------------------------------Работа с Пользователями
        public bool CheckUser(string username)
        {
            string query = $"SELECT COUNT(*) FROM Пользователи WHERE Логин = '{username}'";
            openConnection();
            SqlCommand command = new SqlCommand(query, sqlConnection);
            int count = (int)command.ExecuteScalar();
            closeConnection();
            return count > 0;
        }
//------------------------------------------------------------------Работа с Сотрудниками
        public bool CheckEmployee(string surname, string name, string patronymic)
        {
            string query = $"SELECT COUNT(*) FROM Сотрудники WHERE Фамилия = '{surname}' AND Имя = '{name}' AND Отчество = '{patronymic}'";
            openConnection();
            SqlCommand command = new SqlCommand(query, sqlConnection);
            int count = (int)command.ExecuteScalar();
            closeConnection();
            return count > 0;
        }
        public bool CheckEmployee2(int loginsot)
        {
            string query = "SELECT COUNT(*) FROM Сотрудники WHERE ID_Пользователя = @loginsot";
            openConnection();
            SqlCommand command = new SqlCommand(query, sqlConnection);
            command.Parameters.AddWithValue("@loginsot", loginsot);
            int count = (int)command.ExecuteScalar();
            closeConnection();
            return count > 0;
        }
  //------------------------------------------------------------------Работа с Пациенты
        public bool CheckPatient(string surname, string name, string patronymic)
        {
            string query = $"SELECT COUNT(*) FROM Пациенты WHERE Фамилия = '{surname}' AND Имя = '{name}' AND Отчество = '{patronymic}'";
            openConnection();
            SqlCommand command = new SqlCommand(query, sqlConnection);
            int count = (int)command.ExecuteScalar();
            closeConnection();
            return count > 0;
        }
  //------------------------------------------------------------------Работа с Записями
        public bool CheckRecords(string sotrud, string date, string time)
        {
            date = DateTime.Parse(date).ToString("yyyy-MM-dd"); time = DateTime.Parse(time).ToString("HH:mm");
            string query = "SELECT COUNT(*) FROM Заявки WHERE ID_Сотрудника = @sotrud AND Дата_приема = @date AND Время_приема = @time";
            try
            {
                openConnection();
                SqlCommand command = new SqlCommand(query, sqlConnection);
                command.Parameters.AddWithValue("@sotrud", sotrud);
                command.Parameters.AddWithValue("@date", date);
                command.Parameters.AddWithValue("@time", time);
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
            catch (Exception ex)
            { throw; }finally  {  closeConnection();}
        }
        //------------------------------------------------------------------Работа с Договоры
        public bool CheckJob(string disease, string services, string patient, string sotrud, string date)
        {
            date = DateTime.Parse(date).ToString("dd-MM-yyyy");
            string query = "SELECT COUNT(*) FROM Договоры WHERE ID_Услуги = @disease AND ID_Заболевания = @services AND ID_Клиента = @patient AND ID_Сотрудника = @sotrud AND Дата_составления = @date";
            try
            {
                openConnection();
                SqlCommand command = new SqlCommand(query, sqlConnection);
                command.Parameters.AddWithValue("@disease", disease);
                command.Parameters.AddWithValue("@services", services);
                command.Parameters.AddWithValue("@patient", patient);
                command.Parameters.AddWithValue("@sotrud", sotrud);
                command.Parameters.AddWithValue("@date", date);

                // Используйте ExecuteScalar для получения значения COUNT(*)
                int count = Convert.ToInt32(command.ExecuteScalar());

                return count > 0;
            }
            catch (Exception ex)
            { throw;  } finally {closeConnection();}
        }
    }
}

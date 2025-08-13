using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Media;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Стоматология.Classes
{
    public class PersonalArea
    {
        public static int IdUser { get; private set; }
        public static string LastName { get; private set; } 
        public static string FirstName { get; private set; }
        public static string FatherName { get; private set; }
        public static string NumberTel { get; private set; } 
        public static string Dol { get; private set; }
        public static string Category { get; private set; }
        public static string Ex { get; private set; }
        public static string Status { get; private set; }
        public static string Password { get; private set; }
        public static string Login { get; private set; }
        public byte[] Foto { get; private set; }

        public bool SetPersonalData(string login, string password)
        {
            var db = new DataBase();

            string sqlExpression = " select top 1 * from Сотрудники с  " +
                                       " INNER JOIN Пользователи п ON с.[ID_Пользователя] = п.[ID_Пользователя] " +
                                       " WHERE п.Логин = @Login and п.Пароль = @Password ";

            using (SqlConnection connection = new SqlConnection(db.getConnectionString()))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(sqlExpression, connection))
                {
                    command.Parameters.AddWithValue("@Login", login);
                    command.Parameters.AddWithValue("@Password", password);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();

                            IdUser = (int)reader["ID_Пользователя"];
                            Dol = reader["Должность"].ToString();
                            Category = reader["Категория"].ToString();
                            Ex = reader["Стаж"].ToString();
                            Status = reader["Статус"].ToString();
                            Login = reader["Логин"].ToString();                      
                            Password = reader["Пароль"].ToString();
                            NumberTel = reader["Номер_телефона"].ToString();
                            FirstName = reader["Фамилия"].ToString();
                            LastName = reader["Имя"].ToString();
                            FatherName = reader["Отчество"].ToString();
                            Foto = (byte[])reader["Фото"];
                            return true;
                        }
                    }
                    return false;
                }
            }
        }   
        public static string hashPassword(string password) // Метод хэширова пароля
        {
            MD5 md5 = MD5.Create();
            byte[] b = Encoding.ASCII.GetBytes(password);
            byte[] hash = md5.ComputeHash(b);

            StringBuilder sb = new StringBuilder();
            foreach (var a in hash)
            sb.Append(a.ToString("X2"));

            return Convert.ToString(sb);
        }
    }
}

using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Стоматология.Classes;
using Стоматология.MessageBoxForms;

namespace Стоматология.Forms
{
    public partial class MenuAutchForm : Form
    {
        DataBase db = new DataBase(); // Подключение к базе данных СП4
        private bool mouseDown; // Создание переменных для перемещения формы
        private Point lastLocation;
        public void Alert(string msg, string type, bool liftText)
        {FormAlert frm = new FormAlert();frm.showAlert(msg, type, liftText); }// Создание переменных для сообщения
        public MenuAutchForm()
        {
            InitializeComponent();
            Change.ChangeButtonColorToRed(exit2); Change.ChangeButtonColorToRed(exit); Change.ChangeButtonColorToTransparent(iconButton2); Change.ChangeButtonColorToTransparent(hide2); Change.ChangeButtonColorToTransparent(hide);
        }

        private void MenuAutchForm_Load(object sender, EventArgs e)
        { guna2Panel1.Visible= false; PasswordTextBox.UseSystemPasswordChar = true; }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = DialogForm.Show("Подтверждение выхода", "Вы уверены, что хотите выйти из приложения?",false);
            if (dialogResult == DialogResult.Yes)
            { System.Windows.Forms.Application.Exit(); }
            else
            { return; }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {guna2Panel1.Visible = true; exit.Visible = false; hide.Visible = false; info.Visible = false; this.Text = "Стоматология (Авторизация)";}
        private void guna2CirclePictureBox3_Click(object sender, EventArgs e)
        { guna2Panel1.Visible = false; info.Visible = true; exit.Visible = true; hide.Visible = true; this.Text = "Стоматология (Меню)";}
        private void iconButton8_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = DialogForm.Show("Подтверждение выхода", "Вы уверены, что хотите закрыть программу?", false);
            if (dialogResult == DialogResult.Yes)
            { System.Windows.Forms.Application.Exit(); }
            else
            { return; }
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = DialogForm.Show("Подтверждение выхода", "Вы уверены, что хотите закрыть программу?", false);
            if (dialogResult == DialogResult.Yes)
            { System.Windows.Forms.Application.Exit(); }
            else
            { return; }
        }

        private void guna2CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CheckBox1.Checked)
            { PasswordTextBox.UseSystemPasswordChar = false; }
            else
            { PasswordTextBox.UseSystemPasswordChar = true; }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            if (loginTextBox.Text == "" || loginTextBox.Text == null)
            { this.Alert("Ошибка авторизации", "Введите логин!",false);return;}
            else if (PasswordTextBox.Text == "" || PasswordTextBox.Text == null)
            { this.Alert("Ошибка авторизации", "Введите пароль!", false); return; }         

          var login = loginTextBox.Text; // Логин пользоваетеля
          var password = PersonalArea.hashPassword(PasswordTextBox.Text);  // Пароль пользоваетеля
          var PA = new PersonalArea();

            if (PA.SetPersonalData(login, password))
            {
                db.openConnection();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable table = new DataTable();
                string query = $"Select ID_Пользователя, Логин, Пароль From Пользователи where Логин = '{login}' and Пароль = '{password}'";
                SqlCommand command = new SqlCommand(query, db.GetConnection());
                adapter.SelectCommand = command;
                adapter.Fill(table);

                if (table.Rows.Count == 1)
                {
                    string querystring = $"Select Роль From Пользователи WHERE Логин='{login}'";
                    SqlCommand comm = new SqlCommand(querystring, db.GetConnection());
                    string result = comm.ExecuteScalar()?.ToString(); // Получаем результат как строку

                    if (result == "Администратор")
                    {
                        SqlCommand commandd = new SqlCommand($"SELECT Сотрудники.Фамилия, Сотрудники.Имя, Сотрудники.Отчество FROM Сотрудники INNER JOIN Пользователи ON Сотрудники.ID_Пользователя = Пользователи.ID_Пользователя WHERE Пользователи.Логин = '{login}'", db.GetConnection());
                        SqlDataReader reader = commandd.ExecuteReader();
                        if (reader.Read())
                        {
                            string Фамилия = reader["Фамилия"].ToString(); string Имя = reader["Имя"].ToString(); string Отчество = reader["Отчество"].ToString();
                            this.Alert("Авторизация в аккаунт","Здравствуйте, " + Фамилия + " " + Имя + " " + Отчество + " !" + "\nВы успешно авторизовались под статусом администратора.", true);
                        }
                        this.Hide(); Form admin = new AdminForm(); admin.Show(); reader.Close(); db.closeConnection();
                    }
                    else if (result == "Регистратор")
                    {
                        this.Hide(); db.openConnection();
                        SqlCommand commandd = new SqlCommand($"SELECT Сотрудники.Фамилия, Сотрудники.Имя, Сотрудники.Отчество FROM Сотрудники INNER JOIN Пользователи ON Сотрудники.ID_Пользователя = Пользователи.ID_Пользователя WHERE Пользователи.Логин = '{login}'", db.GetConnection()); SqlDataReader reader = commandd.ExecuteReader();
                        if (reader.Read())
                        {
                            string Фамилия = reader["Фамилия"].ToString(); string Имя = reader["Имя"].ToString(); string Отчество = reader["Отчество"].ToString(); 
                            this.Alert("Авторизация в аккаунт", "Здравствуйте, " + Фамилия + " " + Имя + " " + Отчество + " !" + "\nВы успешно авторизовались под статусом регистратора.", true);
                        }
                        this.Hide(); Form Registrar = new RegistrarForm(); Registrar.Show(); reader.Close(); db.closeConnection();
                    } else if(result == "Врач")
                    {

                        this.Hide(); db.openConnection();
                        SqlCommand commandd = new SqlCommand($"SELECT Сотрудники.Фамилия, Сотрудники.Имя, Сотрудники.Отчество FROM Сотрудники INNER JOIN Пользователи ON Сотрудники.ID_Пользователя = Пользователи.ID_Пользователя WHERE Пользователи.Логин = '{login}'", db.GetConnection()); SqlDataReader reader = commandd.ExecuteReader();
                        if (reader.Read())
                        {
                            string Фамилия = reader["Фамилия"].ToString();string Имя = reader["Имя"].ToString(); string Отчество = reader["Отчество"].ToString();
                            this.Alert("Авторизация в аккаунт", "Здравствуйте, " + Фамилия + " " + Имя + " " + Отчество + " !" + "\nВы успешно авторизовались под статусом врача.", true); 
                        }
                        this.Hide(); Form Doctor = new DoctorForm(); Doctor.Show(); reader.Close(); db.closeConnection();
                    }
                }
            }
            else{this.Alert("Ошибка авторизации", "Такой пользователь не был найден!\nПопробуйте проверить логин и пароль.", true); } db.closeConnection();
        }
        //-------------------------Перемещение 
        private void guna2Panel1_MouseDown(object sender, MouseEventArgs e)
        {mouseDown = true;lastLocation = e.Location;}
        private void guna2Panel1_MouseMove(object sender, MouseEventArgs e)
        {if (mouseDown){ this.Location = new Point((this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y); this.Update(); }}
        private void guna2Panel1_MouseUp(object sender, MouseEventArgs e)
        { mouseDown = false;}
        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        { }
        private void iconButton2_Click(object sender, EventArgs e)
        { loginTextBox.Text = ""; PasswordTextBox.Text = ""; }
        private void iconButton4_Click(object sender, EventArgs e)
        { this.WindowState = FormWindowState.Minimized; this.Alert("Фоновый режим", "Приложение находится в фоновом режиме!", false); }//свернуть форму
        private void iconButton3_Click(object sender, EventArgs e)
        { this.WindowState = FormWindowState.Minimized; this.Alert("Фоновый режим", "Приложение находится в фоновом режиме!", false); }//свернуть форму
        private bool isHelpWindowOpen = false; //для проверки на открытие руководства
        private void guna2CirclePictureBox2_Click(object sender, EventArgs e)
        {
            if (!isHelpWindowOpen)
            {
                string pathToHelpFile = Path.Combine(Application.StartupPath, "Руководство пользователя.chm");
                Help.ShowHelp(this, pathToHelpFile);
                isHelpWindowOpen = true;
            }
            else { this.Alert("Руководство пользователя", "Руководство пользователя уже открыто!", false); }     
        }
        private void guna2CirclePictureBox5_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<AboutTheProgram>().Any())
            { this.Alert("О программе", "Окно о программе уже открыто!", false); }
            else
            {AboutTheProgram aboutForm = new AboutTheProgram(); aboutForm.Show(); }         
        }
    }
}

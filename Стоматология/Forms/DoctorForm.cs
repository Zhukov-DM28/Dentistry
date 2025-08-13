using FontAwesome.Sharp;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Стоматология.Classes;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Word = Microsoft.Office.Interop.Word;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using System.Runtime.InteropServices;
using Org.BouncyCastle.Ocsp;
using System.Windows.Controls;
using MaskedTextBox;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;
using Стоматология.MessageBoxForms;
using Microsoft.Office.Interop.Word;
using Point = System.Drawing.Point;
using Application = System.Windows.Forms.Application;
using DataTable = System.Data.DataTable;
using System.Reflection.Emit;

namespace Стоматология.Forms
{
    public partial class DoctorForm : Form
    {
        DataBase db = new DataBase();
        Change change = new Change();
        private Point lastLocation; private bool mouseDown; // Создание переменных для перемещения формы

        public void Alert(string msg, string type, bool liftText)
        { FormAlert frm = new FormAlert(); frm.showAlert(msg, type, liftText); }// Создание переменных для сообщения
        public DoctorForm()
        {
            InitializeComponent();
        }

        private void DoctorForm_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "сП4DataSet.Договоры". При необходимости она может быть перемещена или удалена.
            this.договорыTableAdapter.Fill(this.сП4DataSet.Договоры);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "сП4DataSet.Заболевании". При необходимости она может быть перемещена или удалена.
            this.заболеванииTableAdapter.Fill(this.сП4DataSet.Заболевании);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "сП4DataSet.Пациенты". При необходимости она может быть перемещена или удалена.
            this.пациентыTableAdapter.Fill(this.сП4DataSet.Пациенты);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "сП4DataSet.Пациенты". При необходимости она может быть перемещена или удалена.
            this.пациентыTableAdapter.Fill(this.сП4DataSet.Пациенты);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "сП4DataSet.Заявки". При необходимости она может быть перемещена или удалена.
            this.заявкиTableAdapter.Fill(this.сП4DataSet.Заявки);
            DateTime currentDateTime = DateTime.Now; string dateString = currentDateTime.ToString("dd/MM/yyyy") + " г."; datelable.Text = dateString; loginlable.Text = PersonalArea.Login; StatusComboBox.Text = PersonalArea.Status;
            Change.ChangeButtonColorToTransparent(exitButton); Change.ChangeButtonColorToTransparent(settingButton); Change.ChangeButtonColorToTransparent(iconButton7); Change.ChangeButtonColorToRed(iconButton8); Change.ChangeButtonColorToTransparent(iconButton9); Change.ChangeButtonColorToTransparent(iconButton10); Change.ChangeButtonColorToTransparent(upButton); Change.ChangeButtonColorToTransparent(downButton);
            NumOfRecords(); UpdateStatusVisibility();
        }
        public void NumOfRecords()
        {
            db.openConnection();
            SqlCommand sqlCommand = new SqlCommand("SELECT Заявки.ID_Заявки, Пациенты.Фамилия + ' ' + Пациенты.Имя + ' ' + Пациенты.Отчество AS Пациент, " + "Сотрудники.Фамилия + ' ' + Сотрудники.Имя + ' ' + Сотрудники.Отчество AS Врач, " + "Заявки.Номер_кабинета, Заявки.Адрес_стоматологии, Заявки.Дата_приема, Заявки.Время_приема, Заявки.Повод_обращения FROM Заявки " + "INNER JOIN Пациенты ON Заявки.ID_Клиента = Пациенты.ID_Клиента INNER JOIN Сотрудники ON Заявки.ID_Сотрудника = Сотрудники.ID_Сотрудника  INNER JOIN Пользователи ON Сотрудники.ID_Пользователя = Пользователи.ID_Пользователя WHERE Пользователи.ID_Пользователя = @UserId", db.GetConnection());
            sqlCommand.Parameters.AddWithValue("@UserId", currentlog);          
            using (SqlDataReader reader = sqlCommand.ExecuteReader())
            {
                int count = 0;
                bool hasRecords = false;

                while (reader.Read())
                {if (!reader.IsDBNull(0)){ count++;  hasRecords = true; } }// Проверяем наличие ID_сотрудника в записи
                if (hasRecords)
                { this.Alert("Записи на прием", $"Общее количество записей пациентов у вас: {count} чел.", false); }
                else
                {this.Alert("Записи на прием", "У вас нет записей на прием!", false); }// Выводим количество записей в сообщении - проверить на ошибки
            }
        }
        private void iconButton5_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = DialogForm.Show("Подтверждение выхода", "Вы уверены, что хотите выйти из аккаунта?\nНесохранённые данные будут потеряны!", true);
            if (dialogResult == DialogResult.Yes) { this.Hide(); Form exit = new MenuAutchForm(); exit.Show(); } else { return; }
        }
        private void iconButton8_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = DialogForm.Show("Подтверждение выхода", "Вы уверены, что хотите закрыть программу?\nНесохранённые данные будут потеряны!", true);
            if (dialogResult == DialogResult.Yes)  { Application.Exit(); } else { return; }
        }
        private void iconButton7_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized) { this.WindowState = FormWindowState.Normal; this.StartPosition = FormStartPosition.CenterScreen; }
            else { this.MaximumSize = Screen.PrimaryScreen.WorkingArea.Size; this.WindowState = FormWindowState.Maximized; }
            iconButton7.Visible = false;
        }
        private void iconButton9_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized) { this.WindowState = FormWindowState.Normal; this.StartPosition = FormStartPosition.CenterScreen; }
            else { this.MaximumSize = Screen.PrimaryScreen.WorkingArea.Size; this.WindowState = FormWindowState.Maximized; }
            iconButton7.Visible = true;
        }
        private void iconButton10_Click(object sender, EventArgs e)
        { this.WindowState = FormWindowState.Minimized; this.Alert("Фоновый режим", "Приложение находится в фоновом режиме!", false); }//свернуть форму
        private void guna2Panel4_DoubleClick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized) { this.WindowState = FormWindowState.Normal; this.StartPosition = FormStartPosition.CenterScreen; iconButton7.Visible = false; }
            else { this.MaximumSize = Screen.PrimaryScreen.WorkingArea.Size; this.WindowState = FormWindowState.Maximized; iconButton7.Visible = true; }
        }
        private void guna2Panel4_MouseDown(object sender, MouseEventArgs e)
        { mouseDown = true; lastLocation = e.Location; }
        private void guna2Panel4_MouseMove(object sender, MouseEventArgs e)
        { if (mouseDown) { this.Location = new Point((this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y); this.Update(); } }
        private void guna2Panel4_MouseUp(object sender, MouseEventArgs e)
        { mouseDown = false; }
        private void iconButton3_Click(object sender, EventArgs e)
        {
            menuPanel.Visible = true; applicationsPanel.Visible = false; klientPanel.Visible = false; diseasePanel.Visible = false; dolPanel.Visible = false; AboutMePanel.Visible = false;
            IconButton activeButton = (IconButton)sender;// установка цвета кнопки
            change.SetButtonColors(activeButton, change.ActiveBackGroundColor, change.ActiveForeGroundColor);
            leftpanel1.Visible = true;

            change.SetButtonColors(iconButton1, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton11, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton6, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton2, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(settingButton, change.DefBackGroundColor, change.DefForeGroundColor); // Сброс цвета кнопок
            leftpanel2.Visible = false; leftpanel3.Visible = false; leftpanel4.Visible = false; leftpanel5.Visible = false;
        }
        private void iconButton1_Click(object sender, EventArgs e)
        {
            applicationsPanel.Visible = true; klientPanel.Visible = false; diseasePanel.Visible = false; dolPanel.Visible = false; AboutMePanel.Visible = false; menuPanel.Visible = false;
            IconButton activeButton = (IconButton)sender;// установка цвета кнопки
            change.SetButtonColors(activeButton, change.ActiveBackGroundColor, change.ActiveForeGroundColor);
            leftpanel2.Visible = true;

            change.SetButtonColors(iconButton3, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton11, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton6, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton2, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(settingButton, change.DefBackGroundColor, change.DefForeGroundColor);// Сброс цвета кнопок
            leftpanel1.Visible = false; leftpanel3.Visible = false; leftpanel4.Visible = false; leftpanel5.Visible = false; TabletAppl();
        }
        private void iconButton11_Click(object sender, EventArgs e)
        {
            klientPanel.Visible = true; applicationsPanel.Visible = false; diseasePanel.Visible = false; dolPanel.Visible = false; AboutMePanel.Visible = false; menuPanel.Visible = false;
            IconButton activeButton = (IconButton)sender;// установка цвета кнопки
            change.SetButtonColors(activeButton, change.ActiveBackGroundColor, change.ActiveForeGroundColor);
            leftpanel3.Visible = true;

            change.SetButtonColors(iconButton3, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton1, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton6, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton2, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(settingButton, change.DefBackGroundColor, change.DefForeGroundColor);// Сброс цвета кнопок
            leftpanel1.Visible = false; leftpanel2.Visible = false; leftpanel4.Visible = false; leftpanel5.Visible = false;
        }
        private void iconButton6_Click(object sender, EventArgs e)
        {
            dolPanel.Visible = true; applicationsPanel.Visible = false; klientPanel.Visible = false; diseasePanel.Visible = false; AboutMePanel.Visible = false; menuPanel.Visible = false;

            IconButton activeButton = (IconButton)sender;// установка цвета кнопки
            change.SetButtonColors(activeButton, change.ActiveBackGroundColor, change.ActiveForeGroundColor);
            leftpanel4.Visible = true;

            change.SetButtonColors(iconButton3, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton1, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton11, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton2, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(settingButton, change.DefBackGroundColor, change.DefForeGroundColor);// Сброс цвета кнопок
            leftpanel1.Visible = false; leftpanel2.Visible = false; leftpanel3.Visible = false; leftpanel5.Visible = false; TabletСontracts();
        }
        private void iconButton2_Click(object sender, EventArgs e)
        {
            diseasePanel.Visible = true; applicationsPanel.Visible = false; klientPanel.Visible = false; dolPanel.Visible = false; AboutMePanel.Visible = false; menuPanel.Visible = false;
            IconButton activeButton = (IconButton)sender;// установка цвета кнопки
            change.SetButtonColors(activeButton, change.ActiveBackGroundColor, change.ActiveForeGroundColor);
            leftpanel5.Visible = true;

            change.SetButtonColors(iconButton3, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton1, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton11, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton6, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(settingButton, change.DefBackGroundColor, change.DefForeGroundColor);// Сброс цвета кнопок
            leftpanel1.Visible = false; leftpanel2.Visible = false; leftpanel3.Visible = false; leftpanel4.Visible = false;
        }     
        //------------------------- Таблица Записи
        private void applicationsPanel_Paint(object sender, PaintEventArgs e)
        {   Change.ChangeButtonColorToTransparent(PDFFileiconButton); Change.ChangeButtonColorToTransparent(iconButton26); Change.ChangeButtonColorToTransparent(iconButton25); Change.ChangeButtonColorToTransparent(iconButton24); }
        string currentlog = PersonalArea.IdUser.ToString();
        public void TabletAppl(string filterText = null)
        {
            DataBase db = new DataBase();
            db.openConnection();
            try
            {
                заявкиDataGridView.DataSource = null; заявкиDataGridView.Rows.Clear(); заявкиDataGridView.Columns.Clear();
                заявкиDataGridView.Columns.Add("ID_Заявки", "Номер"); заявкиDataGridView.Columns.Add("Пациент", " Пациент"); заявкиDataGridView.Columns.Add("Врач", "Врач");
                заявкиDataGridView.Columns.Add("Номер_кабинета", "№ каб."); заявкиDataGridView.Columns.Add("Адрес_стоматологии", "Адрес"); заявкиDataGridView.Columns.Add("Дата_приема", "Дата");
                заявкиDataGridView.Columns.Add("Время_приема", "Время"); заявкиDataGridView.Columns.Add("Повод_обращения", "Повод обращения");
                // Выполняем запрос к базе данных
                SqlCommand sqlCommand = new SqlCommand("SELECT Заявки.ID_Заявки, Пациенты.Фамилия + ' ' + Пациенты.Имя + ' ' + Пациенты.Отчество AS Пациент, " +
                    "Сотрудники.Фамилия + ' ' + Сотрудники.Имя + ' ' + Сотрудники.Отчество AS Врач, " +
                    "Заявки.Номер_кабинета, Заявки.Адрес_стоматологии, Заявки.Дата_приема, Заявки.Время_приема, Заявки.Повод_обращения FROM Заявки " +
                    "INNER JOIN Пациенты ON Заявки.ID_Клиента = Пациенты.ID_Клиента INNER JOIN Сотрудники ON Заявки.ID_Сотрудника = Сотрудники.ID_Сотрудника INNER JOIN Пользователи ON Сотрудники.ID_Пользователя = Пользователи.ID_Пользователя WHERE Пользователи.ID_Пользователя = @UserId", db.GetConnection());
                sqlCommand.Parameters.AddWithValue("@UserId", currentlog);

                if (!string.IsNullOrEmpty(filterText))
                { string[] searchTerms = filterText.Trim().Split(' '); string filterExpression = "";
                    for (int i = 0; i < searchTerms.Length; i++)
                    {
                        string cleanedSearchTerm = searchTerms[i].Replace("*", "").Replace("`", "");
                        filterExpression += $"(Пациенты.Фамилия LIKE '%{cleanedSearchTerm}%' OR Пациенты.Имя LIKE '%{cleanedSearchTerm}%' OR Пациенты.Отчество LIKE '%{cleanedSearchTerm}%')";
                        if (i < searchTerms.Length - 1)
                        { filterExpression += " AND "; }
                       заявкиDataGridView.Columns["Номер_кабинета"].Width = 60; заявкиDataGridView.Columns["Время_приема"].Width = 50; заявкиDataGridView.Columns["Дата_приема"].Width = 100;
                    }
                    if (!string.IsNullOrEmpty(filterExpression))
                    { filterExpression = " AND " + filterExpression; } sqlCommand.CommandText += filterExpression; }
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                List<string[]> data = new List<string[]>();
                while (sqlDataReader.Read())
                {
                    data.Add(new string[8]);
                    for (int i = 0; i < 8; i++)
                    {
                        if (i == 5)
                        {
                            // Преобразование времени в формат только даты
                            data[data.Count - 1][i] = ((DateTime)sqlDataReader[i]).ToString("dd-MM-yyyy г.");                         
                        }
                        else
                        { data[data.Count - 1][i] = sqlDataReader[i].ToString(); }
                    }                  
                }
                заявкиDataGridView.Columns["Номер_кабинета"].Width = 60; заявкиDataGridView.Columns["Время_приема"].Width = 50; заявкиDataGridView.Columns["Дата_приема"].Width = 100;
                sqlDataReader.Close();
                foreach (string[] s in data) заявкиDataGridView.Rows.Add(s);
                data.Clear();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message);
            }
            finally { db.closeConnection(); }
        }
        private void guna2CustomGradientPanel1_Paint(object sender, PaintEventArgs e)
        { }
        private void PDFFileiconButton_Click(object sender, EventArgs e)
        {
            if (заявкиDataGridView.CurrentRow != null)
            {
                DataGridViewRow selectedRow = заявкиDataGridView.CurrentRow;
                int Номер_Талона = Convert.ToInt32(selectedRow.Cells["ID_Заявки"].Value);
                string Пациент = selectedRow.Cells["Пациент"].Value?.ToString();
                string Врач = selectedRow.Cells["Врач"].Value?.ToString();
                string Дата = selectedRow.Cells["Дата_приема"].Value.ToString();
                DateTime date = Convert.ToDateTime(Дата);
               string formattedDate = date.ToString("dd.MM.yyyy") + " г.";
                string Время = selectedRow.Cells["Время_приема"].Value?.ToString();
                string Кабинет = selectedRow.Cells["Номер_кабинета"].Value?.ToString();
                string Повод_обращение = selectedRow.Cells["Повод_обращения"].Value?.ToString();
                string Адрес = selectedRow.Cells["Адрес_стоматологии"].Value?.ToString();
                string outputPath = Path.Combine(Environment.CurrentDirectory, "Талоны", $"Талон на приём к врачу № {Номер_Талона}.docx");
                if (db.IsFileLocked(outputPath))
                { this.Alert("Создание талона ", $"Талон № {Номер_Талона} уже открыт!\nЗакройте его, прежде чем создавать новый талон.", true); }
                else
                {
                    Word._Application oWord = new Word.Application();
                    oWord.Visible = true;
                    Word._Document oDoc = oWord.Documents.Open(Path.Combine(Environment.CurrentDirectory, "Талон на приём к врачу.docx"));
                    oDoc.Bookmarks["id"].Range.Text = Convert.ToString(Номер_Талона);
                    oDoc.Bookmarks["naim"].Range.Text = Convert.ToString(Пациент);
                    oDoc.Bookmarks["sot"].Range.Text = Convert.ToString(Врач);
                    oDoc.Bookmarks["date1"].Range.Text = formattedDate;
                    oDoc.Bookmarks["date2"].Range.Text = Convert.ToString(Время);
                    oDoc.Bookmarks["nom"].Range.Text = Convert.ToString(Кабинет);
                    oDoc.Bookmarks["usl"].Range.Text = Convert.ToString(Повод_обращение);
                    oDoc.Bookmarks["adres"].Range.Text = Convert.ToString(Адрес);
                    oDoc.SaveAs(FileName: outputPath);
                    oDoc.Close();
                    oWord.Quit();
                    this.Alert("Создание талона", $"Талон № {Номер_Талона} на приём к врачу, был создан успешно!", false);
                }
            }
            else
            { this.Alert("Ошибка при создании талона", "Не удалось создать талон на прием к врачу.\nЗапись не найдена!", true); return; }
        }
        private void iconButton26_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = DialogForm.Show("Подтверждение завершения приема", "Вы уверены, что хотите завершить приём?", false);
            if (dialogResult == DialogResult.Yes)              
            {
                List<int> idsToDelete = new List<int>();
                foreach (DataGridViewRow row in заявкиDataGridView.SelectedRows)
                {
                    int id = Convert.ToInt32(row.Cells[0].Value);
                    idsToDelete.Add(id);
                }
                if (idsToDelete.Count > 0)
                {                   
                    string ids = string.Join(",", idsToDelete);
                    string connectionString = db.getConnectionString();
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        SqlCommand delete = new SqlCommand($"DELETE FROM Заявки WHERE ID_Заявки IN ({ids})");
                        delete.Connection = conn;
                        delete.ExecuteNonQuery();                       
                        List<int> allEmployeeIds = new List<int>();// Проверяем, есть ли удаленные сотрудники в поле "Врачи"  
                        string getAllEmployeeIdsQuery = "SELECT ID_Сотрудника FROM Сотрудники";
                        DataTable employeeIdsData = db.getData(getAllEmployeeIdsQuery);
                        foreach (DataRow row in employeeIdsData.Rows)
                        { int employeeId = Convert.ToInt32(row["ID_Сотрудника"]); allEmployeeIds.Add(employeeId); }
                        foreach (int employeeId in allEmployeeIds)
                        {
                            string checkDoctorQuery = $"SELECT COUNT(*) FROM Заявки WHERE ID_Сотрудника = @employeeId";
                            SqlCommand checkCommand = new SqlCommand(checkDoctorQuery, conn);
                            checkCommand.Parameters.AddWithValue("@employeeId", employeeId);
                            int doctorCount = (int)checkCommand.ExecuteScalar();
                            string currentStatusQuery = $"SELECT Статус FROM Сотрудники WHERE ID_Сотрудника = @employeeId";
                            SqlCommand statusCommand = new SqlCommand(currentStatusQuery, conn);
                            statusCommand.Parameters.AddWithValue("@employeeId", employeeId);
                            string currentStatus = statusCommand.ExecuteScalar().ToString();
                            if (doctorCount == 0 && currentStatus != "Свободен")
                            {
                                string newStatus = "Свободен";
                                string updateStatusQuery = $"UPDATE Сотрудники SET Статус = @newStatus WHERE ID_Сотрудника = @employeeId";
                                SqlCommand updateCommand = new SqlCommand(updateStatusQuery, conn);
                                updateCommand.Parameters.AddWithValue("@newStatus", newStatus);
                                updateCommand.Parameters.AddWithValue("@employeeId", employeeId);
                                updateCommand.ExecuteNonQuery();
                                StatusComboBox.Text = newStatus;
                                UpdateStatusVisibility();
                            }
                        }
                    }
                    this.Alert("Завершение приема", "Выбранная запись к врачу была успешно завершена!", false);
                    string query = "SELECT * FROM Заявки";
                    DataTable dt = db.getData(query);
                    заявкиDataGridView.DataSource = dt; TabletAppl();
                }
                else { this.Alert("Ошибка при завершении приема", "Не удалось завершить прием. Запись не найдена!", false); return; }
            }
            else { return; }          
        }
        private void filtrguna2TextBox_TextChanged(object sender, EventArgs e)
        { TabletAppl(filtrguna2TextBox.Text); }
        private void iconButton24_Click(object sender, EventArgs e)
        { filtrguna2TextBox.Text = ""; }

        private void iconButton25_Click(object sender, EventArgs e)
        {         
            if (заявкиDataGridView.CurrentRow != null)
            {
                updateCustomGradientPanel.Visible = true; guna2CustomGradientPanel1.Visible = false; guna2CustomGradientPanel2.Visible = false;
                DataGridViewRow selectedRow = заявкиDataGridView.CurrentRow; nlabel.Text = selectedRow.Cells["ID_Заявки"].Value?.ToString() ?? "";
                numberguna2TextBox2.Text = selectedRow.Cells["Номер_кабинета"].Value?.ToString() ?? ""; adresguna2ComboBox2.Text = selectedRow.Cells["Адрес_стоматологии"].Value?.ToString() ?? "";
                fiopasguna2textBox2.Text = selectedRow.Cells["Пациент"].Value?.ToString() ?? ""; fiosotrudguna2TextBox2.Text = selectedRow.Cells["Врач"].Value?.ToString() ?? ""; dateguna2DateTimePicker.Text = selectedRow.Cells["Дата_приема"].Value?.ToString() ?? "";
                timeguna2ComboBox2.Text = selectedRow.Cells["Время_приема"].Value?.ToString() ?? ""; povodtextBox.Text = selectedRow.Cells["Повод_обращения"].Value?.ToString() ?? "";
            }
            else
            { this.Alert("Ошибка при редактировании", "Не удалось обновить данные. Запись не найдена!", false); return; }
        }

        private void iconButton28_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = DialogForm.Show("Подтверждение выхода при редактировании", "Вы уверены, что хотите отменить редактирование\nзаписи на приём? Несохранённые данные будут потеряны!", true);
            if (dialogResult == DialogResult.Yes) { updateCustomGradientPanel.Visible = false; guna2CustomGradientPanel2.Visible = true; guna2CustomGradientPanel1.Visible = true; }
            else { return; }
        }
        private int selectedEmployeeId; // объявление переменной     
        private void заявкиDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var cell = заявкиDataGridView.Rows[e.RowIndex].Cells["ID_Заявки"];
                if (cell != null && cell.Value != null && int.TryParse(cell.Value.ToString(), out int id))
                { selectedEmployeeId = id; }
            }
        }
        private void guna2Button5_Click_1(object sender, EventArgs e)
        {
            DialogResult dialogResult = DialogForm.Show("Редактирование в таблице", "Вы уверены, что хотите изменить запись?", false);
            if (dialogResult == DialogResult.Yes)              
            {
                if (string.IsNullOrWhiteSpace(fiopasguna2textBox2.Text) || string.IsNullOrWhiteSpace(fiosotrudguna2TextBox2.Text) || string.IsNullOrWhiteSpace(dateguna2DateTimePicker.Text) || string.IsNullOrWhiteSpace(povodtextBox.Text) || string.IsNullOrWhiteSpace(numberguna2TextBox2.Text) || string.IsNullOrWhiteSpace(timeguna2ComboBox2.Text) || string.IsNullOrWhiteSpace(adresguna2ComboBox2.Text))
                { this.Alert("Ошибка при редактировании", "Пожалуйста, заполните все поля! ", false); }
                else
                {
                    try
                    {
                        this.Validate();
                        string kb = numberguna2TextBox2.Text; string adres = adresguna2ComboBox2.Text; DateTime date = dateguna2DateTimePicker.Value; string timeStringg = timeguna2ComboBox2.Text; string povod = povodtextBox.Text;
                        using (var connection = new SqlConnection(db.getConnectionString()))
                        {
                            connection.Open();                         
                            string query = "UPDATE Заявки SET Номер_кабинета = @kb, Адрес_стоматологии = @adres ,Дата_приема = @date , Время_приема = @time, Повод_обращения = @povod WHERE ID_Заявки = @id";
                            using (SqlCommand command = new SqlCommand(query, connection))
                                {
                                    command.Parameters.AddWithValue("@kb", kb); command.Parameters.AddWithValue("@adres", adres); command.Parameters.AddWithValue("@date", date); command.Parameters.AddWithValue("@time", timeStringg); command.Parameters.AddWithValue("@povod", povod);
                                    int id = selectedEmployeeId; command.Parameters.AddWithValue("@id", id);
                                    int rowsAffected = command.ExecuteNonQuery();
                                    if (rowsAffected > 0)
                                    {
                                        this.Alert("Редактирование в таблице", "Данные успешно изменены!", false);
                                        заявкиDataGridView.DataSource = null; заявкиBindingSource.ResetBindings(false);
                                        заявкиBindingSource.DataSource = db.getData("SELECT * FROM Заявки"); заявкиDataGridView.DataSource = заявкиBindingSource;
                                        updateCustomGradientPanel.Visible = false; guna2CustomGradientPanel2.Visible = true; guna2CustomGradientPanel1.Visible = true;
                                    }
                                    else { this.Alert("Ошибка при редактировании", "Не удалось обновить данные. Запись не найдена!", false); return; }
                                }                         
                        }
                    }
                    catch (System.Exception ex)
                    { MessageBox.Show("Ошибка при редактировании: " + ex.Message); }
                }
            }
            else { return; }
            TabletAppl();
        }
        private void povodtextBox_TextChanged(object sender, EventArgs e)
        { }
        private void povodguna2ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        { povodtextBox.Text = povodguna2ComboBox2.SelectedItem.ToString(); }
 //-------------------------------------------Таблица Пациенты
        private void updateCustomGradientPanel_Paint(object sender, PaintEventArgs e)
        { Change.ChangeButtonColorToDodgerBlue(iconButton28); }
        private void klientPanel_Paint(object sender, PaintEventArgs e)
        { Change.ChangeButtonColorToTransparent(iconButton5); Change.ChangeButtonColorToTransparent(iconButton16); Change.ChangeButtonColorToTransparent(iconButton19); Change.ChangeButtonColorToTransparent(filtericonButton2); Change.ChangeButtonColorToDodgerBlue(iconButton23); }
        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        { }
        private void iconButton12_Click(object sender, EventArgs e)
        {
            if (filterguna2Panel.Visible)
            { change.SetButtonColors(filtericonButton2, change.DefBackGroundColor, change.DefForeGroundColor); filterguna2Panel.Visible = false; vidguna2ComboBox.SelectedIndex = -1; пациентыBindingSource.Filter = ""; filtericonButton2.IconChar = IconChar.Filter; }
            else
            { change.SetButtonColors(filtericonButton2, change.ActiveBackGroundColor, change.ActiveForeGroundColor); filterguna2Panel.Visible = true; filtericonButton2.IconChar = IconChar.FilterCircleXmark; }
        }
        private void iconButton5_Click_1(object sender, EventArgs e)
        { searchguna2TextBox.Text = ""; }
        private void searchguna2TextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (пациентыBindingSource != null && searchguna2TextBox.Text != "")
                {
                    string searchTerm = searchguna2TextBox.Text.Trim(); string[] searchTerms = searchTerm.Split(' '); // Разделяем поисковый запрос на фамилию, имя и отчество
                    string filterExpression = "";  // Формируем строку фильтра с оператором AND
                    foreach (string term in searchTerms) { filterExpression += $"([Фамилия] LIKE '%{term}%' OR " + $"[Имя] LIKE '%{term}%' OR " + $"[Отчество] LIKE '%{term}%') AND "; }
                    filterExpression = filterExpression.Remove(filterExpression.Length - 5); пациентыBindingSource.Filter = filterExpression; // Удаляем последний оператор AND
                }
                else { пациентыBindingSource.Filter = ""; }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void vidguna2ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (vidguna2ComboBox.SelectedIndex)
            {
                case 0: пациентыBindingSource.Filter = $"[Пол] like 'М.'"; break;
                case 1: пациентыBindingSource.Filter = $"[Пол] like 'Ж.'"; break; ;
            }
        }
        private void iconButton19_Click(object sender, EventArgs e)
        { vidguna2ComboBox.SelectedIndex = -1; пациентыBindingSource.Filter = ""; }
        private void пациентыDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        { }
        private void iconButton23_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = DialogForm.Show("Подтверждение выхода", "Закрыть информацию о пациенте?", false);
            if (dialogResult == DialogResult.Yes) { updateCustomGradientPanel2.Visible = false; menuPanel2.Visible = true; guna2CustomGradientPanel3.Visible = true; }
            else { return; }
        }
        private void iconButton16_Click(object sender, EventArgs e)
        {          
            if (пациентыDataGridView.CurrentRow != null)
            {
                updateCustomGradientPanel2.Visible = true; menuPanel2.Visible = false; guna2CustomGradientPanel3.Visible = false;
                DataGridViewRow selectedRow = пациентыDataGridView.CurrentRow;              
                famguna2TextBox2.Text = selectedRow.Cells["фамилияDataGridViewTextBoxColumn"].Value?.ToString() ?? "";
                name2TextBox2.Text = selectedRow.Cells["имяDataGridViewTextBoxColumn"].Value?.ToString() ?? "";
                otchTextBox2.Text = selectedRow.Cells["отчествоDataGridViewTextBoxColumn"].Value?.ToString() ?? "";
                maskedTextBox4.Text = selectedRow.Cells["номертелефонаDataGridViewTextBoxColumn"].Value?.ToString() ?? "";

                DateTime dateOfBirth = (selectedRow.Cells["датарожденияDataGridViewTextBoxColumn"].Value as DateTime?) ?? DateTime.MinValue;
                DateTime currentDate = DateTime.Today;
                int age = currentDate.Year - dateOfBirth.Year; // Вычисляем возраст
                if (currentDate < dateOfBirth.AddYears(age))
                { age--; } string yearsText = "";
                if (age % 10 == 1 && age % 100 != 11)
                { yearsText = "год."; }
                else if (age % 10 >= 2 && age % 10 <= 4 && (age % 100 < 10 || age % 100 >= 20))
                { yearsText = "года."; }
                else { yearsText = "лет."; }
                adrestextbox.Text = Environment.NewLine + $"{age} {yearsText}";

                string selectedPolValue = selectedRow.Cells["полDataGridViewTextBoxColumn"].Value?.ToString() ?? "";
                if (selectedPolValue == "М.") // Если значение в таблице "М."
                { polguna2TextBox2.Text = "Мужской"; }// Устанавливаем текст комбобокса соответственно 
                else if (selectedPolValue == "Ж.") // Если значение в таблице "Ж."
                { polguna2TextBox2.Text = "Женский"; }// Устанавливаем текст комбобокса соответственно   
                maskedTextBox3.Text = selectedRow.Cells["серияиномерпаспортаDataGridViewTextBoxColumn"].Value?.ToString() ?? "";
                maskedTextBox2.Text = selectedRow.Cells["сНИЛСDataGridViewTextBoxColumn"].Value?.ToString() ?? "";
                maskedTextBox1.Text = selectedRow.Cells["медицинкийполисDataGridViewTextBoxColumn"].Value?.ToString() ?? "";
                adresguna2TextBox2.Text = selectedRow.Cells["адресDataGridViewTextBoxColumn"].Value?.ToString() ?? "";
            }
            else
            { this.Alert("Информация о пациенте", "Не удалось открыть информацию о пациенте. Запись не найдена!", false); return; }
        }
        //------------------------- Таблица Заболевании
        private void diseasePanel_Paint(object sender, PaintEventArgs e)
        { Change.ChangeButtonColorToTransparent(infoiconButton); Change.ChangeButtonColorToTransparent(cleariconButton); Change.ChangeButtonColorToDodgerBlue(exiticonButton2); }
        private void searchguna2TextBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (заболеванииBindingSource != null && searchguna2TextBox2.Text != "")
                {
                    string searchTerm = searchguna2TextBox2.Text.Trim(); string[] searchTerms = searchTerm.Split(' '); // Разделяем поисковый запрос на фамилию, имя и отчество
                    string filterExpression = "";  // Формируем строку фильтра с оператором AND
                    foreach (string term in searchTerms) { filterExpression += $"([Название] LIKE '%{term}%') AND "; }
                    filterExpression = filterExpression.Remove(filterExpression.Length - 5); заболеванииBindingSource.Filter = filterExpression; // Удаляем последний оператор AND
                }
                else { заболеванииBindingSource.Filter = ""; }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void cleariconButton_Click(object sender, EventArgs e)
        { searchguna2TextBox2.Text = ""; }
        private void infoiconButton_Click(object sender, EventArgs e)
        {
            infoguna2CustomGradientPanel4.Visible = true; guna2CustomGradientPanel6.Visible = false; diseaseguna2CustomGradientPanel5.Visible = false;
            if (заболеванииDataGridView.CurrentRow != null)
            {
                DataGridViewRow selectedRow = заболеванииDataGridView.CurrentRow;               
                infoguna2TextBox2.Text = selectedRow.Cells["названиеDataGridViewTextBoxColumn"].Value?.ToString() ?? "";
                opguna2TextBox.Text = selectedRow.Cells["описаниеDataGridViewTextBoxColumn"].Value?.ToString() ?? ""; ;
            }
            else
            { this.Alert("Информация о заболевании", "Не удалось открыть информацию о заболевании. Запись не найдена!", false); return; }
        }
        private void exiticonButton2_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = DialogForm.Show("Подтверждение выхода", "Закрыть информацию о заболевании?", false);
            if (dialogResult == DialogResult.Yes) { infoguna2CustomGradientPanel4.Visible = false; guna2CustomGradientPanel6.Visible = true; diseaseguna2CustomGradientPanel5.Visible = true; } else { return; } }
        //------------------------ Таблицы Договоры
        private void applguna2GroupBox_Click(object sender, EventArgs e)
        { Change.ChangeButtonColorToTransparent(iconButton13); Change.ChangeButtonColorToTransparent(iconButton4); }
        private void dolPanel_Paint(object sender, PaintEventArgs e)
        { if (appDateTimePicker.Value == null) { appDateTimePicker.Value = DateTime.Today; } Change.ChangeButtonColorToTransparent(cleariconButton2); Change.ChangeButtonColorToDodgerBlue(closeiconButton2); Change.ChangeButtonColorToTransparent(wordfileiconButton); Change.ChangeButtonColorToTransparent(addgogiconButton); Change.ChangeButtonColorToTransparent(deliconButton); Change.ChangeButtonColorToTransparent(updateiconButton); Change.ChangeButtonColorToTransparent(cleariconButton3); Change.ChangeButtonColorToTransparent(iconButton14); }
        private void guna2TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        { if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) { e.Handled = true; } }// отмена ввода символа, если он не является цифрой или управляющим символом
        private void guna2TextBox2_KeyPress(object sender, KeyPressEventArgs e)
        { if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) { e.Handled = true; } }// отмена ввода символа, если он не является цифрой или управляющим символом
        private void guna2TextBox3_KeyPress(object sender, KeyPressEventArgs e)
        { if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) { e.Handled = true; } }// отмена ввода символа, если он не является цифрой или управляющим символом
        private void kolTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        { if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) { e.Handled = true; } }// отмена ввода символа, если он не является цифрой или управляющим символом
        private void uslTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        { if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) { e.Handled = true; } }// отмена ввода символа, если он не является цифрой или управляющим символом
        public void TabletСontracts(string filterText = null)
        {
            DataBase db = new DataBase();
            db.openConnection();
            try
            {
                договорыDataGridView.DataSource = null; договорыDataGridView.Rows.Clear(); договорыDataGridView.Columns.Clear();
                договорыDataGridView.Columns.Add("ID_Договора", "Номер"); договорыDataGridView.Columns.Add("Услуга", " Услуга"); договорыDataGridView.Columns.Add("Заболевание", "Заболевание");
                договорыDataGridView.Columns.Add("Пациент", "Пациент"); договорыDataGridView.Columns.Add("Врач", "Врач"); договорыDataGridView.Columns.Add("Дата_составления", "Дата");
                договорыDataGridView.Columns.Add("Адрес_стоматологии", "Адрес"); договорыDataGridView.Columns.Add("Количество", "Кол-во"); договорыDataGridView.Columns.Add("Цена_услуги", "Цена");
                договорыDataGridView.Columns.Add("Итоговая_стоимость", "Стоимость"); договорыDataGridView.Columns.Add("Вид_оплаты", "Вид оплаты"); договорыDataGridView.Columns.Add("Гарантия", "Гарантия");
                SqlCommand sqlCommand = new SqlCommand("SELECT Договоры.ID_Договора, " +
                    "Услуги.Название AS Услуга, Заболевании.Название AS Заболевание, Пациенты.Фамилия + ' ' + Пациенты.Имя + ' ' + Пациенты.Отчество AS Пациент, Сотрудники.Фамилия + ' ' + Сотрудники.Имя + ' ' + Сотрудники.Отчество AS Врач," +
                    "Договоры.Дата_составления, Договоры.Адрес_стоматологии, Договоры.Количество, Договоры.Цена_услуги, Договоры.Итоговая_стоимость, Договоры.Вид_оплаты, Договоры.Гарантия FROM Договоры " +
                    "INNER JOIN Пациенты ON Договоры.ID_Клиента = Пациенты.ID_Клиента INNER JOIN Услуги ON Договоры.ID_Услуги = Услуги.ID_Услуги INNER JOIN Заболевании ON Договоры.ID_Заболевания = Заболевании.ID_Заболевания INNER JOIN Сотрудники ON Договоры.ID_Сотрудника = Сотрудники.ID_Сотрудника INNER JOIN Пользователи ON Сотрудники.ID_Пользователя = Пользователи.ID_Пользователя " +
                    "WHERE Пользователи.ID_Пользователя = @UserId", db.GetConnection());
                sqlCommand.Parameters.AddWithValue("@UserId", currentlog);
                if (!string.IsNullOrEmpty(filterText))
                {
                    string[] searchTerms = filterText.Trim().Split(' '); string filterExpression = ""; for (int i = 0; i < searchTerms.Length; i++)
                    {
                        string cleanedSearchTerm = searchTerms[i].Replace("*", "").Replace("`", "");
                        filterExpression += $"(Пациенты.Фамилия LIKE '%{cleanedSearchTerm}%' OR Сотрудники.Фамилия LIKE '%{cleanedSearchTerm}%')";
                        if (i < searchTerms.Length - 1)
                        { filterExpression += " AND "; }
                    }
                    if (!string.IsNullOrEmpty(filterExpression)) { filterExpression = " AND " + filterExpression; }
                    sqlCommand.CommandText += filterExpression;
                    договорыDataGridView.Columns["Адрес_стоматологии"].Visible = false; договорыDataGridView.Columns["Вид_оплаты"].Visible = false; договорыDataGridView.Columns["Гарантия"].Visible = false;
                }
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                List<string[]> data = new List<string[]>();
                while (sqlDataReader.Read())
                {
                    data.Add(new string[12]);
                    for (int i = 0; i < 12; i++)
                    {
                        if (i == 5)
                        {
                            // Преобразование времени в формат только даты
                            data[data.Count - 1][i] = ((DateTime)sqlDataReader[i]).ToString("dd-MM-yyyy г."); 
                        }
                        else if (i == 8 || i == 9) // Условие для стоимости и цены
                        {
                            decimal cost = (decimal)sqlDataReader.GetDecimal(i);
                            data[data.Count - 1][i] = cost.ToString("C0"); // Выводим стоимость в рублях без дробной части                                                                   
                        }
                        else
                        { data[data.Count - 1][i] = sqlDataReader[i].ToString(); }
                    }
                }
                договорыDataGridView.Columns["Адрес_стоматологии"].Visible = false; договорыDataGridView.Columns["Вид_оплаты"].Visible = false; договорыDataGridView.Columns["Гарантия"].Visible = false;
                sqlDataReader.Close();
                foreach (string[] s in data) договорыDataGridView.Rows.Add(s);
                data.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { db.closeConnection(); }
        }
        private int number = 0;
        private void iconButton4_Click(object sender, EventArgs e)
        {
            int number = 0; // Инициализация переменной number
            if (int.TryParse(kolTextBox.Text, out number))
            { number++; }
            else { number = 1; }
            kolTextBox.Text = number.ToString(); // Преобразуем число в строку и устанавливаем в textBox
        }
        private void iconButton13_Click(object sender, EventArgs e)
        {
            int number = 0; // Инициализация переменной number
            if (int.TryParse(kolTextBox.Text, out number))
            { if (number > 0) { number--; } }
            else
            { number = 1; }
            kolTextBox.Text = number.ToString(); // Преобразуем число в строку и устанавливаем в kolTextBox2
        }
        private void UpdateResult()      // Считает общию стоимость договора  
        {
            if (!string.IsNullOrEmpty(kolTextBox.Text) && !string.IsNullOrEmpty(cenaTextBox.Text))
            {
                decimal num1, num2; if (Decimal.TryParse(kolTextBox.Text, out num1) && Decimal.TryParse(cenaTextBox.Text, out num2)) { decimal result = num1 * num2; summTextBox.Text = result.ToString("F0"); }
                else { summTextBox.Text = ""; }
            }
            else { summTextBox.Text = ""; }
        }
        private void UpdateResult2() // Считает общию стоимость договора  
        {
            if (!string.IsNullOrEmpty(kolTextBox2.Text) && !string.IsNullOrEmpty(uslTextBox2.Text))
            {
                decimal num1, num2; if (Decimal.TryParse(kolTextBox2.Text, out num1) && Decimal.TryParse(uslTextBox2.Text, out num2)) { decimal result = num1 * num2; summTextBox2.Text = result.ToString("F0"); }
                else { summTextBox2.Text = ""; }
            }
            else { summTextBox2.Text = ""; }
        }
        private void guna2TextBox3_TextChanged(object sender, EventArgs e)
        { UpdateResult(); }
        private void cenaTextBox_TextChanged(object sender, EventArgs e)
        { UpdateResult(); }
        private void kolTextBox_TextChanged(object sender, EventArgs e)
        { UpdateResult(); }
        private void guna2TextBox4_TextChanged(object sender, EventArgs e)
        { TabletСontracts(poickTextBox.Text); }
        private void guna2TextBox3_TextChanged_1(object sender, EventArgs e)
        { UpdateResult2(); }
        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        { UpdateResult2(); }
        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        { UpdateResult2(); }
        private void iconButton13_Click_1(object sender, EventArgs e)
        {
            int number = 0; // Инициализация переменной number
            if (int.TryParse(kolTextBox2.Text, out number))
            {number++; }
            else { number = 1; }
            kolTextBox2.Text = number.ToString(); // Преобразуем число в строку и устанавливаем в textBox
        }
        private void iconButton4_Click_1(object sender, EventArgs e)
        {
            int number = 0; // Инициализация переменной number
            if (int.TryParse(kolTextBox2.Text, out number))
            { if (number > 0) { number--;  }}
            else
            { number = 1;  }
            kolTextBox2.Text = number.ToString(); // Преобразуем число в строку и устанавливаем в kolTextBox2
        }
        private void cleariconButton3_Click(object sender, EventArgs e)
        { poickTextBox.Text = ""; }
        public void LoadData()
        {
            DataBase db = new DataBase();
            db.openConnection();
            try
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT ID_Услуги, Название, Стоимость FROM Услуги", db.GetConnection());
                DataTable dataTable = new DataTable();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlDataAdapter.Fill(dataTable);
                uslComboBox.DisplayMember = "Название"; uslComboBox.ValueMember = "ID_Услуги"; uslComboBox.DataSource = dataTable; uslComboBox.SelectedIndex = -1; uslComboBox.SelectedIndexChanged += (sender, e) =>
                {
                    if (uslComboBox.SelectedItem != null)
                    {
                        DataRowView selectedRow = (DataRowView)uslComboBox.SelectedItem;
                        string selectedID = selectedRow["ID_Услуги"].ToString();
                        DataRow[] foundRows = dataTable.Select("ID_Услуги = '" + selectedID + "'");
                        if (foundRows.Length > 0)
                        {
                            string selectedPrice = foundRows[0]["Стоимость"].ToString(); double price = 0;
                            if (double.TryParse(selectedPrice, out price))
                            { cenaTextBox.Text = price.ToString("N0");}
                        }
                    }
                };
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            db.closeConnection();
        }
        public void LoadData2()
        {
            DataBase db = new DataBase();
            db.openConnection();
            try
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT ID_Заболевания, Название FROM Заболевании", db.GetConnection());
                DataTable dataTable = new DataTable(); SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlDataAdapter.Fill(dataTable); // Создаем копию DataTable, чтобы избежать привязки данных.
                thabComboBox.DisplayMember = "Название"; thabComboBox.ValueMember = "ID_Заболевания"; thabComboBox.DataSource = dataTable; thabComboBox.SelectedIndex = -1;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            db.closeConnection();
        }
        public void LoadData3()
        {
            DataBase db = new DataBase();
            db.openConnection();
            try
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT DISTINCT Пациенты.ID_Клиента, Пациенты.Фамилия + ' ' + Пациенты.Имя + ' ' + Пациенты.Отчество AS 'ФИО' FROM Пациенты INNER JOIN Заявки ON Пациенты.ID_Клиента = Заявки.ID_Клиента INNER JOIN Сотрудники ON Заявки.ID_Сотрудника = Сотрудники.ID_Сотрудника INNER JOIN Пользователи ON Сотрудники.ID_Пользователя = Пользователи.ID_Пользователя WHERE Пользователи.ID_Пользователя = @UserId", db.GetConnection());
                sqlCommand.Parameters.AddWithValue("@UserId", currentlog);
                DataTable dataTable = new DataTable();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlDataAdapter.Fill(dataTable);
                fiopazComboBox2.DisplayMember = "ФИО"; fiopazComboBox2.ValueMember = "ID_Клиента"; fiopazComboBox2.DataSource = dataTable; fiopazComboBox2.SelectedIndex = -1;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            db.closeConnection();
        }

        private string doctorId = "";
        public void LoadData4()
        {
            DataBase db = new DataBase();
            db.openConnection();
            try
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT ID_Сотрудника, Фамилия + ' ' + Имя + ' ' + Отчество AS 'ФИО', Номер_кабинета FROM Сотрудники INNER JOIN Пользователи ON Сотрудники.ID_Пользователя = Пользователи.ID_Пользователя WHERE Пользователи.ID_Пользователя = @UserId", db.GetConnection());
                sqlCommand.Parameters.AddWithValue("@UserId", currentlog);
                DataTable dataTable = new DataTable();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlDataAdapter.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                { DataRow row = dataTable.Rows[0];string fio = row["ФИО"].ToString();  vrachTextBox.Text = fio;   doctorId = row["ID_Сотрудника"].ToString(); }// Устанавливаем ФИО в TextBox
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
            finally
            { db.closeConnection();}          
        }
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (uslComboBox.SelectedItem == null || thabComboBox.SelectedItem == null || fiopazComboBox2.SelectedItem == null || string.IsNullOrWhiteSpace(vrachTextBox.Text) || string.IsNullOrWhiteSpace(appDateTimePicker.Text) || adresComboBox.SelectedItem == null || string.IsNullOrWhiteSpace(kolTextBox.Text) || string.IsNullOrWhiteSpace(cenaTextBox.Text) || string.IsNullOrWhiteSpace(summTextBox.Text) || garComboBox.SelectedItem == null || vidComboBox.SelectedItem == null)
            {this.Alert("Ошибка при добавлении", "Пожалуйста, заполните все поля со звёздочкой!", false); }
            else
            {
                var uslValue = uslComboBox.SelectedValue.ToString(); var thstValue = thabComboBox.SelectedValue.ToString(); var fiopaztValue = fiopazComboBox2.SelectedValue.ToString();
                if (db.CheckJob(uslValue, thstValue, fiopaztValue, doctorId, appDateTimePicker.Text))
                { this.Alert("Ошибка при добавлении", "Такой договор на оказание услуг уже был создан!", false); }
                else if (DateTime.Parse(appDateTimePicker.Text) < DateTime.Today)
                { this.Alert("Ошибка при добавлении", "Нельзя составить договор, если указана дата ниже сегодняшней!", false); }
                else
                {
                    change.SetButtonColors(addgogiconButton, change.DefBackGroundColor, change.DefForeGroundColor);
                    string add = $"insert into Договоры (ID_Услуги, ID_Заболевания, ID_Клиента, ID_Сотрудника, Дата_составления, Адрес_стоматологии, Количество, Цена_услуги, Итоговая_стоимость, Вид_оплаты, Гарантия) values ('{uslValue}', '{thstValue}', '{fiopaztValue}', '{doctorId}', '{appDateTimePicker.Value.ToString("yyyy-MM-dd")}', '{adresComboBox.Text}', '{kolTextBox.Text}', '{cenaTextBox.Text}', '{summTextBox.Text}', '{vidComboBox.Text}', '{garComboBox.Text}')";
                    db.queryExecute(add);
                    this.Alert("Добавление в таблицу", "Новая договор на оказание услуг был успешно добавлен в таблицу.", false);
                    TabletСontracts();
                    change.SetButtonColors(addgogiconButton, change.DefBackGroundColor, change.DefForeGroundColor);
                    договорыDataGridView.Height += 290;applguna2GroupBox.Visible = false; uslComboBox.SelectedIndex = -1;thabComboBox.SelectedIndex = -1; fiopazComboBox2.SelectedIndex = -1;  appDateTimePicker.Text = "";  adresComboBox.SelectedIndex = -1;   kolTextBox.Text = "";  cenaTextBox.Text = "";  summTextBox.Text = "";  garComboBox.SelectedIndex = -1;  vidComboBox.SelectedIndex = -1;
                }
            }
        }                    
        private void deliconButton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = DialogForm.Show("Подтверждение удаления", "Вы уверены, что хотите удалить выбранный договор?", false);
            if (dialogResult == DialogResult.Yes)             
            {
                List<int> idsToDelete = new List<int>();
                foreach (DataGridViewRow row in договорыDataGridView.SelectedRows)
                {
                    int id = Convert.ToInt32(row.Cells[0].Value); // Используем индекс столбца
                    idsToDelete.Add(id);
                }

                if (idsToDelete.Count > 0)
                {
                    string ids = string.Join(",", idsToDelete); // Преобразуем список ID в строку для использования в запросе

                    string connectionString = db.getConnectionString();
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        SqlCommand delete = new SqlCommand($"delete from Договоры where ID_Договора IN ({ids})");
                        delete.Connection = conn;
                        delete.ExecuteNonQuery();
                        this.Alert("Удаление в таблице", "Выбранная запись была успешно удалена!", false);
                        string query = "SELECT * FROM Договоры";
                        DataTable dt = db.getData(query);
                        договорыDataGridView.DataSource = dt;
                    }
                }
            }
            else { return; }
            TabletСontracts();
        }
        private void addgogiconButton_Click(object sender, EventArgs e)
        {   if (applguna2GroupBox.Visible) { this.Alert("Предупреждение при добавлении", "Окно для добавления нового договора уже открыто!", false); }
            else { IconButton activeButton = (IconButton)sender; change.SetButtonColors(activeButton, change.ActiveBackGroundColor, change.ActiveForeGroundColor); договорыDataGridView.Height -= 290; applguna2GroupBox.Visible = true; 
              LoadData(); LoadData2(); LoadData3(); LoadData4(); uslComboBox.SelectedIndex = -1; thabComboBox.SelectedIndex = -1; fiopazComboBox2.SelectedIndex = -1; appDateTimePicker.Text = ""; adresComboBox.SelectedIndex = -1; kolTextBox.Text = ""; cenaTextBox.Text = ""; summTextBox.Text = ""; garComboBox.SelectedIndex = -1; vidComboBox.SelectedIndex = -1; } }
        private void cleariconButton2_Click(object sender, EventArgs e)
        { uslComboBox.SelectedIndex = -1; thabComboBox.SelectedIndex = -1; fiopazComboBox2.SelectedIndex = -1; appDateTimePicker.Text = ""; adresComboBox.SelectedIndex = -1; kolTextBox.Text = ""; cenaTextBox.Text = ""; summTextBox.Text = ""; garComboBox.SelectedIndex = -1; vidComboBox.SelectedIndex = -1; }
        private void closeiconButton2_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = DialogForm.Show("Подтверждение выхода при добавлении", "Вы уверены, что хотите отменить добавление\nнового договора? Несохранённые данные будут потеряны!", true);
            if (dialogResult == DialogResult.Yes)  { change.SetButtonColors(addgogiconButton, change.DefBackGroundColor, change.DefForeGroundColor); uslComboBox.SelectedIndex = -1; thabComboBox.SelectedIndex = -1; fiopazComboBox2.SelectedIndex = -1; appDateTimePicker.Text = ""; adresComboBox.SelectedIndex = -1; kolTextBox.Text = ""; cenaTextBox.Text = ""; summTextBox.Text = ""; garComboBox.SelectedIndex = -1; vidComboBox.SelectedIndex = -1; договорыDataGridView.Height += 290; applguna2GroupBox.Visible = false; }
            else { return; }
        }
        private void guna2CustomGradientPanel4_Paint(object sender, PaintEventArgs e)
        {Change.ChangeButtonColorToDodgerBlue(iconButton15); Change.ChangeButtonColorToTransparent(iconButton13); Change.ChangeButtonColorToTransparent(iconButton4); }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = DialogForm.Show("Редактирование в таблице", "Вы уверены, что хотите изменить запись?", false);
            if (dialogResult == DialogResult.Yes)              
            {
                if (garCombobox2.SelectedItem == null || vidComboBox2.SelectedItem == null || string.IsNullOrWhiteSpace(DateTimePicker2.Text) || adresComboBox2.SelectedItem == null || string.IsNullOrWhiteSpace(kolTextBox2.Text) || string.IsNullOrWhiteSpace(uslTextBox2.Text) || string.IsNullOrWhiteSpace(summTextBox2.Text))
                { this.Alert("Ошибка при редактировании", "Пожалуйста, заполните все поля! ", false); }
                else
                {
                    try
                    {
                        this.Validate();
                        string gr = garCombobox2.Text; string vid = vidComboBox2.Text; DateTime date = DateTimePicker2.Value; string adres = adresComboBox2.Text; string kol = kolTextBox2.Text; string usl = uslTextBox2.Text; string summ = summTextBox2.Text;
                        using (var connection = new SqlConnection(db.getConnectionString()))
                        {
                            connection.Open();
                            string query = "UPDATE Договоры SET Дата_составления = @date, Адрес_стоматологии = @adres , Количество = @kol , Цена_услуги = @usl, Итоговая_стоимость = @summ, Вид_оплаты = @vid ,Гарантия = @gr  WHERE ID_Договора = @id";
                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@date", date); command.Parameters.AddWithValue("@adres", adres); command.Parameters.AddWithValue("@kol", kol); command.Parameters.AddWithValue("@usl", usl); command.Parameters.AddWithValue("@summ", summ); command.Parameters.AddWithValue("@vid", vid); command.Parameters.AddWithValue("@gr", gr);
                                int id = selectedId; command.Parameters.AddWithValue("@id", id);
                                int rowsAffected = command.ExecuteNonQuery();
                                 if (rowsAffected > 0)
                                 {
                                        this.Alert("Редактирование в таблице", "Данные успешно изменены!", false);
                                        договорыDataGridView.DataSource = null; договорыBindingSource.ResetBindings(false);
                                        договорыBindingSource.DataSource = db.getData("SELECT * FROM Договоры"); договорыDataGridView.DataSource = договорыBindingSource;
                                        updateCustomGradientPanel4.Visible = false; guna2CustomGradientPanel7.Visible = true; panel6.Visible = true; dogovorPanel.Visible = true;
                                 }
                             else { this.Alert("Ошибка при редактировании", "Не удалось обновить данные. Запись не найдена!", false); return; }   
                            }
                        }
                    }
                    catch (System.Exception ex)
                    { MessageBox.Show("Ошибка при редактировании: " + ex.Message); }
                }
            }
            else { return; } TabletСontracts();
        }
        private void iconButton15_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = DialogForm.Show("Подтверждение выхода при редактировании", "Вы уверены, что хотите отменить редактирование договора\nна оказание услуг? Несохранённые данные будут потеряны!", true);
            if (dialogResult == DialogResult.Yes) { updateCustomGradientPanel4.Visible = false; guna2CustomGradientPanel7.Visible = true; panel6.Visible = true; dogovorPanel.Visible = true; }
            else { return; }
        }
        private void updateiconButton_Click(object sender, EventArgs e)
        {
            if (applguna2GroupBox.Visible == true)
            { this.Alert("Предупреждение при редактировании", "Закройте панель для добавления нового договора на оказание\nуслуг перед тем, как открыть редактирование записи!", true); return; }         
            else if (договорыDataGridView.CurrentRow != null)
            {
                updateCustomGradientPanel4.Visible = true; guna2CustomGradientPanel7.Visible = false; panel6.Visible = false; dogovorPanel.Visible = false;
                DataGridViewRow selectedRow = договорыDataGridView.CurrentRow;
                nomlabel.Text = selectedRow.Cells["ID_Договора"].Value?.ToString() ?? ""; guna2TextBox4.Text = selectedRow.Cells["Услуга"].Value?.ToString() ?? "";   guna2TextBox5.Text = selectedRow.Cells["Заболевание"].Value?.ToString() ?? "";
                guna2TextBox6.Text = selectedRow.Cells["Пациент"].Value?.ToString() ?? "";guna2TextBox7.Text = selectedRow.Cells["Врач"].Value?.ToString() ?? "";   DateTimePicker2.Text = selectedRow.Cells["Дата_составления"].Value?.ToString() ?? ""; 
                adresComboBox2.Text = selectedRow.Cells["Адрес_стоматологии"].Value?.ToString() ?? "";   kolTextBox2.Text = selectedRow.Cells["Количество"].Value?.ToString() ?? "";
               
                string price = selectedRow.Cells["Цена_услуги"].Value?.ToString();
                if (price != null)
                {uslTextBox2.Text = price.Replace("₽", ""); }
                string totalCost = selectedRow.Cells["Итоговая_стоимость"].Value?.ToString();
                if (totalCost != null)             
                { summTextBox2.Text = totalCost.Replace("₽", ""); }
               
                garCombobox2.Text = selectedRow.Cells["Гарантия"].Value?.ToString() ?? "";
                vidComboBox2.Text = selectedRow.Cells["Вид_оплаты"].Value?.ToString() ?? "";
            }
            else
            { this.Alert("Ошибка при редактировании", "Не удалось обновить данные. Запись не найдена!", false); return; }
        }
        private int selectedId; // объявление переменной
        private void договорыDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var cell = договорыDataGridView.Rows[e.RowIndex].Cells["ID_Договора"];
                if (cell != null && cell.Value != null && int.TryParse(cell.Value.ToString(), out int id))
                { selectedId = id; }
            }
        }
        private void iconButton14_Click(object sender, EventArgs e)
        { string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Положение.pdf"); Process.Start(folderPath); } // Открытие файла Положение.pdf
        private void wordfileiconButton_Click(object sender, EventArgs e)
        {
            if (договорыDataGridView.CurrentRow != null)
            {
                DataGridViewRow selectedRow = договорыDataGridView.CurrentRow;
                int Номер_Договора = Convert.ToInt32(selectedRow.Cells["ID_Договора"].Value);
                string Услуга = selectedRow.Cells["Услуга"].Value?.ToString(); string Заболевание = selectedRow.Cells["Заболевание"].Value?.ToString(); string Пациент = selectedRow.Cells["Пациент"].Value?.ToString(); string Врач = selectedRow.Cells["Врач"].Value?.ToString();
                string Дата = selectedRow.Cells["Дата_составления"].Value.ToString(); DateTime date = Convert.ToDateTime(Дата);
                string formattedDate = date.ToString("dd.MM.yyyy") + " г.";
                string Адрес = selectedRow.Cells["Адрес_стоматологии"].Value?.ToString();
                string Количество = selectedRow.Cells["Количество"].Value?.ToString();
                string Цена = selectedRow.Cells["Цена_услуги"].Value?.ToString();
                string Стоимость = selectedRow.Cells["Итоговая_стоимость"].Value?.ToString();
                string Гарантия = selectedRow.Cells["Гарантия"].Value?.ToString();
                string Вид = selectedRow.Cells["Вид_оплаты"].Value?.ToString();
                
                string outputPath = Path.Combine(Environment.CurrentDirectory, "Договоры", $"Договор на оказание стоматологических услуг № {Номер_Договора}.docx");
                if (db.IsFileLocked(outputPath))
                { this.Alert("Создание договора ", $"Договор на оказание стоматологических услуг № {Номер_Договора} уже открыт!\nЗакройте его, прежде чем создавать новый договор.", true); }
                else
                {
                    Word._Application oWord = new Word.Application();
                    oWord.Visible = true;
                    Word._Document oDoc = oWord.Documents.Open(Path.Combine(Environment.CurrentDirectory, "Договор на оказание медицинских услуг.docx"));
                    oDoc.Bookmarks["id"].Range.Text = Convert.ToString(Номер_Договора); oDoc.Bookmarks["id2"].Range.Text = Convert.ToString(Номер_Договора);
                    oDoc.Bookmarks["services"].Range.Text = Convert.ToString(Услуга);
                    oDoc.Bookmarks["disease"].Range.Text = Convert.ToString(Заболевание);
                    oDoc.Bookmarks["patient"].Range.Text = Convert.ToString(Пациент);
                    oDoc.Bookmarks["sot"].Range.Text = Convert.ToString(Врач);
                    oDoc.Bookmarks["date"].Range.Text = formattedDate; oDoc.Bookmarks["date2"].Range.Text = formattedDate;
                    oDoc.Bookmarks["adres"].Range.Text = Convert.ToString(Адрес);
                    oDoc.Bookmarks["kol"].Range.Text = Convert.ToString(Количество);
                    oDoc.Bookmarks["price"].Range.Text = Convert.ToString(Цена);
                    oDoc.Bookmarks["summ"].Range.Text = Convert.ToString(Стоимость); oDoc.Bookmarks["summ2"].Range.Text = Convert.ToString(Стоимость);
                    oDoc.Bookmarks["guarantee"].Range.Text = Convert.ToString(Гарантия);
                    oDoc.Bookmarks["vid"].Range.Text = Convert.ToString(Вид);

                    oDoc.SaveAs(FileName: outputPath);
                    oDoc.Close();oWord.Quit();
                    this.Alert("Создание договора", $"Договор № {Номер_Договора} на оказание стоматологических услуг,\nбыл создан успешно!", true);
                }
            }
            else
            { this.Alert("Ошибка при создании документа", "Не удалось создать договор на оказание медицинских услуг.\nЗапись не найдена!", true); return; }
        }
        //---------------------------------------Мой аккаунт
        private void LoadPersonalData()
        {
            SurnameTextBox.Text = String.Format("{0} {1} {2}", PersonalArea.FirstName, PersonalArea.LastName, PersonalArea.FatherName); 
            doltextbox2.Text = PersonalArea.Dol; CategoryTextBox.Text = PersonalArea.Category; exTextBox.Text = PersonalArea.Ex; numTextBox.Text = PersonalArea.NumberTel;   loginTextBox.Text = PersonalArea.Login;
            PersonalArea personalArea = new PersonalArea(); 
            if (personalArea.SetPersonalData(PersonalArea.Login, PersonalArea.Password))
            {
                if (personalArea.Foto != null && personalArea.Foto.Length > 0)
                { using (MemoryStream ms = new MemoryStream(personalArea.Foto)) { фотоpictureBox2.Image = System.Drawing.Image.FromStream(ms);  }
                }
            }
        }
        private void settingButton_Click(object sender, EventArgs e)
        {   menuPanel.Visible = true; applicationsPanel.Visible = false; klientPanel.Visible = false; diseasePanel.Visible = false; dolPanel.Visible = false; AboutMePanel.Visible = true; leftpanel1.Visible = false; leftpanel2.Visible = false; leftpanel3.Visible = false; leftpanel4.Visible = false; leftpanel5.Visible = false;
            IconButton activeButton = (IconButton)sender;// установка цвета кнопки
            change.SetButtonColors(activeButton, change.ActiveBackGroundColor, change.ActiveForeGroundColor);
            change.SetButtonColors(iconButton1, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton11, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton6, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton2, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton3, change.DefBackGroundColor, change.DefForeGroundColor); 
            LoadPersonalData(); UpdateStatusVisibility();
        }
        private void guna2CirclePictureBox3_Click(object sender, EventArgs e)
        { AboutMePanel.Visible = false; change.SetButtonColors(settingButton, change.DefBackGroundColor, change.DefForeGroundColor); }
        private void guna2Button3_Click_1(object sender, EventArgs e)
        {
            string oldEnteredPassword = PersonalArea.hashPassword(oldpasswordTextBox.Text); var newpassword = PersonalArea.hashPassword(newpasswordTextBox2.Text);
            if (PersonalArea.Password != oldEnteredPassword)
            { this.Alert("Ошибка при изменении пароля", "Введенный старый пароль неверный.", false); return; } // Если введенный старый пароль не совпадает с хэшированным паролем из базы данных
            else if (newpasswordTextBox.Text == "" || newpasswordTextBox.Text == null)
            { this.Alert("Ошибка при изменении пароля", "Введите новый пароль.", false); return; }
            else if (newpasswordTextBox2.Text == "" || newpasswordTextBox2.Text == null)
            { this.Alert("Ошибка при изменении пароля", "Повторите пароль.", false); return; }
            else if (newpasswordTextBox.Text != newpasswordTextBox2.Text)
            { this.Alert("Ошибка при изменении пароля", "Пароли не совпадают.", false); return; }
            DialogResult dialogResult = DialogForm.Show("Подтверждение на изменение пароля", "Изменить старый пароль на новый?", false);
            if (dialogResult == DialogResult.Yes)
            {
                using (var connection = new SqlConnection(db.getConnectionString()))
                {
                    connection.Open();
                    string queryUpdateData = "UPDATE Пользователи SET Пароль = @newPassword WHERE Пароль = @oldPassword";
                    SqlCommand command = new SqlCommand(queryUpdateData, connection);
                    command.Parameters.AddWithValue("@newPassword", newpassword); command.Parameters.AddWithValue("@oldPassword", PersonalArea.Password);
                    int rowsUpdated = command.ExecuteNonQuery();
                    if (rowsUpdated > 0)
                    { this.Alert("Изменение пароля", "Старый пароль был успешно изменен на новый.", false); passwordPanel.Visible = false; }
                    else
                    { this.Alert("Ошибка при изменении пароля", "Недопустимые символы при изменении пароля.", false); }
                    connection.Close();
                }
            }
            else { return;}
        }
        private void guna2CheckBox2_CheckedChanged_1(object sender, EventArgs e)
        { if (guna2CheckBox2.Checked) { newpasswordTextBox2.UseSystemPasswordChar = false; } else { newpasswordTextBox2.UseSystemPasswordChar = true; } }
        private void iconButton17_Click_1(object sender, EventArgs e)
        { DialogResult dialogResult = DialogForm.Show("Изменение пароля", "Вы точно хотите отменить изменение пароля?", false);
          if (dialogResult == DialogResult.Yes)
           { passwordPanel.Visible = false; newpasswordTextBox.Text = ""; newpasswordTextBox2.Text = ""; oldpasswordTextBox.Text = ""; guna2CheckBox2.Checked = false; }
           else { return; }         
        }
        private void guna2Button4_Click(object sender, EventArgs e)
        { oldpasswordTextBox.Text = ""; passwordPanel.Visible = true; newpasswordTextBox.Text = ""; newpasswordTextBox2.Text = ""; }
        private void upstatusiconButton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = DialogForm.Show("Подтверждение на изменение статуса", "Изменить статус?", false);
            if (dialogResult == DialogResult.Yes)
            {
              using (var connection = new SqlConnection(db.getConnectionString()))
              {
                connection.Open();
                string queryGetEmployeeId = "SELECT ID_Сотрудника FROM Сотрудники WHERE ID_Пользователя = @UserId";
                SqlCommand getEmployeeIdCommand = new SqlCommand(queryGetEmployeeId, connection);
                getEmployeeIdCommand.Parameters.AddWithValue("@UserId", currentlog);
                int currentEmployeeId = (int)getEmployeeIdCommand.ExecuteScalar(); // Получаем ID_сотрудника
                if (currentEmployeeId != 0)
                {
                    string newStatus = StatusComboBox.Text; 
                    string queryUpdateStatus = "UPDATE Сотрудники SET Статус = @newStatus WHERE ID_Сотрудника = @currentEmployeeId";
                    SqlCommand command = new SqlCommand(queryUpdateStatus, connection);
                    command.Parameters.AddWithValue("@newStatus", newStatus);
                    command.Parameters.AddWithValue("@currentEmployeeId", currentEmployeeId);
                    int rowsUpdated = command.ExecuteNonQuery();
                    if (rowsUpdated > 0)
                    { this.Alert("Изменение статуса", "Статус сотрудника был успешно изменен.", false);UpdateStatusVisibility(); }
                    else
                    { this.Alert("Ошибка при изменении статуса", "Не удалось изменить статус сотрудника.", false); }
                } connection.Close();
              }
            }
            else { return; }
        }
        private void AboutMePanel_Paint(object sender, PaintEventArgs e)
        {Change.ChangeButtonColorToTransparent(upstatusiconButton); Change.ChangeButtonColorToTransparent(closeButton); }
        private void UpdateStatusVisibility()
        {          
            string selectedStatus = StatusComboBox.Text;
            if (selectedStatus == "Занят")
            {busypictureBox.Visible = true;freepictureBox.Visible = false;}
            else if (selectedStatus == "Свободен")
            {busypictureBox.Visible = false;freepictureBox.Visible = true; }        
        }
    }
}

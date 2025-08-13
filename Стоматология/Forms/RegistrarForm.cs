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
using System.Reflection.Emit;
using System.Windows.Markup;
using Стоматология.MessageBoxForms;
using Microsoft.Office.Interop.Word;
using Point = System.Drawing.Point;
using Application = System.Windows.Forms.Application;
using DataTable = System.Data.DataTable;
using System.Text.RegularExpressions;

namespace Стоматология.Forms
{
    public partial class RegistrarForm : Form
    {
        DataBase db = new DataBase();
        Change change = new Change();
        private Point lastLocation; private bool mouseDown; // Создание переменных для перемещения формы

        public void Alert(string msg, string type, bool liftText)
        { FormAlert frm = new FormAlert(); frm.showAlert(msg, type, liftText); }// Создание переменных для сообщения
        public RegistrarForm()
        {
            InitializeComponent();
        }
        private void RegistrarForm_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "сП4DataSet.Сотрудники". При необходимости она может быть перемещена или удалена.
            this.сотрудникиTableAdapter.Fill(this.сП4DataSet.Сотрудники);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "сП4DataSet.Услуги". При необходимости она может быть перемещена или удалена.
            this.услугиTableAdapter.Fill(this.сП4DataSet.Услуги);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "сП4DataSet.Заявки". При необходимости она может быть перемещена или удалена.
            this.заявкиTableAdapter.Fill(this.сП4DataSet.Заявки);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "сП4DataSet.Пациенты". При необходимости она может быть перемещена или удалена.
            this.пациентыTableAdapter.Fill(this.сП4DataSet.Пациенты);
            DateTime currentDateTime = DateTime.Now; string dateString = currentDateTime.ToString("dd/MM/yyyy") + " г."; datelable.Text = dateString; loginlable.Text = PersonalArea.Login;
            Change.ChangeButtonColorToTransparent(exitButton); Change.ChangeButtonColorToTransparent(settingButton); Change.ChangeButtonColorToTransparent(iconButton2); Change.ChangeButtonColorToTransparent(iconButton3); Change.ChangeButtonColorToTransparent(iconButton7); Change.ChangeButtonColorToRed(iconButton8); Change.ChangeButtonColorToDodgerBlue(iconButton23); Change.ChangeButtonColorToTransparent(cleariconButton2); Change.ChangeButtonColorToTransparent(close); 
        }
        private void iconButton5_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = DialogForm.Show("Подтверждение выхода", "Вы уверены, что хотите выйти из аккаунта?\nНесохранённые данные будут потеряны!", true);
            if (dialogResult == DialogResult.Yes)
            { this.Hide(); Form exit = new MenuAutchForm(); exit.Show(); ; }
            else
            { return; }
        }

        private void iconButton8_Click_1(object sender, EventArgs e)
        {
            DialogResult dialogResult = DialogForm.Show("Подтверждение выхода", "Вы уверены, что хотите закрыть программу?\nНесохранённые данные будут потеряны!", true);
            if (dialogResult == DialogResult.Yes)
            { Application.Exit(); }
            else
            { return; }        
        }
        private void iconButton2_Click(object sender, EventArgs e)
        { this.WindowState = FormWindowState.Minimized; this.Alert("Фоновый режим", "Приложение находится в фоновом режиме!", false); }//свернуть форму
        private void iconButton7_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized) { this.WindowState = FormWindowState.Normal; this.MaximumSize = Screen.PrimaryScreen.WorkingArea.Size; }
            else { this.MaximumSize = Screen.PrimaryScreen.WorkingArea.Size; this.WindowState = FormWindowState.Maximized; }
            iconButton7.Visible = false;
        }
        private void iconButton3_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized) { this.WindowState = FormWindowState.Normal; this.MaximumSize = Screen.PrimaryScreen.WorkingArea.Size; }
            else { this.MaximumSize = Screen.PrimaryScreen.WorkingArea.Size; this.WindowState = FormWindowState.Maximized; }
            iconButton7.Visible = true;
        }

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

        private void iconButton13_Click(object sender, EventArgs e)
        {
            menuPanel.Visible = true; applicationsPanel.Visible = false; klientPanel.Visible = false; uslugiPanel.Visible = false; sotrudPanel.Visible = false; AboutMePanel.Visible = false;
            IconButton activeButton = (IconButton)sender;// установка цвета кнопки
            change.SetButtonColors(activeButton, change.ActiveBackGroundColor, change.ActiveForeGroundColor);
            leftpanel1.Visible = true; 
            // Сброс цвета кнопок
            change.SetButtonColors(iconButton1, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton11, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton6, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton4, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(settingButton, change.DefBackGroundColor, change.DefForeGroundColor);// Сброс цвета кнопок
            leftpanel2.Visible = false; leftpanel3.Visible = false; leftpanel4.Visible = false; leftpanel5.Visible = false;
        }
        private void iconButton1_Click(object sender, EventArgs e)
        {
            applicationsPanel.Visible = true; klientPanel.Visible = false; uslugiPanel.Visible = false; sotrudPanel.Visible = false; AboutMePanel.Visible = false;
            IconButton activeButton = (IconButton)sender;// установка цвета кнопки
            change.SetButtonColors(activeButton, change.ActiveBackGroundColor, change.ActiveForeGroundColor);
            leftpanel2.Visible = true;
            // Сброс цвета кнопок
            change.SetButtonColors(iconButton13, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton11, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton6, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton4, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(settingButton, change.DefBackGroundColor, change.DefForeGroundColor);// Сброс цвета кнопок
            leftpanel1.Visible = false; leftpanel3.Visible = false; leftpanel4.Visible = false; leftpanel5.Visible = false; TabletAppl();
        }
        private void iconButton11_Click(object sender, EventArgs e)
        {
            klientPanel.Visible = true; applicationsPanel.Visible = false; uslugiPanel.Visible = false; sotrudPanel.Visible = false; AboutMePanel.Visible = false;
            IconButton activeButton = (IconButton)sender;// установка цвета кнопки
            change.SetButtonColors(activeButton, change.ActiveBackGroundColor, change.ActiveForeGroundColor);
            leftpanel3.Visible = true;
            // Сброс цвета кнопок
            change.SetButtonColors(iconButton13, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton1, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton6, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton4, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(settingButton, change.DefBackGroundColor, change.DefForeGroundColor);// Сброс цвета кнопок
            leftpanel1.Visible = false; leftpanel2.Visible = false; leftpanel4.Visible = false; leftpanel5.Visible = false;
        }
        private void iconButton6_Click(object sender, EventArgs e)
        {        
            uslugiPanel.Visible = true; applicationsPanel.Visible = false; klientPanel.Visible = false; sotrudPanel.Visible = false; AboutMePanel.Visible = false;
            IconButton activeButton = (IconButton)sender;// установка цвета кнопки
            change.SetButtonColors(activeButton, change.ActiveBackGroundColor, change.ActiveForeGroundColor);
            leftpanel4.Visible = true; 
            // Сброс цвета кнопок
            change.SetButtonColors(iconButton13, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton1, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton11, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton4, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(settingButton, change.DefBackGroundColor, change.DefForeGroundColor);// Сброс цвета кнопок
            leftpanel1.Visible = false; leftpanel2.Visible = false; leftpanel3.Visible = false; leftpanel5.Visible = false;
        }
        private void iconButton4_Click_1(object sender, EventArgs e)
        {
            sotrudPanel.Visible = true; applicationsPanel.Visible = false; klientPanel.Visible = false; uslugiPanel.Visible = false; AboutMePanel.Visible = false;
            IconButton activeButton = (IconButton)sender;// установка цвета кнопки
            change.SetButtonColors(activeButton, change.ActiveBackGroundColor, change.ActiveForeGroundColor);
            leftpanel5.Visible = true;

            change.SetButtonColors(iconButton13, change.DefBackGroundColor, change.DefForeGroundColor);  change.SetButtonColors(iconButton1, change.DefBackGroundColor, change.DefForeGroundColor);change.SetButtonColors(iconButton11, change.DefBackGroundColor, change.DefForeGroundColor);change.SetButtonColors(iconButton6, change.DefBackGroundColor, change.DefForeGroundColor);change.SetButtonColors(settingButton, change.DefBackGroundColor, change.DefForeGroundColor);// Сброс цвета кнопок
            leftpanel1.Visible = false; leftpanel2.Visible = false; leftpanel3.Visible = false; leftpanel4.Visible = false;
            TableSot();
        }       
        //------------------------------------------- Таблицы Пациенты
        private void klientPanel_Paint(object sender, PaintEventArgs e)
        { Change.ChangeButtonColorToTransparent(iconButton14); Change.ChangeButtonColorToTransparent(iconButton16); Change.ChangeButtonColorToTransparent(iconButton12); Change.ChangeButtonColorToTransparent(iconButton10); Change.ChangeButtonColorToTransparent(iconButton22); Change.ChangeButtonColorToDodgerBlue(iconButton9); }
        private void iconButton10_Click(object sender, EventArgs e)
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
        private void iconButton14_Click_1(object sender, EventArgs e)
        {
            if (userguna2GroupBox.Visible) { this.Alert("Предупреждение при добавлении", "Окно для добавления нового пациента уже открыто!", false); }
            else
            {
                IconButton activeButton = (IconButton)sender; change.SetButtonColors(activeButton, change.ActiveBackGroundColor, change.ActiveForeGroundColor);
                famTextBox.Text = ""; nameTextBox.Text = ""; otchTextBox.Text = ""; nimermaskedTextBox.Text = ""; DateTimePicker.Text = ""; polComboBox.SelectedIndex = -1; pasportmaskedTextBox.Text = ""; cnilsmaskedTextBox.Text = ""; medmaskedTextBox.Text = ""; nameTextBox.Text = ""; adresTextBox.Text = ""; пациентыDataGridView.Height -= 250; userguna2GroupBox.Visible = true;
            }
        }
        private void famTextBox_KeyPress(object sender, KeyPressEventArgs e)
        { if (!char.IsControl(e.KeyChar) && char.IsDigit(e.KeyChar)) { e.Handled = true; } }// отмена ввода символа, если он является цифрой 
        private void nameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        { if (!char.IsControl(e.KeyChar) && char.IsDigit(e.KeyChar)) { e.Handled = true; } }// отмена ввода символа, если он является цифрой 
        private void otchTextBox_KeyPress(object sender, KeyPressEventArgs e)
        { if (!char.IsControl(e.KeyChar) && char.IsDigit(e.KeyChar)) { e.Handled = true; } }// отмена ввода символа, если он является цифрой 
        private void famguna2TextBox2_KeyPress(object sender, KeyPressEventArgs e)
        { if (!char.IsControl(e.KeyChar) && char.IsDigit(e.KeyChar)) { e.Handled = true; } }// отмена ввода символа, если он является цифрой 
        private void name2TextBox2_KeyPress(object sender, KeyPressEventArgs e)
        { if (!char.IsControl(e.KeyChar) && char.IsDigit(e.KeyChar)) { e.Handled = true; } }// отмена ввода символа, если он является цифрой 
        private void otchTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        { if (!char.IsControl(e.KeyChar) && char.IsDigit(e.KeyChar)) { e.Handled = true; } }// отмена ввода символа, если он является цифрой 
        private void povodtextBox_KeyPress(object sender, KeyPressEventArgs e)
        { if (!char.IsControl(e.KeyChar) && char.IsDigit(e.KeyChar)) { e.Handled = true; } }// отмена ввода символа, если он является цифрой 
        private void iconButton9_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = DialogForm.Show("Подтверждение выхода при добавлении", "Вы уверены, что хотите отменить добавление нового\nпациента? Несохранённые данные будут потеряны!", true);
            if (dialogResult == DialogResult.Yes)
            { change.SetButtonColors(iconButton14, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton13, change.DefBackGroundColor, change.DefForeGroundColor); famTextBox.Text = ""; nameTextBox.Text = ""; otchTextBox.Text = ""; nimermaskedTextBox.Text = ""; DateTimePicker.Text = ""; pasportmaskedTextBox.Text = ""; cnilsmaskedTextBox.Text = ""; medmaskedTextBox.Text = ""; nameTextBox.Text = ""; adresTextBox.Text = ""; polComboBox.SelectedIndex = -1; userguna2GroupBox.Visible = false; пациентыDataGridView.Height += 250; }
            else
            { return; }
        }
        private void iconButton22_Click(object sender, EventArgs e)
        { famTextBox.Text = ""; nameTextBox.Text = ""; otchTextBox.Text = ""; nimermaskedTextBox.Text = ""; DateTimePicker.Text = ""; pasportmaskedTextBox.Text = ""; cnilsmaskedTextBox.Text = ""; medmaskedTextBox.Text = ""; nameTextBox.Text = ""; adresTextBox.Text = ""; polComboBox.SelectedIndex = -1; }
        private void guna2Button4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(famTextBox.Text) || string.IsNullOrEmpty(nameTextBox.Text) || string.IsNullOrEmpty(otchTextBox.Text) || string.IsNullOrEmpty(nimermaskedTextBox.Text) || string.IsNullOrEmpty(DateTimePicker.Text) || string.IsNullOrEmpty(pasportmaskedTextBox.Text) || string.IsNullOrEmpty(cnilsmaskedTextBox.Text) || string.IsNullOrEmpty(medmaskedTextBox.Text) || string.IsNullOrEmpty(adresTextBox.Text) || polComboBox.SelectedIndex == -1)
            { this.Alert("Ошибка при добавлении", "Пожалуйста, заполните все поля со звёздочкой!", false); }
            else if (nimermaskedTextBox.MaskCompleted == false || pasportmaskedTextBox.MaskCompleted == false || cnilsmaskedTextBox.MaskCompleted == false || medmaskedTextBox.MaskCompleted == false)
            { this.Alert("Ошибка при добавлении", "Некоторые поля заполнены не до конца!\nПожалуйста, перепроверьте все поля ещё раз.", true); }
            else if (db.CheckPatient(famTextBox.Text, nameTextBox.Text, otchTextBox.Text))
            { this.Alert("Ошибка при добавлении", "Такой пациент уже существует в таблице!", false); }
            else
            {
                DateTime selectedDate = DateTimePicker.Value;
                if (selectedDate.Year == DateTime.Now.Year)
                { this.Alert("Ошибка при добавлении", "Нельзя добавить пациента с текущей датой рождения!", false); }
                else if (selectedDate.Year >= DateTime.Now.Year)
                { this.Alert("Ошибка при добавлении", "Нельзя добавить пациента с будущей датой рождения!", false); }
                else
                {
                    string пол = polComboBox.SelectedItem.ToString();
                    if (polComboBox.SelectedItem.ToString() == "Мужской")
                    { пол = "М."; }
                    else if (polComboBox.SelectedItem.ToString() == "Женский") { пол = "Ж."; }
                    var add = $"insert into Пациенты (Фамилия, Имя, Отчество, Номер_телефона, Дата_рождения, Пол, Серия_и_номер_паспорта, СНИЛС, Медицинкий_полис, Адрес) values ('{famTextBox.Text}', '{nameTextBox.Text}', '{otchTextBox.Text}', '{nimermaskedTextBox.Text}', '{DateTimePicker.Value.ToString("yyyy-MM-dd")}', '{пол}', '{pasportmaskedTextBox.Text}', '{cnilsmaskedTextBox.Text}', '{medmaskedTextBox.Text}', '{adresTextBox.Text}')";
                    db.queryExecute(add);
                    this.Alert("Добавление в таблицу", "Новый пациент был успешно добавлен в таблицу.", false);
                    change.SetButtonColors(iconButton14, change.DefBackGroundColor, change.DefForeGroundColor);
                    change.SetButtonColors(iconButton13, change.DefBackGroundColor, change.DefForeGroundColor); // Сброс цвета кнопок
                    // Обновление данных в таблице
                    пациентыDataGridView.DataSource = null; // Сброс источника данных
                    пациентыDataGridView.DataSource = db.getData("select * from Пациенты");
                    пациентыBindingSource.DataSource = пациентыDataGridView.DataSource;
                    пациентыDataGridView.Height += 250;
                    пациентыBindingSource.ResetBindings(false);

                    userguna2GroupBox.Visible = false;
                    // Применение фильтра заново
                    searchguna2TextBox.Clear(); // Очистка текста в поисковом поле
                    пациентыBindingSource.Filter = $"[Фамилия] LIKE '%{searchguna2TextBox.Text}%' OR [Имя] LIKE '%{searchguna2TextBox.Text}%' OR [Отчество] LIKE '%{searchguna2TextBox.Text}%'"; // Повторное применение фильтра
                }
            }
        }
        private void iconButton16_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = DialogForm.Show("Подтверждение удаления", "Вы уверены, что хотите удалить выбранную запись?", false);
            if (dialogResult == DialogResult.Yes)
            {
                List<int> idsToDelete = new List<int>();
                foreach (DataGridViewRow row in пациентыDataGridView.SelectedRows)
                {
                    int id = Convert.ToInt32(row.Cells["iDКлиентаDataGridViewTextBoxColumn"].Value);
                    idsToDelete.Add(id);
                }

                if (idsToDelete.Count > 0)
                {
                    string ids = string.Join(",", idsToDelete); // Преобразуем список ID в строку для использования в запросе

                    string connectionString = db.getConnectionString();
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        // Добавляем проверку, что сотрудника еще нет в таблице "Сотрудника"
                        SqlCommand checkQuery = new SqlCommand($"SELECT COUNT(*) FROM Заявки WHERE ID_Клиента IN ({ids})");
                        checkQuery.Connection = conn;
                        int count = Convert.ToInt32(checkQuery.ExecuteScalar());
                        if (count > 0)
                        { this.Alert("Удаление в таблице ", "Нельзя удалить пациента,\nу него есть назначенный прием к врачу!", true); }
                        else
                        {
                            SqlCommand delete = new SqlCommand($"delete from Пациенты where ID_Клиента IN ({ids})");
                            delete.Connection = conn;
                            delete.ExecuteNonQuery();
                            this.Alert("Удаление в таблице", "Выбранная запись была успешно удалена!", false);
                            string query = "SELECT * FROM Пациенты";
                            DataTable dt;
                            dt = db.getData(query);
                            пациентыDataGridView.DataSource = dt;
                            searchguna2TextBox.Clear(); // Очистка текста в поисковом поле
                            пациентыBindingSource.DataSource = dt; // Обновление источника данных
                            пациентыBindingSource.Filter = $"Фамилия LIKE '%{searchguna2TextBox.Text}%' OR Имя LIKE '%{searchguna2TextBox.Text}%' OR Отчество LIKE '%{searchguna2TextBox.Text}%'"; // Повторное применение фильтра
                        }
                    }
                }
            }
            else { return; }
        }
        private int Idd; // объявление переменной    
        private void пациентыDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var cell = пациентыDataGridView.Rows[e.RowIndex].Cells["iDКлиентаDataGridViewTextBoxColumn"];
                if (cell != null && cell.Value != null && int.TryParse(cell.Value.ToString(), out int id))
                { Idd = id; }
            }
        }
        private void iconButton12_Click(object sender, EventArgs e)
        {

            if (userguna2GroupBox.Visible == true)
            { this.Alert("Предупреждение при редактировании", "Закройте панель для добавления нового пациента перед тем,\nкак открыть редактирование записи!", true); return; }
            else if (пациентыDataGridView.CurrentRow != null)
            {
                updateCustomGradientPanel2.Visible = true; menuPanel2.Visible = false; guna2CustomGradientPanel3.Visible = false;
                DataGridViewRow selectedRow = пациентыDataGridView.CurrentRow;              
                famguna2TextBox2.Text = selectedRow.Cells["фамилияDataGridViewTextBoxColumn"].Value?.ToString() ?? "";
                name2TextBox2.Text = selectedRow.Cells["имяDataGridViewTextBoxColumn"].Value?.ToString() ?? "";
                otchTextBox2.Text = selectedRow.Cells["отчествоDataGridViewTextBoxColumn"].Value?.ToString() ?? "";
                maskedTextBox4.Text = selectedRow.Cells["номертелефонаDataGridViewTextBoxColumn"].Value?.ToString() ?? "";
                DateTimePicker2.Text = selectedRow.Cells["датарожденияDataGridViewTextBoxColumn"].Value?.ToString() ?? "";
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
            { this.Alert("Ошибка при редактировании", "Не удалось обновить данные. Запись не найдена!", false); return; }
        }
        private void guna2Button6_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = DialogForm.Show("Редактирование в таблице", "Вы уверены, что хотите изменить запись?", false);
            if (dialogResult == DialogResult.Yes)
            {
                if (string.IsNullOrEmpty(famguna2TextBox2.Text) || string.IsNullOrEmpty(name2TextBox2.Text) || string.IsNullOrEmpty(otchTextBox2.Text) || string.IsNullOrEmpty(maskedTextBox4.Text) || string.IsNullOrEmpty(DateTimePicker2.Text) || string.IsNullOrEmpty(maskedTextBox3.Text) || string.IsNullOrEmpty(maskedTextBox2.Text) || string.IsNullOrEmpty(maskedTextBox1.Text) || string.IsNullOrEmpty(adresguna2TextBox2.Text) || string.IsNullOrEmpty(polguna2TextBox2.Text) || string.IsNullOrEmpty(famguna2TextBox2.Text) || string.IsNullOrEmpty(name2TextBox2.Text) || string.IsNullOrEmpty(otchTextBox2.Text))
                { this.Alert("Ошибка при редактировании", "Пожалуйста, заполните все поля!", false); }
                else if (maskedTextBox4.MaskCompleted == false || maskedTextBox3.MaskCompleted == false || maskedTextBox2.MaskCompleted == false || maskedTextBox1.MaskCompleted == false)
                { this.Alert("Ошибка при редактировании", "Некоторые поля заполнены не до конца!\nПожалуйста, перепроверьте все поля ещё раз.", true); }
                else
                {
                    try
                    {
                        this.Validate();
                        string fam = famguna2TextBox2.Text; string name = name2TextBox2.Text; string otch = otchTextBox2.Text;
                        string number = maskedTextBox4.Text; string pasp = maskedTextBox3.Text; string cnils = maskedTextBox2.Text; string polis = maskedTextBox1.Text; string adres = adresguna2TextBox2.Text;
                        string pol = polguna2TextBox2.Text;
                        DateTime selectedDate = DateTimePicker2.Value;
                        if (selectedDate.Year == DateTime.Now.Year)
                        { this.Alert("Ошибка при редактировании", "Нельзя изменить данные пациента если\nу него указана текущая дата рождения!", true); }
                        else if (selectedDate.Year >= DateTime.Now.Year)
                        { this.Alert("Ошибка при редактировании", "Нельзя изменить данные пациента если\nу него указана будущая дата рождения!", true); }
                        else
                        {
                            using (var connection = new SqlConnection(db.getConnectionString()))
                            {
                                connection.Open();
                                string query = "UPDATE Пациенты SET Фамилия= @fam, Имя= @name, Отчество= @otch, Номер_телефона = @number, Дата_рождения = @selectedDate, Серия_и_номер_паспорта = @pasp, СНИЛС = @cnils, Медицинкий_полис = @polis, Адрес = @adres WHERE ID_Клиента = @id";
                                SqlCommand command = new SqlCommand(query, connection);
                                command.Parameters.AddWithValue("@number", number); command.Parameters.AddWithValue("@fam", fam); command.Parameters.AddWithValue("@name", name); command.Parameters.AddWithValue("@otch", otch);
                                command.Parameters.AddWithValue("@selectedDate", selectedDate);
                                command.Parameters.AddWithValue("@pasp", pasp); command.Parameters.AddWithValue("@cnils", cnils); command.Parameters.AddWithValue("@polis", polis); command.Parameters.AddWithValue("@adres", adres);
                                int id = Convert.ToInt32(пациентыDataGridView.CurrentRow.Cells["iDКлиентаDataGridViewTextBoxColumn"].Value);  // Предположим, что ID_Пользователя - это уникальное поле, по которому вы можете определить запись
                                command.Parameters.AddWithValue("@id", id);
                                int rowsAffected = command.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    this.Alert("Редактирование в таблице", "Данные успешно изменены!", false);
                                    пациентыDataGridView.DataSource = null; // Сброс источника данных
                                    пациентыDataGridView.DataSource = db.getData("select * from Пациенты"); // Обновление данных
                                    пациентыBindingSource.DataSource = пациентыDataGridView.DataSource;
                                    пациентыBindingSource.ResetBindings(false);
                                    updateCustomGradientPanel2.Visible = false; guna2CustomGradientPanel3.Visible = true; menuPanel2.Visible = true;
                                    searchguna2TextBox.Clear(); // Очистка текста в поисковом поле
                                    пациентыBindingSource.Filter = $"Фамилия LIKE '%{searchguna2TextBox.Text}%' OR Имя LIKE '%{searchguna2TextBox.Text}%' OR Отчество LIKE '%{searchguna2TextBox.Text}%'"; // Повторное применение фильтра
                                }
                                else
                                { this.Alert("Ошибка при редактировании", "Не удалось обновить данные. Запись не найдена!", false); return; }
                            }
                        }
                    }
                    catch (System.Exception ex) { MessageBox.Show("Ошибка при редактировании: " + ex.Message); }
                }
            } else { return; }
        }

        private void iconButton23_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = DialogForm.Show("Подтверждение выхода при редактировании", "Вы уверены, что хотите отменить редактирование\nпациента? Несохранённые данные будут потеряны!", true);
            if (dialogResult == DialogResult.Yes) { updateCustomGradientPanel2.Visible = false; menuPanel2.Visible = true; guna2CustomGradientPanel3.Visible = true; }
            else { return; }
        }
        //------------------------------------------- Таблицы записи
        private void applicationsPanel_Paint(object sender, PaintEventArgs e)
        { Change.ChangeButtonColorToTransparent(iconButton21); Change.ChangeButtonColorToTransparent(iconButton26); Change.ChangeButtonColorToTransparent(iconButton25); Change.ChangeButtonColorToTransparent(openTalonButton);  Change.ChangeButtonColorToTransparent(PDFFileiconButton); Change.ChangeButtonColorToDodgerBlue(iconButton18); Change.ChangeButtonColorToTransparent(iconButton24); }
        private void guna2CustomGradientPanel1_Paint(object sender, PaintEventArgs e)
        { }
        private void guna2TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        { if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) { e.Handled = true; } }// отмена ввода символа, если он не является цифрой или управляющим символом
        private void numberguna2TextBox2_KeyPress(object sender, KeyPressEventArgs e)
        { if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) { e.Handled = true; } }// отмена ввода символа, если он не является цифрой или управляющим символом
        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        { TabletAppl(filtrguna2TextBox.Text); }
        private void iconButton24_Click(object sender, EventArgs e)
        { filtrguna2TextBox.Text = ""; }
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

                SqlCommand sqlCommand = new SqlCommand("SELECT Заявки.ID_Заявки, Пациенты.Фамилия + ' ' + Пациенты.Имя + ' ' + Пациенты.Отчество AS Пациент, " +
                 "Сотрудники.Фамилия + ' ' + Сотрудники.Имя + ' ' + Сотрудники.Отчество AS Врач, " +
                 "Заявки.Номер_кабинета, Заявки.Адрес_стоматологии, Заявки.Дата_приема, Заявки.Время_приема, Заявки.Повод_обращения FROM Заявки " +
                 "INNER JOIN Пациенты ON Заявки.ID_Клиента = Пациенты.ID_Клиента INNER JOIN Сотрудники ON Заявки.ID_Сотрудника = Сотрудники.ID_Сотрудника", db.GetConnection());
                if (!string.IsNullOrEmpty(filterText))
                {
                    string[] searchTerms = filterText.Trim().Split(' '); string filterExpression = " WHERE ";
                    for (int i = 0; i < searchTerms.Length; i++)
                    {
                        string cleanedSearchTerm = Regex.Replace(searchTerms[i], "[^a-zA-Z0-9\']", "");
                        filterExpression += $"(Сотрудники.Фамилия LIKE '%{searchTerms[i]}%' OR Пациенты.Фамилия LIKE '%{searchTerms[i]}%')";
                        if (i < searchTerms.Length - 1)
                        { filterExpression += " AND "; }
                        
                    }
                    заявкиDataGridView.Columns["Номер_кабинета"].Width = 60; заявкиDataGridView.Columns["Время_приема"].Width = 50; заявкиDataGridView.Columns["Дата_приема"].Width = 100;
                    sqlCommand.CommandText += filterExpression;
                }
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
                        { data[data.Count - 1][i] = sqlDataReader[i].ToString();}                      
                    }
                }
                заявкиDataGridView.Columns["Номер_кабинета"].Width = 60; заявкиDataGridView.Columns["Время_приема"].Width = 50; заявкиDataGridView.Columns["Дата_приема"].Width = 100;
                sqlDataReader.Close();
                foreach (string[] s in data) заявкиDataGridView.Rows.Add(s);
                data.Clear();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { db.closeConnection(); }
        }
        public void LoadData()
        {
            DataBase db = new DataBase();
            db.openConnection();
            try
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT ID_Клиента, Фамилия + ' ' + Имя + ' ' + Отчество AS 'ФИО' FROM Пациенты", db.GetConnection());
                DataTable dataTable = new DataTable(); SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlDataAdapter.Fill(dataTable); // Создаем копию DataTable, чтобы избежать привязки данных.
                fioklComboBox.DisplayMember = "ФИО"; fioklComboBox.ValueMember = "ID_Клиента"; fioklComboBox.DataSource = dataTable; fioklComboBox.SelectedIndex = -1;
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
                SqlCommand sqlCommand = new SqlCommand("SELECT ID_Сотрудника, Фамилия + ' ' + Имя + ' ' + Отчество AS 'ФИО', Номер_кабинета FROM Сотрудники INNER JOIN Пользователи ON Сотрудники.ID_Пользователя = Пользователи.ID_Пользователя WHERE Пользователи.Роль = 'Врач'", db.GetConnection());
                DataTable dataTable = new DataTable();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlDataAdapter.Fill(dataTable);
                sotrudguna2ComboBox.DisplayMember = "ФИО"; sotrudguna2ComboBox.ValueMember = "ID_Сотрудника"; sotrudguna2ComboBox.DataSource = dataTable; sotrudguna2ComboBox.SelectedIndex = -1; sotrudguna2ComboBox.SelectedIndexChanged += (sender, e) =>
                {
                    if (sotrudguna2ComboBox.SelectedItem != null)
                    {
                        DataRowView selectedRow = (DataRowView)sotrudguna2ComboBox.SelectedItem;
                        string selectedID = selectedRow["ID_Сотрудника"].ToString();
                        DataRow[] foundRows = dataTable.Select("ID_Сотрудника = " + selectedID);
                        if (foundRows.Length > 0)
                        {
                            string selectedCab = foundRows[0]["Номер_кабинета"].ToString();
                            kabTextBox.Text = selectedCab;
                        }
                    }
                };
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            db.closeConnection();
        }
        private void guna2GroupBox1_Click(object sender, EventArgs e)
        { Change.ChangeButtonColorToTransparent(iconButton24); }
        private void iconButton21_Click(object sender, EventArgs e)
        {
            if (applguna2GroupBox.Visible) { this.Alert("Предупреждение при добавлении", "Окно для добавления новой записи на приём уже открыто!", false); }
            else
            { IconButton activeButton = (IconButton)sender; change.SetButtonColors(activeButton, change.ActiveBackGroundColor, change.ActiveForeGroundColor); fioklComboBox.SelectedIndex = -1; sotrudguna2ComboBox.SelectedIndex = -1; povodtextBox2.Text = ""; povodComboBox.SelectedIndex = -1; appDateTimePicker.Text = ""; kabTextBox.Text = ""; adresComboBox.SelectedIndex = -1; timeguna2ComboBox.SelectedIndex = -1; заявкиDataGridView.Height -= 260; }
            applguna2GroupBox.Visible = true;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (fioklComboBox.SelectedItem == null || sotrudguna2ComboBox.SelectedItem == null || string.IsNullOrWhiteSpace(povodtextBox2.Text) || timeguna2ComboBox.SelectedItem == null || string.IsNullOrWhiteSpace(appDateTimePicker.Text) || string.IsNullOrWhiteSpace(kabTextBox.Text) || adresComboBox.SelectedItem == null)
            { this.Alert("Ошибка при добавлении", "Пожалуйста, заполните все поля со звёздочкой!", false); }
            else
            {
                var selectedValue = fioklComboBox.SelectedValue.ToString(); var sotrudguna = sotrudguna2ComboBox.SelectedValue.ToString(); var timeString = timeguna2ComboBox.Text; DateTime time;
                if (db.CheckRecords(sotrudguna, appDateTimePicker.Text, timeguna2ComboBox.Text))
                { this.Alert("Ошибка при добавлении", "На это время уже назначен прием к врачу!", false); }
                else if (DateTime.Parse(appDateTimePicker.Text) < DateTime.Today)
                { this.Alert("Ошибка при добавлении", "Нельзя назначить прием пациенту, если\nуказана дата ниже сегодняшней!", true); }
                else
                {
                    change.SetButtonColors(iconButton14, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton13, change.DefBackGroundColor, change.DefForeGroundColor);
                    var add = $"insert into Заявки (ID_Клиента, ID_Сотрудника, Номер_кабинета, Адрес_стоматологии, Дата_приема, Время_приема, Повод_обращения) values ('{selectedValue}', '{sotrudguna}', '{kabTextBox.Text}', '{adresComboBox.Text}', '{appDateTimePicker.Value.ToString("yyyy-MM-dd")}','{timeguna2ComboBox.Text}','{povodtextBox2.Text}')";
                    var updateStatusQuery = $"UPDATE Сотрудники SET Статус = 'Занят' WHERE ID_Сотрудника = '{sotrudguna}'";
                    db.queryExecute(add); db.queryExecute(updateStatusQuery);
                    this.Alert("Добавление в таблицу", "Новая заявка на приём была успешно добавлен в таблицу.", false);
                    change.SetButtonColors(iconButton21, change.DefBackGroundColor, change.DefForeGroundColor); fioklComboBox.SelectedIndex = -1; sotrudguna2ComboBox.SelectedIndex = -1; povodtextBox2.Text = ""; povodComboBox.SelectedIndex = -1; appDateTimePicker.Text = ""; kabTextBox.Text = ""; adresComboBox.SelectedIndex = -1; timeguna2ComboBox.SelectedIndex = -1; applguna2GroupBox.Visible = false; заявкиDataGridView.Height += 260; TabletAppl(); // Очистка полей после добавления
                }
            }
        }
        private void iconButton19_Click(object sender, EventArgs e)
        { fioklComboBox.SelectedIndex = -1; sotrudguna2ComboBox.SelectedIndex = -1; povodComboBox.SelectedIndex = -1; appDateTimePicker.Text = ""; kabTextBox.Text = ""; adresComboBox.SelectedIndex = -1; timeguna2ComboBox.SelectedIndex = -1; povodtextBox2.Text = ""; }
        private void iconButton18_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = DialogForm.Show("Подтверждение выхода при добавлении", "Вы уверены, что хотите отменить добавление новой\nзаписи на приём? Несохранённые данные будут потеряны!", true);
            if (dialogResult == DialogResult.Yes) { change.SetButtonColors(iconButton21, change.DefBackGroundColor, change.DefForeGroundColor); fioklComboBox.SelectedIndex = -1; sotrudguna2ComboBox.SelectedIndex = -1; povodtextBox2.Text = ""; povodComboBox.SelectedIndex = -1; appDateTimePicker.Text = ""; kabTextBox.Text = ""; adresComboBox.SelectedIndex = -1; timeguna2ComboBox.SelectedIndex = -1; заявкиDataGridView.Height += 260; applguna2GroupBox.Visible = false; }
            else { return; }
        }
        private void panel8_Paint(object sender, PaintEventArgs e)
        { LoadData(); LoadData2(); }
        private void iconButton26_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = DialogForm.Show("Подтверждение удаления", "Вы уверены, что хотите удалить выбранную запись?", false);
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
                        // Проверяем, есть ли удаленные сотрудники в поле "Врачи"  
                        List<int> allEmployeeIds = new List<int>();
                        string getAllEmployeeIdsQuery = "SELECT ID_Сотрудника FROM Сотрудники";
                        DataTable employeeIdsData = db.getData(getAllEmployeeIdsQuery);
                        foreach (DataRow row in employeeIdsData.Rows)
                        { int employeeId = Convert.ToInt32(row["ID_Сотрудника"]); allEmployeeIds.Add(employeeId); }
                        // Проверка и изменение статуса для каждого сотрудника
                        foreach (int employeeId in allEmployeeIds)
                        {
                            // Проверка наличия ID сотрудника в таблице "Заявки"
                            string checkDoctorQuery = $"SELECT COUNT(*) FROM Заявки WHERE ID_Сотрудника = {employeeId}";
                            int doctorCount = db.queryExecuteScalar(checkDoctorQuery);
                            if (doctorCount == 0)
                            { string updateStatusQuery = $"UPDATE Сотрудники SET Статус = 'Свободен' WHERE ID_Сотрудника = {employeeId}"; db.queryExecute(updateStatusQuery); }
                        }
                    }
                    this.Alert("Удаление в таблице", "Выбранная запись была успешно удалена!", false);
                    string query = "SELECT * FROM Заявки";
                    DataTable dt = db.getData(query);
                    заявкиDataGridView.DataSource = dt;
                }
            }
            else { return; }
            TabletAppl();
        }
        private void iconButton30_Click_1(object sender, EventArgs e)
        { string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Талоны"); Process.Start(folder); } // Открытие папки Талоны        
        private void updateCustomGradientPanel_Paint(object sender, PaintEventArgs e)
        { Change.ChangeButtonColorToDodgerBlue(iconButton28); }
        private void iconButton28_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = DialogForm.Show("Подтверждение выхода при редактировании", "Вы уверены, что хотите отменить редактирование\nзаписи на приём? Несохранённые данные будут потеряны!", true);
            if (dialogResult == DialogResult.Yes) { updateCustomGradientPanel.Visible = false; guna2CustomGradientPanel2.Visible = true; guna2CustomGradientPanel1.Visible = true; panel8.Visible = true; }
            else { return; }
        }
        private void iconButton25_Click(object sender, EventArgs e)
        {
            if (applguna2GroupBox.Visible == true) 
            { this.Alert("Предупреждение при редактировании", "Закройте панель для добавления новой записи на прием\nперед тем, как открыть редактирование записи!", true); return; }
            else if (заявкиDataGridView.CurrentRow != null)
            {
                updateCustomGradientPanel.Visible = true; guna2CustomGradientPanel1.Visible = false; guna2CustomGradientPanel2.Visible = false; panel8.Visible = false;
                DataGridViewRow selectedRow = заявкиDataGridView.CurrentRow; nlabel.Text = selectedRow.Cells["ID_Заявки"].Value?.ToString() ?? "";
                numberguna2TextBox2.Text = selectedRow.Cells["Номер_кабинета"].Value?.ToString() ?? ""; adresguna2ComboBox2.Text = selectedRow.Cells["Адрес_стоматологии"].Value?.ToString() ?? "";
                fiopasguna2textBox2.Text = selectedRow.Cells["Пациент"].Value?.ToString() ?? ""; fiosotrudguna2TextBox2.Text = selectedRow.Cells["Врач"].Value?.ToString() ?? ""; dateguna2DateTimePicker.Text = selectedRow.Cells["Дата_приема"].Value?.ToString() ?? "";
                timeguna2ComboBox2.Text = selectedRow.Cells["Время_приема"].Value?.ToString() ?? ""; povodtextBox.Text = selectedRow.Cells["Повод_обращения"].Value?.ToString() ?? "";
            }
            else
            { this.Alert("Ошибка при редактировании", "Не удалось обновить данные. Запись не найдена!", false); return; }
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
        private void povodguna2ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        { povodtextBox.Text = povodguna2ComboBox2.SelectedItem.ToString(); }
        private void povodComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {if (povodComboBox.SelectedItem != null) {povodtextBox2.Text = povodComboBox.SelectedItem.ToString(); }  }
        private void guna2Button5_Click(object sender, EventArgs e)
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
                                        заявкиDataGridView.DataSource = null;
                                        заявкиBindingSource.ResetBindings(false);
                                        заявкиBindingSource.DataSource = db.getData("SELECT * FROM Заявки");
                                        заявкиDataGridView.DataSource = заявкиBindingSource;
                                        updateCustomGradientPanel.Visible = false; guna2CustomGradientPanel2.Visible = true; guna2CustomGradientPanel1.Visible = true; panel8.Visible = true;
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
        //------------------------------------------- Таблицы услуги
        private void guna2TextBox1_TextChanged_1(object sender, EventArgs e)
        {
            try
            {
                if (услугиBindingSource != null && searchguna2TextBox2.Text != "")
                {
                    string searchTerm = searchguna2TextBox2.Text.Trim(); string[] searchTerms = searchTerm.Split(' '); // Разделяем поисковый запрос на фамилию, имя и отчество
                    string filterExpression = "";  // Формируем строку фильтра с оператором AND
                    foreach (string term in searchTerms) { filterExpression += $"([Название] LIKE '%{term}%') AND "; }
                    filterExpression = filterExpression.Remove(filterExpression.Length - 5); услугиBindingSource.Filter = filterExpression; // Удаляем последний оператор AND
                }
                else { услугиBindingSource.Filter = ""; }
            }           
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void iconButton33_Click(object sender, EventArgs e)
        { searchguna2TextBox2.Text = ""; }
        private void uslugiPanel_Paint(object sender, PaintEventArgs e)
        { Change.ChangeButtonColorToTransparent(iconButton15); Change.ChangeButtonColorToTransparent(doc); Change.ChangeButtonColorToTransparent(iconButton5); Change.ChangeButtonColorToTransparent(filtericonButton2); Change.ChangeButtonColorToTransparent(iconButton33); Change.ChangeButtonColorToTransparent(iconButton19); }
        private void iconButton15_Click(object sender, EventArgs e)
        { string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Тарифы.pdf"); Process.Start(folderPath); } // Открытие файла Тарифы.pdf
        private void iconButton5_Click_1(object sender, EventArgs e)
        { string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Положение.pdf"); Process.Start(folderPath); } // Открытие файла Положение.pdf
        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (vidguna2ComboBox.SelectedIndex)
            {
                case 0: услугиBindingSource.Filter = $"[Вид] like 'Анестезия'"; break;
                case 1: услугиBindingSource.Filter = $"[Вид] like 'Рентгенография'"; break;
                case 2: услугиBindingSource.Filter = $"[Вид] like 'Терапевтические'"; break;
                case 3: услугиBindingSource.Filter = $"[Вид] like 'Ортопедические'"; break;
                case 4: услугиBindingSource.Filter = $"[Вид] like 'Ортодонтия'"; break;
                case 5: услугиBindingSource.Filter = $"[Вид] like 'Хирургические'"; break;
                case 6: услугиBindingSource.Filter = $"[Вид] like 'Физиотерапия'"; break;               
            }
        }
        private void iconButton19_Click_1(object sender, EventArgs e)
        { vidguna2ComboBox.SelectedIndex = -1; услугиBindingSource.Filter = ""; }
        private void iconButton34_Click(object sender, EventArgs e)
        {
            if (filterguna2Panel.Visible)
            { change.SetButtonColors(filtericonButton2, change.DefBackGroundColor, change.DefForeGroundColor); filterguna2Panel.Visible = false; vidguna2ComboBox.SelectedIndex = -1; услугиBindingSource.Filter = "";filtericonButton2.IconChar = IconChar.Filter; }
            else
            { change.SetButtonColors(filtericonButton2, change.ActiveBackGroundColor, change.ActiveForeGroundColor); filterguna2Panel.Visible = true; filtericonButton2.IconChar = IconChar.FilterCircleXmark; }
        }
        //------------------------------------------- Таблицы врачей
        private System.Drawing.Image ByteArrayToImage(byte[] byteArray)
        { MemoryStream ms = new MemoryStream(byteArray); System.Drawing.Image image = System.Drawing.Image.FromStream(ms); return image; }
        public List<System.Drawing.Image> photoImages = new List<System.Drawing.Image>();
        private void sotrudPanel_Paint(object sender, PaintEventArgs e)
        { Change.ChangeButtonColorToTransparent(iconButton38); Change.ChangeButtonColorToTransparent(iconButton37); Change.ChangeButtonColorToTransparent(iconButton36); Change.ChangeButtonColorToTransparent(clearfiltericonButton2); }
        public void TableSot(string filterText = null , string dolfilterComboBox = null, string statusfilterComboBox = null)
        {
            DataBase db = new DataBase();
            db.openConnection();
            try
            {
                сотрудникиDataGridView.DataSource = null; сотрудникиDataGridView.Rows.Clear(); сотрудникиDataGridView.Columns.Clear();
                сотрудникиDataGridView.Columns.Add("ID_Сотрудника", "Номер"); сотрудникиDataGridView.Columns.Add("Логин", "Логин"); сотрудникиDataGridView.Columns.Add("Фамилия", "Фамилия");
                сотрудникиDataGridView.Columns.Add("Имя", "Имя"); сотрудникиDataGridView.Columns.Add("Отчество", "Отчество"); сотрудникиDataGridView.Columns.Add("Должность", "Должность");
                сотрудникиDataGridView.Columns.Add("Номер_телефона", "Телефон"); сотрудникиDataGridView.Columns.Add("График_работы", "График работы"); сотрудникиDataGridView.Columns.Add("Номер_кабинета", "№ каб.");
                сотрудникиDataGridView.Columns.Add("Статус", "Статус"); сотрудникиDataGridView.Columns.Add("Фото", "Фото"); сотрудникиDataGridView.Columns.Add("Стаж", "Стаж"); сотрудникиDataGridView.Columns.Add("Категория", "Категория"); сотрудникиDataGridView.Columns.Add("Дата_начала_работы", "Начало работы");

                сотрудникиDataGridView.Columns["Фото"].Visible = false; сотрудникиDataGridView.Columns["Логин"].Visible = false; сотрудникиDataGridView.Columns["Номер_кабинета"].Visible = false; сотрудникиDataGridView.Columns["Номер_телефона"].Visible = false;

                SqlCommand sqlCommand = new SqlCommand("SELECT Сотрудники.ID_Сотрудника, Пользователи.Логин, Сотрудники.Фамилия, Сотрудники.Имя, Сотрудники.Отчество, Сотрудники.Должность, Сотрудники.Номер_телефона, Сотрудники.График_работы, Сотрудники.Номер_кабинета, Сотрудники.Статус, Сотрудники.Фото, Сотрудники.Стаж, Сотрудники.Категория, Сотрудники.Дата_начала_работы FROM Сотрудники INNER JOIN Пользователи ON Сотрудники.ID_Пользователя = Пользователи.ID_Пользователя WHERE Пользователи.Роль = 'Врач'", db.GetConnection());
                if (!string.IsNullOrEmpty(filterText))
                {
                    string searchExpression = "";
                    string[] searchTerms = filterText.Trim().Split(' ');
                    for (int i = 0; i < searchTerms.Length; i++)
                    {
                        string cleanedSearchTerm = Regex.Replace(searchTerms[i], "[^a-zA-Z0-9\']", "");
                        searchExpression += $"(Фамилия LIKE '%{searchTerms[i]}%' OR Имя LIKE '%{searchTerms[i]}%' OR Отчество LIKE '%{searchTerms[i]}%')";
                        сотрудникиDataGridView.Columns["ID_Сотрудника"].Visible = false; сотрудникиDataGridView.Columns["Фото"].Visible = false; сотрудникиDataGridView.Columns["Логин"].Visible = false; сотрудникиDataGridView.Columns["Номер_кабинета"].Visible = false; сотрудникиDataGridView.Columns["Стаж"].Visible = false; сотрудникиDataGridView.Columns["Категория"].Visible = false; сотрудникиDataGridView.Columns["Дата_начала_работы"].Visible = false;
                        if (i < searchTerms.Length - 1)
                        {searchExpression += " AND ";}
                    }
                    sqlCommand.CommandText += " AND " + searchExpression;
                }         
                if (!string.IsNullOrEmpty(dolfilterComboBox))    // Применить фильтр по должности
                {
                    string positionExpression = $"(Должность LIKE '%{dolfilterComboBox}%')";
                    sqlCommand.CommandText += " AND " + positionExpression;
                    сотрудникиDataGridView.Columns["ID_Сотрудника"].Visible = false; сотрудникиDataGridView.Columns["Фото"].Visible = false; сотрудникиDataGridView.Columns["Логин"].Visible = false; сотрудникиDataGridView.Columns["Номер_кабинета"].Visible = false; сотрудникиDataGridView.Columns["Стаж"].Visible = false; сотрудникиDataGridView.Columns["Категория"].Visible = false; сотрудникиDataGridView.Columns["Дата_начала_работы"].Visible = false;
                }
                if (!string.IsNullOrEmpty(statusfilterComboBox))    // Применить фильтр по должности
                {
                    string positionEx = $"(Статус LIKE '%{statusfilterComboBox}%')";
                    sqlCommand.CommandText += " AND " + positionEx;
                    сотрудникиDataGridView.Columns["ID_Сотрудника"].Visible = false; сотрудникиDataGridView.Columns["Фото"].Visible = false; сотрудникиDataGridView.Columns["Логин"].Visible = false; сотрудникиDataGridView.Columns["Номер_кабинета"].Visible = false; сотрудникиDataGridView.Columns["Стаж"].Visible = false; сотрудникиDataGridView.Columns["Категория"].Visible = false; сотрудникиDataGridView.Columns["Дата_начала_работы"].Visible = false;
                }
                DateTime currentDate = DateTime.Today;
                string currentDayOfWeek = currentDate.ToString("ddd", CultureInfo.GetCultureInfo("ru-RU"));
                // Читаем данные из SQL и обрабатываем их
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    string[] fields = new string[14];
                    for (int i = 0; i < 14; i++)
                    {fields[i] = sqlDataReader[i].ToString();}
                    // Проверяем наличие графика работы и соответствие текущему дню недели
                    if (sqlDataReader[7] != DBNull.Value)
                    {
                        string workSchedule = sqlDataReader[7].ToString();
                        if (workSchedule.ToLower().Contains(currentDayOfWeek.ToLower()))
                        {
                            // Если у сотрудника есть фото, добавляем его в таблицу
                            if (sqlDataReader[10] != DBNull.Value)
                            {
                                byte[] photoBytes = (byte[])sqlDataReader[10];
                                System.Drawing.Image photoImage = ByteArrayToImage(photoBytes);

                                fields = fields.Concat(new string[] { "" }).ToArray();
                                int rowIndex = сотрудникиDataGridView.Rows.Add(fields);

                                DataGridViewImageCell cell = new DataGridViewImageCell();
                                cell.Value = photoImage;
                                сотрудникиDataGridView.Rows[rowIndex].Cells["Фото"] = cell;
                                // Устанавливаем видимость столбцов
                                сотрудникиDataGridView.Columns["ID_Сотрудника"].Visible = false; сотрудникиDataGridView.Columns["Фото"].Visible = false;   сотрудникиDataGridView.Columns["Логин"].Visible = false;    сотрудникиDataGridView.Columns["Номер_кабинета"].Visible = false;   сотрудникиDataGridView.Columns["Стаж"].Visible = false; сотрудникиDataGridView.Columns["Категория"].Visible = false; сотрудникиDataGridView.Columns["Дата_начала_работы"].Visible = false;
                            }
                            else{int rowIndex = сотрудникиDataGridView.Rows.Add(fields);}
                        }
                    }
                }
                sqlDataReader.Close();
            }
            finally { db.closeConnection(); } 
        }  
        private void guna2TextBox1_TextChanged_2(object sender, EventArgs e)     
        { TableSot(poiskguna2TextBox.Text); }
        private void iconButton36_Click(object sender, EventArgs e)
        { poiskguna2TextBox.Text = ""; }
        private void guna2CustomGradientPanel7_Paint(object sender, PaintEventArgs e)
        {  }
        private void panel9_Paint(object sender, PaintEventArgs e)
        { Change.ChangeButtonColorToTransparent(lasticonButton); Change.ChangeButtonColorToTransparent(nexticonButton); }
        private void iconButton38_Click(object sender, EventArgs e)
        {
            infoPanel.Visible = true; guna2CustomGradientPanel8.Visible = false; guna2CustomGradientPanel7.Visible = false; 
            if (сотрудникиDataGridView.SelectedRows.Count > 0)
            { DataGridViewRow selectedRow = сотрудникиDataGridView.SelectedRows[0]; DisplayDataFromRow(selectedRow);}
        }
        private void iconButton17_Click(object sender, EventArgs e)
        {
            IconButton activeButton = (IconButton)sender;// установка цвета кнопки
            change.SetButtonColors(activeButton, change.ActiveBackGroundColor, change.ActiveForeGroundColor); change.SetButtonColors(lasticonButton, change.DefBackGroundColor, change.DefForeGroundColor);
            if (сотрудникиDataGridView.SelectedRows.Count > 0)
            { int selectedRowIndex = сотрудникиDataGridView.SelectedRows[0].Index; int rowCount = сотрудникиDataGridView.Rows.Count;
                if (selectedRowIndex < rowCount - 1) { сотрудникиDataGridView.Rows[selectedRowIndex].Selected = false; сотрудникиDataGridView.Rows[selectedRowIndex + 1].Selected = true; DisplayDataFromRow(сотрудникиDataGridView.Rows[selectedRowIndex + 1]); } // Перемещение к следующей записи              
                else { сотрудникиDataGridView.Rows[selectedRowIndex].Selected = false; сотрудникиDataGridView.Rows[0].Selected = true; DisplayDataFromRow(сотрудникиDataGridView.Rows[0]); } // Перемещение к началу таблицы
            }          
        }         
        private void iconButton30_Click(object sender, EventArgs e)
        {
            IconButton activeButton = (IconButton)sender;// установка цвета кнопки
            change.SetButtonColors(activeButton, change.ActiveBackGroundColor, change.ActiveForeGroundColor); change.SetButtonColors(nexticonButton, change.DefBackGroundColor, change.DefForeGroundColor);
            if (сотрудникиDataGridView.SelectedRows.Count > 0)
            {int selectedRowIndex = сотрудникиDataGridView.SelectedRows[0].Index; if (selectedRowIndex > 0){сотрудникиDataGridView.Rows[selectedRowIndex].Selected = false; сотрудникиDataGridView.Rows[selectedRowIndex - 1].Selected = true;DisplayDataFromRow(сотрудникиDataGridView.Rows[selectedRowIndex - 1]); }// Перемещение к предыдущей записи
            else { сотрудникиDataGridView.Rows[selectedRowIndex].Selected = false;сотрудникиDataGridView.Rows[сотрудникиDataGridView.Rows.Count - 1].Selected = true;DisplayDataFromRow(сотрудникиDataGridView.Rows[сотрудникиDataGridView.Rows.Count - 1]); }   // Перемещение к концу таблицы
            }
        }
        private void DisplayDataFromRow(DataGridViewRow row)
        {
            DataGridViewRow selectedRow = сотрудникиDataGridView.SelectedRows[0];
            if (selectedRow.Cells["Фото"].Value is Bitmap)
            {
                using (MemoryStream ms = new MemoryStream())
                {                   
                    ((Bitmap)selectedRow.Cells["Фото"].Value).Save(ms, ImageFormat.Png);
                    фотоPictureBox.Image = System.Drawing.Image.FromStream(ms);
                    byte[] photoData = ms.ToArray();
                    infoFIO2TextBox.Text = $"{selectedRow.Cells["Фамилия"].Value?.ToString() ?? ""} {selectedRow.Cells["Имя"].Value?.ToString() ?? ""} {selectedRow.Cells["Отчество"].Value?.ToString() ?? ""}"; infoDOl2TextBox.Text = selectedRow.Cells["Должность"].Value?.ToString() ?? ""; infokat2TextBox.Text = selectedRow.Cells["Категория"].Value?.ToString() ?? ""; infkabchguna2TextBox.Text = selectedRow.Cells["Номер_кабинета"].Value?.ToString() ?? "";
                    infdoSta2TextBox.Text = selectedRow.Cells["Стаж"].Value?.ToString() ?? ""; infostatchguna2TextBox.Text = selectedRow.Cells["Статус"].Value?.ToString() ?? ""; inftellchguna2TextBox.Text = selectedRow.Cells["Номер_телефона"].Value?.ToString() ?? ""; infgraffchguna2TextBox.Text = selectedRow.Cells["График_работы"].Value?.ToString() ?? "";
                }
            }
        }
        private void iconButton37_Click(object sender, EventArgs e)
        {        
            if (filterguna2Panel2.Visible)
            { change.SetButtonColors(iconButton37, change.DefBackGroundColor, change.DefForeGroundColor); filterguna2Panel2.Visible = false; dolfilterguna2ComboBox2.SelectedIndex = -1; statusfilterguna2ComboBox2.SelectedIndex = -1; iconButton38.Location = new Point(396, 12); iconButton37.Location = new Point(433, 12); iconButton37.IconChar = IconChar.Filter; TableSot();}
            else
            { change.SetButtonColors(iconButton37, change.ActiveBackGroundColor, change.ActiveForeGroundColor); filterguna2Panel2.Visible = true; iconButton37.Location = new Point(235, 12); iconButton38.Location = new Point(200, 12); iconButton37.IconChar = IconChar.FilterCircleXmark; }
        }
        private void exitPictureBox_Click(object sender, EventArgs e)
        {infoPanel.Visible = false; guna2CustomGradientPanel8.Visible = true; guna2CustomGradientPanel7.Visible = true; change.SetButtonColors(lasticonButton, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(nexticonButton, change.DefBackGroundColor, change.DefForeGroundColor); }
        private void infostatchguna2TextBox_TextChanged(object sender, EventArgs e)
        {if (infostatchguna2TextBox.Text == "Занят"){ infostatchguna2TextBox.ForeColor = Color.Red;}else if (infostatchguna2TextBox.Text == "Свободен"){infostatchguna2TextBox.ForeColor = Color.Green;} }
        private void iconButton17_Click_1(object sender, EventArgs e)
        {dolfilterguna2ComboBox2.SelectedIndex = -1; statusfilterguna2ComboBox2.SelectedIndex = -1; TableSot(); }
        private void dolfilterguna2ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
         string dolfilter = dolfilterguna2ComboBox2.SelectedItem?.ToString() ?? ""; // Получение выбранного значения для фильтрации по должности
         string statusfilter = statusfilterguna2ComboBox2.SelectedItem?.ToString() ?? ""; // Получение выбранного значения для фильтрации по статусу
         if (!string.IsNullOrEmpty(dolfilter) || !string.IsNullOrEmpty(statusfilter)) { TableSot(dolfilterComboBox: dolfilter, statusfilterComboBox: statusfilter);  }
        }
        private void statusfilterguna2ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
         string dolfilter = dolfilterguna2ComboBox2.SelectedItem?.ToString() ?? ""; // Получение выбранного значения для фильтрации по должности
         string statusfilter = statusfilterguna2ComboBox2.SelectedItem?.ToString() ?? ""; // Получение выбранного значения для фильтрации по статусу
         if (!string.IsNullOrEmpty(dolfilter) || !string.IsNullOrEmpty(statusfilter)) { TableSot(dolfilterComboBox: dolfilter, statusfilterComboBox: statusfilter); }
        }
        //---------------------------------------Мой аккаунт     
        private void LoadPersonalData()
        {
            SurnameTextBox.Text = String.Format("{0} {1} {2}", PersonalArea.FirstName, PersonalArea.LastName, PersonalArea.FatherName);
            doltextbox2.Text = PersonalArea.Dol; CategoryTextBox.Text = PersonalArea.Category;  exTextBox.Text = PersonalArea.Ex; numTextBox.Text = PersonalArea.NumberTel;
            loginTextBox.Text = PersonalArea.Login;
            PersonalArea personalArea = new PersonalArea();
            if (personalArea.SetPersonalData(PersonalArea.Login, PersonalArea.Password))
            {
                if (personalArea.Foto != null && personalArea.Foto.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream(personalArea.Foto))
                    { фотоpictureBox2.Image = System.Drawing.Image.FromStream(ms); }
                }
            }
        }
        private void iconButton4_Click(object sender, EventArgs e)
        {
            AboutMePanel.Visible = true; applicationsPanel.Visible = false; klientPanel.Visible = false; uslugiPanel.Visible = false; sotrudPanel.Visible = false;
            IconButton activeButton = (IconButton)sender;// установка цвета кнопки
            change.SetButtonColors(activeButton, change.ActiveBackGroundColor, change.ActiveForeGroundColor); change.SetButtonColors(iconButton6, change.DefBackGroundColor, change.DefForeGroundColor);
           
            change.SetButtonColors(iconButton13, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton4, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton1, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton11, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton2, change.DefBackGroundColor, change.DefForeGroundColor);
            leftpanel1.Visible = false; leftpanel2.Visible = false; leftpanel3.Visible = false; leftpanel4.Visible = false; leftpanel5.Visible = false;
            LoadPersonalData();
        }  
        private void guna2Button2_Click_2(object sender, EventArgs e)
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
                    command.Parameters.AddWithValue("@newPassword", newpassword);
                    command.Parameters.AddWithValue("@oldPassword", PersonalArea.Password);

                    int rowsUpdated = command.ExecuteNonQuery();

                    if (rowsUpdated > 0)
                    { this.Alert("Изменение пароля", "Старый пароль был успешно изменен на новый.", false); passwordPanel.Visible = false;                     }
                    else
                    { this.Alert("Ошибка при изменении пароля", "Недопустимые символы при изменении пароля.", false); }
                    connection.Close();
                }
            }
            else { this.Alert("Изменение пароля", "Действие было отменено.", false); }
        }
        private void guna2CirclePictureBox2_Click(object sender, EventArgs e)
        { AboutMePanel.Visible = false; change.SetButtonColors(settingButton, change.DefBackGroundColor, change.DefForeGroundColor); }
        private void iconButton17_Click_2(object sender, EventArgs e)
        {
            DialogResult dialogResult = DialogForm.Show("Изменение пароля", "Вы точно хотите отменить изменение пароля?", false);
            if (dialogResult == DialogResult.Yes)              
            { passwordPanel.Visible = false; newpasswordTextBox.Text = ""; newpasswordTextBox2.Text = ""; oldpasswordTextBox.Text = ""; guna2CheckBox2.Checked = false; }
            else { return; }
        }
        private void guna2CheckBox2_CheckedChanged(object sender, EventArgs e)
        { if (guna2CheckBox2.Checked) { newpasswordTextBox2.UseSystemPasswordChar = false; } else { newpasswordTextBox2.UseSystemPasswordChar = true; } }
        private void guna2Button3_Click(object sender, EventArgs e)
        { oldpasswordTextBox.Text = ""; passwordPanel.Visible = true; newpasswordTextBox.Text = ""; newpasswordTextBox2.Text = ""; }

        private void iconButton17_Click_3(object sender, EventArgs e)
        { string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Договоры"); Process.Start(folder); } // Открытие папки Договоры      
    }
}


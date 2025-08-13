using FontAwesome.Sharp;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Forms;
using Стоматология.Classes;
using Стоматология.MessageBoxForms;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Стоматология.Forms
{
    public partial class AdminForm : Form
    {
        DataBase db = new DataBase();
        Change change = new Change();
        private Point lastLocation; private bool mouseDown; // Создание переменных для перемещения формы
        string currentlog = PersonalArea.IdUser.ToString();
        public void Alert(string msg, string type, bool liftText)
        { FormAlert frm = new FormAlert(); frm.showAlert(msg, type, liftText); }// Создание переменных для сообщения
        public AdminForm()
        {
            InitializeComponent();
        }

        private void AdminForm3_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "сП4DataSet.Договоры". При необходимости она может быть перемещена или удалена.
            this.договорыTableAdapter.Fill(this.сП4DataSet.Договоры);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "сП4DataSet.Пользователи". При необходимости она может быть перемещена или удалена.
            this.пользователиTableAdapter.Fill(this.сП4DataSet.Пользователи);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "сП4DataSet.Сотрудники". При необходимости она может быть перемещена или удалена.
            this.сотрудникиTableAdapter.Fill(this.сП4DataSet.Сотрудники);
            DateTime currentDateTime = DateTime.Now; string dateString = currentDateTime.ToString("dd/MM/yyyy") + " г."; datelable.Text = dateString; loginlable.Text = PersonalArea.Login;
            Change.ChangeButtonColorToTransparent(exitButton); Change.ChangeButtonColorToTransparent(settingButton); Change.ChangeButtonColorToTransparent(iconButton2); Change.ChangeButtonColorToTransparent(iconButton3); Change.ChangeButtonColorToTransparent(iconButton7); Change.ChangeButtonColorToRed(iconButton8);
        }
        private void LoadUsersData()
        {
            using (SqlConnection connection = new SqlConnection(db.getConnectionString()))
            {
                string queryUsers = "SELECT * FROM Пользователи WHERE Роль IN ('Врач', 'Регистратор')";
                SqlDataAdapter adapterUsers = new SqlDataAdapter(queryUsers, connection);
                DataTable dataTableUsers = new DataTable();
                adapterUsers.Fill(dataTableUsers);
                // Применение фильтра к исходному источнику данных
                пользователиBindingSource.DataSource = dataTableUsers;
                пользователиBindingSource.Filter = $"[Логин] LIKE '%{searchguna2TextBox.Text}%'"; // Применение фильтра
            }
        }
        private void exitButton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = DialogForm.Show("Подтверждение выхода", "Вы уверены, что хотите выйти из аккаунта?\nНесохранённые данные будут потеряны!", true);
            if (dialogResult == DialogResult.Yes)
            { this.Hide(); Form exit = new MenuAutchForm(); exit.Show(); ; }
            else { return; }
        }
        private void iconButton1_Click(object sender, EventArgs e)
        {
             menuPanel.Visible = true; sotrudPanel.Visible = false; userPanel.Visible = false; AboutMePanel.Visible = false; CreatorChartPanel.Visible = false;
            IconButton activeButton = (IconButton)sender;// установка цвета кнопки
            change.SetButtonColors(activeButton, change.ActiveBackGroundColor, change.ActiveForeGroundColor);
            leftpanel1.Visible = true;
            dolPanel.Visible = false; doc.IconChar = IconChar.NotesMedical;
            change.SetButtonColors(iconButton11, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton13, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(settingButton, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton18, change.DefBackGroundColor, change.DefForeGroundColor); // Сброс цвета кнопок
            leftpanel2.Visible = false; leftpanel3.Visible = false; leftpanel4.Visible = false;
        }
        private void iconButton11_Click(object sender, EventArgs e)
        {
            sotrudPanel.Visible = true; userPanel.Visible = false; AboutMePanel.Visible = false; menuPanel.Visible = false; CreatorChartPanel.Visible = false;
            IconButton activeButton = (IconButton)sender;// установка цвета кнопки
            change.SetButtonColors(activeButton, change.ActiveBackGroundColor, change.ActiveForeGroundColor);
            leftpanel1.Visible = false;
            dolPanel.Visible = false; doc.IconChar = IconChar.NotesMedical;
            change.SetButtonColors(iconButton1, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton13, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton18, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(settingButton, change.DefBackGroundColor, change.DefForeGroundColor);// Сброс цвета кнопок
            leftpanel2.Visible = true; leftpanel3.Visible = false; leftpanel4.Visible = false; TableSot();
        }
        private void iconButton13_Click(object sender, EventArgs e)
        {
            userPanel.Visible = true; sotrudPanel.Visible = false; AboutMePanel.Visible = false; menuPanel.Visible = false; CreatorChartPanel.Visible = false;
            IconButton activeButton = (IconButton)sender;// установка цвета кнопки
            change.SetButtonColors(activeButton, change.ActiveBackGroundColor, change.ActiveForeGroundColor);
            dolPanel.Visible = false; doc.IconChar = IconChar.NotesMedical;
            leftpanel1.Visible = false;
            change.SetButtonColors(iconButton1, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton11, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton18, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(settingButton, change.DefBackGroundColor, change.DefForeGroundColor); // Сброс цвета кнопок
            leftpanel2.Visible = false; leftpanel3.Visible = true; leftpanel4.Visible = false; LoadUsersData();
        }
        private void iconButton18_Click(object sender, EventArgs e)
        {
                var creatorChart = new CreatorChart();
                Dictionary<int, string> idУслуги = new Dictionary<int, string>();
                idУслуги.Add(1, "Проводниковая анестезия (дети, взрослые)"); idУслуги.Add(2, "Прием (осмотр, консультация) врача-стоматолога"); idУслуги.Add(3, "Восстановление зуба композитом");
                idУслуги.Add(4, "Установка пломбы на одном зубе"); idУслуги.Add(5, "Снятие оттиска с одной челюсти"); idУслуги.Add(6, "Удаление зуба");
                idУслуги.Add(7, "Установка брекет-системы на верхнюю челюсть"); idУслуги.Add(8, "Электрофорез лекарственных препаратов при патологии полости рта и зубов"); idУслуги.Add(9, "Приварка зуба"); idУслуги.Add(10, "Панорамическая рентгенография челюсти");

                DataTable договорыTable = GetDataFromDataGridView(договорыDataGridView2, idУслуги);
                creatorChart.ChartBar(gunaChart1, договорыTable, "Статистика по оказанию медицинских услуг.");
                dolPanel.Visible = false; doc.IconChar = IconChar.NotesMedical;image.IconChar = IconChar.Image;
                CreatorChartPanel.Visible = true; userPanel.Visible = false; sotrudPanel.Visible = false; AboutMePanel.Visible = false; menuPanel.Visible = false;
                IconButton activeButton = (IconButton)sender;// установка цвета кнопки
                panelsearch.Visible = false; doc.Location = new Point(836, 12); opendoc.Location = new Point(795, 12); image.Location = new Point(755, 12);

                change.SetButtonColors(activeButton, change.ActiveBackGroundColor, change.ActiveForeGroundColor);
                leftpanel4.Visible = true;
                change.SetButtonColors(iconButton1, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton11, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton13, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(settingButton, change.DefBackGroundColor, change.DefForeGroundColor); // Сброс цвета кнопок
                leftpanel1.Visible = false; leftpanel2.Visible = false; leftpanel3.Visible = false;                          
        }
        private DataTable GetDataFromDataGridView(Guna2DataGridView dataGridView, Dictionary<int, string> idУслуги)
        {
            DataTable dataTable = new DataTable("Договоры");
            dataTable.Columns.Add("Название", typeof(string));
            dataTable.Columns.Add("Стоимость", typeof(double));
            dataTable.Columns.Add("Дата", typeof(DateTime));

            Dictionary<int, ContractData> contractDataMap = new Dictionary<int, ContractData>();

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                int id = Convert.ToInt32(row.Cells["iDУслугиDataGridViewTextBoxColumn1"].Value);
                double стоимость = 0;
                if (row.Cells["итоговаястоимостьDataGridViewTextBoxColumn1"].Value != null)
                { стоимость = Convert.ToDouble(row.Cells["итоговаястоимостьDataGridViewTextBoxColumn1"].Value);}
                DateTime дата = DateTime.MinValue;
                if (row.Cells["датасоставленияDataGridViewTextBoxColumn1"].Value != null)
                { дата = Convert.ToDateTime(row.Cells["датасоставленияDataGridViewTextBoxColumn1"].Value);  }
                if (contractDataMap.ContainsKey(id))
                {
                    ContractData contractData = contractDataMap[id]; contractData.Стоимость += стоимость;
                    if (дата != DateTime.MinValue){contractData.Даты.Add(дата);}
                }
                else
                { string название = idУслуги.ContainsKey(id) ? idУслуги[id] : null; if (стоимость > 0 && дата != DateTime.MinValue)
                {contractDataMap.Add(id, new ContractData { Название = название, Стоимость = стоимость, Даты = new List<DateTime> { дата } });} }
            }

            foreach (var contractDataPair in contractDataMap)
            {
                string название = contractDataPair.Value.Название;
                double стоимость = contractDataPair.Value.Стоимость;
                DateTime дата = contractDataPair.Value.Даты.Max(); // Берем максимальную дату из списка
                dataTable.Rows.Add(название, стоимость, дата);
            }
            return dataTable;      
        }
        private void SaveChartToPDF(Guna.UI2.WinForms.Guna2DataGridView dataGridView)
        {
            var creatorChart = new CreatorChart();
            Dictionary<int, string> idУслуги = new Dictionary<int, string>();
            idУслуги.Add(1, "Проводниковая анестезия (дети, взрослые)"); idУслуги.Add(2, "Прием (осмотр, консультация) врача-стоматолога"); idУслуги.Add(3, "Восстановление зуба композитом");
            idУслуги.Add(4, "Установка пломбы на одном зубе"); idУслуги.Add(5, "Снятие оттиска с одной челюсти"); idУслуги.Add(6, "Удаление зуба");
            idУслуги.Add(7, "Установка брекет-системы на верхнюю челюсть"); idУслуги.Add(8, "Электрофорез лекарственных препаратов при патологии полости рта и зубов"); idУслуги.Add(9, "Приварка зуба"); idУслуги.Add(10, "Панорамическая рентгенография челюсти");
            DataTable договорыTable = GetDataFromDataGridView(dataGridView, idУслуги);
            creatorChart.ChartBar(gunaChart1, договорыTable, "Статистика по оказанию медицинских услуг.");
            using (MemoryStream ms = new MemoryStream())
            {
                // Получаем изображение из диаграммы
                System.Drawing.Bitmap chartImage = new System.Drawing.Bitmap(gunaChart1.Width, gunaChart1.Height);
                gunaChart1.DrawToBitmap(chartImage, new System.Drawing.Rectangle(0, 0, gunaChart1.Width, gunaChart1.Height));
                // Сохраняем изображение в указанной папке
                string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Договоры");
                string filePath = Path.Combine(folderPath, "Диаграмма.png");
                chartImage.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
            }
        }       
        private class ContractData
        {
            public string Название { get; set; }
            public double Стоимость { get; set; }
            public List<DateTime> Даты { get; set; }
        }
        private void iconButton8_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = DialogForm.Show("Подтверждение выхода", "Вы уверены, что хотите закрыть программу?\nНесохранённые данные будут потеряны!", true);
            if (dialogResult == DialogResult.Yes)
            { Application.Exit(); }
            else
            { return; }
        }

        private void iconButton2_Click(object sender, EventArgs e)
        { this.WindowState = FormWindowState.Minimized; this.Alert("Фоновый режим", "Приложение находится в фоновом режиме!", false); }//свернуть форму
        private void iconButton3_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized) { this.WindowState = FormWindowState.Normal; this.StartPosition = FormStartPosition.CenterScreen; }
            else { this.MaximumSize = Screen.PrimaryScreen.WorkingArea.Size; this.WindowState = FormWindowState.Maximized; }
            iconButton7.Visible = true;
        }//полный и неполный экран

        private void iconButton7_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized) { this.WindowState = FormWindowState.Normal; this.StartPosition = FormStartPosition.CenterScreen; }
            else { this.MaximumSize = Screen.PrimaryScreen.WorkingArea.Size; this.WindowState = FormWindowState.Maximized; }
            iconButton7.Visible = false;
        }//полный и неполный экран

        private void guna2Panel4_MouseDown(object sender, MouseEventArgs e)
        { mouseDown = true; lastLocation = e.Location; }
        private void guna2Panel4_MouseMove(object sender, MouseEventArgs e)
        { if (mouseDown) { this.Location = new Point((this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y); this.Update(); } }
        private void guna2Panel4_MouseUp(object sender, MouseEventArgs e)
        { mouseDown = false; }

        private void guna2Panel4_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized) { this.WindowState = FormWindowState.Normal; this.StartPosition = FormStartPosition.CenterScreen; iconButton7.Visible = false; }
            else { this.MaximumSize = Screen.PrimaryScreen.WorkingArea.Size; this.WindowState = FormWindowState.Maximized; iconButton7.Visible = true; }
        }

        //---------------------------------------------------------------------------------Таблица Сотрудники
        private void sotrudPanel_Paint(object sender, PaintEventArgs e)
        {
            Change.ChangeButtonColorToTransparent(clearButton); Change.ChangeButtonColorToTransparent(addsotButton); Change.ChangeButtonColorToTransparent(delsotButton); Change.ChangeButtonColorToTransparent(updatesotButton);
            LoadData2();
        }
        private void guna2TextBox6_KeyPress(object sender, KeyPressEventArgs e)
        { if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) { e.Handled = true; } }// отмена ввода символа, если он не является цифрой или управляющим символом
        private void guna2TextBox2_KeyPress(object sender, KeyPressEventArgs e)
        { if (!char.IsControl(e.KeyChar) && char.IsDigit(e.KeyChar)) { e.Handled = true; } }// отмена ввода символа, если он является цифрой 
        private void guna2TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        { if (!char.IsControl(e.KeyChar) && char.IsDigit(e.KeyChar)) { e.Handled = true; } }// отмена ввода символа, если он является цифрой 
        private void guna2TextBox5_KeyPress(object sender, KeyPressEventArgs e)
        { if (!char.IsControl(e.KeyChar) && char.IsDigit(e.KeyChar)) { e.Handled = true; } }// отмена ввода символа, если он является цифрой
        private void famTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        { if (!char.IsControl(e.KeyChar) && char.IsDigit(e.KeyChar)) { e.Handled = true; } }// отмена ввода символа, если он является цифрой 
        private void nameTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        { if (!char.IsControl(e.KeyChar) && char.IsDigit(e.KeyChar)) { e.Handled = true; } }// отмена ввода символа, если он является цифрой 
        private void otchTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        { if (!char.IsControl(e.KeyChar) && char.IsDigit(e.KeyChar)) { e.Handled = true; } }// отмена ввода символа, если он является цифрой 
        private void kabTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        { if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) { e.Handled = true; } }// отмена ввода символа, если он не является цифрой или управляющим символом
        class ImageUPlouder
        {
            private readonly string _connectionString; public ImageUPlouder(string connectionString)
            {
                _connectionString = connectionString;
            }
            public void Upload(PictureBox фотоPictureBox, string фамилия, string имя, string отчество, string должность, string номерТелефона, string графикРаботы, string номерКабинета, string логин, string стаж, string категория, string дата)
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Сотрудники (Фото, Фамилия, Имя, Отчество, Должность, Номер_телефона, График_работы, Номер_кабинета, ID_Пользователя, Статус, Стаж, Категория, Дата_начала_работы) VALUES (@image, @фамилия, @имя, @отчество, @должность, @номерТелефона, @графикРаботы, @номерКабинета, @логин, 'Свободен', @стаж, @категория, @дата)";
                    var image = new Bitmap(фотоPictureBox.Image);
                    using (var memoryStream = new MemoryStream())
                    {
                        image.Save(memoryStream, ImageFormat.Jpeg); memoryStream.Position = 0;
                        var sqlParameter = new SqlParameter("@image", SqlDbType.VarBinary, (int)memoryStream.Length); sqlParameter.Value = memoryStream.ToArray();
                        command.Parameters.Add(sqlParameter); command.Parameters.AddWithValue("@фамилия", фамилия);
                        command.Parameters.AddWithValue("@имя", имя); command.Parameters.AddWithValue("@отчество", отчество);
                        command.Parameters.AddWithValue("@должность", должность); command.Parameters.AddWithValue("@номерТелефона", номерТелефона);
                        command.Parameters.AddWithValue("@графикРаботы", графикРаботы); command.Parameters.AddWithValue("@номерКабинета", номерКабинета);
                        command.Parameters.AddWithValue("@стаж", стаж); command.Parameters.AddWithValue("@категория", категория); command.Parameters.AddWithValue("@дата", дата);
                        command.Parameters.AddWithValue("@логин", логин);
                    }
                    connection.Open(); command.ExecuteNonQuery();
                }
            }
        }

        private System.Drawing.Image ByteArrayToImage(byte[] byteArray)
        { MemoryStream ms = new MemoryStream(byteArray); System.Drawing.Image image = System.Drawing.Image.FromStream(ms); return image; }
        public List<System.Drawing.Image> photoImages = new List<System.Drawing.Image>();
        public void TableSot(string filterText = null)
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
                SqlCommand sqlCommand = new SqlCommand("SELECT Сотрудники.ID_Сотрудника, Пользователи.Логин, Сотрудники.Фамилия, Сотрудники.Имя, Сотрудники.Отчество, Сотрудники.Должность, Сотрудники.Номер_телефона, Сотрудники.График_работы, Сотрудники.Номер_кабинета, Сотрудники.Статус, Сотрудники.Фото, Сотрудники.Стаж, Сотрудники.Категория, Сотрудники.Дата_начала_работы FROM Сотрудники INNER JOIN Пользователи ON Сотрудники.ID_Пользователя = Пользователи.ID_Пользователя WHERE (Пользователи.ID_Пользователя = @UserId OR (Пользователи.Роль = 'Врач' OR Пользователи.Роль = 'Регистратор'))", db.GetConnection());
                sqlCommand.Parameters.AddWithValue("@UserId", currentlog);
                if (!string.IsNullOrEmpty(filterText))
                {
                    string searchExpression = "";
                    string[] searchTerms = filterText.Trim().Split(' ');
                    for (int i = 0; i < searchTerms.Length; i++)
                    {
                        string cleanedSearchTerm = searchTerms[i].Replace("*", "").Replace("`", "");
                        searchExpression += $"(Сотрудники.Фамилия LIKE '%{cleanedSearchTerm}%' OR Сотрудники.Имя LIKE '%{cleanedSearchTerm}%' OR Сотрудники.Отчество LIKE '%{cleanedSearchTerm}%')";
                       
                        if (i < searchTerms.Length - 1)
                        { searchExpression += " AND "; }
                    }
                    sqlCommand.CommandText += " AND (" + searchExpression + ")";
                    сотрудникиDataGridView.Columns["ID_Сотрудника"].Visible = false; сотрудникиDataGridView.Columns["Категория"].Visible = false; сотрудникиDataGridView.Columns["Стаж"].Visible = false; сотрудникиDataGridView.Columns["Дата_начала_работы"].Visible = false; сотрудникиDataGridView.Columns["Номер_кабинета"].Visible = false; сотрудникиDataGridView.Columns["Фото"].Visible = false; сотрудникиDataGridView.Columns["Статус"].Visible = false; сотрудникиDataGridView.Columns["График_работы"].Visible = false;
                }
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    string[] fields = new string[14];
                    for (int i = 0; i < 14; i++)
                    {
                        fields[i] = sqlDataReader[i].ToString();
                    }

                    if (sqlDataReader[10] != DBNull.Value) // Проверка на DBNull
                    {
                        byte[] photoBytes = (byte[])sqlDataReader[10];
                        System.Drawing.Image photoImage = ByteArrayToImage(photoBytes);

                        fields = fields.Concat(new string[] { "" }).ToArray();
                        int rowIndex = сотрудникиDataGridView.Rows.Add(fields);
                  
                        DataGridViewImageCell cell = new DataGridViewImageCell();
                        cell.Value = photoImage;
                        сотрудникиDataGridView.Columns["ID_Сотрудника"].Visible = false; сотрудникиDataGridView.Rows[rowIndex].Cells["Фото"] = cell;
                        сотрудникиDataGridView.Columns["Категория"].Visible = false; сотрудникиDataGridView.Columns["Стаж"].Visible = false; сотрудникиDataGridView.Columns["Дата_начала_работы"].Visible = false; сотрудникиDataGridView.Columns["Номер_кабинета"].Visible = false; сотрудникиDataGridView.Columns["Фото"].Visible = false; сотрудникиDataGridView.Columns["Статус"].Visible = false; сотрудникиDataGridView.Columns["График_работы"].Visible = false;
                    }
                    else
                    {
                        // Если значение равно DBNull, можно установить пустое изображение или другой заглушку
                        fields = fields.Concat(new string[] { "" }).ToArray();
                        int rowIndex = сотрудникиDataGridView.Rows.Add(fields);
                    }
                }
                sqlDataReader.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { db.closeConnection(); }
        }

        private void addsotButton_Click(object sender, EventArgs e)
        { if (sotrudGroupBox.Visible) { this.Alert("Предупреждение при добавлении", "Окно для добавления нового сотрудника уже открыто!", false); } else { IconButton activeButton = (IconButton)sender; change.SetButtonColors(activeButton, change.ActiveBackGroundColor, change.ActiveForeGroundColor); сотрудникиDataGridView.Height -= 320; sotrudGroupBox.Visible = true; фотоPictureBox2.Image = null; guna2TextBox2.Text = ""; guna2TextBox1.Text = ""; guna2TextBox5.Text = ""; guna2ComboBox3.SelectedIndex = -1; maskedTextBox1.Text = ""; guna2ComboBox4.SelectedIndex = -1; guna2TextBox6.Text = ""; loginCB.SelectedIndex = -1; } }

        private void delsotButton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = DialogForm.Show("Подтверждение удаления", "Вы уверены, что хотите удалить выбранную запись?", false);
            if (dialogResult == DialogResult.Yes)
            {
                List<int> idsToDelete = new List<int>();
                foreach (DataGridViewRow row in сотрудникиDataGridView.SelectedRows)
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
                        // Добавляем проверку, что сотрудника еще нет в таблице "Заявки"
                        SqlCommand checkQuery = new SqlCommand($"SELECT COUNT(*) FROM Заявки WHERE ID_Сотрудника IN ({ids})");
                        checkQuery.Connection = conn;
                        int count = Convert.ToInt32(checkQuery.ExecuteScalar());
                        if (count > 0)
                        { this.Alert("Удаление в таблице ", "Нельзя удалить сотрудника, у которого есть записи на прием!", false); }
                        else
                        {
                            SqlCommand delete = new SqlCommand($"delete from Сотрудники where ID_Сотрудника IN ({ids})");
                            delete.Connection = conn;
                            delete.ExecuteNonQuery();
                            this.Alert("Удаление в таблице", "Выбранная запись была успешно удалена!", false);
                            string query = "SELECT * FROM Сотрудники";
                            DataTable dt = db.getData(query);
                            сотрудникиDataGridView.DataSource = dt;
                        }
                    }
                }
            }
            else { return; }
            TableSot();
        }

        private void selecticon_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image files (*.jpeg, *.jpg, *.png) | *.jpeg; *.jpg; *.png";
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    фотоPictureBox2.Image = System.Drawing.Image.FromFile(openFileDialog.FileName);
                }
            }
        }
        private void closeButton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = DialogForm.Show("Подтверждение выхода при редактировании", "Вы уверены, что хотите отменить добавление нового\nсотрудника? Несохранённые данные будут потеряны!", true);
            if (dialogResult == DialogResult.Yes)           
            { change.SetButtonColors(addsotButton, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton14, change.DefBackGroundColor, change.DefForeGroundColor); фотоPictureBox2.Image = null; guna2TextBox2.Text = ""; guna2TextBox1.Text = ""; guna2TextBox5.Text = ""; guna2ComboBox3.SelectedIndex = -1; maskedTextBox1.Text = ""; guna2ComboBox4.SelectedIndex = -1; guna2TextBox6.Text = ""; loginCB.SelectedIndex = -1; sotrudGroupBox.Visible = false; сотрудникиDataGridView.Height += 320; katComboBox.SelectedIndex = -1; staComboBox.SelectedIndex = -1; DateTimePicker.Text = ""; }
            else { return; }
        }
        private void clearButton2_Click(object sender, EventArgs e)
        { katComboBox.SelectedIndex = -1; staComboBox.SelectedIndex = -1; DateTimePicker.Text = ""; фотоPictureBox2.Image = null; guna2TextBox2.Text = ""; guna2TextBox1.Text = ""; guna2TextBox5.Text = ""; guna2ComboBox3.SelectedIndex = -1; maskedTextBox1.Text = ""; guna2ComboBox4.SelectedIndex = -1; guna2TextBox6.Text = ""; loginCB.SelectedIndex = -1; }
        public void LoadData2()
        {
            DataBase db = new DataBase();
            db.openConnection();
            try
            {               
                SqlCommand sqlCommand = new SqlCommand("SELECT ID_Пользователя, Логин FROM Пользователи WHERE Роль IN ('Врач', 'Регистратор')", db.GetConnection());  // Запрос для выборки логинов из таблицы Пользователи.
                DataTable dataTable = new DataTable();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlDataAdapter.Fill(dataTable);
                loginCB.DisplayMember = "Логин"; loginCB.ValueMember = "ID_Пользователя"; loginCB.DataSource = dataTable;
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
            finally { db.closeConnection(); }
        }
        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        { Change.ChangeButtonColorToTransparent(clearButton); Change.ChangeButtonColorToTransparent(selecticon); Change.ChangeButtonColorToTransparent(clearButton2); Change.ChangeButtonColorToDodgerBlue(closeButton); }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(guna2TextBox1.Text) || string.IsNullOrWhiteSpace(guna2TextBox5.Text) || string.IsNullOrWhiteSpace(guna2ComboBox3.Text) || maskedTextBox1.MaskCompleted == false || string.IsNullOrWhiteSpace(guna2ComboBox4.Text) || string.IsNullOrWhiteSpace(guna2TextBox6.Text) || string.IsNullOrWhiteSpace(guna2TextBox2.Text) || loginCB.SelectedItem == null || DateTimePicker.Value == null || string.IsNullOrWhiteSpace(staComboBox.Text) || string.IsNullOrWhiteSpace(katComboBox.Text))
            { this.Alert("Ошибка при добавлении", "Пожалуйста, заполните все поля со звёздочкой!", false); }
            else if (фотоPictureBox2.Image == null)
            { this.Alert("Ошибка при добавлении", "Обязательно нужно добавить фото сотрудника! ", false); }
            else if (db.CheckEmployee(guna2TextBox2.Text, guna2TextBox1.Text, guna2TextBox5.Text))
            { this.Alert("Ошибка при добавлении", "Такой сотрудник уже существует в таблице!", false); }
            else if (db.CheckEmployee2(Convert.ToInt32(loginCB.SelectedValue)))
            { this.Alert("Ошибка при добавлении", "Один из сотрудников уже использует такой логин!", false); }
            else
            {
                var connectionClass = db.getConnectionString();
                var imageUploader = new ImageUPlouder(connectionClass);
                var selectedValue = loginCB.SelectedValue.ToString(); string date = DateTimePicker.Value.ToString("yyyy-MM-dd");
                change.SetButtonColors(addsotButton, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton14, change.DefBackGroundColor, change.DefForeGroundColor); // Сброс цвета кнопок
                imageUploader.Upload(фотоPictureBox2, guna2TextBox2.Text, guna2TextBox1.Text, guna2TextBox5.Text, guna2ComboBox3.Text, maskedTextBox1.Text, guna2ComboBox4.Text, guna2TextBox6.Text, selectedValue, staComboBox.Text, katComboBox.Text, date); // Загружаем изображение и данные о сотруднике в базу данных 
                this.Alert("Добавление в таблицу", "Новый сотрудник был успешно добавлен в таблицу.", false);
                фотоPictureBox2.Image = null; guna2TextBox2.Text = ""; guna2TextBox1.Text = ""; guna2TextBox5.Text = ""; guna2ComboBox3.SelectedIndex = -1; maskedTextBox1.Text = ""; guna2ComboBox4.SelectedIndex = -1; guna2TextBox6.Text = ""; loginCB.SelectedIndex = -1; sotrudGroupBox.Visible = false; сотрудникиDataGridView.Height += 320; katComboBox.SelectedIndex = -1; staComboBox.SelectedIndex = -1; DateTimePicker.Text = "";
                TableSot();
            }
        }
        private void updatesotButton_Click_1(object sender, EventArgs e)
        {
            if (sotrudGroupBox.Visible == true)
            { this.Alert("Предупреждение при редактировании", "Закройте панель для добавления нового сотрудника перед тем,\nкак открыть редактирование записи!", true); return; }
            else if (сотрудникиDataGridView.CurrentRow != null)
            {
                updateCustomGradientPanel2.Visible = true; sotrudGroupBox.Visible = false; guna2CustomGradientPanel1.Visible = false; menuPanel3.Visible = false;
                DataGridViewRow selectedRow = сотрудникиDataGridView.CurrentRow;
                if (selectedRow.Cells["Фото"].Value is Bitmap)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        ((Bitmap)selectedRow.Cells["Фото"].Value).Save(ms, ImageFormat.Png);  // Change the image format if necessary
                        upfotoBox3.Image = System.Drawing.Image.FromStream(ms);
                        byte[] photoData = ms.ToArray();                      
                        famTextBox2.Text = selectedRow.Cells["Фамилия"].Value?.ToString() ?? "";  nameTextBox2.Text = selectedRow.Cells["Имя"].Value?.ToString() ?? ""; otchTextBox2.Text = selectedRow.Cells["Отчество"].Value?.ToString() ?? "";
                        kabTextBox.Text = selectedRow.Cells["Номер_кабинета"].Value?.ToString() ?? "";doltextBox.Text = selectedRow.Cells["Должность"].Value?.ToString() ?? ""; grafComboBox2.Text = selectedRow.Cells["График_работы"].Value?.ToString() ?? "";
                        maskedTextBox2.Text = selectedRow.Cells["Номер_телефона"].Value?.ToString() ?? ""; katComboBox2.Text = selectedRow.Cells["Категория"].Value?.ToString() ?? ""; staComboBox2.Text = selectedRow.Cells["Стаж"].Value?.ToString() ?? "";
                        DateTimePicker3.Text = selectedRow.Cells["Дата_начала_работы"].Value?.ToString() ?? ""; loginTextBox5.Text = selectedRow.Cells["Логин"].Value?.ToString() ?? "";
                    }
                }
            }
            else
            { this.Alert("Ошибка при редактировании", "Не удалось обновить данные. Запись не найдена!", false); return; }
        }
        private void dolComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        { doltextBox.Text = dolComboBox2.SelectedItem.ToString(); }
        private void guna2Button6_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = DialogForm.Show("Редактирование в таблице", "Вы уверены, что хотите изменить запись?", false);
            if (dialogResult == DialogResult.Yes)
            {
                if (string.IsNullOrWhiteSpace(famTextBox2.Text) || string.IsNullOrWhiteSpace(nameTextBox2.Text) || string.IsNullOrWhiteSpace(otchTextBox2.Text) || string.IsNullOrWhiteSpace(kabTextBox.Text) || string.IsNullOrWhiteSpace(doltextBox.Text) || string.IsNullOrWhiteSpace(grafComboBox2.Text) || string.IsNullOrWhiteSpace(maskedTextBox2.Text) || string.IsNullOrWhiteSpace(staComboBox2.Text) || string.IsNullOrWhiteSpace(katComboBox2.Text) || string.IsNullOrWhiteSpace(doltextBox.Text) || DateTimePicker3.Value == null)
                { this.Alert("Ошибка при редактировании", "Пожалуйста, заполните все поля!", false); }
                else if (upfotoBox3.Image == null)
                { this.Alert("Ошибка при редактировании", "Обязательно нужно добавить фото сотрудника!", false); }
                else
                {
                    try
                    {
                        this.Validate();
                        byte[] ConvertPictureBoxToByteArray(PictureBox pictureBox)
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                pictureBox.Image.Save(ms, ImageFormat.Jpeg); // сохраняем изображение из PictureBox в MemoryStream в формате JPEG
                                return ms.ToArray(); // возвращаем массив байтов из MemoryStream
                            }
                        }
                        string fam = famTextBox2.Text; string name = nameTextBox2.Text; string otch = otchTextBox2.Text; string kab = kabTextBox.Text; string dol = doltextBox.Text; string graf = grafComboBox2.Text; string newLogin = maskedTextBox2.Text; DateTime date = DateTimePicker3.Value; string stat = staComboBox2.Text; string kat = katComboBox2.Text;
                        using (var connection = new SqlConnection(db.getConnectionString()))
                        {
                            connection.Open();
                            byte[] photoData;
                            using (MemoryStream ms = new MemoryStream())
                            {
                                upfotoBox3.Image.Save(ms, ImageFormat.Jpeg);
                                photoData = ms.ToArray();
                            }
                            string query = "UPDATE Сотрудники SET Фото = @photo, Фамилия = @fam, Имя = @name, Отчество = @otch, Номер_кабинета = @kab, Должность = @dol, График_работы = @graf, Номер_телефона = @phone, Стаж = @sta, Категория = @kat, Дата_начала_работы = @date WHERE ID_Сотрудника = @id";
                            SqlCommand command = new SqlCommand(query, connection);
                            command.Parameters.AddWithValue("@photo", photoData); command.Parameters.AddWithValue("@fam", fam); command.Parameters.AddWithValue("@name", name);
                            command.Parameters.AddWithValue("@otch", otch); command.Parameters.AddWithValue("@kab", kab); command.Parameters.AddWithValue("@dol", dol);
                            command.Parameters.AddWithValue("@graf", graf); command.Parameters.AddWithValue("@phone", newLogin); 
                            command.Parameters.AddWithValue("@sta", stat); command.Parameters.AddWithValue("@kat", kat); command.Parameters.AddWithValue("@date", date);
                            int id = selectedEmployeeId;
                            command.Parameters.AddWithValue("@id", id);
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {                            
                                selectedEmployeeId = 0; // Сброс значения переменной selectedEmployeeId
                                this.Alert("Редактирование в таблице", "Данные успешно изменены!", false);
                                сотрудникиDataGridView.DataSource = null; сотрудникиBindingSource.ResetBindings(false); сотрудникиBindingSource.DataSource = db.getData("SELECT * FROM Сотрудники");
                                сотрудникиDataGridView.DataSource = сотрудникиBindingSource; updateCustomGradientPanel2.Visible = false; guna2CustomGradientPanel1.Visible = true;  menuPanel3.Visible = true;
                            }
                            else
                            { this.Alert("Ошибка при редактировании", "Не удалось обновить данные. Запись не найдена!", false); return; }
                        }
                        TableSot();
                    }
                    catch (System.Exception ex)
                    { MessageBox.Show("Ошибка при редактировании: " + ex.Message); }
                }
            }
            else { return; }
            LoadPersonalData();
        }
        private void iconButton25_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image files (*.jpeg, *.jpg, *.png) | *.jpeg; *.jpg; *.png";
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                { upfotoBox3.Image = System.Drawing.Image.FromFile(openFileDialog.FileName); }
            }
        }
        private void updateCustomGradientPanel2_Paint(object sender, PaintEventArgs e)
        { Change.ChangeButtonColorToDodgerBlue(iconButton23); Change.ChangeButtonColorToTransparent(iconButton25); }

        private void iconButton23_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = DialogForm.Show("Подтверждение выхода при редактировании", "Вы уверены, что хотите отменить добавление нового\nсотрудника? Несохранённые данные будут потеряны!", true);
            if (dialogResult == DialogResult.Yes) { updateCustomGradientPanel2.Visible = false; sotrudGroupBox.Visible = false; guna2CustomGradientPanel1.Visible = true; menuPanel3.Visible = true; } else { return; };
        }
        private int selectedEmployeeId; // объявление переменной
        private void сотрудникиDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var cell = сотрудникиDataGridView.Rows[e.RowIndex].Cells["ID_Сотрудника"];
                if (cell != null && cell.Value != null && int.TryParse(cell.Value.ToString(), out int id))
                {selectedEmployeeId = id;}
            }
        }
        private void searchguna2TextBox2_TextChanged(object sender, EventArgs e)
        { TableSot(searchguna2TextBox2.Text); }
        private void clearButton_Click(object sender, EventArgs e)
        { searchguna2TextBox2.Text = ""; }

        //----------------------------------------------------------------------------Таблица Пользователи
        private void userguna2GroupBox_Click(object sender, EventArgs e)
        { }
        private void searchguna2TextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (пользователиBindingSource != null && searchguna2TextBox.Text != "")
                {
                    string searchTerm = searchguna2TextBox.Text.Trim(); string[] searchTerms = searchTerm.Split(' '); // Разделяем поисковый запрос на фамилию, имя и отчество
                    string filterExpression = "";  // Формируем строку фильтра с оператором AND
                    foreach (string term in searchTerms) { filterExpression += $"([Логин] LIKE '%{term}%') AND "; }
                    filterExpression = filterExpression.Remove(filterExpression.Length - 5); пользователиBindingSource.Filter = filterExpression; // Удаляем последний оператор AND
                }
                else { пользователиBindingSource.Filter = ""; }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void iconButton9_Click(object sender, EventArgs e)
        { searchguna2TextBox.Text = ""; }
        private void iconButton14_Click(object sender, EventArgs e)
        {
            if (userguna2GroupBox.Visible) { this.Alert("Предупреждение при добавлении", "Окно для добавления нового пользователя уже открыто !", false); }
            else { IconButton activeButton = (IconButton)sender; change.SetButtonColors(activeButton, change.ActiveBackGroundColor, change.ActiveForeGroundColor); loginTextBox2.Text = ""; passwordTextBox2.Text = ""; rollComboBox2.Text = ""; пользователиDataGridView.Height -= 190; userguna2GroupBox.Visible = true; }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(loginTextBox2.Text) || string.IsNullOrEmpty(passwordTextBox2.Text) || string.IsNullOrEmpty(rollComboBox2.Text))
            { this.Alert("Ошибка при добавлении", "Пожалуйста, заполните все поля со звёздочкой!", false); }
            else if (db.CheckUser(loginTextBox2.Text))
            { this.Alert("Ошибка при добавлении", "Пользователь с таким логином уже существует!", false); }
            else
            {
                var hashedPassword = PersonalArea.hashPassword(passwordTextBox2.Text); // Получение хэшированного пароля
                var add = $"insert into Пользователи (Логин, Пароль, Роль) values ('{loginTextBox2.Text}', '{hashedPassword}', '{rollComboBox2.Text}')";
                db.queryExecute(add);
                this.Alert("Добавление в таблицу", "Новый пользователь был успешно добавлен в таблицу.", false);
                change.SetButtonColors(iconButton14, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(addsotButton, change.DefBackGroundColor, change.DefForeGroundColor); // Сброс цвета кнопок              
              
                пользователиDataGridView.DataSource = null; // Сброс источника данных и обновление данных в таблице
                пользователиDataGridView.DataSource = db.getData("SELECT * FROM Пользователи WHERE Роль IN ('Врач', 'Регистратор')"); // Обновление данных
                пользователиBindingSource.DataSource = пользователиDataGridView.DataSource; пользователиDataGridView.Height += 170; пользователиBindingSource.ResetBindings(false);
                userguna2GroupBox.Visible = false;               
                searchguna2TextBox.Clear(); // Очистка текста в поисковом поле и применяем фильтр заново
                пользователиBindingSource.Filter = $"[Логин] LIKE '%{searchguna2TextBox.Text}%'"; // Повторное применение фильтра      
            }
        }

        private void userPanel_Paint(object sender, PaintEventArgs e)
        { Change.ChangeButtonColorToTransparent(iconButton22); Change.ChangeButtonColorToDodgerBlue(iconButton4); Change.ChangeButtonColorToTransparent(iconButton9); Change.ChangeButtonColorToTransparent(iconButton10); Change.ChangeButtonColorToTransparent(iconButton12); Change.ChangeButtonColorToTransparent(iconButton14); Change.ChangeButtonColorToDodgerBlue(iconButton5); }
        private void guna2CustomGradientPanel3_Paint(object sender, PaintEventArgs e)
        { }
        private void iconButton22_Click(object sender, EventArgs e)
        { loginTextBox2.Text = ""; passwordTextBox2.Text = ""; rollComboBox2.SelectedIndex = -1; }
        private void iconButton4_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = DialogForm.Show("Подтверждение выхода при редактировании", "Вы уверены, что хотите отменить добавление нового\nпользователя? Несохранённые данные будут потеряны!", true);
            if (dialogResult == DialogResult.Yes) { change.SetButtonColors(iconButton14, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(addsotButton, change.DefBackGroundColor, change.DefForeGroundColor); loginTextBox2.Text = ""; passwordTextBox2.Text = ""; rollComboBox2.SelectedIndex = -1; userguna2GroupBox.Visible = false; пользователиDataGridView.Height += 200; }
            else { return; }
        }
        private void iconButton12_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = DialogForm.Show("Подтверждение удаления", "Вы уверены, что хотите удалить выбранную запись?", false);
            if (dialogResult == DialogResult.Yes)
            {
                List<int> idsToDelete = new List<int>();
                foreach (DataGridViewRow row in пользователиDataGridView.SelectedRows)
                {int id = Convert.ToInt32(row.Cells["iDПользователяDataGridViewTextBoxColumn1"].Value);idsToDelete.Add(id); }
                if (idsToDelete.Count > 0)
                {
                    string ids = string.Join(",", idsToDelete); // Преобразуем список ID в строку для использования в запросе
                    string connectionString = db.getConnectionString();
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();                       
                        SqlCommand checkQuery = new SqlCommand($"SELECT COUNT(*) FROM Сотрудники WHERE ID_Пользователя IN ({ids})"); // Добавляем проверку, что сотрудника еще нет в таблице "Сотрудника"
                        checkQuery.Connection = conn;
                        int count = Convert.ToInt32(checkQuery.ExecuteScalar());
                        if (count > 0)
                        { this.Alert("Удаление в таблице ", "Нельзя удалить пользователя,\nу него есть привязка к одному из сотрудников!", true); }
                        else
                        {
                            SqlCommand delete = new SqlCommand($"delete from Пользователи where ID_Пользователя IN ({ids})");
                            delete.Connection = conn;
                            delete.ExecuteNonQuery();
                            this.Alert("Удаление в таблице", "Выбранная запись была успешно удалена!", false);
                            string query = "SELECT * FROM Пользователи WHERE Роль IN('Врач', 'Регистратор')";
                            DataTable dt;
                            dt = db.getData(query);
                            пользователиDataGridView.DataSource = dt; searchguna2TextBox.Clear();   // Применение фильтра заново и очистка текста в поисковом поле
                            пользователиBindingSource.DataSource = dt; // Обновление источника данных
                            пользователиBindingSource.Filter = $"[Логин] LIKE '%{searchguna2TextBox.Text}%'"; // Повторное применение фильтра
                        }
                    }
                }
            }
            else { return; }
        }
        private void iconButton10_Click(object sender, EventArgs e)
        {
            if (userguna2GroupBox.Visible == true)
            {this.Alert("Предупреждение при редактировании", "Закройте панель для добавления нового пользователя перед тем,\nкак открыть редактирование записи!", true); return; }          
            else if (пользователиDataGridView.CurrentRow != null)
            {
                updateCustomGradientPanel.Visible = true; guna2CustomGradientPanel3.Visible = false; menuPanel2.Visible = false;
                DataGridViewRow selectedRow = пользователиDataGridView.CurrentRow;                
                loginTextBox3.Text = selectedRow.Cells["логинDataGridViewTextBoxColumn"].Value?.ToString() ?? "";              
                rollComboBox3.Text = selectedRow.Cells["рольDataGridViewTextBoxColumn"].Value?.ToString() ?? "";
            }
            else
            { this.Alert("Ошибка при редактировании", "Не удалось обновить данные. Запись не найдена!", false); return; }
        }
        private void iconButton5_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = DialogForm.Show("Подтверждение выхода при редактировании", "\"Вы уверены, что хотите отменить редактирование\nпользователя? Несохранённые данные будут потеряны!", true);
            if (dialogResult == DialogResult.Yes) { updateCustomGradientPanel.Visible = false; guna2CustomGradientPanel3.Visible = true; menuPanel2.Visible = true; }
            else { return; }
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = DialogForm.Show("Редактирование в таблице", "Вы уверены, что хотите изменить запись?", false);
            if (dialogResult == DialogResult.Yes)
            {
                if (string.IsNullOrEmpty(loginTextBox3.Text) || string.IsNullOrEmpty(rollComboBox3.Text))
                { this.Alert("Ошибка при редактировании", "Пожалуйста, заполните все поля!", false); }
                else
                {
                    try
                    {
                        this.Validate(); string newRole = rollComboBox3.Text;
                        using (var connection = new SqlConnection(db.getConnectionString()))
                        {
                            connection.Open();
                            string query = "UPDATE Пользователи SET Роль = @roll WHERE ID_Пользователя = @id";
                            SqlCommand command = new SqlCommand(query, connection);
                            command.Parameters.AddWithValue("@roll", rollComboBox3.Text);
                            // Предположим, что ID_Пользователя - это уникальное поле, по которому вы можете определить запись
                            int id = Convert.ToInt32(пользователиDataGridView.CurrentRow.Cells["iDПользователяDataGridViewTextBoxColumn1"].Value);
                            command.Parameters.AddWithValue("@id", id);

                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                this.Alert("Редактирование в таблице", "Данные успешно изменены!", false);
                                // Обновление данных в таблице
                                пользователиDataGridView.DataSource = null; // Сброс источника данных
                                пользователиDataGridView.DataSource = db.getData("SELECT * FROM Пользователи WHERE Роль IN ('Врач', 'Регистратор')"); // Обновление данных
                                пользователиBindingSource.DataSource = пользователиDataGridView.DataSource;

                                пользователиBindingSource.ResetBindings(false);
                                updateCustomGradientPanel.Visible = false; guna2CustomGradientPanel3.Visible = true; menuPanel2.Visible = true;
                                
                                searchguna2TextBox.Clear(); // Очистка текста в поисковом поле и сброс фильтра
                                пользователиBindingSource.Filter = $"[Логин] LIKE '%{searchguna2TextBox.Text}%'"; // Повторное применение фильтра);
                            }
                            else
                            {this.Alert("Ошибка при редактировании", "Не удалось обновить данные. Запись не найдена!", false); return;}
                        }
                    }catch (System.Exception ex) { MessageBox.Show("Ошибка при редактировании: " + ex.Message); }
                }
            }else { return; }
        }
        private void пользователиDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            пользователиDataGridView.SelectionChanged += (s, args) =>
            {
                if (пользователиDataGridView.CurrentRow != null)
                {DataGridViewRow selectedRow = пользователиDataGridView.CurrentRow; loginTextBox2.Text = selectedRow.Cells["логинDataGridViewTextBoxColumn"].Value?.ToString() ?? ""; passwordTextBox2.Text = selectedRow.Cells["парольDataGridViewTextBoxColumn"].Value?.ToString() ?? ""; rollComboBox2.Text = selectedRow.Cells["рольDataGridViewTextBoxColumn"].Value?.ToString() ?? ""; }              
            };
        }
        //----------------------------------------------------------------------------Личный кабинет
        private void LoadPersonalData()
        {
            SurnameTextBox.Text = String.Format("{0} {1} {2}", PersonalArea.FirstName, PersonalArea.LastName, PersonalArea.FatherName);
            doltextbox2.Text = PersonalArea.Dol;
            CategoryTextBox.Text = PersonalArea.Category;
            exTextBox.Text = PersonalArea.Ex;   
            numTextBox.Text = PersonalArea.NumberTel;  
            loginTextBox.Text = PersonalArea.Login;
            PersonalArea personalArea = new PersonalArea();
            if (personalArea.SetPersonalData(PersonalArea.Login, PersonalArea.Password))
            {
                if (personalArea.Foto != null && personalArea.Foto.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream(personalArea.Foto))
                    {
                        фотоpictureBox.Image = System.Drawing.Image.FromStream(ms);
                    }
                }
            }
        }
        private void settingButton_Click(object sender, EventArgs e)
        {
            CreatorChartPanel.Visible = false; menuPanel.Visible = true; sotrudPanel.Visible = false; userPanel.Visible = false; AboutMePanel.Visible = true; leftpanel1.Visible = false; leftpanel2.Visible = false; leftpanel3.Visible = false; leftpanel4.Visible = false;
            IconButton activeButton = (IconButton)sender;// установка цвета кнопки
            change.SetButtonColors(activeButton, change.ActiveBackGroundColor, change.ActiveForeGroundColor);

            change.SetButtonColors(iconButton13, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton4, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton1, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton11, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton2, change.DefBackGroundColor, change.DefForeGroundColor); change.SetButtonColors(iconButton18, change.DefBackGroundColor, change.DefForeGroundColor);
            LoadPersonalData();
        }
        private void exitPictureBox_Click(object sender, EventArgs e)
        { AboutMePanel.Visible = false; change.SetButtonColors(settingButton, change.DefBackGroundColor, change.DefForeGroundColor); }
        private void AboutMePanel_Paint(object sender, PaintEventArgs e)
        { newpasswordTextBox2.UseSystemPasswordChar = true; Change.ChangeButtonColorToTransparent(iconButton6); }

        private void guna2CheckBox2_CheckedChanged(object sender, EventArgs e)
        { if (guna2CheckBox2.Checked) { newpasswordTextBox2.UseSystemPasswordChar = false; } else { newpasswordTextBox2.UseSystemPasswordChar = true; } }

        private void guna2Button1_Click(object sender, EventArgs e)
        { oldpasswordTextBox.Text = ""; passwordPanel.Visible = true; newpasswordTextBox.Text = ""; newpasswordTextBox2.Text = ""; }
        private void guna2Button2_Click(object sender, EventArgs e)
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
                    {
                        this.Alert("Изменение пароля", "Старый пароль был успешно изменен на новый.", false); passwordPanel.Visible = false;
                        пользователиDataGridView.DataSource = null;    // Обновление данных в таблице и Сброс источника данных
                        пользователиDataGridView.DataSource = db.getData("select * from Пользователи"); // Обновление данных
                    }
                    else
                    { this.Alert("Ошибка при изменении пароля", "Недопустимые символы при изменении пароля.", false); }
                    connection.Close();
                }
            }
            else { this.Alert("Изменение пароля", "Действие было отменено.", false); }
        }
        private void iconButton6_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = DialogForm.Show("Изменение пароля", "Вы точно хотите отменить изменение пароля?", false);
            if (dialogResult == DialogResult.Yes)
            { passwordPanel.Visible = false; newpasswordTextBox.Text = ""; newpasswordTextBox2.Text = ""; oldpasswordTextBox.Text = ""; guna2CheckBox2.Checked = false; }
            else { return; }
        }

        //----------------------------------------------------------------------------Статистика
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
                "INNER JOIN Пациенты ON Договоры.ID_Клиента = Пациенты.ID_Клиента INNER JOIN Услуги ON Договоры.ID_Услуги = Услуги.ID_Услуги INNER JOIN Заболевании ON Договоры.ID_Заболевания = Заболевании.ID_Заболевания INNER JOIN Сотрудники ON Договоры.ID_Сотрудника = Сотрудники.ID_Сотрудника", db.GetConnection());
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
        private bool isFirstClick = false;
        private void iconButton21_Click(object sender, EventArgs e)
        {
            if (doc.IconChar == IconChar.NotesMedical) 
            { TabletСontracts(); dolPanel.Visible = true; doclabel.Text = "Договоры на оказание услуг"; doc.IconChar = IconChar.ChartColumn; panelsearch.Visible = true; doc.Location = new Point(436, 12); opendoc.Location = new Point(400, 12); image.Location = new Point(364, 12); image.IconChar = IconChar.Info; }
            else if(doc.IconChar == IconChar.ChartColumn)
            { TabletСontracts(); dolPanel.Visible = false; doclabel.Text = "Статистика оказания медицинских услуг"; doc.IconChar = IconChar.NotesMedical; panelsearch.Visible = false; doc.Location = new Point(836, 12); opendoc.Location = new Point(795, 12); image.IconChar = IconChar.Image; image.Location = new Point(755, 12); }
        }
        private void image_Click(object sender, EventArgs e)
        {
            if (image.IconChar == IconChar.Info)
            {             
                if (договорыDataGridView.CurrentRow != null)
                {
                    updateCustomGradientPanel4.Visible = true; dolPanel.Visible = false; infopanel.Visible = false; dog.Visible = false;
                    DataGridViewRow selectedRow = договорыDataGridView.CurrentRow;
                    iddoc.Text = selectedRow.Cells["ID_Договора"].Value?.ToString() ?? ""; uslTextBox.Text = selectedRow.Cells["Услуга"].Value?.ToString() ?? ""; thabTextBox.Text = selectedRow.Cells["Заболевание"].Value?.ToString() ?? "";
                    fiopacTextBox.Text = selectedRow.Cells["Пациент"].Value?.ToString() ?? ""; fiovrahTextBox.Text = selectedRow.Cells["Врач"].Value?.ToString() ?? ""; dateTextBox.Text = selectedRow.Cells["Дата_составления"].Value?.ToString() ?? "";
                    kolTextBox2.Text = selectedRow.Cells["Количество"].Value?.ToString() ?? ""; uslTextBox2.Text = selectedRow.Cells["Цена_услуги"].Value?.ToString() ?? ""; summTextBox2.Text = selectedRow.Cells["Итоговая_стоимость"].Value?.ToString() ?? "";                  
                    garTextBox.Text = selectedRow.Cells["Гарантия"].Value?.ToString() ?? ""; vidTextBox.Text = selectedRow.Cells["Вид_оплаты"].Value?.ToString() ?? ""; adressTextBox.Text = selectedRow.Cells["Адрес_стоматологии"].Value?.ToString() ?? "";
                }
                else
                { this.Alert("Информация о договоре на оказание услуг", "Не удалось открыть иинформацию о договоре на оказание услуг.\nЗапись не найдена!", true); return; }
            }
            else if (image.IconChar == IconChar.Image)
            {
                SaveChartToPDF(договорыDataGridView2);
                this.Alert("Статистика оказания медицинских услуг", "Изображение диаграммы по оказанию медицинских услуг\nбыло успешно создано.", true); return;
            }
        }
        private void closeButton2_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = DialogForm.Show("Подтверждение выхода", "Закрыть информацию о договоре на оказание услуг?", false);
            if (dialogResult == DialogResult.Yes) { updateCustomGradientPanel4.Visible = false; dolPanel.Visible = true; infopanel.Visible = true; dog.Visible = true; } else { return; }
        }
        private void iconButton20_Click(object sender, EventArgs e)
        { string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Договоры"); Process.Start(folderPath); } // Открытие папки Договоры
        private void CreatorChartPanel_Paint(object sender, PaintEventArgs e)
        { Change.ChangeButtonColorToTransparent(search); Change.ChangeButtonColorToTransparent(image); Change.ChangeButtonColorToTransparent(opendoc); Change.ChangeButtonColorToTransparent(doc); Change.ChangeButtonColorToDodgerBlue(closeButton2);}
        private void guna2TextBox7_TextChanged(object sender, EventArgs e)
        {TabletСontracts(searhtext.Text); }
        private void iconButton17_Click(object sender, EventArgs e)
        { searhtext.Text = ""; }

       
    }
}

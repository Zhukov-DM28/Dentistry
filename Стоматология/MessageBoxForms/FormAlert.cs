using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management.Instrumentation;
using System.Reflection;
using System.Resources;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Стоматология.Classes;
using Стоматология.Properties;

namespace Стоматология.Forms
{
    public partial class FormAlert : Form
    {
        public FormAlert()
        {
            InitializeComponent();Change.ChangeButtonColorToTransparent(closeButton);
        }
        private void ToastForm_Load(object sender, EventArgs e)
        { ToustTimer.Start();}
        public enum enmAction
        {wait,start,close}
        public enum enmType
        {Success,Warning, Error, Info}

        private FormAlert.enmAction action;
        private int x, y;

        public void showAlert(string type, string message, bool liftText)
        {
            this.Opacity = 0.0;
            this.StartPosition = FormStartPosition.Manual;
            string fname;

            for (int i = 1; i < 10; i++)
            {
                fname = "alert" + i.ToString();
                FormAlert frm = (FormAlert)Application.OpenForms[fname];

                if (frm == null)
                {
                    this.Name = fname;
                    this.x = Screen.PrimaryScreen.WorkingArea.Width - this.Width + 10;
                    this.y = Screen.PrimaryScreen.WorkingArea.Height - this.Height * i - 5 * i;
                    this.Location = new Point(this.x, this.y);
                    break;
                }
            }
            this.x = Screen.PrimaryScreen.WorkingArea.Width - base.Width - 5;

            switch (type)
            {
                case "Ошибка авторизации":
                    leftborder.BackColor = Color.Crimson;
                    picIcon.Image = Properties.Resources.icon_cansel;
                    break;
                case "Авторизация в аккаунт":
                    leftborder.BackColor = Color.DodgerBlue;
                    picIcon.Image = Properties.Resources.information;
                    break;

                case "Ошибка при добавлении":
                    leftborder.BackColor = Color.Crimson;
                    picIcon.Image = Properties.Resources.icon_cansel;
                    break;
                case "Ошибка при редактировании":
                    leftborder.BackColor = Color.Crimson;
                    picIcon.Image = Properties.Resources.icon_cansel;
                    break;

                case "Удаление в таблице":
                    leftborder.BackColor = Color.MediumSeaGreen;
                    picIcon.Image = Properties.Resources.icon_ok;
                    break;
                case "Удаление в таблице ":
                    leftborder.BackColor = Color.Gold;
                    picIcon.Image = Properties.Resources.icom_error;
                    break;

                case "Добавление в таблицу":
                    leftborder.BackColor = Color.MediumSeaGreen;
                    picIcon.Image = Properties.Resources.icon_ok;
                    break;
                case "Редактирование в таблице":
                    leftborder.BackColor = Color.MediumSeaGreen;
                    picIcon.Image = Properties.Resources.icon_ok;
                    break;
                case "Предупреждение при добавлении":
                    leftborder.BackColor = Color.Gold;
                    picIcon.Image = Properties.Resources.icom_error;
                    break;
                case "Предупреждение при редактировании":
                    leftborder.BackColor = Color.Gold;
                    picIcon.Image = Properties.Resources.icom_error;
                    break;

                case "Ошибка при изменении пароля":
                    leftborder.BackColor = Color.Crimson;
                    picIcon.Image = Properties.Resources.icon_cansel;
                    break;         
                case "Изменение пароля":
                    leftborder.BackColor = Color.MediumSeaGreen;
                    picIcon.Image = Properties.Resources.icon_ok;
                    break;
                case "Изменение пароля ":
                    leftborder.BackColor = Color.DodgerBlue;
                    picIcon.Image = Properties.Resources.information;
                    break;

                case "Изменение статуса":
                    leftborder.BackColor = Color.MediumSeaGreen;
                    picIcon.Image = Properties.Resources.icon_ok;
                    break;
                case "Ошибка при изменении статуса":
                    leftborder.BackColor = Color.Crimson;
                    picIcon.Image = Properties.Resources.icon_cansel;
                    break;
                case "Ошибка при завершении приема":
                    leftborder.BackColor = Color.Crimson;
                    picIcon.Image = Properties.Resources.icon_cansel;
                    break;
                   
                case "Фоновый режим":
                    leftborder.BackColor = Color.DodgerBlue;
                    picIcon.Image = Properties.Resources.information;
                    break;
                case "Создание талона":
                    leftborder.BackColor = Color.MediumSeaGreen;
                    picIcon.Image = Properties.Resources.icon_ok;
                    break;
                case "Создание талона ":
                    leftborder.BackColor = Color.Gold;
                    picIcon.Image = Properties.Resources.icom_error;
                    break;

                case "Создание договора":
                    leftborder.BackColor = Color.MediumSeaGreen;
                    picIcon.Image = Properties.Resources.icon_ok;
                    break;
                case "Создание договора ":
                    leftborder.BackColor = Color.Gold;
                    picIcon.Image = Properties.Resources.icom_error;
                    break;
                    
                case "Завершение приема":
                    leftborder.BackColor = Color.DodgerBlue;
                    picIcon.Image = Properties.Resources.information;
                    break;
                case "Записи на прием":
                    leftborder.BackColor = Color.DodgerBlue;
                    picIcon.Image = Properties.Resources.information;
                    break;

                 case "Руководство пользователя":
                    leftborder.BackColor = Color.Gold;
                    picIcon.Image = Properties.Resources.icom_error;
                    break;
                case "О программе":
                    leftborder.BackColor = Color.Gold;
                    picIcon.Image = Properties.Resources.icom_error;
                    break;

                case "Ошибка при создании записи":
                    leftborder.BackColor = Color.Crimson;
                    picIcon.Image = Properties.Resources.icon_cansel;
                    break;
                case "Ошибка при создании талона":
                    leftborder.BackColor = Color.Crimson;
                    picIcon.Image = Properties.Resources.icon_cansel;
                    break;
                case "Ошибка при создании документа":
                    leftborder.BackColor = Color.Crimson;
                    picIcon.Image = Properties.Resources.icon_cansel;
                    break;
                   
                case "Информация о договоре на оказание услуг":
                    leftborder.BackColor = Color.Gold;
                    picIcon.Image = Properties.Resources.icom_error;
                    break;
                case "Карточка сотрудника":
                    leftborder.BackColor = Color.Gold;
                    picIcon.Image = Properties.Resources.icom_error;
                    break;
                case "Информация о пациенте":
                    leftborder.BackColor = Color.Gold;
                    picIcon.Image = Properties.Resources.icom_error;
                    break;
                case "Информация о заболевании":
                    leftborder.BackColor = Color.Gold;
                    picIcon.Image = Properties.Resources.icom_error;
                    break;
                case "Статистика оказания медицинских услуг":
                    leftborder.BackColor = Color.MediumSeaGreen;
                    picIcon.Image = Properties.Resources.icon_ok;
                    break;
            }

            this.IbType.Text = type;
    this.ibMessage.Text = message;

    if (liftText)
    {
        y = IbType.Location.Y; // Сохраняем начальную позицию текста IbType
        IbType.Location = new Point(IbType.Location.X, y - 10); // Поднимаем текст IbType на 10 пикселей
        y = ibMessage.Location.Y; // Сохраняем начальную позицию текста ibMessage
        ibMessage.Location = new Point(ibMessage.Location.X, y - 10); // Поднимаем текст ibMessage на 10 пикселей
    }

    this.Show();
    this.action = enmAction.start;
    this.ToustTimer.Interval = 1;
    this.ToustTimer.Start();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            switch (this.action)
            {
                case enmAction.wait:
                    ToustTimer.Interval = 4700;
                    action = enmAction.close;
                    break;
                case FormAlert.enmAction.start:
                    this.ToustTimer.Interval = 1;
                    this.Opacity += 0.1;
                    if (this.x < this.Location.X)
                    {
                        this.Left--;
                    }
                    else
                    {
                        if (this.Opacity == 1.0)
                        {
                            action = FormAlert.enmAction.wait;
                        }
                    }
                    break;
                case enmAction.close:
                    ToustTimer.Interval = 1;
                    this.Opacity -= 0.1;

                    this.Left -= 3;
                    if (base.Opacity == 0.0)
                    {
                        base.Close();
                    }
                    break;
            }
        }    
        private void iconButton8_Click(object sender, EventArgs e)
        { this.Close();}
    }
}

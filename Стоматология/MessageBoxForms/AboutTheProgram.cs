using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Стоматология.Classes;
using Стоматология.Forms;

namespace Стоматология.MessageBoxForms
{
    public partial class AboutTheProgram : Form
    {
        private bool mouseDown; // Создание переменных для перемещения формы
        private Point lastLocation;

        public void Alert(string msg, string type, bool liftText)
        { FormAlert frm = new FormAlert(); frm.showAlert(msg, type, liftText); }// Создание переменных для сообщения
        public AboutTheProgram()
        {InitializeComponent();  }
        private void iconButton8_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = DialogForm.Show("Подтверждение закрытия", "Вы уверены, что хотите закрыть окно о программе?", false);
            if (dialogResult == DialogResult.Yes)
            { this.Close(); }
            else
            { return; }    
        }
        private void iconButton4_Click(object sender, EventArgs e)
        { this.WindowState = FormWindowState.Minimized; this.Alert("Фоновый режим", "Окно о программе находится в фоновом режиме!", false); }//свернуть форму
        private void AboutTheProgram_Load(object sender, EventArgs e)
        { this.Top += 50; Change.ChangeButtonColorToTransparent(iconButton4); Change.ChangeButtonColorToRed(iconButton8);}
    }
}

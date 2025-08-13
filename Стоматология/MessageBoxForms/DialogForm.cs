using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Стоматология.Classes;
using static TheArtOfDevHtmlRenderer.Adapters.RGraphicsPath;

namespace Стоматология.MessageBoxForms
{
    public partial class DialogForm : Form
    {
        private bool mouseDown; // Создание переменных для перемещения формы
        private Point lastLocation;
        public DialogForm()
        {
            InitializeComponent();
        }
        private void DialogForm_Load(object sender, EventArgs e)
        { Change.ChangeButtonColorToRed(iconButton8); }
        public static DialogResult Show(string IbType, string ibMessage, bool liftText)
        {
            DialogForm messageBox = new DialogForm();
            messageBox.IbType.Text = IbType;
            messageBox.ibMessage.Text = ibMessage;

            if (liftText)
            {
                int y = messageBox.ibMessage.Location.Y; // Сохраняем начальную позицию текста ibMessage 
                messageBox.ibMessage.Location = new Point(messageBox.ibMessage.Location.X, y - 10); // Поднимаем текст ibMessage на 10 пикселей 
            }

            messageBox.ShowDialog();
            return messageBox.DialogResult;
        }
        private void btnYes_Click(object sender, EventArgs e)
        { DialogResult = DialogResult.Yes; }
        private void btnNo_Click(object sender, EventArgs e)
        { DialogResult = DialogResult.No; }

        private void iconButton8_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

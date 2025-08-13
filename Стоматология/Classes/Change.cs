using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using FontAwesome.Sharp;

namespace Стоматология.Classes
{
    internal class Change
    {
        public Color ActiveBackGroundColor { get; private set; } = Color.FromArgb(24, 30, 54);
        public Color ActiveForeGroundColor { get; private set; } = Color.DodgerBlue;
        public Color DefBackGroundColor { get; private set; } = Color.FromArgb(24, 30, 54);
        public Color DefForeGroundColor { get; private set; } = Color.White;

        public void SetButtonColors(IconButton button, Color backGroundColor, Color foreGroundColor)
        {
            button.BackColor = backGroundColor;
            button.ForeColor = foreGroundColor;
            button.IconColor = foreGroundColor;
        }

        public static void ChangeButtonColorToTransparent(Button button) // При наведении меняет цвет на прозрачный 
        {
            Color customColor = Color.FromArgb(46, 51, 73);
            button.MouseEnter += (sender, e) => {
                button.BackColor = customColor;
            };
            button.MouseLeave += (sender, e) =>
            {
                if (!button.ClientRectangle.Contains(button.PointToClient(Cursor.Position)))
                {
                    button.BackColor = Color.Transparent; 
                }
            };
        }
        public static void ChangeButtonColorToRed(Button button)
        {
            Color redColor = Color.Red; // добавляем новый цвет для красного

            button.MouseEnter += (sender, e) => {
                button.BackColor = redColor; // меняем customColor на redColor
            };

            button.MouseLeave += (sender, e) =>
            {
                if (!button.ClientRectangle.Contains(button.PointToClient(Cursor.Position)))
                {
                    button.BackColor = Color.Transparent;
                }
            };
        }
        public static void ChangeButtonColorToDodgerBlue(Button button)
        {
            Color customColor = Color.DodgerBlue;
            Color blueColor = Color.DodgerBlue; // добавляем новый цвет для синего

            button.MouseEnter += (sender, e) => {
                button.BackColor = blueColor; // меняем customColor на blueColor
            };

            button.MouseLeave += (sender, e) =>
            {
                if (!button.ClientRectangle.Contains(button.PointToClient(Cursor.Position)))
                {
                    button.BackColor = customColor; // возвращаем исходный цвет
                }
            };
        }       
    }
};

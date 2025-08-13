namespace Стоматология.Forms
{
    partial class FormAlert
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAlert));
            this.leftborder = new System.Windows.Forms.Panel();
            this.IbType = new System.Windows.Forms.Label();
            this.ibMessage = new System.Windows.Forms.Label();
            this.guna2BorderlessForm1 = new Guna.UI2.WinForms.Guna2BorderlessForm(this.components);
            this.picIcon = new System.Windows.Forms.PictureBox();
            this.ToustTimer = new System.Windows.Forms.Timer(this.components);
            this.closeButton = new FontAwesome.Sharp.IconButton();
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // leftborder
            // 
            this.leftborder.BackColor = System.Drawing.Color.White;
            this.leftborder.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftborder.Location = new System.Drawing.Point(0, 0);
            this.leftborder.Name = "leftborder";
            this.leftborder.Size = new System.Drawing.Size(10, 75);
            this.leftborder.TabIndex = 3;
            // 
            // IbType
            // 
            this.IbType.AutoSize = true;
            this.IbType.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.IbType.ForeColor = System.Drawing.Color.DodgerBlue;
            this.IbType.Location = new System.Drawing.Point(76, 16);
            this.IbType.Name = "IbType";
            this.IbType.Size = new System.Drawing.Size(67, 20);
            this.IbType.TabIndex = 5;
            this.IbType.Text = "Пример";
            // 
            // ibMessage
            // 
            this.ibMessage.AutoSize = true;
            this.ibMessage.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ibMessage.ForeColor = System.Drawing.Color.White;
            this.ibMessage.Location = new System.Drawing.Point(78, 37);
            this.ibMessage.Name = "ibMessage";
            this.ibMessage.Size = new System.Drawing.Size(129, 17);
            this.ibMessage.TabIndex = 6;
            this.ibMessage.Text = "Пример сообщения";
            // 
            // guna2BorderlessForm1
            // 
            this.guna2BorderlessForm1.BorderRadius = 12;
            this.guna2BorderlessForm1.ContainerControl = this;
            this.guna2BorderlessForm1.DockIndicatorTransparencyValue = 0.6D;
            this.guna2BorderlessForm1.TransparentWhileDrag = true;
            // 
            // picIcon
            // 
            this.picIcon.Image = global::Стоматология.Properties.Resources.information;
            this.picIcon.Location = new System.Drawing.Point(30, 22);
            this.picIcon.Name = "picIcon";
            this.picIcon.Size = new System.Drawing.Size(30, 30);
            this.picIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picIcon.TabIndex = 4;
            this.picIcon.TabStop = false;
            // 
            // ToustTimer
            // 
            this.ToustTimer.Interval = 20;
            this.ToustTimer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.BackColor = System.Drawing.Color.Transparent;
            this.closeButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.closeButton.FlatAppearance.BorderSize = 0;
            this.closeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeButton.IconChar = FontAwesome.Sharp.IconChar.TimesCircle;
            this.closeButton.IconColor = System.Drawing.Color.White;
            this.closeButton.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.closeButton.IconSize = 30;
            this.closeButton.Location = new System.Drawing.Point(492, 22);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(31, 30);
            this.closeButton.TabIndex = 15;
            this.closeButton.UseVisualStyleBackColor = false;
            this.closeButton.Click += new System.EventHandler(this.iconButton8_Click);
            // 
            // FormAlert
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            this.ClientSize = new System.Drawing.Size(535, 75);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.ibMessage);
            this.Controls.Add(this.IbType);
            this.Controls.Add(this.picIcon);
            this.Controls.Add(this.leftborder);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(535, 75);
            this.MinimumSize = new System.Drawing.Size(535, 75);
            this.Name = "FormAlert";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Стоматология (Сообщение)";
            this.Load += new System.EventHandler(this.ToastForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel leftborder;
        private System.Windows.Forms.PictureBox picIcon;
        private System.Windows.Forms.Label IbType;
        private System.Windows.Forms.Label ibMessage;
        private Guna.UI2.WinForms.Guna2BorderlessForm guna2BorderlessForm1;
        private System.Windows.Forms.Timer ToustTimer;
        private FontAwesome.Sharp.IconButton closeButton;
    }
}
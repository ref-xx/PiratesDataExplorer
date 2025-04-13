namespace PiratesDataExplorer
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            pictureBox1 = new PictureBox();
            listBox1 = new ListBox();
            listBox2 = new ListBox();
            button1 = new Button();
            checkBox1 = new CheckBox();
            checkBox2 = new CheckBox();
            timer1 = new System.Windows.Forms.Timer(components);
            button2 = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(12, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(144, 112);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.Paint += pictureBox1_Paint;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(162, 12);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(120, 319);
            listBox1.TabIndex = 10;
            listBox1.MouseClick += listBox1_MouseClick;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // listBox2
            // 
            listBox2.FormattingEnabled = true;
            listBox2.ItemHeight = 15;
            listBox2.Location = new Point(288, 12);
            listBox2.Name = "listBox2";
            listBox2.Size = new Size(110, 274);
            listBox2.TabIndex = 11;
            listBox2.SelectedIndexChanged += listBox2_SelectedIndexChanged;
            // 
            // button1
            // 
            button1.Location = new Point(81, 276);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 12;
            button1.Text = "Save As...";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(12, 305);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(135, 19);
            checkBox1.TabIndex = 13;
            checkBox1.Text = "Save as PNG on view";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.Checked = true;
            checkBox2.CheckState = CheckState.Checked;
            checkBox2.Location = new Point(12, 134);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(108, 19);
            checkBox2.TabIndex = 14;
            checkBox2.Text = "Auto Load Face";
            checkBox2.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Location = new Point(12, 159);
            button2.Name = "button2";
            button2.Size = new Size(144, 23);
            button2.TabIndex = 15;
            button2.Text = "Next Face Frame >>";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(402, 336);
            Controls.Add(button2);
            Controls.Add(checkBox2);
            Controls.Add(checkBox1);
            Controls.Add(button1);
            Controls.Add(listBox2);
            Controls.Add(listBox1);
            Controls.Add(pictureBox1);
            Name = "Form1";
            Text = "PirateViewer";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private ListBox listBox1;
        private ListBox listBox2;
        private Button button1;
        private CheckBox checkBox1;
        private CheckBox checkBox2;
        private System.Windows.Forms.Timer timer1;
        private Button button2;
    }
}

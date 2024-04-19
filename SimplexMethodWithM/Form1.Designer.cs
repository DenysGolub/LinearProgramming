namespace SimplexMethodWithM
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
            label1 = new Label();
            label2 = new Label();
            variable_count = new NumericUpDown();
            constraint_count = new NumericUpDown();
            label3 = new Label();
            comboBox1 = new ComboBox();
            flowLayoutPanel1 = new FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)variable_count).BeginInit();
            ((System.ComponentModel.ISupportInitialize)constraint_count).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(201, 31);
            label1.Name = "label1";
            label1.Size = new Size(115, 20);
            label1.TabIndex = 3;
            label1.Text = "К-ть обмежень";
            label1.Click += label1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(25, 30);
            label2.Name = "label2";
            label2.Size = new Size(98, 20);
            label2.TabIndex = 3;
            label2.Text = "К-ть змінних";
            label2.Click += label1_Click;
            // 
            // variable_count
            // 
            variable_count.Location = new Point(129, 28);
            variable_count.Name = "variable_count";
            variable_count.Size = new Size(40, 27);
            variable_count.TabIndex = 4;
            variable_count.ValueChanged += variable_count_ValueChanged;
            // 
            // constraint_count
            // 
            constraint_count.Location = new Point(322, 28);
            constraint_count.Name = "constraint_count";
            constraint_count.Size = new Size(40, 27);
            constraint_count.TabIndex = 4;
            constraint_count.ValueChanged += constraint_count_ValueChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(82, 89);
            label3.Name = "label3";
            label3.Size = new Size(28, 20);
            label3.TabIndex = 5;
            label3.Text = "Z=";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(12, 86);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(64, 28);
            comboBox1.TabIndex = 6;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.AutoScrollMinSize = new Size(10000, 0);
            flowLayoutPanel1.Location = new Point(116, 72);
            flowLayoutPanel1.Margin = new Padding(3, 4, 3, 4);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(377, 69);
            flowLayoutPanel1.TabIndex = 8;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(532, 544);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(comboBox1);
            Controls.Add(label3);
            Controls.Add(constraint_count);
            Controls.Add(variable_count);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)variable_count).EndInit();
            ((System.ComponentModel.ISupportInitialize)constraint_count).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label1;
        private Label label2;
        private NumericUpDown variable_count;
        private NumericUpDown constraint_count;
        private Label label3;
        private ComboBox comboBox1;
        private FlowLayoutPanel flowLayoutPanel1;
    }
}

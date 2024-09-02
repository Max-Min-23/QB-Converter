namespace QB_Converter
{
    partial class frmMain
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
            grpConvertType = new GroupBox();
            chkCSVOrder = new CheckBox();
            label4 = new Label();
            label3 = new Label();
            cmbTo = new ComboBox();
            label2 = new Label();
            cmbFrom = new ComboBox();
            txtLog = new TextBox();
            btnClear = new Button();
            Worker = new System.ComponentModel.BackgroundWorker();
            grpConvertType.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(716, 560);
            label1.Name = "label1";
            label1.Size = new Size(95, 15);
            label1.TabIndex = 3;
            label1.Text = "© 2024 Min Max";
            // 
            // grpConvertType
            // 
            grpConvertType.Controls.Add(chkCSVOrder);
            grpConvertType.Controls.Add(label4);
            grpConvertType.Controls.Add(label3);
            grpConvertType.Controls.Add(cmbTo);
            grpConvertType.Controls.Add(label2);
            grpConvertType.Controls.Add(cmbFrom);
            grpConvertType.Location = new Point(16, 12);
            grpConvertType.Name = "grpConvertType";
            grpConvertType.Size = new Size(296, 112);
            grpConvertType.TabIndex = 5;
            grpConvertType.TabStop = false;
            grpConvertType.Text = "Select Convert Type";
            // 
            // chkCSVOrder
            // 
            chkCSVOrder.AutoSize = true;
            chkCSVOrder.Location = new Point(164, 80);
            chkCSVOrder.Name = "chkCSVOrder";
            chkCSVOrder.Size = new Size(109, 19);
            chkCSVOrder.TabIndex = 6;
            chkCSVOrder.Text = "CSV Pitch Order";
            chkCSVOrder.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(164, 32);
            label4.Name = "label4";
            label4.Size = new Size(19, 15);
            label4.TabIndex = 4;
            label4.Text = "To";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(140, 52);
            label3.Name = "label3";
            label3.Size = new Size(19, 15);
            label3.TabIndex = 3;
            label3.Text = "→";
            // 
            // cmbTo
            // 
            cmbTo.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTo.FormattingEnabled = true;
            cmbTo.Location = new Point(160, 48);
            cmbTo.Name = "cmbTo";
            cmbTo.Size = new Size(121, 23);
            cmbTo.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(20, 28);
            label2.Name = "label2";
            label2.Size = new Size(33, 15);
            label2.TabIndex = 1;
            label2.Text = "From";
            // 
            // cmbFrom
            // 
            cmbFrom.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFrom.FormattingEnabled = true;
            cmbFrom.Location = new Point(16, 48);
            cmbFrom.Name = "cmbFrom";
            cmbFrom.Size = new Size(121, 23);
            cmbFrom.TabIndex = 0;
            // 
            // txtLog
            // 
            txtLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtLog.Location = new Point(12, 136);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Both;
            txtLog.Size = new Size(762, 395);
            txtLog.TabIndex = 6;
            // 
            // btnClear
            // 
            btnClear.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClear.Location = new Point(698, 104);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(75, 23);
            btnClear.TabIndex = 7;
            btnClear.Text = "Clear";
            btnClear.UseVisualStyleBackColor = true;
            // 
            // Worker
            // 
            Worker.WorkerReportsProgress = true;
            Worker.WorkerSupportsCancellation = true;
            // 
            // frmMain
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 561);
            Controls.Add(btnClear);
            Controls.Add(txtLog);
            Controls.Add(grpConvertType);
            Controls.Add(label1);
            Name = "frmMain";
            Text = "QB Converter";
            grpConvertType.ResumeLayout(false);
            grpConvertType.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label1;
        private GroupBox grpConvertType;
        private CheckBox chkCSVOrder;
        private Label label4;
        private Label label3;
        private ComboBox cmbTo;
        private Label label2;
        private ComboBox cmbFrom;
        private TextBox txtLog;
        private Button btnClear;
        private System.ComponentModel.BackgroundWorker Worker;
    }
}

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
            labDrop = new Label();
            label1 = new Label();
            grpConvertType = new GroupBox();
            rbtSOneToCSV = new RadioButton();
            rbtCSVToDrm = new RadioButton();
            rbtCSVToPitchlist = new RadioButton();
            rbtCubaseToCSV = new RadioButton();
            rbtPitchlistToDrm = new RadioButton();
            rbtBasedOnONote = new RadioButton();
            rbtBasedOnINote = new RadioButton();
            chkCSVOrder = new CheckBox();
            grpConvertType.SuspendLayout();
            SuspendLayout();
            // 
            // labDrop
            // 
            labDrop.AllowDrop = true;
            labDrop.BorderStyle = BorderStyle.Fixed3D;
            labDrop.Font = new Font("Yu Gothic UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 128);
            labDrop.Location = new Point(4, 244);
            labDrop.Name = "labDrop";
            labDrop.Size = new Size(264, 104);
            labDrop.TabIndex = 1;
            labDrop.Text = "Drop File Here!";
            labDrop.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(172, 352);
            label1.Name = "label1";
            label1.Size = new Size(95, 15);
            label1.TabIndex = 3;
            label1.Text = "© 2024 Min Max";
            // 
            // grpConvertType
            // 
            grpConvertType.Controls.Add(rbtSOneToCSV);
            grpConvertType.Controls.Add(rbtCSVToDrm);
            grpConvertType.Controls.Add(rbtCSVToPitchlist);
            grpConvertType.Controls.Add(rbtCubaseToCSV);
            grpConvertType.Controls.Add(rbtPitchlistToDrm);
            grpConvertType.Controls.Add(rbtBasedOnONote);
            grpConvertType.Controls.Add(rbtBasedOnINote);
            grpConvertType.Location = new Point(8, 12);
            grpConvertType.Name = "grpConvertType";
            grpConvertType.Size = new Size(264, 204);
            grpConvertType.TabIndex = 5;
            grpConvertType.TabStop = false;
            grpConvertType.Text = "Select Convert Type";
            // 
            // rbtSOneToCSV
            // 
            rbtSOneToCSV.AutoSize = true;
            rbtSOneToCSV.Location = new Point(12, 124);
            rbtSOneToCSV.Name = "rbtSOneToCSV";
            rbtSOneToCSV.Size = new Size(114, 19);
            rbtSOneToCSV.TabIndex = 6;
            rbtSOneToCSV.Tag = "4";
            rbtSOneToCSV.Text = "*.Pitchlist -> .csv";
            rbtSOneToCSV.UseVisualStyleBackColor = true;
            // 
            // rbtCSVToDrm
            // 
            rbtCSVToDrm.AutoSize = true;
            rbtCSVToDrm.Location = new Point(12, 172);
            rbtCSVToDrm.Name = "rbtCSVToDrm";
            rbtCSVToDrm.Size = new Size(98, 19);
            rbtCSVToDrm.TabIndex = 5;
            rbtCSVToDrm.Tag = "6";
            rbtCSVToDrm.Text = "*.csv -> *.drm";
            rbtCSVToDrm.UseVisualStyleBackColor = true;
            // 
            // rbtCSVToPitchlist
            // 
            rbtCSVToPitchlist.AutoSize = true;
            rbtCSVToPitchlist.Location = new Point(12, 148);
            rbtCSVToPitchlist.Name = "rbtCSVToPitchlist";
            rbtCSVToPitchlist.Size = new Size(119, 19);
            rbtCSVToPitchlist.TabIndex = 4;
            rbtCSVToPitchlist.Tag = "5";
            rbtCSVToPitchlist.Text = "*.csv -> *.pitchlist";
            rbtCSVToPitchlist.UseVisualStyleBackColor = true;
            // 
            // rbtCubaseToCSV
            // 
            rbtCubaseToCSV.AutoSize = true;
            rbtCubaseToCSV.Location = new Point(12, 100);
            rbtCubaseToCSV.Name = "rbtCubaseToCSV";
            rbtCubaseToCSV.Size = new Size(93, 19);
            rbtCubaseToCSV.TabIndex = 3;
            rbtCubaseToCSV.Tag = "3";
            rbtCubaseToCSV.Text = "*.drm -> .csv";
            rbtCubaseToCSV.UseVisualStyleBackColor = true;
            // 
            // rbtPitchlistToDrm
            // 
            rbtPitchlistToDrm.AutoSize = true;
            rbtPitchlistToDrm.Location = new Point(12, 76);
            rbtPitchlistToDrm.Name = "rbtPitchlistToDrm";
            rbtPitchlistToDrm.Size = new Size(123, 19);
            rbtPitchlistToDrm.TabIndex = 2;
            rbtPitchlistToDrm.Tag = "2";
            rbtPitchlistToDrm.Text = "*.pitchlist -> *.drm";
            rbtPitchlistToDrm.UseVisualStyleBackColor = true;
            // 
            // rbtBasedOnONote
            // 
            rbtBasedOnONote.AutoSize = true;
            rbtBasedOnONote.Checked = true;
            rbtBasedOnONote.Location = new Point(12, 52);
            rbtBasedOnONote.Name = "rbtBasedOnONote";
            rbtBasedOnONote.Size = new Size(229, 19);
            rbtBasedOnONote.TabIndex = 1;
            rbtBasedOnONote.TabStop = true;
            rbtBasedOnONote.Tag = "1";
            rbtBasedOnONote.Text = "*.drm -> *.pitchlist Based On Out Pitch";
            rbtBasedOnONote.UseVisualStyleBackColor = true;
            // 
            // rbtBasedOnINote
            // 
            rbtBasedOnINote.AutoSize = true;
            rbtBasedOnINote.Location = new Point(12, 28);
            rbtBasedOnINote.Name = "rbtBasedOnINote";
            rbtBasedOnINote.Size = new Size(219, 19);
            rbtBasedOnINote.TabIndex = 0;
            rbtBasedOnINote.Tag = "0";
            rbtBasedOnINote.Text = "*.drm -> *.pitchlist Based On In Pitch";
            rbtBasedOnINote.UseVisualStyleBackColor = true;
            // 
            // chkCSVOrder
            // 
            chkCSVOrder.AutoSize = true;
            chkCSVOrder.Location = new Point(12, 220);
            chkCSVOrder.Name = "chkCSVOrder";
            chkCSVOrder.Size = new Size(109, 19);
            chkCSVOrder.TabIndex = 6;
            chkCSVOrder.Text = "CSV Pitch Order";
            chkCSVOrder.UseVisualStyleBackColor = true;
            // 
            // frmMain
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(278, 371);
            Controls.Add(chkCSVOrder);
            Controls.Add(grpConvertType);
            Controls.Add(label1);
            Controls.Add(labDrop);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "frmMain";
            Text = "QB Converter";
            grpConvertType.ResumeLayout(false);
            grpConvertType.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label labDrop;
        private Label label1;
        private GroupBox grpConvertType;
        private RadioButton rbtBasedOnONote;
        private RadioButton rbtBasedOnINote;
        private RadioButton rbtCubaseToCSV;
        private RadioButton rbtPitchlistToDrm;
        private RadioButton rbtCSVToDrm;
        private RadioButton rbtCSVToPitchlist;
        private RadioButton rbtSOneToCSV;
        private CheckBox chkCSVOrder;
    }
}

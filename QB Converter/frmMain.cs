using System.ComponentModel;
using System.Data;

namespace QB_Converter
{
    public partial class frmMain : Form
    {
        const string cmbKey = @"Key";

        public frmMain()
        {
            InitializeComponent();

            new List<ComboBox> { cmbFrom, cmbTo }
            .ToList().ForEach(combo =>
                {
                    combo.DisplayMember = cmbKey;
                    DataTable dt = new();
                    dt.Columns.Add(cmbKey, typeof(string));
                    Enum.GetNames<FileExtension>().ToList().ForEach(x => dt.Rows.Add(x));
                    combo.DataSource = dt.DefaultView;
                }
            );

            cmbFrom.SelectedIndexChanged += cmbToRowFilter;
            cmbToRowFilter(null, new EventArgs());

            btnClear.Click += BtnClear_Click;

            DragEnter += this_DragEnter;
            DragDrop += this_DragDrop;

            Worker.DoWork += Worker_DoWork;
            Worker.ProgressChanged += Worker_ProgressChanged;
            Worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        }

        private void BtnClear_Click(object? sender, EventArgs e)
        {
            txtLog.Text = string.Empty;
        }

        private void cmbToRowFilter(object? sender, EventArgs e)
        {
            if (cmbTo.DataSource is not DataView dv) return;
            dv.RowFilter = $"{cmbKey} <> '{cmbFrom.Text}'";
        }

        private void this_DragEnter(object? sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void this_DragDrop(object? sender, DragEventArgs e)
        {
            if (e.Data is not IDataObject data) return;

            if (data.GetData(DataFormats.FileDrop) is not string[] files) return;

            if (!Enum.TryParse(cmbFrom.Text, out FileExtension from)) return;
            if (!Enum.TryParse(cmbTo.Text, out FileExtension to)) return;

            Enabled = false;
            Worker.RunWorkerAsync((files, from, to));
        }

        private void Worker_DoWork(object? sender, DoWorkEventArgs e)
        {
            if (e.Argument is not (string[] files, FileExtension from, FileExtension to)) return;

            int count = 1;
            foreach (string file in files)
            {
                string result = Map.Import(file, from, to, chkCSVOrder.Checked);
                Worker.ReportProgress(count, result);
                count++;
            }
        }

        private void Worker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            txtLog.Text += $"{e.ProgressPercentage:d4}:{e.UserState}";
        }

        private void Worker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            Enabled = true;
        }
    }
}

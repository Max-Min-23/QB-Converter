namespace QB_Converter
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();

            labDrop.DragEnter += LabDrop_DragEnter;
            labDrop.DragDrop += LabDrop_DragDrop;
        }

        private void LabDrop_DragEnter(object? sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void LabDrop_DragDrop(object? sender, DragEventArgs e)
        {
            if (e.Data is not IDataObject data) return;

            if (data.GetData(DataFormats.FileDrop) is not string[] files) return;

            if (grpConvertType.Controls.Cast<RadioButton>().Where(x => x.Checked).FirstOrDefault() is not RadioButton b)
            {
                return;
            }
            ConvertType type = (ConvertType)int.Parse($"{b.Tag}".PadLeft(1, '0'));

            try
            {
                foreach (string file in files) Map.Import(file, type, chkCSVOrder.Checked);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ÉGÉâÅ[ÇæÇ¡ÇƒÇŒÇÊ\r\n{ex.Message}");
            }
        }
    }
}

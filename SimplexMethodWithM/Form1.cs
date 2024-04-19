using System.Data;

namespace SimplexMethodWithM
{
    public partial class Form1 : Form
    {
        DataTable constraints = new DataTable();
        public Form1()
        {
            InitializeComponent();

            flowLayoutPanel1.Controls.Add(new Button());
            flowLayoutPanel1.Controls.Add(new Button());
            flowLayoutPanel1.Controls.Add(new Button());
            flowLayoutPanel1.Controls.Add(new Button());
            flowLayoutPanel1.FlowDirection = FlowDirection.LeftToRight;

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void variable_count_ValueChanged(object sender, EventArgs e)
        {
            constraints.Columns.Clear();
            constraints.Rows.Clear();
            for (int i = 1; i <= variable_count.Value; i++)
            {
                constraints.Columns.Add("x" + i.ToString());
                //constraints.Rows.Add();
            }

            constraints.Columns.Add("Sign");
            constraints.Columns.Add(" ");

        }

        private void constraint_count_ValueChanged(object sender, EventArgs e)
        {
            constraints.Rows.Clear();

            for (int i = 1; i <= constraint_count.Value; i++)
            {
                constraints.Rows.Add();
                //constraints.Rows.Add();
            }


        }


       

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

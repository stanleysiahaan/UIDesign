using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace UIDesign
{
    public partial class ucProjectDetail : UserControl
    {
        public string projectID;

        public ucProjectDetail(ucChooseProject ucCP, string projectID)
        {
            InitializeComponent();
            //Initializing the database
            DBConnect dbc = new DBConnect();
            dbc.Initialize();
            dbc.OpenConnection();

            //Creating command for MySQL
            DataTable dt = new DataTable();
            string query = "SELECT * FROM project_data WHERE id LIKE '%" + projectID + "%'";

            //Showing the data in suitable text box
            MySqlDataAdapter da = new MySqlDataAdapter(query, dbc.connection);
            da.Fill(dt);
            textBox1.Text = dt.Rows[0][1].ToString();
            textBox2.Text = dt.Rows[0][2].ToString();
            richTextBox1.Text = dt.Rows[0][3].ToString();
            string method_id = dt.Rows[0][4].ToString();
            dbc.CloseConnection();

            //call the name of method_id
            dbc.OpenConnection();
            MySqlCommand cmd = new MySqlCommand();
            cmd = dbc.connection.CreateCommand();
            cmd.CommandText = "SELECT name FROM method_name WHERE id LIKE '%" + method_id + "%'";
            object method_name = cmd.ExecuteScalar();
            textBox3.Text = method_name.ToString();
            dbc.CloseConnection();

            //Show the parameter on the datagridview
            string query2 = "SELECT * FROM method_data WHERE method_id LIKE '%" + method_id + "%'";
            try
            {
                DataTable datatable = new DataTable();
                DataSet dataset = new DataSet();
                dataset.Tables.Add(datatable);
                MySqlDataAdapter dataadapter = new MySqlDataAdapter(query2, dbc.connection);
                dataadapter.Fill(datatable);
                dataGridView1.DataSource = datatable;
                this.dataGridView1.Columns["ID"].Visible = false;
                this.dataGridView1.Columns["method_id"].Visible = false;

                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    if (column.HeaderText == "hour")
                    {
                        column.HeaderText = "Duration (hours)";
                    }
                    else if (column.HeaderText == "minute")
                    {
                        column.HeaderText = "Duration (minute)";
                    }
                    else if (column.HeaderText == "second")
                    {
                        column.HeaderText = "Duration (second)";
                    }
                    else if (column.HeaderText == "oil_temp")
                    {
                        column.HeaderText = "Oil Temperature";
                    }
                    else if (column.HeaderText == "cool_temp")
                    {
                        column.HeaderText = "Coolant Temperature";
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            dbc.CloseConnection();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ucChooseProject ucCP = new ucChooseProject();
            this.Hide();
            this.Parent.Controls.Add(ucCP);
        }
    }
}

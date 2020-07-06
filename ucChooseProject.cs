using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;

namespace UIDesign
{
    
    public partial class ucChooseProject : UserControl
    {
        int row;
        int col;
        private string projectID;

        public ucChooseProject()
        {
            InitializeComponent();
            
            //Declare method to connect to the database
            DBConnect dbc = new DBConnect();
            dbc.Initialize();
            dbc.OpenConnection();

            //Obtaining the method_id based on method_name selected by user
            MySqlCommand cmd = new MySqlCommand();
            cmd = dbc.connection.CreateCommand();

            //Populating dataGridView1 with mySQL data
            string query = "SELECT project_data.id, project_data.costumer_name, project_data.sample_code, project_data.description, method_name.name FROM project_data INNER JOIN method_name ON method_name.id = project_data.method_id";

            try
            {
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                MySqlDataAdapter da = new MySqlDataAdapter(query, dbc.connection);
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            dbc.CloseConnection();
                        
            //Rezise the height of the panel
            int totalRowHeight = dataGridView1.ColumnHeadersHeight;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                totalRowHeight += row.Height;
                dataGridView1.Height = totalRowHeight;
            }    
                        
            // creating a coloumn with a detail button
            DataGridViewButtonColumn bcol1 = new DataGridViewButtonColumn();
            bcol1.HeaderText = "";
            bcol1.Text = "Detail";
            bcol1.Name = "btnDetail";
            bcol1.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Add(bcol1);

            // creating a coloumn with an execute button
            DataGridViewButtonColumn bcol2 = new DataGridViewButtonColumn();
            bcol2.HeaderText = "";
            bcol2.Text = "Execute";
            bcol2.Name = "btnExecute";
            bcol2.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Add(bcol2);
        }

        public void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                //Statements to show engine testing form
                col = 2;
                row = e.RowIndex;
                projectID = dataGridView1[col, row].Value.ToString();
                formEngineTesting formET = new formEngineTesting(this, projectID);
                formET.Show();
            }
            else if (e.ColumnIndex == 0)
            {
                // Column index is 3 to obtain project_id
                col = 2;
                row = e.RowIndex;        
                projectID = dataGridView1[col, row].Value.ToString();

                ucProjectDetail ucProjectDetail = new ucProjectDetail(this, projectID);
                this.Hide();
                this.Parent.Controls.Add(ucProjectDetail);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ucEngineTestingMenu ucETM = new ucEngineTestingMenu();
            this.Hide();
            this.Parent.Controls.Add(ucETM);
        }
    }
}

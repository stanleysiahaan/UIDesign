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
using System.Security.Cryptography;

namespace UIDesign
{
    public partial class ucMethodList : UserControl
    {
        private object methodIdObject;
        public ucMethodList()
        {
            
            InitializeComponent();
            DBConnect dbc = new DBConnect();
            dbc.Initialize();
            dbc.OpenConnection();

            MySqlCommand cmd = new MySqlCommand();
            cmd = dbc.connection.CreateCommand();
            cmd.CommandText = "SELECT name FROM method_name";
            MySqlDataReader dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {
                cbMethodList.Items.Add(dataReader.GetString("name"));
                cbMethodList.Items.Remove("");
            }
            dbc.CloseConnection();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (cbMethodList.Text == "")
            {
                MessageBox.Show("Please Select a Method");
            }
            else
            {
                DBConnect dbc = new DBConnect();
                dbc.Initialize();
                dbc.OpenConnection();

                //Obtaining the method_id based on method_name selected by user
                MySqlCommand cmd = new MySqlCommand();
                cmd = dbc.connection.CreateCommand();
                cmd.CommandText = "SELECT id FROM method_name WHERE name LIKE'%" + cbMethodList.Text + "%'";
                methodIdObject = cmd.ExecuteScalar();
                textBox1.Text = methodIdObject.ToString();

                //Populating dataGridView1 with mySQL data
                string query = "SELECT * FROM method_data WHERE method_id LIKE '%" + methodIdObject.ToString() + "%'";
                
                try
                {
                    DataTable dt = new DataTable();
                    DataSet ds = new DataSet();
                    ds.Tables.Add(dt);
                    MySqlDataAdapter da = new MySqlDataAdapter(query, dbc.connection);
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
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
                        else if (column.HeaderText == "ramp_time")
                        {
                            column.HeaderText = "Ramp Time (second)";
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
        }

        private void dataGridView1_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            //initialize the connection to database
            DBConnect dbc = new DBConnect();
            dbc.Initialize();
            dbc.OpenConnection();

            //Declare a function that detects the changes in dataGridView1
            DataTable changes = ((DataTable)dataGridView1.DataSource).GetChanges();


            //Script below is excecuted if changes does exist
            if (changes != null)
            {
                string data = string.Empty;
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.IsNewRow) continue;
                    data = row.Cells["method_id"].Value.ToString();
                    if (data.Contains("0"))
                    {
                        MessageBox.Show("ADA YANG NOL BROOOO");
                    }                    
                }
                string query = "SELECT * FROM method_data WHERE method_id LIKE '%" + methodIdObject.ToString() + "%'";
                DataSet ds = new DataSet();
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(query, dbc.connection);
                mySqlDataAdapter.Fill(ds);

                MySqlCommandBuilder mcb = new MySqlCommandBuilder(mySqlDataAdapter);
                mySqlDataAdapter.UpdateCommand = mcb.GetUpdateCommand();
                mySqlDataAdapter.Update(changes);
                ((DataTable)dataGridView1.DataSource).AcceptChanges();
                dbc.CloseConnection();
            }
            else
            {
                MessageBox.Show("There is no changes in data");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            dataGridView1_RowValidated(null, null);
            MessageBox.Show("Data successfuly updated");
        }


    }
}

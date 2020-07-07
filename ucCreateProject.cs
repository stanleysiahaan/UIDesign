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
using System.Linq.Expressions;

namespace UIDesign
{
    public partial class ucCreateProject : UserControl
    {
        private object method_id;
        public ucCreateProject()
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
                comboBox1.Items.Add(dataReader.GetString("name"));
                comboBox1.Items.Remove("");
            }

            dbc.CloseConnection();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DBConnect dbc = new DBConnect();
            dbc.Initialize();
            dbc.OpenConnection();

            MySqlCommand cmd = new MySqlCommand();
            cmd = dbc.connection.CreateCommand();

            //obtaining method_id from selected method name available on dropdown list
            string methodName = comboBox1.Text;
            cmd.CommandText = "SELECT id FROM method_name WHERE name LIKE '%" + methodName + "%'";
            method_id = cmd.ExecuteScalar();

            //inserting each data to database
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@costumer_name", textBox1.Text);
            cmd.Parameters.AddWithValue("@sample_code", textBox2.Text);
            cmd.Parameters.AddWithValue("@description", richTextBox1.Text);
            cmd.Parameters.AddWithValue("@method_id", method_id);
            //Assign the query using CommandText
            cmd.CommandText = "INSERT INTO project_data(costumer_name, sample_code, description, method_id)VALUES(@costumer_name, @sample_code, @description, @method_id)";
            cmd.ExecuteNonQuery();

            MessageBox.Show("Recods Inserted");

            dbc.CloseConnection();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ucEngineTestingMenu ucEngineTestingMenu = new ucEngineTestingMenu();
            this.Hide();
            this.Parent.Controls.Add(ucEngineTestingMenu);
        }
    }
}

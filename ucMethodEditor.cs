using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Drawing.Text;

namespace UIDesign
{
    public partial class ucMethodEditor : UserControl
    {
        private object methodIdObject;

        public ucMethodEditor()
        {
            InitializeComponent();
        }

        //To make 'Step' coloumn auto increment
        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            dataGridView1.Rows[e.RowIndex].Cells[0].Value = (e.RowIndex + 1);
        }

        //To skip 'Step' coloumn when user press Tab button
        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Step")
            {
                SendKeys.Send("{TAB}");
            }
        }

        

        private void btnSubmitMethod_Click(object sender, EventArgs e)
        {
            //calling DB Method to this usercontrol
            DBConnect dbc = new DBConnect();

            //Intializing the component needed to connect to DB such as password, un, host, etc
            dbc.Initialize();

            //Open connection to the database.
            dbc.OpenConnection();

            //create my sql command
            MySqlCommand cmd = new MySqlCommand();

            //Assign the connection using Connection
            cmd = dbc.connection.CreateCommand();
            
            //Store Method name to Method_name table
            string methodName = tbMethodName.Text;
            cmd.Parameters.AddWithValue("@name", methodName);
            cmd.CommandText = "INSERT INTO method_name(name)VALUES(@name)";
            cmd.ExecuteNonQuery();

            //Search for method name id
            cmd.CommandText = "SELECT id FROM method_name WHERE name LIKE '%" + tbMethodName.Text + "%'";
            methodIdObject = cmd.ExecuteScalar();

            //Write data each row from datagridview1 to each row in method_data database
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                try
                {
                    if (row.IsNewRow) continue;
                    //genereate query
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@method_id", methodIdObject);
                    cmd.Parameters.AddWithValue("@Step", row.Cells["Step"].Value);
                    cmd.Parameters.AddWithValue("@Torque", row.Cells["Torque"].Value);
                    cmd.Parameters.AddWithValue("@RPM", row.Cells["RPM"].Value);
                    cmd.Parameters.AddWithValue("@hour", row.Cells["hour"].Value);
                    cmd.Parameters.AddWithValue("@minute", row.Cells["minute"].Value);
                    cmd.Parameters.AddWithValue("@second", row.Cells["second"].Value);
                    cmd.Parameters.AddWithValue("@oil_temp", row.Cells["oil_temp"].Value);
                    cmd.Parameters.AddWithValue("@cool_temp", row.Cells["cool_temp"].Value);
                    //Assign the query using CommandText
                    cmd.CommandText = "INSERT INTO method_data(method_id, Step, Torque, RPM, hour, minute, second, oil_temp, cool_temp)VALUES(@method_id, @Step, @Torque, @RPM, @hour, @minute, @second, @oil_temp, @cool_temp)";
                    //Exceute query
                    cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            MessageBox.Show("Records Inserted.");
            //Close connection
            dbc.CloseConnection();
        }
              
    }
}

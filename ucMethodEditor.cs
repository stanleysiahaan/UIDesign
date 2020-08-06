using MySql.Data.MySqlClient;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UIDesign
{
    public partial class ucMethodEditor : UserControl
    {
        public object methodIdObject;
        progressWindow progressW;

        public ucMethodEditor()
        {
            InitializeComponent();
        }

        //To make 'Step' coloumn auto increment
        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            dgPositions.Rows[e.RowIndex].Cells[0].Value = (e.RowIndex + 1);
        }


        private async void btnSubmitMethod_Click(object sender, EventArgs e)
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
            int max = dgPositions.RowCount;
            int i = 0;
            int step = 0;
            //Showing progress bar.
            progressW = new progressWindow(this, max);
            progressW.Show();
            //Search for method name id
            await Task.Delay(1000);
            cmd.CommandText = "SELECT id FROM method_name WHERE name LIKE '%" + tbMethodName.Text + "%'";
            methodIdObject = cmd.ExecuteScalar();
            textBox1.Text = methodIdObject.ToString();
            //Write data each row from datagridview1 to each row in method_data database
            foreach (DataGridViewRow row in dgPositions.Rows)
            {
                progressW._progress = i;
                i++;
                step++;
                await Task.Delay(100);
                try
                {
                    if (row.IsNewRow) continue;
                    //genereate query
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@method_id", methodIdObject);
                    //cmd.Parameters.AddWithValue("@Step", row.Cells["Step"].Value);
                    cmd.Parameters.AddWithValue("@Step", step);
                    cmd.Parameters.AddWithValue("@Torque", row.Cells["Torque"].Value);
                    cmd.Parameters.AddWithValue("@RPM", row.Cells["RPM"].Value);
                    cmd.Parameters.AddWithValue("@hour", row.Cells["hour"].Value);
                    cmd.Parameters.AddWithValue("@minute", row.Cells["minute"].Value);
                    cmd.Parameters.AddWithValue("@second", row.Cells["second"].Value);
                    cmd.Parameters.AddWithValue("@ramp_time", row.Cells["ramp_time"].Value);
                    cmd.Parameters.AddWithValue("@oil_temp", row.Cells["oil_temp"].Value);
                    cmd.Parameters.AddWithValue("@cool_temp", row.Cells["cool_temp"].Value);
                    //Assign the query using CommandText
                    cmd.CommandText = "INSERT INTO method_data(method_id, Step, Torque, RPM, hour, minute, second, ramp_time, oil_temp, cool_temp)VALUES(@method_id, @Step, @Torque, @RPM, @hour, @minute, @second, @ramp_time, @oil_temp, @cool_temp)";
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

        private void button1_Click(object sender, EventArgs e)
        {
            string s = Clipboard.GetText();

            string[] lines = s.Replace("\n", "").Split('\r');

            dgPositions.Rows.Add(lines.Length - 1);
            string[] fields;
            int row = 0;
            int col = 1;

            foreach (string item in lines)
            {
                fields = item.Split('\t');
                foreach (string f in fields)
                {
                    Console.WriteLine(f);
                    dgPositions[col, row].Value = f;
                    col++;
                }
                row++;
                col = 1;
            }
        }
    }

}

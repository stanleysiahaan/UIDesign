using Client;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace UIDesign
{
    public partial class ucSettings : UserControl
    {
        //initializing the database
        DBConnect dbc = new DBConnect();

        public ucSettings()
        {
            InitializeComponent();
            try
            {
                //Calling IP saved in database
                dbc.Initialize();
                dbc.OpenConnection();
                //declaring all parameter
                MySqlDataAdapter da = new MySqlDataAdapter();
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
                DataTable dt3 = new DataTable();
                DataTable dt4 = new DataTable();
                //inserting IP and PORT to DAQ textbox field
                string query1 = "SELECT IP, Port FROM ipaddress WHERE device LIKE 'DAQ'";
                MySqlCommand cmd = new MySqlCommand(query1, dbc.connection);
                da.SelectCommand = cmd;
                da.Fill(dt1);
                tbDAQ.Text = dt1.Rows[0][0].ToString();
                tbDAQPort.Text = dt1.Rows[0][1].ToString();
                //inserting IP and PORT to Gateway textbox field
                string query2 = "SELECT IP, Port FROM ipaddress WHERE device LIKE 'GATEWAY'";
                da.SelectCommand.CommandText = query2;
                da.Fill(dt2);
                tbGateway.Text = dt2.Rows[0][0].ToString();
                tbGatewayPort.Text = dt2.Rows[0][1].ToString();
                //inserting IP and PORT to Oil Coolant textbox field
                string query3 = "SELECT IP, Port FROM ipaddress WHERE device LIKE 'MODBUS_OC'";
                da.SelectCommand.CommandText = query3;
                da.Fill(dt3);
                tbOilCoolant.Text = dt3.Rows[0][0].ToString();
                tbOilCoolantPort.Text = dt3.Rows[0][1].ToString();
                //inserting IP and PORT to Water Coolant textbox field
                string query4 = "SELECT IP, Port FROM ipaddress WHERE device LIKE 'MODBUS_WC'";
                da.SelectCommand.CommandText = query4;
                da.Fill(dt4);
                tbWaterCoolant.Text = dt4.Rows[0][0].ToString();
                tbWaterCoolantPort.Text = dt4.Rows[0][1].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dbc.CloseConnection();
        }

        private void btnCallClient_Click(object sender, EventArgs e)
        {
            ConnectionTestForm form1 = new ConnectionTestForm();
            form1.Show();
        }

        private void btnSaveIP_Click(object sender, EventArgs e)
        {
            dbc.Initialize();
            dbc.OpenConnection();
            MySqlCommand cmd = new MySqlCommand();
            cmd = dbc.connection.CreateCommand();
            try
            {
                //Inserting TEXCEL IP to database
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@IP", tbGateway.Text);
                cmd.Parameters.AddWithValue("@Port", tbGatewayPort.Text);
                cmd.Parameters.AddWithValue("@device", "GATEWAY");
                cmd.CommandText = "INSERT INTO ipaddress (IP, Port, device) VALUES (@IP, @Port, @device) ON duplicate KEY UPDATE IP=@IP, Port=@Port";
                cmd.ExecuteNonQuery();
                //Inserting Oil Coolant IP to database
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@IP", tbOilCoolant.Text);
                cmd.Parameters.AddWithValue("@Port", tbOilCoolantPort.Text);
                cmd.Parameters.AddWithValue("@device", "MODBUS_OC");
                cmd.CommandText = "INSERT INTO ipaddress (IP, Port, device) VALUES (@IP, @Port, @device) ON duplicate KEY UPDATE IP=@IP,Port=@Port";
                cmd.ExecuteNonQuery();
                //Inserting Water Coolant IP to database
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@IP", tbWaterCoolant.Text);
                cmd.Parameters.AddWithValue("@Port", tbWaterCoolantPort.Text);
                cmd.Parameters.AddWithValue("@device", "MODBUS_WC");
                cmd.CommandText = "INSERT INTO ipaddress (IP, Port, device) VALUES (@IP, @Port, @device) ON duplicate KEY UPDATE IP=@IP,Port=@Port";
                cmd.ExecuteNonQuery();
                //Inserting DAQ IP to databse
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@IP", tbDAQ.Text);
                cmd.Parameters.AddWithValue("@Port", tbDAQPort.Text);
                cmd.Parameters.AddWithValue("@device", "DAQ");
                cmd.CommandText = "INSERT INTO ipaddress (IP, Port, device) VALUES (@IP, @Port, @device) ON duplicate KEY UPDATE IP=@IP,Port=@Port";
                cmd.ExecuteNonQuery();
                MessageBox.Show("IP Saved");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dbc.CloseConnection();
        }
    }
}

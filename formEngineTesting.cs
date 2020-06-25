using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Timers;


namespace UIDesign
{    
    public partial class formEngineTesting : Form
    {
        int i;
        Stopwatch sw1 = new Stopwatch();
        System.Timers.Timer Timer1;
        System.Timers.Timer Timer2;
        DBConnect dbc = new DBConnect();
        connectionProtocol connectionProtocol = new connectionProtocol();
        TexcelCommand texcelCommand;
        functionASCII fncascii;
        string elapsed_time;
        //Declare all the IP
        string IPTexcel;
        string PortTexcel;
        string IPOilCoolant;
        string PortOilCoolant;
        string IPWaterCoolant;
        string PortWaterCoolant;
        string IPDAQ;
        string PortDAQ;

        public formEngineTesting(ucChooseProject ucCP, string projectID)
        {
            InitializeComponent();

            //Initializing the database
            dbc.Initialize();
            dbc.OpenConnection();

            //Obtaining the method_id and show it on rtbLogging
            MySqlCommand cmd = new MySqlCommand();
            cmd = dbc.connection.CreateCommand();
            cmd.CommandText = "SELECT method_id FROM project_data WHERE id LIKE '%" + projectID + "%'";
            object methodID = cmd.ExecuteScalar();
            rtbLogging.Text = 
                "Your Project id is: " + projectID + "\r\n" + 
                "Your method id is: " + methodID.ToString() +"\r\n";

            //Show the parameter on the datagridview
            string query = "SELECT * FROM method_data WHERE method_id LIKE '%" + methodID + "%'";
            try
            {
                DataTable datatable = new DataTable();
                DataSet dataset = new DataSet();
                dataset.Tables.Add(datatable);
                MySqlDataAdapter dataadapter = new MySqlDataAdapter(query, dbc.connection);
                dataadapter.Fill(datatable);
                dataGridView1.DataSource = datatable;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            dbc.CloseConnection();

            //Converting hours:minute:second to duration(second)
            int hour; int minute; int second;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                hour = (int)dataGridView1.Rows[i].Cells["hour"].Value;
                minute = (int)dataGridView1.Rows[i].Cells["minute"].Value;
                second = (int)dataGridView1.Rows[i].Cells["second"].Value;
                int _second = (hour * 3600) + (minute * 60) + second;
                dataGridView1.Rows[i].Cells["second"].Value = _second;
            }

            //Deleting hour and minute coloumn
            dataGridView1.Columns["id"].Visible = false;
            dataGridView1.Columns["method_id"].Visible = false;
            dataGridView1.Columns["hour"].Visible = false;
            dataGridView1.Columns["minute"].Visible = false;
            dataGridView1.Columns["second"].Name = "duration";

            //OBTAINING ALL THE DEVICES' IP
            //declaring all parameter
            MySqlDataAdapter da = new MySqlDataAdapter();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            DataTable dt4 = new DataTable();
            //inserting IP and PORT to DAQ textbox field            
            string query1 = "SELECT IP, Port FROM ipaddress WHERE device LIKE 'DAQ'";
            cmd = dbc.connection.CreateCommand();
            cmd.CommandText = query1;
            da.SelectCommand = cmd;
            da.Fill(dt1);
            IPDAQ = dt1.Rows[0][0].ToString();
            PortDAQ = dt1.Rows[0][1].ToString();
            //inserting IP and PORT to Gateway textbox field
            string query2 = "SELECT IP, Port FROM ipaddress WHERE device LIKE 'GATEWAY'";
            da.SelectCommand.CommandText = query2;
            da.Fill(dt2);
            IPTexcel = dt2.Rows[0][0].ToString();
            rtbLogging.AppendText("IP TEXCEL" + IPTexcel + "\r\n");
            PortTexcel = dt2.Rows[0][1].ToString();
            rtbLogging.AppendText("PORT TEXCEL" + PortTexcel + "\r\n");
            //inserting IP and PORT to Oil Coolant textbox field
            string query3 = "SELECT IP, Port FROM ipaddress WHERE device LIKE 'MODBUS_OC'";
            da.SelectCommand.CommandText = query3;
            da.Fill(dt3);
            IPOilCoolant = dt3.Rows[0][0].ToString();
            PortOilCoolant = dt3.Rows[0][1].ToString();
            //inserting IP and PORT to Water Coolant textbox field
            string query4 = "SELECT IP, Port FROM ipaddress WHERE device LIKE 'MODBUS_WC'";
            da.SelectCommand.CommandText = query4;
            da.Fill(dt4);
            IPWaterCoolant = dt4.Rows[0][0].ToString();
            PortWaterCoolant = dt4.Rows[0][1].ToString();
        }

        private void formEngineTesting_Load(object sender, EventArgs e)
        {
        }

        //Sending and logging trigger
        private void commandTimer(object sender, EventArgs e)
        {
            //obtaining torque, rpm and duration value
            string torque = dataGridView1.Rows[i].Cells["Torque"].Value.ToString();
            string rpm = dataGridView1.Rows[i].Cells["RPM"].Value.ToString();
            string duration = dataGridView1.Rows[i].Cells["duration"].Value.ToString();
            string ramp_time = dataGridView1.Rows[i].Cells["ramp_time"].Value.ToString();

            //send those parameters to texcel command
            texcelCommand = new TexcelCommand(torque, rpm, duration, ramp_time);
            string _command = texcelCommand.TorqueThrottle();

            //Printing the commmand to rtblogging
            rtbLogging.AppendText(String.Format("[{1}] : {0} \r\n", _command, elapsed_time));
            rtbLogging.ScrollToCaret();
            i++;

            //Sending the command to functionascii
            fncascii = new functionASCII(_command);

            //Printing the final command
            string cmdFinal = fncascii.commandbuilder();
            rtbLogging.AppendText(String.Format("F: {0}", cmdFinal));

            //Sending command through IP
            string receivePacket = connectionProtocol.SendCommandTexcelSocket(cmdFinal, IPTexcel, PortTexcel); // change this method from SendCommandTexcel to SendCommandTexcelSocket.
            richTextBox1.AppendText(String.Format("[{1}]Respond: {0}\r\n", receivePacket, elapsed_time));

            //Stop sending command
            if (i == dataGridView1.Rows.Count)
            {
                Timer1.Enabled = false;
                //sw1.Stop();
                rtbLogging.AppendText("Your command is done\r\n");
            }
        }

        //Timer elapsed trigger
        private void stopwatch_command (object sender, EventArgs e)
        {
            elapsed_time = DateTime.Now.ToString("hh:mm:ss.fff tt");
            textBox8.Text = elapsed_time;
            //Creating random float number for tachometer
            var rand = new Random();
            double a = rand.NextDouble() * (5 - 4.8) + 4.8;
            aGauge1.Value = (float)a;
            textBox1.Text = a.ToString("###,###.00");
        }

        //Start button trigger
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (toggleSwitch1.Checked == true)
            {
                //Reseting the index if STOP button is clicked
                i = 0;

                //Declaring all the timer and stopwatch
                Timer1 = new System.Timers.Timer(5000);
                Timer1.Elapsed += new ElapsedEventHandler(commandTimer);
                Timer1.AutoReset = true;
                Timer2 = new System.Timers.Timer(1);
                Timer2.Elapsed += new ElapsedEventHandler(stopwatch_command);
                Timer2.AutoReset = true;

                //Starting the timer and stopwatch
                sw1.Start();
                Timer2.Enabled = true;
                Timer1.Enabled = true;

                //Synchronize the timer with the textbox
                Timer1.SynchronizingObject = this;
                Timer2.SynchronizingObject = this;

                //Get rpm and torque value from texcel
                requestDataTexcel();
            }
            else
            {
                MessageBox.Show("CANNOT START THE TEST! \r\nConnection to all equipment has not established!");
            }
        }

        //Stop button trigger
        private void btnStop_Click(object sender, EventArgs e)
        {
                Timer1.Stop();
                Timer2.Stop();
                sw1.Stop();            
        }

        //Menganti REDline
        private void aGauge1_ValueInRangeChanged(object sender, ValueInRangeChangedEventArgs e)
        {
           

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            //float a = trackBar1.Value;
            //a = a / 100;
            //aGauge1.Value = sw1.ElapsedMilliseconds;
            //textBox1.Text = sw1.ElapsedMilliseconds.ToString();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            aGauge2.Value = trackBar2.Value;
            textBox2.Text = trackBar2.Value.ToString();
        }

        public void trackBar3_Scroll (object sender, EventArgs e)
        {
            aGauge3.Value = trackBar3.Value;
            textBox6.Text = trackBar3.Value.ToString();
        }
        public void trackBar4_Scroll(object sender, EventArgs e)
        {
            aGauge4.Value = trackBar4.Value;
            textBox7.Text = trackBar4.Value.ToString();
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnMode_Click(object sender, EventArgs e)
        {
            texcelCommand = new TexcelCommand(null,null,null, null);
            string tohost = texcelCommand.HostControl();           
            //Sending the command to functionascii
            fncascii = new functionASCII(tohost);

            //Printing the final command
            string cmdFinal = fncascii.commandbuilder();
            rtbLogging.AppendText(String.Format("F: {0}", cmdFinal));

            //Sending command through IP
            var packet = connectionProtocol.SendHostControlSocket(cmdFinal, IPTexcel, PortTexcel);
            string sentPacket = packet.Item1;
            string receivedPacket = packet.Item2;
            richTextBox1.AppendText(String.Format("[{1}]Sent: {0} \r\n [{1}]Received: {2}\r\n", sentPacket, DateTime.Now.ToString("hh:mm:ss.fff tt"), receivedPacket));
            if (receivedPacket == "R19,1,E,\r")
            {
                //toggleSwitch1_CheckedChanged(sender, e);
                toggleSwitch1.Checked = true;
            }
            else if (receivedPacket == "R19,2,E,\r")
            {
                toggleSwitch1.Checked = false;
            }
            else
            {
                MessageBox.Show("Texcel not connected! Check IP or contact admin.");
            }
        }

        private void requestDataTexcel()
        {
            texcelCommand = new TexcelCommand(null, null, null, null);
            string requestRpmTorque = texcelCommand.TorqueRpmRequest();
            fncascii = new functionASCII(requestRpmTorque);
            string cmdFinal = fncascii.commandbuilder();
            rtbLogging.AppendText(String.Format("cmd: {0}", cmdFinal));
            string receivePacket = connectionProtocol.SendCommandTexcelSocket(cmdFinal, IPTexcel, PortTexcel);
            richTextBox1.AppendText(String.Format("[{1}]Respond: {0} \r\n", receivePacket, elapsed_time));
        }
    }
}

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
using System.Net.Sockets;
using System.Net;

namespace UIDesign
{
    public partial class formEngineTesting : Form
    {
        //Class to be used in all function.
        Stopwatch sw1 = new Stopwatch();
        System.Timers.Timer Timer1;
        System.Timers.Timer Timer2;
        DBConnect dbc = new DBConnect();
        TexcelCommand texcelCommand;
        functionASCII fncascii;
        Thread threadReceiveData;
        TcpClient client;
        NetworkStream stream;

        //Declare all the global variables
        public string IPTexcel;
        public string PortTexcel;
        string IPOilCoolant;
        string PortOilCoolant;
        string IPWaterCoolant;
        string PortWaterCoolant;
        string IPDAQ;
        string PortDAQ;
        private string _checkChanges;
        public string textToSend;
        public string cmdFinal;
        public string textReceived;
        int i;
        string elapsed_time;

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
             2q        int _second = (hour * 3600) + (minute * 60) + second;
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
            i++;
            //Build the final command
            fncascii = new functionASCII(_command);
            cmdFinal = fncascii.commandbuilder();
            //Sending command through IP
            try
            {
                byte[] bytesToSend = Encoding.ASCII.GetBytes(cmdFinal);
                //send the data through the socket
                stream.Write(bytesToSend, 0, bytesToSend.Length);
                rtbLogging.AppendText(String.Format("[{1}]Sent: {0}", cmdFinal, elapsed_time));
                rtbLogging.ScrollToCaret();
            }
            catch (Exception ex)
            {
                rtbLogging.AppendText(ex.Message);
                rtbLogging.ScrollToCaret();
            }
            stream.Flush();
            //Stop sending command
            if (i == dataGridView1.Rows.Count)
            {
                Timer1.Enabled = false;
                rtbLogging.AppendText("Your command is done\r\n");
                rtbLogging.ScrollToCaret();
            }
        }

        //Time elapsed trigger
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

        //Change the red line REDline. Better put in on the settings menu
        private void aGauge1_ValueInRangeChanged(object sender, ValueInRangeChangedEventArgs e)
        {

        }

        //Change the texcel to host control mode
        private void btnMode_Click(object sender, EventArgs e)
        {
            // Start the thread to listen
            threadReceiveData = new Thread(new ThreadStart(startReceiving));
            threadReceiveData.IsBackground = true;
            threadReceiveData.Start();

            //Building the command
            texcelCommand = new TexcelCommand(null,null,null, null);
            string tohost = texcelCommand.HostControl();           
            fncascii = new functionASCII(tohost);
            string cmdFinal = fncascii.commandbuilder();
            rtbLogging.AppendText(String.Format("Sent: {0}", cmdFinal));
            //Sending command through IP
            byte[] bytesToSend = Encoding.ASCII.GetBytes(cmdFinal);
            //send the data through the socket
            stream.Write(bytesToSend, 0, bytesToSend.Length);
            stream.Flush();
        }

        private void requestDataTexcel()
        {
            texcelCommand = new TexcelCommand(null, null, null, null);
            string requestRpmTorque = texcelCommand.TorqueRpmRequest();
            fncascii = new functionASCII(requestRpmTorque);
            string cmdFinal = fncascii.commandbuilder();
            rtbLogging.AppendText(String.Format("Sent: {0}", cmdFinal));
            //Sending command through IP
            byte[] bytesToSend = Encoding.ASCII.GetBytes(cmdFinal);
            //send the data through the socket
            stream.Write(bytesToSend, 0, bytesToSend.Length);
            stream.Flush();
        }


        //Button to start the thread for receiving data.
        public void StartThreading_Click(object sender, EventArgs e)
        {
        }

        //Recieving in backgroud method.
        public void startReceiving()
        {
            //Data buffer
            byte[] buffer = new byte[1024];
            //Create integer to hold how large the data received is
            Int32 bytesReceived;
            //Loop to continously reading data while the client is connected
            while (client.Connected)//client.Connected
            {
                try
                {
                    //receive the response from the remote device
                    bytesReceived = stream.Read(buffer, 0, buffer.Length);
                    if (bytesReceived > 0)
                    {
                        textReceived = Encoding.ASCII.GetString(buffer, 0, bytesReceived);
                    }

                    //Check the received text and do suitable command
                    if (textReceived == "R19,1,E,\r") //Host control successfully established
                    {
                        toggleSwitch1.Invoke((Action)delegate
                        {
                            toggleSwitch1.Checked = true;
                        });
                        richTextBox1.Invoke((Action)delegate
                        {
                            richTextBox1.AppendText(string.Format("[{1}]Respond: {0}", textReceived, elapsed_time));
                        }
                        );
                    }
                    else if (textReceived == "R19,2,E,\r")//Host control not success
                    {
                        toggleSwitch1.Invoke((Action)delegate
                        {
                            toggleSwitch1.Checked = false;
                            MessageBox.Show("Host control failed. \r\n Check the command.");
                        });
                    }
                    else
                    richTextBox1.Invoke((Action)delegate
                    {
                        richTextBox1.AppendText(string.Format("[{1}]Respond: {0}", textReceived, elapsed_time));
                    }
                    );
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
                stream.Flush();
            }         
        }

        //Establish the connection to the server from client.
        public void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {                
                IPAddress IPadd = IPAddress.Parse(IPTexcel);
                client = new TcpClient();
                client.Connect(IPTexcel, int.Parse(PortTexcel));
                stream = client.GetStream();
                rtbLogging.AppendText("Conected to server.....\r");
            }
            catch (Exception ex)
            {
                rtbLogging.AppendText(ex.Message + "\r\n");
            }

        }
    }
}

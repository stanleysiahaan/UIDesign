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
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Xml.Serialization;

namespace UIDesign
{
    public partial class formEngineTesting : Form
    {
        //Class to be used in all function.
        Stopwatch sw1 = new Stopwatch();
        Stopwatch sw2 = new Stopwatch();
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
        public string textToSend;
        public string cmdFinal;
        public string textReceived;
        int i;
        string elapsed_time;
        string actualCondition;
        double _RPM;
        double _Torque;
        string duration;
        string data_time_stamp;
        string _projectID { get; set; }

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
                "Your method id is: " + methodID.ToString() +"\r\n" +
                "Your project id is: " + projectID +"\r\n";
            _projectID = projectID;

            //Show the parameter on the datagridview
            string query = "SELECT * FROM method_data WHERE method_id LIKE '%" + methodID + "%'";
            try
            {
                DataTable datatable = new DataTable();
                DataSet dataset = new DataSet();
                dataset.Tables.Add(datatable);
                MySqlDataAdapter dataadapter = new MySqlDataAdapter(query, dbc.connection);
                dataadapter.Fill(datatable);
                dgvDemand.DataSource = datatable;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            dbc.CloseConnection();

            //Converting hours:minute:second to duration(second)
            int hour; int minute; int second;
            for (int i = 0; i < dgvDemand.Rows.Count; i++)
            {
                hour = (int)dgvDemand.Rows[i].Cells["hour"].Value;
                minute = (int)dgvDemand.Rows[i].Cells["minute"].Value;
                second = (int)dgvDemand.Rows[i].Cells["second"].Value;
                int _second = (hour * 3600) + (minute * 60) + second;
                dgvDemand.Rows[i].Cells["second"].Value = _second;
            }

            //Deleting hour and minute coloumn
            dgvDemand.Columns["id"].Visible = false;
            dgvDemand.Columns["method_id"].Visible = false;
            dgvDemand.Columns["hour"].Visible = false;
            dgvDemand.Columns["minute"].Visible = false;
            dgvDemand.Columns["second"].Name = "duration";

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

            //Declaring all the timer and stopwatch
            Timer1 = new System.Timers.Timer();
            Timer1.Elapsed += new ElapsedEventHandler(commandTimer);
            Timer1.AutoReset = true;
            Timer2 = new System.Timers.Timer(1);
            Timer2.Elapsed += new ElapsedEventHandler(stopwatch_command);
            Timer2.AutoReset = true;
        }

        private void formEngineTesting_Load(object sender, EventArgs e)
        {

        }

        //Sending and logging trigger
        private void commandTimer(object sender, EventArgs e)
        {
            //obtaining torque, rpm and duration value
            string torque = dgvDemand.Rows[i].Cells["Torque"].Value.ToString();
            string rpm = dgvDemand.Rows[i].Cells["RPM"].Value.ToString();
            duration = dgvDemand.Rows[i].Cells["duration"].Value.ToString();
            string ramp_time = dgvDemand.Rows[i].Cells["ramp_time"].Value.ToString();
            //send those parameters to texcel command
            texcelCommand = new TexcelCommand(torque, rpm, duration, ramp_time);
            string _command = texcelCommand.TorqueThrottle();
            //Showing stage in textbox3
            textBox4.Text = (i+1).ToString();
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
            if (i == dgvDemand.Rows.Count)
            {
                Timer1.Enabled = false;
                rtbLogging.AppendText("Your command is done\r\n");
                rtbLogging.ScrollToCaret();
            }
            Timer1.Stop();
            Timer1.Interval = int.Parse(duration) * 1000;
            Timer1.Start();
        }

        //Time elapsed trigger
        private void stopwatch_command (object sender, EventArgs e)
        {       
            textBox8.Invoke((Action)delegate
            {
                textBox8.Text = sw1.Elapsed.ToString("hh\\:mm\\:ss\\.ff");
            });
        }

        //Start button trigger
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (toggleSwitch1.Checked == true)
            {
                //Reseting the index if STOP button is clicked
                i = 1;
                //Immidiately send the first command
                sendFirstCommand();
                //Starting the timer
                Timer2.Enabled = true;                
                Timer1.Interval = int.Parse(duration)*1000;
                Timer1.Enabled = true;
                //Starting the stopwatch
                sw1.Start();
                //Synchronize the timer object with the main thread
                Timer1.SynchronizingObject = this;
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

        private void sendFirstCommand()
        {
            //obtaining torque, rpm and duration value
            string torque = dgvDemand.Rows[0].Cells["Torque"].Value.ToString();
            string rpm = dgvDemand.Rows[0].Cells["RPM"].Value.ToString();
            duration = dgvDemand.Rows[0].Cells["duration"].Value.ToString();
            string ramp_time = dgvDemand.Rows[0].Cells["ramp_time"].Value.ToString();
            //send those parameters to texcel command
            texcelCommand = new TexcelCommand(torque, rpm, duration, ramp_time);
            string _command = texcelCommand.TorqueThrottle();
            //Showing stage in textbox3
            textBox4.Text = "1";
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
            while (client.Connected)
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
                            richTextBox1.ScrollToCaret();
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
                    else if (textReceived.IndexOf("D2")>-1)
                    {
                        actualCondition = textReceived;
                        string[] _actualCondition = actualCondition.Split(',');
                        _RPM = double.Parse(_actualCondition[1]);
                        _Torque = double.Parse(_actualCondition[2]);
                        aGauge1.Invoke((Action)delegate
                        {
                            aGauge1.Value = (float)_RPM/1000;
                        });
                        textBox1.Invoke((Action)delegate
                        {
                            textBox1.Text = _RPM.ToString();
                        });
                        richTextBox1.Invoke((Action)delegate
                        {
                            data_time_stamp = DateTime.Now.ToString("yyyy-MM-dd H:mm:ss");
                            richTextBox1.AppendText(string.Format("[{1}]Respond: {0}", textReceived, data_time_stamp));
                            richTextBox1.ScrollToCaret();
                        });                        
                        aGauge2.Invoke((Action)delegate
                        {
                            aGauge2.Value = (float)_Torque;
                        });
                        textBox2.Invoke((Action)delegate
                        {
                            textBox2.Text = _Torque.ToString();
                        });
                        richTextBox1.Invoke((Action)delegate
                        {
                            richTextBox1.AppendText(string.Format("[{1}]Respond: {0}", textReceived, elapsed_time));
                            richTextBox1.ScrollToCaret();
                        });
                        //Sending the data to the database
                        sendResponseToDatabase(_RPM, _Torque);
                    }
                    else
                    richTextBox1.Invoke((Action)delegate
                    {
                        richTextBox1.AppendText(string.Format("[{1}]Respond: {0}", textReceived, elapsed_time));
                        richTextBox1.ScrollToCaret();
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

        public void sendResponseToDatabase(double actualRPM, double actualTorque)
        {
            try
            {
                dbc.Initialize();
                dbc.OpenConnection();

                MySqlCommand cmd = new MySqlCommand();
                cmd = dbc.connection.CreateCommand();

                data_time_stamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@project_id", _projectID);
                cmd.Parameters.AddWithValue("@time_stamp", data_time_stamp);
                cmd.Parameters.AddWithValue("@speed", actualRPM);
                cmd.Parameters.AddWithValue("@torque", actualTorque);
                cmd.CommandText = "INSERT INTO testing_result(project_id, time_stamp, speed, torque)VALUES (@project_id, @time_stamp, @speed, @torque)";
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }     

        }
    }
}

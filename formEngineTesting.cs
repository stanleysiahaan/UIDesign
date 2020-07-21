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
using MySqlX.XDevAPI.Relational;
using EasyModbus;
using System.IO;

namespace UIDesign
{
    public partial class formEngineTesting : Form
    {
        //Class to be used in all function.
        Stopwatch sw1 = new Stopwatch();
        Stopwatch sw2 = new Stopwatch();
        System.Timers.Timer Timer1;
        System.Timers.Timer Timer2;
        DateTime timestamp = DateTime.Now;
        //System.Threading.Timer Timer3;
        DBConnect dbc = new DBConnect();
        TexcelCommand texcelCommand = new TexcelCommand();
        functionASCII fncascii = new functionASCII();
        TcpClient client;
        NetworkStream stream;
        //MySqlDataAdapter dataAdapterResult;
        //DataTable dataTableResult;
        //Thread tableDisplayThread;
        Thread threadReceiveData;
        texcelRespondProcessor respondProcessor = new texcelRespondProcessor();
        ModbusClient modbusDAQ;
        ModbusClient modbusOC;
        ModbusClient modbusWC;

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
        int i = 0;
        string actualCondition;
        double _RPM;
        double _Torque;
        string duration;
        string ramp_time;
        //string data_time_stamp;
        int[] coolantTemp;
        int[] lubricantTemp;
        string _projectID { get; set; }
        int totalDemandDuration;
        int totalRampDuration;
        int totalDuration;
        int progressBar1Second = 0;
        int progressBar2Second = 0;
        string[] explodedResponse = null;
        string torque;
        string rpm;
        //Declare the flag
        bool isTesting = false; //FlagforDisplaingDataonTable

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
        //rtbLogging.Text = 
        //    "Your Project id is: " + projectID + "\r\n" + 
        //    "Your method id is: " + methodID.ToString() +"\r\n" +
        //    "Your project id is: " + projectID +"\r\n";
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
        dgvDemand.ClearSelection(); 

        //------------------------OBTAINING ALL THE DEVICES' IP--------------------//
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
        rtbLogging.AppendText("IP PLC Water Coolant" + IPWaterCoolant + "\r\n");
        PortWaterCoolant = dt4.Rows[0][1].ToString();
        rtbLogging.AppendText("PORT PLC Water Coolant" + PortWaterCoolant + "\r\n");

        //Declaring all the timer and stopwatch
        Timer1 = new System.Timers.Timer();
        Timer1.Elapsed += new ElapsedEventHandler(commandTimer);
        //Timer1.AutoReset = true;
        Timer2 = new System.Timers.Timer(1000);
        Timer2.Elapsed += new ElapsedEventHandler(stopwatch_command);
        //Timer2.AutoReset = true;
        dbc.CloseConnection();

        //Calculating total duration
        for (int j=0; j < (int)dgvDemand.RowCount; j++)
        {
            totalDemandDuration += (int)dgvDemand.Rows[j].Cells["duration"].Value;
            totalRampDuration += (int)dgvDemand.Rows[j].Cells["ramp_time"].Value;
        }
        totalDuration = totalDemandDuration + totalRampDuration;
        pbTimeElapsed.Maximum = totalDuration;
        rtbLogging.AppendText(String.Format("Total Duration: {0} s\r", totalDuration));

        //Disabling the mode button.
        btnMode.Enabled = false;
        btnStart.Enabled = false;
    }

        private void formEngineTesting_Load(object sender, EventArgs e)
        {
            // Declare all the thread
            threadReceiveData = new Thread(startReceiving);
            threadReceiveData.IsBackground = true;
            threadReceiveData.Name = "Data receiving thread";
        }

        //Sending and logging trigger
        private void commandTimer(object sender, EventArgs e)
        {
            if (i < dgvDemand.Rows.Count)
            {
                //Higihlighting the current demand
                dgvDemand.ClearSelection();
                dgvDemand.Rows[i].Selected = true;
                torque = dgvDemand.Rows[i].Cells["Torque"].Value.ToString();
                rpm = dgvDemand.Rows[i].Cells["RPM"].Value.ToString();
                duration = dgvDemand.Rows[i].Cells["duration"].Value.ToString();
                ramp_time = dgvDemand.Rows[i].Cells["ramp_time"].Value.ToString();
                //send those parameters to texcel command               
                string _command = texcelCommand.TorqueThrottle(torque, rpm, duration, ramp_time);
                //Showing stage in textbox3
                textBox4.Text = i.ToString();                
                //Build the final command
                cmdFinal = fncascii.commandbuilder(_command);
                //Sending command through IP
                try
                {
                    byte[] bytesToSend = Encoding.ASCII.GetBytes(cmdFinal);
                    //send the data through the socket
                    stream.Write(bytesToSend, 0, bytesToSend.Length);
                    rtbLogging.AppendText(String.Format("[{1}]Sent: {0}", cmdFinal, timestamp));
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
                    btnStop_Click(sender, e);
                    rtbLogging.AppendText("Your command is done\r\n");
                    rtbLogging.ScrollToCaret();                    
                }
                sw2.Restart();
                Timer1.Stop();
                Timer1.Interval = (int.Parse(duration)) * 1000;
                Timer1.Start();
                pbStage.Value = 0;
                progressBar2Second = 0;
                pbStage.Maximum = int.Parse(duration);
                i++;
            }
            else
            {
                btnStop_Click(sender, e);
                MessageBox.Show("Your test is done!");
            }
            //obtaining torque, rpm and duration value

        }

        //Time elapsed trigger
        private void stopwatch_command (object sender, EventArgs e)
        {
            try
            {
                coolantTemp = modbusWC.ReadInputRegisters(1, 1);
                lubricantTemp = modbusOC.ReadInputRegisters(1, 1);
            }
            catch (Exception ex)
            {
                rtbLogging.AppendText(String.Format("[PLC Error]: {0}", ex.Message));
            }

            this.Invoke((Action)delegate
            {
                pbStage.CustomText = sw2.Elapsed.ToString("hh\\:mm\\:ss");
                pbStage.Value = progressBar2Second;
                pbTimeElapsed.CustomText = sw1.Elapsed.ToString("hh\\:mm\\:ss");
                pbTimeElapsed.Value = progressBar1Second;
                textBox6.Text = coolantTemp[0].ToString();
                aGauge3.Value = coolantTemp[0];
                textBox7.Text = lubricantTemp[0].ToString();
                aGauge4.Value = lubricantTemp[0];
            });
            progressBar1Second += 1;
            progressBar2Second += 1;
        }

        //--------COMMAND FOR THE START, STOP, PAUSE, AND MODE BUTTON-------------------------//
        //Start button trigger
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (toggleSwitch1.Checked == true)
            {
                if (i < 1)
                {
                    //Tell the programme wheter the testing on progress or not
                    isTesting = true;
                    //Reseting the index if STOP button is clicked
                    i = 1;
                    //Immidiately send the first command
                    sendFirstCommand();
                    //Starting DAQ record
                    startRecordDAQ();
                    //Starting the timer
                    Timer2.Enabled = true;
                    Timer1.Interval = int.Parse(duration) * 1000;
                    Timer1.Enabled = true;
                    //Starting the stopwatch
                    sw1.Start();
                    sw2.Start();
                    //Synchronize the timer object with the main thread
                    Timer1.SynchronizingObject = this;
                    //Get rpm and torque value from texcel
                    requestDataTexcel();
                    //Set the pbStage maximum value
                    pbStage.Maximum = (int.Parse(duration) + int.Parse(ramp_time));
                }
                else
                {
                    //Tell the programme wheter the testing on progress or not
                    isTesting = true;
                    //Immidiately send the first command
                    sendFirstCommand();
                    //Starting the timer
                    Timer2.Enabled = true;
                    Timer1.Interval = int.Parse(duration) * 1000;
                    Timer1.Enabled = true;
                    //Starting the stopwatch
                    sw1.Start();
                    sw2.Start();
                    //Synchronize the timer object with the main thread
                    Timer1.SynchronizingObject = this;
                    //Set the pbStage maximum value
                    pbStage.Maximum = (int.Parse(duration) + int.Parse(ramp_time));
                }
            }
            else
            {
                MessageBox.Show("CANNOT START THE TEST! \r\nConnection to all equipment has not established!");
            }
        }

        //Pause button trigger
        private void btnPause_Click(object sender, EventArgs e)
        {
            pauseRecordDAQ();
            Timer1.Stop();
            Timer2.Stop();
            sw1.Stop();
            sw2.Stop();
        }

        //Stop button trigger
        private void btnStop_Click(object sender, EventArgs e)
        {           
            string clearRequest = texcelCommand.clearDataRequest();
            string clearDemand = texcelCommand.clearDemandQueue();
            string toManualControl = texcelCommand.ManualControl();
            string cmdFinal1 = clearRequest;
            string cmdFinal2 = fncascii.commandbuilder(clearDemand);
            string cmdFinal3 = fncascii.commandbuilder(toManualControl);
            try
            {
                byte[] bytesToSend = Encoding.ASCII.GetBytes(cmdFinal1);
                stream.Write(bytesToSend, 0, bytesToSend.Length);
                rtbLogging.AppendText(String.Format("[Clear Resqesting]Sent: {0}", cmdFinal1));
                bytesToSend = Encoding.ASCII.GetBytes(cmdFinal2);
                stream.Write(bytesToSend, 0, bytesToSend.Length);
                rtbLogging.AppendText(String.Format("[Clear Demand]Sent: {0}", cmdFinal2));
                bytesToSend = Encoding.ASCII.GetBytes(cmdFinal3);
                stream.Write(bytesToSend, 0, bytesToSend.Length);
                rtbLogging.AppendText(String.Format("[To Manual Control]Sent: {0}", cmdFinal3));
            }
            catch (Exception ex)
            {
                rtbLogging.AppendText(ex.Message);
                rtbLogging.ScrollToCaret();
            }
            isTesting = false;
            Timer1.Stop();
            Timer2.Stop();
            sw1.Stop();
            sw2.Stop();
            stopRecordDAQ();
            //Disconnecting to DAQ
            modbusDAQ.Disconnect();
            toggleSwitch3.Checked = false;
            rtbLogging.AppendText("STOP TESTING");
        }

        //Change the texcel to host control mode
        private void btnMode_Click(object sender, EventArgs e)
        {
            //Building the command           
            string tohost = texcelCommand.HostControl();
            string clearDemandCommand = texcelCommand.clearDemandQueue();            
            string cmdFinal1 = fncascii.commandbuilder(tohost);
            string cmdFinal2 = fncascii.commandbuilder(clearDemandCommand);
            rtbLogging.AppendText(String.Format("[{1}]Sent: {0}", cmdFinal2, timestamp));
            byte[] bytesToSend2 = Encoding.ASCII.GetBytes(cmdFinal2);
            stream.Write(bytesToSend2, 0, bytesToSend2.Length);
            rtbLogging.AppendText(String.Format("[{1}]Sent: {0}", cmdFinal1, timestamp));
            byte[] bytesToSend1 = Encoding.ASCII.GetBytes(cmdFinal1);
            stream.Write(bytesToSend1, 0, bytesToSend1.Length);
        }

        //--------SENDING AND RECIEVING THE COMMAND TO TEXCEL-------------------------//
        private void sendFirstCommand()
        {
            //Highlighting the current demand
            dgvDemand.ClearSelection();
            dgvDemand.Rows[0].Selected = true;
            //obtaining torque, rpm and duration value
            torque = dgvDemand.Rows[0].Cells["Torque"].Value.ToString();
            rpm = dgvDemand.Rows[0].Cells["RPM"].Value.ToString();
            duration = dgvDemand.Rows[0].Cells["duration"].Value.ToString();
            ramp_time = dgvDemand.Rows[0].Cells["ramp_time"].Value.ToString();
            //send those parameters to texcel command
            string _command = texcelCommand.TorqueThrottle(torque, rpm, duration, ramp_time);
            //Showing stage in textbox3
            textBox4.Text = "1";
            //Build the final command
            cmdFinal = fncascii.commandbuilder(_command);
            //Sending command through IP
            try
            {
                byte[] bytesToSend = Encoding.ASCII.GetBytes(cmdFinal);
                //send the data through the socket
                stream.Write(bytesToSend, 0, bytesToSend.Length);
                rtbLogging.AppendText(String.Format("[{1}]Sent: {0}", cmdFinal, timestamp));
                rtbLogging.ScrollToCaret();
            }
            catch (Exception ex)
            {
                rtbLogging.AppendText(ex.Message);
                rtbLogging.ScrollToCaret();
            }
            stream.Flush();
        }

        private void requestDataTexcel()
        {
            string requestRpmTorque = texcelCommand.TorqueRpmRequest();            
            string cmdFinal = fncascii.commandbuilder(requestRpmTorque);
            rtbLogging.AppendText(String.Format("Sent: {0}", cmdFinal));
            byte[] bytesToSend = Encoding.ASCII.GetBytes(cmdFinal);
            stream.Write(bytesToSend, 0, bytesToSend.Length);
            stream.Flush();
        }

        //Recieving in backgroud method.
        public void startReceiving()
        {
            //Data buffer
            byte[] buffer = new byte[256];
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
                    //Exploding the textReceivedPacket from Texcel into single string.
                    char delimiterChar = '\r';
                    explodedResponse = textReceived.Split(delimiterChar);
                    foreach (string responseUnit in explodedResponse)
                    {
                        //Check the received text and do suitable command
                        if (responseUnit == "1") //Host control successfully established
                        {
                            richTextBox1.Invoke((Action)delegate
                            {
                                richTextBox1.AppendText(string.Format("Respond: {0} [Valid checksum]\r\n", responseUnit));
                                richTextBox1.ScrollToCaret();
                            });
                        }
                        else if (responseUnit == "0") //Host control successfully established
                        {
                            richTextBox1.Invoke((Action)delegate
                            {
                                richTextBox1.AppendText(string.Format("Respond: {0} [Invalid checksum]\r\n", responseUnit));
                                richTextBox1.ScrollToCaret();
                            });
                        }
                        else if (responseUnit.IndexOf("R19,1") > -1) //Host control successfully established
                        {
                            richTextBox1.Invoke((Action)delegate
                            {
                                richTextBox1.AppendText(string.Format("Respond: {0} [Host mode success]\r\n", responseUnit));
                                richTextBox1.ScrollToCaret();
                            });
                        }
                        else if (responseUnit.IndexOf("R19,2") > -1)//Host control not success
                        {
                            richTextBox1.Invoke((Action)delegate
                            {
                                richTextBox1.AppendText(string.Format("Respond: {0} [Host mode unsuccess]\r\n", responseUnit));
                            });
                        }
                        else if (responseUnit.IndexOf("D2") > -1)//Actual data of texcel;
                        {
                            actualCondition = responseUnit;
                            string[] _actualCondition = actualCondition.Split(',');
                            _RPM = double.Parse(_actualCondition[2]);
                            _Torque = double.Parse(_actualCondition[3]);
                            aGauge1.Invoke((Action)delegate
                            {
                                aGauge1.Value = (float)_RPM / 1000;
                            });
                            textBox1.Invoke((Action)delegate
                            {
                                textBox1.Text = _RPM.ToString();
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
                                richTextBox1.AppendText(string.Format("Respond: {0}\r\n", responseUnit));
                                richTextBox1.ScrollToCaret();
                            });
                            //Sending the data to the database
                            sendResponseToDatabase(_RPM, _Torque);
                        }
                        else if (responseUnit.IndexOf("D7") > -1)
                        {
                            string[] texcelStatus = responseUnit.Split(',');

                            if (texcelStatus[2] == "0" && texcelStatus[3] == "0" && texcelStatus[4] == "1")
                            {
                                this.Invoke((Action)delegate
                                {
                                    btnStart.Enabled = true;
                                    toggleSwitch1.Checked = true;
                                    richTextBox1.AppendText(string.Format("Respond [1]: {0}\r\n", responseUnit));
                                    richTextBox1.ScrollToCaret();
                                    rtbLogging.AppendText("You CAN start the test!\r\n");
                                    rtbLogging.ScrollToCaret();
                                });
                            }
                            else if (texcelStatus[2] == "1" && texcelStatus[3] == "1" && texcelStatus[4] == "0")
                            {
                                this.Invoke((Action)delegate
                                {

                                    toggleSwitch1.Checked = false;
                                    btnStart.Enabled = false;
                                    richTextBox1.AppendText(string.Format("Respond [2]: {0}\r\n", responseUnit));
                                    richTextBox1.ScrollToCaret();
                                });
                            }
                            else if (texcelStatus[2] == "0" && texcelStatus[3] == "0" && texcelStatus[4] == "0")
                            {
                                this.Invoke((Action)delegate
                                {

                                    toggleSwitch1.Checked = false;
                                    btnStart.Enabled = false;
                                    richTextBox1.AppendText(string.Format("Respond [3]: {0}\r\n", responseUnit));
                                    richTextBox1.ScrollToCaret();
                                });
                            }
                            else if (texcelStatus[2] == "1" && texcelStatus[3] == "1" && texcelStatus[4] == "1")
                            {
                                this.Invoke((Action)delegate
                                {

                                    toggleSwitch1.Checked = true;
                                    btnStart.Enabled = false;
                                    richTextBox1.AppendText(string.Format("Respond [4]: {0}\r\n", responseUnit));
                                    richTextBox1.ScrollToCaret();
                                });
                            }
                            else
                            {
                                this.Invoke((Action)delegate
                                {
                                    richTextBox1.AppendText(string.Format("Respond [else]: {0}\r\n", responseUnit));
                                    richTextBox1.ScrollToCaret();
                                });
                            }
                        }
                        else if (responseUnit == "\r" || responseUnit == "")
                        {
                        }
                        else
                        {
                            this.Invoke((Action)delegate
                            {
                                richTextBox1.AppendText(string.Format("Respond [Else2]: {0}\r\n", responseUnit));
                                richTextBox1.ScrollToCaret();
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.Invoke((Action)delegate
                    {
                        rtbLogging.AppendText(string.Format("[Recieving Error]: {0}\r\n", ex.Message));
                        rtbLogging.ScrollToCaret();
                    });
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
                rtbLogging.AppendText("Conected to Texcel\r\n");
                btnMode.Enabled = true;
                string clearDataRequest = texcelCommand.clearDataRequest();
                byte[] bytesToSend = Encoding.ASCII.GetBytes(clearDataRequest);          
                stream.Write(bytesToSend, 0, bytesToSend.Length);
                
                rtbLogging.AppendText(String.Format("Sent: {0}", clearDataRequest));
                string getTexcelStatus = texcelCommand.getTexcelStatus();
                string cmdFinal = fncascii.commandbuilder(getTexcelStatus);
                bytesToSend = Encoding.ASCII.GetBytes(cmdFinal);
                stream.Write(bytesToSend, 0, bytesToSend.Length);
                rtbLogging.AppendText(String.Format("Sent: {0}", cmdFinal));
                threadReceiveData.Start();
            }
            catch (Exception ex)
            {
                rtbLogging.AppendText(ex.Message + "\r\n");
            }
        }

        //Connect to Coolant Temperature Controller
        private void btnConnectWC_Click(object sender, EventArgs e)
        {
            modbusWC = new ModbusClient(IPWaterCoolant, int.Parse(PortDAQ));
            rtbLogging.AppendText("Connecting to Coolant Conditioner\r\n");
            try
            {
                modbusWC.Connect();
                rtbLogging.AppendText("Connected to water controller\r\n");
                toggleSwitch2.Checked = true;
            }
            catch (Exception ex)
            {
                rtbLogging.AppendText("Coolant Conditioner Error: "+ ex.Message + "\r\n");
            }           

        }

        //Connect to Oil Temperature Controller
        private void btnConnectOC_Click(object sender, EventArgs e)
        {
            modbusOC = new ModbusClient(IPOilCoolant, int.Parse(PortOilCoolant));
            rtbLogging.AppendText("Connecting to Oil Conditioner\r\n");
            try
            {
                modbusOC.Connect();
                rtbLogging.AppendText("Connected to oil controller\r\n");
                toggleSwitch4.Checked = true;
            }
            catch (Exception ex)
            {
                rtbLogging.AppendText("Lubricant Conditioner Error: " + ex.Message + "\r\n");
            }
        }

        //Connecting to DAQ
        private void btnConnectDAQ_Click(object sender, EventArgs e)
        {
            modbusDAQ = new ModbusClient(IPDAQ, int.Parse(PortDAQ));
            rtbLogging.AppendText("Connecting to DAQ\r\n");
            try
            {
                modbusDAQ.Connect();
                rtbLogging.AppendText("Connected to DAQ\r\n");
                toggleSwitch3.Checked = true;

            }
            catch (Exception ex)
            {
                rtbLogging.AppendText("DAQ Error: " + ex.Message + "\r\n");
            }
        }

        //------------------SAVING THE TESTING RESULT TO THE DATABASE----------------------------//
        public void sendResponseToDatabase(double actualRPM, double actualTorque)
        {
            this.Invoke((Action)delegate
            {
                var index = dgvResult.Rows.Add();
                dgvResult.Rows[index].Cells["id"].Value = index + 1;
                dgvResult.Rows[index].Cells["time_stamp"].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                dgvResult.Rows[index].Cells["rpmActual"].Value = actualRPM;
                dgvResult.Rows[index].Cells["rpmDemand"].Value = rpm;
                dgvResult.Rows[index].Cells["torqueActual"].Value = actualTorque;
                dgvResult.Rows[index].Cells["torqueDemand"].Value = torque;
                dgvResult.FirstDisplayedScrollingRowIndex = dgvResult.RowCount - 1;
                dgvResult.Update();
            });
        }

        public void saveDataGridView()
        {
            //SaveFileDialog sfd = new SaveFileDialog();
            //sfd.Filter = "CSV (*.csv)|*.csv";
            //sfd.FileName = "Output.csv";
            //bool fileError = false;
            //if (sfd.ShowDialog() == DialogResult.OK)
            //{
            //    if (File.Exists(sfd.FileName))
            //    {
            //        try
            //        {
            //            File.Delete(sfd.FileName);
            //        }
            //        catch (IOException ex)
            //        {
            //            fileError = true;
            //            MessageBox.Show("It wasn't possible to write the data to the disk." + ex.Message);
            //        }
            //    }
            //    if (!fileError)
            //    {
            //        try
            //        {
            //            int columnCount = dataGridView1.Columns.Count;
            //            string columnNames = "";
            //            string[] outputCsv = new string[dataGridView1.Rows.Count + 1];
            //            for (int i = 0; i < columnCount; i++)
            //            {
            //                columnNames += dataGridView1.Columns[i].HeaderText.ToString() + ",";
            //            }
            //            outputCsv[0] += columnNames;

            //            for (int i = 1; (i - 1) < dataGridView1.Rows.Count; i++)
            //            {
            //                for (int j = 0; j < columnCount; j++)
            //                {
            //                    outputCsv[i] += dataGridView1.Rows[i - 1].Cells[j].Value.ToString() + ",";
            //                }
            //            }

            //            File.WriteAllLines(sfd.FileName, outputCsv, Encoding.UTF8);
            //            MessageBox.Show("Data Exported Successfully !!!", "Info");
            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show("Error :" + ex.Message);
            //        }
            //    }
            //}
        }       

        public void TableDisplay()
        {
            while (isTesting)
            {
                Thread.Sleep(1000);
            }            
        }
        //-------------------------------START STOP PAUSEDAQ Function------------------------------//
        private void startRecordDAQ()
        {
            try
            {
                modbusDAQ.WriteSingleRegister(0, 1);
            }
            catch (Exception ex)
            {
                rtbLogging.AppendText("Modbus DAQ: " + ex.Message + "\r\n");
            }
        }
        private void stopRecordDAQ()
        {
            try
            {
                modbusDAQ.WriteSingleRegister(0, 2);
            }
            catch (Exception ex)
            {
                rtbLogging.AppendText("Modbus DAQ: " + ex.Message + "\r\n");
            }
        }
        private void pauseRecordDAQ()
        {
            try
            {
                modbusDAQ.WriteSingleRegister(0, 3);
            }
            catch (Exception ex)
            {
                rtbLogging.AppendText("Modbus DAQ: " + ex.Message + "\r\n");
            }
        }

        //-------------------------Set point Coolant and Lubricant----------------------//
        private void setPointCoolant()
        {
            try
            {
                modbusWC.WriteSingleRegister(1, 70); //insert the Set Point here
                rtbLogging.AppendText("Coolant Set Point: " + "\r\n");
            }
            catch (Exception ex)
            {
                rtbLogging.AppendText("Coolant Conditioner Error: " + ex.Message + "\r\n");
            }
        }

        //Change the red line REDline. Better put in on the settings menu
        private void aGauge1_ValueInRangeChanged(object sender, ValueInRangeChangedEventArgs e)
        {

        }
    }
}

using EasyModbus;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.WinForms;
using LiveCharts.Configurations;
using Winform.Cartesian.ConstantChanges;
using System.Security;
using System.Windows.Media;

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
        DBConnect dbc = new DBConnect();
        TexcelCommand texcelCommand = new TexcelCommand();
        functionASCII fncascii = new functionASCII();
        TcpClient client;
        NetworkStream stream;
        Thread threadReceiveData;
        texcelRespondProcessor respondProcessor = new texcelRespondProcessor();
        FolderBrowserDialog fbd = new FolderBrowserDialog();
        ModbusClient modbusDAQ;
        ModbusClient modbusOC;
        ModbusClient modbusWC;
        StreamWriter sw;

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
        int commandIndex = 0;
        string actualCondition;
        double _RPM;
        double _Torque;
        string duration;
        string ramp_time;
        int[] coolantTemp;
        float _coolantTemp;
        int[] lubricantTemp;
        float _lubricantTemp;
        string _projectID { get; set; }
        int totalDemandDuration;
        int totalDuration;
        int progressBar1Second = 0;
        int progressBar2Second = 0;
        string[] explodedResponse;
        string torque;
        string rpm;
        string fileName;
        string filePath;

        //Declare the flag
        bool isTesting = false;

        public ChartValues<MeasureModel> ChartValues { get; set; }

        public System.Windows.Forms.Timer Timer { get; set; }
        public Random R { get; set; }

        public formEngineTesting(ucChooseProject ucCP, string projectID)
        {
            InitializeComponent();

            var mapper = Mappers.Xy<MeasureModel>()
                            .X(model => model.DateTime.Ticks)   //use DateTime.Ticks as X
                            .Y(model => model.Value);           //use the value property as Y

            //lets save the mapper globally.
            Charting.For<MeasureModel>(mapper);

            //the ChartValues property will store our values array
            ChartValues = new ChartValues<MeasureModel>();
            cartesianChart1.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Values = ChartValues,
                    PointGeometrySize = 1,
                    StrokeThickness = 1,
                    Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0,0,0,0)),
                }
            };
            cartesianChart1.AxisX.Add(new Axis
            {
                DisableAnimations = true,
                LabelFormatter = value => new System.DateTime((long)value).ToString("HH:mm:ss"),
                Separator = new Separator
                {
                    Step = TimeSpan.FromSeconds(1).Ticks
                }
            });

            //SetAxisLimits(System.DateTime.Now);

            //The next code simulates data changes every 500 ms
            Timer = new System.Windows.Forms.Timer
            {
                Interval = 1000
            };
            Timer.Tick += TimerOnTick;
            R = new Random();
            Timer.Start();


            //Initializing the database
            dbc.Initialize();
            dbc.OpenConnection();

            //Obtaining the method_id and show it on rtbLogging
            MySqlCommand cmd = new MySqlCommand();
            cmd = dbc.connection.CreateCommand();
            cmd.CommandText = "SELECT method_id FROM project_data WHERE id LIKE '%" + projectID + "%'";
            object methodID = cmd.ExecuteScalar();
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
                dgvDemand.ClearSelection();
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

            //Obataining the Coolant's and Lubricant's set point
            tbSetCoolant.Text = dgvDemand.Rows[1].Cells["cool_temp"].Value.ToString();
            tbSetLubricant.Text = dgvDemand.Rows[1].Cells["oil_temp"].Value.ToString();

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
            PortTexcel = dt2.Rows[0][1].ToString();
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
            Timer1.SynchronizingObject = this;
            Timer2 = new System.Timers.Timer(1000);
            Timer2.Elapsed += new ElapsedEventHandler(stopwatch_command);
            dbc.CloseConnection();

            //Calculating total duration
            for (int j = 0; j < (int)dgvDemand.RowCount; j++)
            {
                totalDemandDuration += (int)dgvDemand.Rows[j].Cells["duration"].Value;
            }
            totalDuration = totalDemandDuration;
            pbTimeElapsed.Maximum = totalDuration + 1;

            //Disabling the mode button.
            btnMode.Enabled = false;
            btnStart.Enabled = false;
        }

        //private void SetAxisLimits(System.DateTime now)
        //{
        //    cartesianChart1.AxisX[0].MaxValue = now.Ticks + TimeSpan.FromSeconds(1).Ticks; // lets force the axis to be 100ms ahead
        //    cartesianChart1.AxisX[0].MinValue = now.Ticks - TimeSpan.FromSeconds(8).Ticks; //we only care about the last 8 seconds
        //}
        private void TimerOnTick(object sender, EventArgs eventArgs)
        {
            var now = System.DateTime.Now;
            ChartValues.Add(new MeasureModel
            {
                DateTime = now,
                Value = R.Next(0, 10),
            });
            if (ChartValues.Count > 30)
            {
                cartesianChart1.AxisX[0].Separator.Step = TimeSpan.FromSeconds(30).Ticks;
            }
            else if (ChartValues.Count > 3600)
            {
                cartesianChart1.AxisX[0].Separator.Step = TimeSpan.FromSeconds(3600).Ticks;
            }            
            //SetAxisLimits(now);

            //lets only use the last 30 values
            //if (ChartValues.Count > 30) ChartValues.RemoveAt(0);
        }

        private void formEngineTesting_Load(object sender, EventArgs e)
        {
            threadReceiveData = new Thread(startReceiving);
            threadReceiveData.IsBackground = true;
            threadReceiveData.Name = "Data receiving thread";
        }

        //Sending and logging trigger
        private void commandTimer(object sender, EventArgs e)
        {
            if (commandIndex < dgvDemand.Rows.Count)
            {
                //Higihlighting the current demand
                dgvDemand.ClearSelection();
                dgvDemand.Rows[commandIndex].Selected = true;
                torque = dgvDemand.Rows[commandIndex].Cells["Torque"].Value.ToString();
                rpm = dgvDemand.Rows[commandIndex].Cells["RPM"].Value.ToString();
                duration = dgvDemand.Rows[commandIndex].Cells["duration"].Value.ToString();
                ramp_time = dgvDemand.Rows[commandIndex].Cells["ramp_time"].Value.ToString();
                //send those parameters to texcel command               
                string _command = texcelCommand.TorqueThrottle(torque, rpm, duration, ramp_time);
                //Showing stage in textbox3
                textBox4.Text = (commandIndex + 1).ToString();
                //Build the final command
                cmdFinal = fncascii.commandbuilder(_command);
                //Sending command through IP
                try
                {
                    byte[] bytesToSend = Encoding.ASCII.GetBytes(cmdFinal);
                    //send the data through the socket
                    stream.Write(bytesToSend, 0, bytesToSend.Length);
                    rtbLogging.AppendText(String.Format("[{1}]Sent: {0}", cmdFinal, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    rtbLogging.ScrollToCaret();
                }
                catch (Exception ex)
                {
                    rtbLogging.AppendText(ex.Message);
                    rtbLogging.ScrollToCaret();
                }
                stream.Flush();
                sw2.Restart();
                Timer1.Stop();
                Timer1.Interval = (int.Parse(duration)) * 1000;
                Timer1.Start();
                pbStage.Value = 0;
                progressBar2Second = 0;
                pbStage.Maximum = int.Parse(duration);
                commandIndex++;
            }
            //Stop sending command
            else if (commandIndex == dgvDemand.Rows.Count)
            {
                CommandDone();
            }
        }

        //Stopwatch to send command every 1 second.
        private void stopwatch_command(object sender, EventArgs e)
        {
            try
            {
                coolantTemp = modbusWC.ReadInputRegisters(1, 1);
                lubricantTemp = modbusOC.ReadInputRegisters(3, 1);
                _coolantTemp = coolantTemp[0];
                _lubricantTemp = lubricantTemp[0];
            }
            catch (Exception ex)
            {
                this.Invoke((Action)delegate
                {
                    rtbLogging.AppendText(String.Format("[{0}] Modbus Error: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Message));
                });
            }
            this.Invoke((Action)delegate
            {
                pbStage.CustomText = sw2.Elapsed.ToString("hh\\:mm\\:ss");
                pbStage.Value = progressBar2Second;
                pbTimeElapsed.CustomText = sw1.Elapsed.ToString("hh\\:mm\\:ss");
                pbTimeElapsed.Value = progressBar1Second;
                textBox6.Text = (_coolantTemp / 10).ToString();
                aGauge3.Value = _coolantTemp / 10;
                textBox7.Text = (_lubricantTemp / 10).ToString();
                aGauge4.Value = _lubricantTemp / 10;
            });
            progressBar1Second += 1;
            progressBar2Second += 1;
        }

        //Executed when the command is done
        private void CommandDone()
        {

            isTesting = false;
            rtbLogging.AppendText(String.Format("[{0}] Your test is done.\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            rtbLogging.ScrollToCaret();
            modbusOC.Disconnect();
            tsLubeConnection.Checked = false;
            modbusWC.Disconnect();
            tsCoolantConnection.Checked = false;
            stopRecordDAQ();
            modbusDAQ.Disconnect();
            tsDAQConnection.Checked = false;
            Timer2.Enabled = false;
            Timer1.Enabled = false;
            string cmdFinal2 = fncascii.commandbuilder(texcelCommand.stopTorqueRpmRequest());
            byte[] bytesToSend = Encoding.ASCII.GetBytes(cmdFinal2);
            stream.Write(bytesToSend, 0, bytesToSend.Length);
            string cmdFinal = fncascii.commandbuilder(texcelCommand.IdleCommand());
            bytesToSend = Encoding.ASCII.GetBytes(cmdFinal);
            stream.Write(bytesToSend, 0, bytesToSend.Length);
            stream.Flush();
            sw.Close();
        }

        //--------COMMAND FOR THE START, STOP, PAUSE, AND MODE BUTTON-------------------------//
        //Start button trigger
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (tbFilePath.Text != "")
            {
                if (commandIndex < 1)
                {
                    createFile();
                    isTesting = true;
                    //Immidiately send the first command
                    sendCommandImmediately(commandIndex);
                    commandIndex = 1;
                    //Starting DAQ record
                    startRecordDAQ();
                    //Starting the timer
                    Timer2.Enabled = true;
                    sw2.Start();
                    Timer1.Interval = int.Parse(duration) * 1000;
                    Timer1.Enabled = true;
                    sw1.Start();
                    //Get rpm and torque value from texcel
                    requestDataTexcel();
                    //Set the pbStage maximum value
                    pbStage.Maximum = (int.Parse(duration));
                    //Set the lubricant's and coolant's set point
                    btnSetCoolant_Click(sender, e);
                    btnSetLubricant_Click(sender, e);
                }
                else //Resume the Testing 
                {
                    isTesting = true;
                    startRecordDAQ();
                    sendCommandImmediately(commandIndex);
                    commandIndex = commandIndex + 1;
                    //Starting the timer
                    Timer1.Interval = int.Parse(duration) * 1000;
                    Timer1.Enabled = true;
                    sw1.Start();
                    Timer2.Enabled = true;
                    sw2.Start();
                    //Set the pbStage maximum value
                    pbStage.Maximum = (int.Parse(duration));
                    //Set the lubricant's and coolant's set point
                    btnSetCoolant_Click(sender, e);
                    btnSetLubricant_Click(sender, e);
                }
            }
            else
            {
                MessageBox.Show("Please select report folder path");
            }

        }

        //Pause button trigger
        private void btnPause_Click(object sender, EventArgs e)
        {
            isTesting = false;
            progressBar2Second = 0;
            Timer2.Stop();
            sw2.Stop();
            sw2.Reset();
            pauseRecordDAQ();
            commandIdle();
            Timer1.Stop();
            sw1.Stop();
        }

        //Stop button trigger
        private void btnStop_Click(object sender, EventArgs e)
        {
            isTesting = false;
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
                rtbLogging.AppendText(String.Format("[{0}] Clearing any data request from texcel\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                bytesToSend = Encoding.ASCII.GetBytes(cmdFinal2);
                stream.Write(bytesToSend, 0, bytesToSend.Length);
                rtbLogging.AppendText(String.Format("[{0}] Clearing any demand queue on texcel\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                bytesToSend = Encoding.ASCII.GetBytes(cmdFinal3);
                stream.Write(bytesToSend, 0, bytesToSend.Length);
                rtbLogging.AppendText(String.Format("[{0}] Overide to manual control\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
            catch (Exception ex)
            {
                rtbLogging.AppendText(String.Format("[{1}]Stop Error: {1} \r\n", ex.Message, "[{0}] Clearing any data request from texcel\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                rtbLogging.ScrollToCaret();
            }
            Timer1.Stop();
            Timer2.Stop();
            sw1.Stop();
            sw2.Stop();
            stopRecordDAQ();
            modbusDAQ.Disconnect();
            modbusOC.Disconnect();
            modbusWC.Disconnect();
            tsDAQConnection.Checked = false;
            tsLubeConnection.Checked = false;
            tsCoolantConnection.Checked = false;
            sw.Close();
            rtbLogging.AppendText(String.Format("[{0}] Forcibly stop the test.", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
        }

        //Change the texcel to host control mode
        private void btnMode_Click(object sender, EventArgs e)
        {
            if (!tsTexcelHost.Checked)
            {
                //Building the command           
                string tohost = texcelCommand.HostControl();
                string clearDemandCommand = texcelCommand.clearDemandQueue();
                string cmdFinal1 = fncascii.commandbuilder(tohost);
                string cmdFinal2 = fncascii.commandbuilder(clearDemandCommand);
                rtbLogging.AppendText(String.Format("[{0}] Clear command in Texcel Demand queue\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                byte[] bytesToSend2 = Encoding.ASCII.GetBytes(cmdFinal2);
                stream.Write(bytesToSend2, 0, bytesToSend2.Length);
                rtbLogging.AppendText(String.Format("[{0}] Override to host control\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                byte[] bytesToSend1 = Encoding.ASCII.GetBytes(cmdFinal1);
                stream.Write(bytesToSend1, 0, bytesToSend1.Length);
            }
            else
            {
                MessageBox.Show("Texcel is already in HOST mode");
            }

        }

        //---------------SENDING AND RECIEVING THE COMMAND TO TEXCEL-------------------------//
        private void sendCommandImmediately(int commandIndexImmediately)
        {
            //Highlighting the current demand
            dgvDemand.ClearSelection();
            dgvDemand.Rows[commandIndexImmediately].Selected = true;
            //obtaining torque, rpm and duration value
            torque = dgvDemand.Rows[commandIndexImmediately].Cells["Torque"].Value.ToString();
            rpm = dgvDemand.Rows[commandIndexImmediately].Cells["RPM"].Value.ToString();
            duration = dgvDemand.Rows[commandIndexImmediately].Cells["duration"].Value.ToString();
            ramp_time = dgvDemand.Rows[commandIndexImmediately].Cells["ramp_time"].Value.ToString();
            //send those parameters to texcel command
            string _command = texcelCommand.TorqueThrottle(torque, rpm, duration, ramp_time);
            //Showing stage in textbox3
            textBox4.Text = (commandIndexImmediately).ToString();
            //Build the final command
            cmdFinal = fncascii.commandbuilder(_command);
            //Sending command through IP
            try
            {
                byte[] bytesToSend = Encoding.ASCII.GetBytes(cmdFinal);
                //send the data through the socket
                stream.Write(bytesToSend, 0, bytesToSend.Length);
                rtbLogging.AppendText(String.Format("[{1}] Sent: {0}", cmdFinal, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                rtbLogging.ScrollToCaret();
            }
            catch (Exception ex)
            {
                rtbLogging.AppendText(String.Format("[{1}] Sending command Error: {0}\r\n", ex.Message, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                rtbLogging.ScrollToCaret();
            }
            stream.Flush();
        }
        private void requestDataTexcel()
        {
            try
            {
                string requestRpmTorque = texcelCommand.TorqueRpmRequest();
                string cmdFinal = fncascii.commandbuilder(requestRpmTorque);
                rtbLogging.AppendText(String.Format("[{0}] Requesting data from texcel.\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                byte[] bytesToSend = Encoding.ASCII.GetBytes(cmdFinal);
                stream.Write(bytesToSend, 0, bytesToSend.Length);
                stream.Flush();
            }
            catch (Exception ex)
            {
                rtbLogging.AppendText(String.Format("[{1}] Receiving data Error: {0}\r\n", ex.Message, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        //Recieving in backgroud method.
        public void startReceiving()
        {
            tsTexcelConnection.Checked = false;
            //Data buffer
            byte[] buffer = new byte[256];
            //Create integer to hold how large the data received is
            Int32 bytesReceived;
            //Loop to continously reading data while the client is connected
            while (client.Connected)
            {
                this.Invoke((Action)delegate
                {
                    tsTexcelConnection.Checked = true;
                });
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
                            this.Invoke((Action)delegate
                            {
                                richTextBox1.AppendText(string.Format("Respond: {0} [Valid checksum]\r\n", responseUnit));
                                richTextBox1.ScrollToCaret();
                            });
                        }
                        else if (responseUnit == "0") //Host control successfully established
                        {
                            this.Invoke((Action)delegate
                            {
                                richTextBox1.AppendText(string.Format("Respond: {0} [Invalid checksum]\r\n", responseUnit));
                                richTextBox1.ScrollToCaret();
                            });
                        }
                        else if (responseUnit.IndexOf("R19,1") > -1) //Host control successfully established
                        {
                            this.Invoke((Action)delegate
                            {
                                richTextBox1.AppendText(string.Format("Respond: {0} [Host mode success]\r\n", responseUnit));
                                richTextBox1.ScrollToCaret();
                            });
                        }
                        else if (responseUnit.IndexOf("R19,2") > -1)//Host control not success
                        {
                            this.Invoke((Action)delegate
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
                            this.Invoke((Action)delegate
                            {
                                aGauge1.Value = (float)_RPM / 1000;
                                textBox1.Text = _RPM.ToString();
                                aGauge2.Value = (float)_Torque;
                                textBox2.Text = _Torque.ToString();
                                richTextBox1.AppendText(string.Format("Respond: {0}\r\n", responseUnit));
                                richTextBox1.ScrollToCaret();
                            });
                            //Sending the data to the database
                            if (isTesting) { sendResponseToFile(_RPM, _Torque); }
                        }
                        else if (responseUnit.IndexOf("D7") > -1)
                        {
                            string[] texcelStatus = responseUnit.Split(',');

                            if (texcelStatus[2] == "0" && texcelStatus[3] == "0" && texcelStatus[4] == "1")
                            {
                                this.Invoke((Action)delegate
                                {
                                    btnStart.Enabled = true;
                                    tsTexcelHost.Checked = true;
                                    richTextBox1.AppendText(string.Format("Respond [1]: {0}\r\n", responseUnit));
                                    richTextBox1.ScrollToCaret();
                                    rtbLogging.AppendText(String.Format("[{0}] You can start the test!\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                                    rtbLogging.ScrollToCaret();
                                });
                            }
                            else if (texcelStatus[2] == "1" && texcelStatus[3] == "1" && texcelStatus[4] == "0")
                            {
                                this.Invoke((Action)delegate
                                {
                                    tsTexcelHost.Checked = false;
                                    btnStart.Enabled = false;
                                    richTextBox1.AppendText(string.Format("Respond [2]: {0}\r\n", responseUnit));
                                    richTextBox1.ScrollToCaret();
                                });
                            }
                            else if (texcelStatus[2] == "0" && texcelStatus[3] == "0" && texcelStatus[4] == "0")
                            {
                                this.Invoke((Action)delegate
                                {

                                    tsTexcelHost.Checked = false;
                                    btnStart.Enabled = false;
                                    richTextBox1.AppendText(string.Format("Respond [3]: {0}\r\n", responseUnit));
                                    richTextBox1.ScrollToCaret();
                                });
                            }
                            else if (texcelStatus[2] == "1" && texcelStatus[3] == "1" && texcelStatus[4] == "1")
                            {
                                this.Invoke((Action)delegate
                                {

                                    tsTexcelHost.Checked = true;
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
                        rtbLogging.AppendText(String.Format("[{1}] Recieving data Error: {0}\r\n", ex.Message, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                        rtbLogging.ScrollToCaret();
                    });
                }
                stream.Flush();
            }
        }
        public void commandIdle()
        {
            string cmdFinal = fncascii.commandbuilder(texcelCommand.IdleCommand());
            try
            {
                byte[] bytesToSend = Encoding.ASCII.GetBytes(cmdFinal);
                stream.Write(bytesToSend, 0, bytesToSend.Length);
                rtbLogging.AppendText(String.Format("[{1}] Sent: {0}", cmdFinal, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
            catch (Exception ex)
            {
                rtbLogging.AppendText(String.Format("[{1}] Sending command Error: {0}\r\n", ex.Message, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                rtbLogging.ScrollToCaret();
            }
        }

        //----------------Connection Button to All Device------------------------------------//

        //Establish the connection to the server from client.
        public void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                IPAddress IPadd = IPAddress.Parse(IPTexcel);
                client = new TcpClient();
                client.Connect(IPTexcel, int.Parse(PortTexcel));
                stream = client.GetStream();
                rtbLogging.AppendText(String.Format("[{0}] Connected to Texcel\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                btnMode.Enabled = true;
                string clearDataRequest = texcelCommand.clearDataRequest();
                byte[] bytesToSend = Encoding.ASCII.GetBytes(clearDataRequest);
                stream.Write(bytesToSend, 0, bytesToSend.Length);

                rtbLogging.AppendText(String.Format("[{0}] Clearing data request from Texcel\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                string getTexcelStatus = texcelCommand.getTexcelStatus();
                string cmdFinal = fncascii.commandbuilder(getTexcelStatus);
                bytesToSend = Encoding.ASCII.GetBytes(cmdFinal);
                stream.Write(bytesToSend, 0, bytesToSend.Length);
                rtbLogging.AppendText(String.Format("[{0}] Getting Texcel's status\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                threadReceiveData.Start();
            }
            catch (Exception ex)
            {
                rtbLogging.AppendText(String.Format("[{0}] Texcel's connection Error: {1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Message));
            }
        }

        //Connect to Coolant Temperature Controller
        private void btnConnectWC_Click(object sender, EventArgs e)
        {
            modbusWC = new ModbusClient(IPWaterCoolant, int.Parse(PortDAQ));
            rtbLogging.AppendText(String.Format("[{0}] Connecting to coolant temperature controller\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            try
            {
                modbusWC.Connect();
                rtbLogging.AppendText(String.Format("[{0}] Connected to coolant temperature controller\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                tsCoolantConnection.Checked = true;
            }
            catch (Exception ex)
            {
                rtbLogging.AppendText(String.Format("[{0}] Coolant controller connection Error: {1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Message));
            }

        }

        //Connect to Oil Temperature Controller
        private void btnConnectOC_Click(object sender, EventArgs e)
        {
            modbusOC = new ModbusClient(IPOilCoolant, int.Parse(PortOilCoolant));
            rtbLogging.AppendText(String.Format("[{0}] Connecting to lubricant temperature controller\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            try
            {
                modbusOC.Connect();
                rtbLogging.AppendText(String.Format("[{0}] Connected to lubricant temperature controller\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                tsLubeConnection.Checked = true;
            }
            catch (Exception ex)
            {
                rtbLogging.AppendText(String.Format("[{0}] Coolant controller connection Error: {1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Message));
            }
        }

        //Connecting to DAQ
        private void btnConnectDAQ_Click(object sender, EventArgs e)
        {
            modbusDAQ = new ModbusClient(IPDAQ, int.Parse(PortDAQ));
            rtbLogging.AppendText(String.Format("[{0}] Connecting to DAQ\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            try
            {
                modbusDAQ.Connect();
                rtbLogging.AppendText(String.Format("[{0}] Connected to DAQ\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                tsDAQConnection.Checked = true;

            }
            catch (Exception ex)
            {
                rtbLogging.AppendText(String.Format("[{0}] DAQ connection Error: {1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Message));
            }
        }

        //------------------SAVING THE TESTING RESULT TO THE DATABASE-------------------------//
        public void sendResponseToFile(double actualRPM, double actualTorque)
        {
            StringBuilder sb = new StringBuilder();
            this.Invoke((Action)delegate
            {
                var index = dgvResult.Rows.Add();
                dgvResult.Rows[index].Cells["id"].Value = index + 1;
                dgvResult.Rows[index].Cells["time_stamp"].Value = DateTime.Now.ToString("HH:mm:ss");
                dgvResult.Rows[index].Cells["rpmActual"].Value = actualRPM;
                dgvResult.Rows[index].Cells["rpmDemand"].Value = rpm;
                dgvResult.Rows[index].Cells["torqueActual"].Value = actualTorque;
                dgvResult.Rows[index].Cells["torqueDemand"].Value = torque;
                dgvResult.FirstDisplayedScrollingRowIndex = dgvResult.RowCount - 1;
                dgvResult.Update();
                sb.Append(string.Join(";", index + 1, dgvResult.Rows[index].Cells["time_stamp"].Value.ToString(), actualRPM, rpm, actualTorque, torque));
                sw.WriteLine(sb.ToString());
                sw.Flush();
            });
        }

        public void createFile()
        {
            fileName = String.Format("Texcel_{0}_{1}.csv", DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.ToString("HHmmss"));
            filePath = tbFilePath.Text + @"\" + fileName;
            if (!File.Exists(filePath)) //Create a new file 
            {
                StringBuilder sb = new StringBuilder();
                IEnumerable<string> columnNames = dgvResult.Columns.Cast<DataGridViewTextBoxColumn>().
                    Select(column => column.Name);
                sb.AppendLine(string.Join(";", columnNames));
                File.WriteAllText(filePath, sb.ToString());
            }
            sw = new StreamWriter(filePath, true);
        }

        private void btnDirectory_Click(object sender, EventArgs e)
        {
            fbd.RootFolder = Environment.SpecialFolder.Desktop;
            fbd.Description = "Select testing result directory";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                tbFilePath.Text = fbd.SelectedPath;
            }
        }

        //--------------------------START STOP PAUSE DAQ Function----------------------------//
        private void startRecordDAQ()
        {
            try
            {
                modbusDAQ.WriteSingleRegister(0, 1);
                rtbLogging.AppendText(String.Format("[{0}] Starting the record\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
            catch (Exception ex)
            {
                rtbLogging.AppendText(String.Format("[{0}] Starting record Error: {1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Message));
            }
        }
        private void stopRecordDAQ()
        {
            try
            {
                modbusDAQ.WriteSingleRegister(0, 2);
                rtbLogging.AppendText(String.Format("[{0}] Stopping the record\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
            catch (Exception ex)
            {
                rtbLogging.AppendText(String.Format("[{0}] Stopping record Error: {1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Message));
            }
        }
        private void pauseRecordDAQ()
        {
            try
            {
                modbusDAQ.WriteSingleRegister(0, 3);
                rtbLogging.AppendText(String.Format("[{0}] Pausing the record\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
            catch (Exception ex)
            {
                rtbLogging.AppendText(String.Format("[{0}] Pausing record Error: {1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Message));
            }
        }

        //-------------------------Set point Coolant and Lubricant---------------------------//
        private void btnSetCoolant_Click(object sender, EventArgs e)
        {
            try
            {
                int coolantSetPoint = (int)(double.Parse(tbSetCoolant.Text) * 10);
                modbusWC.WriteSingleRegister(0, coolantSetPoint);
                rtbLogging.AppendText(String.Format("[{1}] Coolant set point: {0} °C\r\n", tbSetCoolant.Text, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
            catch (Exception ex)
            {
                rtbLogging.AppendText(String.Format("[{1}] Lubricant modbus write register Error: {0}\r\n", ex.Message, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        private void btnSetLubricant_Click(object sender, EventArgs e)
        {
            try
            {
                int lubricantSetPoint = (int)(double.Parse(tbSetCoolant.Text) * 10);
                modbusOC.WriteSingleRegister(1, lubricantSetPoint);
                rtbLogging.AppendText(String.Format("[{1}] Lubricant set point: {0} °C\r\n", tbSetLubricant.Text, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
            catch (Exception ex)
            {
                rtbLogging.AppendText(String.Format("[{1}]Lubricant modbus write register Error: {0}\r\n", ex.Message, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        //Configuration of The form
        private void aGauge1_ValueInRangeChanged(object sender, ValueInRangeChangedEventArgs e)
        {

        }
        public void highlightError(object sender, EventArgs e)
        {
            rtbLogging.ScrollToCaret();
            rtbLogging.Find("Error", RichTextBoxFinds.MatchCase);
            rtbLogging.SelectionColor = System.Drawing.Color.Red;
        }


    }
}

namespace Winform.Cartesian.ConstantChanges
{
    public class MeasureModel
    {
        public System.DateTime DateTime { get; set; }
        public double Value { get; set; }
    }
}

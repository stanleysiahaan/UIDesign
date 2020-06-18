using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace UIDesign
{
    class connectionProtocol
    {
        
        public string status { get; set; }
        public string host { get; set; }
        public string port { get; set; }
        public string TestConnection(string host, string port)
        {
            IPAddress IP;
            if (IPAddress.TryParse(host,out IP))
            {
                Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    s.Connect(IP, int.Parse(port));
                    status = "Connection Established";
                }
                catch(Exception ex)
                {
                    status = ex.Message;
                }
            }   
            return status;
        }

        public string SendCommand(string command, string IPTexcel, string PortTexcel)
        {
            int port = int.Parse(PortTexcel);
            string texttosend = command;
            UdpClient udpClient = new UdpClient();
            udpClient.Connect(IPTexcel, port);
            byte[] senddata = Encoding.ASCII.GetBytes(texttosend);
            udpClient.Send(senddata, senddata.Length);
            return texttosend;
        }
    }

}



//==========================Example===============================
/*
//---data to send to the server---
string textToSend = command;
string Server_IP = "192.168.1.17";
//---create a TCPClient object at the IP and port no.---
TcpClient client = new TcpClient(Server_IP, 55);
NetworkStream nwStream = client.GetStream();
byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);

//---send the text---
//Console.WriteLine("Sending : " + textToSend);
nwStream.Write(bytesToSend, 0, bytesToSend.Length);

//---read back the text---
//byte[] bytesToRead = new byte[client.ReceiveBufferSize];
//int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
//Console.WriteLine("Received : " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));
//Console.ReadLine();
client.Close();

return textToSend;
*/

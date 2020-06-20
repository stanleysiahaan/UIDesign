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
        UdpClient udpClient;
        IPAddress ipadd;
        
        public string status { get; set; }
        int port;
        public string TestConnection(string host, string port)
        {
            ipadd = IPAddress.Parse(host);
            IPEndPoint iPEndPoint = new IPEndPoint(ipadd, int.Parse(port));
            try
            {
                udpClient = new UdpClient();
                udpClient.Connect(iPEndPoint);
                status = "Connection Established";
            }
            catch (Exception e)
            {
                status = e.ToString();
            }
            return status;
        }

        public string SendCommandTexcel(string command, string IPTexcel, string PortTexcel)
        {
            port = int.Parse(PortTexcel);
            string texttosend = command;
            udpClient = new UdpClient();
            udpClient.Connect(IPTexcel, port);
            byte[] senddata = Encoding.ASCII.GetBytes(texttosend);
            udpClient.Send(senddata, senddata.Length);
            return texttosend;
        }

        public (string, string) SendHostControl(string tohost, string IPTexcel, string PortTexcel)
        {
            port = int.Parse(PortTexcel);
            string texttosend = tohost;
            udpClient = new UdpClient();
            udpClient.Connect(IPTexcel, port);
            byte[] senddata = Encoding.ASCII.GetBytes(texttosend);
            udpClient.Send(senddata, senddata.Length);
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Parse(IPTexcel), port);
            byte[] receivedData = udpClient.Receive(ref RemoteIpEndPoint);
            string _receivedData = Encoding.ASCII.GetString(receivedData);
            return (texttosend, _receivedData);
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

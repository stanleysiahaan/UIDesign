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
        int port;
        string textReceived;

        public string SendCommandTexcelSocket(string command, string IPTexcel, string PortTexcel)
        {
            port = int.Parse(PortTexcel);
            byte[] bytes = new byte[1024];
            try
            {
                //Connect to a remote server
                IPAddress IPadd = IPAddress.Parse(IPTexcel);
                IPEndPoint remoteEP = new IPEndPoint(IPadd, port);
                //Create a TCP/IP socket.
                Socket sender = new Socket(IPadd.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    //connect to remote Endpoint
                    sender.Connect(remoteEP);
                    //encode the data string into a byte array
                    byte[] bytesToSend = Encoding.ASCII.GetBytes(command);
                    //send the data through the socket
                    int bytesSent = sender.Send(bytesToSend);
                    //receive the response from the remote device
                    int bytesRead = sender.Receive(bytes);
                    textReceived = Encoding.ASCII.GetString(bytes, 0, bytesRead);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return textReceived;
        }

        public (string, string) SendHostControlSocket(string tohost, string IPTexcel, string PortTexcel)
        {
            port = int.Parse(PortTexcel);
            byte[] bytes = new byte[1024];
            try
            {
                //Connect to a remote server
                IPAddress IPadd = IPAddress.Parse(IPTexcel);
                IPEndPoint remoteEP = new IPEndPoint(IPadd, port);
                //Create a TCP/IP socket.
                Socket sender = new Socket(IPadd.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    //connect to remote Endpoint
                    sender.Connect(remoteEP);
                    //encode the data string into a byte array
                    byte[] bytesToSend = Encoding.ASCII.GetBytes(tohost);
                    //send the data through the socket
                    int bytesSent = sender.Send(bytesToSend);
                    //receive the response from the remote device
                    int bytesRead = sender.Receive(bytes);
                    textReceived = Encoding.ASCII.GetString(bytes, 0, bytesRead);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return (tohost, textReceived);
        }
    }

}

//public string SendCommandTexcel(string command, string IPTexcel, string PortTexcel)
//{
//    port = int.Parse(PortTexcel);
//    //--data to send to the server--
//    string texttosend = command;
//    try
//    {
//        //--create a TCPClient Object at the IP and port no--
//        TcpClient client = new TcpClient();
//        NetworkStream nwStream = client.GetStream();
//        byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(texttosend);
//        //--send the text--
//        nwStream.Write(bytesToSend, 0, bytesToSend.Length);
//        //--readback the text--
//        byte[] bytesToRead = new byte[client.ReceiveBufferSize];
//        int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
//        textReceived = Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);
//        client.Close();
//    }
//    catch (Exception ex)
//    {
//        MessageBox.Show(ex.Message);
//    }
//    //--return the sended and received data--
//    return textReceived;
//}

//public (string, string) SendHostControl(string tohost, string IPTexcel, string PortTexcel)
//{
//    port = int.Parse(PortTexcel);
//    //--data to send to the server---
//    string texttosend = tohost;
//    try
//    {
//        //--create a TCPClient object at the IP and port no---
//        TcpClient client = new TcpClient(IPTexcel, port);
//        NetworkStream nwStream = client.GetStream();
//        byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(texttosend);
//        //--send the text--
//        nwStream.Write(bytesToSend, 0, bytesToSend.Length);
//        //--read back the text---
//        byte[] bytesToRead = new byte[client.ReceiveBufferSize];
//        int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
//        textReceived = Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);
//        client.Close();
//    }
//    catch (Exception ex)
//    {
//        MessageBox.Show(ex.Message);
//    }

//    //--return the sending and receiving data--
//    return (texttosend, textReceived);
//}

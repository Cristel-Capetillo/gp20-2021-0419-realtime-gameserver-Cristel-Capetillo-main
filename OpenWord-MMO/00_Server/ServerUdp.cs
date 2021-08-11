using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace OpenWord_MMO._01_Server {
    public class ServerUdp {
        Thread receiveThread;
        UdpClient client;
        UdpClient server;
        int port;
        

        public void OnCommunicationStarted() {
            port = 8051;
            Console.WriteLine("Communication started..."); 
            Console.WriteLine("Server is receiving from client in port: "+port+"");
            receiveThread = new Thread(new ThreadStart(ReceiveData));
            receiveThread.IsBackground = true;
            receiveThread.Start();
            string text= " ";
            do {
                text = Console.ReadLine();
            }
            while(!text.Equals("Exit"));
        }

        
        public void ReceiveData() {
            client = new UdpClient(port);
            while (true) {
                try {
                    IPEndPoint anyIpAddress = new IPEndPoint(IPAddress.Any, 0);
                    byte[] dataToReceive = client.Receive(ref anyIpAddress);
                    string receivedMessage = Encoding.ASCII.GetString(dataToReceive);
                    Console.WriteLine("Have received from client: " + receivedMessage);
                    byte[] dataToSend = dataToReceive;
                    client.Send(dataToSend, dataToSend.Length, anyIpAddress);
                    Console.WriteLine("Have sent back to client: " + receivedMessage);
                }
                catch (Exception exception) {
                    Console.WriteLine(exception.ToString());
                }
            }
        }
    }
}

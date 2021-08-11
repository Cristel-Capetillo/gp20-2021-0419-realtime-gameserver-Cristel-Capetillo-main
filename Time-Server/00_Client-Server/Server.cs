using System;
using System.Net;
using System.Net.Sockets;

namespace TimeServer {
    public class Server {
        public static void StartServer() {
            TcpListener listener = null;
            try {
                listener = new TcpListener(IPAddress.Loopback, 25000);
                listener.Start();
                Console.WriteLine("Server started...");

                Byte[] bytes = new Byte[256];

                while (true) {
                    Console.WriteLine("Waiting for incoming client connection...");
                    TcpClient client = listener.AcceptTcpClient();
                    Console.WriteLine("Connected with client!");
                    NetworkStream stream = client.GetStream();
                    byte[] message = System.Text.Encoding.ASCII.GetBytes(DateTime.Now.ToString());
                    stream.Write(message, 0, message.Length);
                    Console.WriteLine("Have sent: {0}", message);
                    client.Close();
                }
            }
            
            catch (SocketException error) {
                Console.WriteLine("SocketException: {0}", error);
            }
            
            finally {
                if (listener != null)
                    listener.Stop();
            }
        }
    }
}


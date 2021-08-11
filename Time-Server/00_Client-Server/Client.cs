using System;
using System.Net.Sockets;

namespace TimeServer {
    
   public class Client {

        public static void Connect(String server, String message) {
            try {
                Int32 port = 25000;
                TcpClient client = new TcpClient(server, port);
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                NetworkStream stream = client.GetStream();
                stream.Write(data, 0, data.Length);
                Console.WriteLine("Sent: {0}", DateTime.Now.ToString());

                data = new Byte[256];

                String responseData = String.Empty;
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);
                stream.Close();
                client.Close();
            }
            
            catch (ArgumentNullException error) {
                Console.WriteLine("ArgumentNullException: {0}", error);
            }
            
            catch (SocketException error) {
                Console.WriteLine("SocketException: {0}", error);
            }

            Console.WriteLine("\n Press Enter to continue...");
            Console.Read();
        }
    }
}

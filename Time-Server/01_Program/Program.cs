using System;

namespace TimeServer {
    public class Program {
        static void Main(string[] args) {
            Console.Title = "Time Server";
            Server.StartServer();
            Client.Connect("127.0.0.1", message:"Date and time");
        }
    } 
}

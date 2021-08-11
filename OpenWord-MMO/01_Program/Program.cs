using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using OpenWord_MMO._01_Server;

public static class Program {
    static void Main(string[] arguments) {
        ServerUdp client = new ServerUdp();
        
        client.OnCommunicationStarted();
        client.ReceiveData();
    }
}

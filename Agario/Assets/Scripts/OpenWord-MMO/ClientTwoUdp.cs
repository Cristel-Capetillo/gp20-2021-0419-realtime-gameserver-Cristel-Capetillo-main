using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class ClientTwoUdp : MonoBehaviour {
    int localPort;
    string ipAddress;
    int port; 
    IPEndPoint remoteEndPoint;
    UdpClient client;
    UdpClient server;

    string typeMessageUI;
    string lastMessageReceived;
    string allMessages;
  

    void Start() {
        CommunicationStarted();
    }

    void CommunicationStarted() {
        ipAddress = "127.0.01";
        port = 8051;
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
        client = new UdpClient();
        Debug.Log("Sending to this IP: " + ipAddress + "\nSending to this server port: " + port);
    }

    void ClientCommunicatingWithServer(string messageToSend) {
        try {
                byte[] dataToSend = Encoding.ASCII.GetBytes(messageToSend);
                client.Send(dataToSend, dataToSend.Length, remoteEndPoint);
                Debug.Log("Client 2 sent to server: " + messageToSend);
                byte[] dataToReceive = client.Receive(ref remoteEndPoint);
                string receivedFromServer = Encoding.ASCII.GetString(dataToReceive);
                Debug.Log("Client 2 received from server: " + receivedFromServer);
                lastMessageReceived = receivedFromServer;
                allMessages += lastMessageReceived;
        }
        catch (Exception err) {
            Debug.Log(err.ToString());
        }
    }
    
    
    public void OnGUI() {
        Rect rectForReceiving = new Rect(600, 10, 200, 400);
        GUIStyle styleOne = new GUIStyle();
        styleOne.alignment = TextAnchor.UpperLeft;
        styleOne.fontSize = 20;
        styleOne.fontStyle = FontStyle.Bold;
        GUI.Box(rectForReceiving, "Client is 2 now communicating on \n127.0.0.1 " + port + " \n" +
            "\nYour message: \n" + lastMessageReceived + "\nAll messages: \n" + allMessages, styleOne);


        typeMessageUI = GUI.TextField(new Rect(600, 300, 200, 50), typeMessageUI);
        if (GUI.Button(new Rect(600, 400, 200, 50), "Send")) {
            ClientCommunicatingWithServer(typeMessageUI + "\n");
        }


        if (GUI.Button(new Rect(600, 500, 200, 50), "Close")) {
            client.Close();
            Application.Quit();
            Debug.Log("Client 2 has closed the communication");
        }
    }
}



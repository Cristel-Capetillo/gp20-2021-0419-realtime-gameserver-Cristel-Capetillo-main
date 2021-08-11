using System;
using System.Collections;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace TimeServer {
    public class ClientTcp : MonoBehaviour { 
        public string serverIPAddress = "127.0.0.1"; 
        public int port = 25000; 
       
        TcpClient tcpClient; 
        NetworkStream networkStream;
        readonly byte[] bufferingBytes = new byte[49152]; 
        int receivedBytes; 
        string receivedMessage = ""; 
        IEnumerator ListeningToServerCoroutine;
        
        protected Action OnClientStarted = null; 
        protected Action OnClientClosed = null; 
        
        protected void StartClient() {
            try { 
                tcpClient = new TcpClient(); 
                tcpClient.Connect(serverIPAddress, port); 
                OnClientStarted?.Invoke(); 
                ListeningToServerCoroutine = ServerListener(); 
                StartCoroutine(ListeningToServerCoroutine);
            }
            catch (SocketException) { 
                CloseClient(); 
            } 
        }


        IEnumerator ServerListener() {
            networkStream = tcpClient.GetStream();
            do {
                networkStream.BeginRead(bufferingBytes, 0, bufferingBytes.Length, MessageReceived, null);
                if(receivedBytes > 0) { 
                    WhenMessageIsReceived(receivedMessage); 
                    receivedBytes = 0; 
                }
                yield return new WaitForSeconds(1);
            } 
            while(receivedBytes >= 0 && networkStream != null);
        }
        
        void MessageReceived(IAsyncResult result) { 
            if (result.IsCompleted && tcpClient.Connected) { 
                receivedBytes = networkStream.EndRead(result); 
                receivedMessage = Encoding.ASCII.GetString(bufferingBytes, 0, receivedBytes);
            } 
        }


        void WhenMessageIsReceived(string receivedMessage) {
            switch (this.receivedMessage) { 
               case "Close": 
                   CloseClient(); 
                   break; 
               default: 
                   DateAndTimeDataFromServer(receivedMessage, Color.black); 
                   break; 
           } 
           CloseClient();
       }
        
        void CloseClient() {
            if (tcpClient.Connected) { 
                tcpClient.Close(); 
            } 
            if (tcpClient != null) { 
                tcpClient = null; 
            }
            OnClientClosed?.Invoke(); 
        }
             
        protected virtual void DateAndTimeDataFromServer(string message, Color color) { 
            Debug.Log("<b>Client:</b> " + message); 
        }
        
        protected virtual void DateAndTimeDataFromServer(string message) { 
            Debug.Log("<b>Client:</b> " + message); 
        } 
    }
}

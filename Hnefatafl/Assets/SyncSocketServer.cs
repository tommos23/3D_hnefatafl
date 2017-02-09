using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace Assets
{

    public class SyncSocketServer
    {

        // Incoming data from the client.
        public static string data = null;
        public bool messageFlag = false;

        private IPEndPoint localEndPoint;
        private Socket listener;
        private Socket handler;

        public SyncSocketServer(System.Int32 port)
        {
            // Establish the local endpoint for the socket.
            // Dns.GetHostName returns the name of the 
            // host running the application.
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[1];
            localEndPoint = new IPEndPoint(ipAddress, port);

            try {
                // Create a TCP/IP socket.
                listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // Start listening for connections.
                listener.Bind(localEndPoint);
                listener.Listen(10);
            } 
            catch(Exception e)
            {
                Debug.Log(e.ToString());
            }
        }

        public void StartListening()
        {
            messageFlag = false;

            // Data buffer for incoming data.
            byte[] bytes = new Byte[1024];

            // Bind the socket to the local endpoint and 
            // listen for incoming connections.
            try
            {

                Debug.Log("Waiting for a connection...");
                // Program is suspended while waiting for an incoming connection.
                handler = listener.Accept();
                data = null;

                // An incoming connection needs to be processed.
                while (true)
                {
                    bytes = new byte[1024];
                    int bytesRec = handler.Receive(bytes);
                    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    if (data.IndexOf("<EOF>") > -1)
                    {
                        break;
                    }
                }

                // Show the data on the console.
                Console.WriteLine("Text received : {0}", data);

                messageFlag = true;
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }           
        }

        public void SendData(string data)
        {
            // Echo the data back to the client.
            byte[] msg = Encoding.ASCII.GetBytes(data);

            handler.Send(msg);
            handler.Shutdown(SocketShutdown.Both);
        }

        public string GetData()
        {
            return data;
        }

        public void CloseSocket()
        {
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }
    }
}

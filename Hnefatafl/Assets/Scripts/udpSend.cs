﻿/*
 
    -----------------------
    UDP-Send
    -----------------------
    // [url]http://msdn.microsoft.com/de-de/library/bb979228.aspx#ID0E3BAC[/url]
   
    // > gesendetes unter
    // 127.0.0.1 : 8050 empfangen
   
    // nc -lu 127.0.0.1 8050
 
        // todo: shutdown thread at the end
*/
using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class udpSend : MonoBehaviour
{
    // prefs
    private string IP = GameLoad.ipAddrString; // get ip address from splash screen
    public int port;  // define in init

    // "connection" things
    IPEndPoint remoteEndPoint;
    UdpClient client;

    // start from unity3d
    public void Start()
    {
        print("UDPSend.init()");
        
        // Create new udp client
        client = new UdpClient();

        // status
        print("Sending to " + IP + " : " + port);
        print("Testing: nc -lu " + IP + " : " + port);
    }

    public void UpdatePort(int portNumber)
    {
        port = portNumber;
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), port);
    }


    // inputFromConsole
    private void inputFromConsole()
    {
        try
        {
            string text;
            do
            {
                text = Console.ReadLine();

                // Den Text zum Remote-Client senden.
                if (text != "")
                {

                    // Daten mit der UTF8-Kodierung in das Binärformat kodieren.
                    byte[] data = Encoding.UTF8.GetBytes(text);

                    // Den Text zum Remote-Client senden.
                    client.Send(data, data.Length, remoteEndPoint);
                }
            } while (text != "");
        }
        catch (Exception err)
        {
            print(err.ToString());
        }

    }

    // sendData
    public void sendString(string message)
    {
        try
        {

            if (message != "")
            {
                Debug.Log("Sending : " + message + " to " + remoteEndPoint.Address.ToString() + " : "  + remoteEndPoint.Port.ToString());
                //Daten mit der UTF8-Kodierung in das Binärformat kodieren.
                byte[] data = Encoding.UTF8.GetBytes(message);

                // Den message zum Remote-Client senden.
                client.Send(data, data.Length, remoteEndPoint);
            }

            
        }
        catch (Exception err)
        {
            print(err.ToString());
        }

        
    }

    private void OnDestroy()
    {
        client.Close();
    }

}
using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class udpReceive : MonoBehaviour
{

    // receiving Thread
    private Thread receiveThread;

    // udpclient object
    private UdpClient client;

    // port to receive on
    private int port;
    // documented send port as used for gui
    int sendPort;

    // packet information
    public string lastReceivedUDPPacket = "";
    public string allReceivedUDPPackets = ""; // clean up this from time to time!
    public string currentPacket = "";


 
    // called on initialise
    public void Start()
    {

        print("UDPSend.init()");
        print("Receiving on port : " + port);

        // connect thread to receiver function
        receiveThread = new Thread( new ThreadStart(ReceiveData));

    }

    // updates the port we receive on
    public void UpdatePort(int portNumber)
    {
        port = portNumber;
        sendPort = port + 1;
    }


    // call when we are ready to receive a move
    public void StartListening()
    {
        //if not already listening
        if (!receiveThread.IsAlive)
        {
            Assets.ApplicationModel.runThread = true;
            receiveThread = new Thread(new ThreadStart(ReceiveData));
            receiveThread.IsBackground = true;
            receiveThread.Start();
        }
    }

    // getLatestUDPPacket
    public string getLatestUDPPacket()
    {
        string packetToReturn = currentPacket;
        currentPacket = "";
        return packetToReturn;
    }



    // receive thread
    private void ReceiveData()
    {

        client = new UdpClient(port);
        while (Assets.ApplicationModel.runThread)
        {

            try
            {
                // connect to endpoint and get bytes
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = client.Receive(ref anyIP);

                // encode byte array to string
                string text = Encoding.UTF8.GetString(data);
                print(">> " + text);

                // update latest UDPpacket
                lastReceivedUDPPacket = text;
                currentPacket = text;
                allReceivedUDPPackets = allReceivedUDPPackets + text + "\n\r";

                // check if we need to stop listeing (i.e. when a valid move is received)
                if(currentPacket.Contains("<EOF>"))
                {
                    Assets.ApplicationModel.runThread = false;
                    break;
                }

            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }

        // close the client when the thread expires
        client.Close();
    }



    void OnGUI()
    {
        if (port == 11000)
        {
            Rect rectObj = new Rect(40, 10, 200, 400);
            GUIStyle style = new GUIStyle();
            style.alignment = TextAnchor.UpperLeft;
            GUI.Box(rectObj, "Player 1 : \r\n"
                        + "UDP Receiving on port : " + port + " \n"
                        + "UDP Sending on port : " + sendPort
                        + "\n"
                        + "\nLast Move: \n" + lastReceivedUDPPacket
                        + "\n\nAll Moves: \n" + allReceivedUDPPackets
                    , style);
        }

        else
        {
            Rect rectObj = new Rect(Screen.width - 300, 10, 200, 400);
            GUIStyle style = new GUIStyle();
            style.alignment = TextAnchor.UpperLeft;
            GUI.Box(rectObj, "Player 2 : \r\n" +
                          "UDP Receiving on port : " + port + " \n"
                        + "UDP Sending on port : " + sendPort
                        + "\n"
                        + "\nLast Move: \n" + lastReceivedUDPPacket
                        + "\n\nAll Moves: \n" + allReceivedUDPPackets
                    , style);
        }
    }


    private void OnDestroy()
    {
        // kill send thread here
        Assets.ApplicationModel.runThread = false;
        receiveThread.IsBackground = false;
        currentPacket = "<EOF>";
    }
}
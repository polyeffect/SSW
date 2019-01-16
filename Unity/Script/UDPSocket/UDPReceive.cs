using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPReceive : MonoBehaviour
{
    private UDPEventModule UDPEventHandler;

    public int receivePort = 6000;
    Thread thread;
    UdpClient udpReceive;
    static readonly object lockObject = new object();
    string receiveData = "welcome";

    bool processData = false;
    
    void Start()
    {
        UDPEventHandler = this.GetComponent<UDPEventModule>();

        thread = new Thread(new ThreadStart(ThreadMethod));
        thread.Start();
    }

    void Update()
    {
        if (processData)
        {
            lock (lockObject)
            {
                processData = false;
                UDPEventHandler.UDPDataReceiver(receiveData);
            }
        }
    }

    // ---------- Receive data ----------
    private void ThreadMethod()
    {
        udpReceive = new UdpClient(receivePort);

        while (true)
        {
            IPEndPoint RemoteIPEndPoint = new IPEndPoint(IPAddress.Any, receivePort);
            //print(RemoteIPEndPoint);

            byte[] receiveBytes = udpReceive.Receive(ref RemoteIPEndPoint);

            lock (lockObject)
            {
                receiveData = Encoding.ASCII.GetString(receiveBytes);
                //print("Receive data: " + receiveData);

                processData = true;
            }
        }
    }

    // ---------- Quit applucation ----------
    private void OnApplicationQuit()
    {
        udpReceive.Close();
        thread.Abort();
    }
}

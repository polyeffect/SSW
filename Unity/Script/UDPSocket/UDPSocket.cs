using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPSocket : MonoBehaviour
{
    public int receivePort = 6000;  // port: 6000
    public int sendPort = 8888;  // port: 8888

    Thread thread;

    UdpClient udpReceive;
    UdpClient udpSend;
    static readonly object lockObject = new object();

    string receiveData = "welcome";
    string msg = "Hello Unity";

    bool processData = false;

    void Start()
    {
        udpSend = new UdpClient(sendPort);

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
                print("Process Data!!!");
            }
        }
    }

    // ---------- receive data ----------
    private void ThreadMethod()
    {
        udpReceive = new UdpClient(receivePort);

        while(true)
        {
            //IPEndPoint RemoteIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.2"), receivePort);
            IPEndPoint RemoteIPEndPoint = new IPEndPoint(IPAddress.Any, receivePort);
            print(RemoteIPEndPoint);

            byte[] receiveBytes = udpReceive.Receive(ref RemoteIPEndPoint);

            lock (lockObject)
            {
                receiveData = Encoding.ASCII.GetString(receiveBytes);
                print("Receive data: " + receiveData);

                if (receiveData == "i01")
                {
                    processData = true;
                }
            }
        }
    }

    // ---------- Send data ----------
    public void SendSocketData()
    {
        //Debug.Log("Send Socket Data: " + msg);
        IPEndPoint TargetIPEndPoint = new IPEndPoint(IPAddress.Broadcast, sendPort);
        byte[] sendBytes = Encoding.UTF8.GetBytes(msg);
        udpSend.Send(sendBytes, sendBytes.Length, TargetIPEndPoint);
    }

    // ---------- Quit applucation ----------
    private void OnApplicationQuit()
    {
        udpReceive.Close();
        udpSend.Close();
        thread.Abort();
    }
}

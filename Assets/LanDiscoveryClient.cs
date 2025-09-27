using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;

public class LanDiscoveryClient : MonoBehaviour
{
    public string roomCode;
    public event Action<string> OnHostFound;

    private UdpClient udpClient;
    private Thread listenThread;
    private bool running;

    void Start()
    {
        udpClient = new UdpClient();
        udpClient.EnableBroadcast = true;
        running = true;

        // Start listening for responses
        listenThread = new Thread(ListenLoop);
        listenThread.IsBackground = true;
        listenThread.Start();

        // Broadcast discovery message
        BroadcastDiscovery();
    }

    private void BroadcastDiscovery()
    {
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Broadcast, 47777);
        string message = "DISCOVER:" + roomCode;
        byte[] data = Encoding.UTF8.GetBytes(message);
        udpClient.Send(data, data.Length, endPoint);
    }

    private void ListenLoop()
    {
        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);

        while (running)
        {
            try
            {
                byte[] data = udpClient.Receive(ref remoteEP);
                string message = Encoding.UTF8.GetString(data);

                // Expected: "HOST:<ip>"
                if (message.StartsWith("HOST:"))
                {
                    string hostIp = message.Substring("HOST:".Length);
                    Debug.Log("Discovered host at " + hostIp);

                    OnHostFound?.Invoke(hostIp);

                    running = false; // stop after first host
                }
            }
            catch { }
        }
    }

    void OnDestroy()
    {
        running = false;
        udpClient?.Close();
        listenThread?.Abort();
    }
}

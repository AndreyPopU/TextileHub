using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;

public class LanDiscoveryHost : MonoBehaviour
{
    public string roomCode; // set by WebSocket host
    private UdpClient udpServer;
    private Thread listenThread;
    private bool running;

    void Start()
    {
        // Start the server on port 47777
        udpServer = new UdpClient(47777);
        running = true;

        // Create a background thread that listens for messages with the room code
        listenThread = new Thread(ListenLoop);
        listenThread.IsBackground = true;
        listenThread.Start();
    }

    private void ListenLoop() // Running thread that checks for room code messages
    {
        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);

        while (running)
        {
            try
            {
                byte[] data = udpServer.Receive(ref remoteEP);
                string message = Encoding.UTF8.GetString(data);

                // Expected format: "DISCOVER:<roomCode>"
                if (message.StartsWith("DISCOVER:"))
                {
                    // Strip down any unnecessary stuff
                    string requestedCode = message.Substring("DISCOVER:".Length);
                    string cleanRequest = Regex.Replace(requestedCode, @"[\s\u200B-\u200D\uFEFF]", ""); ;

                    // If the requested room code is the same as the hosted - provide the IPAddress to the connecting client
                    if (cleanRequest == roomCode)
                    {
                        print("Request matches room code - joining new player");

                        // Reply with our IP address
                        string ip = GetLocalIPAddress();
                        byte[] response = Encoding.UTF8.GetBytes("HOST:" + ip);
                        udpServer.Send(response, response.Length, remoteEP);
                    }
                }
            }
            catch { Debug.LogError("nqma takuv kod"); } //neka si bude tuk :)
        }
    }

    private string GetLocalIPAddress()
    {
        foreach (var ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
                return ip.ToString();
        }
        return "127.0.0.1";
    }

    void OnDestroy()
    {
        running = false;
        udpServer?.Close();
        listenThread?.Abort();
    }

    private void OnApplicationQuit()
    {
        running = false;
        udpServer?.Close();
        listenThread?.Abort();
    }

    void DebugString(string label, string s) // Debug Function for filtering the room code
    {
        Debug.Log($"{label}: '{s}' (Length {s.Length})");
        for (int i = 0; i < s.Length; i++)
        {
            Debug.Log($"{label}[{i}] = '{s[i]}' (Unicode: {(int)s[i]})");
        }
    }
}

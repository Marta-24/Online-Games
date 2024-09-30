using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using TMPro;

public class ClientUDP : MonoBehaviour
{
    Socket socket;
    public GameObject UItextObj;
    TextMeshProUGUI UItext;
    string clientText;

    void Start()
    {
        UItext = UItextObj.GetComponent<TextMeshProUGUI>();
    }

    public void StartClient()
    {
        // Start a thread to send a message to the server
        Thread mainThread = new Thread(Send);
        mainThread.Start();
    }

    void Update()
    {
        UItext.text = clientText;
    }

    void Send()
    {
        // TO DO 2
        // Create the server's endpoint (IP and port) and initialize the socket
        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9050); // Replace with your server's IP and port
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp); // UDP socket

        // TO DO 2.1
        // Send the Handshake to the server's endpoint
        string handshake = "Hello World";
        byte[] data = Encoding.ASCII.GetBytes(handshake);
        socket.SendTo(data, data.Length, SocketFlags.None, ipep); // Send handshake to server

        clientText = "Sent handshake to server: " + handshake;

        // TO DO 5
        // Start a thread to receive the response from the server
        Thread receive = new Thread(Receive);
        receive.Start();
    }

    void Receive()
    {
        // TO DO 5
        // Prepare to receive data from the server
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0); // Sender (server) endpoint
        EndPoint Remote = (EndPoint)sender;

        byte[] data = new byte[1024]; // Buffer to store received data
        int recv;

        // Receive data from the server
        try
        {
            recv = socket.ReceiveFrom(data, ref Remote);
            string receivedMessage = Encoding.ASCII.GetString(data, 0, recv); // Convert received data to string

            // Display the received message
            clientText = $"Message received from {Remote.ToString()}: {receivedMessage}";
        }
        catch (SocketException e)
        {
            clientText = "Error receiving message: " + e.Message;
        }

        // Optionally, you can close the socket if done
        socket.Close();
    }
}

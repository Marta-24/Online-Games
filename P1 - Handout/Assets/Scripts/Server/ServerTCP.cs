using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using TMPro;
using System.Text;

public class ServerTCP : MonoBehaviour
{
    Socket socket;
    Thread mainThread = null;

    public GameObject UItextObj;
    TextMeshProUGUI UItext;
    string serverText;

    public struct User
    {
        public string name;
        public Socket socket;
    }

    void Start()
    {
        UItext = UItextObj.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        UItext.text = serverText;
    }

    public void startServer()
    {
        serverText = "Starting TCP Server...";

        //TO DO 1
        //Create and bind the socket
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        // Bind the socket to any IP address on port 9050
        IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 9050);
        socket.Bind(localEndPoint);

        // Put the socket in listening mode
        socket.Listen(10);
        serverText += "\nServer listening on port 9050...";

        //TO DO 3
        //Time to check for connections, start a thread using CheckNewConnections
        mainThread = new Thread(CheckNewConnections);
        mainThread.Start();
    }

    void CheckNewConnections()
    {
        while (true)
        {
            User newUser = new User();
            newUser.name = "";

            //TO DO 3
            // Accept any incoming clients
            newUser.socket = socket.Accept();

            // Get the remote endpoint (client info)
            IPEndPoint clientEP = (IPEndPoint)newUser.socket.RemoteEndPoint;
            serverText += "\nConnected with " + clientEP.Address.ToString() + " at port " + clientEP.Port.ToString();

            //TO DO 5
            // For every client, we call a new thread to receive their messages.
            Thread newConnection = new Thread(() => Receive(newUser));
            newConnection.Start();
        }
    }

    void Receive(User user)
    {
        //TO DO 5
        // Create an infinite loop to start receiving messages for this user
        byte[] data = new byte[1024];
        int recv;

        while (true)
        {
            try
            {
                recv = user.socket.Receive(data);
                if (recv == 0)
                    break;

                // Convert received data to string
                string receivedMessage = Encoding.ASCII.GetString(data, 0, recv);
                serverText += "\nReceived: " + receivedMessage;

                //TO DO 6
                // Send a ping back every time a message is received
                Thread answer = new Thread(() => Send(user));
                answer.Start();
            }
            catch (SocketException)
            {
                // Handle socket exceptions
                break;
            }
        }

        // Close the socket when done
        user.socket.Close();
    }

    //TO DO 6
    // Now, we'll use this user's socket to send a "ping".
    void Send(User user)
    {
        // Encode the "ping" message and send it
        string message = "Ping from server";
        byte[] data = Encoding.ASCII.GetBytes(message);
        user.socket.Send(data);
    }
}

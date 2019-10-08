using System;
using System.Net.Sockets;
using System.Threading;

namespace Tcp_Client
{
    class Program
    {
        const string ipAddr = "127.0.0.1";
        const int port = 4646;
        static void Main(string[] args)
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Connect(message: "Get");
            }).Start();
            Console.ReadLine();
        }
        static void Connect(String message)
        {
            try
            {
                TcpClient client = new TcpClient(ipAddr, port);
                NetworkStream stream = client.GetStream();

                byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                stream.Write(data, 0, data.Length);
                Console.WriteLine($"Sent: {message}");

                data = new byte[256];
                int bytes = stream.Read(data, 0, data.Length);
                var response = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine($"Received: {response}");

                Thread.Sleep(1000);
                stream.Close();
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e}");
            }
        }
    }
}
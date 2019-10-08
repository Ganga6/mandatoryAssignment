using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Text.Json;
using BookLib;
using System.Collections.Generic;

namespace TcpServer
{
    class Server
    {
        TcpListener server = null;
        private List<Book> BooksList = new List<Book>
        {
            new Book("Book Title", "Author Name", 200, "1111111111111"),
            new Book("Book Title 1", "Author Name 1", 21, "1111111111112"),
            new Book("Book Title 2", "Author Name 2", 440, "1111111111123"),
            new Book("Book Title 3", "Author Name 3", 10, "1111111111234"),
            new Book("Book Title 4", "Author Name 4", 800, "1111111112345"),
            new Book("Book Title 5", "Author Name 5", 80, "1111111123456"),
        };
        public Server(string ip, int port)
        {
            server = new TcpListener(IPAddress.Parse(ip), port);
            server.Start();
            StartListener();
        }

        public void StartListener()
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    Thread t = new Thread(new ParameterizedThreadStart(HandleClient));
                    t.Start(client);
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine($"SocketException: {e}");
                server.Stop();
            }
        }

        public void HandleClient(object obj)
        {
            TcpClient client = (TcpClient)obj;
            var stream = client.GetStream();

            string data = null;
            byte[] bytes = new byte[256];
            int i;
            try
            {
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    string hex = BitConverter.ToString(bytes);
                    data = Encoding.ASCII.GetString(bytes, 0, i);
                    byte[] reply = Encoding.ASCII.GetBytes(OnDataRecived(data.Trim()));
                    stream.Write(reply, 0, reply.Length);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e}");
                client.Close();
            }
        }

        private string OnDataRecived(string data)
        {
            var responseMsg = "";
            Console.WriteLine($"DeviceId: {Thread.CurrentThread.ManagedThreadId}\t Data(Recivied): '{data}'");
            var commands = data.Split(' ');
            switch (commands[0].ToLower())
            {
                case "getall":
                    responseMsg = JsonSerializer.Serialize(BooksList);
                    break;
                case "get":
                    responseMsg = string.IsNullOrEmpty(commands[1]) ?
                        "You need to provide additional Parameters (Get <ISBN_Number>)" :
                        JsonSerializer.Serialize(BooksList.Find(x => x.ISBN13 == commands[1]));
                    break;
                case "save":
                    //TODO: Make sure the ISBN is actually unique
                    if(string.IsNullOrEmpty(commands[1])) {
                        responseMsg = "You need to provide additional Parameters (Get <ISBN_Number>)";
                    }
                    BooksList.Add(JsonSerializer.Deserialize<Book>(commands[1]));
                    break;
                case "?":
                    responseMsg = "Aviable Commands: GetAll, Get, Save";
                    break;
                default:
                    Console.WriteLine($"DeviceId: {Thread.CurrentThread.ManagedThreadId}\t Data recived does not any known command! ({data})");
                    break;
            }

            Console.WriteLine($"DeviceId: {Thread.CurrentThread.ManagedThreadId}\t Data(Sent): '{responseMsg}'");
            return responseMsg + '\n';
        }
    }
}
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Opgave_1_Csharp;

namespace Opgave_5_TCP_server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("TCP-Server is startet");

            TcpListener listener = new TcpListener(IPAddress.Any, port: 10002);
            listener.Start();

            while (true)
            {
                TcpClient sockert = listener.AcceptTcpClient();
                Console.WriteLine("New client");

                Task.Run(() =>
                {
                    HandleClient(sockert);
                });
            }
        }
        private static void HandleClient(TcpClient sockert)
        {
            NetworkStream ns = sockert.GetStream();
            StreamReader reader = new StreamReader(ns);
            StreamWriter writer = new StreamWriter(ns);
            string message = reader.ReadLine();
        }
     }
}

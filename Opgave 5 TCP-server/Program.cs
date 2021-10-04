using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;
using Opgave_1_Csharp;

namespace Opgave_5_TCP_server
{
    class Program
    {
        public static List<FootballPlayer> footballPlayers = new List<FootballPlayer>
        {
            new FootballPlayer {Id = 1, Name = "Benjamin", Price = 3000000, ShirtNumber = 14},
            new FootballPlayer {Id = 2, Name = "Peter", Price = 250, ShirtNumber = 10}
        };
        static void Main(string[] args)
        {
            Console.WriteLine("TCP-Server is startet");

            TcpListener listener = new TcpListener(IPAddress.Any, port: 2121);
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
            string messageID = reader.ReadLine();

            writer.WriteLine("Valgmuligheder for denne server er HA for hent alle, HID for hent for fodboldspillerns id eller G for at gemme");

            if(message.ToUpper().StartsWith("HA"))
            {
                if(footballPlayers != null)
                {
                    foreach (var players in footballPlayers)
                    {
                        writer.WriteLine($"Spillersnavn: {players.Name} Spillers pris: {players.Price} Spillers trøjenummer: {players.ShirtNumber}");
                        writer.Flush();
                    }
                }   
            } else if(message.ToUpper() == "HID")
            {
                int id = -1;
                if(int.TryParse(messageID, out id))
                {
                    foreach(var players in footballPlayers)
                    {
                        if(players.Id == id)
                        {
                            writer.WriteLine($"Id på den søgte spiller: {players.Id}, Name: {players.Name}, Price: {players.Price}, Shirtnumber: {players.ShirtNumber}");
                            writer.Flush();
                        }
                    }
                }
            }else if(message.ToUpper().StartsWith("G"))
            {
                FootballPlayer player = new FootballPlayer();
                footballPlayers.Add(player);
                player = JsonSerializer.Deserialize<FootballPlayer>(messageID);

                writer.WriteLine("Spiller gemt");
                writer.Flush();
            }
            sockert.Close();
        }
     }
}

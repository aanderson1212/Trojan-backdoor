using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace trojart
{
    class Trojan
    {
        public static bool IsConnected;
        public static NetworkStream Writer;
        static void Main(string[] args)
        {
            TcpClient Connector = new TcpClient();

            GetConnection:
            Console.WriteLine("Enter Server IP: ");
            string IP = Console.ReadLine();

            try
            {
                Connector.Connect(IP, 2000);
                IsConnected = true;
                Console.Title = "Client - Online";
                Writer = Connector.GetStream();
            }

            catch
            {
                Console.WriteLine("Error connecting to target server! press any key to try again.");
                Console.ReadKey();
                Console.Clear();
                goto GetConnection;
            }

            Console.WriteLine("Connection successfully established to " + IP + ".");

            Console.WriteLine("Type HELP for a list of commands.");

            while (IsConnected)
            {
                Console.WriteLine("Enter command : ");

                string CMD = Console.ReadLine();

                if (CMD == "HELP")
                {
                    Console.WriteLine("COMMANDS");
                }
                else
                {
                    SendCommand(CMD);
                }
            }


        }
        public static void SendCommand(string Command)
        {
            try
            {
                byte[] Packet = Encoding.ASCII.GetBytes(Command);
                Writer.Write(Packet, 0, Packet.Length);
                Writer.Flush();
            }
            catch
            {
                IsConnected = false;
                Console.WriteLine("Disconnected from server!");
                Console.ReadKey();
                Writer.Close();
            }
        }
    }
}

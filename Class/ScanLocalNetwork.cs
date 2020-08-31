using HomeConnect.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HomeConnect.Class
{
    public static class ScanLocalNetwork
    {
        public static List<HubModel> hubs = new List<HubModel>();
        private static int j = 2;
        private static int i = 2;
        public static bool IsOk = false;
        public static void Ping_all()
        {
            IPAddress[] addresses = Dns.GetHostAddresses(Dns.GetHostName());
            string[] ipAddress = new string[4];
            if (addresses != null && addresses[0] != null)
            {
                ipAddress = addresses[1].ToString().Split('.');
                j = 2;
                
                for (i = 2; i <= 255; i++)
                {
                    string ping_var = ipAddress[0] + "." + ipAddress[1] + "." + ipAddress[2] + "." + i;
                    Console.WriteLine(ping_var);
                    Ping(ping_var, 1, 4000);

                }

            }
            else
            {
                return;
            }
            

        }

        private static void Ping(string host, int attempts, int timeout)
        {
            for (int i = 0; i < attempts; i++)
            {
                new Thread(delegate ()
                {
                    try
                    {
                        Ping ping = new Ping();
                        ping.PingCompleted += new PingCompletedEventHandler(PingCompleted);
                        ping.SendAsync(host, timeout, host);
                    }
                    catch
                    {
                        j++;
                    }
                }).Start();
            }
        }
        private static void PingCompleted(object sender, PingCompletedEventArgs e)
        {
            string ip = (string)e.UserState;
            if(ip == "192.168.1.59")
            {
                Console.WriteLine("e");
            }
            if (e.Reply != null && e.Reply.Status == IPStatus.Success)
            {
                string hostname = GetHostName(ip) + " " + ip;
                if (hostname == null)
                {
                    hostname = "unamed";
                }
                 ScanLocalNetwork.hubs.Add(new HubModel() { Ip = ip, Name = hostname });
            }
            j++;
            Console.WriteLine("j: " + j.ToString() + " i: " + i.ToString()); 
            if (256 == j)
            {
                IsOk = true;
            }
        }

        private static string GetHostName(string ipAddress)
        {
            try
            {
                IPHostEntry entry = Dns.GetHostEntry(ipAddress);
                if (entry != null)
                {
                    return entry.HostName;
                }
            }
            catch (SocketException)
            {
            }
            return null;
        }
    }
}

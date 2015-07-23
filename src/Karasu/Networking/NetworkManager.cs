using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Karasu.Networking
{
    public static class NetworkManager
    {
        public static string GetLanIp()
        {
            var hostName = Dns.GetHostName();
            var entries = Dns.GetHostAddresses(hostName);

            foreach (var entry in entries)
            {
                if (entry.AddressFamily != AddressFamily.InterNetwork) continue;

                var bytes = entry.GetAddressBytes();

                if (bytes[0] == 10 || (bytes[0] == 172 && 15 < bytes[1] && bytes[1] < 32) || (bytes[0] == 192 && 167 < bytes[1] && bytes[1] < 169))
                {
                    return entry.ToString();
                }
            }

            return null;
        }
    }
}

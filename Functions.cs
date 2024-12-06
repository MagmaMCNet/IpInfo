using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

namespace IpInfo
{
    internal class Functions
    {

        [DllImport("iphlpapi.dll", CharSet = CharSet.Auto)]
        private static extern int GetBestInterface(UInt32 destAddr, out UInt32 bestIfIndex);

        public static string GetIPV4()
        {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                return endPoint.Address.ToString();
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public static string GetDefaultGateway()
        {
            foreach (NetworkInterface f in NetworkInterface.GetAllNetworkInterfaces())
                if (f.OperationalStatus == OperationalStatus.Up)
                    foreach (GatewayIPAddressInformation d in f.GetIPProperties().GatewayAddresses)
                        if (!d.Address.ToString().Contains("::"))
                            return d.Address.ToString();
            return "0.0.0.0";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
namespace UMI.Network.Server
{
    class UMIServerListener
    {
        public static UMIServerListener star;
        public static int maxPlayer { get; private set; }
        public static int port { get; private set; }
        public static Dictionary<int, UMIServerManager> clients = new Dictionary<int, UMIServerManager>();
        public delegate void PacketHandler(int client, UMIPacket packet);
        public static Dictionary<int, PacketHandler> packetHandle;

        public static UdpClient UMIUDPListener;
        public static TcpListener UMITCPListener;
        private static void initializeServerData()
        {
            for (int i = 1; i <= maxPlayer; i++)
            {
                try
                {
                    clients.Add(i, new UMIServerManager(i));
                }
                catch
                {
                    UMISystem.Log(i + "ERR");
                }
            }
            packetHandle = new Dictionary<int, PacketHandler>()
            {
                //receive
                {   (int)YUMIClientPackets.reqConnectServer , UMIServerHandle.connetServer },

            };
            UMISystem.Log("initializeServer");
        }
        public static void Start(int maxPlayerV, int portV)
        {
            maxPlayer = maxPlayerV;
            port = portV;
            UMISystem.Log("Server Running");
            initializeServerData();
            UMITCPListener = new TcpListener(IPAddress.Any, port);
            UMITCPListener.Start();
            UMITCPListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);
            UMIUDPListener = new UdpClient(port);
            UMIUDPListener.BeginReceive(UDPReceiveCallback, null);
        }
        private static void TCPConnectCallback(IAsyncResult result)
        {
            TcpClient client = UMITCPListener.EndAcceptTcpClient(result);
            UMITCPListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);
            UMISystem.Log($"Connect IPAddress From -> {client.Client.RemoteEndPoint}");
            for (int i = 1; i <= maxPlayer; i++)
            {
                if (clients[i].TCP.socket == null)
                {
                    clients[i].TCP.Connect(client);
                    return;
                }
            }
            UMISystem.Log($"UMI::STATUSSERVER()->{client.Client.RemoteEndPoint}.FULL");
        }
        private static void UDPReceiveCallback(IAsyncResult result)
        {
            try
            {
                IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = UMIUDPListener.EndReceive(result, ref clientEndPoint);
                UMIUDPListener.BeginReceive(UDPReceiveCallback, null);
                if (data.Length < 4)
                {
                    UMISystem.Log("server disconnet");
                    return;
                }
                using (UMIPacket packet = new UMIPacket(data))
                {
                    int CID = packet.ReadInt();

                    if (CID == 0)
                    {
                        UMISystem.Log("CID Error");
                        return;
                    }
                    if (clients[CID].UDP.endPoint == null)
                    {
                        clients[CID].UDP.Connect(clientEndPoint);
                        return;
                    }
                    if (clients[CID].UDP.endPoint.ToString() == clientEndPoint.ToString())
                    {
                        clients[CID].UDP.HandleData(packet);
                    }
                }
            }
            catch (Exception ex)
            {
                UMISystem.Log($"Error UDP connect {ex}");
            }
        }
        public static void SendUdpData(IPEndPoint cEndPoint, UMIPacket packet)
        {
            try
            {
                if (cEndPoint != null)
                {
                    UMIUDPListener.BeginSend(packet.ToArray(), packet.Length(), cEndPoint, null, null);
                }
            }
            catch (Exception ex)
            {
                UMISystem.Log($"Error UDP send data {ex}");
            }
        }



    }
}
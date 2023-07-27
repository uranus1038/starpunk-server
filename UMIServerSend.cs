
namespace UMI.Network.Server
{
    public class UMIServerSend
    {
        #region UMIFUNC TCP & UDP Sendeer
        private static void SendTCPData(int toClient, UMIPacket packet)
        {
            packet.WriteLength();
            UMIServerListener.clients[toClient].TCP.SendData(packet);
        }
        private static void SendUDPData(int toClient, UMIPacket packet)
        {
            packet.WriteLength();
            UMIServerListener.clients[toClient].UDP.SendData(packet);
        }
        //Send tcp data to all users
        private static void sendTCPDataALL(UMIPacket packet)
        {
            packet.WriteLength();
            for (int i = 1; i <= UMIServerListener.maxPlayer; i++)
            {
                UMIServerListener.clients[i].TCP.SendData(packet);
            }
        }
        private static void sendTCPDataExceptClient(int exceptClient, UMIPacket packet)
        {
            packet.WriteLength();
            for (int i = 1; i <= UMIServerListener.maxPlayer; i++)
            {
                try
                {
                    if (i != exceptClient)
                    {
                        UMIServerListener.clients[i].TCP.SendData(packet);
                    }
                }
                catch
                {
                    UMISystem.Log("ServerDown");
                }

            }
        }
        //Send udp data to all users
        private static void sendUDPDataALL(UMIPacket packet)
        {
            packet.WriteLength();
            for (int i = 1; i <= UMIServerListener.maxPlayer; i++)
            {
                UMIServerListener.clients[i].UDP.SendData(packet);
            }
        }
        private static void sendUDPDataExceptClient(int exceptClient, UMIPacket packet)
        {
            packet.WriteLength();
            for (int i = 1; i <= UMIServerListener.maxPlayer; i++)
            {
                if (i != exceptClient)
                {
                    UMIServerListener.clients[i].UDP.SendData(packet);
                }
            }
        }
        #endregion

        public static void connect(int UID, string msg)
        {
            using (UMIPacket packet = new UMIPacket((int)YUMIServerPackets.resConnectServer))
            {
                packet.Write(msg);
                packet.Write(UID);
                SendTCPData(UID, packet);
            }
        }
    }

}
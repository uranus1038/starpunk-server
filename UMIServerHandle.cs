using System;
using System.Numerics;
namespace UMI.Network.Server
{
    class UMIServerHandle
    {

        public static void connetServer(int client, UMIPacket packet)
        {
            int UID = packet.ReadInt();
            string userName = packet.ReadString();
            //UMIServer.clients[UID].SendIntoGame(userName);    
            UMISystem.Log($"successfully {UMIServerListener.clients[client].TCP.socket.Client.RemoteEndPoint} and is now player [{UID},{userName}]");
            if (client != UID)
            {
                UMISystem.Log($"Player {UID} id : {client}");
            }
        }

    }
}
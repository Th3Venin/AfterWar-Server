using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UdpConnection
{
    public IPEndPoint endPoint;

    private int id;

    public UdpConnection(int id)
    {
        this.id = id;
    }

    public void Connect(IPEndPoint _endPoint)
    {
        endPoint = _endPoint;
    }

    public void SendData(Packet packet)
    {
        Server.SendUDPData(endPoint, packet);
    }

    /// <summary>Prepares received data to be used by the appropriate packet handler methods.</summary>
    /// <param name="_packetData">The packet containing the recieved data.</param>
    public void HandleData(Packet packetData)
    {
        int packetLength = packetData.ReadInt();
        byte[] packetBytes = packetData.ReadBytes(packetLength);

        ThreadManager.ExecuteOnMainThread(() =>
        {
            using (Packet packet = new Packet(packetBytes))
            {
                int packetId = packet.ReadInt();
                Server.packetHandlers[packetId](id, packet); // Call appropriate method to handle the packet
            }
        });
    }

    /// <summary>Cleans up the UDP connection.</summary>
    public void Disconnect()
    {
        endPoint = null;
    }
}
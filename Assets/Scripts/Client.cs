using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Client
{
    public static int dataBufferSize = 4096;

    public int id;
    public Player player;
    public TcpConnection tcp;
    public UdpConnection udp;

    public Client(int clientId)
    {
        id = clientId;
        tcp = new TcpConnection(id, dataBufferSize);
        udp = new UdpConnection(id);
    }

    public void SendIntoGame(string playerName)
    {
        player = NetworkManager.instance.InstantiatePlayer();
        player.Initialize(id, playerName);

        foreach (Client client in Server.clients.Values)
        {
            if (client.player != null)
            {
                if (client.id != id)
                {
                    ServerSend.SpawnPlayer(id, client.player);
                }
            }
        }

        foreach (Client client in Server.clients.Values)
        {
            if (client.player != null)
            {
                ServerSend.SpawnPlayer(client.id, player);
            }
        }

        foreach(ItemSpawner itemSpawner in ItemSpawner.spawners.Values) {
            ServerSend.CreateItemSpawner(id, itemSpawner.spawnerId, itemSpawner.transform.position, itemSpawner.hasItem);
        }

        ServerSend.MatchStage(id);
    }

    public void Disconnect()
    {
        Debug.Log($"{tcp.socket.Client.RemoteEndPoint} has disconnected.");

        ThreadManager.ExecuteOnMainThread(() =>
        {
            if (player != null)
                UnityEngine.Object.Destroy(player.gameObject);
            player = null;
        });

        tcp.Disconnect();
        udp.Disconnect();

        if (id > 0)
            ServerSend.PlayerDisconnected(id);
    }
}
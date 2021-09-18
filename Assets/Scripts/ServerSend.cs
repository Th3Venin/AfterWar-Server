using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerSend
{
    private static void SendTCPData(int toClient, Packet packet)
    {
        packet.WriteLength();
        Server.clients[toClient].tcp.SendData(packet);
    }

    private static void SendUDPData(int toClient, Packet packet)
    {
        packet.WriteLength();
        Server.clients[toClient].udp.SendData(packet);
    }

    private static void SendTCPDataToAll(Packet packet)
    {
        packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].tcp.SendData(packet);
        }
    }

    private static void SendTCPDataToAll(int exceptClient, Packet packet)
    {
        packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != exceptClient)
            {
                Server.clients[i].tcp.SendData(packet);
            }
        }
    }

    private static void SendUDPDataToAll(Packet packet)
    {
        packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].udp.SendData(packet);
        }
    }

    private static void SendUDPDataToAll(int exceptClient, Packet packet)
    {
        packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != exceptClient)
            {
                Server.clients[i].udp.SendData(packet);
            }
        }
    }

    public static void Welcome(int toClient)
    {
        using (Packet packet = new Packet((int)ServerPackets.welcomeToGame))
        {
            packet.Write(toClient);
            Debug.Log("Sending welcome packet");
            SendTCPData(toClient, packet);
        }
    }

    public static void SpawnPlayer(int toClient, Player player)
    {
        using (Packet packet = new Packet((int)ServerPackets.spawnPlayer))
        {
            packet.Write(player.id);
            packet.Write(player.username);
            packet.Write(player.transform.position);
            packet.Write(player.transform.rotation);
            packet.Write(player.stats.deaths);
            packet.Write(player.stats.score);
            packet.Write(player.stats.kills);
            packet.Write((int)player.stats.equippedWeapon);
            packet.Write(player.stats.health);
            packet.Write(player.stats.armor);
            Debug.Log($"Sent spawn {player.id} to {toClient} with weapon {(int)player.stats.equippedWeapon}");
            SendTCPData(toClient, packet);
        }
    }

    public static void PlayerDisconnected(int playerId)
    {
        using (Packet packet = new Packet((int)ServerPackets.playerDisconnected))
        {
            packet.Write(playerId);

            SendTCPDataToAll(packet);
        }
    }

    public static void PlayerMovement(Player player)
    {
        using (Packet packet = new Packet((int)ServerPackets.playerMovement))
        {
            packet.Write(player.id);
            packet.Write(player.transform.position);
            packet.Write(player.transform.rotation);
            packet.Write(player.keysPressed.Length);
            foreach (bool key in player.keysPressed)
            {
                packet.Write(key);
            }
            packet.Write(player.isGrounded);
            packet.Write(player.spineAngle);
            SendUDPDataToAll(player.id, packet);
        }
    }

    public static void CreateItemSpawner(int toClient, int spawnerId, Vector3 position, bool hasItem)
    {
        using (Packet packet = new Packet((int)ServerPackets.createItemSpawner))
        {
            packet.Write(spawnerId);
            packet.Write(position);
            packet.Write(hasItem);
            packet.Write((int)ItemSpawner.spawners[spawnerId].weaponType);

            SendTCPData(toClient, packet);
        }
    }

    public static void CreateItemSpawner(int spawnerId, Vector3 position, bool hasItem)
    {
        using (Packet packet = new Packet((int)ServerPackets.createItemSpawner))
        {
            packet.Write(spawnerId);
            packet.Write(position);
            packet.Write(hasItem);
            packet.Write((int)ItemSpawner.spawners[spawnerId].weaponType);

            SendTCPDataToAll(packet);
        }
    }

    public static void ItemSpawned(int spawnerId)
    {
        using (Packet packet = new Packet((int)ServerPackets.itemSpawned))
        {
            packet.Write(spawnerId);
            SendTCPDataToAll(packet);
        }
    }

    public static void ItemPickedUp(int spawnerId, int byPlayer)
    {
        using (Packet packet = new Packet((int)ServerPackets.itemPickedUp))
        {
            packet.Write(spawnerId);
            packet.Write(byPlayer);
            SendTCPDataToAll(packet);
        }
    }

    public static void PlayerHealth(Player player)
    {
        using (Packet packet = new Packet((int)ServerPackets.playerHealth))
        {
            packet.Write(player.id);
            packet.Write(player.stats.health);

            SendTCPDataToAll(packet);
        }
    }

    public static void PlayerArmor(Player player)
    {
        using (Packet packet = new Packet((int)ServerPackets.playerArmor))
        {
            packet.Write(player.id);
            packet.Write(player.stats.armor);

            SendTCPDataToAll(packet);
        }
    }

    public static void EquippedWeapon(Player player, WeaponTypes type)
    {
        using (Packet packet = new Packet((int)ServerPackets.equippedWeapon))
        {
            packet.Write(player.id);
            packet.Write((int)type);

            SendTCPDataToAll(packet);
        }

        Weapon weapon = player.transform.root.GetComponent<Inventory>().GetActiveWeaponStats();
        if (weapon != null)
        {
            ServerSend.PlayerAmmo(player.id, weapon.currentMagazine, weapon.ammo);
            Debug.Log("Sent ammo " + weapon.currentMagazine + " " + weapon.ammo);
        }

        Debug.Log("weapon null can't send ammo");
    }

    public static void UnEquippedWeapon(int playerId)
    {
        using (Packet packet = new Packet((int)ServerPackets.unEquippedWeapon))
        {
            packet.Write(playerId);

            SendTCPDataToAll(packet);
        }
    }

    public static void PlayerAmmo(int toPlayer, int magazine, int reserve)
    {
        using (Packet packet = new Packet((int)ServerPackets.playerAmmo))
        {
            packet.Write(toPlayer);
            packet.Write(magazine);
            packet.Write(reserve);

            SendTCPData(toPlayer, packet);
        }
    }

    public static void PlayerWeapons(int toPlayer, WeaponTypes[] weapons)
    {
        using (Packet packet = new Packet((int)ServerPackets.playerWeapons))
        {
            packet.Write((int)weapons[0]);
            packet.Write((int)weapons[1]);
            packet.Write((int)weapons[2]);
            packet.Write((int)weapons[3]);

            SendTCPData(toPlayer, packet);
        }
    }

    public static void PlayerEliminated(Player player)
    {
        using (Packet packet = new Packet((int)ServerPackets.playerEliminated))
        {
            packet.Write(player.id);
            packet.Write(player.stats.damage);
            packet.Write(player.stats.kills);
            float hsPercent = 0;

            if (player.stats.shots != 0)
                hsPercent = (float)player.stats.headShots / (float)player.stats.shots;

            packet.Write(hsPercent);
            SendTCPDataToAll(packet);
        }
    }

    public static void PlayerChosenSpawn(int playerId, Vector2 spawn)
    {
        using (Packet packet = new Packet((int)ServerPackets.playerChosenSpawn))
        {
            packet.Write(playerId);
            packet.Write(spawn);
            SendTCPDataToAll(playerId, packet);
        }
    }

    public static void MatchStage(int playerId)
    {
        using (Packet packet = new Packet((int)ServerPackets.matchStage))
        {
            packet.Write((int)Match.instance.stage);
            SendTCPData(playerId, packet);
        }
    }

    public static void MatchStage()
    {
        using (Packet packet = new Packet((int)ServerPackets.matchStage))
        {
            packet.Write((int)Match.instance.stage);
            SendTCPDataToAll(packet);
        }
    }

    public static void ServerTime(int time)
    {
        using (Packet packet = new Packet((int)ServerPackets.serverTime))
        {
            packet.Write(time);
            SendTCPDataToAll(packet);
        }
    }

    public static void GameInProgress(int toClient)
    {
        using (Packet packet = new Packet((int)ServerPackets.gameInProgress))
        {
            SendTCPData(toClient, packet);
        }
    }

    public static void GameFull(int toClient)
    {
        using (Packet packet = new Packet((int)ServerPackets.gameFull))
        {
            SendTCPData(toClient, packet);
        }
    }

    public static void PlayerWon(Player player)
    {
        using (Packet packet = new Packet((int)ServerPackets.playerWon))
        {
            packet.Write(player.stats.damage);
            packet.Write(player.stats.kills);
            float hsPercent = 0;

            if (player.stats.shots != 0)
                hsPercent = (float)player.stats.headShots / (float)player.stats.shots;

            packet.Write(hsPercent);
            SendTCPData(player.id, packet);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Match : MonoBehaviour
{
    public static Match instance;

    public MatchStage stage;
    public DateTime warmupTimer;
    public DateTime chooseSpawnTimer;
    public DateTime waitingForPlayersTimer;

    public int serverTime = 0;

    public DateTime lastSvTimeSent;

    public int warmupDuration = 10;
    public int chooseSpawnDuration = 60;
    public int waitingForPlayersDuration = 20;

    public int minPlayers = 5;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        stage = MatchStage.stopped;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsServerEmpty())
        {
            ResetMatch();
            return;
        } 
        else
        {
            if (stage == MatchStage.stopped)
            {
                ResetMatch();
                stage = MatchStage.waitingForPlayers;
                waitingForPlayersTimer = DateTime.Now;
                serverTime = waitingForPlayersDuration;
                lastSvTimeSent = DateTime.Now;
                ServerSend.ServerTime(serverTime);
                ServerSend.MatchStage();
            }
        }

        if ((DateTime.Now - lastSvTimeSent).Seconds >= 1)
        {
            serverTime--;
            lastSvTimeSent = DateTime.Now;

            if(serverTime >= 0)
                ServerSend.ServerTime(serverTime);
        }


        ServerSend.MatchStage();

        switch (stage)
        {
            case MatchStage.waitingForPlayers:
                if (GetPlayerNumbers() >= minPlayers)
                {
                    serverTime = warmupDuration;
                    stage = MatchStage.warmup;
                    warmupTimer = DateTime.Now;
                    ServerSend.MatchStage();
                }
                break;
            case MatchStage.warmup:
                if ((DateTime.Now - warmupTimer).Minutes * 60 + (DateTime.Now - warmupTimer).Seconds > warmupDuration)
                {
                    serverTime = chooseSpawnDuration;
                    stage = MatchStage.chooseSpawn;
                    chooseSpawnTimer = DateTime.Now;
                    ServerSend.MatchStage();
                }
                break;
            case MatchStage.chooseSpawn:
                if ((DateTime.Now - chooseSpawnTimer).Minutes * 60 + (DateTime.Now - chooseSpawnTimer).Seconds > chooseSpawnDuration)
                {
                    stage = MatchStage.match;
                    serverTime = -1;
                    ServerSend.MatchStage();
                    foreach (Client client in Server.clients.Values)
                    {
                        if (client.tcp.socket != null)
                        {
                            client.player.inventory.RemoveAllWeapons();
                            client.player.stats.ResetStats();
                        }
                    }
                }
                break;
            case MatchStage.match:
                if (GetPlayerNumbers() == 1)
                {
                    foreach (Client client in Server.clients.Values)
                    {
                        if (client.tcp.socket != null && client.player.stats.health > 0)
                        {
                            ServerSend.PlayerWon(client.player);
                            client.player.PlayerWon();
                            break;
                        }
                    }
                }
                break;
        }
    }

    bool IsServerEmpty()
    {
        bool empty = true;

        foreach (Client c in Server.clients.Values)
        {
            if (c.tcp.socket != null)
            {
                empty = false;
                break;
            }
        }

        return empty;
    }

    private int GetPlayerNumbers()
    {
        int num = 0;

        foreach (Client client in Server.clients.Values)
        {
            if (client.tcp.socket != null)
                num++;
        }

        return num;
    }

    void ResetMatch()
    {
        stage = MatchStage.stopped;
        ItemSpawner.ResetSpawners();
    }
}

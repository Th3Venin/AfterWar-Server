using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;

    public GameObject playerPrefab;
    public List<GameObject> spawnerTypes;
    public int port;

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

    private void Start()
    {
        Application.targetFrameRate = 50;
        QualitySettings.vSyncCount = 0;
        Server.Start(10, port);
    }

    public Player InstantiatePlayer()
    {
        return Instantiate(playerPrefab, new Vector3(-8.25f, 1f, 0f), Quaternion.identity).GetComponent<Player>();
    }

    public ItemSpawner InstantiateItemSpawner(WeaponTypes type, Vector3 position)
    {
        return Instantiate(spawnerTypes[(int)type], position, Quaternion.identity).GetComponent<ItemSpawner>();
    }
}

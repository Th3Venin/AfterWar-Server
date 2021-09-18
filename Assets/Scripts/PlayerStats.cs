using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float health;
    public float ammo;

    public bool hasArmor;
    public float armor;

    public WeaponTypes equippedWeapon;

    public int deaths;
    public int kills;
    public int score;

    public int headShots;
    public int shots;
    public float damage;

    // Start is called before the first frame update
    void Awake()
    {
        equippedWeapon = WeaponTypes.NoWeapon;
        health = 100f;
        armor = 0;
        Debug.Log("Set player stats");
    }

    public void ResetStats()
    {
        kills = 0;
        headShots = 0;
        damage = 0;
        shots = 0;
        health = 100;
        armor = 0;
        equippedWeapon = WeaponTypes.NoWeapon;
        ServerSend.PlayerHealth(transform.root.GetComponent<Player>());
        ServerSend.PlayerAmmo(transform.root.GetComponent<Player>().id, 0, 0);
        ServerSend.PlayerArmor(transform.root.GetComponent<Player>());
    }
    // Update is called once per frame
    void Update()
    {

    }
}

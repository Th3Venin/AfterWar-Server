using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{
    public float range = 100f;
    public float fireRate = 15f;
    public Transform shootOrigin;

    private float nextTimeToFire = 0f;

    // Start is called before the first frame update
    void Start()
    {
        shootOrigin = Utils.RecursiveFindChild(transform.root, "MainCamera");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (this.GetComponentInParent<Player>().keysPressed[6] && Time.time > nextTimeToFire)
        {
            Debug.Log("SHOOT");
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hit;

        if (Physics.Raycast(shootOrigin.position, shootOrigin.forward, out hit, range))
        {
            Debug.Log("hit " + hit.transform.root.tag);
            if (hit.transform.root.tag == "Player")
            {
                PlayerStats stats = hit.transform.GetComponent<Player>().stats;

                stats.health -= (this.damage - (this.damage * stats.armor / 100));

                if (stats.health <= 0)
                {
                    stats.health = 0;
                }

                Debug.Log("Player health " + stats.health);
                ServerSend.PlayerHealth(hit.transform.GetComponent<Player>());
            }
        }
    }
}

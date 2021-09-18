using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{
    public float range;

    public Transform fpsCam;

    // Start is called before the first frame update
    void Start()
    {
        fpsCam = Utils.RecursiveFindChild(transform.root, "MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot()
    {
        Debug.Log("trying to shoot");
        if (reloading)
            return;

        if (currentMagazine <= 0)
        {
            Reload();
            return;
        }

        currentMagazine--;

        ServerSend.PlayerAmmo(transform.root.GetComponent<Player>().id,currentMagazine, ammo);

        RaycastHit hit;

        Transform rig = Utils.RecursiveFindChild(transform.root, "Rig");
        rig.gameObject.SetActive(false);

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {

            PlayerStats stats = hit.transform.root.GetComponent<PlayerStats>();

            if (hit.collider.transform.tag == "Head") //Headshot
            {
                float initHealth = stats.health;
                stats.health -= (2 * this.damage - (this.damage * stats.armor / 200));
                stats.health = Mathf.Ceil(stats.health);
                stats.armor = stats.armor - (initHealth - stats.health) / 2 <= 0 ? 0 : Mathf.Ceil(stats.armor - (initHealth - stats.health) / 2);

                Debug.Log(hit.collider.transform.tag + ": " + stats.health);
                ServerSend.PlayerHealth(hit.transform.root.GetComponent<Player>());
                ServerSend.PlayerArmor(hit.transform.root.GetComponent<Player>());

                Player attack = fpsCam.root.GetComponent<Player>();
                attack.stats.damage += (initHealth - stats.damage);
                attack.stats.shots++;
                attack.stats.headShots++;

                if (stats.health < 0)
                {
                    hit.transform.root.GetComponent<Player>().PlayerEliminated();
                    Debug.Log("Enemy killed!");
                    attack.stats.kills++;
                }
            }
            else if (hit.collider.transform.tag == "Body") //Bodyshot
            {
                float initHealth = stats.health;
                stats.health -= (this.damage - (this.damage * stats.armor / 200));
                stats.health = Mathf.Ceil(stats.health);
                stats.armor = stats.armor - (initHealth - stats.health) / 2 <= 0 ? 0 : Mathf.Ceil(stats.armor - (initHealth - stats.health) / 2);

                Debug.Log(hit.collider.transform.tag + ": " + stats.health);
                ServerSend.PlayerHealth(hit.transform.root.GetComponent<Player>());
                ServerSend.PlayerArmor(hit.transform.root.GetComponent<Player>());

                Player attack = fpsCam.root.GetComponent<Player>();
                attack.stats.damage += (initHealth - stats.damage);
                attack.stats.shots++;

                if (stats.health < 0)
                {
                    hit.transform.root.GetComponent<Player>().PlayerEliminated();
                    Debug.Log("Enemy killed!");
                    attack.stats.kills++;
                }
            }
            else if (hit.collider.transform.tag == "Leg") //Legshot
            {
                float initHealth = stats.health;
                stats.health -= (0.8f * this.damage - (this.damage * stats.armor / 200));
                stats.health = Mathf.Ceil(stats.health);
                stats.armor = stats.armor - (initHealth - stats.health) / 2 <= 0 ? 0 : Mathf.Ceil(stats.armor - (initHealth - stats.health) / 2);
                Debug.Log(hit.collider.transform.tag + ": " + stats.health);
                ServerSend.PlayerHealth(hit.transform.root.GetComponent<Player>());
                ServerSend.PlayerArmor(hit.transform.root.GetComponent<Player>());

                Player attack = fpsCam.root.GetComponent<Player>();
                attack.stats.damage += (initHealth - stats.damage);
                attack.stats.shots++;

                if (stats.health < 0)
                {
                    hit.transform.root.GetComponent<Player>().PlayerEliminated();
                    Debug.Log("Enemy killed!");
                    attack.stats.kills++;
                }
            }
        }

        rig.gameObject.SetActive(true);
    }


    private void SetCollider(bool state)
    {
        
    }
}

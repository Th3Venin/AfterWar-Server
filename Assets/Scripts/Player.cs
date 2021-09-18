using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;
    public string username;

    public Transform shootOrigin;
    public float spineAngle;

    public PlayerStats stats;
    public Inventory inventory;

    public bool[] keysPressed;
    public bool isGrounded;

    public int interactionRange = 20;

    public void Initialize(int id, string username)
    {
        this.id = id;
        this.username = username;
        keysPressed = new bool[8];
        
    }

    public void FixedUpdate()
    {
        if (stats.health <= 0f)
        {
            return;
        }

        ServerSend.PlayerMovement(this);
        Debug.DrawRay(shootOrigin.position, shootOrigin.forward * 2, Color.red);
    }

    public void SetKeysPressed(bool[] keys)
    {
        for (int i = 0; i < keys.Length; i++)
        {
            keysPressed[i] = keys[i];
        }
    }

    public void EquipWeapon(int slot)
    {
        Inventory inventory = transform.root.GetComponent<Inventory>();
        inventory.SelectWeapon(slot);
    }

    public void Interact()
    {
        RaycastHit hit;

        Transform rig = Utils.RecursiveFindChild(transform.root, "Rig");
        rig.gameObject.SetActive(false);

        if (Physics.Raycast(shootOrigin.position, shootOrigin.forward, out hit, interactionRange))
        {
            Debug.Log("Hit something by interaction");
            Debug.Log(hit.transform.tag);
            if (hit.transform.tag == "ItemSpawner")
            {
                Debug.Log("Interacted with item spawner");
                hit.transform.GetComponent<ItemSpawner>().Collided(this.transform.root.GetComponent<ItemCollector>(), true);
            }
        }

        rig.gameObject.SetActive(true);
    }

    public void PlayerEliminated()
    {
        if (Match.instance.stage != MatchStage.match)
        {
            stats.health = 100;
            ServerSend.PlayerHealth(this);
            return;
        }

        if (inventory.GetActiveWeaponStats() != null)
            inventory.DropWeapon(inventory.GetActiveWeaponStats().type);

        ServerSend.PlayerEliminated(this);
        StartCoroutine(EliminatePlayer());
    }

    public void PlayerWon()
    {
        StartCoroutine(EliminatePlayer());
    }

    public IEnumerator EliminatePlayer()
    {
        yield return new WaitForSeconds(5f);
        Server.clients[id].Disconnect();
    }


    /*public void Shoot(Vector3 viewDirection)
    {
        if (health <= 0f)
        {
            return;
        }

        if (Physics.Raycast(shootOrigin.position, viewDirection, out RaycastHit hit, 25f))
        {
            if (hit.collider.CompareTag("Player"))
            {
                if (hit.collider.GetComponent<Player>().TakeDamage(20f))
                {
                    kills++;
                    score += 20;
                    //ServerSend.PlayerEliminated(id, Server.clients[hit.collider.GetComponent<Player>().id].id);
                }
                else
                {
                    score += 2;
                }

                //ServerSend.PlayerStats(this);
            }
        }
    }*/

    /*public bool TakeDamage(float damage)
    {
        if (health <= 0f)
        {
            return false;
        }

        health -= damage;

        if (health <= 0f)
        {
            health = 0f;
            deaths++;
            return true;
        }

        // ServerSend.PlayerHealth(this);
        return false;
    }

    public void ResetPlayer()
    {
        health = maxHealth;
        kills = 0;
        deaths = 0;
        score = 0;
    }*/
}

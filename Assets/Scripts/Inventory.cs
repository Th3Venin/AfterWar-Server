using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<GameObject> weapons = new List<GameObject>();

    public int selectedSlot = 0;

    private void Awake()
    {
        for (int i = 0; i < 4; i++)
        {
            weapons.Add(null);
        }
    }

    public void DropWeapon(WeaponTypes type)
    {
        Player player = transform.root.GetComponent<Player>();

        ServerSend.UnEquippedWeapon(player.id);

        RemoveCurrentWeapon();

        Vector3 pos = player.transform.position;
        pos += new Vector3(player.shootOrigin.forward.x * 3, player.transform.position.y + 0.5f, player.shootOrigin.forward.z * 3);
        ItemSpawner spawner = NetworkManager.instance.InstantiateItemSpawner(type, pos);
        spawner.dropped = true;
        Debug.Log("Spawning spawner, weapon dropped");
        Debug.Log("ID " + spawner.spawnerId);
        ServerSend.CreateItemSpawner(spawner.spawnerId, pos, spawner.hasItem);
    }

    public void RemoveAllWeapons()
    {
        int savedSlot = selectedSlot;

        for(int i = 0; i < 4; i++)
        {
            selectedSlot = i;
            RemoveCurrentWeapon();
        }

        selectedSlot = savedSlot;

        ServerSend.EquippedWeapon(transform.root.GetComponent<Player>(), WeaponTypes.NoWeapon);
    }

    public GameObject GetActiveWeaponObj()
    {
        return weapons[selectedSlot];
    }

    public void RemoveCurrentWeapon()
    {
        if (weapons[selectedSlot] != null)
            Destroy(weapons[selectedSlot]);
        weapons[selectedSlot] = null;
        SendPlayerWeapons();
    }

    public Weapon GetActiveWeaponStats()
    {
        if (weapons[selectedSlot] != null)
            return weapons[selectedSlot].GetComponent<Weapon>();
        return null;
    }

    public void AddWeaponToSlot(GameObject weapon)
    {
        Destroy(weapons[selectedSlot]);
        weapons[selectedSlot] = weapon;
        SelectWeapon(selectedSlot);
        SendPlayerWeapons();
    }

    public void SelectWeapon(int slot)
    {
        DeactivateWeapons();

        selectedSlot = slot;

        if (GetActiveWeaponStats() == null)
        {
            transform.root.GetComponent<Player>().stats.equippedWeapon = WeaponTypes.NoWeapon;
            ServerSend.EquippedWeapon(transform.root.GetComponent<Player>(), WeaponTypes.NoWeapon);
            return;
        }

        transform.root.GetComponent<Player>().stats.equippedWeapon = GetActiveWeaponStats().type;
        ServerSend.EquippedWeapon(transform.root.GetComponent<Player>(), GetActiveWeaponStats().type);
        Debug.Log("Sent type " + GetActiveWeaponStats().type);
        weapons[selectedSlot].SetActive(true);
    }

    private void DeactivateWeapons()
    {
        foreach (GameObject weapon in weapons)
        {
            if (weapon != null)
            {
                weapon.GetComponent<Weapon>().StopReloading();
                weapon.SetActive(false);
            }
        }
    }

    public void SendPlayerWeapons()
    {
        WeaponTypes[] weaponsTypes = new WeaponTypes[4];
        int i = 0;

        foreach(GameObject weapon in weapons)
        {
            if (weapon != null)
            {
                weaponsTypes[i] = weapon.GetComponent<Weapon>().type;
            }
            else
            {
                weaponsTypes[i] = WeaponTypes.NoWeapon;
            }

            i++;
        }

        ServerSend.PlayerWeapons(transform.GetComponentInParent<Player>().id, weaponsTypes);
    }
}

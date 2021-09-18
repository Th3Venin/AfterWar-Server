using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public static Dictionary<int, ItemSpawner> spawners = new Dictionary<int, ItemSpawner>();
    private static int nextSpawnerId = 1;

    public GameObject itemPrefab;
    public WeaponTypes weaponType;

    public bool dropped = false;

    public int spawnerId;
    public bool hasItem;

    // Start is called before the first frame update
    void Awake()
    {
        hasItem = false;
        spawnerId = nextSpawnerId;
        nextSpawnerId++;
        spawners.Add(spawnerId, this);

        SpawnItem();
    }

    public void Collided(ItemCollector collector, bool interacted = false)
    {
        Debug.Log("Collided with itemspawner");

        if (!hasItem)
        {
            Debug.Log("Spawner Empty");
            return;
        }

        Inventory inventory = collector.transform.root.GetComponent<Inventory>();

        if (weaponType == WeaponTypes.Armour)
        {
            PlayerStats stats = collector.transform.root.GetComponent<PlayerStats>();

            if (stats.armor == 100)
                return;

            if (stats.armor + 50 > 100)
            {
                stats.armor = 100;
            }
            else
            {
                stats.armor += 50;
            }

            ServerSend.PlayerArmor(collector.transform.root.GetComponent<Player>());
            Debug.Log("Player has collected armor!");
            ItemPickedUp(collector.transform.root.GetComponent<Player>().id);
        }

        if (weaponType == WeaponTypes.Ammo)
        {
            Weapon weapon = collector.GetComponentInParent<Player>().inventory.GetActiveWeaponStats();

            if (weapon == null)
            {
                return;
            }

            weapon.ammo += 30;

            ServerSend.PlayerAmmo(collector.transform.root.GetComponent<Player>().id, weapon.currentMagazine, weapon.ammo);
            Debug.Log("Player has collected ammo!");
            ItemPickedUp(collector.transform.root.GetComponent<Player>().id);
        }

        if (weaponType == WeaponTypes.Health)
        {
            PlayerStats stats = collector.GetComponentInParent<Player>().stats;

            if (stats.health == 100)
                return;

            stats.health += 50;

            if (stats.health > 100)
                stats.health = 100;

            ServerSend.PlayerHealth(collector.transform.root.GetComponent<Player>());
            Debug.Log("Player has collected health!");
            ItemPickedUp(collector.transform.root.GetComponent<Player>().id);
        }

        if (inventory.GetActiveWeaponObj() != null && !interacted)
        {
            return;
        }

        if (inventory.GetActiveWeaponObj() != null && interacted)
            inventory.DropWeapon(inventory.GetActiveWeaponStats().type);

        if (weaponType == WeaponTypes.AK47)
        {
            Transform weaponHolder = RecursiveFindChild(collector.transform.root, "WeaponHolder");

            GameObject item = Instantiate(itemPrefab, new Vector3(0.156f, 0.34f, 0.036f), Quaternion.Euler(0, 180, -90));

            item.transform.parent = weaponHolder;
            item.transform.localPosition = new Vector3(0.156f, 0.34f, 0.036f);
            item.transform.localRotation = Quaternion.Euler(0, 180, -90);
            item.transform.GetComponent<Weapon>().enabled = true;

            inventory.AddWeaponToSlot(item);

            Debug.Log("Player has collected a weapon!");
            ItemPickedUp(collector.transform.root.GetComponent<Player>().id);
            Weapon weapon = item.GetComponent<Weapon>();
            ServerSend.PlayerAmmo(collector.transform.root.GetComponent<Player>().id, weapon.currentMagazine, weapon.ammo);
        }

        if (weaponType == WeaponTypes.M4)
        {
            Transform weaponHolder = RecursiveFindChild(collector.transform.root, "WeaponHolder");
            GameObject item = Instantiate(itemPrefab, new Vector3(0, 0, 0), Quaternion.Euler(0, 180, -90));

            item.transform.parent = weaponHolder;
            item.transform.localPosition = new Vector3(0.459f, 0.201f, 0.052f);
            item.transform.localRotation = Quaternion.Euler(0, 180, -90);
            item.transform.GetComponent<Weapon>().enabled = true;

            inventory.AddWeaponToSlot(item);

            Debug.Log("Player has collected a weapon!");
            ItemPickedUp(collector.transform.root.GetComponent<Player>().id);
            Weapon weapon = item.GetComponent<Weapon>();
            ServerSend.PlayerAmmo(collector.transform.root.GetComponent<Player>().id, weapon.currentMagazine, weapon.ammo);
        }

        if (weaponType == WeaponTypes.G28)
        {
            Transform weaponHolder = RecursiveFindChild(collector.transform.root, "WeaponHolder");
            GameObject item = Instantiate(itemPrefab, new Vector3(0, 0, 0), Quaternion.Euler(0, 180, -90));

            item.transform.parent = weaponHolder;
            item.transform.localPosition = new Vector3(0.459f, 0.201f, 0.052f);
            item.transform.localRotation = Quaternion.Euler(0, 180, -90);
            item.transform.GetComponent<Weapon>().enabled = true;

            inventory.AddWeaponToSlot(item);

            Debug.Log("Player has collected a weapon!");
            ItemPickedUp(collector.transform.root.GetComponent<Player>().id);
            Weapon weapon = item.GetComponent<Weapon>();
            ServerSend.PlayerAmmo(collector.transform.root.GetComponent<Player>().id, weapon.currentMagazine, weapon.ammo);
        }

        if (weaponType == WeaponTypes.Revolver)
        {
            Transform weaponHolder = RecursiveFindChild(collector.transform.root, "WeaponHolder");
            GameObject item = Instantiate(itemPrefab, new Vector3(0, 0, 0), Quaternion.Euler(0, 180, -90));

            item.transform.parent = weaponHolder;
            item.transform.localPosition = new Vector3(0.459f, 0.201f, 0.052f);
            item.transform.localRotation = Quaternion.Euler(0, 180, -90);
            item.transform.GetComponent<Weapon>().enabled = true;

            inventory.AddWeaponToSlot(item);

            Debug.Log("Player has collected a weapon!");
            ItemPickedUp(collector.transform.root.GetComponent<Player>().id);
            Weapon weapon = item.transform.GetComponent<Weapon>();
            ServerSend.PlayerAmmo(collector.transform.root.GetComponent<Player>().id, weapon.currentMagazine, weapon.ammo);
        }

        /*if (!collector.stats.hasWeapon && itemPrefab.tag == "Weapon" && hasItem)
        {
            Transform righthand = RecursiveFindChild(collector.transform.root, "RightHand");

            item.transform.parent = righthand;
            item.transform.localPosition = new Vector3(0.156f, 0.34f, 0.036f);
            item.transform.localRotation = Quaternion.Euler(0, 180, -90);
            item.transform.GetComponent<Weapon>().enabled = true;
            collector.stats.hasWeapon = true;

            Debug.Log("Player has collected a weapon!");
            Player player = collector.transform.root.GetComponent<Player>();

            player.inventory.AddWeapon(item.GetComponent<Weapon>());
            //player.inventory.lastSlotFilled = (player.inventory.lastSlotFilled + 1) % 4;

            ItemPickedUp(collector.transform.root.GetComponent<Player>().id);
        }*/
    }

    Transform RecursiveFindChild(Transform parent, string tag)
    {
        foreach (Transform child in parent)
        {
            if (child.tag == tag)
            {
                return child;
            }
            else
            {
                Transform found = RecursiveFindChild(child, tag);
                if (found != null)
                {
                    return found;
                }
            }
        }
        return null;
    }

    
    private void SpawnItem()
    {
        GameObject item = Instantiate(itemPrefab, new Vector3(0.156f, 0.34f, 0.036f), Quaternion.Euler(0, 180, -90));

        item.transform.parent = gameObject.transform;
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.Euler(0, 0, 0);
        hasItem = true;
    }

    private void ItemPickedUp(int byPlayer)
    {
        hasItem = false;
        ServerSend.ItemPickedUp(spawnerId, byPlayer);
    }

    public static void ResetSpawners()
    {
        List<int> idRemove = new List<int>();

        foreach (ItemSpawner spawner in spawners.Values)
        {
            if (spawner.dropped)
            {
                idRemove.Add(spawner.spawnerId);
                continue;
            }

            spawner.hasItem = true;
            ServerSend.ItemSpawned(spawner.spawnerId);
        }

        foreach(int id in idRemove)
        {
            Destroy(ItemSpawner.spawners[id].gameObject);
            ItemSpawner.spawners.Remove(id);
        }
    }
}

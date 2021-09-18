using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    public PlayerStats stats;

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ItemSpawner")
        {
            Debug.Log("Collided with item spawner");
            other.GetComponent<ItemSpawner>().Collided(this);
        }
    }
}

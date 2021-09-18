using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage;
    public WeaponTypes type;
    public int ammo;
    public int magazineSize;
    public int currentMagazine;
    public bool reloading = false;

    public Animator animator;

    private void Awake()
    {
        ammo = 0;
        currentMagazine = 0;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }


    public void StopReloading()
    {
        reloading = false;
    }


    public void Reload()
    {
        if (currentMagazine == magazineSize || ammo == 0)
        {
            reloading = false;
            return;
        }

        if (!reloading)
            reloading = true;
        else
            return;

        StartCoroutine(WaitReload());
    }

    public IEnumerator WaitReload()
    {
        yield return new WaitForSeconds(2f);

        int requiredBullets = magazineSize - currentMagazine;

        if (ammo - requiredBullets >= 0)
        {
            currentMagazine += requiredBullets;
            ammo -= requiredBullets;
        }
        else
        {
            currentMagazine += ammo;
            ammo = 0;
        }

        ServerSend.PlayerAmmo(transform.root.GetComponent<Player>().id, currentMagazine, ammo);

        reloading = false;
    }

}

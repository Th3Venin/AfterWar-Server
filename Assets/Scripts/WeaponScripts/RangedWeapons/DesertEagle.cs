using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertEagle : SingleShotWeapon
{
    private bool shot = false;

    // Start is called before the first frame update
    void Awake()
    {
        this.damage = 15f;
        this.range = 100f;
        this.magazineSize = 7;
        this.ammo = 30;
        type = WeaponTypes.Revolver;

        fpsCam = Utils.RecursiveFindChild(transform.root, "MainCamera");
        animator = transform.root.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.GetComponentInParent<Player>().keysPressed[6] && !shot)
        {
            Shoot();
            shot = true;
        }

        if (this.GetComponentInParent<Player>().keysPressed[6] == false)
            shot = false;

        if (currentMagazine == 0 && ammo != 0)
        {
            Debug.Log("Reloading automatically");
            Reload();
        }

    }
}

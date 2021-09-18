using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticWeapon : RangedWeapon
{
    public float fireRate;
    public float nextTimeToFire;

    // Start is called before the first frame update
    void Start()
    {
        fpsCam = Utils.RecursiveFindChild(transform.root, "MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
    
    }
}

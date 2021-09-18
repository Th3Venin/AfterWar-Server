using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShotWeapon : RangedWeapon
{
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

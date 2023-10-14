using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{
    // Start is called before the first frame update
    void Start()
    {
        maxCooldown = 20;
        gunName = "rifle1";
        maxMagazine = 30;
        magazine = 30;
        maxRelodeTime = 100;
        reloadTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Shot(Vector3 pos, float rotate_y)
    {
        Instantiate(bullet, pos, Quaternion.Euler(0, rotate_y, 0));
        Debug.Log("rifle shoted!");
        cooldown = maxCooldown;
    }

}

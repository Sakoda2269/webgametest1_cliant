using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public override void init()
    {
        maxCooldown = 50;
        gunName = "rifle1";
        maxMagazine = 30;
        magazine = 30;
        maxRelodeTime = 1000;
        reloadTime = 0;
    }

    public override void GunShotType(Vector3 pos, float rotate_y)
    {
        if(CanShot()){
            cooldown = maxCooldown;
            Instantiate(bullet, pos, Quaternion.Euler(0, rotate_y, 0));
            magazine--;
            Debug.Log(cooldown);
        }
    }

}

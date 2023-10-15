using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Rifle : Weapon
{
    // Start is called before the first frame update
    // Update is called once per frame

    public override void init()
    {
        maxCooldown = 10;
        gunName = "rifle1";
        maxMagazine = 30;
        magazine = 30;
        maxRelodeTime = 500;
        reloadTime = 0;
        recoil = 2;
    }

    public override void enemyShot(Vector3 pos, float rotate_y, GameObject shoter){
        GameObject shoted_bullet = Instantiate(bullet, pos, Quaternion.Euler(0, rotate_y, 0));
        shoted_bullet.GetComponent<Bullet>().shoter = shoter;
    }

}

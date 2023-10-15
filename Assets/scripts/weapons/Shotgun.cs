using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    // Start is called before the first frame update
    public override void init()
    {
        maxCooldown = 50;
        gunName = "shotgun";
        maxMagazine = 7;
        magazine = 7;
        maxRelodeTime = 500;
        reloadTime = 0;
        recoil = 8;
    }

    public override void enemyShot(Vector3 pos, float rotate_y, GameObject shoter){
        for(int i = 0; i < 30; i++)
        {
            GameObject shoted_bullet = Instantiate(bullet, pos, Quaternion.Euler(0, rotate_y + Random.Range(-6.0f, 6.0f), 0));
            shoted_bullet.GetComponent<Bullet>().shoter = shoter;
        }
        
    }
}

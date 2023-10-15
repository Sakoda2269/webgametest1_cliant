using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public int maxCooldown;
    public int cooldown = 0;
    public int damage = 1;
    public GameObject bullet;
    public string gunName;
    public int maxMagazine;
    public int magazine;
    public int maxRelodeTime;
    public int reloadTime;
    public bool reloading;
    protected float recoil;
    public bool ADS = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
    public abstract void init();
    public void MainUpdate(){
        if(cooldown > 0){
            cooldown--;
        }
        if(reloadTime > 0){
            reloadTime--;
        }
        else if(reloading)
        {
            reloading = false;
            magazine = maxMagazine;
        }
    }
    public float Shot(float rotate_y){
        if(CanShot())
        {
            cooldown = maxCooldown;
            magazine--;
            if(ADS)
            {
                 return Random.Range(-0.2f, 0.2f) + rotate_y;
            }
            return Random.Range(-recoil, recoil) + rotate_y;
        }
        return 0;
    }
    public abstract void enemyShot(Vector3 pos, float rotate_y, GameObject shoter);

    public bool CanShot(){
        if(magazine <= 0 && !reloading)
        {
            reloading = true;
            reloadTime = maxRelodeTime;
            return false;
        }
        return cooldown <= 0 && !reloading;
    }
        
}

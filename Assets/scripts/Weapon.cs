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
            Debug.Log(reloadTime);
        }
        else if(reloading)
        {
            reloading = false;
            magazine = maxMagazine;
        }
    }
    public void Shot(Vector3 pos, float rotate_y){
        if(CanShot())
        {
            GunShotType(pos, rotate_y);
        }
    }
    public abstract void GunShotType(Vector3 pos, float rotate_y);

    public bool CanShot(){
        if(magazine <= 0)
        {
            reloading = true;
            reloadTime = maxRelodeTime;
            return false;
        }
        return cooldown <= 0;
    }
        
}

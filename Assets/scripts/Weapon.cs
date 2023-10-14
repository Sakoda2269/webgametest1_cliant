using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{

    public int cooldown = 20;
    public int damage = 1;
    public GameObject bullet;
    public string gunName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public abstract void Shot(Vector3 pos, float rotate_y);
        
}

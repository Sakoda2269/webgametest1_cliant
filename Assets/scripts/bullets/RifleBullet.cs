using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleBullet : Bullet
{
    // Start is called before the first frame update

    // Update is called once per frame

    protected override void init(){
        speed = 60.0f;
        damage = 5;
    }
}

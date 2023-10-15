using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBullet : Bullet
{
    // Start is called before the first frame update
    protected override void init(){
        speed = 120.0f;
        damage = 2;
        maxDeadTime = 500;
    }
}

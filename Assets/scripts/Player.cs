using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System;
using Newtonsoft.Json;

public class Player : MonoBehaviour
{

    public float _speed = 10f;
    public float sensitiveRotate = 3.0f;
    public bool dead;
    public int maxHealth = 100;
    public int health = 100;
    Rigidbody rb;
    public GameObject weapon;
    
    // Start is called before the first frame update
    void Start()
    {
        dead = false;
        health = maxHealth;
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
        Transform mytrans = this.transform;
        float rotateX = Input.GetAxis("Mouse X") * sensitiveRotate;
        float rotateY = Input.GetAxis("Mouse Y") * sensitiveRotate;
        mytrans.Rotate(0.0f, rotateX, 0.0f);
    }

    void FixedUpdate() {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            var velocity = Vector3.zero;
            if (Input.GetKey(KeyCode.W))
            {
                velocity.z = _speed;
            }
            if (Input.GetKey(KeyCode.A))
            {
                velocity.x = -_speed;
            }
            if (Input.GetKey(KeyCode.S))
            {
                velocity.z = -_speed;
            }
            if (Input.GetKey(KeyCode.D))
            {
                velocity.x = _speed;
            }
            if (velocity.x != 0 || velocity.z != 0)
            {
                // transform.position += 
                rb.velocity = transform.rotation * velocity;
            }
        }
        else
        {
            rb.velocity = new Vector3(0, 0, 0);
        }
    }

    public void Damage(int damage){
        health -= damage;
        if(health <= 0){
            dead = true;
        }
    }

    void OnDestroy() {
    }
}

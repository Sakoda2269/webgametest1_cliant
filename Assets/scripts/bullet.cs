using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Bullet : MonoBehaviour
{
    protected Vector3 forward;
    protected Rigidbody rb;
    public float speed = 30.0f;
    public GameObject shoter;
    public int damage = 5;
    protected int deadTime = 0;
    protected int maxDeadTime = 500;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        forward = this.transform.forward;
        init();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = forward * speed;
        deadTime++;
        if(deadTime > maxDeadTime)
        {
            Destroy(this.gameObject);
        }
    }

    protected abstract void init();

    void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "wall")
        {
            Destroy(this.gameObject);
        }
        if(other.gameObject.tag == "Player")
        {
            if(other.gameObject != shoter)
            {
                // other.gameObject.GetComponent<ball>().dead = true;
                other.gameObject.GetComponent<Player>().Damage(damage);
                Destroy(this.gameObject);
            }
            
        }
    }
}

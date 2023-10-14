using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class bullet : MonoBehaviour
{
    private Vector3 forward;
    private Rigidbody rb;
    public float speed = 10.0f;
    public GameObject shoter;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        forward = this.transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = forward * speed;
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "wall")
        {
            Destroy(this.gameObject);
        }
        if(other.gameObject.tag == "Player")
        {
            if(other.gameObject != shoter)
            {
                other.gameObject.GetComponent<ball>().dead = true;
                Destroy(this.gameObject);
            }
            
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicTrail : MonoBehaviour
{
    Rigidbody rigibo;
    // Start is called before the first frame update
    public float thrust;

    private float count;
    void Start()
    {
        rigibo = GetComponent<Rigidbody>();
        rigibo.AddForce(transform.forward * thrust);
        count = 0;
    }


    private void Update()
    {
        count++;

        Destroy(this.gameObject, 5f);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (count > 1) Destroy(this.gameObject);
    }
}

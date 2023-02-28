using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAssist : MonoBehaviour
{
    // Start is called before the first frame update

    private Vector3 targetpos;
    private bool inrange;

    void Start()
    {
        inrange = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
    /*public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            targetpos = other.transform.position;
            inrange = true;
        }
    }*/
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            targetpos = other.transform.position;
            inrange = true;
        }
    }

    public bool InRangeTeller() { return inrange; }
    public void InrangeOff() { inrange = false; }
    public Vector3 TargetPosTeller() { return targetpos; }
}

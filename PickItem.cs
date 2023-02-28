using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickItem : MonoBehaviour
{
    private GameObject Player;
    PlayerStatus plaste;

    public string itemname;
    public int itemvalue;

    EffectGenerator efegen;

    Rigidbody rb;
    Collider coll;
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        plaste = Player.GetComponent<PlayerStatus>();
        efegen = GameObject.Find("SCORE DIRECTOR").GetComponent<EffectGenerator>();
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
        //coll.isTrigger = false;
        rb.AddForce(transform.up*5,ForceMode.Impulse);
    }
    private void OnTriggerEnter(Collider other)
    {
        coll.isTrigger = true;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
    private void OnTriggerStay(Collider col)
    {
        coll.isTrigger = true;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if (col.gameObject.tag == "Player")//接地判定の方に無視するヤツ
        {
            SoundManager.instance.PlaySE(SoundManager.SE_Type.Pick);
            plaste.SetItem(itemname,"add",itemvalue);
            efegen.GeneratePickFlash(this.gameObject.transform.position);
            Destroy(this.gameObject);
        }
    }
}

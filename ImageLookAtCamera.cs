using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageLookAtCamera : MonoBehaviour
{

    SpriteRenderer itemimage;
    public GameObject cam;

    void Start()
    {
        //itemimage = this.GetComponent<SpriteRenderer>();
        cam = GameObject.Find("Pivot").transform.Find("Campos").transform.Find("Main Camera").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //this.transform.LookAt(cam.transform);
        OtherBillBorad();
    }


    void OtherBillBorad()
    {
        Vector3 p = Camera.main.transform.position;
        transform.LookAt(p);
    }
}

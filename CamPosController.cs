using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CamPosController : MonoBehaviour
{
 
 

    public GameObject campos;
    void Start()
    {
        ChildizeCamera();
    }

    // void FixedUpdate()
    //{
    // if (Input.GetKeyDown(KeyCode.T)/* && cams == false*/) //ChildizeCamera();
    //if (Input.GetKeyDown(KeyCode.Y)/* && cams == true*/) DeChildCamera();
    //}

    public void ChildizeCamera()
    {
        this.gameObject.transform.parent = GameObject.Find("Campos").transform;
        this.transform.position = campos.transform.position;
        this.transform.rotation = campos.transform.rotation;
    }

    public void DeChildCamera()
    {
        this.gameObject.transform.parent = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{

    //Rotations
    private Vector3 currentRotation;
    private Vector3 targetRotation;

    //Hipfire Recoil
    [SerializeField] private float recoilX;
    [SerializeField] private float recoilY;
    [SerializeField] private float recoilZ;

    //settings
    [SerializeField] private float snappiness;
    [SerializeField] private float returnSpeed;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation,Vector3.zero,returnSpeed*Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation,targetRotation,snappiness*Time.deltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    public void RecoilFire()
    {
        targetRotation += new Vector3(recoilX,Random.Range(-recoilY,recoilY),Random.Range(-recoilZ,recoilZ));
    }

    public void UpRecoilParam()
    {
        snappiness = 5;
        returnSpeed = 8;
    }

    public void ResetRecoilParam() {
        snappiness = 8;
        returnSpeed = 4;
    }
}

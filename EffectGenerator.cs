using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectGenerator : MonoBehaviour
{

    public GameObject DeadEffect;

    public GameObject flesh;

    public GameObject[] deadeffect = new GameObject[5];
    ParticleSystem[] deadparticle = new ParticleSystem[5];
    private int deadparticlenum;

    public GameObject[] damageobj = new GameObject[5];
    ParticleSystem[] damageeffects = new ParticleSystem[5];
    private int damparnum;

    public GameObject[] spawnobj = new GameObject[5];
    ParticleSystem[] spawneffects = new ParticleSystem[5];
    private int spawnnum;

    public GameObject[] pickobj = new GameObject[5];
    ParticleSystem[] pickeffects = new ParticleSystem[5];
    private int pickefnum;

    void Start()
    {
        for (int i = 0; deadparticle.Length > i; i++)
        {
            deadparticle[i] = deadeffect[i].GetComponent<ParticleSystem>();
        }

        for (int i = 0; damageeffects.Length > i; i++)
        {
            damageeffects[i] = damageobj[i].GetComponent<ParticleSystem>();
        }
        for (int i = 0; spawneffects.Length > i; i++)
        {
            spawneffects[i] = spawnobj[i].GetComponent<ParticleSystem>();
        }
        for (int i = 0; pickeffects.Length > i; i++)
        {
            pickeffects[i] = pickobj[i].GetComponent<ParticleSystem>();
        }

        damparnum = 0;
        deadparticlenum = 0;
        spawnnum = 0;
        pickefnum = 0;
    }
    public void GenerateDeadSmoke(Vector3 deadpos)
    {
        // GenerateDeadSmokeTypeI(deadpos);
        GenerateDeadSmokeTypeII(deadpos);
    }
    public void GenerateDeadSmokeTypeI(Vector3 deadpos)
    {
        Instantiate(DeadEffect, deadpos, Quaternion.identity);
        Destroy(DeadEffect, 5f);
        Instantiate(flesh, deadpos, Quaternion.identity);
    }
    public void GenerateDeadSmokeTypeII(Vector3 deadpos)
    {
        if (deadparticlenum >= deadparticle.Length) { deadparticlenum = 0; }
        deadparticle[deadparticlenum].transform.position = deadpos;
        deadparticle[deadparticlenum].Play();
        deadparticlenum++;
        Instantiate(flesh, deadpos, Quaternion.identity);
    }


    public void GeneratePickFlash(Vector3 pickpos)
    {
        if (pickefnum >= pickeffects.Length) { pickefnum = 0; }
        pickeffects[pickefnum].transform.position = pickpos;
        pickeffects[pickefnum].Play();
        pickefnum++;
    }

    public void GenerateDamageEffect(Vector3 dampos)
    {
        if (damparnum >= damageeffects.Length) { damparnum = 0; }
        damageeffects[damparnum].transform.position = dampos;
        damageeffects[damparnum].Play();
        damparnum++;
    }

    public void GenerateSpawnEffect(Vector3 spawnpos)
    {
        if (spawnnum >= spawneffects.Length) { spawnnum = 0; }
        spawneffects[spawnnum].transform.position = spawnpos;
        spawneffects[spawnnum].Play();
        spawnnum++;
    }
}

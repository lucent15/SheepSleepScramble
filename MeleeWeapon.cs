using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    Collider attacker;
    PlayerController placon;
    ScoreDirector scored;
    PlayerStatus plaste;
    ShakeObjectsByDOTween shaker;
    void Start()
    {
        attacker = GetComponent<CapsuleCollider>();
        //attacker.enabled = false;
        placon = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        scored = GameObject.Find("SCORE DIRECTOR").GetComponent<ScoreDirector>();
        shaker= GameObject.Find("SCORE DIRECTOR").GetComponent<ShakeObjectsByDOTween>();
        plaste = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();


    }

    private void Update()
    {
        if (placon.Getmeleecolswitch()) { attacker.enabled = true; } else { attacker.enabled = false; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<MakaiHitsujiStatus>().Damage();
            plaste.SetMP("add",10);
            shaker.Shake(0);
            scored.ChangeScoreSubType("melee");
            shaker.Shake(3);
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;


public class MakaiHitsujiStatus : MonoBehaviour
{

    public EnemyStatus eneste;

    public float enemylife;
    public float maxlife;

    private ScoreDirector scored;

    AreaScript aresc;

    WaveController wavcon;

    //PlayerStatus pleste;

    EffectGenerator effegene;

    EnemyMovement enemov;

    GameObject thedirector;

    NavMeshAgent agent;

   

    void Start()
    {
        thedirector = GameObject.Find("SCORE DIRECTOR");
        scored = thedirector.GetComponent<ScoreDirector>();
        effegene = thedirector.GetComponent<EffectGenerator>();
        wavcon = thedirector.GetComponent<WaveController>();
        enemov = GetComponent<EnemyMovement>();

        agent = GetComponent<NavMeshAgent>();


        enemylife = eneste.enemylife;
        maxlife = eneste.enemylife;
        agent.speed = eneste.movespeed;

        effegene.GenerateSpawnEffect(this.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemylife <= 0)
        {
            if (aresc != null) aresc.SubtraInEnemyCount();
            scored.AddKillCount();
            scored.SubStayEnemy();
            scored.ShowEnemyHP(false);
            wavcon.AddKilledEnemy();
            effegene.GenerateDeadSmoke(this.transform.position);
            SoundManager.instance.PlaySE(SoundManager.SE_Type.DeadEnemy);
            Destroy(this.gameObject);
        }

        if (wavcon.GetFinish())
        {
            Destroy(this.gameObject);
        }

    }

    public void Damage()
    {
        SoundManager.instance.PlaySE(SoundManager.SE_Type.DamageEnemy);
        scored.ShowEnemyHP(true);
        scored.ReadyEnemyHPBar(eneste.enemyname, maxlife, enemylife);
        enemylife -= 30;
        scored.EnemyHPinBar(eneste.enemyname, maxlife, enemylife);
        //スコアディレクターにヒット数を加算
        scored.AddHitCount();
        enemov.KnockBack();
        effegene.GenerateDamageEffect(this.transform.position);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerStatus>().Damage(eneste.attackpow);
            //Debug.Log("マカイヒツジの攻撃");
        }

        if (other.gameObject.tag == "Area")
        {
            aresc = other.gameObject.GetComponent<AreaScript>();

        }
    }
}

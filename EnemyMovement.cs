using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class EnemyMovement : MonoBehaviour
{
    // Start is called before the first frame update

    Transform point_a;
    Transform point_b;
    Transform point_c;
    Transform pointt;

    NavMeshAgent agent;

    Vector3 playerPos;  //プレイヤーの位置
    private GameObject player;   //プレイヤーぶち込む枠
    float distance; //プレイヤーとの距離

    EnemyCommander enecom;

    EnemyStatus eneste;

    Rigidbody rb;

    int dice;
    void Start()
    {
        point_a = GameObject.Find("PointA").transform;
        point_b = GameObject.Find("PointB").transform;
        point_c = GameObject.Find("PointC").transform;
        pointt = GameObject.Find("TestPos").transform;

        agent = GetComponent<NavMeshAgent>();

        player = GameObject.Find("Player");

        enecom = GameObject.Find("SCORE DIRECTOR").GetComponent<EnemyCommander>();

        var firsttargetspos = ThrowDice();

        if (firsttargetspos <= 2)
        {
            GOPointA();
        }
        else if (firsttargetspos >= 3 && firsttargetspos <= 4)
        {
            GOPointB();
        }
        else if (firsttargetspos >= 5)
        {
            GOPointC();
        }

        rb = GetComponent<Rigidbody>();

    }

    public void GOPointA()
    {
        agent.destination = point_a.position;
    }
    public void GOPointB()
    {
        agent.destination = point_b.position;
    }
    public void GOPointC()
    {
        agent.destination = point_c.position;
    }

    public void GoPointT()
    {
        agent.destination = pointt.position;
    }

    public void GoPointP()
    {
        agent.destination = playerPos;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GoPointP();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GOPointB();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GOPointC();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            agent.enabled = false;
        }
       

        if (enecom.NowTargetPosition()!=null)
        {
            agent.destination = enecom.NowTargetPosition().position;
        }

        //agent.destination = player.transform.position;
    }

    public int ThrowDice()
    {
        // 乱数の範囲指定で配列のインデックスを取得する
        return Random.Range(1, 6);
    }

    public void KnockBack()
    {
        StartCoroutine(KnockBacking());
    }

    public IEnumerator KnockBacking()
    {
        agent.isStopped = true;
        
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Vector3 knockbackdir = (transform.position - player.transform.position).normalized;
        //rb.AddForce(knockbackdir*10,ForceMode.Impulse);
        if (ThrowDice()<=3) {
            rb.AddForceAtPosition(knockbackdir * 5, transform.right * 1000, ForceMode.Impulse);
        }
        else
        {
            rb.AddForceAtPosition(knockbackdir * 5, -transform.right * 1000, ForceMode.Impulse);
        }
       // rb.AddForceAtPosition(knockbackdir*5,transform.right*100,ForceMode.Impulse);
        
        yield return new WaitForSeconds(1.0f);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        agent.isStopped = false;

    }
}


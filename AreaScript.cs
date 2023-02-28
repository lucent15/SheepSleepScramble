using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaScript : MonoBehaviour
{


    public float areastate = 0;
    public float maxareastate = 100;
    public float minareastate = -100;
    public float inenemycount;
    public bool areacountenemy;
    public bool areacountally;

    public float suppliedmutton;
    public float decreasemuttontime;
    public float subtratime;
    //public Collider area;

    public Image areaicon;
    public Image areagauge;

    PlayerStatus plaste;

    Text areamuttonnumview;


    int secount;
    void Start()
    {
        areastate = 50;
        plaste = GameObject.Find("Player").GetComponent<PlayerStatus>();
        decreasemuttontime = 15;
        areamuttonnumview = GameObject.Find("AreaUI").GetComponent<Text>();
        areamuttonnumview.enabled = false;
    }

    void Update()
    {
        areastate = Mathf.Clamp(areastate, minareastate, maxareastate);

        inenemycount = Mathf.Clamp(inenemycount, 0, 30);

        if (areacountenemy)
        {
            areastate -= (Time.deltaTime * inenemycount);
        }

        if (areacountally) { areastate += (Time.deltaTime * 4); }
        if (suppliedmutton > 0) { areastate += (Time.deltaTime * suppliedmutton); }

        if (areastate > 0)
        {
            areagauge.fillClockwise = true;
            areagauge.fillAmount = areastate / 100;
            areagauge.color = new Color(0, 0.6f, 1, 1);
            if (areastate >= 100) { areaicon.color = new Color(0, 0.5f, 1, 1); } else { areaicon.color = new Color(0.5f, 0.7f, 1, 1); }

        }

        if (areastate < 0)
        {
            areagauge.fillClockwise = false;
            areagauge.fillAmount = Mathf.Abs(areastate) / 100;
            areagauge.color = new Color(1, 0.4f, 0.4f, 1);
            plaste.SetTakedArea(true);
            if (areastate <= -100) { areaicon.color = new Color(1, 0, 0, 1); } else { areaicon.color = new Color(1, 0.5f, 0.5f, 1); }

        }

        if (suppliedmutton > 0)
        {
            subtratime += Time.deltaTime;
            if (subtratime >= decreasemuttontime) { suppliedmutton--;secount--; subtratime = 0; }
        }

        if (secount>=10)
        {
            SoundManager.instance.PlaySE(SoundManager.SE_Type.Cheers);
            secount = 0;
            Debug.Log("リセット");
        }
    }
    public int GetSeCount() { return secount; }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy") { inenemycount++; }//敵が入った数をカウント
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy") { inenemycount--; areacountenemy = false; }//敵が出た数をカウント
        if (other.gameObject.tag == "Player") { areacountally = false; areamuttonnumview.enabled = false; }
    }

    public void SubtraInEnemyCount()
    {
        inenemycount--;
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy") { areacountenemy = true; }// else { inenemycount = 0; }
        if (other.gameObject.tag == "Player")
        {
            areacountally = true;
            areamuttonnumview.enabled = true;
            areamuttonnumview.text = "生肉在庫："+suppliedmutton+"\nFで肉提供";
            if (Input.GetKeyDown(KeyCode.F) && plaste.GetRawMutton() > 0)
            {
                plaste.SetItem("rawmutton", "sub", 1);
                plaste.CountGiveAreaMutton();
                suppliedmutton++;
                secount++;
                SoundManager.instance.PlaySE(SoundManager.SE_Type.Give);
            }
        }
    }

    public float GetAreaState() { return areastate; }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrillController : MonoBehaviour
{

    PlayerStatus plaste;

    public string grillstate;//グリルの状態。wait,cooking

    public int maximumreservenum;//先行投入できる数。料理してcoockedmutonを出す度に減る

    public float burntime;

    public int reservemutton;//焼き待ち肉。これがある限りグリルは燃え続ける

    public GameObject itemcookedmutton;//やいたにく格納

    public Transform[] ejectpos = new Transform[5];//肉投下ぽっじション
    public int ejectnum;

    GameObject grilltop;
    GameObject smoke;

    Text stateview;

    void Start()
    {
        plaste = GameObject.Find("Player").GetComponent<PlayerStatus>();
        grillstate = "wait";
        smoke = transform.Find("SmokeColumn").gameObject;

        grilltop = transform.Find("top").gameObject;
        stateview = GameObject.Find("GrillStateView").GetComponent<Text>();
        stateview.enabled = false;
    }
    void Update()
    {

        if (reservemutton > 0 && grillstate == "wait")
        {
            StartCoroutine(MakingBakedMutton());
        }

        if (grillstate == "cooking")
        {
            float x = -90;
            grilltop.transform.rotation = Quaternion.Euler(x, 0.0f, 0.0f);
            smoke.SetActive(true);
        }

        if (grillstate == "wait")
        {
            grilltop.transform.rotation = Quaternion.Euler(0, 0, 0);
            smoke.SetActive(false);

        }
      

    }

    IEnumerator MakingBakedMutton()
    {
        grillstate = "cooking";
        yield return new WaitForSeconds(burntime);

        reservemutton--;
        Vector3 gripos = transform.position;
        Vector3 ejepos = ejectpos[ejectnum].transform.position;
        Instantiate(itemcookedmutton, ejepos, Quaternion.identity);
        ejectnum++;
        grillstate = "wait";
        if (ejectnum >= ejectpos.Length) { ejectnum = 0; }
    }

    private void OnTriggerStay(Collider col)
    {
        //キーが表示
        if (col.gameObject.tag == "Player")//接地判定の方に無視するヤツ
        {
            //ボタンを押す&&生肉を持っている&&投入数が最大じゃない
            //コルーチンスタート
            stateview.enabled = true;
            stateview.text = reservemutton + "/" + maximumreservenum+"\nFで肉投入";

            if (Input.GetKeyDown(KeyCode.F) && plaste.GetRawMutton() > 0 && maximumreservenum > reservemutton)
            {
                plaste.SetItem("rawmutton", "sub", 1);
                reservemutton++;
                plaste.CountGiveMutton();
                SoundManager.instance.PlaySE(SoundManager.SE_Type.Give);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            stateview.enabled = false;
        }
    }
}

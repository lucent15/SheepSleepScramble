using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{

    public float life;      //プレイヤーのライフ。
    public float sleepiness;//眠気。いる？
    public float magicpow;  //魔力
   // public int cresit;      //金。肉を与えるともらえる
    public int cookedmutton;//料理済みの羊肉
    public int rawmutton;   //生の羊肉

    public float meleeatk;  //近接攻撃力
    public float shotatk;   //魔法攻撃力

    public Text hpvalue;
    public Text magpowvalue;

    public Image hpbar;
    public Image hpbardelay;
    public Image mpbar;

    //public Text cresitvalue;
    public Text rawmuttonvalue;
    public Text cookedmuttonvalue;

    PlayerController placon;

    CapsuleCollider capcol;
    private bool deadbringer;

    EnemyCommander enecom;

    private float delayhpcam;

    private bool healing;

    float damagecount;
    float healcount;
    int givemutton;
    bool taked;
    int giveareamutton;

    void Start()
    {
        life = 100;
        magicpow = 100;
        sleepiness = 0;
        cookedmutton = 0;
        rawmutton = 0;

        placon = GetComponent<PlayerController>();
        enecom = GameObject.Find("SCORE DIRECTOR").GetComponent<EnemyCommander>();
        capcol = GetComponent<CapsuleCollider>();
    }


    void Update()
    {

        ShowUI();

        life = Mathf.Clamp(life, 0, 100);
        magicpow = Mathf.Clamp(magicpow, 0, 100);

        if (life < 100 && cookedmutton > 0 && Input.GetKeyDown(KeyCode.R))
        {
            cookedmutton--;
            healcount++;
            //life += 25;
            StartCoroutine(Healing());
            hpbardelay.fillAmount = ((life + 25) / 100);
        }

        if (life == 0 && !deadbringer)
        {
            placon.DeadState();
            StartCoroutine(RevivingProcess());
            deadbringer = true;
        }
    }
    public float GetHealCount() { return healcount; }
    public float GetDamageCount() { return damagecount; }

    IEnumerator Healing()
    {
        var totalheal = 0;
        SoundManager.instance.PlaySE(SoundManager.SE_Type.EatHeal);
        while (totalheal <= 25)
        {
            yield return null;
            life++;
            totalheal++;
        }
    }
    IEnumerator RevivingProcess()
    {
        yield return new WaitForSeconds(3);
        enecom.SetTotalLife(10, "sub");
        placon.Reviving();
        life = 100;
        magicpow = 100;
        deadbringer = false;
    }
    

    public void Damage(float damagevalue)
    {
        life -= damagevalue;
        StartCoroutine(DelayHP());
        damagecount++;
        SoundManager.instance.PlaySE(SoundManager.SE_Type.Damage);
    }

    IEnumerator DelayHP()//メインとディレイで差がある時に徐々に減らす処理
    {
        yield return new WaitForSeconds(0.5f);
        var delaysum = new WaitForSeconds(0.1f);
        while (hpbar.fillAmount <= hpbardelay.fillAmount)
        {
            delayhpcam -= 2f;
            hpbardelay.fillAmount = delayhpcam / 100;
            yield return null;
        }

        if (hpbar.fillAmount >= hpbardelay.fillAmount)
        {
            hpbardelay.fillAmount = hpbar.fillAmount;
            StopCoroutine(DelayHP());
        }
    }

    public void SetLife(string type, float value)
    {
        if (type == "add") { life += value; }
        if (type == "sub") { life -= value; }
        if (type == "set") { life = value; }
    }
    public float GetLife() { return life; }

    public void SetMP(string type, float value)
    {
        if (type == "add") { magicpow += value; }
        if (type == "sub") { magicpow -= value; }
        if (type == "set") { magicpow = value; }
    }
    public float GetMP() { return magicpow; }

    public void SetItem(string itemtype, string addsubtype, int value)
    {
        if (itemtype == "rawmutton")
        {
            if (addsubtype == "add")
            {
                rawmutton += value;
            }
            else if (addsubtype == "sub")
            {
                rawmutton -= value;
            }
        }
        if (itemtype == "cookedmutton")
        {
            if (addsubtype == "add")
            {
                cookedmutton += value;
            }
            else if (addsubtype == "sub")
            {
                cookedmutton -= value;
            }
        }
        /*if (itemtype == "cresit")
        {
            if (addsubtype == "add")
            {
                cresit += value;
            }
            else if (addsubtype == "sub")
            {
                cresit -= value;
            }
        }*/
    }
    public int GetRawMutton() { return rawmutton; }
    public void CountGiveMutton() { givemutton++; }
    public float GetGiveMutton() { return givemutton; }

    public void SetTakedArea(bool takedarea) { taked = takedarea; }
    public bool GetTakedArea() { return taked; }

    public void CountGiveAreaMutton() { giveareamutton++; }
    public int GetGiveAreaMutton() { return giveareamutton; }



    public void ShowUI()
    {
        hpvalue.text = "HP:" + life;
        magpowvalue.text = "MP:" + magicpow;
        //cresitvalue.text = "cresit:" + cresit;
        rawmuttonvalue.text = "x" + rawmutton;
        cookedmuttonvalue.text = "x" + cookedmutton;

        hpbar.fillAmount = (life / 100);
        mpbar.fillAmount = (magicpow / 100);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name=="DeadLine")
        {
            placon.RePosition();
        }
    }
}

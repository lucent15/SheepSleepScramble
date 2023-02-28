using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class ScoreDirector : MonoBehaviour
{
    public int hitcount;
    public int killcount;
    public float comboscore;
    public int totalkillcount;

    private bool countswitch;
    private float combocounter;
    public float shot_atk_counttime;
    public float melee_atk_counttime;
    private float nowcounttime;

    public Text hitcountui;
    public Text killcountui;
    public Text comboscoreui;
    public Image combobar;

    public int maxstayenemy;
    private int nowstayenemy;
    public bool enemygenerateswitcher;

    Image ehpbarflame;
    Image ehpbarmain;
    Image ehpbardelay;
    Text ehpname;

    private float delayhpcam;
    private bool enemyhpshowing;
    private float ehpshowtimer;

    int maxcombo;

    public GameObject pausepanel;
    bool pausekirikae;

    Vector3 hituidefopos;
    Vector3 killuidefopos;
    // Start is called before the first frame update

    WaveController wavcon;
    EnemyCommander enecom;

    public GameObject maincamera;
    Recoil recoil;

    public GameObject player;
    PlayerController placon;

    Image recoilupimg;
    Image firingupimg;
    Image regeneimg;

    Text infotext;
    bool infoonecallrecoil;
    bool infoonecallrate;
    bool infoonecallregene;

    void Start()
    {
        enemygenerateswitcher = true;
        ehpbarmain = GameObject.Find("EnemyHPBarMain").GetComponent<Image>();
        ehpbardelay = GameObject.Find("EnemyHPBarDelay").GetComponent<Image>();
        ehpbarflame = GameObject.Find("EnemyHPFlame").GetComponent<Image>();
        ehpname = GameObject.Find("EnemyHPName").GetComponent<Text>();
        // EnemyHPswitch(false);
        delayhpcam = 100;

        pausepanel.SetActive(false);
        // Time.timeScale = 2f;
        pausekirikae = false;
        combobar.fillAmount = 0;
        hituidefopos = hitcountui.transform.position;
        killuidefopos = killcountui.transform.position;
        hitcountui.enabled = false;
        killcountui.enabled = false;
        enecom = GameObject.Find("SCORE DIRECTOR").GetComponent<EnemyCommander>();
        wavcon = GameObject.Find("SCORE DIRECTOR").GetComponent<WaveController>();
        recoil = maincamera.GetComponent<Recoil>();

        placon = player.GetComponent<PlayerController>();
        recoilupimg = GameObject.Find("recoilup").GetComponent<Image>();
        firingupimg = GameObject.Find("firingup").GetComponent<Image>();
        regeneimg = GameObject.Find("regene").GetComponent<Image>();
        infotext = GameObject.Find("WaveWaitInfo").GetComponent<Text>();
        infoonecallrecoil = true;
        infoonecallrate = true;
        infoonecallregene = true;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        ShowUI();

        if (Input.GetKeyDown(KeyCode.Q) && enecom.GetResulted() == false && wavcon.GetResultedWin() == false)
        {
            if (pausekirikae == false)
            {
                Time.timeScale = 0.0f;
                PauseSwitch(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                pausekirikae = true;
            }
            else if (pausekirikae == true)
            {
                Time.timeScale = 1.0f;
                PauseSwitch(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                pausekirikae = false;
            }
        }


        if (hitcount != 0)
        {
            combocounter -= Time.deltaTime;
            combobar.fillAmount = (combocounter / nowcounttime);

            if (countswitch/*ヒットカウント増えたか*/)
            {
                combocounter = nowcounttime;
                countswitch = false;
            }
            if (combocounter < 0)
            {
                comboscore += hitcount * killcount;
                if (maxcombo < killcount) { maxcombo = killcount; }
                combocounter = nowcounttime;
                hitcount = 0;
                killcount = 0;
                hitcountui.enabled = false;
                killcountui.enabled = false;
                ScoreAnim();
            }
            if (killcount >= 30)
            {
                recoil.UpRecoilParam(); recoilupimg.enabled = true;
                if (infoonecallrecoil)
                {
                    SoundManager.instance.PlaySE(SoundManager.SE_Type.Cheers2);
                    StartCoroutine(RewardInfo());
                    infoonecallrecoil = false;
                }

            }
            if (killcount >= 50)
            {
                placon.UpFiringRate(); firingupimg.enabled = true;
                if (infoonecallrate)
                {
                    SoundManager.instance.PlaySE(SoundManager.SE_Type.Cheers2);
                    StartCoroutine(RewardInfo());
                    infoonecallrate = false;
                }
            }
            if (killcount>=80)
            {
                placon.RegeneOn(true);
                regeneimg.enabled = true;
                if (infoonecallregene)
                {
                    SoundManager.instance.PlaySE(SoundManager.SE_Type.Cheers2);
                    StartCoroutine(RewardInfo());
                    infoonecallregene=false;
                }
            }
        }
        else
        {
            recoil.ResetRecoilParam();
            placon.ResetFiringRate();
            placon.RegeneOn(false);
            recoilupimg.enabled = false;
            firingupimg.enabled = false;
            regeneimg.enabled = false;
            infoonecallrate = true;
            infoonecallrecoil = true;
            infoonecallregene = true;
        }
        if (maxstayenemy <= nowstayenemy)
        {
            enemygenerateswitcher = false;
        }
        else { enemygenerateswitcher = true; }

        if (enemyhpshowing && ehpshowtimer >= 0)
        {
            ehpshowtimer -= Time.deltaTime;
        }
        if (ehpshowtimer <= 0) { enemyhpshowing = false; ShowEnemyHP(false); }
    }

    IEnumerator RewardInfo()
    {
        if (killcount >= 30) { infotext.text = "コンボボーナス：反動軽減"; }
        if (killcount >= 50) { infotext.text = "コンボボーナス：レート向上"; }
        if (killcount>= 80) { infotext.text = "コンボボーナス：MP自動回復"; }
        COMBONUSINFOANIM();
        yield return new WaitForSeconds(2);
        infotext.text = " ";
    }

    public void ShowUI()
    {
        hitcountui.text = hitcount + "HIT x";
        killcountui.text = killcount + "KILL >>";
        comboscoreui.text = comboscore + "";
    }

    public void DecideFinishComboScore()
    {
        comboscore += hitcount * killcount;
        if (maxcombo < killcount) { maxcombo = killcount; }
        combocounter = nowcounttime;
        hitcount = 0;
        killcount = 0;
    }

    public float GetComboScore() { return comboscore; }
    public int GetTotalKillCount() { return totalkillcount; }
    public int GetMaxCombo() { return maxcombo; }

    public void AddHitCount() { hitcount++; countswitch = true; hitcountui.enabled = true; Hitanim(); }
    public void AddKillCount() { killcount++; totalkillcount++; killcountui.enabled = true; Killanim(); }
    public void AddStayEnemy() { nowstayenemy++; }
    public void SubStayEnemy() { nowstayenemy--; }
    public bool GetGenerateEnemySwitch() { return enemygenerateswitcher; }
    public void ChangeScoreSubType(string atktype)//
    {

        if (atktype == "melee")
        {
            nowcounttime = melee_atk_counttime;
        }
        if (atktype == "shot")
        {
            nowcounttime = shot_atk_counttime;
        }

    }
    public void ShowEnemyHP(bool dottidai)//テキHPバーの表示
    {
        ehpbarflame.enabled = dottidai;
        ehpbarmain.enabled = dottidai;
        ehpbardelay.enabled = dottidai;
        ehpname.enabled = dottidai;
        enemyhpshowing = dottidai;
        ehpshowtimer = 3;
        if (!dottidai) { ehpbardelay.fillAmount = 1; }
    }

    public void ReadyEnemyHPBar(string name, float maxlife, float enemylife)//ダメージ処理前に一旦取得。ディレイの白に変化前を伝えるため
    {
        ehpbarmain.fillAmount = enemylife / maxlife;
        ehpbardelay.fillAmount = enemylife / maxlife; delayhpcam = enemylife;

    }
    public void EnemyHPinBar(string name, float maxlife, float enemylife)//ダメージ処理後。ここで変化して処理
    {
        ehpbarmain.fillAmount = enemylife / maxlife;
        StartCoroutine(DelayHP(maxlife));
        ehpname.text = name;
        //Debug.Log("いまの" + maxlife + "/" + enemylife);
    }

    IEnumerator DelayHP(float maxlife)//メインとディレイで差がある時に徐々に減らす処理
    {
        yield return new WaitForSeconds(0.1f);
        var delaysum = new WaitForSeconds(0.1f);
        while (ehpbarmain.fillAmount <= ehpbardelay.fillAmount)
        {
            delayhpcam -= 2f;
            ehpbardelay.fillAmount = delayhpcam / maxlife;
            yield return null;
        }

        if (ehpbarmain.fillAmount >= ehpbardelay.fillAmount)
        {
            StopCoroutine(DelayHP(maxlife));
        }
    }

    public void PauseSwitch(bool onoff)
    {
        pausepanel.SetActive(onoff);
    }

    void ScoreAnim()
    {
        //comboscoreui.transform.DOScale(1.0f, 0.7f).SetEase(Ease.OutElastic);
        comboscoreui.transform.DOPunchScale(new Vector3(1.5f, 1.5f, 1.5f), 0.7f);
    }
    void Killanim()
    {
        killcountui.transform.DOShakeScale(0.8f, 0.3f);

        var sequence = DOTween.Sequence();

        sequence.Append(killcountui.transform.DOShakeScale(0.8f, 0.3f))
            .AppendCallback(() =>
            {
                killcountui.transform.localScale = new Vector3(1, 1, 1);
            });

    }
    void Hitanim()
    {
        //hitcountui.transform.DOShakeScale(0.8f,0.1f);
        var sequence = DOTween.Sequence();

        sequence.Append(hitcountui.transform.DOShakeScale(0.8f, 0.1f))
            .AppendCallback(() =>
            {
                hitcountui.transform.localScale = new Vector3(1, 1, 1);
            });
    }

    public void COMBONUSINFOANIM()
    {
        var sequence = DOTween.Sequence();

        sequence.Append(infotext.transform.DOScale(2.0f, 0.2f))
            .AppendCallback(() =>
            {
                infotext.transform.localScale = new Vector3(1, 1, 1);
            });
    }


}

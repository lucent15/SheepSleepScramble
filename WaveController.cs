using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;


public class WaveController : MonoBehaviour
{

    [Header("Wave Level Parameter")]
    public int totalwavestep;
    public float wavewaittime;
    public int totalenemy;
    public int everywavegeneratenum;

    [Header("DO NOT TOUCH")]
    public int wavestep;//現在のウェーブ数格納

    public int onewavegenerate;//現在ウェーブにおける最大投下数
    public int generatedenemy;//スポーンさせた敵の数
    public int killedenemynum;//殺した敵の数

    EnemyGenerator generator1;
    EnemyGenerator generator2;
    EnemyGenerator generator3;

    public string wavestate;

    Text wavewaitinfotext;
    public float waitcountui;

    ResultManager resmane;

    ScoreDirector scodir;

    bool startedresult;

    public GameObject retrypanel;
    public GameObject backtitlepanel;

    public bool resultedwin;
    /*敵の総数
ウェーブ数
ウェーブ間の待機時間
ウェーブごとの総合敵投下数　20 40 60 80 100

ウェーブカウント開始
達するとジェネレータオン。順次投下。投下ごとに投下数がカウントされうｒ。
投下数が達するとジェネレ停止。
敵が死ぬ度に投下数が減り、ゼロになるとウェーブ数が進む。
ウェーブカウント開始。以下クリアまでループ
*/
    void Start()
    {
        generator1 = GameObject.Find("Generator1").GetComponent<EnemyGenerator>();
        generator2 = GameObject.Find("Generator2").GetComponent<EnemyGenerator>();
        generator3 = GameObject.Find("Generator3").GetComponent<EnemyGenerator>();
        onewavegenerate += everywavegeneratenum;
        wavestep += 1;
        wavestate = "wait";
        wavewaitinfotext = GameObject.Find("WaveWaitInfo").GetComponent<Text>();
        wavewaitinfotext.enabled = false;
        waitcountui = wavewaittime + 3;

        scodir = GetComponent<ScoreDirector>();

        resmane = GetComponent<ResultManager>();
        startedresult = false;
        //ゲームスタート
        StartCoroutine(WaveStartCount());

        SoundManager.instance.PlayBGM(SoundManager.BGM_Type.INTRO, false);
        resultedwin = false;
    }

    void Update()
    {
        if (generatedenemy >= onewavegenerate && wavestate == "gaming")//生成した数がジェネレートする数を同じになるか超えるか
        {
            GenerateSwitcher(false);

            if (killedenemynum == onewavegenerate && wavestate == "gaming")//殺した数と生成する数が一致したら
            {
                GenerateSwitcher(false);
                //次のウェーブに行く処理。ウェーブ数追加。1waveにおける投下数増加。
                if (totalwavestep == wavestep && !startedresult)
                {
                    resultedwin = true;
                    SoundManager.instance.StopBGM();
                    wavewaitinfotext.text = "finish!";
                    INFOANIM();
                    scodir.DecideFinishComboScore();
                    resmane.StartCoroutine("ResultStart");
                    startedresult = true;
                }

                if (totalwavestep > wavestep)
                {
                    wavestep++;
                    onewavegenerate += everywavegeneratenum;
                    StartCoroutine(WaveStartCount());
                }

            }
        }

        //if (totalwavestep == wavestep) { wavewaitinfotext.text = "finish!"; }

    }
    public bool GetResultedWin() { return resultedwin; }

    IEnumerator WaveStartCount()
    {
        wavestate = "wait";
        generatedenemy = 0;
        killedenemynum = 0;
        wavewaitinfotext.enabled = true;
        waitcountui = wavewaittime;
        //yield return new WaitForSeconds(wavewaittime);

        while (waitcountui >= 0)
        {
            yield return new WaitForSeconds(1.0f);
            waitcountui--;
            wavewaitinfotext.text = "第" + wavestep + "ウェーブまで ... " + waitcountui.ToString();
        }

        GenerateSwitcher(true);
        wavewaitinfotext.text = "戦え！";
        INFOANIM();
        wavestate = "gaming";
        SoundManager.instance.PlayBGM(SoundManager.BGM_Type.LOOPS);

        yield return new WaitForSeconds(4.0f);
        wavewaitinfotext.text = "";
        //wavewaitinfotext.enabled = false;
    }


    public int GetWaveStep() { return wavestep; }
    public bool GetFinish() { return startedresult; }
    public void AddGeneratedEnemy() { generatedenemy++; }
    public void AddKilledEnemy() { killedenemynum++; }
    public void GenerateSwitcher(bool onoff)
    {
        generator1.GeneratorMasterSwitch(onoff);
        generator2.GeneratorMasterSwitch(onoff);
        generator3.GeneratorMasterSwitch(onoff);
    }

    public void INFOANIM()
    {
        var sequence = DOTween.Sequence();

        sequence.Append(wavewaitinfotext.transform.DOScale(2.0f, 0.2f))
            .AppendCallback(() =>
            {
                wavewaitinfotext.transform.localScale = new Vector3(1, 1, 1);
            });
    }

    public void Retry()
    {
        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1.0f;
    }
    public void RetryOpen() { retrypanel.SetActive(true); }
    public void RetryClose() { retrypanel.SetActive(false); }
    public void TitleBack()
    {
        SoundManager.instance.StopBGM();
        SceneManager.LoadScene("TitleScene"); }
    public void TitlebackpanelOpen() { backtitlepanel.SetActive(true); }
    public void TitlebackpanelClose() { backtitlepanel.SetActive(false); }


}

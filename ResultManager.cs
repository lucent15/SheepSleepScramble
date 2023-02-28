using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    /*
    1. リザルトタイトル表示
    2.討伐数タイトル表示と同時にスコア数と討伐数変数に加算。表示量になったら停止。
    3.勢力タイトル表示と同時に勢力変数とスコア変数に加算。表示量になったら停止
    4.コンボスコアタイトル表示と同時に変数加算。
    5.if文で条件を満たすとテキスト枠にボーナススコアネームと改行を追加。それに応じた変数も追加＆改行。加算はナシ。
    6.一寸待って全てを足したトータルスコアを表示する。
    */

    ScoreDirector scodir;
    EnemyCommander enecom;
    PlayerStatus plaste;

    float resultkillscore;
    float resulttotallife;
    float resultcomboscore;

    float bonusscore;

    float totalscore;

    public GameObject resultpanel;
    public Text resulttitle;
    public Text scorename;
    public Text scorenum;
    public Text totalscorename;
    public Text totalscorenum;



    void Start()
    {
        scodir = GetComponent<ScoreDirector>();
        enecom = GetComponent<EnemyCommander>();
        plaste = GameObject.Find("Player").GetComponent<PlayerStatus>();

        //StartCoroutine(ResultStart());
    }

    IEnumerator ResultStart()
    {
        var onecount = new WaitForSeconds(0.1f);
        ScoreGather();
        Debug.Log("ごうけい：" + resultkillscore);
        if (Mathf.CeilToInt(resultkillscore) == 300)
        {
            SoundManager.instance.PlayBGM(SoundManager.BGM_Type.FINISH, false);
        }
        else if (resultkillscore < 300)
        {
            SoundManager.instance.PlayBGM(SoundManager.BGM_Type.ASA, false);

        }
        yield return onecount;
        Time.timeScale = 0.1f;

        yield return onecount;
        resultpanel.SetActive(true);
        resulttitle.enabled = true;
        yield return onecount;
        scorename.enabled = true;
        scorename.text = "討伐数　　　：\n";
        scorenum.text = Mathf.CeilToInt(resultkillscore) + "/300 = " + Mathf.CeilToInt(resultkillscore) * 100 + "\n";//いんとかここ
        yield return onecount;
        scorename.text += "勢力ゲージ　：\n";
        scorenum.text += Mathf.CeilToInt(resulttotallife) + " = " + Mathf.CeilToInt(resulttotallife) * 100 + "\n";//int火ここ
        yield return onecount;
        scorename.text += "コンボスコア：";
        scorenum.text += resultcomboscore;
        yield return onecount;
        //ここからその他スコア表示
        scorename.text += "\n\n"; scorenum.text += "\n\n";
        if (plaste.GetDamageCount() == 0)
        {
            scorename.text += "\nノーダメージボーナス"; scorenum.text += "\n+" + 200000; bonusscore += 200000; yield return onecount;
        }
        if (plaste.GetHealCount() == 0)
        {
            scorename.text += "\n一度も回復しなかった"; scorenum.text += "\n+" + 100000; bonusscore += 100000; yield return onecount;
        }
        if (plaste.GetGiveMutton() >= 36)
        {
            scorename.text += "\nたくさん肉を焼いた"; scorenum.text += "\n+" + 50000; bonusscore += 50000; yield return onecount;
        }
        if (plaste.GetGiveAreaMutton()>=100)
        {
            scorename.text += "\n生肉をたくさん提供した"; scorenum.text += "\n+" + 50000; bonusscore += 50000; yield return onecount;
        }
        if (!plaste.GetTakedArea())
        {
            scorename.text += "\n一度もエリアを取られなかった"; scorenum.text += "\n+" + 100000; bonusscore += 100000; yield return onecount;
        }
        if (scodir.GetMaxCombo() >= 50)
        {
            scorename.text += "\n50キルコンボ達成"; scorenum.text += "\n+" + 5000; bonusscore += 5000; yield return onecount;
        }
        if (scodir.GetMaxCombo() >= 100)
        {
            scorename.text += "\n100キルコンボ達成"; scorenum.text += "\n+" + 10000; bonusscore += 10000; yield return onecount;
        }

        totalscorename.enabled = true;
        yield return onecount;
        totalscorenum.enabled = true;
        totalscore += (resultkillscore * 100);
        totalscore += (resulttotallife * 100);
        totalscore += resultcomboscore;
        totalscore += bonusscore;
        totalscorenum.text = Mathf.CeilToInt(totalscore) + "";
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ScoreGather()
    {
        resultkillscore = scodir.GetTotalKillCount();
        resultcomboscore = scodir.GetComboScore();
        resulttotallife = enecom.GetTotalLife();
    }

}

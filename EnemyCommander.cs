using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCommander : MonoBehaviour
{

    Transform point_a;
    Transform point_b;
    Transform point_c;

    AreaScript aposaresc;
    AreaScript bposaresc;
    AreaScript cposaresc;

    float areastate_a;
    float areastate_b;
    float areastate_c;


    Vector3 playerPos;  //プレイヤーの位置
    private GameObject player;   //プレイヤーぶち込む枠
    float distance; //プレイヤーとの距離
                    // Start is called before the first frame update

    public int commandspan;

    public Image totallifegauge;
    public float totallife;

    Text infobox;

    ResultManager resmane;
    public bool resulted;

    Transform nowTargetPos;
    void Start()
    {
        point_a = GameObject.Find("PointA").transform;
        point_b = GameObject.Find("PointB").transform;
        point_c = GameObject.Find("PointC").transform;
        player = GameObject.Find("Player");
        StartCoroutine(ReTargetPosition());

        aposaresc = GameObject.Find("PointA").GetComponent<AreaScript>();
        bposaresc = GameObject.Find("PointB").GetComponent<AreaScript>();
        cposaresc = GameObject.Find("PointC").GetComponent<AreaScript>();

        totallife = 100;

        infobox = GameObject.Find("WaveWaitInfo").GetComponent<Text>();
        resmane = GetComponent<ResultManager>();
        resulted = false;
    }

    void Update()
    {
        playerPos = player.transform.position;

        areastate_a = aposaresc.GetAreaState();
        areastate_b = bposaresc.GetAreaState();
        areastate_c = cposaresc.GetAreaState();
        totallifegauge.fillAmount = totallife / 100;

        if ((areastate_a + areastate_b + areastate_c) < 0)
        {
            totallife -= (Time.deltaTime);
        }

        if (totallife <= 0)
        {
            if (!resulted)
            {
                SoundManager.instance.StopBGM();
                infobox.text = "FINISH！";
                resmane.StartCoroutine("ResultStart");
            }
            //プレイヤーの操作切る
            //プレイヤーのアニメ待機へ
            resulted = true;
        }

    }
    public bool GetResulted()
    {
        return resulted;
    }



    public void SetTotalLife(float num, string addorsub)
    {
        if (addorsub == "add") { totallife += num; }
        if (addorsub == "sub") { totallife -= num; }
    }
    public float GetTotalLife() { return totallife; }

    public Transform NowTargetPosition() { return nowTargetPos; }

    IEnumerator ReTargetPosition()
    {
        while (true)
        {
            yield return new WaitForSeconds(commandspan);
            //Debug.Log("ターゲット変更！！");
            var dicekekka = ThrowDice();
            if (dicekekka == 1)
            {
                //Debug.Log("プレイヤーから遠い場所を狙え！");
                nowTargetPos = DistanceBetweenPlandPos();
            }
            else if (dicekekka == 2)
            {
                // Debug.Log("取れそうなところ取れ！");
                nowTargetPos = ChoiceNotEnoughtConquestPos();
            }
            else if (dicekekka == 3)
            {
                // Debug.Log("あいつをころせ！！！！");
                nowTargetPos = player.transform;
            }
            dicekekka = 0;
        }
    }

    public Transform DistanceBetweenPlandPos()
    {
        float distancetoA = Vector3.Distance(playerPos, point_a.position);
        float distancetoB = Vector3.Distance(playerPos, point_b.position);
        float distancetoC = Vector3.Distance(playerPos, point_c.position);
        //Debug.Log("a" + distancetoA + "b" + distancetoB + "c" + distancetoC);

        if ((distancetoA > distancetoB && distancetoA > distancetoC) && areastate_a > -100)
        {
            //Debug.Log("A！！");

            return point_a;
        }
        if ((distancetoB > distancetoA && distancetoB > distancetoC) && areastate_b > -100)
        {
            // Debug.Log("B！！");

            return point_b;
        }
        if ((distancetoC > distancetoA && distancetoC > distancetoB) && areastate_c > -100)
        {
            // Debug.Log("C！！");

            return point_c;
        }
        //たとえプレイヤーから遠くても既に取ってる場所を狙わないようにしたい
        //
        /*var randamu = Random.Range(1, 3);
        if (randamu == 1) { Debug.Log("テキトーにA"); return point_a; }
        else if (randamu == 2) { Debug.Log("テキトーにB"); return point_b; }
        else if (randamu == 3) { Debug.Log("テキトーにC"); return point_c; }
        */
        return ChoicePlayerPos();
    }

    public Transform ChoiceNotEnoughtConquestPos()
    {
        //借り変更点：＜に＝を追加してる
        if (areastate_a <= areastate_b && areastate_a <= areastate_c && areastate_a > -100)
        {
            // Debug.Log("A取れそう");
            return point_a;
        }
        if (areastate_b <= areastate_a && areastate_b <= areastate_c && areastate_b > -100)
        {
            //Debug.Log("B取れそう");

            return point_b;
        }
        if (areastate_c <= areastate_a && areastate_c <= areastate_b && areastate_c > -100)
        {
            //Debug.Log("C取れそう");

            return point_c;
        }
        //Debug.Log("決まんねぇから遠いところだ");
        return DistanceBetweenPlandPos();
    }
    //２つの拠点の値が同じだった場合の処理がないので、例えば2拠点撮ってるとどっちも100なので順位が決められなくなる
    //現在の標的の定め方はプレイヤーを避ける消極的なモノなので、プレイヤーに立ち向かうような積極的な定め方が必要。

    public Transform ChoicePlayerPos()
    {
        //-100、エネミー領地ではない拠点を選ぶ。
        float distancetoA = Vector3.Distance(playerPos, point_a.position);
        float distancetoB = Vector3.Distance(playerPos, point_b.position);
        float distancetoC = Vector3.Distance(playerPos, point_c.position);
        //Debug.Log("a:" + distancetoA + "b" + distancetoB + "c" + distancetoC);
        // Debug.Log("a is:" + areastate_a + "b is:" + areastate_b + "c is:" + areastate_c);
        if (areastate_a <= -99)//Aが低いのでBかC
        {
            if (distancetoB > distancetoC)
            {
                //Debug.Log("AyappaB");
                return point_b;
            }
            else
            {
                // Debug.Log("AyappaC");
                return point_c;
            }
        }
        if (areastate_b <= -99)//Bが低いのでAかC
        {
            if (distancetoA > distancetoC)
            {
                // Debug.Log("ByappaA");
                return point_a;
            }
            else
            {
                //  Debug.Log("ByappaC");
                return point_c;
            }
        }
        if (areastate_c <= -99)//Cが低いのでAかB
        {
            if (distancetoA > distancetoB)
            {
                // Debug.Log("CyappaA");
                return point_a;
            }
            else
            {
                //  Debug.Log("CyappaB");
                return point_b;
            }
        }
        // Debug.Log("じゃあ俺、ギャラもらって帰るから");
        return null;
    }

    public int ThrowDice()
    {
        // 乱数の範囲指定で配列のインデックスを取得する
        return Random.Range(1, 4);
    }
}

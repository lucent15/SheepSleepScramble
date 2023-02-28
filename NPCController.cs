using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class NPCController : MonoBehaviour
{

    Animator anim;

    public GameObject area;
    AreaScript aresc;

    bool dotweenswitcher;

   
    void Start()
    {
        aresc = area.GetComponent<AreaScript>();
        anim = GetComponent<Animator>();
        dotweenswitcher = true;
    }
    void Update()
    {
        KillLeak();

        if (aresc.GetAreaState() >= 0 && dotweenswitcher)
        {
            WakeUpShake();
            AnimeChange(true);
            dotweenswitcher = false;
        }
        else if (aresc.GetAreaState() < 0 && !dotweenswitcher)
        {
            AnimeChange(false);
            dotweenswitcher = true;
        }

        if (aresc.GetSeCount() >= 10 && aresc.GetAreaState() >= 0)
        {
            WakeUpShake();
        }
    }
    public void KillLeak()
    {
        if (aresc == null) { aresc = area.GetComponent<AreaScript>();  }
        if (anim == null) { anim = GetComponent<Animator>(); }
    }
    public void AnimeChange(bool state)
    {
        if (state == true)
        {
            switch (ThrowDice())
            {
                case 1:
                    anim.Play("ALLY1");
                    break;
                case 2:
                    anim.Play("ALLY2");
                    break;
                case 3:
                    anim.Play("ALLY3");
                    break;
                case 4:
                    anim.Play("ALLY4");
                    break;
            }


        }
        else if (state == false)
        {

            switch (ThrowDice())
            {
                case 1:
                    anim.Play("ENEMY1");
                    break;
                case 2:
                    anim.Play("ENEMY2");
                    break;
                case 3:
                    anim.Play("ENEMY3");
                    break;
                case 4:
                    anim.Play("ENEMY4");
                    break;

            }
        }
    }
    public int ThrowDice()
    {
        // 乱数の範囲指定で配列のインデックスを取得する
        return Random.Range(1, 5);
    }

    public void WakeUpShake()
    {
        transform.DOJump(transform.position, 3f, 1, 1.5f);
        var sequence = DOTween.Sequence();
        sequence.Append(transform.DOShakeScale(1f, 1.2f))
            .AppendCallback(() =>
            {
                transform.localScale = new Vector3(1, 1, 1);
            });
        //Debug.Log(this.name + ":" + aresc.name + " ;" + aresc.GetSeCount());
    }
}

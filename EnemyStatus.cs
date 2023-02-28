using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//これがないとメニューから作成できない
[CreateAssetMenu(menuName = "EnemyStatus")]
public class EnemyStatus : ScriptableObject
{
    [SerializeField]
    public string enemyname;
    [SerializeField]
    public float enemylife;
    [SerializeField]
    public float attackpow;
    [SerializeField]
    public float movespeed;
}

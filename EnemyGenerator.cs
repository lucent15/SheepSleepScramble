using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{

    public GameObject makaihitsuji;
    public GameObject speedtype;
    public GameObject powertype;

    private float counter;
    public float generaterate;


    private ScoreDirector scored;
    private WaveController wavcon;

    bool masterswitch;



    void Start()
    {
        masterswitch = false;
        scored = GameObject.Find("SCORE DIRECTOR").GetComponent<ScoreDirector>();
        wavcon = GameObject.Find("SCORE DIRECTOR").GetComponent<WaveController>();
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;

        if (masterswitch && (counter > generaterate || Input.GetKeyDown(KeyCode.G)) && scored.GetGenerateEnemySwitch())
        {
            var dicekekka = Random.Range(0, 100);
            if (wavcon.GetWaveStep() < 3) { GenerateNormalType(); }
            if (wavcon.GetWaveStep() == 3)
            {
                if (dicekekka >= 90)
                {
                    GenerateSpeedType();
                }
                else { GenerateNormalType(); }
            }
            if (wavcon.GetWaveStep() == 4)
            {
                if (dicekekka >= 80 && dicekekka <= 90)
                {
                    GenerateSpeedType();
                }
                else if (dicekekka >= 90)
                {
                    GeneratePowerType();
                }
                else if (dicekekka<90) 
                {
                    GenerateNormalType();
                }
            }
            if (wavcon.GetWaveStep()==5)
            {
                if (dicekekka >= 60 && dicekekka <= 80)
                {
                    GenerateSpeedType();
                }
                else if (dicekekka > 80)
                {
                    GeneratePowerType();
                }
                else if (dicekekka < 60)
                {
                    GenerateNormalType();
                }
            }


            counter = 0;
            scored.AddStayEnemy();
            wavcon.AddGeneratedEnemy();
        }
    }

    public void GeneratorMasterSwitch(bool onoff)
    {
        masterswitch = onoff;
    }

    public void GenerateNormalType() { Instantiate(makaihitsuji, this.transform.position, this.transform.rotation); }
    public void GeneratePowerType() { Instantiate(powertype, this.transform.position, this.transform.rotation); }
    public void GenerateSpeedType() { Instantiate(speedtype, this.transform.position, this.transform.rotation); }
}

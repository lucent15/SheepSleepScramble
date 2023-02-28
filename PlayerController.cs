using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    float inputHorizontal;
    float inputVertical;
    bool inputdash;
    bool inputjump;
    bool inputRmouse;
    bool inputLmouse;
    bool inputLmouseUp;

    CharacterController characon;

    public float speed = 0.0f;       //歩行速度
    public float basespeed = 6.0f;
    public float dashspeed = 10.0f;//ダッシュスピード
    public float jumpSpeed = 8.0F;   //ジャンプ力
    public float gravity = 20.0F;    //重力の大きさ

    private Vector3 moveDirection = Vector3.zero;
    private float h, v;

    public GameObject alicia;

    public GameObject magirod;

    Animator anim;

    RaycastHit hit;//接地判定用
    private bool forland;

    [Header("エイム連動の腰とかaimik")]
    [SerializeField]
    private Transform spine;

    private bool canATK;

    [Header("ロッドと石")]
    public GameObject Blade;
    public GameObject Magstone;
    public GameObject magbigstone;
    public GameObject stoneeffect;
    private bool stoefeplayed;
    private bool stoefeplayed2;

    [Header("えふぇくとたちよ")]
    public GameObject mahojin;
    public GameObject changeeffect;

    [Header("武器しまう時間")]
    public float sheathetime;
    private bool rodstate;
    private float sheathecount;

    [Header("efekuto")]
    public TrailRenderer slasheffect;

    //近接攻撃アシスト
    public GameObject meleerange;
    MeleeAssist meleasi;

    public bool meleecolswitch;

    [Header("魔法攻撃のため")]
    public GameObject magicstone;
    public GameObject maincamera;
    public GameObject magiarrowtrail;
    public GameObject muzzleflash;
    ParticleSystem muzzleflashper;
    bool flashswitch;
    public float shootrange;
    public float firingrate;
    private float shotinterval;

    ScoreDirector scored;
    PlayerStatus plaste;
    Image reticle;


    private bool die;

    bool pauseswitcher;

    ShakeObjectsByDOTween shaker;

    GameObject respawnpos;

    Recoil recoilsc;

    bool regeneswitch;
    float regenecount=1f;
    void Start()
    {
        characon = GetComponent<CharacterController>();
        anim = alicia.GetComponent<Animator>();
        canATK = true;
        Blade.SetActive(false);
        Magstone.SetActive(false);
        magbigstone.SetActive(false);

        meleasi = meleerange.GetComponent<MeleeAssist>();

        scored = GameObject.Find("SCORE DIRECTOR").GetComponent<ScoreDirector>();
        plaste = GetComponent<PlayerStatus>();
        shotinterval = 0;
        shaker = GameObject.Find("SCORE DIRECTOR").GetComponent<ShakeObjectsByDOTween>();


        reticle = GameObject.Find("Reticle").GetComponent<Image>();
        //Time.timeScale = 0.1f;
        muzzleflashper = muzzleflash.GetComponent<ParticleSystem>();

        respawnpos = GameObject.Find("ResPawnPos");
        recoilsc = maincamera.GetComponent<Recoil>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (pauseswitcher == false)
            {
                pauseswitcher = true;
            }
            else if (pauseswitcher == true)
            {
                pauseswitcher = false;
            }
        }
        if (!die && Time.timeScale != 0)
        {
            inputHorizontal = Input.GetAxis("Horizontal");
            inputVertical = Input.GetAxis("Vertical");
            inputjump = Input.GetKeyDown(KeyCode.Space);
            inputdash = Input.GetKey(KeyCode.LeftShift);
            inputRmouse = Input.GetMouseButton(1);
            inputLmouse = Input.GetMouseButton(0);
            inputLmouseUp = Input.GetMouseButtonUp(0);
        }
        h = Input.GetAxisRaw("Horizontal");    //左右矢印キーの値(-1.0~1.0)
        v = Input.GetAxisRaw("Vertical");      //上下矢印キーの値(-1.0~1.0)

        // カメラの方向から、X-Z平面の単位ベクトルを取得
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        // 方向キーの入力値とカメラの向きから、移動方向を決定
        Vector3 moveForward =/* ここにスムースがほしい*/ cameraForward * inputVertical + Camera.main.transform.right * inputHorizontal;


        //接地判定とか=========================================
        var radius = transform.lossyScale.x * 0.3f;
        int layerMask = 1 << 8 | 1 << 10 | 1 << 11 | 1 << 2;//ここでは８レイヤ(弾)と９レイヤ（テキ視界）とだけ衝突する　さらに１１（近接）
        layerMask = ~layerMask;//弾自身と、視界、それから１１を無視する
        var isHit = Physics.SphereCast(transform.position, radius, (-transform.up * 0.8f), out hit, 0.9f, layerMask);    //ぶっといキャストで設置判定
        //Physics.SphereCast(開始座標,半径,伸びる方向*,out hit,長さ)
        if (isHit)
        {
            anim.SetBool("fall", false);

            if (forland)
            {
                anim.PlayInFixedTime("landing", 0, 0.0f);
                forland = false;
                moveDirection.y = 0;
            }


            if (inputjump && forland == false
                && !anim.GetCurrentAnimatorStateInfo(0).IsName("landing")
                && !anim.GetCurrentAnimatorStateInfo(0).IsName("FALL"))
            {
                moveDirection.y = jumpSpeed;
                //anim.SetBool("jump", true);
                anim.Play("JUMP", 0, 0.13f);
                SoundManager.instance.PlaySE(SoundManager.SE_Type.Jumping);
            }
        }
        else
        {
            anim.SetBool("fall", true);
            forland = true;
            moveDirection.y -= gravity * Time.deltaTime;
        }
        //接地判定ここまで

        if (inputdash && !inputRmouse)
        {
            speed = dashspeed;
        }
        else if (!inputdash) { speed = basespeed; }

        if (h == 0 && v == 0) { speed = 0; }
        //落下
        characon.Move(moveDirection * Time.deltaTime);


        //移動 ※斜め移動時の平方根
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("landing")
            && !anim.GetCurrentAnimatorStateInfo(0).IsName("ATTACK"))
        {
            if (inputVertical != 0 && inputHorizontal != 0)
            {
                characon.Move(moveForward * Time.deltaTime * speed);
            }
            else
            {
                characon.Move(moveForward * Time.deltaTime * (speed * 1.4f));
            }
        }


        if (!die && moveForward != new Vector3(0, 0, 0)) transform.rotation = Quaternion.LookRotation(moveForward);


        //間に遷移用のカム変数がいる
        //if ((h != 0 || v != 0) || (inputVertical != 0 || inputHorizontal != 0)) { anim.SetFloat("move", 1); } else { anim.SetFloat("move", 0); }

        anim.SetFloat("move", speed, 0.1f, Time.deltaTime);

        //えいむあにめ
        if (inputRmouse)
        {
            anim.SetBool("aim", true);
            mahojin.SetActive(true);
            RodChange(1);
            magbigstone.SetActive(true);
            if (stoefeplayed == false) { stoneeffect.SetActive(false); stoneeffect.SetActive(true); }
            stoefeplayed = true;
            stoefeplayed2 = false;

            //レティクル追加
            reticle.enabled = true;
        }
        else
        {
            anim.SetBool("aim", false);
            mahojin.SetActive(false);
            magbigstone.SetActive(false);
            if (stoefeplayed2 == false) { stoneeffect.SetActive(false); stoneeffect.SetActive(true); }
            stoefeplayed2 = true;
            stoefeplayed = false;
            reticle.enabled = false;
        }

        //近接攻撃アニメ
        if (inputLmouse && !inputRmouse && !anim.GetBool("fall")
            && !anim.GetCurrentAnimatorStateInfo(0).IsName("ATTACK") && canATK)
        {

            if (meleasi.InRangeTeller())
            {
                Vector3 attackpos = meleasi.TargetPosTeller();
                attackpos.y = transform.position.y;
                transform.LookAt(attackpos);//補正
            }
            anim.SetBool("attack", true);
            canATK = false;
            RodChange(0);
            meleasi.InrangeOff();
            SoundManager.instance.PlaySE(SoundManager.SE_Type.Swing);
        }
        else
        {
            anim.SetBool("attack", false);
        }

        //近接攻撃間隔空け用 後述：多分これいらない
        if (!canATK
            && anim.GetCurrentAnimatorStateInfo(0).length >= 0.7)
        {
            canATK = true;
        }

        if (anim.GetBool("attack"))
        {
            anim.Play("ATTACK", 0, 0.3f);
            meleecolswitch = true;
            slasheffect.emitting = true;//0.5made
        }

        if (meleecolswitch) //近接攻撃のコライダー消し
        {
            //ここにメレーウェポンの判定無効化奴
            StartCoroutine(MeleeColOffDelayer());
        }

        //えふぇくと
        if (slasheffect.emitting == true && anim.GetCurrentAnimatorStateInfo(0).IsName("ATTACK")
            && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.4f)
        {
            slasheffect.emitting = false;
        }


        if (Blade.activeSelf == true || Magstone.activeSelf == true) rodstate = true;

        if (rodstate)
        {
            sheathecount += Time.deltaTime;
            if (sheathetime < sheathecount)
            {
                Blade.SetActive(false);
                Magstone.SetActive(false);
                changeeffect.SetActive(false);
                changeeffect.SetActive(true);
                sheathecount = 0;
                rodstate = false;
            }
        }


        regenecount += Time.deltaTime;
        //リジェネMP
        if (regeneswitch&&regenecount>=1) {
            plaste.SetMP("add", 2);
            regenecount = 0;
        }
    }
   

    public void RegeneOn(bool regeneswitcher) { regeneswitch = regeneswitcher; }

    public void DeadState()
    {
        anim.Play("DEAD");
        die = true;
        characon.enabled = false;
    }
    public void Reviving()
    {
        this.transform.position = respawnpos.transform.position;
        die = false;
        anim.Play("JUMP", 0, 0.13f);
        characon.enabled = true;
        moveDirection.y = jumpSpeed / 2;
    }

    public void RePosition()
    {
        characon.enabled = false;

        this.transform.position = respawnpos.transform.position;
        characon.enabled = true;
    }

    IEnumerator MeleeColOffDelayer()//攻撃後の攻撃判定無効か
    {
        yield return new WaitForSeconds(0.2f);
        meleecolswitch = false;
    }


    public bool Getmeleecolswitch() { return meleecolswitch; }
    public void RodChange(int armtype)
    {
        sheathecount = 0;
        if (armtype == 0 && Blade.activeSelf == false)
        {
            Blade.SetActive(true); Magstone.SetActive(false); changeeffect.SetActive(false);
            changeeffect.SetActive(true);
        }
        if (armtype == 1 && Magstone.activeSelf == false)
        {
            Blade.SetActive(false); Magstone.SetActive(true); changeeffect.SetActive(false);
            changeeffect.SetActive(true);
        }
    }
    void LateUpdate()
    {
        // カメラの方向から、X-Z平面の単位ベクトルを取得
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 aimForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 1, 1)).normalized;
        if (inputRmouse)
        {
            transform.rotation = Quaternion.LookRotation(cameraForward);

            spine.rotation = Quaternion.LookRotation(aimForward);

            var lookAtRotation = Quaternion.LookRotation(aimForward, Vector3.up);
            var offsetRotation = Quaternion.FromToRotation(Vector3.forward, Vector3.up);
            spine.rotation = lookAtRotation * offsetRotation;
        }
    }
    void FixedUpdate()
    {
        if (shotinterval <= firingrate) shotinterval++;

        if (inputRmouse && inputLmouse)
        {
            if (shotinterval > firingrate)
            {
                if (plaste.GetMP() > 0) ShotMagic(); shotinterval = 0;
            }
        }
        if (inputLmouseUp)
        {
            shotinterval = 0;
        }
    }

    void ShotMagic()
    {
        ArrowTrail();
        recoilsc.RecoilFire();
        muzzleflashper.Play();

        SoundManager.instance.PlaySE(SoundManager.SE_Type.MagiShot);
        //Action3dCamのシェイクコルーチン起動

        Ray ray = new Ray(maincamera.transform.position, maincamera.transform.forward);

        int layerMask = 1 << 8 | 1 << 10; //８レイヤと10レイヤーとだけ衝突する　を下で反転
        layerMask = ~layerMask; //８レイヤーと10レイヤーとだけ衝突しない


        //目的：10番目のAreaレイヤーとだけ衝突しないようにしたい
        int laymas2 = ~(1 << 10);


        /*RaycastHit[] hits;
        hits = Physics.RaycastAll(maincamera.transform.position,maincamera.transform.forward,100f,laymas2);
        Debug.DrawRay(maincamera.transform.position, maincamera.transform.forward*shootrange, Color.red,5);

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            Debug.Log(hit.transform.name);
            if (hit.transform.tag == "Enemy")
            {
                Debug.Log("撃たれた");
                hit.transform.GetComponent<MakaiHitsujiStatus>().Damage();
                Debug.Log("ダメージ処理ed");
            }
        }*/

        RaycastHit hit;
        //        Debug.DrawRay(ray.origin, ray.direction*shootrange, Color.red, 5);
        //Debug.Log("撃ててる");
        if (Physics.Raycast(ray, out hit, shootrange, laymas2))
        {
            //Debug.Log(hit.transform.name);
            Debug.DrawRay(ray.origin, ray.direction * shootrange, Color.red, 5);
            if (hit.transform.tag == "Enemy")
            {
                hit.transform.GetComponent<MakaiHitsujiStatus>().Damage();
                scored.ChangeScoreSubType("shot");
                //shaker.Shake(0);
                //shaker.Shake(3);


            }
        }

        plaste.SetMP("sub", 1);
    }
    private void ArrowTrail()
    {
        Quaternion qua = Quaternion.Euler(maincamera.transform.eulerAngles);
        Instantiate(magiarrowtrail, magicstone.transform.position, qua);
    }

    public void UpFiringRate()
    {
        firingrate = 5;
    }
    public void ResetFiringRate() { firingrate = 10; }
    private void OnDrawGizmos()
    {
        var radius = transform.lossyScale.x * 0.3f;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + (-transform.up * 0.8f), radius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(0.1f, 0.1f, 0.1f));

        /*if (meleasi.InRangeTeller()) { 
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(meleasi.TargetPosTeller(), 1); }*/
    }
}

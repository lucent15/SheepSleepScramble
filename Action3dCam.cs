/*
このプログラムは次の機能を有します。
• カメラはプレイヤーの操作によって向きを変えられる。水平方向は際限なく、垂直方向は限界値を設ける。
• カメラは 常に画面の中心にキャラが見えるよう向きを調整する。
• カメラは 常に一定の距離を保ってキャラを追尾するよう位置を調整する。
• カメラは Collidor を避けなければならない。つまり壁や柱の中に入り込むことはできない。
• カメラとキャラの間に Collidor があったとき、カメラは一時的に近づいてキャラが見えなくなることを防止する。
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class Action3dCam : MonoBehaviour
{
    // カメラが追従する対象となる GameObject：Inspectorでキャラのオブジェクトをここに指定
    public Transform target;    //pivot

    // カメラと視線を遮る GameObjectのフラグのマスク：全てのオブジェクトを対象にする
    private LayerMask lineOfSightMask = -1;

    private LayerMask teslayemask = 1 << 8 | 1 << 9;//ここでは８レイヤと９レイヤとだけ衝突する



    // カメラが追従する対象からのオフセット：キャラの中心は足元にあるため1.5m上に調整している
    //private Vector3 targetOffset = new Vector3(0.0f, 0.0f, 0.0f);   //pivot

    // ターゲットとカメラの現在の距離：遮るものがあるとこの距離は縮まる
    private float currentDistance = -6.5f;


    public float zoomdis;
    public float defdis;

    // ターゲットとカメラの距離：遮るものがない時の距離
    private float distance = -6.5f;

    // カメラの視点の角度
    private float x = 0.0f;
    private float y = 0.0f;

    // ターゲットとカメラの距離が変更されるスピード（メソッドが返す値を格納する変数）
    private float distanceVelocity = 0.0f;


    private Vector3 comenow;

    private bool poseswitch = false;


    [Header("カメラの移動する速度。原動力！０はダメ")]
    public float camspeed;
    [Header("コンフィグ用")]
    public Slider slider;
    public Toggle Xtoggle;
    public Toggle Ytoggle;
    private bool Xreverse;
    private bool Yreverse;

    //リコイル処理用変数 消す
    private bool recoilstart;
    private float recoilends;
    private bool recoilcontroll;
    private float RCends;
    private float curevelo = 0;
    private float gunrecoilvar;
    private float RCendsChecker;

    void Start()
    {
        
        // 配置されたカメラの視点の角度（初期値）を問い合わせる
        Vector3 angles = transform.eulerAngles; //tukau
        x = angles.y;
        y = angles.x;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && poseswitch == false)
        {
            poseswitch = true;
        }
        else if (Input.GetKeyDown(KeyCode.Q) && poseswitch == true)
        {
            poseswitch = false;
        }
    }

    // カメラの更新は他の更新よりも後に行うべきであるため、LateUpdate() を使う
    void LateUpdate()
    {
        if (poseswitch == false)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }

            if (Input.GetMouseButtonDown(1))
            {
                distance = zoomdis;   //-2.0f
            }
            else if (Input.GetMouseButtonUp(1))
            {
                distance = defdis;
            }

            //カメラ入力
            if (Xreverse == false)
            {
                x += Input.GetAxis("Mouse X") * camspeed; // マウスのX軸の移動：補正を加えている　旧：4.0f
            }
            else
            {
                x -= Input.GetAxis("Mouse X") * camspeed;
            }
            if (Yreverse == false)
            {
                y -= Input.GetAxis("Mouse Y") * camspeed; // マウスのY軸の移動：補正を加えている     //pivot　旧：1.6f
            }
            else if (Yreverse == true)
            {
                y += Input.GetAxis("Mouse Y") * camspeed;
            }

            //x += Input.GetAxis("Stick X"); // コントローラのスティックX軸の角度：名称はInput Managerに設定が必要
            //y += Input.GetAxis("Stick Y"); // コントローラのスティックY軸の角度：名称はInput Managerに設定が必要

            // カメラのY方向に制限を加える   //pivot 次回ここから
            y = Mathf.Clamp(y, -50, 80);

            // 入力された値から視点の角度を計算する
            Quaternion rotation = Quaternion.Euler(y, x, 0);    //pivot

            // ターゲットオブジェクトの中心点にオフセットを足したものをターゲットとする：キャラの頭辺りになる
            Vector3 targetPos = target.position; //oivot     元々はターゲットオフセット＋してた

            // 視点の角度から方向を求める
            Vector3 direction = rotation * -Vector3.forward;    //pivot 元々ディスタンスはなし distanceかけるのやめてみる　多分distanceかけなくていい？
                                                                //Vector3 direction = transform.position; 


            // カメラとターゲットの距離を算出する
            float targetDistance = -AdjustLineOfSight(targetPos, direction);

            // カメラとターゲットの距離をスムースに変更する：遮るものがあったときに機能する
            currentDistance = Mathf.SmoothDamp(currentDistance, targetDistance, ref distanceVelocity, 0.06f);

            // カメラの角度を設定
            //transform.rotation = rotation;  //pivot

            // カメラの位置を設定
            //transform.position = targetPos + direction * currentDistance;   //pivot
            //transform.position.z = currentDistance;   //pivot
            comenow = transform.localPosition;
            comenow.z = currentDistance;
            transform.localPosition = comenow;

        }

        if (recoilstart)
        {
            recoilends += Mathf.SmoothDamp(recoilends, gunrecoilvar, ref curevelo, 0.1f);
            y -= recoilends;
            if (recoilends >= gunrecoilvar)
            {
                recoilends = 0;
                recoilstart = false;
                recoilcontroll = true;
            }
        }
        if (recoilcontroll)
        {
            RCends = Mathf.Lerp(0, gunrecoilvar, 0.08f);
            RCendsChecker += RCends;
            y += RCends;
            if (RCendsChecker >= gunrecoilvar * 2)
            {
                RCends = 0;
                RCendsChecker = 0;
                recoilcontroll = false;
            }
        }
    }

    // カメラとターゲットの距離を算出するメソッド
    private float AdjustLineOfSight(Vector3 target, Vector3 direction)
    {
        RaycastHit hit; // レイキャストを飛ばしてヒットすると設定される変数

        Debug.DrawRay(target, direction * -distance, Color.green);

        /*
         layer8 bullet
         layer9 search
         layer10 camline
         layer11 melee
         */

        int layerMask = 1 << 8 | 1 << 10 | 1 << 11|1<<2;//ここでは８レイヤ(弾)と９レイヤ（テキ視界）とだけ衝突する　さらに１１（近接）
        layerMask = ~layerMask;//弾自身と、視界、それから１１を無視する

        // ターゲットからカメラに向かってレイキャストを飛ばし、途中で遮るものがあったらその情報を返す
        if (Physics.Raycast(target, direction, out hit, -distance, layerMask, QueryTriggerInteraction.UseGlobal))
        {
            //  途中で遮るものがあったとき、そこまでの距離から若干引いて返す
            return hit.distance - 0.5f; // Closer Radius
        }
        else
        {
            // 遮るものがないなら、そのままカメラとターゲットの距離を返す
            return -distance;
        }
    }

    public void Config3dCamSpeed() { camspeed = slider.value; }
    public void Config3dCamXReverse() { Xreverse = Xtoggle.isOn; }
    public void Config3dCamYReverse() { Yreverse = Ytoggle.isOn; }

    public void Load3dcamSpeed(float loadcamspeed) { camspeed = loadcamspeed; }
    public void Load3dCamXReverse(bool loadX) { Xreverse = loadX; }
    public void Load3dCamYReverse(bool loadY) { Yreverse = loadY; }

    public void Recoil(float gunrecoil)
    {
        gunrecoilvar = gunrecoil;
        recoilstart = true;
    }


}



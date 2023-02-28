using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PivotControll : MonoBehaviour
{
    // カメラが追従する対象となる GameObject：Inspectorでキャラのオブジェクトをここに指定
    public Transform target;    //pivot

    //オフセット調整用

    public float Switcher;

    private float karioki;

    public float zoompos;

    public float pivposx;
    public float pivposy;
    public float pivposz;


    // カメラが追従する対象からのオフセット：キャラの中心は足元にあるため1.5m上に調整している
    private Vector3 targetOffset = new Vector3(0.0f, 2.5f, 0.0f);   //pivot

    // カメラの視点の角度
    private float x = 0.0f;
    private float y = 0.0f;

    //private float Switcher = 1.0f;
    private float distanceVelocity = 0.0f;


    private bool poseswitch = false;


    //コンフィグ用
    [Header("カメラの移動する速度。原動力！０はダメ")]
    public float camspeed;
    [Header("コンフィグ用")]
    public Slider slider;
    public Toggle Xtoggle;
    public Toggle Ytoggle;
    [SerializeField] public bool Xreverse;
    [SerializeField] public bool Yreverse;




    //リコイル処理用変数
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
        karioki = Switcher;

        targetOffset = new Vector3(pivposx, pivposy, pivposz);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Setposeswitch(bool iposenow)
    {
        poseswitch = iposenow;
    }

    // Update is called once per frame
    void Update()
    {
        if (poseswitch == false&&Time.timeScale!=0)
        {

            if (Input.GetMouseButton(1))   // && Switcher == 1
            {
                //Switcher = -1.0f;
                Switcher = Mathf.SmoothDamp(Switcher, zoompos, ref distanceVelocity, 0.06f);

            }
            else //if (Input.GetMouseButtonDown(1)) //&&Switcher ==-1
            {
                //Switcher = 1.0f;
                Switcher = Mathf.SmoothDamp(Switcher, karioki, ref distanceVelocity, 0.06f);
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
            // カメラのY方向に制限を加える   //pivot 次回ここから
            y = Mathf.Clamp(y, -50, 80);

            // 入力された値から視点の角度を計算する
            Quaternion rotation = Quaternion.Euler(y, x, 0);    //pivot

            // ターゲットオブジェクトの中心点にオフセットを足したものをターゲットとする：キャラの頭辺りになる
            Vector3 targetPos = target.position + targetOffset; //oivot

            // 視点の角度から方向を求める
            Vector3 direction = rotation * Vector3.right * Switcher;    //pivot

            // カメラの角度を設定
            transform.rotation = rotation;  //pivot

            // カメラの位置を設定
            transform.position = targetPos + direction;   //pivot //間にDirectionタス



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
    }

    public void ConfigCamSpeed() { camspeed = slider.value; }
    public void ConfigCamXReverse() { Xreverse = Xtoggle.isOn; }
    public void ConfigCamYReverse() { Yreverse = Ytoggle.isOn; }

    public void LoadCamSpeed(float loadspeed) { camspeed = loadspeed; }
    public void LoadCamSlider(float loadspeed) { slider.value = loadspeed; }
    public void LoadCamXReverse(bool camrev) { Xreverse = camrev; }
    public void LoadCamYReverse(bool camrev) { Yreverse = camrev; }
    public void LoadCamXToggle(bool camrev) { Xtoggle.isOn = camrev; }
    public void LoadCamYToggle(bool camrev) { Ytoggle.isOn = camrev; }

    public void Recoil(float gunrecoil)
    {
        gunrecoilvar = gunrecoil;
        recoilstart = true;
    }
}

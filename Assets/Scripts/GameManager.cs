using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Debug = UnityEngine.Debug;

public class GameManager : MonoBehaviour
{
    //TextScoreのオブジェクト
    private GameObject textObject;

    //TextHighのオブジェクト
    public GameObject highObject;

    private UnityEngine.UI.Text scoreText;

    private UnityEngine.UI.Text highText;

    public static int Score = 0;

    public static int highScore = 0;

    private float timeleft;


    private GameObject player;

    private Player playerP;

    private Animator playerAnim;

    private GameObject buttonPause;

    private GameObject buttonStart;

    private GameObject pauseImageObject;

    private Image pauseImage;

    private GameObject pauseTextObject;

    private UnityEngine.UI.Text pauseTextComponent;

    private GameObject gameOverTextObject;

    private UnityEngine.UI.Text gameOverTextComponent;

    private GameObject buttonRetry;

    private GameObject buttonResult;

    private int retryCount = 0;

    private GameObject touchTextObject;

    private UnityEngine.UI.Text touchTextComponent;

    private bool touchstart = false;

    //ポーズ中華判定する変数
    public bool isPosed = false;

    // Start is called before the first frame update
    void Start()
    {
        //BGMとSE取得、BGM再生
        Sound.LoadBgm("gamebgm", "bgmpayapaya");
        //Sound.LoadSe("lose", "lose");
        Sound.PlayBgm("gamebgm");
        Sound.LoadSe("tap", "tap");
        Sound.LoadSe("tap2", "tap2");


        //プレイやーのオブジェクトと、Playerスクリプト取得、アニメ取得
        player = GameObject.Find("Player");
        playerP = player.GetComponent<Player>();
        playerAnim = player.GetComponent<Animator>();

        //テキスト呼び出し、テキスト割り当て
        textObject = GameObject.Find("TextScore");
        scoreText = textObject.GetComponent<UnityEngine.UI.Text>();
        scoreText.text = "スコア:00000m";

        //ポーズボタン呼び出し
        buttonPause = GameObject.Find("ButtonPause");

        //スタートボタン呼び出し、最初は非表示
        buttonStart = GameObject.Find("ButtonStart");
        buttonStart.SetActive(false);

        //リトライボタン呼び出し、最初は非表示
        buttonRetry = GameObject.Find("RetryButton");
        buttonRetry.SetActive(false);

        //リザルトボタン呼び出し、最初は非表示
        buttonResult = GameObject.Find("ResultButton");
        buttonResult.SetActive(false);

        //ポーズイメージ呼び出し、割り当て、最初は非表示
        pauseImageObject = GameObject.Find("PauseImage");
        pauseImage = pauseImageObject.GetComponent<Image>();
        pauseImage.enabled = false;

        //ポーズテキスト呼び出し、テキスト割り当て、最初は非表示
        pauseTextObject = GameObject.Find("PauseText");
        pauseTextComponent = pauseTextObject.GetComponent<UnityEngine.UI.Text>();
        pauseTextComponent.enabled = false;

        //ゲームオーバーテキストの呼び出し、最初は非表示
        gameOverTextObject = GameObject.Find("GameOverText");
        gameOverTextComponent = gameOverTextObject.GetComponent<UnityEngine.UI.Text>();
        gameOverTextComponent.enabled = false;

        touchTextObject = GameObject.Find("TouchText");
        touchTextComponent = touchTextObject.GetComponent<UnityEngine.UI.Text>();
        touchTextComponent.enabled = false;

        //ハイスコアのテキスト呼び出しと割り当て
        //highObject = GameObject.Find("TextHigh");
        highText = highObject.GetComponent<UnityEngine.UI.Text>();
        //highText.text = "最高:00000m";

        //保存してるハイスコア呼び出し
        highScore = PlayerPrefs.GetInt("SCORE", 0);

        //ハイスコア出す
        highText.text = highScore.ToString("最高:00000m");

       

    }

    // Update is called once per frame
    void Update()
    {
        //何もしてない状態なら動かないようにする
        playerP.xr = 0;
        playerP.xl = 0;
        //エディタの時
        if (Application.isEditor)
        {
            if (Input.GetMouseButton(0))
            {
                //マウスの座標取得、ｚ座標は０に
                Vector3 mouse = Input.mousePosition;
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(mouse);
                mousePosition.z = 0f;

                //マウスのｘ座標が画面右の時
                if (mousePosition.x > 0f)
                {
                    Debug.Log("右側クリック");
                    playerP.xr = 1.0f;
                }
                //マウスのｘ座標が画面左の時
                else if (mousePosition.x < 0f)
                {
                    Debug.Log("左側クリック");
                    playerP.xl = -1.0f;
                }
            }

        }
        //実機の時
        else
        {
            if (Input.touchCount > 0)
            {
                //タッチの座標取得、ｚ座標は０に
                Touch touch = Input.GetTouch(0);
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                touchPosition.z = 0f;

                //タッチのｘ座標が画面右の時
                if (touchPosition.x > 0f)
                {
                    Debug.Log("右側タッチ");
                    playerP.xr = 1.0f;
                }
                //タッチのｘ座標が画面左の時
                else if (touchPosition.x < 0f)
                {
                    Debug.Log("左側タッチ");
                    playerP.xl = -1.0f;
                }
            }
        }

        //プレイヤーが生きている間
        if (!playerP.isDead)
        {
            //0.4秒ごとに動作
            timeleft -= Time.deltaTime;
            if (timeleft <= 0.0)
            {
                timeleft = 0.02f;

                //スコアを１づつ増やす
                Score += 1;
                scoreText.text = Score.ToString("スコア:00000m");

                //スコアがハイスコアを超えたらHighScoreRecord呼び出し
                if (highScore < Score)
                {

                    highScore = Score;

                    //"SCORE"をキーとして、ハイスコアを保存
                    PlayerPrefs.SetInt("SCORE", highScore);
                    //ディスクへの書き込み
                    PlayerPrefs.Save();

                    //ハイスコアを増やす
                    highText.text = highScore.ToString("最高:00000m");

                }

            }
        }

        if (touchstart)
        {
            //エディタ
            if (Application.isEditor)
            {

                if (Input.GetMouseButtonDown(0))
                {

                    touchTextComponent.enabled = false;

                    Sound.PlaySe("tap");

                    //コライダー復活
                    player.GetComponent<CircleCollider2D>().enabled = true;
                    player.GetComponent<BoxCollider2D>().enabled = true;

                    //アイドルアニメーションにする
                    playerAnim.SetBool("isLive", true);

                    //BGM再生
                    Sound.PlayBgm("gamebgm");

                    //リトライした分カウントを増やす
                    retryCount++;

                    touchstart = false;

                    StartCoroutine("TouchDelay");

                }

                //実機
            }
            else
            {

                if (Input.touchCount > 0)
                {
                    //タッチ情報の取得
                    Touch touch = Input.GetTouch(0);

                    if (touch.phase == TouchPhase.Began)
                    {

                        touchTextComponent.enabled = false;

                        Sound.PlaySe("tap");

                        //コライダー復活
                        player.GetComponent<CircleCollider2D>().enabled = true;
                        player.GetComponent<BoxCollider2D>().enabled = true;

                        //アイドルアニメーションにする
                        playerAnim.SetBool("isLive", true);

                        //BGM再生
                        Sound.PlayBgm("gamebgm");

                        //リトライした分カウントを増やす
                        retryCount++;

                        touchstart = false;

                        StartCoroutine("TouchDelay");

                    }

                }

            }
        }

    }

    public void Pause()
    {
        //プレイヤーが生きている間
        if (!playerP.isDead) {
            Time.timeScale = 0; //時間停止

            Sound.PlaySe("tap");

            //ポーズボタン非表示、スタートボタン表示
            buttonPause.SetActive(false);
            buttonStart.SetActive(true);

            //ポーズイメージとポーズテキスト表示
            pauseImage.enabled = true;
            pauseTextComponent.enabled = true;

            //ポーズ中
            isPosed = true;

        }
    }

    public void StartButton()
    {
        //プレイヤーが生きている間
        if (!playerP.isDead)
        {
            Time.timeScale = 1; //時間再開

            Sound.PlaySe("tap");


            //ポーズボタン表示、スタートボタン非表示
            buttonStart.SetActive(false);
            buttonPause.SetActive(true);

            //ポーズイメージとポーズテキスト非表示
            pauseImage.enabled = false;
            pauseTextComponent.enabled = false;

            //ポーズ解除
            isPosed = false;
        }
    }

    //ゲームオーバーコルーチン
    IEnumerator GameOver()
    {

        Sound.StopBgm();

        yield return new WaitForSeconds(1.0f);

        //Sound.PlaySe("lose");

        //ポーズイメージ表示
        pauseImage.enabled = true;

        //ゲームオーバーテキスト表示
        gameOverTextComponent.enabled = true;

        //リトライ回数が3回以内ならリトライボタン表示
        if(retryCount < 3)
        {
            buttonRetry.SetActive(true);
        }

        //リザルト表示
        buttonResult.SetActive(true);

    }

    IEnumerator TouchDelay()
    {
        yield return new WaitForSeconds(0.5f);

        Debug.Log("コルーチンスタート");
        //プレイヤー復活
        playerP.isDead = false;

    }

    public void RetryButton()
    {
        //リトライ3回まで
        if(retryCount < 3)
        {

            touchstart = true;
            Debug.Log("touch : " + touchstart);

            //ゲームオーバー画面非表示
            gameOverTextComponent.enabled = false;
            buttonRetry.SetActive(false);
            buttonResult.SetActive(false);
            touchTextComponent.enabled = true;
            pauseImage.enabled = false;

            //プレイヤー表示
            player.SetActive(true);

        }
    }

    public void ResultButton()
    {

        Sound.PlaySe("tap2");

        //リザルト画面へ
        SceneManager.LoadScene("Scene_Result");

    }

}

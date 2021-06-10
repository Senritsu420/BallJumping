using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Debug = UnityEngine.Debug;

public class Player : MonoBehaviour
{

    public float speed = 30.0f;

    private Rigidbody2D rb2d;

    private SpriteRenderer spRenderer;

    private Animator anim;

    //右方向のx
    public float xr = 0;
    //左方向のy
    public float xl = 0;

    //プレイヤー死亡判定
    public bool isDead = false;

    private GameObject game;

    private GameManager gameM;

    // Start is called before the first frame update
    void Start()
    {
        this.rb2d = this.GetComponent<Rigidbody2D>();
        this.anim = GetComponent<Animator>();
        this.spRenderer = GetComponent<SpriteRenderer>();

        //GameManager呼び出し
        game = GameObject.Find("GameManager");
        gameM = game.GetComponent<GameManager>();

        Sound.LoadSe("bannn", "bannn");
        Sound.LoadSe("lose", "lose");

        //生きている状態
        anim.SetBool("isLive", true);

    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーが死亡してない間
        if (!isDead & !gameM.isPosed) {
            //右方向に動かす
            if (xr != 0 & this.transform.position.x > -2.5)
            {

                //x座標が2以上になったら速度を0にする
                if (this.transform.position.x >= 2) {
                    rb2d.velocity = new Vector2(0, 0);
                } else { //x座標が-2.2以上2未満の時

                    //力を加える
                    rb2d.AddForce(Vector2.right * xr * speed);

                }
            }
            //左方向に動かす
            else if (xl != 0 & this.transform.position.x < 2.5) {

                //x座標が-2以下になったら速度を0にする
                if (this.transform.position.x <= -2)
                {
                    rb2d.velocity = new Vector2(0, 0);
                } else { //x座標が-2以上2.2未満の時

                    //力を加える
                    rb2d.AddForce(Vector2.right * xl * speed);

                }

            } else {
                //0の時は速度を0にする
                rb2d.velocity = new Vector2(0, 0);

            }
        }

        //速度ベクトルx,y
        float velX = rb2d.velocity.x;
        float velY = rb2d.velocity.y;

        //速度が5以上なら速度を5にする
        if (Mathf.Abs(velX) > 8)
        {
            if (velX > 8.0f)
            {
                rb2d.velocity = new Vector2(8.0f, velY);
            }
            if (velX < -8.0f)
            {
                rb2d.velocity = new Vector2(-8.0f, velY);
            }
        }

        //ポーズ中でないとき
        /*if (!gameM.isPosed) {
            //スプライトの向きを変える
            if (xr != 0)
            {
                spRenderer.flipX = true;
            }
            else if (xl != 0)
            {
                spRenderer.flipX = false;
            }
        }*/

        if (gameM.isPosed)
        {
            //Rigidbodyを停止
            rb2d.velocity = new Vector2(0, 0);
            anim.enabled = false;
        } else {
            anim.enabled = true;

            //スプライトの向きを変える
            if (xr != 0)
            {
                spRenderer.flipX = true;
            }
            else if (xl != 0)
            {
                spRenderer.flipX = false;
            }
        }

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Enemy")
        {
            StartCoroutine("Dead");
            gameM.StartCoroutine("GameOver");
        }
    }

    //死亡時のコルーチン
    IEnumerator Dead()
    {
        //プレイヤー死亡、速度0
        isDead = true;
        rb2d.velocity = new Vector2(0, 0);

        //コライダーなくす
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;

        //トリガー発動、SE再生
        anim.SetBool("isLive", false);
        anim.SetTrigger("TriggerDead");
        Sound.PlaySe("bannn");

        yield return new WaitForSeconds(0.5f);

        this.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        Sound.PlaySe("lose");

    }

}

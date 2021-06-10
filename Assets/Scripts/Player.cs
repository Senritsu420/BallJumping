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

    //�E������x
    public float xr = 0;
    //��������y
    public float xl = 0;

    //�v���C���[���S����
    public bool isDead = false;

    private GameObject game;

    private GameManager gameM;

    // Start is called before the first frame update
    void Start()
    {
        this.rb2d = this.GetComponent<Rigidbody2D>();
        this.anim = GetComponent<Animator>();
        this.spRenderer = GetComponent<SpriteRenderer>();

        //GameManager�Ăяo��
        game = GameObject.Find("GameManager");
        gameM = game.GetComponent<GameManager>();

        Sound.LoadSe("bannn", "bannn");
        Sound.LoadSe("lose", "lose");

        //�����Ă�����
        anim.SetBool("isLive", true);

    }

    // Update is called once per frame
    void Update()
    {
        //�v���C���[�����S���ĂȂ���
        if (!isDead & !gameM.isPosed) {
            //�E�����ɓ�����
            if (xr != 0 & this.transform.position.x > -2.5)
            {

                //x���W��2�ȏ�ɂȂ����瑬�x��0�ɂ���
                if (this.transform.position.x >= 2) {
                    rb2d.velocity = new Vector2(0, 0);
                } else { //x���W��-2.2�ȏ�2�����̎�

                    //�͂�������
                    rb2d.AddForce(Vector2.right * xr * speed);

                }
            }
            //�������ɓ�����
            else if (xl != 0 & this.transform.position.x < 2.5) {

                //x���W��-2�ȉ��ɂȂ����瑬�x��0�ɂ���
                if (this.transform.position.x <= -2)
                {
                    rb2d.velocity = new Vector2(0, 0);
                } else { //x���W��-2�ȏ�2.2�����̎�

                    //�͂�������
                    rb2d.AddForce(Vector2.right * xl * speed);

                }

            } else {
                //0�̎��͑��x��0�ɂ���
                rb2d.velocity = new Vector2(0, 0);

            }
        }

        //���x�x�N�g��x,y
        float velX = rb2d.velocity.x;
        float velY = rb2d.velocity.y;

        //���x��5�ȏ�Ȃ瑬�x��5�ɂ���
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

        //�|�[�Y���łȂ��Ƃ�
        /*if (!gameM.isPosed) {
            //�X�v���C�g�̌�����ς���
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
            //Rigidbody���~
            rb2d.velocity = new Vector2(0, 0);
            anim.enabled = false;
        } else {
            anim.enabled = true;

            //�X�v���C�g�̌�����ς���
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

    //���S���̃R���[�`��
    IEnumerator Dead()
    {
        //�v���C���[���S�A���x0
        isDead = true;
        rb2d.velocity = new Vector2(0, 0);

        //�R���C�_�[�Ȃ���
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;

        //�g���K�[�����ASE�Đ�
        anim.SetBool("isLive", false);
        anim.SetTrigger("TriggerDead");
        Sound.PlaySe("bannn");

        yield return new WaitForSeconds(0.5f);

        this.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        Sound.PlaySe("lose");

    }

}

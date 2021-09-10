using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Debug = UnityEngine.Debug;

public class GameManager : MonoBehaviour
{
    //TextScore�̃I�u�W�F�N�g
    private GameObject textObject;

    //TextHigh�̃I�u�W�F�N�g
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

    //�|�[�Y���ؔ��肷��ϐ�
    public bool isPosed = false;

    // Start is called before the first frame update
    void Start()
    {
        //BGM��SE�擾�ABGM�Đ�
        Sound.LoadBgm("gamebgm", "bgmpayapaya");
        //Sound.LoadSe("lose", "lose");
        Sound.PlayBgm("gamebgm");
        Sound.LoadSe("tap", "tap");
        Sound.LoadSe("tap2", "tap2");


        //�v���C��[�̃I�u�W�F�N�g�ƁAPlayer�X�N���v�g�擾�A�A�j���擾
        player = GameObject.Find("Player");
        playerP = player.GetComponent<Player>();
        playerAnim = player.GetComponent<Animator>();

        //�e�L�X�g�Ăяo���A�e�L�X�g���蓖��
        textObject = GameObject.Find("TextScore");
        scoreText = textObject.GetComponent<UnityEngine.UI.Text>();
        scoreText.text = "�X�R�A:00000m";

        //�|�[�Y�{�^���Ăяo��
        buttonPause = GameObject.Find("ButtonPause");

        //�X�^�[�g�{�^���Ăяo���A�ŏ��͔�\��
        buttonStart = GameObject.Find("ButtonStart");
        buttonStart.SetActive(false);

        //���g���C�{�^���Ăяo���A�ŏ��͔�\��
        buttonRetry = GameObject.Find("RetryButton");
        buttonRetry.SetActive(false);

        //���U���g�{�^���Ăяo���A�ŏ��͔�\��
        buttonResult = GameObject.Find("ResultButton");
        buttonResult.SetActive(false);

        //�|�[�Y�C���[�W�Ăяo���A���蓖�āA�ŏ��͔�\��
        pauseImageObject = GameObject.Find("PauseImage");
        pauseImage = pauseImageObject.GetComponent<Image>();
        pauseImage.enabled = false;

        //�|�[�Y�e�L�X�g�Ăяo���A�e�L�X�g���蓖�āA�ŏ��͔�\��
        pauseTextObject = GameObject.Find("PauseText");
        pauseTextComponent = pauseTextObject.GetComponent<UnityEngine.UI.Text>();
        pauseTextComponent.enabled = false;

        //�Q�[���I�[�o�[�e�L�X�g�̌Ăяo���A�ŏ��͔�\��
        gameOverTextObject = GameObject.Find("GameOverText");
        gameOverTextComponent = gameOverTextObject.GetComponent<UnityEngine.UI.Text>();
        gameOverTextComponent.enabled = false;

        touchTextObject = GameObject.Find("TouchText");
        touchTextComponent = touchTextObject.GetComponent<UnityEngine.UI.Text>();
        touchTextComponent.enabled = false;

        //�n�C�X�R�A�̃e�L�X�g�Ăяo���Ɗ��蓖��
        //highObject = GameObject.Find("TextHigh");
        highText = highObject.GetComponent<UnityEngine.UI.Text>();
        //highText.text = "�ō�:00000m";

        //�ۑ����Ă�n�C�X�R�A�Ăяo��
        highScore = PlayerPrefs.GetInt("SCORE", 0);

        //�n�C�X�R�A�o��
        highText.text = highScore.ToString("�ō�:00000m");

       

    }

    // Update is called once per frame
    void Update()
    {
        //�������ĂȂ���ԂȂ瓮���Ȃ��悤�ɂ���
        playerP.xr = 0;
        playerP.xl = 0;
        //�G�f�B�^�̎�
        if (Application.isEditor)
        {
            if (Input.GetMouseButton(0))
            {
                //�}�E�X�̍��W�擾�A�����W�͂O��
                Vector3 mouse = Input.mousePosition;
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(mouse);
                mousePosition.z = 0f;

                //�}�E�X�̂����W����ʉE�̎�
                if (mousePosition.x > 0f)
                {
                    Debug.Log("�E���N���b�N");
                    playerP.xr = 1.0f;
                }
                //�}�E�X�̂����W����ʍ��̎�
                else if (mousePosition.x < 0f)
                {
                    Debug.Log("�����N���b�N");
                    playerP.xl = -1.0f;
                }
            }

        }
        //���@�̎�
        else
        {
            if (Input.touchCount > 0)
            {
                //�^�b�`�̍��W�擾�A�����W�͂O��
                Touch touch = Input.GetTouch(0);
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                touchPosition.z = 0f;

                //�^�b�`�̂����W����ʉE�̎�
                if (touchPosition.x > 0f)
                {
                    Debug.Log("�E���^�b�`");
                    playerP.xr = 1.0f;
                }
                //�^�b�`�̂����W����ʍ��̎�
                else if (touchPosition.x < 0f)
                {
                    Debug.Log("�����^�b�`");
                    playerP.xl = -1.0f;
                }
            }
        }

        //�v���C���[�������Ă����
        if (!playerP.isDead)
        {
            //0.4�b���Ƃɓ���
            timeleft -= Time.deltaTime;
            if (timeleft <= 0.0)
            {
                timeleft = 0.02f;

                //�X�R�A���P�Â��₷
                Score += 1;
                scoreText.text = Score.ToString("�X�R�A:00000m");

                //�X�R�A���n�C�X�R�A�𒴂�����HighScoreRecord�Ăяo��
                if (highScore < Score)
                {

                    highScore = Score;

                    //"SCORE"���L�[�Ƃ��āA�n�C�X�R�A��ۑ�
                    PlayerPrefs.SetInt("SCORE", highScore);
                    //�f�B�X�N�ւ̏�������
                    PlayerPrefs.Save();

                    //�n�C�X�R�A�𑝂₷
                    highText.text = highScore.ToString("�ō�:00000m");

                }

            }
        }

        if (touchstart)
        {
            //�G�f�B�^
            if (Application.isEditor)
            {

                if (Input.GetMouseButtonDown(0))
                {

                    touchTextComponent.enabled = false;

                    Sound.PlaySe("tap");

                    //�R���C�_�[����
                    player.GetComponent<CircleCollider2D>().enabled = true;
                    player.GetComponent<BoxCollider2D>().enabled = true;

                    //�A�C�h���A�j���[�V�����ɂ���
                    playerAnim.SetBool("isLive", true);

                    //BGM�Đ�
                    Sound.PlayBgm("gamebgm");

                    //���g���C�������J�E���g�𑝂₷
                    retryCount++;

                    touchstart = false;

                    StartCoroutine("TouchDelay");

                }

                //���@
            }
            else
            {

                if (Input.touchCount > 0)
                {
                    //�^�b�`���̎擾
                    Touch touch = Input.GetTouch(0);

                    if (touch.phase == TouchPhase.Began)
                    {

                        touchTextComponent.enabled = false;

                        Sound.PlaySe("tap");

                        //�R���C�_�[����
                        player.GetComponent<CircleCollider2D>().enabled = true;
                        player.GetComponent<BoxCollider2D>().enabled = true;

                        //�A�C�h���A�j���[�V�����ɂ���
                        playerAnim.SetBool("isLive", true);

                        //BGM�Đ�
                        Sound.PlayBgm("gamebgm");

                        //���g���C�������J�E���g�𑝂₷
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
        //�v���C���[�������Ă����
        if (!playerP.isDead) {
            Time.timeScale = 0; //���Ԓ�~

            Sound.PlaySe("tap");

            //�|�[�Y�{�^����\���A�X�^�[�g�{�^���\��
            buttonPause.SetActive(false);
            buttonStart.SetActive(true);

            //�|�[�Y�C���[�W�ƃ|�[�Y�e�L�X�g�\��
            pauseImage.enabled = true;
            pauseTextComponent.enabled = true;

            //�|�[�Y��
            isPosed = true;

        }
    }

    public void StartButton()
    {
        //�v���C���[�������Ă����
        if (!playerP.isDead)
        {
            Time.timeScale = 1; //���ԍĊJ

            Sound.PlaySe("tap");


            //�|�[�Y�{�^���\���A�X�^�[�g�{�^����\��
            buttonStart.SetActive(false);
            buttonPause.SetActive(true);

            //�|�[�Y�C���[�W�ƃ|�[�Y�e�L�X�g��\��
            pauseImage.enabled = false;
            pauseTextComponent.enabled = false;

            //�|�[�Y����
            isPosed = false;
        }
    }

    //�Q�[���I�[�o�[�R���[�`��
    IEnumerator GameOver()
    {

        Sound.StopBgm();

        yield return new WaitForSeconds(1.0f);

        //Sound.PlaySe("lose");

        //�|�[�Y�C���[�W�\��
        pauseImage.enabled = true;

        //�Q�[���I�[�o�[�e�L�X�g�\��
        gameOverTextComponent.enabled = true;

        //���g���C�񐔂�3��ȓ��Ȃ烊�g���C�{�^���\��
        if(retryCount < 3)
        {
            buttonRetry.SetActive(true);
        }

        //���U���g�\��
        buttonResult.SetActive(true);

    }

    IEnumerator TouchDelay()
    {
        yield return new WaitForSeconds(0.5f);

        Debug.Log("�R���[�`���X�^�[�g");
        //�v���C���[����
        playerP.isDead = false;

    }

    public void RetryButton()
    {
        //���g���C3��܂�
        if(retryCount < 3)
        {

            touchstart = true;
            Debug.Log("touch : " + touchstart);

            //�Q�[���I�[�o�[��ʔ�\��
            gameOverTextComponent.enabled = false;
            buttonRetry.SetActive(false);
            buttonResult.SetActive(false);
            touchTextComponent.enabled = true;
            pauseImage.enabled = false;

            //�v���C���[�\��
            player.SetActive(true);

        }
    }

    public void ResultButton()
    {

        Sound.PlaySe("tap2");

        //���U���g��ʂ�
        SceneManager.LoadScene("Scene_Result");

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using NCMB;


using Debug = UnityEngine.Debug;

public class ResultManager : MonoBehaviour
{

    private GameObject scoreObject;

    private UnityEngine.UI.Text scoreComponent;

    private GameObject highObject;

    private UnityEngine.UI.Text highComponent;

    private GameObject inputObject;

    private InputField inputComponent;

    public static string name;

    public static string selfID;

    private GameObject rankingObject;

    private UnityEngine.UI.Text rankingText;

    private GameObject warningObject;

    private UnityEngine.UI.Text warningText;

    //name�������������ʂ���ϐ�
    private bool nameOK = true;

    // Start is called before the first frame update
    void Start()
    {

        Sound.LoadSe("tap2", "tap2");

        //�ۑ����Ă�n�C�X�R�A�Ăяo��
        GameManager.highScore = PlayerPrefs.GetInt("SCORE", 0);

        scoreObject = GameObject.Find("ScoreText");
        scoreComponent = scoreObject.GetComponent<UnityEngine.UI.Text>();
        scoreComponent.text = GameManager.Score.ToString("�X�R�A:00000m");

        //�n�C�X�R�A���b�Z�[�W���ŏ��͔�\��
        highObject = GameObject.Find("HighText");
        highComponent = highObject.GetComponent<UnityEngine.UI.Text>();
        highComponent.enabled = false;

        //���ӂ��郁�b�Z�[�W���ŏ��͔�\��
        warningObject = GameObject.Find("WarningText");
        warningText = warningObject.GetComponent<UnityEngine.UI.Text>();
        warningText.enabled = false;


        //�����L���O���b�Z�[�W�擾
        rankingObject = GameObject.Find("RankingText");
        rankingText = rankingObject.GetComponent<UnityEngine.UI.Text>();

        //InputField��ǂݍ���
        inputObject = GameObject.Find("InputField");
        inputComponent = inputObject.GetComponent<InputField>();

        //�n�C�X�R�A�ƃX�R�A�������Ƃ��̂ݕ\��
        if (GameManager.Score == GameManager.highScore)
        {
            highComponent.enabled = true;
        }

        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("HighScore");

        //score�t�B�[���h�̍~���Ńf�[�^���擾
        query.OrderByDescending("score");

        //����������5���ɐݒ�
        query.Limit = 5;

        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {

            //��������������
            if (e == null)
            {
                for (int i = 0; i < 5; i++)
                {
                    rankingText.text += (i + 1) + ":" + objList[i]["name"] + "   " + objList[i]["score"] + "m" + "\n";

                }
            } else {
                UnityEngine.Debug.Log("�����L���O�擾���s");
            }
        });

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�^�C�g����
    public void GoToTitleButton()
    {
        Sound.PlaySe("tap2");
        SceneManager.LoadScene("Scene_Title");
    }

    //InputField�ɓ��͂��ꂽ���e�𑗐M
    public void EndInputText()
    {
        Sound.PlaySe("tap2");

        name = inputComponent.text;
        int len = name.Length;
        Debug.Log(name);
        inputComponent.text = "";

        NCMBObject obj = new NCMBObject("HighScore");
        //�I�u�W�F�N�g�ɒl��ݒ�
        if (name == null | name == "")
        {
            //���O����Ȃ�guest�ɂ���
            name = "guest";
            obj["name"] = name;
            nameOK = true;

        }
        else {
            //name�̒�����5�����ȓ�
            if (len < 6) {
                //���ꂽ���O��o�^
                obj["name"] = name;
                nameOK = true;

                //6�����ȏ�
            }
            else {
                //name���������Ȃ�
                nameOK = false;

                //warningText�\��
                warningText.enabled = true;

            }
        }

        if (nameOK) {
            obj["score"] = GameManager.Score;
            //�f�[�^�X�g�A�ւ̓o�^
            obj.SaveAsync((NCMBException e) => {
                if (e != null)
                {
                    //�G���[����
                    UnityEngine.Debug.Log("�����L���O�o�^���s");
                }
                else
                {
                    //�������̏���
                    selfID = obj.ObjectId;
                    Debug.Log(selfID);
                    SceneManager.LoadScene("Scene_Ranking");
                }
            });
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;
using NCMB;

using Debug = UnityEngine.Debug;

public class RankingManager : MonoBehaviour
{

    private GameObject textObject;

    private UnityEngine.UI.Text rankingText;

    private GameObject myScoreObject;

    private UnityEngine.UI.Text myScoreText;

    // Start is called before the first frame update
    void Start()
    {

        Sound.LoadSe("tap2", "tap2");

        var scrollView = GameObject.Find("Scroll View");

        var viewPort = scrollView.transform.Find("Viewport");
        Transform content = viewPort.transform.Find("Content");

        textObject = GameObject.Find("RankingText");
        rankingText = textObject.GetComponent<UnityEngine.UI.Text>();

        //�ۑ����Ă�n�C�X�R�A�Ăяo��
        GameManager.highScore = PlayerPrefs.GetInt("SCORE", 0);

        myScoreObject = GameObject.Find("MyScoreText");
        myScoreText = myScoreObject.GetComponent<UnityEngine.UI.Text>();
        myScoreText.text = GameManager.highScore.ToString("�n�C�X�R�A:00000m");


        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("HighScore");

        //score�t�B�[���h�̍~���Ńf�[�^���擾
        query.OrderByDescending("score");

        //����������5���ɐݒ�
        query.Limit = 50;

        //���Contents�쐬���Č��h�����悭����
        GameObject Contents = new GameObject("Contents" + 0.ToString());

        Contents.transform.parent = content.transform;
        var Rect = Contents.AddComponent<RectTransform>();
        Rect.transform.localPosition = new Vector2(4, 0);
        Rect.transform.localScale = new Vector2(1.8f, 0.7f);
        Rect.sizeDelta = new Vector2(0, 20);

        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {

            //��������������
            if (e == null)
            {
                for (int i = 0; i < 50; i++)
                {

                    GameObject Contents = new GameObject("Contents" + (i + 1).ToString());

                    Contents.transform.parent = content.transform;
                    var Rect = Contents.AddComponent<RectTransform>();
                    Rect.transform.localPosition = new Vector2(4, 0);
                    Rect.transform.localScale = new Vector2(1.8f, 0.7f);
                    Rect.sizeDelta = new Vector2(0, 40);

                    Contents.AddComponent<CanvasRenderer>();
                    Contents.AddComponent<Image>();
                    Contents.AddComponent<LayoutElement>().preferredHeight = 100;


                    GameObject text = new GameObject("Text" + (i + 1).ToString());
                    text.transform.parent = Contents.transform;
                    var rect = text.AddComponent<RectTransform>();
                    rect.transform.localPosition = new Vector2(4, 0);
                    rect.transform.localScale = new Vector2(0.28f, 1.3f);
                    rect.sizeDelta = new Vector2(800, 80);

                    text.AddComponent<CanvasRenderer>();


                    // Asset/Font�̉��ɂ���t�H���g�������i�Ⴄ�t�H���_�ɓ���Ă���ꍇ�͕ύX�j
                    string[] fonts = AssetDatabase.FindAssets("t:Font", new string[] { "Assets/Font" });

                    //Font�t�H���_���̈�Ԗڂ̃t�H���g��font�ɓ����
                    Font font = AssetDatabase.LoadAssetAtPath<Font>(AssetDatabase.GUIDToAssetPath(fonts[0]));

                    var textChild = text.AddComponent<Text>();
                    textChild.text = "                                      " +  (i + 1) + ":" + objList[i]["name"] + "   " + objList[i]["score"] + "m";
                    //textChild.alignment = TextAnchor.MiddleCenter;
                    textChild.fontSize = 30;

                    textChild.font = font;

                    //textChild.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font; ;

                    if (objList[i].ObjectId == ResultManager.selfID)
                    {

                        textChild.color = Color.red;

                    } else {

                        textChild.color = Color.black;

                    }
                    //rankingText.text += (i + 1) + ":" + objList[i]["name"] + "   " + objList[i]["score"] + "\n";
                }
            }
            else
            {
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

}

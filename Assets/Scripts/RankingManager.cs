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

        //保存してるハイスコア呼び出し
        GameManager.highScore = PlayerPrefs.GetInt("SCORE", 0);

        myScoreObject = GameObject.Find("MyScoreText");
        myScoreText = myScoreObject.GetComponent<UnityEngine.UI.Text>();
        myScoreText.text = GameManager.highScore.ToString("ハイスコア:00000m");


        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("HighScore");

        //scoreフィールドの降順でデータを取得
        query.OrderByDescending("score");

        //検索件数を5件に設定
        query.Limit = 50;

        //空のContents作成して見栄えをよくする
        GameObject Contents = new GameObject("Contents" + 0.ToString());

        Contents.transform.parent = content.transform;
        var Rect = Contents.AddComponent<RectTransform>();
        Rect.transform.localPosition = new Vector2(4, 0);
        Rect.transform.localScale = new Vector2(1.8f, 0.7f);
        Rect.sizeDelta = new Vector2(0, 20);

        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {

            //検索成功したら
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


                    // Asset/Fontの下にあるフォントを検索（違うフォルダに入れている場合は変更）
                    string[] fonts = AssetDatabase.FindAssets("t:Font", new string[] { "Assets/Font" });

                    //Fontフォルダ内の一番目のフォントをfontに入れる
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
                UnityEngine.Debug.Log("ランキング取得失敗");
            }
        });

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //タイトルへ
    public void GoToTitleButton()
    {
        Sound.PlaySe("tap2");
        SceneManager.LoadScene("Scene_Title");
    }

}

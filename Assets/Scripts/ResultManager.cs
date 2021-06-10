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

    //nameが正しいか判別する変数
    private bool nameOK = true;

    // Start is called before the first frame update
    void Start()
    {

        Sound.LoadSe("tap2", "tap2");

        //保存してるハイスコア呼び出し
        GameManager.highScore = PlayerPrefs.GetInt("SCORE", 0);

        scoreObject = GameObject.Find("ScoreText");
        scoreComponent = scoreObject.GetComponent<UnityEngine.UI.Text>();
        scoreComponent.text = GameManager.Score.ToString("スコア:00000m");

        //ハイスコアメッセージを最初は非表示
        highObject = GameObject.Find("HighText");
        highComponent = highObject.GetComponent<UnityEngine.UI.Text>();
        highComponent.enabled = false;

        //注意するメッセージを最初は非表示
        warningObject = GameObject.Find("WarningText");
        warningText = warningObject.GetComponent<UnityEngine.UI.Text>();
        warningText.enabled = false;


        //ランキングメッセージ取得
        rankingObject = GameObject.Find("RankingText");
        rankingText = rankingObject.GetComponent<UnityEngine.UI.Text>();

        //InputFieldを読み込む
        inputObject = GameObject.Find("InputField");
        inputComponent = inputObject.GetComponent<InputField>();

        //ハイスコアとスコアが同じときのみ表示
        if (GameManager.Score == GameManager.highScore)
        {
            highComponent.enabled = true;
        }

        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("HighScore");

        //scoreフィールドの降順でデータを取得
        query.OrderByDescending("score");

        //検索件数を5件に設定
        query.Limit = 5;

        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {

            //検索成功したら
            if (e == null)
            {
                for (int i = 0; i < 5; i++)
                {
                    rankingText.text += (i + 1) + ":" + objList[i]["name"] + "   " + objList[i]["score"] + "m" + "\n";

                }
            } else {
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

    //InputFieldに入力された内容を送信
    public void EndInputText()
    {
        Sound.PlaySe("tap2");

        name = inputComponent.text;
        int len = name.Length;
        Debug.Log(name);
        inputComponent.text = "";

        NCMBObject obj = new NCMBObject("HighScore");
        //オブジェクトに値を設定
        if (name == null | name == "")
        {
            //名前が空ならguestにする
            name = "guest";
            obj["name"] = name;
            nameOK = true;

        }
        else {
            //nameの長さが5文字以内
            if (len < 6) {
                //入れた名前を登録
                obj["name"] = name;
                nameOK = true;

                //6文字以上
            }
            else {
                //nameが正しくない
                nameOK = false;

                //warningText表示
                warningText.enabled = true;

            }
        }

        if (nameOK) {
            obj["score"] = GameManager.Score;
            //データストアへの登録
            obj.SaveAsync((NCMBException e) => {
                if (e != null)
                {
                    //エラー処理
                    UnityEngine.Debug.Log("ランキング登録失敗");
                }
                else
                {
                    //成功時の処理
                    selfID = obj.ObjectId;
                    Debug.Log(selfID);
                    SceneManager.LoadScene("Scene_Ranking");
                }
            });
        }
    }

}

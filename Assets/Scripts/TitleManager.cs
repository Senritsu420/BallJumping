using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class TitleManager : MonoBehaviour
{

    private GameObject highScoreObject;

    private UnityEngine.UI.Text highScoreText;

    private GameObject explainImageObject;

    private Image explainImage;

    private GameObject buttonClose;

    private GameObject player;

    private Player playerP;

    private SpriteRenderer spRenderer;

    // Start is called before the first frame update
    void Start()
    {
        Sound.LoadBgm("titlebgm", "bgmpop");
        Sound.PlayBgm("titlebgm");
        Sound.LoadSe("tap2", "tap2");

        explainImageObject = GameObject.Find("ExplainImage");
        explainImage = explainImageObject.GetComponent<Image>();
        explainImage.enabled = false;

        buttonClose = GameObject.Find("CloseButton");
        buttonClose.SetActive(false);

        highScoreObject = GameObject.Find("HighScoreText");
        highScoreText = highScoreObject.GetComponent<UnityEngine.UI.Text>();

        player = GameObject.Find("Player");
        spRenderer = player.GetComponent<SpriteRenderer>();

        //保存してるハイスコア呼び出し
        GameManager.highScore = PlayerPrefs.GetInt("SCORE", 0);

        highScoreText.text = GameManager.highScore.ToString("最高記録:00000m");

        //Scoreを初期化
        GameManager.Score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PushStartButton()
    {
        Sound.StopBgm();
        Sound.PlaySe("tap2");
        SceneManager.LoadScene("Scene_Game");
    }

    public void PushRankingButton()
    {
        Sound.StopBgm();
        Sound.PlaySe("tap2");
        SceneManager.LoadScene("Scene_Ranking");
    }

    public void PushExplainButton()
    {
        spRenderer.enabled = false;
        explainImage.enabled = true;
        buttonClose.SetActive(true);
        Sound.PlaySe("tap2");

    }

    public void PushCloseButton()
    {
        spRenderer.enabled = true;
        explainImage.enabled = false;
        buttonClose.SetActive(false);
        Sound.PlaySe("tap2");

    }
}

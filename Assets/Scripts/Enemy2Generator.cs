using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Enemy2Generator : MonoBehaviour
{
    //敵プレハブ
    public GameObject enemyPrefab;
    //時間間隔の最小値
    public float minTime = 2f;
    //時間間隔の最大値
    public float maxTime = 4f;
    //X座標の最小値
    public float xMinPosition = -2f;
    //X座標の最大値
    public float xMaxPosition = 2f;
    //敵生成時間間隔
    private float interval;
    //経過時間
    private float time = 0f;

    private float timeleft;

    private GameObject player;

    private Player playerP;

    private int enemyCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        //時間間隔を決定する
        interval = GetRandomTime();

        //プレイやーのオブジェクトと、Playerスクリプト取得
        player = GameObject.Find("Player");
        playerP = player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerP.isDead)
        {

            RandomEnemy();

        }
    }

    private void RandomEnemy()
    {

        //時間計測
        time += Time.deltaTime;

        //経過時間が生成時間になったとき(生成時間より大きくなったとき)
        if (time > interval)
        {

            //enemyが3つ以下の時
            if (enemyCount < 1)
            {

                enemyCount++;

                //enemyをインスタンス化する(生成する)
                GameObject enemy = Instantiate(enemyPrefab);
                //生成した敵の座標を決定する(現状X=0,Y=10,Z=20の位置に出力)
                enemy.transform.position = GetRandomPosition();

                //経過時間を初期化して再度時間計測を始める
                time = 0f;
                //次に発生する時間間隔を決定する
                interval = GetRandomTime();
            }

            if (this.transform.position.y < -6)
            {
                enemyCount--;

            }

            if (playerP.isDead)
            {
                Destroy(this);
            }

        }
    }

    //ランダムな時間を生成する関数
    private float GetRandomTime()
    {
        return Random.Range(minTime, maxTime);
    }

    //ランダムな位置を生成する関数
    private Vector2 GetRandomPosition()
    {
        //それぞれの座標をランダムに生成する
        float x = Random.Range(xMinPosition, xMaxPosition);

        //Vector2型のPositionを返す
        return new Vector2(x, 6);
    }

}

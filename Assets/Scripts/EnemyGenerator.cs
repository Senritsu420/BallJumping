using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class EnemyGenerator : MonoBehaviour
{

    //�G�v���n�u
    public GameObject enemyPrefab;
    //���ԊԊu�̍ŏ��l
    public float minTime = 3f;
    //���ԊԊu�̍ő�l
    public float maxTime = 5f;
    //X���W�̍ŏ��l
    public float xMinPosition = -2f;
    //X���W�̍ő�l
    public float xMaxPosition = 2f;
    //�G�������ԊԊu
    private float interval;
    //�o�ߎ���
    private float time = 0f;

    private float timeleft;

    private GameObject player;

    private Player playerP;

    private int enemyCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        //���ԊԊu�����肷��
        interval = GetRandomTime();

        //�v���C��[�̃I�u�W�F�N�g�ƁAPlayer�X�N���v�g�擾
        player = GameObject.Find("Player");
        playerP = player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        //0.4�b���Ƃɓ���
        //timeleft -= Time.deltaTime;

        if (!playerP.isDead) {
            //if (timeleft <= 0.0)
            //{

                RandomEnemy();

                //0.4�b����
                //timeleft = 0.4f;

            //}
        }
    }

    private void RandomEnemy()
    {

        //���Ԍv��
        time += Time.deltaTime;

        //�o�ߎ��Ԃ��������ԂɂȂ����Ƃ�(�������Ԃ��傫���Ȃ����Ƃ�)
        if (time > interval)
        {


            //enemy��3�ȉ��A���v���C���[�������Ă��鎞
            if (enemyCount < 1 & !playerP.isDead)
            {

                enemyCount++;

                //enemy���C���X�^���X������(��������)
                GameObject enemy = Instantiate(enemyPrefab);
                //���������G�̍��W�����肷��(����X=0,Y=10,Z=20�̈ʒu�ɏo��)
                enemy.transform.position = GetRandomPosition();

                //�o�ߎ��Ԃ����������čēx���Ԍv�����n�߂�
                time = 0f;
                //���ɔ������鎞�ԊԊu�����肷��
                interval = GetRandomTime();
            }

            if(this.transform.position.y < -6)
            {
                enemyCount--;
            }

            if (playerP.isDead)
            {
                Destroy(this);
            }

        }
    }

    //�����_���Ȏ��Ԃ𐶐�����֐�
    private float GetRandomTime()
    {
        return Random.Range(minTime, maxTime);
    }

    //�����_���Ȉʒu�𐶐�����֐�
    private Vector2 GetRandomPosition()
    {
        //���ꂼ��̍��W�������_���ɐ�������
        float x = Random.Range(xMinPosition, xMaxPosition);

        //Vector2�^��Position��Ԃ�
        return new Vector2(x, 6);
    }
}
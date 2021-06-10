using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightBlock : MonoBehaviour
{

    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Track a single touch as a direction control.
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Handle finger movements based on TouchPhase
            switch (touch.phase)
            {
                //When a touch has first been detected, change the message and record the starting position
                case TouchPhase.Began:
                    Debug.Log("�E�N���b�N");
                    player.xr = 1.0f;
                    break;

                //Determine if the touch is a moving touch
                case TouchPhase.Moved:
                    // Determine direction by comparing the current touch position with the initial one
                    Debug.Log("�h���b�O");
                    player.xr = 1.0f;
                    break;

                case TouchPhase.Ended:
                    // Report that the touch has ended when it ends
                    Debug.Log("�������u��");
                    player.xr = 0;
                    break;
            }
        }
    }

    //�@�����ꂽ��
    private void OnMouseDown()
    {
        Debug.Log("�E�N���b�N");
        player.xr = 1.0f;
    }

    //�@��������
    private void OnMouseUp()
    {
        Debug.Log("�������u��");
        player.xr = 0;
    }
}

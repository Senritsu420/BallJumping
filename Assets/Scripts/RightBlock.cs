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
                    Debug.Log("右クリック");
                    player.xr = 1.0f;
                    break;

                //Determine if the touch is a moving touch
                case TouchPhase.Moved:
                    // Determine direction by comparing the current touch position with the initial one
                    Debug.Log("ドラッグ");
                    player.xr = 1.0f;
                    break;

                case TouchPhase.Ended:
                    // Report that the touch has ended when it ends
                    Debug.Log("離した瞬間");
                    player.xr = 0;
                    break;
            }
        }
    }

    //　押された時
    private void OnMouseDown()
    {
        Debug.Log("右クリック");
        player.xr = 1.0f;
    }

    //　離した時
    private void OnMouseUp()
    {
        Debug.Log("離した瞬間");
        player.xr = 0;
    }
}

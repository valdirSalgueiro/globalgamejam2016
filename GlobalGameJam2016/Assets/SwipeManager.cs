using UnityEngine;
using System.Collections;

public class SwipeManager : MonoBehaviour
{

    public float minSwipeDistY;

    public float minSwipeDistX;

    private Vector2 startPos;


    void Start()
    {

    }

    void Update()
    {
        //#if UNITY_ANDROID
        if (Input.touchCount > 0)
        {

            Touch touch = Input.touches[0];
            if (touch.tapCount == 2) {
                RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), Vector2.zero);

            }
            switch (touch.phase)
            {

                case TouchPhase.Began:
                    startPos = touch.position;
                    break;

                case TouchPhase.Ended:
                    float swipeDistVertical = (new Vector3(0, touch.position.y, 0) - new Vector3(0, startPos.y, 0)).magnitude;
                    if (swipeDistVertical > minSwipeDistY)
                    {
                        float swipeValue = Mathf.Sign(touch.position.y - startPos.y);
                        if (swipeValue > 0)
                        {
                            Debug.Log("cima");
                        }
                        else if (swipeValue < 0)
                        {
                            Debug.Log("baixo");
                        }
                    }

                    float swipeDistHorizontal = (new Vector3(touch.position.x, 0, 0) - new Vector3(startPos.x, 0, 0)).magnitude;
                    if (swipeDistHorizontal > minSwipeDistX)
                    {
                        float swipeValue = Mathf.Sign(touch.position.x - startPos.x);
                        if (swipeValue > 0)
                        {
                            Debug.Log("direito");
                        }
                        else if (swipeValue < 0)
                        {
                            Debug.Log("esquerda");
                        }
                    }
                    break;
            }
        }
    }
}
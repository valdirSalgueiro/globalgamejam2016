using UnityEngine;
using System.Collections;

public class SwipeManager : MonoBehaviour
{

    public float minSwipeDistY;

    public float minSwipeDistX;

    private Vector2 startPos;

    public GameObject blue;
    public GameObject red;
    public GameObject green;
    public GameObject yellow;

    Renderer blueRend;
    Renderer redRend;
    Renderer greenRend;
    Renderer yellowRend;


    void Start()
    {
        blueRend = blue.GetComponent<Renderer>();
        blueRend.enabled = false;
        yellowRend = yellow.GetComponent<Renderer>();
        yellowRend.enabled = false;
        redRend = red.GetComponent<Renderer>();
        redRend.enabled = false;
        greenRend = green.GetComponent<Renderer>();
        greenRend.enabled = false;
    }

    void Update()
    {
        //#if UNITY_ANDROID
        if (Input.touchCount > 0)

        {

            Touch touch = Input.touches[0];



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
                            blueRend.enabled = true;
                            Debug.Log("cima");
                        }
                        else if (swipeValue < 0)
                        {
                            redRend.enabled = true;
                            Debug.Log("baixo");
                        }
                    }

                    float swipeDistHorizontal = (new Vector3(touch.position.x, 0, 0) - new Vector3(startPos.x, 0, 0)).magnitude;

                    if (swipeDistHorizontal > minSwipeDistX)

                    {

                        float swipeValue = Mathf.Sign(touch.position.x - startPos.x);

                        if (swipeValue > 0)
                        {
                            yellowRend.enabled = true;
                            Debug.Log("direito");
                        }

                        else if (swipeValue < 0)
                        {
                            greenRend.enabled = true;
                            Debug.Log("esquerda");
                        }

                    }
                    break;
            }
        }
    }
}
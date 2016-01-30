using UnityEngine;
using System.Collections;

public class BackgroundAnimator : MonoBehaviour {

    SpriteRenderer renderer;
    float opacity = 0f;
    float sign = 1;

	// Use this for initialization
	void Start () {
        renderer = GetComponent<SpriteRenderer>();
        opacity = Random.Range(0f, 1f);
	}
	
	// Update is called once per frame
	void Update () {
        renderer.color = new Color(1f, 1f, 1f, opacity);

        if (opacity >= 1.0f)
        {
            sign = -1;
        }
        if (opacity <= 0) {
            sign = 1;
        }
        opacity += Time.deltaTime * sign;
	}
}

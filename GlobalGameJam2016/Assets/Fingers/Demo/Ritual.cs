using UnityEngine;
using System.Collections;

public class Ritual : MonoBehaviour {

//	public float fadeSpeed = 0.01f;
//	public float fadeTime = 5f;
//	public bool fadeIn = true;

	public float minimum = 0.0f;
	public float maximum = 1.0f;
	public float duration = 10f;
	private float startTime;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
	}

	void Update()
	{
		//float t = (Time.time - startTime) / duration;
		float alpha = Mathf.Lerp(1f, 0.0f, (Time.time - startTime));
		Debug.Log ("[PEDRO] STEP : " +  alpha.ToString() + " - " + Mathf.SmoothStep(minimum, maximum, alpha));
		this.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, Mathf.SmoothStep(minimum, maximum, alpha));
		if (Mathf.SmoothStep (minimum, maximum, alpha) <= 0)
			GameObject.Destroy (this.gameObject);
	}

	// Update is called once per frame
//	void Update () {
//		if (fadeIn) {
//			float Fade = Mathf.SmoothDamp(0f,1f,ref fadeSpeed,fadeTime);
//			this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,Fade);
//		}

//		if (!fadeIn) {
//			float Fade = Mathf.SmoothDamp(1f,0f,ref fadeSpeed,fadeTime);
//			this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,Fade);
//		}
//	}
}

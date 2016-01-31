using UnityEngine;
using System.Collections;

public class CloudAnimator : MonoBehaviour {

    float speed = 0f;

	// Use this for initialization
	void Start () {
        speed = Random.Range(0.005f, 0.01f);
        transform.position = new Vector3(Random.Range(-17, 17), Random.Range(0, 3));
    }
	
	// Update is called once per frame
	void Update () {
        transform.position += new Vector3(1,0,0)*speed;
        if (transform.position.x > 17) {
            speed = Random.Range(0.01f, 0.02f);
            transform.position = new Vector3(-18, Random.Range(0, 3));
        }
	}

}

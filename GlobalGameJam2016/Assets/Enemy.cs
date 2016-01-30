using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public float speed = 1.0f;

    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, Vector3.zero) < 1 )
        {
            Destroy(gameObject);
        }
    }
}

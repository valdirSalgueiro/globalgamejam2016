using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("collision name = " + col.gameObject.name);
        if (col.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
            Debug.Log("morreu");
			SceneManager.LoadScene("DemoScene");
        }
    }
}

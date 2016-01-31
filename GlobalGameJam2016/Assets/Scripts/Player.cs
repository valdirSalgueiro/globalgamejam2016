using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using DigitalRubyShared;

public class Player : MonoBehaviour {
	public GameObject TelaEscura;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.gameObject.transform.Rotate(0,0,-Time.deltaTime * 3);
	}

	public void Reiniciar()
	{
		SceneManager.LoadScene("DemoScene");
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("collision name = " + col.gameObject.name);
        if (col.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
			int hs = PlayerPrefs.GetInt ("High Score");
			if(DemoScript.score > hs)
				PlayerPrefs.SetInt("High Score", DemoScript.score);
			TelaEscura.SetActive(true);
            Debug.Log("morreu");
        }
    }
}

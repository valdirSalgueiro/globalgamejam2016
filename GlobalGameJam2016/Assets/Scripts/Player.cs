using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
	public GameObject TelaEscura;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
			TelaEscura.SetActive(true);
            Debug.Log("morreu");
        }
    }
}

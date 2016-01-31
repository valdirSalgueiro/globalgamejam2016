using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour {
    public GameObject TelaEscura;
    private bool pausado;
	// Use this for initialization
	void Start () {
        pausado = false;
	
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausado == false)
            {
                Pausa();
                Screen.lockCursor = true;
                Cursor.visible = true; 
            }
            else
            {
                desPausa();
            }
            
        }
	
	}


    public void Pausa()
    {
        Time.timeScale = 0;
        TelaEscura.SetActive(true);

        pausado = true;
    }

    public void desPausa()
    {
        Time.timeScale = 1;
		Debug.Log ("[PEDRO] teste");
        TelaEscura.SetActive(false);
        Screen.lockCursor = false;
        pausado = false;
    }

    public void Restart()
    {
		desPausa();
		Application.LoadLevel("DemoScene");
    }

    public void Exit()
    {
        Application.Quit();
    }
}

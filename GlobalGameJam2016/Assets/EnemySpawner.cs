using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{

    // Update is called once per frame
    public PlayerHealth playerHealth;       // Reference to the player's heatlh.
    public GameObject enemy;                // The enemy prefab to be spawned.
    public GameObject enemySinWave;
    public float spawnTime = 3f;            // How long between each spawn.



    void Start()
    {
        // Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
        StartCoroutine(Spawn());
    }


    IEnumerator Spawn()
    {
        while (playerHealth.currentHealth > 0)
        {
            int side = Random.Range(0, 4);
            Vector3 position = Vector3.zero;

            switch (side)
            {
                case 0:
                    //left
                    position = Camera.main.ViewportToWorldPoint(new Vector3(-0.1f, Random.Range(0.0f, 1.0f), 0.0f));
                    break;
                case 1:
                    //right
                    position = Camera.main.ViewportToWorldPoint(new Vector3(1.1f, Random.Range(0.0f, 1.0f), 0.0f));
                    break;
                case 2:
                    //up
                    position = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0.0f, 1.0f), -0.1f, 0.0f));
                    break;
                case 3:
                    //down
                    position = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0.0f, 1.0f), 1.1f, 0.0f));
                    break;
            }


            // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
            int enemyType = Random.Range(0, 2);

            switch (enemyType)
            {
                case 0:
                    Instantiate(enemy, position, Quaternion.identity);
                    yield return new WaitForSeconds(Random.Range(0, 3));
                    break;
                case 1:
                    Instantiate(enemySinWave, position, Quaternion.identity);
                    yield return new WaitForSeconds(Random.Range(0, 2));
                    break;
            }

        }
    }
    void Update()
    {

    }
}

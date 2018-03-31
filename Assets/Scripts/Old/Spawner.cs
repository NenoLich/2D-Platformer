using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
	public float spawnTime = 5f;		// The amount of time between each spawn.
	public float spawnDelay = 0;		// The amount of time before spawning starts.
	public GameObject[] enemies;		// Array of enemy prefabs.
    public bool flip = false;


	void Start ()
	{
		// Start calling the Spawn function repeatedly after a delay .
		InvokeRepeating("Spawn", spawnDelay, spawnTime);
	}


    void Spawn()
    {
        // Instantiate a random enemy.
        int enemyIndex = Random.Range(0, enemies.Length);
        GameObject enemy = Instantiate(enemies[enemyIndex], transform.position, transform.rotation);
        if (flip)
        {
            enemy.GetComponent<Enemy>().Flip();
        }

		// Play the spawning effect from all of the particle systems.
		foreach(ParticleSystem p in GetComponentsInChildren<ParticleSystem>())
		{
			p.Play();
		}
	}
}

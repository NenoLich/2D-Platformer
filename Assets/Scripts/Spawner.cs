using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
	public float spawnTime = 5f;		
	public float spawnDelay = 0;		
	public GameObject[] enemies;		
    public bool flip = false;


	void Start ()
	{
		InvokeRepeating("Spawn", spawnDelay, spawnTime);
	}


    void Spawn()
    {
        int enemyIndex = Random.Range(0, enemies.Length);
        GameObject enemy = Instantiate(enemies[enemyIndex], transform.position, transform.rotation);
        if (flip)
        {
            enemy.GetComponent<EnemyHealth>().Flip();
        }

		foreach(ParticleSystem p in GetComponentsInChildren<ParticleSystem>())
		{
			p.Play();
		}
	}
}

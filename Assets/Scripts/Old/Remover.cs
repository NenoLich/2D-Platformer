﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Remover : MonoBehaviour
{
	public GameObject splash;


	void OnTriggerEnter2D(Collider2D col)
	{
		// If the player hits the trigger...
		if(col.gameObject.tag == "Player")
		{
			// .. stop the camera tracking the player
			GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>().enabled = false;

            GameObject HpBar = GameObject.FindGameObjectWithTag("HealthBar");

            if (HpBar!=null && HpBar.activeSelf)
			{
                HpBar.SetActive(false);
			}

			// ... instantiate the splash where the player falls in.
			//Instantiate(splash, col.transform.position, transform.rotation);
			// ... destroy the player.
			Destroy (col.gameObject);
            StartCoroutine(EndGame());
		}
		else
		{
			// ... instantiate the splash where the enemy falls in.
			//Instantiate(splash, col.transform.position, transform.rotation);

			// Destroy the enemy.
			Destroy (col.gameObject);	
		}
	}

	IEnumerator EndGame()
	{
		// ... pause briefly
		yield return new WaitForSeconds(1.5f);
        // ... and then reload the level.
        GameObject.Find("StartLevelSceneUI").gameObject.GetComponent<SceneController>().ShowMenu("Defeat"); 
    }
}

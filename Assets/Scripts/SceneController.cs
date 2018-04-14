using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

	void Update ()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            transform.Find("Pause").gameObject.SetActive(true);
            Time.timeScale = 0;
        }
	}

    public void Defeat()
    {
        transform.Find("Defeat").gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void Victory()
    {
        transform.Find("Victory").gameObject.SetActive(true);
        Time.timeScale = 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

    public bool menuIsActive=false;

    private Pauser pauser;

    private void Awake()
    {
        pauser = GameObject.Find("Pauser").GetComponent<Pauser>();
    }

    void Update ()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            ShowMenu("Pause");
        }
	}

    public void ShowMenu(string panel)
    {
        if (!menuIsActive)
        {
            transform.Find(panel).gameObject.SetActive(true);
            menuIsActive = true;
            pauser.Pause();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameButton : MonoBehaviour {

    public void NewGameButtonClick()
    {
        GameObject pauser = GameObject.Find("Pauser");
        if (pauser!=null)
            pauser.GetComponent<Pauser>().Pause();

        SceneManager.LoadScene("StartLevel");
    }
}

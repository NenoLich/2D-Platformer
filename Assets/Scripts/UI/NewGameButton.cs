using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameButton : MonoBehaviour {

    public void NewGameButtonClick()
    {
        GameObject.Find("Pauser").GetComponent<Pauser>().Pause();
        SceneManager.LoadScene("StartLevel");
    }
}

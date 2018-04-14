using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeButton : MonoBehaviour {

    public void ResumeButtonClick()
    {
        transform.parent.gameObject.SetActive(false);
        transform.root.GetComponent<SceneController>().menuIsActive = false;
        GameObject.Find("Pauser").GetComponent<Pauser>().Pause();
    }
}

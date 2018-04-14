using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeButton : MonoBehaviour {

    public void ResumeButtonClick()
    {
        transform.parent.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour {

    public GameObject parentPanel;

    private Settings settings;

    private void Awake()
    {
        settings = GameObject.FindGameObjectWithTag("Settings").GetComponent<Settings>();
        transform.Find("Volume").gameObject.GetComponent<Slider>().value = settings.volume;
    }

    public void BackButtonClick()
    {
        parentPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void VolumeChanged(float value)
    {
        settings.volume = value;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsButton : MonoBehaviour {

    public void SettingsButtonClick()
    {
        GameObject settingsPanel = transform.root.Find("Settings").gameObject;
        GameObject parentPanel = transform.parent.gameObject;
        settingsPanel.GetComponent<SettingsPanel>().parentPanel = parentPanel;
        parentPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }
}

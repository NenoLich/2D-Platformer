using UnityEngine;
using System.Collections;
using UnityEngine.PostProcessing;

public class Pauser : MonoBehaviour
{
    private bool paused = false;
    private GameObject player;
    private PostProcessingProfile postProcProf;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        postProcProf = ScriptableObject.CreateInstance("PostProcessingProfile") as PostProcessingProfile; 
    }

    public void Pause()
    {
        Time.timeScale = paused ? 1 : 0;
        var settings = postProcProf.chromaticAberration.settings;
        settings.intensity = 1-Time.timeScale;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PostProcessingBehaviour>().profile.chromaticAberration.settings = settings;

        if (player!=null)
            player.GetComponent<PlayerAttack>().enabled = !player.GetComponent<PlayerAttack>().enabled;

        paused = !paused;
    }
}

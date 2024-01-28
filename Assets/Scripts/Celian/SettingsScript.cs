using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class SettingsScript : MonoBehaviour
{

    [SerializeField] private Canvas titleCanvas;
    [SerializeField] private Canvas settingCanvas;

    public AudioMixer audioMixer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void backButton(){
        titleCanvas.gameObject.SetActive(true);
        settingCanvas.gameObject.SetActive(false);
    }

    public void setVolume(float volume){
        audioMixer.SetFloat("volume", volume);
    }
}

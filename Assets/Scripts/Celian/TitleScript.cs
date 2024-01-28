using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScript : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Canvas titleCanvas;
    [SerializeField] private Canvas settingCanvas;

    void Start()
    {
        settingCanvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void quitButton()
    {
        Application.Quit();
    }

    public void startButton(){
        SceneManager.LoadScene("ChangeThis");
    }

    public void settingsButton(){
        titleCanvas.gameObject.SetActive(false);
        settingCanvas.gameObject.SetActive(true);
    }
}

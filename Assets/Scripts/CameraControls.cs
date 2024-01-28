using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class CameraControls : MonoBehaviour
{
    [SerializeField] private float transitionTime = 0.5f;
    [SerializeField] private float indicationTime = 1f;
    [SerializeField] private int cameraLenght = 1536;

    [Header("Actions")]
    [SerializeField] private InputActionReference toggleCameraAction;
    [SerializeField] private InputActionReference switchCameraAction;

    [Header("Scene Objects")]
    [SerializeField] private GameObject cameraPanel;
    [SerializeField] private GameObject cameraImages;
    [SerializeField] private Image indicationArrowLeft;
    [SerializeField] private Image indicationArrowRight;

    [Header("Indications")]
    [SerializeField] private float minTransparency = 0.2f;
    [SerializeField] private float maxTransparency = 0.6f;


    [Header("List of raw image prefab")]
    [SerializeField] private List<RawImage> cameras;

    private UpgradesManager upgradesManager;

    private bool isCameraOn;
    private int currentCamera = 0; 
    private List<bool> camerasOn = new List<bool>();
    private int numberOfCameraOn;
    private bool isSwitching;


    public void OnEnable()
    {
        cameraPanel.SetActive(false);

        upgradesManager = UpgradesManager.Instance;

        camerasOn = upgradesManager.areCameraAvailable();
        numberOfCameraOn = 0;

        // Si il n'y a pas de caméra débloquées, on n'active pas les actions
        foreach(bool isOn in camerasOn) { if (isOn) { numberOfCameraOn++; } }
        if (numberOfCameraOn == 0) { return; }

        toggleCameraAction.action.Enable();
        toggleCameraAction.action.started += toggleCameraPanel; 
        switchCameraAction.action.Enable();

        createCameraView();
    }
    public void OnDisable()
    {
        toggleCameraAction.action.Disable();
        toggleCameraAction.action.started -= toggleCameraPanel;
        switchCameraAction.action.Disable();
    }

    public void createCameraView()
    {
        int first = -1;
        int end = 0;
        for (int i = 0; i < camerasOn.Count; i++)
        {
            if (camerasOn[i])
            {
                if (first == -1) { first = i; }
                Instantiate(cameras[i], cameraImages.transform).transform.localPosition = new Vector3(cameraLenght * i, 0, 0);
                end = i;
            }
        }

        if (numberOfCameraOn != 1)
        {
            Instantiate(cameras[first], cameraImages.transform).transform.localPosition = new Vector3(cameraLenght * (end + 1), 0, 0);
            Instantiate(cameras[end], cameraImages.transform).transform.localPosition = new Vector3(cameraLenght * -1, 0, 0);
        }
    }

    public void toggleCameraPanel(InputAction.CallbackContext contect)
    {
        if (isCameraOn)
        {
            cameraPanel.SetActive(false);
            if (numberOfCameraOn != 1) { switchCameraAction.action.started -= switchCamera; }
            StopAllCoroutines();
        }
        else
        {
            cameraPanel.SetActive(true);
            if (numberOfCameraOn != 1) { switchCameraAction.action.started += switchCamera; StartCoroutine(ChangeCameraIndication(1)); }

        }
        isCameraOn = !isCameraOn;
    }

    public void switchCamera(InputAction.CallbackContext contexts)
    {
        int direction = (int)switchCameraAction.action.ReadValue<Vector2>().x;
        if (!isSwitching) { StartCoroutine(ChangeCamera(direction)); isSwitching = true; }        
    }

    public IEnumerator ChangeCamera(int direction)
    {
        float t = 0f;
        float start = - cameraLenght * currentCamera;
        float end = start - cameraLenght * direction;

        while (t < transitionTime)
        {
            t += Time.deltaTime;
            Vector2 newPosition = new Vector2(Mathf.Lerp(start, end, t / transitionTime), 0);
            cameraImages.transform.localPosition = newPosition;
            yield return null;
        }

        currentCamera += direction;
        if (currentCamera < 0 || currentCamera >= numberOfCameraOn)
        {
            currentCamera = (currentCamera + numberOfCameraOn) % numberOfCameraOn;
            cameraImages.transform.localPosition = new Vector3(-currentCamera * cameraLenght, 0, 0);
        }

        isSwitching = false;
    }

    public IEnumerator ChangeCameraIndication(int modificator)
    {
        float time = 0f;
        Debug.Log("clignoqtement" + modificator.ToString());
        while (time < indicationTime)
        {
            time += Time.deltaTime;
            Color temp_color_left = indicationArrowLeft.color;
            Color temp_color_right = indicationArrowRight.color;

            if (modificator > 0)
            {
                temp_color_left.a = Mathf.Lerp(minTransparency, maxTransparency, time / indicationTime);
                temp_color_right.a = Mathf.Lerp(minTransparency, maxTransparency, time / indicationTime);
            }
            else
            {
                temp_color_left.a = Mathf.Lerp(maxTransparency, minTransparency, time / indicationTime);
                temp_color_right.a = Mathf.Lerp(maxTransparency, minTransparency, time / indicationTime);
            }
            

            indicationArrowLeft.color = temp_color_left;
            indicationArrowRight.color = temp_color_right;

            yield return null;
        }
        Debug.Log("fin clignoqtement");

        StartCoroutine(ChangeCameraIndication(-modificator));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    public Image tapToStart;
    public TMP_Text mainText;
    public TMP_Text tapToStartText;
    public PlayerInput playerInput;
    public TimeManager timer2;
    public GameObject backgroundMusic;
    [HideInInspector] public bool buttonPressed;
    private InputAction tapToStartAction;

    void Start()
    {
        tapToStartAction = playerInput.actions["TapToStart"];
        buttonPressed = false;
    }

    void Update()
    {
        if (tapToStartAction.triggered && !buttonPressed)
        {
            tapToStart.enabled = false;
            tapToStartText.enabled = false;
            mainText.enabled = false;
            buttonPressed = true;;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene("Demo");
        timer2.timeValue = timer2.initialTimeValue;
        AudioListener.volume = 1;
    }
    public void Exit()
    {
        Application.Quit();
    }
}

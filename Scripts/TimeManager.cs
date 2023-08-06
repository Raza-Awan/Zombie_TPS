using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public CharacterAiming characterAiming;
    public PlayerHealth playerHealth;
    public Image Filler;
    public TMP_Text text;
    public float initialTimeValue;
    public float timeValue = 60f;
    public UiManager uiManager;
    public PlayerControllerScript playerControllerScript;
    public GameObject gameOverPanel;
    public GameObject youWinPanel;
    public GameObject playerControlsUI;
    public GameObject TimerUI;
    public GameObject Enemies;
    public EnemiesCounter enemiesCounter;
    public AudioSource zombieAudioSource;
    public AudioClip[] zombieSounds;
    public AudioSource gunAudio;

    private void Awake()
    {
        initialTimeValue = timeValue;
        gameOverPanel.SetActive(false);
        youWinPanel.SetActive(false);
        zombieAudioSource.enabled = false;
    }

    private void Update()
    {
        if (!zombieAudioSource.isPlaying && zombieAudioSource.enabled == true) // giving random audios to zombies
        {
            zombieAudioSource.clip = zombieSounds[Random.Range(0, zombieSounds.Length)];
            zombieAudioSource.Play();
        }

        if (enemiesCounter.enemiesCount <= 0) // when all enemies dies do thid
        {
            zombieAudioSource.Stop();
            zombieAudioSource.enabled = false;
        }


        if (uiManager.buttonPressed == true) // when start button pressed
        {
            zombieAudioSource.volume = 0.5f;
            zombieAudioSource.enabled = true;

            if (timeValue > 0)
            {
                timeValue -= Time.deltaTime;
            }
            else // on end game
            {
                OnEnd();
                TimerUI.SetActive(false);
                zombieAudioSource.Stop();
                AudioListener.volume = 0;
                zombieAudioSource.enabled = false;
                uiManager.backgroundMusic.SetActive(false);
            }
            DisplayTime(timeValue);
        }

        if(playerHealth.currentHealth <= 0)  // in case player loses/dies
        {
            playerControllerScript.enabled = false;
            gameOverPanel.SetActive(true);
            playerControlsUI.SetActive(false);
            TimerUI.SetActive(false);
            Invoke("DisableEnemies", 1.5f);
            uiManager.backgroundMusic.SetActive(false);
            zombieAudioSource.enabled = false;
            zombieAudioSource.volume = 0;
            characterAiming.Sound = false;
            gunAudio.enabled = false;
            //timer2.audioSource.Stop();
        }
    }

    void DisableEnemies()
    {
        Enemies.SetActive(false);
    }

    void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        //text.text = $"{timeToDisplay / 60:00}:{timeToDisplay % 60:00}";
        text.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        Filler.fillAmount = Mathf.InverseLerp(0, initialTimeValue, timeValue);
    }

    void OnEnd()
    {
        if (enemiesCounter.enemiesCount > 0) // player survived the given time but couldn't kill all the enemies
        {
            playerControllerScript.enabled = false;
            gameOverPanel.SetActive(true);
            playerControlsUI.SetActive(false);
        }

        else // in case player wins
        {
            playerControllerScript.enabled = false;
            youWinPanel.SetActive(true);
            playerControlsUI.SetActive(false);
        }
    }
}

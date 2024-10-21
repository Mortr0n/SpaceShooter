using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.Internal;
using UnityEngine.Scripting;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;


public class GameManagerController : MonoBehaviour
{
    public static GameManagerController Instance { get; private set; }


    private PlayerController playerController;
    public Button restartButton;

    public AudioSource[] backgroundMusicArray;
    public AudioSource backgroundMusic;

    public TextMeshProUGUI levelText;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    public void GameOver()
    {
        Debug.Log("Game Over");
        ShowRestartButton();
    }


   public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        levelText = GameObject.Find("LevelText").GetComponent<TextMeshProUGUI>();

        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        if (playerController != null)
        {
            playerController.RestartPlayerSettings();
        }
        HideRestartButton();

        UpdateLevelText("0");
        StartCoroutine(InitializeAfterSceneLoad());
    }

    
    // for anything needing to have the scene fully loaded.  The health bar specifically didn't want to update so this helped
    IEnumerator InitializeAfterSceneLoad()
    {
        yield return new WaitForEndOfFrame();

        if (playerController != null)
        {
            playerController.PlayerUpdateHealthBar();
        }
    }

    private void OnEnable()
    {
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unsubscribe from the sceneLoaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void PlayBackgroundMusic()
    {
        if (!backgroundMusic.isPlaying)  { backgroundMusic.Play();  }
    }

    public void StopBackgroundMusic()
    {
        if (backgroundMusic.isPlaying) { backgroundMusic.Stop(); }
    }

    public void NewGameParameters()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.SetPlayerLevel(0);
        }
    }

    public void UpdateLevelText(string levelString)
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        levelText = GameObject.Find("LevelText").GetComponent<TextMeshProUGUI>();

        if (levelText != null)
        {
            levelText.text = "LVL: " + levelString;
        }
    }

    public void ShowRestartButton()
    {
        restartButton.gameObject.SetActive(true);
    }

    public void HideRestartButton()
    {
        restartButton.gameObject.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnDestroy()
    {
        Debug.Log("Gamemanagercontroller was destroyed");
    }
}

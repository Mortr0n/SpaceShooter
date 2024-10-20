using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class GameManagerController : MonoBehaviour
{
    public static GameManagerController Instance { get; private set; }


    public PlayerController playerController;

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



   public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        levelText = GameObject.Find("LevelText").GetComponent<TextMeshProUGUI>();

        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        UpdateLevelText("0");
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
        Debug.Log($"Updating level text {playerController.GetPlayerLevel()}");
        levelText = GameObject.Find("LevelText").GetComponent<TextMeshProUGUI>();
        Debug.Log($"levelString {levelString} and levelText {levelText}");
        if (levelText != null)
        {
            //string lvlText = playerController.GetPlayerLevel().ToString();
            levelText.text = "Player Level: " + levelString;
        }
    }

    void OnDestroy()
    {
        Debug.Log("Gamemanagercontroller was destroyed");
    }
}

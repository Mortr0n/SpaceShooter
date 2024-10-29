using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameManagerController.Instance != null)
        {
            GameManagerController.Instance.StartGame();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

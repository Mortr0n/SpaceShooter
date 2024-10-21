using UnityEngine;
using UnityEngine.UI;

public class ExpBarController : MonoBehaviour
{
    public float maxExp = 100;
    public float currentExp;
    public Image expBar;

    public void UpdateExpBar()
    {
        PlayerController playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        Debug.Log($"Player exp {playerController.GetCurrentExp()} and max {playerController.GetExpToLevel()} and level {playerController.GetPlayerLevel()} ");
        if (expBar == null)
        {
            expBar = GameObject.Find("ExperienceBar").GetComponent<Image>();
            if (expBar == null)
            {
                Debug.LogError("ExpBar not found");
            }
        }

        if (expBar != null)
        {
            expBar.fillAmount = (float)playerController.GetCurrentExp() / playerController.GetExpToLevel();
        }

    }
}

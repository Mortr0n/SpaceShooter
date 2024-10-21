using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;
    public Image healthBarFill;

    public void UpdateHealthBar()
    {
        PlayerController playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        Debug.Log($"Player health {playerController.GetCurrentHealth()} and max {playerController.GetMaxHealth()} and level {playerController.GetPlayerLevel()} ");
        if (healthBarFill == null)
        {
            healthBarFill = GameObject.Find("HealthBarFill").GetComponent<Image>();
            if (healthBarFill == null)
            {
                Debug.LogError("HealthBarfill not found");
            }
        }

        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)playerController.GetCurrentHealth() / playerController.GetMaxHealth();
        }

    }
}

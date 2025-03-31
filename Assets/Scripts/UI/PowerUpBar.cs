using UnityEngine;

public class PowerUpBar : MonoBehaviour
{
    public GameObject[] powerUpIcons; // 0 = Air, 1 = Water, etc.
    void Start()
    {
        // Hide all icons at the beginning
        foreach (GameObject icon in powerUpIcons)
        {
            icon.SetActive(false);
        }

        // Show unlocked power-ups based on player's progress
        int levelsCompleted = SceneHandler.Instance.GetLevel();

        levelsCompleted = levelsCompleted > 4 ? 4 : levelsCompleted;

        if (levelsCompleted < 0) {
            return;
        }

        for (int i = 0; i < levelsCompleted && i < powerUpIcons.Length; i++)
        {
            powerUpIcons[i].SetActive(true);
        }
    }
}

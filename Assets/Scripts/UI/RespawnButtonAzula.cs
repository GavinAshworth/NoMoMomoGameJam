using UnityEngine;
using UnityEngine.UI;

public class RespawnButtonAzula : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    private void Update()
    {
        // Check if we meet the display conditions
        bool shouldBeActive = 
            LevelHandler.Instance != null && 
            GameManager.Instance != null &&
            LevelHandler.Instance.level == 5 && 
            GameManager.Instance.score >= 5000;

        // Only update visibility if needed
        if (gameObject.activeSelf != shouldBeActive)
        {
            gameObject.SetActive(shouldBeActive);
        }
    }

    private void OnButtonClick()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RespawnForCost();
        }
        else
        {
            Debug.LogError("GameManager instance not found!");
        }
    }
}
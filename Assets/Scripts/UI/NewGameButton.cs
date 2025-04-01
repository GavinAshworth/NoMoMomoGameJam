using UnityEngine;
using UnityEngine.UI;

public class NewGameButton : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        // Call GameManager's function directly
        if (GameManager.Instance != null)
        {
            GameManager.Instance.NewGame();
        }
        else
        {
            Debug.LogError("GameManager instance not found!");
        }
    }
}
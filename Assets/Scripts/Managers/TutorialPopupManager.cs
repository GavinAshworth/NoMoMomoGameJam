using UnityEngine;

public class TutorialPopupManager : MonoBehaviour
{
    public GameObject popupPanel;

    public void TogglePopup()
    {
        popupPanel.SetActive(!popupPanel.activeSelf);
    }
}

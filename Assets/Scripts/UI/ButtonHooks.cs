using UnityEngine;
// File is used to handle button inputs. Methods are attached to specifc buttons.
// Currently buttons only direct to next scene in main menu, but could have more when menu fully implemented

public class ButtonHooks : MonoBehaviour
{
    public void LoadNextScene()
    {
        AudioManager.Instance.PlaySFX("ButtonClick");
        Destroy(gameObject); // Destroys button prior to animation, could be replaced with smoother transition
        SceneHandler.Instance.LoadNextScene();
    }

    public void ExitToMenu()
    {
        SceneHandler.Instance.LoadMenuScene();
    }
}

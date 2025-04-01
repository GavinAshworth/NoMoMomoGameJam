using Unity.VisualScripting;
using UnityEngine;

public class PersistentUI : MonoBehaviour
{
    private static bool exists = false;

    // this script adds the UIRoot to the do not destroy on load so they persist through 
    void Awake()
    {
        if (!exists)
        {
            DontDestroyOnLoad(gameObject);
            exists = true;
        }
        else
        {
            Destroy(gameObject); // prevent dupes
        }
    }
}

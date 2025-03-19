using UnityEngine;

public class ParentProjectileSpawner : MonoBehaviour
{
    [SerializeField] Sprite projectileSprite;
    [SerializeField] GameObject projectileObject;

    public Sprite GetSprite() => projectileSprite;
    public GameObject GetProjectileObject() => projectileObject;
}

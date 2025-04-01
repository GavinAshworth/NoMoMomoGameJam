using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float hoverScale = 1.1f; // 10% bigger
    [SerializeField] private float scaleSpeed = 5f; // Smoothing speed
    
    private Vector3 originalScale;
    private bool isHovering;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Smoothly interpolate scale
        Vector3 targetScale = isHovering ? originalScale * hoverScale : originalScale;
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, scaleSpeed * Time.deltaTime); 
        
        
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
    }
}
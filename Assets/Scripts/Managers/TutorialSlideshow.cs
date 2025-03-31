using UnityEngine;

public class TutorialSlideshow : MonoBehaviour
{
    public GameObject[] slides;
    private int currentSlide = 0;

    void Start()
    {
        ShowSlide(0);
    }

    public void ShowSlide(int index)
    {
        if (index < 0 || index >= slides.Length) return;

        for (int i = 0; i < slides.Length; i++)
            slides[i].SetActive(i == index);

        currentSlide = index;
    }

    public void NextSlide()
    {
        if (currentSlide < slides.Length - 1)
            ShowSlide(currentSlide + 1);
    }

    public void PreviousSlide()
    {
        if (currentSlide > 0)
            ShowSlide(currentSlide - 1);
    }

    public void CloseTutorial()
    {
        gameObject.SetActive(false);
    }
}

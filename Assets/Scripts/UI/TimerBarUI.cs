using UnityEngine;
using UnityEngine.UI;

public class TimerBarUI : MonoBehaviour
{
    [SerializeField] private Image timerFill;
    [SerializeField] private float totalTime = 60f;

    private float remainingTime;

    void Start()
    {
        remainingTime = totalTime;
        timerFill.fillAmount = 1f;
    }

    void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            float percent = remainingTime / totalTime;
            timerFill.fillAmount = percent;
        }
    }

    public void ResetTimer()
    {
        remainingTime = totalTime;
        timerFill.fillAmount = 1f;
    }

    public float getTime() {
        return this.remainingTime;
    }

    public void addTime(float time) {
        this.remainingTime += time;

        if (this.remainingTime > this.totalTime) {
            this.remainingTime = this.totalTime;
        }
    }
}

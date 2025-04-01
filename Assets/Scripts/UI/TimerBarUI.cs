using UnityEngine;
using UnityEngine.UI;

public class TimerBarUI : MonoBehaviour
{
    [SerializeField] private Image timerFill;
    [SerializeField] private float totalTime = 60f;
    private bool play;
    private float remainingTime;

    void Start()
    {
        Play();
        remainingTime = totalTime;
        timerFill.fillAmount = 1f;
    }

    void Update()
    {
        if (remainingTime > 0 && play)
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

    public void AddTime(float time) {
        this.remainingTime += time;

        if (this.remainingTime > this.totalTime) {
            this.remainingTime = this.totalTime;
        }
    }

    public void Play()
    {
        this.play = true;
    }

    public void Pause()
    {
        this.play = false;
    }
}

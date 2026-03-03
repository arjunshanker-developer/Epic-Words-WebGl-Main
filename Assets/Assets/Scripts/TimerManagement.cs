using UnityEngine;
using UnityEngine.UI;

public class TimerManagement : MonoBehaviour
{
    public static TimerManagement instance;

    [Header("Timer Settings")]
    public float totalTime = 30f;

    [Header("UI")]
    public Image fillerImage;

    float currentTime;
    bool isRunning;

    public System.Action OnTimeOver;
    Color greenColor;
    Color yellowColor;
    Color redColor;

    public GameObject stopWatch;
    private void Awake()
    {
        instance = this;
        greenColor = HexToColor("#4CAF50");
        yellowColor = HexToColor("#FFC107");
        redColor = HexToColor("#F44336");
    }
    public void StartTimer()
    {
        currentTime = totalTime;
        isRunning = true;

        fillerImage.fillAmount = 1f;
        fillerImage.color = Color.green;
    }

    void Update()
    {
        if (!isRunning) return;

        currentTime -= Time.deltaTime;

        float percent = Mathf.Clamp01(currentTime / totalTime);
        fillerImage.fillAmount = percent;

        UpdateColor(percent);

        if (currentTime <= 0f)
        {
            isRunning = false;
            fillerImage.fillAmount = 0f;
            OnTimeOver?.Invoke();
        }
    }

    void UpdateColor(float percent)
    {
        percent = Mathf.Clamp01(percent);
        Debug.Log(currentTime);
        if (percent > 0.5f)
        {
            // Green → Yellow
            float t = Mathf.InverseLerp(1f, 0.5f, percent);
            fillerImage.color = Color.Lerp(greenColor, yellowColor, t);
        }
        else
        {
            // Yellow → Red
            float t = Mathf.InverseLerp(0.5f, 0f, percent);
            fillerImage.color = Color.Lerp(yellowColor, redColor, t);
        }

        if(currentTime > 22f)
        {
            stopWatch.GetComponent<PulseButtonWithIdle>().pulseAmount = 0.1f;
            stopWatch.GetComponent<PulseButtonWithIdle>().pulseDuration = 0.3f;

        }
        else if(currentTime > 12f)
        {
            stopWatch.GetComponent<PulseButtonWithIdle>().pulseAmount = 0.1f;
            stopWatch.GetComponent<PulseButtonWithIdle>().pulseDuration = 0.2f;
        }
        else 
        {
            stopWatch.GetComponent<PulseButtonWithIdle>().pulseAmount = 0.2f;
            stopWatch.GetComponent<PulseButtonWithIdle>().pulseDuration = 0.1f;
        }
        
        if(fillerImage.fillAmount == 0)
        {
            Debug.Log("Current Time to show result canvas " + currentTime);
            GameManager.Instance.ShowResultCanvas();
        }
    }

    Color HexToColor(string hex)
    {
        Color color;
        ColorUtility.TryParseHtmlString(hex, out color);
        return color;
    }

    public float GetRemainingPercent()
    {
        return Mathf.Clamp01(currentTime / totalTime);
    }

    public void StopTimer()
    {
        isRunning = false;
    }
    public void ResetTimer()
    {
        isRunning = false;

        currentTime = totalTime;

        fillerImage.fillAmount = 1f;
        fillerImage.color = greenColor;

        // Reset stopwatch pulse to idle state
        var pulse = stopWatch.GetComponent<PulseButtonWithIdle>();
        pulse.pulseAmount = 0.1f;
        pulse.pulseDuration = 0.3f;
    }
    public void ResumeTimer()
    {
        if (currentTime > 0f)
        {
            isRunning = true;
        }
    }

}

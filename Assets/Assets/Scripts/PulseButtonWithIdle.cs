using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PulseButtonWithIdle : MonoBehaviour
{
    [Header("Pulse Settings")]
    public float pulseAmount = 0.1f;      // Scale increase
    public float pulseDuration = 0.5f;    // How long the pulse lasts

    [Header("Idle Settings")]
    private float pulseInterval = 1f;      // Time between pulses

    [Header("References")]
    public Transform button;              // Assign the button you want to pulse

    public bool pulseOnStart = true;

    private Vector3 originalScale;
    private float lastPressTime;

    Coroutine pulse;

    void Start()
    {
        if (button == null)
        {
            Debug.LogWarning("PulseButtonWithIdle: Button reference is missing!");
            return;
        }

        

        // If any axis is zero, set it to 1
        originalScale = button.localScale;
        if (originalScale.x == 0) originalScale.x = 1f;
        if (originalScale.y == 0) originalScale.y = 1f;
        if (originalScale.z == 0) originalScale.z = 1f;

        lastPressTime = Time.time;
        if (pulseOnStart)
            pulse = StartCoroutine(PulseRoutine());
    }

    public void OnButtonPressed()
    {

        if (button == null) return;

        Debug.Log("OnButtonPressed");
        lastPressTime = Time.time;

        // Reset scale safely
        Vector3 safeScale = originalScale;
        if (safeScale.x == 0) safeScale.x = 1f;
        if (safeScale.y == 0) safeScale.y = 1f;
        if (safeScale.z == 0) safeScale.z = 1f;

        button.localScale = safeScale;
    }

    public void TogglePulse(bool status)
    {
        Debug.Log("TogglePulse1 " + status);
        if (pulse != null)
            StopCoroutine(pulse);

        if (status)
            pulse = StartCoroutine(PulseRoutine());
        else
            OnButtonPressed();
    }

    private IEnumerator PulseRoutine()
    {
        if (button == null) yield break;

        while (true)
        {
            if (Time.time - lastPressTime >= pulseInterval)
            {
                float timer = 0f;
                while (timer < pulseDuration)
                {
                    timer += Time.deltaTime;

                    float pulseScale = 1 + Mathf.Sin((timer / pulseDuration) * Mathf.PI) * pulseAmount;

                    Vector3 targetScale = originalScale;
                    targetScale.x = Mathf.Max(originalScale.x * pulseScale, 1f);
                    targetScale.y = Mathf.Max(originalScale.y * pulseScale, 1f);
                    targetScale.z = Mathf.Max(originalScale.z * pulseScale, 1f);

                    button.localScale = targetScale;

                    yield return null;
                }

                // Reset after pulse
                button.localScale = originalScale;

                // Reset timer for next pulse
                lastPressTime = Time.time;
            }

            yield return null;
        }
    }
}

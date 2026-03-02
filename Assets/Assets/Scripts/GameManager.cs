
using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("References")]
    [SerializeField] private LevelDatabase database;
    [SerializeField] private LetterWheelManager wheelManager;
    [SerializeField] private ImageGrid imageGrid;

    private int currentLevelIndex = 0;
    [SerializeField] private AnswerSlotManager slotManager;


    [Header("IQ Section")]
    public GameObject arrow;
    public Transform arrowTargetPos0, arrowTargetPos20, arrowTargetPos50, arrowTargetPos120;
    public GameObject IQPoint;
    public Material zeroMaterial, twentyMaterial, fiftyMaterial, onetwentyMaterial;
    public Transform iqPointIndicatorFinalPos;
    public GameObject iqPointIndicator;
    private int iqStage = 0;
    private int currentIQ = 0;
    public TextMeshProUGUI iqPoints;

    private Coroutine iqCoroutine;

    public CanvasGroup resultCanvas;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        LoadLevel(currentLevelIndex);
    }

    public LevelData GetCurrentLevel()
    {
        return database.GetLevel(currentLevelIndex);
    }
    public int GetCurrentLevelIndex()
    {
        return currentLevelIndex;
    }
   
    public void LoadLevel(int index)
    {
        LevelData level = database.GetLevel(index);

        if (level == null)
        {
            Debug.LogError("Level not found!");
            GameManager.Instance.ShowResultCanvas();
            return;
        }

        wheelManager.SetupLevel(level);
        imageGrid.SetupImages(level);

       slotManager.SetupSlots(level.answer.Length);
    }

    public void NextLevel()
    {
        currentLevelIndex++;
        LoadLevel(currentLevelIndex);
    }

    public string GetCurrentAnswer()
    {
        return database.GetLevel(currentLevelIndex).answer.ToUpper();
    }

    #region IQ

    private Coroutine resultFadeCoroutine;

    public void ShowResultCanvas()
    {
        if (resultFadeCoroutine != null)
            StopCoroutine(resultFadeCoroutine);

        resultFadeCoroutine = StartCoroutine(FadeCanvas(0f, 1f, 0.5f));
    }
    private IEnumerator FadeCanvas(float startAlpha, float endAlpha, float duration)
    {
        float timer = 0f;

        resultCanvas.alpha = startAlpha;
        resultCanvas.gameObject.SetActive(true); // Make sure it's active

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = timer / duration;
            resultCanvas.alpha = Mathf.Lerp(startAlpha, endAlpha, t);

            yield return null;
        }

        resultCanvas.alpha = endAlpha;
    }
    public void ChangePositionTo20()
    {
        iTween.MoveTo(arrow, iTween.Hash("position", arrowTargetPos20.position, "time", 0.8f, "easetpe", iTween.EaseType.linear));
    }

    public void ChangePositionTo50()
    {
        iTween.MoveTo(arrow, iTween.Hash("position", arrowTargetPos50.position, "time", 0.8f, "easetpe", iTween.EaseType.linear));
    }
    public void ChangePositionTo120()
    {
        iTween.MoveTo(arrow, iTween.Hash("position", arrowTargetPos120.position, "time", 0.8f, "easetpe", iTween.EaseType.linear));
    }
    public void ChangePositionTo0()
    {
        iTween.MoveTo(arrow, iTween.Hash("position", arrowTargetPos0.position, "time", 0.8f, "easetpe", iTween.EaseType.linear));
    }

    public void IncreaseIQ()
    {
        if (iqStage < 3)
            iqStage++;
        UpdateIQValue();
        ApplyIQState();
    }

    public void DecreaseIQ()
    {
        if (iqStage > 0)
            iqStage--;
        UpdateIQValue();
        ApplyIQState();
    }

    private void ApplyIQState()
    {
        switch (iqStage)
        {
            case 0:
                
                ChangePositionTo0();
                break;

            case 1:
               
                ChangePositionTo20();
                break;

            case 2:
               
                ChangePositionTo50();
                break;

            case 3:
              
                ChangePositionTo120();
                break;
        }
    }
    private void UpdateIQValue()
    {
        int targetValue = 0;

        switch (iqStage)
        {
            case 0:
                targetValue = 0;
                iqPoints.fontMaterial = zeroMaterial;
                break;
            case 1: targetValue = 20;
                iqPoints.fontMaterial = twentyMaterial;
                break;
            case 2: targetValue = 50;
                iqPoints.fontMaterial = fiftyMaterial;
                break;
            case 3: targetValue = 130;
                iqPoints.fontMaterial = onetwentyMaterial;
                break;
        }

        iqCoroutine = StartCoroutine(AnimateIQ(targetValue));
    }
    private IEnumerator AnimateIQ(int targetValue)
    {
        if (iqCoroutine != null)
            StopCoroutine(iqCoroutine);

        int startValue = currentIQ;

        float duration = 0.8f; // animation speed
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = timer / duration;
            int value = Mathf.RoundToInt(Mathf.Lerp(startValue, targetValue, t));

            iqPoints.text = value.ToString();

            yield return null;
        }

        currentIQ = targetValue;
        iqPoints.text = currentIQ.ToString();
    }
    #endregion
}
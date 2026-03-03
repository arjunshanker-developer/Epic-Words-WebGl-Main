using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeController : MonoBehaviour
{
    public static SwipeController Instance;

    [SerializeField] private UILineRenderer uiLineRenderer;
    [SerializeField] private AnswerSlotManager slotManager;

    private bool isSwiping;
    private List<LetterButtons> selectedLetters = new List<LetterButtons>();
    public SkeletonGraphic fireworks;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (!isSwiping || selectedLetters.Count == 0)
            return;

        RectTransform last =
            selectedLetters[selectedLetters.Count - 1]
            .GetComponent<RectTransform>();

        // Convert last letter center to LineContainer local space
        Vector2 start = uiLineRenderer.GetLocalPointInContainer(last);

        // Convert mouse position to LineContainer local space
        Vector2 mouseLocal;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            uiLineRenderer.LineContainer,
            Input.mousePosition,
            null,
            out mouseLocal);

        uiLineRenderer.UpdateFollowLine(start, mouseLocal);
    }

    public void StartSwipe(LetterButtons button)
    {
        isSwiping = true;
        selectedLetters.Clear();
        uiLineRenderer.ClearAll();

        AddLetter(button);
        uiLineRenderer.StartFollowLine();
        button.gameObject.transform.parent.gameObject.GetComponent<Image>().enabled = true;
    }

    public void TrySelect(LetterButtons button)
    {
        if (!isSwiping) return;

        // Backtracking
        if (selectedLetters.Count >= 2 &&
     selectedLetters[selectedLetters.Count - 2] == button)
        {
            LetterButtons last = selectedLetters[selectedLetters.Count - 1];

            last.gameObject.transform.parent.gameObject
                .GetComponent<Image>().enabled = false;

            last.ResetSelection();
            selectedLetters.RemoveAt(selectedLetters.Count - 1);


            // 🔥 REMOVE LAST TILE
            slotManager.ClearFromIndex(selectedLetters.Count);

            RedrawPermanentLines();

            return;
        }

        if (button.IsSelected) return;
        button.gameObject.transform.parent.gameObject.GetComponent<Image>().enabled = true;
        AddLetter(button);
    }
    int winCount = 0;
    public void EndSwipe()
    {
        isSwiping = false;
        int level = GameManager.Instance.GetCurrentLevelIndex();

        uiLineRenderer.StopFollowLine();

        string formedWord = "";

        foreach (var l in selectedLetters)
        {
            l.gameObject.transform.parent.gameObject.GetComponent<Image>().enabled = false;
            formedWord += l.Letter;
        }

        formedWord = formedWord.ToUpper();

        Debug.Log("Formed Word: " + formedWord);

        string correctAnswer = GameManager.Instance.GetCurrentAnswer();
        if (formedWord == correctAnswer)
        {
            Debug.Log("Correct!");
            GameManager.Instance.IncreaseIQ();
            TimerManagement.instance.StopTimer();
            TimerManagement.instance.stopWatch.GetComponent<PulseButtonWithIdle>().TogglePulse(false);
            uiLineRenderer.ClearAll();
            // 1️⃣ Play correct animation
            slotManager.PlayCorrectAnimation();
            var trackEntry = fireworks.AnimationState.SetAnimation(0, "firecrackers", false);
            trackEntry.Complete += delegate
            {
                fireworks.AnimationState.SetAnimation(0, "empty", false);
               
               
                    StartCoroutine(HandleCorrectComplete());
               

            };
            // 2️⃣ Disable further swiping during animation
            isSwiping = false;

            // 3️⃣ Wait before clearing and loading next level
            //Invoke(nameof(HandleCorrectComplete), 1.3f);
        }
        else
        {
            Debug.Log("Wrong!");
            GameManager.Instance.DecreaseIQ();

            // Disable input during animation
            isSwiping = false;

            // Get ALL AnswerSlots in scene
            AnswerSlot[] slots = FindObjectsByType<AnswerSlot>(FindObjectsSortMode.None);

            // Count how many are active (have letters)
            int remainingAnimations = 0;

            foreach (var slot in slots)
            {
                // Only animate filled slots
                if (!string.IsNullOrEmpty(slot.GetComponentInChildren<TMPro.TextMeshProUGUI>().text))
                {
                    remainingAnimations++;

                    slot.PlayWrongAnim(() =>
                    {
                        remainingAnimations--;

                        // When all animations finish → reset
                        if (remainingAnimations == 0)
                        {
                            ResetLetters();
                        }
                    });
                }
            }

            // Safety: if somehow no slots were filled
            if (remainingAnimations == 0)
            {
                ResetLetters();
            }
        }
    }
    IEnumerator HandleCorrectComplete()
    {
        
        yield return new WaitForSeconds(0.2f);
        ResetLetters();
        LoadNextLevel();
    }
    void LoadNextLevel()
    {
        GameManager.Instance.NextLevel();
    }

    void AddLetter(LetterButtons button)
    {
        if (selectedLetters.Count > 0)
        {
            RectTransform prev =
                selectedLetters[selectedLetters.Count - 1]
                .GetComponent<RectTransform>();

            RectTransform current =
                button.GetComponent<RectTransform>();

            uiLineRenderer.AddPermanentLine(prev, current);
        }

        button.Select();
        selectedLetters.Add(button);
        slotManager.FillSlot(selectedLetters.Count - 1, button.Letter);
    }

    void RedrawPermanentLines()
    {
        uiLineRenderer.ClearAll();

        for (int i = 1; i < selectedLetters.Count; i++)
        {
            RectTransform prev =
                selectedLetters[i - 1].GetComponent<RectTransform>();

            RectTransform current =
                selectedLetters[i].GetComponent<RectTransform>();

            uiLineRenderer.AddPermanentLine(prev, current);
        }

        uiLineRenderer.StartFollowLine();
    }

    void ResetLetters()
    {
        foreach (var l in selectedLetters)
            l.ResetSelection();

        selectedLetters.Clear();
        uiLineRenderer.ClearAll();

        // 🔥 CLEAR ANSWER TILES
        slotManager.ClearAll();
    }
}

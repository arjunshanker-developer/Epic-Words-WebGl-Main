using Spine.Unity;
using System;
using TMPro;
using UnityEngine;

public class AnswerSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI letterText;
    public TextMeshProUGUI letterText2;
    public SkeletonGraphic anim;
    

    public void SetLetter(string letter)
    {
        letterText.text = letter;
        letterText2.text = letter;
        anim.AnimationState.SetAnimation(0, "word_swipe", false);
    }

    public void Clear()
    {
        letterText.text = "";
        letterText2.text = "";
        anim.AnimationState.SetAnimation(0, "idle_empty", false);
    }

    public void PlayWrongAnim(Action onComplete)
    {
        Debug.Log("WrongWord Anim Called");

        var trackEntry = anim.AnimationState.SetAnimation(0, "word_wrong", false);

        trackEntry.Complete += (entry) =>
        {
            anim.AnimationState.SetAnimation(0, "idle_empty", false);
            onComplete?.Invoke();   // 🔥 Notify SwipeController
        };
    }

    public void PlayWin()
    {
        anim.AnimationState.SetAnimation(0, "right answer", false);
    }
}
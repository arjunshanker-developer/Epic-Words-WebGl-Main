using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LetterButtons : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler
{
    [SerializeField] private TextMeshProUGUI letterText;

    public string Letter { get; private set; }
    public bool IsSelected { get; private set; }

    void Awake()
    {
        // Automatically read the text from TMP
        Letter = letterText.text;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SwipeController.Instance.StartSwipe(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SwipeController.Instance.TrySelect(this);
    }
    public void SetLetter(string letter)
    {
        Letter = letter;
        letterText.text = letter;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        SwipeController.Instance.EndSwipe();
    }

    public void Select()
    {
        IsSelected = true;
        transform.localScale = Vector3.one * 1.2f;
    }

    public void ResetSelection()
    {
        IsSelected = false;
        transform.localScale = Vector3.one;
    }
}

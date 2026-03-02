using UnityEngine;
using System.Collections.Generic;

public class AnswerSlotManager : MonoBehaviour
{
    public static AnswerSlotManager instance;

    [SerializeField] private GameObject answerTilePrefab;
    [SerializeField] private Transform container;

    private List<AnswerSlot> activeSlots = new List<AnswerSlot>();

    private void Awake()
    {
        instance = this;
    }

    public void SetupSlots(int answerLength)
    {
        ClearAllTiles();

        for (int i = 0; i < answerLength; i++)
        {
            GameObject tile = Instantiate(answerTilePrefab, container);
            AnswerSlot slot = tile.GetComponent<AnswerSlot>();
            activeSlots.Add(slot);
        }
    }

    public void FillSlot(int index, string letter)
    {
        if (index < activeSlots.Count)
            activeSlots[index].SetLetter(letter);
    }

    public void ClearFromIndex(int index)
    {
        if (index < activeSlots.Count)
            activeSlots[index].Clear();
    }

    public void ClearAll()
    {
        foreach (var slot in activeSlots)
            slot.Clear();
    }
    public void PlayCorrectAnimation()
    {
        foreach (var slot in activeSlots)
            slot.PlayWin();
    }
    private void ClearAllTiles()
    {
        foreach (Transform child in container)
            Destroy(child.gameObject);

        activeSlots.Clear();
    }
}
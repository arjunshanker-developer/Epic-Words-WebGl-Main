using UnityEngine;
using System.Collections.Generic;

public class LetterWheelManager : MonoBehaviour
{
    [Header("Letter Buttons In Wheel")]
    [SerializeField] private LetterButtons[] letterButtons;

    private const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public void SetupLevel(LevelData level)
    {
        List<char> letters = GenerateLetters(level.answer, level.totalLetterCount);

        for (int i = 0; i < letterButtons.Length; i++)
        {
            if (i < letters.Count)
            {
                letterButtons[i].gameObject.SetActive(true);
                letterButtons[i].SetLetter(letters[i].ToString());
            }
            else
            {
                letterButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private List<char> GenerateLetters(string answer, int totalCount)
    {
        List<char> letters = new List<char>();

        // Add answer letters
        foreach (char c in answer.ToUpper())
            letters.Add(c);

        // Fill remaining with random letters
        while (letters.Count < totalCount)
        {
            char randomChar = alphabet[Random.Range(0, alphabet.Length)];
            letters.Add(randomChar);
        }

        // Shuffle letters
        for (int i = 0; i < letters.Count; i++)
        {
            int randomIndex = Random.Range(i, letters.Count);
            char temp = letters[i];
            letters[i] = letters[randomIndex];
            letters[randomIndex] = temp;
        }

        return letters;
    }
}
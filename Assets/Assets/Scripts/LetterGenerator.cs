using System.Collections.Generic;
using UnityEngine;

public static class LetterGenerator
{
    private const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public static List<char> GenerateLetters(string answer, int totalCount)
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

        // Shuffle
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
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevel", menuName = "4Pic/Level Data")]
public class LevelData : ScriptableObject
{
    [Header("Answer")]
    public string answer;

    [Header("Images")]
    public Sprite image1;
    public Sprite image2;
    public Sprite image3;
    public Sprite image4;

    [Header("Letter Settings")]
    public int totalLetterCount = 8;
}
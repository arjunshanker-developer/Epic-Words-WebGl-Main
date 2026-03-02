using UnityEngine;

public class LevelDatabase : MonoBehaviour
{
    public LevelData[] levels;

    public LevelData GetLevel(int index)
    {
        if (index < 0 || index >= levels.Length)
            return null;

        return levels[index];
    }
}
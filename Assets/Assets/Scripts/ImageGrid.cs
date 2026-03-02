using UnityEngine;
using UnityEngine.UI;

public class ImageGrid : MonoBehaviour
{
    [SerializeField] private Image image1;
    [SerializeField] private Image image2;
    [SerializeField] private Image image3;
    [SerializeField] private Image image4;

    public void SetupImages(LevelData level)
    {
        image1.sprite = level.image1;
        image2.sprite = level.image2;
        image3.sprite = level.image3;
        image4.sprite = level.image4;
    }
}
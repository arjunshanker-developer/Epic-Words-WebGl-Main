using System.Collections.Generic;
using UnityEngine;

public class UILineRenderer : MonoBehaviour
{
    [SerializeField] private RectTransform lineContainer;
    [SerializeField] private GameObject linePrefab;

    private List<GameObject> activeLines = new List<GameObject>();
    private GameObject followLine;
    public RectTransform LineContainer => lineContainer;
    public void AddPermanentLine(RectTransform from, RectTransform to)
    {
        GameObject line = Instantiate(linePrefab, lineContainer);
        RectTransform lineRect = line.GetComponent<RectTransform>();

        Vector2 start = GetLocalPointInContainer(from);
        Vector2 end = GetLocalPointInContainer(to);

        PositionLine(lineRect, start, end);

        activeLines.Add(line);
    }
    public Vector2 GetLocalPointInContainer(RectTransform target)
    {
        Vector2 worldPoint = target.TransformPoint(target.rect.center);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            lineContainer,
            RectTransformUtility.WorldToScreenPoint(null, worldPoint),
            null,
            out Vector2 localPoint);

        return localPoint;
    }
    public void StartFollowLine()
    {
        followLine = CreateLine();
    }

    public void UpdateFollowLine(Vector2 from, Vector2 to)
    {
        if (followLine == null) return;
        PositionLine(followLine.GetComponent<RectTransform>(), from, to);
    }

    public void StopFollowLine()
    {
        if (followLine != null)
            Destroy(followLine);
    }

    public void ClearAll()
    {
        foreach (var line in activeLines)
            Destroy(line);

        activeLines.Clear();

        StopFollowLine();
    }

    GameObject CreateLine()
    {
        return Instantiate(linePrefab, lineContainer);
    }

    void PositionLine(RectTransform lineRect, Vector2 start, Vector2 end)
    {
        Vector2 direction = end - start;
        float distance = direction.magnitude;

        lineRect.sizeDelta = new Vector2(25f, distance);
        lineRect.anchoredPosition = start + direction / 2f;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        lineRect.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }
}

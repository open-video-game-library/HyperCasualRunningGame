using UnityEngine;

[RequireComponent(typeof(RectTransform))]
[ExecuteAlways]
public class SafeAreaRect : MonoBehaviour
{
    private void Update()
    {
        var safeArea = Screen.safeArea;
        var resolution = new Vector2Int(Screen.width, Screen.height);
        var normalizedMin = new Vector2(safeArea.xMin / resolution.x, safeArea.yMin / resolution.y);
        var normalizedMax = new Vector2(safeArea.xMax / resolution.x, safeArea.yMax / resolution.y);

        var rectTransform = (RectTransform)transform;
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.sizeDelta = Vector2.zero;
        rectTransform.anchorMin = normalizedMin;
        rectTransform.anchorMax = normalizedMax;
    }
}
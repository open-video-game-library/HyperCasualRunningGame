using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MU5Attribute;

public class ListGroup : MonoBehaviour
{
    public RectTransform rectTransform;

    [Space(20)]
    public Object element;
    public int size = 0;
    List<RectTransform> contents = new List<RectTransform>();

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    // Start is called before the first frame update
    void Start()
    {
        InitializeContents();
    }

    void OnValidate()
    {
        InitializeContents();
        OnSizeChanged(size);
    }

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    void InitializeContents()
    {
        contents.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (!child.TryGetComponent(element.GetType(), out Component tmp)) continue;

            if (child.TryGetComponent(out RectTransform _rectTransform)) contents.Add(_rectTransform);
        }
        foreach (var content in contents)
        {
            bool isActive = content.gameObject.activeSelf;
            content.gameObject.SetActive(true);
            content.gameObject.SetActive(isActive);
        }
    }

    public void OnSizeChanged(int num)
    {
        size = Mathf.Clamp(num, 0, contents.Count);
        for (int i = 0; i < size; i++)
        {
            contents[i].gameObject.SetActive(true);
        }
        for (int i = size; i < contents.Count; i++)
        {
            contents[i].gameObject.SetActive(false);
        }

        ChangeHeight();
    }

    void ChangeHeight()
    {
        float height = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (!child.gameObject.activeSelf) break;
            if (child.TryGetComponent(out RectTransform _rectTransform)) height += _rectTransform.sizeDelta.y;
        }
        Vector2 sizeDelta = rectTransform.sizeDelta;
        sizeDelta.y = height;
        rectTransform.sizeDelta = sizeDelta;
    }

    int GetActiveContentsCount()
    {
        int count = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (!child.gameObject.activeSelf) continue;
            if (!child.TryGetComponent(element.GetType(), out Component tmp)) continue;
            count++;
        }
        return count;
    }
}

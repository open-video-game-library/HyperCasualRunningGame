using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FoldoutGroup : MonoBehaviour
{
    [SerializeField] Button foldoutBtn;
    [SerializeField] Image foldArrow_img;

    List<GameObject> children = new List<GameObject>();
    bool isOpen = false;

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            children.Add(transform.GetChild(i).gameObject);
        }
        isOpen = false;
        ShowChildren(isOpen);
    }

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    public void OnBtnPush_foldoutBtn()
    {
        isOpen = !isOpen;
        ShowChildren(isOpen);
    }

    void ShowChildren(bool _isOpen)
    {
        if (_isOpen) foldArrow_img.transform.localEulerAngles = new Vector3(0, 0, 0);
        else foldArrow_img.transform.localEulerAngles = new Vector3(0, 0, 90);

        foreach (var child in children)
        {
            child.SetActive(_isOpen);
        }
    }
}

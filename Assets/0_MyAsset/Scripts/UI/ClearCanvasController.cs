using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearCanvasController : MonoBehaviour
{
    public void OnBtnPush_next()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

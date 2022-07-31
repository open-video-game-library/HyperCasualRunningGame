using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FailCanvasController : MonoBehaviour
{
    public void OnBtnPush_retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

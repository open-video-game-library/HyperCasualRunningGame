using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartCanvasController : MonoBehaviour
{
    public void OnBtnPush_openSettings()
    {
        CanvasManager.i.OpenSettingsCanvas();
    }

    public void OnBtnPush_reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FOVFitter : MonoBehaviour
{
    [SerializeField] Camera subCamera;

    [Space(10)]
    [SerializeField] bool applyFitHorizontal = true;
    [SerializeField] Vector2 aspect_default = new Vector2(1440, 2560);
    [SerializeField] float fov_deg_default = 60;

    Camera _camera;
    float aspectRatio;
    float aspectRatio_default;
    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<Camera>();
        aspectRatio = (float)Screen.width / Screen.height;
        aspectRatio_default = aspect_default.x / aspect_default.y;

        FOVFit_Horizontal();
    }

    void FOVFit_Horizontal()
    {
        if (!applyFitHorizontal) return;

        float targetHeight = Screen.height * aspectRatio_default / aspectRatio;
        float distance = Screen.height / 2 / Mathf.Tan(fov_deg_default * Mathf.Deg2Rad / 2);
        float fov_deg = Mathf.Rad2Deg * Mathf.Atan2(targetHeight / 2, distance) * 2;

        _camera.fieldOfView = fov_deg;
        if (subCamera != null) subCamera.fieldOfView = fov_deg;
    }
}

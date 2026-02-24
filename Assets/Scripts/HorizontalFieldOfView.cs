using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class HorizontalFieldOfView : MonoBehaviour
{
    public int fieldOfView = 90;
    int previousFieldOFView;
    Camera cam;

    void Awake()
    {
        if (GetComponent<Camera>() != null)
        {
            cam = GetComponent<Camera>();
        }
        previousFieldOFView = fieldOfView;
    }

    void Update()
    {
        if (cam == null)
        {
            if (GetComponent<Camera>() != null)
            {
                cam = GetComponent<Camera>();
            }
        }
        if (previousFieldOFView != fieldOfView)
        {
            updateFOV();
            previousFieldOFView = fieldOfView;
        }
    }

    void updateFOV()
    {
        float hFOVrad = fieldOfView * Mathf.Deg2Rad;
        float camH = Mathf.Tan(hFOVrad * 0.5f) / cam.aspect;
        float vFOVrad = Mathf.Atan(camH) * 2;
        cam.fieldOfView = vFOVrad * Mathf.Rad2Deg;
    }
}

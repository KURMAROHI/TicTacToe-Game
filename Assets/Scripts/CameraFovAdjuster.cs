using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFovAdjuster : MonoBehaviour
{
    Camera camera;
    public float BaseAsepect;
    public float currentAspect;
    float baseFOV = 60f;
    public float ReduceAspect = 7f;
    float thresold = 0.04f;
    void Start()
    {
        BaseAsepect = 1080f / 1920f;
        camera = GetComponent<Camera>();
        AdjustFov();
    }

    public void AdjustFov()
    {
        currentAspect = (float)Screen.width / (float)Screen.height;
        float AspectMultiplier = BaseAsepect / currentAspect;
        Debug.LogError("==>Current Aspect|" + currentAspect + "::" + AspectMultiplier);
        if (currentAspect < BaseAsepect - thresold)
        {
            camera.fieldOfView = (baseFOV * AspectMultiplier) - ReduceAspect;
        }
        // else if (currentAspect > BaseAsepect + 0.04)
        else if (currentAspect > BaseAsepect + thresold)
        {
            camera.fieldOfView = (baseFOV * AspectMultiplier);
        }
        else
        {
            camera.fieldOfView = baseFOV;
        }
    }

    // Update is called once per frame
    // void Update()
    // {
    //     currentAspect=
    // }
}

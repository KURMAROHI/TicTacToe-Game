using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectControl : MonoBehaviour
{
    public float AspectRatio;
    void Start()
    {
        Debug.Log("==>Aspect Ratio|" + (float)Screen.height/(float)Screen.width);
        AspectRatio =  (float)Screen.height/(float)Screen.width;
    }


    void Update()
    {

    }
}

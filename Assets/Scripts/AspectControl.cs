using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectControl : MonoBehaviour
{
    public float AspectRatio;
    void Start()
    {
        Debug.Log("==>Aspect Ratio|" + (float)Screen.width/(float)Screen.height);
        AspectRatio =  (float)Screen.width/(float)Screen.height;
    }



}

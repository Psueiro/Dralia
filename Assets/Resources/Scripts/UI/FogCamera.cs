using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogCamera : MonoBehaviour
{
    public RenderTexture rt;

    private void Awake()
    {
        rt = new RenderTexture(100, 100, 1, RenderTextureFormat.ARGB32);
        rt.Create();
        GetComponent<Camera>().targetTexture = rt;
    }
}

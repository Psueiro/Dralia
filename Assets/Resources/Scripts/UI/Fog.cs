using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour
{
    public Camera[] fogCameras;
    public Shader shader;
    Material mat;

    private void Start()
    {
        CreateMaterial(shader);
        GetComponent<SpriteRenderer>().material = mat;
    }

    void CreateMaterial(Shader s)
    {
        mat = new Material(s);
        mat.SetTexture("_fog", fogCameras[0].targetTexture);
        mat.SetTexture("_mist", fogCameras[1].targetTexture);
    }

}

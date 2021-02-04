using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap
{
    CamController cam;
    public float magnifierx;
    public float magnifierz;
    public RectTransform rectangle;
    public Vector2 localMousePosition;

    public Minimap(RectTransform r, CamController c)
    {
        cam = c;
        rectangle = r;
        magnifierx = (cam.maxlimitx - cam.minlimitx) / rectangle.sizeDelta.x;
        magnifierz = (cam.maxlimity - cam.minlimity) / rectangle.sizeDelta.y;
    }

    public void Update()
    {
        localMousePosition = rectangle.InverseTransformPoint(Input.mousePosition);       
    }

    public void MoveCamera()
    {
        if (rectangle.rect.Contains(localMousePosition))
            cam.transform.position = new Vector3(localMousePosition.x * magnifierx, cam.transform.position.y, localMousePosition.y * magnifierz);
    }

    public Vector3 MoveUnit()
    {
           return new Vector3(localMousePosition.x * magnifierx,0 , localMousePosition.y * magnifierz);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlacementChecker : MonoBehaviour
{
    Color _ableColor = new Color(0.2f, 0.9f, 0.3f, 0.8f);
    Color _unableColor = new Color(0.8f, 0.2f, 0.3f, 0.8f);
    MeshRenderer _myMat;
    public Stats myStat;
    public LayerMask myLayerMask;
    public LayerMask fowLayerMask;
    public RaycastHit[] hits;
    public RaycastHit[] fowHits;
    public bool visible;
    public bool able;
    public int layerAble;
    public int cryCost;
    public int minCost;
    public float boundX;
    public float boundY;
    public float boundZ;
    public float id;
    Quaternion _rot;

    public float fadeTimer;

    void Start()
    {
        boundY = 1;
        _myMat = GetComponent<MeshRenderer>();
        _rot = Quaternion.Euler(0, 0, 0);
        BoxCastHeight();
        Positioning(boundX, boundZ);
    }

    void Update()
    {
        Positioning(boundX, boundZ);
        ColorChange();
        BoxCastHeight();
        ExtractorTimer();
    }

    void BoxCastHeight()
    {
        if (layerAble == 9 || layerAble == 10)
        {
            boundY = 0;
            myLayerMask = 1813;
        }
        else
        {
            boundY = 1;
            myLayerMask = 18199;
        }
    }

    void ExtractorTimer()
    {
        if ((layerAble == 9 || layerAble == 10) && able)
        {
            if (fadeTimer < 0.1f)
                fadeTimer += 1 * Time.deltaTime;
            else
                able = false;
        }
    }

    public void Positioning(float theXsize, float theZsize)
    {
        Ray ray;
        if (id == 0)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        }
        else ray = new Ray();

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
            transform.position = new Vector3(hit.point.x, hit.point.y + 4, hit.point.z);
        //Acceder al bounds.size o extents del collider no FUNCA, no se porque, aparentemente lo considera un 0 si se accede de un prefab asi que voy a hacer a lo bruto y usar un switch porque no tengo tiempo
        Vector3 center = new Vector3(transform.position.x, transform.position.y / 2, transform.position.z);

        Vector3 halfExtents = new Vector3(theXsize * transform.localScale.x, boundY, theZsize * transform.localScale.z) / 2;

        if (id == 0)
        {
            fowHits = Physics.BoxCastAll(center, halfExtents, Vector3.down, _rot, 10, fowLayerMask);
            for (int i = 0; i < fowHits.Length; i++)
            {
                if (fowHits[i].collider.gameObject)
                {
                    if (fowHits[i].collider.gameObject.layer == 12)
                    {
                        visible = true;
                        break;
                    }
                    else
                    {
                        visible = false;
                    }
                }
            }
        }
        else visible = true;

        hits = Physics.BoxCastAll(center, halfExtents, Vector3.down, _rot, 10, myLayerMask);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.gameObject)
            {
                if (hits[i].collider.gameObject.layer != layerAble)
                {
                    able = false;
                    break;
                }
                else
                {
                    if (visible)
                        able = true;
                    else able = false;
                    fadeTimer = 0;
                }
            }
        }
    }
    void ColorChange()
    {
        for (int i = 0; i < _myMat.materials.Length; i++)
        {
            if (able) _myMat.materials[i].color = _ableColor; else _myMat.materials[i].color = _unableColor;
        }
    }
}
//Make it so it requests an exact boxcast to server to check if the place is available
//Make it so it requests an exact boxcast before the BuildingPlaceholder is spawned to check if that area is available
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionSquare : MonoBehaviour
{
    public LayerMask detectLayer;
    public Vector3 startpos;
    public SpriteRenderer sp;
    private RaycastHit[] _hits;
    public SelectionManager sel;
    float _testx;
    float _testz;
    SoundManager sound;

    private void Start()
    {
        if (sound == null) sound = Camera.main.GetComponent<SoundManager>();
    }

    public void Spread()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        float _distx;
        float _distz;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 _v3;
            _v3 = hit.point;
            _v3.y = 4;

            Vector3 stx = new Vector3(startpos.x, 0, 0);
            Vector3 v3x = new Vector3(hit.point.x, 0, 0);
            if (v3x.x > stx.x)
            {
                _distx = Vector3.Distance(stx, v3x);
                _testx = sp.size.x;
            }
            else
            {
                _distx = -Vector3.Distance(stx, v3x);
                _testx = -sp.size.x;
            }

            Vector3 stz = new Vector3(0, 0, startpos.z);
            Vector3 v3z = new Vector3(0, 0, _v3.z);

            if (v3z.z < stz.z)
            {
                _distz = Vector3.Distance(stz, v3z);
                _testz = -sp.size.y;
            }
            else
            {
                _distz = -Vector3.Distance(stz, v3z);
                _testz = sp.size.y;
            }

            sp.size = new Vector2(_distx, _distz);
            transform.position = new Vector3(startpos.x + _distx / 2, startpos.y, startpos.z - _distz / 2);
        }
    }

    public void Select()
    {
        Vector3 center = new Vector3(transform.position.x, transform.position.y / 2, transform.position.z);
        Vector3 halfExtents = new Vector3(_testx, transform.position.y, -_testz) / 2;
        _hits = Physics.BoxCastAll(center, halfExtents, Vector3.down, Quaternion.Euler(0, 0, 0), 10, detectLayer);
    }

    public void Highlight()
    {
        float highlightTimer = 0.1f;
        for (int i = 0; i < _hits.Length; i++)
        {
            if (_hits[i].collider.gameObject && _hits[i].collider.gameObject.GetComponent<Stats>())
            {            
                if (!sel.highlightedUnits.Contains(_hits[i].collider.gameObject.GetComponent<Stats>()))
                {
                    sel.highlightedUnits.Add(_hits[i].collider.gameObject.GetComponent<Stats>());
                    sel.highlightTimers.Add(highlightTimer);
                }else
                {                    
                    for (int j = 0; j < sel.highlightTimers.Count; j++)
                    {   
                        if(_hits[i].collider.gameObject == sel.highlightedUnits[j].gameObject)
                        sel.highlightTimers[j] = highlightTimer;
                    }
                }                
            }
        }
    }

    public void Release()
    {
        if (Time.timeScale > 0) 
        for (int i = 0; i < _hits.Length; i++)
        {
            if (_hits[i].collider.gameObject && _hits[i].collider.gameObject.GetComponent<Stats>())
            {
                if (!sel.selectedUnits.Contains(_hits[i].collider.gameObject.GetComponent<Stats>()))
                {
                    sel.selectedUnits.Add(_hits[i].collider.gameObject.GetComponent<Stats>());
                }
            }
        }
        if (sel.selectedUnits.Count == 1 && sel.selectedUnits[0].civ.id == 0 &&!sel.selectedUnits[0].GetComponent<BuildingPlaceholder>()) sound.Play(sel.selectedUnits[0].sounds[0]);
    }
}

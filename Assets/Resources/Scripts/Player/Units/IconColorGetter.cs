using UnityEngine;

public class IconColorGetter : MonoBehaviour
{
    Civilization _civ;
    SpriteRenderer _spr;

    void Start()
    {
        _civ = transform.parent.GetComponentInParent<Civilization>();
        _spr = GetComponent<SpriteRenderer>();        
        if(transform.parent.GetComponent<Stats>())
        _spr.color = _civ.color;
    }
}

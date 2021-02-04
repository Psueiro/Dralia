using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthbarSpawner : MonoBehaviour
{
    public Healthbar prefabHealthbar;
    ObjectPool<Healthbar> _pool;
    SelectionManager _sel;

    void Start()
    {
        _pool = new ObjectPool<Healthbar>(HealthbarFactory, TurnOnHealthbar, TurnOffHealthbar, 100);
        _sel = GetComponentInParent<SelectionManager>();
    }

    void Update()
    {
        if (_sel.units.Count > transform.childCount && transform.childCount < 200)
        {
            _pool.GetObject();
        }
        for (int i = 0; i < _sel.units.Count; i++)
        {
            if (!transform.GetChild(i).GetComponent<Healthbar>().target)
            {
                transform.GetChild(i).GetComponent<Healthbar>().target = _sel.units[i];
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    public Healthbar HealthbarFactory()
    {
        return Instantiate(prefabHealthbar,gameObject.transform);
    }

    void TurnOnHealthbar(Healthbar h)
    {
        h.gameObject.SetActive(true);
    }

    void TurnOffHealthbar(Healthbar h)
    {
        h.gameObject.SetActive(false);
    }

    public void ReturnHealthbar(Healthbar h)
    {
        _pool.ReturnObject(h);
    }
}

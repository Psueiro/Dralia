using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public List<float> highlightTimers = new List<float>();

    public List<Stats> units = new List<Stats>();
    public List<Stats> realUnits = new List<Stats>();
    public List<Stats> highlightedUnits = new List<Stats>();
    public List<Stats> selectedUnits = new List<Stats>();
    public List<KeyCode> selectionHotkeys = new List<KeyCode>();
    public List<List<Stats>> hotKeyedSelections = new List<List<Stats>>();
    public Dictionary<KeyCode, List<Stats>> HotKeyAssigner = new Dictionary<KeyCode, List<Stats>>();

    public Color civColor;
    private Color _highlightColor;
    private Color _selectColor;

    public SelectionSquare selSquare;

    private void Start()
    {
        civColor = GetComponent<Civilization>().color;

        _highlightColor = new Color(1 + 0.25f, 1 + 0.25f, 1 + 0.25f, 1);
        _selectColor = new Color(1 + 0.1f, 1 + 1f, 1 + 0.1f, 1);
        SelectionHotkeyAdder();
    }

    private void Update()
    {
        ListAssigner();
        if (highlightedUnits.Count > 0) HighlightFade();
    }

    void SelectionHotkeyAdder()
    {
        selectionHotkeys.Add(KeyCode.Alpha1);
        selectionHotkeys.Add(KeyCode.Alpha2);
        selectionHotkeys.Add(KeyCode.Alpha3);
        selectionHotkeys.Add(KeyCode.Alpha4);
        selectionHotkeys.Add(KeyCode.Alpha5);
        selectionHotkeys.Add(KeyCode.Alpha6);
        selectionHotkeys.Add(KeyCode.Alpha7);
        selectionHotkeys.Add(KeyCode.Alpha8);
        selectionHotkeys.Add(KeyCode.Alpha9);
        selectionHotkeys.Add(KeyCode.Alpha0);
    }

    public void HotKeying(int key)
    {
        if (!HotKeyAssigner.ContainsKey(selectionHotkeys[key]))
        {
            HotKeyAssigner.Add(selectionHotkeys[key], new List<Stats>());
        }
        else
            HotKeyAssigner[selectionHotkeys[key]].Clear();

        for (int i = 0; i < selectedUnits.Count; i++)
        {
            HotKeyAssigner[selectionHotkeys[key]].Add(selectedUnits[i]);
        }
    }

    public void HotCalling(int key)
    {
        if (HotKeyAssigner.ContainsKey(selectionHotkeys[key]))
        {
            selectedUnits.Clear();
            for (int i = 0; i < HotKeyAssigner[selectionHotkeys[key]].Count; i++)
            {
                selectedUnits.Add(HotKeyAssigner[selectionHotkeys[key]][i]);
            }
        }
    }

    public void ActivateSquare(Vector3 v)
    {
        if (!selSquare) selSquare = Instantiate(Resources.Load<SelectionSquare>("GameObjects/SelectionSquare"));
        selSquare.transform.parent = transform;
        selSquare.sel = this;
        selSquare.sp = selSquare.GetComponent<SpriteRenderer>();
        Vector3 v2 = new Vector3(v.x, v.y + 4, v.z);
        selSquare.transform.position = v2;
        selSquare.startpos = v2;
        if (!selSquare.sel) selSquare.sel = this;
        selSquare.gameObject.SetActive(true);
    }

    public void DeactivateSquare()
    {
        selSquare.sp.size = new Vector2(0.01f, 0.01f);
        selSquare.gameObject.SetActive(false);
    }

    void ListAssigner()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<Stats>() && !units.Contains(transform.GetChild(i).GetComponent<Stats>()))
                units.Add(transform.GetChild(i).GetComponent<Stats>());
            if (transform.GetChild(i).GetComponent<Stats>() && !realUnits.Contains(transform.GetChild(i).GetComponent<Stats>()) && !transform.GetChild(i).GetComponent<NotRealUnits>())
                realUnits.Add(transform.GetChild(i).GetComponent<Stats>());
        }
        ColorAssign();
    }

    void ColorAssign()
    {
        for (int i = 0; i < units.Count; i++)
        {
            if (!units[i].GetComponent<BuildingPlaceholder>())
            {
                if (!highlightedUnits.Contains(units[i]) && !selectedUnits.Contains(units[i]))
                {
                    for (int j = 0; j < units[i].GetComponents<Renderer>().Length; j++)
                    {
                        for (int k = 0; k < units[i].GetComponents<Renderer>()[j].materials.Length; k++)
                        {
                            units[i].GetComponents<Renderer>()[j].materials[k].color = civColor;
                        }
                    }


                    for (int j = 0; j < units[i].GetComponentsInChildren<Renderer>().Length - 2; j++)
                    {
                        for (int k = 0; k < units[i].GetComponentsInChildren<Renderer>()[j].materials.Length; k++)
                        {
                            units[i].GetComponentsInChildren<Renderer>()[j].materials[k].color = civColor;
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < highlightedUnits.Count; j++)
                    {
                        for (int l = 0; l < highlightedUnits[j].GetComponentsInChildren<Renderer>().Length - 1; l++)
                        {
                            highlightedUnits[j].GetComponentsInChildren<Renderer>()[l].material.color = _highlightColor;

                        }
                    }
                    for (int k = 0; k < selectedUnits.Count; k++)
                    {
                        for (int l = 0; l < selectedUnits[k].GetComponentsInChildren<Renderer>().Length - 1; l++)
                        {
                            selectedUnits[k].GetComponentsInChildren<Renderer>()[l].material.color = _selectColor;
                        }
                    }
                }
            }
        }
    }

    void HighlightFade()
    {
        for (int i = 0; i < highlightedUnits.Count; i++)
        {
            if (highlightTimers[i] > 0)
                highlightTimers[i] -= 1 * Time.deltaTime;
            else
            {
                highlightedUnits.Remove(highlightedUnits[i]);
                highlightTimers.Remove(highlightTimers[i]);
            }
        }
    }

    public void SelectionFade()
    {
        for (int i = 0; i < selectedUnits.Count; i++)
        {
            selectedUnits.Clear();
        }
    }

    public void BuildingPlaceHolderPreview(float f)
    {
        for (int i = 0; i < units.Count; i++)
        {
            BuildingPlaceholder placeholder = units[i].GetComponent<BuildingPlaceholder>();
            if (placeholder && placeholder.percent < 1) placeholder.ColorManagement(units[i].GetComponent<Renderer>().material.color, f);
        }
    }

    public void Deletion(Stats me)
    {
        if (units.Contains(me)) units.Remove(me);
        if (highlightedUnits.Contains(me)) highlightedUnits.Remove(me);
        if (selectedUnits.Contains(me)) selectedUnits.Remove(me);
    }
}

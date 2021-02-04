using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Civilization : Entity
{
    SelectionManager sel;

    private void Start()
    {
        sel = GetComponent<SelectionManager>();
    }
}

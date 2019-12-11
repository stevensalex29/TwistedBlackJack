using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem{
    // Attributes
    public string Name;
    public int Value;
    public GameObject Prefab;

    public InventoryItem(string name, int value, GameObject prefab)
    {
        Value = value;
        Name = name;
        Prefab = prefab;
    }
}

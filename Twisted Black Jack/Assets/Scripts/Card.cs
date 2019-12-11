using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card{

    // Attributes
    public string Name;
    public int Value;

	public Card(string name, int value)
    {
        Value = value;
        Name = name;
    }
}

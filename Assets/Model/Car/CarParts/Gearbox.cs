using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Gearbox : CarPart
{
    public int TopSpeed;

    public Gearbox(string name, int topSpeed,int tier) : base(name,tier)
    {
        TopSpeed = topSpeed;
    }

    public override string ToString()
    {
        return $"{Name} Gearbox with {TopSpeed} top speed";
    }
}

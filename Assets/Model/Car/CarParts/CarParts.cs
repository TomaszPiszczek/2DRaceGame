using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class CarPart 
{
    public string Name;
    public int Tier;
    // 0-A 1-B 2-C 3-D 4-E

    protected CarPart(string name,int tier)
    {
        Name = name;
        this.Tier = tier;
    }
}

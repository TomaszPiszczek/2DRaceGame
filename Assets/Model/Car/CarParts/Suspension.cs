using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Suspenssion : CarPart
{
    public double TurnFactor;

    public Suspenssion(string name, double turnFactor,int tier) : base(name,tier)
    {
        TurnFactor = turnFactor;
    }

    public override string ToString()
    {
        return $"{Name} Suspension with {TurnFactor} turn factor";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Turbocharger : CarPart
{
    public float HpBoost; 

    public Turbocharger(string name, float hpBoost,int tier) : base(name,tier)
    {
        HpBoost = hpBoost;
    }

    public override string ToString()
    {
        return $"{Name} Turbocharger with {HpBoost} power boost";
    }
}

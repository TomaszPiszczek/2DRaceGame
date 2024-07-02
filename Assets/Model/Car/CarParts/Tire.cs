using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tire : CarPart
{
    public string Type; // Soft/Hard/Medium
    public double Friction;
    public int Durability;

    public Tire(string name, string type, double friction, int durability,int tier) : base(name,tier)
    {
        Type = type;
        Friction = friction;
        Durability = durability;
    }

    public override string ToString()
    {
        return $"{Name} {Type} Tire durability: {Durability} friction: {Friction}";
    }
}

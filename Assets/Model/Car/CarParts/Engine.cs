using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Engine : CarPart
{
    public int HorsePower;
    public int FuelConsuption;

    public Engine(string name, int horsePower, int fuelConsuption,int tier) : base(name,tier)
    {
        HorsePower = horsePower;
        FuelConsuption = fuelConsuption;
    }

    public override string ToString()
    {
        return $"{Name} Engine with {HorsePower} HP and FuelConsuption {FuelConsuption}";
    }
}

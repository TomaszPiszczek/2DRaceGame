using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public Garage garage;
    public int money;

    public PlayerData(Player player)
    {
        this.garage = player.garage;
        this.money = player.money;
    }
    
}
